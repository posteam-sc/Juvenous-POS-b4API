namespace POS
{
    partial class CancelTransaction
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CancelTransaction));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvPackageList = new System.Windows.Forms.DataGridView();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvCancelledList = new System.Windows.Forms.DataGridView();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItmName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCancelledQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCancelledAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblMainTransaction = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblPatientName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblCancelledAmount = new System.Windows.Forms.Label();
            this.colProductCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnitPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiscountPercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFrequency = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUsedQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFOC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTransactionId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTransactionDetailID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConsignmentPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCancel = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colCancelled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPackageList)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCancelledList)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvPackageList);
            this.groupBox2.Controls.Add(this.btnSubmit);
            this.groupBox2.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(20, 71);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(1220, 302);
            this.groupBox2.TabIndex = 39;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Package List";
            // 
            // dgvPackageList
            // 
            this.dgvPackageList.AllowUserToAddRows = false;
            this.dgvPackageList.AllowUserToDeleteRows = false;
            this.dgvPackageList.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPackageList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPackageList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPackageList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colProductCode,
            this.colItemName,
            this.colQty,
            this.colTax,
            this.colUnitPrice,
            this.colDiscountPercent,
            this.colCost,
            this.colFrequency,
            this.colTotalQty,
            this.colUsedQty,
            this.colFOC,
            this.colTransactionId,
            this.colTransactionDetailID,
            this.colConsignmentPrice,
            this.colCancel,
            this.colCancelled});
            this.dgvPackageList.Location = new System.Drawing.Point(11, 33);
            this.dgvPackageList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvPackageList.MultiSelect = false;
            this.dgvPackageList.Name = "dgvPackageList";
            this.dgvPackageList.RowHeadersVisible = false;
            this.dgvPackageList.Size = new System.Drawing.Size(1204, 249);
            this.dgvPackageList.TabIndex = 20;
            this.dgvPackageList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPackageList_CellClick);
            this.dgvPackageList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvPackageList_DataBindingComplete);
            // 
            // btnSubmit
            // 
            this.btnSubmit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnSubmit.FlatAppearance.BorderSize = 0;
            this.btnSubmit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnSubmit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmit.Image = global::POS.Properties.Resources.refund_big;
            this.btnSubmit.Location = new System.Drawing.Point(668, 386);
            this.btnSubmit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(103, 32);
            this.btnSubmit.TabIndex = 18;
            this.btnSubmit.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvCancelledList);
            this.groupBox1.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(20, 395);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(1111, 182);
            this.groupBox1.TabIndex = 38;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cancelled Package List";
            // 
            // dgvCancelledList
            // 
            this.dgvCancelledList.AllowUserToAddRows = false;
            this.dgvCancelledList.AllowUserToDeleteRows = false;
            this.dgvCancelledList.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCancelledList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvCancelledList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCancelledList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDate,
            this.colTime,
            this.colPCode,
            this.colItmName,
            this.colCancelledQty,
            this.colCancelledAmount});
            this.dgvCancelledList.Location = new System.Drawing.Point(11, 31);
            this.dgvCancelledList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvCancelledList.Name = "dgvCancelledList";
            this.dgvCancelledList.RowHeadersVisible = false;
            this.dgvCancelledList.Size = new System.Drawing.Size(1093, 132);
            this.dgvCancelledList.TabIndex = 26;
            this.dgvCancelledList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvCancelledList_DataBindingComplete);
            // 
            // colDate
            // 
            dataGridViewCellStyle10.Format = "dd-MM-yyyy";
            dataGridViewCellStyle10.NullValue = null;
            this.colDate.DefaultCellStyle = dataGridViewCellStyle10;
            this.colDate.HeaderText = "Date";
            this.colDate.Name = "colDate";
            this.colDate.ReadOnly = true;
            // 
            // colTime
            // 
            this.colTime.HeaderText = "Time";
            this.colTime.Name = "colTime";
            // 
            // colPCode
            // 
            this.colPCode.HeaderText = "Product Code";
            this.colPCode.Name = "colPCode";
            // 
            // colItmName
            // 
            this.colItmName.HeaderText = "Item Name";
            this.colItmName.Name = "colItmName";
            this.colItmName.Width = 250;
            // 
            // colCancelledQty
            // 
            this.colCancelledQty.HeaderText = "Cancelled Qty";
            this.colCancelledQty.Name = "colCancelledQty";
            // 
            // colCancelledAmount
            // 
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colCancelledAmount.DefaultCellStyle = dataGridViewCellStyle11;
            this.colCancelledAmount.HeaderText = "Cancelled Amount";
            this.colCancelledAmount.Name = "colCancelledAmount";
            this.colCancelledAmount.ReadOnly = true;
            // 
            // lblMainTransaction
            // 
            this.lblMainTransaction.AutoSize = true;
            this.lblMainTransaction.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainTransaction.Location = new System.Drawing.Point(168, 43);
            this.lblMainTransaction.Name = "lblMainTransaction";
            this.lblMainTransaction.Size = new System.Drawing.Size(17, 25);
            this.lblMainTransaction.TabIndex = 36;
            this.lblMainTransaction.Text = "-";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.Location = new System.Drawing.Point(1107, 43);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(17, 25);
            this.lblTime.TabIndex = 34;
            this.lblTime.Text = "-";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.Location = new System.Drawing.Point(1107, 12);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(17, 25);
            this.lblDate.TabIndex = 33;
            this.lblDate.Text = "-";
            // 
            // lblPatientName
            // 
            this.lblPatientName.AutoSize = true;
            this.lblPatientName.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPatientName.Location = new System.Drawing.Point(135, 11);
            this.lblPatientName.Name = "lblPatientName";
            this.lblPatientName.Size = new System.Drawing.Size(17, 25);
            this.lblPatientName.TabIndex = 32;
            this.lblPatientName.Text = "-";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(1036, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 25);
            this.label3.TabIndex = 31;
            this.label3.Text = "Time :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1036, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 25);
            this.label2.TabIndex = 30;
            this.label2.Text = "Date :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(17, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 25);
            this.label1.TabIndex = 29;
            this.label1.Text = "Patient Name :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(19, 43);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(133, 25);
            this.label7.TabIndex = 35;
            this.label7.Text = "Main Transaction :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(24, 606);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(194, 25);
            this.label4.TabIndex = 40;
            this.label4.Text = "Total Cancelled Amount:";
            // 
            // lblCancelledAmount
            // 
            this.lblCancelledAmount.AutoSize = true;
            this.lblCancelledAmount.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCancelledAmount.Location = new System.Drawing.Point(224, 606);
            this.lblCancelledAmount.Name = "lblCancelledAmount";
            this.lblCancelledAmount.Size = new System.Drawing.Size(18, 25);
            this.lblCancelledAmount.TabIndex = 41;
            this.lblCancelledAmount.Text = "-";
            // 
            // colProductCode
            // 
            this.colProductCode.HeaderText = "Product Code";
            this.colProductCode.Name = "colProductCode";
            this.colProductCode.Width = 80;
            // 
            // colItemName
            // 
            this.colItemName.HeaderText = "Item Name";
            this.colItemName.Name = "colItemName";
            this.colItemName.Width = 180;
            // 
            // colQty
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colQty.DefaultCellStyle = dataGridViewCellStyle2;
            this.colQty.HeaderText = "Qty";
            this.colQty.Name = "colQty";
            this.colQty.Width = 45;
            // 
            // colTax
            // 
            this.colTax.HeaderText = "Tax";
            this.colTax.Name = "colTax";
            this.colTax.Width = 55;
            // 
            // colUnitPrice
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colUnitPrice.DefaultCellStyle = dataGridViewCellStyle3;
            this.colUnitPrice.HeaderText = "Unit Price";
            this.colUnitPrice.Name = "colUnitPrice";
            this.colUnitPrice.ReadOnly = true;
            this.colUnitPrice.Width = 75;
            // 
            // colDiscountPercent
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colDiscountPercent.DefaultCellStyle = dataGridViewCellStyle4;
            this.colDiscountPercent.HeaderText = "Discount Percent";
            this.colDiscountPercent.Name = "colDiscountPercent";
            this.colDiscountPercent.ReadOnly = true;
            this.colDiscountPercent.Width = 65;
            // 
            // colCost
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colCost.DefaultCellStyle = dataGridViewCellStyle5;
            this.colCost.HeaderText = "Cost";
            this.colCost.Name = "colCost";
            this.colCost.ReadOnly = true;
            // 
            // colFrequency
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colFrequency.DefaultCellStyle = dataGridViewCellStyle6;
            this.colFrequency.HeaderText = "Frequency";
            this.colFrequency.Name = "colFrequency";
            this.colFrequency.Width = 70;
            // 
            // colTotalQty
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colTotalQty.DefaultCellStyle = dataGridViewCellStyle7;
            this.colTotalQty.HeaderText = "Total Offset Qty";
            this.colTotalQty.Name = "colTotalQty";
            this.colTotalQty.ReadOnly = true;
            this.colTotalQty.Width = 65;
            // 
            // colUsedQty
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colUsedQty.DefaultCellStyle = dataGridViewCellStyle8;
            this.colUsedQty.HeaderText = "Used Qty";
            this.colUsedQty.Name = "colUsedQty";
            this.colUsedQty.Width = 55;
            // 
            // colFOC
            // 
            this.colFOC.HeaderText = "IsFOC";
            this.colFOC.Name = "colFOC";
            this.colFOC.Width = 50;
            // 
            // colTransactionId
            // 
            this.colTransactionId.HeaderText = "ID";
            this.colTransactionId.Name = "colTransactionId";
            this.colTransactionId.Visible = false;
            // 
            // colTransactionDetailID
            // 
            this.colTransactionDetailID.HeaderText = "TransactionDetailID";
            this.colTransactionDetailID.Name = "colTransactionDetailID";
            this.colTransactionDetailID.Visible = false;
            // 
            // colConsignmentPrice
            // 
            this.colConsignmentPrice.HeaderText = "Consignment Price";
            this.colConsignmentPrice.Name = "colConsignmentPrice";
            this.colConsignmentPrice.ReadOnly = true;
            this.colConsignmentPrice.Visible = false;
            // 
            // colCancel
            // 
            this.colCancel.HeaderText = "";
            this.colCancel.Name = "colCancel";
            this.colCancel.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCancel.Text = "Cancel";
            this.colCancel.UseColumnTextForLinkValue = true;
            this.colCancel.Width = 60;
            // 
            // colCancelled
            // 
            this.colCancelled.HeaderText = "Cancelled";
            this.colCancelled.Name = "colCancelled";
            this.colCancelled.Visible = false;
            // 
            // CancelTransaction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 654);
            this.Controls.Add(this.lblCancelledAmount);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblMainTransaction);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblPatientName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label7);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "CancelTransaction";
            this.Text = "Cancel Transaction";
            this.Load += new System.EventHandler(this.CancelTransaction_Load);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPackageList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCancelledList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvPackageList;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvCancelledList;
        private System.Windows.Forms.Label lblMainTransaction;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblPatientName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblCancelledAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItmName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCancelledQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCancelledAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProductCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTax;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnitPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiscountPercent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFrequency;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUsedQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFOC;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTransactionId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTransactionDetailID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConsignmentPrice;
        private System.Windows.Forms.DataGridViewLinkColumn colCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCancelled;
    }
}