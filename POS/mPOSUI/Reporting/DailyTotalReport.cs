using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting;
using POS.APP_Data;

using Microsoft.Reporting.WinForms;

namespace POS
{
    public partial class DailyTotalReport : Form
    {
        public DailyTotalReport()
        {
            InitializeComponent();
            CenterToScreen();
        }

        #region variable
        POSEntities entity = new POSEntities();
        List<TotalDailyReport> Tdrlist = new List<TotalDailyReport>();

        int totalTransaction,totalQty, totalRefundQty;
        Int64 totalCashAmount, totalCreditAmount, totalGiftCardAmount, totalFOCAmount,totalMPUAmount, totalTestAmount, totalRefundAmount, totalRepaidAmount;
        string counterName = "";


        int counterId = 0;
        #endregion

        private void DailyTotalReport_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            List<APP_Data.Counter> CounterList = new List<APP_Data.Counter>();
            APP_Data.Counter counterObj1 = new APP_Data.Counter();
            counterObj1.Id = 0;
            counterObj1.Name = "Select";
            CounterList.Add(counterObj1);
            CounterList.AddRange((from bList in entity.Counters select bList).ToList());
            cboCounterName.DataSource = CounterList;
            cboCounterName.DisplayMember = "Name";
            cboCounterName.ValueMember = "Id";
            cboCounterName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboCounterName.AutoCompleteSource = AutoCompleteSource.ListItems;
            loadData();
        }


