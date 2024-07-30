using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS.APP_Data;

namespace POS
{
    public partial class Taxes : Form
    {
        #region Variables

        POSEntities posEntity = new POSEntities();
        private bool isEdit = false;
        private ToolTip tp = new ToolTip();
        int currentId;
        int currentTaxId = 0;
        #endregion
        #region Event

        public Taxes()
        {
            InitializeComponent();
        }

        private void Taxes_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            bool notbackoffice = Utility.IsNotBackOffice();
            if (notbackoffice)
            {
                Utility.Gpvisible(groupBox1, isEdit);
            }
            dgvTaxList.AutoGenerateColumns = false;
            dgvTaxList.DataSource = posEntity.Taxes.Where(t=>t.IsDelete==false).ToList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Boolean hasError = false;

            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            //Validation
            if (txtName.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtName, "Error");
                tp.Show("Please fill up tax name!", txtName);
                hasError = true;
            }

            if (txtPercent.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtPercent, "Error");
                tp.Show("Please fill up tax percent!", txtPercent);
                hasError = true;
            }
            //else
            //{
            //    int n;
            //    bool isNumeric = int.TryParse(txtPercent.Text, out  n);
            //    if (!isNumeric)
            //    {
            //        tp.SetToolTip(txtPercent, "Error");
            //        tp.Show("Please fill only number!", txtPercent);
            //    }
            //    hasError = isNumeric ? false : true;
            //}

            if (!hasError)
            {
                if (lblStatus.Text == "Add")
                {
                    Tax taxObj = new Tax();
                    Tax taxObj2 = (from t in posEntity.Taxes where t.Name == txtName.Text select t).FirstOrDefault();
                    if (taxObj2 == null)
                    {

                        taxObj.Name = txtName.Text;
                        taxObj.TaxPercent = Convert.ToDecimal(txtPercent.Text);
                        taxObj.IsDelete = false;
                        posEntity.Taxes.Add(taxObj);
                        posEntity.SaveChanges();
                        MessageBox.Show("Successfully Saved!", "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);


                        currentTaxId = taxObj.Id;
                        dgvTaxList.DataSource = "";
                        dgvTaxList.DataSource = posEntity.Taxes.ToList();
                        Clear();
                        #region active Setting
                        Back_TaxData_ToSetting();
                        #endregion
                    }
                    else
                    {
                        tp.SetToolTip(txtName, "Error");
                        tp.Show("This tax name is already exist!", txtName);
                    }
                }
                else
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to update?", "Update", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (result.Equals(DialogResult.OK))
                    {
                        int count;
                        Tax taxObj = (from t in posEntity.Taxes where t.Id == currentId select t).FirstOrDefault();
                        count = (from t in posEntity.Taxes where t.Id != currentId && t.Name == txtName.Text select t).ToList().Count;
                        if (count == 0)
                        {

                            taxObj.Name = txtName.Text;
                            taxObj.TaxPercent = Convert.ToDecimal(txtPercent.Text);
                            taxObj.IsDelete = false;
                            posEntity.Entry(taxObj).State = EntityState.Modified;
                            MessageBox.Show("Successfully Update!", "Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            currentTaxId = taxObj.Id;
                            dgvTaxList.DataSource = "";
                            dgvTaxList.DataSource = posEntity.Taxes.ToList();
                            posEntity.SaveChanges();
                            Clear();
                            bool notbackoffice = Utility.IsNotBackOffice();
                            if (notbackoffice)
                            {
                                Utility.Gpvisible(groupBox1, false);
                            }
                            Back_TaxData_ToSetting();
                        }
                        else
                        {
                            tp.SetToolTip(txtName, "Error");
                            tp.Show("This tax name is already exist!", txtName);
                        }

                    }
                    else
                    {
                        Clear();

                    }

                }
            }
        }

        private void dgvTaxList_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                //Role Management
                RoleManagementController controller = new RoleManagementController();
                controller.Load(MemberShip.UserRoleId);
                if (controller.TaxRate.EditOrDelete || MemberShip.isAdmin)
                {
                    //to edit
                    if (e.ColumnIndex == 3)
                    {
                        bool notbackoffice = Utility.IsNotBackOffice();
                        isEdit = true;
                        DataGridViewRow row = dgvTaxList.Rows[e.RowIndex];
                        currentId = Convert.ToInt32(row.Cells[0].Value);
                        Tax taxObj1 = (from t in posEntity.Taxes where t.Id == currentId select t).FirstOrDefault();
                        txtName.Text = taxObj1.Name;
                        txtPercent.Text = taxObj1.TaxPercent.ToString();
                        lblStatus.Text = "UPDATE";
                        this.Text = "Edit Tax Rate";
                        groupBox1.Text = "Edit Tax Rate";
                        btnAdd.Image = Properties.Resources.update_small;
                        btnCancel.Visible = true;
                      
                        if (notbackoffice)
                        {
                            Utility.Gpvisible(groupBox1, isEdit);
                        }
                    }
                    if (e.ColumnIndex == 4)
                    {
                        DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (result.Equals(DialogResult.OK))
                        {
                            Clear();
                            DataGridViewRow row = dgvTaxList.Rows[e.RowIndex];
                            currentId = Convert.ToInt32(row.Cells[0].Value);
                            Tax taxObj1 = (from t in posEntity.Taxes where t.Id == currentId select t).FirstOrDefault();
                            int count = (from p in posEntity.Products where p.TaxId == currentId select p).ToList().Count;
                            string cId = currentId.ToString();
                            APP_Data.Setting sObj = (from s in posEntity.Settings where s.Value == cId && s.Key == "default_tax_rate" select s).FirstOrDefault();
                            if (count > 0 || sObj != null)
                            {
                                //To show message box 
                                MessageBox.Show("This product category is currently in use!", "Enable to delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                dgvTaxList.DataSource = "";
                                posEntity.Taxes.Remove(taxObj1);
                                posEntity.SaveChanges();
                                dgvTaxList.DataSource = posEntity.Taxes.ToList();
                                currentTaxId = 0;
                                Back_TaxData_ToSetting();
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("You are not allowed to edit/delete tax rate", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Taxes_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(txtName);
            tp.Hide(txtPercent);
        }
        #endregion

        #region Function

        private void Clear()
        {
            txtName.Text = "";
            txtPercent.Text = "";
            btnAdd.Text = "";

            lblStatus.Text = "Add";
            this.Text = "Add New Taxes";
            btnAdd.Image = Properties.Resources.add_small;
            btnCancel.Visible = false;
            bool notbackoffice = Utility.IsNotBackOffice();
            if (notbackoffice)
            {
                Utility.Gpvisible(groupBox1, false);
            }
        }

        private void Back_TaxData_ToSetting()
        {
            #region active PaidByCreditWithPrePaidDebt
            if (System.Windows.Forms.Application.OpenForms["Setting"] != null)
            {
                Setting newForm = (Setting)System.Windows.Forms.Application.OpenForms["Setting"];
                newForm.ReloadTax();
                newForm.SetCurrentTax(currentTaxId);
            }
            #endregion
            currentTaxId = 0;
        }
        #endregion

        private void txtPercent_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                 (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

    }
}
