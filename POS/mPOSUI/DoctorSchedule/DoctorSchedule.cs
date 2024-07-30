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

namespace POS.mPOSUI.DoctorSchedule
{
    public partial class DoctorSchedulefrm : Form
    {
        public string DoctorScheduleId { get; set; }
        public bool mon, tue, wed, thu, fri, sat, sun=false;
        public int doctorId { get; set; }
        public string fromtime { get; set; }
        public string totime { get; set; }
        public bool isEdit { get; set; }
        POSEntities entity = new POSEntities();
        public DoctorSchedulefrm()
        {
            InitializeComponent();
        }

        private void DoctorSchedule_Load(object sender, EventArgs e)
        {
            BindCboDoctor();
            bindControl();
        }

        private void bindControl()
        {
            if (isEdit)
            {
                btnSave.Text = "Update";
                string[] FromTimedataLine = fromtime.Split(':');
                string[] ToTimedataLine = totime.Split(':');
                cboDoctor.SelectedValue = doctorId;
                chkmon.Checked = mon ;
                chktue.Checked=tue;
                chkwed.Checked = wed;
                chkthu.Checked = thu;
                chkfri.Checked = fri;
                chksat.Checked = sat;
                chksun.Checked = sun;
                txtfromtimehh.Text = FromTimedataLine[0];
                txtfromtimemm.Text = FromTimedataLine[1];
                txttotimehh.Text = ToTimedataLine[0];
                txttotimemm.Text = ToTimedataLine[1];
            }
        }

        private void BindCboDoctor()
        {
            List<Customer> doctorList = new List<Customer>();
            Customer doctor = new Customer();
            doctor.Id = 0;
            doctor.Name = "All";
            doctorList.Add(doctor);
            doctorList.AddRange(entity.Customers.Where(x=>x.CustomerTypeId!=1));//filter for patient id
            cboDoctor.DataSource = doctorList;
            cboDoctor.DisplayMember = "Name";
            cboDoctor.ValueMember = "Id";
        }

        private void txtfromtimehh_KeyPress(object sender, KeyPressEventArgs e)
        {
            checkOnlyNumKeys(sender, e);
        }

        private void txtfromtimemm_KeyPress(object sender, KeyPressEventArgs e)
        {
            checkOnlyNumKeys(sender, e);
        }

        private void txttotimehh_KeyPress(object sender, KeyPressEventArgs e)
        {
            checkOnlyNumKeys(sender, e);
        }
        private void clearControls()
        {
            txtfromtimehh.Text = txtfromtimemm.Text = txttotimehh.Text = txttotimemm.Text = string.Empty;
            cboDoctor.SelectedIndex = 0;
            chkmon.Checked = chktue.Checked = chkwed.Checked = chkthu.Checked = chkfri.Checked = chksat.Checked = chksun.Checked = false;
        }
        private void txttotimemm_KeyPress(object sender, KeyPressEventArgs e)
        {
            checkOnlyNumKeys(sender, e);
        }

