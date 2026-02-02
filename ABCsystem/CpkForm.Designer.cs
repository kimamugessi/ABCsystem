namespace ABCsystem
{
    partial class CpkForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CpkForm));
            this.chartCpk = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chartCpk)).BeginInit();
            this.SuspendLayout();
            // 
            // chartCpk
            // 
            chartArea1.Name = "ChartArea1";
            this.chartCpk.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartCpk.Legends.Add(legend1);
            this.chartCpk.Location = new System.Drawing.Point(26, 12);
            this.chartCpk.Name = "chartCpk";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Value";
            this.chartCpk.Series.Add(series1);
            this.chartCpk.Size = new System.Drawing.Size(493, 296);
            this.chartCpk.TabIndex = 0;
            this.chartCpk.Text = "chart1";
            // 
            // CpkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.chartCpk);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CpkForm";
            this.Text = "Statistic CpK Display";
            ((System.ComponentModel.ISupportInitialize)(this.chartCpk)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartCpk;
    }
}