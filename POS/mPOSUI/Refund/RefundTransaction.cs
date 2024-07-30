using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS.APP_Data;

namespace POS
{
    public partial class RefundTransaction : Form
    {
        #region Variables

        public string transactionId { get; set; }

        private POSEntities entity = new POSEntities();

        public int DiscountAount { get; set; }

        Transaction currentTransaction;

        int discount = 0;

        public decimal? memCradDiscount = 0;

        int Qty = 0;

        List<Stock_Transaction> productList = new List<Stock_Transaction>();

        List<Transaction> _RefundTranList = new List<Transaction>();

        int UnCheckedCount = 0;

        public Boolean IsCash = false;
        Boolean IsStatus = true;
        //List<long?> _productIdList=new List<long?>();
        #endregion

        #region Events

        public RefundTransaction()
        {
            InitializeComponent();
        }

        private void RefundTransaction_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            LoadData();

        }

        private void Visible_Refund(Boolean v)
        {
            lblTotalREfundAmount.Visible = v;
            lblRefund.Visible = v;
        }

        private void LoadData()
        {
            entity = new POSEntities();


            currentTransaction = (from c in entity.Transactions where c.Id == transactionId && c.IsDeleted == false select c).FirstOrDefault();
            dgvItemLists.AutoGenerateColumns = false;
            dgvItemLists.Rows.Clear();

            foreach (TransactionDetail td in currentTransaction.TransactionDetails)
            {
                if (td.IsDeleted != true)
                {
                    int qty = Convert.ToInt32(td.Qty);

                    for (int count = 0; count < qty; count++)
                    {
                        dgvItemLists.AllowUserToAddRows = true;
                        DataGridViewRow row = (DataGridViewRow)dgvItemLists.Rows[count].Clone();

                        int _productId = Convert.ToInt32(td.ProductId);
                        //_productIdList.Add(_productId);
                        row.Cells[0].Value = false;
                        row.Cells[1].Value = td.Product.ProductCode;
                        row.Cells[2].Value = td.Product.Name;
                        row.Cells[3].Value = 1;
                        row.Cells[4].Value = td.UnitPrice;
                        row.Cells[5].Value = td.DiscountRate;
                        row.Cells[6].Value = td.TaxRate;
                        row.Cells[7].Value = getActualCost(td.Product, Convert.ToInt32(td.DiscountRate), (int)td.UnitPrice);
                        row.Cells[8].Value = td.ProductId;
                        row.Cells[9].Value = td.Id.ToString();
                        row.Cells[10].Value = td.ConsignmentPrice == null ? 0 : td.ConsignmentPrice;
                        row.Cells[11].Value = count + 1;

                        //row.Cells[12].Value = td.IsFOC;
                        if (td.IsFOC == true)
                        {
                            row.Cells[12].Value = "FOC";
                        }
                        else
                        {
                            row.Cells[12].Value = "";
                        }

                        if (td.IsFOC == true)
                        {
                            row.Cells[0].ReadOnly = true;
                            row.Cells[0].Style.BackColor = Color.LightGray;

                        }
                        dgvItemLists.Rows.Add(row);



                        _RefundTranList = entity.Transactions.Where(x => x.Type == TransactionType.CreditRefund || x.Type == TransactionType.Refund).Where(x => x.ParentId == currentTransaction.Id).ToList().Where(x => x.IsDeleted != true).ToList();


                        if (_RefundTranList.Count > 0)
                        {
                            var _refundIdList = _RefundTranList.Select(x => x.Id).ToList();

                            var _tempproductIdList = entity.TransactionDetails.Where(x => _refundIdList.Contains(x.TransactionId)).Select(x => x.ProductId).ToList();

                            int productId = Convert.ToInt32(_tempproductIdList.Where(x => x == _productId).Select(x => x).FirstOrDefault());

                            int _refundTotalQty = Convert.ToInt32(_tempproductIdList.Where(x => x == _productId).Select(x => x).Count());


                            int tempcount = Convert.ToInt32(row.Cells[11].Value);
                            if (productId != 0)
                            {
                                if (tempcount <= _refundTotalQty)
                                {
                                    row.Cells[0].ReadOnly = true;
                                    row.Cells[0].Style.BackColor = Color.LightGray;

                                }
                                else
                                {
                                    if (td.IsFOC == true)
                                    {
                                        row.Cells[0].ReadOnly = true;
                                        row.Cells[0].Style.BackColor = Color.LightGray;

                                    }
                                    else
                                    {
                                        row.Cells[0].ReadOnly = false;
                                        row.Cells[0].Style.BackColor = Color.White;
                                        UnCheckedCount += 1;
                                    }

                                }
                            }
                            else
                            {
                                if (td.IsFOC == true)
                                {
                                    row.Cells[0].ReadOnly = true;
                                    row.Cells[0].Style.BackColor = Color.LightGray;

                                }
                                else
                                {
                                    row.Cells[0].ReadOnly = false;
                                    row.Cells[0].Style.BackColor = Color.White;
                                    UnCheckedCount += 1;
                                }
                            }
                        }
                        else
                        {
                            UnCheckedCount += 1;
                        }

                    }

                }
            }

            dgvItemLists.AllowUserToAddRows = false;
            DiscountAount = -1;

            // dgvItemLists.DataSource = currentTransaction.TransactionDetails.ToList();

            dgvRedundedList.DataSource = null;
            dgvRedundedList.AutoGenerateColumns = false;
            //dgvRedundedList.DataSource = entity.Transactions.Where(x => x.Type == TransactionType.CreditRefund || x.Type == TransactionType.Refund).Where(x => x.ParentId == currentTransaction.Id).ToList().Where(x => x.IsDeleted != true).ToList();
            dgvRedundedList.DataSource = _RefundTranList;


            lblSalePerson.Text = currentTransaction.User.Name;
            lblDate.Text = currentTransaction.DateTime.Value.ToString("dd-MM-yyyy");
            lblTime.Text = currentTransaction.DateTime.Value.ToString("hh:mm");

            int settlementReceiveAmt = 0;

            if (IsStatus)
            {
                if (IsCash)
                {
                    Visible_Refund(true);
                    lblCash.Text = currentTransaction.RecieveAmount.ToString();
                }
                else
                {
                    Visible_Refund(true);


                    if (currentTransaction.IsSettlement == true)
                    {
                        settlementReceiveAmt = Convert.ToInt32(entity.Transactions.Where(x => x.Id == currentTransaction.TranVouNos).Select(x => x.RecieveAmount).FirstOrDefault());
                    }

                    var prepaidAmt = Convert.ToInt32(entity.UsePrePaidDebts.Where(x => x.CreditTransactionId == transactionId).Select(x => x.UseAmount).Sum());

                    lblCash.Text = (currentTransaction.RecieveAmount + prepaidAmt + settlementReceiveAmt).ToString();
                }



                lblTotal.Text = (currentTransaction.TotalAmount).ToString();

                discount = 0;
                foreach (TransactionDetail td in currentTransaction.TransactionDetails)
                {
                    discount += Convert.ToInt32(((td.UnitPrice) * (td.DiscountRate / 100)) * td.Qty);
                    //tax += Convert.ToInt32((td.UnitPrice * (td.TaxRate / 100)) * td.Qty);
                }
                lblDiscount.Text = (currentTransaction.DiscountAmount - discount).ToString();

                if (Convert.ToInt32(lblDiscount.Text) < 0)
                {

                    lblDiscount.Text = (Convert.ToInt32(lblDiscount.Text) * -1).ToString();

                }

                if (currentTransaction.MCDiscountPercent != null && Convert.ToInt32(currentTransaction.MCDiscountPercent) != 0)
                {
                    if (currentTransaction.MCDiscountAmt != null && Convert.ToInt32(currentTransaction.MCDiscountAmt) != 0)
                    {
                        lblMemberCardDiscount.Text = Convert.ToInt32(currentTransaction.MCDiscountAmt).ToString();
                    }
                    else
                    {
                        lblMemberCardDiscount.Text = Convert.ToInt32(currentTransaction.BDDiscountAmt).ToString();
                    }
                }
                else
                {
                    lblMemberCardDiscount.Text = "0";
                }

                lblMainTransaction.Text = currentTransaction.Id;
                if (currentTransaction.Type == TransactionType.Credit)
                {
                    lblChangeGivenTitle.Text = "Payable Credit  :";


                    //lblChangeGiven.Text = (currentTransaction.TotalAmount - currentTransaction.RecieveAmount ).ToString();
                    //lblChangeGiven.Text = ((currentTransaction.TotalAmount - currentTransaction.RecieveAmount) -( totalrefund )).ToString();
                    //lblChangeGiven.Text = ((currentTransaction.TotalAmount - Convert.ToInt32(lblCash.Text)) - (totalrefund)).ToString();


                    lblChangeGiven.Text = ((currentTransaction.TotalAmount - Convert.ToInt32(lblCash.Text))).ToString();

                }
                else
                {
                    lblChangeGiven.Text = ((currentTransaction.RecieveAmount - currentTransaction.TotalAmount)).ToString();
                }


                if (Convert.ToInt32(lblChangeGiven.Text) < 0)
                {
                    lblChangeGiven.Text = (Convert.ToInt32(lblChangeGiven.Text) * -1).ToString();
                }

                ////if (currentTransaction.Type == TransactionType.Credit)
                ////{
                ////    if (lblCash.Text == lblChangeGiven.Text)
                ////    {
                ////        lblChangeGivenTitle.Text = "Change Given";
                ////    }
                ////}

                if (Convert.ToInt32(currentTransaction.MCDiscountAmt) != 0 || currentTransaction.MCDiscountAmt != null)
                {
                    memCradDiscount = currentTransaction.MCDiscountAmt;
                }
                else if (Convert.ToInt32(currentTransaction.BDDiscountAmt) != 0 || currentTransaction.BDDiscountAmt != null)
                {
                    memCradDiscount = currentTransaction.BDDiscountAmt;
                }


            }


            var refund = dgvRedundedList.Rows.Cast<DataGridViewRow>()
                         .Select(r => Convert.ToInt32(r.Cells[3].Value)).Sum();

            var refundDiscount = dgvRedundedList.Rows.Cast<DataGridViewRow>()
                      .Select(r => Convert.ToInt32(r.Cells[4].Value)).Sum();
            int totalrefund = 0;

            //if (IsCash)
            //{
            totalrefund = refund - refundDiscount;
            //}
            //else
            //{
            //    totalrefund = refund + refundDiscount;
            //}

            lblRefund.Text = totalrefund.ToString();

        }

