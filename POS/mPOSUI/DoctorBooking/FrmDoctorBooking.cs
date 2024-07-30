using POS.APP_Data;
using POS.mPOSUI.DoctorBookingAvailableTime;
using POS.mPOSUI.DoctorSchedule;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace POS.mPOSUI.DoctorBooking
{
    public partial class FrmDoctorBooking : Form
    {
        private POSEntities entity;
        public APP_Data.DoctorBooking DoctorBookingEntity { get; set; }
        public bool IsEdit { get; set; }
        public int doctorId { get; set; }
        public int patientId { get; set; }
        private ToolTip tp = new ToolTip();
        public POS.APP_Data.DoctorSchedule doctorScheduleEntity { get; set; }
        public FrmDoctorBooking()
        {
            InitializeComponent();
        }

        private void FrmDoctorBooking_Load(object sender, EventArgs e)
        {
            if (IsEdit&& DoctorBookingEntity!=null) {
                bindPatient(DoctorBookingEntity.PatientId);
                bindDoctor(DoctorBookingEntity.DoctorId);
                bindAssistantNurse();
                bindPackage(DoctorBookingEntity.PatientId);
                bindControls();
            }
            else {
                bindPatient(patientId);
                bindDoctor(doctorId);
                bindAssistantNurse();
                bindPackage(patientId);
                bindControls();
            }
        }

        private void bindControls()
        {
            if(doctorScheduleEntity != null)
            {
                if(doctorScheduleEntity.Mon == true) txtDay.Text = "Monday";
                if(doctorScheduleEntity.Tue == true) txtDay.Text = "Tuesday";
                if(doctorScheduleEntity.Wed == true) txtDay.Text = "Wednesday";
                if(doctorScheduleEntity.Thu == true) txtDay.Text = "Thursday";
                if(doctorScheduleEntity.Fri == true) txtDay.Text = "Friday";
                if(doctorScheduleEntity.Sat == true) txtDay.Text = "Saturday";
                if(doctorScheduleEntity.Sun == true) txtDay.Text = "Sunday";
                lblFromTime.Text = doctorScheduleEntity.FromTime;
                lblToTime.Text = doctorScheduleEntity.ToTime;
            }
            if (IsEdit) {
                txtDay.Text = DoctorBookingEntity.Day;
                lblFromTime.Text = DoctorBookingEntity.FromTime;
                lblToTime.Text = DoctorBookingEntity.ToTime;
                dtFromdate.Text = DoctorBookingEntity.FromDate.Value.ToShortDateString();
                txtBookingCase.Text = DoctorBookingEntity.BookingCase;
            }

        }

        public void bindPatient(int patientId)
        {
            if(patientId==0)
            {
                entity = new POSEntities();
                List<APP_Data.Customer> patientList = new List<APP_Data.Customer>();
                APP_Data.Customer patient = new APP_Data.Customer();
                patient.Id = 0;
                patient.Name = "Select";
                patientList.Add(patient);
                patientList.AddRange(entity.Customers.Where(x => x.CustomerTypeId == 1).ToList());
                cboPatientName.DataSource = patientList;
                cboPatientName.DisplayMember = "Name";
                cboPatientName.ValueMember = "Id";
            }else
            {
                entity = new POSEntities();
                List<APP_Data.Customer> patientList = new List<APP_Data.Customer>();
                patientList.AddRange(entity.Customers.Where(x => x.Id==patientId).ToList());
                cboPatientName.DataSource = patientList;
                cboPatientName.DisplayMember = "Name";
                cboPatientName.ValueMember = "Id";
            }
        }
        public void bindDoctor(int doctorId)
        {
            if(doctorId==0)
            {
                entity = new POSEntities();
                List<APP_Data.Customer> doctorList = new List<APP_Data.Customer>();
                APP_Data.Customer doctor = new APP_Data.Customer();
                doctor.Id = 0;
                doctor.Name = "Select";
                doctorList.Add(doctor);
                doctorList.AddRange(entity.Customers.Where(x => x.CustomerTypeId == 2 || x.CustomerTypeId == 3).ToList());
                cboDoctorName.DataSource = doctorList;
                cboDoctorName.DisplayMember = "Name";
                cboDoctorName.ValueMember = "Id";
            }else
            {
                entity = new POSEntities();
                List<APP_Data.Customer> doctorList = new List<APP_Data.Customer>();
                doctorList.AddRange(entity.Customers.Where(x => x.Id == doctorId).ToList());
                cboDoctorName.DataSource = doctorList;
                cboDoctorName.DisplayMember = "Name";
                cboDoctorName.ValueMember = "Id";
            }
        }
        public void bindAssistantNurse()
        {
            if (IsEdit) {
                entity = new POSEntities();
                List<APP_Data.Customer> nurseList = new List<APP_Data.Customer>();
                nurseList.AddRange(entity.Customers.Where(x => x.Id==DoctorBookingEntity.AssistNurseId).ToList());
                cboNurse.DataSource = nurseList;
                cboNurse.DisplayMember = "Name";
                cboNurse.ValueMember = "Id"; 
            }else {
                entity = new POSEntities();
                List<APP_Data.Customer> nurseList = new List<APP_Data.Customer>();
                APP_Data.Customer nurse = new Customer();
                nurse.Id = 0;
                nurse.Name = "Select";
                nurseList.Add(nurse);
                nurseList.AddRange(entity.Customers.Where(x => x.CustomerTypeId == 4).ToList());
                cboNurse.DataSource = nurseList;
                cboNurse.DisplayMember = "Name";
                cboNurse.ValueMember = "Id";
            }
        }
        public void bindPackage(int patientId)
        {
            if(patientId==0)
            {
                entity = new POSEntities();
                List<APP_Data.Product> productList = new List<APP_Data.Product>();
                APP_Data.Product product = new APP_Data.Product();
                product.Id = 0;
                product.Name = "Select";
                productList.Add(product);
                productList.AddRange(entity.Products.Where(x => x.IsPackage == true));
                cboPackage.DataSource = productList;
                cboPackage.DisplayMember = "Name";
                cboPackage.ValueMember = "Id"; 
            }else
            {
                entity = new POSEntities();
                List<APP_Data.Product> productList = new List<APP_Data.Product>();
                productList.AddRange(from c in entity.PackagePurchasedInvoices
                                     join p in entity.Products on c.ProductId equals p.Id
                                     where c.CustomerId == patientId && c.IsDelete == false && p.IsPackage == true
                                     select p);
                cboPackage.DataSource = productList;
                cboPackage.DisplayMember = "Name";
                cboPackage.ValueMember = "Id";
            }
            
        }
    

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            bool hasError = false;
            
            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            //Validation
            if (cboPatientName.SelectedIndex  <0)
            {
                tp.SetToolTip(cboPatientName, "Error");
                tp.Show("Please Choose Patient Name!", cboPatientName);
                hasError = true;
            }else if (cboDoctorName.SelectedIndex<0){
                tp.SetToolTip(cboDoctorName, "Error");
                tp.Show("Please Choose Doctor Name!", cboDoctorName);
                hasError = true;
            }
            if(!hasError)
            {
                int PatientId=Convert.ToInt32(cboPatientName.SelectedValue);

                if(doctorScheduleEntity == null)//there is no doctor schedule is selected.
                {
                    MessageBox.Show("Please select doctor's schedule to book ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                bool IsExistData = entity.DoctorBookings.Any(x => x.DoctorId == doctorScheduleEntity.DoctorId 
                                                                            &&( x.IsBook==true || x.IsConfirm==true) 
                                                                            && x.DoctorScheduleId== doctorScheduleEntity.DoctorScheduleId 
                                                                            && x.IsDelete==false);
               
                if(IsExistData)
                {
                    MessageBox.Show("Doctor  is already booked or confirmed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DateTime selectedDayDraft=Convert.ToDateTime(dtFromdate.Text);
                string doctorScheduleDay= txtDay.Text;
                string userSelectedDay = selectedDayDraft.DayOfWeek.ToString();
                if(!userSelectedDay.Equals(doctorScheduleDay))
                {
                    MessageBox.Show("your selceted day and doctor's schedule day is not same.please try it.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                APP_Data.DoctorBooking doctorBooking = new APP_Data.DoctorBooking();
                    doctorBooking.DoctorBookingId = System.Guid.NewGuid().ToString();
                    doctorBooking.PatientId = PatientId;
                    doctorBooking.DoctorId = doctorScheduleEntity.DoctorId;
                    doctorBooking.AssistNurseId = Convert.ToInt32(cboNurse.SelectedValue);
                    doctorBooking.DoctorScheduleId = doctorScheduleEntity.DoctorScheduleId;
                    doctorBooking.FromDate = doctorBooking.ToDate = Convert.ToDateTime(dtFromdate.Text);
                    doctorBooking.FromTime = doctorScheduleEntity.FromTime;
                    doctorBooking.ToTime = doctorScheduleEntity.ToTime;
                    doctorBooking.Day = txtDay.Text;
                    doctorBooking.PackageId= Convert.ToInt32(cboPackage.SelectedValue);
                  doctorBooking.BookingCase = txtBookingCase.Text;
                    doctorBooking.IsBook = true;
                    doctorBooking.CreatedDate = DateTime.Now;
                    doctorBooking.CreatedUserId = MemberShip.UserId;
                    entity.DoctorBookings.Add(doctorBooking);
                    int i = entity.SaveChanges();
                if(i > 0) { MessageBox.Show("Successfully Save", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information); ClearControls(); }         
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        private void ClearControls()
        {
            bindPatient(patientId=0);
            bindDoctor(doctorId=0);
            bindAssistantNurse();
            bindPackage(0);
            bindControls();
            txtBookingCase.Text = txtDay.Text = lblFromTime.Text = lblToTime.Text = string.Empty;
        }

        private void btnViewDoctorSchedule_Click(object sender, EventArgs e)
        {
            AvailableDoctorBookingTime availableDoctorBookingTimeForm = new AvailableDoctorBookingTime();
                availableDoctorBookingTimeForm.doctorId = Convert.ToInt32(cboDoctorName.SelectedValue);
               availableDoctorBookingTimeForm.patientId = Convert.ToInt32(cboPatientName.SelectedValue);          
              if(availableDoctorBookingTimeForm.patientId != 0) this.Close();
                availableDoctorBookingTimeForm.Show();
            
        }

        private void cboPatientName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cboPackage.SelectedIndex!=-1)
            {
                int patientId = Convert.ToInt32(cboPatientName.SelectedValue);
                entity = new POSEntities();
                List<APP_Data.Product> productList = new List<APP_Data.Product>();
                APP_Data.Product product = new APP_Data.Product();
                product.Id = 0;
                product.Name = "Select";
                productList.Add(product);
                productList.AddRange(from c in entity.PackagePurchasedInvoices
                                     join p in entity.Products on c.ProductId equals p.Id
                                     where c.CustomerId == patientId && c.IsDelete == false && p.IsPackage == true
                                     select p);                                   
                cboPackage.DataSource = productList;
                cboPackage.DisplayMember = "Name";
                cboPackage.ValueMember = "Id"; 
            }

        }

        private void btnNewPatient_Click(object sender, EventArgs e) {
            new NewCustomer().Show();
            }
        }
}
