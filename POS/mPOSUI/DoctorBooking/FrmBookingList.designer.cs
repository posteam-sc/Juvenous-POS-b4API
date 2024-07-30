namespace POS.mPOSUI.DoctorBooking
{
    partial class FrmBookingList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBookingList));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtDate = new System.Windows.Forms.DateTimePicker();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvBookingList = new System.Windows.Forms.DataGridView();
            this.cboPatient = new System.Windows.Forms.ComboBox();
            this.rdoConfirm = new System.Windows.Forms.RadioButton();
            this.rdoBooking = new System.Windows.Forms.RadioButton();
            this.DoctorBookingId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ToDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FromTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ToTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Day = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BookingCase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBookingList)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 46);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Patient/Doctor Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(499, 46);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "Date:";
            // 
            // dtDate
            // 
            this.dtDate.CustomFormat = "dd-MM-yyyy";
            this.dtDate.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtDate.Location = new System.Drawing.Point(587, 39);
            this.dtDate.Margin = new System.Windows.Forms.Padding(4);
            this.dtDate.Name = "dtDate";
            this.dtDate.Size = new System.Drawing.Size(240, 32);
            this.dtDate.TabIndex = 2;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(202)))), ((int)(((byte)(125)))));
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Image = global::POS.Properties.Resources.refresh_small;
            this.btnRefresh.Location = new System.Drawing.Point(1044, 32);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4, 7, 4, 7);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 57);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
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
            this.btnSearch.Location = new System.Drawing.Point(936, 32);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 7, 4, 7);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 57);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvBookingList);
            this.groupBox1.Location = new System.Drawing.Point(21, 134);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(1584, 448);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Booking List Recrod(s):";
            // 
            // dgvBookingList
            // 
            this.dgvBookingList.AllowUserToAddRows = false;
            this.dgvBookingList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBookingList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DoctorBookingId,
            this.colName,
            this.Column2,
            this.Column7,
            this.Column1,
            this.ToDate,
            this.FromTime,
            this.ToTime,
            this.Day,
            this.BookingCase,
            this.Column3,
            this.Column9,
            this.Column10});
            this.dgvBookingList.Location = new System.Drawing.Point(8, 43);
            this.dgvBookingList.Margin = new System.Windows.Forms.Padding(4);
            this.dgvBookingList.Name = "dgvBookingList";
            this.dgvBookingList.Size = new System.Drawing.Size(1551, 398);
            this.dgvBookingList.TabIndex = 0;
            this.dgvBookingList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBookingList_CellClick);
            // 
            // cboPatient
            // 
            this.cboPatient.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboPatient.FormattingEnabled = true;
            this.cboPatient.Location = new System.Drawing.Point(191, 46);
            this.cboPatient.Margin = new System.Windows.Forms.Padding(4);
            this.cboPatient.Name = "cboPatient";
            this.cboPatient.Size = new System.Drawing.Size(211, 33);
            this.cboPatient.TabIndex = 10;
            // 
            // rdoConfirm
            // 
            this.rdoConfirm.AutoSize = true;
            this.rdoConfirm.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoConfirm.Location = new System.Drawing.Point(436, 98);
            this.rdoConfirm.Margin = new System.Windows.Forms.Padding(4);
            this.rdoConfirm.Name = "rdoConfirm";
            this.rdoConfirm.Size = new System.Drawing.Size(75, 29);
            this.rdoConfirm.TabIndex = 11;
            this.rdoConfirm.Text = "Cancel";
            this.rdoConfirm.UseVisualStyleBackColor = true;
            this.rdoConfirm.CheckedChanged += new System.EventHandler(this.rdoConfirm_CheckedChanged);
            // 
            // rdoBooking
            // 
            this.rdoBooking.AutoSize = true;
            this.rdoBooking.Checked = true;
            this.rdoBooking.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoBooking.Location = new System.Drawing.Point(243, 100);
            this.rdoBooking.Margin = new System.Windows.Forms.Padding(4);
            this.rdoBooking.Name = "rdoBooking";
            this.rdoBooking.Size = new System.Drawing.Size(83, 29);
            this.rdoBooking.TabIndex = 12;
            this.rdoBooking.TabStop = true;
            this.rdoBooking.Text = "Booking";
            this.rdoBooking.UseVisualStyleBackColor = true;
            this.rdoBooking.CheckedChanged += new System.EventHandler(this.rdoBooking_CheckedChanged);
            // 
            // DoctorBookingId
            // 
            this.DoctorBookingId.DataPropertyName = "DoctorBookingId";
            this.DoctorBookingId.HeaderText = "id";
            this.DoctorBookingId.Name = "DoctorBookingId";
            this.DoctorBookingId.Visible = false;
            // 
            // colName
            // 
            this.colName.DataPropertyName = "PatientName";
            this.colName.HeaderText = "Patient Name";
            this.colName.Name = "colName";
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "DoctorName";
            this.Column2.HeaderText = "Doctor Name";
            this.Column2.Name = "Column2";
            // 
            // Column7
            // 
            this.Column7.DataPropertyName = "AssistNurseName";
            this.Column7.HeaderText = "Assistant Nurse";
            this.Column7.Name = "Column7";
            this.Column7.Width = 130;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "PackageName";
            this.Column1.HeaderText = "Package";
            this.Column1.Name = "Column1";
            // 
            // ToDate
            // 
            this.ToDate.DataPropertyName = "BookingDate";
            dataGridViewCellStyle1.Format = "dd-MM-yyyy";
            dataGridViewCellStyle1.NullValue = null;
            this.ToDate.DefaultCellStyle = dataGridViewCellStyle1;
            this.ToDate.HeaderText = "Date";
            this.ToDate.Name = "ToDate";
            this.ToDate.Width = 80;
            // 
            // FromTime
            // 
            this.FromTime.DataPropertyName = "FromTime";
            this.FromTime.HeaderText = "From Time ";
            this.FromTime.Name = "FromTime";
            this.FromTime.Width = 90;
            // 
            // ToTime
            // 
            this.ToTime.DataPropertyName = "ToTime";
            this.ToTime.HeaderText = "To Time";
            this.ToTime.Name = "ToTime";
            this.ToTime.Width = 80;
            // 
            // Day
            // 
            this.Day.DataPropertyName = "BookingDay";
            dataGridViewCellStyle2.Format = "dd-MM-yyyy";
            dataGridViewCellStyle2.NullValue = null;
            this.Day.DefaultCellStyle = dataGridViewCellStyle2;
            this.Day.HeaderText = "Day";
            this.Day.Name = "Day";
            this.Day.Width = 80;
            // 
            // BookingCase
            // 
            this.BookingCase.DataPropertyName = "BookingCase";
            this.BookingCase.HeaderText = "Booking Case";
            this.BookingCase.Name = "BookingCase";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "";
            this.Column3.Name = "Column3";
            this.Column3.Text = "Cancel";
            this.Column3.UseColumnTextForLinkValue = true;
            this.Column3.Width = 50;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "";
            this.Column9.Name = "Column9";
            this.Column9.Text = "Edit";
            this.Column9.UseColumnTextForLinkValue = true;
            // 
            // Column10
            // 
            this.Column10.HeaderText = "";
            this.Column10.Name = "Column10";
            this.Column10.Text = "Delete";
            this.Column10.UseColumnTextForLinkValue = true;
            // 
            // FrmBookingList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1624, 598);
            this.Controls.Add(this.rdoBooking);
            this.Controls.Add(this.rdoConfirm);
            this.Controls.Add(this.cboPatient);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.dtDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmBookingList";
            this.Text = "Booking List";
            this.Load += new System.EventHandler(this.FrmBookingList_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBookingList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtDate;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvBookingList;
        private System.Windows.Forms.ComboBox cboPatient;
        private System.Windows.Forms.RadioButton rdoConfirm;
        private System.Windows.Forms.RadioButton rdoBooking;
        private System.Windows.Forms.DataGridViewTextBoxColumn DoctorBookingId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ToDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn FromTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ToTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Day;
        private System.Windows.Forms.DataGridViewTextBoxColumn BookingCase;
        private System.Windows.Forms.DataGridViewLinkColumn Column3;
        private System.Windows.Forms.DataGridViewLinkColumn Column9;
        private System.Windows.Forms.DataGridViewLinkColumn Column10;
    }
}