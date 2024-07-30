using Microsoft.Reporting.WinForms;
using POS.APP_Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace POS.mPOSUI.Reporting
{
    public partial class DoctorOffsetReport : Form
    {
        POSEntities entity = new POSEntities();

        public DoctorOffsetReport()
        {
            InitializeComponent();
        }

        private void DoctorOffsetReport_Load(object sender, EventArgs e)
        {
            DateTime fromDate = dtFromDate.Value.Date;
            DateTime toDate = dtToDate.Value.Date;
            bindDoctor();
            reportLoadData();
            this.rpDoctorOffSetReport.RefreshReport();

        }

        public void bindDoctor()
        {
            entity = new POSEntities();
            List<APP_Data.Customer> doctorList = new List<APP_Data.Customer>();
            APP_Data.Customer doctor = new APP_Data.Customer();
            doctor.Id = 0;
            doctor.Name = "Select";
            doctorList.Add(doctor);
            doctorList.AddRange(entity.Customers.Where(x => x.CustomerTypeId != 1).ToList());
            cboDoctor.DataSource = doctorList;
            cboDoctor.DisplayMember = "Name";
            cboDoctor.ValueMember = "Id";
        }

        public void reportLoadData()
        {
            int doctorId = Convert.ToInt32(cboDoctor.SelectedValue);

            if (doctorId != 0)
            {
                var data = (from d in entity.PackageUsedHistories
                            where d.UsedDate >= dtFromDate.Value.Date && d.UsedDate <= dtToDate.Value.Date && (d.CustomerIDAsDoctor == doctorId
                            || d.CustomerIDAsAssistantNurse == doctorId || d.CustomerIDAsTherapist == doctorId)
                            && d.IsDelete == false
                            select new
                            {
                                UseQty = d.UseQty,
                                DoctorName = d.CustomerIDAsDoctor == 0 ? null : d.CustomerIDAsDoctor,
                                TherapistName = d.CustomerIDAsTherapist == 0 ? null : d.CustomerIDAsTherapist,
                                AssistantNurseName = d.CustomerIDAsAssistantNurse == 0 ? null : d.CustomerIDAsAssistantNurse,
                                PackageName = d.PackagePurchasedInvoice.Product.Id,
                                Remark = d.Remark,
                                date = d.UsedDate,
                                PriceOfOffsetProcdure = (d.PackagePurchasedInvoice.TransactionDetail.TotalAmount / d.PackagePurchasedInvoice.UseQty)

                            }).ToList();

                if (data.Count > 0)
                {
                    List<DoctorOffset> DoctorOffsetList = new List<DoctorOffset>();
                    foreach (var item in data)
                    {
                        DoctorOffset doctorOffSet = new DoctorOffset();
                        doctorOffSet.UseQty = item.UseQty;
                        doctorOffSet.DoctorName = (from c in entity.Customers where c.Id == item.DoctorName select c.Name).SingleOrDefault();
                        doctorOffSet.TherapistName = (from c in entity.Customers where c.Id == item.TherapistName select c.Name).SingleOrDefault();
                        doctorOffSet.AssistantNurseName = (from c in entity.Customers where c.Id == item.AssistantNurseName select c.Name).SingleOrDefault();
                        doctorOffSet.PackageName = (from p in entity.Products where p.Id == item.PackageName select p.Name).SingleOrDefault();
                        doctorOffSet.Remark = item.Remark;
                        doctorOffSet.Date = item.date.ToString("dd-MM-yyyy");
                        doctorOffSet.PriceOfOffsetProcdure = Convert.ToInt32(item.PriceOfOffsetProcdure);
                        DoctorOffsetList.Add(doctorOffSet);
                    }

                    rpDoctorOffSetReport.Visible = true;
                    ReportDataSource rds = new ReportDataSource();
                    rds.Name = "DoctorOffset";
                    rds.Value = DoctorOffsetList;

                    string reportPath = Application.StartupPath + "\\Reports\\DoctorOffset.rdlc";
                    rpDoctorOffSetReport.LocalReport.ReportPath = reportPath;
                    rpDoctorOffSetReport.LocalReport.DataSources.Clear();
                    rpDoctorOffSetReport.LocalReport.DataSources.Add(rds);
                    ReportParameter Date = new ReportParameter("Date", " from " + dtFromDate.Value.Date.ToString("dd-MM-yyyy") + " To " + dtToDate.Value.Date.ToString("dd-MM-yyyy"));
                    rpDoctorOffSetReport.LocalReport.SetParameters(Date);
                    rpDoctorOffSetReport.RefreshReport();

                }
                else
                {
                    MessageBox.Show("There is no data to show!");
                }
            }
            else
            {

                var data = (from d in entity.PackageUsedHistories
                            where d.UsedDate >= dtFromDate.Value.Date && d.UsedDate <= dtToDate.Value.Date
                            && d.IsDelete == false
                            select new
                            {
                                UseQty = d.UseQty,
                                DoctorName =  d.CustomerIDAsDoctor == 0 ? null : d.CustomerIDAsDoctor,
                                TherapistName = d.CustomerIDAsTherapist == 0 ? null : d.CustomerIDAsTherapist,
                                AssistantNurseName = d.CustomerIDAsAssistantNurse == 0 ? null : d.CustomerIDAsAssistantNurse,
                                PackageName = d.PackagePurchasedInvoice.Product.Id,
                                Remark = d.Remark,
                                date = d.UsedDate,
                                PriceOfOffsetProcdure = (d.PackagePurchasedInvoice.TransactionDetail.TotalAmount / d.PackagePurchasedInvoice.UseQty)
                            }).ToList();
                if (data.Count > 0)
                {
                    List<DoctorOffset> DoctorOffsetList = new List<DoctorOffset>();
                    foreach (var item in data)
                    {
                        DoctorOffset doctorOffSet = new DoctorOffset();
                        doctorOffSet.UseQty = item.UseQty;
                        doctorOffSet.DoctorName = (from c in entity.Customers where c.Id == item.DoctorName select c.Name).SingleOrDefault();
                        doctorOffSet.TherapistName = (from c in entity.Customers where c.Id == item.TherapistName select c.Name).SingleOrDefault();
                        doctorOffSet.AssistantNurseName = (from c in entity.Customers where c.Id == item.AssistantNurseName select c.Name).SingleOrDefault();
                        doctorOffSet.PackageName = (from p in entity.Products where p.Id == item.PackageName select p.Name).SingleOrDefault();
                        doctorOffSet.Remark = item.Remark;
                        doctorOffSet.Date = item.date.ToString("dd-MM-yyyy");
                        doctorOffSet.PriceOfOffsetProcdure = Convert.ToInt32(item.PriceOfOffsetProcdure);
                        DoctorOffsetList.Add(doctorOffSet);
                    }

                    rpDoctorOffSetReport.Visible = true;
                    ReportDataSource rds = new ReportDataSource();
                    rds.Name = "DoctorOffset";
                    rds.Value = DoctorOffsetList;

                    string reportPath = Application.StartupPath + "\\Reports\\DoctorOffset.rdlc";
                    rpDoctorOffSetReport.LocalReport.ReportPath = reportPath;
                    rpDoctorOffSetReport.LocalReport.DataSources.Clear();
                    rpDoctorOffSetReport.LocalReport.DataSources.Add(rds);
                    ReportParameter Date = new ReportParameter("Date", " from " + dtFromDate.Value.Date.ToString("dd-MM-yyyy") + " To " + dtToDate.Value.Date.ToString("dd-MM-yyyy"));
                    rpDoctorOffSetReport.LocalReport.SetParameters(Date);
                    rpDoctorOffSetReport.RefreshReport();
                }
                else
                {
                    MessageBox.Show("There is no data to show!");
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            reportLoadData();
        }

        class DoctorOffset
        {
            public int UseQty { get; set; }
            public string DoctorName { get; set; }
            public string TherapistName { get; set; }
            public string AssistantNurseName { get; set; }
            public string PackageName { get; set; }
            public string Date { get; set; }
            public int PriceOfOffsetProcdure { get; set; }
            public String Remark { get; set; }
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            dtFromDate.Value = DateTime.Now;
            dtToDate.Value = DateTime.Now;
            cboDoctor.SelectedValue = 0;
            reportLoadData();
        }
    }
}
