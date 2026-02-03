using ABCsystem.Core;
using ABCsystem.Setting;
using ABCsystem.Teach;
using ABCsystem.Util;
using ABCsystem.Window;
using ABCsystem4.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ReaLTaiizor.Util.RoundInt;

namespace ABCsystem
{
    public partial class MainForm : Form
    {
        private CameraForm _cameraForm;
        private WindowConstraintBehavior _cameraConstraint;

        private LogForm _logForm;
        private WindowConstraintBehavior _logConstraint;

        private ModelTreeForm _modelTreeForm;
        private WindowConstraintBehavior _modelTreeConstraint;

        private SetupForm _setupForm;
        private WindowConstraintBehavior _setupConstraint;

        private CpkForm _cpkForm;
        private WindowConstraintBehavior _cpkConstraint;

        private bool _startupFormsShown = false;
        private Rectangle GetPanelBoundsScreen()
        {
            return panelChildForm.RectangleToScreen(panelChildForm.ClientRectangle);
        }
        public MainForm()
        {
            InitializeComponent();
            customizeDesing();

            Global.Inst.Initialize();

            LoadSetting();
            this.WindowState = FormWindowState.Maximized;
        }
        
        private void customizeDesing()
        {
            panelFileSubmenu.Visible = false;
            panelTeachSubmenu.Visible = false;
            panelViewSubmenu.Visible = false;
        }

        private void LoadSetting()
        {
            // cycleModeMenuItem.Checked = SettingXml.Inst.CycleMode;
        }

