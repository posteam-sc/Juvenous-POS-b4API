
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Objects;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
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

        private void btnExport_Click(object sender, EventArgs e)
        {
            List<GetCustomerList_Result> customerList = new List<GetCustomerList_Result>();
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                Title = "Save an Excel File"
            };

            customerList = entity.GetCustomerList(0).ToList();

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                // Check if filePath is valid
                if (string.IsNullOrEmpty(filePath))
                {
                    MessageBox.Show("Invalid file path.");
                    return;
                }

                try
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Customers");

                        // Add headers
                        worksheet.Cell(1, 1).Value = "Id";
                        worksheet.Cell(1, 2).Value = "Title";
                        worksheet.Cell(1, 3).Value = "Name	";
                        worksheet.Cell(1, 4).Value = "PhoneNumber	";
                        worksheet.Cell(1, 5).Value = "Address	";
                        worksheet.Cell(1, 6).Value = "NRC	";
                        worksheet.Cell(1, 7).Value = "Email	";
                        worksheet.Cell(1, 8).Value = "CityId	";
                        worksheet.Cell(1, 9).Value = "TownShip	";
                        worksheet.Cell(1, 10).Value = "Gender	";
                        worksheet.Cell(1, 11).Value = "Birthday	";
                        worksheet.Cell(1, 12).Value = "MemberTypeID	";
                        worksheet.Cell(1, 13).Value = "VIPMemberId	";
                        worksheet.Cell(1, 14).Value = "StartDate	";
                        worksheet.Cell(1, 15).Value = "CustomerCode	";
                        worksheet.Cell(1, 16).Value = "CustomerTypeId	";
                        worksheet.Cell(1, 17).Value = "PromoteDate";
                        worksheet.Cell(1, 18).Value = "Maritalstatus	";
                        worksheet.Cell(1, 19).Value = "EmergencyContactPhone	";
                        worksheet.Cell(1, 20).Value = "EmergencyContactName	";
                        worksheet.Cell(1, 21).Value = "Relationship	";
                        worksheet.Cell(1, 22).Value = "MainConcern	";
                        worksheet.Cell(1, 23).Value = "MedicalHistory	";
                        worksheet.Cell(1, 24).Value = "DrugAllergy	";
                        worksheet.Cell(1, 25).Value = "ProfilePath	";
                        worksheet.Cell(1, 26).Value = "Remark	";
                        worksheet.Cell(1, 27).Value = "ReferredID";
                        // Add data
                        for (int i = 0; i < customerList.Count; i++)
                        {
                            worksheet.Cell(i + 2, 1).Value = customerList[i].Id;
                            worksheet.Cell(i + 2, 2).Value = customerList[i].Title;
                            worksheet.Cell(i + 2, 3).Value = customerList[i].Name;
                            worksheet.Cell(i + 2, 4).Value = customerList[i].PhoneNumber;
                            worksheet.Cell(i + 2, 5).Value = customerList[i].Address;
                            worksheet.Cell(i + 2, 6).Value = customerList[i].NRC;
                            worksheet.Cell(i + 2, 7).Value = customerList[i].Email;
                            worksheet.Cell(i + 2, 8).Value = customerList[i].CityId;
                            worksheet.Cell(i + 2, 9).Value = customerList[i].TownShip;
                            worksheet.Cell(i + 2, 10).Value = customerList[i].Gender;
                            worksheet.Cell(i + 2, 11).Value = customerList[i].Birthday;
                            worksheet.Cell(i + 2, 12).Value = customerList[i].MemberTypeID;
                            worksheet.Cell(i + 2, 13).Value = customerList[i].VIPMemberId;
                            worksheet.Cell(i + 2, 14).Value = customerList[i].StartDate;
                            worksheet.Cell(i + 2, 15).Value = customerList[i].CustomerCode;
                            worksheet.Cell(i + 2, 16).Value = customerList[i].CustomerTypeId;
                            worksheet.Cell(i + 2, 17).Value = customerList[i].PromoteDate;
                            worksheet.Cell(i + 2, 18).Value = customerList[i].Maritalstatus;
                            worksheet.Cell(i + 2, 19).Value = customerList[i].EmergencyContactPhone;
                            worksheet.Cell(i + 2, 20).Value = customerList[i].EmergencyContactName;
                            worksheet.Cell(i + 2, 21).Value = customerList[i].Relationship;
                            worksheet.Cell(i + 2, 22).Value = customerList[i].MainConcern;
                            worksheet.Cell(i + 2, 23).Value = customerList[i].MedicalHistory;
                            worksheet.Cell(i + 2, 24).Value = customerList[i].DrugAllergy;
                            worksheet.Cell(i + 2, 25).Value = customerList[i].ProfilePath;
                            worksheet.Cell(i + 2, 26).Value = customerList[i].Remark;
                            worksheet.Cell(i + 2, 27).Value = customerList[i].ReferredID;
                        }

                        workbook.SaveAs(saveFileDialog.FileName);
                    }

                    MessageBox.Show("File saved successfully to " + filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }   
    }
}
