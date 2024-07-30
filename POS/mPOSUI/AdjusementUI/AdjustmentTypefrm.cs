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
    public partial class AdjustmentTypefrm : Form
    {
        #region Variables

        POSEntities posEntity = new POSEntities();
        private ToolTip tp = new ToolTip();

        private bool isEdit = false;
        private int AdjustmentTypeId = 0;
        #endregion

        #region Event
        public AdjustmentTypefrm()
        {
            InitializeComponent();
        }

        private void AdjustmentType_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            dgvAdjustmentTypeList.AutoGenerateColumns = false;
            dgvAdjustmentTypeList.DataSource = (from b in posEntity.AdjustmentTypes orderby b.Id descending select b).ToList();
        }

        private void Save()
        {
            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            if (txtName.Text.Trim() != string.Empty)
            {
                APP_Data.AdjustmentType AdjustmentType = new APP_Data.AdjustmentType();
                APP_Data.AdjustmentType bObj = (from b in posEntity.AdjustmentTypes where b.Name == txtName.Text select b).FirstOrDefault();
                if (bObj == null)
                {
                    dgvAdjustmentTypeList.DataSource = "";

                    //Role Management
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    
                    //New AdjustmentType
                    if (!isEdit)
                    {
                        //if (controller.AdjustmentType.Add || MemberShip.isAdmin)
                        //{
                            AdjustmentType.Name = txtName.Text;
                            posEntity.AdjustmentTypes.Add(AdjustmentType);
                            posEntity.SaveChanges();
                            dgvAdjustmentTypeList.DataSource = (from b in posEntity.AdjustmentTypes orderby b.Id descending select b).ToList();
                            AdjustmentTypeId = AdjustmentType.Id;
                        //}
                        //else
                        //{
                        //    dgvAdjustmentTypeList.DataSource = (from b in posEntity.AdjustmentTypes orderby b.Id descending select b).ToList();
                        //    MessageBox.Show("You are not allowed to add new AdjustmentType", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        //    return;
                        //}
                   
                    }
                    //Edit Current AdjustmentType
                    else
                    {
                        //if (controller.AdjustmentType.EditOrDelete || MemberShip.isAdmin)
                        //{

                            APP_Data.AdjustmentType EditAdjustmentType = posEntity.AdjustmentTypes.Where(x => x.Id == AdjustmentTypeId).FirstOrDefault();
                            EditAdjustmentType.Name = txtName.Text.Trim();
                            posEntity.SaveChanges();

                            dgvAdjustmentTypeList.DataSource = (from b in posEntity.AdjustmentTypes orderby b.Id descending select b).ToList();
                            Clear();
                            AdjustmentTypeId = EditAdjustmentType.Id;
                        //}
                        //else
                        //{
                        //    dgvAdjustmentTypeList.DataSource = (from b in posEntity.AdjustmentTypes orderby b.Id descending select b).ToList();
                        //    MessageBox.Show("You are not allowed to edit AdjustmentType", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        //    return;
                        //}
                     
                    }

                    // MessageBox.Show("Successfully Saved!", "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    #region active new AdjustmentType
                    Back_TypeData_To_Adjustment(AdjustmentTypeId);
                    #endregion

                }
                else
                {
                    tp.SetToolTip(txtName, "Error");
                    tp.Show("This AdjustmentType name is already exist!", txtName);
                }
            }
            else
            {
                tp.SetToolTip(txtName, "Error");
                tp.Show("Please fill up Type name!", txtName);
            }
            txtName.Text = "";
            txtName.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Save();
            
        }

        private void dgvAdjustmentTypeList_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            int currentId;
            if (e.RowIndex >= 0)
            {
                //Delete
                if (e.ColumnIndex == 3)
                {
                    //Role Management
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    //if (controller.AdjustmentType.EditOrDelete || MemberShip.isAdmin)
                    //{
                        DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (result.Equals(DialogResult.OK))
                        {
                            DataGridViewRow row = dgvAdjustmentTypeList.Rows[e.RowIndex];
                            currentId = Convert.ToInt32(row.Cells[0].Value);
                            int count = (from p in posEntity.Adjustments where p.AdjustmentTypeId == currentId select p).ToList().Count;
                            if (count < 1)
                            {
                                dgvAdjustmentTypeList.DataSource = "";
                                APP_Data.AdjustmentType AdjustmentTypeList = (from b in posEntity.AdjustmentTypes where b.Id == currentId select b).FirstOrDefault();
                                posEntity.AdjustmentTypes.Remove(AdjustmentTypeList);
                                posEntity.SaveChanges();
                                dgvAdjustmentTypeList.DataSource = (from adj in posEntity.AdjustmentTypes select adj).ToList();
                                MessageBox.Show("Successfully Deleted!", "Delete Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                AdjustmentTypeId = 0;
                                #region active new AdjustmentType
                                Back_TypeData_To_Adjustment(AdjustmentTypeId);
                                #endregion
                            }
                            else
                            {
                                //To show message box 
                                MessageBox.Show("This AdjustmentType name is currently in use!", "Enable to delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    //}
                    //else
                    //{
                    //    MessageBox.Show("You are not allowed to delete AdjustmentType", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //}
                }
                //Edit
                else if (e.ColumnIndex == 2)
                {

                    //Role Management
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    //if (controller.AdjustmentType.EditOrDelete || MemberShip.isAdmin)
                    //{

                        DataGridViewRow row = dgvAdjustmentTypeList.Rows[e.RowIndex];
                        currentId = Convert.ToInt32(row.Cells[0].Value);
                        AdjustmentTypeId=currentId;
                        APP_Data.AdjustmentType AdjustmentType = (from b in posEntity.AdjustmentTypes where b.Id == currentId select b).FirstOrDefault();
                        txtName.Text = AdjustmentType.Name;
                        isEdit = true;
                        this.Text = "Edit AdjustmentType";
                        AdjustmentTypeId = AdjustmentType.Id;
                        btnAdd.Image = Properties.Resources.save_small;
                    //}
                    //else
                    //{
                    //    MessageBox.Show("You are not allowed to edit AdjustmentType", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //}
                }
            }
        }

        private void AdjustmentType_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(txtName);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Save();               
            }
        }

        #endregion

        #region Method

        private void Clear()
        {
            isEdit = false;
            this.Text = "Add New  Type";
            txtName.Text = string.Empty;
            AdjustmentTypeId = 0;
            btnAdd.Image = Properties.Resources.add_small;
        }

        private void Back_TypeData_To_Adjustment(int AdjustmentTypeId)
        {
            #region active new AdjustmentType
            if (System.Windows.Forms.Application.OpenForms["AdjustmentFrm"] != null)
            {
                AdjustmentFrm newForm = (AdjustmentFrm)System.Windows.Forms.Application.OpenForms["AdjustmentFrm"];
                newForm.Bind_AdjustmentType();
                newForm.SetCurrentAdjustmentType(AdjustmentTypeId);
            }
            #endregion

            AdjustmentTypeId = 0;
        }
        #endregion


    }
}