        private void imageOpenToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            CameraForm cameraForm = FormManager.GetForm<CameraForm>();
            if (cameraForm == null) return;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "이미지 파일 선택";
                openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif";
                openFileDialog.Multiselect = false;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    Global.Inst.InspStage.SetImageBuffer(filePath);
                    Global.Inst.InspStage.CurModel.InspectImagePath = filePath;

                }
            }
        }
        private void SetupMenuItem_Click(object sender, EventArgs e)
        {
            SLogger.Write($"환경설정창 열기");
            SetupForm setupForm = new SetupForm();
            setupForm.ShowDialog();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Global.Inst.Dispose();
        }
        private string GetMdoelTitle(Model curModel)
        {
            if (curModel == null)
                return "";

            string modelName = curModel.ModelName;
            return $"{Define.PROGRAM_NAME} - MODEL : {modelName}";
        }

        private void modelNewMenuItem_Click(object sender, EventArgs e)
        {
            //신규 모델 추가를 위한 모델 정보를 받기 위한 창 띄우기
            NewModel newModel = new NewModel();
            newModel.ShowDialog();

            Model curModel = Global.Inst.InspStage.CurModel;
            if (curModel != null)
            {
                this.Text = GetMdoelTitle(curModel);
            }
        }

        private void modelOpenMenuItem_Click(object sender, EventArgs e)
        {
            //모델 파일 열기
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "모델 파일 선택";
                openFileDialog.Filter = "Model Files|*.xml;";
                openFileDialog.Multiselect = false;
                openFileDialog.InitialDirectory = SettingXml.Inst.ModelDir;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    if (Global.Inst.InspStage.LoadModel(filePath))
                    {
                        Model curModel = Global.Inst.InspStage.CurModel;
                        if (curModel != null)
                        {
                            this.Text = GetMdoelTitle(curModel);
                        }
                    }
                }
            }
        }

        private void modelSaveMenuItem_Click(object sender, EventArgs e)
        {
            //모델 파일 저장
            Global.Inst.InspStage.SaveModel("");
        }

        private void modelSaveAsMenuItem_Click(object sender, EventArgs e)
        {
            //다른이름으로 모델 파일 저장
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = SettingXml.Inst.ModelDir;
                saveFileDialog.Title = "모델 파일 선택";
                saveFileDialog.Filter = "Model Files|*.xml;";
                saveFileDialog.DefaultExt = "xml";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    Global.Inst.InspStage.SaveModel(filePath);
                }
            }
        }

        private void hideSubMenu()
        {
            if (panelFileSubmenu.Visible == true)
                panelFileSubmenu.Visible = false;
            if (panelTeachSubmenu.Visible == true)
                panelTeachSubmenu.Visible = false;
            if (panelViewSubmenu.Visible == true)
                panelViewSubmenu.Visible = false;
        }

        private void showSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                hideSubMenu();
                subMenu.Visible = true;
            }
            else
                subMenu.Visible = false;
        }
        #region Panel Button

        private void btnFile_Click(object sender, EventArgs e)
        {
            showSubMenu(panelFileSubmenu);
        }

        private void btnModelNew_Click(object sender, EventArgs e)
        {
            //openViewForm(new NewModel());

            NewModel newModel = new NewModel();

            newModel.StartPosition = FormStartPosition.Manual;
            newModel.FormBorderStyle = FormBorderStyle.FixedSingle;
            newModel.MaximizeBox = false;
            newModel.MinimizeBox = false;

            newModel.Show(this);
            newModel.PerformLayout();

            var bounds = GetPanelBoundsScreen();
            int margin = 12;
            int gap = 10;

            // 기준: CameraForm 우측 상단
            int x, y, w, h;

            if (_cameraForm != null && !_cameraForm.IsDisposed && _cameraForm.Visible)
            {
                x = _cameraForm.Bounds.Right + margin;
                y = _cameraForm.Bounds.Top; 
            }
            else
            {
                x = bounds.Right - newModel.Width - margin;
                y = bounds.Top + margin;
            }

            // 우측 컬럼 폭: panel 오른쪽 끝까지 채우기
            w = bounds.Right - x - margin;
            if (w < newModel.Width) w = newModel.Width;
            h = 215;

            // 화면 밖 보정
            if (x + w > bounds.Right - margin) w = bounds.Right - margin - x;
            if (y + h > bounds.Bottom - margin) y = bounds.Bottom - margin - h;

            newModel.Bounds = new Rectangle(x, y, w, h);
            newModel.BringToFront();

            // SetupForm / 티칭창 재배치(있으면)
            int rightX = x;
            int rightW = w;

            int bottomY = (_cameraForm != null && !_cameraForm.IsDisposed && _cameraForm.Visible)
                ? _cameraForm.Bounds.Bottom
                : bounds.Bottom - margin;

            // 티칭창은 CameraForm 우측 하단에 붙일 거라서, 먼저 teach 높이 확보
            int teachH = 180;
            int teachY = bottomY - teachH;

            if (_modelTreeForm != null && !_modelTreeForm.IsDisposed && _modelTreeForm.Visible)
            {
                _modelTreeForm.Bounds = new Rectangle(rightX, teachY, rightW, teachH);
            }

            // SetupForm은 NewModel 아래 ~ Teach 위 사이에 넣기
            if (_setupForm != null && !_setupForm.IsDisposed && _setupForm.Visible)
            {
                int setupH = 220;
                int setupY = newModel.Bottom + gap;

                // 사이 공간 계산
                int maxH = teachY - gap - setupY;
                if (maxH < 120) maxH = 120;                // 너무 작으면 최소 확보
                if (setupH > maxH) setupH = maxH;

                _setupForm.Bounds = new Rectangle(rightX, setupY, rightW, setupH);
            }

            Model curModel = Global.Inst.InspStage.CurModel;
            if (curModel != null) this.Text = GetMdoelTitle(curModel);

            hideSubMenu();
        }

        private void btnModelOpen_Click(object sender, EventArgs e)
        {
            //모델 파일 열기
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "모델 파일 선택";
                openFileDialog.Filter = "Model Files|*.xml;";
                openFileDialog.Multiselect = false;
                openFileDialog.RestoreDirectory = true;

                string modelDir = SettingXml.Inst.ModelDir;

                if (string.IsNullOrEmpty(modelDir) || !Directory.Exists(modelDir))
                    modelDir = Application.StartupPath;

                openFileDialog.InitialDirectory = modelDir;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    if (Global.Inst.InspStage.LoadModel(filePath))
                    {
                        Model curModel = Global.Inst.InspStage.CurModel;
                        if (curModel != null)
                        {
                            this.Text = GetMdoelTitle(curModel);
                        }
                    }
                }
            }
            hideSubMenu();
        }

        private void btnModelSave_Click(object sender, EventArgs e)
        {
            //모델 파일 저장
            Global.Inst.InspStage.SaveModel("");
            hideSubMenu();
        }

        private void btnModelSaveAs_Click(object sender, EventArgs e)
        {
            //다른이름으로 모델 파일 저장
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = SettingXml.Inst.ModelDir;
                saveFileDialog.Title = "모델 파일 선택";
                saveFileDialog.Filter = "Model Files|*.xml;";
                saveFileDialog.DefaultExt = "xml";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    Global.Inst.InspStage.SaveModel(filePath);
                }
            }
            hideSubMenu();
        }

        private void btnImageOpen_Click(object sender, EventArgs e)
        {
            CameraForm cameraForm = FormManager.GetForm<CameraForm>();
            if (cameraForm == null) return;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "이미지 파일 선택";
                openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif";
                openFileDialog.Multiselect = false;
                openFileDialog.RestoreDirectory = true;

                // 초기 이미지 경로 결정
                string imageDir = SettingXml.Inst.ImageDir;

                if (string.IsNullOrEmpty(imageDir) || !Directory.Exists(imageDir))
                    imageDir = Application.StartupPath;

                openFileDialog.InitialDirectory = imageDir;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    Global.Inst.InspStage.SetImageBuffer(filePath);
                    Global.Inst.InspStage.CurModel.InspectImagePath = filePath;

                    // 마지막 이미지 폴더 저장
                    SettingXml.Inst.ImageDir = Path.GetDirectoryName(filePath);
                    SettingXml.Save();

                    cameraForm.UpdateDisplay();
                }
            }
            hideSubMenu();
        }

        private void btnImageSave_Click(object sender, EventArgs e)
        {
            hideSubMenu();
        }

        private void btnOperation_Click(object sender, EventArgs e)
        {
            OpenOperationForm(new RunForm());
            hideSubMenu();
        }

        private void btnTeach_Click(object sender, EventArgs e)
        {
            showSubMenu(panelTeachSubmenu);
        }

        private void btnROI_Click(object sender, EventArgs e)
        {
            ShowModelTreeForm();
            hideSubMenu();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            showSubMenu(panelViewSubmenu);
        }

        private void btnCamera_Click(object sender, EventArgs e)
        {

            // 1) FormManager에서 단일 인스턴스 가져오기
            _cameraForm = FormManager.GetForm<CameraForm>();

            // 2) 이미 떠있으면 앞으로
            if (_cameraForm.Visible)
            {
                _cameraForm.BringToFront();
                _cameraForm.Activate();
                hideSubMenu();
                return;
            }

            // 3) 아직 안 떠있으면 Show
            _cameraForm.Show(this);

            // 4) 제약 붙이기 (핸들 생성 후 attach)
            _cameraConstraint = new WindowConstraintBehavior(_cameraForm, GetPanelBoundsScreen);

            // 5) 초기 크기/위치
            var bounds = GetPanelBoundsScreen();

            // 좌우 여백
            int margin = 12;
            int topOffset = panelChlid.Height + 8;

            // Camera: 좌측 70% 폭, 전체 높이의 60% 정도
            int w = (int)(bounds.Width * 0.70);
            int h = (int)(bounds.Height * 0.80);

            int x = bounds.Left + margin;
            int y = bounds.Top + margin + topOffset;

            _cameraForm.Bounds = new Rectangle(x, y, w, h);

            // 6) 닫히면 참조 정리 (중복 핸들러 방지 위해 한 번만 걸리는지 주의)
            _cameraForm.FormClosed -= CameraForm_FormClosed;
            _cameraForm.FormClosed += CameraForm_FormClosed;
            ShowCameraForm();
            hideSubMenu();
        }
        // CameraForm 시작할때 띄워질 수 있도록 코드
        private void ShowCameraForm()
        {
            _cameraForm = FormManager.GetForm<CameraForm>();

            if (_cameraForm.Visible)
            {
                _cameraForm.BringToFront();
                _cameraForm.Activate();
                return;
            }

            _cameraForm.Show(this);

            _cameraConstraint = new WindowConstraintBehavior(_cameraForm, GetPanelBoundsScreen);

            // 좌우 여백
            int margin = 12;
            int topOffset = panelChlid.Height + 8;

            // Camera: 초기 크기/위치 (폭의 70%, 높이의 85%)
            var bounds = GetPanelBoundsScreen();

            int w = (int)(bounds.Width * 0.70);
            int h = (int)(bounds.Height * 0.65);

            int x = bounds.Left + margin;
            int y = bounds.Top + margin + topOffset;

            _cameraForm.Bounds = new Rectangle(x, y, w, h);

            _cameraForm.FormClosed -= CameraForm_FormClosed;
            _cameraForm.FormClosed += CameraForm_FormClosed;
        }
        // FormClosed 26.01.28
        private void CameraForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _cameraConstraint = null;
            _cameraForm = null;
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            // 무조건 단일 인스턴스
            _logForm = FormManager.GetForm<LogForm>();

            if (_logForm.Visible)
            {
                _logForm.BringToFront();
                _logForm.Activate();
                return;
            }

            _logForm.Show(this);

            _logConstraint = new WindowConstraintBehavior(_logForm, GetPanelBoundsScreen);

            var bounds = GetPanelBoundsScreen();
            int margin = 12;

            int w = _cameraForm.Width;
            int h = (int)(bounds.Height * 0.28);

            int x = bounds.Left + margin;
            int y = bounds.Bottom - h - margin;

            _logForm.Bounds = new Rectangle(x, y, w, h);

            _logForm.FormClosed -= LogForm_FormClosed;
            _logForm.FormClosed += LogForm_FormClosed;

            hideSubMenu();
        }
        // LogForm 시작할 때 띄워질 수 있도록 코드
        private void ShowLogForm()
        {
            _logForm = FormManager.GetForm<LogForm>();

            if (_logForm.Visible)
            {
                _logForm.BringToFront();
                _logForm.Activate();
                return;
            }

            _logForm.Show(this);

            _logConstraint = new WindowConstraintBehavior(_logForm, GetPanelBoundsScreen);

            // 초기 크기/위치
            var bounds = GetPanelBoundsScreen();

            int margin = 12;

            // Log: 우측 28% 폭, 아래쪽 35% 높이
            int w = (int)(bounds.Width * 0.28);
            int h = (int)(bounds.Height * 0.35);

            int x = bounds.Right - w - margin;
            int y = bounds.Bottom - h - margin;

            _logForm.Bounds = new Rectangle(x, y, w, h);
            _logForm.FormClosed -= LogForm_FormClosed;
            _logForm.FormClosed += LogForm_FormClosed;
        }

        // FormClosed 구성 26.01.28
        private void LogForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _logConstraint = null;
            _logForm = null;
        }
        private void btnSetting_Click(object sender, EventArgs e)
        {
            SLogger.Write($"환경설정창 열기");

            // 이미 떠있으면 앞으로
            if (_setupForm != null && !_setupForm.IsDisposed && _setupForm.Visible)
            {
                _setupForm.BringToFront();
                _setupForm.Activate();
                hideSubMenu();
                return;
            }

            _setupForm = new SetupForm();

            // 팝업 형태로
            _setupForm.StartPosition = FormStartPosition.Manual;
            _setupForm.FormBorderStyle = FormBorderStyle.Sizable; 
            _setupForm.Show(this);

            // 여기부터 추가: 세팅창을 티칭창 위로 배치
            var bounds = GetPanelBoundsScreen();
            int margin = 12;

            // 우측 컬럼 폭: 티칭창 기준
            int colW = (_modelTreeForm != null && !_modelTreeForm.IsDisposed && _modelTreeForm.Visible)
                ? _modelTreeForm.Width
                : (int)(bounds.Width * 0.25);

            // 우측 컬럼 x: CameraForm 오른쪽 기준
            int colX;
            if (_cameraForm != null && !_cameraForm.IsDisposed && _cameraForm.Visible)
                colX = _cameraForm.Bounds.Right + margin;
            else
                colX = bounds.Right - colW - margin;

            // SetupForm 크기
            int w = colW;
            int h = (int)(bounds.Height * 0.21);

            int yOffset = 0;

            int x = colX;
            int y;

            // 티칭창이 있으면: "티칭창 위"에 고정 (중간)
            if (_modelTreeForm != null && !_modelTreeForm.IsDisposed && _modelTreeForm.Visible)
            {
                y = _modelTreeForm.Top - h - margin + yOffset;
            }
            else
            {
                // 티칭창 없으면: 그냥 컬럼 아래쪽(대충 중간쯤)으로
                y = bounds.Top + margin + 220;
            }

            // 화면 밖 보정
            if (x + w > bounds.Right - margin) x = bounds.Right - w - margin;
            if (x < bounds.Left + margin) x = bounds.Left + margin;
            if (y < bounds.Top + margin) y = bounds.Top + margin;
            if (y + h > bounds.Bottom - margin) y = bounds.Bottom - h - margin;

            _setupForm.Bounds = new Rectangle(x, y, w, h);

            //openViewForm(new SetupForm());
            hideSubMenu();
        }
        #endregion

        private Form activeForm = null;
        private void openViewForm(Form ViewForm)
        {
            if(activeForm != null)
                activeForm.Close();
            activeForm = ViewForm;
            ViewForm.TopLevel = false;
            ViewForm.FormBorderStyle = FormBorderStyle.None;
            ViewForm.Dock = DockStyle.Fill;
            panelChildForm.Controls.Add(ViewForm);
            panelChildForm.Tag =ViewForm;
            ViewForm.BringToFront();
            ViewForm.Show();
        }

        private Form activeOperationForm = null;
        private void OpenOperationForm(Form ViewForm)
        {
            if (activeOperationForm != null && !activeOperationForm.IsDisposed)
                activeOperationForm.Close();

            activeOperationForm = ViewForm;

            ViewForm.TopLevel = false;
            ViewForm.FormBorderStyle = FormBorderStyle.None;
            ViewForm.Dock = DockStyle.Fill;

            panelOperation.Controls.Clear();          // ⭐ 기존 화면 제거
            panelOperation.Controls.Add(ViewForm);
            panelOperation.Tag = ViewForm;

            ViewForm.BringToFront();
            ViewForm.Show();
        }

        private void ShowModelTreeForm()
        {
            // 1. 직접 생성하지 않고 FormManager에서 인스턴스를 가져옵니다.
            _modelTreeForm = FormManager.GetForm<ModelTreeForm>();

            // 2. 이미 화면에 떠 있다면 앞으로 가져오고 종료
            if (_modelTreeForm.Visible)
            {
                _modelTreeForm.BringToFront();
                _modelTreeForm.Activate();
                return;
            }

            // --- 이하 기존 위치/크기 설정 로직 동일 유지 ---

            // panel 기준으로 가두기
            _modelTreeConstraint = new WindowConstraintBehavior(
                _modelTreeForm,
                () => panelChildForm.RectangleToScreen(panelChildForm.ClientRectangle)
            );

            // 폼 띄우기 (Owner 설정)
            _modelTreeForm.Show(this);

            // 초기 크기/위치 (panel의 25%)
            var bounds = panelChildForm.RectangleToScreen(panelChildForm.ClientRectangle);

            int margin = 12;

            // 티칭창 크기: 패널의 25~30% 정도 (취향)
            int w = (int)(bounds.Width * 0.275);
            int h = (int)(bounds.Height * 0.19);

            int x;
            int y;

            // CameraForm이 떠있으면: CameraForm 오른쪽 아래로
            if (_cameraForm != null && !_cameraForm.IsDisposed && _cameraForm.Visible)
            {
                x = _cameraForm.Bounds.Right + margin;
                y = _cameraForm.Bounds.Bottom - h;

                // 화면 밖으로 나가면 보정
                if (x + w > bounds.Right - margin) x = bounds.Right - w - margin;
                if (y + h > bounds.Bottom - margin) y = bounds.Bottom - h - margin;
                if (y < bounds.Top + margin) y = bounds.Top + margin;
            }
            else
            {
                // CameraForm이 없으면: 우측 아래
                x = bounds.Right - w - margin;
                y = bounds.Bottom - h - margin;
            }

            _modelTreeForm.Bounds = new Rectangle(x, y, w, h);

            _modelTreeForm.FormClosed += (s, e) =>
            {
                _modelTreeConstraint = null;
                _modelTreeForm = null;
            };
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (_startupFormsShown) return;
            _startupFormsShown = true;

            ShowCameraForm();
            ShowModelTreeForm();
            OpenOperationForm(new RunForm());
        }

        private void btnCpk_Click(object sender, EventArgs e)
        {
            _cpkForm = FormManager.GetForm<CpkForm>();

            if (_cpkForm.Visible)
            {
                _cpkForm.BringToFront();
                _cpkForm.Activate();
                return;
            }

            _cpkForm.Show(this);

            _cpkConstraint = new WindowConstraintBehavior(_cpkForm, GetPanelBoundsScreen);

            var bounds = GetPanelBoundsScreen();
            var logBounds = _logForm.Bounds;
            int margin = 20;

            int w = _modelTreeForm.Width;
            int h = (int)(bounds.Height * 0.28);

            int x = bounds.Right - w - margin;
            int y = logBounds.Top;

            _cpkForm.Bounds = new Rectangle(x, y, w, h);

            _cpkForm.FormClosed -= CpkForm_FormClosed;
            _cpkForm.FormClosed += CpkForm_FormClosed;

            hideSubMenu();
        }

        private void CpkForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _cpkConstraint = null;
            _cpkForm = null;
        }
    }
}
