using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using POS.APP_Data;
using Microsoft.Reporting.WinForms;
using System.Data.Objects;

namespace POS
{
    public partial class CancelReport : Form
    {
        #region Variables
        POSEntities entity = new POSEntities();
        #endregion
        public CancelReport()
        {
            InitializeComponent();
        }

        private void CancelReport_Load(object sender, EventArgs e)
        {
            this.cancelledReportViewer.Refresh();
            LoadData();
        }

        private void rdoSales_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void rdoAll_CheckedChanged(object sender, EventArgs e)
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

        }
        #region Methods
        private void LoadData()
        {
            List<CancellationLog> cancelledList = new List<CancellationLog>();
            IQueryable<CancellationLog> cancelledLog = entity.CancellationLogs.Where(x => EntityFunctions.TruncateTime(x.DateTime) >= dtpFrom.Value.Date && EntityFunctions.TruncateTime(x.DateTime) <= dtpTo.Value.Date);
            if (cancelledLog.Any())
            {
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
                if (cancelledList != null)
                {
                    dsReportTemp dsReportTemp = new dsReportTemp();
                    dsReportTemp.CancelReportDataTable dtCancelReport = (dsReportTemp.CancelReportDataTable)dsReportTemp.Tables["CancelReport"];

                    foreach (CancellationLog cl in cancelledList)
                    {
                        dsReportTemp.CancelReportRow newRow = dtCancelReport.NewCancelReportRow();
                        newRow.TransactionId = cl.TransactionId;
                        newRow.ProductCode = cl.TransactionDetail.Product.ProductCode;
                        newRow.ItemName = cl.TransactionDetail.Product.Name;
                        newRow.DateTime = cl.DateTime.ToShortDateString();
                        newRow.CancelledQty = cl.CancelledQty;
                        newRow.CancelledAmount = cl.CancelledAmount;
                        newRow.CancelledBy = cl.User.Name;
                        newRow.Type = cl.Transaction.Type;
                        dtCancelReport.AddCancelReportRow(newRow);
                    }

                    ReportDataSource rds = new ReportDataSource("DataSet1", dsReportTemp.Tables["CancelReport"]);

                    string reportPath = string.Empty;
                    reportPath = Application.StartupPath + "\\Reports\\CancelReport.rdlc";

                    cancelledReportViewer.LocalReport.ReportPath = reportPath;
                    cancelledReportViewer.LocalReport.DataSources.Clear();
                    cancelledReportViewer.LocalReport.DataSources.Add(rds);

                    ReportParameter Period = new ReportParameter("Period", " From " + dtpFrom.Value.Date.ToString("dd-MM-yyyy") + " To " + dtpTo.Value.Date.ToString("dd-MM-yyyy"));

                    cancelledReportViewer.LocalReport.SetParameters(Period);

                    cancelledReportViewer.RefreshReport();

                }

            }
           

        }
        #endregion
    }
}
