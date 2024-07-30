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
    public partial class frmCustomerSaleReport : Form
    {

       

        #region Varibables
        POSEntities entity = new POSEntities();
        List<CustomerSaleController> cuslist = new List<CustomerSaleController>();
        int cId = 0; int pId = 0;
        bool IsStart = false;
        System.Data.Objects.ObjectResult<GetCustomerSaleById_Result> resultlist;
        #endregion

        #region event
        public frmCustomerSaleReport()
        {
            InitializeComponent();
        }
        private void frmCustomerSaleReport_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            List<APP_Data.Product> ProductList = new List<APP_Data.Product>();

            APP_Data.Product pdObj2 = new APP_Data.Product();
            pdObj2.Id = 0;
            pdObj2.Name = "All";
            ProductList.Add(pdObj2);
            ProductList.AddRange((from plist in entity.Products select plist).ToList());
            cboProductName.DataSource = ProductList;
            cboProductName.DisplayMember = "Name";
            cboProductName.ValueMember = "Id";
            cboProductName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboProductName.AutoCompleteSource = AutoCompleteSource.ListItems;


            List<APP_Data.Customer> CustomerList = new List<APP_Data.Customer>();

            APP_Data.Customer cuObj2 = new APP_Data.Customer();
            cuObj2.Id = 0;
            cuObj2.Name = "All";

            CustomerList.Add(cuObj2);
            CustomerList.AddRange((from clist in entity.Customers select clist).ToList());
            cboCustomerName.DataSource = CustomerList;
            cboCustomerName.DisplayMember = "Name";
            cboCustomerName.ValueMember = "Id";
            cboCustomerName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboCustomerName.AutoCompleteSource = AutoCompleteSource.ListItems;
            this.reportViewer1.Refresh();

            Utility.BindShopALL(cboshoplist);
            cboshoplist.Text = SettingController.DefaultShop.ShopName;
            Utility.ShopComBo_EnableOrNot(cboshoplist);
            IsStart = true;
            loadData();
        }

        private void cboProductName_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadData();
        }


        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cboCustomerName.SelectedIndex = 0;
            cboProductName.SelectedIndex = 0;
            dtFrom.Value = DateTime.Now;
            dtTo.Value = DateTime.Now;
        }

        private void cboCustomerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void cboshoplist_selectedIndexChanged(object sender, EventArgs e)
        {
            loadData();
        }
        #endregion

        #region method

        private void loadData()
        {
            if (IsStart)
            {

                cId = 0; pId = 0;
                DateTime fromDate = dtFrom.Value.Date;
                DateTime toDate = dtTo.Value.Date;

                int shopid = Convert.ToInt32(cboshoplist.SelectedValue);
                string currentshortcode = (from p in entity.Shops where p.Id == shopid select p.ShortCode).FirstOrDefault();
               // string currentshopname = (from p in entity.Shops where p.Id == shopid select p.ShopName).FirstOrDefault();
                cuslist.Clear();

                if(currentshortcode == null)
                {
                    currentshortcode = "";
                }

                

                if (cboCustomerName.SelectedIndex > 0)
                {
                    cId = Convert.ToInt32(cboCustomerName.SelectedValue);
                }
                if (cboProductName.SelectedIndex > 0)
                {
                    pId = Convert.ToInt32(cboProductName.SelectedValue);
                }

               
                resultlist = entity.GetCustomerSaleById(cId, pId, fromDate, toDate, currentshortcode);
                    
               
                
                ShowReportViewer();
            }
        }
        private void ShowReportViewer()
        {
            string ProductName = "", CustomerName = "";

          
            if (cId > 0)
            {

                APP_Data.Customer c = entity.Customers.Where(x => x.Id == cId).FirstOrDefault();
                CustomerName = c.Name;
            }
            else
            {
                CustomerName = "All Customer";
            }
            if (pId > 0)
            {
                APP_Data.Product p = entity.Products.Where(x => x.Id == pId).FirstOrDefault();
                ProductName = p.Name;
            }
            else
            {
                ProductName = "All Product";
            }

            //  ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["CustomerSaleReport"]);
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DataSet1";
            rds.Value = resultlist;
            string reportPath = Application.StartupPath + "\\Reports\\CustomerSale.rdlc";
            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            //ReportParameter ShopName = new ReportParameter("ShopName", "Customer Sale Report for " + currentshopname);
            //reportViewer1.LocalReport.SetParameters(ShopName);

            ReportParameter Header = new ReportParameter("Header", ProductName + "  Of  " + CustomerName + " From " + dtFrom.Value.Date.ToString("dd-MM-yyyy") + "  To  " + dtTo.Value.Date.ToString("dd-MM-yyyy"));
            reportViewer1.LocalReport.SetParameters(Header);
            reportViewer1.RefreshReport();


        }
        #endregion
       
  


       
    }
}
