using ABCsystem.Core;
using ABCsystem.Util;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace ABCsystem.Algorithm
{
    public class EdgeAlgorithm : InspAlgorithm
    {
        private static readonly Point2f InvalidPoint = new Point2f(-1, -1);

        public int EdgeThreshold { get; set; } = 30;

        // 엣지 윤곽선 시각화용 점 표시 간격 (1: 모든 엣지 점 표시, 5: 5픽셀마다 1점 표시)
        public int DrawStride { get; set; } = 1;

        [XmlIgnore]
        public OpenCvSharp.Point2f FoundEdgePoint => _edgePoint;

        // 스캔 방향 : 라인별 "첫 엣지"를 찾는 탐색 방향
        public enum ScanDirection
        {
            LeftToRight, RightToLeft, TopToBottom, BottomToTop
        }

        public ScanDirection ScanDir { get; set; } = ScanDirection.LeftToRight;
        public int ExtremeTol { get; set; } = 1;

        // 세로 스캔(↑/↓)에서 대표점 Y 기준
        // true  : 엣지 라인의 중앙값 사용 (권장, 방향 무관)
        // false : 스캔 방향에 따른 extreme(Y) 사용
        public bool UseCenterYOnVertical { get; set; } = true;

        // Align(기준점)용
        public bool UseAsAlignment { get; set; } = false;   // InspAlignEdge일 때 true
        public double TeachAnchorX { get; set; } = double.NaN;  // 정상 모델에서 저장할 기준 X. 아직 티칭 전이면 NaN 유지

        // 런타임 결과 (XML 저장 X)
        // 라인별로 픽킹된 엣지 수 (라인당 1개씩)
        [XmlIgnore] public int OutEdgeCount { get; private set; } = 0;
        // 픽킹된 점들의 bounding box(ROI 전체좌표계 기준). 디버그/참고용.
        [XmlIgnore] private Rect _edgeBoundingRect = new Rect(0, 0, 0, 0);
        // 대표점(전체좌표계 기준) / 화면 표시
        private Point2f _edgePoint = InvalidPoint;
        // 라인별로 찾은 "첫 엣지" 점들 중 일부를 화면에 같이 찍기 위한 리스트(전체좌표)
        [XmlIgnore] private readonly List<Point2f> _pickedEdgePoints = new List<Point2f>();

        // Align 런타임 결과
        [XmlIgnore] public bool HasAnchor { get; private set; } = false;
        [XmlIgnore] public Point2f AnchorPoint { get; private set; }  // 찾은 첫 엣지점

        public EdgeAlgorithm()
        {
            InspectType = InspectType.InspEdge;
        }

        public override InspAlgorithm Clone()
        {
            var cloneAlgo = new EdgeAlgorithm();
            CopyBaseTo(cloneAlgo);
            cloneAlgo.EdgeThreshold = this.EdgeThreshold;
            cloneAlgo.ScanDir = this.ScanDir;
            cloneAlgo.ExtremeTol = this.ExtremeTol;
            cloneAlgo.UseCenterYOnVertical = this.UseCenterYOnVertical;

            // Align 옵션도 복사
            cloneAlgo.UseAsAlignment = this.UseAsAlignment;
            cloneAlgo.TeachAnchorX = this.TeachAnchorX;

            // InspectType도 상황에 맞게 복사
            cloneAlgo.InspectType = this.InspectType;

            return cloneAlgo;
        }

        public override bool CopyFrom(InspAlgorithm sourceAlgo)
        {
            var src = (EdgeAlgorithm)sourceAlgo;
            EdgeThreshold = src.EdgeThreshold;
            ScanDir = src.ScanDir;
            ExtremeTol = src.ExtremeTol;
            UseCenterYOnVertical = src.UseCenterYOnVertical;

            // Align 옵션도 복사
            UseAsAlignment = src.UseAsAlignment;
            TeachAnchorX = src.TeachAnchorX;

            // InspectType 복사
            InspectType = src.InspectType;

            return true;
        }

        private bool IsHorizontalScan => (ScanDir == ScanDirection.LeftToRight || ScanDir == ScanDirection.RightToLeft);
        private bool IsForward => (ScanDir == ScanDirection.LeftToRight || ScanDir == ScanDirection.TopToBottom);

        private bool IsEdgeAt(Mat gray, int x, int y)
        {
            if (x <= 0 || y <= 0 || x >= gray.Cols - 1 || y >= gray.Rows - 1) return false;

            byte pL = gray.Get<byte>(y, x - 1);
            byte pR = gray.Get<byte>(y, x + 1);
            byte pU = gray.Get<byte>(y - 1, x);
            byte pD = gray.Get<byte>(y + 1, x);

            int diffX = pR - pL;
            int diffY = pD - pU;

            return IsHorizontalScan ? Math.Abs(diffX) >= EdgeThreshold : Math.Abs(diffY) >= EdgeThreshold;
        }

        private static void UpdateBounds(ref int minX, ref int minY, ref int maxX, ref int maxY, int x, int y)
        {
            if (x < minX) minX = x; if (y < minY) minY = y;
            if (x > maxX) maxX = x; if (y > maxY) maxY = y;
        }

        private static float Median(List<int> values)
        {
            if (values == null || values.Count == 0) return float.NaN;

            var sorted = values.OrderBy(v => v).ToList();
            int n = sorted.Count;
            if (n % 2 == 1) return sorted[n / 2];

            return (sorted[n / 2 - 1] + sorted[n / 2]) * 0.5f;
        }

        private Point2f FindEdgePointByScan(Mat gray, Rect roi, out int pickedCount, out Rect pickedBounding)
        {
            pickedCount = 0;
            pickedBounding = new Rect(0, 0, 0, 0);

            int rows = gray.Rows;
            int cols = gray.Cols;
            int tol = Math.Max(0, ExtremeTol);

            var xs = new List<int>();
            var ys = new List<int>();

            int minX = cols, minY = rows, maxX = -1, maxY = -1;
            int lineCount = IsHorizontalScan ? rows : cols;

            for (int line = 1; line < lineCount - 1; line++)
            {
                bool found = false;
                if (IsHorizontalScan)
                {
                    int y = line;
                    if (IsForward) { for (int x = 1; x < cols - 1; x++) if (IsEdgeAt(gray, x, y)) { xs.Add(x); ys.Add(y); found = true; break; } }
                    else { for (int x = cols - 2; x >= 1; x--) if (IsEdgeAt(gray, x, y)) { xs.Add(x); ys.Add(y); found = true; break; } }
                }
                else
                {
                    int x = line;
                    if (IsForward) { for (int y = 1; y < rows - 1; y++) if (IsEdgeAt(gray, x, y)) { xs.Add(x); ys.Add(y); found = true; break; } }
                    else { for (int y = rows - 2; y >= 1; y--) if (IsEdgeAt(gray, x, y)) { xs.Add(x); ys.Add(y); found = true; break; } }
                }

                if (found)
                {
                    int curX = xs.Last(), curY = ys.Last();
                    pickedCount++;
                    UpdateBounds(ref minX, ref minY, ref maxX, ref maxY, curX, curY);
                    if (DrawStride <= 1 || (pickedCount % DrawStride) == 0)
                        _pickedEdgePoints.Add(new Point2f(roi.X + curX, roi.Y + curY));
                }
            }

            if (pickedCount == 0) return InvalidPoint;
            pickedBounding = new Rect(roi.X + minX, roi.Y + minY, (maxX - minX + 1), (maxY - minY + 1));

            if (IsHorizontalScan)
            {
                int extremeX = IsForward ? xs.Min() : xs.Max();
                double sumY = 0.0; int cntY = 0;
                for (int i = 0; i < xs.Count; i++)
                {
                    if (IsForward) { if (xs[i] <= extremeX + tol) { sumY += ys[i]; cntY++; } }
                    else { if (xs[i] >= extremeX - tol) { sumY += ys[i]; cntY++; } }
                }
                float yAvg = (cntY > 0) ? (float)(sumY / cntY) : (float)ys.Average();
                return new Point2f(roi.X + extremeX, roi.Y + yAvg);
            }
            else
            {
                float xCenter = Median(xs);
                int xCenterInt = (int)(xCenter + 0.5f);
                var ysNearX = new List<int>();
                for (int i = 0; i < xs.Count; i++) if (Math.Abs(xs[i] - xCenterInt) <= 2) ysNearX.Add(ys[i]);
                if (ysNearX.Count == 0) ysNearX = ys;

                float yValue = UseCenterYOnVertical ? Median(ysNearX) : (IsForward ? (float)ys.Min() : (float)ys.Max());
                return new Point2f(roi.X + xCenter, roi.Y + yValue);
            }
        }

        public override bool DoInspect()
        {
            ResetResult();
            _edgePoint = InvalidPoint; _edgeBoundingRect = new Rect(0, 0, 0, 0);
            OutEdgeCount = 0; _pickedEdgePoints.Clear();
            HasAnchor = false; AnchorPoint = InvalidPoint;

            // Align 결과 초기화
            HasAnchor = false;
            AnchorPoint = InvalidPoint;

            // 원본 취득
            Mat srcImage = Global.Inst.InspStage.GetMat(0, ImageChannel);
            SLogger.Write($"[Edge] ch={ImageChannel} src={(srcImage == null ? "NULL" : $"{srcImage.Width}x{srcImage.Height} c={srcImage.Channels()}")}");
            if (srcImage == null || srcImage.Empty()) return false;

            Rect roi = InspRect;
            if (roi.Width <= 1 || roi.Height <= 1 || roi.Right > srcImage.Width || roi.Bottom > srcImage.Height) return false;

            Mat gray = srcImage[roi];
            if (gray.Channels() == 3) Cv2.CvtColor(gray, gray, ColorConversionCodes.BGR2GRAY);

            // Align(기준점) 모드 분기
            if (UseAsAlignment)
            {
                var prevDir = ScanDir; ScanDir = ScanDirection.LeftToRight;
                _edgePoint = FindEdgePointByScan(gray, roi, out int pickedCountA, out Rect pickedBoundingA);
                ScanDir = prevDir;
                OutEdgeCount = pickedCountA; _edgeBoundingRect = pickedBoundingA;
                if (_edgePoint.X < 0) return false;
                HasAnchor = true; AnchorPoint = _edgePoint;
                IsDefect = false; IsInspected = true;
                return true;
            }

            // 기존 Edge 검사 모드
            _edgePoint = FindEdgePointByScan(gray, roi, out int pickedCount, out Rect pickedBounding);
            OutEdgeCount = pickedCount; _edgeBoundingRect = pickedBounding;
            IsDefect = false; IsInspected = true;
            ResultString.Add($"Point: ({_edgePoint.X:0.0}, {_edgePoint.Y:0.0})");
            return true;
        }

        public override int GetResultRect(out List<DrawInspectInfo> resultArea)
        {
            resultArea = new List<DrawInspectInfo>();
            if (_edgePoint.X < 0) return 0;

            // Align 모드면 표시 타입을 Align으로
            var drawType = UseAsAlignment ? InspectType.InspAlignEdge : InspectType.InspEdge;

            // 샘플 엣지 점들(노란색: DecisionType.Info)
            foreach (var pt in _pickedEdgePoints)
                resultArea.Add(new DrawInspectInfo(pt, "", InspectType.InspEdge, DecisionType.Info, ""));

            // 대표점 표시
            if (UseAsAlignment)
                resultArea.Add(new DrawInspectInfo(_edgePoint, "Anchor", drawType, DecisionType.Defect, ""));
            else
                resultArea.Add(new DrawInspectInfo(_edgePoint, $"Edge:{OutEdgeCount}", drawType, DecisionType.Good, ""));

            return resultArea.Count;
        }
    }
}