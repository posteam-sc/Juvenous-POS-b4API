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
    public partial class Customer_Package_detail : Form
    {
        public Customer_Package_detail()
        {
            InitializeComponent();
        }

        private void Customer_Package_detail_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
        }
    }
}
