namespace POS
{
    partial class AdjustmentFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdjustmentFrm));
            this.label3 = new System.Windows.Forms.Label();
            this.txtAdjustmentQty = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtReason = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtResponsiblePerson = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpAdjustmentDate = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cboProduct = new System.Windows.Forms.ComboBox();
            this.txtUnitPrice = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cboSign = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnNewBrand = new System.Windows.Forms.Button();
            this.cboAdjType = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDamage = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.label3.Location = new System.Drawing.Point(3, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "* Unit Price  :";
            // 
            // txtAdjustmentQty
            // 
            this.txtAdjustmentQty.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.txtAdjustmentQty.Location = new System.Drawing.Point(62, 2);
            this.txtAdjustmentQty.Name = "txtAdjustmentQty";
            this.txtAdjustmentQty.Size = new System.Drawing.Size(116, 27);
            this.txtAdjustmentQty.TabIndex = 1;
            this.txtAdjustmentQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtQty_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.label4.Location = new System.Drawing.Point(3, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "* Adjustment Qty:";
            // 
            // txtReason
            // 
            this.txtReason.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.txtReason.Location = new System.Drawing.Point(166, 237);
            this.txtReason.Multiline = true;
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(515, 110);
            this.txtReason.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.label6.Location = new System.Drawing.Point(3, 195);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(124, 20);
            this.label6.TabIndex = 10;
            this.label6.Text = "* Responsible Person";
            // 
            // txtResponsiblePerson
            // 
            this.txtResponsiblePerson.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.txtResponsiblePerson.Location = new System.Drawing.Point(166, 198);
            this.txtResponsiblePerson.Name = "txtResponsiblePerson";
            this.txtResponsiblePerson.Size = new System.Drawing.Size(320, 27);
            this.txtResponsiblePerson.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.label7.Location = new System.Drawing.Point(3, 234);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 20);
            this.label7.TabIndex = 12;
            this.label7.Text = " Reason";
            // 
            // dtpAdjustmentDate
            // 
            this.dtpAdjustmentDate.CustomFormat = "dd-MM-yyyy";
            this.dtpAdjustmentDate.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.dtpAdjustmentDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpAdjustmentDate.Location = new System.Drawing.Point(166, 159);
            this.dtpAdjustmentDate.Name = "dtpAdjustmentDate";
            this.dtpAdjustmentDate.Size = new System.Drawing.Size(178, 27);
            this.dtpAdjustmentDate.TabIndex = 9;
            this.dtpAdjustmentDate.Value = new System.DateTime(2023, 4, 23, 0, 0, 0, 0);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.label8.Location = new System.Drawing.Point(3, 156);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(114, 20);
            this.label8.TabIndex = 8;
            this.label8.Text = "* Adjustment Date";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.83041F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76.16959F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cboProduct, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtUnitPrice, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtResponsiblePerson, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.dtpAdjustmentDate, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtReason, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 2);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(14, 15);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.0562F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.97641F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.97641F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.97641F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.97641F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.97641F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34.06177F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(684, 360);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.label1.Location = new System.Drawing.Point(3, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "* Type";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.label5.Location = new System.Drawing.Point(3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 20);
            this.label5.TabIndex = 0;
            this.label5.Text = "* Product Name :";
            // 
            // cboProduct
            // 
            this.cboProduct.DropDownWidth = 400;
            this.cboProduct.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.cboProduct.FormattingEnabled = true;
            this.cboProduct.Location = new System.Drawing.Point(166, 5);
            this.cboProduct.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.cboProduct.Name = "cboProduct";
            this.cboProduct.Size = new System.Drawing.Size(320, 28);
            this.cboProduct.TabIndex = 1;
            this.cboProduct.SelectedValueChanged += new System.EventHandler(this.cboProduct_SelectedValueChanged);
            this.cboProduct.Leave += new System.EventHandler(this.cboProduct_Leave);
            // 
            // txtUnitPrice
            // 
            this.txtUnitPrice.Enabled = false;
            this.txtUnitPrice.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.txtUnitPrice.Location = new System.Drawing.Point(166, 42);
            this.txtUnitPrice.Name = "txtUnitPrice";
            this.txtUnitPrice.Size = new System.Drawing.Size(116, 27);
            this.txtUnitPrice.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cboSign);
            this.panel1.Controls.Add(this.txtAdjustmentQty);
            this.panel1.Location = new System.Drawing.Point(166, 120);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(494, 32);
            this.panel1.TabIndex = 7;
            // 
            // cboSign
            // 
            this.cboSign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSign.FormattingEnabled = true;
            this.cboSign.Items.AddRange(new object[] {
            "-",
            "+"});
            this.cboSign.Location = new System.Drawing.Point(3, 2);
            this.cboSign.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboSign.Name = "cboSign";
            this.cboSign.Size = new System.Drawing.Size(53, 28);
            this.cboSign.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnNewBrand);
            this.panel2.Controls.Add(this.cboAdjType);
            this.panel2.Location = new System.Drawing.Point(166, 81);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(494, 32);
            this.panel2.TabIndex = 5;
            // 
            // btnNewBrand
            // 
            this.btnNewBrand.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnNewBrand.FlatAppearance.BorderSize = 0;
            this.btnNewBrand.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnNewBrand.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnNewBrand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewBrand.Image = global::POS.Properties.Resources.type_button;
            this.btnNewBrand.Location = new System.Drawing.Point(205, 1);
            this.btnNewBrand.Name = "btnNewBrand";
            this.btnNewBrand.Size = new System.Drawing.Size(87, 27);
            this.btnNewBrand.TabIndex = 1;
            this.btnNewBrand.UseVisualStyleBackColor = true;
            this.btnNewBrand.Click += new System.EventHandler(this.btnNewBrand_Click);
            // 
            // cboAdjType
            // 
            this.cboAdjType.FormattingEnabled = true;
            this.cboAdjType.Location = new System.Drawing.Point(0, 0);
            this.cboAdjType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboAdjType.Name = "cboAdjType";
            this.cboAdjType.Size = new System.Drawing.Size(178, 28);
            this.cboAdjType.TabIndex = 0;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.Red;
            this.label13.Location = new System.Drawing.Point(18, 397);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(94, 13);
            this.label13.TabIndex = 15;
            this.label13.Text = "* Mandatory Fileds";
            // 
            // btnCancel
            // 
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(202)))), ((int)(((byte)(125)))));
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Image = global::POS.Properties.Resources.cancel_big;
            this.btnCancel.Location = new System.Drawing.Point(584, 387);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(114, 33);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDamage
            // 
            this.btnDamage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(202)))), ((int)(((byte)(125)))));
            this.btnDamage.FlatAppearance.BorderSize = 0;
            this.btnDamage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnDamage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnDamage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDamage.Image = global::POS.Properties.Resources.save_big;
            this.btnDamage.Location = new System.Drawing.Point(478, 387);
            this.btnDamage.Name = "btnDamage";
            this.btnDamage.Size = new System.Drawing.Size(114, 33);
            this.btnDamage.TabIndex = 1;
            this.btnDamage.UseVisualStyleBackColor = true;
            this.btnDamage.Click += new System.EventHandler(this.btnAdjustment_Click);
            // 
            // AdjustmentFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(714, 440);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDamage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AdjustmentFrm";
            this.Text = "Adjustment";
            this.Load += new System.EventHandler(this.AdjustmentFrm_Load);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.AdjustmentFrm_MouseMove);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAdjustmentQty;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtReason;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtResponsiblePerson;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtpAdjustmentDate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnDamage;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboProduct;
        private System.Windows.Forms.TextBox txtUnitPrice;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cboSign;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboAdjType;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnNewBrand;
    }
}