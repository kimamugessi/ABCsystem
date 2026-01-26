namespace ABCsystem.Property
{
    partial class EdgeProp
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
            this.btnEdge = new System.Windows.Forms.Button();
            this.cbEdgeType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnEdge
            // 
            this.btnEdge.Location = new System.Drawing.Point(27, 102);
            this.btnEdge.Name = "btnEdge";
            this.btnEdge.Size = new System.Drawing.Size(238, 58);
            this.btnEdge.TabIndex = 0;
            this.btnEdge.Text = "Inspect Edge";
            this.btnEdge.UseVisualStyleBackColor = true;
            this.btnEdge.Click += new System.EventHandler(this.btnEdge_Click);
            // 
            // cbEdgeType
            // 
            this.cbEdgeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEdgeType.FormattingEnabled = true;
            this.cbEdgeType.Items.AddRange(new object[] {
            "→",
            "←",
            "↑",
            "↓"});
            this.cbEdgeType.Location = new System.Drawing.Point(27, 39);
            this.cbEdgeType.Name = "cbEdgeType";
            this.cbEdgeType.Size = new System.Drawing.Size(238, 32);
            this.cbEdgeType.TabIndex = 1;
            this.cbEdgeType.SelectedIndexChanged += new System.EventHandler(this.cbEdgeType_SelectedIndexChanged);
            // 
            // EdgeProp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbEdgeType);
            this.Controls.Add(this.btnEdge);
            this.Name = "EdgeProp";
            this.Size = new System.Drawing.Size(509, 468);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnEdge;
        private System.Windows.Forms.ComboBox cbEdgeType;
    }
}
