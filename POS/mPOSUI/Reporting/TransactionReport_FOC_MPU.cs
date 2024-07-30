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

namespace POS
{
    public partial class TransactionReport_FOC_MPU : Form
    {
        #region Variable

        POSEntities entity = new POSEntities();
        List<Transaction> transList = new List<Transaction>();
        List<Transaction> RtransList = new List<Transaction>();
        List<Transaction> DtransList = new List<Transaction>();
        List<Transaction> CRtransList = new List<Transaction>();
        List<Transaction> GCtransList = new List<Transaction>();
        List<Transaction> CtransList = new List<Transaction>();
        List<Transaction> MPUtransList = new List<Transaction>();
        List<Transaction> FOCtrnsList = new List<Transaction>();
        private ToolTip tp = new ToolTip();
        List<Transaction> TesterList = new List<Transaction>();
        Boolean Isstart = false;
        bool isBankTransfer, isMPU, isMaster, isJCB, isVisa, isUnionPay, isCBPay, isKBZPay, isOnePay, isSaiPay, isAYAPay, isWavePay = false;



        #endregion

        #region Event
        public TransactionReport_FOC_MPU()
        {
            InitializeComponent();
        }

        private void TransactionReport_FOC_MPU_Load(object sender, EventArgs e)
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

            Utility.BindShop(cboshoplist, true);
            cboshoplist.Text = SettingController.DefaultShop.ShopName;
            Utility.ShopComBo_EnableOrNot(cboshoplist);
            Isstart = true;
            cboshoplist.SelectedIndex = 0;
            this.reportViewer1.RefreshReport();
            LoadData();
            gbPaymentType.Enabled = true;
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
            chkCash.Enabled = true;
            chkGiftCard.Enabled = true;
            chkFOC.Enabled = true;
            chkCredit.Enabled = true;
            chkTester.Enabled = true;
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
                cboCashier.SelectedIndex = 0;
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
                cboCounter.SelectedIndex = 0;
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
            gbPaymentType.Enabled = true;
            chkCash.Enabled = true;
            chkGiftCard.Enabled = false;
            chkFOC.Enabled = false;
            chkCredit.Enabled = false;
            chkTester.Enabled = false;
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


        private void chkFOC_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void chkTester_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cboshoplist_selectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void chkBank_CheckedChanged(object sender, EventArgs e)
        {
            bool b = chkBank.Checked ? true : false;
            chkBankTransfer.CheckedChanged -= chkBankTransfer_CheckedChanged;
            chkBankTransfer.Checked = b;
            chkBankTransfer.CheckedChanged += chkBankTransfer_CheckedChanged;

            chkMPU.CheckedChanged -= chkMPU_CheckedChanged;
            chkMPU.Checked = b;
            chkMPU.CheckedChanged += chkMPU_CheckedChanged;

            chkJCB.CheckedChanged -= chkJCB_CheckedChanged;
            chkJCB.Checked = b;
            chkJCB.CheckedChanged += chkJCB_CheckedChanged;

            chkVisa.CheckedChanged -= chkVisa_CheckedChanged;
            chkVisa.Checked = b;
            chkVisa.CheckedChanged += chkVisa_CheckedChanged;

            chkMaster.CheckedChanged -= chkMaster_CheckedChanged;
            chkMaster.Checked = b;
            chkMaster.CheckedChanged += chkMaster_CheckedChanged;

            chkUnionPay.CheckedChanged -= chkUnionPay_CheckedChanged;
            chkUnionPay.Checked = b;
            chkUnionPay.CheckedChanged += chkUnionPay_CheckedChanged;

            chkAYAPay.CheckedChanged -= chkAYAPay_CheckedChanged;
            chkAYAPay.Checked = b;
            chkAYAPay.CheckedChanged += chkAYAPay_CheckedChanged;

            chkWavePay.CheckedChanged -= chkWavePay_CheckedChanged;
            chkWavePay.Checked = b;
            chkWavePay.CheckedChanged += chkWavePay_CheckedChanged;

            chkOnePay.CheckedChanged -= chkOnePay_CheckedChanged;
            chkOnePay.Checked = b;
            chkOnePay.CheckedChanged += chkOnePay_CheckedChanged;

            chkSaiPay.CheckedChanged -= chkSaiPay_CheckedChanged;
            chkSaiPay.Checked = b;
            chkSaiPay.CheckedChanged += chkSaiPay_CheckedChanged;

            chkKBZPay.CheckedChanged -= chkKBZPay_CheckedChanged;
            chkKBZPay.Checked = b;
            chkKBZPay.CheckedChanged += chkKBZPay_CheckedChanged;

            chkCBPay.CheckedChanged -= chkCBPay_CheckedChanged;
            chkCBPay.Checked = b;
            chkCBPay.CheckedChanged += chkCBPay_CheckedChanged;
            LoadData();
        }

