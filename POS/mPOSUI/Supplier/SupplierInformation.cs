using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS.APP_Data;

namespace POS
{
    public partial class SupplierInformation : Form
    {
        #region Variable

        POSEntities entity = new POSEntities();
        public int supplierId;
        public int OldCreditAmount;

        #endregion
        #region Event

        public SupplierInformation()
        {
            InitializeComponent();
        }

        private void SupplierInformation_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            Supplier sp = (from s in entity.Suppliers where s.Id == supplierId select s).FirstOrDefault();
            lblName.Text = sp.Name;
            lblEnail.Text = (sp.Email == null) ? "-" : sp.Email;
            lblPhNo.Text = sp.PhoneNumber;
            lblAddress.Text = sp.Address;
            lblContactPerson.Text = sp.ContactPerson;
            lblCreditAmount.Text = OldCreditAmount.ToString();
        }
        #endregion
    }
}
