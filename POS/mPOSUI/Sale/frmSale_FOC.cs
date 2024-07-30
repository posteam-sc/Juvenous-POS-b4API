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
    public partial class frmSale_FOC : Form
    {
        public frmSale_FOC()
        {
            InitializeComponent();
        }


        #region Variable
        POSEntities entity = new POSEntities();
        #endregion

      

        #region Function
        private void Barcode_Input()
        {
            string _barcode = txtBarcode.Text;
            long productId = (from p in entity.Products where p.Barcode == _barcode && p.IsConsignment == false select p.Id).FirstOrDefault();
            cboProductName.SelectedValue = productId;
            cboProductName.Focus();
        }
     
        #endregion


        private void frmSale_FOC_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            Utility.BindProduct(cboProductName);
        }

        private void txtBarcode_Leave(object sender, EventArgs e)
        {
            Barcode_Input();
        }

        private void cboProductName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProductName.SelectedIndex > 0)
            {
                long productId = Convert.ToInt32(cboProductName.SelectedValue);
                string barcode = (from p in entity.Products where p.Id == productId select p.Barcode).FirstOrDefault();
                txtBarcode.Text = barcode;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
        
            int _proId = Convert.ToInt32(cboProductName.SelectedValue);

            Sales form = new Sales();
            form.FOCQty = Convert.ToInt32(txtQty.Text);
            form.Add_DataToGrid(_proId);
            this.Close();
        }




    }
}
