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
    public partial class ProductDetailQty : Form
    {
        #region Variable
        public int ProductId { get; set; }
        private POSEntities entity = new POSEntities();
        #endregion

        #region Event
        public ProductDetailQty()
        {
            InitializeComponent();
        }

        private void ProductDetailQty_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            dgvQtyList.Columns["colUpdatedDate"].DefaultCellStyle.Format = "dd-MMM-yyyy hh:mm:ss tt";
            dgvQtyList.AutoGenerateColumns = false;
            Product p = entity.Products.Where(x => x.Id == ProductId).FirstOrDefault();
            if (p != null)
            {
                lblBarcode.Text = p.Barcode;
                lblName.Text = p.Name;
                lblSKU.Text = p.ProductCode;

                IQueryable<object> q = from c in entity.ProductQuantityChanges
                                       join u in entity.Users on c.UserID equals u.Id
                                       where c.ProductId == ProductId
                                       select new { UpdateDate = c.UpdateDate, StockInQty = (c.StockInQty >=0)  ?  c.StockInQty  :  0,  StockOutQty= ( c.StockInQty <0)  ?  c.StockInQty * (-1)  :  0,   User = u.Name };
                List<object> _qtyChange = new List<object>(q);

                dgvQtyList.AutoGenerateColumns = false;
                dgvQtyList.DataSource = _qtyChange;

                lblTotalStockInQty.Text = dgvQtyList.Rows.Cast<DataGridViewRow>()
                         .Sum(t => Convert.ToInt32(t.Cells["colStockInQty"].Value)).ToString();

                lblTotalStockOutQty.Text = dgvQtyList.Rows.Cast<DataGridViewRow>()
                          .Sum(t => Convert.ToInt32(t.Cells["colStockOutQty"].Value)).ToString();
            }
        
        }
        #endregion

        
    }
}
