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
using System.Data.Objects;

namespace POS
{
    public partial class NoveltySystem : Form
    {
        #region Variables

        POSEntities entity = new POSEntities();
        List<Product> pList = new List<Product>();
        List<ProductInNovelty> PNovList = new List<ProductInNovelty>();
        ToolTip tp = new ToolTip();
        public bool isEdit = false;
        public int noveltyId;
        Boolean IsAlreadySale = false;
        #endregion
        public NoveltySystem()
        {
            InitializeComponent();
        }

        private void NoveltySystem_Load(object sender, EventArgs e)
        {
            dgvProductList.AutoGenerateColumns = false;
            List<APP_Data.Brand> brandList = new List<APP_Data.Brand>();
            APP_Data.Brand brandObj = new APP_Data.Brand();
            brandObj.Name = "Select";
            brandObj.Id = 0;
            brandList.Add(brandObj);
            brandList.AddRange(entity.Brands.ToList());            
            cboLine.DataSource = brandList;
            cboLine.DisplayMember = "Name";
            cboLine.ValueMember = "Id";

            tlpProduct.Enabled = false;
            btnAdd.Enabled = false;
            if (isEdit)
            {
                cboLine.Enabled = false;
                entity = new POSEntities();
                APP_Data.NoveltySystem noveltySysObj = entity.NoveltySystems.FirstOrDefault(x => x.Id == noveltyId);
                List<ProductInNovelty> PList = entity.ProductInNovelties.Where(x => x.NoveltySystemId == noveltyId && x.IsDeleted==false).ToList();
            
                DateTime fromDate = noveltySysObj.ValidFrom.Value;
                DateTime toDate = noveltySysObj.ValidTo.Value;
                List<Transaction> tList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                foreach (ProductInNovelty p in PList)
                {
                    foreach (Transaction t in tList)
                    {
                        if (t.TransactionDetails.Where(x => x.ProductId == p.ProductId).Count() > 0)
                        {
                            IsAlreadySale = true;
                        }
                    }
                }
                if (IsAlreadySale)
                {
                    dtpFrom.Enabled = false;
                }

                dtpFrom.Value = noveltySysObj.ValidFrom.Value;
                dtpTo.Value = noveltySysObj.ValidTo.Value;
                cboLine.Text = noveltySysObj.Brand.Name;
                List<Product> productList = entity.Products.Where(x => x.BrandId == noveltySysObj.BrandId).ToList();
                cboProduct.DataSource = productList;
                cboProduct.DisplayMember = "Name";
                cboProduct.ValueMember = "Id";
                tlpProduct.Enabled = true;
                btnAdd.Enabled = true;
                List<ProductInNovelty> pNoveltyList = noveltySysObj.ProductInNovelties.Where(x=>x.IsDeleted==false).ToList();               
                foreach (ProductInNovelty p in pNoveltyList)
                {
                    Product pObj = new Product();
                    pObj = entity.Products.FirstOrDefault(x => x.Id == p.ProductId);
                    pList.Add(pObj);
                }
                dgvProductList.DataSource = pList;
                PNovList = noveltySysObj.ProductInNovelties.ToList();
                btnSave.Image = POS.Properties.Resources.update_big;
            }
        }

        private void cboLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvProductList.DataSource = "";
            pList.Clear();
            
            if (cboLine.SelectedIndex > 0)
            {
                int BrandId = Convert.ToInt32(cboLine.SelectedValue);
                List<Product> productList = entity.Products.Where(x => x.BrandId == BrandId).ToList();
                cboProduct.DataSource = productList;
                cboProduct.DisplayMember = "Name";
                cboProduct.ValueMember = "Id";
                tlpProduct.Enabled = true;
                btnAdd.Enabled = true;
            }
            else
            {
                tlpProduct.Enabled = false;
               
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            dgvProductList.DataSource = "";
            int productId = Convert.ToInt32(cboProduct.SelectedValue);
            pList.Add(entity.Products.Where(x => x.Id == productId).FirstOrDefault());
            ProductInNovelty pNov = new ProductInNovelty();
            pNov.NoveltySystemId = noveltyId;
            pNov.ProductId = productId;
            pNov.IsDeleted = false;
            PNovList.Add(pNov);
            dgvProductList.DataSource = pList;

        }

