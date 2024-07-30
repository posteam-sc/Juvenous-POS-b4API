using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS
{
    public partial class GeneralPassword : Form
    {
        public GeneralPassword()
        {
            InitializeComponent();
        }
        public Form  Parent { get; set; }
        private void GeneralPassword_Load(object sender, EventArgs e)
        {
            this.TopMost = SettingController.TopMost;
            if (Parent.Name== "MDIParent")//Minize Check
            {
                lblMessage.Text = "Please approve you are allowed to minize mPOS.";
            }
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtApprovePass.Text))
            {
                MessageBox.Show("Please enter password.");
            }
            else
            {

                ((MDIParent)Parent).minimizePass = txtApprovePass.Text.Trim();
                this.Close();
            }
        }
    }
}
