using POS.APP_Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS.mPOSUI.MedicalPerson
{
    public partial class FrmMedicalPersonDetailInfo : Form
    {
        POSEntities entity = new POSEntities();
        public int customerId;
        public FrmMedicalPersonDetailInfo()
        {
            InitializeComponent();
        }

        private void FrmMedicalPersonDetailInfo_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            Customer cust = (from c in entity.Customers where c.Id == customerId select c).FirstOrDefault<Customer>();
            lblName.Text = cust.Title + " " + cust.Name;
            lblPhoneNumber.Text = cust.PhoneNumber != "" ? cust.PhoneNumber : "-";

            lblNrc.Text = cust.NRC != "" ? cust.NRC : "-";

            lblAddress.Text = cust.Address != "" ? cust.Address : "-";

            lblEmail.Text = cust.Email != "" ? cust.Email : "-";

            lblGender.Text = cust.Gender != "" ? cust.Gender : "-";
            lblEmerPh.Text = cust.EmergencyContactPhone != "" ? cust.EmergencyContactPhone : "-";
            lblContactName.Text = cust.EmergencyContactName != "" ? cust.EmergencyContactName : "-";
            lblMartialStatus.Text = cust.Maritalstatus != "" ? cust.Maritalstatus : "-";
            lblRS.Text = cust.Relationship != "" ? cust.Relationship : "-";

            lblBirthday.Text = cust.Birthday != null ? Convert.ToDateTime(cust.Birthday).ToString("dd-MM-yyyy") : "-";
            lblCity.Text = cust.City != null ? cust.City.CityName : "-";
        }
    }
}
