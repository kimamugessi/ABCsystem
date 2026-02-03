using System;
using System.Collections.Generic;
using System.Drawing;
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
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public class InspWorker
    {
        // --- [설정 및 상수] ---
        private const float SLOPE_LIMIT = 0.1f;
        private const float GAP_NO_CAP = 500f;
        private const float GAP_OK_MIN = 350f;
        private const float GAP_OK_MAX = 380f;

        private CancellationTokenSource _cts;
        private readonly InspectBoard _inspectBoard = new InspectBoard();

        // --- [상태 및 카운트] ---
        public bool IsRunning { get; private set; } = false;
        private int _totalCount = 0;
        private int _okCount = 0;
        private int _ngCount = 0;

        public void Stop() => _cts?.Cancel();

        public void ResetCounts() => _totalCount = _okCount = _ngCount = 0;

        // --- [메인 루프 제어] ---
        public void StartCycleInspectImage()
        {
            if (IsRunning) return;

            _cts = new CancellationTokenSource();
            Task.Run(() => InspectionLoop(_cts.Token));
        }

        private void InspectionLoop(CancellationToken token)
        {
            try
            {
                IsRunning = true;
                Global.Inst.InspStage.ResetImageIndex();
                Global.Inst.InspStage.SetWorkingState(WorkingState.INSPECT);
                SLogger.Write("InspectionLoop Start");

                while (!token.IsCancellationRequested)
                {
                    Global.Inst.InspStage.OneCycle();

                    int total = Global.Inst.InspStage.GetFileCount();
                    int current = Global.Inst.InspStage.GetCurrentFileIndex();

                    // 종료 조건 체크
                    if (total > 0 && (current + 1) >= total)
                    {
                        SLogger.Write("All images processed. Auto stopping.");
                        break;
                    }

                    Thread.Sleep(200);
                }
            }
            catch (Exception ex)
            {
                SLogger.Write($"[Loop Error] {ex.Message}");
            }
            finally
            {
                IsRunning = false;
                Global.Inst.InspStage.SetWorkingState(WorkingState.NONE);
                SLogger.Write("InspectionLoop End");
            }
        }

        // --- [핵심 검사 로직] ---
        public bool RunInspect(out bool isDefect)
        {
            isDefect = false;
            Model curModel = Global.Inst.InspStage.CurModel;
            if (curModel == null) return false;

            // 1. 알고리즘 실행
            foreach (var win in curModel.InspWindowList)
            {
                if (win != null) UpdateInspData(win);
            }
            _inspectBoard.InspectWindowList(curModel.InspWindowList);

            // 2. 판정 수행
            string status = CalculateJudge(out isDefect);

            // 3. 통계 업데이트
            _totalCount++;
            if (isDefect) _ngCount++; else _okCount++;

            // 4. UI 갱신 및 결과 저장
            ProcessResult(status, isDefect);

            return true;
        }

        private string CalculateJudge(out bool isDefect)
        {
            isDefect = false;
            var cameraForm = FormManager.GetForm<CameraForm>();
            if (cameraForm == null) return "NG";

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

            // 최종 isDefect 설정
            isDefect = (imageStatus != "OK");
            return imageStatus;
        }

        private void ProcessResult(string status, bool isDefect)
        {
            var cameraForm = FormManager.GetForm<CameraForm>();
            cameraForm?.SetInspResultCount(_totalCount, _okCount, _ngCount);

            if (isDefect)
            {
                int idx = Global.Inst.InspStage.GetCurrentFileIndex();
                string fileName = $"Insp_No_{idx + 1:D4}";

                // 별도 스레드에서 지연 후 저장 (UI 렌더링 대기)
                SaveDefectImage(status, fileName);
            }
        }

        private void SaveDefectImage(string status, string fileName)
        {
            try
            {
                string dir = Path.Combine(@"D:\불량", status);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                string fullPath = Path.Combine(dir, $"{fileName}_{DateTime.Now:HHmmss_fff}.jpg");

                // UI 캡처 대신 현재 검사 중인 원본 이미지를 직접 가져와 저장
                // Mat 데이터는 UI 변경의 영향을 받지 않습니다.
                using (var mat = Global.Inst.InspStage.GetMat(0, 0)) // 채널에 맞춰 수정
                {
                    if (mat != null && !mat.IsDisposed)
                    {
                        mat.SaveImage(fullPath);
                    }
                }
            }
            catch (Exception ex)
            {
                SLogger.Write($"[Save Fail] {ex.Message}");
            }
        }

        // --- [알고리즘 및 결과 표시] ---
        private bool UpdateInspData(InspWindow win)
        {
            if (win == null) return false;
            win.PatternLearn();
            foreach (var algo in win.AlgorithmList)
            {
                algo.TeachRect = algo.InspRect = win.WindowArea;
                algo.SetInspData(Global.Inst.InspStage.GetMat(0, algo.ImageChannel));
            }
            return true;
        }

        public bool TryInspect(InspWindow win, InspectType type)
        {
            if (win != null)
            {
                if (!UpdateInspData(win)) return false;
                _inspectBoard.Inspect(win);
                DisplayResultAll(Global.Inst.InspStage.CurModel.InspWindowList, InspectType.InspNone);
            }
            else
            {
                RunInspect(out _);
            }

            // 결과 폼 갱신
            var resForm = FormManager.GetForm<ResultForm>();
            if (win != null) resForm?.AddWindowResult(win);
            else resForm?.AddModelResult(Global.Inst.InspStage.CurModel);

            return true;
        }

        private void DisplayResultAll(List<InspWindow> windows, InspectType type)
        {
            if (windows == null) return;
            var totalArea = new List<DrawInspectInfo>();

            foreach (var win in windows)
            {
                foreach (var algo in win.AlgorithmList)
                {
                    if (type != InspectType.InspNone && algo.InspectType != type) continue;
                    if (algo.GetResultRect(out var res) > 0)
                    {
                        res.ForEach(d => d.windowUid = win.UID);
                        totalArea.AddRange(res);
                    }
                }
            }
            FormManager.GetForm<CameraForm>()?.AddRect(totalArea);
        }
    }

}