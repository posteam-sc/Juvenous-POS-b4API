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
    public partial class PurchaseDetailList : Form
    {
        #region Variables

        POSEntities entity = new POSEntities();
        public int mainPurchaseId;
        public bool IsDelelog=false;
        public int PurDetailId = 0;
        #endregion

        #region Event

        public PurchaseDetailList()
        {
            InitializeComponent();
            CenterToScreen();            
        }

        private void PurchaseDetailList_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            loadData();            
        }

        public void loadData()
        {             
            if (IsDelelog)
            {
                dgvProductList.AutoGenerateColumns = false;
                MainPurchase currentMP = (from mp in entity.MainPurchases where mp.Id == mainPurchaseId select mp).FirstOrDefault();
                if (currentMP != null)
                {
                    lblSupplerName.Text = (currentMP.Supplier == null) ? "-" : currentMP.Supplier.Name;
                    lblDate.Text = currentMP.Date.Value.ToString("dd-MM-yyyy");
                    lblVoucherNo.Text = (currentMP.VoucherNo == null) ? "-" : currentMP.VoucherNo;
               
                    lblTotalAmount.Text = currentMP.TotalAmount.ToString();
                    lblcash.Text = currentMP.Cash.ToString();
                   // lblOldCredit.Text = currentMP.OldCreditAmount.ToString();
                    lblOldCredit.Text = (currentMP.TotalAmount - currentMP.DiscountAmount - currentMP.Cash).ToString();
                    lblSettlement.Text = currentMP.SettlementAmount.ToString();
                    lblDiscount.Text = currentMP.DiscountAmount.ToString();
                }

                if (PurDetailId == 0)
                {
//                    var gridData = (from pd in entity.PurchaseDetails where pd.MainPurchaseId == mainPurchaseId && pd.IsDeleted==false select pd).ToList();
                    var gridData = (from pd in entity.PurchaseDetails where pd.MainPurchaseId == mainPurchaseId && pd.IsDeleted == true select pd).ToList();
                    dgvProductList.DataSource = gridData;
                    lblTotalQty.Text = gridData.Sum(x => x.Qty).ToString();
                }
                else
                {
                   // var gridData = (from pd in entity.PurchaseDetails where pd.MainPurchaseId == mainPurchaseId && pd.IsDeleted == false select pd).ToList();
                    var gridData = (from pd in entity.PurchaseDetails where pd.MainPurchaseId == mainPurchaseId && pd.IsDeleted == true select pd).ToList();
                    dgvProductList.DataSource = gridData;
                    lblTotalQty.Text = gridData.Sum(x => x.Qty).ToString();
                }
            }
            else
            {
                dgvProductList.AutoGenerateColumns = false;
                MainPurchase currentMP = (from mp in entity.MainPurchases where mp.Id == mainPurchaseId && mp.IsDeleted == false select mp).FirstOrDefault();
                if (currentMP != null)
                {
                    lblSupplerName.Text = (currentMP.Supplier == null) ? "-" : currentMP.Supplier.Name;
                    lblDate.Text = currentMP.Date.Value.ToString("dd-MM-yyyy");
                    lblVoucherNo.Text = (currentMP.VoucherNo == null) ? "-" : currentMP.VoucherNo;
                    lblTotalAmount.Text = currentMP.TotalAmount.ToString();
                    lblcash.Text = currentMP.Cash.ToString();
                    //lblOldCredit.Text = currentMP.OldCreditAmount.ToString();
                    lblOldCredit.Text = (currentMP.TotalAmount - currentMP.DiscountAmount - currentMP.Cash).ToString();
                    lblSettlement.Text = currentMP.SettlementAmount.ToString();
                    lblDiscount.Text = currentMP.DiscountAmount.ToString();
                }
                var gridData = (from pd in entity.PurchaseDetails where pd.MainPurchaseId == mainPurchaseId && pd.IsDeleted == false select pd).ToList();
                dgvProductList.DataSource = gridData;
                lblTotalQty.Text = gridData.Sum(x => x.Qty).ToString();
            }
        }

        private void dgvProductList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvProductList.Rows)
            {
                PurchaseDetail purchaseDetailObj = (PurchaseDetail)row.DataBoundItem;
                row.Cells[0].Value = purchaseDetailObj.Id;
                row.Cells[1].Value = purchaseDetailObj.Product.Barcode;
                row.Cells[2].Value = purchaseDetailObj.Product.Name;
                row.Cells[3].Value = purchaseDetailObj.Qty;
                row.Cells[4].Value = purchaseDetailObj.UnitPrice;
                row.Cells[5].Value = purchaseDetailObj.expiredDate == null ? "" : purchaseDetailObj.expiredDate.Value.Date.ToString("dd-MM-yyyy");
            }
        }

        #endregion

      
        #region Function


        #endregion
    }
}
