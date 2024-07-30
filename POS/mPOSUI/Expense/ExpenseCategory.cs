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
    public partial class ExpenseCategory : Form
    {
        #region Variables

        POSEntities posEntity = new POSEntities();
        private ToolTip tp = new ToolTip();

        private bool isEdit = false;
        private int ExpCagId = 0;
        int currentId;
        #endregion

        #region Method
        private void Clear()
        {
            isEdit = false;
            this.Text = "Add New Expense Category";
            this.Text="Add New Expense Category";
            txtName.Text = string.Empty;
            ExpCagId = 0;
            btnAdd.Image = Properties.Resources.add_small;
            bool notbackoffice = Utility.IsNotBackOffice();
            if (notbackoffice)
            {
                Utility.Gpvisible(groupBox1, false);
            }
        }

        private void Back_Data()
        {
            #region active new Expense Entry
            if (System.Windows.Forms.Application.OpenForms["ExpenseEntry"] != null)
            {
                ExpenseEntry newForm = (ExpenseEntry)System.Windows.Forms.Application.OpenForms["ExpenseEntry"];
                newForm.Bind_ExpCag();
                newForm.SetCurrentExpCag(ExpCagId);
            }
            #endregion
          
        }

        #endregion

        #region Event
        public ExpenseCategory()
        {
            InitializeComponent();
        }
      
        private void ExpenseCategory_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            bool notbackoffice = Utility.IsNotBackOffice();
            if (notbackoffice)
            {
                Utility.Gpvisible(groupBox1, isEdit);
            }
            dgvExpCagList.AutoGenerateColumns = false;
            dgvExpCagList.DataSource = (from c in posEntity.ExpenseCategories select c).ToList();
        }

        //private void btnAdd_Click(object sender, EventArgs e)
        //{

        //    bool notbackoffice = Utility.IsNotBackOffice();
        //    if (notbackoffice)
        //    {
        //        Utility.Gpvisible(groupBox1, isEdit);
        //    }
        //    dgvExpCagList.AutoGenerateColumns = false;
        //    dgvExpCagList.DataSource = (from c in posEntity.ExpenseCategories where c.IsDelete == false select c).ToList();
        //}

        private void btnAdd_Click(object sender, EventArgs e)
        {

            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            if (txtName.Text.Trim() != string.Empty)
            {
                APP_Data.ExpenseCategory cObj = new APP_Data.ExpenseCategory();

                dgvExpCagList.DataSource = "";

                //Role Management
                RoleManagementController controller = new RoleManagementController();
                controller.Load(MemberShip.UserRoleId);

                //New Brand
                if (!isEdit)
                {
                    APP_Data.ExpenseCategory expCag = (from expCagobj in posEntity.ExpenseCategories where expCagobj.Name == txtName.Text && expCagobj.IsDelete == false select expCagobj).FirstOrDefault();
                    if (expCag == null)
                    {
                        if (controller.ExpenseCategory.Add || MemberShip.isAdmin)
                        {
                            cObj.Name = txtName.Text;
                            cObj.IsDelete = false;
                            posEntity.ExpenseCategories.Add(cObj);
                            posEntity.SaveChanges();

                            MessageBox.Show("Successfully Saved!", "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            ExpCagId = cObj.Id;
                            txtName.Text = "";
                        }
                        else
                        {

                            MessageBox.Show("You are not allowed to add new Expense Category", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }
                    else
                    {
                        tp.SetToolTip(txtName, "Error");
                        tp.Show("This Expense Category name is already exist!", txtName);
                    }
                }

                  //Edit Current Expense Category
                else
                {
                    APP_Data.ExpenseCategory expCag = (from expCagobj in posEntity.ExpenseCategories where expCagobj.Name == txtName.Text && expCagobj.Id != currentId && expCagobj.IsDelete == false select expCagobj).FirstOrDefault();
                    if (expCag == null)
                    {
                        if (controller.Brand.EditOrDelete || MemberShip.isAdmin)
                        {

                            APP_Data.ExpenseCategory EditexpCag = posEntity.ExpenseCategories.Where(x => x.Id == ExpCagId).FirstOrDefault();
                            EditexpCag.Name = txtName.Text.Trim();
                            posEntity.SaveChanges();



                            ExpCagId = EditexpCag.Id;
                            Clear();
                        }
                        else
                        {

                            MessageBox.Show("You are not allowed to edit expense category", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }
                    else
                    {

                        tp.SetToolTip(txtName, "Error");
                        tp.Show("This Expense Category name is already exist!", txtName);
                    }
                    bool notbackoffice = Utility.IsNotBackOffice();
                    if (notbackoffice)
                    {
                        Utility.Gpvisible(groupBox1, false);
                    }
                }



            }
            else
            {
                tp.SetToolTip(txtName, "Error");
                tp.Show("Please fill up Expense Category name!", txtName);
            }

            dgvExpCagList.DataSource = (from b in posEntity.ExpenseCategories where b.IsDelete == false orderby b.Id descending select b).ToList();
          
        }

        private void dgvExpCagList_CellClick(object sender, DataGridViewCellEventArgs e)
        {

    
            List<APP_Data.ExpenseCategory> cList = (from c in posEntity.ExpenseCategories select c).ToList();

            if (e.RowIndex >= 0)
            {
                //Edit
                if (e.ColumnIndex == 2)
                {
                    bool notbackoffice = Utility.IsNotBackOffice();
                    //Role Management
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.ExpenseCategory.EditOrDelete || MemberShip.isAdmin)
                    {
                        DataGridViewRow row = dgvExpCagList.Rows[e.RowIndex];
                        currentId = Convert.ToInt32(row.Cells[0].Value);

                        APP_Data.ExpenseCategory expCag = (from b in posEntity.ExpenseCategories where b.Id == currentId select b).FirstOrDefault();
                        txtName.Text = expCag.Name;
                        isEdit = true;
                        this.Text = "Edit Expense Category";
                        groupBox1.Text = "Edit Expense Category";
                        ExpCagId = expCag.Id;
                        btnAdd.Image = Properties.Resources.update_small;
                      
                        if (notbackoffice)
                        {
                            Utility.Gpvisible(groupBox1, isEdit);
                        }
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to edit Expense Category", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }

                //Delete
                if (e.ColumnIndex == 3)
                {

                    //Role Management
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.ExpenseCategory.EditOrDelete || MemberShip.isAdmin)
                    {

                        if (cList.Count == 1)
                        {
                            MessageBox.Show("Expense Category table should have at least one Expense Category!", "Enable to delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            if (result.Equals(DialogResult.OK))
                            {
                                Clear();
                                DataGridViewRow row = dgvExpCagList.Rows[e.RowIndex];
                                currentId = Convert.ToInt32(row.Cells[0].Value);
                                int count = (from t in posEntity.Expenses where t.ExpenseCategoryId == currentId select t).ToList().Count;
                                if (count < 1)
                                {
                                    dgvExpCagList.DataSource = "";
                                    APP_Data.ExpenseCategory expCag = (from c in posEntity.ExpenseCategories where c.Id == currentId select c).FirstOrDefault();
                                    posEntity.ExpenseCategories.Remove(expCag);
                                    posEntity.SaveChanges();
                                    dgvExpCagList.DataSource = (from c in posEntity.ExpenseCategories select c).ToList();
                                    MessageBox.Show("Successfully Deleted!", "Delete Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }

                                else
                                {
                                    //To show message box 
                                    MessageBox.Show("This Expense Category name is currently in use!", "Enable to delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to delete Expense Category", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }

            }
        }

        private void ExpenseCategory_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(txtName);
        }

        private void ExpenseCategory_FormClosing(object sender, FormClosingEventArgs e)
        {
            Back_Data();
        }

        #endregion

    }
}
