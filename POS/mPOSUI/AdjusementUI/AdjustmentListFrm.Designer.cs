namespace POS
{
    partial class AdjustmentListFrm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdjustmentListFrm));
            this.dgvAdjustmentList = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DamageQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStockOutQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AdjustmentDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ResponsibleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Approved = new System.Windows.Forms.DataGridViewLinkColumn();
            this.Delete = new System.Windows.Forms.DataGridViewLinkColumn();
            this.GpDamageList = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpk_fromDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.GpSearchByPeriod = new System.Windows.Forms.GroupBox();
            this.dtpk_toDate = new System.Windows.Forms.DateTimePicker();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cboAdjType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cboBrand = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdoAll = new System.Windows.Forms.RadioButton();
            this.rdNonConsignment = new System.Windows.Forms.RadioButton();
            this.rdConsignment = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.lblStockOut = new System.Windows.Forms.Label();
            this.lblStockIn = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdoApproved = new System.Windows.Forms.RadioButton();
            this.rdoPending = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdjustmentList)).BeginInit();
            this.GpDamageList.SuspendLayout();
            this.GpSearchByPeriod.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvAdjustmentList
            // 
            this.dgvAdjustmentList.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAdjustmentList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvAdjustmentList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAdjustmentList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.Column4,
            this.Column3,
            this.Column1,
            this.colName,
            this.Price,
            this.DamageQty,
            this.colStockOutQty,
            this.TotalCost,
            this.AdjustmentDateTime,
            this.ResponsibleName,
            this.colType,
            this.Column2,
            this.Approved,
            this.Delete});
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAdjustmentList.DefaultCellStyle = dataGridViewCellStyle12;
            this.dgvAdjustmentList.Location = new System.Drawing.Point(9, 26);
            this.dgvAdjustmentList.Name = "dgvAdjustmentList";
            this.dgvAdjustmentList.ReadOnly = true;
            this.dgvAdjustmentList.RowHeadersVisible = false;
            this.dgvAdjustmentList.Size = new System.Drawing.Size(1234, 506);
            this.dgvAdjustmentList.TabIndex = 0;
            this.dgvAdjustmentList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAdjustmentlist_CellClick);
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Visible = false;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "AdjustmentNo";
            this.Column4.HeaderText = "Adjustment No";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 150;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "ProductId";
            this.Column3.HeaderText = "ProductId";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Visible = false;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "ProductCode";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column1.HeaderText = "Product Code";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 150;
            // 
            // colName
            // 
            this.colName.DataPropertyName = "Name";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colName.DefaultCellStyle = dataGridViewCellStyle3;
            this.colName.HeaderText = "Product Name";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Width = 200;
            // 
            // Price
            // 
            this.Price.DataPropertyName = "Price";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Price.DefaultCellStyle = dataGridViewCellStyle4;
            this.Price.HeaderText = "Price";
            this.Price.Name = "Price";
            this.Price.ReadOnly = true;
            this.Price.Width = 70;
            // 
            // DamageQty
            // 
            this.DamageQty.DataPropertyName = "StockIn";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.DamageQty.DefaultCellStyle = dataGridViewCellStyle5;
            this.DamageQty.HeaderText = "Stock In";
            this.DamageQty.Name = "DamageQty";
            this.DamageQty.ReadOnly = true;
            this.DamageQty.Width = 50;
            // 
            // colStockOutQty
            // 
            this.colStockOutQty.DataPropertyName = "StockOut";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colStockOutQty.DefaultCellStyle = dataGridViewCellStyle6;
            this.colStockOutQty.HeaderText = "Stock Out";
            this.colStockOutQty.Name = "colStockOutQty";
            this.colStockOutQty.ReadOnly = true;
            this.colStockOutQty.Width = 50;
            // 
            // TotalCost
            // 
            this.TotalCost.DataPropertyName = "TotalCost";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.TotalCost.DefaultCellStyle = dataGridViewCellStyle7;
            this.TotalCost.HeaderText = "Total Cost";
            this.TotalCost.Name = "TotalCost";
            this.TotalCost.ReadOnly = true;
            // 
            // AdjustmentDateTime
            // 
            this.AdjustmentDateTime.DataPropertyName = "AdjustmentDateTime";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.Format = "dd-MM-yyyy";
            dataGridViewCellStyle8.NullValue = null;
            this.AdjustmentDateTime.DefaultCellStyle = dataGridViewCellStyle8;
            this.AdjustmentDateTime.HeaderText = "Adjustment Date";
            this.AdjustmentDateTime.Name = "AdjustmentDateTime";
            this.AdjustmentDateTime.ReadOnly = true;
            // 
            // ResponsibleName
            // 
            this.ResponsibleName.DataPropertyName = "ResponsibleName";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ResponsibleName.DefaultCellStyle = dataGridViewCellStyle9;
            this.ResponsibleName.HeaderText = "Respon:Person";
            this.ResponsibleName.Name = "ResponsibleName";
            this.ResponsibleName.ReadOnly = true;
            // 
            // colType
            // 
            this.colType.DataPropertyName = "Type";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colType.DefaultCellStyle = dataGridViewCellStyle10;
            this.colType.HeaderText = "Type";
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "Reason";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle11;
            this.Column2.HeaderText = "Reason";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Approved
            // 
            this.Approved.HeaderText = "";
            this.Approved.Name = "Approved";
            this.Approved.ReadOnly = true;
            this.Approved.Text = "Approved";
            this.Approved.UseColumnTextForLinkValue = true;
            this.Approved.Width = 90;
            // 
            // Delete
            // 
            this.Delete.HeaderText = "";
            this.Delete.Name = "Delete";
            this.Delete.ReadOnly = true;
            this.Delete.Text = "Delete";
            this.Delete.UseColumnTextForLinkValue = true;
            this.Delete.Width = 50;
            // 
            // GpDamageList
            // 
            this.GpDamageList.Controls.Add(this.dgvAdjustmentList);
            this.GpDamageList.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GpDamageList.Location = new System.Drawing.Point(9, 136);
            this.GpDamageList.Name = "GpDamageList";
            this.GpDamageList.Size = new System.Drawing.Size(1241, 539);
            this.GpDamageList.TabIndex = 5;
            this.GpDamageList.TabStop = false;
            this.GpDamageList.Text = "Adjustment List";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "From:";
            // 
            // dtpk_fromDate
            // 
            this.dtpk_fromDate.CustomFormat = "dd-MM-yyyy";
            this.dtpk_fromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpk_fromDate.Location = new System.Drawing.Point(91, 19);
            this.dtpk_fromDate.Name = "dtpk_fromDate";
            this.dtpk_fromDate.Size = new System.Drawing.Size(126, 27);
            this.dtpk_fromDate.TabIndex = 1;
            this.dtpk_fromDate.ValueChanged += new System.EventHandler(this.dtpk_fromDate_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(260, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "To:";
            // 
            // GpSearchByPeriod
            // 
            this.GpSearchByPeriod.Controls.Add(this.dtpk_toDate);
            this.GpSearchByPeriod.Controls.Add(this.label1);
            this.GpSearchByPeriod.Controls.Add(this.dtpk_fromDate);
            this.GpSearchByPeriod.Controls.Add(this.label2);
            this.GpSearchByPeriod.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GpSearchByPeriod.Location = new System.Drawing.Point(323, 16);
            this.GpSearchByPeriod.Name = "GpSearchByPeriod";
            this.GpSearchByPeriod.Size = new System.Drawing.Size(477, 57);
            this.GpSearchByPeriod.TabIndex = 0;
            this.GpSearchByPeriod.TabStop = false;
            this.GpSearchByPeriod.Text = "By Period";
            // 
            // dtpk_toDate
            // 
            this.dtpk_toDate.CustomFormat = "dd-MM-yyyy";
            this.dtpk_toDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpk_toDate.Location = new System.Drawing.Point(317, 19);
            this.dtpk_toDate.Name = "dtpk_toDate";
            this.dtpk_toDate.Size = new System.Drawing.Size(126, 27);
            this.dtpk_toDate.TabIndex = 3;
            this.dtpk_toDate.Value = new System.DateTime(2023, 4, 21, 0, 0, 0, 0);
            this.dtpk_toDate.ValueChanged += new System.EventHandler(this.dtpk_toDate_ValueChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.cboAdjType);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.cboBrand);
            this.groupBox4.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(9, 81);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(523, 51);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.label7.Location = new System.Drawing.Point(242, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 20);
            this.label7.TabIndex = 2;
            this.label7.Text = "Type  :";
            // 
            // cboAdjType
            // 
            this.cboAdjType.FormattingEnabled = true;
            this.cboAdjType.Location = new System.Drawing.Point(314, 17);
            this.cboAdjType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboAdjType.Name = "cboAdjType";
            this.cboAdjType.Size = new System.Drawing.Size(178, 28);
            this.cboAdjType.TabIndex = 3;
            this.cboAdjType.SelectedIndexChanged += new System.EventHandler(this.cboAdjType_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(20, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 20);
            this.label6.TabIndex = 0;
            this.label6.Text = "Brand :";
            // 
            // cboBrand
            // 
            this.cboBrand.FormattingEnabled = true;
            this.cboBrand.Location = new System.Drawing.Point(77, 16);
            this.cboBrand.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboBrand.Name = "cboBrand";
            this.cboBrand.Size = new System.Drawing.Size(139, 28);
            this.cboBrand.TabIndex = 1;
            this.cboBrand.SelectedValueChanged += new System.EventHandler(this.rdConsignment_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdoAll);
            this.groupBox2.Controls.Add(this.rdNonConsignment);
            this.groupBox2.Controls.Add(this.rdConsignment);
            this.groupBox2.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(815, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(426, 57);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Counter";
            // 
            // rdoAll
            // 
            this.rdoAll.AutoSize = true;
            this.rdoAll.Checked = true;
            this.rdoAll.Location = new System.Drawing.Point(12, 24);
            this.rdoAll.Name = "rdoAll";
            this.rdoAll.Size = new System.Drawing.Size(39, 24);
            this.rdoAll.TabIndex = 2;
            this.rdoAll.TabStop = true;
            this.rdoAll.Text = "All";
            this.rdoAll.UseVisualStyleBackColor = true;
            // 
            // rdNonConsignment
            // 
            this.rdNonConsignment.AutoSize = true;
            this.rdNonConsignment.Location = new System.Drawing.Point(82, 22);
            this.rdNonConsignment.Name = "rdNonConsignment";
            this.rdNonConsignment.Size = new System.Drawing.Size(172, 24);
            this.rdNonConsignment.TabIndex = 1;
            this.rdNonConsignment.Text = "Non-Consignment Counter";
            this.rdNonConsignment.UseVisualStyleBackColor = true;
            this.rdNonConsignment.CheckedChanged += new System.EventHandler(this.rdConsignment_CheckedChanged);
            // 
            // rdConsignment
            // 
            this.rdConsignment.AutoSize = true;
            this.rdConsignment.Location = new System.Drawing.Point(273, 22);
            this.rdConsignment.Name = "rdConsignment";
            this.rdConsignment.Size = new System.Drawing.Size(146, 24);
            this.rdConsignment.TabIndex = 0;
            this.rdConsignment.Text = "Consignment Counter";
            this.rdConsignment.UseVisualStyleBackColor = true;
            this.rdConsignment.CheckedChanged += new System.EventHandler(this.rdConsignment_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(206, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 20);
            this.label5.TabIndex = 2;
            this.label5.Text = "Total Stock Out    :";
            // 
            // lblStockOut
            // 
            this.lblStockOut.AutoSize = true;
            this.lblStockOut.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStockOut.Location = new System.Drawing.Point(321, 17);
            this.lblStockOut.Name = "lblStockOut";
            this.lblStockOut.Size = new System.Drawing.Size(13, 20);
            this.lblStockOut.TabIndex = 3;
            this.lblStockOut.Text = "-";
            // 
            // lblStockIn
            // 
            this.lblStockIn.AutoSize = true;
            this.lblStockIn.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStockIn.Location = new System.Drawing.Point(117, 17);
            this.lblStockIn.Name = "lblStockIn";
            this.lblStockIn.Size = new System.Drawing.Size(13, 20);
            this.lblStockIn.TabIndex = 1;
            this.lblStockIn.Text = "-";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(11, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Total Stock In   :";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lblStockOut);
            this.groupBox1.Controls.Add(this.lblStockIn);
            this.groupBox1.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(556, 81);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(476, 51);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rdoApproved);
            this.groupBox3.Controls.Add(this.rdoPending);
            this.groupBox3.Location = new System.Drawing.Point(9, 16);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(304, 57);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "By Status";
            // 
            // rdoApproved
            // 
            this.rdoApproved.AutoSize = true;
            this.rdoApproved.Location = new System.Drawing.Point(142, 24);
            this.rdoApproved.Name = "rdoApproved";
            this.rdoApproved.Size = new System.Drawing.Size(71, 17);
            this.rdoApproved.TabIndex = 1;
            this.rdoApproved.TabStop = true;
            this.rdoApproved.Text = "Approved";
            this.rdoApproved.UseVisualStyleBackColor = true;
            this.rdoApproved.CheckedChanged += new System.EventHandler(this.rdoApproved_CheckedChanged);
            // 
            // rdoPending
            // 
            this.rdoPending.AutoSize = true;
            this.rdoPending.Checked = true;
            this.rdoPending.Location = new System.Drawing.Point(16, 24);
            this.rdoPending.Name = "rdoPending";
            this.rdoPending.Size = new System.Drawing.Size(64, 17);
            this.rdoPending.TabIndex = 0;
            this.rdoPending.TabStop = true;
            this.rdoPending.Text = "Pending";
            this.rdoPending.UseVisualStyleBackColor = true;
            this.rdoPending.CheckedChanged += new System.EventHandler(this.rdoPending_CheckedChanged);
            // 
            // AdjustmentListFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1203, 680);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.GpDamageList);
            this.Controls.Add(this.GpSearchByPeriod);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AdjustmentListFrm";
            this.Text = "Adjustment List";
            this.Load += new System.EventHandler(this.AdjustmentListFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdjustmentList)).EndInit();
            this.GpDamageList.ResumeLayout(false);
            this.GpSearchByPeriod.ResumeLayout(false);
            this.GpSearchByPeriod.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvAdjustmentList;
        private System.Windows.Forms.GroupBox GpDamageList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpk_fromDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox GpSearchByPeriod;
        private System.Windows.Forms.DateTimePicker dtpk_toDate;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboBrand;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdNonConsignment;
        private System.Windows.Forms.RadioButton rdConsignment;
        private System.Windows.Forms.RadioButton rdoAll;
        private System.Windows.Forms.ComboBox cboAdjType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblStockOut;
        private System.Windows.Forms.Label lblStockIn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdoApproved;
        private System.Windows.Forms.RadioButton rdoPending;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn DamageQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStockOutQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn AdjustmentDateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ResponsibleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewLinkColumn Approved;
        private System.Windows.Forms.DataGridViewLinkColumn Delete;
    }
}