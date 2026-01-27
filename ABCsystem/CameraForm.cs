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
using System.IO;
using ABCsystem.Core;
using OpenCvSharp;
using ABCsystem.Algorithm;
using ABCsystem.Teach;
using ABCsystem.UIControl;
using ABCsystem.Util;

namespace ABCsystem
{
    //public partial class CameraForm: Form
    public partial class CameraForm : DockContent
    {
        eImageChannel _currentImageChannel = eImageChannel.Gray;
        public CameraForm()
        {
            InitializeComponent();

            this.FormClosed += CameraForm_FormClosed;

            imageViewer.DiagramEntityEvent += ImageViewer_DiagramEntityEvent;

            mainViewToolbar.ButtonChanged += Toolbar_ButtonChanged;

        }

        private void ImageViewer_DiagramEntityEvent(object sender, DiagramEntityEventArgs e)
        {
            SLogger.Write($"ImageViewer Action {e.ActionType.ToString()}");
            switch (e.ActionType)
            {
                case EntityActionType.Select:
                    // 1. 이미지 뷰어에서 현재 선택된 모든 엔티티의 UID 리스트 가져오기
                    // (ImageViewCtrl의 _multiSelectedEntities를 활용하거나 EventArgs에 리스트가 있다면 사용)
                    var selectedUids = imageViewer.GetSelectedUids();

                    // 2. MainForm을 통해 ModelTreeForm의 노드들을 선택 표시
                    var modelTreeForm = MainForm.GetDockForm<ModelTreeForm>();
                    if (modelTreeForm != null)
                    {
                        modelTreeForm.SelectNodesByUids(selectedUids);
                    }
                    break;
                case EntityActionType.Inspect:
                    UpdateDiagramEntity();
                    Global.Inst.InspStage.TryInspection(e.InspWindow);
                    break;
                case EntityActionType.Add:
                    Global.Inst.InspStage.AddInspWindow(e.WindowType, e.Rect);
                    break;
                case EntityActionType.Copy:
                    Global.Inst.InspStage.AddInspWindow(e.InspWindow, e.OffsetMove);
                    break;
                case EntityActionType.Move:
                    Global.Inst.InspStage.MoveInspWindow(e.InspWindow, e.OffsetMove);
                    break;
                case EntityActionType.Resize:
                    Global.Inst.InspStage.ModifyInspWindow(e.InspWindow, e.Rect);
                    break;
                case EntityActionType.Delete:
                    Global.Inst.InspStage.DelInspWindow(e.InspWindow);
                    break;
                case EntityActionType.DeleteList:
                    Global.Inst.InspStage.DelInspWindow(e.InspWindowList);
                    break;
            }
        }

        private void imageViewer_Load(object sender, EventArgs e)
        {

        }

        public void LoadImage(string filePath)  //이미지 경로받아 PictureBox에 이미지를 로드
        {
            if (File.Exists(filePath) == false) return; //파일이 없다면 리턴

            Image bitmap = Image.FromFile(filePath);
            imageViewer.LoadBitmap((Bitmap)bitmap);
        }

        public Mat GetDisplayImage()
        {
            return Global.Inst.InspStage.ImageSpace.GetMat(0,_currentImageChannel);
        }

        private void CameraForm_Resize(object sender, EventArgs e)
        {
            int margin = 0;
            imageViewer.Width = this.Width - mainViewToolbar.Width-margin * 2;
            imageViewer.Height = this.Height - margin * 2;

            imageViewer.Location = new System.Drawing.Point(margin, margin);
        }

        public void UpdateDisplay(Bitmap bitmap = null)
        {
            if (bitmap == null)
            {
                //#6_INSP_STAGE#3 업데이트시 bitmap이 없다면 InspSpace에서 가져온다
                bitmap = Global.Inst.InspStage.GetBitmap(0,_currentImageChannel);
                if (bitmap == null)
                    return;
            }

            if (imageViewer != null)
                imageViewer.LoadBitmap(bitmap);
        }

        public void UpdateImageViewer()
        {
            imageViewer.UpdateInspParam();
            imageViewer.Invalidate();
        }

