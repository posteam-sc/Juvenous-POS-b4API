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
    public partial class GWPTransactionsReport : Form
    {
        #region Variables
      
        POSEntities entity = new POSEntities();
        List<GWPTransactionsController> FinalResultList = new List<GWPTransactionsController>();
        List<GWPSetController> GWPSetList = new List<GWPSetController>();
        int InvoiceCount = 0;
        int ProductCount = 0;
        #endregion
        #region Event
        public GWPTransactionsReport()
        {
            InitializeComponent();
        }

        private void GWP_Transactions_Load(object sender, EventArgs e)
        {
            Counter_Bind();
            this.bindMemberType();
            LoadData();
            this.reportViewer1.RefreshReport();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void rdbVIP_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void rdbNon_VIP_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cboCounter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
        #endregion
        #region Function

        private void Counter_Bind()
        {
            List<APP_Data.Counter> counterList = new List<APP_Data.Counter>();
            APP_Data.Counter counterObj = new APP_Data.Counter();
            counterObj.Id = 0;
            counterObj.Name = "Select";
            counterList.Add(counterObj);
            counterList.AddRange((from c in entity.Counters orderby c.Id select c).ToList());
            cboCounter.DataSource = counterList;
            cboCounter.DisplayMember = "Name";
            cboCounter.ValueMember = "Id";
        }
        private void bindMemberType() {
            List<APP_Data.MemberType> memberTypeList = new List<APP_Data.MemberType>();
            APP_Data.MemberType memberType = new APP_Data.MemberType();
            memberType.Id = 0;
            memberType.Name = "Select";
            memberTypeList.Add(memberType);
            memberTypeList.AddRange((from c in entity.MemberTypes orderby c.Id select c).ToList());
            cboMemberType.DataSource = memberTypeList;
            cboMemberType.DisplayMember = "Name";
            cboMemberType.ValueMember = "Id";
            }


        private void LoadData()
        {
            int counterId = 0;
            if (cboCounter.SelectedIndex > 0)
            {
                counterId = Convert.ToInt32(cboCounter.SelectedValue);
            }
            
            if (cboMemberType.SelectedIndex > 0) {
                memberTypeId = Convert.ToInt32(cboMemberType.SelectedValue);
                }
            InvoiceCount = 0; ProductCount = 0;
            DateTime fromDate = dtFrom.Value.Date;
            DateTime toDate = dtTo.Value.Date;
        
            string TId = string.Empty;
            FinalResultList.Clear();
            GWPSetList.Clear();
            List<GWPTransactionsController> GWPTransList = new List<GWPTransactionsController>();
            System.Data.Objects.ObjectResult<GetGWPTransactions_Result> gWPTransactionsResult = entity.GetGWPTransactions(memberTypeId, fromDate, toDate,counterId);

            #region select GWP Transactions
            foreach (GetGWPTransactions_Result gWPTransactionsResultItems in gWPTransactionsResult) {
                //check that transaction is in the attachGiftSystemForTransaction
                List<GiftSystemInTransaction> attachGiftList = entity.GiftSystemInTransactions.Where(x => x.TransactionId == gWPTransactionsResultItems.InvoiceNo).ToList();
                if (attachGiftList.Count > 0) {
                    //count transaction
                    if (TId != gWPTransactionsResultItems.InvoiceNo) {

                        InvoiceCount++;
                        TId = gWPTransactionsResultItems.InvoiceNo;
                        }

                    Product product = entity.Products.FirstOrDefault(x => x.Id == gWPTransactionsResultItems.productId);
                    if (product.IsWrapper == true) {
                        List<WrapperItem> wrappItems = entity.WrapperItems.Where(x => x.ParentProductId == product.Id).ToList();
                        List<Product> productList = new List<Product>();
                        foreach (WrapperItem w in wrappItems) {
                            productList.Add(w.Product1);
                            }
                        foreach (Product productItems in productList) {
                            GWPTransactionsController gWPTransactionsController = new GWPTransactionsController();
                            gWPTransactionsController.Name = gWPTransactionsResultItems.Name;
                            gWPTransactionsController.TransactionNo = gWPTransactionsResultItems.InvoiceNo;
                            gWPTransactionsController.ItemCode = productItems.ProductCode;
                            gWPTransactionsController.GiftName = (gWPTransactionsResultItems.GiftName == null) ? "" : gWPTransactionsResultItems.GiftName;
                            gWPTransactionsController.Discount = gWPTransactionsResultItems.Dis;
                            gWPTransactionsController.Qty = Convert.ToInt32(gWPTransactionsResultItems.Qty);
                            gWPTransactionsController.TotalAmount = Convert.ToDecimal(productItems.Price) * Convert.ToInt32(gWPTransactionsResultItems.Qty) - (((Convert.ToDecimal(productItems.Price) * Convert.ToInt32(gWPTransactionsResultItems.Qty)) / 100) * gWPTransactionsResultItems.Dis);
                            FinalResultList.Add(gWPTransactionsController);
                            ProductCount++;
                            }
                        }
                    else {
                        GWPTransactionsController gWPTransactionsController = new GWPTransactionsController();
                        gWPTransactionsController.Name = (gWPTransactionsResultItems.Name == null) ? "Unknown" : gWPTransactionsResultItems.Name;
                        gWPTransactionsController.TransactionNo = gWPTransactionsResultItems.InvoiceNo;
                        gWPTransactionsController.ItemCode = product.ProductCode;
                        gWPTransactionsController.GiftName = (gWPTransactionsResultItems.GiftName == null) ? "" : gWPTransactionsResultItems.GiftName;
                        gWPTransactionsController.Discount = gWPTransactionsResultItems.Dis;
                        gWPTransactionsController.Qty = Convert.ToInt32(gWPTransactionsResultItems.Qty);
                        gWPTransactionsController.TotalAmount = Convert.ToDecimal(gWPTransactionsResultItems.Total);
                        FinalResultList.Add(gWPTransactionsController);
                        ProductCount++;
                        }
                    }
                }
            #endregion

            #region GWP Total Qty and Amount by time period
            System.Data.Objects.ObjectResult<GetGWPSetQtyAndAmount_Result> GWPSetResult = entity.GetGWPSetQtyAndAmount(memberTypeId, fromDate, toDate, counterId);
            foreach (GetGWPSetQtyAndAmount_Result resultitems in GWPSetResult)
            {
                GWPSetController gWPSetController = new GWPSetController();
                gWPSetController.Id = resultitems.Id;
                gWPSetController.Name = resultitems.Name;
                gWPSetController.Qty = Convert.ToInt32(resultitems.Qty);
                gWPSetController.Amount = Convert.ToInt64(resultitems.Amount);
                GWPSetList.Add(gWPSetController);
            }
            #endregion
            ShowReportViewer();
        }

        private void ShowReportViewer()
        {
            string Member = string.Empty;
            var data = entity.MemberTypes.Where(x => x.Id == memberTypeId);
            foreach (var item in data) { Member = item.Name; }
            dsReportTemp dsReport = new dsReportTemp();
            dsReportTemp.GWPTransactionDataTable dtGWPTransReport = (dsReportTemp.GWPTransactionDataTable)dsReport.Tables["GWPTransaction"];
            foreach (GWPTransactionsController g in FinalResultList)
            {
                dsReportTemp.GWPTransactionRow newRow = dtGWPTransReport.NewGWPTransactionRow();
                newRow.Name = g.Name;
                newRow.InvoiceNo = g.TransactionNo;
                newRow.ProductCode = g.ItemCode;
                newRow.GiftName = g.GiftName;
                newRow.Dis = g.Discount;
                newRow.Qty = g.Qty;
                newRow.TotalAmount = g.TotalAmount;
                dtGWPTransReport.AddGWPTransactionRow(newRow);
            }


            ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["GWPTransaction"]);

            dsReportTemp dsReportGWP = new dsReportTemp();
            dsReportTemp.GWPSetDataTable dtGWPSetReport = (dsReportTemp.GWPSetDataTable)dsReportGWP.Tables["GWPSet"];
            foreach (GWPSetController s in GWPSetList)
            {
                dsReportTemp.GWPSetRow newRow = dtGWPSetReport.NewGWPSetRow();
                newRow.Id = s.Id.ToString();
                newRow.Name = s.Name;
                newRow.Qty = s.Qty.ToString();
                newRow.Amount = s.Amount.ToString();
                dtGWPSetReport.AddGWPSetRow(newRow);
            }
            ReportDataSource rds2 = new ReportDataSource("DataSet2", dsReportGWP.Tables["GWPSet"]);

            string reportPath = Application.StartupPath + "\\Reports\\GWP_Transactions.rdlc";

            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.LocalReport.DataSources.Add(rds2);

            ReportParameter TransCount = new ReportParameter("TransCount", InvoiceCount.ToString());
            reportViewer1.LocalReport.SetParameters(TransCount);

            ReportParameter ItemCount = new ReportParameter("ItemCount", ProductCount.ToString());
            reportViewer1.LocalReport.SetParameters(ItemCount);

            ReportParameter Header = new ReportParameter("Header", "GWP Transaction of "+Member +" from " + dtFrom.Value.Date.ToString("dd-MM-yyyy") + " To " + dtTo.Value.Date.ToString("dd-MM-yyyy"));
            reportViewer1.LocalReport.SetParameters(Header);

            reportViewer1.RefreshReport();
        }

        #endregion

        private void cboMemberType_SelectedIndexChanged(object sender, EventArgs e) {
            LoadData();
            }
        public int memberTypeId { get; set; }
        }
}
