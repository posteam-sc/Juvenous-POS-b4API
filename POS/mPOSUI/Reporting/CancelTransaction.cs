using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using POS.APP_Data;
using System.Collections.Generic;
using System.Drawing;

namespace POS
{
    public partial class CancelTransaction : Form
    {
        public string transactionId { get; set; }
        List<PackageProduct> pkgList;
        POSEntities entity = new POSEntities();
        Transaction currentTransaction;

        #region Events
        public CancelTransaction(string transId)
        {
            InitializeComponent();
            transactionId = transId;
        }


        private void CancelTransaction_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgvPackageList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

            foreach (DataGridViewRow row in dgvPackageList.Rows)
            {
                PackageProduct pkg = (PackageProduct)row.DataBoundItem;
                row.Cells[colProductCode.Index].Value = pkg.ProductCode;
                row.Cells[colItemName.Index].Value = pkg.ItemName;
                row.Cells[colQty.Index].Value = pkg.Qty;

                row.Cells[colTax.Index].Value = pkg.Tax.ToString("n2") + "%";
                row.Cells[colUnitPrice.Index].Value = pkg.Unitprice;
                row.Cells[colDiscountPercent.Index].Value = pkg.DiscountPercent.ToString("n2") + "%";
                row.Cells[colCost.Index].Value = pkg.Cost.ToString("n2");
                row.Cells[colFOC.Index].Value = pkg.IsFOC;
                row.Cells[colFrequency.Index].Value = pkg.Frequency;
                row.Cells[colTotalQty.Index].Value = pkg.TotalOffsetQty;
                row.Cells[colUsedQty.Index].Value = pkg.UsedQty;
                row.Cells[colTransactionId.Index].Value = pkg.TransactionId;
                row.Cells[colTransactionDetailID.Index].Value = pkg.TransactionDetailId;
                row.Cells[colConsignmentPrice.Index].Value = pkg.ConsignmentPrice;
                row.Cells[colCancelled.Index].Value = pkg.IsCancelled;
                if (pkg.IsCancelled)
                {
                    row.DefaultCellStyle.BackColor = Color.LightSkyBlue;
                }

            }
        }
        private void dgvCancelledList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvCancelledList.Rows)
            {
                CancellationLog log = (CancellationLog)row.DataBoundItem;
                row.Cells[colDate.Index].Value = log.DateTime.Date;
                row.Cells[colTime.Index].Value = log.DateTime.TimeOfDay.Hours + ":" + log.DateTime.TimeOfDay.Minutes;
                row.Cells[colPCode.Index].Value = log.TransactionDetail.Product.ProductCode;
                row.Cells[colItmName.Index].Value = log.TransactionDetail.Product.Name;
                row.Cells[colCancelledQty.Index].Value = log.CancelledQty;
                row.Cells[colCancelledAmount.Index].Value = log.CancelledAmount.ToString("n2");
            }
        }

        private void dgvPackageList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (e.RowIndex > -1)
            {

                if (e.ColumnIndex == colCancel.Index)
                {
                    bool isCancelled = Convert.ToBoolean(dgvPackageList.Rows[e.RowIndex].Cells[colCancelled.Index].Value);
                    if (isCancelled) return;

                    int totalQty = Convert.ToInt32(dgvPackageList.Rows[e.RowIndex].Cells[colTotalQty.Index].Value);
                    int usedQty = Convert.ToInt32(dgvPackageList.Rows[e.RowIndex].Cells[colUsedQty.Index].Value);
                    int availabeQty = totalQty - usedQty;
                    if (availabeQty == 0)
                    {
                        MessageBox.Show("This package is used up.", "Cancellation Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    else if (usedQty == 0)
                    {
                        MessageBox.Show("This package is not used yet. You might want to delete it.", "Cancellation Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    else
                    {
                        long tdId = Convert.ToInt64(dgvPackageList.Rows[e.RowIndex].Cells[colTransactionDetailID.Index].Value);

                        PackagePurchasedInvoice pki = entity.PackagePurchasedInvoices.Where(x => x.TransactionDetailId == tdId).FirstOrDefault();
                        if (pki != null)
                        {
                            //package is cancelled in the list
                            PackageProduct pkg = pkgList.FirstOrDefault(x => x.TransactionDetailId == tdId);
                            if (pkg != null)
                            {
                                //calculate cancelled amount
                                decimal cancelledAmount;
                                bool IsPaid = (bool)currentTransaction.IsPaid;
                                bool IsFOC = pkg.IsFOC != "";
                                if (IsFOC || IsPaid)
                                {
                                    cancelledAmount = 0;
                                }
                                else
                                {

                                    decimal receivedAmt = (decimal)currentTransaction.RecieveAmount;
                                    decimal prepaidAmt = 0;
                                    if (currentTransaction.UsePrePaidDebts != null)
                                    {
                                        prepaidAmt = (decimal)currentTransaction.UsePrePaidDebts.Sum(x => x.UseAmount);
                                    }

                                    decimal totalReceivedAmt = receivedAmt + prepaidAmt;

                                    ////Used Amount of Previous cancelled packages
                                    //List<PackageProduct> previousCancelledList = pkgList.Where(x => x.IsCancelled == true).ToList();
                                    //decimal previousUsedAmt = 0; 
                                    //foreach (PackageProduct p in previousCancelledList)
                                    //{
                                    //    decimal price = p.Cost / p.TotalOffsetQty;
                                    //    previousUsedAmt += price * p.UsedQty;
                                    //}

                                    //Used Amount of all the package in the list
                                    decimal allPkgUsedAmt = 0;
                                    foreach (PackageProduct p in pkgList)
                                    {
                                        decimal price = p.Cost / p.TotalOffsetQty;
                                        allPkgUsedAmt += price * p.UsedQty;
                                    }

                                    decimal totalCost = Convert.ToDecimal(dgvPackageList.Rows[e.RowIndex].Cells[colCost.Index].Value);
                                    decimal offsetUnitPrice = totalCost / pkg.TotalOffsetQty;

                                    //decimal totalUsedAmt = (offsetUnitPrice * pkg.UsedQty) + allPkgUsedAmt;
                                    decimal totalUsedAmt = allPkgUsedAmt;
                                    if (totalUsedAmt > totalReceivedAmt)
                                    {
                                        MessageBox.Show("Total received/settlement amount is less than package's used amount", "Cancellation Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        return;
                                    }

                                    cancelledAmount = offsetUnitPrice * (pkg.TotalOffsetQty - pkg.UsedQty);
                                    decimal previousCancelledAmt = 0;
                                    if (currentTransaction.CancellationLogs != null)
                                    {
                                        previousCancelledAmt = currentTransaction.CancellationLogs.Sum(x => x.CancelledAmount);
                                    }
                                    decimal outstandingAmt = (decimal)currentTransaction.TotalAmount - totalReceivedAmt - previousCancelledAmt;
                                    if (cancelledAmount > outstandingAmt)
                                    {
                                        cancelledAmount -= (totalReceivedAmt - totalUsedAmt);
                                    }
                                }
                                DialogResult result = MessageBox.Show("Are you sure you want to cancel this package?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (result == DialogResult.No)
                                {
                                    return;
                                }
                                int cancelledQty = pkg.TotalOffsetQty - pkg.UsedQty;
                                pkg.IsCancelled = true;
                              //  pkg.IsAllOffsetUsed = true; // mark all the offset used when cancelled

                                //package is cancelled
                                pki.IsCancelled = true;
                                entity.Entry(pki).State = EntityState.Modified;
                                entity.SaveChanges();

                                //transaction detail is cancelled
                                TransactionDetail td = entity.TransactionDetails.FirstOrDefault(x => x.Id == tdId);
                                td.IsCancelled = true;
                                entity.Entry(td).State = EntityState.Modified;
                                entity.SaveChanges();

                                //transaction is cancelled

                                bool ToCancelTransaction = pkgList.All(x => x.IsCancelled);
                                if (ToCancelTransaction)
                                {
                                    currentTransaction.IsCancelled = true;
                                    entity.Entry(currentTransaction).State = EntityState.Modified;
                                    entity.SaveChanges();
                                }

                                CancellationLog log = new CancellationLog();
                                log.TransactionId = transactionId;
                                log.TransactionDetailId = tdId;
                                log.DateTime = DateTime.Now;
                                log.TotalOffsetQty = totalQty;
                                log.CancelledQty = cancelledQty;
                                log.CancelledAmount = cancelledAmount;
                                log.CancelledBy = MemberShip.UserId;
                                entity.CancellationLogs.Add(log);
                                entity.SaveChanges();
                                LoadData();
                                MessageBox.Show("Package is successfully cancelled");
                            }

                        }
                    }
                }

            }
        }
        #endregion




        #region Methods
        private void LoadData()
        {
            currentTransaction = (from t in entity.Transactions where t.Id == transactionId select t).FirstOrDefault();
            lblPatientName.Text = currentTransaction.Customer.Title + " " + currentTransaction.Customer.Name;
            lblDate.Text = currentTransaction.DateTime.Value.ToString("dd-MM-yyyy");
            lblTime.Text = currentTransaction.DateTime.Value.ToString("hh:mm");
            lblMainTransaction.Text = transactionId;
           
            PackageProduct pkg;
            dgvPackageList.DataSource = null;
            dgvCancelledList.DataSource = null;
            dgvCancelledList.AutoGenerateColumns = false;
            dgvPackageList.AutoGenerateColumns = false;

            pkgList = new List<PackageProduct>();
            foreach (TransactionDetail td in currentTransaction.TransactionDetails)
            {
                if (td.IsDeleted != true && td.Product.IsPackage)
                {
                    pkg = new PackageProduct();
                    pkg.ProductCode = td.Product.ProductCode;
                    pkg.ItemName = td.Product.Name;
                    pkg.Qty =(int) td.Qty;
                    pkg.Unitprice = (decimal)td.UnitPrice;
                    pkg.Tax = td.TaxRate;
                    pkg.DiscountPercent = td.DiscountRate;
                    pkg.Cost = (decimal) ((td.UnitPrice - (td.UnitPrice * (td.DiscountRate / 100))) * td.Qty); 
                    pkg.IsFOC = (bool)td.IsFOC ? "FOC" : "";
                    pkg.TotalOffsetQty = (int)entity.PackagePurchasedInvoices.Where(x => x.TransactionDetailId == td.Id && x.IsDelete == false).Select(x => x.packageFrequency * td.Qty).FirstOrDefault();
                    PackagePurchasedInvoice pki = entity.PackagePurchasedInvoices.Where(x => x.TransactionDetailId == td.Id && x.IsDelete == false).FirstOrDefault();
                    if (pki != null)
                    {
                        pkg.UsedQty = pki.UseQty;
                        pkg.Frequency = (int) pki.packageFrequency; 
                    }
                   
                    pkg.ConsignmentPrice = td.ConsignmentPrice == null ? 0 : (decimal)td.ConsignmentPrice;
                    pkg.TransactionId = transactionId;
                    pkg.TransactionDetailId = td.Id;
                    pkg.IsCancelled = td.IsCancelled;
                  //  pkg.IsAllOffsetUsed = pkg.TotalOffsetQty == pkg.UsedQty;
                    pkgList.Add(pkg);

                }
            }
            dgvPackageList.DataSource = pkgList;
            decimal totalCancelledAmt = 0;          
            List<CancellationLog> cancelledList = currentTransaction.CancellationLogs.ToList();
            if (cancelledList.Count > 0)
            {
                dgvCancelledList.DataSource = cancelledList;
                totalCancelledAmt = cancelledList.Sum(x=> x.CancelledAmount);                
            }
            lblCancelledAmount.Text = totalCancelledAmt.ToString("n2");
        }

       
        #endregion
        #region Models
        internal class PackageProduct
        {
            public string ProductCode;
            public string ItemName;
            public int Qty;
            public int Frequency;
            public int TotalOffsetQty;
            public int UsedQty;
            public decimal Tax;
            public decimal Unitprice;
            public decimal DiscountPercent;
            public decimal Cost;
            public string IsFOC;
            public decimal ConsignmentPrice;
            public string TransactionId;
            public long TransactionDetailId;
            public bool IsCancelled;
         //   public bool IsAllOffsetUsed;
        }

        #endregion

      
    }
}
