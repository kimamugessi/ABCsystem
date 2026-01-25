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
            this.SuspendLayout();
            // 
            // btnGrab
            // 
            this.btnGrab.Font = new System.Drawing.Font("한컴산뜻돋움", 20.29091F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnGrab.ForeColor = System.Drawing.Color.Black;
            this.btnGrab.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnGrab.Location = new System.Drawing.Point(57, 22);
            this.btnGrab.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnGrab.Name = "btnGrab";
            this.btnGrab.Size = new System.Drawing.Size(70, 70);
            this.btnGrab.TabIndex = 0;
            this.btnGrab.Text = "📸";
            this.btnGrab.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnGrab.UseVisualStyleBackColor = true;
            this.btnGrab.Click += new System.EventHandler(this.btnGrab_Click);
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("한컴산뜻돋움", 20.29091F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnStart.ForeColor = System.Drawing.Color.Black;
            this.btnStart.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnStart.Location = new System.Drawing.Point(57, 106);
            this.btnStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(70, 70);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "▶️";
            this.btnStart.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnLive
            // 
            this.btnLive.Font = new System.Drawing.Font("한컴산뜻돋움", 20.29091F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLive.ForeColor = System.Drawing.Color.Black;
            this.btnLive.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLive.Location = new System.Drawing.Point(135, 22);
            this.btnLive.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnLive.Name = "btnLive";
            this.btnLive.Size = new System.Drawing.Size(70, 70);
            this.btnLive.TabIndex = 2;
            this.btnLive.Text = "🎥";
            this.btnLive.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLive.UseVisualStyleBackColor = true;
            this.btnLive.Click += new System.EventHandler(this.btnLive_Click);
            // 
            // btmStop
            // 
            this.btmStop.Font = new System.Drawing.Font("한컴산뜻돋움", 20.29091F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btmStop.ForeColor = System.Drawing.Color.Black;
            this.btmStop.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btmStop.Location = new System.Drawing.Point(135, 106);
            this.btmStop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btmStop.Name = "btmStop";
            this.btmStop.Size = new System.Drawing.Size(70, 70);
            this.btmStop.TabIndex = 1;
            this.btmStop.Text = "⏹️";
            this.btmStop.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btmStop.UseVisualStyleBackColor = true;
            this.btmStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Window;
            this.label1.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163635F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(75, 68);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "촬상";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Window;
            this.label2.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163635F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(153, 68);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "LIVE";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("한컴산뜻돋움", 9.818182F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(15, 133);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 19);
            this.label3.TabIndex = 3;
            this.label3.Text = "검사";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.Window;
            this.label4.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163635F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(75, 152);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 18);
            this.label4.TabIndex = 3;
            this.label4.Text = "시작";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.Window;
            this.label5.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163635F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(153, 152);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 18);
            this.label5.TabIndex = 3;
            this.label5.Text = "중지";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("한컴산뜻돋움", 9.818182F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(15, 49);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 19);
            this.label6.TabIndex = 3;
            this.label6.Text = "사진";
            // 
            // RunForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 203);
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
            this.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
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
    }
}