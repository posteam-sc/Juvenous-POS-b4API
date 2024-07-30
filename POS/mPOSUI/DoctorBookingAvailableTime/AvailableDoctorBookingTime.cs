using POS.APP_Data;
using POS.mPOSUI.DoctorBooking;
using POS.mPOSUI.DoctorSchedule;
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

namespace POS.mPOSUI.DoctorBookingAvailableTime {
    public partial class AvailableDoctorBookingTime : Form {
        POSEntities entity = new POSEntities();
        public int doctorId { get; set; }
        public int patientId { get; set; }
        public AvailableDoctorBookingTime() {
            InitializeComponent();
            }

        private void AvailableDoctorBookingTime_Load(object sender, EventArgs e) {
            bindDoctor(doctorId);
            bindgvAvailableDoctorScheduleList(doctorId, dtpFromDate.Value, dtpToDate.Value);
            }



        private void bindgvAvailableDoctorScheduleList(int doctorId,DateTime fromDate,DateTime toDate) {
            if (doctorId == 0) {
               
                var data = (from doctorschedult in entity.DoctorSchedules
                            join doctor in entity.Customers on doctorschedult.DoctorId equals doctor.Id
                            where doctorschedult.IsDelete == false &&
                            !(from db in entity.DoctorBookings where db.IsDelete==false && db.IsBook==true
                               && db.FromDate >= fromDate && db.ToDate <= toDate
                              select db.DoctorScheduleId).Contains(doctorschedult.DoctorScheduleId)
                            select new {
                                doctorScheduleId = doctorschedult.DoctorScheduleId,
                                DoctorName = doctor.Title + "" + doctor.Name,
                                FromTime = doctorschedult.FromTime,
                                ToTime = doctorschedult.ToTime,
                                Monday = doctorschedult.Mon == true ? "Duty On" : "Duty Off",
                                Tuesday = doctorschedult.Tue == true ? "Duty On" : "Duty Off",
                                Wednesday = doctorschedult.Wed == true ? "Duty On" : "Duty Off",
                                Thursday = doctorschedult.Thu == true ? "Duty On" : "Duty Off",
                                Friday = doctorschedult.Fri == true ? "Duty On" : "Duty Off",
                                Saturday = doctorschedult.Sat == true ? "Duty On" : "Duty Off",
                                Sunday = doctorschedult.Sun == true ? "Duty On" : "Duty Off",
                                Book_Doctor = "Book Now"
                                }).ToList();
                gvdoctorscheduleList.DataSource = data;
                }
            else {
                var data = (from doctorschedult in entity.DoctorSchedules
                            join doctor in entity.Customers on doctorschedult.DoctorId equals doctor.Id
                            where doctorschedult.IsDelete == false   
                             &&  !(from db in entity.DoctorBookings
                              where db.IsDelete == false && db.IsBook == true && db.DoctorId == doctorId
                              && db.FromDate >= fromDate && db.ToDate <= toDate
                                   select db.DoctorScheduleId).Contains(doctorschedult.DoctorScheduleId)
                            select new {
                                doctorScheduleId = doctorschedult.DoctorScheduleId,
                                DoctorName = doctor.Title + "" + doctor.Name,
                                FromTime = doctorschedult.FromTime,
                                ToTime = doctorschedult.ToTime,
                                Monday = doctorschedult.Mon == true ? "Duty On" : "Duty Off",
                                Tuesday = doctorschedult.Tue == true ? "Duty On" : "Duty Off",
                                Wednesday = doctorschedult.Wed == true ? "Duty On" : "Duty Off",
                                Thursday = doctorschedult.Thu == true ? "Duty On" : "Duty Off",
                                Friday = doctorschedult.Fri == true ? "Duty On" : "Duty Off",
                                Saturday = doctorschedult.Sat == true ? "Duty On" : "Duty Off",
                                Sunday = doctorschedult.Sun == true ? "Duty On" : "Duty Off",
                                Book_Doctor = "Book Now"
                                }).ToList();
                gvdoctorscheduleList.DataSource = data;
                }
            gvdoctorscheduleList.Columns["doctorScheduleId"].Visible = false;
            ChangeCellColor();
            }

