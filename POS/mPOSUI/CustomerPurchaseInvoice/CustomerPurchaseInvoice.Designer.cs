namespace POS.mPOSUI.CustomerPurchaseInvoice
{
    partial class CustomerPurchaseInvoice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerPurchaseInvoice));
            this.label2 = new System.Windows.Forms.Label();
            this.cbomemberId = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboCustomerId = new System.Windows.Forms.ComboBox();
            this.groupPurchaseInvoice = new System.Windows.Forms.GroupBox();
            this.txtPatientID = new System.Windows.Forms.TextBox();
            this.dtToDate = new System.Windows.Forms.DateTimePicker();
            this.dtDate = new System.Windows.Forms.DateTimePicker();
            this.btnClearSearch = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.Date = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgvDeleteLogList = new System.Windows.Forms.DataGridView();
            this.tabhistory = new System.Windows.Forms.TabPage();
            this.gvpackageUsedHistory = new System.Windows.Forms.DataGridView();
            this.usepackagetab = new System.Windows.Forms.TabControl();
            this.tabusepackage = new System.Windows.Forms.TabPage();
            this.CD = new System.Windows.Forms.DataGridView();
            this.groupPurchaseInvoice.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeleteLogList)).BeginInit();
            this.tabhistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvpackageUsedHistory)).BeginInit();
            this.usepackagetab.SuspendLayout();
            this.tabusepackage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CD)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Patient Name:";
            // 
            // cbomemberId
            // 
            this.cbomemberId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbomemberId.FormattingEnabled = true;
            this.cbomemberId.Location = new System.Drawing.Point(506, 67);
            this.cbomemberId.Name = "cbomemberId";
            this.cbomemberId.Size = new System.Drawing.Size(200, 23);
            this.cbomemberId.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(414, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Member Type:";
            // 
            // cboCustomerId
            // 
            this.cboCustomerId.FormattingEnabled = true;
            this.cboCustomerId.Location = new System.Drawing.Point(113, 67);
            this.cboCustomerId.Name = "cboCustomerId";
            this.cboCustomerId.Size = new System.Drawing.Size(217, 23);
            this.cboCustomerId.TabIndex = 1;
            // 
            // groupPurchaseInvoice
            // 
            this.groupPurchaseInvoice.Controls.Add(this.txtPatientID);
            this.groupPurchaseInvoice.Controls.Add(this.dtToDate);
            this.groupPurchaseInvoice.Controls.Add(this.cbomemberId);
            this.groupPurchaseInvoice.Controls.Add(this.dtDate);
            this.groupPurchaseInvoice.Controls.Add(this.btnClearSearch);
            this.groupPurchaseInvoice.Controls.Add(this.label1);
            this.groupPurchaseInvoice.Controls.Add(this.btnSearch);
            this.groupPurchaseInvoice.Controls.Add(this.cboCustomerId);
            this.groupPurchaseInvoice.Controls.Add(this.label3);
            this.groupPurchaseInvoice.Controls.Add(this.Date);
            this.groupPurchaseInvoice.Controls.Add(this.label4);
            this.groupPurchaseInvoice.Controls.Add(this.label2);
            this.groupPurchaseInvoice.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupPurchaseInvoice.Location = new System.Drawing.Point(19, 24);
            this.groupPurchaseInvoice.Name = "groupPurchaseInvoice";
            this.groupPurchaseInvoice.Size = new System.Drawing.Size(1004, 122);
            this.groupPurchaseInvoice.TabIndex = 5;
            this.groupPurchaseInvoice.TabStop = false;
            this.groupPurchaseInvoice.Text = "Search by:";
            // 
            // txtPatientID
            // 
            this.txtPatientID.Location = new System.Drawing.Point(113, 26);
            this.txtPatientID.Name = "txtPatientID";
            this.txtPatientID.Size = new System.Drawing.Size(217, 21);
            this.txtPatientID.TabIndex = 11;
            // 
            // dtToDate
            // 
            this.dtToDate.CustomFormat = "dd-MM-yyyy";
            this.dtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtToDate.Location = new System.Drawing.Point(784, 23);
            this.dtToDate.Name = "dtToDate";
            this.dtToDate.Size = new System.Drawing.Size(151, 21);
            this.dtToDate.TabIndex = 10;
            // 
            // dtDate
            // 
            this.dtDate.CustomFormat = "dd-MM-yyyy";
            this.dtDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtDate.Location = new System.Drawing.Point(506, 23);
            this.dtDate.Name = "dtDate";
            this.dtDate.Size = new System.Drawing.Size(151, 21);
            this.dtDate.TabIndex = 10;
            // 
            // btnClearSearch
            // 
            this.btnClearSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnClearSearch.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(202)))), ((int)(((byte)(125)))));
            this.btnClearSearch.FlatAppearance.BorderSize = 0;
            this.btnClearSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClearSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClearSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearSearch.Image = global::POS.Properties.Resources.refresh_small;
            this.btnClearSearch.Location = new System.Drawing.Point(897, 54);
            this.btnClearSearch.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnClearSearch.Name = "btnClearSearch";
            this.btnClearSearch.Size = new System.Drawing.Size(75, 46);
            this.btnClearSearch.TabIndex = 9;
            this.btnClearSearch.UseVisualStyleBackColor = false;
            this.btnClearSearch.Click += new System.EventHandler(this.btnClearSearch_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(202)))), ((int)(((byte)(125)))));
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Image = global::POS.Properties.Resources.search_small;
            this.btnSearch.Location = new System.Drawing.Point(784, 54);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 46);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click_1);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(720, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "To Date :";
            // 
            // Date
            // 
            this.Date.AutoSize = true;
            this.Date.Location = new System.Drawing.Point(414, 26);
            this.Date.Name = "Date";
            this.Date.Size = new System.Drawing.Size(39, 15);
            this.Date.TabIndex = 2;
            this.Date.Text = "Date :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 15);
            this.label4.TabIndex = 2;
            this.label4.Text = "Patient ID:";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgvDeleteLogList);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1010, 299);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Delete Log";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvDeleteLogList
            // 
            this.dgvDeleteLogList.AllowUserToAddRows = false;
            this.dgvDeleteLogList.BackgroundColor = System.Drawing.Color.White;
            this.dgvDeleteLogList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDeleteLogList.Location = new System.Drawing.Point(0, 3);
            this.dgvDeleteLogList.Name = "dgvDeleteLogList";
            this.dgvDeleteLogList.Size = new System.Drawing.Size(1007, 296);
            this.dgvDeleteLogList.TabIndex = 0;
            // 
            // tabhistory
            // 
            this.tabhistory.Controls.Add(this.gvpackageUsedHistory);
            this.tabhistory.Location = new System.Drawing.Point(4, 22);
            this.tabhistory.Name = "tabhistory";
            this.tabhistory.Padding = new System.Windows.Forms.Padding(3);
            this.tabhistory.Size = new System.Drawing.Size(1010, 299);
            this.tabhistory.TabIndex = 1;
            this.tabhistory.Text = "Offset History";
            this.tabhistory.UseVisualStyleBackColor = true;
            // 
            // gvpackageUsedHistory
            // 
            this.gvpackageUsedHistory.BackgroundColor = System.Drawing.Color.White;
            this.gvpackageUsedHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvpackageUsedHistory.Location = new System.Drawing.Point(0, 3);
            this.gvpackageUsedHistory.Name = "gvpackageUsedHistory";
            this.gvpackageUsedHistory.Size = new System.Drawing.Size(1007, 296);
            this.gvpackageUsedHistory.TabIndex = 0;
            this.gvpackageUsedHistory.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvpackageUsedHistory_CellClick);
            // 
            // usepackagetab
            // 
            this.usepackagetab.Controls.Add(this.tabusepackage);
            this.usepackagetab.Controls.Add(this.tabhistory);
            this.usepackagetab.Controls.Add(this.tabPage1);
            this.usepackagetab.Location = new System.Drawing.Point(12, 164);
            this.usepackagetab.Name = "usepackagetab";
            this.usepackagetab.SelectedIndex = 0;
            this.usepackagetab.Size = new System.Drawing.Size(1018, 325);
            this.usepackagetab.TabIndex = 7;
            this.usepackagetab.SelectedIndexChanged += new System.EventHandler(this.usepackagetab_SelectedIndexChanged);
            this.usepackagetab.TabIndexChanged += new System.EventHandler(this.usepackagetab_TabIndexChanged);
            // 
            // tabusepackage
            // 
            this.tabusepackage.Controls.Add(this.CD);
            this.tabusepackage.Location = new System.Drawing.Point(4, 22);
            this.tabusepackage.Name = "tabusepackage";
            this.tabusepackage.Padding = new System.Windows.Forms.Padding(3);
            this.tabusepackage.Size = new System.Drawing.Size(1010, 299);
            this.tabusepackage.TabIndex = 0;
            this.tabusepackage.Text = "Procedure";
            this.tabusepackage.UseVisualStyleBackColor = true;
            // 
            // CD
            // 
            this.CD.AllowUserToAddRows = false;
            this.CD.BackgroundColor = System.Drawing.Color.White;
            this.CD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CD.Location = new System.Drawing.Point(0, 3);
            this.CD.Name = "CD";
            this.CD.Size = new System.Drawing.Size(1007, 296);
            this.CD.TabIndex = 6;
            this.CD.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvPurchaseInoivce_CellClick);
            // 
            // CustomerPurchaseInvoice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1048, 519);
            this.Controls.Add(this.usepackagetab);
            this.Controls.Add(this.groupPurchaseInvoice);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CustomerPurchaseInvoice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Patient Purchase Invoice";
            this.Load += new System.EventHandler(this.CustomerPurchaseInvoice_Load);
            this.groupPurchaseInvoice.ResumeLayout(false);
            this.groupPurchaseInvoice.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeleteLogList)).EndInit();
            this.tabhistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvpackageUsedHistory)).EndInit();
            this.usepackagetab.ResumeLayout(false);
            this.tabusepackage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CD)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbomemberId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboCustomerId;
        private System.Windows.Forms.GroupBox groupPurchaseInvoice;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClearSearch;
        private System.Windows.Forms.DateTimePicker dtDate;
        private System.Windows.Forms.Label Date;
        private System.Windows.Forms.DateTimePicker dtToDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dgvDeleteLogList;
        private System.Windows.Forms.TabPage tabhistory;
        private System.Windows.Forms.DataGridView gvpackageUsedHistory;
        private System.Windows.Forms.TabControl usepackagetab;
        private System.Windows.Forms.TabPage tabusepackage;
        private System.Windows.Forms.DataGridView CD;
        private System.Windows.Forms.TextBox txtPatientID;
        private System.Windows.Forms.Label label4;
    }
}