using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using POS.APP_Data;

namespace POS
{
    public partial class Transaction_Reports : Form
    {
        #region Variable

        POSEntities entity = new POSEntities();
        List<Transaction> transList = new List<Transaction>();
        List<Transaction> RtransList = new List<Transaction>();
        List<Transaction> DtransList = new List<Transaction>();
        List<Transaction> CRtransList = new List<Transaction>();
        List<Transaction> GCtransList = new List<Transaction>();
        List<Transaction> CtransList = new List<Transaction>();
        private ToolTip tp = new ToolTip();
        
        #endregion
        #region Event

        public Transaction_Reports()
        {
            InitializeComponent();
        }

        private void Transaction_Reports_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            List<APP_Data.Counter> counterList = new List<APP_Data.Counter>();
            APP_Data.Counter counterObj = new APP_Data.Counter();
            counterObj.Id = 0;
            counterObj.Name = "Select";
            counterList.Add(counterObj);
            counterList.AddRange((from c in entity.Counters orderby c.Id select c).ToList());
            cboCounter.DataSource = counterList;
            cboCounter.DisplayMember = "Name";
            cboCounter.ValueMember = "Id";

            List<APP_Data.User> userList = new List<APP_Data.User>();
            APP_Data.User userObj = new APP_Data.User();
            userObj.Id = 0;
            userObj.Name = "Select";
            userList.Add(userObj);
            userList.AddRange((from u in entity.Users orderby u.Id select u).ToList());
            cboCashier.DataSource = userList;
            cboCashier.DisplayMember = "Name";
            cboCashier.ValueMember = "Id";

            this.reportViewer1.RefreshReport();
            LoadData();
            
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadData();

        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void rdbSale_CheckedChanged(object sender, EventArgs e)
        {
            gbPaymentType.Enabled = true;
            LoadData();
        }

        private void rdbRefund_CheckedChanged(object sender, EventArgs e)
        {
            gbPaymentType.Enabled = false;
            LoadData();
        }

