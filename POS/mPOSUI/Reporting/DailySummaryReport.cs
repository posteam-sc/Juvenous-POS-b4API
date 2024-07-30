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
    public partial class DailySummaryReport : Form
    {
        #region Variable
        List<Transaction> DtransList = new List<Transaction>();
        POSEntities entity = new POSEntities();
        //List<Product> itemList = new List<Product>();
        List<ReportItemSummary> itemList = new List<ReportItemSummary>();
        List<ReportItemSummary> IList = new List<ReportItemSummary>();
        long CashTotal = 0, CreditTotal = 0, CreditRefundAmt = 0, FOCAmount = 0, MPUAmount = 0, TesterAmount = 0, GiftCardAmount = 0, Total = 0, CreditReceive = 0, RefundAmt = 0; long UseGiftAmount = 0; long CashAmtFromGiftCard = 0;
        decimal totalSettlement = 0;
        List<Transaction> AllTranslist = new List<Transaction>();
        List<ReportItemSummary> FinalResultList = new List<ReportItemSummary>();
        Boolean Isstart = false;
        decimal bankSettlementAmt;
        decimal bankRecieveCreditAmt;
        decimal bankRecieveCashAmt;
        decimal oldBankSaleAmt;
        //decimal oldBankSettlementAmt;
        #endregion

        #region Event
        public DailySummaryReport()
        {
            InitializeComponent();
        }

        private void Loc_ItemSummary_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            Utility.BindShop(cboshoplist, true);
            cboshoplist.Text = SettingController.DefaultShop.ShopName;
            Isstart = true;
            Utility.ShopComBo_EnableOrNot(cboshoplist);
            cboshoplist.SelectedIndex = 0;
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

        private void rdbSale_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void gbList_Enter(object sender, EventArgs e)
        {

        }

        private void chkCash_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void chkGiftCard_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void chkMPU_CheckedChanged(object sender, EventArgs e)
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

                DateTime fromDate = dtFrom.Value.Date;
                DateTime toDate = dtTo.Value.Date;
                // bool IsSale = rdbSale.Checked;
                //bool IsCash = chkCash.Checked, IsCredit = chkCredit.Checked, IsFOC = chkFOC.Checked, IsMPU = chkMPU.Checked, IsGiftCard = chkGiftCard.Checked, IsTester = chkTester.Checked;
                bool IsCash = true, IsCredit = true, IsFOC = true, IsMPU = true, IsGiftCard = true, IsTester = true, IsRefund = true;
                CashTotal = 0; CreditTotal = 0; FOCAmount = 0; MPUAmount = 0; TesterAmount = 0; GiftCardAmount = 0; RefundAmt = 0; CreditRefundAmt = 0; Total = 0; CashAmtFromGiftCard = 0;
                IList.Clear();
                itemList.Clear();
                System.Data.Objects.ObjectResult<SelectItemListByDate_Result> resultList;
                //System.Data.Objects.ObjectResult<SelectItemListByDate_Result> FinalResultList ;
                //FinalResultList = null;
                List<Transaction> transList = new List<Transaction>();
                FinalResultList = new List<ReportItemSummary>();
                //if (IsSale)
                //{
                transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit || t.Type == TransactionType.Refund || t.Type == TransactionType.CreditRefund) && (t.IsDeleted == null || t.IsDeleted == false) && ((currentshortcode != "0" && t.Id.Substring(2, 2) == currentshortcode) || (currentshortcode == "0" && 1 == 1)) select t).ToList<Transaction>();
                //}
                //else
                //{
                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Refund || t.Type == TransactionType.CreditRefund) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                //}
                DtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Settlement || t.Type == TransactionType.Prepaid) && (t.IsDeleted == null || t.IsDeleted == false) && ((currentshortcode != "0" && t.Id.Substring(2, 2) == currentshortcode) || (currentshortcode == "0" && 1 == 1)) select t).ToList<Transaction>();

                List<Transaction> BankPrepaidList = (from t in entity.Transactions
                                                       where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Prepaid) && (t.IsDeleted == null || t.IsDeleted == false) && (t.PaymentTypeId >= 501) && ((currentshortcode != "0" && t.Id.Substring(2, 2) == currentshortcode) || (currentshortcode == "0" && 1 == 1))select t).ToList();

                decimal bankPrepaidAmount = 0; decimal useAmt = 0;

                foreach (Transaction tr in BankPrepaidList)
                {
                    bankPrepaidAmount += (decimal)tr.TotalAmount;
                  //  useAmt += tr.UsePrePaidDebts1 == null ? 0 : tr.UsePrePaidDebts1.Sum(x => x.UseAmount).Value;
                }
                //bankPrepaidAmount -= useAmt;
                bankSettlementAmt = Convert.ToDecimal((from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Settlement) && (t.IsDeleted == null || t.IsDeleted == false) && (t.PaymentTypeId >= 501) && ((currentshortcode != "0" && t.Id.Substring(2, 2) == currentshortcode) || (currentshortcode == "0" && 1 == 1)) select t.TotalAmount).Sum()); // TTN Modification
                bankSettlementAmt += bankPrepaidAmount;

                bankRecieveCreditAmt = Convert.ToDecimal((from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.Type == TransactionType.Credit && (t.IsDeleted == null || t.IsDeleted == false) && (t.PaymentTypeId >= 501) && ((currentshortcode != "0" && t.Id.Substring(2, 2) == currentshortcode) || (currentshortcode == "0" && 1 == 1)) select t.RecieveAmount).Sum());
                bankRecieveCashAmt = Convert.ToDecimal((from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.Type == TransactionType.Sale && (t.IsDeleted == null || t.IsDeleted == false) && (t.PaymentTypeId >= 501) && ((currentshortcode != "0" && t.Id.Substring(2, 2) == currentshortcode) || (currentshortcode == "0" && 1 == 1)) select t.RecieveAmount).Sum());
                oldBankSaleAmt = Convert.ToDecimal((from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.Type == TransactionType.Sale && (t.IsDeleted == null || t.IsDeleted == false) && (t.PaymentTypeId == 5) && ((currentshortcode != "0" && t.Id.Substring(2, 2) == currentshortcode) || (currentshortcode == "0" && 1 == 1)) select t.RecieveAmount).Sum());
                //oldBankSettlementAmt = Convert.ToDecimal((from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Settlement || t.Type == TransactionType.Prepaid) && (t.IsDeleted == null || t.IsDeleted == false) && (t.PaymentTypeId >= 501) && ((currentshortcode != "0" && t.Id.Substring(2, 2) == currentshortcode) || (currentshortcode == "0" && 1 == 1)) select t.TotalAmount).Sum());
                resultList = entity.SelectItemListByDate(fromDate, toDate, currentshortcode);
                // int count = resultList.Count();
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
                int totalDiscountRFTransAmount = 0;
                int totalDiscountCrdRFTransAmount = 0;
                //  string[] type = { "Refund", "CreditRefund" };
                string[] type = { "Refund", "CreditRefund" };
                if (IsCash)
                {
                    itemList.AddRange(FinalResultList.Where(x => x.PaymentId == 1 && !type.Contains(x.Remark)).ToList());
                    CashTotal += Convert.ToInt64(transList.Where(x => x.Type == "Sale" && x.PaymentTypeId == 1 && !type.Contains(x.Type)).Sum(x => x.TotalAmount));
                    AllTranslist.AddRange(transList.Where(x => x.Type == "Sale" && x.PaymentTypeId == 1 && !type.Contains(x.Type)).ToList());
                }
                if (IsCredit)
                {
                    //itemList.AddRange(FinalResultList.Where(x => x.PaymentId == "Credit").ToList());
                    CreditTotal += Convert.ToInt64(transList.Where(x => x.Type == "Credit").Sum(x => x.TotalAmount));
                    AllTranslist.AddRange(transList.Where(x => x.Type == "Credit" && !type.Contains(x.Type)).ToList());
                    CreditReceive += Convert.ToInt64(transList.Where(x => x.Type == "Credit" && !type.Contains(x.Type)).Sum(x => x.RecieveAmount));
                }
                if (IsGiftCard)
                {
                    itemList.AddRange(FinalResultList.Where(x => x.PaymentId == 3 && !type.Contains(x.Remark)).ToList());
                    GiftCardAmount += FinalResultList.Where(x => x.PaymentId == 3 && !type.Contains(x.Remark)).Sum(x => x.totalAmount);
                    AllTranslist.AddRange(transList.Where(x => x.PaymentTypeId == 3 && !type.Contains(x.Type)).ToList());
                    UseGiftAmount += Convert.ToInt64(transList.Where(x => x.PaymentTypeId == 3 && !type.Contains(x.Type)).Sum(x => x.GiftCardAmount));
                    //  CashAmtFromGiftCard += Convert.ToInt64(transList.Where(x => x.PaymentTypeId == 3 && !type.Contains(x.Type)).Sum(x => x.TotalAmount+ x.MCDiscountAmt - x.GiftCardAmount));
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
                }
                if (IsRefund)
                {

                    //    RefundAmt += FinalResultList.Where(x => x.Remark=="Refund").Sum(x => x.SellingPrice * x.Qty);
                    totalDiscountRFTransAmount = Convert.ToInt32(transList.Where(x => x.Id.Substring(0, 2) == "RF" && x.Type != "CreditRefund").Sum(x => x.DiscountAmount));
                    totalDiscountCrdRFTransAmount = Convert.ToInt32(transList.Where(x => x.Id.Substring(0, 2) == "RF" && x.Type == "CreditRefund").Sum(x => x.DiscountAmount));
                    RefundAmt += FinalResultList.Where(x => x.Remark == "Refund").Sum(x => x.totalAmount);
                    CreditRefundAmt += FinalResultList.Where(x => x.Remark == "CreditRefund").Sum(x => x.totalAmount);
                    if (totalDiscountRFTransAmount != 0)
                    {
                        RefundAmt -= totalDiscountRFTransAmount;
                    }
                    if (totalDiscountRFTransAmount != 0)
                    {
                        CreditRefundAmt -= totalDiscountCrdRFTransAmount;
                    }
                    AllTranslist.AddRange(transList.Where(x => type.Contains(x.Type)).ToList());
                    // lblPeriod.Text = fromDate.ToString() + " to " + toDate.ToString();
                    // lblNumberofTransaction.Text = transList.Count.ToString();

                    //lblTotalAmount.Text = "";

                    // itemList.AddRange(FinalResultList.Where(x
                }
                if (IsFOC)
                {
                    itemList.AddRange(FinalResultList.Where(x => x.PaymentId == 4).ToList());
                    //  FOCAmount += FinalResultList.Where(x => x.PaymentId == 4).Sum(x => x.totalAmount);
                    FOCAmount += FinalResultList.Where(x => x.IsFOC == true).Sum(x => x.SellingPrice * x.Qty);
                    AllTranslist.AddRange(transList.Where(x => x.PaymentTypeId == 4).ToList());
                }
                if (IsMPU)
                {
                    itemList.AddRange(FinalResultList.Where(x => x.PaymentId >= 501 && x.PaymentId == 5).ToList());
                    MPUAmount += Convert.ToInt64(transList.Where(x => (x.PaymentTypeId >= 501 || x.PaymentTypeId == 5) && x.Type == "Sale").Sum(x => x.RecieveAmount));
                    AllTranslist.AddRange(transList.Where(x => (x.PaymentTypeId >= 501 || x.PaymentTypeId == 5) && x.Type == "Sale").ToList());
                }
                if (IsTester)
                {
                    itemList.AddRange(FinalResultList.Where(x => x.PaymentId == 6).ToList());
                    TesterAmount += FinalResultList.Where(x => x.PaymentId == 6).Sum(x => x.totalAmount);
                    AllTranslist.AddRange(transList.Where(x => x.PaymentTypeId == 6).ToList());
                }

                ShowReportViewer();
            }
        }

        private void ShowReportViewer()
        {
            int shopid = Convert.ToInt32(cboshoplist.SelectedValue);
            string shopname = (from p in entity.Shops where p.Id == shopid select p.ShopName).FirstOrDefault();
            dsReportTemp dsReport = new dsReportTemp();
            //dsReportTemp.ItemListDataTable dtItemReport = (dsReportTemp.ItemListDataTable)dsReport.Tables["LO'c_ItemSummary"];
            dsReportTemp._LO_c_ItemSummaryDataTable dtItemReport = (dsReportTemp._LO_c_ItemSummaryDataTable)dsReport.Tables["LO'c_ItemSummary"];


            FinalResultList = FinalResultList.OrderBy(x => x.Name).ToList();
            foreach (ReportItemSummary p in FinalResultList)
            {
                //dsReportTemp.ItemListRow newRow = dtItemReport.NewItemListRow();
                dsReportTemp._LO_c_ItemSummaryRow newRow = dtItemReport.New_LO_c_ItemSummaryRow();
                newRow.ItemCode = p.Id;
                newRow.Name = p.Name;
                newRow.Size = p.Size;
                newRow.DiscountRate = p.discountrate;
                newRow.Qty = p.Qty.ToString();
                newRow.UnitPrice = p.UnitPrice.ToString();
                newRow.TotalAmount = Convert.ToInt64(p.totalAmount);
                newRow.SellingPrice = Convert.ToInt64(p.SellingPrice);
                newRow.Remark = p.Remark.ToString();
                dtItemReport.Add_LO_c_ItemSummaryRow(newRow);
            }
            Total = CashTotal + CreditTotal + FOCAmount + MPUAmount + GiftCardAmount + TesterAmount;

            //if (rdbSale.Checked)
            //{
            totalSettlement = DtransList.Where(x => x.Type == "Settlement").Sum(x => x.TotalAmount).Value;
            List<Transaction> prepaidList = DtransList.Where(x => x.Type == TransactionType.Prepaid).ToList();
            decimal prepaidAmount = 0; decimal useAmount = 0;
            foreach (Transaction tr in prepaidList)
            {
                prepaidAmount += (decimal)tr.RecieveAmount;
              //  useAmount += tr.UsePrePaidDebts1 == null ? 0 : tr.UsePrePaidDebts1.Sum(x => x.UseAmount).Value;
            }
           // prepaidAmount -= useAmount;
            // totalSettlement += DtransList.Where(x => x.Type == "Prepaid").Sum(x => x.RecieveAmount).Value;
            totalSettlement += prepaidAmount;
            //}
            //else
            //{
            //    totalSettlement = 0;
            //}
            //CashTotal += GiftCardAmount;
            //To Find DiscountAmountOfAllTransactions
            decimal OverAllDis = 0;
            decimal OverAllMCDis = 0;
            decimal OverAllDisG = 0;
            decimal OverAllMCDisG = 0;
            decimal CreditMCDis = 0;
            decimal OverAllDisCrd = 0;
            decimal OverAllMCDisCrd = 0;
            decimal OverAllDisMpu = 0;
            decimal OverAllMCDisMpu = 0;
            decimal overallPercentDis = 0;
            decimal overallCreditPercentDis = 0;
            decimal overallCardPercentDis = 0;
            decimal overallGiftAmt = 0;

            decimal CreditMCDisCrd = 0;
            int cashmcdiscount = 0;
            int totalDiscountRFTransAmount = 0;
            int totalDiscountCrdRFTransAmount = 0;
            Int64 _creditoveralldiscount = 0;
            totalDiscountRFTransAmount = Convert.ToInt32(AllTranslist.Where(x => x.Id.Substring(0, 2) == "RF" && x.Type != "CreditRefund").Sum(x => x.DiscountAmount));
            totalDiscountCrdRFTransAmount = Convert.ToInt32(AllTranslist.Where(x => x.Id.Substring(0, 2) == "RF" && x.Type == "CreditRefund").Sum(x => x.DiscountAmount));
            //zp edited for creditRefund discount
            List<Transaction> creAllTranslist = AllTranslist.Where(x => x.Type == "Credit").ToList();
            List<Transaction> otherallTranslist = AllTranslist.Where(x => x.Type != "Credit").ToList();

            foreach (Transaction t in otherallTranslist)
            {

                List<TransactionDetail> tdList = new List<TransactionDetail>();
                // tdList = t.TransactionDetails.ToList();
                tdList = t.TransactionDetails.Where(x => x.TransactionId.Substring(0, 2) != "RF").ToList();
                List<GiftSystemInTransaction> gtList = new List<GiftSystemInTransaction>();
                gtList = t.GiftSystemInTransactions.Where(x => x.TransactionId.Substring(0, 2) != "RF" && x.GiftSystem.DiscountPercentForTransaction != 0).ToList();
                int itemDis = 0;

                foreach (TransactionDetail td in tdList)
                {
                    itemDis += Convert.ToInt32((td.UnitPrice * (td.DiscountRate / 100)) * td.Qty);

                }


                if (t.PaymentTypeId >= 501)
                {
                    MPUAmount -= Convert.ToInt64(t.DiscountAmount - itemDis);
                    OverAllDisMpu += Convert.ToDecimal(t.DiscountAmount - itemDis);
                    overallCardPercentDis += itemDis;
                    if ((int)(t.MCDiscountAmt == null ? 0 : t.MCDiscountAmt) != 0)
                    {
                        MPUAmount -= Convert.ToInt64(t.MCDiscountAmt);
                        OverAllMCDisMpu += Convert.ToDecimal(t.MCDiscountAmt);
                    }
                    else if ((int)(t.BDDiscountAmt == null ? 0 : t.BDDiscountAmt) != 0)
                    {
                        MPUAmount -= Convert.ToInt64(t.BDDiscountAmt);
                        OverAllMCDisMpu += Convert.ToDecimal(t.BDDiscountAmt);
                    }
                    if ((int)(t.GiftCardAmount == null ? 0 : t.GiftCardAmount) != 0)
                    {
                        OverAllDisG += Convert.ToDecimal(t.GiftCardAmount);
                    }
                }
                else if (t.PaymentTypeId == 5)
                {
                    MPUAmount -= Convert.ToInt64(t.DiscountAmount - itemDis);
                    OverAllDisMpu += Convert.ToDecimal(t.DiscountAmount - itemDis);
                    overallCardPercentDis += itemDis;
                    if ((int)(t.MCDiscountAmt == null ? 0 : t.MCDiscountAmt) != 0)
                    {
                        MPUAmount -= Convert.ToInt64(t.MCDiscountAmt);
                        OverAllMCDisMpu += Convert.ToDecimal(t.MCDiscountAmt);
                    }
                    else if ((int)(t.BDDiscountAmt == null ? 0 : t.BDDiscountAmt) != 0)
                    {
                        MPUAmount -= Convert.ToInt64(t.BDDiscountAmt);
                        OverAllMCDisMpu += Convert.ToDecimal(t.BDDiscountAmt);
                    }
                }
                else if (t.PaymentTypeId == 3)
                {
                    OverAllDisG += Convert.ToDecimal(t.DiscountAmount - itemDis);
                    overallPercentDis += itemDis;
                    if ((int)(t.MCDiscountAmt == null ? 0 : t.MCDiscountAmt) != 0)
                    {
                        OverAllMCDisG += Convert.ToDecimal(t.MCDiscountAmt);
                    }
                    else if ((int)(t.BDDiscountAmt == null ? 0 : t.BDDiscountAmt) != 0)
                    {
                        OverAllMCDisG += Convert.ToDecimal(t.BDDiscountAmt);
                    }
                }
                else
                {
                    OverAllDis += Convert.ToDecimal(t.DiscountAmount - itemDis);
                    overallPercentDis += itemDis;

                    if ((int)(t.MCDiscountAmt == null ? 0 : t.MCDiscountAmt) != 0)
                    {
                        OverAllMCDis += Convert.ToDecimal(t.MCDiscountAmt);
                    }
                    else if ((int)(t.BDDiscountAmt == null ? 0 : t.BDDiscountAmt) != 0)
                    {
                        OverAllMCDis += Convert.ToDecimal(t.BDDiscountAmt);
                    }
                    if ((int)(t.GiftCardAmount == null ? 0 : t.GiftCardAmount) != 0)
                    {
                        OverAllDisG += Convert.ToDecimal(t.GiftCardAmount);
                    }
                }


            }

            //var creditData = (from a in AllTranslist where a.Type == "Credit" select a).ToList();
            //Int64 _totalMCDisocunt = Convert.ToInt64(creditData.Sum(x => x.MCDiscountAmt));
            //Int64 _totalBDDiscount = Convert.ToInt64(creditData.Sum(x => x.BDDiscountAmt));
            //_creditoveralldiscount = Convert.ToInt64(creditData.Sum(x => x.DiscountAmount));
            //CreditMCDis = _totalMCDisocunt + _totalBDDiscount;
            //OverAllDis -= _creditoveralldiscount;
            //cashmcdiscount = Convert.ToInt32((OverAllMCDis) - CreditMCDis);

            foreach (Transaction t in creAllTranslist)
            {

                List<TransactionDetail> tdList = new List<TransactionDetail>();
                // tdList = t.TransactionDetails.ToList();
                tdList = t.TransactionDetails.Where(x => x.TransactionId.Substring(0, 2) != "RF").ToList();
                int itemDis = 0;
                foreach (TransactionDetail td in tdList)
                {
                    itemDis += Convert.ToInt32((td.UnitPrice * (td.DiscountRate / 100)) * td.Qty);
                }
                OverAllDisCrd += Convert.ToDecimal(t.DiscountAmount - itemDis);
                overallCreditPercentDis += itemDis;

                if ((int)(t.MCDiscountAmt == null ? 0 : t.MCDiscountAmt) != 0)
                {
                    OverAllMCDisCrd += Convert.ToDecimal(t.MCDiscountAmt);
                }
                else if ((int)(t.BDDiscountAmt == null ? 0 : t.BDDiscountAmt) != 0)
                {
                    OverAllMCDisCrd += Convert.ToDecimal(t.BDDiscountAmt);
                }


            }
            cashmcdiscount = Convert.ToInt32((OverAllMCDis));

            if (totalDiscountRFTransAmount != 0)
            {
                OverAllDis -= totalDiscountRFTransAmount;
            }

            // decimal actualAmount = (Convert.ToDecimal(CashTotal + CreditReceive) - (OverAllDis) - (OverAllMCDis)) + totalSettlement;
            //     decimal actualAmount = (Convert.ToDecimal(CashTotal + CreditReceive + CashAmtFromGiftCard) + (totalSettlement + CreditMCDis) - (OverAllDis) - (OverAllMCDis));
            //decimal actualAmount = (Convert.ToDecimal(CashTotal + CreditReceive + CashAmtFromGiftCard) + (totalSettlement)-(OverAllDis));
            decimal actualAmount = (Convert.ToDecimal(CashTotal + CreditReceive + CashAmtFromGiftCard) + (totalSettlement) - (OverAllDis) - (OverAllMCDis));
            decimal allPaymentDis = OverAllDis + OverAllDisCrd + OverAllDisMpu + OverAllMCDis + OverAllMCDisCrd + OverAllMCDisG + OverAllMCDisMpu;
            ReportDataSource rds = new ReportDataSource("ItemSummary", dsReport.Tables["LO'c_ItemSummary"]);
            string reportPath = Application.StartupPath + "\\Reports\\DailySummary.rdlc";
            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            ReportParameter ItemReportTitle = new ReportParameter("ItemReportTitle", gbList.Text + " for " + shopname);
            reportViewer1.LocalReport.SetParameters(ItemReportTitle);

            ReportParameter Date = new ReportParameter("Date", " from " + dtFrom.Value.Date.ToString("dd-MM-yyyy") + " To " + dtTo.Value.Date.ToString("dd-MM-yyyy"));
            reportViewer1.LocalReport.SetParameters(Date);

            ReportParameter TotalAmount = new ReportParameter("TotalAmount", (Total - FOCAmount).ToString());
            reportViewer1.LocalReport.SetParameters(TotalAmount);

            //ReportParameter CreditAmount = new ReportParameter("CreditAmount", Convert.ToInt64((CreditTotal - CreditReceive - Convert.ToInt32(OverAllMCDisCrd) - OverAllDisCrd)).ToString());
            //reportViewer1.LocalReport.SetParameters(CreditAmount);
            ReportParameter CreditAmount = new ReportParameter("CreditAmount", Convert.ToInt64((CreditTotal - CreditReceive - Convert.ToInt32(OverAllMCDisCrd) - OverAllMCDisCrd)).ToString());
            reportViewer1.LocalReport.SetParameters(CreditAmount);

            //ReportParameter CashAmount = new ReportParameter("CashAmount", (CashTotal + CreditReceive + CashAmtFromGiftCard ).ToString());
            //reportViewer1.LocalReport.SetParameters(CashAmount);
            ReportParameter CashAmount = new ReportParameter("CashAmount", ((CashTotal + CreditReceive + CashAmtFromGiftCard + totalSettlement) - (bankSettlementAmt + bankRecieveCreditAmt)).ToString());
            reportViewer1.LocalReport.SetParameters(CashAmount);
            ReportParameter OverallPDis = new ReportParameter("OverallPDis", (overallPercentDis + overallCreditPercentDis + overallCardPercentDis).ToString());
            reportViewer1.LocalReport.SetParameters(OverallPDis);
            ReportParameter OverallCardPDiscount = new ReportParameter("OverallCardPDiscount", overallCardPercentDis.ToString());
            reportViewer1.LocalReport.SetParameters(OverallCardPDiscount);

            ReportParameter DisAmount = new ReportParameter("DisAmount", OverAllDis.ToString());
            reportViewer1.LocalReport.SetParameters(DisAmount);
            ReportParameter MemberCardDiscount = new ReportParameter("MemberCardDiscount", Convert.ToInt32(OverAllMCDis).ToString());
            reportViewer1.LocalReport.SetParameters(MemberCardDiscount);
            ReportParameter CreditDiscountAmt = new ReportParameter("CreditDiscountAmt", OverAllDisCrd.ToString());
            reportViewer1.LocalReport.SetParameters(CreditDiscountAmt);

            ReportParameter CreditMemberDis = new ReportParameter("CreditMemberDis", Convert.ToInt32(OverAllMCDisCrd).ToString());
            reportViewer1.LocalReport.SetParameters(CreditMemberDis);

            ReportParameter MPUDiscountAmt = new ReportParameter("MPUDiscountAmt", OverAllDisMpu.ToString());
            reportViewer1.LocalReport.SetParameters(MPUDiscountAmt);


            ReportParameter MPUMemberDis = new ReportParameter("MPUMemberDis", Convert.ToInt32(OverAllMCDisMpu).ToString());
            reportViewer1.LocalReport.SetParameters(MPUMemberDis);

            ReportParameter UsedGiftAmount = new ReportParameter("UsedGiftAmount", UseGiftAmount.ToString());
            reportViewer1.LocalReport.SetParameters(UsedGiftAmount);
            ReportParameter GCDiscountAmt = new ReportParameter("GCDiscountAmt", OverAllDisG.ToString());
            reportViewer1.LocalReport.SetParameters(GCDiscountAmt);


            ReportParameter GCMemberAmt = new ReportParameter("GCMemberAmt", Convert.ToInt32(OverAllMCDisG).ToString());
            reportViewer1.LocalReport.SetParameters(GCMemberAmt);

            ReportParameter FOC = new ReportParameter("FOC", FOCAmount.ToString());
            reportViewer1.LocalReport.SetParameters(FOC);

            ReportParameter MPU = new ReportParameter("MPU", (bankRecieveCreditAmt + bankRecieveCashAmt + bankSettlementAmt + oldBankSaleAmt).ToString());
            reportViewer1.LocalReport.SetParameters(MPU);

            ReportParameter Tester = new ReportParameter("Tester", TesterAmount.ToString());
            reportViewer1.LocalReport.SetParameters(Tester);

            ReportParameter TotalSettlement = new ReportParameter("TotalSettlement", totalSettlement.ToString());
            reportViewer1.LocalReport.SetParameters(TotalSettlement);

            ReportParameter ActualAmount = new ReportParameter("ActualAmount", (Convert.ToInt32(actualAmount + oldBankSaleAmt + bankSettlementAmt + oldBankSaleAmt).ToString()));
            reportViewer1.LocalReport.SetParameters(ActualAmount);

            ReportParameter TotalRefund = new ReportParameter("TotalRefund", RefundAmt.ToString());
            reportViewer1.LocalReport.SetParameters(TotalRefund);

            ReportParameter CreditRefund = new ReportParameter("CreditRefund", CreditRefundAmt.ToString());
            reportViewer1.LocalReport.SetParameters(CreditRefund);
            //ReportParameter CashInHand = new ReportParameter("CashInHand", (actualAmount - RefundAmt ).ToString());
            //reportViewer1.LocalReport.SetParameters(CashInHand);
            //ReportParameter CashInHand = new ReportParameter("CashInHand", (((Total + totalSettlement) - MPUAmount) - (FOCAmount + UseGiftAmount + (CreditTotal - CreditReceive - Convert.ToInt32(OverAllMCDisCrd) - OverAllDisCrd - OverAllMCDisCrd))).ToString());
            //reportViewer1.LocalReport.SetParameters(CashInHand);
            //ReportParameter CashInHand = new ReportParameter("CashInHand", (Total -FOCAmount).ToString());
            //reportViewer1.LocalReport.SetParameters(CashInHand);
            reportViewer1.RefreshReport();
        }
        #endregion


    }
}
