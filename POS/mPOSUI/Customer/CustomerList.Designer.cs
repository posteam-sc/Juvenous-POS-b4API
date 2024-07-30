namespace POS
{
    partial class CustomerList
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerList));
            this.btnAddNewCustomer = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoBirthMonth = new System.Windows.Forms.RadioButton();
            this.rdoBirth_Day = new System.Windows.Forms.RadioButton();
            this.dtpBirthday = new System.Windows.Forms.DateTimePicker();
            this.rdoBirthDate = new System.Windows.Forms.RadioButton();
            this.lblSearchTitle = new System.Windows.Forms.Label();
            this.rdoMemberCardNo = new System.Windows.Forms.RadioButton();
            this.rdoCustomerCode = new System.Windows.Forms.RadioButton();
            this.rdoCustomerName = new System.Windows.Forms.RadioButton();
            this.btnClearSearch = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboMemberType = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvCustomerList = new System.Windows.Forms.DataGridView();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPatientID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColPatientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColDateofBirth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColViewDetail = new System.Windows.Forms.DataGridViewLinkColumn();
            this.ColViewPackage = new System.Windows.Forms.DataGridViewLinkColumn();
            this.ColEdit = new System.Windows.Forms.DataGridViewLinkColumn();
            this.ColDelete = new System.Windows.Forms.DataGridViewLinkColumn();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomerList)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAddNewCustomer
            // 
            this.btnAddNewCustomer.BackColor = System.Drawing.Color.Transparent;
            this.btnAddNewCustomer.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(202)))), ((int)(((byte)(125)))));
            this.btnAddNewCustomer.FlatAppearance.BorderSize = 0;
            this.btnAddNewCustomer.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAddNewCustomer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAddNewCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddNewCustomer.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnAddNewCustomer.Image = global::POS.Properties.Resources.addnewcustomer;
            this.btnAddNewCustomer.Location = new System.Drawing.Point(732, -5);
            this.btnAddNewCustomer.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnAddNewCustomer.Name = "btnAddNewCustomer";
            this.btnAddNewCustomer.Size = new System.Drawing.Size(215, 79);
            this.btnAddNewCustomer.TabIndex = 1;
            this.btnAddNewCustomer.UseVisualStyleBackColor = false;
            this.btnAddNewCustomer.Click += new System.EventHandler(this.btnAddNewCustomer_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoBirthMonth);
            this.groupBox1.Controls.Add(this.rdoBirth_Day);
            this.groupBox1.Controls.Add(this.dtpBirthday);
            this.groupBox1.Controls.Add(this.rdoBirthDate);
            this.groupBox1.Controls.Add(this.lblSearchTitle);
            this.groupBox1.Controls.Add(this.rdoMemberCardNo);
            this.groupBox1.Controls.Add(this.rdoCustomerCode);
            this.groupBox1.Controls.Add(this.rdoCustomerName);
            this.groupBox1.Controls.Add(this.btnClearSearch);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtSearch);
            this.groupBox1.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(55)))), ((int)(((byte)(46)))));
            this.groupBox1.Location = new System.Drawing.Point(7, 59);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(914, 106);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search By";
            // 
            // rdoBirthMonth
            // 
            this.rdoBirthMonth.AutoSize = true;
            this.rdoBirthMonth.Location = new System.Drawing.Point(760, 24);
            this.rdoBirthMonth.Name = "rdoBirthMonth";
            this.rdoBirthMonth.Size = new System.Drawing.Size(103, 29);
            this.rdoBirthMonth.TabIndex = 8;
            this.rdoBirthMonth.Text = "BirthMonth";
            this.rdoBirthMonth.UseVisualStyleBackColor = true;
            this.rdoBirthMonth.CheckedChanged += new System.EventHandler(this.rdoBirthMonth_CheckedChanged);
            // 
            // rdoBirth_Day
            // 
            this.rdoBirth_Day.AutoSize = true;
            this.rdoBirth_Day.Location = new System.Drawing.Point(637, 24);
            this.rdoBirth_Day.Name = "rdoBirth_Day";
            this.rdoBirth_Day.Size = new System.Drawing.Size(88, 29);
            this.rdoBirth_Day.TabIndex = 7;
            this.rdoBirth_Day.Text = "BirthDay";
            this.rdoBirth_Day.UseVisualStyleBackColor = true;
            this.rdoBirth_Day.CheckedChanged += new System.EventHandler(this.rdoBirth_Day_CheckedChanged);
            // 
            // dtpBirthday
            // 
            this.dtpBirthday.CustomFormat = "dd-MM-yyyy";
            this.dtpBirthday.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpBirthday.Location = new System.Drawing.Point(130, 66);
            this.dtpBirthday.Name = "dtpBirthday";
            this.dtpBirthday.Size = new System.Drawing.Size(262, 32);
            this.dtpBirthday.TabIndex = 4;
            // 
            // rdoBirthDate
            // 
            this.rdoBirthDate.AutoSize = true;
            this.rdoBirthDate.Location = new System.Drawing.Point(516, 25);
            this.rdoBirthDate.Name = "rdoBirthDate";
            this.rdoBirthDate.Size = new System.Drawing.Size(63, 29);
            this.rdoBirthDate.TabIndex = 2;
            this.rdoBirthDate.Text = "DOB";
            this.rdoBirthDate.UseVisualStyleBackColor = true;
            this.rdoBirthDate.CheckedChanged += new System.EventHandler(this.rdoBirthday_CheckedChanged);
            // 
            // lblSearchTitle
            // 
            this.lblSearchTitle.AutoSize = true;
            this.lblSearchTitle.Location = new System.Drawing.Point(15, 69);
            this.lblSearchTitle.Name = "lblSearchTitle";
            this.lblSearchTitle.Size = new System.Drawing.Size(129, 25);
            this.lblSearchTitle.TabIndex = 3;
            this.lblSearchTitle.Text = "Member Card No.";
            // 
            // rdoMemberCardNo
            // 
            this.rdoMemberCardNo.AutoSize = true;
            this.rdoMemberCardNo.Checked = true;
            this.rdoMemberCardNo.Location = new System.Drawing.Point(18, 25);
            this.rdoMemberCardNo.Name = "rdoMemberCardNo";
            this.rdoMemberCardNo.Size = new System.Drawing.Size(150, 29);
            this.rdoMemberCardNo.TabIndex = 0;
            this.rdoMemberCardNo.TabStop = true;
            this.rdoMemberCardNo.Text = "Member Card No.";
            this.rdoMemberCardNo.UseVisualStyleBackColor = true;
            this.rdoMemberCardNo.CheckedChanged += new System.EventHandler(this.rdoMemberCardNo_CheckedChanged);
            // 
            // rdoCustomerCode
            // 
            this.rdoCustomerCode.AutoSize = true;
            this.rdoCustomerCode.Location = new System.Drawing.Point(186, 25);
            this.rdoCustomerCode.Name = "rdoCustomerCode";
            this.rdoCustomerCode.Size = new System.Drawing.Size(98, 29);
            this.rdoCustomerCode.TabIndex = 1;
            this.rdoCustomerCode.Text = "Patient ID";
            this.rdoCustomerCode.UseVisualStyleBackColor = true;
            this.rdoCustomerCode.CheckedChanged += new System.EventHandler(this.rdoCustomerCode_CheckedChanged);
            // 
            // rdoCustomerName
            // 
            this.rdoCustomerName.AutoSize = true;
            this.rdoCustomerName.Location = new System.Drawing.Point(347, 25);
            this.rdoCustomerName.Name = "rdoCustomerName";
            this.rdoCustomerName.Size = new System.Drawing.Size(121, 29);
            this.rdoCustomerName.TabIndex = 1;
            this.rdoCustomerName.Text = "Patient Name";
            this.rdoCustomerName.UseVisualStyleBackColor = true;
            this.rdoCustomerName.CheckedChanged += new System.EventHandler(this.rdoCustomerName_CheckedChanged);
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
            this.btnClearSearch.Location = new System.Drawing.Point(512, 55);
            this.btnClearSearch.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnClearSearch.Name = "btnClearSearch";
            this.btnClearSearch.Size = new System.Drawing.Size(75, 46);
            this.btnClearSearch.TabIndex = 6;
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
            this.btnSearch.Location = new System.Drawing.Point(414, 55);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 46);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(130, 66);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(262, 32);
            this.txtSearch.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cboMemberType);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.groupBox2.Location = new System.Drawing.Point(9, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(395, 56);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // cboMemberType
            // 
            this.cboMemberType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboMemberType.FormattingEnabled = true;
            this.cboMemberType.Location = new System.Drawing.Point(126, 18);
            this.cboMemberType.Name = "cboMemberType";
            this.cboMemberType.Size = new System.Drawing.Size(181, 33);
            this.cboMemberType.TabIndex = 1;
            this.cboMemberType.SelectedIndexChanged += new System.EventHandler(this.cboMemberType_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 21);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(105, 25);
            this.label10.TabIndex = 0;
            this.label10.Text = "Member Type";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgvCustomerList);
            this.groupBox3.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.groupBox3.Location = new System.Drawing.Point(7, 172);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1160, 460);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Patient List";
            // 
            // dgvCustomerList
            // 
            this.dgvCustomerList.AllowUserToAddRows = false;
            this.dgvCustomerList.AllowUserToResizeColumns = false;
            this.dgvCustomerList.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCustomerList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCustomerList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCustomerList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column6,
            this.colMemId,
            this.colPatientID,
            this.ColPatientName,
            this.ColDateofBirth,
            this.ColAge,
            this.ColAddress,
            this.Column2,
            this.Column5,
            this.Column8,
            this.ColViewDetail,
            this.ColViewPackage,
            this.ColEdit,
            this.ColDelete});
            this.dgvCustomerList.Location = new System.Drawing.Point(6, 20);
            this.dgvCustomerList.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.dgvCustomerList.Name = "dgvCustomerList";
            this.dgvCustomerList.RowHeadersWidth = 51;
            this.dgvCustomerList.Size = new System.Drawing.Size(1154, 431);
            this.dgvCustomerList.TabIndex = 4;
            this.dgvCustomerList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCustomerList_CellClick);
            this.dgvCustomerList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvCustomerList_DataBindingComplete);
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "Id";
            this.Column6.HeaderText = "ID";
            this.Column6.MinimumWidth = 6;
            this.Column6.Name = "Column6";
            this.Column6.Visible = false;
            this.Column6.Width = 125;
            // 
            // colMemId
            // 
            this.colMemId.DataPropertyName = "VIPMemberId";
            this.colMemId.HeaderText = "Member Card Id";
            this.colMemId.MinimumWidth = 6;
            this.colMemId.Name = "colMemId";
            this.colMemId.ReadOnly = true;
            this.colMemId.Width = 125;
            // 
            // colPatientID
            // 
            this.colPatientID.DataPropertyName = "CustomerCode";
            this.colPatientID.HeaderText = "Patient ID";
            this.colPatientID.MinimumWidth = 6;
            this.colPatientID.Name = "colPatientID";
            this.colPatientID.Width = 125;
            // 
            // ColPatientName
            // 
            this.ColPatientName.DataPropertyName = "Name";
            this.ColPatientName.HeaderText = "Patient Name";
            this.ColPatientName.MinimumWidth = 6;
            this.ColPatientName.Name = "ColPatientName";
            this.ColPatientName.Width = 210;
            // 
            // ColDateofBirth
            // 
            this.ColDateofBirth.DataPropertyName = "Birthday";
            dataGridViewCellStyle2.Format = " dd-MM-yyyy";
            dataGridViewCellStyle2.NullValue = null;
            this.ColDateofBirth.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColDateofBirth.HeaderText = "Date of Birth";
            this.ColDateofBirth.MinimumWidth = 6;
            this.ColDateofBirth.Name = "ColDateofBirth";
            this.ColDateofBirth.Width = 125;
            // 
            // ColAge
            // 
            this.ColAge.DataPropertyName = "Age";
            this.ColAge.HeaderText = "Age (Years Old)";
            this.ColAge.MinimumWidth = 6;
            this.ColAge.Name = "ColAge";
            this.ColAge.Width = 125;
            // 
            // ColAddress
            // 
            this.ColAddress.DataPropertyName = "Address";
            this.ColAddress.HeaderText = "Address";
            this.ColAddress.MinimumWidth = 6;
            this.ColAddress.Name = "ColAddress";
            this.ColAddress.Width = 125;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "PhoneNumber";
            this.Column2.HeaderText = "Phone Number";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.Width = 120;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "Email";
            this.Column5.HeaderText = "Email";
            this.Column5.MinimumWidth = 6;
            this.Column5.Name = "Column5";
            this.Column5.Width = 150;
            // 
            // Column8
            // 
            this.Column8.DataPropertyName = "NRC";
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Zawgyi-One", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Column8.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column8.HeaderText = "NRIC";
            this.Column8.MinimumWidth = 6;
            this.Column8.Name = "Column8";
            this.Column8.Width = 120;
            // 
            // ColViewDetail
            // 
            this.ColViewDetail.HeaderText = "";
            this.ColViewDetail.MinimumWidth = 6;
            this.ColViewDetail.Name = "ColViewDetail";
            this.ColViewDetail.Text = "Detail";
            this.ColViewDetail.UseColumnTextForLinkValue = true;
            this.ColViewDetail.VisitedLinkColor = System.Drawing.Color.Blue;
            this.ColViewDetail.Width = 80;
            // 
            // ColViewPackage
            // 
            this.ColViewPackage.HeaderText = "";
            this.ColViewPackage.MinimumWidth = 6;
            this.ColViewPackage.Name = "ColViewPackage";
            this.ColViewPackage.Text = "View Package";
            this.ColViewPackage.UseColumnTextForLinkValue = true;
            this.ColViewPackage.Width = 125;
            // 
            // ColEdit
            // 
            this.ColEdit.HeaderText = "";
            this.ColEdit.MinimumWidth = 6;
            this.ColEdit.Name = "ColEdit";
            this.ColEdit.Text = "Edit";
            this.ColEdit.UseColumnTextForLinkValue = true;
            this.ColEdit.Width = 80;
            // 
            // ColDelete
            // 
            this.ColDelete.HeaderText = "";
            this.ColDelete.MinimumWidth = 6;
            this.ColDelete.Name = "ColDelete";
            this.ColDelete.Text = "Delete";
            this.ColDelete.UseColumnTextForLinkValue = true;
            this.ColDelete.Width = 80;
            // 
            // CustomerList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1172, 632);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnAddNewCustomer);
            this.Font = new System.Drawing.Font("Zawgyi-One", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "CustomerList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Patient List";
            this.Load += new System.EventHandler(this.CustomerList_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomerList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAddNewCustomer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnClearSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.RadioButton rdoMemberCardNo;
        private System.Windows.Forms.RadioButton rdoCustomerName;
        private System.Windows.Forms.Label lblSearchTitle;
        private System.Windows.Forms.RadioButton rdoBirthDate;
        private System.Windows.Forms.DateTimePicker dtpBirthday;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cboMemberType;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dgvCustomerList;
        private System.Windows.Forms.RadioButton rdoCustomerCode;
        private System.Windows.Forms.RadioButton rdoBirth_Day;
        private System.Windows.Forms.RadioButton rdoBirthMonth;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPatientID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColPatientName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColDateofBirth;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColAge;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewLinkColumn ColViewDetail;
        private System.Windows.Forms.DataGridViewLinkColumn ColViewPackage;
        private System.Windows.Forms.DataGridViewLinkColumn ColEdit;
        private System.Windows.Forms.DataGridViewLinkColumn ColDelete;
    }
}