        private void rdbSummary_CheckedChanged(object sender, EventArgs e)
        {
            gbPaymentType.Enabled = false;
            LoadData();
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            #region [ Print ]
            if (rdbSummary.Checked == false)
            {
                dsReportTemp dsReport = new dsReportTemp();
                dsReportTemp.TransactionListDataTable dtTransactionReport = (dsReportTemp.TransactionListDataTable)dsReport.Tables["TransactionList"];
                foreach (Transaction transaction in transList)
                {
                    dsReportTemp.TransactionListRow newRow = dtTransactionReport.NewTransactionListRow();
                    newRow.TransactionId = transaction.Id;
                    newRow.Date = Convert.ToDateTime(transaction.DateTime);
                    newRow.SalePerson = transaction.User.Name;
                    newRow.PaymentMethod = transaction.PaymentType.Name;
                    newRow.Amount = Convert.ToInt32(transaction.TotalAmount);
                    newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                    newRow.Type = transaction.Type;
                    dtTransactionReport.AddTransactionListRow(newRow);
                }

                string reportPath = "";
                ReportViewer rv = new ReportViewer();
                ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["TransactionList"]);
                reportPath = Application.StartupPath + "\\Reports\\TransactionReport.rdlc";
                rv.Reset();
                rv.LocalReport.ReportPath = reportPath;
                rv.LocalReport.DataSources.Add(rds);


                ReportParameter TransactionTitle = new ReportParameter("TransactionTitle", gbTransactionList.Text + dtpFrom.Value.Date.ToString("dd-MM-yyyy") + " To " + dtpTo.Value.Date.ToString("dd-MM-yyyy"));
                rv.LocalReport.SetParameters(TransactionTitle);

                PrintDoc.PrintReport(rv);
            }
            else
            {
                int totalSale = 0, totalRefund = 0, totalDebt = 0, totalCreditRefund = 0, totalSummary = 0; int totalGiftCard = 0, totalCredit = 0, totalCreditRecieve = 0, totalCashInHand = 0, totalExpense = 0, totalIncomeAmount = 0;
                #region Transaction
                dsReportTemp dsReport = new dsReportTemp();
                dsReportTemp.TransactionListDataTable dtTransactionReport = (dsReportTemp.TransactionListDataTable)dsReport.Tables["TransactionList"];

                foreach (Transaction transaction in transList)
                {
                    dsReportTemp.TransactionListRow newRow = dtTransactionReport.NewTransactionListRow();
                    newRow.TransactionId = transaction.Id;
                    newRow.Date = Convert.ToDateTime(transaction.DateTime);
                    newRow.SalePerson = transaction.User.Name;
                    newRow.PaymentMethod = transaction.PaymentType.Name;
                    newRow.Amount = Convert.ToInt32(transaction.TotalAmount);
                    newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                    newRow.Type = transaction.Type;
                    totalSale += Convert.ToInt32(transaction.TotalAmount);
                    dtTransactionReport.AddTransactionListRow(newRow);
                }
                ReportDataSource rds1 = new ReportDataSource("SaleDataSet", dsReport.Tables["TransactionList"]);
                #endregion
                #region Refund
                dsReportTemp dsReportRefund = new dsReportTemp();
                dsReportTemp.TransactionListDataTable dtTransactionReportRefund = (dsReportTemp.TransactionListDataTable)dsReportRefund.Tables["TransactionList"];
                foreach (Transaction transaction in RtransList)
                {
                    dsReportTemp.TransactionListRow newRow = dtTransactionReportRefund.NewTransactionListRow();
                    newRow.TransactionId = transaction.Id;
                    newRow.Date = Convert.ToDateTime(transaction.DateTime);
                    newRow.SalePerson = transaction.User.Name;
                    newRow.PaymentMethod = transaction.PaymentType.Name;
                    newRow.Amount = Convert.ToInt32(transaction.TotalAmount);
                    newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                    newRow.Type = transaction.Type;
                    totalRefund += Convert.ToInt32(transaction.TotalAmount);
                    dtTransactionReportRefund.AddTransactionListRow(newRow);
                }
                ReportDataSource rds2 = new ReportDataSource("RefundDataSet", dsReportRefund.Tables["TransactionList"]);
                #endregion
                #region Debt
                dsReportTemp dsReportDebt = new dsReportTemp();
                dsReportTemp.TransactionListDataTable dtTransactionReportDebt = (dsReportTemp.TransactionListDataTable)dsReportDebt.Tables["TransactionList"];
                foreach (Transaction transaction in DtransList)
                {
                    dsReportTemp.TransactionListRow newRow = dtTransactionReportDebt.NewTransactionListRow();
                    newRow.TransactionId = transaction.Id;
                    newRow.Date = Convert.ToDateTime(transaction.DateTime);
                    newRow.SalePerson = transaction.User.Name;
                    newRow.PaymentMethod = transaction.PaymentType.Name;
                    newRow.Amount = Convert.ToInt32(transaction.TotalAmount);
                    newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                    newRow.Type = transaction.Type;
                    totalDebt += Convert.ToInt32(transaction.TotalAmount);
                    dtTransactionReportDebt.AddTransactionListRow(newRow);
                }
                ReportDataSource rds3 = new ReportDataSource("DebtDataSet", dsReportDebt.Tables["TransactionList"]);
                #endregion
                #region CreditRefund
                dsReportTemp dsReportCreditRefund = new dsReportTemp();
                dsReportTemp.TransactionListDataTable dtTransactionReportCreditRefund = (dsReportTemp.TransactionListDataTable)dsReportCreditRefund.Tables["TransactionList"];
                foreach (Transaction transaction in CRtransList)
                {
                    dsReportTemp.TransactionListRow newRow = dtTransactionReportCreditRefund.NewTransactionListRow();
                    newRow.TransactionId = transaction.Id;
                    newRow.Date = Convert.ToDateTime(transaction.DateTime);
                    newRow.SalePerson = transaction.User.Name;
                    newRow.PaymentMethod = transaction.PaymentType.Name;
                    newRow.Amount = Convert.ToInt32(transaction.TotalAmount);
                    newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                    newRow.Type = transaction.Type;
                    totalCreditRefund += Convert.ToInt32(transaction.TotalAmount);
                    dtTransactionReportCreditRefund.AddTransactionListRow(newRow);
                }
                ReportDataSource rds4 = new ReportDataSource("CreditRefundDataSet", dsReportCreditRefund.Tables["TransactionList"]);
                #endregion
                #region GiftCard
                dsReportTemp dsReportGiftCardTransaction = new dsReportTemp();
                dsReportTemp.TransactionListDataTable dtTransactionReportGiftCard = (dsReportTemp.TransactionListDataTable)dsReportGiftCardTransaction.Tables["TransactionList"];
                foreach (Transaction transaction in GCtransList)
                {
                    dsReportTemp.TransactionListRow newRow = dtTransactionReportGiftCard.NewTransactionListRow();
                    newRow.TransactionId = transaction.Id;
                    newRow.Date = Convert.ToDateTime(transaction.DateTime);
                    newRow.SalePerson = transaction.User.Name;
                    newRow.PaymentMethod = transaction.PaymentType.Name;
                    newRow.Amount = Convert.ToInt32(transaction.TotalAmount);
                    newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                    newRow.Type = transaction.Type;
                    totalGiftCard += Convert.ToInt32(transaction.TotalAmount);
                    dtTransactionReportGiftCard.AddTransactionListRow(newRow);
                }
                ReportDataSource rds5 = new ReportDataSource("GiftCardDataSet", dsReportGiftCardTransaction.Tables["TransactionList"]);
                #endregion
                #region Credit
                dsReportTemp dsReportCreditTransaction = new dsReportTemp();
                dsReportTemp.TransactionListDataTable dtTransactionReportCredit = (dsReportTemp.TransactionListDataTable)dsReportCreditTransaction.Tables["TransactionList"];
                foreach (Transaction transaction in CtransList)
                {
                    dsReportTemp.TransactionListRow newRow = dtTransactionReportCredit.NewTransactionListRow();
                    newRow.TransactionId = transaction.Id;
                    newRow.Date = Convert.ToDateTime(transaction.DateTime);
                    newRow.SalePerson = transaction.User.Name;
                    newRow.PaymentMethod = transaction.PaymentType.Name;
                    newRow.Amount = Convert.ToInt32(transaction.TotalAmount);
                    newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                    newRow.Type = transaction.Type;
                    totalCredit += Convert.ToInt32(transaction.TotalAmount);
                    totalCreditRecieve += Convert.ToInt32(transaction.RecieveAmount);
                    dtTransactionReportCredit.AddTransactionListRow(newRow);
                }
                ReportDataSource rds6 = new ReportDataSource("CreditDataSet", dsReportCreditTransaction.Tables["TransactionList"]);
                #endregion
                //totalSummary = (totalSale + totalDebt + totalCreditRefund + totalGiftCard) - totalRefund;
                totalSummary = ((totalSale + totalDebt + totalCredit + totalGiftCard) - (totalRefund + totalCreditRecieve));
                totalCashInHand = (totalSale + totalDebt + totalGiftCard + totalCreditRecieve) - totalRefund;
                totalExpense = (totalRefund - totalCreditRefund);
                totalIncomeAmount = (totalSale + totalDebt + totalCredit + totalGiftCard);
                ReportViewer rv = new ReportViewer();
                string reportPath = Application.StartupPath + "\\Reports\\TransactionSummaryReport.rdlc";
                rv.Reset();
                rv.LocalReport.ReportPath = reportPath;
                rv.LocalReport.DataSources.Clear();
                rv.LocalReport.DataSources.Add(rds1);
                rv.LocalReport.DataSources.Add(rds2);
                rv.LocalReport.DataSources.Add(rds3);
                rv.LocalReport.DataSources.Add(rds4);
                rv.LocalReport.DataSources.Add(rds5);
                rv.LocalReport.DataSources.Add(rds6);

                ReportParameter TotalSale = new ReportParameter("TotalSale", totalSale.ToString());
                rv.LocalReport.SetParameters(TotalSale);

                ReportParameter CreditRecieve = new ReportParameter("CreditRecieve", totalCreditRecieve.ToString());
                rv.LocalReport.SetParameters(CreditRecieve);

                ReportParameter Expense = new ReportParameter("Expense", totalExpense.ToString());
                rv.LocalReport.SetParameters(Expense);

                ReportParameter IncomeAmount = new ReportParameter("IncomeAmount", totalIncomeAmount.ToString());
                rv.LocalReport.SetParameters(IncomeAmount);

                ReportParameter CashInHand = new ReportParameter("CashInHand", totalCashInHand.ToString());
                rv.LocalReport.SetParameters(CashInHand);

                ReportParameter TotalDebt = new ReportParameter("TotalDebt", totalDebt.ToString());
                rv.LocalReport.SetParameters(TotalDebt);

                ReportParameter TotalRefund = new ReportParameter("TotalRefund", totalRefund.ToString());
                rv.LocalReport.SetParameters(TotalRefund);

                ReportParameter TotalSummary = new ReportParameter("TotalSummary", totalSummary.ToString());
                rv.LocalReport.SetParameters(TotalSummary);

                ReportParameter TotalCreditRefund = new ReportParameter("TotalCreditRefund", totalCreditRefund.ToString());
                rv.LocalReport.SetParameters(TotalCreditRefund);

                ReportParameter TotalGiftCard = new ReportParameter("TotalGiftCard", totalGiftCard.ToString());
                rv.LocalReport.SetParameters(TotalGiftCard);

                ReportParameter TotalCredit = new ReportParameter("TotalCredit", totalCredit.ToString());
                rv.LocalReport.SetParameters(TotalCredit);

                ReportParameter HeaderTitle = new ReportParameter("HeaderTitle", "Transaction Summary for " + dtpFrom.Value.Date.ToString("dd/MM/yyyy") + " To " + dtpTo.Value.Date.ToString("dd/MM/yyyy"));
                rv.LocalReport.SetParameters(HeaderTitle);

                PrintDoc.PrintReport(rv);
            }
            #endregion
        }       