        public void UpdateDiagramEntity()
        {
            //imageViewer.ResetEntity();
            // ROI(엔티티)만 새로 그리되, 검사 결과(_rectInfos)는 유지
            imageViewer.ResetEntity(clearResults: false);

            Model model = Global.Inst.InspStage.CurModel;
            List<DiagramEntity> diagramEntityList = new List<DiagramEntity>();

            foreach (InspWindow window in model.InspWindowList)
            {
                if (window == null)
                    continue;

                DiagramEntity entity = new DiagramEntity()
                {
                    LinkedWindow = window,
                    EntityROI = new Rectangle(
                        window.WindowArea.X, window.WindowArea.Y,
                            window.WindowArea.Width, window.WindowArea.Height),
                    EntityColor = imageViewer.GetWindowColor(window.InspWindowType),
                    IsHold = window.IsTeach
                };
                diagramEntityList.Add(entity);
            }

            imageViewer.SetDiagramEntityList(diagramEntityList);
        }

        public void SelectDiagramEntity(InspWindow window)
        {
            imageViewer.SelectDiagramEntity(window);
        }

        //#8_INSPECT_BINARY#18 imageViewer에 검사 결과 정보를 연결해주기 위한 함수
        public void ResetDisplay()
        {
            imageViewer.ResetEntity();
        }

        //FIXME 검사 결과를 그래픽으로 출력하기 위한 정보를 받는 함수
        public void AddRect(List<DrawInspectInfo> rectInfos)
        {
            imageViewer.AddRect(rectInfos);
        }

        //#10_INSPWINDOW#24 새로운 ROI를 추가하는 함수
        public void AddRoi(InspWindowType inspWindowType)
        {
            imageViewer.NewRoi(inspWindowType);
        }

        //#13_INSP_RESULT#6 검사 양불판정 갯수 설정 함수
        public void SetInspResultCount(int totalArea, int okCnt, int ngCnt)
        {
            imageViewer.SetInspResultCount(new InspectResultCount(totalArea, okCnt, ngCnt));
        }

        public void SetWorkingState(WorkingState workingState)
        {
            string state = "";
            switch (workingState)
            {
                case WorkingState.INSPECT:
                    state = "INSPECT";
                    break;

                case WorkingState.LIVE:
                    state = "LIVE";
                    break;

                case WorkingState.ALARM:
                    state = "ALARM";
                    break;
            }

            imageViewer.WorkingState = state;
            imageViewer.Invalidate();
        }

        private void Toolbar_ButtonChanged(object sender, ToolbarEventArgs e) {
            switch (e.Button)
            {
                case ToolbarButton.ShowROI:
                    if(e.IsChecked)
                        UpdateDiagramEntity();
                    else 
                        imageViewer.ResetEntity();
                    break;
                case ToolbarButton.ChannelColor:
                    _currentImageChannel = eImageChannel.Color;
                    UpdateDisplay();
                    break;
                case ToolbarButton.ChannelGray:
                    _currentImageChannel = eImageChannel.Gray;
                    UpdateDisplay();
                    break;
                case ToolbarButton.ChannelRed:
                    _currentImageChannel = eImageChannel.Red;
                    UpdateDisplay();
                    break;
                case ToolbarButton.ChannelGreen:
                    _currentImageChannel = eImageChannel.Green;
                    UpdateDisplay();
                    break;
                case ToolbarButton.ChannelBlue:
                    _currentImageChannel = eImageChannel.Blue;
                    UpdateDisplay();
                    break;
            }
        }

        public void SetImageChannel(eImageChannel channel)
        {
            mainViewToolbar.SetSelectButton(channel);
            UpdateDisplay();
        }

        private void CameraForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainViewToolbar.ButtonChanged -= Toolbar_ButtonChanged;

            imageViewer.DiagramEntityEvent -= ImageViewer_DiagramEntityEvent;

            this.FormClosed -= CameraForm_FormClosed;
        }
    public void SelectRoiByUid(string uid)
{
    // 1. 모델에서 데이터 찾기
    var model = Global.Inst.InspStage.CurModel;
    if (model == null) return;

    var window = model.InspWindowList.FirstOrDefault(w => w.UID == uid);

    if (window != null)
    {
        // 2. 중요: UI 스레드에서 실행되도록 보장 (반응이 없는 경우 대비)
        if (this.InvokeRequired)
        {
            this.Invoke(new MethodInvoker(() => SelectRoiByUid(uid)));
            return;
        }

        // 3. ImageViewCtrl의 선택 로직 호출
        // 이 메서드가 내부적으로 _multiSelectedEntities에 추가하고 Invalidate()를 호출합니다.
        imageViewer.SelectDiagramEntity(window);

        // 4. 추가 확인: 강제로 포커스를 주고 다시 그리기를 한 번 더 호출
        imageViewer.Focus();
        imageViewer.Invalidate(); 
    }
}
    }
}
