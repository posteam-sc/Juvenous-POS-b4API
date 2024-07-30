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
    public partial class PurchaseDiscountReport_frm : Form
    {
        public PurchaseDiscountReport_frm()
        {
            InitializeComponent();
            CenterToScreen();
        }

        #region Variables

            POSEntities entity = new POSEntities();
            int supplierId;
            int TotalDiscountAmount;
            int TotalVoucherNo;
            Int64 TotalAmount;
            string SName;
            List<PurchaseDiscountController> pdDisList = new List<PurchaseDiscountController>();
        #endregion

        #region Events

        private void PurchaseDiscountReport_frm_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
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
            LoadData();

        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cboSupplier_SelectedValueChanged(object sender, EventArgs e)
        {
            
        }
        #endregion
        #region Functions

        public void LoadData()
        {
            DateTime fromDate = dtFrom.Value.Date;
            DateTime toDate = dtTo.Value.Date;
            supplierId = 0;
            TotalDiscountAmount = 0;
            TotalVoucherNo = 0;
            TotalAmount = 0;
            SName = "";
            pdDisList.Clear();

            if (cboSupplier.SelectedIndex > 0)
            {
                supplierId = Convert.ToInt32(cboSupplier.SelectedValue);
                APP_Data.Supplier sp = entity.Suppliers.Where(x => x.Id == supplierId).FirstOrDefault();
                if (sp != null)
                {
                    SName = " " + sp.Name.ToString() + " - Supplier ";
                }
            }
            System.Data.Objects.ObjectResult<PurchaseDiscountReport_Result> purlist = entity.PurchaseDiscountReport(fromDate, toDate, supplierId);
            foreach (PurchaseDiscountReport_Result p in purlist)
            {
                PurchaseDiscountController pdCon = new PurchaseDiscountController();

                pdCon.PurchaseDate =Convert.ToDateTime(p.Purchase_Date);
                pdCon.VoucherNo = p.VoucherNo.ToString();
                pdCon.SupplierName = (p.SupplierName.ToString() == "") ? "-" : p.SupplierName.ToString();
                pdCon.TotalAmount = Convert.ToInt64(p.TotalAmount);
                pdCon.DiscountAmount = Convert.ToInt32(p.DiscountAmount);

                TotalAmount += Convert.ToInt64(p.TotalAmount);
                TotalDiscountAmount += Convert.ToInt32(p.DiscountAmount);
                TotalVoucherNo++;
                pdDisList.Add(pdCon);               
            }
            ShowReportViewer();
        }

        private void ShowReportViewer()
        {
            dsReportTemp dsReport = new dsReportTemp();
            dsReportTemp.PurchaseDiscountTableDataTable dtPurDistbl = (dsReportTemp.PurchaseDiscountTableDataTable)dsReport.Tables["PurchaseDiscountTable"];
            foreach (PurchaseDiscountController pdCon in pdDisList)
            {
                dsReportTemp.PurchaseDiscountTableRow newRow = dtPurDistbl.NewPurchaseDiscountTableRow();
                newRow.PurchaseDate = Convert.ToDateTime(pdCon.PurchaseDate);
                newRow.VoucherNo = pdCon.VoucherNo.ToString();
                newRow.SupplierName = pdCon.SupplierName.ToString();
                newRow.TotalAmount = pdCon.TotalAmount.ToString();
                newRow.DiscountAmount = pdCon.DiscountAmount.ToString();

                dtPurDistbl.AddPurchaseDiscountTableRow(newRow);
            }

            ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["PurchaseDiscountTable"]);
            string reportPath = Application.StartupPath + "\\Reports\\PurchaseDiscount.rdlc";
            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            ReportParameter totalAmount = new ReportParameter("totalAmount", TotalAmount.ToString());
            reportViewer1.LocalReport.SetParameters(totalAmount);

            ReportParameter totalDiscountAmount = new ReportParameter("totalDiscountAmount",TotalDiscountAmount.ToString());
            reportViewer1.LocalReport.SetParameters(totalDiscountAmount);

            ReportParameter totalVoucherNo = new ReportParameter("totalVoucherNo",TotalVoucherNo.ToString());
            reportViewer1.LocalReport.SetParameters(totalVoucherNo);

            ReportParameter Header = new ReportParameter("Header", "Purchase Discount Report " + SName + " from " + dtFrom.Value.ToString("dd-MM-yyyy") + " to " + dtTo.Value.ToString("dd-MM-yyyy"));
            reportViewer1.LocalReport.SetParameters(Header);
            reportViewer1.RefreshReport();           
        }

        #endregion 

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            cboSupplier.SelectedIndex = 0;
     
        }

        private void cboSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        
    }
}
