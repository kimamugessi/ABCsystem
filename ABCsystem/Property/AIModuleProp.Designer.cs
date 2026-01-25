namespace ABCsystem.Property
{
    partial class AIModuleProp
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
            this.cbAIModelType = new System.Windows.Forms.ComboBox();
            this.txtAIModelPath = new System.Windows.Forms.TextBox();
            this.btnInspAI = new System.Windows.Forms.Button();
            this.btnLoadModel = new System.Windows.Forms.Button();
            this.btnSelAIModel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbAIModelType
            // 
            this.cbAIModelType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAIModelType.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.cbAIModelType.FormattingEnabled = true;
            this.cbAIModelType.Items.AddRange(new object[] {
            "Segmentation(SEG)",
            "Detection(DET)",
            "Classification(CLS)"});
            this.cbAIModelType.Location = new System.Drawing.Point(18, 30);
            this.cbAIModelType.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbAIModelType.Name = "cbAIModelType";
            this.cbAIModelType.Size = new System.Drawing.Size(279, 26);
            this.cbAIModelType.TabIndex = 1;
            this.cbAIModelType.SelectedIndexChanged += new System.EventHandler(this.cbAIModelType_SelectedIndexChanged);
            // 
            // txtAIModelPath
            // 
            this.txtAIModelPath.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.txtAIModelPath.Location = new System.Drawing.Point(18, 75);
            this.txtAIModelPath.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtAIModelPath.Multiline = true;
            this.txtAIModelPath.Name = "txtAIModelPath";
            this.txtAIModelPath.Size = new System.Drawing.Size(279, 39);
            this.txtAIModelPath.TabIndex = 2;
            this.txtAIModelPath.Visible = false;
            this.txtAIModelPath.TextChanged += new System.EventHandler(this.txtAIModelPath_TextChanged);
            // 
            // btnInspAI
            // 
            this.btnInspAI.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.btnInspAI.Location = new System.Drawing.Point(18, 248);
            this.btnInspAI.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnInspAI.Name = "btnInspAI";
            this.btnInspAI.Size = new System.Drawing.Size(102, 39);
            this.btnInspAI.TabIndex = 6;
            this.btnInspAI.Text = "AI 검사";
            this.btnInspAI.UseVisualStyleBackColor = true;
            this.btnInspAI.Click += new System.EventHandler(this.btnInspAI_Click);
            // 
            // btnLoadModel
            // 
            this.btnLoadModel.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.btnLoadModel.Location = new System.Drawing.Point(18, 192);
            this.btnLoadModel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnLoadModel.Name = "btnLoadModel";
            this.btnLoadModel.Size = new System.Drawing.Size(102, 39);
            this.btnLoadModel.TabIndex = 5;
            this.btnLoadModel.Text = "모델 로딩";
            this.btnLoadModel.UseVisualStyleBackColor = true;
            this.btnLoadModel.Click += new System.EventHandler(this.btnLoadModel_Click);
            // 
            // btnSelAIModel
            // 
            this.btnSelAIModel.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.btnSelAIModel.Location = new System.Drawing.Point(18, 138);
            this.btnSelAIModel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSelAIModel.Name = "btnSelAIModel";
            this.btnSelAIModel.Size = new System.Drawing.Size(105, 39);
            this.btnSelAIModel.TabIndex = 4;
            this.btnSelAIModel.Text = "AI모델 선택";
            this.btnSelAIModel.UseVisualStyleBackColor = true;
            this.btnSelAIModel.Click += new System.EventHandler(this.btnSelAIModel_Click);
            // 
            // AIModuleProp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnInspAI);
            this.Controls.Add(this.btnLoadModel);
            this.Controls.Add(this.btnSelAIModel);
            this.Controls.Add(this.txtAIModelPath);
            this.Controls.Add(this.cbAIModelType);
            this.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "AIModuleProp";
            this.Size = new System.Drawing.Size(312, 519);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbAIModelType;
        private System.Windows.Forms.TextBox txtAIModelPath;
        private System.Windows.Forms.Button btnInspAI;
        private System.Windows.Forms.Button btnLoadModel;
        private System.Windows.Forms.Button btnSelAIModel;
    }
}
