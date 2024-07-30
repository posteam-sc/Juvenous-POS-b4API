using Microsoft.Reporting.WinForms;
using POS.APP_Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.Objects;

namespace POS
{
    public partial class frmPaidByCredit : Form
    {
        #region Variables
        public List<TransactionDetail> DetailList = new List<TransactionDetail>();
        public string transactionParentId { get; set; }
        
        public int ExtraDiscount { get; set; }

        public Boolean isDraft { get; set; }

        public Boolean isDebt { get; set; }

        public string DraftId { get; set; }

        
        public long DebtAmount { get; set; }

        public Boolean IsWholeSale { get; set; }
       
        int paymentType;
        public long GiftDiscountAmt { get; set; }
      
        
        public List<Transaction> CreditTransaction { get; set; }

        //public List<Transaction> PrePaidTransaction { get; set; }

        private POSEntities entity = new POSEntities();

        private ToolTip tp = new ToolTip();

        private long totalAmount = 0;

        public decimal BDDiscount { get; set; }

        public decimal MCDiscount { get; set; }

        public int CustomerId { get; set; }

        public int? MemberTypeId { get; set; }

        public decimal? MCDiscountPercent { get; set; }

        
        public decimal TotalAmt = 0;

        decimal AmountWithExchange;

        long total;

        string resultId = "-";

        
        
        public List<string> TranIdList = new List<string>();

        
        public Boolean IsPrint = true;
        public string Note = "";

        public int? tableId = null;
       // public int servicefee = 0;
        int currencyID;
        long receiveAmount = 0;

        #endregion

        public frmPaidByCredit()
        {
            InitializeComponent();
        }


        private void frmPaidByCredit_Load(object sender, EventArgs e)
        {
            BindCurrency();
            BindPaymentMethodCombo();
            BindTransaction();

        }

        #region Bind Currency 

        private void BindCurrency()
        {
            POSEntities entity = new POSEntities();
            Currency curreObj = new Currency();
            List<Currency> currencyList = new List<Currency>();
            currencyList = entity.Currencies.ToList();
            foreach (Currency c in currencyList)
            {
                cboCurrency.Items.Add(c.CurrencyCode);
            }
            currencyID = 0;
            if (SettingController.DefaultCurrency != 0)
            {
                currencyID = Convert.ToInt32(SettingController.DefaultCurrency);
                curreObj = entity.Currencies.FirstOrDefault(x => x.Id == currencyID);
                cboCurrency.Text = curreObj.CurrencyCode;
            }
            //txtExchangeRate.Text = SettingController.DefaultExchangeRate.ToString();

        }

        #endregion

        #region BindPayment
        private void BindPaymentMethodCombo()
        {
            cboPaymentMethod.DataSource = entity.PaymentTypes.Where(x => x.Id == 1 || x.Id == 5).ToList();
            cboPaymentMethod.DisplayMember = "Name";
            cboPaymentMethod.ValueMember = "Id";
        }
        #endregion

        #region Method
        private void BindTransaction()
        {
            foreach (Transaction tObj in CreditTransaction)
            {
                totalAmount += (long)tObj.TotalAmount - (long)tObj.RecieveAmount-(long)tObj.UsePrePaidDebts.Sum(x=>x.UseAmount); // total sum of all records in outstanding grid
                List<Transaction> rtList = (from t in entity.Transactions where t.Type == TransactionType.CreditRefund && t.ParentId == tObj.Id select t).ToList();
                if (rtList.Count > 0)
                {
                    foreach (Transaction rt in rtList)
                    {
                        totalAmount -= (long)rt.RecieveAmount;
                    }
                }
               
                if (tObj.CancellationLogs != null)
                {
                    totalAmount -= Convert.ToInt32(tObj.CancellationLogs.Sum(x => x.CancelledAmount));
                }

            }
            // Console.WriteLine("Prepaid Count = " + PrePaidTransaction.Count.ToString());
            //foreach (Transaction tObj in PrePaidTransaction)
            //{
            //    long useAmount = (tObj.UsePrePaidDebts1 == null) ? 0 : (int)tObj.UsePrePaidDebts1.Sum(x => x.UseAmount);
            //    prePaidAmount += (long)tObj.TotalAmount;
            //    prePaidAmount -= useAmount;

            //}


            lblTotalCost.Text = Utility.CalculateExchangeRate(currencyID, totalAmount).ToString();

            if (SettingController.TicketSale)
            {
                txtRecieveAmt.Text = lblTotalCost.Text;
            }
        }

