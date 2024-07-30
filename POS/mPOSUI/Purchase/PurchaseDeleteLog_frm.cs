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
    public partial class PurchaseDeleteLog_frm : Form
    {
        #region Variables

        private POSEntities entity = new POSEntities();

        #endregion

        #region Events

        public PurchaseDeleteLog_frm()
        {
            InitializeComponent();
        }

       

        private void dgvPurchaseDeleteLog_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvPurchaseDeleteLog.Rows)
            {
                APP_Data.PurchaseDeleteLog current = (APP_Data.PurchaseDeleteLog)row.DataBoundItem;
                row.Cells[1].Value = current.Counter.Name;
                row.Cells[2].Value = current.User.Name;
            }
        }

        private void dgvPurchaseDeleteLogPartial_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvPurchaseDeleteLogPartial.Rows)
            {

                APP_Data.PurchaseDeleteLog current = (APP_Data.PurchaseDeleteLog)row.DataBoundItem;                
                row.Cells[1].Value = current.Counter.Name;
                row.Cells[2].Value = current.User.Name;
                row.Cells[3].Value = current.PurchaseDetail.Product.Name;
                row.Cells[4].Value = current.PurchaseDetail.Qty;
                row.Cells[5].Value = current.MainPurchaseId;
                row.Cells[8].Value = current.PurchaseDetail.Id;
            }
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        #endregion

        #region Function
        
        private void LoadData()
        {
            DateTime fromDate = dtFrom.Value.Date;
            DateTime toDate = dtTo.Value.Date;

          
            List<APP_Data.PurchaseDeleteLog> mainLog = (from d in entity.PurchaseDeleteLogs where System.Data.Objects.EntityFunctions.TruncateTime((DateTime)d.DeletedDate) >= fromDate && System.Data.Objects.EntityFunctions.TruncateTime((DateTime)d.DeletedDate) <= toDate && d.IsParent == true select d).ToList();
            dgvPurchaseDeleteLog.AutoGenerateColumns = false;
            dgvPurchaseDeleteLog.DataSource = mainLog;

            List<APP_Data.PurchaseDeleteLog> partialLog = (from d in entity.PurchaseDeleteLogs where System.Data.Objects.EntityFunctions.TruncateTime((DateTime)d.DeletedDate) >= fromDate && System.Data.Objects.EntityFunctions.TruncateTime((DateTime)d.DeletedDate) <= toDate && d.IsParent != true select d).ToList();
            dgvPurchaseDeleteLogPartial.AutoGenerateColumns = false;
            dgvPurchaseDeleteLogPartial.DataSource = partialLog;
        }
        #endregion



        private void dgvPurchaseDeleteLogPartial_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 7)
                {
                    int currentmainpurchsaeId = Convert.ToInt32(dgvPurchaseDeleteLogPartial.Rows[e.RowIndex].Cells[5].Value);
                    int purDetailId = Convert.ToInt32(dgvPurchaseDeleteLogPartial.Rows[e.RowIndex].Cells[8].Value);
                    PurchaseDetailList newform = new PurchaseDetailList();
                    newform.mainPurchaseId = currentmainpurchsaeId;
                    newform.PurDetailId = purDetailId;
                    newform.IsDelelog = true;
                    newform.ShowDialog();
                }
            }
        }

        private void dgvPurchaseDeleteLog_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 5)
                {
                   
                        int currentmainpurchsaeId = Convert.ToInt32(dgvPurchaseDeleteLog.Rows[e.RowIndex].Cells[3].Value);
                        PurchaseDetailList newform = new PurchaseDetailList();
                        newform.mainPurchaseId = currentmainpurchsaeId;
                        newform.IsDelelog = true;
                        newform.ShowDialog();
                }
            }
        }

        private void PurchaseDeleteLog_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            LoadData();
        }

       
    }
}
