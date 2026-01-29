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
            if (_algo == null || _win == null) return;

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
