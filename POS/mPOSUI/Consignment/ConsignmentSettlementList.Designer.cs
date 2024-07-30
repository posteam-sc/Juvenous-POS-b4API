namespace POS
{
    partial class ConsignmentSettlementList
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsignmentSettlementList));
            this.dgvConSettlementList = new System.Windows.Forms.DataGridView();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTranDetailId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConsignmentNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConsignor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSettlementDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSettlementAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreatedUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFromSaleDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colToSaleDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colViewDetail = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colDelete = new System.Windows.Forms.DataGridViewLinkColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboYear = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cboMonth = new System.Windows.Forms.ComboBox();
            this.cboshoplist = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTotalSettlementAmount = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboConsignor = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConSettlementList)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvConSettlementList
            // 
            this.dgvConSettlementList.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvConSettlementList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvConSettlementList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvConSettlementList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column6,
            this.colTranDetailId,
            this.colConsignmentNo,
            this.colConsignor,
            this.colSettlementDate,
            this.colSettlementAmount,
            this.colCreatedUser,
            this.colFromSaleDate,
            this.colToSaleDate,
            this.colComment,
            this.colViewDetail,
            this.colDelete});
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvConSettlementList.DefaultCellStyle = dataGridViewCellStyle10;
            this.dgvConSettlementList.Location = new System.Drawing.Point(7, 128);
            this.dgvConSettlementList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvConSettlementList.Name = "dgvConSettlementList";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvConSettlementList.RowHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.dgvConSettlementList.Size = new System.Drawing.Size(1187, 503);
            this.dgvConSettlementList.TabIndex = 3;
            this.dgvConSettlementList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvConSettlementList_CellClick);
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "Id";
            this.Column6.HeaderText = "ID";
            this.Column6.Name = "Column6";
            this.Column6.Visible = false;
            // 
            // colTranDetailId
            // 
            this.colTranDetailId.DataPropertyName = "TranDetailID";
            this.colTranDetailId.HeaderText = "TransactionDetailId";
            this.colTranDetailId.Name = "colTranDetailId";
            this.colTranDetailId.ReadOnly = true;
            this.colTranDetailId.Visible = false;
            // 
            // colConsignmentNo
            // 
            this.colConsignmentNo.DataPropertyName = "ConsignmentNo";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.colConsignmentNo.DefaultCellStyle = dataGridViewCellStyle2;
            this.colConsignmentNo.HeaderText = "Consignment No.";
            this.colConsignmentNo.Name = "colConsignmentNo";
            this.colConsignmentNo.ReadOnly = true;
            this.colConsignmentNo.Width = 130;
            // 
            // colConsignor
            // 
            this.colConsignor.DataPropertyName = "Consignor";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.colConsignor.DefaultCellStyle = dataGridViewCellStyle3;
            this.colConsignor.HeaderText = "Consignor";
            this.colConsignor.Name = "colConsignor";
            this.colConsignor.ReadOnly = true;
            this.colConsignor.Width = 180;
            // 
            // colSettlementDate
            // 
            this.colSettlementDate.DataPropertyName = "SettlementDate";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            dataGridViewCellStyle4.Format = "dd-MM-yyyy";
            dataGridViewCellStyle4.NullValue = null;
            this.colSettlementDate.DefaultCellStyle = dataGridViewCellStyle4;
            this.colSettlementDate.HeaderText = "Settlement Date";
            this.colSettlementDate.Name = "colSettlementDate";
            this.colSettlementDate.ReadOnly = true;
            // 
            // colSettlementAmount
            // 
            this.colSettlementAmount.DataPropertyName = "SettlementAmount";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            dataGridViewCellStyle5.Format = "N0";
            dataGridViewCellStyle5.NullValue = null;
            this.colSettlementAmount.DefaultCellStyle = dataGridViewCellStyle5;
            this.colSettlementAmount.HeaderText = "Settlement Amount";
            this.colSettlementAmount.Name = "colSettlementAmount";
            this.colSettlementAmount.ReadOnly = true;
            this.colSettlementAmount.Width = 80;
            // 
            // colCreatedUser
            // 
            this.colCreatedUser.DataPropertyName = "CreatedUser";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.colCreatedUser.DefaultCellStyle = dataGridViewCellStyle6;
            this.colCreatedUser.HeaderText = "Created User";
            this.colCreatedUser.Name = "colCreatedUser";
            this.colCreatedUser.ReadOnly = true;
            this.colCreatedUser.Width = 150;
            // 
            // colFromSaleDate
            // 
            this.colFromSaleDate.DataPropertyName = "FromSaleDate";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            dataGridViewCellStyle7.Format = "dd - MMM - yyyy";
            dataGridViewCellStyle7.NullValue = null;
            this.colFromSaleDate.DefaultCellStyle = dataGridViewCellStyle7;
            this.colFromSaleDate.HeaderText = "From Sale Date";
            this.colFromSaleDate.Name = "colFromSaleDate";
            this.colFromSaleDate.ReadOnly = true;
            // 
            // colToSaleDate
            // 
            this.colToSaleDate.DataPropertyName = "ToSaleDate";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            dataGridViewCellStyle8.Format = "dd - MMM - yyyy";
            this.colToSaleDate.DefaultCellStyle = dataGridViewCellStyle8;
            this.colToSaleDate.HeaderText = "To Sale Date";
            this.colToSaleDate.Name = "colToSaleDate";
            this.colToSaleDate.ReadOnly = true;
            // 
            // colComment
            // 
            this.colComment.DataPropertyName = "Comment";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.colComment.DefaultCellStyle = dataGridViewCellStyle9;
            this.colComment.HeaderText = "Comment";
            this.colComment.Name = "colComment";
            this.colComment.ReadOnly = true;
            this.colComment.Width = 150;
            // 
            // colViewDetail
            // 
            this.colViewDetail.HeaderText = "";
            this.colViewDetail.Name = "colViewDetail";
            this.colViewDetail.Text = "View Detail";
            this.colViewDetail.UseColumnTextForLinkValue = true;
            this.colViewDetail.VisitedLinkColor = System.Drawing.Color.Blue;
            this.colViewDetail.Width = 80;
            // 
            // colDelete
            // 
            this.colDelete.HeaderText = "";
            this.colDelete.Name = "colDelete";
            this.colDelete.Text = "Delete";
            this.colDelete.UseColumnTextForLinkValue = true;
            this.colDelete.Width = 80;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboYear);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cboMonth);
            this.groupBox1.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.groupBox1.Location = new System.Drawing.Point(9, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(518, 65);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose Year And Month";
            // 
            // cboYear
            // 
            this.cboYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboYear.FormattingEnabled = true;
            this.cboYear.Location = new System.Drawing.Point(149, 27);
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(89, 23);
            this.cboYear.TabIndex = 1;
            this.cboYear.SelectedIndexChanged += new System.EventHandler(this.cboYear_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(108, 20);
            this.label6.TabIndex = 0;
            this.label6.Text = "Settlement Year :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(275, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "Settlement &Month:";
            // 
            // cboMonth
            // 
            this.cboMonth.DropDownWidth = 150;
            this.cboMonth.FormattingEnabled = true;
            this.cboMonth.Items.AddRange(new object[] {
            "Janauary",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"});
            this.cboMonth.Location = new System.Drawing.Point(395, 27);
            this.cboMonth.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboMonth.Name = "cboMonth";
            this.cboMonth.Size = new System.Drawing.Size(100, 28);
            this.cboMonth.TabIndex = 3;
            this.cboMonth.SelectedIndexChanged += new System.EventHandler(this.cboMonth_SelectedIndexChanged);
            // 
            // cboshoplist
            // 
            this.cboshoplist.FormattingEnabled = true;
            this.cboshoplist.Location = new System.Drawing.Point(422, 27);
            this.cboshoplist.Name = "cboshoplist";
            this.cboshoplist.Size = new System.Drawing.Size(200, 28);
            this.cboshoplist.TabIndex = 3;
            this.cboshoplist.SelectedIndexChanged += new System.EventHandler(this.cboshoplist_selectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(371, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 20);
            this.label5.TabIndex = 2;
            this.label5.Text = "Shop :";
            // 
            // txtTotalSettlementAmount
            // 
            this.txtTotalSettlementAmount.AutoSize = true;
            this.txtTotalSettlementAmount.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.txtTotalSettlementAmount.Location = new System.Drawing.Point(224, 16);
            this.txtTotalSettlementAmount.Name = "txtTotalSettlementAmount";
            this.txtTotalSettlementAmount.Size = new System.Drawing.Size(151, 20);
            this.txtTotalSettlementAmount.TabIndex = 2;
            this.txtTotalSettlementAmount.Text = "Total Settlement Amount";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(191, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(10, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "-";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.label2.Location = new System.Drawing.Point(20, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(151, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Total Settlement Amount";
            // 
            // cboConsignor
            // 
            this.cboConsignor.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.cboConsignor.FormattingEnabled = true;
            this.cboConsignor.Location = new System.Drawing.Point(158, 28);
            this.cboConsignor.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboConsignor.Name = "cboConsignor";
            this.cboConsignor.Size = new System.Drawing.Size(190, 28);
            this.cboConsignor.TabIndex = 1;
            this.cboConsignor.SelectedIndexChanged += new System.EventHandler(this.cboConsignor_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.label3.Location = new System.Drawing.Point(6, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "&Consignment Counter :";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtTotalSettlementAmount);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(9, 68);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(411, 53);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cboshoplist);
            this.groupBox3.Controls.Add(this.cboConsignor);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.groupBox3.Location = new System.Drawing.Point(533, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(661, 65);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "By Counter And Shop";
            // 
            // ConsignmentSettlementList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1203, 663);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.dgvConSettlementList);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConsignmentSettlementList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Consignment Settlement List";
            this.Load += new System.EventHandler(this.ConsignmentSettlementList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvConSettlementList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvConSettlementList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cboConsignor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label txtTotalSettlementAmount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboMonth;
        private System.Windows.Forms.ComboBox cboshoplist;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cboYear;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTranDetailId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConsignmentNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConsignor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSettlementDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSettlementAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreatedUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFromSaleDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colToSaleDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colComment;
        private System.Windows.Forms.DataGridViewLinkColumn colViewDetail;
        private System.Windows.Forms.DataGridViewLinkColumn colDelete;
    }
}