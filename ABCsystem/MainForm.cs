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
            openViewForm(new NewModel());
            ////신규 모델 추가를 위한 모델 정보를 받기 위한 창 띄우기
            //NewModel newModel = new NewModel();
            //newModel.ShowDialog();

            Model curModel = Global.Inst.InspStage.CurModel;
            if (curModel != null)
            {
                this.Text = GetMdoelTitle(curModel);
            }
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

            // Camera: 좌측 70% 폭, 전체 높이의 60% 정도
            int w = (int)(bounds.Width * 0.70);
            int h = (int)(bounds.Height * 0.80);

            int x = bounds.Left + margin;
            int y = bounds.Top + margin;

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

            // Camera: 초기 크기/위치 (폭의 70%, 높이의 85%)
            var bounds = GetPanelBoundsScreen();

            int w = (int)(bounds.Width * 0.70);
            int h = (int)(bounds.Height * 0.65);

            int x = bounds.Left + margin;
            int y = bounds.Top + margin;

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
            // 초기 크기
            int margin = 12;

            // Log: 우측 28% 폭, 아래쪽 35% 높이
            int w = (int)(bounds.Width * 0.30);
            int h = (int)(bounds.Height * 0.35);

            int x = bounds.Right - w - margin;
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
            openViewForm(new SetupForm());
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
            if (_modelTreeForm != null && !_modelTreeForm.IsDisposed)
            {
                _modelTreeForm.BringToFront();
                _modelTreeForm.Activate();
                return;
            }

            _modelTreeForm = new ModelTreeForm();

            // panel 기준으로 가두기
            _modelTreeConstraint = new WindowConstraintBehavior(
                _modelTreeForm,
                () => panelChildForm.RectangleToScreen(panelChildForm.ClientRectangle)
            );

            _modelTreeForm.Show(this);

            // 초기 크기/위치 (예: panel의 45%)
            var bounds = panelChildForm.RectangleToScreen(panelChildForm.ClientRectangle);

            int w = (int)(bounds.Width * 0.45);
            int h = (int)(bounds.Height * 0.6);

            int x = bounds.Left + 20;   // 왼쪽에 살짝 붙여도 좋고
            int y = bounds.Top + 20;

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
            ShowLogForm();
        }

    }
}
