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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CameraForm));
            this.mainViewToolbar = new ABCsystem.UIControl.MainViewToolbar();
            this.imageViewer = new ABCsystem.UIControl.ImageViewCtrl();
            this.SuspendLayout();
            // 
            // mainViewToolbar
            // 
            this.mainViewToolbar.Dock = System.Windows.Forms.DockStyle.Right;
            this.mainViewToolbar.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.mainViewToolbar.Location = new System.Drawing.Point(605, 0);
            this.mainViewToolbar.Margin = new System.Windows.Forms.Padding(4);
            this.mainViewToolbar.Name = "mainViewToolbar";
            this.mainViewToolbar.Size = new System.Drawing.Size(73, 444);
            this.mainViewToolbar.TabIndex = 1;
            // 
            // imageViewer
            // 
            this.imageViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageViewer.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.imageViewer.Location = new System.Drawing.Point(0, 0);
            this.imageViewer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.imageViewer.Name = "imageViewer";
            this.imageViewer.Size = new System.Drawing.Size(678, 444);
            this.imageViewer.TabIndex = 0;
            this.imageViewer.WorkingState = "";
            // 
            // CameraForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 444);
            this.Controls.Add(this.mainViewToolbar);
            this.Controls.Add(this.imageViewer);
            this.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163635F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "CameraForm";
            this.Text = "Viewer";
            this.Resize += new System.EventHandler(this.CameraForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private UIControl.ImageViewCtrl imageViewer;
        private UIControl.MainViewToolbar mainViewToolbar;
    }
}