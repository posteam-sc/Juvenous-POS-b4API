namespace POS.mPOSUI
{
    partial class frmUnitConversionDetail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUnitConversionDetail));
            this.dgvUnitConversionDetail = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.lblFromProductName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblQty = new System.Windows.Forms.Label();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnitConversionDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvUnitConversionDetail
            // 
            this.dgvUnitConversionDetail.AllowUserToAddRows = false;
            this.dgvUnitConversionDetail.AllowUserToDeleteRows = false;
            this.dgvUnitConversionDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUnitConversionDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column2,
            this.Column3});
            this.dgvUnitConversionDetail.Location = new System.Drawing.Point(12, 61);
            this.dgvUnitConversionDetail.Name = "dgvUnitConversionDetail";
            this.dgvUnitConversionDetail.ReadOnly = true;
            this.dgvUnitConversionDetail.Size = new System.Drawing.Size(444, 222);
            this.dgvUnitConversionDetail.TabIndex = 0;
            this.dgvUnitConversionDetail.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvUnitConversionDetail_DataBindingComplete);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "From Product Name :";
            // 
            // lblFromProductName
            // 
            this.lblFromProductName.AutoSize = true;
            this.lblFromProductName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromProductName.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblFromProductName.Location = new System.Drawing.Point(151, 30);
            this.lblFromProductName.Name = "lblFromProductName";
            this.lblFromProductName.Size = new System.Drawing.Size(124, 15);
            this.lblFromProductName.TabIndex = 1;
            this.lblFromProductName.Text = "From Product Name :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(376, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Qty:";
            // 
            // lblQty
            // 
            this.lblQty.AutoSize = true;
            this.lblQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQty.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblQty.Location = new System.Drawing.Point(409, 30);
            this.lblQty.Name = "lblQty";
            this.lblQty.Size = new System.Drawing.Size(24, 15);
            this.lblQty.TabIndex = 1;
            this.lblQty.Text = "Qty";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "To Product Name";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 150;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Convert Qty";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // frmUnitConversionDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 295);
            this.Controls.Add(this.lblQty);
            this.Controls.Add(this.lblFromProductName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvUnitConversionDetail);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmUnitConversionDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Unit Conversion Detail";
            this.Load += new System.EventHandler(this.frmUnitConversionDetail_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnitConversionDetail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvUnitConversionDetail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblFromProductName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
    }
}