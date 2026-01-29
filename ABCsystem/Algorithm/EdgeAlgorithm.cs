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
            cloneAlgo.UseAsAlignment = this.UseAsAlignment;
            cloneAlgo.TeachAnchorX = this.TeachAnchorX;
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
            UseAsAlignment = src.UseAsAlignment;
            TeachAnchorX = src.TeachAnchorX;
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

            foreach (var pt in _pickedEdgePoints)
                resultArea.Add(new DrawInspectInfo(pt, "", InspectType.InspEdge, DecisionType.Info, ""));

            if (UseAsAlignment)
                resultArea.Add(new DrawInspectInfo(_edgePoint, "Anchor", drawType, DecisionType.Defect, ""));
            else
                resultArea.Add(new DrawInspectInfo(_edgePoint, $"Edge:{OutEdgeCount}", drawType, DecisionType.Good, ""));

            return resultArea.Count;
        }
    }
}