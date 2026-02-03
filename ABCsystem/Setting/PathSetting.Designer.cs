namespace ABCsystem4.Setting
{
    partial class PathSetting
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSelImageDir = new System.Windows.Forms.Button();
            this.txtImageDir = new System.Windows.Forms.TextBox();
            this.lbImageDir = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnSelModelDir = new System.Windows.Forms.Button();
            this.txtModelDir = new System.Windows.Forms.TextBox();
            this.lbModelDir = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSelImageDir
            // 
            this.btnSelImageDir.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.btnSelImageDir.Location = new System.Drawing.Point(299, 56);
            this.btnSelImageDir.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnSelImageDir.Name = "btnSelImageDir";
            this.btnSelImageDir.Size = new System.Drawing.Size(45, 31);
            this.btnSelImageDir.TabIndex = 13;
            this.btnSelImageDir.Text = "...";
            this.btnSelImageDir.UseVisualStyleBackColor = true;
            this.btnSelImageDir.Click += new System.EventHandler(this.btnSelImageDir_Click);
            // 
            // txtImageDir
            // 
            this.txtImageDir.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.txtImageDir.Location = new System.Drawing.Point(105, 57);
            this.txtImageDir.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtImageDir.Name = "txtImageDir";
            this.txtImageDir.Size = new System.Drawing.Size(210, 31);
            this.txtImageDir.TabIndex = 12;
            this.txtImageDir.TextChanged += new System.EventHandler(this.txtImageDir_TextChanged);
            // 
            // lbImageDir
            // 
            this.lbImageDir.AutoSize = true;
            this.lbImageDir.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.lbImageDir.Location = new System.Drawing.Point(14, 62);
            this.lbImageDir.Name = "lbImageDir";
            this.lbImageDir.Size = new System.Drawing.Size(107, 25);
            this.lbImageDir.TabIndex = 11;
            this.lbImageDir.Text = "이미지 경로";
            this.lbImageDir.Click += new System.EventHandler(this.lbImageDir_Click);
            // 
            // btnApply
            // 
            this.btnApply.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.btnApply.Location = new System.Drawing.Point(258, 98);
            this.btnApply.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(86, 35);
            this.btnApply.TabIndex = 10;
            this.btnApply.Text = "적용";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnSelModelDir
            // 
            this.btnSelModelDir.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.btnSelModelDir.Location = new System.Drawing.Point(299, 15);
            this.btnSelModelDir.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnSelModelDir.Name = "btnSelModelDir";
            this.btnSelModelDir.Size = new System.Drawing.Size(45, 31);
            this.btnSelModelDir.TabIndex = 9;
            this.btnSelModelDir.Text = "...";
            this.btnSelModelDir.UseVisualStyleBackColor = true;
            this.btnSelModelDir.Click += new System.EventHandler(this.btnSelModelDir_Click);
            // 
            // txtModelDir
            // 
            this.txtModelDir.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.txtModelDir.Location = new System.Drawing.Point(105, 15);
            this.txtModelDir.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtModelDir.Name = "txtModelDir";
            this.txtModelDir.Size = new System.Drawing.Size(210, 31);
            this.txtModelDir.TabIndex = 8;
            this.txtModelDir.TextChanged += new System.EventHandler(this.txtModelDir_TextChanged);
            // 
            // lbModelDir
            // 
            this.lbModelDir.AutoSize = true;
            this.lbModelDir.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.lbModelDir.Location = new System.Drawing.Point(14, 20);
            this.lbModelDir.Name = "lbModelDir";
            this.lbModelDir.Size = new System.Drawing.Size(89, 25);
            this.lbModelDir.TabIndex = 7;
            this.lbModelDir.Text = "모델 경로";
            this.lbModelDir.Click += new System.EventHandler(this.lbModelDir_Click);
            // 
            // PathSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnSelImageDir);
            this.Controls.Add(this.txtImageDir);
            this.Controls.Add(this.lbImageDir);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnSelModelDir);
            this.Controls.Add(this.txtModelDir);
            this.Controls.Add(this.lbModelDir);
            this.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "PathSetting";
            this.Size = new System.Drawing.Size(432, 161);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelImageDir;
        private System.Windows.Forms.TextBox txtImageDir;
        private System.Windows.Forms.Label lbImageDir;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnSelModelDir;
        private System.Windows.Forms.TextBox txtModelDir;
        private System.Windows.Forms.Label lbModelDir;
    }
}
