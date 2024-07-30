

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
using System.Data.Objects;
using Microsoft.Reporting.WinForms;

namespace POS
{
    public partial class StockReceiveList : Form
    {
        #region variable
        POSEntities entity = new POSEntities();
        bool Approved = false;
        DateTime _fromDate = new DateTime();
        DateTime _toDate = new DateTime();
        bool _startProcess = false;
        bool stockInStatus = true;
        bool stocktranStatus = false;
        bool stockReturnStatus = false;
   
          List<strockInheader> StockProductList = new List<strockInheader>();
          List<Stock_Transaction> productList = new List<Stock_Transaction>();
        #endregion
        #region event
        public StockReceiveList()
        {
            InitializeComponent();
        }

        private void StockReceiveList_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            searchStatus();
            Default_Date();
            Radio_Status();
     

            toolStripShopStatus.Text = SettingController.DefaultShop.ShopName;
            _startProcess = true;
            LoadData();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            Date_Assign();
            LoadData();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            Date_Assign();
            LoadData();
        }
        #endregion


        private void LoadData()
        {
            if (_startProcess == true)
            {

                if (Approved == true)
                {
                    dgvStockReceive.Columns[6].Visible = false;
                    dgvStockReceive.Columns[7].Visible = false;
                    dgvStockReceive.Columns[8].Visible = false;
                }
                else
                {
                    dgvStockReceive.Columns[6].Visible = true;
                    dgvStockReceive.Columns[7].Visible = true;
                    dgvStockReceive.Columns[8].Visible = true;
                }

       

                List<stockheader> stocklist = new List<stockheader>();
                string txtstockcode = txtStockCode.Text;
                if (stockInStatus == true)
                {
                    dgvStockReceive.Columns["FromShop"].HeaderText = "From Shop";
                    var gridview = (from p in entity.StockInHeaders

                                    where // ((txtstockcode == "" && ((EntityFunctions.TruncateTime(p.Date.Value) >= _fromDate)
                                    //     && (EntityFunctions.TruncateTime(p.Date.Value) <= _toDate)))
                                    //     || (txtstockcode != "" && p.StockCode == txtstockcode)
                                    //     )
                                         //|| (_fromDate == null && _toDate == null))
                                        //&&
                                        p.IsDelete == false && p.IsApproved == Approved &&
                                        
                                           p.FromShopId!=SettingController.DefaultShop.Id &&
                                           p.ToShopId==SettingController.DefaultShop.Id
                                      
                                    orderby p.StockCode,p.Date descending
                                    select new
                                    {
                                        id= p.id,
                                        StockInHeaderId = p.StockCode,
                                        Date = p.Date.Value,
                                        FromShop =(from s in entity.Shops where s.Id== p.FromShopId select s.ShopName).FirstOrDefault(),
                                        Status=p.Status
                                    }).ToList();

                    foreach (var grid in gridview)
                    {
                        stockheader stock3 = new stockheader();
                        stock3.StockId = grid.id;
                        stock3.StockCode = grid.StockInHeaderId;
                        stock3.date = grid.Date;
                        stock3.fromshop = grid.FromShop;
                        stock3.status = grid.Status;
                        int Count = stocklist.Count();
                        if (Count > 0)
                        {


                            stocklist.Insert(Count, stock3);
                        }
                        else
                        {
                            stocklist.Add(stock3);
                        }
                    }
               
                }
                else if(stocktranStatus==true || stockReturnStatus==true)
                {
                    dgvStockReceive.Columns["FromShop"].HeaderText = "To Shop";
                    var gridview = (from p in entity.StockInHeaders

                                    where ((txtstockcode == "" && ((EntityFunctions.TruncateTime(p.Date.Value) >= _fromDate)
                                                   && (EntityFunctions.TruncateTime(p.Date.Value) <= _toDate)))
                                                   || (txtstockcode != "" && p.StockCode == txtstockcode)
                                                   )
                                   
                                                  && p.IsDelete == false && p.IsApproved == Approved &&
                                    
                                            p.FromShopId == SettingController.DefaultShop.Id &&
                                           p.ToShopId != SettingController.DefaultShop.Id &&
                                         (p.Status == "StockTransfer" || p.Status == "StockReturn") 
                                    orderby p.Date descending
                                    select new
                                    {
                                        id = p.id,
                                        StockInHeaderId = p.StockCode,
                                        Date = p.Date,
                                        FromShop = (from s in entity.Shops where s.Id == p.ToShopId select s.ShopName).FirstOrDefault(),
                                        Status=p.Status
                                    }).ToList();
                    foreach (var grid in gridview)
                    {
                        stockheader stock3 = new stockheader();
                        stock3.StockId = grid.id;
                        stock3.StockCode = grid.StockInHeaderId;
                        stock3.date = grid.Date;
                        stock3.fromshop = grid.FromShop;
                        stock3.status = grid.Status;
                        int Count = stocklist.Count();
                        if (Count > 0)
                        {


                            stocklist.Insert(Count, stock3);
                        }
                        else
                        {
                            stocklist.Add(stock3);
                        }
                    }
                }
 


                dgvStockReceive.AutoGenerateColumns = false;
                dgvStockReceive.DataSource = stocklist;
               
            }
        }


