namespace ABCsystem.Property
{
    partial class BinaryProp
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
                if (binRangeTrackbar != null)
                    binRangeTrackbar.RangeChanged -= Range_RangeChanged;

                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpBinary = new System.Windows.Forms.GroupBox();
            this.cbChannel = new System.Windows.Forms.ComboBox();
            this.cbHighlight = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbHighlight = new System.Windows.Forms.Label();
            this.lbChannel = new System.Windows.Forms.Label();
            this.chkUse = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbBinMethod = new System.Windows.Forms.ComboBox();
            this.chkRotatedRect = new System.Windows.Forms.CheckBox();
            this.dataGridViewFilter = new System.Windows.Forms.DataGridView();
            this.binRangeTrackbar = new ABCsystem.UIControl.RangeTrackbar();
            this.grpBinary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFilter)).BeginInit();
            this.SuspendLayout();
            // 
            // grpBinary
            // 
            this.grpBinary.Controls.Add(this.cbChannel);
            this.grpBinary.Controls.Add(this.cbHighlight);
            this.grpBinary.Controls.Add(this.label2);
            this.grpBinary.Controls.Add(this.lbHighlight);
            this.grpBinary.Controls.Add(this.binRangeTrackbar);
            this.grpBinary.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.grpBinary.Location = new System.Drawing.Point(13, 54);
            this.grpBinary.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpBinary.Name = "grpBinary";
            this.grpBinary.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpBinary.Size = new System.Drawing.Size(311, 172);
            this.grpBinary.TabIndex = 0;
            this.grpBinary.TabStop = false;
            this.grpBinary.Text = "이진화";
            // 
            // cbChannel
            // 
            this.cbChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbChannel.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.cbChannel.FormattingEnabled = true;
            this.cbChannel.Location = new System.Drawing.Point(97, 87);
            this.cbChannel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbChannel.Name = "cbChannel";
            this.cbChannel.Size = new System.Drawing.Size(133, 26);
            this.cbChannel.TabIndex = 2;
            this.cbChannel.SelectedIndexChanged += new System.EventHandler(this.cbChannel_SelectedIndexChanged);
            // 
            // cbHighlight
            // 
            this.cbHighlight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbHighlight.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.cbHighlight.FormattingEnabled = true;
            this.cbHighlight.Location = new System.Drawing.Point(97, 130);
            this.cbHighlight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbHighlight.Name = "cbHighlight";
            this.cbHighlight.Size = new System.Drawing.Size(133, 26);
            this.cbHighlight.TabIndex = 2;
            this.cbHighlight.SelectedIndexChanged += new System.EventHandler(this.cbHighlight_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.label2.Location = new System.Drawing.Point(13, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 18);
            this.label2.TabIndex = 7;
            this.label2.Text = "이미지 채널";
            // 
            // lbHighlight
            // 
            this.lbHighlight.AutoSize = true;
            this.lbHighlight.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.lbHighlight.Location = new System.Drawing.Point(13, 134);
            this.lbHighlight.Name = "lbHighlight";
            this.lbHighlight.Size = new System.Drawing.Size(73, 18);
            this.lbHighlight.TabIndex = 7;
            this.lbHighlight.Text = "하이라이트";
            // 
            // lbChannel
            // 
            this.lbChannel.AutoSize = true;
            this.lbChannel.Location = new System.Drawing.Point(6, 59);
            this.lbChannel.Name = "lbChannel";
            this.lbChannel.Size = new System.Drawing.Size(69, 12);
            this.lbChannel.TabIndex = 7;
            this.lbChannel.Text = "이미지 채널";
            // 
            // chkUse
            // 
            this.chkUse.AutoSize = true;
            this.chkUse.Checked = true;
            this.chkUse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUse.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.chkUse.Location = new System.Drawing.Point(16, 19);
            this.chkUse.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkUse.Name = "chkUse";
            this.chkUse.Size = new System.Drawing.Size(53, 22);
            this.chkUse.TabIndex = 4;
            this.chkUse.Text = "검사";
            this.chkUse.UseVisualStyleBackColor = true;
            this.chkUse.CheckedChanged += new System.EventHandler(this.chkUse_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.label1.Location = new System.Drawing.Point(13, 246);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 18);
            this.label1.TabIndex = 7;
            this.label1.Text = "검사 타입";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // cbBinMethod
            // 
            this.cbBinMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBinMethod.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.cbBinMethod.FormattingEnabled = true;
            this.cbBinMethod.Location = new System.Drawing.Point(84, 242);
            this.cbBinMethod.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbBinMethod.Name = "cbBinMethod";
            this.cbBinMethod.Size = new System.Drawing.Size(133, 26);
            this.cbBinMethod.TabIndex = 2;
            this.cbBinMethod.SelectedIndexChanged += new System.EventHandler(this.cbBinMethod_SelectedIndexChanged);
            // 
            // chkRotatedRect
            // 
            this.chkRotatedRect.AutoSize = true;
            this.chkRotatedRect.Checked = true;
            this.chkRotatedRect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRotatedRect.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.chkRotatedRect.Location = new System.Drawing.Point(15, 419);
            this.chkRotatedRect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkRotatedRect.Name = "chkRotatedRect";
            this.chkRotatedRect.Size = new System.Drawing.Size(92, 22);
            this.chkRotatedRect.TabIndex = 16;
            this.chkRotatedRect.Text = "회전사각형";
            this.chkRotatedRect.UseVisualStyleBackColor = true;
            this.chkRotatedRect.CheckedChanged += new System.EventHandler(this.chkRotatedRect_CheckedChanged_1);
            // 
            // dataGridViewFilter
            // 
            this.dataGridViewFilter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFilter.Location = new System.Drawing.Point(13, 285);
            this.dataGridViewFilter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridViewFilter.Name = "dataGridViewFilter";
            this.dataGridViewFilter.RowHeadersWidth = 62;
            this.dataGridViewFilter.RowTemplate.Height = 23;
            this.dataGridViewFilter.Size = new System.Drawing.Size(243, 120);
            this.dataGridViewFilter.TabIndex = 15;
            this.dataGridViewFilter.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewFilter_CellContentClick);
            this.dataGridViewFilter.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewFilter_CellValueChanged);
            // 
            // binRangeTrackbar
            // 
            this.binRangeTrackbar.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.binRangeTrackbar.Location = new System.Drawing.Point(16, 21);
            this.binRangeTrackbar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.binRangeTrackbar.Name = "binRangeTrackbar";
            this.binRangeTrackbar.Size = new System.Drawing.Size(278, 66);
            this.binRangeTrackbar.TabIndex = 6;
            this.binRangeTrackbar.ValueLeft = 0;
            this.binRangeTrackbar.ValueRight = 128;
            // 
            // BinaryProp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkRotatedRect);
            this.Controls.Add(this.dataGridViewFilter);
            this.Controls.Add(this.cbBinMethod);
            this.Controls.Add(this.chkUse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grpBinary);
            this.Font = new System.Drawing.Font("한컴산뜻돋움", 9.163636F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "BinaryProp";
            this.Size = new System.Drawing.Size(339, 507);
            this.grpBinary.ResumeLayout(false);
            this.grpBinary.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFilter)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBinary;
        private UIControl.RangeTrackbar binRangeTrackbar;
        private System.Windows.Forms.ComboBox cbHighlight;
        private System.Windows.Forms.Label lbHighlight;
        private System.Windows.Forms.CheckBox chkUse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbBinMethod;
        private System.Windows.Forms.CheckBox chkRotatedRect;
        private System.Windows.Forms.DataGridView dataGridViewFilter;
        private System.Windows.Forms.ComboBox cbChannel;
        private System.Windows.Forms.Label lbChannel;
        private System.Windows.Forms.Label label2;
    }
}
