using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using POS.APP_Data;
using Microsoft.Reporting.WinForms;
using System.Data.Objects;

namespace POS
{
    public partial class frmNetIncomeReport : Form
    {
        #region Initialized
        public frmNetIncomeReport()
        {
            InitializeComponent();
        }
        #endregion

        #region variable
        POSEntities entity = new POSEntities();
        int year = 0;
        int month = 0;
        string monthName = string.Empty;
        Boolean IsStart = false;
        #endregion

        private void frmNetIncomeReport_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            var startYear = Convert.ToDateTime(SettingController.Company_StartDate).Year;
            cboYear.DataSource = Enumerable.Range(startYear, 100).ToList();
            cboYear.Text = DateTime.Now.Year.ToString();
            cboMonth.Text = DateTime.Now.ToString("MMMM");

            Utility.BindShop(cboshoplist, true);

            cboshoplist.Text = SettingController.DefaultShop.ShopName;
            Utility.ShopComBo_EnableOrNot(cboshoplist);
            cboshoplist.SelectedIndex = 0;
            IsStart = true;
            LoadData();
        }


        private void LoadData()
        {
            if (IsStart == true)
            {
                try
                {

                    #region user filter
                    int shopid = Convert.ToInt32(cboshoplist.SelectedValue);
                    string currentshopcode = "";
                    string currentshopname = "";
                    if (shopid != 0)
                    {
                        currentshopcode = (from p in entity.Shops where p.Id == shopid select p.ShortCode).FirstOrDefault();
                        currentshopname = (from p in entity.Shops where p.Id == shopid select p.ShopName).FirstOrDefault();
                    }
                    else
                    {
                        currentshopname = "ALL";
                    }
                    long SaleRevenue = 0, SaleQty = 0, RefundTotalAmt = 0, SaleAmt = 0;
                    long CostofGoodSlod = 0;
                    year = Convert.ToInt32(cboYear.SelectedValue);
                    monthName = cboMonth.Text;
                    month = DateTime.ParseExact(monthName, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                    #endregion

                    #region Sale Revenue
                    if (SettingController.IsSourcecode)
                    {
                        var SaleList = entity.Transactions.Where(x => x.DateTime.Value.Year == year
                                                                                 && x.DateTime.Value.Month == month
                                                                                 && x.IsComplete == true
                                                                                && (x.Type == TransactionType.Prepaid || x.Type == TransactionType.Settlement)
                                                                                 && (x.IsDeleted == null || x.IsDeleted == false)).ToList();
                        if (SaleList.Count > 0)
                        {
                            SaleAmt = Convert.ToInt64(SaleList.Sum(x => x.TotalAmount));
                            SaleRevenue = SaleAmt - Convert.ToInt32(RefundTotalAmt);
                        }
                    }
                    else
                    {
                        var SaleList = (from td in entity.TransactionDetails
                                        join t in entity.Transactions on td.TransactionId equals t.Id
                                        join p in entity.Products on td.ProductId equals p.Id
                                        where
                                    (t.DateTime.Value.Year) == year &&
                                    (t.DateTime.Value.Month) == month
                                     && t.IsDeleted == false && t.PaymentTypeId != 4 && t.PaymentTypeId != 6 && t.IsComplete == true
                                     && t.IsActive == true && ((shopid != 0 && t.Id.Substring(2, 2) == currentshopcode) || (shopid == 0 && 1 == 1))
                                        select new
                                        {
                                            TranId = td.TransactionId,
                                            ParentId = t.ParentId,
                                            Type = t.Type,
                                            TotalAmt = td.TotalAmount,
                                            Quantity = td.Qty
                                        }
                                    ).ToList();

                        if (SaleList.Count > 0)
                        {
                            List<string> _type = new List<string> { "Sale", "Credit" };
                            var _tranIdList = SaleList.Where(x => _type.Contains(x.Type)).Select(x => x.TranId).ToList();
                            var _RefundList = SaleList.Where(x => _tranIdList.Contains(x.ParentId) && x.Type == "Refund").ToList();
                            RefundTotalAmt = _RefundList.Sum(x => Convert.ToInt32(x.TotalAmt));

                            var _salerevenueList = SaleList.Where(x => _type.Contains(x.Type)).Sum(x => x.TotalAmt);
                            SaleQty = Convert.ToInt16(SaleList.Where(y => y.Type != "Refund").Sum(x => x.Quantity));
                            SaleAmt = Convert.ToInt32(_salerevenueList);
                            SaleRevenue = Convert.ToInt32(_salerevenueList) - Convert.ToInt32(RefundTotalAmt);
                        }
                    }
                    #endregion

                    #region Cost of Good Solds
                    int OpeningBalanceQty = 0, PurchaseQty = 0, ClosingBalanceQty = 0;
                    long OpeningBalanceAmt = 0, PurchaseAmt = 0, ClosingBalanceAmt = 0;
                    var data = entity.NetIncomeReportByYearMonth(year, month);
                    if (SettingController.IsSourcecode)
                    {
                        foreach (var item in data)
                        {
                            int ProductPrice = 0;
                            //finding the product price from the purchase detail
                            PurchaseDetail purchaseDetailEntity = entity.PurchaseDetails.FirstOrDefault(x => x.ProductId == item.Id && x.IsDeleted == false && x.Date.Value.Year == year && x.Date.Value.Month == month);
                            if (purchaseDetailEntity != null) ProductPrice = (int)purchaseDetailEntity.UnitPrice.Value;
                            PurchaseQty += (int)item.Purchase;
                            PurchaseAmt += ((int)item.Purchase) * ProductPrice;
                        }
                    }
                    else
                    {
                        foreach (var item in data)
                        {
                            int ProductPrice = 0;
                            //finding the product price from the purchase detail
                            PurchaseDetail purchaseDetailEntity = entity.PurchaseDetails.FirstOrDefault(x => x.ProductId == item.Id && x.IsDeleted == false && x.Date.Value.Year == year && x.Date.Value.Month == month);
                            List<PurchaseDetail> purchaseDetail = (from m in entity.MainPurchases
                                                                   join p in entity.PurchaseDetails on m.Id equals p.MainPurchaseId
                                                                   where p.IsDeleted == false && m.IsCompletedInvoice == true && p.ProductId == item.Id && p.Date.Value.Year == year && p.Date.Value.Month == month
                                                                   select p).ToList();
                            //if (purchaseDetailEntity != null)
                            //{
                            //    ProductPrice = (int)purchaseDetailEntity.UnitPrice.Value;
                            //}

                            if (purchaseDetail.Count > 0)
                            {
                                foreach (var d in purchaseDetail)
                                {
                                    var i = d.CurrentQy * d.UnitPrice;
                                    ClosingBalanceAmt += (long)i;
                                    PurchaseAmt += (long)(d.Qty * d.UnitPrice);
                                    OpeningBalanceAmt += ((int)item.Opening) * ((int)d.UnitPrice);
                                   
                                }
                                OpeningBalanceQty += (int)item.Opening;
                                PurchaseQty += (int)item.Purchase;
                                ClosingBalanceQty += (int)item.Closing;
                            }
                            else
                            {
                                //finding the product price from the purchse detail backwrok to 1 monthe latter.
                                //purchaseDetailEntity = entity.PurchaseDetails.FirstOrDefault(x => x.ProductId == item.Id && x.IsDeleted == false && x.Date.Value.Year == year && x.Date.Value.Month == month - (month - DateTime.Now.Month));
                                purchaseDetail = (from m in entity.MainPurchases
                                                  join p in entity.PurchaseDetails on m.Id equals p.MainPurchaseId
                                                  where p.IsDeleted == false && m.IsCompletedInvoice == true && p.ProductId == item.Id && p.Date.Value.Year == year && p.Date.Value.Month == month - (month - DateTime.Now.Month)
                                                  select p).ToList();
                                //if (purchaseDetailEntity != null)
                                //{
                                //    ProductPrice = (int)purchaseDetailEntity.UnitPrice.Value;
                                //}
                                if (purchaseDetail.Count > 0)
                                {
                                    foreach (var d in purchaseDetail)
                                    {
                                        var i = d.CurrentQy * d.UnitPrice;
                                        ClosingBalanceAmt += (long)i;
                                        PurchaseAmt += (long)(item.Purchase * d.UnitPrice);
                                        OpeningBalanceAmt += ((int)d.CurrentQy) * ((int)d.UnitPrice);

                                    }
                                }
                                //finding the product price from the product table
                                else
                                {
                                    Product productEntity = entity.Products.FirstOrDefault(x => x.Id == item.Id);
                                    ProductPrice = (int)productEntity.Price;
                                    ClosingBalanceAmt += ((int)item.Closing) * ProductPrice;
                                    PurchaseAmt += (int)item.Purchase * ProductPrice;                                   
                                    OpeningBalanceAmt += ((int)item.Opening) * ProductPrice;
                                    
                                }
                                OpeningBalanceQty += (int)item.Opening;
                                PurchaseQty += (int)item.Purchase;
                                ClosingBalanceQty += (int)item.Closing;

                                //ClosingBalanceAmt += ((int)item.Closing) * ProductPrice;
                            }//end of foreach loop for finding purchase,opening,closing qty and amount.
                        }//end of else [normal pos system]
                        CostofGoodSlod = (OpeningBalanceAmt + PurchaseAmt) - ClosingBalanceAmt;
                        #endregion

                        #region Expense data getting
                        var ExpenseList = (from e in entity.Expenses
                                           join ec in entity.ExpenseCategories on e.ExpenseCategoryId equals ec.Id

                                           where e.IsApproved == true && e.IsDeleted == false
                                           && (e.ExpenseDate.Value.Year) == year &&
                                         (e.ExpenseDate.Value.Month) == month
                                         && ((shopid != 0 && e.Id.Substring(2, 2) == currentshopcode) || (shopid == 0 && 1 == 1))
                                           select new
                                           {
                                               TotalAmount = e.TotalExpenseAmount,
                                               ExpCag = ec.Name
                                           }
                                       ).ToList().GroupBy(x => x.ExpCag).ToList()
                                       .Select(x => new
                                       {
                                           ExpCag = x.First().ExpCag,
                                           TotalAmount = x.Sum(xl => xl.TotalAmount)
                                       }).ToList();
                        #endregion

                        #region Assign Data to For DataSet
                        dsReportTemp dsReport = new dsReportTemp();
                        dsReportTemp.NetIncomeDataTable dtPdReport = (dsReportTemp.NetIncomeDataTable)dsReport.Tables["NetIncome"];
                        foreach (var item in ExpenseList)
                        {
                            dsReportTemp.NetIncomeRow newRow = dtPdReport.NewNetIncomeRow();
                            newRow.ExpenseCategory = item.ExpCag;
                            newRow.Cost = (long)item.TotalAmount;
                            dtPdReport.AddNetIncomeRow(newRow);
                        }
                        #endregion

                        #region Show Report Viewer Part

                        ReportDataSource rds = new ReportDataSource("NetIncomeDataSet", dsReport.Tables["NetIncome"]);
                        string reportPath = string.Empty;
                        if (SettingController.IsSourcecode) reportPath = Application.StartupPath + "\\Reports\\SourcecodeNetIncomeReport.rdlc";
                        else reportPath = Application.StartupPath + "\\Reports\\NetIncomeReport.rdlc";

                        reportViewer1.LocalReport.ReportPath = reportPath;
                        reportViewer1.LocalReport.DataSources.Clear();
                        this.reportViewer1.ZoomMode = ZoomMode.Percent;
                        reportViewer1.LocalReport.DataSources.Add(rds);

                        ReportParameter Month = new ReportParameter("MonthName", cboMonth.Text.ToString());
                        reportViewer1.LocalReport.SetParameters(Month);

                        ReportParameter ShortName = new ReportParameter("ShopName", currentshopname);
                        reportViewer1.LocalReport.SetParameters(ShortName);

                        ReportParameter TSaleRevenue = new ReportParameter("SalesRevenue", SaleRevenue.ToString());
                        reportViewer1.LocalReport.SetParameters(TSaleRevenue);

                        ReportParameter GoodSold = new ReportParameter("CostOfGoodSold", CostofGoodSlod.ToString());
                        reportViewer1.LocalReport.SetParameters(GoodSold);

                        //new customization field start here.
                        ReportParameter SaleReturn = new ReportParameter("SaleReturn", "SaleReturn");
                        reportViewer1.LocalReport.SetParameters(SaleReturn);

                        ReportParameter SaleQtyp = new ReportParameter("SaleQty", "( " + SaleQty.ToString() + " Qty )");
                        reportViewer1.LocalReport.SetParameters(SaleQtyp);

                        ReportParameter parSaleAmt = new ReportParameter("SaleAmt", SaleAmt.ToString());
                        reportViewer1.LocalReport.SetParameters(parSaleAmt);

                        ReportParameter RefundAmt = new ReportParameter("RefundAmt", RefundTotalAmt.ToString());
                        reportViewer1.LocalReport.SetParameters(RefundAmt);

                        ReportParameter parOpeningBalanceQty = new ReportParameter("OpeningBalanceQty", "( " + OpeningBalanceQty.ToString() + " Qty )");
                        reportViewer1.LocalReport.SetParameters(parOpeningBalanceQty);

                        ReportParameter parOpeningBalanceAmt = new ReportParameter("OpeningBalanceAmt", OpeningBalanceAmt.ToString());
                        reportViewer1.LocalReport.SetParameters(parOpeningBalanceAmt);

                        ReportParameter parPurchaseQty = new ReportParameter("PurchaseQty", "( " + PurchaseQty.ToString() + " Qty )");
                        reportViewer1.LocalReport.SetParameters(parPurchaseQty);


                        ReportParameter parPurchaseAmt = new ReportParameter("PurchaseAmt", PurchaseAmt.ToString());
                        reportViewer1.LocalReport.SetParameters(parPurchaseAmt);

                        ReportParameter parClosingBalanceQty = new ReportParameter("ClosingBalanceQty", "( " + ClosingBalanceQty.ToString() + " Qty )");
                        reportViewer1.LocalReport.SetParameters(parClosingBalanceQty);


                        ReportParameter parClosingBalanceAmt = new ReportParameter("ClosingBalanceAmt", ClosingBalanceAmt.ToString());
                        reportViewer1.LocalReport.SetParameters(parClosingBalanceAmt);
                        reportViewer1.RefreshReport();

                        #endregion

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Some Error Occur in mpos :(");
                    return;
                }
            }

        }

        private void cboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cboMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cboshoplist_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
