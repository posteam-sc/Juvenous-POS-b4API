using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Microsoft.Reporting.WinForms;
using POS.APP_Data;

namespace POS
{
    public partial class FrmCustomerInfomation : Form
    {
        
        public FrmCustomerInfomation()
        {
            InitializeComponent();
        }
        APP_Data.POSEntities entity = new APP_Data.POSEntities();        
        
        private void FrmCustomerInfomation_Load(object sender, EventArgs e)
        {
            cboMemberType.SelectedIndexChanged -= cboMemberType_SelectedIndexChanged;
            txtRow.TextChanged -= txtRow_TextChanged;
            Localization.Localize_FormControls(this);           
            Bind_MemberType();
            VisbleByRadio();
            txtRow.Text = "All";
           
            cboInformationType.Items.Add("All");
            cboInformationType.Items.Add("Patient");
            cboInformationType.Items.Add("Doctor");
            cboInformationType.Items.Add("Therapist");
            cboInformationType.Items.Add("Nurse Aid");
            cboInformationType.SelectedIndex = 0;
            LoadData();
            cboMemberType.SelectedIndexChanged += cboMemberType_SelectedIndexChanged;
            txtRow.TextChanged += txtRow_TextChanged;
        }


        private void LoadData()
        {
            
            IList<dynamic> customerList = null;
            int totalRow = 0;           
            // Patient Report here requires all the customers with or without transactions
            // So we need left outer join; firstly do inner join and use DefaultIfEmpty() method to product left outer join
            // Obviously, values of fields of transactions of customers with no transactions will be NULL 
            // That's why we need to add loj.Type == null in where clause
            // Filtering out deleted and draft transactions will exclude associated customers, therefore we don't do it here
            // Instead we take RecieveAmount of those (deleted and draft) transactions as zero
            var tempcustomerListQuery = (from row in (from c in entity.Customers
                                                  join t in entity.Transactions on c.Id equals t.CustomerId into temp
                                                  from loj in temp.DefaultIfEmpty() // left outer join
                                                  where loj.Type == null || loj.Type == "Sale" || loj.Type == "Settlement" || loj.Type == "Credit" || loj.Type == "Prepaid"
                                                  select new  {   c.Title, c.Name, c.City, c.Birthday, c.Gender, c.NRC, c.PhoneNumber, c.Email, c.Address, c.CityId, c.TownShip, c.VIPMemberId, c.CustomerCode, c.CustomerTypeId, c.MemberTypeID, 
                                                       RecieveAmount = (loj.IsDeleted == false || loj.IsDeleted == null) && loj.IsComplete == true ? loj.RecieveAmount :  0}).AsEnumerable() 
                                      group row by new
                                      {
                                         Title = row.Title,
                                         Name = row.Name,
                                         City = row.City,
                                         Birthday = row.Birthday,
                                         Gender = row.Gender,
                                         NRC = row.NRC,
                                         PhoneNumber = row.PhoneNumber,
                                         Email = row.Email,
                                         Address = row.Address,
                                         CityId = row.CityId,
                                         TownShip = row.TownShip,
                                         VIPMemberId = row.VIPMemberId,
                                         CustomerCode = row.CustomerCode,
                                         CustomerTypeId = row.CustomerTypeId,
                                         MemberTypeID = row.MemberTypeID                                    
                                        
                                     }
                                  into cgrp
                                     select new
                                     {
                                         Title = cgrp.Key.Title,
                                         Name = cgrp.Key.Name,
                                         City = cgrp.Key.City,
                                         Birthday = cgrp.Key.Birthday,
                                         Gender = cgrp.Key.Gender,
                                         NRC = cgrp.Key.NRC,
                                         PhoneNumber = cgrp.Key.PhoneNumber,
                                         Email = cgrp.Key.Email,
                                         Address = cgrp.Key.Address,
                                         CityId = cgrp.Key.CityId,
                                         TownShip = cgrp.Key.TownShip,
                                         VIPMemberId = cgrp.Key.VIPMemberId,
                                         CustomerCode = cgrp.Key.CustomerCode,
                                         CustomerTypeId = cgrp.Key.CustomerTypeId,
                                         MemberTypeID = cgrp.Key.MemberTypeID,                                         
                                         TotalSpentAmount = cgrp.Sum(x => x.RecieveAmount),
                                         Age = cgrp.Key.Birthday == null ? null :
                                         cgrp.Key.Birthday.Value.AddYears(DateTime.Now.Year - cgrp.Key.Birthday.Value.Year).Date > DateTime.Now.Date ?
                                         (DateTime.Now.Year - cgrp.Key.Birthday.Value.Year - 1).ToString() : (DateTime.Now.Year - cgrp.Key.Birthday.Value.Year).ToString()
                                     });

            var customerListQuery = tempcustomerListQuery;

            if (cboMemberType.SelectedIndex == 0)
            {
               
                customerListQuery = tempcustomerListQuery;

                if (cboInformationType.Text == "All")
                {
                    //customerListQuery = tempcustomerListQuery;
                }
                else if (cboInformationType.Text == "Patient")                {
                    
                    customerListQuery = customerListQuery.Where(x => x.CustomerTypeId == 1);
                }
                else if (cboInformationType.Text == "Doctor")                {
                    
                    customerListQuery = customerListQuery.Where(x => x.CustomerTypeId == 2);
                }
                else if (cboInformationType.Text == "Therapist")
                {
                    customerListQuery = customerListQuery.Where(x => x.CustomerTypeId == 3);
                }
                else if (cboInformationType.Text == "Nurse Aid")
                {
                    customerListQuery = customerListQuery.Where(x => x.CustomerTypeId == 4);
                }

                if (txtSearch.Visible == true)
                {
                    if (txtSearch.Text.Trim() != string.Empty)
                    {
                        if (rdoMemberCardNo.Checked)
                        {
                            //Search BY Member Card No                           
                            customerListQuery = customerListQuery.Where(x => x.VIPMemberId == txtSearch.Text.Trim());
                        }
                        else if (rdoCustomerName.Checked)
                        {
                            //Search BY Customer Name 
                            customerListQuery = customerListQuery.Where(x => x.Name.Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()));
                        }
                    }
                }
                else
                {
                    DateTime fromDate = dtpBirthday.Value.Date;

                    if (rdoBirthday.Checked)
                    {
                        customerListQuery = customerListQuery.Where(x => x.Birthday != null && x.Birthday.Value.Date == fromDate);
                    }
                    else if (rdoBirth_Day.Checked)
                    {
                        customerListQuery = customerListQuery.Where(x => x.Birthday != null && x.Birthday.Value.Day == fromDate.Day && x.Birthday.Value.Month == fromDate.Month);
                    }
                    else if (rdoBirthMonth.Checked)
                    {
                        customerListQuery = customerListQuery.Where(x => x.Birthday != null && x.Birthday.Value.Month == fromDate.Month);
                    }
                }

            }
            else
            {
                customerListQuery = tempcustomerListQuery.Where(x=> x.MemberTypeID == Convert.ToInt32(cboMemberType.SelectedValue));
                
                if (cboInformationType.Text == "All")
                {
                    //customerListQuery = tempcustomerListQuery.Where(x => x.MemberTypeID == Convert.ToInt32(cboMemberType.SelectedValue));
                }
                else if (cboInformationType.Text == "Patient")
                {
                    customerListQuery = customerListQuery.Where(x => x.CustomerTypeId == 1);
                }
                else if (cboInformationType.Text == "Doctor")                {
                    
                    customerListQuery = customerListQuery.Where(x => x.CustomerTypeId == 2);
                }
                else if (cboInformationType.Text == "Therapist")                {
                   
                    customerListQuery = customerListQuery.Where(x => x.CustomerTypeId == 3);
                }
                else if (cboInformationType.Text == "Nurse Aid")
                {
                   
                    customerListQuery = customerListQuery.Where(x => x.CustomerTypeId == 4);
                }

                if (txtSearch.Visible == true)
                {
                    if (txtSearch.Text.Trim() != string.Empty)
                    {
                        if (rdoMemberCardNo.Checked)
                        {
                            //Search BY Member Card No                            
                            customerListQuery = customerListQuery.Where(x => x.VIPMemberId == txtSearch.Text.Trim());                            
                        }
                        else if (rdoCustomerName.Checked)
                        {
                            customerListQuery = customerListQuery.Where(x => x.Name.Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()));
                        }
                    }
                }
                else
                {
                    DateTime fromDate = dtpBirthday.Value.Date;

                    if (rdoBirthday.Checked)
                    {
                        customerListQuery = customerListQuery.Where(x => x.Birthday != null && x.Birthday.Value.Date == fromDate);
                    }
                    else if (rdoBirth_Day.Checked)
                    {
                        customerListQuery = customerListQuery.Where(x => x.Birthday != null && x.Birthday.Value.Day == fromDate.Day && x.Birthday.Value.Month == fromDate.Month);
                    }
                    else if (rdoBirthMonth.Checked)
                    {
                        customerListQuery = customerListQuery.Where(x => x.Birthday != null && x.Birthday.Value.Month == fromDate.Month);
                    }
                }

            }
           

