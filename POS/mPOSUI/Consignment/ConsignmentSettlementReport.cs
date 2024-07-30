using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace POS
{
    public partial class ConsignmentSettlementReport : Form
    {
        #region Initialize
        public ConsignmentSettlementReport()
        {
            InitializeComponent();
        }
        #endregion

        #region Variable
        public  List<object> _Data = new List<object>();
      public string Consignor = "";
      public string ConsignmentNo = "";
      public string SettlementDate = "";
      public string Month = "";
      public string Comment = "";
        #endregion

      #region Event
      private void ConsignmentSettlementReport_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            #region Print
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "ConsignmentSettlement_DataSet";
            rds.Value = _Data;


            string reportPath = Application.StartupPath + "\\Reports\\ConsignmentSettlementReport.rdlc";

            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            ReportParameter _monthName= new ReportParameter("Month", Month);
            reportViewer1.LocalReport.SetParameters(_monthName);

            ReportParameter _Consignor = new ReportParameter("Consignor", Consignor);
            reportViewer1.LocalReport.SetParameters(_Consignor);

            ReportParameter _ConsignmentNo = new ReportParameter("ConsignmentNo", ConsignmentNo);
            reportViewer1.LocalReport.SetParameters(_ConsignmentNo);

            ReportParameter _SettlementDate = new ReportParameter("SettlementDate", SettlementDate);
            reportViewer1.LocalReport.SetParameters(_SettlementDate);

            reportViewer1.RefreshReport();
            #endregion
        }
      #endregion
    }
}
