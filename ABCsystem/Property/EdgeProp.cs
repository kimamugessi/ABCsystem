using ABCsystem.Algorithm;
using ABCsystem.Core;
using ABCsystem.Teach;
using System;
using System.Windows.Forms;

namespace ABCsystem.Property
{
    public partial class EdgeProp : UserControl
    {
        // Inspect Edge 버튼 클릭 이벤트 (PropertiesForm 에서 구독)
        public event Action InspectEdgeClicked;

        private InspWindow _win;
        private EdgeAlgorithm _algo;

        public EdgeProp()
        {
            InitializeComponent();

            btnEdge.Click += btnEdge_Click;
            cbEdgeType.SelectedIndexChanged += cbEdgeType_SelectedIndexChanged;
        }

        public void SetAlgorithm(InspWindow win, EdgeAlgorithm algo)
        {
            _win = win;
            _algo = algo;

            if (_algo == null) return;

            // Body 윈도우면 기본값 Align로
            bool defaultAlign = (_win != null && _win.InspWindowType == InspWindowType.Body);

            // 이미 Align 모드로 저장돼 있으면 그대로, 아니면 Body면 Align로 맞춤
            bool isAlign = _algo.UseAsAlignment || _algo.InspectType == InspectType.InspAlignEdge;

            if (!isAlign && defaultAlign)
            {
                // 내부 값도 Align로 맞춰두면 UX가 훨씬 자연스러움
                _algo.UseAsAlignment = true;
                _algo.InspectType = InspectType.InspAlignEdge;
                _algo.ScanDir = EdgeAlgorithm.ScanDirection.LeftToRight;

                cbEdgeType.SelectedItem = "Align";
                return;
            }

            // 일반 처리
            cbEdgeType.SelectedItem = isAlign ? "Align" : ToArrow(_algo.ScanDir);
        }


        private void btnEdge_Click(object sender, EventArgs e)
        {
            // 버튼 누르기 직전에 한번 더 확실히 반영
            ApplyComboToAlgorithm();

            InspectEdgeClicked?.Invoke();
        }

        private void cbEdgeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyComboToAlgorithm();
        }

        private void ApplyComboToAlgorithm()
        {
            if (_algo == null) return;

            string arrow = cbEdgeType.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(arrow)) return;

            // Align 선택 시
            if (arrow.Equals("Align", StringComparison.OrdinalIgnoreCase))
            {
                _algo.UseAsAlignment = true;
                _algo.InspectType = InspectType.InspAlignEdge;
                _algo.ScanDir = EdgeAlgorithm.ScanDirection.LeftToRight;
                return;
            }

            // 스캔방향 선택 시
            _algo.UseAsAlignment = false;
            _algo.InspectType = InspectType.InspEdge;
            _algo.ScanDir = FromArrow(arrow);
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
