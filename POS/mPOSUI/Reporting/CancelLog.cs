using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using POS.APP_Data;
using System.Data.Objects;

namespace POS
{
    public partial class CancelLog : Form
    {
        #region Variables
        POSEntities entity = new POSEntities();
        #endregion
        #region Events
        public CancelLog()
        {
            InitializeComponent();
        }

        private void CancelLog_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void rdoAll_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void rdoSales_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void rdoCredit_CheckedChanged(object sender, EventArgs e)
        {
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

        private void dgvCancelLog_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvCancelLog.Rows)
            {
                CancellationLog log = (CancellationLog)row.DataBoundItem;
                row.Cells[colTransactionId.Index].Value = log.TransactionId;
                row.Cells[colProductCode.Index].Value = log.TransactionDetail.Product.ProductCode;
                row.Cells[colItemName.Index].Value = log.TransactionDetail.Product.Name;
                row.Cells[colDatetime.Index].Value = log.DateTime.ToString("dd/MM/yyyy");
                row.Cells[colQty.Index].Value = log.CancelledQty.ToString();
                row.Cells[colCancelledAmount.Index].Value = log.CancelledAmount.ToString("n2");
                row.Cells[colCancelBy.Index].Value = log.User.Name;
                row.Cells[colType.Index].Value = log.Transaction.Type;
            }          
        }

        #endregion

        #region Methods
        private void LoadData()
        {
            dgvCancelLog.DataSource = null;
            dgvCancelLog.AutoGenerateColumns = false;
            lblCancelledAmount.Text = "0.0";
            IQueryable<CancellationLog> cancelledLog = entity.CancellationLogs.Where(x => EntityFunctions.TruncateTime(x.DateTime) >= dtpFrom.Value.Date && EntityFunctions.TruncateTime(x.DateTime) <= dtpTo.Value.Date);
            if (cancelledLog.Any())
            {
                List<CancellationLog> cancelledList = new List<CancellationLog>();
                if (rdoAll.Checked)
                {
                    cancelledList = cancelledLog.ToList(); 
                    
                }
                else if (rdoCredit.Checked)
                {
                    cancelledList = cancelledLog.Where(x => x.Transaction.Type == "Credit").ToList();                  
                }
                else if (rdoSales.Checked)
                {
                    cancelledList = cancelledLog.Where(x => x.Transaction.Type == "Sale").ToList();                    
                }
                decimal totalCancelledAmt = 0;
                if (cancelledList.Count > 0)
                {
                    dgvCancelLog.DataSource = cancelledList;
                    totalCancelledAmt = cancelledList.Sum(x => x.CancelledAmount);
                }
                lblCancelledAmount.Text = totalCancelledAmt.ToString("n2");
            }


        }
        #endregion

       
    }
}
