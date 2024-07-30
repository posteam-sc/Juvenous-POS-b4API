namespace POS
{
    partial class AssignTicketButton
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssignTicketButton));
            this.dgvButtons = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ButtonName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ButtonText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Assignproductid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Defined = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Defined1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Edit = new System.Windows.Forms.DataGridViewLinkColumn();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cboProduct = new System.Windows.Forms.ComboBox();
            this.txtbutton = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvButtons)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvButtons
            // 
            this.dgvButtons.AllowUserToAddRows = false;
            this.dgvButtons.AllowUserToDeleteRows = false;
            this.dgvButtons.BackgroundColor = System.Drawing.Color.White;
            this.dgvButtons.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvButtons.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.ButtonName,
            this.ButtonText,
            this.Assignproductid,
            this.ProductName,
            this.Defined,
            this.Defined1,
            this.Edit});
            this.dgvButtons.Location = new System.Drawing.Point(14, 135);
            this.dgvButtons.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.dgvButtons.Name = "dgvButtons";
            this.dgvButtons.ReadOnly = true;
            this.dgvButtons.Size = new System.Drawing.Size(455, 303);
            this.dgvButtons.TabIndex = 1;
            this.dgvButtons.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvButtons_CellClick);
            this.dgvButtons.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvButtons_DataBindingComplete);
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Visible = false;
            // 
            // ButtonName
            // 
            this.ButtonName.DataPropertyName = "ButtonName";
            this.ButtonName.HeaderText = "ButtonName";
            this.ButtonName.Name = "ButtonName";
            this.ButtonName.ReadOnly = true;
            // 
            // ButtonText
            // 
            this.ButtonText.DataPropertyName = "ButtonText";
            this.ButtonText.HeaderText = "ButtonText";
            this.ButtonText.Name = "ButtonText";
            this.ButtonText.ReadOnly = true;
            // 
            // Assignproductid
            // 
            this.Assignproductid.DataPropertyName = "Assignproductid";
            this.Assignproductid.HeaderText = "Assignproductid";
            this.Assignproductid.Name = "Assignproductid";
            this.Assignproductid.ReadOnly = true;
            this.Assignproductid.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Assignproductid.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Assignproductid.Visible = false;
            // 
            // ProductName
            // 
            this.ProductName.DataPropertyName = "ProductName";
            this.ProductName.HeaderText = "ProductName";
            this.ProductName.Name = "ProductName";
            this.ProductName.ReadOnly = true;
            // 
            // Defined
            // 
            this.Defined.DataPropertyName = "Defined";
            this.Defined.HeaderText = "Defined";
            this.Defined.Name = "Defined";
            this.Defined.ReadOnly = true;
            this.Defined.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Defined.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Defined.Visible = false;
            // 
            // Defined1
            // 
            this.Defined1.DataPropertyName = "Defined1";
            this.Defined1.HeaderText = "Defined1";
            this.Defined1.Name = "Defined1";
            this.Defined1.ReadOnly = true;
            this.Defined1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Defined1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Defined1.Visible = false;
            // 
            // Edit
            // 
            this.Edit.HeaderText = "";
            this.Edit.Name = "Edit";
            this.Edit.ReadOnly = true;
            this.Edit.Text = "Edit";
            this.Edit.UseColumnTextForLinkValue = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(283, 81);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(87, 35);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Update";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(382, 81);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 35);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cboProduct
            // 
            this.cboProduct.FormattingEnabled = true;
            this.cboProduct.Location = new System.Drawing.Point(283, 32);
            this.cboProduct.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.cboProduct.Name = "cboProduct";
            this.cboProduct.Size = new System.Drawing.Size(186, 28);
            this.cboProduct.TabIndex = 5;
            // 
            // txtbutton
            // 
            this.txtbutton.Location = new System.Drawing.Point(63, 34);
            this.txtbutton.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtbutton.Name = "txtbutton";
            this.txtbutton.ReadOnly = true;
            this.txtbutton.Size = new System.Drawing.Size(141, 27);
            this.txtbutton.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(210, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Product";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Button";
            // 
            // AssignTicketButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 471);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtbutton);
            this.Controls.Add(this.cboProduct);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvButtons);
            this.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "AssignTicketButton";
            this.Text = "AssignTicketButton";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AssignTicketButton_FormClosing);
            this.Load += new System.EventHandler(this.AssignTicketButton_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvButtons)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvButtons;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cboProduct;
        private System.Windows.Forms.TextBox txtbutton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn ButtonName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ButtonText;
        private System.Windows.Forms.DataGridViewTextBoxColumn Assignproductid;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Defined;
        private System.Windows.Forms.DataGridViewTextBoxColumn Defined1;
        private System.Windows.Forms.DataGridViewLinkColumn Edit;

    }
}