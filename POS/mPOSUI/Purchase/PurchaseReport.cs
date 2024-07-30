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
    public partial class PurchaseReport : Form
    {
        public PurchaseReport()
        {
            InitializeComponent();
        }


        #region Varibles

        POSEntities entity = new POSEntities();
        List<APP_Data.Supplier> splist = new List<Supplier>();
        List<PurchaseReportController> purlist = new List<PurchaseReportController>();


       

        int SupplierId = 0, BrandId = 0, ProductId=0;
        string sName,Stype = "";
        Int64 totalAmount = 0;int totalQty = 0;

        #endregion

        #region Functions

        public void loadData()
        {
            totalAmount = 0; totalQty = 0;
            int SupplierId = 0, BrandId = 0, ProductId = 0;
            Stype = "";
            Name = "";
            DateTime fromDate = dtFrom.Value.Date;
            DateTime toDate = dtTo.Value.Date;
            purlist.Clear();

            if (rdoSupplierName.Checked)
            {
                if (cboSupplier.SelectedIndex > 0)
                {
                    SupplierId = Convert.ToInt32(cboSupplier.SelectedValue);
                    Stype = "SupplierName";
                    APP_Data.Supplier sp = entity.Suppliers.Where(x => x.Id == SupplierId).FirstOrDefault();
                    sName = sp.Name + " at Supplier";

                }
                else
                {
                    sName = "All Supplier";
                }
            }

            else if (rdoBrandName.Checked)
            {
                if (cboBrand.SelectedIndex > 0)
                {
                    BrandId = Convert.ToInt32(cboBrand.SelectedValue);
                    Stype = "BrandName";
                    APP_Data.Brand b = entity.Brands.Where(x => x.Id == BrandId).FirstOrDefault();
                    sName = b.Name + " at Brand ";
                }
                else
                {
                    sName = " All Brand";
                }
            }
            else if (rdoProductName.Checked)
            {
                if (cboProductName.SelectedIndex > 0)
                {
                    ProductId = Convert.ToInt32(cboProductName.SelectedValue);
                    Stype = "ProductName";
                    APP_Data.Product p = entity.Products.Where(x => x.Id == ProductId).FirstOrDefault();
                    sName = p.Name + " at Product ";
                }
                else
                {
                    sName = " All Product";
                }  
            }
            else
            {
                Stype = "All";
                sName = "-";
            }

            System.Data.Objects.ObjectResult<PurchaseReport_Result> pResult = entity.PurchaseReport(fromDate, toDate, SupplierId, BrandId, ProductId, Stype);
            foreach (PurchaseReport_Result pr in pResult)
            {
                PurchaseReportController puObj = new PurchaseReportController();
                puObj.Date = Convert.ToDateTime(pr.Date);
                puObj.ProductName = pr.ProductName;
                puObj.SupplierName = pr.SupplierName;
                puObj.UnitPrice = Convert.ToInt32(pr.UnitPrice);
                puObj.Qty = Convert.ToInt32(pr.Qty);
                puObj.TotalAmount = Convert.ToInt64(pr.UnitPrice*pr.Qty);
                puObj.VourcherNo = pr.VoucherNo.ToString();
                totalAmount += Convert.ToInt64(pr.UnitPrice * pr.Qty);
                totalQty += Convert.ToInt32(pr.Qty);
                
                purlist.Add(puObj);
            }

            ShowReportViewr();
        }

        private void ShowReportViewr()
        {
            dsReportTemp dsReport = new dsReportTemp();
            dsReportTemp.PurchaseReportDataTable dtPurReport = (dsReportTemp.PurchaseReportDataTable)dsReport.Tables["PurchaseReport"];
            foreach (PurchaseReportController pdRCon in purlist)
            {
                dsReportTemp.PurchaseReportRow newRow = dtPurReport.NewPurchaseReportRow();
                newRow.Date = pdRCon.Date.ToString("dd-MM-yyyy");
                newRow.ProductName = pdRCon.ProductName;
                newRow.SupplierName = pdRCon.SupplierName;
                newRow.UnitPrice = pdRCon.UnitPrice.ToString();
                newRow.Qty = pdRCon.Qty.ToString();
                newRow.TotalAmount = pdRCon.TotalAmount.ToString();
                newRow.VoucherNo = pdRCon.VourcherNo.ToString();
                dtPurReport.AddPurchaseReportRow(newRow);
            }
            ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["PurchaseReport"]);
            string reportPath = Application.StartupPath + "\\Reports\\PurchaseReport.rdlc";
            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            ReportParameter TotalQty = new ReportParameter("TotalQty", totalQty.ToString());
            reportViewer1.LocalReport.SetParameters(TotalQty);

            ReportParameter TotalAmount = new ReportParameter("TotalAmount", totalAmount.ToString());
            reportViewer1.LocalReport.SetParameters(TotalAmount);

            ReportParameter Header = new ReportParameter("Header", "Purchase Report " + sName + " from " + dtFrom.Value.ToString("dd-MM-yyyy") + " to " + dtTo.Value.ToString("dd-MM-yyyy"));
            reportViewer1.LocalReport.SetParameters(Header);
            reportViewer1.RefreshReport();
        }

        #endregion

        #region Events

        private void PurchaseReport_Load(object sender, EventArgs e)
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


            List<APP_Data.Brand> BrandList = new List<APP_Data.Brand>();
            APP_Data.Brand brandObj1 = new APP_Data.Brand();
            brandObj1.Id = 0;
            brandObj1.Name = "All";
            BrandList.Add(brandObj1);
            BrandList.AddRange((from bList in entity.Brands select bList).ToList());
            cboBrand.DataSource = BrandList;
            cboBrand.DisplayMember = "Name";
            cboBrand.ValueMember = "Id";
            cboBrand.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboBrand.AutoCompleteSource = AutoCompleteSource.ListItems;


            List<APP_Data.Supplier> SupplierList = new List<APP_Data.Supplier>();
            APP_Data.Supplier SupplierObj1 = new APP_Data.Supplier();
            SupplierObj1.Id = 0;
            SupplierObj1.Name = "All";
            SupplierList.Add(SupplierObj1);
            SupplierList.AddRange((from slist in entity.Suppliers select slist).ToList());
            cboSupplier.DataSource = SupplierList;
            cboSupplier.DisplayMember = "Name";
            cboSupplier.ValueMember = "Id";
            cboSupplier.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboSupplier.AutoCompleteSource = AutoCompleteSource.ListItems;
            rdoSupplierName.Checked = true;
            rdoProductName.Checked = false;
            rdoBrandName.Checked = false;
            loadData();

        }

        private void cboBrand_SelectedValueChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void cboSupplier_SelectedValueChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void cboProductName_SelectedValueChanged(object sender, EventArgs e)
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
            cboBrand.SelectedIndex = 0;
            cboProductName.SelectedIndex = 0;
            cboSupplier.SelectedIndex = 0;
            dtFrom.Value = DateTime.Now;
            dtTo.Value = DateTime.Now;

            rdoSupplierName.Checked = true;
            rdoProductName.Checked = false;
            rdoBrandName.Checked = false;
            cboSupplier.Enabled = true;
            cboProductName.Enabled = false;
            cboBrand.Enabled = false;            

            loadData();
        }
        #endregion    

        private void rdoSupplierName_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoSupplierName.Checked)
            {
                rdoSupplierName.Checked = true;
                rdoProductName.Checked = false;
                rdoBrandName.Checked = false;
                cboSupplier.Enabled = true;
                cboProductName.Enabled = false;
                cboBrand.Enabled = false;
                cboProductName.SelectedIndex = 0;
                cboBrand.SelectedIndex = 0;
            }
            
        }

        private void rdoProductName_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoProductName.Checked)
            {
                rdoSupplierName.Checked = false;
                rdoProductName.Checked = true;
                rdoBrandName.Checked = false;
                cboSupplier.Enabled = false;
                cboProductName.Enabled = true;
                cboBrand.Enabled = false;
                cboBrand.SelectedIndex = 0;
                cboSupplier.SelectedIndex = 0;
            }
            
        }

        private void rdoBrandName_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoBrandName.Checked)
            {
                rdoSupplierName.Checked = false;
                rdoProductName.Checked = false;
                rdoBrandName.Checked = true;

                cboSupplier.Enabled = false;
                cboProductName.Enabled = false;
                cboBrand.Enabled = true;
                cboSupplier.SelectedIndex = 0;
                cboProductName.SelectedIndex = 0;
            }
          
        }
               
    }
}
