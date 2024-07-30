namespace POS
{
    partial class ExpenseDeleteLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExpenseDeleteLog));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvExpenseList = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cboshoplist = new System.Windows.Forms.ComboBox();
            this.Shop = new System.Windows.Forms.Label();
            this.cboExpenseCag = new System.Windows.Forms.ComboBox();
            this.lblsName = new System.Windows.Forms.Label();
            this.txtTotalExpenseAmt = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalExpenseAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSettlementAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExpenseCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiscountAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExpenseList)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtTo);
            this.groupBox1.Controls.Add(this.dtFrom);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.groupBox1.Location = new System.Drawing.Point(13, 5);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(679, 57);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "By Period";
            // 
            // dtTo
            // 
            this.dtTo.CustomFormat = "dd-MM-yyyy";
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtTo.Location = new System.Drawing.Point(430, 22);
            this.dtTo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(190, 32);
            this.dtTo.TabIndex = 3;
            this.dtTo.ValueChanged += new System.EventHandler(this.dtTo_ValueChanged);
            // 
            // dtFrom
            // 
            this.dtFrom.CustomFormat = "dd-MM-yyyy";
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtFrom.Location = new System.Drawing.Point(102, 22);
            this.dtFrom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(202, 32);
            this.dtFrom.TabIndex = 1;
            this.dtFrom.ValueChanged += new System.EventHandler(this.dtFrom_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(374, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "To";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "From";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvExpenseList);
            this.groupBox2.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.groupBox2.Location = new System.Drawing.Point(5, 139);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(858, 492);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Expense Delete";
            // 
            // dgvExpenseList
            // 
            this.dgvExpenseList.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvExpenseList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvExpenseList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvExpenseList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.colDate,
            this.Column3,
            this.colTotalExpenseAmount,
            this.colSettlementAmount,
            this.colExpenseCategory,
            this.colDiscountAmt});
            this.dgvExpenseList.Location = new System.Drawing.Point(8, 28);
            this.dgvExpenseList.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.dgvExpenseList.Name = "dgvExpenseList";
            this.dgvExpenseList.Size = new System.Drawing.Size(833, 521);
            this.dgvExpenseList.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cboshoplist);
            this.groupBox3.Controls.Add(this.Shop);
            this.groupBox3.Controls.Add(this.cboExpenseCag);
            this.groupBox3.Controls.Add(this.lblsName);
            this.groupBox3.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.groupBox3.Location = new System.Drawing.Point(11, 69);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(723, 64);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "By Expense Category And Shop";
            // 
            // cboshoplist
            // 
            this.cboshoplist.FormattingEnabled = true;
            this.cboshoplist.Location = new System.Drawing.Point(497, 26);
            this.cboshoplist.Name = "cboshoplist";
            this.cboshoplist.Size = new System.Drawing.Size(200, 33);
            this.cboshoplist.TabIndex = 3;
            this.cboshoplist.SelectedIndexChanged += new System.EventHandler(this.cboshoplist_selectedindexchanged);
            // 
            // Shop
            // 
            this.Shop.AutoSize = true;
            this.Shop.Location = new System.Drawing.Point(428, 27);
            this.Shop.Name = "Shop";
            this.Shop.Size = new System.Drawing.Size(44, 25);
            this.Shop.TabIndex = 2;
            this.Shop.Text = "Shop";
            // 
            // cboExpenseCag
            // 
            this.cboExpenseCag.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.cboExpenseCag.FormattingEnabled = true;
            this.cboExpenseCag.Location = new System.Drawing.Point(171, 27);
            this.cboExpenseCag.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.cboExpenseCag.Name = "cboExpenseCag";
            this.cboExpenseCag.Size = new System.Drawing.Size(202, 33);
            this.cboExpenseCag.TabIndex = 1;
            this.cboExpenseCag.SelectedIndexChanged += new System.EventHandler(this.cboExpenseCag_SelectedIndexChanged);
            // 
            // lblsName
            // 
            this.lblsName.AutoSize = true;
            this.lblsName.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.lblsName.Location = new System.Drawing.Point(26, 27);
            this.lblsName.Name = "lblsName";
            this.lblsName.Size = new System.Drawing.Size(136, 25);
            this.lblsName.TabIndex = 0;
            this.lblsName.Text = "Expense Category:";
            // 
            // txtTotalExpenseAmt
            // 
            this.txtTotalExpenseAmt.AutoSize = true;
            this.txtTotalExpenseAmt.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.txtTotalExpenseAmt.Location = new System.Drawing.Point(212, 647);
            this.txtTotalExpenseAmt.Name = "txtTotalExpenseAmt";
            this.txtTotalExpenseAmt.Size = new System.Drawing.Size(20, 25);
            this.txtTotalExpenseAmt.TabIndex = 12;
            this.txtTotalExpenseAmt.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.label4.Location = new System.Drawing.Point(12, 647);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(176, 25);
            this.label4.TabIndex = 11;
            this.label4.Text = "Total Expense Amount  :";
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "Id";
            this.Column1.HeaderText = "Id";
            this.Column1.Name = "Column1";
            this.Column1.Visible = false;
            this.Column1.Width = 80;
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
            this.colDiscountAmt.Width = 200;
            // 
            // ExpenseDeleteLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(870, 676);
            this.Controls.Add(this.txtTotalExpenseAmt);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Zawgyi-One", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ExpenseDeleteLog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Expense Delete Log";
            this.Load += new System.EventHandler(this.ExpenseDeleteLog_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvExpenseList)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cboExpenseCag;
        private System.Windows.Forms.Label lblsName;
        private System.Windows.Forms.DataGridView dgvExpenseList;
        private System.Windows.Forms.Label txtTotalExpenseAmt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboshoplist;
        private System.Windows.Forms.Label Shop;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalExpenseAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSettlementAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExpenseCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiscountAmt;
    }
}