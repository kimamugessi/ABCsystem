using ABCsystem.Core;
using ABCsystem.Util;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace ABCsystem.Algorithm
{
    // EdgeAlgorithm.cs
    // ROI 내부에서 지정된 스캔 방향(→ ← ↑ ↓)으로 라인별 "첫 엣지"를 찾고 대표점을 계산하는 클래스
    // 결과는 대표점(_edgePoint) 1개와, 라인별 픽킹 개수(OutEdgeCount)로 제공
    public class EdgeAlgorithm : InspAlgorithm
    {
        // 엣지 검출 실패/미검출
        private static readonly Point2f InvalidPoint = new Point2f(-1, -1);

        // 엣지로 판단할 임계값 (클수록 엣지 덜 검출)
        public int EdgeThreshold { get; set; } = 30;

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

        // 엣지 윤곽선 시각화용 점 표시 간격 (1: 모든 엣지 점 표시, 5: 5픽셀마다 1점 표시)
        public int DrawStride { get; set; } = 1;

        // 스캔 방향 : 라인별 "첫 엣지"를 찾는 탐색 방향
        public enum ScanDirection
        {
            LeftToRight,   // →
            RightToLeft,   // ←
            TopToBottom,   // ↓
            BottomToTop    // ↑
        }

        // 기본값: →
        public ScanDirection ScanDir { get; set; } = ScanDirection.LeftToRight;

        // 대표점 계산 시 extreme(가장 먼저 만난 경계) 근처의 점들을 함께 사용하기 위한 허용 범위(px)
        // 노이즈로 인해 extreme이 1~2px 튀는 현상 완화
        public int ExtremeTol { get; set; } = 1;

        // 세로 스캔(↑/↓)에서 대표점 Y 기준
        // true  : 엣지 라인의 중앙값 사용 (권장, 방향 무관)
        // false : 스캔 방향에 따른 extreme(Y) 사용
        public bool UseCenterYOnVertical { get; set; } = true;

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

            return cloneAlgo;
        }

        public override bool CopyFrom(InspAlgorithm sourceAlgo)
        {
            var src = (EdgeAlgorithm)sourceAlgo;

            EdgeThreshold = src.EdgeThreshold;
            ScanDir = src.ScanDir;
            ExtremeTol = src.ExtremeTol;
            UseCenterYOnVertical = src.UseCenterYOnVertical;

            return true;
        }

        // true: y를 고정하고 x를 움직이는 스캔 (→/←)
        // false: x를 고정하고 y를 움직이는 스캔 (↓/↑)
        private bool IsHorizontalScan => (ScanDir == ScanDirection.LeftToRight || ScanDir == ScanDirection.RightToLeft);

        // true: 좌표 증가 방향 스캔 (→/↓)
        // false: 좌표 감소 방향 스캔 (←/↑)
        private bool IsForward => (ScanDir == ScanDirection.LeftToRight || ScanDir == ScanDirection.TopToBottom);

        // ROI 내부 좌표(x,y)가 엣지인지 판단
        // →/← : 좌우 변화(diffX) 기준 (수직 경계)
        // ↑/↓ : 상하 변화(diffY) 기준 (수평 경계)
        private bool IsEdgeAt(Mat gray, int x, int y)
        {
            // 경계 1픽셀은 이웃 참조가 불가능하므로 검사 제외
            if (x <= 0 || y <= 0 || x >= gray.Cols - 1 || y >= gray.Rows - 1)
                return false;

            // OpenCvSharp 버전/환경 호환을 위해 Get<T>() 사용
            byte pL = gray.Get<byte>(y, x - 1);
            byte pR = gray.Get<byte>(y, x + 1);
            byte pU = gray.Get<byte>(y - 1, x);
            byte pD = gray.Get<byte>(y + 1, x);

            int diffX = pR - pL;
            int diffY = pD - pU;

            return IsHorizontalScan
                ? Math.Abs(diffX) >= EdgeThreshold
                : Math.Abs(diffY) >= EdgeThreshold;
        }

        // 픽킹된 점들의 bounding box 업데이트(ROI 내부 좌표)
        private static void UpdateBounds(ref int minX, ref int minY, ref int maxX, ref int maxY, int x, int y)
        {
            if (x < minX) minX = x;
            if (y < minY) minY = y;
            if (x > maxX) maxX = x;
            if (y > maxY) maxY = y;
        }

        // 정수 리스트의 중앙값(median). 짝수개면 두 중앙값 평균
        private static float Median(List<int> values)
        {
            if (values == null || values.Count == 0) return float.NaN;

            values.Sort();
            int n = values.Count;
            if (n % 2 == 1) return values[n / 2];
            return (values[n / 2 - 1] + values[n / 2]) * 0.5f;
        }

        // 방향 스캔으로 라인별 "첫 엣지"를 픽킹하고 대표점을 계산
        private Point2f FindEdgePointByScan(Mat gray, Rect roi, out int pickedCount, out Rect pickedBounding)
        {
            pickedCount = 0; // 라인별로 픽킹된 점 개수
            pickedBounding = new Rect(0, 0, 0, 0); // 픽킹된 점들의 bounding box(전체좌표)

            int rows = gray.Rows;
            int cols = gray.Cols;
            int tol = Math.Max(0, ExtremeTol);

            // 픽킹된 점(ROI 내부 좌표)
            var xs = new List<int>();
            var ys = new List<int>();

            int minX = cols, minY = rows, maxX = -1, maxY = -1;

            // "라인" 기준 축 설정
            // →/← : y라인을 고정하고 x를 움직임 (y라인 수 = rows)
            // ↑/↓ : x라인을 고정하고 y를 움직임 (x라인 수 = cols)
            int lineCount = IsHorizontalScan ? rows : cols;

            for (int line = 1; line < lineCount - 1; line++)
            {
                bool found = false;

                if (IsHorizontalScan)
                {
                    int y = line;

                    if (IsForward) // →
                    {
                        for (int x = 1; x < cols - 1; x++)
                        {
                            if (IsEdgeAt(gray, x, y))
                            {
                                xs.Add(x);
                                ys.Add(y);

                                pickedCount++;
                                UpdateBounds(ref minX, ref minY, ref maxX, ref maxY, x, y);

                                if (DrawStride <= 1 || (pickedCount % DrawStride) == 0)
                                    _pickedEdgePoints.Add(new Point2f(roi.X + x, roi.Y + y));

                                found = true;
                                break;
                            }
                        }
                    }
                    else // ←
                    {
                        for (int x = cols - 2; x >= 1; x--)
                        {
                            if (IsEdgeAt(gray, x, y))
                            {
                                xs.Add(x);
                                ys.Add(y);

                                pickedCount++;
                                UpdateBounds(ref minX, ref minY, ref maxX, ref maxY, x, y);

                                if (DrawStride <= 1 || (pickedCount % DrawStride) == 0)
                                    _pickedEdgePoints.Add(new Point2f(roi.X + x, roi.Y + y));

                                found = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    int x = line;

                    if (IsForward) // ↓
                    {
                        for (int y = 1; y < rows - 1; y++)
                        {
                            if (IsEdgeAt(gray, x, y))
                            {
                                xs.Add(x);
                                ys.Add(y);

                                pickedCount++;
                                UpdateBounds(ref minX, ref minY, ref maxX, ref maxY, x, y);

                                if (DrawStride <= 1 || (pickedCount % DrawStride) == 0)
                                    _pickedEdgePoints.Add(new Point2f(roi.X + x, roi.Y + y));

                                found = true;
                                break;
                            }
                        }
                    }
                    else // ↑
                    {
                        for (int y = rows - 2; y >= 1; y--)
                        {
                            if (IsEdgeAt(gray, x, y))
                            {
                                xs.Add(x);
                                ys.Add(y);

                                pickedCount++;
                                UpdateBounds(ref minX, ref minY, ref maxX, ref maxY, x, y);

                                if (DrawStride <= 1 || (pickedCount % DrawStride) == 0)
                                    _pickedEdgePoints.Add(new Point2f(roi.X + x, roi.Y + y));

                                found = true;
                                break;
                            }
                        }
                    }
                }

                if (!found) continue;
            }

            if (pickedCount == 0)
                return InvalidPoint;

            // bounding box(픽킹 점 기준, 전체좌표)
            if (maxX >= minX && maxY >= minY)
            {
                pickedBounding = new Rect(
                    roi.X + minX,
                    roi.Y + minY,
                    (maxX - minX + 1),
                    (maxY - minY + 1)
                );
            }

            // 대표점 계산
            if (IsHorizontalScan)
            {
                // →면 가장 작은 x, ←면 가장 큰 x
                int extremeX = IsForward ? xs.Min() : xs.Max();

                // extremeX 근처 y 평균 (노이즈 완화)
                double sumY = 0.0;
                int cntY = 0;

                for (int i = 0; i < xs.Count; i++)
                {
                    if (IsForward)
                    {
                        if (xs[i] <= extremeX + tol) { sumY += ys[i]; cntY++; }
                    }
                    else
                    {
                        if (xs[i] >= extremeX - tol) { sumY += ys[i]; cntY++; }
                    }
                }

                float yAvg = (cntY > 0) ? (float)(sumY / cntY) : (float)ys.Average();
                return new Point2f(roi.X + extremeX, roi.Y + yAvg);
            }
            else
            {
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

            _edgePoint = InvalidPoint;
            _edgeBoundingRect = new Rect(0, 0, 0, 0);
            OutEdgeCount = 0;
            _pickedEdgePoints.Clear();

            // 원본 취득
            Mat srcImage = Global.Inst.InspStage.GetMat(0, ImageChannel);
            if (srcImage == null || srcImage.Empty())
            {
                ResultString.Add("NG: srcImage is null/empty");
                IsDefect = true;
                IsInspected = true;
                return false;
            }

            // ROI 유효성 체크
            Rect roi = InspRect;
            if (roi.Width <= 1 || roi.Height <= 1 ||
                roi.X < 0 || roi.Y < 0 ||
                roi.Right > srcImage.Width || roi.Bottom > srcImage.Height)
            {
                ResultString.Add("NG: ROI(InspRect) is invalid");
                IsDefect = true;
                IsInspected = true;
                return false;
            }

            // ROI 잘라오기
            Mat roiImg = srcImage[roi];

            // ROI가 컬러면 그레이 변환
            Mat gray = roiImg;
            if (roiImg.Type() == MatType.CV_8UC3)
            {
                gray = new Mat();
                Cv2.CvtColor(roiImg, gray, ColorConversionCodes.BGR2GRAY);
            }

            // 방향 스캔으로 대표점 산출
            _edgePoint = FindEdgePointByScan(gray, roi, out int pickedCount, out Rect pickedBounding);

            OutEdgeCount = pickedCount;
            _edgeBoundingRect = pickedBounding;

            // 현재는 NG 판정 사용 안 함. Good 표시
            IsDefect = false;

            ResultString.Add($"ScanDir: {ScanDir}");
            ResultString.Add($"EdgeThreshold: {EdgeThreshold}");
            ResultString.Add($"PickedEdgeCount: {OutEdgeCount}");
            ResultString.Add($"Point: ({_edgePoint.X:0.0}, {_edgePoint.Y:0.0})");

            IsInspected = true;
            return true;
        }

        public override int GetResultRect(out List<DrawInspectInfo> resultArea)
        {
            resultArea = new List<DrawInspectInfo>();

            // 유효한 대표점이 없으면 화면 표시 안함
            if (_edgePoint.X < 0 || _edgePoint.Y < 0)
                return resultArea.Count;

            // 샘플 엣지 점들(노란색: DecisionType.Info)
            foreach (var pt in _pickedEdgePoints)
            {
                resultArea.Add(new DrawInspectInfo(pt, "", InspectType.InspEdge, DecisionType.Info));
            }

            // 대표점(초록색) + 텍스트(EdgeCount) 화면 표시
            resultArea.Add(new DrawInspectInfo(_edgePoint, $"Edge:{OutEdgeCount}", InspectType.InspEdge, DecisionType.Good));

            return resultArea.Count;
        }
    }
}
