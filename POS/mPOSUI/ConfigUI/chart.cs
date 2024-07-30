using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Objects;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using POS.APP_Data;
using System.Windows.Forms.DataVisualization.Charting;
using System.Globalization;


namespace POS
{
    public partial class chart : Form
    {
        POSEntities entity = new POSEntities();
        //Boolean Isstart = false;
        List<ReportItemSummary> itemList = new List<ReportItemSummary>();
        List<ReportItemSummary> IList = new List<ReportItemSummary>();
        long CashTotal = 0, CreditTotal = 0, FOCAmount = 0, MPUAmount = 0, TesterAmount = 0, GiftCardAmount = 0, Total = 0, CreditReceive = 0, RefundAmt = 0; long UseGiftAmount = 0; long CashAmtFromGiftCard = 0;
        //long totalSettlement = 0;
        List<Transaction> AllTranslist = new List<Transaction>();
        List<ReportItemSummary> FinalResultList = new List<ReportItemSummary>();
        List<dailysaleclass> dailychart = new List<dailysaleclass>();

        List<TopProductHolder> itemtopList = new List<TopProductHolder>();

        List<SaleBreakDownController> ResultList = new List<SaleBreakDownController>();
        List<SaleBreakDownController> FinalResultListCat = new List<SaleBreakDownController>();
        List<SaleBreakDownController> spresultList = new List<SaleBreakDownController>();

        List<CustomerInfoHolder> crlist = new List<CustomerInfoHolder>();
        List<ProfitAndLoss> plist = new List<ProfitAndLoss>();
        public chart()
        {
            InitializeComponent();
        }

        private void chart_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            LoadDataDaily();
            toptenproduct();
            salebreakdowncategory();
            Recieveable();
            NetIncome();
            MinQty();
            Payable();
            topproduct.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Zawgyi-One", 9, FontStyle.Regular);
            foreach (dailysaleclass d in dailychart.Where(d => d.Payment_Type != "Refund").ToList())
            {

                dailysalechart1.Series["DailySale"].Points.AddXY(d.Payment_Type, d.Total);

            }
            decimal dtotal = 0;
            dailychart.Where(d => d.Payment_Type != "FOC").ToList().ForEach(d => dtotal += d.Total);
            ZeroSeperate(lbltotal, (int)dtotal);
            if (itemtopList.Count != 0)
            {
                foreach (TopProductHolder d in itemtopList)
                {
                    topproduct.Series["Daily_top10product"].Points.AddXY(d.Name, d.Qty);
                }
            }
            else
            {
                topproduct.Series["Daily_top10product"].Points.AddXY("Product", 0);
            }

            foreach (SaleBreakDownController s in FinalResultListCat.OrderByDescending(x => x.saleQty).ToList())
            {
                if (s.BreakDown != 0)
                {
                    salebreakchart.Series["Sale_BreakDown_By_Category"].Points.AddXY(s.BreakDown + "%", s.BreakDown);
                }
            }
            foreach (SaleBreakDownController s in FinalResultListCat.OrderByDescending(x => x.saleQty).ToList())
            {
                if (s.BreakDown != 0)
                {
                    salebreakchart.Series["Sale_BreakDown_By_Category"].Sort(PointSortOrder.Descending, "Y");
                    int i = 0;
                    foreach (DataPoint dp in salebreakchart.Series["Sale_BreakDown_By_Category"].Points)
                    {

                        if (dp.YValues[i].ToString() == s.BreakDown.ToString())
                        {
                            dp.LegendText = s.Name;
                        }
                    }
                    i++;
                }
            }
        }
        public void FormFresh()
        {
            LoadDataDaily();
            toptenproduct();
            salebreakdowncategory();
            Recieveable();
            NetIncome();
            MinQty();
            Payable();
        }
        void ZeroSeperate(Label lab, Int32 value)
        {
            CultureInfo ci = new CultureInfo("en-us");
            lab.Text = lab.Name == "lbltotal" ? "Total Sale : " + value.ToString("N01", ci) + " Ks" : value.ToString("N01", ci) + " Ks";
            lab.Refresh();
        }
        void MinQty()
        {
            List<Product> productList = (from p in entity.Products where p.Qty <= p.MinStockQty && p.IsNotifyMinStock == true orderby p.Id select p).ToList();
            lblMinqty.Text = productList.Count().ToString();
        }
        private void Recieveable()
        {
            List<Customer> customerList = new List<Customer>();
            entity = new POSEntities();
            crlist.Clear();
            List<Transaction> transList = entity.Transactions.Where(t => t.Type == "Credit" && t.IsPaid == false && t.IsDeleted == false && t.IsCancelled == false).ToList();
            decimal receivable = 0;
            transList.ForEach(x => receivable += (decimal)(x.TotalAmount - x.RecieveAmount));
            transList.ForEach(x => receivable -= (decimal)(x.Transaction1.Where(t => t.IsDeleted == false).Sum(y => y.RecieveAmount)));// for refund
            transList.ForEach(x => receivable -= (decimal)(x.UsePrePaidDebts.Sum(y => y.UseAmount))); // for prepaid
            transList.ForEach(x => receivable -= x.CancellationLogs.Sum(y => y.CancelledAmount)); // for cancel          
            ZeroSeperate(lblReceivable, (Int32)receivable);
        }
       
