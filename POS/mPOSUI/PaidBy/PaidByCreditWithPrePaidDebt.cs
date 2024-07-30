using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using POS.APP_Data;

namespace POS
{
    public partial class PaidByCreditWithPrePaidDebt : Form
    {
        #region Variables

        public List<TransactionDetail> DetailList = new List<TransactionDetail>();

        public string DId { get; set; }


        public int Discount { get; set; }

        public int Tax { get; set; }

        public int ExtraDiscount { get; set; }

        public int ExtraTax { get; set; }

        public Boolean isDraft { get; set; }

        public string DraftId { get; set; }

        public Boolean IsWholeSale { get; set; }

        public int? CustomerId { get; set; }

        private POSEntities entity = new POSEntities();

        private ToolTip tp = new ToolTip();

        //private long outstandingBalance = 0;
        long OldOutstandingAmount = 0;
        int PrepaidDebt = 0;

        public decimal BDDiscount { get; set; }

        public decimal MCDiscount { get; set; }

        public int? MemberTypeId { get; set; }
        public List<string> TranIdList = new List<string>();
        Transaction CreditT = new Transaction();

        public decimal? MCDiscountPercent { get; set; }

        public DialogResult _result;

        public decimal TotalAmt = 0;

        string resultId;

        int Qty = 0;

        List<Stock_Transaction> productList = new List<Stock_Transaction>();
        public string Note = "";
        public Boolean IsPrint = true;
        public int? tableId = null;
        public int servicefee = 0;
        public long GiftDiscountAmt { get; set; }
        public List<GiftSystem> GiftList = new List<GiftSystem>();
        public string insertedId = "";

        int paymentType;
        #endregion

