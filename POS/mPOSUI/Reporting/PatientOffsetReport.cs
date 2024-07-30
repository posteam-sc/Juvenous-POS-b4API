using Microsoft.Reporting.WinForms;
using POS.APP_Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS.mPOSUI.Reporting
{
    public partial class PatientOffsetReport : Form
    {
        POSEntities entity = new POSEntities();
        public PatientOffsetReport()
        {
            InitializeComponent();
        }

        private void PatientOffsetReport_Load(object sender, EventArgs e)
        {
            DateTime fromDate = dtpFromDate.Value.Date;
            DateTime toDate = dtpToDate.Value.Date;
            bindCustomer();            
            this.reportViewer1.RefreshReport();
            
        }
        public void bindCustomer()
        {
            entity = new POSEntities();
            List<APP_Data.Customer> customerList = new List<APP_Data.Customer>();
            APP_Data.Customer customer = new APP_Data.Customer();
            customer.Id = 0;
            customer.Name = "Select";
            customerList.Add(customer);
            customerList.AddRange(entity.Customers.Where(x => x.CustomerTypeId == 1).ToList());
            cboPatientName.DataSource = customerList;
            cboPatientName.DisplayMember = "Name";
            cboPatientName.ValueMember = "Id";
        }
        class CustomerOffset
        {

            public int UsedQty { get; set; }
            public string CustomerName { get; set; }
            public string TransactionDate { get; set; }
            public int AvailableQty { get; set; }
              public string PackageName { get; set; }
            public String Remark { get; set; }
            public string UsedDate { get; set; }
        }
        public void reportLoadData()
        {
            //int MemberId = Convert.ToInt32(txtMemberId.Text);
            int patientID =Convert.ToInt32( cboPatientName.SelectedValue);
           if (patientID != 0)
            {
                var data = (from d in entity.PackageUsedHistories
                            where d.PackagePurchasedInvoice.InvoiceDate >= dtpFromDate.Value.Date
                            && d.PackagePurchasedInvoice.InvoiceDate <= dtpToDate.Value.Date &&
                            (d.ActualOffsetBy == patientID)
                            && d.IsDelete == false
                            select new
                            {
                                UseQty = d.UseQty,
                                CustomerName = d.Customer.Name,
                                TransactionDate = d.PackagePurchasedInvoice.InvoiceDate,
                                AvailableQty = (d.PackagePurchasedInvoice.packageFrequency * d.PackagePurchasedInvoice.TransactionDetail.Qty) - d.PackagePurchasedInvoice.UseQty,
                                PackageName = d.PackagePurchasedInvoice.Product.Id,
                                UsedDate = d.UsedDate,
                                Remark = d.Remark
                            }).ToList();
                if (data.Count > 0)
                {
                    List<CustomerOffset> customerOffsetList = new List<CustomerOffset>();
                    foreach (var item in data)
                    {
                        CustomerOffset customeroffset = new CustomerOffset();
                        customeroffset.UsedQty = item.UseQty;
                        customeroffset.CustomerName = item.CustomerName;
                        customeroffset.AvailableQty = Convert.ToInt32(item.AvailableQty);
                        customeroffset.TransactionDate = item.TransactionDate.Date.ToString("dd-MM-yyyy");
                        customeroffset.PackageName = (from p in entity.Products where p.Id == item.PackageName select p.Name).SingleOrDefault();
                        customeroffset.Remark = item.Remark;
                        customeroffset.UsedDate = item.UsedDate.Date.ToString("dd-MM-yyyy");
                        customerOffsetList.Add(customeroffset);
                    }

                    reportViewer1.Visible = true;
                    ReportDataSource rds = new ReportDataSource();
                    rds.Name = "CustomerOffset";
                    rds.Value = customerOffsetList;

                    string reportPath = Application.StartupPath + "\\Reports\\PatientOffset.rdlc";
                    reportViewer1.LocalReport.ReportPath = reportPath;
                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(rds);
                    reportViewer1.RefreshReport();
                }
                else
                {
                    MessageBox.Show("There is no data to show!");
                }
            }else
            {
                var data = (from d in entity.PackageUsedHistories
                            where d.UsedDate >= dtpFromDate.Value.Date && d.UsedDate <= dtpToDate.Value.Date &&
                            d.IsDelete == false
                            select new
                            {
                                UseQty = d.UseQty,
                                CustomerName = d.Customer.Name,
                                TransactionDate = d.PackagePurchasedInvoice.InvoiceDate,
                                AvailableQty = (d.PackagePurchasedInvoice.packageFrequency * d.PackagePurchasedInvoice.TransactionDetail.Qty) - d.PackagePurchasedInvoice.UseQty,
                                PackageName = d.PackagePurchasedInvoice.Product.Id,
                                UsedDate = d.UsedDate,
                                Remark = d.Remark
                            }).ToList();
                if (data.Count > 0)
                {
                    List<CustomerOffset> customerOffsetList = new List<CustomerOffset>();
                    foreach (var item in data)
                    {
                        CustomerOffset customeroffset = new CustomerOffset();
                        customeroffset.UsedQty = item.UseQty;
                        customeroffset.CustomerName = item.CustomerName;
                        customeroffset.AvailableQty = Convert.ToInt32(item.AvailableQty);
                        customeroffset.TransactionDate = item.TransactionDate.Date.ToString("dd-MM-yyyy");
                        customeroffset.PackageName = (from p in entity.Products where p.Id == item.PackageName select p.Name).SingleOrDefault();
                        customeroffset.Remark = item.Remark;
                        customeroffset.UsedDate = item.UsedDate.Date.ToString("dd-MM-yyyy");
                        customerOffsetList.Add(customeroffset);
                    }

                    reportViewer1.Visible = true;
                    ReportDataSource rds = new ReportDataSource();
                    rds.Name = "CustomerOffset";
                    rds.Value = customerOffsetList;

                    string reportPath = Application.StartupPath + "\\Reports\\PatientOffset.rdlc";
                    reportViewer1.LocalReport.ReportPath = reportPath;
                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(rds);
                    reportViewer1.RefreshReport();
                }
                else
                {
                    MessageBox.Show("There is no data to show!");
                }
            }
                
          
        }

        

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            reportLoadData();
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            dtpFromDate.Value = DateTime.Now;
            dtpToDate.Value = DateTime.Now;
            cboPatientName.SelectedIndex = 0;
            txtMemberId.Text = string.Empty;
        }
    }
}