        public void loadData()
        {

            entity = new APP_Data.POSEntities();
            counterId = 0;
            totalCashAmount = 0; totalCreditAmount = 0; totalGiftCardAmount = 0; totalFOCAmount = 0; totalMPUAmount=0; totalTestAmount = 0; totalRefundAmount = 0; totalRepaidAmount = 0;
            totalTransaction = 0; totalRefundQty = 0; totalQty = 0;
            counterName = "";


            Tdrlist.Clear();
            entity=new POSEntities();
            DateTime startDate = dtFrom.Value.Date;
            DateTime endDate = dtTo.Value.Date;

            if(cboCounterName.SelectedIndex > 0)
            {
                counterId = Convert.ToInt32(cboCounterName.SelectedValue);
                APP_Data.Counter c = entity.Counters.Where(x => x.Id == counterId).FirstOrDefault();
                counterName = "  For  " + c.Name;
            }
            while (startDate <= endDate)
            {
                TotalDailyReport tdObj = new TotalDailyReport();

               //for Total TransactionNumber and Qty

                System.Data.Objects.ObjectResult<GetTotalTransactionQtyAndQty_Result> totalTran = entity.GetTotalTransactionQtyAndQty(startDate,counterId);
                foreach (GetTotalTransactionQtyAndQty_Result tr in totalTran)
                {
                    //tdObj.Date =Convert.ToDateTime(tr.SaleDate);

                    tdObj.Date = Convert.ToDateTime(startDate.Date);
                    tdObj.TotalTransaction = (Convert.ToInt32(tr.TotalTransaction) == 0) ? 0 : Convert.ToInt32(tr.TotalTransaction);
                    tdObj.TotalQty = Convert.ToInt32(tr.TotalQty);

                    totalTransaction += Convert.ToInt32(tdObj.TotalTransaction);
                    totalQty += Convert.ToInt32(tdObj.TotalQty);  
                }                
                //for Total CashAmount

                System.Data.Objects.ObjectResult<GetTotalAmountForCash_Result> totalCash = entity.GetTotalAmountForCash(startDate,1,counterId);
                foreach (GetTotalAmountForCash_Result tr in totalCash)
                {
                    tdObj.TotalCashAmount = Convert.ToInt64(tr.TotalAmount);
                    totalCashAmount += Convert.ToInt64(tdObj.TotalCashAmount);
                }
                //for Total Credit

                System.Data.Objects.ObjectResult<GetTotalAmountForCash_Result> totalCredit = entity.GetTotalAmountForCash(startDate,2,counterId);
                foreach (GetTotalAmountForCash_Result tr in totalCredit)
                {
                    tdObj.TotalCreditAmount = Convert.ToInt64(tr.TotalAmount);
                    totalCreditAmount += Convert.ToInt64(tdObj.TotalCreditAmount);
                }
                //for Total GiftCard

                System.Data.Objects.ObjectResult<GetTotalAmountForCash_Result> totalGiftCard = entity.GetTotalAmountForCash(startDate, 3,counterId);
                foreach (GetTotalAmountForCash_Result tr in totalGiftCard)
                {
                    tdObj.TotalGiftCardAmount = Convert.ToInt64(tr.TotalAmount);
                    totalGiftCardAmount += Convert.ToInt64(tdObj.TotalGiftCardAmount);
                }
                //for Total FOC

                System.Data.Objects.ObjectResult<GetTotalAmountForCash_Result> totalFOC = entity.GetTotalAmountForCash(startDate, 4,counterId);
                foreach (GetTotalAmountForCash_Result tr in totalFOC)
                {
                    tdObj.TotalFOCAmount = Convert.ToInt64(tr.TotalAmount);
                    totalFOCAmount += Convert.ToInt64(tdObj.TotalFOCAmount);
                }
                //for Total MPU
                System.Data.Objects.ObjectResult<GetTotalAmountForCash_Result> totalMPU = entity.GetTotalAmountForCash(startDate, 5,counterId);
                foreach (GetTotalAmountForCash_Result tr in totalMPU)
                {
                    tdObj.TotalMPUAmount = Convert.ToInt64(tr.TotalAmount);
                    totalMPUAmount  += Convert.ToInt64(tdObj.TotalMPUAmount);
                }
                //for Total MPU

                System.Data.Objects.ObjectResult<GetTotalAmountForRefund_Result> totalReund = entity.GetTotalAmountForRefund(startDate,counterId);
                foreach (GetTotalAmountForRefund_Result tr in totalReund)
                {
                    tdObj.TotalRefundAmount = Convert.ToInt64(tr.TotalAmount);
                    tdObj.TotalRefundQty = Convert.ToInt64(tr.TotalQty);

                    totalRefundAmount += Convert.ToInt64(tdObj.TotalRefundAmount);
                    totalRefundQty += Convert.ToInt32(tdObj.TotalRefundQty);
                }
                //GetTotalPrepaidAmount

                System.Data.Objects.ObjectResult<GetTotalAmountForPrepaid_Result> totalPrepaid = entity.GetTotalAmountForPrepaid(startDate,1,counterId);
                foreach (GetTotalAmountForPrepaid_Result tr in totalPrepaid)
                {
                    tdObj.RepaidAmount = Convert.ToInt64(tr.TotalAmount);
                    totalRepaidAmount += Convert.ToInt64(tdObj.RepaidAmount);
                }             
               
                tdObj.Date = startDate.Date;
                Tdrlist.Add(tdObj);
                startDate = startDate.AddDays(1);
                
            }
            ShowReportViewr();
        }
        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void cboCounterName_SelectedValueChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void ShowReportViewr()
        {
            dsReportTemp dsReport = new dsReportTemp();
            dsReportTemp.DailyTotalSaleDataTable dtDailyTotalSale = (dsReportTemp.DailyTotalSaleDataTable)dsReport.Tables["DailyTotalSale"];

            foreach (TotalDailyReport tdObj in Tdrlist)
            {
                dsReportTemp.DailyTotalSaleRow newRow = dtDailyTotalSale.NewDailyTotalSaleRow();
                newRow.Date = tdObj.Date.ToString("dd/MM/yyyy");
                newRow.TotalTransaction = tdObj.TotalTransaction.ToString();
                newRow.TotalQty = tdObj.TotalQty.ToString();
                newRow.TotalCashAmount = tdObj.TotalCashAmount.ToString();
                newRow.TotalCreditAmount = tdObj.TotalCreditAmount.ToString();
                newRow.TotalGiftCardAmount = tdObj.TotalGiftCardAmount.ToString();
                newRow.TotalMPUAmount = tdObj.TotalMPUAmount.ToString();
                newRow.TotalFOCAmount = tdObj.TotalFOCAmount.ToString();
                newRow.TotalTesterAmount = tdObj.TotalTesterAmount.ToString();
                newRow.TotalRefundAmount = tdObj.TotalRefundAmount.ToString();
                newRow.TotalRefundQty = tdObj.TotalRefundQty.ToString();
                newRow.RepaidAmount = tdObj.RepaidAmount.ToString();
                dtDailyTotalSale.AddDailyTotalSaleRow(newRow);   
            }

            ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["DailyTotalSale"]);
            string reportPath = Application.StartupPath + "\\Reports\\DailyTotalSale.rdlc";
            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            ReportParameter Header = new ReportParameter("Header", " Daily Total Transaction Report From  " +dtFrom.Value.ToString("dd-MM-yyyy")+ " To " + dtTo.Value.ToString("dd-MM-yyyy") + counterName );
            reportViewer1.LocalReport.SetParameters(Header);

