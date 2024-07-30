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
    public partial class MsgBox : Form
    {
        public string Message { get; set; }
        public MsgBox()
        {
            InitializeComponent();
        }

        private void MsgBox_Load(object sender, EventArgs e)
        {
            textBox1.Text = "Imported file include existed ProductName or ProductCode or BarCode from database!!" + Environment.NewLine + Environment.NewLine +
                        Message + Environment.NewLine + "in your file should be removed or modified.";
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Ret();
            this.Close();
            
        }

        private DialogResult Ret()
        {
            return DialogResult.OK;
        }
    }
}