        int DeleteproductID = 0;
        private void dgvProductList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int Index = e.RowIndex;
                DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result.Equals(DialogResult.OK))
                {
                    dgvProductList.DataSource = "";
                    DeleteproductID = Convert.ToInt32(pList[Index].Id);
                    pList.RemoveAt(Index);
                    foreach(var p in PNovList.Where(x => x.ProductId == DeleteproductID))
                    {
                        p.IsDeleted = true;
                    }
                 
                    
                    dgvProductList.DataSource = pList;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool hasError = false;

            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            if (cboLine.SelectedIndex == 0)
            {
                tp.SetToolTip(cboLine, "Error");
                tp.Show("Please select line!", cboLine);
                hasError = true;
            }
            else if (pList.Count == 0)
            {
                tp.SetToolTip(cboProduct, "Error");
                tp.Show("Please select product!", cboProduct);
                hasError = true;
            }
            else if (dtpFrom.Value > dtpTo.Value)
            {
                tp.SetToolTip(dtpFrom, "Error");
                tp.Show("Please select correct time range!", dtpFrom);
                hasError = true;
            }
            if (!hasError)
            {
                if (!isEdit)
                {
                    APP_Data.NoveltySystem noveltyObj = new APP_Data.NoveltySystem();
                    noveltyObj.ValidFrom = dtpFrom.Value.Date;
                    noveltyObj.ValidTo = dtpTo.Value.Date;
                    noveltyObj.BrandId = Convert.ToInt32(cboLine.SelectedValue);
                    noveltyObj.UpdateDate = DateTime.Now;
                    foreach (ProductInNovelty p  in PNovList)
                    {
                        ProductInNovelty pNObj = new ProductInNovelty();
                        pNObj.ProductId = p.ProductId;
                        
                        pNObj.IsDeleted = false;
                        noveltyObj.ProductInNovelties.Add(pNObj);
                    }
                    entity.NoveltySystems.Add(noveltyObj);
                    entity.SaveChanges();

                    MessageBox.Show("Successfully Saved!", "Save");
                    Clear();
                    this.Dispose();
                }
                else 
                {
                    Boolean IsContinue = true;
                    if (IsAlreadySale)
                    {
                        if (dtpTo.Value.Date < System.DateTime.Now.Date)
                        {
                            MessageBox.Show("To Date must be greather than or equal Today Date!","mPOS");
                            IsContinue = false;
                        }
                    }

                    if (IsContinue)
                    {
                        DialogResult result = MessageBox.Show("Are you sure you want to update?", "Update", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (result.Equals(DialogResult.OK))
                        {
                            APP_Data.NoveltySystem editObj = entity.NoveltySystems.Where(x => x.Id == noveltyId).FirstOrDefault();
                            editObj.ValidFrom = dtpFrom.Value.Date;
                            editObj.ValidTo = dtpTo.Value.Date;
                            editObj.BrandId = Convert.ToInt32(cboLine.SelectedValue);
                            editObj.UpdateDate = DateTime.Now;
                            List<APP_Data.ProductInNovelty> oldnov=entity.ProductInNovelties.Where(x=>x.NoveltySystemId==noveltyId).ToList();
                            if(PNovList.Count()>oldnov.Count())
                            {                               
                                foreach (ProductInNovelty p in PNovList)
                                {
                                    var existedrow = oldnov.Where(x => x.NoveltySystemId == noveltyId && x.ProductId == p.ProductId).FirstOrDefault();
                                    if (existedrow == null) // add new productIn novelty
                                    {
                                        ProductInNovelty pNObj = new ProductInNovelty();
                                        pNObj.ProductId = p.ProductId;
                                        pNObj.IsDeleted = false;
                                        editObj.ProductInNovelties.Add(pNObj);
                                        entity.Entry(editObj).State = System.Data.EntityState.Modified;
                                        entity.SaveChanges();
                                    }
                                    else
                                    {
                                        ProductInNovelty poldnov = entity.ProductInNovelties.Where(x => x.ProductId == p.ProductId && x.NoveltySystemId==noveltyId).FirstOrDefault();
                                        poldnov.IsDeleted = p.IsDeleted;
                                        entity.SaveChanges();
                                    }
                                }
                            }
                            else
                            {
                                foreach(ProductInNovelty p in PNovList)
                                {
                                    ProductInNovelty poldnov = entity.ProductInNovelties.Where(x => x.ProductId == p.ProductId && x.NoveltySystemId==noveltyId).FirstOrDefault();
                                    poldnov.IsDeleted = p.IsDeleted;
                                    entity.SaveChanges();
                                }
                            }                      
                            MessageBox.Show("Successfully Updated!", "Update");
                            Clear();
                            this.Dispose();
                        }
                    }
                }
            
            }
        }

        #region Function

        private void To_NoveltyList()
        {
            if (System.Windows.Forms.Application.OpenForms["Novelty_List"] != null)
            {
                entity = new POSEntities();
                Novelty_List newForm = (Novelty_List)System.Windows.Forms.Application.OpenForms["Novelty_List"];
                newForm.Bind_Novelty();
            }
        }

        void Clear()
        {
           
            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;
            cboLine.SelectedIndex = 0;
            cboLine.Enabled = true;
            tlpProduct.Enabled = false;
            btnAdd.Enabled = false;
            cboProduct.DataSource = null;
            pList.Clear();
            PNovList.Clear();
            dgvProductList.DataSource = "";
            this.Dispose();
         
        }
        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void NoveltySystem_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(cboLine);
            tp.Hide(cboProduct);
            tp.Hide(dtpFrom);
            tp.Hide(dtpTo);
        }
    }
}
