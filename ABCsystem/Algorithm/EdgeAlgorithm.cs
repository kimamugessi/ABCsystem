using ABCsystem.Core;
using ABCsystem.Util;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ABCsystem.Algorithm
{
    //song : 엣지 추출 알고리즘
    public class EdgeAlgorithm : InspAlgorithm
    {
        // 경계(에지)로 판단할 임계값 (클수록 에지 덜 검출)
        public int EdgeThreshold { get; set; } = 30;

        // 결과: 에지 픽셀 수 - 라인별 1개씩 픽킹된 수
        public int OutEdgeCount { get; private set; } = 0;

        // 결과: 에지 Bounding Box - 픽킹된 점들 기준
        [XmlIgnore]
        private Rect _edgeBoundingRect = new Rect(0, 0, 0, 0);

        // 결과: 화면에 찍을 점(대표점)
        private Point2f _edgePoint = new Point2f(-1, -1);

        // 어떤 끝점을 찍을지 (일단 유지/사용안할수도있음)
        public enum EdgePointMode
        {
            Left,   // 오른쪽 끝점 (maxX)
            Right,  // 왼쪽 끝점 (minX)
            Top     // 아래 끝점 (maxY)
        }

        //기존 시스템과의 호환성 때문에 의도적으로 살려둔 코드
        //임시 (지금 알고리즘이랑 동작이랑 무관)
        public EdgePointMode PointMode { get; set; } = EdgePointMode.Left;

        //스캔 방향
        public enum ScanDirection
        {
            LeftToRight,   // →
            RightToLeft,   // ←
            TopToBottom,   // ↓
            BottomToTop    // ↑
        }

        // 기본값 : 기존 동작 느낌에 가장 가까운 → 로
        public ScanDirection ScanDir { get; set; } = ScanDirection.LeftToRight;

        // extreme 근처 모으는 허용 픽셀(노이즈 줄이기)
        public int ExtremeTol { get; set; } = 1;

        public EdgeAlgorithm()
        {
            InspectType = InspectType.InspEdge;
        }

        public override InspAlgorithm Clone()
        {
            var cloneAlgo = new EdgeAlgorithm();
            CopyBaseTo(cloneAlgo);

            cloneAlgo.EdgeThreshold = this.EdgeThreshold;
            cloneAlgo.PointMode = this.PointMode;
            cloneAlgo.ScanDir = this.ScanDir;
            cloneAlgo.ExtremeTol = this.ExtremeTol;

            return cloneAlgo;
        }

        public override bool CopyFrom(InspAlgorithm sourceAlgo)
        {
            EdgeAlgorithm src = (EdgeAlgorithm)sourceAlgo;
            this.EdgeThreshold = src.EdgeThreshold;
            this.PointMode = src.PointMode;
            this.ScanDir = src.ScanDir;
            this.ExtremeTol = src.ExtremeTol;

            return true;
        }

        // (x,y)가 엣지인지 판단
        private bool IsEdgeAt(Mat gray, int x, int y)
        {
            // 경계 1픽셀은 검사 불가
            if (x <= 0 || y <= 0 || x >= gray.Cols - 1 || y >= gray.Rows - 1)
                return false;

            byte pL = gray.At<byte>(y, x - 1);
            byte pR = gray.At<byte>(y, x + 1);
            byte pU = gray.At<byte>(y - 1, x);
            byte pD = gray.At<byte>(y + 1, x);

            int diffX = pR - pL;
            int diffY = pD - pU;

            // →/← : 수직 경계(좌우 변화) = diffX
            // ↑/↓ : 수평 경계(상하 변화) = diffY
            switch (ScanDir)
            {
                case ScanDirection.LeftToRight:
                case ScanDirection.RightToLeft:
                    return Math.Abs(diffX) >= EdgeThreshold;

                case ScanDirection.TopToBottom:
                case ScanDirection.BottomToTop:
                    return Math.Abs(diffY) >= EdgeThreshold;

                default:
                    return false;
            }
        }

        // 방향 스캔으로 "한 줄당 첫 엣지"를 픽킹해서 대표점을 만든다
        private Point2f FindEdgePointByScan(Mat gray, Rect roi, out int pickedCount, out Rect pickedBounding)
        {
            pickedCount = 0;
            pickedBounding = new Rect(0, 0, 0, 0);

            int rows = gray.Rows;
            int cols = gray.Cols;
            int tol = Math.Max(0, ExtremeTol);

            // 픽킹된 점(ROI 내부 좌표)
            List<int> xs = new List<int>();
            List<int> ys = new List<int>();

            int minX = cols, minY = rows, maxX = -1, maxY = -1;

            // ---- → / ← : y라인마다 x방향으로 첫 엣지 ----
            if (ScanDir == ScanDirection.LeftToRight || ScanDir == ScanDirection.RightToLeft)
            {
                for (int y = 1; y < rows - 1; y++)
                {
                    bool found = false;

                    if (ScanDir == ScanDirection.LeftToRight)
                    {
                        for (int x = 1; x < cols - 1; x++)
                        {
                            if (IsEdgeAt(gray, x, y))
                            {
                                xs.Add(x);
                                ys.Add(y);
                                found = true;
                                break;
                            }
                        }
                    }
                    else // RightToLeft
                    {
                        for (int x = cols - 2; x >= 1; x--)
                        {
                            if (IsEdgeAt(gray, x, y))
                            {
                                xs.Add(x);
                                ys.Add(y);
                                found = true;
                                break;
                            }
                        }
                    }

                    if (found)
                    {
                        pickedCount++;
                        int xPicked = xs[xs.Count - 1];
                        int yPicked = ys[ys.Count - 1];

                        if (xPicked < minX) minX = xPicked;
                        if (yPicked < minY) minY = yPicked;
                        if (xPicked > maxX) maxX = xPicked;
                        if (yPicked > maxY) maxY = yPicked;
                    }
                }

                if (pickedCount == 0)
                    return new Point2f(-1, -1);

                // extremeX 결정 (→면 minX가 "가장 먼저 만나는 경계", ←면 maxX)
                int extremeX = (ScanDir == ScanDirection.LeftToRight) ? xs.Min() : xs.Max();

                // extremeX 근처의 y를 평균내서 대표 y
                double sumY = 0.0;
                int cntY = 0;

                for (int i = 0; i < xs.Count; i++)
                {
                    if (ScanDir == ScanDirection.LeftToRight)
                    {
                        if (xs[i] <= extremeX + tol) { sumY += ys[i]; cntY++; }
                    }
                    else
                    {
                        if (xs[i] >= extremeX - tol) { sumY += ys[i]; cntY++; }
                    }
                }

                float yAvg = (cntY > 0) ? (float)(sumY / cntY) : (float)ys.Average();

                // bounding rect (픽킹 점 기준)
                if (maxX >= minX && maxY >= minY)
                {
                    pickedBounding = new Rect(
                        roi.X + minX,
                        roi.Y + minY,
                        (maxX - minX + 1),
                        (maxY - minY + 1)
                    );
                }

                // 대표점(전체 좌표)
                return new Point2f(roi.X + extremeX, roi.Y + yAvg);
            }
            // ---- ↑ / ↓ : x라인마다 y방향으로 첫 엣지 ----
            else
            {
                for (int x = 1; x < cols - 1; x++)
                {
                    bool found = false;

                    if (ScanDir == ScanDirection.TopToBottom)
                    {
                        for (int y = 1; y < rows - 1; y++)
                        {
                            if (IsEdgeAt(gray, x, y))
                            {
                                xs.Add(x);
                                ys.Add(y);
                                found = true;
                                break;
                            }
                        }
                    }
                    else // BottomToTop
                    {
                        for (int y = rows - 2; y >= 1; y--)
                        {
                            if (IsEdgeAt(gray, x, y))
                            {
                                xs.Add(x);
                                ys.Add(y);
                                found = true;
                                break;
                            }
                        }
                    }

                    if (found)
                    {
                        pickedCount++;
                        int xPicked = xs[xs.Count - 1];
                        int yPicked = ys[ys.Count - 1];

                        if (xPicked < minX) minX = xPicked;
                        if (yPicked < minY) minY = yPicked;
                        if (xPicked > maxX) maxX = xPicked;
                        if (yPicked > maxY) maxY = yPicked;
                    }
                }

                if (pickedCount == 0)
                    return new Point2f(-1, -1);

                // extremeY 결정 (↓면 minY가 "가장 먼저 만나는 경계", ↑면 maxY)
                int extremeY = (ScanDir == ScanDirection.TopToBottom) ? ys.Min() : ys.Max();

                // extremeY 근처의 x를 median으로 (좌우 쏠림 방지)
                List<int> xsNear = new List<int>();
                for (int i = 0; i < ys.Count; i++)
                {
                    if (ScanDir == ScanDirection.TopToBottom)
                    {
                        if (ys[i] <= extremeY + tol) xsNear.Add(xs[i]);
                    }
                    else
                    {
                        if (ys[i] >= extremeY - tol) xsNear.Add(xs[i]);
                    }
                }

                if (xsNear.Count == 0)
                    return new Point2f(-1, -1);

                xsNear.Sort();
                float xMed = (xsNear.Count % 2 == 1)
                    ? xsNear[xsNear.Count / 2]
                    : (xsNear[xsNear.Count / 2 - 1] + xsNear[xsNear.Count / 2]) * 0.5f;

                // bounding rect (픽킹 점 기준)
                if (maxX >= minX && maxY >= minY)
                {
                    pickedBounding = new Rect(
                        roi.X + minX,
                        roi.Y + minY,
                        (maxX - minX + 1),
                        (maxY - minY + 1)
                    );
                }

                // 대표점(전체 좌표)
                return new Point2f(roi.X + xMed, roi.Y + extremeY);
            }
        }

        public override bool DoInspect()
        {
            ResetResult();

            //결과값 초기화
            _edgePoint = new Point2f(-1, -1);
            _edgeBoundingRect = new Rect(0, 0, 0, 0);
            OutEdgeCount = 0;

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

            // 컬러로 들어오면 그레이로 변환
            Mat gray = roiImg;
            if (roiImg.Type() == MatType.CV_8UC3)
            {
                gray = new Mat();
                Cv2.CvtColor(roiImg, gray, ColorConversionCodes.BGR2GRAY);
            }

            // 방향 스캔으로 대표점 산출
            int pickedCount;
            Rect pickedBounding;
            _edgePoint = FindEdgePointByScan(gray, roi, out pickedCount, out pickedBounding);

            OutEdgeCount = pickedCount;
            _edgeBoundingRect = pickedBounding;

            // 판정(현재는 엣지 못 찾으면 NG로 할지 애매해서 기존처럼 Good 처리 유지)
            IsDefect = false;

            ResultString.Add($"(Legacy) PointMode: {PointMode}");
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

            if (_edgePoint.X < 0 || _edgePoint.Y < 0)
                return resultArea.Count;

            // 최종 대표점(초록색으로 표시 Good)
            resultArea.Add(new DrawInspectInfo(_edgePoint, $"Edge:{OutEdgeCount}", InspectType.InspEdge, DecisionType.Good));

            return resultArea.Count;
        }
    }
}
