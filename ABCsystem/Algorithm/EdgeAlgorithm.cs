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
        public int DrawStride { get; set; } = 1;

        [XmlIgnore]
        public OpenCvSharp.Point2f FoundEdgePoint => _edgePoint;

        // 엣지 윤곽선 시각화용 점 표시 간격 (1: 모든 엣지 점 표시, 5: 5픽셀마다 1점 표시)
        public int DrawStride { get; set; } = 1;

        // 스캔 방향 : 라인별 "첫 엣지"를 찾는 탐색 방향
        public enum ScanDirection
        {
            LeftToRight, RightToLeft, TopToBottom, BottomToTop
        }

        public ScanDirection ScanDir { get; set; } = ScanDirection.LeftToRight;
        public int ExtremeTol { get; set; } = 1;
        public bool UseCenterYOnVertical { get; set; } = true;
        public bool UseAsAlignment { get; set; } = false;
        public double TeachAnchorX { get; set; } = double.NaN;

        [XmlIgnore] public int OutEdgeCount { get; private set; } = 0;
        [XmlIgnore] private Rect _edgeBoundingRect = new Rect(0, 0, 0, 0);
        private Point2f _edgePoint = InvalidPoint;
        [XmlIgnore] private readonly List<Point2f> _pickedEdgePoints = new List<Point2f>();

        [XmlIgnore] public bool HasAnchor { get; private set; } = false;
        [XmlIgnore] public Point2f AnchorPoint { get; private set; }

        // 세로 스캔(↑/↓)에서 대표점 Y 기준
        // true  : 엣지 라인의 중앙값 사용 (권장, 방향 무관)
        // false : 스캔 방향에 따른 extreme(Y) 사용
        public bool UseCenterYOnVertical { get; set; } = true;

        // Align(기준점)용
        public bool UseAsAlignment { get; set; } = false;   // InspAlignEdge일 때 true
        public double TeachAnchorX { get; set; } = double.NaN;  // 정상 모델에서 저장할 기준 X. 아직 티칭 전이면 NaN 유지

        // 런타임 결과 (XML 저장 X)
        // 라인별로 픽킹된 엣지 수 (라인당 1개씩)
        [XmlIgnore]
        public int OutEdgeCount { get; private set; } = 0;
        // 픽킹된 점들의 bounding box(ROI 전체좌표계 기준). 디버그/참고용.
        [XmlIgnore]
        private Rect _edgeBoundingRect = new Rect(0, 0, 0, 0);
        // 대표점(전체좌표계 기준) / 화면 표시
        private Point2f _edgePoint = InvalidPoint;
        // 라인별로 찾은 "첫 엣지" 점들 중 일부를 화면에 같이 찍기 위한 리스트(전체좌표)
        [XmlIgnore]
        private readonly List<Point2f> _pickedEdgePoints = new List<Point2f>();

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
                // 세로 스캔(↓/↑)
                // x 라인마다 "첫 엣지"가 1개씩 픽킹되므로, 픽킹 점들이 엣지 라인을 따라 분포
                // 대표점은 엣지 라인의 중앙 x에서의 y로 잡는 것이 방향/노이즈에 덜 민감

                // 대표 X = 전체 픽킹 점들의 중앙값
                float xCenter = Median(xs);
                int xCenterInt = (int)(xCenter + 0.5f);

                // 대표 Y 후보: xCenter 근처 점들의 y를 모은다
                int tolX = 2; // y를 수집하기 위한 허용 오차(px). 필요하면 1~3 정도로 조절
                var ysNearX = new List<int>();

                for (int i = 0; i < xs.Count; i++)
                {
                    if (Math.Abs(xs[i] - xCenterInt) <= tolX)
                        ysNearX.Add(ys[i]);
                }

                if (ysNearX.Count == 0)
                {
                    // 혹시 중앙 근처에 점이 없으면 전체 y의 중앙값으로 fallback
                    ysNearX = ys;
                }

                float yValue;

                if (UseCenterYOnVertical)
                {
                    // 중앙 y(혹은 중앙 x 부근 y 중앙값) 추출
                    yValue = Median(ysNearX);
                }
                else
                {
                    // 기존 방식 extremeY 사용
                    int extremeY = IsForward ? ys.Min() : ys.Max();
                    yValue = extremeY;
                }

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
            if (srcImage == null || srcImage.Empty()) return false;

            Rect roi = InspRect;
            if (roi.Width <= 1 || roi.Height <= 1 || roi.Right > srcImage.Width || roi.Bottom > srcImage.Height) return false;

            Mat gray = srcImage[roi];
            if (gray.Channels() == 3) Cv2.CvtColor(gray, gray, ColorConversionCodes.BGR2GRAY);

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

            // Align(기준점) 모드 분기
            if (UseAsAlignment)
            {
                // 기준점은 "무조건 좌 -> 우" 스캔 + 첫 엣지
                var prevDir = ScanDir;
                ScanDir = ScanDirection.LeftToRight;

                _edgePoint = FindEdgePointByScan(gray, roi, out int pickedCountA, out Rect pickedBoundingA);

                // 원복(혹시라도 다른 로직에서 ScanDir 참조할까봐)
                ScanDir = prevDir;

                OutEdgeCount = pickedCountA;
                _edgeBoundingRect = pickedBoundingA;

                if (_edgePoint.X < 0 || _edgePoint.Y < 0)
                {
                    // 뚜껑 없음 등: 기준점 실패
                    HasAnchor = false;
                    IsDefect = true;
                    IsInspected = true;

                    ResultString.Add("NG: Align anchor edge not found");
                    ResultString.Add($"ScanDir(forced): LeftToRight");
                    ResultString.Add($"EdgeThreshold: {EdgeThreshold}");
                    ResultString.Add($"PickedEdgeCount: {OutEdgeCount}");

                    return false;
                }

                // 성공
                HasAnchor = true;
                AnchorPoint = _edgePoint;

                IsDefect = false;
                IsInspected = true;

                ResultString.Add("OK: Align anchor found");
                ResultString.Add($"ScanDir(forced): LeftToRight");
                ResultString.Add($"EdgeThreshold: {EdgeThreshold}");
                ResultString.Add($"PickedEdgeCount: {OutEdgeCount}");
                ResultString.Add($"AnchorPoint: ({AnchorPoint.X:0.0}, {AnchorPoint.Y:0.0})");

                // 티칭값이 있으면 참고로 출력
                if (!double.IsNaN(TeachAnchorX))
                    ResultString.Add($"TeachAnchorX: {TeachAnchorX:0.0}");

                return true;
            }

            // 기존 Edge 검사 모드
            // 방향 스캔으로 대표점 산출
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

            var drawType = UseAsAlignment ? InspectType.InspAlignEdge : InspectType.InspEdge;

            // Align 모드면 표시 타입을 Align으로
            var drawType = UseAsAlignment ? InspectType.InspAlignEdge : InspectType.InspEdge;

            // 샘플 엣지 점들(노란색: DecisionType.Info)
            foreach (var pt in _pickedEdgePoints)
                resultArea.Add(new DrawInspectInfo(pt, "", InspectType.InspEdge, DecisionType.Info, ""));

            // 대표점 표시
            if (UseAsAlignment)
            {
                // 기준점은 텍스트를 Anchor로 표시(원하면 dx는 InspectBoard에서 계산 후 별도 표시)
                resultArea.Add(new DrawInspectInfo(_edgePoint, $"Anchor", drawType, DecisionType.Defect));
            }
            else
            {
                // 대표점(초록색) + 텍스트(EdgeCount) 화면 표시
                resultArea.Add(new DrawInspectInfo(_edgePoint, $"Edge:{OutEdgeCount}", drawType, DecisionType.Good));
            }
            if (UseAsAlignment)
                resultArea.Add(new DrawInspectInfo(_edgePoint, "Anchor", drawType, DecisionType.Defect, ""));
            else
                resultArea.Add(new DrawInspectInfo(_edgePoint, $"Edge:{OutEdgeCount}", drawType, DecisionType.Good, ""));

            return resultArea.Count;
        }
    }
}