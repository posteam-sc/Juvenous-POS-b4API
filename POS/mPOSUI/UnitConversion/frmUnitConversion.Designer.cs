namespace POS
{
    partial class frmUnitConversion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUnitConversion));
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboToProduct = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblStockMax = new System.Windows.Forms.Label();
            this.cboFromProduct = new System.Windows.Forms.ComboBox();
            this.Balance = new System.Windows.Forms.Label();
            this.txtConvertQty = new System.Windows.Forms.TextBox();
            this.txtFromProductQty = new System.Windows.Forms.TextBox();
            this.txtToProductQty = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.dgvUnitConversion = new System.Windows.Forms.DataGridView();
            this.ProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Units = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qunatity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SellingPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Delete = new System.Windows.Forms.DataGridViewLinkColumn();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPiecePerPack = new System.Windows.Forms.TextBox();
            this.lblFromProductSize = new System.Windows.Forms.Label();
            this.lbltoProductSize = new System.Windows.Forms.Label();
            this.txtRemainAmount = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSellingPrice = new System.Windows.Forms.TextBox();
            this.lblsellingPrice = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lblUnit = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnitConversion)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(153, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Convert Qty (From Product)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 181);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "Current To Stock Balance";
            // 
            // cboToProduct
            // 
            this.cboToProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboToProduct.FormattingEnabled = true;
            this.cboToProduct.Location = new System.Drawing.Point(195, 144);
            this.cboToProduct.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cboToProduct.Name = "cboToProduct";
            this.cboToProduct.Size = new System.Drawing.Size(245, 23);
            this.cboToProduct.TabIndex = 2;
            this.cboToProduct.SelectedIndexChanged += new System.EventHandler(this.cboToProduct_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "To Product";
            // 
            // lblStockMax
            // 
            this.lblStockMax.AutoSize = true;
            this.lblStockMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStockMax.Location = new System.Drawing.Point(12, 18);
            this.lblStockMax.Name = "lblStockMax";
            this.lblStockMax.Size = new System.Drawing.Size(81, 15);
            this.lblStockMax.TabIndex = 0;
            this.lblStockMax.Text = "From Product";
            // 
            // cboFromProduct
            // 
            this.cboFromProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboFromProduct.FormattingEnabled = true;
            this.cboFromProduct.Location = new System.Drawing.Point(195, 15);
            this.cboFromProduct.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cboFromProduct.Name = "cboFromProduct";
            this.cboFromProduct.Size = new System.Drawing.Size(245, 23);
            this.cboFromProduct.TabIndex = 0;
            this.cboFromProduct.SelectedIndexChanged += new System.EventHandler(this.cboFromProduct_SelectedIndexChanged);
            // 
            // Balance
            // 
            this.Balance.AutoSize = true;
            this.Balance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Balance.Location = new System.Drawing.Point(12, 52);
            this.Balance.Name = "Balance";
            this.Balance.Size = new System.Drawing.Size(160, 15);
            this.Balance.TabIndex = 2;
            this.Balance.Text = "Current From Stock Balance";
            // 
            // txtConvertQty
            // 
            this.txtConvertQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConvertQty.Location = new System.Drawing.Point(195, 80);
            this.txtConvertQty.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtConvertQty.Name = "txtConvertQty";
            this.txtConvertQty.Size = new System.Drawing.Size(146, 21);
            this.txtConvertQty.TabIndex = 1;
            this.txtConvertQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtConvertQty.TextChanged += new System.EventHandler(this.txtConvertQty_TextChanged);
            this.txtConvertQty.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtConvertQty_KeyDown);
            this.txtConvertQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtConvertQty_KeyPress);
            // 
            // txtFromProductQty
            // 
            this.txtFromProductQty.Enabled = false;
            this.txtFromProductQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFromProductQty.Location = new System.Drawing.Point(195, 49);
            this.txtFromProductQty.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtFromProductQty.Name = "txtFromProductQty";
            this.txtFromProductQty.Size = new System.Drawing.Size(146, 21);
            this.txtFromProductQty.TabIndex = 3;
            this.txtFromProductQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtToProductQty
            // 
            this.txtToProductQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtToProductQty.Location = new System.Drawing.Point(195, 178);
            this.txtToProductQty.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtToProductQty.Name = "txtToProductQty";
            this.txtToProductQty.Size = new System.Drawing.Size(146, 21);
            this.txtToProductQty.TabIndex = 7;
            this.txtToProductQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
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
            this.btnCancel.Location = new System.Drawing.Point(284, 436);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(109, 45);
            this.btnCancel.TabIndex = 8;
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
            this.btnSubmit.Location = new System.Drawing.Point(134, 436);
            this.btnSubmit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(135, 45);
            this.btnSubmit.TabIndex = 7;
            this.btnSubmit.Tag = "6";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // dgvUnitConversion
            // 
            this.dgvUnitConversion.AllowUserToAddRows = false;
            this.dgvUnitConversion.AllowUserToDeleteRows = false;
            this.dgvUnitConversion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUnitConversion.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProductId,
            this.ProductName,
            this.Size,
            this.Units,
            this.Qunatity,
            this.SellingPrice,
            this.Delete});
            this.dgvUnitConversion.Location = new System.Drawing.Point(15, 279);
            this.dgvUnitConversion.Name = "dgvUnitConversion";
            this.dgvUnitConversion.ReadOnly = true;
            this.dgvUnitConversion.Size = new System.Drawing.Size(519, 150);
            this.dgvUnitConversion.TabIndex = 6;
            this.dgvUnitConversion.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUnitConversion_CellClick);
            // 
            // ProductId
            // 
            this.ProductId.HeaderText = "Product ID";
            this.ProductId.Name = "ProductId";
            this.ProductId.ReadOnly = true;
            this.ProductId.Visible = false;
            // 
            // ProductName
            // 
            this.ProductName.HeaderText = "Product Name";
            this.ProductName.Name = "ProductName";
            this.ProductName.ReadOnly = true;
            this.ProductName.Width = 150;
            // 
            // Size
            // 
            this.Size.HeaderText = "Size";
            this.Size.Name = "Size";
            this.Size.ReadOnly = true;
            this.Size.Visible = false;
            // 
            // Units
            // 
            this.Units.HeaderText = "Units";
            this.Units.Name = "Units";
            this.Units.ReadOnly = true;
            // 
            // Qunatity
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N0";
            dataGridViewCellStyle1.NullValue = null;
            this.Qunatity.DefaultCellStyle = dataGridViewCellStyle1;
            this.Qunatity.HeaderText = "Quantity";
            this.Qunatity.Name = "Qunatity";
            this.Qunatity.ReadOnly = true;
            this.Qunatity.Width = 60;
            // 
            // SellingPrice
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N0";
            dataGridViewCellStyle2.NullValue = null;
            this.SellingPrice.DefaultCellStyle = dataGridViewCellStyle2;
            this.SellingPrice.HeaderText = "Selling Price";
            this.SellingPrice.Name = "SellingPrice";
            this.SellingPrice.ReadOnly = true;
            // 
            // Delete
            // 
            this.Delete.HeaderText = "";
            this.Delete.Name = "Delete";
            this.Delete.ReadOnly = true;
            this.Delete.Text = "delete";
            this.Delete.UseColumnTextForLinkValue = true;
            this.Delete.Width = 60;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 212);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 15);
            this.label4.TabIndex = 14;
            this.label4.Text = "Pieces Per Pack";
            // 
            // txtPiecePerPack
            // 
            this.txtPiecePerPack.Enabled = false;
            this.txtPiecePerPack.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPiecePerPack.Location = new System.Drawing.Point(195, 209);
            this.txtPiecePerPack.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtPiecePerPack.Name = "txtPiecePerPack";
            this.txtPiecePerPack.Size = new System.Drawing.Size(146, 21);
            this.txtPiecePerPack.TabIndex = 3;
            this.txtPiecePerPack.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPiecePerPack.TextChanged += new System.EventHandler(this.txtPiecePerPack_TextChanged);
            this.txtPiecePerPack.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPiecePerPack_KeyDown);
            this.txtPiecePerPack.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPiecePerPack_KeyPress);
            // 
            // lblFromProductSize
            // 
            this.lblFromProductSize.AutoSize = true;
            this.lblFromProductSize.Location = new System.Drawing.Point(459, 20);
            this.lblFromProductSize.Name = "lblFromProductSize";
            this.lblFromProductSize.Size = new System.Drawing.Size(0, 13);
            this.lblFromProductSize.TabIndex = 16;
            // 
            // lbltoProductSize
            // 
            this.lbltoProductSize.AutoSize = true;
            this.lbltoProductSize.Location = new System.Drawing.Point(459, 149);
            this.lbltoProductSize.Name = "lbltoProductSize";
            this.lbltoProductSize.Size = new System.Drawing.Size(0, 13);
            this.lbltoProductSize.TabIndex = 17;
            // 
            // txtRemainAmount
            // 
            this.txtRemainAmount.Enabled = false;
            this.txtRemainAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRemainAmount.Location = new System.Drawing.Point(195, 112);
            this.txtRemainAmount.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtRemainAmount.Name = "txtRemainAmount";
            this.txtRemainAmount.Size = new System.Drawing.Size(146, 21);
            this.txtRemainAmount.TabIndex = 18;
            this.txtRemainAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 15);
            this.label5.TabIndex = 19;
            this.label5.Text = "Total Remain Amount";
            // 
            // txtSellingPrice
            // 
            this.txtSellingPrice.Enabled = false;
            this.txtSellingPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSellingPrice.Location = new System.Drawing.Point(195, 240);
            this.txtSellingPrice.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtSellingPrice.Name = "txtSellingPrice";
            this.txtSellingPrice.Size = new System.Drawing.Size(146, 21);
            this.txtSellingPrice.TabIndex = 4;
            this.txtSellingPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSellingPrice.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSellingPrice_KeyDown);
            this.txtSellingPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSellingPrice_KeyPress);
            // 
            // lblsellingPrice
            // 
            this.lblsellingPrice.AutoSize = true;
            this.lblsellingPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblsellingPrice.Location = new System.Drawing.Point(12, 243);
            this.lblsellingPrice.Name = "lblsellingPrice";
            this.lblsellingPrice.Size = new System.Drawing.Size(126, 15);
            this.lblsellingPrice.TabIndex = 20;
            this.lblsellingPrice.Text = "To Stock Selling Price";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(390, 223);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(88, 38);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "&Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lblUnit
            // 
            this.lblUnit.AutoSize = true;
            this.lblUnit.Location = new System.Drawing.Point(347, 117);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(0, 13);
            this.lblUnit.TabIndex = 23;
            // 
            // frmUnitConversion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 485);
            this.Controls.Add(this.lblUnit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtSellingPrice);
            this.Controls.Add(this.lblsellingPrice);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtRemainAmount);
            this.Controls.Add(this.lbltoProductSize);
            this.Controls.Add(this.lblFromProductSize);
            this.Controls.Add(this.txtPiecePerPack);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dgvUnitConversion);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Balance);
            this.Controls.Add(this.cboToProduct);
            this.Controls.Add(this.txtConvertQty);
            this.Controls.Add(this.txtToProductQty);
            this.Controls.Add(this.cboFromProduct);
            this.Controls.Add(this.txtFromProductQty);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.lblStockMax);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmUnitConversion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Unit Conversion";
            this.Load += new System.EventHandler(this.frmUnitConversion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnitConversion)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboToProduct;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblStockMax;
        private System.Windows.Forms.ComboBox cboFromProduct;
        private System.Windows.Forms.Label Balance;
        private System.Windows.Forms.TextBox txtConvertQty;
        private System.Windows.Forms.TextBox txtFromProductQty;
        private System.Windows.Forms.TextBox txtToProductQty;
        private System.Windows.Forms.DataGridView dgvUnitConversion;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPiecePerPack;
        private System.Windows.Forms.Label lblFromProductSize;
        private System.Windows.Forms.Label lbltoProductSize;
        private System.Windows.Forms.TextBox txtRemainAmount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSellingPrice;
        private System.Windows.Forms.Label lblsellingPrice;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Size;
        private System.Windows.Forms.DataGridViewTextBoxColumn Units;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qunatity;
        private System.Windows.Forms.DataGridViewTextBoxColumn SellingPrice;
        private System.Windows.Forms.DataGridViewLinkColumn Delete;
    }
}