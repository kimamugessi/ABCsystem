using ABCsystem.Algorithm;
using ABCsystem.Teach;
using ABCsystem.Core;
using ABCsystem.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ABCsystem.Util;


namespace ABCsystem
{
    public partial class RunForm: Form
    {

        // RunForm.cs 내부 (클래스 멤버)
        private InspWindow _edgeWin;
        private EdgeAlgorithm _edgeAlgo;
        private bool _edgeUiBinding = false; // cb 이벤트 재진입 방지용

        public RunForm()
        {
            InitializeComponent();

            cbEdgeType.SelectedIndex = 0;
            btnEdge.Click += btnEdge_Click;
            cbEdgeType.SelectedIndexChanged += cbEdgeType_SelectedIndexChanged;
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
                bool cycleMode = chkCycleMode.Checked;
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

        private void btnEdge_Click(object sender, EventArgs e)
        {
            var win = Global.Inst.CurTeachWindow;
            if (win == null)
            {
                SLogger.Write("[EDGE_BTN] CurTeachWindow is null");
                return;
            }

            var algo = win.FindInspAlgorithm(InspectType.InspEdge) as EdgeAlgorithm;
            if (algo == null)
            {
                SLogger.Write($"[EDGE_BTN] EdgeAlgorithm not found. win={win.UID} type={win.InspWindowType}");
                return;
            }

            algo.UseAsAlignment = false;

            var arrow = cbEdgeType.SelectedItem?.ToString();
            if (!string.IsNullOrWhiteSpace(arrow))
                algo.ScanDir = FromArrow(arrow);

            if (algo.EdgeThreshold <= 0)
                algo.EdgeThreshold = 30;

            SLogger.Write($"[EDGE_BTN] run win={win.UID} scanDir={algo.ScanDir} thr={algo.EdgeThreshold}");

            Global.Inst.InspStage.InspWorker.TryInspect(win, InspectType.InspEdge);
            Global.Inst.InspStage.RedrawMainView();
        }

        private void cbEdgeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_edgeUiBinding) return;

            ApplyComboToAlgorithm();
        }

        private void ApplyComboToAlgorithm()
        {
            if (_edgeAlgo == null || _edgeWin == null) return;

            _edgeAlgo.UseAsAlignment = false;

            string arrow = cbEdgeType.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(arrow))
            {
                _edgeAlgo.ScanDir = EdgeAlgorithm.ScanDirection.LeftToRight; // 기본 → 
                return;
            }

            _edgeAlgo.ScanDir = FromArrow(arrow);
        }

        public void SetEdgeAlgorithm(InspWindow win, EdgeAlgorithm algo)
        {
            _edgeWin = win;
            _edgeAlgo = algo;

            _edgeUiBinding = true;
            try
            {
                cbEdgeType.Items.Clear();

                if (_edgeWin == null || _edgeAlgo == null)
                {
                    cbEdgeType.Enabled = false;
                    btnEdge.Enabled = false;

                    cbEdgeType.SelectedIndex = 0;
                    return;
                }

                cbEdgeType.Enabled = true;
                btnEdge.Enabled = true;

                _edgeAlgo.UseAsAlignment = false;

                var arrow = ToArrow(_edgeAlgo.ScanDir);
                cbEdgeType.SelectedItem = arrow ?? "→";
                if (cbEdgeType.SelectedItem == null) cbEdgeType.SelectedItem = "→";
            }
            finally
            {
                _edgeUiBinding = false;
            }
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
