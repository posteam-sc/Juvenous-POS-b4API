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
    public partial class Novelty_Detail : Form
    {
        #region Variables
        POSEntities entity = new POSEntities();
        public int noveltyId;
        #endregion
        #region Event
        public Novelty_Detail()
        {
            InitializeComponent();
        }

        private void Novelty_Detail_Load(object sender, EventArgs e)
        {
            dgvProductList.AutoGenerateColumns = false;
            APP_Data.NoveltySystem noveltySysObj = entity.NoveltySystems.FirstOrDefault(x => x.Id == noveltyId);
            lblLine.Text = noveltySysObj.Brand.Name;
            lblPeriod.Text = noveltySysObj.ValidFrom.Value.Date + " to " + noveltySysObj.ValidTo.Value.Date;
            List<ProductInNovelty> pNoveltyList = noveltySysObj.ProductInNovelties.Where(x=>x.IsDeleted==false).ToList();
            List<Product> pList = new List<Product>();
            foreach (ProductInNovelty p in pNoveltyList)
            {
                Product pObj = new Product();
                pObj = entity.Products.FirstOrDefault(x => x.Id == p.ProductId);
                pList.Add(pObj);
            }
            dgvProductList.DataSource = pList;
        }
        #endregion
    }
}