        //private decimal getActualCost(Product prod)
        //{
        //    decimal? actualCost = 0;
        //    //decrease discount ammount            
        //    actualCost = prod.Price - ((prod.Price / 100) * prod.DiscountRate);
        //    //add tax ammount            
        //    actualCost = actualCost + ((prod.Price / 100) * prod.Tax.TaxPercent);
        //    return (decimal)actualCost;
        //}

        private decimal getActualCost(Product prod, int discountpercent, int dynamicPrice = 0)
        {
            decimal? actualCost = 0;
            if (dynamicPrice > 0)
            {
                actualCost = dynamicPrice - ((dynamicPrice / 100) * discountpercent);
                //add tax ammount            
                actualCost = actualCost + ((dynamicPrice / 100) * prod.Tax.TaxPercent);
                return (decimal)actualCost;
            }
            else
            {
                actualCost = prod.Price - ((prod.Price / 100) * discountpercent);
                //add tax ammount            
                actualCost = actualCost + ((prod.Price / 100) * prod.Tax.TaxPercent);
                return (decimal)actualCost;
            }
            //decrease discount ammount            

        }

        private void dgvItemLists_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvItemLists.Rows)
            {
                TransactionDetail transactionDetailObj = (TransactionDetail)row.DataBoundItem;
                row.Cells[1].Value = transactionDetailObj.Product.ProductCode;
                row.Cells[2].Value = transactionDetailObj.Product.Name;
                row.Cells[3].Value = transactionDetailObj.Qty;
                row.Cells[4].Value = transactionDetailObj.UnitPrice;
                row.Cells[5].Value = transactionDetailObj.DiscountRate + "%";
                row.Cells[6].Value = transactionDetailObj.TaxRate + "%";
                //row.Cells[7].Value = transactionDetailObj.TotalAmount;
                row.Cells[7].Value = (transactionDetailObj.UnitPrice * transactionDetailObj.Qty) / 100 * transactionDetailObj.DiscountRate;
            }
        }

        private void dgvRedundedList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvRedundedList.Rows)
            {
                Transaction transObj = (Transaction)row.DataBoundItem;
                row.Cells[0].Value = transObj.Id;
                row.Cells[1].Value = transObj.DateTime.Value.ToString("dd-MM-yyyy");
                row.Cells[2].Value = transObj.DateTime.Value.ToString("hh:mm");


                ////////string ParentId = transObj.ParentId;
                ////////if (ParentId !=string.Empty)
                ////////{
                ////////    var tranDetialList = entity.TransactionDetails.Where(x => x.TransactionId == ParentId && _productIdList.OfType<long?>().Contains(x.ProductId)).ToList();

                ////////    int _totalMememberDisAmt = 0;
                ////////    foreach (var tran in tranDetialList)
                ////////    {
                ////////        int _memberDisAmt = Convert.ToInt32(tran.UnitPrice * tran.Qty)/100 * Convert.ToInt32(transObj.MCDiscountPercent);
                ////////        _totalMememberDisAmt += _memberDisAmt;
                ////////    }

                ////////    row.Cells[3].Value = transObj.TotalAmount - _totalMememberDisAmt;
                ////////}
                ////////else
                ////////{
                ////////    row.Cells[3].Value = transObj.TotalAmount;
                ////////}

                row.Cells[3].Value = transObj.TotalAmount;
                row.Cells[4].Value = transObj.DiscountAmount;

            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
           DialogResult result = MessageBox.Show("Are you sure you want to refund these products?", "mPOS", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result.Equals(DialogResult.OK))
            {
                List<TransactionDetail> refundItems = new List<TransactionDetail>();
                int totalRefundAmount = 0;
                int totalDiscountAllItem = 0;



                #region Old Code
                //foreach (DataGridViewRow row in dgvItemLists.Rows)
                //{
                //    if (Convert.ToBoolean(row.Cells[0].Value))
                //    {
                //        TransactionDetail refundDetail = new TransactionDetail();
                //        TransactionDetail transactionDetailObj = (TransactionDetail)row.DataBoundItem;
                //        refundDetail.ProductId = transactionDetailObj.Product.Id;
                //        transactionDetailObj.Product.Qty = transactionDetailObj.Product.Qty + transactionDetailObj.Qty;
                //        refundDetail.Qty = transactionDetailObj.Qty;
                //        refundDetail.UnitPrice = transactionDetailObj.UnitPrice;
                //        refundDetail.DiscountRate = transactionDetailObj.DiscountRate;
                //        refundDetail.TaxRate = transactionDetailObj.TaxRate;
                //        refundDetail.TotalAmount = transactionDetailObj.TotalAmount;
                //        totalRefundAmount += (int)transactionDetailObj.TotalAmount;
                //        totalDiscountAllItem += Convert.ToInt32(((transactionDetailObj.UnitPrice) * (transactionDetailObj.DiscountRate / 100)) * transactionDetailObj.Qty);
                //        refundItems.Add(refundDetail);
                //    }
                //}

                //User refund more than one items

                #endregion

                int _totalMememberDisAmt = 0;
                foreach (DataGridViewRow row in dgvItemLists.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value))
                    {
                        TransactionDetail refundDetail = new TransactionDetail();
                        refundDetail.ProductId = Convert.ToInt32(row.Cells[8].Value);
                        int currentProductId = Convert.ToInt32(row.Cells[8].Value);
                        Product pro = (from p in entity.Products where p.Id == currentProductId select p).FirstOrDefault<Product>();
                        pro.Qty = pro.Qty + Convert.ToInt32(row.Cells[3].Value);
                        refundDetail.Qty = Convert.ToInt32(row.Cells[3].Value);
                        refundDetail.UnitPrice = Convert.ToInt32(row.Cells[4].Value);
                        refundDetail.SellingPrice = Convert.ToInt32(row.Cells[4].Value);
                        refundDetail.IsFOC = false;
                        refundDetail.Product = entity.Products.Where(p => p.Id == refundDetail.ProductId).FirstOrDefault();
                        entity.Entry(pro).State = System.Data.EntityState.Modified;
                        entity.SaveChanges();


                        //calculation member card discount

                        int _memberDisAmt = 0;
                        if (currentTransaction.MCDiscountPercent != null && Convert.ToInt32(currentTransaction.MCDiscountPercent) != 0)
                        {

                            _memberDisAmt = Convert.ToInt32(refundDetail.UnitPrice * refundDetail.Qty) / 100 * Convert.ToInt32(currentTransaction.MCDiscountPercent);
                            _totalMememberDisAmt += _memberDisAmt;
                            //zp edited

                        }
                        else
                        {
                            _totalMememberDisAmt = 0;
                        }

                        //save in stocktransaction

                        Stock_Transaction st = new Stock_Transaction();
                        st.ProductId = pro.Id;
                        Qty = Convert.ToInt32(row.Cells[3].Value);
                        st.Refund = Qty;
                        productList.Add(st);
                        Qty = 0;

                        string DiscountRate = row.Cells[5].Value.ToString();
                        string TaxRate = row.Cells[6].Value.ToString();

                        refundDetail.DiscountRate = Convert.ToDecimal(DiscountRate.Replace("%", ""));
                        refundDetail.TaxRate = Convert.ToDecimal(TaxRate.Replace("%", ""));
                        refundDetail.TotalAmount = Convert.ToInt32(row.Cells[7].Value) - _memberDisAmt;
                        totalRefundAmount += Convert.ToInt32(row.Cells[7].Value);
                        totalDiscountAllItem += Convert.ToInt32(((Convert.ToInt32(row.Cells[4].Value)) * (Convert.ToDecimal(DiscountRate.Replace("%", "")) / 100)) * Convert.ToInt32(row.Cells[3].Value));
                        refundDetail.Id = Convert.ToInt32(row.Cells[9].Value.ToString());

                        refundDetail.ConsignmentPrice = Convert.ToInt32(row.Cells[10].Value);
                        refundDetail.IsDeleted = false;
                        refundItems.Add(refundDetail);


                    }
                }


                if (refundItems.Count > 0)
                {
                    string resultId = string.Empty;
                    string rId = string.Empty;
                    long CurrentDebt = 0;
                    int cusId;
                    int MemberTypeId;
                    cusId = (int)currentTransaction.CustomerId;
                    MemberTypeId = Convert.ToInt32(entity.Customers.Where(x => x.Id == cusId).Select(x => x.MemberTypeID).FirstOrDefault());
                    if (currentTransaction.PaymentTypeId == 2 && currentTransaction.IsPaid == false)
                    {
                        #region Credit Refund

                        CurrentDebt = Convert.ToInt64(entity.Transactions.Where(x => x.PaymentType.Name == "Credit").Where(x => x.Id == currentTransaction.Id).Where(x => x.IsPaid == false).Sum(x => x.TotalAmount - x.RecieveAmount));
                        if (currentTransaction.UsePrePaidDebts != null)
                        {
                            CurrentDebt -= (long)currentTransaction.UsePrePaidDebts.Sum(x => x.UseAmount);
                        }
                        Transaction DiscountTrans = entity.Transactions.Where(x => x.PaymentType.Name == "Credit").Where(x => x.Id == currentTransaction.Id).Where(x => x.IsPaid == false).FirstOrDefault();
                        //long Discount = (long)DiscountTrans.TransactionDetails.Sum(x => (x.UnitPrice * (x.DiscountRate / 100))).Value;
                        //CurrentDebt -= Convert.ToInt64(currentTransaction.DiscountAmount - discount);
                        List<Transaction> DebtList = entity.Transactions.Where(x => x.CustomerId == currentTransaction.CustomerId).ToList();
                        long DebtTotal = Convert.ToInt64(DebtList.Where(x => x.PaymentType.Name == "Credit").Where(x => x.IsPaid == false).Sum(x => x.TotalAmount - x.RecieveAmount));
                        List<Transaction> prePaidTranList = entity.Transactions.Where(tras => tras.Type == TransactionType.Prepaid).Where(x => x.CustomerId == currentTransaction.CustomerId).Where(trans => trans.IsActive == false).ToList();
                        long PrePaidamount = 0;
                        foreach (Transaction PrePaidDebtTrans in prePaidTranList)
                        {
                            int useAmount = (PrePaidDebtTrans.UsePrePaidDebts1 == null) ? 0 : (int)PrePaidDebtTrans.UsePrePaidDebts1.Sum(x => x.UseAmount);
                            PrePaidamount += (long)PrePaidDebtTrans.TotalAmount - useAmount;
                        }
                        long CreditRefundAmount = Convert.ToInt64(entity.Transactions.Where(x => x.ParentId == currentTransaction.Id).Where(x => x.Type == TransactionType.CreditRefund).Sum(x => x.RecieveAmount));
                        CurrentDebt -= CreditRefundAmount;

                        if (totalRefundAmount <= ((currentTransaction.TotalAmount + currentTransaction.DiscountAmount) - totalDiscountAllItem))
                        {

                            if (currentTransaction.DiscountAmount - discount > 0)
                            {
                                if (DiscountAount < 0)
                                {
                                    //if (UnCheckedCount == 1)
                                    //{
                                    RefundDiscount newForm = new RefundDiscount();
                                    newForm.ShowDialog();
                                    return;
                                    //}
                                    //else
                                    //{
                                    //    DiscountAount = 0;
                                    //}
                                }
                            }
                            else
                            {
                                DiscountAount = 0;
                            }
                            // refund amount is smaller than current debt
                            if ((totalRefundAmount - DiscountAount) < CurrentDebt)
                            {

                                //    System.Data.Objects.ObjectResult<String> Id = entity.InsertRefundTransaction(DateTime.Now, MemberShip.UserId, MemberShip.CounterId, TransactionType.CreditRefund, true, true, 1, 0, DiscountAount, refundItems.Sum(refund => refund.TotalAmount) - _totalMememberDisAmt, refundItems.Sum(refund => refund.TotalAmount) - DiscountAount - _totalMememberDisAmt, transactionId, null, cusId, SettingController.DefaultShop.Id, MemberTypeId);
                                System.Data.Objects.ObjectResult<String> Id = entity.InsertRefundTransaction(DateTime.Now, MemberShip.UserId, MemberShip.CounterId, TransactionType.CreditRefund, true, true, 1, 0, DiscountAount, refundItems.Sum(refund => refund.TotalAmount), refundItems.Sum(refund => refund.TotalAmount) - DiscountAount, transactionId, null, cusId, SettingController.DefaultShop.Id, MemberTypeId);
                                resultId = Id.FirstOrDefault().ToString();
                                CurrentDebt -= (totalRefundAmount - DiscountAount);
                                if (PrePaidamount > 0)
                                {
                                    if (CurrentDebt <= PrePaidamount)
                                    {
                                        Transaction CreditT = (from tr in entity.Transactions where tr.Id == currentTransaction.Id select tr).FirstOrDefault<Transaction>();
                                        CreditT.IsPaid = true;
                                        entity.Entry(CreditT).State = EntityState.Modified;
                                        entity.SaveChanges();
                                        foreach (Transaction PrePaidDebtTrans in prePaidTranList)
                                        {
                                            long CurrentPrePaid = 0;

                                            int useAmount = (PrePaidDebtTrans.UsePrePaidDebts1 == null) ? 0 : (int)PrePaidDebtTrans.UsePrePaidDebts1.Sum(x => x.UseAmount);
                                            CurrentPrePaid = (long)PrePaidDebtTrans.TotalAmount - useAmount;
                                            if (CurrentDebt >= CurrentPrePaid)
                                            {
                                                PrePaidDebtTrans.IsActive = true;
                                                UsePrePaidDebt usePrePaidDObj = new UsePrePaidDebt();
                                                usePrePaidDObj.UseAmount = (int)CurrentPrePaid;
                                                usePrePaidDObj.PrePaidDebtTransactionId = PrePaidDebtTrans.Id;
                                                usePrePaidDObj.CreditTransactionId = currentTransaction.Id;
                                                usePrePaidDObj.CashierId = MemberShip.UserId;
                                                usePrePaidDObj.CounterId = MemberShip.CounterId;
                                                entity.UsePrePaidDebts.Add(usePrePaidDObj);
                                                entity.SaveChanges();
                                                CurrentDebt -= CurrentPrePaid;
                                            }
                                            else
                                            {
                                                UsePrePaidDebt usePrePaidDObj = new UsePrePaidDebt();
                                                usePrePaidDObj.UseAmount = (int)CurrentDebt;
                                                usePrePaidDObj.PrePaidDebtTransactionId = PrePaidDebtTrans.Id;
                                                usePrePaidDObj.CreditTransactionId = currentTransaction.Id;
                                                usePrePaidDObj.CashierId = MemberShip.UserId;
                                                usePrePaidDObj.CounterId = MemberShip.CounterId;
                                                entity.UsePrePaidDebts.Add(usePrePaidDObj);
                                                entity.SaveChanges();
                                                CurrentDebt = 0;
                                            }
                                        }
                                    }
                                }
                            }
                            //refundAmount is equal current debet
                            else if ((totalRefundAmount - DiscountAount) == CurrentDebt)
                            {

                                //  System.Data.Objects.ObjectResult<String> Id = entity.InsertRefundTransaction(DateTime.Now, MemberShip.UserId, MemberShip.CounterId, TransactionType.CreditRefund, true, true, 1, 0, DiscountAount, refundItems.Sum(refund => refund.TotalAmount) - _totalMememberDisAmt, refundItems.Sum(refund => refund.TotalAmount) - DiscountAount - _totalMememberDisAmt, transactionId, null, cusId,SettingController.DefaultShop.Id, MemberTypeId);
                                System.Data.Objects.ObjectResult<String> Id = entity.InsertRefundTransaction(DateTime.Now, MemberShip.UserId, MemberShip.CounterId, TransactionType.CreditRefund, true, true, 1, 0, DiscountAount, refundItems.Sum(refund => refund.TotalAmount), refundItems.Sum(refund => refund.TotalAmount) - DiscountAount, transactionId, null, cusId, SettingController.DefaultShop.Id, MemberTypeId);
                                resultId = Id.FirstOrDefault().ToString();
                                Transaction CreditT = (from tr in entity.Transactions where tr.Id == currentTransaction.Id select tr).FirstOrDefault<Transaction>();
                                CreditT.IsPaid = true;
                                entity.Entry(CreditT).State = EntityState.Modified;
                                entity.SaveChanges();
                            }
                            //if refund amount is greather than current debt, add refund row and credit refund
                            else
                            {

                                //   System.Data.Objects.ObjectResult<String> Id = entity.InsertRefundTransaction(DateTime.Now, MemberShip.UserId, MemberShip.CounterId, TransactionType.Refund, true, true, 1, 0, DiscountAount, (totalRefundAmount - _totalMememberDisAmt) - CurrentDebt, (totalRefundAmount - _totalMememberDisAmt) - CurrentDebt - DiscountAount, transactionId, null, cusId,SettingController.DefaultShop.Id, MemberTypeId);
                                System.Data.Objects.ObjectResult<String> Id = entity.InsertRefundTransaction(DateTime.Now, MemberShip.UserId, MemberShip.CounterId, TransactionType.Refund, true, true, 1, 0, DiscountAount, (totalRefundAmount - _totalMememberDisAmt) - CurrentDebt, (totalRefundAmount - _totalMememberDisAmt) - CurrentDebt - DiscountAount, transactionId, null, cusId, SettingController.DefaultShop.Id, MemberTypeId);
                                resultId = Id.FirstOrDefault().ToString();

                                System.Data.Objects.ObjectResult<String> CreditRefundId = entity.InsertRefundTransaction(DateTime.Now, MemberShip.UserId, MemberShip.CounterId, TransactionType.CreditRefund, true, true, 1, 0, 0, CurrentDebt, CurrentDebt, transactionId, null, cusId, SettingController.DefaultShop.Id, MemberTypeId);
                                rId = CreditRefundId.FirstOrDefault().ToString();
                                //entity = new POSEntities();                           
                                //currentTransaction.IsPaid = true;
                                entity.Paid(true, currentTransaction.Id);
                                //entity.SaveChanges();
                            }
                        }
                        else
                        {
                            CurrentDebt = 0;
                            if (totalRefundAmount < ((currentTransaction.TotalAmount + currentTransaction.DiscountAmount) - totalDiscountAllItem))
                            {

                                if (currentTransaction.DiscountAmount - discount > 0)
                                {
                                    if (DiscountAount < 0)
                                    {
                                        //if (UnCheckedCount == 1)
                                        //{
                                        RefundDiscount newForm = new RefundDiscount();
                                        newForm.ShowDialog();
                                        return;
                                        //}
                                        //else
                                        //{
                                        //    DiscountAount = 0;
                                        //}
                                    }
                                }
                                else
                                {
                                    DiscountAount = 0;
                                }

                                //   System.Data.Objects.ObjectResult<String> Id = entity.InsertRefundTransaction(DateTime.Now, MemberShip.UserId, MemberShip.CounterId, TransactionType.CreditRefund, true, true, 1, 0, DiscountAount, refundItems.Sum(refund => refund.TotalAmount) - _totalMememberDisAmt, refundItems.Sum(refund => refund.TotalAmount) - DiscountAount - _totalMememberDisAmt, transactionId, null, cusId,SettingController.DefaultShop.Id, MemberTypeId);
                                System.Data.Objects.ObjectResult<String> Id = entity.InsertRefundTransaction(DateTime.Now, MemberShip.UserId, MemberShip.CounterId, TransactionType.CreditRefund, true, true, 1, 0, DiscountAount, refundItems.Sum(refund => refund.TotalAmount), refundItems.Sum(refund => refund.TotalAmount) - DiscountAount, transactionId, null, cusId, SettingController.DefaultShop.Id, MemberTypeId);
                                resultId = Id.FirstOrDefault().ToString();
                            }
                            else if (totalRefundAmount == (currentTransaction.TotalAmount + (currentTransaction.DiscountAmount - totalDiscountAllItem)))
                            {

                                //   System.Data.Objects.ObjectResult<String> Id = entity.InsertRefundTransaction(DateTime.Now, MemberShip.UserId, MemberShip.CounterId, TransactionType.CreditRefund, true, true, 1, 0, (currentTransaction.DiscountAmount - totalDiscountAllItem), refundItems.Sum(refund => refund.TotalAmount) - _totalMememberDisAmt, refundItems.Sum(refund => refund.TotalAmount) - (currentTransaction.DiscountAmount - totalDiscountAllItem) - _totalMememberDisAmt, transactionId, null, cusId,SettingController.DefaultShop.Id, MemberTypeId);
                                System.Data.Objects.ObjectResult<String> Id = entity.InsertRefundTransaction(DateTime.Now, MemberShip.UserId, MemberShip.CounterId, TransactionType.CreditRefund, true, true, 1, 0, (currentTransaction.DiscountAmount - totalDiscountAllItem), refundItems.Sum(refund => refund.TotalAmount), refundItems.Sum(refund => refund.TotalAmount) - (currentTransaction.DiscountAmount - totalDiscountAllItem), transactionId, null, cusId, SettingController.DefaultShop.Id, MemberTypeId);
                                resultId = Id.FirstOrDefault().ToString();
                            }

                        }
                        #endregion


                        //List<Transaction> DebtList = entity.Transactions.Where(x => x.CustomerId == currentTransaction.CustomerId).ToList();                  

                    }
                    //Sale Refunds
                    else
                    {

                        if (totalRefundAmount < ((currentTransaction.TotalAmount + currentTransaction.DiscountAmount + memCradDiscount) - totalDiscountAllItem))
                        {

                            if (currentTransaction.DiscountAmount - discount > 0)
                            {
                                if (DiscountAount < 0)
                                {
                                    //if (UnCheckedCount == 1)
                                    //{
                                    RefundDiscount newForm = new RefundDiscount();
                                    newForm.ShowDialog();
                                    return;
                                    //}
                                    //else
                                    //{
                                    //    DiscountAount = 0;
                                    //}
                                }
                            }
                            else
                            {
                                DiscountAount = 0;
                            }

                            //   System.Data.Objects.ObjectResult<String> Id = entity.InsertRefundTransaction(DateTime.Now, MemberShip.UserId, MemberShip.CounterId, TransactionType.Refund, true, true, 1, 0, DiscountAount, refundItems.Sum(refund => refund.TotalAmount) - _totalMememberDisAmt, refundItems.Sum(refund => refund.TotalAmount) - DiscountAount - _totalMememberDisAmt, transactionId, null, cusId, SettingController.DefaultShop.Id, MemberTypeId);
                            System.Data.Objects.ObjectResult<String> Id = entity.InsertRefundTransaction(DateTime.Now, MemberShip.UserId, MemberShip.CounterId, TransactionType.Refund, true, true, 1, 0, DiscountAount, refundItems.Sum(refund => refund.TotalAmount), refundItems.Sum(refund => refund.TotalAmount) - DiscountAount, transactionId, null, cusId, SettingController.DefaultShop.Id, MemberTypeId);
                            resultId = Id.FirstOrDefault().ToString();

                        }
                        else if (totalRefundAmount == (currentTransaction.TotalAmount + (currentTransaction.DiscountAmount + memCradDiscount - totalDiscountAllItem)))
                        {

                            //      System.Data.Objects.ObjectResult<String> Id = entity.InsertRefundTransaction(DateTime.Now, MemberShip.UserId, MemberShip.CounterId, TransactionType.Refund, true, true, 1, 0, (currentTransaction.DiscountAmount - totalDiscountAllItem), refundItems.Sum(refund => refund.TotalAmount) - _totalMememberDisAmt, refundItems.Sum(refund => refund.TotalAmount) - (currentTransaction.DiscountAmount - totalDiscountAllItem) - _totalMememberDisAmt, transactionId, null, cusId, SettingController.DefaultShop.Id, MemberTypeId);

                            System.Data.Objects.ObjectResult<String> Id = entity.InsertRefundTransaction(DateTime.Now, MemberShip.UserId, MemberShip.CounterId, TransactionType.Refund, true, true, 1, 0, (currentTransaction.DiscountAmount - totalDiscountAllItem), refundItems.Sum(refund => refund.TotalAmount), refundItems.Sum(refund => refund.TotalAmount) - (currentTransaction.DiscountAmount - totalDiscountAllItem), transactionId, null, cusId, SettingController.DefaultShop.Id, MemberTypeId);
                            resultId = Id.FirstOrDefault().ToString();

                        }



                    }

                    #region Purchase Delete
                    foreach (TransactionDetail detail in refundItems)
                    {

                        //ZP modify purchase detail with child product Id and order by purchase date ascending
                        if (detail.Product.IsWrapper == true)
                        {
                            List<WrapperItem> wList = detail.Product.WrapperItems.ToList();
                            foreach (WrapperItem w in wList)
                            {

                                Product wpObj = (from p in entity.Products where p.Id == w.ChildProductId select p).FirstOrDefault();

                                List<APP_Data.PurchaseDetailInTransaction> puInTranDetail = (from p in entity.PurchaseDetailInTransactions where p.TransactionDetailId == detail.Id && p.ProductId == wpObj.Id orderby p.Id descending select p).ToList();
                                if (puInTranDetail.Count > 0)
                                {
                                    foreach (PurchaseDetailInTransaction p in puInTranDetail)
                                    {
                                        PurchaseDetail pud = entity.PurchaseDetails.Where(x => x.Id == p.PurchaseDetailId).FirstOrDefault();
                                        if (pud != null)
                                        {
                                            if (pud.CurrentQy <= pud.Qty && p.Qty > 0)
                                            {
                                                pud.CurrentQy = pud.CurrentQy + (w.ChildQty * detail.Qty);
                                                p.Qty = p.Qty - (w.ChildQty * detail.Qty);
                                                entity.Entry(pud).State = EntityState.Modified;
                                                entity.Entry(p).State = EntityState.Modified;
                                                entity.SaveChanges();

                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            List<APP_Data.PurchaseDetailInTransaction> puInTranDetail = (from p in entity.PurchaseDetailInTransactions where p.TransactionDetailId == detail.Id && p.ProductId == detail.ProductId orderby p.Id descending select p).ToList();
                            if (puInTranDetail.Count > 0)
                            {
                                foreach (PurchaseDetailInTransaction p in puInTranDetail)
                                {
                                    PurchaseDetail pud = entity.PurchaseDetails.Where(x => x.Id == p.PurchaseDetailId).FirstOrDefault();
                                    if (pud != null)
                                    {
                                        if (pud.CurrentQy <= pud.Qty && p.Qty > 0)
                                        {
                                            pud.CurrentQy = pud.CurrentQy + detail.Qty;
                                            p.Qty = p.Qty - detail.Qty;
                                            entity.Entry(pud).State = EntityState.Modified;
                                            entity.Entry(p).State = EntityState.Modified;
                                            entity.SaveChanges();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    entity = new POSEntities();

                    //Normal Refund's Detail Transactions List
                    Transaction insertedTransaction = (from trans in entity.Transactions where trans.Id == resultId select trans).FirstOrDefault<Transaction>();
                    foreach (TransactionDetail detail in refundItems)
                    {
                        detail.Product = (from prod in entity.Products where prod.Id == (long)detail.ProductId select prod).FirstOrDefault();
                        //detail.Product.Qty = detail.Product.Qty + detail.Qty;

                        if (detail.Product.IsWrapper == true)
                        {
                            List<WrapperItem> wList = detail.Product.WrapperItems.ToList();
                            if (wList.Count > 0)
                            {
                                foreach (WrapperItem w in wList)
                                {
                                    Product wpObj = (from p in entity.Products where p.Id == w.ChildProductId select p).FirstOrDefault();
                                    wpObj.Qty = wpObj.Qty + w.ChildQty;


                                    Stock_Transaction stwp = new Stock_Transaction();
                                    stwp.ProductId = w.ChildProductId;
                                    Qty = Convert.ToInt32(w.ChildQty * detail.Qty);
                                    stwp.Refund = Qty;
                                    productList.Add(stwp);

                                    Qty = 0;
                                }
                            }
                        }
                        detail.IsConsignmentPaid = false;
                        insertedTransaction.TransactionDetails.Add(detail);
                    }
                    insertedTransaction.IsDeleted = false;

                    entity.SaveChanges();

                    entity = new POSEntities();
                    //Credit refund's Detail Transactions List
                    Transaction ITrans = new Transaction();
                    if (rId != string.Empty)
                    {
                        ITrans = (from trans in entity.Transactions where trans.Id == rId select trans).FirstOrDefault<Transaction>();
                        foreach (TransactionDetail detail in refundItems)
                        {
                            TransactionDetail rftDetail = new TransactionDetail();
                            rftDetail.ProductId = detail.ProductId;
                            rftDetail.Qty = detail.Qty;
                            rftDetail.TaxRate = detail.TaxRate;
                            rftDetail.TotalAmount = detail.TotalAmount;
                            rftDetail.TransactionId = detail.TransactionId;
                            rftDetail.UnitPrice = detail.UnitPrice;
                            rftDetail.SellingPrice = detail.SellingPrice;
                            rftDetail.IsFOC = detail.IsFOC;
                            rftDetail.DiscountRate = detail.DiscountRate;
                            rftDetail.IsConsignmentPaid = false;
                            ITrans.TransactionDetails.Add(rftDetail);
                        }
                    }


                    var tranList = (from t in entity.Transactions where t.Id == lblMainTransaction.Text select t).FirstOrDefault();

                    tranList.ParentId = resultId;
                    entity.Entry(tranList).State = System.Data.EntityState.Modified;
                    entity.SaveChanges();


                    //save in stocktransaction
                    Save_RefundQty_ToStockTransaction(productList, Convert.ToDateTime(currentTransaction.DateTime));
                    productList.Clear();


                    if (DiscountAount == -1)
                    {
                        DiscountAount = 0;
                    }
                    int totalAmt = Convert.ToInt32(refundItems.Sum(x => x.UnitPrice)) - totalDiscountAllItem - DiscountAount;
                    Update_Settlement(lblMainTransaction.Text, totalAmt);
                    IsStatus = false;
                    MessageBox.Show("Refund process completed!");
                    //  this.Dispose();
                    LoadData();


                }
                else
                {
                    MessageBox.Show("Please choose at least one item to refund!");
                }
            }
            else
            {
                DiscountAount = -1;
            }
        }

        private void dgvRedundedList_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                string currentTransactionId = dgvRedundedList.Rows[e.RowIndex].Cells[0].Value.ToString();
                if (e.ColumnIndex == 5)
                {
                    RefundDetail newForm = new RefundDetail();
                    newForm.transactionId = currentTransactionId;
                    newForm.IsRefund = false;
                    newForm.ShowDialog();
                }
            }
        }

        #endregion

        #region Function
        private void Update_Settlement(string tranId, int amt)
        {
            var ts = (from t in entity.Transactions where t.Id == tranId select t).FirstOrDefault();

            string settlementNo = ts.TranVouNos;
            if (settlementNo != "")
            {
                var settlementResult = (from t in entity.Transactions where t.Id == settlementNo select t).FirstOrDefault();
                settlementResult.TotalAmount = settlementResult.TotalAmount - amt;

                string _tranVouNos = settlementResult.TranVouNos;

                //if (_tranVouNos != null)
                //{
                string[] _tranWord = _tranVouNos.Split(',');

                if (settlementResult.TotalAmount == 0)
                {

                    settlementResult.IsDeleted = true;

                }
                entity.Entry(settlementResult).State = EntityState.Modified;
                entity.SaveChanges();
            }
            //  }



        }

        #region for saving Refund Qty in Stock Transaction table
        private void Save_RefundQty_ToStockTransaction(List<Stock_Transaction> productList, DateTime _tranDate)
        {
            int _year, _month;

            _year = _tranDate.Year;
            _month = _tranDate.Month;
            Utility.Refund_Run_Process(_year, _month, productList);
        }
        #endregion

        public void Reload()
        {
            btnSubmit_Click(this, null);
        }

        #endregion

        private void RefundTransaction_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (System.Windows.Forms.Application.OpenForms["CreditTransactionList"] != null)
            {
                CreditTransactionList openedForm = (CreditTransactionList)System.Windows.Forms.Application.OpenForms["CreditTransactionList"];
                openedForm.LoadData();
                this.Dispose();
            }
            else if (System.Windows.Forms.Application.OpenForms["TransactionList"] != null)
            {
                TransactionList openedForm = (TransactionList)System.Windows.Forms.Application.OpenForms["TransactionList"];
                openedForm.LoadData();
                this.Dispose();
            }

        }


    }
}
