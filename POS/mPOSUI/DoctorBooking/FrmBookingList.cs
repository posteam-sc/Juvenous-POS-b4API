using POS.APP_Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS.mPOSUI.DoctorBooking
{
    public partial class FrmBookingList : Form
    {
        POSEntities entity = new POSEntities();

        public FrmBookingList()
        {
            InitializeComponent();
        }

        private void FrmBookingList_Load(object sender, EventArgs e)
        {
            bindPatient();
            bindBookingList(0,DateTime.MinValue,rdoBooking.Checked);
            dgvBookingList.AutoGenerateColumns = false;
        }
        public void LoadData()
        {
            int patientId = Convert.ToInt32(cboPatient.SelectedValue);
            DateTime chooseDate = dtDate.Value.Date;
            bindBookingList(patientId, chooseDate,rdoBooking.Checked);
        }
        public void bindPatient()
        {
            entity = new POSEntities();
            List<APP_Data.Customer> patientList = new List<APP_Data.Customer>();
            APP_Data.Customer patient = new APP_Data.Customer();
            patient.Id = 0;
            patient.Name = "Select";
            patientList.Add(patient);
            patientList.AddRange(entity.Customers.ToList());
            cboPatient.DataSource = patientList;
            cboPatient.DisplayMember = "Name";
            cboPatient.ValueMember = "Id";
        }
        public void bindBookingList(int patientId,DateTime selectDate,bool IsBook)
        {
            dgvBookingList.AutoGenerateColumns = false;
            if(patientId==0&& selectDate== DateTime.MinValue)//not filter..
            {
                var doctorBookingList = (from db in entity.DoctorBookings
                                         join ds in entity.DoctorSchedules on db.DoctorScheduleId equals ds.DoctorScheduleId
                                         join ppi in entity.PackagePurchasedInvoices on db.PackageId equals ppi.ProductId
                                         join p in entity.Products on ppi.ProductId equals p.Id
                                         where db.IsDelete == false && db.IsBook==IsBook
                                         select new
                                         {
                                             DoctorBookingId = db.DoctorBookingId,
                                             PatientName =db.Customer.Name,
                                             DoctorName = db.Customer1.Name,
                                             AssistNurseName =db.Customer2.Name,
                                             PackageName = p.Name,
                                             BookingDate = db.FromDate,
                                             BookingDay = db.Day,
                                             FromTime = db.FromTime,
                                             ToTime = db.ToTime,
                                             BookingCase=db.BookingCase
                                       }).Distinct().ToList();
                dgvBookingList.DataSource = doctorBookingList;
            }
            else if(patientId != 0 && selectDate == DateTime.MinValue)//filter with patient id..
            {
                var doctorBookingList = (from db in entity.DoctorBookings
                                         join ds in entity.DoctorSchedules on db.DoctorScheduleId equals ds.DoctorScheduleId
                                         join ppi in entity.PackagePurchasedInvoices on db.PackageId equals ppi.ProductId
                                         join p in entity.Products on ppi.ProductId equals p.Id
                                         where db.IsDelete == false && db.IsBook == IsBook && db.PatientId== patientId
                                         select new
                                         {
                                             DoctorBookingId = db.DoctorBookingId,
                                             PatientName = db.Customer.Name,
                                             DoctorName = db.Customer1.Name,
                                             AssistNurseName = db.Customer2.Name,
                                             PackageName = p.Name,
                                             BookingDate = db.FromDate,
                                             BookingDay = db.Day,
                                             FromTime = db.FromTime,
                                             ToTime = db.ToTime,
                                             BookingCase = db.BookingCase
                                       }).Distinct().ToList();
                dgvBookingList.DataSource = doctorBookingList;
            }
            else if(patientId == 0 && selectDate != DateTime.MinValue)// filter selected date..
            {
                var doctorBookingList = (from db in entity.DoctorBookings
                                         join ds in entity.DoctorSchedules on db.DoctorScheduleId equals ds.DoctorScheduleId
                                         join ppi in entity.PackagePurchasedInvoices on db.PackageId equals ppi.ProductId
                                         join p in entity.Products on ppi.ProductId equals p.Id
                                         where db.IsDelete == false && db.IsBook == IsBook && db.FromDate==selectDate
                                         select new
                                         {
                                             DoctorBookingId = db.DoctorBookingId,
                                             PatientName = db.Customer.Name,
                                             DoctorName = db.Customer1.Name,
                                             AssistNurseName = db.Customer2.Name,
                                             PackageName = p.Name,
                                             BookingDate = db.FromDate,
                                             BookingDay = db.Day,
                                             FromTime = db.FromTime,
                                             ToTime = db.ToTime,
                                             BookingCase = db.BookingCase
                                       }).Distinct().ToList();
                dgvBookingList.DataSource = doctorBookingList;
            }
            if(patientId != 0 && selectDate != DateTime.MinValue)// filter patient and select date..
            {
                var doctorBookingList = (from db in entity.DoctorBookings
                                         join ds in entity.DoctorSchedules on db.DoctorScheduleId equals ds.DoctorScheduleId
                                         join ppi in entity.PackagePurchasedInvoices on db.PackageId equals ppi.ProductId
                                         join p in entity.Products on ppi.ProductId equals p.Id
                                         where db.IsDelete == false && db.IsBook == IsBook && db.PatientId==patientId && db.FromDate==selectDate
                                         select new
                                         {
                                             DoctorBookingId = db.DoctorBookingId,//0
                                             PatientName = db.Customer.Name,//1
                                             DoctorName = db.Customer1.Name,//2
                                             AssistNurseName = db.Customer2.Name,//3
                                             PackageName = p.Name,//4
                                             BookingDate = db.FromDate,//5
                                             BookingDay = db.Day,//6
                                             FromTime = db.FromTime,//7
                                             ToTime = db.ToTime,//7
                                             BookingCase = db.BookingCase//8
                                       }).Distinct().ToList();
                dgvBookingList.DataSource = doctorBookingList;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgvBookingList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 10) { //confirm 
                    DialogResult result = MessageBox.Show("Are you sure to cancel this booking record?", "Confrim", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (result.Equals(DialogResult.OK)) {
                        int index = e.RowIndex;// get the Row Index
                        DataGridViewRow selectedRow = this.dgvBookingList.Rows[index];
                        string doctorBookingId = selectedRow.Cells[0].Value.ToString();
                        APP_Data.DoctorBooking doctorBooking = new APP_Data.DoctorBooking();
                        doctorBooking = (from d in entity.DoctorBookings where  d.DoctorBookingId== doctorBookingId select d).FirstOrDefault();
                        doctorBooking.IsBook = false;
                        doctorBooking.IsConfirm = true;
                        entity.DoctorBookings.AddOrUpdate(doctorBooking);
                        int i=entity.SaveChanges();
                        if (i > 0) {
                            MessageBox.Show("Doctor Booking Record is canceled..", "Infromation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.bindBookingList(0, DateTime.MinValue, rdoBooking.Checked);
                        }
                        }
                }
                else if (e.ColumnIndex == 11)//edit
                {
                    int index = e.RowIndex;// get the Row Index
                    DataGridViewRow selectedRow = this.dgvBookingList.Rows[index];
                    string doctorBookingId = selectedRow.Cells[0].Value.ToString();
                    APP_Data.DoctorBooking doctorBooking = new APP_Data.DoctorBooking();
                    doctorBooking = (from d in entity.DoctorBookings where d.DoctorBookingId == doctorBookingId select d).FirstOrDefault();
                    FrmDoctorBooking frmDoctorBooking = new FrmDoctorBooking();
                    frmDoctorBooking.DoctorBookingEntity = doctorBooking;
                    frmDoctorBooking.IsEdit = true;
                    frmDoctorBooking.Show();
                }
                else if (e.ColumnIndex == 12)//delete
                {
                    DialogResult result = MessageBox.Show("Are you sure to delete this booking record?", "Delete Confrim", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (result.Equals(DialogResult.OK)) {
                        int index = e.RowIndex;// get the Row Index
                        DataGridViewRow selectedRow = this.dgvBookingList.Rows[index];
                        string doctorBookingId = selectedRow.Cells[0].Value.ToString();
                        APP_Data.DoctorBooking doctorBooking = new APP_Data.DoctorBooking();
                        doctorBooking = (from d in entity.DoctorBookings where d.DoctorBookingId == doctorBookingId select d).FirstOrDefault();
                        doctorBooking.IsDelete = true;
                        entity.DoctorBookings.AddOrUpdate(doctorBooking);
                        int i = entity.SaveChanges();
                        if (i > 0) {
                            MessageBox.Show("successfully Delete", "Infromation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.bindBookingConfirmList(0, DateTime.MinValue, rdoConfirm.Checked);
                        }
                        }
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.bindBookingList(0, DateTime.MinValue,rdoBooking.Checked);
            cboPatient.SelectedIndex = 0;
            rdoBooking.Checked = false;
            rdoConfirm.Checked = false;
            rdoBooking.Checked = true;
        }

        private void rdoBooking_CheckedChanged(object sender, EventArgs e) {
            bindBookingList(0, DateTime.MinValue, rdoBooking.Checked);
        }

        private void rdoConfirm_CheckedChanged(object sender, EventArgs e) {
            bindBookingConfirmList(0, DateTime.MinValue, rdoConfirm.Checked);
        }
        public void bindBookingConfirmList(int patientId, DateTime selectDate, bool IsConfirm) {
            dgvBookingList.AutoGenerateColumns = false;
            if (patientId == 0 && selectDate == DateTime.MinValue)//not filter..
            {
                var doctorBookingList = (from db in entity.DoctorBookings
                                         join ds in entity.DoctorSchedules on db.DoctorScheduleId equals ds.DoctorScheduleId
                                         join ppi in entity.PackagePurchasedInvoices on db.PackageId equals ppi.ProductId
                                         join p in entity.Products on ppi.ProductId equals p.Id
                                         where db.IsDelete == false && db.IsConfirm == IsConfirm
                                         select new {
                                             DoctorBookingId = db.DoctorBookingId,
                                             PatientName = db.Customer.Name,
                                             DoctorName = db.Customer1.Name,
                                             AssistNurseName = db.Customer2.Name,
                                             PackageName = p.Name,
                                             BookingDate = db.FromDate,
                                             BookingDay = db.Day,
                                             FromTime = db.FromTime,
                                             ToTime = db.ToTime,
                                             BookingCase = db.BookingCase
                                       }).Distinct().ToList();
                dgvBookingList.DataSource = doctorBookingList;
            }
            else if (patientId != 0 && selectDate == DateTime.MinValue)//filter with patient id..
            {
                var doctorBookingList = (from db in entity.DoctorBookings
                                         join ds in entity.DoctorSchedules on db.DoctorScheduleId equals ds.DoctorScheduleId
                                         join ppi in entity.PackagePurchasedInvoices on db.PackageId equals ppi.ProductId
                                         join p in entity.Products on ppi.ProductId equals p.Id
                                         where db.IsDelete == false && db.IsConfirm == IsConfirm && db.PatientId == patientId
                                         select new {
                                             DoctorBookingId = db.DoctorBookingId,
                                             PatientName = db.Customer.Name,
                                             DoctorName = db.Customer1.Name,
                                             AssistNurseName = db.Customer2.Name,
                                             PackageName = p.Name,
                                             BookingDate = db.FromDate,
                                             BookingDay = db.Day,
                                             FromTime = db.FromTime,
                                             ToTime = db.ToTime,
                                             BookingCase = db.BookingCase
                                       }).Distinct().ToList();
                dgvBookingList.DataSource = doctorBookingList;
            }
            else if (patientId == 0 && selectDate != DateTime.MinValue)// filter selected date..
            {
                var doctorBookingList = (from db in entity.DoctorBookings
                                         join ds in entity.DoctorSchedules on db.DoctorScheduleId equals ds.DoctorScheduleId
                                         join ppi in entity.PackagePurchasedInvoices on db.PackageId equals ppi.ProductId
                                         join p in entity.Products on ppi.ProductId equals p.Id
                                         where db.IsDelete == false && db.IsConfirm == IsConfirm && db.FromDate == selectDate
                                         select new {
                                             DoctorBookingId = db.DoctorBookingId,
                                             PatientName = db.Customer.Name,
                                             DoctorName = db.Customer1.Name,
                                             AssistNurseName = db.Customer2.Name,
                                             PackageName = p.Name,
                                             BookingDate = db.FromDate,
                                             BookingDay = db.Day,
                                             FromTime = db.FromTime,
                                             ToTime = db.ToTime,
                                             BookingCase = db.BookingCase
                                       }).Distinct().ToList();
                dgvBookingList.DataSource = doctorBookingList;
            }
            if (patientId != 0 && selectDate != DateTime.MinValue)// filter patient and select date..
            {
                var doctorBookingList = (from db in entity.DoctorBookings
                                         join ds in entity.DoctorSchedules on db.DoctorScheduleId equals ds.DoctorScheduleId
                                         join ppi in entity.PackagePurchasedInvoices on db.PackageId equals ppi.ProductId
                                         join p in entity.Products on ppi.ProductId equals p.Id
                                         where db.IsDelete == false && db.IsConfirm== IsConfirm && db.PatientId == patientId && db.FromDate == selectDate
                                         select new {
                                             DoctorBookingId = db.DoctorBookingId,//0
                                             PatientName = db.Customer.Name,//1
                                             DoctorName = db.Customer1.Name,//2
                                             AssistNurseName = db.Customer2.Name,//3
                                             PackageName = p.Name,//4
                                             BookingDate = db.FromDate,//5
                                             BookingDay = db.Day,//6
                                             FromTime = db.FromTime,//7
                                             ToTime = db.ToTime,//7
                                             BookingCase = db.BookingCase//8
                                         }).Distinct().ToList();
                dgvBookingList.DataSource = doctorBookingList;
            }
        }
    }
}