            ReportParameter TotalTransaction = new ReportParameter("TotalTransaction", totalTransaction.ToString());
            reportViewer1.LocalReport.SetParameters(TotalTransaction);

            ReportParameter TotalQty = new ReportParameter("TotalQty",totalQty.ToString());
            reportViewer1.LocalReport.SetParameters(TotalQty);

            ReportParameter TotalCashAmount = new ReportParameter("TotalCashAmount",totalCashAmount.ToString());
            reportViewer1.LocalReport.SetParameters(TotalCashAmount);

            ReportParameter TotalCreditAmount = new ReportParameter("TotalCreditAmount",totalCreditAmount.ToString());
            reportViewer1.LocalReport.SetParameters(TotalCreditAmount);

            ReportParameter TotalGiftCardAmount = new ReportParameter("TotalGiftCardAmount", totalGiftCardAmount.ToString());
            reportViewer1.LocalReport.SetParameters(TotalGiftCardAmount);

            ReportParameter TotalMPUAmount = new ReportParameter("TotalMPUAmount", totalMPUAmount.ToString());
            reportViewer1.LocalReport.SetParameters(TotalMPUAmount);

            ReportParameter TotalFOCAmount = new ReportParameter("TotalFOCAmount",totalFOCAmount.ToString());
            reportViewer1.LocalReport.SetParameters(TotalFOCAmount);

            ReportParameter TotalTestAmount = new ReportParameter("TotalTestAmount", totalTestAmount.ToString());
            reportViewer1.LocalReport.SetParameters(TotalTestAmount);

            ReportParameter TotalRefundQty = new ReportParameter("TotalRefundQty", totalRefundQty.ToString());
            reportViewer1.LocalReport.SetParameters(TotalRefundQty);

            ReportParameter TotalRefundAmount = new ReportParameter("TotalRefundAmount", totalRefundAmount.ToString());
            reportViewer1.LocalReport.SetParameters(TotalRefundAmount);

            ReportParameter TotalRepaidAmount = new ReportParameter("TotalRepaidAmount", totalRepaidAmount.ToString());
            reportViewer1.LocalReport.SetParameters(TotalRepaidAmount);
            reportViewer1.RefreshReport();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dtFrom.Value = DateTime.Now;
            dtTo.Value = DateTime.Now;

            List<APP_Data.Counter> CounterList = new List<APP_Data.Counter>();
            APP_Data.Counter counterObj1 = new APP_Data.Counter();
            counterObj1.Id = 0;
            counterObj1.Name = "Select";
            CounterList.Add(counterObj1);
            CounterList.AddRange((from bList in entity.Counters select bList).ToList());
            cboCounterName.DataSource = CounterList;
            cboCounterName.DisplayMember = "Name";
            cboCounterName.ValueMember = "Id";
            cboCounterName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboCounterName.AutoCompleteSource = AutoCompleteSource.ListItems;
            loadData();

        }
    }
}
