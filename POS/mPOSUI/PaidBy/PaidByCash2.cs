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
    public partial class PaidByCash2 : Form
    {
        #region Variables
        string shopname = string.Empty;
        string tranno = string.Empty;
        public List<TransactionDetail> DetailList = new List<TransactionDetail>();
        public List<GiftSystem> GiftList = new List<GiftSystem>();
        public int Discount { get; set; }
        public string transactionParentId { get; set; }
        public int Tax { get; set; }

        public int ExtraDiscount { get; set; }

        public int ExtraTax { get; set; }

        public Boolean isDraft { get; set; }

        public Boolean isDebt { get; set; }

        public string DraftId { get; set; }

        public string DebtId { get; set; }

        public long DebtAmount { get; set; }

        public Boolean IsWholeSale { get; set; }
        public int Frequency { get; set; }


        public long GiftDiscountAmt { get; set; }
        public long PrePaidAmount { get; set; }

        public List<Transaction> CreditTransaction { get; set; }

        public List<Transaction> PrePaidTransaction { get; set; }

        private POSEntities entity = new POSEntities();

        private ToolTip tp = new ToolTip();

        private long totalAmount = 0, prePaidAmount = 0;

        public decimal BDDiscount { get; set; }

        public decimal MCDiscount { get; set; }

        public int CustomerId { get; set; }

        public int? MemberTypeId { get; set; }

        public decimal? MCDiscountPercent { get; set; }

        public DialogResult _result;

        public decimal TotalAmt = 0;

        private decimal AmountWithExchange = 0;

        string CurrencySymbol = string.Empty;

        long total;

        string resultId = "-";

        int Qty = 0;

        List<Stock_Transaction> productList = new List<Stock_Transaction>();

        public List<string> TranIdList = new List<string>();

        Transaction CreditT = new Transaction();

        public Boolean IsPrint = true;
        public string Note = "";

        public int? tableId = null;
        public int servicefee = 0;
        #endregion

        public PaidByCash2()
        {
            InitializeComponent();
        }

        #region Hot keys handler
        void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.U)      //  Ctrl + U => Focus Currency
            {
                cboCurrency.DroppedDown = true;
                if (cboCurrency.Focused != true)
                {
                    cboCurrency.Focus();
                }
            }
            else if (e.Control && e.KeyCode == Keys.R)      // Ctrl + R => Focus Receive Amt
            {
                cboCurrency.DroppedDown = false;
                txtReceiveAmount.Focus();
            }
            else if (e.Control && e.KeyCode == Keys.S)      // Ctrl + S => Click Save
            {
                cboCurrency.DroppedDown = false;
                btnSubmit.PerformClick();
            }
            else if (e.Control && e.KeyCode == Keys.C)      // Ctrl + C => Focus DropDown Customer
            {
                cboCurrency.DroppedDown = false;
                btnCancel.PerformClick();
            }
        }
        #endregion

        private void PaidByCash2_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            #region Setting Hot Kyes For the Controls
            SendKeys.Send("%"); SendKeys.Send("%"); // Clicking "Alt" on page load to show underline of Hot Keys
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form_KeyDown);
            #endregion

            #region currency
            Currency curreObj = new Currency();
            List<Currency> currencyList = new List<Currency>();
            currencyList = entity.Currencies.ToList();
            foreach (Currency c in currencyList)
            {
                cboCurrency.Items.Add(c.CurrencyCode);
            }
            int id = 0;
            if (SettingController.DefaultCurrency != 0)
            {
                id = Convert.ToInt32(SettingController.DefaultCurrency);
                curreObj = entity.Currencies.FirstOrDefault(x => x.Id == id);
                cboCurrency.Text = curreObj.CurrencyCode;
            }
            //txtExchangeRate.Text = SettingController.DefaultExchangeRate.ToString();
            #endregion

            if (!isDebt)
            {
                total = (long)(DetailList.Sum(x => x.TotalAmount) + (((long)DetailList.Sum(x => x.TotalAmount) * servicefee) / 100) - ExtraDiscount - BDDiscount - MCDiscount) - GiftDiscountAmt;
                lblTotalCost.Text = Utility.CalculateExchangeRate(id, total).ToString();
                TotalAmt = Convert.ToDecimal(lblTotalCost.Text);
                AmountWithExchange = Convert.ToDecimal(lblTotalCost.Text);
            }
            else
            {
                foreach (Transaction tObj in CreditTransaction)
                {
                    if (tObj.Transaction1.Count <= 0)
                    {
                        totalAmount += (long)tObj.TotalAmount - (long)tObj.RecieveAmount;
                    }
                    //Has refund
                    else
                    {
                        totalAmount += (long)tObj.TotalAmount - (long)tObj.RecieveAmount;
                        foreach (Transaction Refund in tObj.Transaction1.Where(x => x.IsDeleted != true && x.Type=="CreditRefund"))
                        {
                            totalAmount -= (long)Refund.RecieveAmount;
                        }
                    }
                    if (tObj.UsePrePaidDebts != null)
                    {
                        long prepaid = (long)tObj.UsePrePaidDebts.Sum(x => x.UseAmount);
                        totalAmount -= prepaid;
                    }
                }
                foreach (Transaction tObj in PrePaidTransaction)
                {
                    prePaidAmount += (long)tObj.TotalAmount;
                    long useAmount = (tObj.UsePrePaidDebts1 == null) ? 0 : (int)tObj.UsePrePaidDebts1.Sum(x => x.UseAmount);
                    prePaidAmount -= useAmount;
                }
                DebtAmount = (totalAmount);
                lblTotalCost.Text = Utility.CalculateExchangeRate(id, DebtAmount).ToString();
            }
            // txtReceiveAmount.Focus();
            if (SettingController.TicketSale)
            {
                txtReceiveAmount.Text = lblTotalCost.Text;
            }
        }
        class TD4Ticket
        {
            public long? Tdid { get; set; }
            public int? Qty { get; set; }

            public string Category { get; set; }
        }
        List<TD4Ticket> tdlist = new List<TD4Ticket>();
        public List<PurchaseDetail> GetPulist(Product product)
        {
            return new List<PurchaseDetail>();
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            // string _defaultPrinter = Utility.GetDefaultPrinter();
            Boolean hasError = false;
            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            long receiveAmount = 0;
            long totalCost = (long)DetailList.Sum(x => x.TotalAmount) + (((long)DetailList.Sum(x => x.TotalAmount) * servicefee) / 100) - ExtraDiscount - (long)BDDiscount - (long)MCDiscount - GiftDiscountAmt;
            //total cost wint unit price
            long unitpriceTotalCost = (long)DetailList.Sum(x => x.UnitPrice * x.Qty);
            Int64.TryParse(txtReceiveAmount.Text, out receiveAmount);
            decimal totalCashSaleAmount = Convert.ToDecimal(lblTotalCost.Text);

            if (cboCurrency.SelectedIndex == -1)
            {
                tp.SetToolTip(cboCurrency, "Error");
                tp.Show("Please select currency!", cboCurrency);
                return;
            }
            string currVal = cboCurrency.Text;
            int currencyId = (from c in entity.Currencies where c.CurrencyCode == currVal select c.Id).SingleOrDefault();
            Currency cu = entity.Currencies.FirstOrDefault(x => x.Id == currencyId);

            //Validation
            if (receiveAmount == 0)
            {
                tp.SetToolTip(txtReceiveAmount, "Error");
                tp.Show("Please fill up receive amount!", txtReceiveAmount);
                hasError = true;
            }
            else if (receiveAmount < AmountWithExchange)
            {
                tp.SetToolTip(txtReceiveAmount, "Error");
                tp.Show("Receive amount must be greater than total cost!", txtReceiveAmount);
                hasError = true;
            }

            if (!hasError)
            {

                System.Data.Objects.ObjectResult<String> Id;
                CurrencySymbol = string.Empty;
                Transaction insertedTransaction = new Transaction();
                List<Transaction> RefundList = new List<Transaction>();
                decimal change = 0;
                if (cu.CurrencyCode == "USD")
                {
                    totalCashSaleAmount = (decimal)totalCashSaleAmount * (decimal)cu.LatestExchangeRate;
                    receiveAmount = receiveAmount * (long)cu.LatestExchangeRate;
                    CurrencySymbol = "$";
                    change = Convert.ToDecimal(lblChanges.Text) * (decimal)cu.LatestExchangeRate;
                }
                else
                {
                    CurrencySymbol = "Ks";
                    change = Convert.ToDecimal(lblChanges.Text);
                }
                if (!isDebt)
                {
                    Id = entity.InsertTransaction(DateTime.Now, MemberShip.UserId, MemberShip.CounterId, TransactionType.Sale, true, true, 1, ExtraTax + Tax, ExtraDiscount + Discount, totalCost, totalCashSaleAmount, null, CustomerId, MCDiscount, BDDiscount, MemberTypeId, MCDiscountPercent, false, "", IsWholeSale,GiftDiscountAmt, SettingController.DefaultShop.Id, SettingController.DefaultShop.ShortCode, Note, tableId, servicefee, null);
                    entity = new POSEntities();
                    resultId = Id.FirstOrDefault().ToString();
                    insertedTransaction = (from trans in entity.Transactions where trans.Id == resultId select trans).FirstOrDefault<Transaction>();
                    string TId = insertedTransaction.Id;
                    insertedTransaction.IsDeleted = false;
                    insertedTransaction.ReceivedCurrencyId = currencyId;
                    foreach (TransactionDetail detail in DetailList)
                    {
                        detail.IsDeleted = false;//Update IsDelete (Null to 0)
                        if (detail.ConsignmentPrice == null)
                        {
                            detail.ConsignmentPrice = 0;
                        }

                        detail.Product = (from prod in entity.Products where prod.Id == (long)detail.ProductId select prod).FirstOrDefault();

                        Boolean? IsConsignmentPaid = Utility.IsConsignmentPaid(detail.Product);

                        var detailID = entity.InsertTransactionDetail(TId, Convert.ToInt32(detail.ProductId), Convert.ToInt32(detail.Qty), Convert.ToInt32(detail.UnitPrice), Convert.ToDouble(detail.DiscountRate), Convert.ToDouble(detail.TaxRate), Convert.ToInt32(detail.TotalAmount), detail.IsDeleted, detail.ConsignmentPrice, IsConsignmentPaid, detail.IsFOC, detail.SellingPrice).SingleOrDefault();
                        //add ticket
                        if (SettingController.TicketSale && detail.Product.ProductCategory.Name == "Ticket")
                        {
                            TD4Ticket td4tk = new TD4Ticket();
                            td4tk.Tdid = (long?)detailID;
                            td4tk.Qty = detail.Qty;
                            td4tk.Category = detail.Product.ProductSubCategory == null ? "" : detail.Product.ProductSubCategory.Name;
                            tdlist.Add(td4tk);
                        }
                        if (detail.Product.IsPackage == false && detail.Product.IsService == false)
                        {
                            detail.Product.Qty = detail.Product.Qty - detail.Qty;
                        }

                        //save in stocktransaction
                        Stock_Transaction st = new Stock_Transaction();
                        st.ProductId = detail.Product.Id;
                        if (detail.Product.IsService == false && detail.Product.IsPackage == false)
                        {
                            Qty = Convert.ToInt32(detail.Qty);
                        }

                        st.Sale = Qty;
                        productList.Add(st);
                        Qty = 0;
                        if (detail.Product.IsWrapper == true)
                        {//to add qty for child item
                            List<WrapperItem> wList = detail.Product.WrapperItems.ToList();
                            if (wList.Count > 0)
                            {
                                foreach (WrapperItem w in wList)
                                {
                                    Product wpObj = (from p in entity.Products where p.Id == w.ChildProductId select p).FirstOrDefault();
                                    wpObj.Qty = wpObj.Qty - (w.ChildQty * detail.Qty);
                                    SPDetail spDetail = new SPDetail();
                                    spDetail.TransactionDetailID = Convert.ToInt32(detailID);
                                    spDetail.DiscountRate = detail.DiscountRate;
                                    spDetail.ParentProductID = w.ParentProductId;
                                    spDetail.ChildProductID = w.ChildProductId;
                                    spDetail.ChildQty = w.ChildQty * detail.Qty;
                                    spDetail.Price = wpObj.Price;
                                    entity.insertSPDetail(spDetail.TransactionDetailID, spDetail.ParentProductID, spDetail.ChildProductID, spDetail.Price, spDetail.DiscountRate, "PC", spDetail.ChildQty);
                                    Stock_Transaction stwp = new Stock_Transaction();
                                    stwp.ProductId = w.ChildProductId;
                                    Qty = Convert.ToInt32(w.ChildQty * detail.Qty);
                                    stwp.Sale = Qty;
                                    productList.Add(stwp);
                                    Qty = 0;
                                }
                            }
                        }//end of product is Wrapper.
                        if (detail.Product.IsPackage == true)
                        {
                            POSEntities posEntity = new POSEntities();
                            PackagePurchasedInvoice packagePurchasedInvoice = new PackagePurchasedInvoice();
                            packagePurchasedInvoice.PackagePurchasedInvoiceId = System.Guid.NewGuid().ToString();
                            packagePurchasedInvoice.TransactionDetailId = Convert.ToInt64(detailID);
                            packagePurchasedInvoice.CustomerId = CustomerId;
                            packagePurchasedInvoice.InvoiceDate = DateTime.Now;
                            packagePurchasedInvoice.ProductId = (long)detail.ProductId;
                            packagePurchasedInvoice.packageFrequency = detail.Product.PackageQty;
                            packagePurchasedInvoice.UseQty = 0;
                            packagePurchasedInvoice.IsDelete = false;
                            packagePurchasedInvoice.UserId = MemberShip.UserId;
                            posEntity.PackagePurchasedInvoices.Add(packagePurchasedInvoice);
                            posEntity.SaveChanges();
                        }//end of product Is Package
                        entity.SaveChanges();
                    }//end of transaction detail

                    //Add promotion gift records for this transaction
                    if (GiftList.Count > 0)
                    {
                        foreach (GiftSystem gs in GiftList)
                        {
                            GiftSystemInTransaction gft = new GiftSystemInTransaction();
                            gft.GiftSystemId = gs.Id;
                            gft.TransactionId = insertedTransaction.Id;
                            entity.GiftSystemInTransactions.Add(gft);
                        }
                    }
                    //saving to db gift item lists
                    entity.SaveChanges();

                    //save in stocktransaction
                    Save_SaleQty_ToStockTransaction(productList);
                    productList.Clear();

                    ExchangeRateForTransaction ex = new ExchangeRateForTransaction();
                    ex.TransactionId = TId;
                    ex.CurrencyId = cu.Id;
                    ex.ExchangeRate = Convert.ToInt32(cu.LatestExchangeRate);
                    entity.ExchangeRateForTransactions.Add(ex);
                    entity.SaveChanges();


                    #region purchase
                    // for Purchase Detail and PurchaseDetailInTransacton.

                    foreach (TransactionDetail detail in insertedTransaction.TransactionDetails)
                    {
                        int pId = Convert.ToInt32(detail.ProductId);

                        if (detail.Product.IsWrapper == true)
                        {//ZP Get purchase detail with child product Id and order by purchase date ascending
                            List<WrapperItem> wList = detail.Product.WrapperItems.ToList();
                            foreach (WrapperItem w in wList)
                            {
                                int Qty = Convert.ToInt32(w.ChildQty) * Convert.ToInt32(detail.Qty);
                                Product wpObj = (from p in entity.Products where p.Id == w.ChildProductId select p).FirstOrDefault();

                                List<APP_Data.PurchaseDetail> pulist = Utility.InventoryByControlMethod(wpObj.Id, entity);

                                if (pulist.Count() > 0)
                                {
                                    int TotalQty = Convert.ToInt32(pulist.Sum(x => x.CurrentQy));

                                    if (TotalQty >= Qty)
                                    {
                                        foreach (PurchaseDetail p in pulist)
                                        {
                                            if (Qty > 0)
                                            {
                                                if (p.CurrentQy >= Qty)
                                                {
                                                    PurchaseDetailInTransaction pdObjInTran = new PurchaseDetailInTransaction();
                                                    pdObjInTran.ProductId = w.ChildProductId;
                                                    pdObjInTran.TransactionDetailId = detail.Id;
                                                    pdObjInTran.PurchaseDetailId = p.Id;
                                                    pdObjInTran.Date = detail.Transaction.DateTime;
                                                    pdObjInTran.IsSpecialChild = true;
                                                    pdObjInTran.Qty = Qty;
                                                    p.CurrentQy = p.CurrentQy - Qty;
                                                    Qty = 0;

                                                    entity.PurchaseDetailInTransactions.Add(pdObjInTran);
                                                    entity.Entry(p).State = EntityState.Modified;
                                                    entity.SaveChanges();
                                                    break;
                                                }
                                                else if (p.CurrentQy <= Qty)
                                                {
                                                    PurchaseDetailInTransaction pdObjInTran = new PurchaseDetailInTransaction();
                                                    pdObjInTran.ProductId = w.ChildProductId;
                                                    pdObjInTran.TransactionDetailId = detail.Id;
                                                    pdObjInTran.PurchaseDetailId = p.Id;
                                                    pdObjInTran.Date = detail.Transaction.DateTime;
                                                    pdObjInTran.IsSpecialChild = true;
                                                    pdObjInTran.Qty = p.CurrentQy;

                                                    Qty = Convert.ToInt32(Qty - p.CurrentQy);
                                                    p.CurrentQy = 0;
                                                    entity.PurchaseDetailInTransactions.Add(pdObjInTran);
                                                    entity.Entry(p).State = EntityState.Modified;
                                                    entity.SaveChanges();

                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            int Qty = Convert.ToInt32(detail.Qty);

                            List<PurchaseDetail> pulist = Utility.InventoryByControlMethod(pId, entity);


                            if (pulist.Count > 0) //pulist.Count() > 0
                            {
                                int TotalQty = Convert.ToInt32(pulist.Sum(x => x.CurrentQy));

                                if (TotalQty >= Qty)
                                {
                                    foreach (PurchaseDetail p in pulist)
                                    {
                                        if (Qty > 0)
                                        {
                                            if (p.CurrentQy >= Qty)
                                            {
                                                PurchaseDetailInTransaction pdObjInTran = new PurchaseDetailInTransaction();
                                                pdObjInTran.ProductId = pId;
                                                pdObjInTran.TransactionDetailId = detail.Id;
                                                pdObjInTran.PurchaseDetailId = p.Id;
                                                pdObjInTran.Date = detail.Transaction.DateTime;
                                                pdObjInTran.IsSpecialChild = false;
                                                pdObjInTran.Qty = Qty;
                                                p.CurrentQy = p.CurrentQy - Qty;
                                                Qty = 0;

                                                entity.PurchaseDetailInTransactions.Add(pdObjInTran);
                                                entity.Entry(p).State = EntityState.Modified;
                                                entity.SaveChanges();
                                                break;
                                            }
                                            else if (p.CurrentQy <= Qty)
                                            {
                                                PurchaseDetailInTransaction pdObjInTran = new PurchaseDetailInTransaction();
                                                pdObjInTran.ProductId = pId;
                                                pdObjInTran.TransactionDetailId = detail.Id;
                                                pdObjInTran.PurchaseDetailId = p.Id;
                                                pdObjInTran.Date = detail.Transaction.DateTime;
                                                pdObjInTran.IsSpecialChild = false;
                                                pdObjInTran.Qty = p.CurrentQy;

                                                Qty = Convert.ToInt32(Qty - p.CurrentQy);
                                                p.CurrentQy = 0;
                                                entity.PurchaseDetailInTransactions.Add(pdObjInTran);
                                                entity.Entry(p).State = EntityState.Modified;
                                                entity.SaveChanges();

                                            }//end of if p.currency<=qty
                                        }//end of if qty>0
                                    }//end of foreach loop 
                                }//end of total qty >0
                            }//end of purchase list>0

                        }//end of else             
                    }//end of foreach transaction detail
                    #endregion

                    //Print Invoice
                    #region [ Print ]
                    if (IsPrint)
                    {

                        dsReportTemp dsReport = new dsReportTemp();
                        dsReportTemp.ItemListDataTable dtReport = (dsReportTemp.ItemListDataTable)dsReport.Tables["ItemList"];
                        int _tAmt = 0;
                        foreach (TransactionDetail transaction in DetailList)
                        {
                            dsReportTemp.ItemListRow newRow = dtReport.NewItemListRow();
                            newRow.ItemId = transaction.Product.ProductCode;
                            newRow.Name = transaction.Product.Name;
                            newRow.Qty = transaction.Qty.ToString();
                            newRow.PhotoPath = string.IsNullOrEmpty(transaction.Product.PhotoPath) ? "" : Application.StartupPath + transaction.Product.PhotoPath.Remove(0, 1);
                            newRow.DiscountPercent = transaction.DiscountRate.ToString();
                            newRow.TotalAmount = (int)transaction.UnitPrice * (int)transaction.Qty;
                            newRow.Frequency = Convert.ToString(transaction.Product.PackageQty * transaction.Qty);
                            if (transaction.IsFOC == true) newRow.IsFOC = "FOC";
                            switch (Utility.GetDefaultPrinter())
                            {
                                case "A4 Printer":
                                    newRow.UnitPrice = transaction.UnitPrice.ToString();
                                    break;
                                case "Slip Printer":
                                    newRow.UnitPrice = "1@" + transaction.UnitPrice.ToString();
                                    break;
                            }

                            _tAmt += newRow.TotalAmount;
                            dtReport.AddItemListRow(newRow);
                        }//end of for each.

                        string reportPath = "";
                        ReportViewer rv = new ReportViewer();
                        ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["ItemList"]);

                        reportPath = Application.StartupPath + Utility.GetReportPath("Cash");


                        rv.Reset();
                        rv.LocalReport.ReportPath = reportPath;
                        rv.LocalReport.DataSources.Add(rds);

                        Utility.Slip_Log(rv);

                        Utility.Slip_A4_Footer(rv);
                        APP_Data.Customer customer = entity.Customers.Where(x => x.Id == CustomerId).FirstOrDefault();

                        ReportParameter CustomerName = new ReportParameter("CustomerName", customer.Name);
                        rv.LocalReport.SetParameters(CustomerName);
                        if (Utility.GetDefaultPrinter() == "A4 Printer")
                        {
                            ReportParameter CusPhoneNumber = new ReportParameter("CusPhoneNumber", customer.PhoneNumber);
                            rv.LocalReport.SetParameters(CusPhoneNumber);
                        }
                       
                        if (insertedTransaction.Note == "")
                        {
                            ReportParameter Notes = new ReportParameter("Notes", " ");
                            rv.LocalReport.SetParameters(Notes);
                        }
                        else
                        {
                            ReportParameter Notes = new ReportParameter("Notes", insertedTransaction.Note);
                            rv.LocalReport.SetParameters(Notes);
                        }

                        if (Convert.ToInt32(insertedTransaction.MCDiscountAmt) != 0)
                        {
                            Int64 _mcDiscountAmt = Convert.ToInt64(insertedTransaction.MCDiscountAmt);
                            ReportParameter MCDiscountAmt = new ReportParameter("MCDiscount", "-" + _mcDiscountAmt.ToString());
                            rv.LocalReport.SetParameters(MCDiscountAmt);
                        }

                        else if (Convert.ToInt32(insertedTransaction.BDDiscountAmt) != 0)
                        {
                            Int64 _bcDiscountAmt = Convert.ToInt64(insertedTransaction.BDDiscountAmt);
                            ReportParameter BCDiscountAmt = new ReportParameter("MCDiscount", "-" + _bcDiscountAmt.ToString());
                            rv.LocalReport.SetParameters(BCDiscountAmt);
                        }
                        else
                        {
                            ReportParameter BCDiscountAmt = new ReportParameter("MCDiscount", "0");
                            rv.LocalReport.SetParameters(BCDiscountAmt);
                        }
                        if (SettingController.SelectDefaultPrinter != "Slip Printer")
                        {
                            ReportParameter PrintProductImage = new ReportParameter("PrintImage", SettingController.ShowProductImageIn_A4Reports.ToString());
                            rv.LocalReport.SetParameters(PrintProductImage);
                        }
                        ReportParameter TAmt = new ReportParameter("TAmt", _tAmt.ToString());
                        rv.LocalReport.SetParameters(TAmt);

                        ReportParameter ShopName = new ReportParameter("ShopName", SettingController.ShopName);
                        rv.LocalReport.SetParameters(ShopName);

                        ReportParameter BranchName = new ReportParameter("BranchName", SettingController.BranchName);
                        rv.LocalReport.SetParameters(BranchName);

                        ReportParameter Phone = new ReportParameter("Phone", SettingController.PhoneNo);
                        rv.LocalReport.SetParameters(Phone);

                        ReportParameter OpeningHours = new ReportParameter("OpeningHours", SettingController.OpeningHours);
                        rv.LocalReport.SetParameters(OpeningHours);

                        ReportParameter TransactionId = new ReportParameter("TransactionId", resultId.ToString());
                        rv.LocalReport.SetParameters(TransactionId);

                        APP_Data.Counter c = entity.Counters.Where(x => x.Id == MemberShip.CounterId).FirstOrDefault();

                        ReportParameter CounterName = new ReportParameter("CounterName", c.Name);
                        rv.LocalReport.SetParameters(CounterName);
                        
                        ReportParameter InvoiceDate = new ReportParameter("InvoiceDate",((DateTime)insertedTransaction.DateTime).ToString("dd-MMM-yyyy"));
                        rv.LocalReport.SetParameters(InvoiceDate);

                        ReportParameter CasherName = new ReportParameter("CasherName", MemberShip.UserName);
                        rv.LocalReport.SetParameters(CasherName);

                        Int64 totalAmountRep = insertedTransaction.TotalAmount == null ? 0 : Convert.ToInt64(insertedTransaction.TotalAmount);
                        ReportParameter TotalAmount = new ReportParameter("TotalAmount", totalAmountRep.ToString());
                        rv.LocalReport.SetParameters(TotalAmount);

                        Int64 taxAmountRep = insertedTransaction.TaxAmount == null ? 0 : Convert.ToInt64(insertedTransaction.TaxAmount);
                        ReportParameter TaxAmount = new ReportParameter("TaxAmount", taxAmountRep.ToString());
                        rv.LocalReport.SetParameters(TaxAmount);

                        ReportParameter CurrencyCode = new ReportParameter("CurrencyCode", CurrencySymbol);
                        rv.LocalReport.SetParameters(CurrencyCode);

                        Int64 disAmountRep = insertedTransaction.DiscountAmount == null ? 0 : Convert.ToInt64(insertedTransaction.DiscountAmount);

                        if (disAmountRep == 0)
                        {
                            ReportParameter DiscountAmount = new ReportParameter("DiscountAmount", disAmountRep.ToString());
                            rv.LocalReport.SetParameters(DiscountAmount);
                        }
                        else
                        {
                            ReportParameter DiscountAmount = new ReportParameter("DiscountAmount", "-" + (disAmountRep - ExtraDiscount).ToString());
                            rv.LocalReport.SetParameters(DiscountAmount);
                        }
                        if (ExtraDiscount != 0)
                        {
                            ReportParameter AdditionalDiscount = new ReportParameter("AdditionalDiscount", "-" + ExtraDiscount.ToString());
                            rv.LocalReport.SetParameters(AdditionalDiscount);
                        }

                        ReportParameter PaidAmount = new ReportParameter("PaidAmount", txtReceiveAmount.Text);
                        rv.LocalReport.SetParameters(PaidAmount);

                        ReportParameter Change = new ReportParameter("Change", lblChanges.Text);
                        rv.LocalReport.SetParameters(Change);


                        if (SettingController.UseTable || SettingController.UseQueue)
                        {

                            bool IsRestaurant = SettingController.UseTable ? SettingController.UseTable : SettingController.UseQueue ? SettingController.UseQueue : false;
                            ReportParameter restaurant = new ReportParameter("IsRestaurant", IsRestaurant.ToString());
                            rv.LocalReport.SetParameters(restaurant);

                            ReportParameter tblORque = new ReportParameter("TBLorQue", SettingController.UseTable && insertedTransaction.TableIdOrQue != null ? "# : " + entity.RestaurantTables.Find(insertedTransaction.TableIdOrQue).Number
                                                                           : SettingController.UseQueue && insertedTransaction.TableIdOrQue != null ? "# : " + insertedTransaction.TableIdOrQue.ToString().PadLeft(4, '0') : null);
                            rv.LocalReport.SetParameters(tblORque);
                            ReportParameter servicepercent = new ReportParameter("ServicePercent", insertedTransaction.ServiceFee.ToString());
                            rv.LocalReport.SetParameters(servicepercent);
                            ReportParameter servicecharge = new ReportParameter("ServiceFee", ((insertedTransaction.TransactionDetails.Sum(a => a.TotalAmount)) * insertedTransaction.ServiceFee / 100).ToString());
                            rv.LocalReport.SetParameters(servicecharge);
                        }



                        if (Utility.GetDefaultPrinter() == "A4 Printer")
                        {
                            ReportParameter CusAddress = new ReportParameter("CusAddress", customer.Address);
                            rv.LocalReport.SetParameters(CusAddress);

                            //add a parameter for email
                            ReportParameter Email = new ReportParameter("Email", SettingController.Email);
                            rv.LocalReport.SetParameters(Email);

                            //add a parameter for website
                            ReportParameter Website = new ReportParameter("Website", SettingController.Website);
                            rv.LocalReport.SetParameters(Website);

                            ReportParameter PhoneImagePath = new ReportParameter("PhoneImagePath", "file:\\" + Application.StartupPath + "\\Images\\phone.png");
                            rv.LocalReport.SetParameters(PhoneImagePath);

                            ReportParameter WebsiteImagePath = new ReportParameter("WebsiteImagePath", "file:\\" + Application.StartupPath + "\\Images\\website.png");
                            rv.LocalReport.SetParameters(WebsiteImagePath);

                            ReportParameter EmailImagePath = new ReportParameter("EmailImagePath", "file:\\" + Application.StartupPath + "\\Images\\mail.png");
                            rv.LocalReport.SetParameters(EmailImagePath);

                            ReportParameter LocationImagePath = new ReportParameter("LocationImagePath", "file:\\" + Application.StartupPath + "\\Images\\location.png");
                            rv.LocalReport.SetParameters(LocationImagePath);
                        }
                        ReportParameter TotalGiftDiscount = new ReportParameter("TotalGiftDiscount", GiftDiscountAmt.ToString());
                        rv.LocalReport.SetParameters(TotalGiftDiscount);
                        Utility.Get_Print(rv);

                        #endregion
                    }
                    //generate ticket
                    if (SettingController.TicketSale)
                    {
                        foreach (var item in tdlist)
                        {
                            for (int i = 0; i < item.Qty; i++)
                            {
                                System.Data.Objects.ObjectResult<int?> result = entity.InsertTicket(item.Tdid, false, DateTime.Now, item.Category, SettingController.DefaultShop.ShortCode);
                            }
                            System.Data.Objects.ObjectResult<GetTicketBy_TDID_Result> getresult = entity.GetTicketBy_TDID(item.Tdid);

                            var ticketList = getresult.Select(n => new Ticket { TicketNo = n.TicketNo, CreatedDate = n.CreatedDate }).ToList();
                            foreach (Ticket t in ticketList)
                            {
                                shopname = SettingController.DefaultShop.ShopName;
                                dsReportTemp dsReport = new dsReportTemp();
                                dsReportTemp.TicketReportDataTable dtReport = (dsReportTemp.TicketReportDataTable)dsReport.Tables["TicketReport"];
                                dsReportTemp.TicketReportRow newRow = dtReport.NewTicketReportRow();
                                newRow.DateTime = t.CreatedDate.Date.ToString("dd-MM-yyyy");
                                newRow.EventName = shopname;

                                QRCoder.QRCodeGenerator qrgenerator = new QRCoder.QRCodeGenerator();
                                QRCoder.QRCodeData qrdata = qrgenerator.CreateQrCode(t.TicketNo, QRCoder.QRCodeGenerator.ECCLevel.Q);
                                QRCoder.QRCode qrcode = new QRCoder.QRCode(qrdata);
                                Bitmap qrImage = qrcode.GetGraphic(20);
                                ImageConverter converter = new ImageConverter();

                                newRow.QRCode = (byte[])converter.ConvertTo(qrImage, typeof(byte[]));
                                dtReport.AddTicketReportRow(newRow);
                                string reportPath = "";
                                ReportViewer rv = new ReportViewer();
                                ReportDataSource rds = new ReportDataSource("Ticket", dsReport.Tables["TicketReport"]);
                                reportPath = Application.StartupPath + "\\Reports\\Ticket.rdlc";
                                rv.Reset();
                                rv.LocalReport.ReportPath = reportPath;
                                rv.LocalReport.DataSources.Add(rds);
                                ReportParameter TranNo = new ReportParameter("TranNo", entity.Tickets.Where(a => a.TicketNo == t.TicketNo).FirstOrDefault().TransactionDetail.Transaction.Id);
                                rv.LocalReport.SetParameters(TranNo);
                                ReportParameter ShopName = new ReportParameter("ShopName", shopname);
                                rv.LocalReport.SetParameters(ShopName);
                                Utility.Get_Print(rv);
                            }
                        }
                    }

                    _result = MessageShow();
                    if (_result == DialogResult.OK)
                    {
                        if (System.Windows.Forms.Application.OpenForms["chart"] != null)
                        {
                            chart newForm = (chart)System.Windows.Forms.Application.OpenForms["chart"];
                            newForm.FormFresh();
                        }
                    }
                }
                else
                {
                    if (lblChangesText.Text == "Changes")
                    {
                        receiveAmount -= Convert.ToInt64(lblChanges.Text);
                    }
                    long totalAmount = receiveAmount + prePaidAmount;
                    long totalCredit = 0;
                    Int64.TryParse(lblTotalCost.Text, out totalCredit);
                    long DebtAmount = 0;
                    if (totalAmount != 0)
                    {
                        if (CreditTransaction.Count > 0)
                        {
                            int index = CreditTransaction.Count;
                            for (int outer = index - 1; outer >= 1; outer--)
                            {
                                for (int inner = 0; inner < outer; inner++)
                                {
                                    if (CreditTransaction[inner].TotalAmount - CreditTransaction[inner].RecieveAmount < CreditTransaction[inner + 1].TotalAmount - CreditTransaction[inner + 1].RecieveAmount)
                                    {
                                        Transaction t = CreditTransaction[inner];
                                        CreditTransaction[inner] = CreditTransaction[inner + 1];
                                        CreditTransaction[inner + 1] = t;
                                    }
                                }
                            }
                            foreach (Transaction CT in CreditTransaction)
                            {
                                long CreditAmount = 0;
                                CreditAmount = (long)CT.TotalAmount - (long)CT.RecieveAmount;
                                RefundList = (from tr in entity.Transactions where tr.ParentId == CT.Id && tr.Type == TransactionType.CreditRefund select tr).ToList();
                                if (RefundList.Count > 0)
                                {
                                    foreach (Transaction TRefund in RefundList)
                                    {
                                        CreditAmount -= (long)TRefund.RecieveAmount;
                                    }
                                }
                                if (CT.UsePrePaidDebts != null)
                                {
                                    long prePaid = (long)CT.UsePrePaidDebts.Sum(x => x.UseAmount);
                                    CreditAmount -= prePaid;
                                }
                                if (CreditAmount <= totalAmount)
                                {
                                    CreditT = (from t in entity.Transactions where t.Id == CT.Id select t).FirstOrDefault<Transaction>();
                                    CreditT.IsPaid = true;

                                    TranIdList.Add(CreditT.Id);
                                    entity.Entry(CreditT).State = EntityState.Modified;
                                    entity.SaveChanges();
                                    totalAmount -= CreditAmount;
                                    if (CreditAmount <= receiveAmount)
                                    {
                                        DebtAmount += CreditAmount;
                                        receiveAmount -= CreditAmount;
                                    }
                                    else
                                    {
                                        CreditAmount -= receiveAmount;
                                        DebtAmount += receiveAmount;
                                        receiveAmount = 0;
                                        foreach (Transaction PrePaidDebtTrans in PrePaidTransaction)
                                        {
                                            long PrePaidamount = 0;
                                            //int useAmount = 0;
                                            int useAmount = (PrePaidDebtTrans.UsePrePaidDebts1 == null) ? 0 : (int)PrePaidDebtTrans.UsePrePaidDebts1.Sum(x => x.UseAmount);
                                            PrePaidamount = (long)PrePaidDebtTrans.TotalAmount - useAmount;
                                            if (CreditAmount >= PrePaidamount)
                                            {
                                                Transaction PD = (from PT in entity.Transactions where PT.Id == PrePaidDebtTrans.Id select PT).FirstOrDefault<Transaction>();
                                                PD.IsActive = true;
                                                entity.Entry(PD).State = EntityState.Modified;
                                                UsePrePaidDebt usePrePaidDObj = new UsePrePaidDebt();
                                                usePrePaidDObj.UseAmount = (int)PrePaidamount;
                                                usePrePaidDObj.PrePaidDebtTransactionId = PrePaidDebtTrans.Id;
                                                usePrePaidDObj.CreditTransactionId = CT.Id;
                                                usePrePaidDObj.CashierId = MemberShip.UserId;
                                                usePrePaidDObj.CounterId = MemberShip.CounterId;
                                                entity.UsePrePaidDebts.Add(usePrePaidDObj);
                                                entity.SaveChanges();
                                                CreditAmount -= PrePaidamount;
                                            }
                                            else
                                            {
                                                UsePrePaidDebt usePrePaidDObj = new UsePrePaidDebt();
                                                usePrePaidDObj.UseAmount = (int)CreditAmount;
                                                usePrePaidDObj.PrePaidDebtTransactionId = PrePaidDebtTrans.Id;
                                                usePrePaidDObj.CreditTransactionId = CT.Id;
                                                usePrePaidDObj.CashierId = MemberShip.UserId;
                                                usePrePaidDObj.CounterId = MemberShip.CounterId;
                                                entity.UsePrePaidDebts.Add(usePrePaidDObj);
                                                entity.SaveChanges();
                                                CreditAmount -= PrePaidamount;
                                            }
                                        }

                                        PrePaidTransaction = (from PDT in entity.Transactions where PDT.Type == TransactionType.Prepaid && PDT.IsActive == false select PDT).ToList();
                                    }
                                }
                            }
                            if (DebtAmount > 0)
                            {
                                string joinedTranIdList = string.Join(",", TranIdList);
                                System.Data.Objects.ObjectResult<string> DebtId = entity.InsertTransaction(DateTime.Now, MemberShip.UserId, MemberShip.CounterId, TransactionType.Settlement, true, true, 1, 0, 0, DebtAmount, DebtAmount, null, CustomerId, MCDiscount, BDDiscount, MemberTypeId, MCDiscountPercent, true, joinedTranIdList, IsWholeSale, 0, SettingController.DefaultShop.Id, SettingController.DefaultShop.ShortCode, Note, tableId, servicefee, null);
                                string _Debt = DebtId.FirstOrDefault().ToString();

                                foreach (var t in TranIdList)
                                {
                                    var result = (from tr in entity.Transactions where tr.Id == t select tr).FirstOrDefault();
                                    result.TranVouNos = _Debt;
                                    result.IsSettlement = true;
                                    entity.Entry(result).State = EntityState.Modified;
                                    entity.SaveChanges();
                                }
                                entity = new POSEntities();
                                resultId = _Debt;
                                insertedTransaction = (from trans in entity.Transactions where trans.Id == resultId select trans).FirstOrDefault<Transaction>();
                                insertedTransaction.ReceivedCurrencyId = cu.Id;
                                ExchangeRateForTransaction ex = new ExchangeRateForTransaction();
                                ex.TransactionId = resultId;
                                ex.CurrencyId = cu.Id;
                                ex.ExchangeRate = Convert.ToInt32(cu.LatestExchangeRate);
                                entity.ExchangeRateForTransactions.Add(ex);
                                entity.SaveChanges();
                                entity.SaveChanges();
                            }
                        }
                        else
                        {
                            totalAmount -= prePaidAmount;
                            receiveAmount -= prePaidAmount;
                        }
                    }
                    if (receiveAmount > 0)
                    {
                        System.Data.Objects.ObjectResult<string> PreDebtId = entity.InsertTransaction(DateTime.Now, MemberShip.UserId, MemberShip.CounterId, TransactionType.Prepaid, true, false, 1, 0, 0, receiveAmount, receiveAmount, null, CustomerId, MCDiscount, BDDiscount, MemberTypeId, MCDiscountPercent, false, "", IsWholeSale, 0, SettingController.DefaultShop.Id, SettingController.DefaultShop.ShortCode, Note, tableId, servicefee, transactionParentId);
                        entity.SaveChanges();
                        if (DebtAmount == 0)
                        {
                            entity = new POSEntities();
                            resultId = PreDebtId.FirstOrDefault().ToString();
                            insertedTransaction = (from trans in entity.Transactions where trans.Id == resultId select trans).FirstOrDefault<Transaction>();
                            insertedTransaction.ReceivedCurrencyId = cu.Id;
                            ExchangeRateForTransaction ex = new ExchangeRateForTransaction();
                            ex.TransactionId = resultId;
                            ex.CurrencyId = cu.Id;
                            ex.ExchangeRate = Convert.ToInt32(cu.LatestExchangeRate);
                            entity.ExchangeRateForTransactions.Add(ex);
                            entity.SaveChanges();
                        }

                    }
                    if (isDraft)
                    {
                        Transaction draft = (from trans in entity.Transactions where trans.Id == DraftId select trans).FirstOrDefault<Transaction>();
                        if (draft != null)
                        {
                            draft.TransactionDetails.Clear();
                            var Detail = entity.TransactionDetails.Where(d => d.TransactionId == draft.Id);
                            foreach (var d in Detail)
                            {
                                entity.TransactionDetails.Remove(d);
                            }
                            entity.Transactions.Remove(draft);
                            entity.SaveChanges();
                        }
                    }


                    //Print Invoice
                    #region [ Print ]
                    if (IsPrint)
                    {
                        ReportViewer rv = new ReportViewer();
                        string reportPath = Application.StartupPath + Utility.GetReportPath("Settlement");
                        rv.Reset();
                        rv.LocalReport.ReportPath = reportPath;
                        Utility.Slip_Log(rv);
                        Utility.Slip_A4_Footer(rv);
                        APP_Data.Customer cus = entity.Customers.Where(x => x.Id == CustomerId).FirstOrDefault();

                        ReportParameter CustomerName = new ReportParameter("CustomerName", cus.Name);
                        rv.LocalReport.SetParameters(CustomerName);

                        if (insertedTransaction.Note == "")
                        {
                            ReportParameter Notes = new ReportParameter("Notes", " ");
                            rv.LocalReport.SetParameters(Notes);
                        }
                        else
                        {
                            ReportParameter Notes = new ReportParameter("Notes", insertedTransaction.Note);
                            rv.LocalReport.SetParameters(Notes);
                        }

                        ReportParameter ShopName = new ReportParameter("ShopName", SettingController.ShopName);
                        rv.LocalReport.SetParameters(ShopName);

                        ReportParameter BranchName = new ReportParameter("BranchName", SettingController.BranchName);
                        rv.LocalReport.SetParameters(BranchName);

                        ReportParameter Phone = new ReportParameter("Phone", SettingController.PhoneNo);
                        rv.LocalReport.SetParameters(Phone);

                        ReportParameter OpeningHours = new ReportParameter("OpeningHours", SettingController.OpeningHours);
                        rv.LocalReport.SetParameters(OpeningHours);

                        ReportParameter TransactionId = new ReportParameter("TransactionId", resultId.ToString());
                        rv.LocalReport.SetParameters(TransactionId);

                        APP_Data.Counter c = entity.Counters.Where(x => x.Id == MemberShip.CounterId).FirstOrDefault();

                        ReportParameter CounterName = new ReportParameter("CounterName", c.Name);
                        rv.LocalReport.SetParameters(CounterName);

                        ReportParameter InvoiceDate = new ReportParameter("InvoiceDate", Convert.ToDateTime(insertedTransaction.DateTime).ToString("dd-MMM-yyyy"));
                        rv.LocalReport.SetParameters(InvoiceDate);

                        ReportParameter CasherName = new ReportParameter("CasherName", MemberShip.UserName);
                        rv.LocalReport.SetParameters(CasherName);


                        ReportParameter TotalAmount = new ReportParameter("TotalAmount", lblTotalCost.Text.ToString());
                        rv.LocalReport.SetParameters(TotalAmount);



                        ReportParameter PaidAmount = new ReportParameter("PaidAmount", txtReceiveAmount.Text);
                        rv.LocalReport.SetParameters(PaidAmount);

                        int balance = Convert.ToInt32(lblTotalCost.Text) - Convert.ToInt32(txtReceiveAmount.Text);
                        balance = balance < 0 ? 0 : balance;
                        ReportParameter Balance = new ReportParameter("Balance", balance.ToString());
                        rv.LocalReport.SetParameters(Balance);

                        int _change = Convert.ToInt32(txtReceiveAmount.Text) - Convert.ToInt32(lblTotalCost.Text);

                        _change = _change < 0 ? 0 : _change;
                        ReportParameter Change = new ReportParameter("Change", _change.ToString());
                        rv.LocalReport.SetParameters(Change);

                        if (Utility.GetDefaultPrinter() == "A4 Printer")
                        {
                            ReportParameter CusAddress = new ReportParameter("CusAddress", cus.Address);
                            rv.LocalReport.SetParameters(CusAddress);

                            //add a parameter for email
                            ReportParameter Email = new ReportParameter("Email", SettingController.Email);
                            rv.LocalReport.SetParameters(Email);

                            //add a parameter for website
                            ReportParameter Website = new ReportParameter("Website", SettingController.Website);
                            rv.LocalReport.SetParameters(Website);

                            ReportParameter PhoneImagePath = new ReportParameter("PhoneImagePath", "file:\\" + Application.StartupPath + "\\Images\\phone.png");
                            rv.LocalReport.SetParameters(PhoneImagePath);

                            ReportParameter WebsiteImagePath = new ReportParameter("WebsiteImagePath", "file:\\" + Application.StartupPath + "\\Images\\website.png");
                            rv.LocalReport.SetParameters(WebsiteImagePath);

                            ReportParameter EmailImagePath = new ReportParameter("EmailImagePath", "file:\\" + Application.StartupPath + "\\Images\\mail.png");
                            rv.LocalReport.SetParameters(EmailImagePath);

                        }

                        Utility.Get_Print(rv);
                        #endregion
                    }
                    _result = MessageShow();
                }

                if (!isDebt)
                {
                    if (System.Windows.Forms.Application.OpenForms["Sales"] != null)
                    {
                        Sales newForm = (Sales)System.Windows.Forms.Application.OpenForms["Sales"];
                        newForm.note = "";
                        newForm.Clear();
                    }
                }
                else
                {
                    if (System.Windows.Forms.Application.OpenForms["CustomerDetail"] != null)
                    {
                        CustomerDetail newForm = (CustomerDetail)System.Windows.Forms.Application.OpenForms["CustomerDetail"];
                        newForm.Reload();
                    }
                }
                Note = "";
                this.Dispose();
            }
            if (_result.Equals(DialogResult.OK))
            {
                Common cm = new Common();
                cm.MemberTypeId = MemberTypeId;
                cm.TotalAmt = TotalAmt;
                cm.CustomerId = CustomerId;
                cm.type = 'S';
                cm.TransactionId = resultId;
                cm.Get_MType();
                if (System.Windows.Forms.Application.OpenForms["Sales"] != null)
                {
                    Sales newForm = (Sales)System.Windows.Forms.Application.OpenForms["Sales"];

                    newForm.Clear();
                }
            }
        }

        private DialogResult MessageShow()
        {
            DialogResult result = MessageBox.Show(this, "Payment Completed", "mPOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return result;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void PaidByCash_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(txtReceiveAmount);
            tp.Hide(cboCurrency);
        }

        private void txtReceiveAmount_KeyUp(object sender, KeyEventArgs e)
        {
            int amount = 0;
            Int32.TryParse(txtReceiveAmount.Text, out amount);
            int Cost = 0;
            Int32.TryParse(lblTotalCost.Text, out Cost);
            if (!isDebt)
            {
                lblChanges.Text = (amount - (DetailList.Sum(x => x.TotalAmount) - ExtraDiscount - MCDiscount - BDDiscount)).ToString();
            }
            else
            {
                string currVal = cboCurrency.Text;
                int cId = (from c in entity.Currencies where c.CurrencyCode == currVal select c.Id).SingleOrDefault();
                Currency currencyObj = entity.Currencies.FirstOrDefault(x => x.Id == cId);
                if (amount >= DebtAmount)
                {
                    lblChanges.Text = (amount - DebtAmount).ToString();
                    lblChangesText.Text = "Changes";
                }
                else
                {
                    lblChangesText.Text = "Net Payable";
                    lblChanges.Text = (DebtAmount - amount).ToString();
                }
            }
        }

        private void txtReceiveAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtReceiveAmount_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && char.IsLetter('.'))
            {
                e.Handled = true;
            }
        }

        private void txtReceiveAmount_KeyUp_1(object sender, KeyEventArgs e)
        {
            decimal amount = 0;
            decimal.TryParse(txtReceiveAmount.Text, out amount);
            decimal Cost = 0;
            decimal.TryParse(lblTotalCost.Text, out Cost);

            if (txtReceiveAmount.Text != string.Empty)
            {
                if (!isDebt)
                {
                    lblChanges.Text = (amount - Cost).ToString();
                }
                else
                {
                    decimal DAmount = Convert.ToDecimal(lblTotalCost.Text);
                    string currVal = cboCurrency.Text;
                    int cId = (from c in entity.Currencies where c.CurrencyCode == currVal select c.Id).SingleOrDefault();
                    Currency currencyObj = entity.Currencies.FirstOrDefault(x => x.Id == cId);
                    if (amount >= DAmount)
                    {
                        lblChanges.Text = (amount - DAmount).ToString();
                        lblChangesText.Text = "Changes";
                    }
                    else
                    {
                        lblChangesText.Text = "Net Payable";
                        lblChanges.Text = (DAmount - amount).ToString();
                    }
                }
            }

        }

        private void cboCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            int currencyId = 0;
            string currVal = cboCurrency.Text;
            currencyId = (from c in entity.Currencies where c.CurrencyCode == currVal select c.Id).SingleOrDefault();
            if (currencyId != 0)
            {
                Currency cu = entity.Currencies.FirstOrDefault(x => x.Id == currencyId);
                if (cu != null)
                {
                    if (!isDebt)
                    {

                        lblTotalCost.Text = Utility.CalculateExchangeRate(cu.Id, total).ToString();
                        AmountWithExchange = Convert.ToDecimal(lblTotalCost.Text);
                        decimal receive = 0;

                        Decimal.TryParse(txtReceiveAmount.Text, out receive);
                        decimal changes = AmountWithExchange - receive;

                        lblChanges.Text = changes.ToString();
                    }
                    else
                    {

                        lblTotalCost.Text = Utility.CalculateExchangeRate(cu.Id, DebtAmount).ToString();
                        AmountWithExchange = Convert.ToDecimal(lblTotalCost.Text);
                        decimal receive = 0;

                        Decimal.TryParse(txtReceiveAmount.Text, out receive);
                        decimal changes = AmountWithExchange - receive;
                        lblChanges.Text = changes.ToString();
                    }

                }
            }
        }

        #region Function
        #region for saving Sale Qty in Stock Transaction table
        private void Save_SaleQty_ToStockTransaction(List<Stock_Transaction> productList)
        {
            int _year, _month;

            _year = System.DateTime.Now.Year;
            _month = System.DateTime.Now.Month;
            Utility.Sale_Run_Process(_year, _month, productList);
        }
        #endregion
        #endregion
    }
}
