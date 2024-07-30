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
    public partial class OutstandingCustomerList : Form
    {
        private POSEntities entity = new POSEntities();
        List<CustomerInfoHolder> crlist = new List<CustomerInfoHolder>();

        public OutstandingCustomerList()
        {
            InitializeComponent();
            dgvCustomerList.AutoGenerateColumns = false;
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

        private void CustomerList_Resize(object sender, EventArgs e)
        {
            int height = this.Height;
            int width = this.Width;

            dgvCustomerList.Height = this.Height - 250;
            dgvCustomerList.Width = this.Width - 100;
            dgvCustomerList.Top = ((this.Height / 10) + 50);

            btnAddNewCustomer.Width = this.Width / 5;
            btnAddNewCustomer.Height = this.Height / 10;
        }

        private void OutstandingCustomerList_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            Utility.BindCustomer(cboCustomerName);
            DataBind();
        }

        private void dgvCustomerList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

            foreach (DataGridViewRow row in dgvCustomerList.Rows)
            {
                CustomerInfoHolder cInfo = (CustomerInfoHolder)row.DataBoundItem;

                row.Cells[0].Value = cInfo.Id.ToString();
                row.Cells[1].Value = cInfo.Name.ToString();
                row.Cells[2].Value = cInfo.PhNo.ToString();
                row.Cells[3].Value = cInfo.PayableAmount;
                row.Cells[4].Value = cInfo.RefundAmount;
            }



        }

        private void dgvCustomerList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                //Delete
                if (e.ColumnIndex == 7)
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
                            Customer cust = (Customer)row.DataBoundItem;
                            cust = (from c in entity.Customers where c.Id == cust.Id select c).FirstOrDefault<Customer>();

                            //Need to recheck
                            if (cust.Transactions.Count > 0)
                            {
                                MessageBox.Show("This customer has outstanding amount!", "Unable to Delete");
                                return;
                            }
                            else
                            {
                                entity.Customers.Remove(cust);
                                entity.SaveChanges();
                                DataBind();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to edit customer", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                //Edit
                else if (e.ColumnIndex == 6)
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
                //View Detail
                else if (e.ColumnIndex == 5)
                {
                    //Role Management
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.OutstandingCustomer.ViewDetail || MemberShip.isAdmin)
                    {
                        //Show Customer Detail Form
                        CustomerDetail form = new CustomerDetail();
                        form.customerId = Convert.ToInt32(dgvCustomerList.Rows[e.RowIndex].Cells[0].Value);
                        form.TotalOutstanding = Convert.ToInt64(dgvCustomerList.Rows[e.RowIndex].Cells[3].Value);
                        form.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to view outstanding customer detail", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void DataBind()
        {
            List<Customer> customerList = new List<Customer>();
            entity = new POSEntities();
            crlist.Clear();
            int _cusId = 0;

            //customerList = (from c in entity.Customers select c).ToList();
            if (cboCustomerName.SelectedIndex != 0)
            {
                _cusId = Convert.ToInt32(cboCustomerName.SelectedValue);

            }


            customerList = (from c in entity.Customers
                            join t in entity.Transactions on c.Id equals t.CustomerId
                            where (t.Type == "Credit" || t.Type == "CreditRefund") && (_cusId == 0 && 1 == 1 || _cusId != 0 && c.Id == _cusId)
                            && (t.ShopId == SettingController.DefaultShop.Id)
                            select c).Distinct().ToList();


            foreach (Customer c in customerList)
            {
                int totalDebt = 0; long totalRefund = 0;
                CustomerInfoHolder crObj = new CustomerInfoHolder();

                crObj.Id = c.Id;
                crObj.Name = c.Name;
                crObj.PhNo = c.PhoneNumber;
                List<Transaction> rtList = new List<Transaction>();

                foreach (Transaction tf in c.Transactions)
                {
                    if (tf.Type == "Credit" && tf.IsPaid == false && tf.IsDeleted == false)
                    {
                        totalDebt += (int)((tf.TotalAmount) - tf.RecieveAmount);
                        rtList = (from rt in entity.Transactions where rt.Type == TransactionType.CreditRefund && rt.ParentId == tf.Id && rt.IsDeleted == false select rt).ToList();

                        if (rtList.Count > 0)
                        {
                            foreach (Transaction rt in rtList)
                            {
                                totalDebt -= (int)rt.RecieveAmount;
                            }
                        }

                        if (tf.UsePrePaidDebts != null)
                        {
                            totalDebt -= Convert.ToInt32(tf.UsePrePaidDebts.Sum(x => x.UseAmount));
                        }

                        if (tf.CancellationLogs != null)
                        {
                            totalDebt -= Convert.ToInt32(tf.CancellationLogs.Sum(x => x.CancelledAmount));
                        }

                    }

                    if (tf.Type == TransactionType.CreditRefund && tf.IsDeleted == false)
                    {
                        totalRefund += (long)tf.RecieveAmount;
                    }                  
                }
                if (totalDebt > 0)
                {                       
                    crObj.PayableAmount = totalDebt;
                    crObj.RefundAmount = totalRefund;

                    crlist.Add(crObj);
                }
           }
            dgvCustomerList.DataSource = null;
            dgvCustomerList.DataSource = crlist;
        }

        private void OutstandingCustomerList_Activated(object sender, EventArgs e)
        {
            DataBind();
        }

        private void cboCustomerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataBind();
        }

        private void dgvCustomerList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
