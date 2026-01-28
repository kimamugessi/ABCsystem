using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABCsystem.Algorithm;
using ABCsystem.Core;
using ABCsystem.Teach;
using OpenCvSharp;

namespace ABCsystem.Inspect
{
    public class InspectBoard
    {
        public InspectBoard() { }
        public bool Inspect(InspWindow window) 
        {
            if (window == null)
                return false;
            if (!InspectWindow(window))
                return false;
            return true;
        }
        private bool InspectWindow(InspWindow window)
        {
            window.ResetInspResult();
            foreach (InspAlgorithm algo in window.AlgorithmList)
            {
                if (algo.IsUse == false) continue;
                if (!algo.DoInspect()) return false;

                string resultInfo = string.Join("\r\n", algo.ResultString);

                InspResult inspResult = new InspResult
                {
                    ObjectID = window.UID,
                    InspType = algo.InspectType,
                    IsDefect = algo.IsDefect,
                    ResultInfos = resultInfo
                };

                switch (algo.InspectType)
                {
                    case InspectType.InspMatch:
                        MatchAlgorithm matchAlgo = algo as MatchAlgorithm;
                        inspResult.ResultValue = $"{matchAlgo.OutScore}";
                        break;
                    case InspectType.InspBinary:
                        BlobAlgorithm blobAlgo = algo as BlobAlgorithm;
                        int min = blobAlgo.BlobFilters[blobAlgo.FILTER_COUNT].min;
                        int max = blobAlgo.BlobFilters[blobAlgo.FILTER_COUNT].max;

                        inspResult.ResultValue = $"{blobAlgo.OutBlobCount}/{min}~{max}";
                        break;
                }
                List<DrawInspectInfo> resultArea = new List<DrawInspectInfo>();
                int resultCnt = algo.GetResultRect(out resultArea);

                foreach (var di in resultArea)
                    di.windowUid = window.UID;

                inspResult.ResultRectList = resultArea;

                window.AddInspResult(inspResult);
            }
            return true;

        }
        public bool InspectWindowList(List<InspWindow> windowList)
        {
            if (windowList == null || windowList.Count <= 0)
                return false;

            Point alignOffset = new Point(0, 0);

            // Body 윈도우에서 AlignEdge 먼저 실행해서 alignOffset 계산
            InspWindow bodyWindow = windowList.Find(w => w.InspWindowType == Core.InspWindowType.Body);
            if (bodyWindow == null)
                return false; // Body 없으면 정렬 불가

            EdgeAlgorithm alignAlgo = bodyWindow.FindInspAlgorithm(InspectType.InspAlignEdge) as EdgeAlgorithm;
            if (alignAlgo == null || !alignAlgo.IsUse)
                return false; // AlignEdge 없으면 정렬 불가

            // AlignEdge는 offset 적용 전(TeachRect 기준)에서 실행되어야 함
            if (!InspectWindow(bodyWindow))
                return false;

            // TeachAnchorX가 없으면(티칭 안 됨) 실패 처리
            if (double.IsNaN(alignAlgo.TeachAnchorX))
                return false;

            // 기준점 못 찾으면(뚜껑 없음 등) 실패 처리
            if (!alignAlgo.HasAnchor || alignAlgo.AnchorPoint.X < 0)
                return false;

            // dx = 현재 - 티칭 (물체가 오른쪽으로 가면 +)
            int dx = (int)Math.Round(alignAlgo.AnchorPoint.X - alignAlgo.TeachAnchorX);
            alignOffset = new Point(dx, 0);

            // alignOffset 적용 후 나머지 윈도우 검사
            // Body는 이미 AlignEdge로 1회 검사됨(중복 방지)
            foreach (InspWindow window in windowList)
            {
                window.SetInspOffset(alignOffset);

                if (window.InspWindowType == Core.InspWindowType.Body)
                    continue; // Body는 Align 계산용으로 이미 1번 검사했으니 스킵

                if (!InspectWindow(window))
                    return false;
            }

            return true;
        }
    }
}