        private void bindDoctor(int doctorId) {
            if (doctorId == 0) {
                List<Customer> doctorList = new List<Customer>();
                Customer doctor = new Customer();
                doctor.Id = 0;
                doctor.Name = "All";
                doctorList.Add(doctor);
                doctorList.AddRange(entity.Customers.Where(x => x.CustomerTypeId != 1));//filter for patient id
                cboDoctor.DataSource = doctorList;
                cboDoctor.DisplayMember = "Name";
                cboDoctor.ValueMember = "Id";
                }
            else {
                List<Customer> doctorList = new List<Customer>();
                doctorList.AddRange(entity.Customers.Where(x => x.Id == doctorId));//filter for patient id
                cboDoctor.DataSource = doctorList;
                cboDoctor.DisplayMember = "Name";
                cboDoctor.ValueMember = "Id";
                }
            }
        private void ChangeCellColor() {
            int rowscount = gvdoctorscheduleList.Rows.Count;
            for (int i = 0; i < rowscount; i++) {
                if (Convert.ToString(gvdoctorscheduleList.Rows[i].Cells[4].Value) == "Duty On") {
                    gvdoctorscheduleList.Rows[i].Cells[4].Style.BackColor = Color.Green;

                    }
                else { gvdoctorscheduleList.Rows[i].Cells[4].Style.BackColor = Color.Yellow; }
                if (Convert.ToString(gvdoctorscheduleList.Rows[i].Cells[7].Value) == "Duty On") {
                    gvdoctorscheduleList.Rows[i].Cells[7].Style.BackColor = Color.Green;
                    }
                else {
                    gvdoctorscheduleList.Rows[i].Cells[7].Style.BackColor = Color.Yellow;
                    }
                if (Convert.ToString(gvdoctorscheduleList.Rows[i].Cells[8].Value) == "Duty On") {
                    gvdoctorscheduleList.Rows[i].Cells[8].Style.BackColor = Color.Green;
                    }
                else { gvdoctorscheduleList.Rows[i].Cells[8].Style.BackColor = Color.Yellow; }
                if (Convert.ToString(gvdoctorscheduleList.Rows[i].Cells[9].Value) == "Duty On") {
                    gvdoctorscheduleList.Rows[i].Cells[9].Style.BackColor = Color.Green;
                    }
                else { gvdoctorscheduleList.Rows[i].Cells[9].Style.BackColor = Color.Yellow; }
                if (Convert.ToString(gvdoctorscheduleList.Rows[i].Cells[5].Value) == "Duty On") {
                    gvdoctorscheduleList.Rows[i].Cells[5].Style.BackColor = Color.Green;
                    }
                else { gvdoctorscheduleList.Rows[i].Cells[5].Style.BackColor = Color.Yellow; }
                if (Convert.ToString(gvdoctorscheduleList.Rows[i].Cells[10].Value) == "Duty On") {
                    gvdoctorscheduleList.Rows[i].Cells[10].Style.BackColor = Color.Green;
                    }
                else gvdoctorscheduleList.Rows[i].Cells[10].Style.BackColor = Color.Yellow;
                if (Convert.ToString(gvdoctorscheduleList.Rows[i].Cells[6].Value) == "Duty On") {
                    gvdoctorscheduleList.Rows[i].Cells[6].Style.BackColor = Color.Green;
                    }
                else gvdoctorscheduleList.Rows[i].Cells[6].Style.BackColor = Color.Yellow;
                gvdoctorscheduleList.Rows[i].Cells[11].Style.Font = new Font(gvdoctorscheduleList.DefaultCellStyle.Font, FontStyle.Underline);
                }
            }

        private void gvdoctorscheduleList_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (e.ColumnIndex == 11) {//book to select 
                int index = e.RowIndex;// get the Row Index
                DataGridViewRow selectedRow = this.gvdoctorscheduleList.Rows[index];
                string doctorScheduleId = selectedRow.Cells[0].Value.ToString();
                POS.APP_Data.DoctorSchedule doctorScheduleEntity = entity.DoctorSchedules.Where(x => x.DoctorScheduleId == doctorScheduleId && x.IsDelete == false).SingleOrDefault();
                FrmDoctorBooking frmDoctorBooking = new FrmDoctorBooking();
                frmDoctorBooking.doctorScheduleEntity = doctorScheduleEntity;
                frmDoctorBooking.doctorId = doctorId;
                frmDoctorBooking.patientId = patientId;
                this.Close();
                frmDoctorBooking.Show();
                }//end of delete cell click
            }

        private void btnSearch_Click(object sender, EventArgs e) {
            bindgvAvailableDoctorScheduleList(Convert.ToInt32(cboDoctor.SelectedValue),dtpFromDate.Value,dtpToDate.Value);
            }

        }
    }