            if (!string.Equals(txtRow.Text, "All"))
            {
                Int32.TryParse(txtRow.Text, out totalRow);
                customerListQuery = customerListQuery.Take(totalRow);
            }

            customerList = customerListQuery.ToList<dynamic>();
            ShowReportViewer(customerList);
        }

        private void ShowReportViewer(IList<dynamic> customerList) 
        {

            dsReportTemp dsReport = new dsReportTemp();
            dsReportTemp.CustomerInformationDataTable dtCustomerReport = (dsReportTemp.CustomerInformationDataTable)dsReport.Tables["CustomerInformation"];
            
            foreach (var c in customerList)           
            {
                
                dsReportTemp.CustomerInformationRow newRow = dtCustomerReport.NewCustomerInformationRow();
                newRow.Title = c.Title;
                newRow.Name = c.Name; 
                newRow.Birthday = (c.Birthday == null) ? "" : c.Birthday.ToString("dd-MM-yyyy");
                newRow.Gender = (c.Gender == "") ? "" : c.Gender;
                newRow.NRC = (c.NRC == null || c.NRC == "") ? "" : c.NRC;
                newRow.Age = c.Age;
                newRow.PhNo = (c.PhoneNumber == "" || c.PhoneNumber == "") ? "" : c.PhoneNumber;
                newRow.Email = (c.Email == null || c.Email == "") ? "" : c.Email;
                newRow.Address = (c.Address == null || c.Address == "") ? "" : c.Address;
                newRow.TownShip = (c.TownShip == null || c.TownShip == "") ? "" : c.TownShip;
                newRow.City = (c.City.CityName == null || c.City.CityName == "") ? "" : c.City.CityName;
                newRow.VIPMemberId = (c.VIPMemberId == null || c.VIPMemberId == "") ? "" : c.VIPMemberId;//recheck
                newRow.SpentAmount =  c.TotalSpentAmount.ToString("N0"); //N0:  comma with no decimal places
                dtCustomerReport.AddCustomerInformationRow(newRow);
            }

            ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["CustomerInformation"]);
           // ReportDataSource rds = new ReportDataSource("DataSet1", _reportCustomerList);
            string reportPath = string.Empty;
            reportPath = Application.StartupPath + "\\Reports\\AllMemberInformation.rdlc";

            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.RefreshReport();
        }

        #region Method
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
        #endregion

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void rdoMemberCardNo_CheckedChanged(object sender, EventArgs e)
        {
            lblSearchTitle.Text = "Member Card No.";
            VisibleControl(true, false);
        }

        private void rdoCustomerName_CheckedChanged(object sender, EventArgs e)
        {
            lblSearchTitle.Text = "Customer Name";
            VisibleControl(true, false);
        }

        private void VisibleControl(bool t, bool f)
        {
            txtSearch.Visible = t;
            dtpBirthday.Visible = f;
        }

        private void VisbleByRadio()
        {
            if (rdoMemberCardNo.Checked == true || rdoCustomerName.Checked == true)
            {
                VisibleControl(true, false);
            }
            else
            {
                VisibleControl(false, true);
            }
        }

        private void rdoBirthday_CheckedChanged(object sender, EventArgs e)
        {
            lblSearchTitle.Text = "Date of Birth";
            VisibleControl(false, true);
        }

        private void cboMemberType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Refersh();
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

        private void txtRow_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void Refersh()
        {

            txtSearch.Text = "";
            rdoMemberCardNo.Checked = true;
            cboMemberType.Text = "All";
            txtRow.Text = "All";
            LoadData();
        }
        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            Refersh();
        }
    }
    
}
