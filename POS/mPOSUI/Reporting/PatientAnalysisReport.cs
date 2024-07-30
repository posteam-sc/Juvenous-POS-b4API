using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Objects;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using POS.APP_Data;

namespace POS
{
    
    public partial class frmPatientAnalysisReport : Form
    {
        #region Variables

        POSEntities entity = new POSEntities();        
        List<APP_Data.Customer> custList = new List<APP_Data.Customer>();
        List<PatientAnalysisReport_Result> PatientAnalysisReportList;
        List<PatientAnalysisReport_Result> cList = new List<PatientAnalysisReport_Result>();  
              
        #endregion

        #region Events

        public frmPatientAnalysisReport()
        {
            InitializeComponent();
        }


        private void PatientAnalysisReportTest_Load(object sender, EventArgs e)
        {
            cboName.SelectedIndexChanged -= cboName_SelectedIndexChanged;
            cboSortedColumns.SelectedIndexChanged -= cboSortedColumns_SelectedIndexChanged;         
            txtRow.Text = "All";
            cboSortedColumns.SelectedIndex = 0;
            PatientAnalysisReportList = new List<PatientAnalysisReport_Result>();
            Localization.Localize_FormControls(this);
            Bind_Customer();

            this.reportViewer1.RefreshReport();

            LoadData();

            cboName.SelectedIndexChanged += cboName_SelectedIndexChanged;
            cboSortedColumns.SelectedIndexChanged += cboSortedColumns_SelectedIndexChanged;
        }
        private void rdoASC_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoASC.Checked)
            {
                txtRow.Text = "All";
                cboName.SelectedIndex = 0;
                cboSortedColumns.SelectedIndex = 0;
                cList = PatientAnalysisReportList;
                ShowReportViewer();
            }
                
        }

        private void rdoDESC_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoDESC.Checked)
            {
                txtRow.Text = "All";
                cboName.SelectedIndex = 0;
                cboSortedColumns.SelectedIndex = 0;                
                cList = PatientAnalysisReportList;
                ShowReportViewer();
            }
            
        }

        private void btnRowFilter_Click(object sender, EventArgs e)
        {
            cboName.SelectedIndex = 0;
            if (rdoASC.Checked && cboSortedColumns.SelectedIndex > 0)
            {
                AscendingOrder();
            }
            else if (rdoDESC.Checked && cboSortedColumns.SelectedIndex > 0)
            {
                DescendingOrder();
            }
            else
            {
                cList = PatientAnalysisReportList;
            }
            int totalRow = 0;
            if (!string.Equals(txtRow.Text, "All"))
            {
                int.TryParse(txtRow.Text, out totalRow);
                cList = cList.Take(totalRow).ToList();
            }

            ShowReportViewer();
        }
        private void cboSortedColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdoASC.Checked && cboSortedColumns.SelectedIndex > 0)
            {
                AscendingOrder();           
            }
            else if (rdoDESC.Checked && cboSortedColumns.SelectedIndex > 0)
            {
               DescendingOrder();
            }
            ShowReportViewer();
        }
       
        

        private void cboName_SelectedIndexChanged(object sender, EventArgs e)
        {
            int custID = Convert.ToInt32(cboName.SelectedValue);            
            txtRow.Text = "All";
            rdoASC.Checked = false;
            rdoDESC.Checked = false;
            cboSortedColumns.SelectedIndex = 0;
            if (cboName.SelectedIndex == 0)
            {
               cList = PatientAnalysisReportList;
            }
            else
            {
                cList = PatientAnalysisReportList.Where(x => x.id == custID).ToList();
            }
            
            ShowReportViewer();
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtRow.Text = "All";
            cboName.SelectedIndex = 0;
            cboSortedColumns.SelectedIndex = 0;
            rdoASC.Checked = false;
            rdoDESC.Checked = false;           
          
            cList = PatientAnalysisReportList.ToList();
            ShowReportViewer();
        }

        

        #endregion

        #region Function
        private void Bind_Customer()
        {

            Customer cust = new Customer();
            cust.Id = 0;
            cust.Name = "All";           
            custList.Add(cust);
            custList.AddRange((from c in entity.Customers select c).ToList());
            cboName.DataSource = custList;
            cboName.DisplayMember = "Name";
            cboName.ValueMember = "Id";
            cboName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboName.AutoCompleteSource = AutoCompleteSource.ListItems;
        }
       
        private void LoadData()
        {
            Cursor.Current = Cursors.WaitCursor;
            Application.UseWaitCursor = true;
            
            cList.Clear();
            PatientAnalysisReportList = entity.PatientAnalysisReport().ToList();            
            cList = PatientAnalysisReportList;      

            ShowReportViewer();
            Application.UseWaitCursor = false;
            
        }
        public void DescendingOrder()
        {
            int columnIndex = cboSortedColumns.SelectedIndex;
            switch (columnIndex)
            {
                case 1:
                    cList = PatientAnalysisReportList.OrderByDescending(x => x.TotalSpentAmount).ToList();
                    break;
                case 2:
                    cList = PatientAnalysisReportList.OrderByDescending(x => x.OutstandingBalance).ToList();
                    break;
                case 3:
                    cList = PatientAnalysisReportList.OrderByDescending(x => x.VisitCount).ToList();
                    break;
                case 4:
                    cList = PatientAnalysisReportList.OrderByDescending(x => x.NoOfReferrals).ToList();
                    break;
                case 5:
                    cList = PatientAnalysisReportList.OrderByDescending(x => x.SpentAmountByReferrals).ToList();
                    break;
            }
            //ShowReportViewer();
        }
        public void AscendingOrder()
        {
            int columnIndex = cboSortedColumns.SelectedIndex;
            switch (columnIndex)
            {
                case 1:
                    cList = PatientAnalysisReportList.OrderBy(x => x.TotalSpentAmount).ToList();
                    break;
                case 2:
                    cList = PatientAnalysisReportList.OrderBy(x => x.OutstandingBalance).ToList();
                    break;
                case 3:
                    cList = PatientAnalysisReportList.OrderBy(x => x.VisitCount).ToList();
                    break;
                case 4:
                    cList = PatientAnalysisReportList.OrderBy(x => x.NoOfReferrals).ToList();
                    break;
                case 5:
                    cList = PatientAnalysisReportList.OrderBy(x => x.SpentAmountByReferrals).ToList();
                    break;
            }
           // ShowReportViewer();
        }
        private void ShowReportViewer()
        {            
            dsReportTemp dsReportTemp = new dsReportTemp();
            dsReportTemp.PatientAnalysisReportDataTable dtPatientAnalysis = (dsReportTemp.PatientAnalysisReportDataTable)dsReportTemp.Tables["PatientAnalysisReport"];

            foreach (PatientAnalysisReport_Result pl in cList)
            {
                dsReportTemp.PatientAnalysisReportRow newRow = dtPatientAnalysis.NewPatientAnalysisReportRow();
                newRow.CustomerCode = pl.CustomerCode;
                newRow.Name = pl.Name;
                newRow.DOB = pl.Birthday == null ? "" : pl.Birthday.Value.Date.ToString("dd-MM-yyyy");
                newRow.Age = pl.Age.ToString();
                newRow.PhNo = pl.PhoneNumber;
                newRow.Address = pl.Address;
                newRow.TotalSpentAmount = pl.TotalSpentAmount == null ? 0 : (long)pl.TotalSpentAmount;
                newRow.OutstandingBalance = pl.OutstandingBalance == null ? 0 : (long)pl.OutstandingBalance;
                newRow.VisitCount = pl.VisitCount == null ? 0: (int)pl.VisitCount;
                newRow.NoOfReferredPersons = pl.NoOfReferrals == null ? 0 : (int)pl.NoOfReferrals;
                newRow.TotalSpentAmtByReferredPersons = pl.SpentAmountByReferrals == null ? 0 : (long)pl.SpentAmountByReferrals;
                dtPatientAnalysis.AddPatientAnalysisReportRow(newRow);
            }

            ReportDataSource rds = new ReportDataSource("DataSet1", dsReportTemp.Tables["PatientAnalysisReport"]);
                    
            string reportPath = string.Empty;
            reportPath = Application.StartupPath + "\\Reports\\PatientAnalysisReport.rdlc";
           
            ReportParameter HeaderTitle = new ReportParameter("HeaderTitle", "Patient Analysis Report");            
           
            reportViewer1.LocalReport.SetParameters(HeaderTitle);       
            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);          

            reportViewer1.RefreshReport();
        }

        #endregion

    }

}
