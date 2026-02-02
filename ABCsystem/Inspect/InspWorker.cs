using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            // 1. 검사 시작 전 인덱스와 카운트를 초기화 (버튼 눌렀을 때 한 번 수행)
            Global.Inst.InspStage.ResetImageIndex();

            Global.Inst.InspStage.SetWorkingState(WorkingState.INSPECT);
            IsRunning = true;

            SLogger.Write("InspectionLoop Start");

            while (!token.IsCancellationRequested)
            {
                // 2. 검사 수행
                Global.Inst.InspStage.OneCycle();

                int totalCount = Global.Inst.InspStage.GetFileCount();
                int currentIndex = Global.Inst.InspStage.GetCurrentFileIndex();

                //SLogger.Write($"Progress: {currentIndex + 1} / {totalCount}");

                // 3. [종료 조건] 마지막 이미지까지 검사를 마쳤다면 루프 탈출
                if (totalCount > 0 && (currentIndex + 1) >= totalCount)
                {
                    SLogger.Write("All images processed. Auto stopping.");
                    break;
                }

                Thread.Sleep(200);
            }

            // 4. 최종 종료 처리 (상태를 NONE으로 변경하여 버튼을 다시 누를 수 있게 함)
            IsRunning = false;
            Global.Inst.InspStage.SetWorkingState(WorkingState.NONE);
            SLogger.Write("InspectionLoop End");
        }

        private int _totalAccumulatedCount = 0;
        private int _okAccumulatedCount = 0;
        private int _ngAccumulatedCount = 0;
        public void ResetCounts()
        {
            _totalAccumulatedCount = 0;
            _okAccumulatedCount = 0;
            _ngAccumulatedCount = 0;
        }
        public bool RunInspect(out bool isDefect)
        {
            isDefect = false;
            Model curMode = Global.Inst.InspStage.CurModel;

            // 1. 기존 알고리즘 검사 로직 실행
            foreach (var win in curMode.InspWindowList) { if (win != null) UpdateInspData(win); }
            _inspectBoard.InspectWindowList(curMode.InspWindowList);

            // 2. UI 객체(Viewer) 데이터 가져오기
            var cameraForm = FormManager.GetForm<CameraForm>();
            if (cameraForm == null) return false;

            var viewer = cameraForm.ImageViewer;
            var heightLines = viewer.GetHeightLineList(); // List<DiagramEntity[]>

            // 3. DrawHeightLine과 동일한 수식으로 이미지 판정
            string imageStatus = "OK"; // 기본값은 OK

            if (heightLines == null || heightLines.Count == 0)
            {
                imageStatus = "NG"; // 검사 라인이 없으면 NG 처리
            }
            else
            {
                foreach (var lineSet in heightLines)
                {
                    if (lineSet == null || lineSet.Length < 3) continue;

                    // 좌표 획득 (Public으로 변경한 메서드 사용)
                    PointF vP1 = viewer.GetEdgePoint(lineSet[0]);
                    PointF vP2 = viewer.GetEdgePoint(lineSet[1]);
                    PointF vP3 = viewer.GetEdgePoint(lineSet[2]);

                    float dx = vP2.X - vP1.X;
                    float dy = vP2.Y - vP1.Y;

                    if (Math.Abs(dx) > 0.0001f)
                    {
                        float slope = Math.Abs(dy / dx);
                        if (slope > 0.1f) { imageStatus = "NG"; break; } // 기울기 불량

                        float targetY = vP1.Y + (dy / dx) * (vP3.X - vP1.X);
                        float pixelLength = Math.Abs(targetY - vP3.Y);

                        // --- DrawHeightLine 판정 기준과 100% 동일화 ---
                        if (pixelLength >= 500)    //+기준 길이 조건에 따라 수정(텍스트 색상): 그외 빨강색
                        {
                            imageStatus = "NO CAP"; // NO CAP은 불량으로 간주
                            break;
                        }
                        else if (pixelLength >= 350 && pixelLength <= 380)  //+기준 길이 조건에 따라 수정(텍스트 색상): 350px 이상 360px 이하 시 라임색
                        {
                            // 이 라인은 OK, 다음 라인 계속 체크
                        }
                        else
                        {
                            imageStatus = "NG";
                            break;
                        }
                    }
                    else { imageStatus = "NG"; break; }
                }
            }

            // 4. 누적 카운트 업데이트 (이미지 단위)
            _totalAccumulatedCount++;

            if (imageStatus == "OK")
            {
                _okAccumulatedCount++;
                isDefect = false;
            }
            else
            {
                _ngAccumulatedCount++; // NG 또는 NO CAP일 때
                isDefect = true;
            }

            // 5. UI 결과 업데이트
            // 이제 파라미터로 넘기는 값들은 ROI 개수가 아닌 '누적 이미지 수'입니다.
            cameraForm.SetInspResultCount(_totalAccumulatedCount, _okAccumulatedCount, _ngAccumulatedCount);

            return true;
        }
        public bool TryInspect(InspWindow inspObj, InspectType inspType)
        {
            SLogger.Write($"[TryInspect] win={inspObj?.UID} inspTypeParam={inspType}");

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

            ResultForm resultForm = FormManager.GetForm<ResultForm>();
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
                        // 각 결과에 windowUid 주입 (ROI 식별)
                        foreach (var di in resultArea)
                        {
                            if (string.IsNullOrEmpty(di.windowUid))
                                di.windowUid = win.UID;
                        }

                        totalArea.AddRange(resultArea);
                    }
                }
            }

            var cameraForm = FormManager.GetForm<CameraForm>();
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
                var cameraForm = FormManager.GetForm<CameraForm>();
                if (cameraForm != null)
                {
                    cameraForm.AddRect(totalArea);
                }
            }

            return true;
        }
    }
}