        private int SaveRecord(bool mon,bool tue,bool wed,bool thu,bool fri,bool sat,bool sun){
            
                POS.APP_Data.DoctorSchedule doctorSchedule = new POS.APP_Data.DoctorSchedule();
                doctorSchedule.DoctorScheduleId = System.Guid.NewGuid().ToString();
                doctorSchedule.DoctorId = Convert.ToInt32(cboDoctor.SelectedValue);
                doctorSchedule.Mon = mon;
                doctorSchedule.Tue = tue;
                doctorSchedule.Wed = wed;
                doctorSchedule.Thu = thu;
                doctorSchedule.Fri = fri;
                doctorSchedule.Sat = sat;
                doctorSchedule.Sun = sun;
                doctorSchedule.FromTime = txtfromtimehh.Text + ":" + txtfromtimemm.Text;
                doctorSchedule.ToTime = txttotimehh.Text + ":" + txttotimemm.Text;
                doctorSchedule.IsDelete = false;
                doctorSchedule.UserId = MemberShip.UserId;
                entity.DoctorSchedules.Add(doctorSchedule);
                return entity.SaveChanges();
              
            
        }
        private void UpdateRecord(bool mon, bool tue, bool wed, bool thu, bool fri, bool sat, bool sun)
        {
            POS.APP_Data.DoctorSchedule doctorSchedule = entity.DoctorSchedules.Where(x => x.DoctorScheduleId == DoctorScheduleId).SingleOrDefault();          
            if (doctorSchedule != null)
            {
                doctorSchedule.DoctorId = Convert.ToInt32(cboDoctor.SelectedValue);
                doctorSchedule.Mon = mon;
                doctorSchedule.Tue = tue;
                doctorSchedule.Wed = wed;
                doctorSchedule.Thu = thu;
                doctorSchedule.Fri = fri;
                doctorSchedule.Sat = sat;
                doctorSchedule.Sun = sun;
                doctorSchedule.FromTime = txtfromtimehh.Text + ":" + txtfromtimemm.Text;
                doctorSchedule.ToTime = txttotimehh.Text + ":" + txttotimemm.Text;
                doctorSchedule.UserId = MemberShip.UserId;             
                entity.DoctorSchedules.AddOrUpdate(doctorSchedule);
                entity.SaveChanges();

            }
            


        }
        private void checkOnlyNumKeys(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (cboDoctor.SelectedIndex <= 0)
            {
                MessageBox.Show("select one doctor", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtfromtimehh.Text) || string.IsNullOrEmpty(txtfromtimemm.Text) ||
                string.IsNullOrEmpty(txttotimehh.Text) || string.IsNullOrEmpty(txttotimemm.Text))
            {
                MessageBox.Show("please fill from time and to time ", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if ((txtfromtimehh.Text.Trim().Length > 2) || (txtfromtimemm.Text.Trim().Length > 2) ||
                (txttotimehh.Text.Trim().Length > 2) || (txttotimemm.Text.Trim().Length > 2))
            {
                MessageBox.Show("please insert 2 digit value(s)", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Convert.ToInt32(txtfromtimehh.Text) > 24 || Convert.ToInt32(txtfromtimemm.Text) > 59
                || Convert.ToInt32(txttotimehh.Text) > 24 || Convert.ToInt32(txttotimemm.Text) > 59)
            {
                MessageBox.Show("invalid time(use 24 hours format!!)", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            bool isMon = chkmon.Checked;
            bool isTue = chktue.Checked;
            bool isWed = chkwed.Checked;
            bool isThu = chkthu.Checked;
            bool isFri = chkfri.Checked;
            bool isSat = chksat.Checked;
            bool isSun = chksun.Checked;
            DialogResult result = MessageBox.Show("Are you sure you to " + btnSave.Text + " it?", "Saving doctor schedule", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result.Equals(DialogResult.OK))
            {
                if (isMon)
                {
                    if (isEdit) UpdateRecord(isMon, false, false, false, false, false, false);
                    else SaveRecord(isMon, false, false, false, false, false, false);
                }
                if (isTue)
                {
                    if (isEdit) UpdateRecord(false, isTue, false, false, false, false, false);
                    else
                        SaveRecord(false, isTue, false, false, false, false, false);
                }
                if (isWed)
                {
                    if (isEdit) UpdateRecord(false, false, isWed, false, false, false, false);
                    else
                        SaveRecord(false, false, isWed, false, false, false, false);
                }
                if (isThu)
                {
                    if (isEdit) UpdateRecord(false, false, false, isThu, false, false, false);
                    else
                        SaveRecord(false, false, false, isThu, false, false, false);
                }
                if (isFri)
                {
                    if (isEdit) UpdateRecord(false, false, false, false, isFri, false, false);
                    else
                        SaveRecord(false, false, false, false, isFri, false, false);
                }
                if (isSat)
                {
                    if (isEdit) UpdateRecord(false, false, false, false, false, isSat, false);
                    else
                        SaveRecord(false, false, false, false, false, isSat, false);
                }
                if (isSun)
                {
                    if (isEdit) UpdateRecord(false, false, false, false, false, false, isSun);
                    else
                        SaveRecord(false, false, false, false, false, false, isSun);
                }
                MessageBox.Show(btnSave.Text + "Success ! ", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clearControls();

            }
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            clearControls();
        }
    }
}
