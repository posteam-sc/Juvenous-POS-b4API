using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS
{
    public partial class VersionInfo : Form
    {
    
        public VersionInfo()
        {
            InitializeComponent();
        }

        private void VersionInfo_Load(object sender, EventArgs e)
        {
            var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            lblproductversion.Text = versionInfo.ProductVersion;
            lblcopyright.Text = versionInfo.LegalCopyright;
            lblcontact.Visible = lblcontact1.Visible = lblcontact2.Visible = true;

        }
    }
}