        private void chkCashier_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCashier.Checked)
            {
                lblCashierName.Enabled = true;
                cboCashier.Enabled = true;
            }
            else
            {
                lblCashierName.Enabled = false;
                cboCashier.Enabled = false;
                LoadData();
            }
            
        }

        private void chkCounter_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCounter.Checked)
            {
                lblCounterName.Enabled = true;
                cboCounter.Enabled = true;
            }
            else
            {
                lblCounterName.Enabled = false;
                cboCounter.Enabled = false;
                LoadData();
            }
            
        }

        private void cboCashier_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cboCounter_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cboCashier_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cboCounter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void rdbDebt_CheckedChanged(object sender, EventArgs e)
        {
            gbPaymentType.Enabled = false;
            LoadData();
        }

        private void chkCash_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void chkGiftCard_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void chkCredit_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }
        #endregion    
        

        #region Function

        private void LoadData()
        {
            DateTime fromDate = dtpFrom.Value.Date;
            DateTime toDate = dtpTo.Value.Date;
            bool IsSale = rdbSale.Checked;
            bool IsRefund = rdbRefund.Checked;
            bool IsDebt = rdbDebt.Checked;
            bool IsCounter = chkCounter.Checked;
            bool IsCashier = chkCashier.Checked;
            bool IsCredit = chkCredit.Checked;
            bool IsCash = chkCash.Checked;
            bool IsGiftCard = chkGiftCard.Checked;
            bool IsSummary = rdbSummary.Checked;

            int CashierId = 0;
            int CounterId = 0;

            Boolean hasError = false;

            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            //Validation
            if (IsCounter)
            {
                if (cboCounter.SelectedIndex == 0)
                {
                    tp.SetToolTip(cboCounter, "Error");
                    tp.Show("Please select counter name!", cboCounter);
                    hasError = true;
                }
            }
            else if (IsCashier)
            {
                if (cboCashier.SelectedIndex == 0)
                {
                    tp.SetToolTip(cboCashier, "Error");
                    tp.Show("Please select counter name!", cboCashier);
                    hasError = true;
                }
            }
            if (!hasError)
            {
                if (cboCounter.SelectedIndex > 0)
                {
                    CounterId = Convert.ToInt32(cboCounter.SelectedValue);
                }
                if (cboCashier.SelectedIndex > 0)
                {
                    CashierId = Convert.ToInt32(cboCashier.SelectedValue);
                }
                #region get transaction with cashier & counter
                if (IsCashier == true && IsCounter == true)
                {                   
                    if (IsSale)
                    {
                        if (IsCredit == true && IsCash == true && IsGiftCard == true)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId select t).ToList<Transaction>();
                        }
                        else if (IsCredit == true && IsCash == true && IsGiftCard == false)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && t.PaymentTypeId != 3 select t).ToList<Transaction>();
                        }
                        else if (IsCredit == false && IsCash == true && IsGiftCard == true)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && t.PaymentTypeId != 2 select t).ToList<Transaction>();
                        }
                        else if (IsCredit == true && IsCash == false && IsGiftCard == true)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && t.PaymentTypeId != 1 select t).ToList<Transaction>();
                        }
                        else if (IsCredit == true && IsCash == false && IsGiftCard == false)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && t.PaymentTypeId == 2 select t).ToList<Transaction>();
                        }
                        else if (IsCredit == false && IsCash == true && IsGiftCard == false)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && t.PaymentTypeId == 1 select t).ToList<Transaction>();
                        }
                        else if (IsCredit == false && IsCash == false && IsGiftCard == true)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && t.PaymentTypeId == 3 select t).ToList<Transaction>();
                        }
                        else if (IsCredit == false && IsCash == false && IsGiftCard == false)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && t.PaymentTypeId != 1 && t.PaymentTypeId != 2 && t.PaymentTypeId != 3 select t).ToList<Transaction>();
                        }
                        
                        ShowReportViewer();                     
                        lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                        // lblNumberofTransaction.Text = transList.Count.ToString();
                        gbTransactionList.Text = "Sale Transaction Report for ";
                        // lblTotalAmount.Text = "";
                    }
                    else if(IsRefund)
                    {
                        transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Refund || t.Type == TransactionType.CreditRefund) && t.CounterId == CounterId && t.UserId == CashierId select t).ToList<Transaction>();
                        ShowReportViewer();
                        lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                        // lblNumberofTransaction.Text = transList.Count.ToString();
                        gbTransactionList.Text = "Refund Transaction Report for ";
                        //lblTotalAmount.Text = "";
                    }
                    else if (IsDebt)
                    {
                        transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Prepaid || t.Type == TransactionType.Settlement) select t).ToList<Transaction>();
                        
                        ShowReportViewer();
                        lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                        // lblNumberofTransaction.Text = transList.Count.ToString();
                        gbTransactionList.Text = "Debt Transaction Report for ";
                        //lblTotalAmount.Text = "";
                    }

                    else if (IsSummary)
                    {
                        transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 1 && t.CounterId == CounterId && t.UserId == CashierId select t).ToList<Transaction>();
                        RtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Refund && t.CounterId == CounterId && t.UserId == CashierId select t).ToList<Transaction>();
                        DtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Settlement || t.Type == TransactionType.Prepaid) && t.CounterId == CounterId && t.UserId == CashierId select t).ToList<Transaction>();
                        CRtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.CreditRefund && t.CounterId == CounterId && t.UserId == CashierId select t).ToList<Transaction>();
                        GCtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 3 && t.CounterId == CounterId && t.UserId == CashierId select t).ToList<Transaction>();
                        CtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Credit && t.CounterId == CounterId && t.UserId == CashierId select t).ToList<Transaction>();
                        ShowReportViewer1();
                        lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                        // lblNumberofTransaction.Text = transList.Count.ToString();
                        gbTransactionList.Text = "Transaction Summary Report for ";
                        //lblTotalAmount.Text = "";
                    }
                }
                #endregion
                #region get transaction with cashier only
                else if (IsCashier == true && IsCounter == false)
                {
                    if (IsSale)
                    {
                        if (IsCredit == true && IsCash == true && IsGiftCard == true)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId select t).ToList<Transaction>();
                        }
                        else if (IsCredit == true && IsCash == true && IsGiftCard == false)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && t.PaymentTypeId != 3 select t).ToList<Transaction>();
                        }
                        else if (IsCredit == false && IsCash == true && IsGiftCard == true)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && t.PaymentTypeId != 2 select t).ToList<Transaction>();
                        }
                        else if (IsCredit == true && IsCash == false && IsGiftCard == true)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && t.PaymentTypeId != 1 select t).ToList<Transaction>();
                        }
                        else if (IsCredit == true && IsCash == false && IsGiftCard == false)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && t.PaymentTypeId == 2 select t).ToList<Transaction>();
                        }
                        else if (IsCredit == false && IsCash == true && IsGiftCard == false)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && t.PaymentTypeId == 1 select t).ToList<Transaction>();
                        }
                        else if (IsCredit == false && IsCash == false && IsGiftCard == true)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && t.PaymentTypeId == 3 select t).ToList<Transaction>();
                        }
                        else if (IsCredit == false && IsCash == false && IsGiftCard == false)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && t.PaymentTypeId != 2 && t.PaymentTypeId != 1 && t.PaymentTypeId != 3 select t).ToList<Transaction>();
                        }
                        
                        ShowReportViewer();
                        lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                        // lblNumberofTransaction.Text = transList.Count.ToString();
                        gbTransactionList.Text = "Sale Transaction Report for ";
                        // lblTotalAmount.Text = "";
                    }
                    else if(IsRefund)
                    {
                        transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Refund || t.Type == TransactionType.CreditRefund) && t.UserId == CashierId select t).ToList<Transaction>();
                        ShowReportViewer();
                        lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                        // lblNumberofTransaction.Text = transList.Count.ToString();
                        gbTransactionList.Text = "Refund Transaction Report for ";
                        //lblTotalAmount.Text = "";
                    }
                    else if (IsDebt)
                    {
                        transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Prepaid || t.Type == TransactionType.Settlement) && t.UserId == CashierId select t).ToList<Transaction>();
                        ShowReportViewer();
                        lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                        // lblNumberofTransaction.Text = transList.Count.ToString();
                        gbTransactionList.Text = "Debt Transaction Report for ";
                        //lblTotalAmount.Text = "";
                    }
                    else if (IsSummary)
                    {
                        transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 1 && t.UserId == CashierId select t).ToList<Transaction>();
                        RtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Refund && t.UserId == CashierId select t).ToList<Transaction>();
                        DtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Settlement || t.Type == TransactionType.Prepaid) && t.UserId == CashierId select t).ToList<Transaction>();
                        CRtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.CreditRefund && t.UserId == CashierId select t).ToList<Transaction>();
                        GCtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 3 && t.UserId == CashierId select t).ToList<Transaction>();
                        CtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Credit && t.UserId == CashierId select t).ToList<Transaction>();
                        ShowReportViewer1();
                        lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                        // lblNumberofTransaction.Text = transList.Count.ToString();
                        gbTransactionList.Text = "Transaction Summary Report for ";
                        //lblTotalAmount.Text = "";
                    }
                }
                #endregion
                #region get all transactions with counter only
                else if (IsCashier == false && IsCounter == true)
                {
                    if (IsSale)
                    {
                        #region Payment
                        if (IsCredit == true && IsCash == true && IsGiftCard == true)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId select t).ToList<Transaction>();
                        }
                        else if (IsCredit == true && IsCash == true && IsGiftCard == false)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.PaymentTypeId != 3 select t).ToList<Transaction>();
                        }
                        else if (IsCredit == false && IsCash == true && IsGiftCard == true)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.PaymentTypeId != 2 select t).ToList<Transaction>();
                        }
                        else if (IsCredit == true && IsCash == false && IsGiftCard == true)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.PaymentTypeId != 1 select t).ToList<Transaction>();
                        }
                        else if (IsCredit == true && IsCash == false && IsGiftCard == false)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.PaymentTypeId == 2 select t).ToList<Transaction>();
                        }
                        else if (IsCredit == false && IsCash == true && IsGiftCard == false)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.PaymentTypeId == 1 select t).ToList<Transaction>();
                        }
                        else if (IsCredit == false && IsCash == false && IsGiftCard == true)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.PaymentTypeId == 3 select t).ToList<Transaction>();
                        }
                        else if (IsCredit == false && IsCash == false && IsGiftCard == false)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.PaymentTypeId != 1 && t.PaymentTypeId != 2 && t.PaymentTypeId != 3 select t).ToList<Transaction>();
                        }
                        #endregion
                        ShowReportViewer();
                        lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                        // lblNumberofTransaction.Text = transList.Count.ToString();
                        gbTransactionList.Text = "Sale Transaction Report for ";
                        // lblTotalAmount.Text = "";
                    }
                    else if(IsRefund)
                    {
                        transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Refund || t.Type == TransactionType.CreditRefund) && t.CounterId == CounterId select t).ToList<Transaction>();
                        ShowReportViewer();
                        lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                        // lblNumberofTransaction.Text = transList.Count.ToString();
                        gbTransactionList.Text = "Refund Transaction Report for ";
                        //lblTotalAmount.Text = "";
                    }
                    else if (IsDebt)
                    {
                        transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Prepaid || t.Type == TransactionType.Settlement) && t.CounterId == CounterId select t).ToList<Transaction>();
                        ShowReportViewer();
                        lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                        // lblNumberofTransaction.Text = transList.Count.ToString();
                        gbTransactionList.Text = "Debt Transaction Report for ";
                        //lblTotalAmount.Text = "";
                    }
                    else if (IsSummary)
                    {
                        transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 1 && t.CounterId == CounterId select t).ToList<Transaction>();
                        RtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Refund && t.CounterId == CounterId select t).ToList<Transaction>();
                        DtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Settlement || t.Type == TransactionType.Prepaid) && t.CounterId == CounterId select t).ToList<Transaction>();
                        CRtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.CreditRefund && t.CounterId == CounterId select t).ToList<Transaction>();
                        GCtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 3 && t.CounterId == CounterId select t).ToList<Transaction>();
                        CtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Credit && t.CounterId == CounterId select t).ToList<Transaction>();
                        ShowReportViewer1();
                        lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                        // lblNumberofTransaction.Text = transList.Count.ToString();
                        gbTransactionList.Text = "Transaction Summary Report";
                        //lblTotalAmount.Text = "";
                    }
                }
                #endregion
                #region get all transactions
                else
                {
                    if (IsSale)
                    {
                        if (IsCredit == true && IsCash == true && IsGiftCard == true)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) select t).ToList<Transaction>();
                        }
                        else if (IsCredit == true && IsCash == true && IsGiftCard == false)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.PaymentTypeId != 3 select t).ToList<Transaction>();
                        }
                        else if (IsCredit == false && IsCash == true && IsGiftCard == true)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.PaymentTypeId != 2 select t).ToList<Transaction>();
                        }
                        else if (IsCredit == true && IsCash == false && IsGiftCard == true)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.PaymentTypeId != 1 select t).ToList<Transaction>();
                        }
                        else if (IsCredit == true && IsCash == false && IsGiftCard == false)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.PaymentTypeId == 2 select t).ToList<Transaction>();
                        }
                        else if (IsCredit == false && IsCash == true && IsGiftCard == false)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.PaymentTypeId == 1 select t).ToList<Transaction>();
                        }
                        else if (IsCredit == false && IsCash == false && IsGiftCard == true)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.PaymentTypeId == 3 select t).ToList<Transaction>();
                        }
                        else if (IsCredit == false && IsCash == false && IsGiftCard == false)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.PaymentTypeId != 1 && t.PaymentTypeId != 2 && t.PaymentTypeId != 3 select t).ToList<Transaction>();
                        }
                        
                        ShowReportViewer();
                        lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                        // lblNumberofTransaction.Text = transList.Count.ToString();
                        gbTransactionList.Text = "Sale Transaction Report for ";
                        // lblTotalAmount.Text = "";
                    }
                    else if(IsRefund)
                    {
                        transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Refund || t.Type == TransactionType.CreditRefund) select t).ToList<Transaction>();
                        ShowReportViewer();
                        lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                        // lblNumberofTransaction.Text = transList.Count.ToString();
                        gbTransactionList.Text = "Refund Transaction Report for ";
                        //lblTotalAmount.Text = "";
                    }
                    else if (IsDebt)
                    {
                        transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Prepaid || t.Type == TransactionType.Settlement) select t).ToList<Transaction>();
                        ShowReportViewer();
                        lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                        // lblNumberofTransaction.Text = transList.Count.ToString();
                        gbTransactionList.Text = "Debt Transaction Report for ";
                        //lblTotalAmount.Text = "";
                    }
                    else if (IsSummary)
                    {
                        transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 1 select t).ToList<Transaction>();
                        RtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Refund select t).ToList<Transaction>();
                        DtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Settlement || t.Type == TransactionType.Prepaid) select t).ToList<Transaction>();
                        CRtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.CreditRefund select t).ToList<Transaction>();
                        GCtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 3 select t).ToList<Transaction>();
                        CtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Credit select t).ToList<Transaction>();
                        ShowReportViewer1();
                        lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                        // lblNumberofTransaction.Text = transList.Count.ToString();
                        gbTransactionList.Text = "Transaction Summary Report";
                        //lblTotalAmount.Text = "";
                    }
                }
                #endregion
            }
        }

        private void ShowReportViewer()
        {
            dsReportTemp dsReport = new dsReportTemp();
            dsReportTemp.TransactionListDataTable dtTransactionReport = (dsReportTemp.TransactionListDataTable)dsReport.Tables["TransactionList"];
            foreach (Transaction transaction in transList)
            {
                dsReportTemp.TransactionListRow newRow = dtTransactionReport.NewTransactionListRow();
                newRow.TransactionId = transaction.Id;
                newRow.Date = Convert.ToDateTime(transaction.DateTime);
                newRow.SalePerson = transaction.User.Name;
                newRow.PaymentMethod = transaction.PaymentType.Name;
                newRow.Amount = Convert.ToInt32(transaction.TotalAmount);
                newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                newRow.Type = transaction.Type;
                dtTransactionReport.AddTransactionListRow(newRow);
            }

            ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["TransactionList"]);
            string reportPath = Application.StartupPath + "\\Reports\\TransactionReport.rdlc";
            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            ReportParameter TransactionTitle = new ReportParameter("TransactionTitle", gbTransactionList.Text + dtpFrom.Value.Date.ToString("dd-MM-yyyy")+ " To " + dtpTo.Value.Date.ToString("dd-MM-yyyy"));
            reportViewer1.LocalReport.SetParameters(TransactionTitle);
            reportViewer1.RefreshReport();
        }

        private void ShowReportViewer1()
        {
            int totalSale = 0, totalRefund = 0, totalDebt = 0, totalCreditRefund = 0, totalSummary = 0; int totalGiftCard = 0, totalCredit = 0, totalCreditRecieve = 0, totalCashInHand = 0, totalExpense = 0, totalIncomeAmount = 0;
            #region Transaction 
            dsReportTemp dsReport = new dsReportTemp();
            dsReportTemp.TransactionListDataTable dtTransactionReport = (dsReportTemp.TransactionListDataTable)dsReport.Tables["TransactionList"];
            
            foreach (Transaction transaction in transList)
            {
                dsReportTemp.TransactionListRow newRow = dtTransactionReport.NewTransactionListRow();
                newRow.TransactionId = transaction.Id;
                newRow.Date = Convert.ToDateTime(transaction.DateTime);
                newRow.SalePerson = transaction.User.Name;
                newRow.PaymentMethod = transaction.PaymentType.Name;
                newRow.Amount = Convert.ToInt32(transaction.TotalAmount);
                newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                newRow.Type = transaction.Type;
                totalSale += Convert.ToInt32(transaction.TotalAmount);
                dtTransactionReport.AddTransactionListRow(newRow);
            }            
            ReportDataSource rds1 = new ReportDataSource("SaleDataSet", dsReport.Tables["TransactionList"]);
            #endregion
            #region Refund
            dsReportTemp dsReportRefund = new dsReportTemp();
            dsReportTemp.TransactionListDataTable dtTransactionReportRefund = (dsReportTemp.TransactionListDataTable)dsReportRefund.Tables["TransactionList"];
            foreach (Transaction transaction in RtransList)
            {
                dsReportTemp.TransactionListRow newRow = dtTransactionReportRefund.NewTransactionListRow();
                newRow.TransactionId = transaction.Id;
                newRow.Date = Convert.ToDateTime(transaction.DateTime);
                newRow.SalePerson = transaction.User.Name;
                newRow.PaymentMethod = transaction.PaymentType.Name;
                newRow.Amount = Convert.ToInt32(transaction.TotalAmount);
                newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                newRow.Type = transaction.Type;
                totalRefund += Convert.ToInt32(transaction.TotalAmount);
                dtTransactionReportRefund.AddTransactionListRow(newRow);
            }
            ReportDataSource rds2 = new ReportDataSource("RefundDataSet", dsReportRefund.Tables["TransactionList"]);
            #endregion
            #region Debt
            dsReportTemp dsReportDebt = new dsReportTemp();
            dsReportTemp.TransactionListDataTable dtTransactionReportDebt = (dsReportTemp.TransactionListDataTable)dsReportDebt.Tables["TransactionList"];
            foreach (Transaction transaction in DtransList)
            {
                if (transaction.Type == TransactionType.Settlement)
                {
                    dsReportTemp.TransactionListRow newRow = dtTransactionReportDebt.NewTransactionListRow();
                    newRow.TransactionId = transaction.Id;
                    newRow.Date = Convert.ToDateTime(transaction.DateTime);
                    newRow.SalePerson = transaction.User.Name;
                    newRow.PaymentMethod = transaction.PaymentType.Name;
                    newRow.Amount = Convert.ToInt32(transaction.TotalAmount);
                    newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                    newRow.Type = transaction.Type;
                    totalDebt += Convert.ToInt32(transaction.TotalAmount);
                    dtTransactionReportDebt.AddTransactionListRow(newRow);
                }
                else // if prepaid
                {
                    long useprepaid = transaction.UsePrePaidDebts1 == null ? 0 : transaction.UsePrePaidDebts1.Sum(x => x.UseAmount).Value;
                    dsReportTemp.TransactionListRow newRow = dtTransactionReportDebt.NewTransactionListRow();
                    newRow.TransactionId = transaction.Id;
                    newRow.Date = Convert.ToDateTime(transaction.DateTime);
                    newRow.SalePerson = transaction.User.Name;
                    newRow.PaymentMethod = transaction.PaymentType.Name;
                    newRow.Amount = Convert.ToInt32(transaction.TotalAmount - useprepaid);
                    newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                    newRow.Type = transaction.Type;
                    totalDebt += Convert.ToInt32(transaction.TotalAmount - useprepaid);
                    dtTransactionReportDebt.AddTransactionListRow(newRow);
                }
                
               
            }
            ReportDataSource rds3 = new ReportDataSource("DebtDataSet", dsReportDebt.Tables["TransactionList"]);
            #endregion
            #region CreditRefund
            dsReportTemp dsReportCreditRefund = new dsReportTemp();
            dsReportTemp.TransactionListDataTable dtTransactionReportCreditRefund = (dsReportTemp.TransactionListDataTable)dsReportCreditRefund.Tables["TransactionList"];
            foreach (Transaction transaction in CRtransList)
            {
                dsReportTemp.TransactionListRow newRow = dtTransactionReportCreditRefund.NewTransactionListRow();
                newRow.TransactionId = transaction.Id;
                newRow.Date = Convert.ToDateTime(transaction.DateTime);
                newRow.SalePerson = transaction.User.Name;
                newRow.PaymentMethod = transaction.PaymentType.Name;
                newRow.Amount = Convert.ToInt32(transaction.TotalAmount);
                newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                newRow.Type = transaction.Type;
                totalCreditRefund += Convert.ToInt32(transaction.TotalAmount);
                dtTransactionReportCreditRefund.AddTransactionListRow(newRow);
            }
            ReportDataSource rds4 = new ReportDataSource("CreditRefundDataSet", dsReportCreditRefund.Tables["TransactionList"]);
            #endregion
            #region GiftCard
            dsReportTemp dsReportGiftCardTransaction = new dsReportTemp();
            dsReportTemp.TransactionListDataTable dtTransactionReportGiftCard = (dsReportTemp.TransactionListDataTable)dsReportGiftCardTransaction.Tables["TransactionList"];
            foreach (Transaction transaction in GCtransList)
            {
                dsReportTemp.TransactionListRow newRow = dtTransactionReportGiftCard.NewTransactionListRow();
                newRow.TransactionId = transaction.Id;
                newRow.Date = Convert.ToDateTime(transaction.DateTime);
                newRow.SalePerson = transaction.User.Name;
                newRow.PaymentMethod = transaction.PaymentType.Name;
                newRow.Amount = Convert.ToInt32(transaction.TotalAmount);
                newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                newRow.Type = transaction.Type;
                totalGiftCard += Convert.ToInt32(transaction.TotalAmount);
                dtTransactionReportGiftCard.AddTransactionListRow(newRow);
            }
            ReportDataSource rds5 = new ReportDataSource("GiftCardDataSet", dsReportGiftCardTransaction.Tables["TransactionList"]);
            #endregion
            #region Credit
            dsReportTemp dsReportCreditTransaction = new dsReportTemp();
            dsReportTemp.TransactionListDataTable dtTransactionReportCredit = (dsReportTemp.TransactionListDataTable)dsReportCreditTransaction.Tables["TransactionList"];
            foreach (Transaction transaction in CtransList)
            {
                dsReportTemp.TransactionListRow newRow = dtTransactionReportCredit.NewTransactionListRow();
                newRow.TransactionId = transaction.Id;
                newRow.Date = Convert.ToDateTime(transaction.DateTime);
                newRow.SalePerson = transaction.User.Name;
                newRow.PaymentMethod = transaction.PaymentType.Name;
                newRow.Amount = Convert.ToInt32(transaction.TotalAmount);
                newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                newRow.Type = transaction.Type;
                totalCredit += Convert.ToInt32(transaction.TotalAmount);
                totalCreditRecieve += Convert.ToInt32(transaction.RecieveAmount);
                dtTransactionReportCredit.AddTransactionListRow(newRow);
            }
            ReportDataSource rds6 = new ReportDataSource("CreditDataSet", dsReportCreditTransaction.Tables["TransactionList"]);
            #endregion
            //totalSummary = (totalSale + totalDebt + totalCreditRefund + totalGiftCard) - totalRefund;
            totalSummary = ((totalSale + totalDebt + totalCredit + totalGiftCard) - (totalRefund + totalCreditRecieve));
            totalCashInHand = (totalSale + totalDebt + totalGiftCard + totalCreditRecieve) - totalRefund;
            totalExpense = (totalRefund - totalCreditRefund);
            totalIncomeAmount = (totalSale + totalDebt + totalCredit+ totalGiftCard);
            string reportPath = Application.StartupPath + "\\Reports\\TransactionSummaryReport.rdlc";
            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds1);
            reportViewer1.LocalReport.DataSources.Add(rds2);
            reportViewer1.LocalReport.DataSources.Add(rds3);
            reportViewer1.LocalReport.DataSources.Add(rds4);
            reportViewer1.LocalReport.DataSources.Add(rds5);
            reportViewer1.LocalReport.DataSources.Add(rds6);

            ReportParameter TotalSale = new ReportParameter("TotalSale", totalSale.ToString());
            reportViewer1.LocalReport.SetParameters(TotalSale);

            ReportParameter CreditRecieve = new ReportParameter("CreditRecieve", totalCreditRecieve.ToString());
            reportViewer1.LocalReport.SetParameters(CreditRecieve);

            ReportParameter Expense = new ReportParameter("Expense", totalExpense.ToString());
            reportViewer1.LocalReport.SetParameters(Expense);

            ReportParameter IncomeAmount = new ReportParameter("IncomeAmount", totalIncomeAmount.ToString());
            reportViewer1.LocalReport.SetParameters(IncomeAmount);

            ReportParameter CashInHand = new ReportParameter("CashInHand", totalCashInHand.ToString());
            reportViewer1.LocalReport.SetParameters(CashInHand);

            ReportParameter TotalDebt = new ReportParameter("TotalDebt", totalDebt.ToString());
            reportViewer1.LocalReport.SetParameters(TotalDebt);

            ReportParameter TotalRefund = new ReportParameter("TotalRefund", totalRefund.ToString());
            reportViewer1.LocalReport.SetParameters(TotalRefund);

            ReportParameter TotalSummary = new ReportParameter("TotalSummary", totalSummary.ToString());
            reportViewer1.LocalReport.SetParameters(TotalSummary);

            ReportParameter TotalCreditRefund = new ReportParameter("TotalCreditRefund", totalCreditRefund.ToString());
            reportViewer1.LocalReport.SetParameters(TotalCreditRefund);

            ReportParameter TotalGiftCard = new ReportParameter("TotalGiftCard", totalGiftCard.ToString());
            reportViewer1.LocalReport.SetParameters(TotalGiftCard);

            ReportParameter TotalCredit = new ReportParameter("TotalCredit", totalCredit.ToString());
            reportViewer1.LocalReport.SetParameters(TotalCredit);

            ReportParameter HeaderTitle = new ReportParameter("HeaderTitle", "Transaction Summary for " + dtpFrom.Value.Date.ToString("dd-MM-yyyy") + " To " + dtpTo.Value.Date.ToString("dd-MM-yyyy"));
            reportViewer1.LocalReport.SetParameters(HeaderTitle);

            reportViewer1.RefreshReport();
        }

        
        #endregion

       

       
        

       

       

        









    }
}
