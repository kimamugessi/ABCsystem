using ABCsystem.Algorithm;
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

namespace ABCsystem.Property
{
    //sssong : 엣지 검사 Tab
    public partial class EdgeProp : UserControl
    {
        // Inspect Edge 버튼 클릭 이벤트(외부(PropertiesForm)에서 구독)
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

            // 알고리즘 값 -> 콤보 UI 동기화
            if (_algo != null)
                cbEdgeType.SelectedItem = ToArrow(_algo.ScanDir);
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
