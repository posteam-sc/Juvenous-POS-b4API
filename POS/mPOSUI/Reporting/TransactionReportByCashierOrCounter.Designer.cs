namespace POS
{
    partial class TransactionReportByCashierOrCounter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransactionReportByCashierOrCounter));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblCounterName = new System.Windows.Forms.Label();
            this.lblCashierName = new System.Windows.Forms.Label();
            this.cboCounter = new System.Windows.Forms.ComboBox();
            this.cboCashier = new System.Windows.Forms.ComboBox();
            this.rbdCounter = new System.Windows.Forms.RadioButton();
            this.rdbCashier = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblCounterName);
            this.groupBox1.Controls.Add(this.lblCashierName);
            this.groupBox1.Controls.Add(this.cboCounter);
            this.groupBox1.Controls.Add(this.cboCashier);
            this.groupBox1.Controls.Add(this.rbdCounter);
            this.groupBox1.Controls.Add(this.rdbCashier);
            this.groupBox1.Font = new System.Drawing.Font("Zawgyi-One", 9F);
            this.groupBox1.Location = new System.Drawing.Point(37, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(637, 110);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "By Cashier or Counter";
            // 
            // lblCounterName
            // 
            this.lblCounterName.AutoSize = true;
            this.lblCounterName.Location = new System.Drawing.Point(266, 68);
            this.lblCounterName.Name = "lblCounterName";
            this.lblCounterName.Size = new System.Drawing.Size(88, 20);
            this.lblCounterName.TabIndex = 5;
            this.lblCounterName.Text = "Counter Name";
            // 
            // lblCashierName
            // 
            this.lblCashierName.AutoSize = true;
            this.lblCashierName.Location = new System.Drawing.Point(266, 32);
            this.lblCashierName.Name = "lblCashierName";
            this.lblCashierName.Size = new System.Drawing.Size(82, 20);
            this.lblCashierName.TabIndex = 4;
            this.lblCashierName.Text = "Cashier Name";
            // 
            // cboCounter
            // 
            this.cboCounter.FormattingEnabled = true;
            this.cboCounter.Location = new System.Drawing.Point(382, 65);
            this.cboCounter.Name = "cboCounter";
            this.cboCounter.Size = new System.Drawing.Size(227, 28);
            this.cboCounter.TabIndex = 3;
            // 
            // cboCashier
            // 
            this.cboCashier.FormattingEnabled = true;
            this.cboCashier.Location = new System.Drawing.Point(382, 29);
            this.cboCashier.Name = "cboCashier";
            this.cboCashier.Size = new System.Drawing.Size(227, 28);
            this.cboCashier.TabIndex = 2;
            // 
            // rbdCounter
            // 
            this.rbdCounter.AutoSize = true;
            this.rbdCounter.Location = new System.Drawing.Point(64, 66);
            this.rbdCounter.Name = "rbdCounter";
            this.rbdCounter.Size = new System.Drawing.Size(71, 24);
            this.rbdCounter.TabIndex = 1;
            this.rbdCounter.TabStop = true;
            this.rbdCounter.Text = "Counter";
            this.rbdCounter.UseVisualStyleBackColor = true;
            // 
            // rdbCashier
            // 
            this.rdbCashier.AutoSize = true;
            this.rdbCashier.Location = new System.Drawing.Point(64, 30);
            this.rdbCashier.Name = "rdbCashier";
            this.rdbCashier.Size = new System.Drawing.Size(65, 24);
            this.rdbCashier.TabIndex = 0;
            this.rdbCashier.TabStop = true;
            this.rdbCashier.Text = "Cashier";
            this.rdbCashier.UseVisualStyleBackColor = true;
            // 
            // TransactionReportByCashierOrCounter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 534);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TransactionReportByCashierOrCounter";
            this.Text = "TransactionReportByCashierOrCounter";
            this.Load += new System.EventHandler(this.TransactionReportByCashierOrCounter_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdbCashier;
        private System.Windows.Forms.RadioButton rbdCounter;
        private System.Windows.Forms.ComboBox cboCounter;
        private System.Windows.Forms.ComboBox cboCashier;
        private System.Windows.Forms.Label lblCounterName;
        private System.Windows.Forms.Label lblCashierName;
    }
}