namespace ABCsystem
{
    partial class CameraForm
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
            this.mainViewToolbar = new ABCsystem.UIControl.MainViewToolbar();
            this.imageViewer = new ABCsystem.UIControl.ImageViewCtrl();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modelNewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modelOpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modelSaveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modelSaveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.imageOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageSaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setupToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.inspectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cycleModeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainViewToolbar
            // 
            this.mainViewToolbar.Dock = System.Windows.Forms.DockStyle.Right;
            this.mainViewToolbar.Location = new System.Drawing.Point(727, 0);
            this.mainViewToolbar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.mainViewToolbar.Name = "mainViewToolbar";
            this.mainViewToolbar.Size = new System.Drawing.Size(73, 450);
            this.mainViewToolbar.TabIndex = 1;
            // 
            // imageViewer
            // 
            this.imageViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageViewer.Location = new System.Drawing.Point(0, 0);
            this.imageViewer.Margin = new System.Windows.Forms.Padding(4);
            this.imageViewer.Name = "imageViewer";
            this.imageViewer.Size = new System.Drawing.Size(800, 450);
            this.imageViewer.TabIndex = 0;
            this.imageViewer.WorkingState = "";
            this.imageViewer.Load += new System.EventHandler(this.imageViewer_Load);
            // 
            // MainMenu
            // 
            this.MainMenu.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.MainMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.setupToolStripMenuItem,
            this.inspectToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(727, 33);
            this.MainMenu.TabIndex = 2;
            this.MainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modelNewMenuItem,
            this.modelOpenMenuItem,
            this.modelSaveMenuItem,
            this.modelSaveAsMenuItem,
            this.toolStripSeparator1,
            this.imageOpenToolStripMenuItem,
            this.imageSaveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(55, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // modelNewMenuItem
            // 
            this.modelNewMenuItem.Name = "modelNewMenuItem";
            this.modelNewMenuItem.Size = new System.Drawing.Size(237, 34);
            this.modelNewMenuItem.Text = "Model New";
            // 
            // modelOpenMenuItem
            // 
            this.modelOpenMenuItem.Name = "modelOpenMenuItem";
            this.modelOpenMenuItem.Size = new System.Drawing.Size(237, 34);
            this.modelOpenMenuItem.Text = "Model Open";
            // 
            // modelSaveMenuItem
            // 
            this.modelSaveMenuItem.Name = "modelSaveMenuItem";
            this.modelSaveMenuItem.Size = new System.Drawing.Size(237, 34);
            this.modelSaveMenuItem.Text = "Model Save";
            // 
            // modelSaveAsMenuItem
            // 
            this.modelSaveAsMenuItem.Name = "modelSaveAsMenuItem";
            this.modelSaveAsMenuItem.Size = new System.Drawing.Size(237, 34);
            this.modelSaveAsMenuItem.Text = "Model Save As";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(234, 6);
            // 
            // imageOpenToolStripMenuItem
            // 
            this.imageOpenToolStripMenuItem.Name = "imageOpenToolStripMenuItem";
            this.imageOpenToolStripMenuItem.Size = new System.Drawing.Size(237, 34);
            this.imageOpenToolStripMenuItem.Text = "image Open";
            // 
            // imageSaveToolStripMenuItem
            // 
            this.imageSaveToolStripMenuItem.Name = "imageSaveToolStripMenuItem";
            this.imageSaveToolStripMenuItem.Size = new System.Drawing.Size(237, 34);
            this.imageSaveToolStripMenuItem.Text = "Image Save";
            // 
            // setupToolStripMenuItem
            // 
            this.setupToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setupToolStripMenuItem1});
            this.setupToolStripMenuItem.Name = "setupToolStripMenuItem";
            this.setupToolStripMenuItem.Size = new System.Drawing.Size(75, 29);
            this.setupToolStripMenuItem.Text = "Setup";
            // 
            // setupToolStripMenuItem1
            // 
            this.setupToolStripMenuItem1.Name = "setupToolStripMenuItem1";
            this.setupToolStripMenuItem1.Size = new System.Drawing.Size(161, 34);
            this.setupToolStripMenuItem1.Text = "Setup";
            // 
            // inspectToolStripMenuItem
            // 
            this.inspectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cycleModeMenuItem});
            this.inspectToolStripMenuItem.Name = "inspectToolStripMenuItem";
            this.inspectToolStripMenuItem.Size = new System.Drawing.Size(87, 29);
            this.inspectToolStripMenuItem.Text = "Inspect";
            // 
            // cycleModeMenuItem
            // 
            this.cycleModeMenuItem.CheckOnClick = true;
            this.cycleModeMenuItem.Name = "cycleModeMenuItem";
            this.cycleModeMenuItem.Size = new System.Drawing.Size(212, 34);
            this.cycleModeMenuItem.Text = "Cycle Mode";
            // 
            // CameraForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.MainMenu);
            this.Controls.Add(this.mainViewToolbar);
            this.Controls.Add(this.imageViewer);
            this.Name = "CameraForm";
            this.Text = "CameraForm";
            this.Resize += new System.EventHandler(this.CameraForm_Resize);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UIControl.ImageViewCtrl imageViewer;
        private UIControl.MainViewToolbar mainViewToolbar;
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modelNewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modelOpenMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modelSaveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modelSaveAsMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem imageOpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageSaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setupToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem inspectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cycleModeMenuItem;
    }
}