        private void chkBankTransfer_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBankTransfer.Checked)
            {
                chkBank.CheckedChanged -= chkBank_CheckedChanged;
                chkBank.Checked = true;
                chkBank.CheckedChanged += chkBank_CheckedChanged;


            }
            if (chkBankTransfer.Checked == false && chkMPU.Checked == false && chkJCB.Checked == false && chkVisa.Checked == false
                && chkMaster.Checked == false && chkUnionPay.Checked == false && chkAYAPay.Checked == false && chkWavePay.Checked == false
                && chkOnePay.Checked == false && chkSaiPay.Checked == false && chkKBZPay.Checked == false && chkCBPay.Checked == false)
            {
                chkBank.CheckedChanged -= chkBank_CheckedChanged;
                chkBank.Checked = false;
                chkBank.CheckedChanged += chkBank_CheckedChanged;

            }
            LoadData();
        }

        private void chkMPU_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMPU.Checked)
            {
                chkBank.CheckedChanged -= chkBank_CheckedChanged;
                chkBank.Checked = true;
                chkBank.CheckedChanged += chkBank_CheckedChanged;
            }
            if (chkBankTransfer.Checked == false && chkMPU.Checked == false && chkJCB.Checked == false && chkVisa.Checked == false
                && chkMaster.Checked == false && chkUnionPay.Checked == false && chkAYAPay.Checked == false && chkWavePay.Checked == false
                && chkOnePay.Checked == false && chkSaiPay.Checked == false && chkKBZPay.Checked == false && chkCBPay.Checked == false)
            {
                chkBank.CheckedChanged -= chkBank_CheckedChanged;
                chkBank.Checked = false;
                chkBank.CheckedChanged += chkBank_CheckedChanged;

            }
            LoadData();
        }

        private void chkJCB_CheckedChanged(object sender, EventArgs e)
        {
            if (chkJCB.Checked)
            {
                chkBank.CheckedChanged -= chkBank_CheckedChanged;
                chkBank.Checked = true;
                chkBank.CheckedChanged += chkBank_CheckedChanged;
            }
            if (chkBankTransfer.Checked == false && chkMPU.Checked == false && chkJCB.Checked == false && chkVisa.Checked == false
                && chkMaster.Checked == false && chkUnionPay.Checked == false && chkAYAPay.Checked == false && chkWavePay.Checked == false
                && chkOnePay.Checked == false && chkSaiPay.Checked == false && chkKBZPay.Checked == false && chkCBPay.Checked == false)
            {
                chkBank.CheckedChanged -= chkBank_CheckedChanged;
                chkBank.Checked = false;
                chkBank.CheckedChanged += chkBank_CheckedChanged;
            }
            LoadData();
        }

        private void chkVisa_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVisa.Checked)
            {
                chkBank.CheckedChanged -= chkBank_CheckedChanged;
                chkBank.Checked = true;
                chkBank.CheckedChanged += chkBank_CheckedChanged;
                
            }
            if (chkBankTransfer.Checked == false && chkMPU.Checked == false && chkJCB.Checked == false && chkVisa.Checked == false
                && chkMaster.Checked == false && chkUnionPay.Checked == false && chkAYAPay.Checked == false && chkWavePay.Checked == false
                && chkOnePay.Checked == false && chkSaiPay.Checked == false && chkKBZPay.Checked == false && chkCBPay.Checked == false)
            {
                chkBank.CheckedChanged -= chkBank_CheckedChanged;
                chkBank.Checked = false;
                chkBank.CheckedChanged += chkBank_CheckedChanged;
                
            }
            LoadData();
        }

        private void chkMaster_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMaster.Checked)
            {
                chkBank.CheckedChanged -= chkBank_CheckedChanged;
                chkBank.Checked = true;
                chkBank.CheckedChanged += chkBank_CheckedChanged;
            }
            if (chkBankTransfer.Checked == false && chkMPU.Checked == false && chkJCB.Checked == false && chkVisa.Checked == false
                && chkMaster.Checked == false && chkUnionPay.Checked == false && chkAYAPay.Checked == false && chkWavePay.Checked == false
                && chkOnePay.Checked == false && chkSaiPay.Checked == false && chkKBZPay.Checked == false && chkCBPay.Checked == false)
            {
                chkBank.CheckedChanged -= chkBank_CheckedChanged;
                chkBank.Checked = false;
                chkBank.CheckedChanged += chkBank_CheckedChanged;
            }
            LoadData();
        }

        private void chkUnionPay_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUnionPay.Checked)
            {
                chkBank.CheckedChanged -= chkBank_CheckedChanged;
                chkBank.Checked = true;
                chkBank.CheckedChanged += chkBank_CheckedChanged;
            }
            if (chkBankTransfer.Checked == false && chkMPU.Checked == false && chkJCB.Checked == false && chkVisa.Checked == false
                && chkMaster.Checked == false && chkUnionPay.Checked == false && chkAYAPay.Checked == false && chkWavePay.Checked == false
                && chkOnePay.Checked == false && chkSaiPay.Checked == false && chkKBZPay.Checked == false && chkCBPay.Checked == false)
            {
                chkBank.CheckedChanged -= chkBank_CheckedChanged;
                chkBank.Checked = false;
                chkBank.CheckedChanged += chkBank_CheckedChanged;
            }
            LoadData();
        }

        private void chkAYAPay_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAYAPay.Checked)
            {
                chkBank.CheckedChanged -= chkBank_CheckedChanged;
                chkBank.Checked = true;
                chkBank.CheckedChanged += chkBank_CheckedChanged;
            }
            if (chkBankTransfer.Checked == false && chkMPU.Checked == false && chkJCB.Checked == false && chkVisa.Checked == false
                && chkMaster.Checked == false && chkUnionPay.Checked == false && chkAYAPay.Checked == false && chkWavePay.Checked == false
                && chkOnePay.Checked == false && chkSaiPay.Checked == false && chkKBZPay.Checked == false && chkCBPay.Checked == false)
            {
                chkBank.CheckedChanged -= chkBank_CheckedChanged;
                chkBank.Checked = false;
                chkBank.CheckedChanged += chkBank_CheckedChanged;
            }
            LoadData();
        }

        private void chkWavePay_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWavePay.Checked)
            {
                chkBank.CheckedChanged -= chkBank_CheckedChanged;
                chkBank.Checked = true;
                chkBank.CheckedChanged += chkBank_CheckedChanged;
            }
            if (chkBankTransfer.Checked == false && chkMPU.Checked == false && chkJCB.Checked == false && chkVisa.Checked == false
                && chkMaster.Checked == false && chkUnionPay.Checked == false && chkAYAPay.Checked == false && chkWavePay.Checked == false
                && chkOnePay.Checked == false && chkSaiPay.Checked == false && chkKBZPay.Checked == false && chkCBPay.Checked == false)
            {
                chkBank.CheckedChanged -= chkBank_CheckedChanged;
                chkBank.Checked = false;
                chkBank.CheckedChanged += chkBank_CheckedChanged;
            }
            LoadData();
        }

        private void chkOnePay_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOnePay.Checked)
            {
                chkBank.CheckedChanged -= chkBank_CheckedChanged;
                chkBank.Checked = true;
                chkBank.CheckedChanged += chkBank_CheckedChanged;
            }
            if (chkBankTransfer.Checked == false && chkMPU.Checked == false && chkJCB.Checked == false && chkVisa.Checked == false
                && chkMaster.Checked == false && chkUnionPay.Checked == false && chkAYAPay.Checked == false && chkWavePay.Checked == false
                && chkOnePay.Checked == false && chkSaiPay.Checked == false && chkKBZPay.Checked == false && chkCBPay.Checked == false)
            {
                chkBank.CheckedChanged -= chkBank_CheckedChanged;
                chkBank.Checked = false;
                chkBank.CheckedChanged += chkBank_CheckedChanged;
            }
            LoadData();
        }

        private void chkSaiPay_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSaiPay.Checked)
            {
                chkBank.CheckedChanged -= chkBank_CheckedChanged;
                chkBank.Checked = true;
                chkBank.CheckedChanged += chkBank_CheckedChanged;
            }
            if (chkBankTransfer.Checked == false && chkMPU.Checked == false && chkJCB.Checked == false && chkVisa.Checked == false
                && chkMaster.Checked == false && chkUnionPay.Checked == false && chkAYAPay.Checked == false && chkWavePay.Checked == false
                && chkOnePay.Checked == false && chkSaiPay.Checked == false && chkKBZPay.Checked == false && chkCBPay.Checked == false)
            {
                chkBank.CheckedChanged -= chkBank_CheckedChanged;
                chkBank.Checked = false;
                chkBank.CheckedChanged += chkBank_CheckedChanged;
            }
            LoadData();
        }

        private void chkKBZPay_CheckedChanged(object sender, EventArgs e)
        {
            if (chkKBZPay.Checked)
            {
                chkBank.CheckedChanged -= chkBank_CheckedChanged;
                chkBank.Checked = true;
                chkBank.CheckedChanged += chkBank_CheckedChanged;
            }
            if (chkBankTransfer.Checked == false && chkMPU.Checked == false && chkJCB.Checked == false && chkVisa.Checked == false
                && chkMaster.Checked == false && chkUnionPay.Checked == false && chkAYAPay.Checked == false && chkWavePay.Checked == false
                && chkOnePay.Checked == false && chkSaiPay.Checked == false && chkKBZPay.Checked == false && chkCBPay.Checked == false)
            {
                chkBank.CheckedChanged -= chkBank_CheckedChanged;
                chkBank.Checked = false;
                chkBank.CheckedChanged += chkBank_CheckedChanged;
            }
            LoadData();
        }

        private void chkCBPay_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCBPay.Checked)
            {
                chkBank.CheckedChanged -= chkBank_CheckedChanged;
                chkBank.Checked = true;
                chkBank.CheckedChanged += chkBank_CheckedChanged;
            }
            if (chkBankTransfer.Checked == false && chkMPU.Checked == false && chkJCB.Checked == false && chkVisa.Checked == false
                && chkMaster.Checked == false && chkUnionPay.Checked == false && chkAYAPay.Checked == false && chkWavePay.Checked == false
                && chkOnePay.Checked == false && chkSaiPay.Checked == false && chkKBZPay.Checked == false && chkCBPay.Checked == false)
            {
                chkBank.CheckedChanged -= chkBank_CheckedChanged;
                chkBank.Checked = false;
                chkBank.CheckedChanged += chkBank_CheckedChanged;
            }
            LoadData();
        }
        #endregion

        #region Function

        private void LoadData()
        {
            if (Isstart == true)
            {

                int shopid = Convert.ToInt32(cboshoplist.SelectedValue);
                string currentshortcode = "";
                if (shopid != 0)
                {
                    currentshortcode = (from p in entity.Shops where p.Id == shopid select p.ShortCode).FirstOrDefault();
                }
                else
                {
                    currentshortcode = "0";
                }


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
                bool IsFOC = chkFOC.Checked;
                bool IsBank = chkBank.Checked;
                bool IsTester = chkTester.Checked;
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
                    isBankTransfer = chkBankTransfer.Checked;
                    isMPU = chkMPU.Checked;
                    isVisa = chkVisa.Checked;
                    isWavePay = chkWavePay.Checked;
                    isAYAPay = chkAYAPay.Checked;
                    isJCB = chkJCB.Checked;
                    isUnionPay = chkUnionPay.Checked;
                    isKBZPay = chkKBZPay.Checked;
                    isCBPay = chkCBPay.Checked;
                    isSaiPay = chkSaiPay.Checked;
                    isMaster = chkMaster.Checked;
                    isOnePay = chkOnePay.Checked;

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
                            if (IsBank)
                            {
                                transList = (from t in entity.Transactions
                                             where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true
                                                 && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId
                                                 && ((IsCash && t.PaymentTypeId == 1) || (IsCredit && t.PaymentTypeId == 2) || (IsGiftCard && t.PaymentTypeId == 3) || (IsFOC && t.PaymentTypeId == 4) ||
                                                 (isBankTransfer && t.PaymentType.Name.Trim() == "Bank Transfer") || (isMPU && t.PaymentType.Name.Trim() == "MPU") || (isJCB && t.PaymentType.Name.Trim() == "JCB")
                                                 || (isVisa && t.PaymentType.Name.Trim() == "Visa") || (isMaster && t.PaymentType.Name.Trim() == "Master") || (isUnionPay && t.PaymentType.Name.Trim() == "Union Pay")
                                                 || (isKBZPay && t.PaymentType.Name.Trim() == "KBZ Pay") || (isCBPay && t.PaymentType.Name.Trim() == "CB Pay") || (isAYAPay && t.PaymentType.Name.Trim() == "AYA Pay")
                                                 || (isWavePay && t.PaymentType.Name.Trim() == "Wave Pay") || (isOnePay && t.PaymentType.Name.Trim() == "One Pay") || (isSaiPay && t.PaymentType.Name.Trim() == "Sai Sai Pay")
                                                 || (IsTester && t.PaymentTypeId == 6)) &&
                                                 (t.IsDeleted == null || t.IsDeleted == false)
                                                 && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1))
                                             orderby t.PaymentTypeId
                                             select t).ToList<Transaction>();

                            }
                            else
                            {
                                transList = (from t in entity.Transactions
                                             where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true
                                                 && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId
                                                 && ((IsCash && t.PaymentTypeId == 1) || (IsCredit && t.PaymentTypeId == 2) || (IsGiftCard && t.PaymentTypeId == 3) || (IsFOC && t.PaymentTypeId == 4)
                                                 || (IsTester && t.PaymentTypeId == 6)) &&
                                                 (t.IsDeleted == null || t.IsDeleted == false)
                                                 && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1))
                                             orderby t.PaymentTypeId
                                             select t).ToList<Transaction>();
                            }

                            ShowReportViewer();
                            lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                            gbTransactionList.Text = "Sale Transaction Report ";
                        }
                        else if (IsRefund)
                        {
                            transList = (from t in entity.Transactions
                                         where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true
                                             && t.IsActive == true && (t.Type == TransactionType.Refund || t.Type == TransactionType.CreditRefund) && t.CounterId == CounterId && t.UserId == CashierId
                                             && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1))
                                         select t).ToList<Transaction>();
                            ShowReportViewer();
                            lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                            gbTransactionList.Text = "Refund Transaction Report ";
                        }
                        else if (IsDebt)
                        {
                            if (IsBank)
                            {
                                transList = (from t in entity.Transactions
                                             where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Prepaid || t.Type == TransactionType.Settlement) && (
              (isBankTransfer && t.PaymentType.Name.Trim() == "Bank Transfer") || (IsCash && t.PaymentTypeId == 1) || (isMPU && t.PaymentType.Name.Trim() == "MPU") || (isJCB && t.PaymentType.Name.Trim() == "JCB")
                   || (isVisa && t.PaymentType.Name.Trim() == "Visa") || (isMaster && t.PaymentType.Name.Trim() == "Master") || (isUnionPay && t.PaymentType.Name.Trim() == "Union Pay")
                   || (isKBZPay && t.PaymentType.Name.Trim() == "KBZ Pay") || (isCBPay && t.PaymentType.Name.Trim() == "CB Pay") || (isAYAPay && t.PaymentType.Name.Trim() == "AYA Pay")
                   || (isWavePay && t.PaymentType.Name.Trim() == "Wave Pay") || (isOnePay && t.PaymentType.Name.Trim() == "One Pay") || (isSaiPay && t.PaymentType.Name.Trim() == "Sai Sai Pay"))
                   && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1))
                                             select t).ToList<Transaction>();

                            }

                            else
                            {
                                transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Prepaid || t.Type == TransactionType.Settlement) && (IsCash && t.PaymentTypeId == 1) && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            }

                            ShowReportViewer();
                            lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                            gbTransactionList.Text = "Debt Transaction Report ";
                        }

                        else if (IsSummary)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 1 && t.CounterId == CounterId && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            RtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Refund && t.CounterId == CounterId && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            DtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Settlement || t.Type == TransactionType.Prepaid) && t.CounterId == CounterId && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            CRtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.CreditRefund && t.CounterId == CounterId && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            GCtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 3 && t.CounterId == CounterId && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            CtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Credit && t.CounterId == CounterId && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            MPUtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId >= 501 && t.CounterId == CounterId && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            FOCtrnsList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 4 && t.CounterId == CounterId && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            TesterList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 6 && t.CounterId == CounterId && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            ShowReportViewer1();
                            lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                            gbTransactionList.Text = "Transaction Report ";
                        }
                    }
                    #endregion

                    #region get transaction with cashier only
                    else if (IsCashier == true && IsCounter == false)
                    {
                        if (IsSale)
                        {

                            if (IsBank)
                            {
                                transList = (from t in entity.Transactions
                                             where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true
                                                 && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId
                                                 && ((IsCash && t.PaymentTypeId == 1) || (IsCredit && t.PaymentTypeId == 2) || (IsGiftCard && t.PaymentTypeId == 3) || (IsFOC && t.PaymentTypeId == 4) ||
                                                 (isBankTransfer && t.PaymentType.Name.Trim() == "Bank Transfer") || (isMPU && t.PaymentType.Name.Trim() == "MPU") || (isJCB && t.PaymentType.Name.Trim() == "JCB")
                                                 || (isVisa && t.PaymentType.Name.Trim() == "Visa") || (isMaster && t.PaymentType.Name.Trim() == "Master") || (isUnionPay && t.PaymentType.Name.Trim() == "Union Pay")
                                                 || (isKBZPay && t.PaymentType.Name.Trim() == "KBZ Pay") || (isCBPay && t.PaymentType.Name.Trim() == "CB Pay") || (isAYAPay && t.PaymentType.Name.Trim() == "AYA Pay")
                                                 || (isWavePay && t.PaymentType.Name.Trim() == "Wave Pay") || (isOnePay && t.PaymentType.Name.Trim() == "One Pay") || (isSaiPay && t.PaymentType.Name.Trim() == "Sai Sai Pay")
                                                 || (IsTester && t.PaymentTypeId == 6)) &&
                                                 (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1))
                                             orderby t.PaymentTypeId
                                             select t).ToList<Transaction>();
                            }
                            else
                            {
                                transList = (from t in entity.Transactions
                                             where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true
                                                 && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId
                                                 && ((IsCash && t.PaymentTypeId == 1) || (IsCredit && t.PaymentTypeId == 2) || (IsGiftCard && t.PaymentTypeId == 3) || (IsFOC && t.PaymentTypeId == 4) || (IsTester && t.PaymentTypeId == 6)) &&
                                                 (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1))
                                             orderby t.PaymentTypeId
                                             select t).ToList<Transaction>();
                            }
                            ShowReportViewer();
                            lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                            // lblNumberofTransaction.Text = transList.Count.ToString();
                            gbTransactionList.Text = "Sale Transaction Report ";
                            // lblTotalAmount.Text = "";
                        }
                        else if (IsRefund)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Refund || t.Type == TransactionType.CreditRefund) && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && t.Id.Substring(2, 2) == currentshortcode select t).ToList<Transaction>();
                            ShowReportViewer();
                            lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                            // lblNumberofTransaction.Text = transList.Count.ToString();
                            gbTransactionList.Text = "Refund Transaction Report ";
                            //lblTotalAmount.Text = "";
                        }
                        else if (IsDebt)
                        {
                            if (IsBank)
                            {
                                transList = (from t in entity.Transactions
                                             where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Prepaid || t.Type == TransactionType.Settlement) && (
              (isBankTransfer && t.PaymentType.Name.Trim() == "Bank Transfer") || (IsCash && t.PaymentTypeId == 1) || (isMPU && t.PaymentType.Name.Trim() == "MPU") || (isJCB && t.PaymentType.Name.Trim() == "JCB")
                   || (isVisa && t.PaymentType.Name.Trim() == "Visa") || (isMaster && t.PaymentType.Name.Trim() == "Master") || (isUnionPay && t.PaymentType.Name.Trim() == "Union Pay")
                   || (isKBZPay && t.PaymentType.Name.Trim() == "KBZ Pay") || (isCBPay && t.PaymentType.Name.Trim() == "CB Pay") || (isAYAPay && t.PaymentType.Name.Trim() == "AYA Pay")
                   || (isWavePay && t.PaymentType.Name.Trim() == "Wave Pay") || (isOnePay && t.PaymentType.Name.Trim() == "One Pay") || (isSaiPay && t.PaymentType.Name.Trim() == "Sai Sai Pay"))
                   && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1))
                                             select t).ToList<Transaction>();

                            }

                            else
                            {
                                transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Prepaid || t.Type == TransactionType.Settlement) && (IsCash && t.PaymentTypeId == 1) && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            }
                            ShowReportViewer();
                            lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                            // lblNumberofTransaction.Text = transList.Count.ToString();
                            gbTransactionList.Text = "Debt Transaction Report ";
                            //lblTotalAmount.Text = "";
                        }
                        else if (IsSummary)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 1 && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            RtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Refund && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            DtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Settlement || t.Type == TransactionType.Prepaid) && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            CRtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.CreditRefund && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            GCtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 3 && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            CtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Credit && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            MPUtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId >= 501 && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            FOCtrnsList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 4 && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            TesterList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 6 && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            ShowReportViewer1();
                            lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                            // lblNumberofTransaction.Text = transList.Count.ToString();
                            gbTransactionList.Text = "Transaction Report ";
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

                            //*Update SD*
                            if (IsBank)
                            {
                                transList = (from t in entity.Transactions
                                             where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true
                                                 && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId
                                                 && ((IsCash && t.PaymentTypeId == 1) || (IsCredit && t.PaymentTypeId == 2) || (IsGiftCard && t.PaymentTypeId == 3) || (IsFOC && t.PaymentTypeId == 4) ||
                                                  (isBankTransfer && t.PaymentType.Name.Trim() == "Bank Transfer") || (isMPU && t.PaymentType.Name.Trim() == "MPU") || (isJCB && t.PaymentType.Name.Trim() == "JCB")
                                                 || (isVisa && t.PaymentType.Name.Trim() == "Visa") || (isMaster && t.PaymentType.Name.Trim() == "Master") || (isUnionPay && t.PaymentType.Name.Trim() == "Union Pay")
                                                 || (isKBZPay && t.PaymentType.Name.Trim() == "KBZ Pay") || (isCBPay && t.PaymentType.Name.Trim() == "CB Pay") || (isAYAPay && t.PaymentType.Name.Trim() == "AYA Pay")
                                                 || (isWavePay && t.PaymentType.Name.Trim() == "Wave Pay") || (isOnePay && t.PaymentType.Name.Trim() == "One Pay") || (isSaiPay && t.PaymentType.Name.Trim() == "Sai Sai Pay")
                                                 || (IsTester && t.PaymentTypeId == 6)) &&
                                                 (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1))
                                             orderby t.PaymentTypeId
                                             select t).ToList<Transaction>();
                            }
                            else
                            {
                                transList = (from t in entity.Transactions
                                             where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true
                                                 && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId
                                                 && ((IsCash && t.PaymentTypeId == 1) || (IsCredit && t.PaymentTypeId == 2) || (IsGiftCard && t.PaymentTypeId == 3) || (IsFOC && t.PaymentTypeId == 4) || (IsTester && t.PaymentTypeId == 6)) &&
                                                 (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1))
                                             orderby t.PaymentTypeId
                                             select t).ToList<Transaction>();
                            }

                            #endregion
                            ShowReportViewer();
                            lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                            gbTransactionList.Text = "Sale Transaction Report ";
                        }
                        else if (IsRefund)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Refund || t.Type == TransactionType.CreditRefund) && t.CounterId == CounterId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            ShowReportViewer();
                            lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                            gbTransactionList.Text = "Refund Transaction Report ";
                        }
                        else if (IsDebt)
                        {
                            if (IsBank)
                            {
                                transList = (from t in entity.Transactions
                                             where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Prepaid || t.Type == TransactionType.Settlement) && (
              (isBankTransfer && t.PaymentType.Name.Trim() == "Bank Transfer") || (IsCash && t.PaymentTypeId == 1) || (isMPU && t.PaymentType.Name.Trim() == "MPU") || (isJCB && t.PaymentType.Name.Trim() == "JCB")
                   || (isVisa && t.PaymentType.Name.Trim() == "Visa") || (isMaster && t.PaymentType.Name.Trim() == "Master") || (isUnionPay && t.PaymentType.Name.Trim() == "Union Pay")
                   || (isKBZPay && t.PaymentType.Name.Trim() == "KBZ Pay") || (isCBPay && t.PaymentType.Name.Trim() == "CB Pay") || (isAYAPay && t.PaymentType.Name.Trim() == "AYA Pay")
                   || (isWavePay && t.PaymentType.Name.Trim() == "Wave Pay") || (isOnePay && t.PaymentType.Name.Trim() == "One Pay") || (isSaiPay && t.PaymentType.Name.Trim() == "Sai Sai Pay"))
                   && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1))
                                             select t).ToList<Transaction>();

                            }

                            else
                            {
                                transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Prepaid || t.Type == TransactionType.Settlement) && (IsCash && t.PaymentTypeId == 1) && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            }
                            ShowReportViewer();
                            lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                            gbTransactionList.Text = "Debt Transaction Report ";
                        }
                        else if (IsSummary)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 1 && t.CounterId == CounterId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            RtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Refund && t.CounterId == CounterId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            DtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Settlement || t.Type == TransactionType.Prepaid) && t.CounterId == CounterId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            CRtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.CreditRefund && t.CounterId == CounterId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            GCtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 3 && t.CounterId == CounterId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            CtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Credit && t.CounterId == CounterId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            MPUtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId >= 501 && t.CounterId == CounterId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            FOCtrnsList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 4 && t.CounterId == CounterId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            TesterList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 6 && t.CounterId == CounterId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            ShowReportViewer1();
                            lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                            gbTransactionList.Text = "Transaction Report ";
                        }
                    }
                    #endregion

                    #region get all transactions
                    else
                    {
                        if (IsSale)
                        {
                            if (IsBank)
                            {
                                transList = (from t in entity.Transactions
                                             where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate &&
                                             EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate
                                             && t.IsComplete == true && t.IsActive == true
                                             && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit)
                                             && ((IsCash && t.PaymentTypeId == 1) || (IsCredit && t.PaymentTypeId == 2) || (IsGiftCard && t.PaymentTypeId == 3) || (IsFOC && t.PaymentTypeId == 4) ||
                                              (isBankTransfer && t.PaymentType.Name.Trim() == "Bank Transfer") || (isMPU && t.PaymentType.Name.Trim() == "MPU") || (isJCB && t.PaymentType.Name.Trim() == "JCB")
                                                 || (isVisa && t.PaymentType.Name.Trim() == "Visa") || (isMaster && t.PaymentType.Name.Trim() == "Master") || (isUnionPay && t.PaymentType.Name.Trim() == "Union Pay")
                                                 || (isKBZPay && t.PaymentType.Name.Trim() == "KBZ Pay") || (isCBPay && t.PaymentType.Name.Trim() == "CB Pay") || (isAYAPay && t.PaymentType.Name.Trim() == "AYA Pay")
                                                 || (isWavePay && t.PaymentType.Name.Trim() == "Wave Pay") || (isOnePay && t.PaymentType.Name.Trim() == "One Pay") || (isSaiPay && t.PaymentType.Name.Trim() == "Sai Sai Pay")
                                             || (IsTester && t.PaymentTypeId == 6)) &&
                                             (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1))
                                             orderby t.PaymentTypeId
                                             select t).ToList<Transaction>();

                            }
                            else
                            {
                                //getting the transaction data Here.
                                transList = (from t in entity.Transactions
                                             where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate &&
                                             EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate
                                             && t.IsComplete == true && t.IsActive == true
                                             && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit)
                                             && ((IsCash && t.PaymentTypeId == 1) || (IsCredit && t.PaymentTypeId == 2) || (IsGiftCard && t.PaymentTypeId == 3) || (IsFOC && t.PaymentTypeId == 4) || (IsTester && t.PaymentTypeId == 6)) &&
                                             (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1))
                                             orderby t.PaymentTypeId
                                             select t).ToList<Transaction>();
                            }

                            ShowReportViewer();
                            lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                            gbTransactionList.Text = "Sale Transaction Report ";

                        }
                        else if (IsRefund)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Refund || t.Type == TransactionType.CreditRefund) && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            ShowReportViewer();
                            lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                            gbTransactionList.Text = "Refund Transaction Report ";
                        }
                        else if (IsDebt)
                        {
                            if (IsBank)
                            {
                                transList = (from t in entity.Transactions
                                             where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Prepaid || t.Type == TransactionType.Settlement) && (
              (isBankTransfer && t.PaymentType.Name.Trim() == "Bank Transfer") || (IsCash && t.PaymentTypeId == 1) || (isMPU && t.PaymentType.Name.Trim() == "MPU") || (isJCB && t.PaymentType.Name.Trim() == "JCB")
                   || (isVisa && t.PaymentType.Name.Trim() == "Visa") || (isMaster && t.PaymentType.Name.Trim() == "Master") || (isUnionPay && t.PaymentType.Name.Trim() == "Union Pay")
                   || (isKBZPay && t.PaymentType.Name.Trim() == "KBZ Pay") || (isCBPay && t.PaymentType.Name.Trim() == "CB Pay") || (isAYAPay && t.PaymentType.Name.Trim() == "AYA Pay")
                   || (isWavePay && t.PaymentType.Name.Trim() == "Wave Pay") || (isOnePay && t.PaymentType.Name.Trim() == "One Pay") || (isSaiPay && t.PaymentType.Name.Trim() == "Sai Sai Pay"))
                   && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1))
                                             select t).ToList<Transaction>();

                            }

                            else
                            {
                                transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Prepaid || t.Type == TransactionType.Settlement) && (IsCash && t.PaymentTypeId == 1) && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            }

                            ShowReportViewer();
                            lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                            gbTransactionList.Text = "Debt Transaction Report ";
                        }
                        else if (IsSummary)
                        {
                            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 1 && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            RtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Refund && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            DtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Settlement || t.Type == TransactionType.Prepaid) && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            CRtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.CreditRefund && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            GCtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 3 && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            CtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Credit && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            MPUtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId >= 501 && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            FOCtrnsList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 4 && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            TesterList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 6 && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            ShowReportViewer1();
                            lblPeriod.Text = fromDate.ToString("dd-MM-yyyy") + " to " + toDate.ToString("dd-MM-yyyy");
                            gbTransactionList.Text = "Transaction Report ";

                        }
                    }
                    #endregion
                }
            }
        }

        private void ShowReportViewer()
        {
            int shopid = Convert.ToInt32(cboshoplist.SelectedValue);
            string shopname = (from p in entity.Shops where p.Id == shopid select p.ShopName).FirstOrDefault();

            dsReportTemp dsReport = new dsReportTemp();
            dsReportTemp.TransactionListDataTable dtTransactionReport = (dsReportTemp.TransactionListDataTable)dsReport.Tables["TransactionList"];
            string reportPath = string.Empty;
            if (SettingController.IsSourcecode)
            {
                reportPath = Application.StartupPath + "\\Reports\\SourcecodeTransactionReport.rdlc";
                foreach (Transaction transaction in transList)
                {
                    dsReportTemp.TransactionListRow newRow = dtTransactionReport.NewTransactionListRow();
                    newRow.TransactionId = transaction.Id;
                    newRow.Date = Convert.ToDateTime(transaction.DateTime);
                    newRow.SalePerson = transaction.User.Name;
                    newRow.PaymentMethod = transaction.PaymentType.Name;
                    if (transaction.Type == TransactionType.CreditRefund || transaction.Type == TransactionType.Refund)
                    {
                        newRow.Amount = Convert.ToInt32(transaction.RecieveAmount);
                    }
                    //else if (transaction.Type == TransactionType.Credit)
                    //{
                    //    newRow.Amount = Convert.ToInt32(transaction.TotalAmount - transaction.RecieveAmount);
                    //}                   
                    else
                    {
                        newRow.Amount = Convert.ToInt32(transaction.TotalAmount);
                    }
                    newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                    newRow.Type = transaction.Type;

                    if (transaction.ParentId != null)
                    {
                        var transactionDetailData = entity.TransactionDetails.Where(x => x.TransactionId == transaction.ParentId).ToList();
                        int qty = 0;
                        foreach (var data in transactionDetailData)
                        {
                            newRow.ProductName = entity.Products.Where(x => x.Id == data.ProductId).SingleOrDefault().Name;
                            qty += (int)data.Qty;
                        }
                        newRow.Qty = qty;
                    }
                    else
                    {
                        var transactionDetailData = entity.TransactionDetails.Where(x => x.TransactionId == transaction.Id).ToList();
                        foreach (var data in transactionDetailData)
                        {
                            newRow.ProductName = entity.Products.Where(x => x.Id == data.ProductId).SingleOrDefault().Name;
                        }
                        newRow.Qty = Convert.ToInt32(transaction.TransactionDetails.Where(x => x.IsDeleted == false).Sum(x => x.Qty));
                    }
                    dtTransactionReport.AddTransactionListRow(newRow);
                }
            }
            else
            {
                reportPath = Application.StartupPath + "\\Reports\\TransactionReport.rdlc";
                foreach (Transaction transaction in transList)
                {
                    dsReportTemp.TransactionListRow newRow = dtTransactionReport.NewTransactionListRow();
                    newRow.TransactionId = transaction.Id;
                    newRow.Date = Convert.ToDateTime(transaction.DateTime);
                    newRow.SalePerson = transaction.User.Name;
                    newRow.PaymentMethod = transaction.PaymentType.Name;
                    if (transaction.Type == TransactionType.CreditRefund || transaction.Type == TransactionType.Refund)
                    {
                        newRow.Amount = Convert.ToInt32(transaction.RecieveAmount);
                    }
                    //else if (transaction.Type == TransactionType.Credit)
                    //{
                    //    newRow.Amount = Convert.ToInt32(transaction.TotalAmount - transaction.RecieveAmount);
                    //}
                    else
                    {
                        newRow.Amount = Convert.ToInt32(transaction.TotalAmount);
                    }
                    newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                    newRow.Type = transaction.Type;
                    newRow.Qty = Convert.ToInt32(transaction.TransactionDetails.Where(x => x.IsDeleted == false).Sum(x => x.Qty));
                    dtTransactionReport.AddTransactionListRow(newRow);
                }
            }

            ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["TransactionList"]);

            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            ReportParameter TransactionTitle = new ReportParameter("TransactionTitle", gbTransactionList.Text + "for " + shopname);
            reportViewer1.LocalReport.SetParameters(TransactionTitle);

            ReportParameter Date = new ReportParameter("Date", " from " + dtpFrom.Value.Date.ToString("dd-MM-yyyy") + " To " + dtpTo.Value.Date.ToString("dd-MM-yyyy"));
            reportViewer1.LocalReport.SetParameters(Date);

            reportViewer1.RefreshReport();
        }

        private void ShowReportViewer1()
        {
            string reportPath = string.Empty;
            int shopid = Convert.ToInt32(cboshoplist.SelectedValue);
            string shopname = (from p in entity.Shops where p.Id == shopid select p.ShopName).FirstOrDefault();
            string currentshortcode = (from p in entity.Shops where p.Id == shopid select p.ShortCode).FirstOrDefault();
            int totalSale = 0, totalRefund = 0, totalDebt = 0, totalCreditRefund = 0, totalSummary = 0; int totalGiftCard = 0,
                totalCashFromGiftCard = 0, totalCredit = 0, totalCreditRecieve = 0, totalCashInHand = 0, totalExpense = 0, totalIncomeAmount = 0, totalMPU = 0, totalFOC = 0, totalReceived = 0;
            long totalDiscount = 0, totalRefundDiscount = 0, totalCreditRefundDiscount = 0; long totalTester = 0;
            long totalMCDiscount = 0;
            int totalOtherFOC = 0;
            #region Transaction
            dsReportTemp dsReport = new dsReportTemp();
            dsReportTemp.TransactionListDataTable dtTransactionReport = (dsReportTemp.TransactionListDataTable)dsReport.Tables["TransactionList"];

            if (SettingController.IsSourcecode)
            {
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
                    newRow.Qty = Convert.ToInt32(transaction.TransactionDetails.Where(x => x.IsDeleted == false).Sum(x => x.Qty));
                    totalSale += Convert.ToInt32(transaction.TotalAmount);
                    totalOtherFOC += Convert.ToInt32(transaction.TransactionDetails.Where(x => x.IsFOC == true).Sum((x => x.SellingPrice * x.Qty)));
                    var transactionDetailData = entity.TransactionDetails.Where(x => x.TransactionId == transaction.Id).ToList();
                    foreach (var data in transactionDetailData)
                    {
                        newRow.ProductName = entity.Products.Where(x => x.Id == data.ProductId).SingleOrDefault().Name;
                    }
                    dtTransactionReport.AddTransactionListRow(newRow);
                }
            }
            else
            {
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
                    newRow.Qty = Convert.ToInt32(transaction.TransactionDetails.Where(x => x.IsDeleted == false).Sum(x => x.Qty));
                    totalSale += Convert.ToInt32(transaction.TotalAmount);
                    totalOtherFOC += Convert.ToInt32(transaction.TransactionDetails.Where(x => x.IsFOC == true).Sum((x => x.SellingPrice * x.Qty)));
                    dtTransactionReport.AddTransactionListRow(newRow);
                }
            }
            ReportDataSource rds1 = new ReportDataSource("SaleDataSet", dsReport.Tables["TransactionList"]);
            #endregion
            #region Get Discount Value

            DateTime fromDate = dtpFrom.Value.Date;
            DateTime toDate = dtpTo.Value.Date;
            int CashierId = 0;
            int CounterId = 0;

            List<Transaction> discounttransList = new List<Transaction>();

            //If user use filter for both Counter and Casher
            if (cboCounter.SelectedIndex > 0 && cboCashier.SelectedIndex > 0)
            {
                CounterId = Convert.ToInt32(cboCounter.SelectedValue);
                CashierId = Convert.ToInt32(cboCashier.SelectedValue);

                discounttransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.CounterId == CounterId && t.UserId == CashierId && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.IsDeleted == null || t.IsDeleted == false) && t.Id.Substring(2, 2) == currentshortcode select t).ToList<Transaction>();
            }
            // User just use Counter filter
            else if (cboCounter.SelectedIndex > 0)
            {
                CounterId = Convert.ToInt32(cboCounter.SelectedValue);
                discounttransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.CounterId == CounterId && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.IsDeleted == null || t.IsDeleted == false) && t.Id.Substring(2, 2) == currentshortcode select t).ToList<Transaction>();
            }
            // User just use Casher filter
            else if (cboCashier.SelectedIndex > 0)
            {
                CashierId = Convert.ToInt32(cboCashier.SelectedValue);
                discounttransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.UserId == CashierId && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.IsDeleted == null || t.IsDeleted == false) && t.Id.Substring(2, 2) == currentshortcode select t).ToList<Transaction>();
            }
            // User ignore both filter
            else
            {
                discounttransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.IsDeleted == null || t.IsDeleted == false) && t.Id.Substring(2, 2) == currentshortcode select t).ToList<Transaction>();
            }

            foreach (Transaction t in discounttransList)
            {
                long itemdiscount = (long)t.TransactionDetails.Sum(x => (x.UnitPrice * (x.DiscountRate / 100)) * x.Qty);
                totalDiscount += (long)t.DiscountAmount - itemdiscount;

                if ((int)(t.MCDiscountAmt) != 0)
                {
                    totalMCDiscount += (long)(t.MCDiscountAmt);
                }
                else if ((int)(t.BDDiscountAmt) != 0)
                {
                    totalMCDiscount += (long)(t.BDDiscountAmt);
                }

            }

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
                newRow.Amount = Convert.ToInt32(transaction.RecieveAmount);
                newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                newRow.Type = transaction.Type;
                newRow.Qty = Convert.ToInt32(transaction.TransactionDetails.Sum(x => x.Qty));
                totalRefund += Convert.ToInt32(transaction.RecieveAmount);
                dtTransactionReportRefund.AddTransactionListRow(newRow);
            }
            ReportDataSource rds2 = new ReportDataSource("RefundDataSet", dsReportRefund.Tables["TransactionList"]);
            totalRefundDiscount = RtransList.Sum(x => x.DiscountAmount).Value;
            #endregion
            #region Debt
            dsReportTemp dsReportDebt = new dsReportTemp();
            dsReportTemp.TransactionListDataTable dtTransactionReportDebt = (dsReportTemp.TransactionListDataTable)dsReportDebt.Tables["TransactionList"];
            if (SettingController.IsSourcecode)
            {
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
                        newRow.Qty = Convert.ToInt32(transaction.TransactionDetails.Sum(x => x.Qty));
                        newRow.Type = transaction.Type;
                        totalDebt += Convert.ToInt32(transaction.TotalAmount);
                        var transactionDetailData = entity.TransactionDetails.Where(x => x.TransactionId == transaction.Id).ToList();
                        foreach (var data in transactionDetailData)
                        {
                            newRow.ProductName = entity.Products.Where(x => x.Id == data.ProductId).SingleOrDefault().Name;
                        }
                        dtTransactionReportDebt.AddTransactionListRow(newRow);
                    }
                    else
                    {
                       // long useprepaid = transaction.UsePrePaidDebts1 == null ? 0 : transaction.UsePrePaidDebts1.Sum(x => x.UseAmount).Value;
                        dsReportTemp.TransactionListRow newRow = dtTransactionReportDebt.NewTransactionListRow();
                        newRow.TransactionId = transaction.Id;
                        newRow.Date = Convert.ToDateTime(transaction.DateTime);
                        newRow.SalePerson = transaction.User.Name;
                        newRow.PaymentMethod = transaction.PaymentType.Name;
                        newRow.Amount = Convert.ToInt32(transaction.TotalAmount);
                        newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                        newRow.Qty = Convert.ToInt32(transaction.TransactionDetails.Sum(x => x.Qty));
                        newRow.Type = transaction.Type;
                        totalDebt += Convert.ToInt32(transaction.TotalAmount);
                        var transactionDetailData = entity.TransactionDetails.Where(x => x.TransactionId == transaction.Id).ToList();
                        foreach (var data in transactionDetailData)
                        {
                            newRow.ProductName = entity.Products.Where(x => x.Id == data.ProductId).SingleOrDefault().Name;
                        }
                        dtTransactionReportDebt.AddTransactionListRow(newRow);
                    }
                   
                }
            }
            else
            {
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
                        newRow.Qty = Convert.ToInt32(transaction.TransactionDetails.Sum(x => x.Qty));
                        newRow.Type = transaction.Type;
                        totalDebt += Convert.ToInt32(transaction.TotalAmount);

                        dtTransactionReportDebt.AddTransactionListRow(newRow);
                    }
                    else
                    {
                       // long useprepaid = transaction.UsePrePaidDebts1 == null ? 0 : transaction.UsePrePaidDebts1.Sum(x => x.UseAmount).Value;
                        dsReportTemp.TransactionListRow newRow = dtTransactionReportDebt.NewTransactionListRow();
                        newRow.TransactionId = transaction.Id;
                        newRow.Date = Convert.ToDateTime(transaction.DateTime);
                        newRow.SalePerson = transaction.User.Name;
                        newRow.PaymentMethod = transaction.PaymentType.Name;
                        newRow.Amount = Convert.ToInt32(transaction.TotalAmount);
                        newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                        newRow.Qty = Convert.ToInt32(transaction.TransactionDetails.Sum(x => x.Qty));
                        newRow.Type = transaction.Type;
                        totalDebt += Convert.ToInt32(transaction.TotalAmount);

                        dtTransactionReportDebt.AddTransactionListRow(newRow);
                    }
                   
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
                newRow.Amount = Convert.ToInt32(transaction.RecieveAmount);
                newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                newRow.Type = transaction.Type;
                newRow.Qty = Convert.ToInt32(transaction.TransactionDetails.Sum(x => x.Qty));

                totalCreditRefund += Convert.ToInt32(transaction.RecieveAmount);
                dtTransactionReportCreditRefund.AddTransactionListRow(newRow);
            }
            ReportDataSource rds4 = new ReportDataSource("CreditRefundDataSet", dsReportCreditRefund.Tables["TransactionList"]);
            totalCreditRefundDiscount = CRtransList.Sum(x => x.DiscountAmount).Value;
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
                newRow.Amount = Convert.ToInt32(transaction.TotalAmount - transaction.RecieveAmount);
                newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                newRow.Type = transaction.Type;

                newRow.Qty = Convert.ToInt32(transaction.TransactionDetails.Where(x => x.IsDeleted == false).Sum(x => x.Qty));
                totalOtherFOC += Convert.ToInt32(transaction.TransactionDetails.Where(x => x.IsFOC == true).Sum((x => x.SellingPrice * x.Qty)));

                totalGiftCard += Convert.ToInt32(transaction.GiftCardAmount);
                totalCashFromGiftCard += (Convert.ToInt32(transaction.TotalAmount) - Convert.ToInt32(transaction.GiftCardAmount));
                dtTransactionReportGiftCard.AddTransactionListRow(newRow);
            }
            ReportDataSource rds5 = new ReportDataSource("GiftCardDataSet", dsReportGiftCardTransaction.Tables["TransactionList"]);
            #endregion
            #region Credit
            dsReportTemp dsReportCreditTransaction = new dsReportTemp();
            dsReportTemp.TransactionListDataTable dtTransactionReportCredit = (dsReportTemp.TransactionListDataTable)dsReportCreditTransaction.Tables["TransactionList"];
            if (SettingController.IsSourcecode)
            {
                foreach (Transaction transaction in CtransList)
                {
                    dsReportTemp.TransactionListRow newRow = dtTransactionReportCredit.NewTransactionListRow();
                    newRow.TransactionId = transaction.Id;
                    newRow.Date = Convert.ToDateTime(transaction.DateTime);
                    newRow.SalePerson = transaction.User.Name;
                    newRow.PaymentMethod = transaction.PaymentType.Name;
                    //if (transaction.Type == TransactionType.Credit)
                    //{
                    //    newRow.Amount = Convert.ToInt32(transaction.TotalAmount - transaction.RecieveAmount);
                    //}
                    //else
                    //{
                        newRow.Amount = Convert.ToInt32(transaction.TotalAmount);
                    //}
                    
                    newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                    newRow.Type = transaction.Type;
                    newRow.Qty = Convert.ToInt32(transaction.TransactionDetails.Where(x => x.IsDeleted == false).Sum(x => x.Qty));
                    totalOtherFOC += Convert.ToInt32(transaction.TransactionDetails.Where(x => x.IsFOC == true).Sum((x => x.SellingPrice * x.Qty)));
                    totalCredit += Convert.ToInt32(transaction.TotalAmount);
                    totalCreditRecieve += Convert.ToInt32(transaction.RecieveAmount);
                    var transactionDetailData = entity.TransactionDetails.Where(x => x.TransactionId == transaction.Id).ToList();
                    foreach (var data in transactionDetailData)
                    {
                        newRow.ProductName = entity.Products.Where(x => x.Id == data.ProductId).SingleOrDefault().Name;
                    }
                    dtTransactionReportCredit.AddTransactionListRow(newRow);
                }
            }
            else
            {
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
                    newRow.Qty = Convert.ToInt32(transaction.TransactionDetails.Where(x => x.IsDeleted == false).Sum(x => x.Qty));
                    totalOtherFOC += Convert.ToInt32(transaction.TransactionDetails.Where(x => x.IsFOC == true).Sum((x => x.SellingPrice * x.Qty)));
                    totalCredit += Convert.ToInt32(transaction.TotalAmount);
                    totalCreditRecieve += Convert.ToInt32(transaction.RecieveAmount);
                    dtTransactionReportCredit.AddTransactionListRow(newRow);
                }
                ReportDataSource rds6 = new ReportDataSource("CreditDataSet", dsReportCreditTransaction.Tables["TransactionList"]);
                #endregion
                #region MPU
                dsReportTemp dsReportMPUTransaction = new dsReportTemp();
                dsReportTemp.TransactionListDataTable dtTransactionReportMPU = (dsReportTemp.TransactionListDataTable)dsReportMPUTransaction.Tables["TransactionList"];
                foreach (Transaction transaction in MPUtransList)
                {
                    dsReportTemp.TransactionListRow newRow = dtTransactionReportMPU.NewTransactionListRow();
                    newRow.TransactionId = transaction.Id;
                    newRow.Date = Convert.ToDateTime(transaction.DateTime);
                    newRow.SalePerson = transaction.User.Name;
                    newRow.PaymentMethod = transaction.PaymentType.Name;
                    newRow.Amount = Convert.ToInt32(transaction.TotalAmount);
                    newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                    newRow.Type = transaction.Type;
                    newRow.Qty = Convert.ToInt32(transaction.TransactionDetails.Where(x => x.IsDeleted == false).Sum(x => x.Qty));
                    totalOtherFOC += Convert.ToInt32(transaction.TransactionDetails.Where(x => x.IsFOC == true).Sum((x => x.SellingPrice * x.Qty)));
                    totalMPU += Convert.ToInt32(transaction.TotalAmount);
                    dtTransactionReportMPU.AddTransactionListRow(newRow);
                }
                ReportDataSource rds7 = new ReportDataSource("MPUDataSet", dsReportMPUTransaction.Tables["TransactionList"]);
                #endregion
                #region FOC
                dsReportTemp dsReportFOCTransaction = new dsReportTemp();
                dsReportTemp.TransactionListDataTable dtTransactionReportFOC = (dsReportTemp.TransactionListDataTable)dsReportFOCTransaction.Tables["TransactionList"];
                foreach (Transaction transaction in FOCtrnsList)
                {
                    dsReportTemp.TransactionListRow newRow = dtTransactionReportFOC.NewTransactionListRow();
                    newRow.TransactionId = transaction.Id;
                    newRow.Date = Convert.ToDateTime(transaction.DateTime);
                    newRow.SalePerson = transaction.User.Name;
                    newRow.PaymentMethod = transaction.PaymentType.Name;

                    newRow.Amount = Convert.ToInt32(transaction.TransactionDetails.Sum((x => x.SellingPrice * x.Qty)));
                    newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                    newRow.Type = transaction.Type;
                    newRow.Qty = Convert.ToInt32(transaction.TransactionDetails.Where(x => x.IsDeleted == false).Sum(x => x.Qty));

                    totalFOC += Convert.ToInt32(transaction.TransactionDetails.Sum((x => x.SellingPrice * x.Qty)));
                    dtTransactionReportFOC.AddTransactionListRow(newRow);
                }
                ReportDataSource rds8 = new ReportDataSource("FOCDataSet", dsReportFOCTransaction.Tables["TransactionList"]);
                #endregion

                #region Tester
                dsReportTemp dsReportTesterTransaction = new dsReportTemp();
                dsReportTemp.TransactionListDataTable dtTransactionReportTester = (dsReportTemp.TransactionListDataTable)dsReportTesterTransaction.Tables["TransactionList"];
                foreach (Transaction transaction in TesterList)
                {
                    dsReportTemp.TransactionListRow newRow = dtTransactionReportTester.NewTransactionListRow();
                    newRow.TransactionId = transaction.Id;
                    newRow.Date = Convert.ToDateTime(transaction.DateTime);
                    newRow.SalePerson = transaction.User.Name;
                    newRow.PaymentMethod = transaction.PaymentType.Name;
                    newRow.Amount = Convert.ToInt32(transaction.TotalAmount);
                    newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                    newRow.Type = transaction.Type;
                    newRow.Qty = Convert.ToInt32(transaction.TransactionDetails.Where(x => x.IsDeleted == false).Sum(x => x.Qty));
                    totalOtherFOC += Convert.ToInt32(transaction.TransactionDetails.Where(x => x.IsFOC == true).Sum((x => x.SellingPrice * x.Qty)));

                    totalTester += Convert.ToInt64(transaction.TotalAmount);
                    dtTransactionReportTester.AddTransactionListRow(newRow);
                }
                ReportDataSource rds9 = new ReportDataSource("TesterDataSet", dsReportTesterTransaction.Tables["TransactionList"]);
                #endregion

                totalSummary = ((totalSale + totalCredit + totalGiftCard + totalCashFromGiftCard + totalMPU) - (totalRefund + totalCreditRefund + totalFOC));
                totalCashInHand = (totalSale + totalCashFromGiftCard + totalDebt + totalCreditRecieve) - totalRefund;
                totalExpense = (totalRefund + totalCreditRefund + totalFOC);
                totalIncomeAmount = (totalSale + totalCredit + totalGiftCard + totalCashFromGiftCard + totalMPU);
                totalReceived = (totalSale + totalCashFromGiftCard + totalDebt + totalCreditRecieve);
                if (SettingController.IsSourcecode) { reportPath = Application.StartupPath + "\\Reports\\SourcecodeTransactionsDetailReport.rdlc"; }
                else
                {
                    reportPath = Application.StartupPath + "\\Reports\\TransactionsDetailReport.rdlc";
                }
                reportViewer1.LocalReport.ReportPath = reportPath;
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds1);
                reportViewer1.LocalReport.DataSources.Add(rds2);
                reportViewer1.LocalReport.DataSources.Add(rds3);
                reportViewer1.LocalReport.DataSources.Add(rds4);
                reportViewer1.LocalReport.DataSources.Add(rds5);
                reportViewer1.LocalReport.DataSources.Add(rds6);
                reportViewer1.LocalReport.DataSources.Add(rds7);
                reportViewer1.LocalReport.DataSources.Add(rds8);
                reportViewer1.LocalReport.DataSources.Add(rds9);


                ReportParameter TotalDiscount = new ReportParameter("TotalDiscount", totalDiscount.ToString());
                reportViewer1.LocalReport.SetParameters(TotalDiscount);

                ReportParameter TotalMCDiscount = new ReportParameter("TotalMCDiscount", totalMCDiscount.ToString());
                reportViewer1.LocalReport.SetParameters(TotalMCDiscount);


                ReportParameter TotalRefundDiscount = new ReportParameter("TotalRefundDiscount", totalRefundDiscount.ToString());
                reportViewer1.LocalReport.SetParameters(TotalRefundDiscount);

                ReportParameter TotalCreditRefundDiscount = new ReportParameter("TotalCreditRefundDiscount", totalCreditRefundDiscount.ToString());
                reportViewer1.LocalReport.SetParameters(TotalCreditRefundDiscount);

                ReportParameter ActualAmount = new ReportParameter("ActualAmount", totalReceived.ToString());
                reportViewer1.LocalReport.SetParameters(ActualAmount);

                ReportParameter TotalFOC = new ReportParameter("TotalFOC", totalFOC.ToString());
                reportViewer1.LocalReport.SetParameters(TotalFOC);

                ReportParameter TotalOtherFOC = new ReportParameter("TotalOtherFOC", totalOtherFOC.ToString());
                reportViewer1.LocalReport.SetParameters(TotalOtherFOC);

                ReportParameter TotalMPU = new ReportParameter("TotalMPU", totalMPU.ToString());
                reportViewer1.LocalReport.SetParameters(TotalMPU);

                ReportParameter TotalSale = new ReportParameter("TotalSale", (totalSale + totalCashFromGiftCard).ToString());
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

                ReportParameter HeaderTitle = new ReportParameter("HeaderTitle", "Transaction Summary for " + shopname);
                reportViewer1.LocalReport.SetParameters(HeaderTitle);

                ReportParameter Date = new ReportParameter("Date", " from " + dtpFrom.Value.Date.ToString("dd-MM-yyyy") + " To " + dtpTo.Value.Date.ToString("dd-MM-yyyy"));
                reportViewer1.LocalReport.SetParameters(Date);

                ReportParameter TesterTotal = new ReportParameter("TesterTotal", totalTester.ToString());
                reportViewer1.LocalReport.SetParameters(TesterTotal);


                reportViewer1.RefreshReport();
            }
        }


        #endregion

    }
}
