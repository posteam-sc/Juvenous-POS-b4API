
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Objects;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using POS.APP_Data;
using POS.mPOSUI.CustomerPurchaseInvoice;

namespace POS
{
    public partial class CustomerList : Form
    {
        #region Variables

        private POSEntities entity = new POSEntities();

        #endregion

        #region Event
        public CustomerList()
        {
            InitializeComponent();
        }

        private void CustomerList_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            dgvCustomerList.AutoGenerateColumns = false;
            Bind_MemberType();
            VisbleByRadio();
            LoadData();
        }

        public void DataBind()
        {
            entity = new POSEntities();
            dgvCustomerList.DataSource = entity.GetCustomerList(0).ToList(); //(from c in entity.Customers where c.CustomerTypeId == 1 select c).ToList();
        }

        private void dgvCustomerList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvCustomerList.Rows)
            {
                // Customer cs = (Customer)row.DataBoundItem;               

                // row.Cells[5].Value = Loc_CustomerPointSystem.GetPointFromCustomerId(cs.Id).ToString();
            }
        }

        private void dgvCustomerList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                //View detail information of customer
                if (e.ColumnIndex == ColViewDetail.Index)
                {
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.Customer.ViewDetail || MemberShip.isAdmin)
                    {
                        if (System.Windows.Forms.Application.OpenForms["CustomerDetailInfo"] != null)
                        {
                            CustomerDetailInfo newForm = (CustomerDetailInfo)System.Windows.Forms.Application.OpenForms["CustomerDetailInfo"];
                            newForm.customerId = Convert.ToInt32(dgvCustomerList.Rows[e.RowIndex].Cells[0].Value);
                            newForm.ShowDialog();
                        }
                        else
                        {
                            CustomerDetailInfo newForm = new CustomerDetailInfo();
                            newForm.customerId = Convert.ToInt32(dgvCustomerList.Rows[e.RowIndex].Cells[0].Value);
                            newForm.ShowDialog();
                        }
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to view detail  customer", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                }
                else if (e.ColumnIndex == ColViewPackage.Index)
                {
                    CustomerOffsetfrm customerOffsetForm = new CustomerOffsetfrm();
                    customerOffsetForm.customerID = Convert.ToInt32(dgvCustomerList.Rows[e.RowIndex].Cells[0].Value);
                    customerOffsetForm.ShowDialog();
                    //CustomerPurchaseInvoice customerPurchaseInvoiceForm = new CustomerPurchaseInvoice();
                    //customerPurchaseInvoiceForm.editCustomerID= Convert.ToInt32(dgvCustomerList.Rows[e.RowIndex].Cells[0].Value);
                    //customerPurchaseInvoiceForm.ShowDialog();
                }
                //Edit this User
                else if (e.ColumnIndex == ColEdit.Index)
                {
                    //Role Management
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.Customer.EditOrDelete || MemberShip.isAdmin)
                    {
                        NewCustomer form = new NewCustomer();
                        form.isEdit = true;
                        form.Text = "Edit Customer";
                        form.CustomerId = Convert.ToInt32(dgvCustomerList.Rows[e.RowIndex].Cells[0].Value);
                        form.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to edit customer", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                //Delete this User
                else if (e.ColumnIndex == ColDelete.Index)
                {
                    //Role Management
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.Customer.EditOrDelete || MemberShip.isAdmin)
                    {

                        DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (result.Equals(DialogResult.OK))
                        {
                            DataGridViewRow row = dgvCustomerList.Rows[e.RowIndex];
                            // cust = (Customer)row.DataBoundItem;
                            int customerId = Int32.Parse(row.Cells[0].Value.ToString());
                            Customer cust = (from c in entity.Customers where c.Id == customerId select c).FirstOrDefault<Customer>();

                            //Need to recheck
                            if (cust.Transactions.Count > 0)
                            {
                                MessageBox.Show("This customer already made transactions!", "Unable to Delete");
                                return;
                            }
                            else
                            {
                                entity.Customers.Remove(cust);
                                entity.SaveChanges();
                                LoadData();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to delete customer", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                if (System.Windows.Forms.Application.OpenForms["Sales"] != null)
                {
                    Sales newForm = (Sales)System.Windows.Forms.Application.OpenForms["Sales"];
                    newForm.Clear();
                }
            }
        }

        private void btnAddNewCustomer_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.Customer.Add || MemberShip.isAdmin)
            {
                NewCustomer form = new NewCustomer();
                form.isEdit = false;
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to add new customer", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        #endregion

        #region Function

        private void Bind_MemberType()
        {
            List<APP_Data.MemberType> mTypeList = new List<APP_Data.MemberType>();
            APP_Data.MemberType mType = new APP_Data.MemberType();
            mType.Id = 0;
            mType.Name = "All";
            mTypeList.Add(mType);
            mTypeList.AddRange(entity.MemberTypes.ToList());
            cboMemberType.DataSource = mTypeList;
            cboMemberType.DisplayMember = "Name";
            cboMemberType.ValueMember = "Id";
        }

        private void LoadData()
        {
            // List<Customer> customerList = new List<Customer>();
            List<GetCustomerList_Result> customerList = new List<GetCustomerList_Result>();

            if (cboMemberType.SelectedIndex == 0)
            {
                // customerList = (from c in entity.Customers where c.CustomerTypeId == 1 select c).ToList();
                customerList = entity.GetCustomerList(0).ToList();
            }
            else
            {
                //customerList = (from c in entity.Customers.AsEnumerable() where c.MemberTypeID == Convert.ToInt32(cboMemberType.SelectedValue) select c).ToList();
                customerList = entity.GetCustomerList(Convert.ToInt32(cboMemberType.SelectedValue)).ToList();
            }

            if (txtSearch.Visible == true)
            {
                if (txtSearch.Text.Trim() != string.Empty)
                {
                    if (rdoMemberCardNo.Checked)
                    {
                        //Search BY Member Card No
                        customerList = customerList.Where(x => x.VIPMemberId == txtSearch.Text.Trim()).ToList();
                    }
                    else if (rdoCustomerName.Checked)
                    {
                        //Search BY Customer Name 
                        customerList = customerList.Where(x => x.Name.Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower())).ToList();
                    }
                    else if (rdoCustomerCode.Checked)
                    {
                        //Search BY Customer ID 
                        customerList = customerList.Where(x => x.CustomerCode.Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower())).ToList();
                    }

                }
            }
            else
            {
                if (rdoBirthDate.Checked)
                {
                    DateTime fromDate = dtpBirthday.Value.Date;

                    var filterCustomer = (from c in customerList where c.Birthday != null select c).ToList();
                    customerList = (from f in filterCustomer where f.Birthday.Value.Date == fromDate select f).ToList<GetCustomerList_Result>();
                    // customerList = (from f in filterCustomer where f.Birthday.Value.Date == fromDate select f).ToList<Customer>();
                }
                else if (rdoBirth_Day.Checked)
                {
                    DateTime fromDate = dtpBirthday.Value.Date;

                    var filterCustomer = (from c in customerList where c.Birthday != null select c).ToList();
                    customerList = (from f in filterCustomer where (f.Birthday.Value.Date.Day == fromDate.Day && f.Birthday.Value.Date.Month == fromDate.Month) select f).ToList<GetCustomerList_Result>();
                }
                else if (rdoBirthMonth.Checked)
                {
                    DateTime fromDate = dtpBirthday.Value.Date;

                    var filterCustomer = (from c in customerList where c.Birthday != null select c).ToList();
                    customerList = (from f in filterCustomer where f.Birthday.Value.Date.Month == fromDate.Month select f).ToList<GetCustomerList_Result>();
                }
            }


            dgvCustomerList.DataSource = customerList;
            if (customerList.Count == 0)
            {
                MessageBox.Show("Item not found!", "Cannot find");
            }
        }

        #endregion

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            dgvCustomerList.DataSource = entity.Customers.ToList();
            txtSearch.Text = "";
            rdoMemberCardNo.Checked = true;
            cboMemberType.Text = "All";
            LoadData();
        }

        private void rdoMemberCardNo_CheckedChanged(object sender, EventArgs e)
        {
            lblSearchTitle.Text = "Member Card No.";
            VisibleControl(true, false);
        }

        private void rdoCustomerName_CheckedChanged(object sender, EventArgs e)
        {
            lblSearchTitle.Text = "Patient Name";
            VisibleControl(true, false);
        }

        private void rdoBirthday_CheckedChanged(object sender, EventArgs e)
        {
            lblSearchTitle.Text = "Date Of Birth";
            VisibleControl(false, true);
        }

        private void VisibleControl(bool t, bool f)
        {
            txtSearch.Visible = t;
            dtpBirthday.Visible = f;
        }

        private void VisbleByRadio()
        {
            if (rdoMemberCardNo.Checked == true || rdoCustomerName.Checked == true || rdoCustomerCode.Checked == true)
            {
                VisibleControl(true, false);
            }
            else
            {
                VisibleControl(false, true);
            }
        }

        private void cboMemberType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void rdoCustomerCode_CheckedChanged(object sender, EventArgs e)
        {
            lblSearchTitle.Text = "Patient ID";
            VisibleControl(true, false);
        }

        private void rdoBirth_Day_CheckedChanged(object sender, EventArgs e)
        {
            lblSearchTitle.Text = "BirthDay";            
            dtpBirthday.Value = DateTime.Now;
            VisibleControl(false, true);
        }

        private void rdoBirthMonth_CheckedChanged(object sender, EventArgs e)
        {

            lblSearchTitle.Text = "BirthMonth";
            dtpBirthday.Value = DateTime.Now;
            VisibleControl(false, true);
        }
    }
}
