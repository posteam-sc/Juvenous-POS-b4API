namespace POS
{
    partial class frmlocalize
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmlocalize));
            this.dgvctlLocalize = new System.Windows.Forms.DataGridView();
            this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FormId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ControlName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FormName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Eng = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZawGyi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MM3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Other1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Other2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AllowToLoad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.cboForm = new System.Windows.Forms.ComboBox();
            this.cboControl = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.gbLanguage = new System.Windows.Forms.GroupBox();
            this.pcLoading = new System.Windows.Forms.PictureBox();
            this.lbllanguage = new System.Windows.Forms.Label();
            this.loading = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.dgvctlLocalize)).BeginInit();
            this.gbLanguage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvctlLocalize
            // 
            this.dgvctlLocalize.AllowUserToAddRows = false;
            this.dgvctlLocalize.AllowUserToDeleteRows = false;
            this.dgvctlLocalize.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvctlLocalize.BackgroundColor = System.Drawing.SystemColors.HighlightText;
            this.dgvctlLocalize.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvctlLocalize.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.No,
            this.Id,
            this.FormId,
            this.ControlName,
            this.Type,
            this.FormName,
            this.Eng,
            this.ZawGyi,
            this.MM3,
            this.Other1,
            this.Other2,
            this.AllowToLoad});
            this.dgvctlLocalize.Location = new System.Drawing.Point(10, 113);
            this.dgvctlLocalize.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.dgvctlLocalize.MultiSelect = false;
            this.dgvctlLocalize.Name = "dgvctlLocalize";
            this.dgvctlLocalize.Size = new System.Drawing.Size(1183, 440);
            this.dgvctlLocalize.TabIndex = 0;
            this.dgvctlLocalize.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvctlLocalize_CellBeginEdit);
            this.dgvctlLocalize.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvctlLocalize_CellEndEdit);
            this.dgvctlLocalize.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvctlLocalize_CellValueChanged);
            // 
            // No
            // 
            this.No.DataPropertyName = "No";
            this.No.HeaderText = "No";
            this.No.Name = "No";
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Visible = false;
            // 
            // FormId
            // 
            this.FormId.DataPropertyName = "FormId";
            this.FormId.HeaderText = "FormId";
            this.FormId.Name = "FormId";
            this.FormId.ReadOnly = true;
            this.FormId.Visible = false;
            // 
            // ControlName
            // 
            this.ControlName.DataPropertyName = "ControlName";
            this.ControlName.HeaderText = "ControlName";
            this.ControlName.Name = "ControlName";
            this.ControlName.ReadOnly = true;
            this.ControlName.Width = 200;
            // 
            // Type
            // 
            this.Type.DataPropertyName = "Type";
            this.Type.HeaderText = "Control Type";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            // 
            // FormName
            // 
            this.FormName.DataPropertyName = "Name";
            this.FormName.HeaderText = "FormName";
            this.FormName.Name = "FormName";
            this.FormName.ReadOnly = true;
            this.FormName.Width = 130;
            // 
            // Eng
            // 
            this.Eng.DataPropertyName = "Eng";
            this.Eng.HeaderText = "Eng Name";
            this.Eng.Name = "Eng";
            this.Eng.ReadOnly = true;
            this.Eng.Width = 200;
            // 
            // ZawGyi
            // 
            this.ZawGyi.DataPropertyName = "ZawGyi";
            this.ZawGyi.HeaderText = "ZawGyi Font";
            this.ZawGyi.Name = "ZawGyi";
            // 
            // MM3
            // 
            this.MM3.DataPropertyName = "MM3";
            this.MM3.HeaderText = "MM3 Font";
            this.MM3.Name = "MM3";
            // 
            // Other1
            // 
            this.Other1.DataPropertyName = "Other1";
            this.Other1.HeaderText = "တၿခားဘာသာစကား";
            this.Other1.Name = "Other1";
            this.Other1.Width = 120;
            // 
            // Other2
            // 
            this.Other2.DataPropertyName = "Other2";
            this.Other2.HeaderText = "Other Font 2";
            this.Other2.Name = "Other2";
            this.Other2.Width = 120;
            // 
            // AllowToLoad
            // 
            this.AllowToLoad.DataPropertyName = "AllowToLoad";
            this.AllowToLoad.HeaderText = "AllowToLoad";
            this.AllowToLoad.Name = "AllowToLoad";
            this.AllowToLoad.ReadOnly = true;
            this.AllowToLoad.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Form :";
            // 
            // cboForm
            // 
            this.cboForm.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboForm.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboForm.FormattingEnabled = true;
            this.cboForm.Location = new System.Drawing.Point(84, 18);
            this.cboForm.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.cboForm.Name = "cboForm";
            this.cboForm.Size = new System.Drawing.Size(265, 28);
            this.cboForm.TabIndex = 2;
            this.cboForm.SelectedIndexChanged += new System.EventHandler(this.cboForm_SelectedIndexChanged);
            this.cboForm.SelectedValueChanged += new System.EventHandler(this.cboForm_SelectedValueChanged);
            // 
            // cboControl
            // 
            this.cboControl.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboControl.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboControl.DropDownHeight = 150;
            this.cboControl.FormattingEnabled = true;
            this.cboControl.IntegralHeight = false;
            this.cboControl.Location = new System.Drawing.Point(474, 18);
            this.cboControl.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.cboControl.Name = "cboControl";
            this.cboControl.Size = new System.Drawing.Size(168, 28);
            this.cboControl.TabIndex = 4;
            this.cboControl.SelectedIndexChanged += new System.EventHandler(this.cboControl_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(380, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Control Type :";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(101, 41);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 28);
            this.btnExport.TabIndex = 5;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(20, 41);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 28);
            this.btnImport.TabIndex = 6;
            this.btnImport.Text = "Import Lanugage";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // gbLanguage
            // 
            this.gbLanguage.Controls.Add(this.btnExport);
            this.gbLanguage.Controls.Add(this.btnImport);
            this.gbLanguage.Location = new System.Drawing.Point(789, 6);
            this.gbLanguage.Name = "gbLanguage";
            this.gbLanguage.Size = new System.Drawing.Size(194, 89);
            this.gbLanguage.TabIndex = 7;
            this.gbLanguage.TabStop = false;
            this.gbLanguage.Text = "Language  File";
            // 
            // pcLoading
            // 
            this.pcLoading.Image = global::POS.Properties.Resources.loading_sm;
            this.pcLoading.Location = new System.Drawing.Point(708, 22);
            this.pcLoading.Name = "pcLoading";
            this.pcLoading.Size = new System.Drawing.Size(62, 57);
            this.pcLoading.TabIndex = 7;
            this.pcLoading.TabStop = false;
            this.pcLoading.Visible = false;
            // 
            // lbllanguage
            // 
            this.lbllanguage.AutoSize = true;
            this.lbllanguage.Font = new System.Drawing.Font("Zawgyi-One", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbllanguage.Location = new System.Drawing.Point(691, 82);
            this.lbllanguage.Name = "lbllanguage";
            this.lbllanguage.Size = new System.Drawing.Size(92, 15);
            this.lbllanguage.TabIndex = 8;
            this.lbllanguage.Text = "Processing Language";
            this.lbllanguage.Visible = false;
            // 
            // loading
            // 
            this.loading.DoWork += new System.ComponentModel.DoWorkEventHandler(this.loading_DoWork);
            this.loading.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.loading_RunWorkerCompleted);
            // 
            // frmlocalize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1205, 567);
            this.Controls.Add(this.lbllanguage);
            this.Controls.Add(this.cboControl);
            this.Controls.Add(this.pcLoading);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboForm);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvctlLocalize);
            this.Controls.Add(this.gbLanguage);
            this.Font = new System.Drawing.Font("Zawgyi-One", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "frmlocalize";
            this.Text = "Ctrl Localize";
            this.Load += new System.EventHandler(this.ControlLocalizer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvctlLocalize)).EndInit();
            this.gbLanguage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pcLoading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvctlLocalize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboForm;
        private System.Windows.Forms.ComboBox cboControl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.GroupBox gbLanguage;
        private System.Windows.Forms.PictureBox pcLoading;
        private System.Windows.Forms.Label lbllanguage;
        private System.ComponentModel.BackgroundWorker loading;
        private System.Windows.Forms.DataGridViewTextBoxColumn No;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn FormId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ControlName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn FormName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Eng;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZawGyi;
        private System.Windows.Forms.DataGridViewTextBoxColumn MM3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Other1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Other2;
        private System.Windows.Forms.DataGridViewTextBoxColumn AllowToLoad;
    }
}