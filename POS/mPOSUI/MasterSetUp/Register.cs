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
    public partial class Register : Form
    {
        private POSEntities entity = new POSEntities();

        public Register()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            String Key = txtLicenseKey.Text.Trim();
            Authorize currentKey = new Authorize();
            foreach (Authorize aut in entity.Authorizes)
            {
                if (Utility.DecryptString(aut.licenseKey, "ABCD") == Key) currentKey = aut;
            }

            if (currentKey.Id != 0)
            {
                if (currentKey.macAddress == null)
                {
                    try
                    {
                        currentKey.macAddress = Utility.EncryptString(Utility.GetSystemMACID(),"ABCD");
                        entity.SaveChanges();
                        MessageBox.Show("Registration complete", "Complete");
                        

                        Login newform = new Login();
                        newform.WindowState = FormWindowState.Maximized;
                        newform.MdiParent = ((MDIParent)this.ParentForm);
                        newform.Show();
                        this.Dispose();
                    }
                    catch (Exception exe)
                    {
                        MessageBox.Show(exe.Message, "Error");
                    }
                }
                else
                {
                    MessageBox.Show("The Key is already in use");
                }
            }
            else
            {
                MessageBox.Show("Wrong License Key", "Error");
            }
        }

        private void txtLicenseKey_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.AcceptButton = btnRegister;
        }

        private void Register_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
        }
    }
}
