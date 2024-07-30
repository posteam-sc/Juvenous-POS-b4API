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
    public partial class AdjustmentDeleteLog : Form
    {
      //---
   
        #region Variables

        private POSEntities entity = new POSEntities();
        bool IsStart = false;
        #endregion

        #region Events

        public AdjustmentDeleteLog()
        {
            InitializeComponent();
        }

        private void DamageDeleteLog_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            Utility.BindShop(cboshoplist);
            cboshoplist.Text = SettingController.DefaultShop.ShopName;
            Utility.ShopComBo_EnableOrNot(cboshoplist);
            Utility.Bind_AdjustmentType(cboAdjType);
            IsStart = true;
            LoadData();
        }

        private void cboAdjType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cboshoplist_selectedindexchanged(object sender, EventArgs e)
        {
            LoadData();
        }

        #endregion

        #region Function
        
        private void LoadData()
        {
            if (IsStart)
            {
                DateTime fromDate = dtFrom.Value.Date;
                DateTime toDate = dtTo.Value.Date;

                int typeId = Convert.ToInt32(cboAdjType.SelectedValue);
                int shopid = Convert.ToInt32(cboshoplist.SelectedValue);
                string currentshortcode = (from p in entity.Shops where p.Id == shopid select p.ShortCode).FirstOrDefault();
                entity = new POSEntities();

                IQueryable<object> q = from d in entity.Adjustments
                                       join p in entity.Products on d.ProductId equals p.Id
                                       join u in entity.Users on d.DeletedUserId equals u.Id
                                       join adj in entity.AdjustmentTypes on d.AdjustmentTypeId equals adj.Id
                                       where d.IsDeleted == true
                                       && (EntityFunctions.TruncateTime((DateTime)d.AdjustmentDateTime) >= fromDate
                                       && EntityFunctions.TruncateTime((DateTime)d.AdjustmentDateTime) <= toDate)
                                       && ((typeId == 0 && 1 == 1) || (typeId != 0 && d.AdjustmentTypeId == typeId))
                                       && d.AdjustmentNo.Substring(3,2)==currentshortcode
                                       select new
                                       {
                                           AdjustmentNo=d.AdjustmentNo,
                                           DamageId = d.Id,
                                           DeletedDate = d.DeletedDate,
                                           DeletedUser = u.Name,
                                           ProductName = p.Name,
                                           UnitPrice = p.Price,
                                           StockIn = d.AdjustmentQty > 0 ? d.AdjustmentQty : 0,
                                           StockOut = d.AdjustmentQty < 0 ? d.AdjustmentQty * -1 : 0,
                                           TotalCost = d.AdjustmentQty < 0 ? (d.AdjustmentQty * p.Price * -1) : (d.AdjustmentQty * p.Price),
                                           DamageDateTime = d.AdjustmentDateTime,
                                           ResponsibleName = d.ResponsibleName,
                                           Type=adj.Name,
                                           Reason = d.Reason
                                       };
                List<object> _adjustment = new List<object>(q);
                dgvAdjustmentDeleteLog.AutoGenerateColumns = false;
                dgvAdjustmentDeleteLog.DataSource = _adjustment;

                lblStockIn.Text = (dgvAdjustmentDeleteLog.Rows.Cast<DataGridViewRow>()
                                                                       .Sum(t => Convert.ToInt32(t.Cells[5].Value))).ToString();

                lblStockOut.Text = (dgvAdjustmentDeleteLog.Rows.Cast<DataGridViewRow>()
                                                               .Sum(t => Convert.ToInt32(t.Cells[6].Value))).ToString();
            }
        }
        #endregion

    }
}
