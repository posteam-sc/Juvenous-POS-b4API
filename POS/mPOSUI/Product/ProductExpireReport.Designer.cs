namespace POS
{
    partial class ProductExpireReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductExpireReport));
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cboExpireDay = new System.Windows.Forms.ComboBox();
            this.expdays = new System.Windows.Forms.Label();
            this.txtCurrentDate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboSubCatgory = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboCategory = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboBrand = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rvExpire = new Microsoft.Reporting.WinForms.ReportViewer();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnRefresh);
            this.gbFilter.Controls.Add(this.btnSearch);
            this.gbFilter.Controls.Add(this.cboExpireDay);
            this.gbFilter.Controls.Add(this.expdays);
            this.gbFilter.Controls.Add(this.txtCurrentDate);
            this.gbFilter.Controls.Add(this.label5);
            this.gbFilter.Controls.Add(this.cboSubCatgory);
            this.gbFilter.Controls.Add(this.label3);
            this.gbFilter.Controls.Add(this.cboCategory);
            this.gbFilter.Controls.Add(this.label2);
            this.gbFilter.Controls.Add(this.cboBrand);
            this.gbFilter.Controls.Add(this.label1);
            this.gbFilter.Location = new System.Drawing.Point(7, -1);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(979, 148);
            this.gbFilter.TabIndex = 2;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Search Filter";
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackgroundImage = global::POS.Properties.Resources.refresh_big;
            this.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRefresh.Location = new System.Drawing.Point(745, 85);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(110, 38);
            this.btnRefresh.TabIndex = 19;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImage = global::POS.Properties.Resources.search_big;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.Location = new System.Drawing.Point(745, 30);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(110, 38);
            this.btnSearch.TabIndex = 18;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cboExpireDay
            // 
            this.cboExpireDay.FormattingEnabled = true;
            this.cboExpireDay.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "6"});
            this.cboExpireDay.Location = new System.Drawing.Point(128, 61);
            this.cboExpireDay.Name = "cboExpireDay";
            this.cboExpireDay.Size = new System.Drawing.Size(150, 28);
            this.cboExpireDay.TabIndex = 17;
            // 
            // expdays
            // 
            this.expdays.AutoSize = true;
            this.expdays.Location = new System.Drawing.Point(52, 61);
            this.expdays.Name = "expdays";
            this.expdays.Size = new System.Drawing.Size(55, 20);
            this.expdays.TabIndex = 16;
            this.expdays.Text = "Expire in";
            // 
            // txtCurrentDate
            // 
            this.txtCurrentDate.Location = new System.Drawing.Point(128, 19);
            this.txtCurrentDate.Name = "txtCurrentDate";
            this.txtCurrentDate.ReadOnly = true;
            this.txtCurrentDate.Size = new System.Drawing.Size(150, 27);
            this.txtCurrentDate.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 20);
            this.label5.TabIndex = 14;
            this.label5.Text = "Current Date";
            // 
            // cboSubCatgory
            // 
            this.cboSubCatgory.FormattingEnabled = true;
            this.cboSubCatgory.Location = new System.Drawing.Point(502, 112);
            this.cboSubCatgory.Name = "cboSubCatgory";
            this.cboSubCatgory.Size = new System.Drawing.Size(172, 28);
            this.cboSubCatgory.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(413, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Sub Category";
            // 
            // cboCategory
            // 
            this.cboCategory.FormattingEnabled = true;
            this.cboCategory.Location = new System.Drawing.Point(502, 69);
            this.cboCategory.Name = "cboCategory";
            this.cboCategory.Size = new System.Drawing.Size(172, 28);
            this.cboCategory.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(438, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Category";
            // 
            // cboBrand
            // 
            this.cboBrand.FormattingEnabled = true;
            this.cboBrand.Location = new System.Drawing.Point(502, 22);
            this.cboBrand.Name = "cboBrand";
            this.cboBrand.Size = new System.Drawing.Size(172, 28);
            this.cboBrand.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(447, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Brand";
            // 
            // rvExpire
            // 
            this.rvExpire.Location = new System.Drawing.Point(7, 153);
            this.rvExpire.Name = "rvExpire";
            this.rvExpire.Size = new System.Drawing.Size(979, 489);
            this.rvExpire.TabIndex = 3;
            // 
            // ProductExpireReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 654);
            this.Controls.Add(this.rvExpire);
            this.Controls.Add(this.gbFilter);
            this.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "ProductExpireReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Product Expire Report";
            this.Load += new System.EventHandler(this.ProductExpireReport_Load);
            this.gbFilter.ResumeLayout(false);
            this.gbFilter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox gbFilter;
        private Microsoft.Reporting.WinForms.ReportViewer rvExpire;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboBrand;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboSubCatgory;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboCategory;
        private System.Windows.Forms.TextBox txtCurrentDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboExpireDay;
        private System.Windows.Forms.Label expdays;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSearch;
    }
}