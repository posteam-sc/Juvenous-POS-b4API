namespace POS
{
    partial class UnitConversionfrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UnitConversionfrm));
            this.colEdit = new System.Windows.Forms.DataGridViewImageColumn();
            this.colTS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSrNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaxStockId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStockId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStockNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStockName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUMId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnitRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBaseUnit = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.txtUnitMax = new System.Windows.Forms.Label();
            this.lblStockMax = new System.Windows.Forms.Label();
            this.cboFromProduct = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPiecePerPack = new System.Windows.Forms.TextBox();
            this.lblIncludingPack = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cboToProduct = new System.Windows.Forms.ComboBox();
            this.txtUnitNormal = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Balance = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblFromPerPurPrice = new System.Windows.Forms.Label();
            this.txtMaxPurPrice = new System.Windows.Forms.Label();
            this.txtFromQty = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.txtNormalPurPrice = new System.Windows.Forms.Label();
            this.lblToPerPurPrice = new System.Windows.Forms.Label();
            this.txtToQty = new System.Windows.Forms.TextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lblMaxBalance = new System.Windows.Forms.Label();
            this.txtBalanceMax = new System.Windows.Forms.TextBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.lblNormalBalance = new System.Windows.Forms.Label();
            this.txtBalanceNormal = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // colEdit
            // 
            this.colEdit.HeaderText = "Edit";
            this.colEdit.Name = "colEdit";
            this.colEdit.Width = 35;
            // 
            // colTS
            // 
            this.colTS.DataPropertyName = "TS";
            this.colTS.HeaderText = "TS";
            this.colTS.Name = "colTS";
            this.colTS.Visible = false;
            // 
            // colSrNo
            // 
            this.colSrNo.DataPropertyName = "SrNo";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colSrNo.DefaultCellStyle = dataGridViewCellStyle1;
            this.colSrNo.HeaderText = "Sr No.";
            this.colSrNo.Name = "colSrNo";
            this.colSrNo.ReadOnly = true;
            this.colSrNo.Width = 50;
            // 
            // colMaxStockId
            // 
            this.colMaxStockId.DataPropertyName = "MaxStockId";
            this.colMaxStockId.HeaderText = "";
            this.colMaxStockId.Name = "colMaxStockId";
            this.colMaxStockId.Visible = false;
            // 
            // colStockId
            // 
            this.colStockId.DataPropertyName = "StockId";
            this.colStockId.HeaderText = "StockID";
            this.colStockId.Name = "colStockId";
            this.colStockId.Visible = false;
            // 
            // colStockNo
            // 
            this.colStockNo.DataPropertyName = "StockNo";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colStockNo.DefaultCellStyle = dataGridViewCellStyle2;
            this.colStockNo.HeaderText = "Stock No";
            this.colStockNo.Name = "colStockNo";
            this.colStockNo.ReadOnly = true;
            this.colStockNo.Width = 135;
            // 
            // colStockName
            // 
            this.colStockName.DataPropertyName = "StockName";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colStockName.DefaultCellStyle = dataGridViewCellStyle3;
            this.colStockName.HeaderText = "Stock Name";
            this.colStockName.Name = "colStockName";
            this.colStockName.ReadOnly = true;
            this.colStockName.Width = 200;
            // 
            // colUMId
            // 
            this.colUMId.DataPropertyName = "UMId";
            this.colUMId.HeaderText = "UMId";
            this.colUMId.Name = "colUMId";
            this.colUMId.Visible = false;
            // 
            // colUM
            // 
            this.colUM.DataPropertyName = "Description";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colUM.DefaultCellStyle = dataGridViewCellStyle4;
            this.colUM.HeaderText = "Description";
            this.colUM.Name = "colUM";
            this.colUM.ReadOnly = true;
            this.colUM.Width = 150;
            // 
            // colUnit
            // 
            this.colUnit.DataPropertyName = "Unit";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colUnit.DefaultCellStyle = dataGridViewCellStyle5;
            this.colUnit.HeaderText = "Unit Type";
            this.colUnit.Name = "colUnit";
            this.colUnit.ReadOnly = true;
            // 
            // colUnitRate
            // 
            this.colUnitRate.DataPropertyName = "UnitRate";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colUnitRate.DefaultCellStyle = dataGridViewCellStyle6;
            this.colUnitRate.HeaderText = "Rate";
            this.colUnitRate.Name = "colUnitRate";
            this.colUnitRate.ReadOnly = true;
            this.colUnitRate.Width = 110;
            // 
            // colBaseUnit
            // 
            this.colBaseUnit.HeaderText = "Base Unit";
            this.colBaseUnit.Name = "colBaseUnit";
            this.colBaseUnit.ReadOnly = true;
            this.colBaseUnit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colBaseUnit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colBaseUnit.Width = 70;
            // 
            // txtUnitMax
            // 
            this.txtUnitMax.AutoSize = true;
            this.txtUnitMax.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUnitMax.Location = new System.Drawing.Point(270, 3);
            this.txtUnitMax.Name = "txtUnitMax";
            this.txtUnitMax.Size = new System.Drawing.Size(0, 20);
            this.txtUnitMax.TabIndex = 1;
            // 
            // lblStockMax
            // 
            this.lblStockMax.AutoSize = true;
            this.lblStockMax.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStockMax.Location = new System.Drawing.Point(3, 0);
            this.lblStockMax.Name = "lblStockMax";
            this.lblStockMax.Size = new System.Drawing.Size(208, 20);
            this.lblStockMax.TabIndex = 0;
            this.lblStockMax.Text = "From Product (Maximum Stock Unit)";
            // 
            // cboFromProduct
            // 
            this.cboFromProduct.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboFromProduct.FormattingEnabled = true;
            this.cboFromProduct.Location = new System.Drawing.Point(0, 0);
            this.cboFromProduct.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cboFromProduct.Name = "cboFromProduct";
            this.cboFromProduct.Size = new System.Drawing.Size(245, 28);
            this.cboFromProduct.TabIndex = 0;
            this.cboFromProduct.SelectedIndexChanged += new System.EventHandler(this.cboFromProduct_SelectedIndexChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.12825F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.87175F));
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtPiecePerPack, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblIncludingPack, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblStockMax, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.Balance, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.panel5, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel6, 1, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 11);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.98513F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.75465F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.24164F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.3829F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.3829F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.01115F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.49814F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(591, 269);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 228);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(163, 20);
            this.label4.TabIndex = 12;
            this.label4.Text = "Total Converted Normal Qty";
            // 
            // txtPiecePerPack
            // 
            this.txtPiecePerPack.Enabled = false;
            this.txtPiecePerPack.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPiecePerPack.Location = new System.Drawing.Point(228, 198);
            this.txtPiecePerPack.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtPiecePerPack.Name = "txtPiecePerPack";
            this.txtPiecePerPack.Size = new System.Drawing.Size(146, 27);
            this.txtPiecePerPack.TabIndex = 11;
            this.txtPiecePerPack.Leave += new System.EventHandler(this.txtPiecesPerPack_Leave);
            // 
            // lblIncludingPack
            // 
            this.lblIncludingPack.AutoSize = true;
            this.lblIncludingPack.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIncludingPack.Location = new System.Drawing.Point(3, 193);
            this.lblIncludingPack.Name = "lblIncludingPack";
            this.lblIncludingPack.Size = new System.Drawing.Size(94, 20);
            this.lblIncludingPack.TabIndex = 10;
            this.lblIncludingPack.Text = "Pieces Per Pack";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 157);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(187, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Convert Qty (Maximum Product)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Current Stock Balance";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cboToProduct);
            this.panel2.Controls.Add(this.txtUnitNormal);
            this.panel2.Location = new System.Drawing.Point(228, 83);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(351, 32);
            this.panel2.TabIndex = 5;
            // 
            // cboToProduct
            // 
            this.cboToProduct.Enabled = false;
            this.cboToProduct.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboToProduct.FormattingEnabled = true;
            this.cboToProduct.Location = new System.Drawing.Point(0, 0);
            this.cboToProduct.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cboToProduct.Name = "cboToProduct";
            this.cboToProduct.Size = new System.Drawing.Size(245, 28);
            this.cboToProduct.TabIndex = 0;
            this.cboToProduct.SelectedIndexChanged += new System.EventHandler(this.cboToProduct_SelectedIndexChanged);
            // 
            // txtUnitNormal
            // 
            this.txtUnitNormal.AutoSize = true;
            this.txtUnitNormal.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUnitNormal.Location = new System.Drawing.Point(270, 3);
            this.txtUnitNormal.Name = "txtUnitNormal";
            this.txtUnitNormal.Size = new System.Drawing.Size(0, 20);
            this.txtUnitNormal.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(183, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "To Product (Normal Stock Unit)";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cboFromProduct);
            this.panel1.Controls.Add(this.txtUnitMax);
            this.panel1.Location = new System.Drawing.Point(228, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(351, 33);
            this.panel1.TabIndex = 1;
            // 
            // Balance
            // 
            this.Balance.AutoSize = true;
            this.Balance.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Balance.Location = new System.Drawing.Point(3, 43);
            this.Balance.Name = "Balance";
            this.Balance.Size = new System.Drawing.Size(130, 20);
            this.Balance.TabIndex = 2;
            this.Balance.Text = "Current Stock Balance";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblFromPerPurPrice);
            this.panel3.Controls.Add(this.txtMaxPurPrice);
            this.panel3.Controls.Add(this.txtFromQty);
            this.panel3.Location = new System.Drawing.Point(228, 160);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(354, 30);
            this.panel3.TabIndex = 9;
            // 
            // lblFromPerPurPrice
            // 
            this.lblFromPerPurPrice.AutoSize = true;
            this.lblFromPerPurPrice.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromPerPurPrice.Location = new System.Drawing.Point(219, 7);
            this.lblFromPerPurPrice.Name = "lblFromPerPurPrice";
            this.lblFromPerPurPrice.Size = new System.Drawing.Size(110, 20);
            this.lblFromPerPurPrice.TabIndex = 2;
            this.lblFromPerPurPrice.Text = "Per Purchase Price";
            this.lblFromPerPurPrice.Visible = false;
            // 
            // txtMaxPurPrice
            // 
            this.txtMaxPurPrice.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtMaxPurPrice.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaxPurPrice.Location = new System.Drawing.Point(152, 7);
            this.txtMaxPurPrice.Name = "txtMaxPurPrice";
            this.txtMaxPurPrice.Size = new System.Drawing.Size(61, 20);
            this.txtMaxPurPrice.TabIndex = 1;
            this.txtMaxPurPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFromQty
            // 
            this.txtFromQty.Enabled = false;
            this.txtFromQty.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFromQty.Location = new System.Drawing.Point(0, 2);
            this.txtFromQty.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtFromQty.Name = "txtFromQty";
            this.txtFromQty.Size = new System.Drawing.Size(146, 27);
            this.txtFromQty.TabIndex = 0;
            this.txtFromQty.Leave += new System.EventHandler(this.txtFromQty_Leave);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.txtNormalPurPrice);
            this.panel4.Controls.Add(this.lblToPerPurPrice);
            this.panel4.Controls.Add(this.txtToQty);
            this.panel4.Location = new System.Drawing.Point(228, 231);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(354, 35);
            this.panel4.TabIndex = 13;
            // 
            // txtNormalPurPrice
            // 
            this.txtNormalPurPrice.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtNormalPurPrice.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNormalPurPrice.Location = new System.Drawing.Point(152, 6);
            this.txtNormalPurPrice.Name = "txtNormalPurPrice";
            this.txtNormalPurPrice.Size = new System.Drawing.Size(61, 20);
            this.txtNormalPurPrice.TabIndex = 3;
            this.txtNormalPurPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblToPerPurPrice
            // 
            this.lblToPerPurPrice.AutoSize = true;
            this.lblToPerPurPrice.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToPerPurPrice.Location = new System.Drawing.Point(219, 7);
            this.lblToPerPurPrice.Name = "lblToPerPurPrice";
            this.lblToPerPurPrice.Size = new System.Drawing.Size(110, 20);
            this.lblToPerPurPrice.TabIndex = 2;
            this.lblToPerPurPrice.Text = "Per Purchase Price";
            this.lblToPerPurPrice.Visible = false;
            // 
            // txtToQty
            // 
            this.txtToQty.Enabled = false;
            this.txtToQty.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtToQty.Location = new System.Drawing.Point(0, 3);
            this.txtToQty.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtToQty.Name = "txtToQty";
            this.txtToQty.Size = new System.Drawing.Size(146, 27);
            this.txtToQty.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.lblMaxBalance);
            this.panel5.Controls.Add(this.txtBalanceMax);
            this.panel5.Location = new System.Drawing.Point(228, 46);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(358, 31);
            this.panel5.TabIndex = 14;
            // 
            // lblMaxBalance
            // 
            this.lblMaxBalance.AutoSize = true;
            this.lblMaxBalance.Enabled = false;
            this.lblMaxBalance.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaxBalance.Location = new System.Drawing.Point(160, 3);
            this.lblMaxBalance.Name = "lblMaxBalance";
            this.lblMaxBalance.Size = new System.Drawing.Size(13, 20);
            this.lblMaxBalance.TabIndex = 3;
            this.lblMaxBalance.Text = "-";
            this.lblMaxBalance.Visible = false;
            // 
            // txtBalanceMax
            // 
            this.txtBalanceMax.Enabled = false;
            this.txtBalanceMax.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBalanceMax.Location = new System.Drawing.Point(0, 0);
            this.txtBalanceMax.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtBalanceMax.Name = "txtBalanceMax";
            this.txtBalanceMax.Size = new System.Drawing.Size(146, 27);
            this.txtBalanceMax.TabIndex = 3;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.lblNormalBalance);
            this.panel6.Controls.Add(this.txtBalanceNormal);
            this.panel6.Location = new System.Drawing.Point(228, 124);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(360, 30);
            this.panel6.TabIndex = 15;
            // 
            // lblNormalBalance
            // 
            this.lblNormalBalance.AutoSize = true;
            this.lblNormalBalance.Enabled = false;
            this.lblNormalBalance.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNormalBalance.Location = new System.Drawing.Point(160, 4);
            this.lblNormalBalance.Name = "lblNormalBalance";
            this.lblNormalBalance.Size = new System.Drawing.Size(13, 20);
            this.lblNormalBalance.TabIndex = 3;
            this.lblNormalBalance.Text = "-";
            this.lblNormalBalance.Visible = false;
            // 
            // txtBalanceNormal
            // 
            this.txtBalanceNormal.Enabled = false;
            this.txtBalanceNormal.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBalanceNormal.Location = new System.Drawing.Point(0, 1);
            this.txtBalanceNormal.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtBalanceNormal.Name = "txtBalanceNormal";
            this.txtBalanceNormal.Size = new System.Drawing.Size(146, 27);
            this.txtBalanceNormal.TabIndex = 7;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Image = global::POS.Properties.Resources.cancel_big;
            this.btnCancel.Location = new System.Drawing.Point(478, 287);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(109, 45);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Tag = "7";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.Transparent;
            this.btnSubmit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnSubmit.FlatAppearance.BorderSize = 0;
            this.btnSubmit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnSubmit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmit.Image = global::POS.Properties.Resources.button_convert;
            this.btnSubmit.Location = new System.Drawing.Point(337, 287);
            this.btnSubmit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(135, 45);
            this.btnSubmit.TabIndex = 1;
            this.btnSubmit.Tag = "6";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // UnitConversionfrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 331);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "UnitConversionfrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stock Unit Conversion";
            this.Load += new System.EventHandler(this.UnitConversionfrm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewImageColumn colEdit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTS;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSrNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaxStockId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStockId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStockNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStockName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUMId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUM;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnitRate;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colBaseUnit;
        private System.Windows.Forms.Label txtUnitMax;
        private System.Windows.Forms.Label lblStockMax;
        private System.Windows.Forms.ComboBox cboFromProduct;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtBalanceMax;
        private System.Windows.Forms.Label Balance;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cboToProduct;
        private System.Windows.Forms.Label txtUnitNormal;
        private System.Windows.Forms.TextBox txtBalanceNormal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtToQty;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPiecePerPack;
        private System.Windows.Forms.Label lblIncludingPack;
        private System.Windows.Forms.TextBox txtFromQty;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label txtMaxPurPrice;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblFromPerPurPrice;
        private System.Windows.Forms.Label lblToPerPurPrice;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label lblMaxBalance;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label lblNormalBalance;
        private System.Windows.Forms.Label txtNormalPurPrice;

    }
}