        private void Default_Date()
        {
            dtTo.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            dtFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            Date_Assign();
        }

        private void Date_Assign()
        {
            _fromDate = dtFrom.Value;
            _toDate = dtTo.Value;
        }


        private void CheckDuplicate()
        {
          
        }
        private void dgvStockReceive_cellclick(object sender, DataGridViewCellEventArgs e)
        {
            Boolean HaveError=false ;
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (e.RowIndex >= 0)
            {
                //RoleManagementController controller = new RoleManagementController();
                //controller.Load(MemberShip.UserRoleId);
                Boolean viewDetail = true;
                long stockId = Convert.ToInt32(dgvStockReceive.Rows[e.RowIndex].Cells[1].Value);
                StockInHeader stock = (from p in entity.StockInHeaders where p.id == stockId select p).FirstOrDefault();
                string toshop = (from p in entity.Shops where p.Id == stock.ToShopId select p.ShopName).FirstOrDefault();
                string fromshop = (from p in entity.Shops where p.Id == stock.FromShopId select p.ShopName).FirstOrDefault();
                     var stockdetail = (from p in entity.StockInDetails where p.StockInHeaderId == stock.id select p).ToList();
                    //StockProductList.AddRange(stockdetail.Select(_stock =>
                    //    new strockInheader
                    //    {
                    //        StockInHeaderId=_stock.StockInHeaderId,
                    //        StockInDetailId=_stock.Id,
                    //        productId=_stock.ProductId,
                    //        productname=_stock.Product.Name,
                    //        Qty=_stock.Qty,
                    //        barcode=_stock.Product.Barcode

                    //    }));
             foreach(var p in stockdetail)
             {
                 strockInheader s = new strockInheader();
                          s.StockInHeaderId=p.StockInHeaderId;
                            s.StockInDetailId=p.Id;
                            s.productId=p.ProductId;
                            s.productname=p.Product.Name;
                            s.Qty=p.Qty;
                            s.barcode = p.Product.Barcode;
                            StockProductList.Add(s);
             }
                if (e.ColumnIndex == 5)
                {
                    //if (MemberShip.isAdmin)
                    //{
                    if (stock.IsApproved == true)
                    {
                        StockTransReturnForm newform = new StockTransReturnForm();
                        newform.StockTransId = stockId;
                        newform.viewdetail = viewDetail;
                        newform.stockTranferStatus = stocktranStatus;
                        newform.StockInStatus = stockInStatus;
                        newform.StockReturnStatus = stockReturnStatus;
                        newform.btnvisible = true;
                        newform.ShowDialog();
                    }
                    else
                    {
                        StockTransReturnForm newform = new StockTransReturnForm();
                        newform.StockTransId = stockId;
                        newform.viewdetail = viewDetail;
                        newform.stockTranferStatus = stocktranStatus;
                        newform.StockInStatus = stockInStatus;
                        newform.StockReturnStatus = stockReturnStatus;
                        newform.btnvisible = false;
                        newform.ShowDialog();
                    }
                    //}
                    //else
                    //{
                    //    MessageBox.Show("You are not allowed to view Stock detail.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //}

                }
                else if (e.ColumnIndex == 6)//edit
                {
                    if (!MemberShip.isAdmin && !controller.StockManagement.EditOrDelete )
                    {
                        MessageBox.Show("You don't have permission to Edit or Delete Stock Transaction");
                        return;
                    }
                    if (stockInStatus == true && Approved == false && stock.ToShopId == SettingController.DefaultShop.Id && stock.Status != "StockIn")
                    {
                        this.dgvStockReceive.Rows[e.RowIndex].Cells[6].ReadOnly = true;

                        return;

                    }
                    else
                    {
                        if (stock.IsApproved == true)
                        {
                            MessageBox.Show("You have already approved Stock transaction No. " + stock.StockCode + " You cannot edit it anymore.");
                            return;
                        }
                        else
                        {

                            StockTransReturnForm newform = new StockTransReturnForm();
                            newform.StockTransId = stockId;
                            newform.stockTranferStatus = stocktranStatus;
                            newform.StockInStatus = stockInStatus;
                            newform.StockReturnStatus = stockReturnStatus;
                            newform.ShowDialog();
                        }
                    }
                    //}
                    //else
                    //{
                    //    MessageBox.Show("You are not allowed to edit Stock .", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //}
                }
                else if (e.ColumnIndex == 7)//approve
                {
               
                    if (!MemberShip.isAdmin && !controller.StockManagement.Approved)
                    {
                        MessageBox.Show("You don't have permission to Approve Stock Transaction");
                        return;
                    }
                    if (stock.IsApproved == true)
                        {
                            MessageBox.Show("This Stock No. " + stock.StockCode + " is already Approved!", "Information");
                            return;
                        }
                        else
                        {
                            DialogResult result1 = DialogResult.Cancel;
                            var SameStock = (from p in entity.StockInHeaders where p.id != stock.id && p.StockCode == stock.StockCode && p.FromShopId == stock.FromShopId && p.ToShopId == stock.ToShopId select p).FirstOrDefault();
                            if (SameStock != null)
                            {
                                result1 = MessageBox.Show("Please note that  you cannot edit  Stock transaction  anymore and duplicate stock transaction number is automatic deleted after you clicked Approved. ", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            }
                            else
                            {
                                result1 = MessageBox.Show("Please note that you cannot edit  Stock transaction  anymore after you clicked Approved. ", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            }
                            if (result1.Equals(DialogResult.OK))
                            {
                                string status = stock.Status;
                                stock.IsApproved = true;
                                stock.UpdatedDate = DateTime.Now;
                                stock.UpdatedUser = MemberShip.UserId;

                                entity.Entry(stock).State = EntityState.Modified;
                                entity.SaveChanges();


                            //    List<StockInDetail> stockdetail = (from p in entity.StockInDetails where p.StockInHeaderId == stockId select p).ToList();
                                foreach (var st in stockdetail)
                                {
                                    APP_Data.Product pro = entity.Products.Where(x => x.Id == st.ProductId).FirstOrDefault();

                                    if (pro != null)
                                    {
                                         Stock_Transaction FStockTrans = new Stock_Transaction();
                                        FStockTrans.ProductId = pro.Id;
                                        int _qty = 0;
                                        if ((status == "StockIn" || status == "StockTransfer" || status == "StockReturn") && stock.ToShopId == SettingController.DefaultShop.Id)
                                        {
                                            pro.Qty = pro.Qty + st.Qty;
                                            _qty =Convert.ToInt32(st.Qty);
                                            FStockTrans.StockIn = _qty;
                                            entity.Entry(pro).State = EntityState.Modified;
                                            entity.SaveChanges();
                                             // by SYM                 
                                            APP_Data.MainPurchase mainPurchaseObj = new MainPurchase();
                                            APP_Data.PurchaseDetail purchaseDetailObj = new PurchaseDetail();
                                            if (!HaveError)
                                            {
                                                mainPurchaseObj.SupplierId = null;
                                                mainPurchaseObj.Date = Convert.ToDateTime(dgvStockReceive.Rows[e.RowIndex].Cells[2].Value);
                                                mainPurchaseObj.VoucherNo = null;
                                                mainPurchaseObj.TotalAmount = 0;
                                                mainPurchaseObj.Cash = 0;
                                                mainPurchaseObj.OldCreditAmount = 0;
                                                mainPurchaseObj.SettlementAmount = 0;
                                                mainPurchaseObj.IsActive = true;
                                                mainPurchaseObj.DiscountAmount = 0;
                                                mainPurchaseObj.IsDeleted = false;
                                                mainPurchaseObj.CreatedDate = DateTime.Now;
                                                mainPurchaseObj.CreatedBy = MemberShip.UserId;
                                                mainPurchaseObj.UpdatedDate = null;
                                                mainPurchaseObj.UpdatedBy = null;
                                                mainPurchaseObj.IsCompletedInvoice = true;
                                                mainPurchaseObj.IsCompletedPaid = true;
                                                mainPurchaseObj.IsPurchase = false;

                                                purchaseDetailObj.ProductId = Convert.ToInt64(st.ProductId);
                                                purchaseDetailObj.Qty = Convert.ToInt32(st.Qty);
                                                purchaseDetailObj.UnitPrice = 0;
                                                purchaseDetailObj.CurrentQy = Convert.ToInt32(st.Qty);
                                                purchaseDetailObj.IsDeleted = false;
                                                purchaseDetailObj.Date = null;
                                                purchaseDetailObj.DeletedUser = null;
                                                purchaseDetailObj.Date = Convert.ToDateTime(dgvStockReceive.Rows[e.RowIndex].Cells[2].Value);
                                                purchaseDetailObj.ConvertQty = 0;

                                                mainPurchaseObj.PurchaseDetails.Add(purchaseDetailObj);
                                                entity.MainPurchases.Add(mainPurchaseObj);

                                                //entity.Entry(mainPurchaseObj).State = EntityState.Added;
                                                entity.SaveChanges();
                                            }


                                        }
                                        else if ((status == "StockTransfer" || status == "StockReturn") && stock.FromShopId == SettingController.DefaultShop.Id)
                                        {
                                            pro.Qty = pro.Qty - st.Qty;
                                            _qty = Convert.ToInt32(st.Qty);
                                            FStockTrans.StockOut = _qty;
                                            entity.Entry(pro).State = EntityState.Modified;
                                            entity.SaveChanges();
                                            // by SYM  

                                            int stockQty = Convert.ToInt32(st.Qty);
                                            int pId = Convert.ToInt32(st.ProductId);

                                            // Get purchase detail with same product Id and order by purchase date ascending
                                            List<APP_Data.PurchaseDetail> pulist = (from p in entity.PurchaseDetails
                                                                                    join m in entity.MainPurchases on p.MainPurchaseId equals m.Id
                                                                                    where p.ProductId == pId && p.IsDeleted == false && m.IsCompletedInvoice == true && p.CurrentQy > 0
                                                                                    orderby p.Date ascending
                                                                                    select p).ToList();

                                            if (pulist.Count > 0)
                                            {
                                                int TotalQty = Convert.ToInt32(pulist.Sum(x => x.CurrentQy));

                                                if (TotalQty >= stockQty)
                                                {
                                                    foreach (PurchaseDetail p in pulist)
                                                    {
                                                        if (stockQty > 0)
                                                        {
                                                            if (p.CurrentQy >= stockQty)
                                                            {
                                                                p.CurrentQy = p.CurrentQy - stockQty;
                                                                stockQty = 0;

                                                                entity.Entry(p).State = EntityState.Modified;
                                                                entity.SaveChanges();
                                                                break;
                                                            }
                                                            else if (p.CurrentQy <= stockQty)
                                                            {
                                                                stockQty = Convert.ToInt32(stockQty - p.CurrentQy);
                                                                p.CurrentQy = 0;

                                                                entity.Entry(p).State = EntityState.Modified;
                                                                entity.SaveChanges();

                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        //entity.Entry(pro).State = EntityState.Modified;
                                        //entity.SaveChanges();





                                        productList.Add(FStockTrans);


                                




                                        if (SameStock != null)
                                        {
                                            SameStock.IsDelete = true;
                                            entity.Entry(SameStock).State = EntityState.Modified;
                                            entity.SaveChanges();
                                        }
                                    }
                                }
                                Save_StockQty_ToStockTransaction(productList, Convert.ToDateTime(stock.Date));
                                productList.Clear();
                                MessageBox.Show("Successfully Approved Stock no. " + stock.StockCode);

                      

                            }

                        }
                    //}
                    //else
                    //{
                    //    MessageBox.Show("You are not allowed to approve this Stock  .", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //}
                }
                else if (e.ColumnIndex == 8)//delete
                {
                    if (!MemberShip.isAdmin && !controller.StockManagement.EditOrDelete)
                    {
                        MessageBox.Show("You don't have permission to Edit or Delete Stock Transaction");
                        return;
                    }
                    if (stock.IsApproved == false)
                        {

                            DialogResult result1 = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            if (result1.Equals(DialogResult.OK))
                            {


                                APP_Data.StockInHeader stockheader = entity.StockInHeaders.Where(x => x.id == stockId).FirstOrDefault();

                            
                                string StockInheaderId = stockheader.id.ToString();
                                stockheader.IsDelete = true;

                                entity.Entry(stockheader).State = EntityState.Modified;
                                entity.SaveChanges();
                                MessageBox.Show("Successfully deleted Stock no. " + stockheader.StockCode);
                            }
                        }
                        else
                        {
                            MessageBox.Show("You have already approved Stock transaction No. " + stock.StockCode + " You cannot delete it anymore.");
                            return;
                        }
                    }
                //}
                //else
                //{
                //    MessageBox.Show("You are not allowed to delete this Stock  .", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //}
            }
            LoadData();
            //dgvStockReceive.Rows[0].Cells[0].Selected = false;
            //if (e.ColumnIndex >= 0)
            //{
            //    dgvStockReceive.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
            //}
       
        }

     

        private void Radio_Status()
        {
            if (rdopending.Checked == true)
            {
                Approved = false;
            }
            else
            {
                Approved = true;
            }
        }

        private void rdopending_checkedChanged(object sender, EventArgs e)
        {
            Radio_Status();
            Date_Assign();
            LoadData();
        }

        private void rdoStockIn_CheckedChanged(object sender, EventArgs e)
        {
            Radio_stock();
            LoadData();
        }

        private void Radio_stock()
        {
            if (rdoStockIn.Checked == true)
            {
                stockInStatus = true;
                stocktranStatus = false;
                stockReturnStatus = false;
            }
            else if(rdoStockTranfer.Checked==true)
            {
                stockInStatus = false;
                stocktranStatus = true;
                stockReturnStatus = true;
              
            }
      
        }

        private void rdoStockTran_CheckedChanged(object sender, EventArgs e)
        {
            Radio_stock();
            LoadData();
        }

        private void dgvStockReceive_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {


            //var dupvalues = (from row in entity.StockInHeaders.AsEnumerable()
            //                 let stockcode = row.StockCode
            //                 let Fromshopid = row.FromShopId
            //                 let Toshopid = row.ToShopId
            //                 where row.IsApproved == false && row.IsDelete == false
            //                 group row by new { stockcode, FromShop, Toshopid } into grp
            //                 where grp.Count() > 1
            //                 select new
            //                 {
            //                     dupcode = grp.Key.stockcode
            //                 }).ToArray();

            //    for (int i = 0; i < dupvalues.Count(); i++)
            //    {
                    foreach (DataGridViewRow row in dgvStockReceive.Rows)
                    {
                        stockheader stock = (stockheader)row.DataBoundItem;
                        row.Cells[0].Value = stock.StockCode.Trim();
                        row.Cells[1].Value = Convert.ToInt64(stock.StockId);
                         row.Cells[2].Value = stock.date.ToString();
                        row.Cells[3].Value = stock.fromshop.Trim(); ;
                        row.Cells[4].Value = stock.status.Trim();

                      //  if (dupvalues[i].dupcode.ToString() == row.Cells[0].Value.ToString())
                      //  {
                            // row.DefaultCellStyle.BackColor =Color.LightBlue;
                       // }
                        //if (stockInStatus == true && Approved==false && stock.fromshop != SettingController.DefaultShop.ShopName && stock.status!="StockIn")
                        //{ 
                        //    row.Cells[6].ReadOnly = true;
                        
                        //}
                    }
            //    }
            //}

         
        }

        bool IsTheSameCellValue(int column, int row)
        {
         
            DataGridViewCell currCell =

                dgvStockReceive.Rows[row].Cells[column];

            DataGridViewCell prevCell =

               dgvStockReceive.Rows[row-1].Cells[column];



            if ((currCell.Value == prevCell.Value) ||

               (currCell.Value == "" && prevCell.Value != null) || (prevCell.Value != null &&

               currCell.Value.ToString() == prevCell.Value.ToString()))
            {

                return true;

            }

            else
            {

                return false;

            }
        }

        private void dgvStockReceive_cellpainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

  
            e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;

            // Ignore column and row headers and first row

            if (e.RowIndex <= 0)
            {
                if(dgvStockReceive.Rows.Count == 1)
                {
                e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Single;
                }
                return;
            }
            if (e.RowIndex == 1 || e.RowIndex == 2)
            {
            }

            if (e.RowIndex ==4|| e.RowIndex == 5)
            {
            }

            if (IsTheSameCellValue(0, e.RowIndex))
            {
                e.AdvancedBorderStyle.Top =
                  DataGridViewAdvancedCellBorderStyle.None;
            }
            else
            {
                e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.Inset;
            }

             if (e.RowIndex == dgvStockReceive.Rows.Count - 1 || e.RowIndex==-1)
             {
        
                     e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Single;
                
             }
        }

        private void dgvStockReceive_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex == 0)

                return;





            if (IsTheSameCellValue(0, e.RowIndex))
            {
                DataGridViewCell currCell =

             dgvStockReceive.Rows[e.RowIndex].Cells[0];
                currCell.Value = string.Empty;

                e.FormattingApplied = true;

            }

        }
        private void searchStatus()
        {
            if (rdbDate.Checked == true)
            {
                gpByPeriod.Enabled = true;
                groupBox2.Enabled = false;
                txtStockCode.Text = "";
            }
            else
            {
                gpByPeriod.Enabled = false;
                groupBox2.Enabled = true;
            }
        }

        private void rdbDate_CheckedChanged(object sender, EventArgs e)
        {
            searchStatus();
        }

        private void RdbId_CheckedChanged(object sender, EventArgs e)
        {
            searchStatus();
            LoadData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }


        #region for saving Adjustment Qty in Stock Transaction table
        private void Save_StockQty_ToStockTransaction(List<Stock_Transaction> productList, DateTime _tranDate)
        {
            int _year, _month;

            _year = _tranDate.Year;
            _month = _tranDate.Month;
            Utility.Stock_Run_Process(_year, _month, productList);
        }
        #endregion

 
            
   
    }
}

