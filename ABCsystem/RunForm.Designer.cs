namespace ABCsystem
{
    partial class RunForm
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
            this.btnGrab = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnLive = new System.Windows.Forms.Button();
            this.btmStop = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.chkCycleMode = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbEdgeType = new System.Windows.Forms.ComboBox();
            this.btnEdge = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGrab
            // 
            this.btnGrab.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnGrab.Font = new System.Drawing.Font("Segoe UI", 18.32727F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGrab.ForeColor = System.Drawing.Color.White;
            this.btnGrab.Location = new System.Drawing.Point(58, 42);
            this.btnGrab.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnGrab.Name = "btnGrab";
            this.btnGrab.Size = new System.Drawing.Size(62, 64);
            this.btnGrab.TabIndex = 0;
            this.btnGrab.Text = "📸";
            this.btnGrab.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnGrab.UseVisualStyleBackColor = false;
            this.btnGrab.Click += new System.EventHandler(this.btnGrab_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.Green;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 18.32727F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(290, 42);
            this.btnStart.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(62, 64);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "▶️";
            this.btnStart.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnLive
            // 
            this.btnLive.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLive.Font = new System.Drawing.Font("Segoe UI", 18.32727F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLive.ForeColor = System.Drawing.Color.White;
            this.btnLive.Location = new System.Drawing.Point(132, 42);
            this.btnLive.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnLive.Name = "btnLive";
            this.btnLive.Size = new System.Drawing.Size(62, 64);
            this.btnLive.TabIndex = 2;
            this.btnLive.Text = "🎥";
            this.btnLive.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLive.UseVisualStyleBackColor = false;
            this.btnLive.Click += new System.EventHandler(this.btnLive_Click);
            // 
            // btmStop
            // 
            this.btmStop.BackColor = System.Drawing.Color.Red;
            this.btmStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btmStop.Font = new System.Drawing.Font("Segoe UI", 18.32727F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btmStop.ForeColor = System.Drawing.Color.White;
            this.btmStop.Location = new System.Drawing.Point(365, 42);
            this.btmStop.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btmStop.Name = "btmStop";
            this.btmStop.Size = new System.Drawing.Size(62, 64);
            this.btmStop.TabIndex = 1;
            this.btmStop.Text = "⏹️";
            this.btmStop.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btmStop.UseVisualStyleBackColor = false;
            this.btmStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(66, 111);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "촬상";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(141, 111);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "LIVE";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(285, 6);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 25);
            this.label3.TabIndex = 3;
            this.label3.Text = "검사";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(299, 112);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 25);
            this.label4.TabIndex = 3;
            this.label4.Text = "시작";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(374, 112);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 25);
            this.label5.TabIndex = 3;
            this.label5.Text = "중지";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(52, 8);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 25);
            this.label6.TabIndex = 3;
            this.label6.Text = "사진";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Gray;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(794, 8);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(29, 27);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "❌";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chkCycleMode
            // 
            this.chkCycleMode.AutoSize = true;
            this.chkCycleMode.Font = new System.Drawing.Font("Segoe UI", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCycleMode.ForeColor = System.Drawing.Color.White;
            this.chkCycleMode.Location = new System.Drawing.Point(348, 5);
            this.chkCycleMode.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkCycleMode.Name = "chkCycleMode";
            this.chkCycleMode.Size = new System.Drawing.Size(137, 29);
            this.chkCycleMode.TabIndex = 5;
            this.chkCycleMode.Text = "Cycle Mode";
            this.chkCycleMode.UseVisualStyleBackColor = true;
            this.chkCycleMode.CheckedChanged += new System.EventHandler(this.chkCycleMode_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(534, 8);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 25);
            this.label7.TabIndex = 6;
            this.label7.Text = "Edge";
            // 
            // cbEdgeType
            // 
            this.cbEdgeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEdgeType.Font = new System.Drawing.Font("Segoe UI", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbEdgeType.FormattingEnabled = true;
            this.cbEdgeType.Items.AddRange(new object[] {
            "Align",
            "→",
            "←",
            "↑",
            "↓"});
            this.cbEdgeType.Location = new System.Drawing.Point(539, 57);
            this.cbEdgeType.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbEdgeType.Name = "cbEdgeType";
            this.cbEdgeType.Size = new System.Drawing.Size(102, 33);
            this.cbEdgeType.TabIndex = 7;
            // 
            // btnEdge
            // 
            this.btnEdge.BackColor = System.Drawing.Color.White;
            this.btnEdge.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEdge.Font = new System.Drawing.Font("Segoe UI", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdge.Location = new System.Drawing.Point(650, 51);
            this.btnEdge.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnEdge.Name = "btnEdge";
            this.btnEdge.Size = new System.Drawing.Size(104, 44);
            this.btnEdge.TabIndex = 8;
            this.btnEdge.Text = "Inspect";
            this.btnEdge.UseVisualStyleBackColor = false;
            this.btnEdge.Click += new System.EventHandler(this.btnEdge_Click);
            // 
            // RunForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(52)))), ((int)(((byte)(59)))));
            this.ClientSize = new System.Drawing.Size(1039, 148);
            this.Controls.Add(this.btnEdge);
            this.Controls.Add(this.cbEdgeType);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.chkCycleMode);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btmStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnLive);
            this.Controls.Add(this.btnGrab);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "RunForm";
            this.Text = "RunForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGrab;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnLive;
        private System.Windows.Forms.Button btmStop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox chkCycleMode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbEdgeType;
        private System.Windows.Forms.Button btnEdge;
    }
}