        private bool CheckValidation()
        {
            if (cboCurrency.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select Currency!", "Paid By Credit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboCurrency.Focus();
                return false;
            }
            if (receiveAmount == 0)
            {
                MessageBox.Show("Please fill up receive amount!", "Paid By Credit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtRecieveAmt.Focus();
                return false;
            }
            else if (receiveAmount < AmountWithExchange)
            {

                MessageBox.Show("Receive amount must be greater than total cost!", "Paid By Credit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtRecieveAmt.Focus();
                return false;
            }

            return true;
        }
      
        private void Calculate()
        {
            POSEntities entity = new POSEntities();
            DialogResult _result = new DialogResult();

            Boolean hasError = false;
            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            long receiveAmount = 0;
            long totalCost = (long)DetailList.Sum(x => x.TotalAmount) - ExtraDiscount - (long)BDDiscount - (long)MCDiscount - GiftDiscountAmt;
            //total cost wint unit price
            long unitpriceTotalCost = (long)DetailList.Sum(x => x.UnitPrice * x.Qty);
            Int64.TryParse(txtRecieveAmt.Text, out receiveAmount);
            decimal totalCashSaleAmount = Convert.ToDecimal(lblTotalCost.Text);
            string strPaymentType;
            if (cboBankPayment.Visible == true)
            {
                strPaymentType = cboBankPayment.Text;
            }
            else
            {
                strPaymentType = cboPaymentMethod.Text;
            }

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
                tp.SetToolTip(txtRecieveAmt, "Error");
                tp.Show("Please fill up receive amount!", txtRecieveAmt);
                hasError = true;
            }
            else if (receiveAmount < AmountWithExchange)
            {
                tp.SetToolTip(txtRecieveAmt, "Error");
                tp.Show("Receive amount must be greater than total cost!", txtRecieveAmt);
                hasError = true;
            }
            if (!hasError)
            {
                System.Data.Objects.ObjectResult<String> Id;
                Transaction toPrintTransaction = new Transaction();
                List<Transaction> RefundList = new List<Transaction>();
                decimal change = 0;
                if (cu.CurrencyCode == "USD")
                {
                    totalCashSaleAmount = (decimal)totalCashSaleAmount * (decimal)cu.LatestExchangeRate;
                    receiveAmount = receiveAmount * (long)cu.LatestExchangeRate;
                    change = Convert.ToDecimal(lblChanges.Text) * (decimal)cu.LatestExchangeRate;
                }
                else
                {
                    change = Convert.ToDecimal(lblChanges.Text);
                }

                if (lblChangesText.Text == "Changes")
                {
                    receiveAmount -= Convert.ToInt64(lblChanges.Text);
                }
                if (cboPaymentMethod.SelectedIndex == 0)
                {
                    paymentType = Convert.ToInt32(cboPaymentMethod.SelectedValue);
                }
                else
                {
                    paymentType = Convert.ToInt32(cboBankPayment.SelectedValue);
                }
                long totalAmount = receiveAmount;


                Transaction currentTrans = null;
                long settleAmount = 0;
                long CreditAmount = 0;
               
                if (CreditTransaction.Count > 0)
                {
                    CreditTransaction = CreditTransaction.OrderBy(x => x.DateTime).ToList(); //khs 15-aug-2023
                    foreach (Transaction CT in CreditTransaction)
                    {
                        if (totalAmount > 0)
                        {
                            CreditAmount = (long)CT.TotalAmount - (long)CT.RecieveAmount - (long)CT.UsePrePaidDebts.Sum(x => x.UseAmount);
                            RefundList = (from tr in entity.Transactions where tr.ParentId == CT.Id && tr.Type == TransactionType.CreditRefund select tr).ToList();
                            if (RefundList.Count > 0)
                            {
                                foreach (Transaction TRefund in RefundList)
                                {
                                    CreditAmount -= (long)TRefund.RecieveAmount;
                                }
                            }

                            if (CreditAmount <= totalAmount)
                            {
                                TranIdList.Add(CT.Id); // add setteled tranId to list
                                totalAmount -= CreditAmount;
                                settleAmount += CreditAmount;
                            }
                            else
                            {
                                currentTrans = CT;
                                break;
                            }
                        }
                    }
                    if (settleAmount > 0)
                    {
                        //save settlement transaction
                        string joinedTranIdList = string.Join(",", TranIdList);
                        string settledId = entity.InsertTransaction(DateTime.Now, MemberShip.UserId, MemberShip.CounterId, TransactionType.Settlement, true, true, paymentType, 0, 0, settleAmount, settleAmount, null, CustomerId, MCDiscount, BDDiscount, MemberTypeId, MCDiscountPercent, true, joinedTranIdList, IsWholeSale, 0, SettingController.DefaultShop.Id, SettingController.DefaultShop.ShortCode, Note, tableId, null, null).FirstOrDefault();

                        foreach (string t in TranIdList)
                        {
                            Transaction CreditTran = entity.Transactions.FirstOrDefault(x=> x.Id == t);
                            CreditTran.IsPaid = true;
                            CreditTran.TranVouNos = settledId;
                            CreditTran.IsSettlement = true;
                            entity.Entry(CreditTran).State = EntityState.Modified;
                            entity.SaveChanges();

                            //update prepaid transactions of the current credit transaction
                            List<string> PrepaidTransID = CreditTran.UsePrePaidDebts.Select(x => x.PrePaidDebtTransactionId).ToList();
                            if (PrepaidTransID != null)
                            {
                                List<Transaction> PrepaidList = entity.Transactions.Where(x => PrepaidTransID.Contains(x.Id)).ToList();
                                foreach (Transaction trans in PrepaidList)
                                {
                                    trans.IsPaid = true;
                                    entity.Entry(trans).State = EntityState.Modified;
                                    entity.SaveChanges();
                                }
                            }
                        }

                        Transaction settlementTrans = entity.Transactions.FirstOrDefault(x=> x.Id == settledId);
                        settlementTrans.ReceivedCurrencyId = cu.Id;
                        entity.Entry(settlementTrans).State = EntityState.Modified;
                        entity.SaveChanges();

                        ExchangeRateForTransaction ex = new ExchangeRateForTransaction();
                        ex.TransactionId = settledId;
                        ex.CurrencyId = cu.Id;
                        ex.ExchangeRate = Convert.ToInt32(cu.LatestExchangeRate);
                        entity.ExchangeRateForTransactions.Add(ex);
                        entity.SaveChanges();
                        //set settlement transaction as to-print transaction
                        toPrintTransaction = settlementTrans;
                    }
                    if (currentTrans != null)
                    {
                        //save as prepaid transactions
                       // ObjectResult<string> 
                        string prepaidId = entity.InsertTransaction(DateTime.Now, MemberShip.UserId, MemberShip.CounterId, TransactionType.Prepaid, false, true, paymentType, 0, 0, totalAmount, totalAmount, null, CustomerId, MCDiscount, BDDiscount, MemberTypeId, MCDiscountPercent, false, null, IsWholeSale, 0, SettingController.DefaultShop.Id, SettingController.DefaultShop.ShortCode, Note, tableId, null, null).FirstOrDefault();

                        UsePrePaidDebt usePrePaidDObj = new UsePrePaidDebt();
                        usePrePaidDObj.UseAmount = (int)totalAmount;
                        usePrePaidDObj.PrePaidDebtTransactionId = prepaidId;
                        usePrePaidDObj.CreditTransactionId = currentTrans.Id;
                        usePrePaidDObj.CashierId = MemberShip.UserId;
                        usePrePaidDObj.CounterId = MemberShip.CounterId;
                        currentTrans.UsePrePaidDebts1.Add(usePrePaidDObj);
                        entity.UsePrePaidDebts.Add(usePrePaidDObj);
                        entity.SaveChanges();

                        Transaction prepaidTrans = entity.Transactions.FirstOrDefault(x => x.Id == prepaidId);
                        prepaidTrans.ReceivedCurrencyId = cu.Id;
                        entity.Entry(prepaidTrans).State = EntityState.Modified;
                        entity.SaveChanges();

                        ExchangeRateForTransaction ex = new ExchangeRateForTransaction();
                        ex.TransactionId = prepaidId;
                        ex.CurrencyId = cu.Id;
                        ex.ExchangeRate = Convert.ToInt32(cu.LatestExchangeRate);
                        entity.ExchangeRateForTransactions.Add(ex);
                        entity.SaveChanges();
                        // if there is no settlement, set prepaid transaction as to-print transaction
                        if (settleAmount == 0)
                        {
                            toPrintTransaction = prepaidTrans;
                        }
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
                    ReceiptDataLog receiptData = new ReceiptDataLog();


                    ReportViewer rv = new ReportViewer();
                    string reportName = Utility.GetReportPath("Settlement");
                    string reportPath = Application.StartupPath + reportName;
                    rv.Reset();
                    receiptData.InvoiceName = reportName;                  
                    rv.LocalReport.ReportPath = reportPath;
                    
                    Utility.Slip_Log(rv);
                    Utility.Slip_A4_Footer(rv);
                    APP_Data.Customer cus = entity.Customers.Where(x => x.Id == CustomerId).FirstOrDefault();

                    ReportParameter CustomerName = new ReportParameter("CustomerName", cus.Name);
                    rv.LocalReport.SetParameters(CustomerName);
                    receiptData.PatientName = cus.Name;
                    receiptData.PatientPhoneNo = cus.PhoneNumber;
                    receiptData.Address = cus.Address;

                    if (toPrintTransaction.Note == "")
                    {
                        ReportParameter Notes = new ReportParameter("Notes", " ");
                        rv.LocalReport.SetParameters(Notes);
                        receiptData.Note = " ";
                    }
                    else
                    {
                        ReportParameter Notes = new ReportParameter("Notes", toPrintTransaction.Note);
                        rv.LocalReport.SetParameters(Notes);
                        receiptData.Note = toPrintTransaction.Note;
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

                    receiptData.TransactionId = toPrintTransaction.Id;
                    ReportParameter TransactionId = new ReportParameter("TransactionId", receiptData.TransactionId);
                    rv.LocalReport.SetParameters(TransactionId);

                    APP_Data.Counter c = entity.Counters.Where(x => x.Id == MemberShip.CounterId).FirstOrDefault();

                    receiptData.CounterName = c.Name;
                    ReportParameter CounterName = new ReportParameter("CounterName", c.Name);
                    rv.LocalReport.SetParameters(CounterName);


                    receiptData.InvoiceDate = Convert.ToDateTime(toPrintTransaction.DateTime).ToString("dd-MMM-yyyy");
                    ReportParameter InvoiceDate = new ReportParameter("InvoiceDate", receiptData.InvoiceDate);
                    rv.LocalReport.SetParameters(InvoiceDate);


                    receiptData.CashierName = MemberShip.UserName;
                    ReportParameter CasherName = new ReportParameter("CasherName", receiptData.CashierName);
                    rv.LocalReport.SetParameters(CasherName);

                    receiptData.Total = lblTotalCost.Text;
                    ReportParameter TotalAmount = new ReportParameter("TotalAmount", lblTotalCost.Text);
                    rv.LocalReport.SetParameters(TotalAmount);

                    receiptData.PaymentType = strPaymentType;
                    ReportParameter PaymentType = new ReportParameter("PaymentType", strPaymentType);
                    rv.LocalReport.SetParameters(PaymentType);

                    receiptData.PaidAmount = txtRecieveAmt.Text;
                    ReportParameter PaidAmount = new ReportParameter("PaidAmount", txtRecieveAmt.Text);
                    rv.LocalReport.SetParameters(PaidAmount);

                    int balance = Convert.ToInt32(lblTotalCost.Text) - Convert.ToInt32(txtRecieveAmt.Text);
                    balance = balance < 0 ? 0 : balance;
                    receiptData.Balance = balance.ToString();
                    ReportParameter Balance = new ReportParameter("Balance", balance.ToString());
                    rv.LocalReport.SetParameters(Balance);

                    int _change = Convert.ToInt32(txtRecieveAmt.Text) - Convert.ToInt32(lblTotalCost.Text);

                    _change = _change < 0 ? 0 : _change;

                    receiptData.Change = _change.ToString();
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
                        
                        ReportParameter PhoneImagePath = new ReportParameter("PhoneImagePath","file:\\" + Application.StartupPath + "\\Images\\phone.png");
                        rv.LocalReport.SetParameters(PhoneImagePath);

                        ReportParameter WebsiteImagePath = new ReportParameter("WebsiteImagePath", "file:\\" + Application.StartupPath + "\\Images\\website.png");
                        rv.LocalReport.SetParameters(WebsiteImagePath);

                        ReportParameter EmailImagePath = new ReportParameter("EmailImagePath", "file:\\" + Application.StartupPath + "\\Images\\mail.png");
                        rv.LocalReport.SetParameters(EmailImagePath);

                        ReportParameter LocationImagePath = new ReportParameter("LocationImagePath", "file:\\" + Application.StartupPath + "\\Images\\location.png");
                        rv.LocalReport.SetParameters(LocationImagePath);
                    }
                    ReportParameter CusPhoneNumber = new ReportParameter("CusPhoneNumber", receiptData.PatientPhoneNo);
                    rv.LocalReport.SetParameters(CusPhoneNumber);
                    Utility.Get_Print(rv);
                    rv.Reset();
                    Utility.InsertReceiptLog(receiptData);
                    
                    #endregion
                }
                _result = MessageShow();
                if (System.Windows.Forms.Application.OpenForms["CustomerDetail"] != null)
                {
                    CustomerDetail newForm = (CustomerDetail)System.Windows.Forms.Application.OpenForms["CustomerDetail"];
                    newForm.Reload();
                }
                Note = "";
                this.Dispose();
            }// if !hasError

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

        #endregion

        #region Selected Index Change
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

                        Decimal.TryParse(txtRecieveAmt.Text, out receive);
                        decimal changes = AmountWithExchange - receive;

                        lblChanges.Text = changes.ToString();
                    }
                    else
                    {

                        lblTotalCost.Text = Utility.CalculateExchangeRate(cu.Id, DebtAmount).ToString();
                        AmountWithExchange = Convert.ToDecimal(lblTotalCost.Text);
                        decimal receive = 0;

                        Decimal.TryParse(txtRecieveAmt.Text, out receive);
                        decimal changes = AmountWithExchange - receive;
                        lblChanges.Text = changes.ToString();
                    }

                }
            }
        }

