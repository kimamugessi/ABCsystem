namespace ABCsystem
{
    partial class NewModel
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
            this.btnCreate = new System.Windows.Forms.Button();
            this.txtModelInfo = new System.Windows.Forms.RichTextBox();
            this.txtModelName = new System.Windows.Forms.TextBox();
            this.lbModelInfo = new System.Windows.Forms.Label();
            this.lbModelName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCreate
            // 
            this.btnCreate.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.btnCreate.Location = new System.Drawing.Point(194, 217);
            this.btnCreate.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(90, 45);
            this.btnCreate.TabIndex = 9;
            this.btnCreate.Text = "만들기";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // txtModelInfo
            // 
            this.txtModelInfo.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.txtModelInfo.Location = new System.Drawing.Point(12, 81);
            this.txtModelInfo.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtModelInfo.Name = "txtModelInfo";
            this.txtModelInfo.Size = new System.Drawing.Size(272, 126);
            this.txtModelInfo.TabIndex = 8;
            this.txtModelInfo.Text = "";
            // 
            // txtModelName
            // 
            this.txtModelName.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.txtModelName.Location = new System.Drawing.Point(67, 14);
            this.txtModelName.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtModelName.Name = "txtModelName";
            this.txtModelName.Size = new System.Drawing.Size(217, 26);
            this.txtModelName.TabIndex = 7;
            // 
            // lbModelInfo
            // 
            this.lbModelInfo.AutoSize = true;
            this.lbModelInfo.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.lbModelInfo.Location = new System.Drawing.Point(14, 58);
            this.lbModelInfo.Name = "lbModelInfo";
            this.lbModelInfo.Size = new System.Drawing.Size(64, 18);
            this.lbModelInfo.TabIndex = 6;
            this.lbModelInfo.Text = "모델 정보";
            // 
            // lbModelName
            // 
            this.lbModelName.AutoSize = true;
            this.lbModelName.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.lbModelName.Location = new System.Drawing.Point(14, 17);
            this.lbModelName.Name = "lbModelName";
            this.lbModelName.Size = new System.Drawing.Size(47, 18);
            this.lbModelName.TabIndex = 5;
            this.lbModelName.Text = "모델명";
            // 
            // NewModel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 269);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.txtModelInfo);
            this.Controls.Add(this.txtModelName);
            this.Controls.Add(this.lbModelInfo);
            this.Controls.Add(this.lbModelName);
            this.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "NewModel";
            this.Text = "NewModel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.RichTextBox txtModelInfo;
        private System.Windows.Forms.TextBox txtModelName;
        private System.Windows.Forms.Label lbModelInfo;
        private System.Windows.Forms.Label lbModelName;
    }
}