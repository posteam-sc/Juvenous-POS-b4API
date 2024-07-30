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
    public partial class AddPackageChild : Form
    {
        public AddPackageChild()
        {
            InitializeComponent();
        }

        private void AddPackageChild_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
        }
    }
}
