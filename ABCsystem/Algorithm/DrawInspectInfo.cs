using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABCsystem.Core;
using OpenCvSharp;

namespace ABCsystem.Algorithm
{
    public class DrawInspectInfo
    {
        public Rect rect;
        public Point2f[] rotatedPoints;
        public string info;
        public InspectType inspectType;
        public DecisionType decision;
        public bool UseRotatedRect=false;

        //song
        public string  windowUid = "";

        //song : 점 표시용
        public bool UsePoint = false;
        public Point2f point;

        public DrawInspectInfo()
        {
            rect = new Rect();
            rotatedPoints = null;
            info=string.Empty;
            inspectType = InspectType.InspNone;
            decision=DecisionType.None;
        }

        public DrawInspectInfo(Rect _rect, string _info, InspectType _inspectType, DecisionType _decision, string _windowUid = "")
        {
            rect = _rect;
            info = _info;
            inspectType = _inspectType;
            decision = _decision;
            windowUid = _windowUid; //song
        }

        //song : 점 생성자
        public DrawInspectInfo(Point2f _point, string _info, InspectType _inspectType, DecisionType _decision, string _windowUid = "")
        {
            point = _point;
            info = _info;
            inspectType = _inspectType;
            decision = _decision;
            UsePoint = true;
            rect = new Rect(0, 0, 0, 0);
            windowUid = _windowUid; //song
        }

        public void SetRotatedRectPoints(Point2f[] _rotatedPoints)
        {
            if (_rotatedPoints == null) return;
            rotatedPoints = new Point2f[_rotatedPoints.Length];
            for (int i = 0; i < _rotatedPoints.Length; i++)
            {
                rotatedPoints[i] = _rotatedPoints[i];
            }
            UseRotatedRect = true;
        }
    }
}
