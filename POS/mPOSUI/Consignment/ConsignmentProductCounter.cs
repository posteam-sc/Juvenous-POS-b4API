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
    public partial class ConsignmentProductCounter : Form
    {
        #region Variables

        POSEntities entity = new POSEntities();

        private ToolTip tp = new ToolTip();

        private bool isEdit = false;

        private int CounterId = 0;

        #endregion

        #region Event
        public ConsignmentProductCounter()
        {
            InitializeComponent();
        }

        private void ConsignmentProductCounter_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            bool notbackoffice = Utility.IsNotBackOffice();
            if (notbackoffice)
            {
                Utility.Gpvisible(groupBox1, isEdit);
            }
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
                tp.Show("Please fill up consignment counter name!", txtName);
                hasError = true;
            }
            //else if (txtPhone.Text.Trim() == string.Empty)
            //{
            //    tp.SetToolTip(txtPhone, "Error");
            //    tp.Show("Please fill up phone number!", txtPhone);
            //    hasError = true;
            //}
            //else if (txtEmail.Text.Trim() == string.Empty)
            //{
            //    tp.SetToolTip(txtEmail, "Error");
            //    tp.Show("Please fill up email address!", txtEmail);
            //    hasError = true;
            //}
            //else if (txtCounterLocation.Text.Trim() == string.Empty)
            //{
            //    tp.SetToolTip(txtCounterLocation, "Error");
            //    tp.Show("Please fill up counter address!", txtCounterLocation);
            //    hasError = true;
            //}

            if (!hasError)
            {
                //new
                if (!isEdit)
                {
                    ConsignmentCounter consignmentCounterObj1 = new ConsignmentCounter();
                    ConsignmentCounter consignmentCounterObj2 = (from c in entity.ConsignmentCounters where c.Name == txtName.Text select c).FirstOrDefault();
                    if (consignmentCounterObj2 == null)
                    {
                        consignmentCounterObj1.Name = txtName.Text;
                        consignmentCounterObj1.PhoneNo = txtPhone.Text;
                        consignmentCounterObj1.Email = txtEmail.Text;
                        consignmentCounterObj1.IsDelete = false;
                        consignmentCounterObj1.Address = txtCounterLocation.Text;
                        entity.ConsignmentCounters.Add(consignmentCounterObj1);
                        entity.SaveChanges();
                        DataBind();
                        Clear();
                        MessageBox.Show("Successfully Saved!", "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        tp.SetToolTip(txtName, "Error");
                        tp.Show("This consignment counter name is already exist!", txtName);
                    }

                
                    if (System.Windows.Forms.Application.OpenForms["NewProduct"] != null)
                    {
                        NewProduct newForm = (NewProduct)System.Windows.Forms.Application.OpenForms["NewProduct"];
                        newForm.ReloadConsignor();
                        newForm.SetCurrentConsignor(consignmentCounterObj1.Id);
                    }
                }
                //edit
                else
                {
                    APP_Data.ConsignmentCounter editCounter = (from c in entity.ConsignmentCounters where c.Id == CounterId select c).FirstOrDefault();
                    int searchCounter = (from c in entity.ConsignmentCounters where c.Id != CounterId && c.Name==txtName.Text select c).Count();
                    if (searchCounter > 0)
                    {
                        tp.SetToolTip(txtName, "Error");
                        tp.Show("This consignment counter name is already exist!", txtName);
                    }
                    else
                    {
                        editCounter.Name = txtName.Text;
                        editCounter.PhoneNo = txtPhone.Text;
                        editCounter.Email = txtEmail.Text;
                        editCounter.Address = txtCounterLocation.Text;
                        entity.SaveChanges();
                        DataBind();
                        Clear();
                        MessageBox.Show("Successfully Saved!", "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        if (System.Windows.Forms.Application.OpenForms["NewProduct"] != null)
                        {
                            NewProduct newForm = (NewProduct)System.Windows.Forms.Application.OpenForms["NewProduct"];
                            newForm.ReloadConsignor();
                            newForm.SetCurrentConsignor(editCounter.Id);
                        }
                    }
                }
            }
        }

        private void dgvCosignmentCounterList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                //Role Management
                RoleManagementController controller = new RoleManagementController();
                controller.Load(MemberShip.UserRoleId);
                if (controller.Consignor.EditOrDelete || MemberShip.isAdmin)
                {
                //edit
                if (e.ColumnIndex == 5)
                {

                    bool notbackoffice = Utility.IsNotBackOffice();
                    DataGridViewRow row = dgvCosignmentCounterList.Rows[e.RowIndex];
                    int currentId = Convert.ToInt32(row.Cells[0].Value);
                    ConsignmentCounter counter = (from p in entity.ConsignmentCounters where p.Id == currentId select p).SingleOrDefault();
                    txtName.Text = counter.Name;
                    txtPhone.Text = counter.PhoneNo;
                    txtEmail.Text = counter.Email;
                    txtCounterLocation.Text = counter.Address;
                    isEdit = true;
                    CounterId = currentId;
                    btnAdd.Image = Properties.Resources.update_small;
                    this.Text = "Edit Consignor";

              
                    if (notbackoffice)
                    {
                        Utility.Gpvisible(groupBox1, isEdit);
                    }
                }
                //delete
                if (e.ColumnIndex == 6)
                {

                    DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (result.Equals(DialogResult.OK))
                    {
                        DataGridViewRow row = dgvCosignmentCounterList.Rows[e.RowIndex];
                        int currentId = Convert.ToInt32(row.Cells[0].Value);
                        int count = (from p in entity.Products where p.ConsignmentCounterId == currentId select p).ToList().Count;
                        if (count < 1)
                        {
                            ConsignmentCounter DeleteObj = (from c in entity.ConsignmentCounters where c.Id == currentId select c).FirstOrDefault();
                            DeleteObj.IsDelete = true;
                            entity.SaveChanges();
                            DataBind();
                            MessageBox.Show("Successfully Deleted!", "Delete Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);


                            if (System.Windows.Forms.Application.OpenForms["NewProduct"] != null)
                            {
                                NewProduct newForm = (NewProduct)System.Windows.Forms.Application.OpenForms["NewProduct"];
                                newForm.ReloadConsignor();
                                newForm.SetCurrentConsignor(0);
                            }
                        }
                        else
                        {
                            MessageBox.Show("This consignment counter name is currently in use!", "Enable to delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }
                else
                {
                    MessageBox.Show("You are not allowed to edit/delete consignor", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
           
        }

        #endregion
        #region Function

        private void DataBind()
        {
            bool notbackoffice = Utility.IsNotBackOffice();
            if (notbackoffice)
            {
                Utility.Gpvisible(groupBox1, false);
            }
            entity = new POSEntities();
            dgvCosignmentCounterList.AutoGenerateColumns = false;
            dgvCosignmentCounterList.DataSource = entity.ConsignmentCounters.ToList();
        }

        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            isEdit = false;
            txtName.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtCounterLocation.Text = string.Empty;
            CounterId = 0;
            btnAdd.Image = Properties.Resources.add_small;
            this.Text = "Add New Consignor";
            bool notbackoffice = Utility.IsNotBackOffice();
            if (notbackoffice)
            {
                Utility.Gpvisible(groupBox1, false);
            }
        }

        private void tableLayoutPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(txtName);
            tp.Hide(txtPhone);
            tp.Hide(txtEmail);
            tp.Hide(txtCounterLocation);
        }
    }
}
