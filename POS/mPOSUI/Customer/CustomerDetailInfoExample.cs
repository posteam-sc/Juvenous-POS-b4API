using System;
using System.Windows.Forms;

namespace POS
{
    public partial class CustomerDetailInfoExample : Form
    {
        public CustomerDetailInfoExample()
        {
            InitializeComponent();
        }

        private void CustomerDetailInfoExample_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
        }
    }
}
