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
    public partial class ProductDetailPrice : Form
    {
        #region Variable
        public int ProductId { get; set; }
        private POSEntities entity = new POSEntities();
        #endregion
        #region Event
        public ProductDetailPrice()
        {
            InitializeComponent();
        }

        private void ProductDetailPrice_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            dgvPriceList.AutoGenerateColumns = false;
            Product p = entity.Products.Where(x => x.Id == ProductId).FirstOrDefault();
            lblBarcode.Text = p.Barcode;
            lblName.Text = p.Name;
            lblSKU.Text = p.ProductCode;

            List<ProductPriceChange> PC = entity.ProductPriceChanges.Where(x => x.ProductId == ProductId).ToList();
            dgvPriceList.DataSource = PC;
        }

        private void dgvPriceList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvPriceList.Rows)
            {
                ProductPriceChange PC = (ProductPriceChange)row.DataBoundItem;
                row.Cells[0].Value = PC.UpdateDate.ToString();
                row.Cells[1].Value = PC.OldPrice.ToString();
                row.Cells[2].Value = PC.Price.ToString();
                row.Cells[3].Value = PC.User.Name;
            }
        }

        #endregion

        
    }
}
