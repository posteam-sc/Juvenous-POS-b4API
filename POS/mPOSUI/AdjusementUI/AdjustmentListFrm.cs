//New


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

namespace POS
{
    public partial class AdjustmentListFrm : Form
    {        
        //
        #region Variable

        POSEntities entity = new POSEntities();
        ToolTip tp = new ToolTip();

        List<Adjustment> _dmglist = new List<Adjustment>();

        int Qty = 0;

        List<Stock_Transaction> productList = new List<Stock_Transaction>();

        bool IsStart = false;

        bool IsApproved = false;
        #endregion

        #region Function


        #region for saving Adjustment Qty in Stock Transaction table
        private void Save_AdjustmentQty_ToStockTransaction(List<Stock_Transaction> productList, DateTime _tranDate)
        {
            int _year, _month;

            _year = _tranDate.Year;
            _month = _tranDate.Month;
            Utility.Adjustment_Run_Process(_year, _month, productList);
        }
        #endregion

        private void Bind_Brand()
        {
            List<APP_Data.Brand> BrandList = new List<APP_Data.Brand>();
            entity = new POSEntities();
            APP_Data.Brand brandObj1 = new APP_Data.Brand();
            brandObj1.Id = 0;
            brandObj1.Name = "Select";
       
            BrandList.Add(brandObj1);
       
            BrandList.AddRange((from bList in entity.Brands select bList).ToList());
            cboBrand.DataSource = BrandList;
            cboBrand.DisplayMember = "Name";
            cboBrand.ValueMember = "Id";
            cboBrand.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboBrand.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void BindDgv()
        {
            loadData();
        }

        private DateTime TruncateTime(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        private Char Radio_Status()
        {
            Char _St = '\0';
            if(rdoAll.Checked)
            {
                _St='A';
            }
            else if (rdConsignment.Checked)
            {
                _St='C';
            }
            else
            {
                _St = 'N';
            }
            return _St;
        }

        private void Radio_Select()
        {
            if (rdoPending.Checked == true)
            {
                IsApproved = false;
            }
            else
            {
                IsApproved = true;
            }
        }
       

        public void loadData()
        {
            if (IsStart)
            {
                Radio_Select();
                dgvAdjustmentList.Columns["AdjustmentDateTime"].DefaultCellStyle.Format = "dd-MMM-yyyy";
                int _braindId = 0;
                if (cboBrand.SelectedIndex > 0)
                {
                    _braindId = Convert.ToInt32(cboBrand.SelectedValue);
                }

                Char _status = Radio_Status();

                dgvAdjustmentList.DataSource = null;
                DateTime fromDate = dtpk_fromDate.Value.Date;
                DateTime toDate = dtpk_toDate.Value.Date;
                int typeId = Convert.ToInt32(cboAdjType.SelectedValue);               
               
                entity = new POSEntities();

                if (IsApproved == true)
                {
                    dgvAdjustmentList.Columns[13].Visible = false;
                    dgvAdjustmentList.Columns[14].Visible = false;
                }
                else
                {
                    dgvAdjustmentList.Columns[13].Visible = true;
                    dgvAdjustmentList.Columns[14].Visible = true;
                }

                IQueryable<object> q = from d in entity.Adjustments
                                       join p in entity.Products on d.ProductId equals p.Id
                                       join adj in entity.AdjustmentTypes on d.AdjustmentTypeId equals adj.Id
                                       where d.IsDeleted == false
                                       && (EntityFunctions.TruncateTime((DateTime)d.AdjustmentDateTime) >= fromDate
                                       && EntityFunctions.TruncateTime((DateTime)d.AdjustmentDateTime) <= toDate)
                                       && ((_status == 'A' && 1 == 1) || (_status == 'C' && p.IsConsignment == true) || (_status == 'N' && p.IsConsignment == false))
                                       && ((_braindId == 0 && 1 == 1) || (_braindId != 0 && p.BrandId == _braindId)) && ((typeId == 0 && 1== 1) || (typeId != 0 && d.AdjustmentTypeId == typeId))
                                       && (d.IsApproved == IsApproved)
                                       select new
                                       {
                                           AdjustmentNo=d.AdjustmentNo,
                                           Id = d.Id,
                                           ProductId = d.ProductId,
                                           ProductCode = p.ProductCode,
                                           Name = p.Name,
                                           Price = p.Price,
                                           StockOut = d.AdjustmentQty < 0 ? d.AdjustmentQty * -1 : 0,
                                           StockIn = d.AdjustmentQty > 0 ? d.AdjustmentQty : 0,
                                           TotalCost = d.AdjustmentQty < 0 ? (d.AdjustmentQty * -1) * p.Price : d.AdjustmentQty * p.Price,
                                           AdjustmentDateTime = d.AdjustmentDateTime,
                                           Type=adj.Name,
                                           ResponsibleName = d.ResponsibleName,
                                           Reason = d.Reason
                                       };
                List<object> _adjustment = new List<object>(q);
                dgvAdjustmentList.AutoGenerateColumns = false;
                dgvAdjustmentList.DataSource = _adjustment;
                lblStockIn.Text = (dgvAdjustmentList.Rows.Cast<DataGridViewRow>()
                                                                           .Sum(t => Convert.ToInt32(t.Cells[5].Value))).ToString();

                lblStockOut.Text = (dgvAdjustmentList.Rows.Cast<DataGridViewRow>()
                                                               .Sum(t => Convert.ToInt32(t.Cells[6].Value))).ToString();
            }
        }
        #endregion

        #region Events
        public AdjustmentListFrm()
        {
            InitializeComponent();
            CenterToScreen();
        }
        private void rdConsignment_CheckedChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void cboAdjType_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void AdjustmentListFrm_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            dtpk_fromDate.Value = DateTime.Now.Date;
            dtpk_toDate.Value = DateTime.Now.Date;
            Bind_Brand();
            Utility.Bind_AdjustmentType(cboAdjType);            
         
            IsStart = true;
            loadData();
        }

        private void dgvAdjustmentlist_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Boolean HaveError = false;
            if (e.RowIndex >= 0)
            {
                //Role Management
                RoleManagementController controller = new RoleManagementController();
                controller.Load(MemberShip.UserRoleId);
                int dmgID = Convert.ToInt32(dgvAdjustmentList.CurrentRow.Cells["Id"].Value);

               
                // by SYM

                int currentAdjustmentID = Convert.ToInt32(dgvAdjustmentList.Rows[e.RowIndex].Cells[0].Value);
                string adjustmentNo = dgvAdjustmentList.Rows[e.RowIndex].Cells[1].Value.ToString();
                Adjustment _adjustment = (from a in entity.Adjustments where a.Id == currentAdjustmentID select a).FirstOrDefault();

                // Approved
                if (e.ColumnIndex == 13)
                {
                    if (_adjustment.IsApproved == true)
                    {
                        MessageBox.Show("This Invoice No. " + adjustmentNo + " is already Approved!", "Information");
                        return;
                    }
                    else
                    {
                        RoleManagementController roleManagementController = new RoleManagementController();
                        roleManagementController.Load(MemberShip.UserRoleId);
                        if (controller.PurchaseRole.Approved || MemberShip.isAdmin)
                        {
                            string stockQty = "";
                            DialogResult result = MessageBox.Show("Please note that you cannot edit  Adjustment list anymore after you clicked Approved.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (result.Equals(DialogResult.OK))
                            {
                                List<APP_Data.Adjustment> _adj = entity.Adjustments.Where(x => x.Id == currentAdjustmentID && x.IsDeleted == false).ToList();
                                int tempQty = 0;
                                int _adjQty = 0;


                                APP_Data.Product pdObj = entity.Products.Where(x => x.Id == _adjustment.ProductId).FirstOrDefault();
                                if (Convert.ToInt32(dgvAdjustmentList.Rows[e.RowIndex].Cells[6].Value) != 0)
                                {
                                    tempQty = Convert.ToInt32(dgvAdjustmentList.Rows[e.RowIndex].Cells[6].Value);
                                    _adjQty = tempQty;
                                    stockQty = "+" + Convert.ToString(_adjQty);
                                    //increate qty inproduct  after approved
                                    if (pdObj != null)
                                    {
                                        pdObj.Qty = pdObj.Qty + _adjQty;
                                        entity.Entry(pdObj).State = EntityState.Modified;
                                        entity.SaveChanges();

                                    }

                                    APP_Data.MainPurchase mainPurchaseObj = new MainPurchase();
                                    APP_Data.PurchaseDetail purchaseDetailObj = new PurchaseDetail();

                                    if (!HaveError)
                                    {
                                        mainPurchaseObj.SupplierId = null;
                                        mainPurchaseObj.Date = _adjustment.AdjustmentDateTime;
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


                                        purchaseDetailObj.ProductId = _adjustment.ProductId;
                                        purchaseDetailObj.Qty = _adjustment.AdjustmentQty;
                                        purchaseDetailObj.UnitPrice = 0;
                                        purchaseDetailObj.CurrentQy = _adjustment.AdjustmentQty;
                                        purchaseDetailObj.IsDeleted = false;
                                        purchaseDetailObj.Date = null;
                                        purchaseDetailObj.DeletedUser = null;
                                        purchaseDetailObj.Date = _adjustment.AdjustmentDateTime;
                                        purchaseDetailObj.ConvertQty = 0;


                                        mainPurchaseObj.PurchaseDetails.Add(purchaseDetailObj);
                                        entity.MainPurchases.Add(mainPurchaseObj);

                                        entity.Entry(mainPurchaseObj).State = EntityState.Added;
                                        entity.SaveChanges();
                                    }



                                }
                                else
                                {
                                    tempQty = Convert.ToInt32(dgvAdjustmentList.Rows[e.RowIndex].Cells[7].Value);
                                    _adjQty = tempQty;
                                    stockQty = "-" + Convert.ToString(_adjQty);
                                    //increate qty inproduct  after approved
                                    if (pdObj != null)
                                    {
                                        pdObj.Qty = pdObj.Qty - _adjQty;
                                        entity.Entry(pdObj).State = EntityState.Modified;
                                        entity.SaveChanges();

                                    }

                                    // Get purchase detail with same product Id and order by purchase date ascending
                                    List<APP_Data.PurchaseDetail> pulist = (from p in entity.PurchaseDetails
                                                                            join m in entity.MainPurchases on p.MainPurchaseId equals m.Id
                                                                            where p.ProductId == _adjustment.ProductId && p.IsDeleted == false && m.IsCompletedInvoice == true && p.CurrentQy > 0
                                                                            orderby p.Date ascending
                                                                            select p).ToList();

                                    if (pulist.Count > 0)
                                    {
                                        int TotalQty = Convert.ToInt32(pulist.Sum(x => x.CurrentQy));

                                        if (TotalQty >= _adjQty)
                                        {
                                            foreach (PurchaseDetail p in pulist)
                                            {
                                                if (_adjQty > 0)
                                                {
                                                    if (p.CurrentQy >= _adjQty)
                                                    {
                                                        p.CurrentQy = p.CurrentQy - _adjQty;
                                                        _adjQty = 0;

                                                        entity.Entry(p).State = EntityState.Modified;
                                                        entity.SaveChanges();
                                                        break;
                                                    }
                                                    else if (p.CurrentQy <= _adjQty)
                                                    {
                                                        _adjQty = Convert.ToInt32(_adjQty - p.CurrentQy);
                                                        p.CurrentQy = 0;

                                                        entity.Entry(p).State = EntityState.Modified;
                                                        entity.SaveChanges();

                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                _adjustment.IsApproved = true;

                                entity.Entry(_adjustment).State = EntityState.Modified;
                                entity.SaveChanges();

                                //save in stocktransaction
                                Stock_Transaction st = new Stock_Transaction();
                                st.ProductId = pdObj.Id;

                                st.AdjustmentQty = Convert.ToInt32(stockQty);

                                productList.Add(st);
                                Qty = 0;

                                Save_AdjustmentQty_ToStockTransaction(productList, Convert.ToDateTime(_adjustment.AdjustmentDateTime));
                                productList.Clear();

                                MessageBox.Show("Successfully Approved Adjustment no. " + adjustmentNo);

                                loadData();

                            }
                        }
                        else
                        {
                            MessageBox.Show("You are not allowed to approve Adjustment.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }

                    }
                }

                //Delete
                else if (e.ColumnIndex == 14)
                {
                    if (controller.Adjustment.EditOrDelete || MemberShip.isAdmin)
                    {
                        DialogResult result1 = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (result1.Equals(DialogResult.OK))
                        {

                            int _productid = Convert.ToInt32(dgvAdjustmentList.Rows[e.RowIndex].Cells[2].Value);
                            int tempQty = 0;
                            string _adjQty = "";
                            if (Convert.ToInt32(dgvAdjustmentList.Rows[e.RowIndex].Cells[6].Value) != 0)
                            {
                                tempQty = Convert.ToInt32(dgvAdjustmentList.Rows[e.RowIndex].Cells[5].Value);
                                _adjQty =  tempQty.ToString();
                            }
                            else
                            {
                                tempQty = Convert.ToInt32(dgvAdjustmentList.Rows[e.RowIndex].Cells[7].Value);
                                _adjQty = "-" + tempQty;
                            }
                            //update qty inproduct  after approve
                            APP_Data.Product pdObj = entity.Products.Where(x => x.Id == _productid).FirstOrDefault();
                            if (pdObj != null)
                            {
                                pdObj.Qty = pdObj.Qty +  (Convert.ToInt32(_adjQty) * -1);
                                //entity.Entry(pdObj).State = EntityState.Modified;
                                entity.SaveChanges();

                             
                            }

                            int _adjustmentId = Convert.ToInt32(dgvAdjustmentList.Rows[e.RowIndex].Cells[0].Value);
                            APP_Data.Adjustment _adjust = entity.Adjustments.Where(x => x.Id == _adjustmentId).FirstOrDefault();
                            _adjust.IsDeleted = true;
                            _adjust.DeletedDate = DateTime.Now;
                            _adjust.DeletedUserId = MemberShip.UserId;
                            entity.Entry(_adjust).State = EntityState.Modified;
                            entity.SaveChanges();

                            loadData();
                        }
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to delete damage permission.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void dtpk_fromDate_ValueChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void dtpk_toDate_ValueChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void cboshoplist_selectedIndexChanged(object sender, EventArgs e)
        {
            loadData();
        }

        // by SYM
        private void rdoApproved_CheckedChanged(object sender, EventArgs e)
        {
            Radio_Select();
            loadData();
        }
        // by SYM
        private void rdoPending_CheckedChanged(object sender, EventArgs e)
        {
            Radio_Select();
            loadData();
        }

        #endregion

       

     

    }
}
