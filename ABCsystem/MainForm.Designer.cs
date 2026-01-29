namespace ABCsystem
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panelSideMenu = new System.Windows.Forms.Panel();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnSetting = new System.Windows.Forms.Button();
            this.panelViewSubmenu = new System.Windows.Forms.Panel();
            this.btnLog = new System.Windows.Forms.Button();
            this.btnCamera = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.panelTeachSubmenu = new System.Windows.Forms.Panel();
            this.btnROI = new System.Windows.Forms.Button();
            this.btnTeach = new System.Windows.Forms.Button();
            this.btnOperation = new System.Windows.Forms.Button();
            this.panelFileSubmenu = new System.Windows.Forms.Panel();
            this.btnImageSave = new System.Windows.Forms.Button();
            this.btnImageOpen = new System.Windows.Forms.Button();
            this.btnModelSaveAs = new System.Windows.Forms.Button();
            this.btnModelSave = new System.Windows.Forms.Button();
            this.btnModelOpen = new System.Windows.Forms.Button();
            this.btnModelNew = new System.Windows.Forms.Button();
            this.btnFile = new System.Windows.Forms.Button();
            this.panelLogo = new System.Windows.Forms.Panel();
            this.picABC = new System.Windows.Forms.PictureBox();
            this.panelOperation = new System.Windows.Forms.Panel();
            this.panelChildForm = new System.Windows.Forms.Panel();
            this.nightCtlChlid = new ReaLTaiizor.Controls.NightControlBox();
            this.panelChlid = new System.Windows.Forms.Panel();
            this.picChlid = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelSideMenu.SuspendLayout();
            this.panelViewSubmenu.SuspendLayout();
            this.panelTeachSubmenu.SuspendLayout();
            this.panelFileSubmenu.SuspendLayout();
            this.panelLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picABC)).BeginInit();
            this.panelChildForm.SuspendLayout();
            this.panelChlid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picChlid)).BeginInit();
            this.SuspendLayout();
            // 
            // panelSideMenu
            // 
            this.panelSideMenu.AutoScroll = true;
            this.panelSideMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(7)))), ((int)(((byte)(17)))));
            this.panelSideMenu.Controls.Add(this.btnHelp);
            this.panelSideMenu.Controls.Add(this.btnSetting);
            this.panelSideMenu.Controls.Add(this.panelViewSubmenu);
            this.panelSideMenu.Controls.Add(this.btnView);
            this.panelSideMenu.Controls.Add(this.panelTeachSubmenu);
            this.panelSideMenu.Controls.Add(this.btnTeach);
            this.panelSideMenu.Controls.Add(this.btnOperation);
            this.panelSideMenu.Controls.Add(this.panelFileSubmenu);
            this.panelSideMenu.Controls.Add(this.btnFile);
            this.panelSideMenu.Controls.Add(this.panelLogo);
            this.panelSideMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSideMenu.Location = new System.Drawing.Point(0, 0);
            this.panelSideMenu.Name = "panelSideMenu";
            this.panelSideMenu.Size = new System.Drawing.Size(250, 600);
            this.panelSideMenu.TabIndex = 0;
            // 
            // btnHelp
            // 
            this.btnHelp.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnHelp.FlatAppearance.BorderSize = 0;
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHelp.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHelp.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnHelp.Location = new System.Drawing.Point(0, 800);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnHelp.Size = new System.Drawing.Size(224, 50);
            this.btnHelp.TabIndex = 9;
            this.btnHelp.Text = "Help";
            this.btnHelp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHelp.UseVisualStyleBackColor = true;
            // 
            // btnSetting
            // 
            this.btnSetting.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSetting.FlatAppearance.BorderSize = 0;
            this.btnSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetting.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetting.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnSetting.Location = new System.Drawing.Point(0, 750);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnSetting.Size = new System.Drawing.Size(224, 50);
            this.btnSetting.TabIndex = 8;
            this.btnSetting.Text = "Setting";
            this.btnSetting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // panelViewSubmenu
            // 
            this.panelViewSubmenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(32)))), ((int)(((byte)(39)))));
            this.panelViewSubmenu.Controls.Add(this.btnLog);
            this.panelViewSubmenu.Controls.Add(this.btnCamera);
            this.panelViewSubmenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelViewSubmenu.Location = new System.Drawing.Point(0, 650);
            this.panelViewSubmenu.Name = "panelViewSubmenu";
            this.panelViewSubmenu.Size = new System.Drawing.Size(224, 100);
            this.panelViewSubmenu.TabIndex = 7;
            // 
            // btnLog
            // 
            this.btnLog.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLog.FlatAppearance.BorderSize = 0;
            this.btnLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLog.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLog.ForeColor = System.Drawing.Color.LightGray;
            this.btnLog.Location = new System.Drawing.Point(0, 50);
            this.btnLog.Name = "btnLog";
            this.btnLog.Padding = new System.Windows.Forms.Padding(35, 0, 0, 0);
            this.btnLog.Size = new System.Drawing.Size(224, 50);
            this.btnLog.TabIndex = 1;
            this.btnLog.Text = "Log";
            this.btnLog.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLog.UseVisualStyleBackColor = true;
            this.btnLog.Click += new System.EventHandler(this.btnLog_Click);
            // 
            // btnCamera
            // 
            this.btnCamera.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCamera.FlatAppearance.BorderSize = 0;
            this.btnCamera.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCamera.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCamera.ForeColor = System.Drawing.Color.LightGray;
            this.btnCamera.Location = new System.Drawing.Point(0, 0);
            this.btnCamera.Name = "btnCamera";
            this.btnCamera.Padding = new System.Windows.Forms.Padding(35, 0, 0, 0);
            this.btnCamera.Size = new System.Drawing.Size(224, 50);
            this.btnCamera.TabIndex = 0;
            this.btnCamera.Text = "Camera Viewer";
            this.btnCamera.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCamera.UseVisualStyleBackColor = true;
            this.btnCamera.Click += new System.EventHandler(this.btnCamera_Click);
            // 
            // btnView
            // 
            this.btnView.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnView.FlatAppearance.BorderSize = 0;
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnView.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnView.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnView.Location = new System.Drawing.Point(0, 600);
            this.btnView.Name = "btnView";
            this.btnView.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnView.Size = new System.Drawing.Size(224, 50);
            this.btnView.TabIndex = 6;
            this.btnView.Text = "View";
            this.btnView.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // panelTeachSubmenu
            // 
            this.panelTeachSubmenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(32)))), ((int)(((byte)(39)))));
            this.panelTeachSubmenu.Controls.Add(this.btnROI);
            this.panelTeachSubmenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTeachSubmenu.Location = new System.Drawing.Point(0, 550);
            this.panelTeachSubmenu.Name = "panelTeachSubmenu";
            this.panelTeachSubmenu.Size = new System.Drawing.Size(224, 50);
            this.panelTeachSubmenu.TabIndex = 5;
            // 
            // btnROI
            // 
            this.btnROI.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnROI.FlatAppearance.BorderSize = 0;
            this.btnROI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnROI.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnROI.ForeColor = System.Drawing.Color.LightGray;
            this.btnROI.Location = new System.Drawing.Point(0, 0);
            this.btnROI.Name = "btnROI";
            this.btnROI.Padding = new System.Windows.Forms.Padding(35, 0, 0, 0);
            this.btnROI.Size = new System.Drawing.Size(224, 50);
            this.btnROI.TabIndex = 0;
            this.btnROI.Text = "ROI";
            this.btnROI.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnROI.UseVisualStyleBackColor = true;
            this.btnROI.Click += new System.EventHandler(this.btnROI_Click);
            // 
            // btnTeach
            // 
            this.btnTeach.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnTeach.FlatAppearance.BorderSize = 0;
            this.btnTeach.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTeach.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTeach.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnTeach.Location = new System.Drawing.Point(0, 500);
            this.btnTeach.Name = "btnTeach";
            this.btnTeach.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnTeach.Size = new System.Drawing.Size(224, 50);
            this.btnTeach.TabIndex = 4;
            this.btnTeach.Text = "Teach";
            this.btnTeach.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTeach.UseVisualStyleBackColor = true;
            this.btnTeach.Click += new System.EventHandler(this.btnTeach_Click);
            // 
            // btnOperation
            // 
            this.btnOperation.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnOperation.FlatAppearance.BorderSize = 0;
            this.btnOperation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOperation.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOperation.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnOperation.Location = new System.Drawing.Point(0, 450);
            this.btnOperation.Name = "btnOperation";
            this.btnOperation.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnOperation.Size = new System.Drawing.Size(224, 50);
            this.btnOperation.TabIndex = 3;
            this.btnOperation.Text = "Operation";
            this.btnOperation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOperation.UseVisualStyleBackColor = true;
            this.btnOperation.Click += new System.EventHandler(this.btnOperation_Click);
            // 
            // panelFileSubmenu
            // 
            this.panelFileSubmenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(32)))), ((int)(((byte)(39)))));
            this.panelFileSubmenu.Controls.Add(this.btnImageSave);
            this.panelFileSubmenu.Controls.Add(this.btnImageOpen);
            this.panelFileSubmenu.Controls.Add(this.btnModelSaveAs);
            this.panelFileSubmenu.Controls.Add(this.btnModelSave);
            this.panelFileSubmenu.Controls.Add(this.btnModelOpen);
            this.panelFileSubmenu.Controls.Add(this.btnModelNew);
            this.panelFileSubmenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFileSubmenu.Location = new System.Drawing.Point(0, 150);
            this.panelFileSubmenu.Name = "panelFileSubmenu";
            this.panelFileSubmenu.Size = new System.Drawing.Size(224, 300);
            this.panelFileSubmenu.TabIndex = 2;
            // 
            // btnImageSave
            // 
            this.btnImageSave.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnImageSave.FlatAppearance.BorderSize = 0;
            this.btnImageSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImageSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImageSave.ForeColor = System.Drawing.Color.LightGray;
            this.btnImageSave.Location = new System.Drawing.Point(0, 250);
            this.btnImageSave.Name = "btnImageSave";
            this.btnImageSave.Padding = new System.Windows.Forms.Padding(35, 0, 0, 0);
            this.btnImageSave.Size = new System.Drawing.Size(224, 50);
            this.btnImageSave.TabIndex = 5;
            this.btnImageSave.Text = "Image Save";
            this.btnImageSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImageSave.UseVisualStyleBackColor = true;
            this.btnImageSave.Click += new System.EventHandler(this.btnImageSave_Click);
            // 
            // btnImageOpen
            // 
            this.btnImageOpen.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnImageOpen.FlatAppearance.BorderSize = 0;
            this.btnImageOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImageOpen.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImageOpen.ForeColor = System.Drawing.Color.LightGray;
            this.btnImageOpen.Location = new System.Drawing.Point(0, 200);
            this.btnImageOpen.Name = "btnImageOpen";
            this.btnImageOpen.Padding = new System.Windows.Forms.Padding(35, 0, 0, 0);
            this.btnImageOpen.Size = new System.Drawing.Size(224, 50);
            this.btnImageOpen.TabIndex = 4;
            this.btnImageOpen.Text = "Image Open";
            this.btnImageOpen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImageOpen.UseVisualStyleBackColor = true;
            this.btnImageOpen.Click += new System.EventHandler(this.btnImageOpen_Click);
            // 
            // btnModelSaveAs
            // 
            this.btnModelSaveAs.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnModelSaveAs.FlatAppearance.BorderSize = 0;
            this.btnModelSaveAs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModelSaveAs.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnModelSaveAs.ForeColor = System.Drawing.Color.LightGray;
            this.btnModelSaveAs.Location = new System.Drawing.Point(0, 150);
            this.btnModelSaveAs.Name = "btnModelSaveAs";
            this.btnModelSaveAs.Padding = new System.Windows.Forms.Padding(35, 0, 0, 0);
            this.btnModelSaveAs.Size = new System.Drawing.Size(224, 50);
            this.btnModelSaveAs.TabIndex = 3;
            this.btnModelSaveAs.Text = "Model Save As";
            this.btnModelSaveAs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnModelSaveAs.UseVisualStyleBackColor = true;
            this.btnModelSaveAs.Click += new System.EventHandler(this.btnModelSaveAs_Click);
            // 
            // btnModelSave
            // 
            this.btnModelSave.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnModelSave.FlatAppearance.BorderSize = 0;
            this.btnModelSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModelSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnModelSave.ForeColor = System.Drawing.Color.LightGray;
            this.btnModelSave.Location = new System.Drawing.Point(0, 100);
            this.btnModelSave.Name = "btnModelSave";
            this.btnModelSave.Padding = new System.Windows.Forms.Padding(35, 0, 0, 0);
            this.btnModelSave.Size = new System.Drawing.Size(224, 50);
            this.btnModelSave.TabIndex = 2;
            this.btnModelSave.Text = "Model Save";
            this.btnModelSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnModelSave.UseVisualStyleBackColor = true;
            this.btnModelSave.Click += new System.EventHandler(this.btnModelSave_Click);
            // 
            // btnModelOpen
            // 
            this.btnModelOpen.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnModelOpen.FlatAppearance.BorderSize = 0;
            this.btnModelOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModelOpen.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnModelOpen.ForeColor = System.Drawing.Color.LightGray;
            this.btnModelOpen.Location = new System.Drawing.Point(0, 50);
            this.btnModelOpen.Name = "btnModelOpen";
            this.btnModelOpen.Padding = new System.Windows.Forms.Padding(35, 0, 0, 0);
            this.btnModelOpen.Size = new System.Drawing.Size(224, 50);
            this.btnModelOpen.TabIndex = 1;
            this.btnModelOpen.Text = "Model Open";
            this.btnModelOpen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnModelOpen.UseVisualStyleBackColor = true;
            this.btnModelOpen.Click += new System.EventHandler(this.btnModelOpen_Click);
            // 
            // btnModelNew
            // 
            this.btnModelNew.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnModelNew.FlatAppearance.BorderSize = 0;
            this.btnModelNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModelNew.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnModelNew.ForeColor = System.Drawing.Color.LightGray;
            this.btnModelNew.Location = new System.Drawing.Point(0, 0);
            this.btnModelNew.Name = "btnModelNew";
            this.btnModelNew.Padding = new System.Windows.Forms.Padding(35, 0, 0, 0);
            this.btnModelNew.Size = new System.Drawing.Size(224, 50);
            this.btnModelNew.TabIndex = 0;
            this.btnModelNew.Text = "Model New";
            this.btnModelNew.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnModelNew.UseVisualStyleBackColor = true;
            this.btnModelNew.Click += new System.EventHandler(this.btnModelNew_Click);
            // 
            // btnFile
            // 
            this.btnFile.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnFile.FlatAppearance.BorderSize = 0;
            this.btnFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFile.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFile.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnFile.Location = new System.Drawing.Point(0, 100);
            this.btnFile.Name = "btnFile";
            this.btnFile.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnFile.Size = new System.Drawing.Size(224, 50);
            this.btnFile.TabIndex = 1;
            this.btnFile.Text = "File";
            this.btnFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // panelLogo
            // 
            this.panelLogo.Controls.Add(this.picABC);
            this.panelLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLogo.Location = new System.Drawing.Point(0, 0);
            this.panelLogo.Name = "panelLogo";
            this.panelLogo.Size = new System.Drawing.Size(224, 100);
            this.panelLogo.TabIndex = 0;
            // 
            // picABC
            // 
            this.picABC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picABC.Image = ((System.Drawing.Image)(resources.GetObject("picABC.Image")));
            this.picABC.Location = new System.Drawing.Point(0, 0);
            this.picABC.Name = "picABC";
            this.picABC.Size = new System.Drawing.Size(224, 100);
            this.picABC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picABC.TabIndex = 0;
            this.picABC.TabStop = false;
            // 
            // panelOperation
            // 
            this.panelOperation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(21)))), ((int)(((byte)(32)))));
            this.panelOperation.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelOperation.Location = new System.Drawing.Point(250, 500);
            this.panelOperation.Name = "panelOperation";
            this.panelOperation.Size = new System.Drawing.Size(700, 100);
            this.panelOperation.TabIndex = 1;
            // 
            // panelChildForm
            // 
            this.panelChildForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(30)))), ((int)(((byte)(45)))));
            this.panelChildForm.Controls.Add(this.panelChlid);
            this.panelChildForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChildForm.Location = new System.Drawing.Point(250, 0);
            this.panelChildForm.Name = "panelChildForm";
            this.panelChildForm.Size = new System.Drawing.Size(700, 500);
            this.panelChildForm.TabIndex = 2;
            // 
            // nightCtlChlid
            // 
            this.nightCtlChlid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nightCtlChlid.BackColor = System.Drawing.Color.Transparent;
            this.nightCtlChlid.CloseHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.nightCtlChlid.CloseHoverForeColor = System.Drawing.Color.White;
            this.nightCtlChlid.Cursor = System.Windows.Forms.Cursors.Hand;
            this.nightCtlChlid.DefaultLocation = true;
            this.nightCtlChlid.DisableMaximizeColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.nightCtlChlid.DisableMinimizeColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.nightCtlChlid.EnableCloseColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.nightCtlChlid.EnableMaximizeButton = true;
            this.nightCtlChlid.EnableMaximizeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.nightCtlChlid.EnableMinimizeButton = true;
            this.nightCtlChlid.EnableMinimizeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.nightCtlChlid.Location = new System.Drawing.Point(561, 0);
            this.nightCtlChlid.MaximizeHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.nightCtlChlid.MaximizeHoverForeColor = System.Drawing.Color.White;
            this.nightCtlChlid.MinimizeHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.nightCtlChlid.MinimizeHoverForeColor = System.Drawing.Color.White;
            this.nightCtlChlid.Name = "nightCtlChlid";
            this.nightCtlChlid.Size = new System.Drawing.Size(139, 31);
            this.nightCtlChlid.TabIndex = 1;
            // 
            // panelChlid
            // 
            this.panelChlid.Controls.Add(this.label1);
            this.panelChlid.Controls.Add(this.picChlid);
            this.panelChlid.Controls.Add(this.nightCtlChlid);
            this.panelChlid.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelChlid.Location = new System.Drawing.Point(0, 0);
            this.panelChlid.Name = "panelChlid";
            this.panelChlid.Size = new System.Drawing.Size(700, 35);
            this.panelChlid.TabIndex = 0;
            // 
            // picChlid
            // 
            this.picChlid.Image = ((System.Drawing.Image)(resources.GetObject("picChlid.Image")));
            this.picChlid.Location = new System.Drawing.Point(6, 3);
            this.picChlid.Name = "picChlid";
            this.picChlid.Size = new System.Drawing.Size(31, 32);
            this.picChlid.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picChlid.TabIndex = 2;
            this.picChlid.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(43, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 28);
            this.label1.TabIndex = 1;
            this.label1.Text = "ABCsystem";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 600);
            this.Controls.Add(this.panelChildForm);
            this.Controls.Add(this.panelOperation);
            this.Controls.Add(this.panelSideMenu);
            this.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(950, 600);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.panelSideMenu.ResumeLayout(false);
            this.panelViewSubmenu.ResumeLayout(false);
            this.panelTeachSubmenu.ResumeLayout(false);
            this.panelFileSubmenu.ResumeLayout(false);
            this.panelLogo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picABC)).EndInit();
            this.panelChildForm.ResumeLayout(false);
            this.panelChlid.ResumeLayout(false);
            this.panelChlid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picChlid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSideMenu;
        private System.Windows.Forms.Panel panelFileSubmenu;
        private System.Windows.Forms.Button btnModelSave;
        private System.Windows.Forms.Button btnModelOpen;
        private System.Windows.Forms.Button btnModelNew;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.Button btnImageOpen;
        private System.Windows.Forms.Button btnModelSaveAs;
        private System.Windows.Forms.Button btnOperation;
        private System.Windows.Forms.Panel panelTeachSubmenu;
        private System.Windows.Forms.Button btnTeach;
        private System.Windows.Forms.Panel panelViewSubmenu;
        private System.Windows.Forms.Button btnCamera;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnROI;
        private System.Windows.Forms.Button btnSetting;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Button btnLog;
        private System.Windows.Forms.Panel panelOperation;
        private System.Windows.Forms.Panel panelChildForm;
        private System.Windows.Forms.Button btnImageSave;
        private System.Windows.Forms.Panel panelLogo;
        private System.Windows.Forms.PictureBox picABC;
        private System.Windows.Forms.Panel panelChlid;
        private ReaLTaiizor.Controls.NightControlBox nightCtlChlid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picChlid;
    }
}