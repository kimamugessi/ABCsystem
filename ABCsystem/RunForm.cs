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

        public void SetAlgorithm(InspWindow win, EdgeAlgorithm algo)
        {
            _win = win;
            _algo = algo;

            cbEdgeType.SelectedIndexChanged -= cbEdgeType_SelectedIndexChanged;
            cbEdgeType.Items.Clear();

            // 아이템을 무조건 추가 (ROI를 선택했을 때 목록이 비어있지 않게 함)
            cbEdgeType.Items.AddRange(new object[] { "→", "←", "↑", "↓" });

            if (_win == null || _algo == null)
            {
                // 알고리즘이 없더라도 기본적으로 첫 번째 아이템 선택
                cbEdgeType.SelectedIndex = 0;
                cbEdgeType.SelectedIndexChanged += cbEdgeType_SelectedIndexChanged;
                return;
            }

            _algo.UseAsAlignment = false;
            cbEdgeType.SelectedItem = ToArrow(_algo.ScanDir);

            if (cbEdgeType.SelectedItem == null) cbEdgeType.SelectedIndex = 0;

            cbEdgeType.SelectedIndexChanged += cbEdgeType_SelectedIndexChanged;
        }

        private void btnEdge_Click(object sender, EventArgs e)
        {
            var win = Global.Inst.CurTeachWindow;
            if (win == null) return;

            // 1. 기존 알고리즘 제거
            win.AlgorithmList.RemoveAll(a => a.InspectType == InspectType.InspEdge || a.InspectType == InspectType.InspAlignEdge);

            // 2. 새로운 알고리즘 객체 생성
            EdgeAlgorithm newAlgo = new EdgeAlgorithm();

            // 수정된 부분: SelectedItem이 null인지 확인하고, null이면 기본값 "→" 사용
            string selectedArrow = "→";
            if (cbEdgeType.SelectedItem != null)
            {
                selectedArrow = cbEdgeType.SelectedItem.ToString();
            }
            newAlgo.ScanDir = FromArrow(selectedArrow);

            win.AlgorithmList.Add(newAlgo);

            // 3. 검사 실행
            Global.Inst.InspStage.InspWorker.TryInspect(win, newAlgo.InspectType);
            Global.Inst.InspStage.RedrawMainView();
        }
        private void ApplyComboToAlgorithm()
        {
            if (_algo == null || _win == null) return;

            // 선택된 텍스트가 "←" 인지 디버깅으로 확인해보세요.
            string sel = cbEdgeType.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(sel)) return;

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