        int year = 0;
        int month = 0;
        long cost = 0;
        public void NetIncome()
        {
            bool IsStart = true;
            if (IsStart == true)
            {

                int shopid = entity.Shops.Where(a => a.IsDefaultShop == true).Where(a => a.ShopName == SettingController.DefaultShop.ShopName).FirstOrDefault().Id;
                string currentshopcode = (from p in entity.Shops where p.Id == shopid select p.ShortCode).FirstOrDefault();
                string currentshopname = (from p in entity.Shops where p.Id == shopid select p.ShopName).FirstOrDefault();
                int SaleRevenue = 0, CostofGoodSlod = 0;
                year = DateTime.Now.Year;
                // monthName = DateTime.Now.Date.Month;
                month = DateTime.Now.Date.Month;// DateTime.ParseExact(monthName, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                #region Sale Revenue
                var SaleList = (from td in entity.TransactionDetails
                                join t in entity.Transactions on td.TransactionId equals t.Id
                                join p in entity.Products on td.ProductId equals p.Id
                                //join pc in entity.ProductCategories on p.ProductCategoryId equals pc.Id
                                where
                             (t.DateTime.Value.Year) == year &&
                            (t.DateTime.Value.Month) == month
                             && t.IsDeleted == false && t.PaymentTypeId != 4 && t.PaymentTypeId != 6 && t.IsComplete == true
                             && t.IsActive == true && t.Id.Substring(2, 2) == currentshopcode
                                select new
                                {
                                    TranId = td.TransactionId,
                                    ParentId = t.ParentId,
                                    Type = t.Type,
                                    TotalAmt = td.TotalAmount

                                }
                                ).ToList();

                if (SaleList.Count > 0)
                {
                    List<string> _type = new List<string> { "Sale", "Credit" };
                    var _tranIdList = SaleList.Where(x => _type.Contains(x.Type)).Select(x => x.TranId).ToList();
                    int _RefundTotalAmt = 0;
                    var _RefundList = SaleList.Where(x => _tranIdList.Contains(x.ParentId) && x.Type == "Refund").ToList();

                    _RefundTotalAmt = _RefundList.Sum(x => Convert.ToInt32(x.TotalAmt));


                    var _salerevenueList = SaleList.Where(x => _type.Contains(x.Type)).Sum(x => x.TotalAmt);
                    SaleRevenue = Convert.ToInt32(_salerevenueList) - Convert.ToInt32(_RefundTotalAmt);

                }
                #endregion

                #region Cost of Good Solds

                var GoodSoldExpList = (from pd in entity.PurchaseDetails
                                       join pdt in entity.PurchaseDetailInTransactions on pd.Id equals pdt.PurchaseDetailId
                                       join td in entity.TransactionDetails on pdt.TransactionDetailId equals td.Id
                                       join t in entity.Transactions on td.TransactionId equals t.Id
                                       join p in entity.Products on pd.ProductId equals p.Id
                                       //join pc in entity.ProductCategories on p.ProductCategoryId equals pc.Id
                                       where (t.DateTime.Value.Year) == year &&
                                      (t.DateTime.Value.Month) == month
                                            && t.IsDeleted == false && t.IsComplete == true
                                               && t.IsActive == true && pd.IsDeleted == false && td.IsDeleted == false
                                               && t.Id.Substring(2, 2) == currentshopcode
                                       select new
                                       {
                                           TotalAmt = (pdt.Qty * pd.UnitPrice),

                                       }
                              ).ToList();

                if (GoodSoldExpList.Count > 0)
                {
                    var _goodSoldTotalAmt = GoodSoldExpList.Sum(x => x.TotalAmt);
                    CostofGoodSlod = Convert.ToInt32(_goodSoldTotalAmt);
                }
                #endregion

                #region "Salary,Utilities,Rent and General  Expenses "
                var ExpenseList = (from e in entity.Expenses
                                   join ec in entity.ExpenseCategories on e.ExpenseCategoryId equals ec.Id

                                   where e.IsApproved == true && e.IsDeleted == false
                           && (e.ExpenseDate.Value.Year) == year &&
                                 (e.ExpenseDate.Value.Month) == month && e.Id.Substring(2, 2) == currentshopcode
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


                long netIncome = SaleRevenue - (CostofGoodSlod +cost);
                ZeroSeperate(lblNetgain, (Int32)netIncome);
            }
        }

        public void Payable()
        {
            List<MainPurchase> _mainPurchase = entity.MainPurchases.Where(m => m.IsCompletedPaid == false && m.IsCompletedInvoice == true && m.IsDeleted == false && m.OldCreditAmount != 0).ToList();
            decimal? payable = 0;
            _mainPurchase.ForEach(p => payable += p.OldCreditAmount);

              List<string> type = new List<string> { "Refund", "CreditRefund" };

                    //select consingment all records 
                    var ConsignList = (from td in entity.TransactionDetails
                                       join t in entity.Transactions on td.TransactionId equals t.Id
                                       join p in entity.Products on td.ProductId equals p.Id
                                       join c in entity.ConsignmentCounters on p.ConsignmentCounterId equals c.Id
                                       where p.IsConsignment == true
                                               && (td.IsConsignmentPaid == false) && (td.IsDeleted == false)
                                               && (t.Id.Substring(2,2) == SettingController.DefaultShop.ShortCode)
                                       group td by new { td.UnitPrice, td.ProductId, p.Name, td.ConsignmentPrice, t.Type, td.DiscountRate, p.Barcode } into totalConsignQty
                                       select new
                                       {
                                           ProductId = totalConsignQty.Key.ProductId,
                                           Barcode = totalConsignQty.Key.Barcode,
                                           Name = totalConsignQty.Key.Name,
                                           SellingPrice = (totalConsignQty.Key.UnitPrice) - (totalConsignQty.Key.UnitPrice * totalConsignQty.Key.DiscountRate / 100),
                                           ConsignmentPrice = totalConsignQty.Key.ConsignmentPrice,
                                           RefundQty = type.Contains(totalConsignQty.Key.Type) ? totalConsignQty.Sum(o => o.Qty) : 0,
                                           ConsginQty = !type.Contains(totalConsignQty.Key.Type) ? totalConsignQty.Sum(o => o.Qty) : 0,
                                           Type = totalConsignQty.Key.Type
                                       }).Distinct();

                    var CosignQtyList = ConsignList.Where(x => x.ConsginQty > 0).Select(x => x.ConsginQty).Sum();

                    if (CosignQtyList > 0)
                    {
                        //filter consignment list already minus refund
                        var q = (from g in ConsignList
                                                group g by new { g.ProductId, g.Name, g.SellingPrice, g.ConsignmentPrice, g.Barcode } into _gridData
                                                select new
                                                {
                                                    ProductId = _gridData.Key.ProductId,
                                                    Barcode = _gridData.Key.Barcode,
                                                    Name = _gridData.Key.Name,
                                                    SellingPrice = _gridData.Key.SellingPrice,
                                                    ConsignmentPrice = _gridData.Key.ConsignmentPrice,
                                                    ConsignQty = _gridData.Sum(s => s.ConsginQty) - _gridData.Sum(s => s.RefundQty),
                                                    TotalProfit = (_gridData.Key.SellingPrice - _gridData.Key.ConsignmentPrice) * (_gridData.Sum(s => s.ConsginQty) - _gridData.Sum(s => s.RefundQty)),
                                                    TotalConsignmentPrice = (_gridData.Sum(s => s.ConsginQty) - _gridData.Sum(s => s.RefundQty)) * _gridData.Key.ConsignmentPrice,
                                                }).ToList();

                        q.ForEach(qa => payable += qa.TotalConsignmentPrice);
                    }
          
            lblPayable.Text = payable.ToString() + " Ks";
            ZeroSeperate(lblPayable, (Int32)payable);
        }
        private void cboshoplist_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void salebreakdowncategory()
        {
            DateTime toDate = DateTime.Today;

            DateTime fromDate = DateTime.Today.AddMonths(-1);
            decimal total = 0;
            decimal spTotal = 0;
            System.Data.Objects.ObjectResult<SaleBreakDownBySegmentWithUnitValue_Result> rList = entity.SaleBreakDownBySegmentWithUnitValue(fromDate, toDate, false, SettingController.DefaultShop.ShortCode);
            List<SaleBreakDownController> ResultList2 = new List<SaleBreakDownController>();
            foreach (SaleBreakDownBySegmentWithUnitValue_Result r in rList)
            {
                SaleBreakDownController saleObj = new SaleBreakDownController();
                saleObj.bId = Convert.ToInt32(r.Id);
                saleObj.Name = r.Name;
                saleObj.Sales = (r.TotalSale == null) ? 0 : Convert.ToDecimal(r.TotalSale);
                saleObj.saleQty = (r.SaleQty == null) ? 0 : Convert.ToInt32(r.SaleQty);
                saleObj.Refund = (r.TotalRefund == null) ? 0 : Convert.ToDecimal(r.TotalRefund);
                saleObj.refundQty = (r.RefundQty == null) ? 0 : Convert.ToInt32(r.RefundQty);
                total += Convert.ToInt32(r.TotalSale);
                ResultList.Add(saleObj);
                ResultList2.Add(saleObj);
            }

            System.Data.Objects.ObjectResult<SaleBreakDownBySegmentWithUnitValue_Result> specialPromotionList = entity.SaleBreakDownBySegmentWithUnitValue(fromDate, toDate, true, SettingController.DefaultShop.ShortCode);

            foreach (SaleBreakDownBySegmentWithUnitValue_Result sp in specialPromotionList)
            {
                SaleBreakDownController saleObj = new SaleBreakDownController();
                saleObj.bId = Convert.ToInt32(sp.Id);
                saleObj.Name = sp.Name;
                saleObj.Sales = (sp.TotalSale == null) ? 0 : Convert.ToDecimal(sp.TotalSale);
                saleObj.saleQty = (sp.SaleQty == null) ? 0 : Convert.ToInt32(sp.SaleQty);
                saleObj.Refund = (sp.TotalRefund == null) ? 0 : Convert.ToDecimal(sp.TotalRefund);
                saleObj.refundQty = (sp.RefundQty == null) ? 0 : Convert.ToInt32(sp.RefundQty);
                spTotal += Convert.ToInt32(sp.TotalSale);
                spresultList.Add(saleObj);

            }

            int i = 0;
            foreach (SaleBreakDownController sb in ResultList2)
            {

                System.Data.Objects.ObjectResult<GetSaleSpecialPromotionSegmentByCustomerId_Result> splist = entity.GetSaleSpecialPromotionSegmentByCustomerId(fromDate, toDate, sb.bId, false, SettingController.DefaultShop.ShortCode);
                List<SpecialPromotionController> SPCList = new List<SpecialPromotionController>();

                foreach (GetSaleSpecialPromotionSegmentByCustomerId_Result sp in splist)
                {
                    SpecialPromotionController sObj = new SpecialPromotionController();

                    sObj.Name = sp.Name;
                    sObj.saleQty = Convert.ToInt32(sp.SaleQty);
                    sObj.Sales = Convert.ToInt32(sp.TotalSale);
                    sObj.refundQty = Convert.ToInt32(sp.RefundQty);
                    sObj.Refund = Convert.ToInt32(sp.TotalRefund);
                    SPCList.Add(sObj);

                }
                if (SPCList.Count > 0)
                {
                    ResultList[i].saleQty += SPCList[0].saleQty;
                    ResultList[i].Sales += Convert.ToDecimal(SPCList[0].Sales);
                    ResultList[i].refundQty += SPCList[0].refundQty;
                    ResultList[i].Refund += Convert.ToDecimal(SPCList[0].Refund);
                    total += Convert.ToInt32(SPCList[0].Sales);
                }

                i++;

            }





            FinalResultListCat.Clear();
            foreach (SaleBreakDownController s in ResultList)
            {
                SaleBreakDownController sObj = new SaleBreakDownController();
                s.BreakDown = (s.Sales == 0) ? 0 : Math.Round((s.Sales / total) * 100);
                FinalResultListCat.Add(s);
            }
        }
        private void toptenproduct()
        {
            DateTime toDate = DateTime.Today;

            DateTime fromDate = DateTime.Today.AddMonths(-1);

            int totalRow = 10;

            itemtopList.Clear();
            System.Data.Objects.ObjectResult<Top100SaleItemList_Result> resultList;
            resultList = entity.Top100SaleItemList(fromDate, toDate, false, totalRow, SettingController.DefaultShop.ShortCode);
            foreach (Top100SaleItemList_Result r in resultList)
            {
                TopProductHolder p = new TopProductHolder();
                if (itemtopList.Where(x => x.ProductId == r.ProductCode).FirstOrDefault() != null)
                {
                    p = itemtopList.Where(x => x.ProductId == r.ProductCode).FirstOrDefault();

                    p.Qty += Convert.ToInt32(r.Qty);

                }
                else
                {
                    p.ProductId = r.ProductCode;
                    p.Name = r.ProductName;
                    p.Discount = 0;
                    p.UnitPrice = Convert.ToInt64(r.UnitPrice);
                    p.Qty = Convert.ToInt32(r.Qty);
                    p.totalAmount = Convert.ToInt64(r.Amount);
                    itemtopList.Add(p);
                }

            }
        }
        private void LoadDataDaily()
        {

            List<Transaction> transList = new List<Transaction>();
            List<Transaction> DtransList = new List<Transaction>();

            DateTime toDate = DateTime.Today;

            DateTime fromDate = DateTime.Today.AddMonths(-1);
            string currentshortcode = (from p in entity.Shops where p.Id == SettingController.DefaultShop.Id select p.ShortCode).FirstOrDefault();

            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit || t.Type == TransactionType.Refund || t.Type == TransactionType.CreditRefund) && (t.IsDeleted == null || t.IsDeleted == false) && t.Id.Substring(2, 2) == currentshortcode select t).ToList<Transaction>();

            DtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Settlement || t.Type == TransactionType.Prepaid) && (t.IsDeleted == null || t.IsDeleted == false) && t.Id.Substring(2, 2) == currentshortcode select t).ToList<Transaction>();

            bool IsCash = true, IsCredit = true, IsFOC = true, IsMPU = true, IsGiftCard = true, IsTester = true, IsRefund = true;
            CashTotal = 0; CreditTotal = 0; FOCAmount = 0; MPUAmount = 0; TesterAmount = 0; GiftCardAmount = 0; RefundAmt = 0; Total = 0;

            System.Data.Objects.ObjectResult<SelectItemListByDate_Result> resultList;
            resultList = entity.SelectItemListByDate(fromDate, toDate, currentshortcode);
            foreach (SelectItemListByDate_Result r in resultList)
            {
                ReportItemSummary p = new ReportItemSummary();
                p.Id = r.ItemId;
                p.Name = r.ItemName;
                p.Qty = (int)r.ItemQty;
                p.UnitPrice = Convert.ToInt32(r.UnitPrice);
                p.SellingPrice = Convert.ToInt32(r.SellingPrice);
                p.discountrate = (int)r.DiscountRate;
                p.totalAmount = Convert.ToInt64(r.ItemTotalAmount);
                p.PaymentId = (int)r.PaymentTypeId;
                p.Size = r.Size;
                p.IsFOC = Convert.ToBoolean(r.IsFOC);

                //if (p.IsFOC == true)
                //{
                //    p.Remark = "FOC";
                //}
                //else
                //{
                //    p.Remark = "";
                //}
                p.Remark = r.Type;
                FinalResultList.Add(p);
            }
            AllTranslist.Clear();
            CreditReceive = 0;
            UseGiftAmount = 0;
            //if (IsSale == true)
            //{

            string[] type = { "Refund", "CreditRefund" };
            if (IsCash)
            {
                itemList.AddRange(FinalResultList.Where(x => x.PaymentId == 1 && !type.Contains(x.Remark)).ToList());
                CashTotal += FinalResultList.Where(x => x.PaymentId == 1 && !type.Contains(x.Remark)).Sum(x => x.totalAmount);
                AllTranslist.AddRange(transList.Where(x => x.PaymentTypeId == 1 && !type.Contains(x.Type)).ToList());
                dailysaleclass sale = new dailysaleclass();
                sale.Payment_Type = "Cash";
                sale.Total = CashTotal;
                dailychart.Add(sale);
            }
            if (IsCredit)
            {
                itemList.AddRange(FinalResultList.Where(x => x.PaymentId == 2 && !type.Contains(x.Remark)).ToList());
                CreditTotal += FinalResultList.Where(x => x.PaymentId == 2 && !type.Contains(x.Remark)).Sum(x => x.totalAmount);
                AllTranslist.AddRange(transList.Where(x => x.PaymentTypeId == 2 && !type.Contains(x.Type)).ToList());
                CreditReceive += Convert.ToInt64(transList.Where(x => x.PaymentTypeId == 2 && !type.Contains(x.Type)).Sum(x => x.RecieveAmount));
                dailysaleclass sale = new dailysaleclass();
                sale.Payment_Type = "Credit";
                sale.Total = CreditTotal;
                dailychart.Add(sale);
            }
            if (IsGiftCard)
            {
                itemList.AddRange(FinalResultList.Where(x => x.PaymentId == 3 && !type.Contains(x.Remark)).ToList());
                GiftCardAmount += FinalResultList.Where(x => x.PaymentId == 3 && !type.Contains(x.Remark)).Sum(x => x.totalAmount);
                AllTranslist.AddRange(transList.Where(x => x.PaymentTypeId == 3 && !type.Contains(x.Type)).ToList());
                UseGiftAmount += Convert.ToInt64(transList.Where(x => x.PaymentTypeId == 3 && !type.Contains(x.Type)).Sum(x => x.GiftCardAmount));
                CashAmtFromGiftCard += Convert.ToInt64(transList.Where(x => x.PaymentTypeId == 3 && !type.Contains(x.Type)).Sum(x => x.TotalAmount - x.GiftCardAmount));
                //List<Transaction> GTransList = transList.Where(x => x.PaymentTypeId == 3).ToList();
                //foreach (Transaction t in GTransList)
                //{
                //    List<GiftCardInTransaction> GList = t.GiftCardInTransactions.ToList();
                //    foreach (GiftCardInTransaction g in GList)
                //    {
                //        UseGiftAmount += Convert.ToInt64(g.GiftCardAmount);
                //    }
                //}
                dailysaleclass sale = new dailysaleclass();
                sale.Payment_Type = "GiftCard";
                sale.Total = GiftCardAmount;
                dailychart.Add(sale);
            }
            if (IsRefund)
            {

                RefundAmt += FinalResultList.Where(x => type.Contains(x.Remark)).Sum(x => x.SellingPrice * x.Qty);
                AllTranslist.AddRange(transList.Where(x => type.Contains(x.Type)).ToList());
                // lblPeriod.Text = fromDate.ToString() + " to " + toDate.ToString();
                // lblNumberofTransaction.Text = transList.Count.ToString();

                //lblTotalAmount.Text = "";

                // itemList.AddRange(FinalResultList.Where(x
                dailysaleclass sale = new dailysaleclass();
                sale.Payment_Type = "Refund";
                sale.Total = RefundAmt;
                //dailychart.Add(sale);
            }
            if (IsFOC)
            {
                itemList.AddRange(FinalResultList.Where(x => x.PaymentId == 4).ToList());
                //  FOCAmount += FinalResultList.Where(x => x.PaymentId == 4).Sum(x => x.totalAmount);
                FOCAmount += FinalResultList.Where(x => x.IsFOC == true).Sum(x => x.SellingPrice * x.Qty);
                AllTranslist.AddRange(transList.Where(x => x.PaymentTypeId == 4).ToList());
                dailysaleclass sale = new dailysaleclass();
                sale.Payment_Type = "FOC";
                sale.Total = FOCAmount;
                dailychart.Add(sale);
            }
            if (IsMPU)
            {
                itemList.AddRange(FinalResultList.Where(x => x.PaymentId == 5).ToList());
                MPUAmount += FinalResultList.Where(x => x.PaymentId == 5).Sum(x => x.totalAmount);
                AllTranslist.AddRange(transList.Where(x => x.PaymentTypeId == 5).ToList());
                dailysaleclass sale = new dailysaleclass();
                sale.Payment_Type = "MPU";
                sale.Total = MPUAmount;
                dailychart.Add(sale);
            }
        }

        private void lblMinqty_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PurchaseOrderItem poi = new PurchaseOrderItem();
            poi.ShowDialog();
        }
    }
}
