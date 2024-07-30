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
using POS.APP_Data;
using Microsoft.Reporting.WinForms;

namespace POS
{
    public partial class CreditTransactionList : Form
    {
        #region Variables

        private POSEntities entity = new POSEntities();

        int Qty = 0;

        List<Stock_Transaction> productList = new List<Stock_Transaction>();
        public int index = 0;
        Boolean Isstart = false;

        private bool IsPrint = true;
        #endregion

        #region Event

        public CreditTransactionList()
        {
            InitializeComponent();
        }

        private void CreditTransactionList_Load(object sender, EventArgs e)
        {
            dgvTransactionList.AutoGenerateColumns = false;
            Utility.BindShop(cboshoplist);
            cboshoplist.Text = SettingController.DefaultShop.ShopName;
            Utility.ShopComBo_EnableOrNot(cboshoplist);
            Isstart = true;
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

        private void dgvTransactionList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            index = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                List<string> type = new List<string>();
                type.Add(TransactionType.Settlement);
                type.Add(TransactionType.Prepaid);
                string currentTransactionId = dgvTransactionList.Rows[e.RowIndex].Cells[0].Value.ToString();
                var crd = (from p in entity.Transactions where p.Id == currentTransactionId select p).FirstOrDefault();

                List<APP_Data.TransactionDetail> _isConsignmentPaidTranList = entity.TransactionDetails.Where(x => x.TransactionId == currentTransactionId && x.IsDeleted == false && x.IsConsignmentPaid == true).ToList();
                //Refund
                if (e.ColumnIndex == 9)
                {

                    if (type.Contains(dgvTransactionList.Rows[e.RowIndex].Cells[1].Value.ToString()))
                    {
                        colRefund.ReadOnly = true;
                        MessageBox.Show("Non Refundable!", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else if (crd.PaymentTypeId == 2 && crd.RecieveAmount > 0)
                    {
                        colRefund.ReadOnly = true;
                        MessageBox.Show("Non Refundable!", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        if (_isConsignmentPaidTranList.Count > 0)
                        {
                            MessageBox.Show("This transaction already paid  Consignment. It cannot be deleted!");
                        }
                        else
                        {
                            var ts = (from t in entity.Transactions where t.Id == currentTransactionId select t).FirstOrDefault();

                            string date = dgvTransactionList.Rows[e.RowIndex].Cells[2].Value.ToString();
                            DateTime _Trandate = Utility.Convert_Date(date);

                            Update_Settlement(ts, _Trandate, true, true);
                        }
                    }
                }

                //View Detail
                else if (e.ColumnIndex == 10)
                {
                    if (type.Contains(dgvTransactionList.Rows[e.RowIndex].Cells[1].Value.ToString()))
                    {
                        //colViewDetail.ReadOnly = true;
                        switch (dgvTransactionList.Rows[e.RowIndex].Cells[1].Value.ToString())
                        {
                            case "Settlement":
                                var result = (from t in entity.Transactions where t.Id == currentTransactionId select t.TranVouNos).FirstOrDefault();
                                MessageBox.Show("Settlement for Vouncher No. " + result, "mPOS");
                                break;

                            case "Prepaid":
                                colViewDetail.ReadOnly = true;
                                break;

                        }

                    }
                    else
                    {
                        TransactionDetailForm newForm = new TransactionDetailForm();
                        newForm.transactionId = currentTransactionId;
                        newForm.IsCash = false;
                        newForm.Show();
                    }
                }
                //Delete
                else if (e.ColumnIndex == 11)
                {

                    if (type.Contains(dgvTransactionList.Rows[e.RowIndex].Cells[1].Value.ToString()))
                    {
                        colDelete.ReadOnly = true;
                    }
                    else
                    {
                        Transaction ts = entity.Transactions.Where(x => x.Id == currentTransactionId).FirstOrDefault();
                        var isPurchasePackakgeInvoiceData = entity.PackageUsedHistories.Any(x => x.PackagePurchasedInvoice.TransactionDetail.TransactionId == currentTransactionId && x.IsDelete == false);
                        if (ts.Transaction1.Count > 0)
                        {
                            MessageBox.Show("This transaction already make refund or paid. So it can't be delete!"); // Prepaid ရှိနေတာ
                        }
                        else if (_isConsignmentPaidTranList.Count > 0)
                        {
                            MessageBox.Show("This transaction already paid  Consignment. It cannot be deleted!");
                        }
                        else if (isPurchasePackakgeInvoiceData)
                        {
                            MessageBox.Show("This transaction already used by Offset. It cannot be deleted!");
                            return;
                        }
                        else
                        {

                            DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            if (result.Equals(DialogResult.OK))
                            {
                                string date = dgvTransactionList.Rows[e.RowIndex].Cells[2].Value.ToString();
                                DateTime _Trandate = Utility.Convert_Date(date);
                                if (Update_Settlement(ts, _Trandate, true))
                                {
                                    Transaction_Delete(ts, _Trandate);
                                }

                                #region Update Receipt Data Log ( Add by YMO)

                                List<Transaction> transaction = entity.Transactions.Where(x => x.DateTime > ts.DateTime).ToList();
                                if (transaction.Count != 0)
                                {
                                    var leftamount = ts.TotalAmount - ts.RecieveAmount;
                                    foreach (var t in transaction)
                                    {
                                        ReceiptDataLog dataLog = entity.ReceiptDataLogs.Where(x => x.TransactionId == t.Id).FirstOrDefault();
                                        if (dataLog != null)
                                        {
                                            dataLog.TransactionId = t.Id;
                                            dataLog.PreviousOutstanding = dataLog.PreviousOutstanding - leftamount;
                                            dataLog.Netpayable = dataLog.PreviousOutstanding + Convert.ToDecimal(dataLog.Total);
                                            dataLog.Balance = Convert.ToString(dataLog.Netpayable);

                                            entity.Entry(dataLog).State = EntityState.Modified;
                                            entity.SaveChanges();
                                        }
                                    }
                                }

                                #endregion
                            }

                        }
                    }

                }
                else if (e.ColumnIndex == 12)
                {
                    if (type.Contains(dgvTransactionList.Rows[e.RowIndex].Cells[1].Value.ToString()))
                    {
                        colDeleteAndCopy.ReadOnly = true;
                    }
                    else
                    {
                        Transaction ts = entity.Transactions.Where(x => x.Id == currentTransactionId).FirstOrDefault();
                        var isDeleteCopyPurchasePackakgeInvoiceData = entity.PackageUsedHistories.Any(x => x.PackagePurchasedInvoice.TransactionDetail.TransactionId == currentTransactionId && x.IsDelete == false);

                        List<Transaction> rlist = new List<Transaction>();

                        if (ts.Transaction1.Count > 0)
                        {
                            rlist = ts.Transaction1.Where(x => x.IsDeleted == false).ToList();
                        }
                        if (rlist.Count > 0)
                        {
                            MessageBox.Show("This transaction already make refund. So it can't be delete!");
                        }
                        else if (_isConsignmentPaidTranList.Count > 0)
                        {
                            MessageBox.Show("This transaction already paid  Consignment. It cannot be deleted!");
                        }
                        else if (isDeleteCopyPurchasePackakgeInvoiceData)
                        {
                            MessageBox.Show("This transaction already used by Offset. It cannot be deleted!");
                            return;
                        }
                        else
                        {
                            if (MessageBox.Show("Are you sure want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                            {
                                return;
                            }

                            #region Update Receipt Data Log ( Add by YMO)

                            List<Transaction> transaction = entity.Transactions.Where(x => x.DateTime > ts.DateTime).ToList();
                            if (transaction.Count != 0)
                            {
                                var leftamount = ts.TotalAmount - ts.RecieveAmount;
                                foreach (var t in transaction)
                                {
                                    ReceiptDataLog dataLog = entity.ReceiptDataLogs.Where(x => x.TransactionId == t.Id).FirstOrDefault();
                                    if (dataLog != null)
                                    {
                                        dataLog.TransactionId = t.Id;
                                        dataLog.PreviousOutstanding = dataLog.PreviousOutstanding - leftamount;
                                        dataLog.Netpayable = dataLog.PreviousOutstanding + Convert.ToDecimal(dataLog.Total);
                                        dataLog.Balance = Convert.ToString(dataLog.Netpayable);

                                        entity.Entry(dataLog).State = EntityState.Modified;
                                        entity.SaveChanges();
                                    }
                                }
                            }

                            #endregion

                            ts.IsDeleted = true;
                            ts.UpdatedDate = DateTime.Now;
                            // add gift card amount
                            if (ts.GiftCardId != null && ts.GiftCard != null)
                            {
                                ts.GiftCard.Amount = ts.GiftCard.Amount + Convert.ToInt32(ts.GiftCardAmount);
                            }
                            foreach (TransactionDetail detail in ts.TransactionDetails)
                            {
                                //detail.IsDeleted = false;
                                detail.IsDeleted = true;
                                detail.Product.Qty = detail.Product.Qty + detail.Qty;


                                //save in stocktransaction

                                Stock_Transaction st = new Stock_Transaction();
                                st.ProductId = detail.Product.Id;
                                Qty -= Convert.ToInt32(detail.Qty);
                                st.Sale = Qty;
                                productList.Add(st);
                                Qty = 0;


                                if (detail.Product.IsWrapper == true)
                                {
                                    List<SPDetail> wList = detail.Product.SPDetails.Where(x => x.TransactionDetailID == detail.Id).ToList();
                                    if (wList.Count > 0)
                                    {
                                        foreach (SPDetail w in wList)
                                        {
                                            Product wpObj = (from p in entity.Products where p.Id == w.ChildProductID select p).FirstOrDefault();
                                            wpObj.Qty = wpObj.Qty + (w.ChildQty);

                                            Stock_Transaction stwp = new Stock_Transaction();
                                            stwp.ProductId = w.ChildProductID;
                                            Qty -= Convert.ToInt32(w.ChildQty);
                                            stwp.Sale = Qty;
                                            productList.Add(stwp);

                                            Qty = 0;
                                        }
                                    }
                                }

                                //For Purchase 
                                #region Purchase Delete

                                if (detail.Product.IsWrapper == true)
                                {//ZP Get purchase detail with child product Id and order by purchase date ascending
                                    List<SPDetail> splist = detail.Product.SPDetails.Where(x => x.TransactionDetailID == detail.Id).ToList();
                                    foreach (SPDetail w in splist)
                                    {
                                        //  int qty = Convert.ToInt32(w.Qty);
                                        Product wpObj = (from p in entity.Products where p.Id == w.ChildProductID select p).FirstOrDefault();

                                        List<APP_Data.PurchaseDetailInTransaction> puInTranDetail = entity.PurchaseDetailInTransactions.Where(x => x.TransactionDetailId == detail.Id && x.ProductId == wpObj.Id).ToList();
                                        if (puInTranDetail.Count > 0)
                                        {
                                            foreach (PurchaseDetailInTransaction p in puInTranDetail)
                                            {
                                                PurchaseDetail pud = entity.PurchaseDetails.Where(x => x.Id == p.PurchaseDetailId).FirstOrDefault();
                                                if (pud != null)
                                                {
                                                    pud.CurrentQy = pud.CurrentQy + p.Qty;
                                                }
                                                entity.Entry(pud).State = EntityState.Modified;
                                                entity.SaveChanges();

                                                //entity.PurchaseDetailInTransactions.Remove(p);
                                                //entity.SaveChanges();

                                                p.Qty = 0;
                                                entity.Entry(p).State = EntityState.Modified;

                                                entity.PurchaseDetailInTransactions.Remove(p);
                                                entity.SaveChanges();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    List<APP_Data.PurchaseDetailInTransaction> puInTranDetail = entity.PurchaseDetailInTransactions.Where(x => x.TransactionDetailId == detail.Id && x.ProductId == detail.ProductId).ToList();
                                    if (puInTranDetail.Count > 0)
                                    {
                                        foreach (PurchaseDetailInTransaction p in puInTranDetail)
                                        {
                                            PurchaseDetail pud = entity.PurchaseDetails.Where(x => x.Id == p.PurchaseDetailId).FirstOrDefault();
                                            if (pud != null)
                                            {
                                                pud.CurrentQy = pud.CurrentQy + p.Qty;
                                            }
                                            entity.Entry(pud).State = EntityState.Modified;
                                            entity.SaveChanges();

                                            //entity.PurchaseDetailInTransactions.Remove(p);
                                            //entity.SaveChanges();

                                            p.Qty = 0;
                                            entity.Entry(p).State = EntityState.Modified;

                                            entity.PurchaseDetailInTransactions.Remove(p);
                                            entity.SaveChanges();
                                        }
                                    }
                                }
                                #endregion
                            }

                            string date = dgvTransactionList.Rows[e.RowIndex].Cells[2].Value.ToString();
                            DateTime _Trandate = Utility.Convert_Date(date);
                            //save in stock transaction
                            Save_SaleQty_ToStockTransaction(productList, _Trandate);
                            productList.Clear();
                            DeleteLog dl = new DeleteLog();
                            dl.DeletedDate = DateTime.Now;
                            dl.CounterId = MemberShip.CounterId;
                            dl.UserId = MemberShip.UserId;
                            dl.IsParent = true;
                            dl.TransactionId = ts.Id;

                            entity.DeleteLogs.Add(dl);

                            entity.SaveChanges();
                            if (System.Windows.Forms.Application.OpenForms["Sales"] != null)
                            {
                                Sales openedForm = (Sales)System.Windows.Forms.Application.OpenForms["Sales"];
                                openedForm.DeleteCopy(currentTransactionId);
                                this.Dispose();
                            }
                        }
                    }

                }
                else if (e.ColumnIndex == 13)
                {
                    string Note = (from p in entity.Transactions where p.Id == currentTransactionId select p.Note).FirstOrDefault();
                    AddNote note = new AddNote();
                    note.editnote = Note;
                    note.status = "EDIT";
                    note.tranid = currentTransactionId;
                    note.ShowDialog();
                }
                else if (e.ColumnIndex == 14)
                {
                    if (dgvTransactionList.Rows[e.RowIndex].Cells[1].Value.ToString() == "Settlement" || dgvTransactionList.Rows[e.RowIndex].Cells[1].Value.ToString() == "Prepaid" || dgvTransactionList.Rows[e.RowIndex].Cells[1].Value.ToString() == "Credit") // || dgvTransactionList.Rows[e.RowIndex].Cells[1].Value.ToString() == "Credit"
                    {
                        //Print Invoice
                        #region [ Print ]
                        Utility.ReprintReceipt(currentTransactionId);
                        #endregion
                    }
                    else
                    {
                        MessageBox.Show("You can't reprint this transaction.");
                    }
                }
                else if (e.ColumnIndex == colCancel.Index)//Cancel Transaction
                {

                    if (type.Contains(dgvTransactionList.Rows[e.RowIndex].Cells[1].Value.ToString()))
                    {
                        colCancel.ReadOnly = true;
                        MessageBox.Show("This transaction type can't be cancelled.", "Cancellation Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                   

                    Transaction ts = entity.Transactions.FirstOrDefault(x => x.Id == currentTransactionId && x.IsCancelled == false);
                    if (ts == null)
                    {
                        MessageBox.Show("This transaction is already cancelled.");
                        return;
                    }
                    else
                    {
                        List<Transaction> rlist = new List<Transaction>();
                        if (ts.Transaction1.Count > 0)
                        {
                            rlist = ts.Transaction1.Where(x => x.IsDeleted == false).ToList();
                        }
                        if (rlist.Count > 0)
                        {
                            MessageBox.Show("This transaction is already refunded.", "Cancellation Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        //  }
                        bool isPackage = entity.PackagePurchasedInvoices.Any(x => x.TransactionDetail.TransactionId == ts.Id && x.IsDelete == false);
                        if (!isPackage)
                        {
                            MessageBox.Show("This transaction does not contain package product", "Cancellation Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }

                        CancelTransaction cancelForm = new CancelTransaction(ts.Id);
                        cancelForm.Show();

                    }
                }

            }
        }

        private void dgvTransactionList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvTransactionList.Rows)
            {
                Transaction currentt = (Transaction)row.DataBoundItem;
                row.Cells[0].Value = currentt.Id;
                row.Cells[1].Value = currentt.Type;
                row.Cells[2].Value = currentt.DateTime.Value.ToString("dd-MM-yyyy");
                row.Cells[3].Value = currentt.DateTime.Value.ToString("hh:mm");
                row.Cells[4].Value = currentt.User.Name;
                row.Cells[5].Value = (currentt.Customer == null) ? "-" : currentt.Customer.Name;
                row.Cells[6].Value = currentt.PaymentType.Name;
                if (currentt.Type == "Settlement" || currentt.Type == "Prepaid")
                {
                    row.Cells[7].Value = currentt.RecieveAmount;
                    // row.DefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#c6dff9");
                    row.DefaultCellStyle.BackColor = Color.LightSkyBlue;
                    row.Cells[8].Value = 0;
                    this.dgvTransactionList.Columns["RePrint"].Visible = true;

                }
                else
                {
                    List<string> type = new List<string>();
                    type.Add(TransactionType.CreditRefund);
                    type.Add(TransactionType.Refund);
                    var refundList = (from t in entity.Transactions where type.Contains(t.Type) && t.ParentId == currentt.Id && t.IsDeleted == false select t).ToList();
                    int refundAmt = Convert.ToInt32(refundList.Sum(x => x.TotalAmount));
                    //row.Cells[6].Value = currentt.TotalAmount - currentt.UsePrePaidDebts.Sum(x => x.UseAmount).Value - currentt.RecieveAmount - refundAmt;

                    var usePrepaidAmt = 0;
                    // usePrepaidAmt = Convert.ToInt32(entity.UsePrePaidDebts.AsEnumerable().Where(x => x.CreditTransactionId == currentt.Id).Select(x => x.UseAmount).Sum());


                    var DiscountAmt = Convert.ToInt32(refundList.Sum(x => x.DiscountAmount));
                    int currentRefundAmt = refundAmt - DiscountAmt;

                    int receiveAmt = Convert.ToInt32(currentt.RecieveAmount);
                    //                 

                    //if (currentRefundAmt != 0)
                    //{
                    int _inAmt = (receiveAmt + usePrepaidAmt);

                    row.Cells[7].Value = currentt.TotalAmount - _inAmt - currentRefundAmt;
                    //}
                    //else
                    //{
                    //    row.Cells[6].Value = currentt.TotalAmount - (receiveAmt + usePrepaidAmt);
                    //}
                    row.Cells[8].Value = currentRefundAmt;
                }

                if (BOOrPOS.IsBackOffice == true)
                {
                    if (dgvTransactionList.Columns[12].Visible != false)
                    {
                        dgvTransactionList.Columns[12].Visible = false;
                    }
                }
                else
                {
                    if (dgvTransactionList.Columns[12].Visible != false)
                    {
                        dgvTransactionList.Columns[12].Visible = true;
                    }
                }

            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void rdbDate_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbDate.Checked)
            {
                gbDate.Enabled = true;
                gbId.Enabled = false;
                txtId.Clear();
            }
            else
            {
                gbDate.Enabled = false;
                gbId.Enabled = true;
            }
            LoadData();
        }

        private void cboshoplist_selectedindexchanged(object sender, EventArgs e)
        {
            LoadData();
        }
        #endregion

        #region Function
        public Boolean Update_Settlement(Transaction ts, DateTime _date, Boolean IsSettlement = false, Boolean IsRefund = false)
        {
            Boolean IsContinue = false;
            if (ts.IsSettlement == true)
            {
                string Id = ts.Id;
                string settlementVouNo = ts.TranVouNos;
                string text = "";
                if (IsRefund == true)
                {
                    text = "Refund";
                }
                else
                {
                    text = "Delete";
                }

                //if (Convert.ToInt32(dgvTransactionList.Rows[index].Cells[6].Value) > Convert.ToInt32(dgvTransactionList.Rows[index].Cells[7].Value))
                //{
                if (IsRefund)
                {
                    if (Convert.ToInt32(dgvTransactionList.Rows[index].Cells[6].Value) < Convert.ToInt32(dgvTransactionList.Rows[index].Cells[7].Value))
                    {
                        RefundTransaction newForm = new RefundTransaction();
                        newForm.transactionId = ts.Id;
                        newForm.Show();
                        return IsContinue;
                    }

                }
                DialogResult result1 = MessageBox.Show("'" + Id + "' is already made settlement with Vouncher No. '"
              + settlementVouNo + "' ! Are you sure want to " + text + " TransactionId '" + Id + "' ?", "mPOS", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result1.Equals(DialogResult.OK))
                {
                    if (IsRefund)
                    {

                        RefundTransaction newForm = new RefundTransaction();
                        newForm.transactionId = ts.Id;
                        newForm.Show();

                    }
                    else
                    {
                        var settlementResult = (from t in entity.Transactions where t.Id == settlementVouNo select t).FirstOrDefault();
                        settlementResult.TotalAmount = settlementResult.TotalAmount - ts.TotalAmount;

                        string _tranVouNos = settlementResult.TranVouNos;
                        string[] _tranWord = _tranVouNos.Split(',');

                        if (_tranWord.Length > 1)
                        {
                            var _tranList = (from t in _tranWord where t != Id select t).ToList();
                            string joinedTranIdList = string.Join(",", _tranList);

                            settlementResult.TranVouNos = joinedTranIdList;
                            entity.Entry(settlementResult).State = EntityState.Modified;
                            entity.SaveChanges();
                        }
                        else
                        {
                            settlementResult.IsDeleted = true;
                            entity.Entry(settlementResult).State = EntityState.Modified;
                            entity.SaveChanges();
                        }


                        if (IsSettlement)
                        {
                            Transaction_Delete(ts, _date);
                        }
                    }
                    IsContinue = true;
                }
                //}
                //else
                //{
                //    MessageBox.Show( "Invoice No." + ts.Id + "  is already refunds all items.","mPOS");
                //}


            }
            else
            {
                if (!IsRefund)
                {
                    //DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    //if (result.Equals(DialogResult.OK))
                    //{
                    //if (IsSettlement)
                    //{
                    //Transaction_Delete(ts, _date);
                    return IsContinue = true;
                    //    IsContinue = true;
                    //}

                    //}

                }
                else
                {
                    RefundTransaction newForm = new RefundTransaction();
                    newForm.transactionId = ts.Id;
                    newForm.Show();
                }

            }
            return IsContinue;
        }

        private void Transaction_Delete(Transaction ts, DateTime _date)
        {
            ts.IsDeleted = true;
            //save in stocktransaction

            foreach (TransactionDetail detail in ts.TransactionDetails.Where(x => x.IsDeleted == false))
            {
                detail.IsDeleted = true;
                detail.Product.Qty = detail.Product.Qty + detail.Qty;

                Stock_Transaction st = new Stock_Transaction();
                st.ProductId = detail.Product.Id;
                Qty -= Convert.ToInt32(detail.Qty);
                st.Sale = Qty;
                productList.Add(st);
                Qty = 0;
                //zp modify credit promotion item
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
                            Qty -= Convert.ToInt32(w.ChildQty * detail.Qty);
                            stwp.Sale = Qty;
                            productList.Add(stwp);
                            Qty = 0;

                        }
                    }
                }
                #region Purchase Delete
                if (detail.Product.IsWrapper == true)
                {//ZP Get purchase detail with child product Id and order by purchase date ascending
                    List<SPDetail> splist = detail.Product.SPDetails.ToList();
                    foreach (SPDetail w in splist)
                    {
                        //  int qty = Convert.ToInt32(w.Qty);
                        Product wpObj = (from p in entity.Products where p.Id == w.ChildProductID select p).FirstOrDefault();

                        List<APP_Data.PurchaseDetailInTransaction> puInTranDetail = entity.PurchaseDetailInTransactions.Where(x => x.TransactionDetailId == detail.Id && x.ProductId == wpObj.Id).ToList();
                        if (puInTranDetail.Count > 0)
                        {
                            foreach (PurchaseDetailInTransaction p in puInTranDetail)
                            {
                                PurchaseDetail pud = entity.PurchaseDetails.Where(x => x.Id == p.PurchaseDetailId).FirstOrDefault();
                                if (pud != null)
                                {
                                    pud.CurrentQy = pud.CurrentQy + p.Qty;
                                }
                                entity.Entry(pud).State = EntityState.Modified;
                                entity.SaveChanges();

                                //entity.PurchaseDetailInTransactions.Remove(p);
                                //entity.SaveChanges();

                                p.Qty = 0;
                                entity.Entry(p).State = EntityState.Modified;

                                entity.PurchaseDetailInTransactions.Remove(p);
                                entity.SaveChanges();
                            }
                        }
                    }
                }
                else
                {
                    List<APP_Data.PurchaseDetailInTransaction> puInTranDetail = entity.PurchaseDetailInTransactions.Where(x => x.TransactionDetailId == detail.Id && x.ProductId == detail.ProductId).ToList();
                    if (puInTranDetail.Count > 0)
                    {
                        foreach (PurchaseDetailInTransaction p in puInTranDetail)
                        {
                            PurchaseDetail pud = entity.PurchaseDetails.Where(x => x.Id == p.PurchaseDetailId).FirstOrDefault();
                            if (pud != null)
                            {
                                pud.CurrentQy = pud.CurrentQy + p.Qty;
                            }
                            entity.Entry(pud).State = EntityState.Modified;
                            entity.SaveChanges();

                            //entity.PurchaseDetailInTransactions.Remove(p);
                            //entity.SaveChanges();

                            p.Qty = 0;
                            entity.Entry(p).State = EntityState.Modified;

                            entity.PurchaseDetailInTransactions.Remove(p);
                            entity.SaveChanges();
                        }
                    }
                }
                #endregion



                // update Prepaid Transaction id = false   and delete list in useprepaiddebt table
                Utility.Plus_PreaidAmt(ts);


            }
            //save in stock transaction

            Save_SaleQty_ToStockTransaction(productList, _date);
            productList.Clear();
            DeleteLog dl = new DeleteLog();
            dl.DeletedDate = DateTime.Now;
            dl.CounterId = MemberShip.CounterId;
            dl.UserId = MemberShip.UserId;
            dl.IsParent = true;
            dl.TransactionId = ts.Id;

            entity.DeleteLogs.Add(dl);

            entity.SaveChanges();
            var result = entity.Transactions.Where(x => x.Id == "WS000003").Select(x => x.IsDeleted).FirstOrDefault();


            if (System.Windows.Forms.Application.OpenForms["chart"] != null)
            {
                chart newForm = (chart)System.Windows.Forms.Application.OpenForms["chart"];
                newForm.FormFresh();
            }
            LoadData();
        }

        #region for saving Sale Qty in Stock Transaction table
        private void Save_SaleQty_ToStockTransaction(List<Stock_Transaction> productList, DateTime _tranDate)
        {
            int _year, _month;

            _year = _tranDate.Year;
            _month = _tranDate.Month;
            Utility.Sale_Run_Process(_year, _month, productList);
        }
        #endregion


        public void LoadData()
        {
            if (Isstart == true)
            {
                entity = new POSEntities();
                dgvTransactionList_CustomCellFormatting();
                dgvTransactionList.DataSource = "";

                int shopid = Convert.ToInt32(cboshoplist.SelectedValue);
                string currentshortcode = (from p in entity.Shops where p.Id == shopid select p.ShortCode).FirstOrDefault();
                List<string> type = new List<string>();
                type.Add(TransactionType.Credit);
                type.Add(TransactionType.Settlement);
                type.Add(TransactionType.Prepaid);

                if (rdbDate.Checked)
                {
                    DateTime fromDate = dtpFrom.Value.Date;
                    DateTime toDate = dtpTo.Value.Date;



                    List<Transaction> transList = (from t in entity.Transactions
                                                   where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate
                                                       && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true
                                                       && type.Contains(t.Type) && t.Id.Substring(2, 2) == currentshortcode
                                                   select t).ToList<Transaction>(); // && t.Type == TransactionType.Credit


                    dgvTransactionList.DataSource = transList.Where(x => x.IsDeleted != true).ToList();



                    //   dgvTransactionList.AutoGenerateColumns = false;
                    //    dgvTransactionList.DataSource = transList.Where(x => x.IsDeleted != true).ToList();
                }
                else
                {
                    string Id = txtId.Text;

                    if (Id.Trim() != string.Empty)
                    {
                        List<Transaction> transList = (from t in entity.Transactions where t.Id == Id && type.Contains(t.Type) && t.Id.Substring(2, 2) == currentshortcode select t).ToList().Where(x => x.IsDeleted != true).ToList();
                        if (transList.Count > 0)
                        {
                            dgvTransactionList.DataSource = transList;
                        }
                        else
                        {
                            dgvTransactionList.DataSource = "";
                            MessageBox.Show("Item not found!", "Cannot find");
                        }
                    }
                    else
                    {
                        dgvTransactionList.DataSource = "";
                    }
                }
            }
        }

        private void dgvTransactionList_CustomCellFormatting()
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            // Transaction Delete
            if (!MemberShip.isAdmin && !controller.CreditTransaction.EditOrDelete)
            {
                dgvTransactionList.Columns["colDelete"].Visible = false;
            }
            // Transaction Delete And Copy
            if (!MemberShip.isAdmin && !controller.CreditTransaction.DeleteAndCopy)
            {
                dgvTransactionList.Columns["colDeleteAndCopy"].Visible = false;
            }
            // Transaction Cancel
            if (!MemberShip.isAdmin && !controller.CreditTransaction.Cancel)
            {
                dgvTransactionList.Columns["colCancel"].Visible = false;
            }
        }
        #endregion

    }
}
