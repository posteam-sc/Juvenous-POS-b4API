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
    public partial class UnitForm : Form
    {
        #region Variables

        POSEntities entity = new POSEntities();
        private ToolTip tp = new ToolTip();
        private bool isEdit = false;
        private int UnitId = 0;
        #endregion
        #region Event
        public UnitForm()
        {
            InitializeComponent();
        }

        private void Unit_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            bool notbackoffice = Utility.IsNotBackOffice();
            if (notbackoffice)
            {
                Utility.Gpvisible(groupBox1, isEdit);
            }
            dgvUnitList.AutoGenerateColumns = false;
            DataBind();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            bool hasError = false;
            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            if (txtName.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtName, "Error");
                tp.Show("Please fill up unit name!", txtName);
                hasError = true;
            }
            if (!hasError)
            {
                APP_Data.Unit unitObj1 = new APP_Data.Unit();
                APP_Data.Unit unitObj2 = (from u in entity.Units where u.UnitName == txtName.Text select u).FirstOrDefault();
                if (unitObj2 == null)
                {
                    if (!isEdit)
                    {
                        unitObj1.UnitName = txtName.Text;
                        entity.Units.Add(unitObj1);
                        entity.SaveChanges();
                        DataBind();
                        UnitId = unitObj1.Id;
                        MessageBox.Show("Successfully Saved!", "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        APP_Data.Unit EditUnit = entity.Units.Where(x => x.Id == UnitId).FirstOrDefault();
                        EditUnit.UnitName = txtName.Text.Trim();
                        entity.SaveChanges();

                        dgvUnitList.DataSource = (from b in entity.Units orderby b.Id descending select b).ToList();
                        Clear();
                        UnitId = EditUnit.Id;
                        bool notbackoffice = Utility.IsNotBackOffice();
                        if (notbackoffice)
                        {
                            Utility.Gpvisible(groupBox1, false);
                        }
                    }

                    #region active new Product
                    if (System.Windows.Forms.Application.OpenForms["NewProduct"] != null)
                    {
                        NewProduct newForm = (NewProduct)System.Windows.Forms.Application.OpenForms["NewProduct"];
                        newForm.ReloadUnit();
                        newForm.SetCurrentUnit(UnitId);
                    }
                    #endregion
                }
                else
                {
                    tp.SetToolTip(txtName, "Error");
                    tp.Show("This unit name is already exist!", txtName);
                }
                txtName.Text = "";
            }
        }

        #endregion

        #region Function
        private void Clear()
        {
            isEdit = false;
            this.Text = "Add New Unit";
            groupBox1.Text = "Add New Unit";
            txtName.Text = string.Empty;
            UnitId = 0;
            btnAdd.Image = Properties.Resources.add_small;
          
            bool notbackoffice = Utility.IsNotBackOffice();
            if (notbackoffice)
            {
                Utility.Gpvisible(groupBox2, false);
            }
        }

        private void DataBind()
        {
            entity = new POSEntities();
            dgvUnitList.DataSource = (from u in entity.Units orderby u.Id descending select u).ToList();
        }

        #endregion

        private void dgvUnitList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentId;
            if (e.RowIndex >= 0)
            {
                //Role Management
                RoleManagementController controller = new RoleManagementController();
                controller.Load(MemberShip.UserRoleId);
                if (controller.MeasurementUnit.EditOrDelete || MemberShip.isAdmin)
                {

                    //Edit
                    if (e.ColumnIndex == 2)
                    {

                        //Role Management
                        bool notbackoffice = Utility.IsNotBackOffice();
                        DataGridViewRow row = dgvUnitList.Rows[e.RowIndex];
                        currentId = Convert.ToInt32(row.Cells[0].Value);

                        APP_Data.Unit Unit = (from b in entity.Units where b.Id == currentId select b).FirstOrDefault();
                        txtName.Text = Unit.UnitName;
                        isEdit = true;
                        this.Text = "Edit Unit";
                        groupBox1.Text = "Edit Unit";
                        UnitId = Unit.Id;
                        btnAdd.Image = Properties.Resources.update_small;
                      
                        if (notbackoffice)
                        {
                            Utility.Gpvisible(groupBox1, isEdit);
                        }

                    }
                    //delete
                    if (e.ColumnIndex == 3)
                    {
                        DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (result.Equals(DialogResult.OK))
                        {
                            Clear();
                            DataGridViewRow row = dgvUnitList.Rows[e.RowIndex];
                            currentId = Convert.ToInt32(row.Cells[0].Value);
                            int count = (from p in entity.Products where p.UnitId == currentId select p).ToList().Count;
                            if (count < 1)
                            {
                                APP_Data.Unit DeleteObj = (from u in entity.Units where u.Id == currentId select u).FirstOrDefault();
                                entity.Units.Remove(DeleteObj);
                                entity.SaveChanges();
                                DataBind();
                                MessageBox.Show("Successfully Deleted!", "Delete Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                #region active new Product
                                if (System.Windows.Forms.Application.OpenForms["NewProduct"] != null)
                                {
                                    NewProduct newForm = (NewProduct)System.Windows.Forms.Application.OpenForms["NewProduct"];
                                    newForm.ReloadUnit();
                                    newForm.SetCurrentUnit(0);
                                }
                                #endregion
                            }
                            else
                            {
                                MessageBox.Show("This unit name is currently in use!", "Enable to delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("You are not allowed to edit/delete measurement unit", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void UnitForm_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(txtName);
        }
    }
}