        #region Event
        public PaidByCreditWithPrePaidDebt()
        {
            InitializeComponent();
        }
        #region Hot keys handler
        void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.N)      //  Ctrl + N => Focus CustomerName
            {
                cboCustomerList.DroppedDown = true;
                if (cboCustomerList.Focused != true)
                {
                    cboCustomerList.Focus();
                }
            }
            else if (e.Control && e.KeyCode == Keys.R)      // Ctrl + R => Focus Receive Amt
            {
                cboCustomerList.DroppedDown = false;
                txtReceiveAmount.Focus();
            }
            else if (e.Control && e.KeyCode == Keys.S)      // Ctrl + S => Click Save
            {
                cboCustomerList.DroppedDown = false;
                btnSubmit.PerformClick();
            }
            else if (e.Control && e.KeyCode == Keys.C)      // Ctrl + C => Focus DropDown Customer
            {
                cboCustomerList.DroppedDown = false;
                btnCancel.PerformClick();
            }
        }
        #endregion
        private void PaidByCreditWithPrePaidDebt_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            #region Setting Hot Kyes For the Controls
            SendKeys.Send("%"); SendKeys.Send("%"); // Clicking "Alt" on page load to show underline of Hot Keys
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form_KeyDown);
            #endregion

            List<Customer> CustomerList = new List<Customer>();
            Customer none = new Customer();
            none.Name = "Select Customer";
            none.Id = 0;
            CustomerList.Add(none);
            CustomerList.AddRange(entity.Customers.ToList());
            cboCustomerList.DataSource = CustomerList;
            cboCustomerList.DisplayMember = "Name";
            cboCustomerList.ValueMember = "Id";
            cboCustomerList.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboCustomerList.AutoCompleteSource = AutoCompleteSource.ListItems;

            BindPaymentMethodCombo();

            if (CustomerId != null)
            {
                cboCustomerList.Text = entity.Customers.Where(x => x.Id == CustomerId).FirstOrDefault().Name;
            }

            int previouseBalance = Convert.ToInt32(lblPreviousBalance.Text);

            lblAccuralCost.Text = (DetailList.Sum(x => x.TotalAmount) + (((long)DetailList.Sum(x => x.TotalAmount) * servicefee) / 100) - ExtraDiscount - MCDiscount - BDDiscount - GiftDiscountAmt).ToString();
           // IsPrePaidCheck();
            TotalAmt = Convert.ToDecimal(lblTotalCost.Text);
            txtReceiveAmount.Focus();
        }
        private void BindPaymentMethodCombo()
        {
            cboPaymentType.DataSource = entity.PaymentTypes.Where(x => x.Id == 1 || x.Id == 5).ToList();
            cboPaymentType.DisplayMember = "Name";
            cboPaymentType.ValueMember = "Id";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            
            Boolean hasError = false;

            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            //Validation
            if (cboCustomerList.SelectedIndex == 0)
            {
                tp.SetToolTip(cboCustomerList, "Error");
                tp.Show("Please select customer name!", cboCustomerList);
                hasError = true;
            }

            else if (txtReceiveAmount.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtReceiveAmount, "Error");
                tp.Show("Please fill up receive amount!", txtReceiveAmount);
                hasError = true;
            }

            else if (Convert.ToInt32(txtReceiveAmount.Text) > Convert.ToInt32(lblAccuralCost.Text))
            {
                MessageBox.Show("Receive Amount should be less than current cost. Please fill it again!");
                txtReceiveAmount.Focus();
                hasError = true;
            }
            else if(Convert.ToInt32(txtReceiveAmount.Text) == Convert.ToInt32(lblAccuralCost.Text))
            {
                DialogResult result = MessageBox.Show("If you want to pay fully amount to the current cost, Please change your transaction to Cash Type! ", "mPOS", MessageBoxButtons.OK);
                hasError = true;
                this.Close();
               
            }

            if (!hasError)
            {

                long totalAmount = 0; long totalCost = 0; long receiveAmount = 0; 
               // Int64.TryParse(txtReceiveAmount.Text.Trim(), out receiveAmount);
                //string temp = txtReceiveAmount.Text;
                 receiveAmount = Convert.ToInt64(txtReceiveAmount.Text);
               
                //Int64.TryParse(lblAccuralCost.Text, out totalCost);
                totalCost = (long)(DetailList.Sum(x => x.TotalAmount) + (((long)DetailList.Sum(x => x.TotalAmount) * servicefee) / 100) - ExtraDiscount - (long)MCDiscount - (long)BDDiscount - GiftDiscountAmt);

                long unitpriceTotalCost = (long)DetailList.Sum(x => x.UnitPrice * x.Qty);//Edit ZMH


               
                

               //totalAmount = receiveAmount + PrepaidDebt;



                if (lblNetPayableTitle.Text == "Change")
                {
                    totalAmount -= Convert.ToInt64(lblNetPayable.Text);
                    receiveAmount -= Convert.ToInt64(lblNetPayable.Text);
                }
                 
               
                int customerId = 0;
                Int32.TryParse(cboCustomerList.SelectedValue.ToString(), out customerId);

                //set old credit transaction record to paid coz this transaction store old outstanding amount
                Customer cust = (from c in entity.Customers where c.Id == customerId select c).FirstOrDefault<Customer>();
                List<Transaction> unpaidTransList = cust.Transactions.Where(t => t.IsPaid == false && t.Type != TransactionType.Prepaid).ToList();
                List<Transaction> prePaidTranList = cust.Transactions.Where(type => type.Type == TransactionType.Prepaid).Where(active => active.IsActive == false).ToList();
                
                //insert credit Transaction
                System.Data.Objects.ObjectResult<string> Id;
                Transaction insertedTransaction = new Transaction();

                if (cboPaymentType.SelectedIndex == 0)
                {
                    paymentType = Convert.ToInt32(cboPaymentType.SelectedValue);
                }
               else
                {
                    paymentType = Convert.ToInt32(cboBankTransfer.SelectedValue);
                }
                //Console.WriteLine("ReceiveTextBox" + txtReceiveAmount.Text.ToString());
                //Console.WriteLine("1" + totalCost.ToString());
                //Console.WriteLine("2" + receiveAmount.ToString());


                #region add sale, debt, prepaid transaction when receiveamount is greater than totalCost
                if (receiveAmount >= totalCost) // here totalAmt = receivemount
                {
                    
                   // Id = entity.InsertTransaction(DateTime.Now, MemberShip.UserId, MemberShip.CounterId, TransactionType.Sale, true, true, paymentType, ExtraTax + Tax, ExtraDiscount + Discount, DetailList.Sum(x => x.TotalAmount) + (DetailList.Sum(x => x.TotalAmount) * servicefee / 100) - ExtraDiscount, totalCost, null, customerId, MCDiscount, BDDiscount, MemberTypeId, MCDiscountPercent, false, "", IsWholeSale, GiftDiscountAmt, SettingController.DefaultShop.Id, SettingController.DefaultShop.ShortCode, Note, tableId, servicefee, null);
                    Id = entity.InsertTransaction(DateTime.Now, MemberShip.UserId, MemberShip.CounterId, TransactionType.Sale, true, true, paymentType, ExtraTax + Tax, ExtraDiscount + Discount, DetailList.Sum(x => x.TotalAmount) + (DetailList.Sum(x => x.TotalAmount) * servicefee / 100) - ExtraDiscount, totalCost, null, customerId, MCDiscount, BDDiscount, MemberTypeId, MCDiscountPercent, false, "", IsWholeSale, GiftDiscountAmt, SettingController.DefaultShop.Id, SettingController.DefaultShop.ShortCode, Note, tableId, servicefee, null);
                    resultId = Id.FirstOrDefault().ToString();
                    insertedId = resultId; // for prepaid saving's parentId/VousNo
                    insertedTransaction = (from trans in entity.Transactions where trans.Id == resultId select trans).FirstOrDefault<Transaction>();
                   
                    string TId = insertedTransaction.Id;
                    int i = 0;
                    foreach (TransactionDetail detail in DetailList)
                    {
                        //Console.WriteLine((++i).ToString());
                        detail.IsDeleted = false;//Update IsDelete (Null to 0)
                        if (detail.ConsignmentPrice == null)
                        {
                            detail.ConsignmentPrice = 0;
                        }

                        detail.Product = (from prod in entity.Products where prod.Id == (long)detail.ProductId select prod).FirstOrDefault();

                        Boolean? IsConsignmentPaid = Utility.IsConsignmentPaid(detail.Product);

                        var detailID = entity.InsertTransactionDetail(TId, Convert.ToInt32(detail.ProductId), Convert.ToInt32(detail.Qty), Convert.ToInt32(detail.UnitPrice), Convert.ToDouble(detail.DiscountRate), Convert.ToDouble(detail.TaxRate), Convert.ToInt32(detail.TotalAmount), detail.IsDeleted, detail.ConsignmentPrice, IsConsignmentPaid, detail.IsFOC, detail.SellingPrice).SingleOrDefault();
                       
                        detail.Product = (from prod in entity.Products where prod.Id == (long)detail.ProductId select prod).FirstOrDefault();
                        if (detail.Product.IsPackage == false && detail.Product.IsService == false)
                        {
                            detail.Product.Qty = detail.Product.Qty - detail.Qty;
                        }

                        //save in stocktransaction

                        Stock_Transaction st = new Stock_Transaction();
                        st.ProductId = detail.Product.Id;
                        if (detail.Product.IsPackage == false && detail.Product.IsService == false)
                        {
                            Qty = Convert.ToInt32(detail.Qty);
                        }
                        st.Sale = Qty;
                        productList.Add(st);
                        Qty = 0;
                        //Boolean? IsConsignmentPaid = Utility.IsConsignmentPaid(detail.Product);
                        //var detailID = entity.InsertTransactionDetail(TId, Convert.ToInt32(detail.ProductId), Convert.ToInt32(detail.Qty), Convert.ToInt32(detail.UnitPrice), Convert.ToDouble(detail.DiscountRate), Convert.ToDouble(detail.TaxRate), Convert.ToInt32(detail.TotalAmount), detail.IsDeleted, detail.ConsignmentPrice, IsConsignmentPaid, detail.IsFOC, Convert.ToInt32(detail.SellingPrice)).SingleOrDefault();
                        //Console.WriteLine("DetailID: " + detailID.ToString());
                        // Console.WriteLine("Product ID: " + detail.Product.Id.ToString());


                       // Boolean? IsConsignmentPaid = Utility.IsConsignmentPaid(detail.Product);
                        // var detailID = entity.InsertTransactionDetail(TId, Convert.ToInt32(detail.ProductId), Convert.ToInt32(detail.Qty), Convert.ToInt32(detail.UnitPrice), Convert.ToDouble(detail.DiscountRate), Convert.ToDouble(detail.TaxRate), Convert.ToInt32(detail.TotalAmount), detail.IsDeleted, detail.ConsignmentPrice, IsConsignmentPaid, detail.IsFOC, Convert.ToInt32(detail.SellingPrice)).SingleOrDefault();
                       
                        if (detail.Product.IsWrapper == true)
                        {
                           
                            List<WrapperItem> wList = detail.Product.WrapperItems.ToList();
                            if (wList.Count > 0)
                            {
                                foreach (WrapperItem w in wList)
                                {
                                    Product wpObj = (from p in entity.Products where p.Id == w.ChildProductId select p).FirstOrDefault();
                                    wpObj.Qty = wpObj.Qty - (w.ChildQty * detail.Qty);

                                    SPDetail spDetail = new SPDetail();
                                    spDetail.TransactionDetailID = Convert.ToInt32(detail.Id);
                                    spDetail.DiscountRate = detail.DiscountRate;
                                    spDetail.ParentProductID = w.ParentProductId;
                                    spDetail.ChildProductID = w.ChildProductId;
                                    spDetail.Price = wpObj.Price;
                                    spDetail.ChildQty = w.ChildQty * detail.Qty;
                                    entity.insertSPDetail(spDetail.TransactionDetailID, spDetail.ParentProductID, spDetail.ChildProductID, spDetail.Price, spDetail.DiscountRate, "PC", spDetail.ChildQty);
                                    Stock_Transaction stwp = new Stock_Transaction();
                                    stwp.ProductId = w.ChildProductId;
                                    Qty = Convert.ToInt32(w.ChildQty * detail.Qty);
                                    stwp.Sale = Qty;
                                    productList.Add(stwp);

                                    Qty = 0;
                                }
                            }
                            
                        }
                        if (detail.Product.IsPackage == true)
                        {
                           
                            POSEntities posEntity = new POSEntities();
                            PackagePurchasedInvoice packagePurchasedInvoice = new PackagePurchasedInvoice();
                            packagePurchasedInvoice.PackagePurchasedInvoiceId = System.Guid.NewGuid().ToString();
                            packagePurchasedInvoice.TransactionDetailId = Convert.ToInt64(detailID);
                            packagePurchasedInvoice.CustomerId = Convert.ToInt32(cboCustomerList.SelectedValue);
                            packagePurchasedInvoice.InvoiceDate = DateTime.Now;
                            packagePurchasedInvoice.ProductId = (long)detail.ProductId;
                            packagePurchasedInvoice.packageFrequency = detail.Product.PackageQty;
                            packagePurchasedInvoice.UseQty = 0;
                            packagePurchasedInvoice.IsDelete = false;
                            packagePurchasedInvoice.UserId = MemberShip.UserId;
                            posEntity.PackagePurchasedInvoices.Add(packagePurchasedInvoice);
                            posEntity.SaveChanges();
                        }//end of product Is Package
                        //entity.SaveChanges();
                        //detail.IsDeleted = false;
                        //detail.IsConsignmentPaid = Utility.IsConsignmentPaid(detail.Product);
                        //insertedTransaction.TransactionDetails.Add(detail);
                        entity.SaveChanges();
                    }
                    
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
                    insertedTransaction.IsDeleted = false;
                    //totalAmount -= totalCost;
                    receiveAmount -= totalCost;
                    entity.SaveChanges();
                   
                    #region current transation paid and remain amount is used other credit transaction
                    long originalReceiveAmt = receiveAmount;
                    long usePrepaid = 0; long totalPrepaid = PrepaidDebt;
                    if (receiveAmount > 0) // sales မှာသိမ်းပြီးနောက် ပိုသေးတာ
                    {
                        totalAmount = receiveAmount + PrepaidDebt;
                        if (unpaidTransList.Count > 0)
                        {                           

                            long settleAmount = 0;

                            int index = unpaidTransList.Count;
                            for (int outer = index - 1; outer >= 1; outer--) // for sorting desc??
                            {
                                for (int inner = 0; inner < outer; inner++)
                                {
                                    if (unpaidTransList[inner].TotalAmount - unpaidTransList[inner].RecieveAmount < unpaidTransList[inner + 1].TotalAmount - unpaidTransList[inner + 1].RecieveAmount)
                                    {
                                        Transaction t = unpaidTransList[inner];
                                        unpaidTransList[inner] = unpaidTransList[inner + 1];
                                        unpaidTransList[inner + 1] = t;
                                    }
                                }
                            }

                            List<Transaction> RefundList = new List<Transaction>();
                            foreach (Transaction CT in unpaidTransList)
                            {

                                if (totalAmount > 0)
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
                                    if (CreditAmount <= totalAmount) // ဒီ Credit ကိုချေဖို့ လုံလောက်တယ်
                                    {
                                        CreditT = (from t in entity.Transactions where t.Id == CT.Id select t).FirstOrDefault<Transaction>();
                                        CreditT.IsPaid = true;
                                        TranIdList.Add(CreditT.Id); // add setteled tranId to list
                                        entity.Entry(CreditT).State = EntityState.Modified;
                                        entity.SaveChanges();
                                        if (receiveAmount >= CreditAmount)
                                        {
                                            receiveAmount -= CreditAmount;
                                            totalAmount -= CreditAmount;
                                            settleAmount += CreditAmount;
                                            CreditAmount = 0;
                                           

                                        }
                                        else
                                        {
                                            CreditAmount -= receiveAmount;
                                            settleAmount += receiveAmount;
                                            totalAmount -= receiveAmount;
                                            receiveAmount = 0;

                                            if (totalPrepaid > 0)
                                            {
                                                foreach (Transaction trans in prePaidTranList)
                                                {
                                                    if (trans.IsPaid == false && trans.IsActive == false)
                                                    {
                                                        long currentUseAmt = 0;
                                                        long previousUseAmt = (long)(trans.UsePrePaidDebts1 == null ? 0 : trans.UsePrePaidDebts1.Sum(x => x.UseAmount));
                                                        long prepaidDebt = (long)trans.TotalAmount - previousUseAmt; // - currentUseAmt;
                                                        if (prepaidDebt >= CreditAmount) // ဒီ Prepaid နဲ့တင်ကျေ
                                                        {
                                                            CreditAmount = 0;
                                                            totalAmount -= CreditAmount;
                                                            currentUseAmt = CreditAmount;
                                                            totalPrepaid -= CreditAmount;
                                                            usePrepaid += currentUseAmt;
                                                            if (currentUseAmt == prepaidDebt)// Amount ကွက်တိဆိုရင် အဲ့ဒီ Prepaid transaction ကို Active True ပေး
                                                            {
                                                                trans.IsActive = true;
                                                                Transaction PD = (from PT in entity.Transactions where PT.Id == trans.Id select PT).FirstOrDefault<Transaction>();
                                                                PD.IsActive = true;
                                                                PD.IsPaid = true;
                                                                entity.Entry(PD).State = EntityState.Modified;
                                                                entity.SaveChanges();
                                                            }
                                                            else //if (prepaidDebt > currentUseAmt) // Prepaid အကုန်မသုံးရဘဲ ကျန်နေသေးရင် ဘယ်လောက်သုံးထားလဲ သိမ်းမယ်
                                                            {
                                                                UsePrePaidDebt usePrePaidDObj = new UsePrePaidDebt();
                                                                usePrePaidDObj.UseAmount = (int)currentUseAmt;
                                                                usePrePaidDObj.PrePaidDebtTransactionId = trans.Id;
                                                                usePrePaidDObj.CreditTransactionId = CT.Id;
                                                                usePrePaidDObj.CashierId = MemberShip.UserId;
                                                                usePrePaidDObj.CounterId = MemberShip.CounterId;
                                                                trans.UsePrePaidDebts1.Add(usePrePaidDObj);
                                                                entity.UsePrePaidDebts.Add(usePrePaidDObj);                                                                
                                                                entity.SaveChanges();
                                                            }

                                                            // break;
                                                        }
                                                        else //if (CreditAmount > prepaidDebt) နောက် Prepaid ကို ဆက်သုံးရမယ်
                                                        {
                                                            CreditAmount -= prepaidDebt;
                                                            totalAmount -= prepaidDebt;
                                                            currentUseAmt = prepaidDebt;
                                                            totalPrepaid -= prepaidDebt;
                                                            usePrepaid += currentUseAmt;

                                                            trans.IsActive = true;
                                                            Transaction PD = (from PT in entity.Transactions where PT.Id == trans.Id select PT).FirstOrDefault<Transaction>();
                                                            PD.IsActive = true;
                                                            PD.IsPaid = true;
                                                            entity.Entry(PD).State = EntityState.Modified;
                                                            entity.SaveChanges();

                                                        }
                                                    }


                                                }
                                            }
                                        }                              
                            
                                    }
                                }

                            }

                            if (settleAmount > 0) // save transaction type 'Settlement' // DebtAmount > 0   ဆိုတဲ့အဓိပ္ပါယ်က အကြွေးဆပ်ခဲ့တယ်ဆိုတဲ့အဓိပ္ပါယ်
                            {

                                string joinedTranIdList = string.Join(",", TranIdList);
                                System.Data.Objects.ObjectResult<string> DebtId = entity.InsertTransaction(DateTime.Now, MemberShip.UserId, MemberShip.CounterId, TransactionType.Settlement, true, true, paymentType, 0, 0, settleAmount, settleAmount, null, CustomerId, MCDiscount, BDDiscount, MemberTypeId, MCDiscountPercent, true, joinedTranIdList, IsWholeSale, 0, SettingController.DefaultShop.Id, SettingController.DefaultShop.ShortCode, Note, tableId, servicefee, null);
                                string _Debt = DebtId.FirstOrDefault().ToString();

                                foreach (var t in TranIdList)
                                {
                                    var result = (from tr in entity.Transactions where tr.Id == t select tr).FirstOrDefault();
                                    result.TranVouNos = _Debt;
                                    result.IsSettlement = true;
                                    entity.Entry(result).State = EntityState.Modified;
                                    entity.SaveChanges();
                                }
                            }
                               

                            if (receiveAmount > 0) // save as transaction type of 'Prepaid' // ဒါက ဆပ်ပြီးလို့ပိုနေတဲ့ဟာရှိရင် Prepaid နဲ့သိမ်းထားမှာ
                            {
                                string UnsettledTransIDs = string.Empty;
                                List<string> UnsettledTranList = unpaidTransList.Select(x => x.Id).ToList();
                                if (settleAmount > 0)
                                {
                                    UnsettledTranList = UnsettledTranList.Except(TranIdList).ToList();
                                }

                                UnsettledTransIDs = string.Join(",", UnsettledTranList);
                                System.Data.Objects.ObjectResult<string> PreDebtId = entity.InsertTransaction(DateTime.Now, MemberShip.UserId, MemberShip.CounterId, TransactionType.Prepaid, false, false, paymentType, 0, 0, receiveAmount, receiveAmount, null, CustomerId, MCDiscount, BDDiscount, MemberTypeId, MCDiscountPercent, false, UnsettledTransIDs, IsWholeSale, 0, SettingController.DefaultShop.Id, SettingController.DefaultShop.ShortCode, Note, tableId, servicefee, null);
                                entity.SaveChanges();

                               
                            }


                            


                        }

                    }

                    #endregion

                }//end of if
                #endregion

                #region add credit Transaction
                else // if (receivedAmt  < actualCost)
                {
                    Id = entity.InsertTransaction(DateTime.Now, MemberShip.UserId, MemberShip.CounterId, TransactionType.Credit, false, true, paymentType, ExtraTax + Tax, ExtraDiscount + Discount, DetailList.Sum(x => x.TotalAmount) + (DetailList.Sum(x => x.TotalAmount) * servicefee / 100) - ExtraDiscount - (int)MCDiscount - (int)BDDiscount, receiveAmount, null, customerId, MCDiscount, BDDiscount, MemberTypeId, MCDiscountPercent, false, "", IsWholeSale, GiftDiscountAmt, SettingController.DefaultShop.Id, SettingController.DefaultShop.ShortCode, Note, tableId, servicefee, null);
                    resultId = Id.FirstOrDefault().ToString();
                    insertedId = resultId; // for prepaid saving's parentId
                    insertedTransaction = (from trans in entity.Transactions where trans.Id == resultId select trans).FirstOrDefault<Transaction>();

                    string TId = insertedTransaction.Id;
                    Console.WriteLine("In Credit, DetailList exists? Ans: " + DetailList.Any().ToString());

                    foreach (TransactionDetail detail in DetailList)
                    {
                        Console.WriteLine("Detail Product ID" + detail.ProductId.ToString());
                        detail.IsDeleted = false;//Update IsDelete (Null to 0)
                        if (detail.ConsignmentPrice == null)
                        {
                            detail.ConsignmentPrice = 0;
                        }

                        detail.Product = (from prod in entity.Products where prod.Id == (long)detail.ProductId select prod).FirstOrDefault();

                        Boolean? IsConsignmentPaid = Utility.IsConsignmentPaid(detail.Product);

                        var detailID = entity.InsertTransactionDetail(TId, Convert.ToInt32(detail.ProductId), Convert.ToInt32(detail.Qty), Convert.ToInt32(detail.UnitPrice), Convert.ToDouble(detail.DiscountRate), Convert.ToDouble(detail.TaxRate), Convert.ToInt32(detail.TotalAmount), detail.IsDeleted, detail.ConsignmentPrice, IsConsignmentPaid, detail.IsFOC, detail.SellingPrice).SingleOrDefault();
                        //add ticket
                        detail.Product = (from prod in entity.Products where prod.Id == (long)detail.ProductId select prod).FirstOrDefault();
                        if (detail.Product.IsPackage == false && detail.Product.IsService == false)
                        {
                            detail.Product.Qty = detail.Product.Qty - detail.Qty;
                        }

                        //save in stocktransaction

                        Stock_Transaction st = new Stock_Transaction();
                        st.ProductId = detail.Product.Id;
                        if (detail.Product.IsPackage == false && detail.Product.IsService == false)
                        {
                            Qty = Convert.ToInt32(detail.Qty);
                        }
                        st.Sale = Qty;
                        productList.Add(st);
                        Qty = 0;
                        //Boolean? IsConsignmentPaid = Utility.IsConsignmentPaid(detail.Product);
                        //var detailID = entity.InsertTransactionDetail(TId, Convert.ToInt32(detail.ProductId), Convert.ToInt32(detail.Qty), Convert.ToInt32(detail.UnitPrice), Convert.ToDouble(detail.DiscountRate), Convert.ToDouble(detail.TaxRate), Convert.ToInt32(detail.TotalAmount), detail.IsDeleted, detail.ConsignmentPrice, IsConsignmentPaid, detail.IsFOC, Convert.ToInt32(detail.SellingPrice)).SingleOrDefault();
                        //Console.WriteLine("DetailID: " + detailID.ToString());
                       // Console.WriteLine("Product ID: " + detail.Product.Id.ToString());
                        if (detail.Product.IsWrapper == true)
                        {
                            Console.WriteLine("IsWrapper: true");
                            List<WrapperItem> wList = detail.Product.WrapperItems.ToList();
                            if (wList.Count > 0)
                            {
                                foreach (WrapperItem w in wList)
                                {
                                    Product wpObj = (from p in entity.Products where p.Id == w.ChildProductId select p).FirstOrDefault();
                                    wpObj.Qty = wpObj.Qty - (w.ChildQty * detail.Qty);

                                    SPDetail spDetail = new SPDetail();
                                    spDetail.TransactionDetailID = Convert.ToInt32(detail.Id);
                                    spDetail.DiscountRate = detail.DiscountRate;
                                    spDetail.ParentProductID = w.ParentProductId;
                                    spDetail.ChildProductID = w.ChildProductId;
                                    spDetail.Price = wpObj.Price;
                                    spDetail.ChildQty = w.ChildQty * detail.Qty;
                                    entity.insertSPDetail(spDetail.TransactionDetailID, spDetail.ParentProductID, spDetail.ChildProductID, spDetail.Price, spDetail.DiscountRate, "PC", spDetail.ChildQty);
                                    Stock_Transaction stwp = new Stock_Transaction();
                                    stwp.ProductId = w.ChildProductId;
                                    Qty = Convert.ToInt32(w.ChildQty * detail.Qty);
                                    stwp.Sale = Qty;
                                    productList.Add(stwp);

                                    Qty = 0;
                                }
                            }
                           
                        }
                        if (detail.Product.IsPackage == true)
                        {
                           // Console.WriteLine("I am in Credit!!");
                            POSEntities posEntity = new POSEntities();
                            PackagePurchasedInvoice packagePurchasedInvoice = new PackagePurchasedInvoice();
                            packagePurchasedInvoice.PackagePurchasedInvoiceId = System.Guid.NewGuid().ToString();
                            packagePurchasedInvoice.TransactionDetailId = Convert.ToInt64(detailID);
                            packagePurchasedInvoice.CustomerId = Convert.ToInt32(cboCustomerList.SelectedValue);
                            packagePurchasedInvoice.InvoiceDate = DateTime.Now;
                            packagePurchasedInvoice.ProductId = (long)detail.ProductId;
                            packagePurchasedInvoice.packageFrequency = detail.Product.PackageQty;
                            packagePurchasedInvoice.UseQty = 0;
                            packagePurchasedInvoice.IsDelete = false;
                            packagePurchasedInvoice.UserId = MemberShip.UserId;
                            posEntity.PackagePurchasedInvoices.Add(packagePurchasedInvoice);
                            int effectedrow = posEntity.SaveChanges();
                          //  Console.WriteLine("Package Purchase Invoice Record: " + " " + effectedrow.ToString());
                        }//end of product Is Package
                        
                        

                        entity.SaveChanges();
                    }
                   // entity.SaveChanges();
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
                    insertedTransaction.IsDeleted = false;
                   // totalAmount = Convert.ToInt32(txtReceiveAmount.Text);
                    entity.SaveChanges();


                   
                }
                #endregion

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

                            if (pulist.Count > 0)
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
                        List<APP_Data.PurchaseDetail> pulist = Utility.InventoryByControlMethod(pId, entity);

                        if (pulist.Count > 0)
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

                                        }
                                    }
                                }
                            }
                        }

                    }



                }
                #endregion

                CustomerId = Convert.ToInt32(cboCustomerList.SelectedValue);

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
                        newRow.Name = transaction.Product.Name;
                        newRow.Qty = transaction.Qty.ToString();
                        newRow.DiscountPercent = transaction.DiscountRate.ToString();
                        newRow.Frequency = Convert.ToString(transaction.Product.PackageQty * transaction.Qty);
                        newRow.TotalAmount = (int)transaction.UnitPrice * (int)transaction.Qty;
                        newRow.PhotoPath = string.IsNullOrEmpty(transaction.Product.PhotoPath) ? "" : Application.StartupPath + transaction.Product.PhotoPath.Remove(0, 1);


                        if (transaction.IsFOC == true)
                        {
                            newRow.IsFOC = "FOC";
                        }

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
                    }

                    string reportPath = "";
                    ReportViewer rv = new ReportViewer();
                    ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["ItemList"]);
                    ReceiptDataLog receiptData = new ReceiptDataLog();
                    receiptData.InvoiceName = Utility.GetReportPath("Credit");
                    reportPath = Application.StartupPath + receiptData.InvoiceName;
                    rv.Reset();
                    rv.LocalReport.ReportPath = reportPath;
                    rv.LocalReport.DataSources.Add(rds);

                    Utility.Slip_Log(rv);

                    Utility.Slip_A4_Footer(rv);
                    if (Convert.ToInt32(insertedTransaction.MCDiscountAmt) != 0)
                    {
                        Int64 _mcDiscountAmt = Convert.ToInt64(insertedTransaction.MCDiscountAmt);
                        ReportParameter MCDiscountAmt = new ReportParameter("MCDiscount", "-" + _mcDiscountAmt.ToString());
                        rv.LocalReport.SetParameters(MCDiscountAmt);
                        receiptData.LoyaltyDiscount = _mcDiscountAmt;
                    }


                    else if (Convert.ToInt32(insertedTransaction.BDDiscountAmt) != 0)
                    {
                        Int64 _bcDiscountAmt = Convert.ToInt64(insertedTransaction.BDDiscountAmt);
                        ReportParameter BCDiscountAmt = new ReportParameter("MCDiscount", "-" + _bcDiscountAmt.ToString());
                        rv.LocalReport.SetParameters(BCDiscountAmt);
                        receiptData.LoyaltyDiscount = _bcDiscountAmt;
                    }
                    else
                    {
                        ReportParameter BCDiscountAmt = new ReportParameter("MCDiscount", "0");
                        rv.LocalReport.SetParameters(BCDiscountAmt);
                        receiptData.LoyaltyDiscount = 0;
                    }
                    if (SettingController.SelectDefaultPrinter != "Slip Printer")
                    {
                        ReportParameter PrintProductImage = new ReportParameter("PrintImage", SettingController.ShowProductImageIn_A4Reports.ToString());
                        rv.LocalReport.SetParameters(PrintProductImage);
                    }
                    ReportParameter TAmt = new ReportParameter("TAmt", _tAmt.ToString());
                    rv.LocalReport.SetParameters(TAmt);
                    receiptData.Total = _tAmt.ToString();

                    if (insertedTransaction.Note == "")
                    {
                        ReportParameter Notes = new ReportParameter("Notes", " ");
                        rv.LocalReport.SetParameters(Notes);
                        receiptData.Note = " ";
                    }
                    else
                    {
                        ReportParameter Notes = new ReportParameter("Notes", insertedTransaction.Note);
                        rv.LocalReport.SetParameters(Notes);
                        receiptData.Note = insertedTransaction.Note;
                    }
                    receiptData.ShopName = SettingController.ShopName;
                    ReportParameter ShopName = new ReportParameter("ShopName", receiptData.ShopName);
                    rv.LocalReport.SetParameters(ShopName);

                    receiptData.BranchName = SettingController.BranchName;
                    ReportParameter BranchName = new ReportParameter("BranchName", receiptData.BranchName);
                    rv.LocalReport.SetParameters(BranchName);

                    ReportParameter Phone = new ReportParameter("Phone", SettingController.PhoneNo);
                    rv.LocalReport.SetParameters(Phone);

                    ReportParameter OpeningHours = new ReportParameter("OpeningHours", SettingController.OpeningHours);
                    rv.LocalReport.SetParameters(OpeningHours);

                    receiptData.TransactionId = resultId;
                    ReportParameter TransactionId = new ReportParameter("TransactionId", resultId.ToString());
                    rv.LocalReport.SetParameters(TransactionId);

                    APP_Data.Counter counter = entity.Counters.FirstOrDefault(x => x.Id == MemberShip.CounterId);
                    receiptData.CounterName = counter.Name;
                    ReportParameter CounterName = new ReportParameter("CounterName", counter.Name);
                    rv.LocalReport.SetParameters(CounterName);

                    ReportParameter TotalGiftDiscount = new ReportParameter("TotalGiftDiscount", GiftDiscountAmt.ToString());
                    rv.LocalReport.SetParameters(TotalGiftDiscount);
                    receiptData.TotalDiscount = GiftDiscountAmt;

                    ReportParameter InvoiceDate = new ReportParameter("InvoiceDate", ((DateTime)insertedTransaction.DateTime).ToString("dd-MMM-yyyy"));
                    rv.LocalReport.SetParameters(InvoiceDate);
                    receiptData.InvoiceDate = ((DateTime)insertedTransaction.DateTime).ToString("dd-MMM-yyyy");

                    receiptData.CashierName = MemberShip.UserName;
                    ReportParameter CasherName = new ReportParameter("CasherName", receiptData.CashierName);
                    rv.LocalReport.SetParameters(CasherName);

                    Int64 totalAmountRep = insertedTransaction.TotalAmount == null ? 0 : Convert.ToInt64(insertedTransaction.TotalAmount) - GiftDiscountAmt; //Edit By ZMH
                    ReportParameter TotalAmount = new ReportParameter("TotalAmount", totalAmountRep.ToString());
                    rv.LocalReport.SetParameters(TotalAmount);
                    receiptData.Total = totalAmountRep.ToString();



                    //ReportParameter TotalAmount = new ReportParameter("TotalAmount", insertedTransaction.TotalAmount.ToString()); //Edit By ZMH 
                    //rv.LocalReport.SetParameters(TotalAmount);

                    ReportParameter TaxAmount = new ReportParameter("TaxAmount", insertedTransaction.TaxAmount.ToString());
                    rv.LocalReport.SetParameters(TaxAmount);
                    receiptData.Tax = insertedTransaction.TaxAmount;
                    if (Convert.ToInt32(insertedTransaction.DiscountAmount) == 0)
                    {
                        ReportParameter DiscountAmount = new ReportParameter("DiscountAmount", insertedTransaction.DiscountAmount.ToString());
                        rv.LocalReport.SetParameters(DiscountAmount);
                        receiptData.TotalDiscount = insertedTransaction.DiscountAmount;

                    }
                    else
                    {
                        ReportParameter DiscountAmount = new ReportParameter("DiscountAmount", "-" + (insertedTransaction.DiscountAmount - ExtraDiscount).ToString());
                        rv.LocalReport.SetParameters(DiscountAmount);
                        receiptData.TotalDiscount = insertedTransaction.DiscountAmount - ExtraDiscount;

                    }
                    if (ExtraDiscount != 0)
                    {
                        ReportParameter AdditionalDiscount = new ReportParameter("AdditionalDiscount", "-" + ExtraDiscount.ToString());                        
                        rv.LocalReport.SetParameters(AdditionalDiscount);
                        receiptData.AdditionalDiscount = ExtraDiscount;
                    }


                    ReportParameter PaidAmount = new ReportParameter("PaidAmount", txtReceiveAmount.Text);
                    rv.LocalReport.SetParameters(PaidAmount);
                    receiptData.PaidAmount = txtReceiveAmount.Text;

                    ReportParameter PrevOutstanding = new ReportParameter("PrevOutstanding", lblPreviousBalance.Text);
                    rv.LocalReport.SetParameters(PrevOutstanding);
                    receiptData.PreviousOutstanding = string.IsNullOrEmpty(lblPreviousBalance.Text)?0:decimal.Parse(lblPreviousBalance.Text);
                    //ReportParameter PrePaidDebt = new ReportParameter("PrePaidDebt", PrepaidDebt.ToString());
                    //rv.LocalReport.SetParameters(PrePaidDebt);

                    receiptData.Netpayable = (OldOutstandingAmount + insertedTransaction.TotalAmount - PrepaidDebt);
                    ReportParameter NetPayable = new ReportParameter("NetPayable", receiptData.Netpayable.ToString());
                    rv.LocalReport.SetParameters(NetPayable);

                    receiptData.Balance = ((OldOutstandingAmount + insertedTransaction.TotalAmount - PrepaidDebt) - Convert.ToInt64(txtReceiveAmount.Text)).ToString();
                    ReportParameter Balance = new ReportParameter("Balance", receiptData.Balance);
                    rv.LocalReport.SetParameters(Balance);
                 
                    ReportParameter CustomerName = new ReportParameter("CustomerName", cboCustomerList.Text);
                    rv.LocalReport.SetParameters(CustomerName);
                    receiptData.PatientName = cboCustomerList.Text;
                    receiptData.PatientPhoneNo = cust.PhoneNumber;
                    receiptData.Address = cust.Address;
                    if (Utility.GetDefaultPrinter() == "A4 Printer")
                    {
                        ReportParameter CusPhoneNumber = new ReportParameter("CusPhoneNumber", cust.PhoneNumber);
                        rv.LocalReport.SetParameters(CusPhoneNumber);
                        ReportParameter CusAddress = new ReportParameter("CusAddress", insertedTransaction.Customer.Address);
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

                    Utility.Get_Print(rv);

                    Utility.InsertReceiptLog(receiptData);


                }
                #endregion
                _result = MessageShow();
                if (System.Windows.Forms.Application.OpenForms["Sales"] != null)
                {
                    Sales newForm = (Sales)System.Windows.Forms.Application.OpenForms["Sales"];
                    newForm.note = "";
                    newForm.Clear();
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
                    newForm.note = "";
                    newForm.Clear();
                }
            }
        }

        private DialogResult MessageShow()
        {
            DialogResult result = MessageBox.Show("Payment Completed", "mPOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return result;
        }
        private void cboCustomerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            PrepaidDebt = 0;
            if (cboCustomerList.SelectedIndex != 0)
            {
                //get outstanding amount
                int customerId = Convert.ToInt32(cboCustomerList.SelectedValue.ToString());               
                List<Transaction> OldOutStandingList = entity.Transactions.Where(x => x.CustomerId == customerId && (x.IsDeleted == false || x.IsDeleted == null)).ToList();
                OldOutstandingAmount = 0;
                foreach (Transaction tf in OldOutStandingList)
                {
                    if (tf.Type == "Credit" && tf.IsPaid == false && tf.IsDeleted == false)
                    {
                        List<Transaction> rtList = new List<Transaction>();
                        OldOutstandingAmount += (int)((tf.TotalAmount) - tf.RecieveAmount);
                        rtList = (from rt in entity.Transactions where rt.Type == TransactionType.CreditRefund && rt.ParentId == tf.Id && rt.IsDeleted == false select rt).ToList();

                        if (rtList.Count > 0)
                        {
                            foreach (Transaction rt in rtList)
                            {
                                OldOutstandingAmount -= (int)rt.RecieveAmount;
                            }
                        }

                        if (tf.UsePrePaidDebts != null)
                        {
                            OldOutstandingAmount -= Convert.ToInt32(tf.UsePrePaidDebts.Sum(x => x.UseAmount));
                        }

                        if (tf.CancellationLogs != null)
                        {
                            OldOutstandingAmount -= Convert.ToInt32(tf.CancellationLogs.Sum(x => x.CancelledAmount));
                        }

                    }

                }


                if (OldOutstandingAmount < 0)
                {
                    lblPreviousBalance.Text = "0";
                   // lblPrePaid.Text = PrepaidDebt.ToString();
                    lblTotalCost.Text = ((DetailList.Sum(x => x.TotalAmount) + (((long)DetailList.Sum(x => x.TotalAmount) * servicefee) / 100) - ExtraDiscount - MCDiscount - BDDiscount)).ToString();
                    lblNetPayable.Text = ((DetailList.Sum(x => x.TotalAmount) + (((long)DetailList.Sum(x => x.TotalAmount) * servicefee) / 100) - ExtraDiscount - MCDiscount - BDDiscount)).ToString();
                }
                else
                {
                    lblPreviousBalance.Text = OldOutstandingAmount.ToString();
                   // lblPrePaid.Text = PrepaidDebt.ToString();
                    lblTotalCost.Text = ((DetailList.Sum(x => x.TotalAmount) + (((long)DetailList.Sum(x => x.TotalAmount) * servicefee) / 100) - ExtraDiscount - MCDiscount - BDDiscount) + OldOutstandingAmount - PrepaidDebt).ToString();
                    lblNetPayable.Text = ((DetailList.Sum(x => x.TotalAmount) + (((long)DetailList.Sum(x => x.TotalAmount) * servicefee) / 100) - ExtraDiscount - MCDiscount - BDDiscount) + OldOutstandingAmount - PrepaidDebt).ToString();
                }
                //to 
                int amount = 0;
                Int32.TryParse(txtReceiveAmount.Text, out amount);
                int totalCost = 0;
                Int32.TryParse(lblTotalCost.Text, out totalCost);

                if (amount >= totalCost)
                {
                    lblNetPayableTitle.Text = "Change";
                    lblNetPayable.Text = (amount - totalCost).ToString();
                }
                else
                {
                    lblNetPayableTitle.Text = "Net Payable";
                    lblNetPayable.Text = (totalCost - amount).ToString();
                }
            }
            else
            {
                lblPreviousBalance.Text = "0";
                lblTotalCost.Text = ((DetailList.Sum(x => x.TotalAmount) + (((long)DetailList.Sum(x => x.TotalAmount) * servicefee) / 100) - ExtraDiscount - MCDiscount - BDDiscount) + 0).ToString();
                lblNetPayable.Text = ((DetailList.Sum(x => x.TotalAmount) + (((long)DetailList.Sum(x => x.TotalAmount) * servicefee) / 100) - ExtraDiscount - MCDiscount - BDDiscount) + 0).ToString();

                //to 
                int amount = 0;
                Int32.TryParse(txtReceiveAmount.Text, out amount);

                if (amount >= (Int32.Parse(lblTotalCost.Text)))
                {
                    lblNetPayableTitle.Text = "Change";
                    lblNetPayable.Text = (amount - (DetailList.Sum(x => x.TotalAmount) + (((long)DetailList.Sum(x => x.TotalAmount) * servicefee) / 100) - ExtraDiscount)).ToString();
                }
                else
                {
                    lblNetPayableTitle.Text = "NetPayable";
                    lblNetPayable.Text = ((DetailList.Sum(x => x.TotalAmount) + (((long)DetailList.Sum(x => x.TotalAmount) * servicefee) / 100) - ExtraDiscount) - amount).ToString();
                }
            }
        }

        private void txtReceiveAmount_KeyUp(object sender, KeyEventArgs e)
        {
            int amount = 0;
            Int32.TryParse(txtReceiveAmount.Text, out amount);
            int totalCost = 0;
            Int32.TryParse(lblTotalCost.Text, out totalCost);

            if (amount >= totalCost)
            {
                lblNetPayableTitle.Text = "Change";
                lblNetPayable.Text = (amount - totalCost).ToString();
            }
            else
            {
                lblNetPayableTitle.Text = "NetPayable";
                lblNetPayable.Text = (totalCost - amount).ToString();
            }

            int receiveamt = 0;
            if (txtReceiveAmount.Text == string.Empty)
            {
                receiveamt = 0;
            }
            else
            {
                receiveamt = Convert.ToInt32(txtReceiveAmount.Text);
            }

            if (receiveamt > Convert.ToInt32(lblTotalCost.Text))
            {
                lblNetPayableTitle.Text = "Change";
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            NewCustomer newform = new NewCustomer();
            newform.Show();
        }

        private void PaidByCreditWithPrePaidDebt_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(cboCustomerList);
            tp.Hide(txtReceiveAmount);
        }

        private void txtReceiveAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

        }

        

      
        #endregion

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

        public void LoadForm()
        {
            List<Customer> CustomerList = new List<Customer>();
            Customer none = new Customer();
            none.Name = "Select Customer";
            none.Id = 0;
            CustomerList.Add(none);
            CustomerList.AddRange(entity.Customers.ToList());
            cboCustomerList.DataSource = CustomerList;
            cboCustomerList.DisplayMember = "Name";
            cboCustomerList.ValueMember = "Id";
            cboCustomerList.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboCustomerList.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        #endregion

        private void cboPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPaymentType.SelectedIndex == 1)
            {
                lblBank.Visible = true;
                cboBankTransfer.Visible = true;
                POSEntities posEntites = new POSEntities();
                var bankList = posEntites.PaymentTypes.Where(x => x.Id >= 501).ToList();
                cboBankTransfer.DataSource = bankList;
                cboBankTransfer.DisplayMember = "Name";
                cboBankTransfer.ValueMember = "Id";
            }
            else
            {
                lblBank.Visible = false;
                cboBankTransfer.Visible = false;
                cboBankTransfer.DataSource = null;
            }
        }

        private void txtReceiveAmount_TextChanged(object sender, EventArgs e)
        {
            if (txtReceiveAmount.Text == Convert.ToString(0))
            {
                cboPaymentType.Enabled = false;
            }
            else
            {
                cboPaymentType.Enabled = true;
            }
        }

       
    }
}
