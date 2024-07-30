using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using POS.APP_Data;

namespace POS
{
    public partial class OutstandingCustomerReport : Form
    {
        #region Variable
        POSEntities entity = new POSEntities();
        CustomerInfoHolder[] cInfoList;
        List<APP_Data.Customer> custList = new List<APP_Data.Customer>();
        List<APP_Data.GetOutStandingCustomerList_Result> OutstandingCustomerlist;
        List<APP_Data.GetOutStandingCustomerList_Result> crList;
        #endregion
        public OutstandingCustomerReport()
        {
            InitializeComponent();
        }

        private void OutstandingCustomerReport_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            entity = new POSEntities();
            Bind_Customer();
            OutstandingCustomerlist = entity.GetOutStandingCustomerList().ToList();       
            this.reportViewer1.RefreshReport();
            LoadData();
        }

        

        #region Function
        private void Bind_Customer()
        {
            APP_Data.Customer cust = new APP_Data.Customer();
            cust.Id = 0;
            cust.Name = "All";
            custList.Add(cust);
            custList.AddRange((from c in entity.Customers select c).ToList());
            cboName.DataSource = custList;
            cboName.DisplayMember = "Name";
            cboName.ValueMember = "Id";
            cboName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboName.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void LoadData()
        {
          
            if (cboName.SelectedIndex == 0)
            {
                crList = OutstandingCustomerlist;
            }
            else
            {
                int custID = Convert.ToInt32(cboName.SelectedValue);
                crList = OutstandingCustomerlist.Where(x => x.Id == custID).ToList();             
            }
           
            ShowReportViewer();
        }

        private void ShowReportViewer()
        {
            dsReportTemp dsReport = new dsReportTemp();
            dsReportTemp.OutstandingCustomerDataTable dtOutstandingCusReport = (dsReportTemp.OutstandingCustomerDataTable)dsReport.Tables["OutstandingCustomer"];
            if (crList != null)
            {
                foreach (APP_Data.GetOutStandingCustomerList_Result c in crList)
                {
                    dsReportTemp.OutstandingCustomerRow newRow = dtOutstandingCusReport.NewOutstandingCustomerRow();

                    newRow.CustomerId = c.Id.ToString();
                    newRow.Name = c.Name;
                    newRow.PhoneNo = c.PhoneNumber;
                    newRow.Address = c.Address;
                    newRow.OutstandingAmount = (int)c.OutstandingBalance;
                    dtOutstandingCusReport.AddOutstandingCustomerRow(newRow);


                }

                ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["OutstandingCustomer"]);
                string reportPath = Application.StartupPath + "\\Reports\\OutstandingReport.rdlc";
                reportViewer1.LocalReport.ReportPath = reportPath;
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);

                ReportParameter HeadTitle = new ReportParameter("HeadTitle", "Customer Information and their outstanding amount for " + SettingController.ShopName);
                reportViewer1.LocalReport.SetParameters(HeadTitle);

                reportViewer1.RefreshReport();

            }
           

        }

        #endregion

        private void cboName_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        
    }
}
