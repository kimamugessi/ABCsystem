using ABCsystem.Algorithm;
using ABCsystem.Core;
using ABCsystem.Setting;
using ABCsystem.Teach;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ABCsystem
{
    public partial class RunForm: Form
    {
        private InspWindow _win;
        private EdgeAlgorithm _algo;

        public RunForm()
        {
            InitializeComponent();

            btnEdge.Click += btnEdge_Click;
            cbEdgeType.SelectedIndexChanged += cbEdgeType_SelectedIndexChanged;

            // ROI 선택 변경 이벤트 구독
            Global.Inst.InspStage.SelectedInspWindowChanged += OnSelectedWindowChanged;

            // 폼 닫힐 때 구독 해제(메모리/중복호출 방지)
            this.FormClosed += (s, e) =>
            {
                Global.Inst.InspStage.SelectedInspWindowChanged -= OnSelectedWindowChanged;
            };

            // RunForm이 이미 떠있을 때 현재 선택 ROI 반영
            OnSelectedWindowChanged(Global.Inst.CurTeachWindow);
        }

        private void btnGrab_Click(object sender, EventArgs e)
        {
            Global.Inst.InspStage.CheckImageBuffer();
            Global.Inst.InspStage.Grab(0);
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            string serialID = $"{DateTime.Now:MM-dd HH:mm:ss}";
            Global.Inst.InspStage.InspectReady("LOT_NUMBER", serialID);

            if (SettingXml.Inst.CamType == Grab.CameraType.None)
            {
                bool cycleMode = SettingXml.Inst.CycleMode;
                //bool cycleMode = chkCycleMode.Checked;
                Global.Inst.InspStage.CycleInspect(cycleMode);
            }
            else
            {
                Global.Inst.InspStage.StartAutoRun();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Global.Inst.InspStage.StopCycle();
        }
        private void btnLive_Click(object sender, EventArgs e)
        {
            Global.Inst.InspStage.LiveMode = !Global.Inst.InspStage.LiveMode;

            if (Global.Inst.InspStage.LiveMode)
            {
                Global.Inst.InspStage.SetWorkingState(WorkingState.LIVE);

                Global.Inst.InspStage.CheckImageBuffer();
                Global.Inst.InspStage.Grab(0);
            }
            else
            {
                Global.Inst.InspStage.SetWorkingState(WorkingState.NONE);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //#15_INSP_WORKER#3 Cycle 모드 설정
        private void chkCycleMode_CheckedChanged(object sender, EventArgs e)
        {
            // 현재 체크 상태 확인
            bool isChecked = chkCycleMode.Checked;
            SettingXml.Inst.CycleMode = chkCycleMode.Checked;

        }

        public void SetAlgorithm(InspWindow win, EdgeAlgorithm algo)
        {
            _win = win;
            _algo = algo;

            cbEdgeType.SelectedIndexChanged -= cbEdgeType_SelectedIndexChanged;
            cbEdgeType.Items.Clear();

            if (_win == null || _algo == null)
            {
                cbEdgeType.SelectedIndexChanged += cbEdgeType_SelectedIndexChanged;
                return;
            }

            // Body: Align만
            if (_win.InspWindowType == InspWindowType.Body)
            {
                cbEdgeType.Items.Add("Align");

                // Align은 무조건 → 강제 + UseAsAlignment 강제
                _algo.UseAsAlignment = true;
                _algo.ScanDir = EdgeAlgorithm.ScanDirection.LeftToRight;

                cbEdgeType.SelectedItem = "Align";
            }

            // Base: 화살표만
            else if (_win.InspWindowType == InspWindowType.Base)
            {
                cbEdgeType.Items.AddRange(new object[] { "→", "←", "↑", "↓" });

                // Base는 Align 금지
                _algo.UseAsAlignment = false;

                // 현재 ScanDir에 맞춰 UI 선택
                var arrow = ToArrow(_algo.ScanDir);
                cbEdgeType.SelectedItem = arrow;
                if (cbEdgeType.SelectedItem == null) cbEdgeType.SelectedItem = "→";
            }
            // 기타 윈도우
            else
            {
                cbEdgeType.Items.AddRange(new object[] { "→", "←", "↑", "↓" });
                cbEdgeType.SelectedItem = "→";
                _algo.UseAsAlignment = false;
            }

            cbEdgeType.SelectedIndexChanged += cbEdgeType_SelectedIndexChanged;
        }

        private void btnEdge_Click(object sender, EventArgs e)
        {
            var win = _win ?? Global.Inst.CurTeachWindow;
            if (win == null) return;

            var algo = _algo ?? (win.FindInspAlgorithm(InspectType.InspEdge) as EdgeAlgorithm);
            if (algo == null) return;

            _win = win;
            _algo = algo;

            // 콤보 선택값을 알고리즘에 반영
            ApplyComboToAlgorithm();

            // 4. 검사 타입 결정 (Body 타입이면 AlignEdge로 실행)
            InspectType runType = (win.InspWindowType == InspWindowType.Body)
                                  ? InspectType.InspAlignEdge
                                  : InspectType.InspEdge;

            Global.Inst.InspStage.InspWorker.TryInspect(win, runType);

            if (runType == InspectType.InspAlignEdge && algo.HasAnchor)
                algo.TeachAnchorX = algo.AnchorPoint.X;

            Global.Inst.InspStage.RedrawMainView();
        }

        private void ApplyComboToAlgorithm()
        {
            if (_algo == null || _win == null) return;

            if (cbEdgeType.SelectedItem == null && cbEdgeType.Items.Count > 0)
                cbEdgeType.SelectedIndex = 0;

            // Body는 항상 Align 고정
            if (_win.InspWindowType == InspWindowType.Body)
            {
                _algo.UseAsAlignment = true;
                _algo.ScanDir = EdgeAlgorithm.ScanDirection.LeftToRight;
                return;
            }

            // Base는 항상 화살표 고정
            if (_win.InspWindowType == InspWindowType.Base)
            {
                _algo.UseAsAlignment = false;

                string arrow = cbEdgeType.SelectedItem?.ToString();
                if (string.IsNullOrWhiteSpace(arrow)) return;

                _algo.ScanDir = FromArrow(arrow);
                return;
            }

            // 기타는 기본 화살표 처리
            _algo.UseAsAlignment = false;
            string sel = cbEdgeType.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(sel)) return;
            _algo.ScanDir = FromArrow(sel);
        }

        private void OnSelectedWindowChanged(InspWindow win)
        {
            if (win == null)
            {
                SetAlgorithm(null, null);   // 콤보 비우기
                return;
            }

            var edgeAlgo = win.FindInspAlgorithm(InspectType.InspEdge) as EdgeAlgorithm;
            SetAlgorithm(win, edgeAlgo);    // 콤보 채우고 방향/Align 세팅
        }

        private void cbEdgeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyComboToAlgorithm();
        }

        private static EdgeAlgorithm.ScanDirection FromArrow(string arrow)
        {
            switch (arrow)
            {
                case "→": return EdgeAlgorithm.ScanDirection.LeftToRight;
                case "←": return EdgeAlgorithm.ScanDirection.RightToLeft;
                case "↑": return EdgeAlgorithm.ScanDirection.BottomToTop;
                case "↓": return EdgeAlgorithm.ScanDirection.TopToBottom;
                default: return EdgeAlgorithm.ScanDirection.LeftToRight;
            }
        }

        private static string ToArrow(EdgeAlgorithm.ScanDirection dir)
        {
            switch (dir)
            {
                case EdgeAlgorithm.ScanDirection.LeftToRight: return "→";
                case EdgeAlgorithm.ScanDirection.RightToLeft: return "←";
                case EdgeAlgorithm.ScanDirection.BottomToTop: return "↑";
                case EdgeAlgorithm.ScanDirection.TopToBottom: return "↓";
                default: return "→";
            }
        }
    }
}
