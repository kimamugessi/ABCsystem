using ABCsystem.Algorithm;
using ABCsystem.Core;
using ABCsystem.Property;
using ABCsystem.Teach;
using ABCsystem.Util;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ABCsystem
{
    public partial class PropertiesForm : DockContent
    {
        //song
        private InspWindow _curWindow;

        //속성탭을 관리하기 위한 딕셔너리
        Dictionary<string, TabPage> _allTabs = new Dictionary<string, TabPage>();
        public PropertiesForm()
        {
            InitializeComponent();
        }
        private void LoadOptionControl(InspectType inspType)   //속성 탭이 이미 있다면 그것을 반환(1), 없다면 새로 생성(2)
        {
            string tabName = inspType.ToString();

            foreach (TabPage tabPage in tabPropControl.TabPages)    //(1)
            {
                if (tabPage.Text == tabName) return;
            }

            if (_allTabs.TryGetValue(tabName, out TabPage page))    //딕셔너리에 있으면 추가
            {
                tabPropControl.TabPages.Add(page);
                return;
            }

            UserControl _inspProp = CreateUserControl(inspType);    //새 UserControl 생성
            if (_inspProp == null) return;

            TabPage newTab = new TabPage(tabName)     //(2)
            {
                Dock = DockStyle.Fill
            };
            _inspProp.Dock = DockStyle.Fill;
            newTab.Controls.Add(_inspProp);
            tabPropControl.TabPages.Add(newTab);
            tabPropControl.SelectedTab = newTab;    //새 탭 선택

            _allTabs[tabName] = newTab;
        }

        private UserControl CreateUserControl(InspectType inspPropType)    //속성 탭 생성하는 매서드
        {
            UserControl curProp = null;
            switch (inspPropType)
            {
                case InspectType.InspBinary:
                    BinaryProp blobProp = new BinaryProp();

                    blobProp.RangeChanged += RangeSlider_RangeChanged;
                    //blobProp.PropertyChanged += PropertyChanged;
                    blobProp.ImageChannelChanged += ImageChannelChanged;
                    curProp = blobProp;
                    break;
                case InspectType.InspMatch:
                    MatchInspProp matchProp = new MatchInspProp();
                    //matchProp.PropertyChanged += PropertyChanged;
                    curProp = matchProp;
                    break;
                case InspectType.InspFilter:
                    ImageFilterProp filterProp = new ImageFilterProp();
                    curProp = filterProp;
                    break;
                case InspectType.InspAIModule:
                    AIModuleProp aiModuleProp = new AIModuleProp();
                    curProp = aiModuleProp;
                    break;
                //song
                case InspectType.InspEdge:
                    {
                        EdgeProp edgeProp = new EdgeProp();

                        // 버튼 클릭 이벤트 연결
                        edgeProp.InspectEdgeClicked += InspectEdgeRequested;

                        curProp = edgeProp;
                        break;
                    }
                default:
                    MessageBox.Show("유효하지 않은 옵션입니다.");
                    return null;
            }
            return curProp;
        }

        //song : EdgeProp의 InspectEdgeClicked 이벤트가 호출할 함수
        private void InspectEdgeRequested()
        {
            var win = Global.Inst.CurTeachWindow;
            SLogger.Write($"[EDGE_BTN] Clicked. win={(win == null ? "null" : win.UID)}");

            if (win == null) return;

            var edgeAlgo = win.FindInspAlgorithm(InspectType.InspEdge) as EdgeAlgorithm;
            if (edgeAlgo != null && edgeAlgo.EdgeThreshold <= 0)
                edgeAlgo.EdgeThreshold = 30;   // 임시 기본 임계값. ROI 명암에 따라 조정해야함. 지금 흑백에선 30 ㄱㅊ
            SLogger.Write($"[EDGE_BTN] edgeAlgo={(edgeAlgo == null ? "null" : "ok")} " + $"thr={(edgeAlgo?.EdgeThreshold.ToString() ?? "-")} " + $"scanDir={(edgeAlgo == null ? "-" : edgeAlgo.ScanDir.ToString())}");



            Global.Inst.InspStage.InspWorker.TryInspect(win, InspectType.InspEdge);
            Global.Inst.InspStage.RedrawMainView();
        }

        public void ShowProperty(InspWindow window)
        {
            Global.Inst.CurTeachWindow = window; //song

            foreach (InspAlgorithm algo in window.AlgorithmList)
            {
                LoadOptionControl(algo.InspectType);
            }

            //song : 탭 만든 뒤 실제 알고리즘 값 연결
            UpdateProperty(window);
        }

        public void ResetProperty() {  tabPropControl.TabPages.Clear(); }
        public void UpdateProperty(InspWindow window)
        {
            Global.Inst.CurTeachWindow = window; //song

            if (window == null) return;
            foreach (TabPage tabPage in tabPropControl.TabPages)
            {
                if (tabPage.Controls.Count > 0)
                {
                    UserControl uc = tabPage.Controls[0] as UserControl;
                    if (uc is BinaryProp binaryProp)
                    {
                        BlobAlgorithm blobAlgo = (BlobAlgorithm)window.FindInspAlgorithm(InspectType.InspBinary);
                        if (blobAlgo == null) continue;

                        binaryProp.SetAlgorithm(blobAlgo);
                    }
                    else if (uc is MatchInspProp matchProp)
                    {
                        MatchAlgorithm matchAlgo = (MatchAlgorithm)window.FindInspAlgorithm(InspectType.InspMatch);
                        if (matchAlgo == null)
                            continue;

                        window.PatternLearn();

                        matchProp.SetAlgorithm(matchAlgo);
                    }
                    //song
                    else if (uc is EdgeProp edgeProp)
                    {
                        EdgeAlgorithm edgeAlgo = (EdgeAlgorithm)window.FindInspAlgorithm(InspectType.InspEdge);
                        if (edgeAlgo == null) continue;

                        edgeProp.SetAlgorithm(window, edgeAlgo);
                    }
                }
            }
        }
        private void RangeSlider_RangeChanged(object sender, RangeChangedEventArgs e)
        {
            int lowerValue = e.LowerValue;
            int upperValue = e.UpperValue;
            bool invert = e.Invert;
            ShowBinaryMode showBinMode = e.ShowBinMode;
            Global.Inst.InspStage.PreView?.SetBinary(lowerValue, upperValue, invert, showBinMode);
        }

        //private void PropertyChanged(object sender, EventArgs e)
        //{
        //    Global.Inst.InspStage.RedrawMainView();
        //}
        private void ImageChannelChanged(object sender, ImageChannelEventArgs e)
        {
            Global.Inst.InspStage.SetPreviewImage(e.Channel);
        }
        public EdgeProp EdgePropControl
        {
            get
            {
                // 탭 컨트롤에서 EdgeProp 타입을 찾아 반환합니다.
                foreach (TabPage page in tabPropControl.TabPages)
                {
                    if (page.Controls.Count > 0 && page.Controls[0] is EdgeProp ctrl)
                        return ctrl;
                }
                return null;
            }
        }
    }
}
