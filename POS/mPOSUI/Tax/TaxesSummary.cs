using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using POS.APP_Data;

namespace POS
{
    public partial class TaxesSummary : Form
    {
        #region Variable

        POSEntities entity = new POSEntities();

        
        List<Transaction> tList = new List<Transaction>();    
        #endregion

        #region Event

        public TaxesSummary()
        {
            InitializeComponent();
        }

        private void TaxesSummary_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            LoadData();
            this.reportViewer1.RefreshReport();
        }

        private void rdbSale_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void rdbRefund_CheckedChanged(object sender, EventArgs e)
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            #region [ Print ]

            dsReportTemp dsReport = new dsReportTemp();
            
            dsReportTemp.TaxesListDataTable dtTaxesReport = (dsReportTemp.TaxesListDataTable)dsReport.Tables["TaxesList"];

            foreach (Transaction t in tList)
            {
                dsReportTemp.TaxesListRow newRow = dtTaxesReport.NewTaxesListRow();
                newRow.Date = Convert.ToDateTime(t.DateTime);
                newRow.TotalAmount = Convert.ToInt32(t.TaxAmount);
                dtTaxesReport.AddTaxesListRow(newRow);
            }
            
            

            string reportPath = "";
            ReportViewer rv = new ReportViewer();
            ReportDataSource rds = new ReportDataSource("TaxReport", dsReport.Tables["TaxesList"]);
            reportPath = Application.StartupPath + "\\Reports\\TaxesReport.rdlc";
            rv.Reset();
            rv.LocalReport.ReportPath = reportPath;
            rv.LocalReport.DataSources.Add(rds);


            ReportParameter TaxReportTitle = new ReportParameter("TaxReportTitle", gbList.Text + " for " + SettingController.ShopName);
            rv.LocalReport.SetParameters(TaxReportTitle);

            ReportParameter Date = new ReportParameter("Date", " from " + dtFrom.Value.Date.ToString("dd/MM/yyyy") + " To " + dtTo.Value.Date.ToString("dd/MM/yyyy"));
            reportViewer1.LocalReport.SetParameters(Date);

            PrintDoc.PrintReport(rv);
            #endregion
        }
        #endregion

        #region Function

        private void LoadData()
        {
            DateTime fromDate = dtFrom.Value.Date;
            DateTime toDate = dtTo.Value.Date;
            bool IsSale = rdbSale.Checked;
            tList.Clear();
           
            System.Data.Objects.ObjectResult<SelectTaxesListByDate_Result> resultList;
            resultList = entity.SelectTaxesListByDate(fromDate, toDate, IsSale);
            
            foreach (SelectTaxesListByDate_Result r in resultList)
            {
                Transaction t = new Transaction();
                t.DateTime = r.TDate;
                t.TaxAmount = r.Amount;
                tList.Add(t);
            }
            ShowReportViewer();
            //SelectTaxesListByDate_Result a = new SelectTaxesListByDate_Result();
            //a.
            if (IsSale)
            {
                gbList.Text = "Sales Tax Report";
            }
            else
            {
                gbList.Text = "Refund Tax Report";
            }
        }

        private void ShowReportViewer()
        {
            
            dsReportTemp dsReport = new dsReportTemp();
            dsReportTemp.TaxesListDataTable dtTaxesReport = (dsReportTemp.TaxesListDataTable)dsReport.Tables["TaxesList"];            
            
            foreach (Transaction t in tList)
            {
                dsReportTemp.TaxesListRow newRow = dtTaxesReport.NewTaxesListRow();
                newRow.Date = Convert.ToDateTime(t.DateTime);
                newRow.TotalAmount = Convert.ToInt32(t.TaxAmount);
                dtTaxesReport.AddTaxesListRow(newRow);
            }
            

            ReportDataSource rds = new ReportDataSource("TaxReport", dsReport.Tables["TaxesList"]);
            string reportPath = Application.StartupPath + "\\Reports\\TaxesReport.rdlc";
            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            ReportParameter TaxReportTitle = new ReportParameter("TaxReportTitle", gbList.Text + " for " + SettingController.ShopName);
            reportViewer1.LocalReport.SetParameters(TaxReportTitle);
            
            ReportParameter Date = new ReportParameter("Date", " from " + dtFrom.Value.Date.ToString("dd-MM-yyyy") + " To " + dtTo.Value.Date.ToString("dd-MM-yyyy"));
            reportViewer1.LocalReport.SetParameters(Date);
            reportViewer1.RefreshReport();
        }
        #endregion

        

       
    }
}
