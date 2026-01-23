namespace ABCsystem4.Setting
{
    partial class CameraSetting
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
            this.btnApply = new System.Windows.Forms.Button();
            this.cbCameraType = new System.Windows.Forms.ComboBox();
            this.lbCameraType = new System.Windows.Forms.Label();
            this.lbExposure = new System.Windows.Forms.Label();
            this.tbExposure = new System.Windows.Forms.TextBox();
            this.lbExpUnit = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(183, 105);
            this.btnApply.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(130, 39);
            this.btnApply.TabIndex = 5;
            this.btnApply.Text = "적용";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // cbCameraType
            // 
            this.cbCameraType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCameraType.FormattingEnabled = true;
            this.cbCameraType.Location = new System.Drawing.Point(111, 9);
            this.cbCameraType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbCameraType.Name = "cbCameraType";
            this.cbCameraType.Size = new System.Drawing.Size(200, 26);
            this.cbCameraType.TabIndex = 4;
            // 
            // lbCameraType
            // 
            this.lbCameraType.AutoSize = true;
            this.lbCameraType.Location = new System.Drawing.Point(4, 14);
            this.lbCameraType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbCameraType.Name = "lbCameraType";
            this.lbCameraType.Size = new System.Drawing.Size(104, 18);
            this.lbCameraType.TabIndex = 3;
            this.lbCameraType.Text = "카메라 종료";
            // 
            // lbExposure
            // 
            this.lbExposure.AutoSize = true;
            this.lbExposure.Location = new System.Drawing.Point(4, 52);
            this.lbExposure.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbExposure.Name = "lbExposure";
            this.lbExposure.Size = new System.Drawing.Size(80, 18);
            this.lbExposure.TabIndex = 3;
            this.lbExposure.Text = "노출시간";
            // 
            // tbExposure
            // 
            this.tbExposure.Location = new System.Drawing.Point(110, 50);
            this.tbExposure.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbExposure.Name = "tbExposure";
            this.tbExposure.Size = new System.Drawing.Size(151, 28);
            this.tbExposure.TabIndex = 6;
            // 
            // lbExpUnit
            // 
            this.lbExpUnit.AutoSize = true;
            this.lbExpUnit.Location = new System.Drawing.Point(271, 54);
            this.lbExpUnit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbExpUnit.Name = "lbExpUnit";
            this.lbExpUnit.Size = new System.Drawing.Size(33, 18);
            this.lbExpUnit.TabIndex = 3;
            this.lbExpUnit.Text = "ms";
            // 
            // CameraSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbExposure);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.cbCameraType);
            this.Controls.Add(this.lbExpUnit);
            this.Controls.Add(this.lbExposure);
            this.Controls.Add(this.lbCameraType);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "CameraSetting";
            this.Size = new System.Drawing.Size(354, 184);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.ComboBox cbCameraType;
        private System.Windows.Forms.Label lbCameraType;
        private System.Windows.Forms.Label lbExposure;
        private System.Windows.Forms.TextBox tbExposure;
        private System.Windows.Forms.Label lbExpUnit;
    }
}