        private void cboPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPaymentMethod.SelectedIndex == 1)
            {
                lblBank.Visible = true;
                cboBankPayment.Visible = true;
                POSEntities posEntites = new POSEntities();
                var bankList = posEntites.PaymentTypes.Where(x => x.Id >= 501).ToList();
                cboBankPayment.DataSource = bankList;
                cboBankPayment.DisplayMember = "Name";
                cboBankPayment.ValueMember = "Id";
            }
            else
            {
                lblBank.Visible = false;
                cboBankPayment.Visible = false;
                cboBankPayment.DataSource = null;
            }
        }
        #endregion

        #region Key Press and KeyUp

        private void txtRecieveAmt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && char.IsLetter('.'))
            {
                e.Handled = true;
            }
            if (txtRecieveAmt.Text == "0")
            {
                cboPaymentMethod.Enabled = false;
            }
            else
            {
                cboPaymentMethod.Enabled = true;
            }
        }

        private void txtRecieveAmt_KeyUp(object sender, KeyEventArgs e)
        {
            decimal amount = 0;
            decimal.TryParse(txtRecieveAmt.Text, out amount);
            decimal Cost = 0;
            decimal.TryParse(lblTotalCost.Text, out Cost);

            if (txtRecieveAmt.Text != string.Empty)
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

        #endregion

        #region Button Click

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Calculate();
        } 
      
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        #endregion

    }
}