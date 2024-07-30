namespace POS
{
    partial class csvExportImport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(csvExportImport));
            this.bntExport = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.gbimported = new System.Windows.Forms.GroupBox();
            this.dgvImport = new System.Windows.Forms.DataGridView();
            this.importprogress = new System.Windows.Forms.ProgressBar();
            this.lblstatus = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.gbimported.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvImport)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bntExport
            // 
            this.bntExport.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.bntExport.Font = new System.Drawing.Font("Zawgyi-One", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntExport.Location = new System.Drawing.Point(981, 502);
            this.bntExport.Name = "bntExport";
            this.bntExport.Size = new System.Drawing.Size(101, 31);
            this.bntExport.TabIndex = 0;
            this.bntExport.Text = "Export";
            this.bntExport.UseVisualStyleBackColor = true;
            this.bntExport.Click += new System.EventHandler(this.bntExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnImport.Font = new System.Drawing.Font("Zawgyi-One", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImport.Location = new System.Drawing.Point(835, 502);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(101, 31);
            this.btnImport.TabIndex = 1;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // gbimported
            // 
            this.gbimported.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbimported.Controls.Add(this.dgvImport);
            this.gbimported.Location = new System.Drawing.Point(12, 12);
            this.gbimported.Name = "gbimported";
            this.gbimported.Size = new System.Drawing.Size(1098, 471);
            this.gbimported.TabIndex = 4;
            this.gbimported.TabStop = false;
            this.gbimported.Text = "Imported product";
            // 
            // dgvImport
            // 
            this.dgvImport.AllowDrop = true;
            this.dgvImport.AllowUserToAddRows = false;
            this.dgvImport.AllowUserToDeleteRows = false;
            this.dgvImport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvImport.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvImport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvImport.Location = new System.Drawing.Point(26, 19);
            this.dgvImport.Name = "dgvImport";
            this.dgvImport.ReadOnly = true;
            this.dgvImport.Size = new System.Drawing.Size(1044, 431);
            this.dgvImport.TabIndex = 6;
            // 
            // importprogress
            // 
            this.importprogress.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.importprogress.Location = new System.Drawing.Point(146, 505);
            this.importprogress.Name = "importprogress";
            this.importprogress.Size = new System.Drawing.Size(369, 19);
            this.importprogress.TabIndex = 6;
            // 
            // lblstatus
            // 
            this.lblstatus.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblstatus.AutoSize = true;
            this.lblstatus.Font = new System.Drawing.Font("Zawgyi-One", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblstatus.ForeColor = System.Drawing.Color.Blue;
            this.lblstatus.Location = new System.Drawing.Point(82, 0);
            this.lblstatus.Name = "lblstatus";
            this.lblstatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblstatus.Size = new System.Drawing.Size(49, 22);
            this.lblstatus.TabIndex = 7;
            this.lblstatus.Text = "Ready";
            this.lblstatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.flowLayoutPanel1.Controls.Add(this.lblstatus);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(7, 503);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.flowLayoutPanel1.Size = new System.Drawing.Size(134, 34);
            this.flowLayoutPanel1.TabIndex = 8;
            // 
            // csvExportImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1122, 551);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.importprogress);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.bntExport);
            this.Controls.Add(this.gbimported);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "csvExportImport";
            this.RightToLeftLayout = true;
            this.Text = "CSV Export&Import";
            this.Load += new System.EventHandler(this.csvExportImport_Load);
            this.gbimported.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvImport)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bntExport;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.GroupBox gbimported;
        private System.Windows.Forms.DataGridView dgvImport;
        private System.Windows.Forms.ProgressBar importprogress;
        private System.Windows.Forms.Label lblstatus;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}