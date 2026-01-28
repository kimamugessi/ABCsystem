using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ABCsystem.Algorithm;
using ABCsystem.Core;
using ABCsystem.Teach;
using ABCsystem.Util;
using OpenCvSharp;

namespace ABCsystem.Inspect
{
    public class InspWorker
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();

        private InspectBoard _inspectBoard = new InspectBoard();

        public bool IsRunning { get; set; } = false;

        public InspWorker()
        {
        }
        public void Stop() { _cts.Cancel(); }

        public void StartCycleInspectImage()
        {
            _cts = new CancellationTokenSource();
            Task.Run(() => InspectionLoop(this, _cts.Token));
        }

        private void InspectionLoop(InspWorker inspWorker, CancellationToken token)
        {
            Global.Inst.InspStage.SetWorkingState(WorkingState.INSPECT);

            SLogger.Write("InspectionLoop Start");

            IsRunning = true;

            while (!token.IsCancellationRequested)
            {
                Global.Inst.InspStage.OneCycle();
            }

            IsRunning = false;

            SLogger.Write("InspectionLoop End");
        }
        public bool RunInspect(out bool isDefect)
        {
            isDefect = false;
            Model curMode = Global.Inst.InspStage.CurModel;
            if (curMode == null) return false;

            List<InspWindow> inspWindowList = curMode.InspWindowList;

            // 1. 검사 시작 전 모든 윈도우의 이전 결과값을 강제로 초기화
            foreach (var inspWindow in inspWindowList)
            {
                if (inspWindow == null) continue;

                // 이전 결과 리스트와 알고리즘 내부 좌표(-1, -1) 초기화
                inspWindow.ResetInspResult();

                // 현재 이미지에 맞춰 데이터 업데이트
                UpdateInspData(inspWindow);
            }

            // 2. 실제 알고리즘 연산 수행 (이때 새로운 좌표가 계산됨)
            _inspectBoard.InspectWindowList(inspWindowList);

            int totalCnt = 0;
            int okCnt = 0;
            int ngCnt = 0;

            foreach (var inspWindow in inspWindowList)
            {
                if (inspWindow == null) continue;
                totalCnt++;

                if (inspWindow.IsDefect())
                {
                    isDefect = true;
                    ngCnt++;
                }
                else
                {
                    okCnt++;
                }
            }

            // 3. 결과 표시 (이미지 갱신 직후에 그리기)
            DisplayResultAll(inspWindowList, InspectType.InspNone);

            // UI 카운트 갱신
            if (totalCnt > 0)
            {
                var cameraForm = MainForm.GetDockForm<CameraForm>();
                cameraForm?.SetInspResultCount(totalCnt, okCnt, ngCnt);
            }

            return true;
        }
        public bool TryInspect(InspWindow inspObj, InspectType inspType)
        {
            if (inspObj != null)
            {
                if (!UpdateInspData(inspObj))
                    return false;

                _inspectBoard.Inspect(inspObj);

                //DisplayResult(inspObj, inspType); //song

                // song : Camera Viewer처럼 전체 ROI 결과를 다시 그리기
                Model curMode = Global.Inst.InspStage.CurModel;
                DisplayResultAll(curMode.InspWindowList, InspectType.InspNone);
            }
            else
            {
                bool isDefect = false;
                RunInspect(out isDefect);
            }

            ResultForm resultForm = MainForm.GetDockForm<ResultForm>();
            if (resultForm != null)
            {
                if (inspObj != null)
                    resultForm.AddWindowResult(inspObj);
                else
                {
                    Model curMode = Global.Inst.InspStage.CurModel;
                    resultForm.AddModelResult(curMode);
                }
            }
             
            return true;
        }

        //각 알고리즘 타입 별로 검사에 필요한 데이터를 입력하는 함수
        private bool UpdateInspData(InspWindow inspWindow)
        {
            if (inspWindow == null)
                return false;

            Rect windowArea = inspWindow.WindowArea;

            inspWindow.PatternLearn();

            foreach (var inspAlgo in inspWindow.AlgorithmList)
            {
                //검사 영역 초기화
                inspAlgo.TeachRect = windowArea;
                inspAlgo.InspRect = windowArea;  // ROI 갱신

                Mat srcImage = Global.Inst.InspStage.GetMat(0, inspAlgo.ImageChannel);
                inspAlgo.SetInspData(srcImage);
            }

            return true;
        }

        //song
        // 여러 InspWindow의 결과를 한 번에 모아서 cameraForm에 출력한다.
        // (AddRect 내부에서 Clear가 발생하더라도 "전체 결과"를 다시 넣기 때문에 점이 유지됨)
        private bool DisplayResultAll(List<InspWindow> windows, InspectType inspType)
        {
            if (windows == null)
                return false;

            List<DrawInspectInfo> totalArea = new List<DrawInspectInfo>();

            for (int winIndex = 0; winIndex < windows.Count; winIndex++)
            {
                var win = windows[winIndex];
                if (win == null) continue;

                List<InspAlgorithm> inspAlgorithmList = win.AlgorithmList;
                foreach (var algorithm in inspAlgorithmList)
                {
                    if (algorithm.InspectType != inspType && inspType != InspectType.InspNone)
                        continue;

                    List<DrawInspectInfo> resultArea;
                    int resultCnt = algorithm.GetResultRect(out resultArea);
                    if (resultCnt > 0 && resultArea != null)
                    {
                        // 각 결과에 windowId 주입 (ROI 식별)
                        foreach (var di in resultArea)
                        {
                            di.windowId = winIndex;   // 또는 win.Id 같은 게 있으면 그걸로
                        }

                        totalArea.AddRange(resultArea);
                    }
                }
            }

            var cameraForm = MainForm.GetDockForm<CameraForm>();
            if (cameraForm != null)
            {
                cameraForm.AddRect(totalArea);
            }

            return true;
        }

        //InspWindow내의 알고리즘 중에서, 인자로 입력된 알고리즘과 같거나,
        //인자가 None이면 모든 알고리즘의 검사 결과(Rect 영역)를 얻어, cameraForm에 출력한다.
        private bool DisplayResult(InspWindow inspObj, InspectType inspType)
        {
            if (inspObj == null)
                return false;

            List<DrawInspectInfo> totalArea = new List<DrawInspectInfo>();

            List<InspAlgorithm> inspAlgorithmList = inspObj.AlgorithmList;
            foreach (var algorithm in inspAlgorithmList)
            {
                if (algorithm.InspectType != inspType && inspType != InspectType.InspNone)
                    continue;

                List<DrawInspectInfo> resultArea = new List<DrawInspectInfo>();
                int resultCnt = algorithm.GetResultRect(out resultArea);
                if (resultCnt > 0)
                {
                    totalArea.AddRange(resultArea);
                }
            }

            if (totalArea.Count > 0)
            {
                //찾은 위치를 이미지상에서 표시
                var cameraForm = MainForm.GetDockForm<CameraForm>();
                if (cameraForm != null)
                {
                    cameraForm.AddRect(totalArea);
                }
            }

            return true;
        }
    }
}

