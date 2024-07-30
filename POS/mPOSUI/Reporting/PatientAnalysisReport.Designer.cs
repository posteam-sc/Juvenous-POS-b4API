namespace POS
{
    partial class frmPatientAnalysisReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPatientAnalysisReport));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.txtRow = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rdoDESC = new System.Windows.Forms.RadioButton();
            this.rdoASC = new System.Windows.Forms.RadioButton();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cboSortedColumns = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRowFilter = new System.Windows.Forms.Button();
            this.btnClearSearch = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(369, 71);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search By Name";
            // 
            // cboName
            // 
            this.cboName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboName.FormattingEnabled = true;
            this.cboName.Location = new System.Drawing.Point(148, 27);
            this.cboName.Name = "cboName";
            this.cboName.Size = new System.Drawing.Size(194, 28);
            this.cboName.TabIndex = 3;
            this.cboName.SelectedIndexChanged += new System.EventHandler(this.cboName_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Patient Name :";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.reportViewer1);
            this.groupBox2.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 200);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(1236, 578);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Patient Analysis Report";
            // 
            // reportViewer1
            // 
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "POS.bin.Debug.Reports.PatientAnalysisReport.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(6, 26);
            this.reportViewer1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ShowPrintButton = false;
            this.reportViewer1.ShowRefreshButton = false;
            this.reportViewer1.ShowStopButton = false;
            this.reportViewer1.ShowZoomControl = false;
            this.reportViewer1.Size = new System.Drawing.Size(1217, 544);
            this.reportViewer1.TabIndex = 5;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.txtRow);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.groupBox5.Location = new System.Drawing.Point(399, 12);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox5.Size = new System.Drawing.Size(253, 69);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            // 
            // txtRow
            // 
            this.txtRow.Location = new System.Drawing.Point(123, 27);
            this.txtRow.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtRow.Name = "txtRow";
            this.txtRow.Size = new System.Drawing.Size(104, 27);
            this.txtRow.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 20);
            this.label4.TabIndex = 0;
            this.label4.Text = "Total Row :";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rdoDESC);
            this.groupBox4.Controls.Add(this.rdoASC);
            this.groupBox4.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(12, 110);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox4.Size = new System.Drawing.Size(281, 71);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Order By";
            // 
            // rdoDESC
            // 
            this.rdoDESC.AutoSize = true;
            this.rdoDESC.Location = new System.Drawing.Point(167, 27);
            this.rdoDESC.Name = "rdoDESC";
            this.rdoDESC.Size = new System.Drawing.Size(90, 24);
            this.rdoDESC.TabIndex = 7;
            this.rdoDESC.TabStop = true;
            this.rdoDESC.Text = "Descending";
            this.rdoDESC.UseVisualStyleBackColor = true;
            this.rdoDESC.CheckedChanged += new System.EventHandler(this.rdoDESC_CheckedChanged);
            // 
            // rdoASC
            // 
            this.rdoASC.AutoSize = true;
            this.rdoASC.Location = new System.Drawing.Point(21, 29);
            this.rdoASC.Name = "rdoASC";
            this.rdoASC.Size = new System.Drawing.Size(83, 24);
            this.rdoASC.TabIndex = 6;
            this.rdoASC.TabStop = true;
            this.rdoASC.Text = "Ascending";
            this.rdoASC.UseVisualStyleBackColor = true;
            this.rdoASC.CheckedChanged += new System.EventHandler(this.rdoASC_CheckedChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.cboSortedColumns);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.Location = new System.Drawing.Point(324, 110);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox6.Size = new System.Drawing.Size(369, 71);
            this.groupBox6.TabIndex = 4;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Sorted By Column";
            // 
            // cboSortedColumns
            // 
            this.cboSortedColumns.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboSortedColumns.FormattingEnabled = true;
            this.cboSortedColumns.Items.AddRange(new object[] {
            "Select",
            "Total Spending Amount",
            "Outstanding Balance",
            "Visit Count",
            "No. Of Referrals",
            "Total Spent Amount By Refferals"});
            this.cboSortedColumns.Location = new System.Drawing.Point(145, 25);
            this.cboSortedColumns.Name = "cboSortedColumns";
            this.cboSortedColumns.Size = new System.Drawing.Size(194, 28);
            this.cboSortedColumns.TabIndex = 3;
            this.cboSortedColumns.SelectedIndexChanged += new System.EventHandler(this.cboSortedColumns_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Choose Column:";
            // 
            // btnRowFilter
            // 
            this.btnRowFilter.BackColor = System.Drawing.Color.Transparent;
            this.btnRowFilter.FlatAppearance.BorderSize = 0;
            this.btnRowFilter.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRowFilter.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRowFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRowFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRowFilter.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnRowFilter.Image = global::POS.Properties.Resources.Frame_39849;
            this.btnRowFilter.Location = new System.Drawing.Point(689, 35);
            this.btnRowFilter.Name = "btnRowFilter";
            this.btnRowFilter.Size = new System.Drawing.Size(101, 42);
            this.btnRowFilter.TabIndex = 9;
            this.btnRowFilter.UseVisualStyleBackColor = false;
            this.btnRowFilter.Click += new System.EventHandler(this.btnRowFilter_Click);
            // 
            // btnClearSearch
            // 
            this.btnClearSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnClearSearch.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(202)))), ((int)(((byte)(125)))));
            this.btnClearSearch.FlatAppearance.BorderSize = 0;
            this.btnClearSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClearSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClearSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearSearch.Image = global::POS.Properties.Resources.Frame_39848;
            this.btnClearSearch.Location = new System.Drawing.Point(729, 132);
            this.btnClearSearch.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnClearSearch.Name = "btnClearSearch";
            this.btnClearSearch.Size = new System.Drawing.Size(107, 44);
            this.btnClearSearch.TabIndex = 8;
            this.btnClearSearch.UseVisualStyleBackColor = false;
            this.btnClearSearch.Click += new System.EventHandler(this.btnClearSearch_Click);
            // 
            // frmPatientAnalysisReport
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1272, 784);
            this.Controls.Add(this.btnRowFilter);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.btnClearSearch);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPatientAnalysisReport";
            this.Text = "Patient Analysis Report";
            this.Load += new System.EventHandler(this.PatientAnalysisReportTest_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cboName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox txtRow;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnClearSearch;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rdoDESC;
        private System.Windows.Forms.RadioButton rdoASC;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ComboBox cboSortedColumns;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRowFilter;
    }
}