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
    public partial class newMemberType : Form
    {
        #region Variables
        public string memType = string.Empty;
        POSEntities posEntity = new POSEntities();
        private ToolTip tp = new ToolTip();
        private bool isEdit = false;
        private int memberId = 0;
        #endregion

        public newMemberType()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            if (txtType.Text.Trim() != string.Empty)
            {
                APP_Data.MemberType mType = new APP_Data.MemberType();
                APP_Data.MemberType bObj = (from b in posEntity.MemberTypes where b.Name == txtType.Text select b).FirstOrDefault();
                if (bObj == null)
                {
                    memType = txtType.Text.Trim();
                    dgvMemberList.DataSource = "";

                    //Role Management
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);

                    //New Member
                    if (!isEdit)
                    {
                        if (controller.MemberRule.Add || MemberShip.isAdmin)
                        {
                            mType.Name = txtType.Text;
                            mType.IsDelete = false;
                            posEntity.MemberTypes.Add(mType);
                            posEntity.SaveChanges();
                            dgvMemberList.DataSource = (from m in posEntity.MemberTypes orderby m.Id descending select m).ToList();

                        }
                        else
                        {
                            dgvMemberList.DataSource = (from m in posEntity.MemberTypes orderby m.Id descending select m).ToList();
                            MessageBox.Show("You are not allowed to add new Member", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }
                    //Edit Current member
                    else
                    {
                        if (controller.MemberRule.EditOrDelete || MemberShip.isAdmin)
                        {

                            APP_Data.MemberType EditMember = posEntity.MemberTypes.Where(x => x.Id == memberId).FirstOrDefault();
                            EditMember.Name = txtType.Text.Trim();
                            posEntity.SaveChanges();

                            dgvMemberList.DataSource = (from m in posEntity.MemberTypes orderby m.Id descending select m).ToList();
                            Clear();
                        }
                        else
                        {
                            dgvMemberList.DataSource = (from m in posEntity.MemberTypes orderby m.Id descending select m).ToList();
                            MessageBox.Show("You are not allowed to edit member", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        bool notbackoffice = Utility.IsNotBackOffice();
                        if (notbackoffice)
                        {
                            Utility.Gpvisible(groupBox1, false);
                        }
                    }

                    MessageBox.Show("Successfully Saved!", "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    #region active new member rule
                    if (System.Windows.Forms.Application.OpenForms["MemberRule"] != null)
                    {
                        MemberRule newForm = (MemberRule)System.Windows.Forms.Application.OpenForms["MemberRule"];
                        newForm.reloadMember(memType);
                        this.Close();
                    }

                    if (System.Windows.Forms.Application.OpenForms["NewCustomer"] != null)
                    {
                        NewCustomer newForm = (NewCustomer)System.Windows.Forms.Application.OpenForms["NewCustomer"];

                        newForm.reloadMemberType(memType);
                        this.Close();
                    }
                    #endregion
                }
                else
                {
                    tp.SetToolTip(txtType, "Error");
                    tp.Show("This member name is already exist!", txtType);
                }
            }
            else
            {
                tp.SetToolTip(txtType, "Error");
                tp.Show("Please fill up member name!", txtType);
            }
            txtType.Text = "";
        }

        private void MemberType_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            bool notbackoffice = Utility.IsNotBackOffice();
            if (notbackoffice)
            {
                Utility.Gpvisible(groupBox1, isEdit);
            }
            dgvMemberList.AutoGenerateColumns = false;
            dgvMemberList.DataSource = (from m in posEntity.MemberTypes orderby m.Id descending select m).ToList();
        }

        private void newMemberType_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(txtType);
        }

        private void Clear()
        {
            isEdit = false;
            txtType.Text = string.Empty;
            memberId = 0;
            btnAdd.Image = Properties.Resources.add_small;
            this.Text = "Add Member Card Type";
            bool notbackoffice = Utility.IsNotBackOffice();
            if (notbackoffice)
            {
                Utility.Gpvisible(groupBox1, false);
            }
        }

        private void dgvMemberList_CellClick(object sender, DataGridViewCellEventArgs e)
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
                    if (controller.MemberRule.EditOrDelete || MemberShip.isAdmin)
                    {
                        DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (result.Equals(DialogResult.OK))
                        {
                            DataGridViewRow row = dgvMemberList.Rows[e.RowIndex];
                            currentId = Convert.ToInt32(row.Cells[0].Value);
                            int count = (from p in posEntity.MemberCardRules where p.MemberTypeId == currentId select p).ToList().Count;
                            if (count < 1)
                            {
                                dgvMemberList.DataSource = "";
                                APP_Data.MemberType mType = (from b in posEntity.MemberTypes where b.Id == currentId select b).FirstOrDefault();
                                mType.IsDelete = true;
                                posEntity.SaveChanges();
                                dgvMemberList.DataSource = (from m in posEntity.MemberTypes select m).ToList();
                                MessageBox.Show("Successfully Deleted!", "Delete Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                if (System.Windows.Forms.Application.OpenForms["MemberRule"] != null)
                                {
                                    MemberRule Form = (MemberRule)System.Windows.Forms.Application.OpenForms["MemberRule"];
                                    Form.reloadMember();
                                    this.Close();
                                }

                                if (System.Windows.Forms.Application.OpenForms["NewCustomer"] != null)
                                {
                                    NewCustomer newForm = (NewCustomer)System.Windows.Forms.Application.OpenForms["NewCustomer"];

                                    newForm.reloadMemberType("Select");
                                    this.Close();
                                }
                            }
                            else
                            {
                                //To show message box 
                                MessageBox.Show("This member name is currently in use!", "Unable to delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to delete member", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                //Edit
                else if (e.ColumnIndex == 2)
                {
                    bool notbackoffice = Utility.IsNotBackOffice();
                    //Role Management
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.MemberRule.EditOrDelete || MemberShip.isAdmin)
                    {

                        DataGridViewRow row = dgvMemberList.Rows[e.RowIndex];
                        currentId = Convert.ToInt32(row.Cells[0].Value);

                        APP_Data.MemberType mType = (from b in posEntity.MemberTypes where b.Id == currentId select b).FirstOrDefault();
                        txtType.Text = mType.Name;
                        this.Text = "Edit Member Card Type";
                        isEdit = true;
                        memberId = mType.Id;
                        btnAdd.Image = Properties.Resources.update_small;
                        if (notbackoffice)
                        {
                            Utility.Gpvisible(groupBox1, isEdit);
                        }
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to edit member", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }
    }
}
