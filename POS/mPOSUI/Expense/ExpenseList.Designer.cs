namespace POS
{
    partial class ExpenseList
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExpenseList));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvExpenseList = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtTotalExpenseAmt = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboExpenseCag = new System.Windows.Forms.ComboBox();
            this.lblExpense = new System.Windows.Forms.Label();
            this.lblsName = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdoApproved = new System.Windows.Forms.RadioButton();
            this.rdoPending = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cboshoplist = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnaddexpense = new System.Windows.Forms.Button();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalExpenseAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSettlementAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExpenseCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiscountAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEdit = new System.Windows.Forms.DataGridViewLinkColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExpenseList)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvExpenseList);
            this.groupBox1.Location = new System.Drawing.Point(4, 147);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.groupBox1.Size = new System.Drawing.Size(1162, 524);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Expense List";
            // 
            // dgvExpenseList
            // 
            this.dgvExpenseList.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvExpenseList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvExpenseList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvExpenseList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDate,
            this.Column3,
            this.colTotalExpenseAmount,
            this.colSettlementAmount,
            this.colExpenseCategory,
            this.colDiscountAmt,
            this.colEdit,
            this.Column7,
            this.Column6,
            this.Column8});
            this.dgvExpenseList.Location = new System.Drawing.Point(16, 27);
            this.dgvExpenseList.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.dgvExpenseList.Name = "dgvExpenseList";
            this.dgvExpenseList.Size = new System.Drawing.Size(1146, 487);
            this.dgvExpenseList.TabIndex = 0;
            this.dgvExpenseList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvExpenseList_CellClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtTotalExpenseAmt);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cboExpenseCag);
            this.groupBox2.Controls.Add(this.lblExpense);
            this.groupBox2.Controls.Add(this.lblsName);
            this.groupBox2.Location = new System.Drawing.Point(4, 75);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(938, 66);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "By Supplier";
            // 
            // txtTotalExpenseAmt
            // 
            this.txtTotalExpenseAmt.AutoSize = true;
            this.txtTotalExpenseAmt.Location = new System.Drawing.Point(716, 29);
            this.txtTotalExpenseAmt.Name = "txtTotalExpenseAmt";
            this.txtTotalExpenseAmt.Size = new System.Drawing.Size(20, 25);
            this.txtTotalExpenseAmt.TabIndex = 4;
            this.txtTotalExpenseAmt.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(516, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(176, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Total Expense Amount  :";
            // 
            // cboExpenseCag
            // 
            this.cboExpenseCag.FormattingEnabled = true;
            this.cboExpenseCag.Location = new System.Drawing.Point(178, 23);
            this.cboExpenseCag.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.cboExpenseCag.Name = "cboExpenseCag";
            this.cboExpenseCag.Size = new System.Drawing.Size(180, 33);
            this.cboExpenseCag.TabIndex = 2;
            this.cboExpenseCag.SelectedIndexChanged += new System.EventHandler(this.cboExpCag_SelectedValueChanged);
            // 
            // lblExpense
            // 
            this.lblExpense.AutoSize = true;
            this.lblExpense.Location = new System.Drawing.Point(138, 26);
            this.lblExpense.Name = "lblExpense";
            this.lblExpense.Size = new System.Drawing.Size(17, 25);
            this.lblExpense.TabIndex = 1;
            this.lblExpense.Text = "-";
            // 
            // lblsName
            // 
            this.lblsName.AutoSize = true;
            this.lblsName.Location = new System.Drawing.Point(12, 26);
            this.lblsName.Name = "lblsName";
            this.lblsName.Size = new System.Drawing.Size(136, 25);
            this.lblsName.TabIndex = 0;
            this.lblsName.Text = "Expense Category:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rdoApproved);
            this.groupBox3.Controls.Add(this.rdoPending);
            this.groupBox3.Location = new System.Drawing.Point(4, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(304, 58);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "By Status";
            // 
            // rdoApproved
            // 
            this.rdoApproved.AutoSize = true;
            this.rdoApproved.Location = new System.Drawing.Point(142, 24);
            this.rdoApproved.Name = "rdoApproved";
            this.rdoApproved.Size = new System.Drawing.Size(95, 29);
            this.rdoApproved.TabIndex = 1;
            this.rdoApproved.TabStop = true;
            this.rdoApproved.Text = "Approved";
            this.rdoApproved.UseVisualStyleBackColor = true;
            this.rdoApproved.CheckedChanged += new System.EventHandler(this.rdoPending_CheckedChanged);
            // 
            // rdoPending
            // 
            this.rdoPending.AutoSize = true;
            this.rdoPending.Checked = true;
            this.rdoPending.Location = new System.Drawing.Point(16, 24);
            this.rdoPending.Name = "rdoPending";
            this.rdoPending.Size = new System.Drawing.Size(83, 29);
            this.rdoPending.TabIndex = 0;
            this.rdoPending.TabStop = true;
            this.rdoPending.Text = "Pending";
            this.rdoPending.UseVisualStyleBackColor = true;
            this.rdoPending.CheckedChanged += new System.EventHandler(this.rdoPending_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dtTo);
            this.groupBox4.Controls.Add(this.dtFrom);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Location = new System.Drawing.Point(314, 13);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox4.Size = new System.Drawing.Size(413, 58);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "By Period";
            // 
            // dtTo
            // 
            this.dtTo.CustomFormat = "dd-MM-yyyy";
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtTo.Location = new System.Drawing.Point(257, 23);
            this.dtTo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(126, 32);
            this.dtTo.TabIndex = 3;
            this.dtTo.ValueChanged += new System.EventHandler(this.dtFrom_ValueChanged);
            // 
            // dtFrom
            // 
            this.dtFrom.CustomFormat = "dd-MM-yyyy";
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtFrom.Location = new System.Drawing.Point(69, 23);
            this.dtFrom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(126, 32);
            this.dtFrom.TabIndex = 1;
            this.dtFrom.ValueChanged += new System.EventHandler(this.dtFrom_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(216, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "To";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 25);
            this.label4.TabIndex = 0;
            this.label4.Text = "From";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cboshoplist);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Location = new System.Drawing.Point(742, 13);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(359, 58);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "By Shop";
            // 
            // cboshoplist
            // 
            this.cboshoplist.FormattingEnabled = true;
            this.cboshoplist.Location = new System.Drawing.Point(128, 20);
            this.cboshoplist.Name = "cboshoplist";
            this.cboshoplist.Size = new System.Drawing.Size(200, 33);
            this.cboshoplist.TabIndex = 1;
            this.cboshoplist.SelectedIndexChanged += new System.EventHandler(this.cboshoplist_selectedvaluechanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Shop Name";
            // 
            // btnaddexpense
            // 
            this.btnaddexpense.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnaddexpense.FlatAppearance.BorderSize = 0;
            this.btnaddexpense.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnaddexpense.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnaddexpense.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnaddexpense.Image = global::POS.Properties.Resources.Asset_3;
            this.btnaddexpense.Location = new System.Drawing.Point(974, 87);
            this.btnaddexpense.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnaddexpense.Name = "btnaddexpense";
            this.btnaddexpense.Size = new System.Drawing.Size(127, 54);
            this.btnaddexpense.TabIndex = 6;
            this.btnaddexpense.UseVisualStyleBackColor = true;
            this.btnaddexpense.Click += new System.EventHandler(this.btnaddexpense_Click_1);
            // 
            // colDate
            // 
            this.colDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colDate.DataPropertyName = "ExpenseDate";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Format = "dd-MM-yyyy";
            dataGridViewCellStyle2.NullValue = null;
            this.colDate.DefaultCellStyle = dataGridViewCellStyle2;
            this.colDate.HeaderText = "Expense Date";
            this.colDate.Name = "colDate";
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column3.DataPropertyName = "ExpenseNo";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column3.HeaderText = "Expense No.";
            this.Column3.Name = "Column3";
            this.Column3.Width = 110;
            // 
            // colTotalExpenseAmount
            // 
            this.colTotalExpenseAmount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colTotalExpenseAmount.DataPropertyName = "TotalExpenseAmount";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N0";
            dataGridViewCellStyle4.NullValue = null;
            this.colTotalExpenseAmount.DefaultCellStyle = dataGridViewCellStyle4;
            this.colTotalExpenseAmount.HeaderText = "Total Expense Amount";
            this.colTotalExpenseAmount.Name = "colTotalExpenseAmount";
            this.colTotalExpenseAmount.Width = 110;
            // 
            // colSettlementAmount
            // 
            this.colSettlementAmount.DataPropertyName = "CreatedUser";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colSettlementAmount.DefaultCellStyle = dataGridViewCellStyle5;
            this.colSettlementAmount.HeaderText = "Created User";
            this.colSettlementAmount.Name = "colSettlementAmount";
            this.colSettlementAmount.Width = 150;
            // 
            // colExpenseCategory
            // 
            this.colExpenseCategory.DataPropertyName = "ExpenseCag";
            this.colExpenseCategory.HeaderText = "Expense Category";
            this.colExpenseCategory.Name = "colExpenseCategory";
            this.colExpenseCategory.ReadOnly = true;
            // 
            // colDiscountAmt
            // 
            this.colDiscountAmt.DataPropertyName = "Comment";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colDiscountAmt.DefaultCellStyle = dataGridViewCellStyle6;
            this.colDiscountAmt.HeaderText = "Comment";
            this.colDiscountAmt.Name = "colDiscountAmt";
            this.colDiscountAmt.ReadOnly = true;
            this.colDiscountAmt.Width = 150;
            // 
            // colEdit
            // 
            this.colEdit.HeaderText = "";
            this.colEdit.Name = "colEdit";
            this.colEdit.Text = "Edit";
            this.colEdit.UseColumnTextForLinkValue = true;
            // 
            // Column7
            // 
            this.Column7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column7.HeaderText = "";
            this.Column7.Name = "Column7";
            this.Column7.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column7.Text = "ViewDetail";
            this.Column7.UseColumnTextForLinkValue = true;
            this.Column7.Width = 90;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "";
            this.Column6.Name = "Column6";
            this.Column6.Text = "Approved";
            this.Column6.UseColumnTextForLinkValue = true;
            // 
            // Column8
            // 
            this.Column8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column8.HeaderText = "";
            this.Column8.Name = "Column8";
            this.Column8.Text = "Delete";
            this.Column8.UseColumnTextForLinkValue = true;
            this.Column8.Width = 80;
            // 
            // ExpenseList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1168, 676);
            this.Controls.Add(this.btnaddexpense);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "ExpenseList";
            this.Text = "Expense List";
            this.Load += new System.EventHandler(this.ExpenseListBySupplier_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvExpenseList)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvExpenseList;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboExpenseCag;
        private System.Windows.Forms.Label lblExpense;
        private System.Windows.Forms.Label lblsName;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdoApproved;
        private System.Windows.Forms.RadioButton rdoPending;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label txtTotalExpenseAmt;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox cboshoplist;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnaddexpense;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalExpenseAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSettlementAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExpenseCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiscountAmt;
        private System.Windows.Forms.DataGridViewLinkColumn colEdit;
        private System.Windows.Forms.DataGridViewLinkColumn Column7;
        private System.Windows.Forms.DataGridViewLinkColumn Column6;
        private System.Windows.Forms.DataGridViewLinkColumn Column8;
    }
}