using POS.APP_Data;
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
    public partial class PriceChangeHistoryList : Form
    {
        #region Variable

        private POSEntities entity = new POSEntities();
        private bool IsCategoryId = false;
        private bool IsSubCategoryId = false;
        private bool IsBrandId = false;
        private bool Isname = false;
        private int CategoryId;
        private int subCategoryId;
        private int BrandId;
        private string name;
        private List<ProductPriceChange> productPriceList = new List<ProductPriceChange>();
        #endregion


        public PriceChangeHistoryList()
        {
            InitializeComponent();
        }

        private void PriceChangeHistoryList_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            dgvItemList.AutoGenerateColumns = false;
            List<APP_Data.Brand> BrandList = new List<APP_Data.Brand>();
            APP_Data.Brand brandObj1 = new APP_Data.Brand();
            brandObj1.Id = 0;
            brandObj1.Name = "Select";
            APP_Data.Brand brandObj2 = new APP_Data.Brand();
            brandObj2.Id = 1;
            brandObj2.Name = "None";
            BrandList.Add(brandObj1);
            BrandList.Add(brandObj2);
            BrandList.AddRange((from bList in entity.Brands select bList).ToList());
            cboBrand.DataSource = BrandList;
            cboBrand.DisplayMember = "Name";
            cboBrand.ValueMember = "Id";
            cboBrand.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboBrand.AutoCompleteSource = AutoCompleteSource.ListItems;

            List<APP_Data.ProductSubCategory> pSubCatList = new List<APP_Data.ProductSubCategory>();
            APP_Data.ProductSubCategory SubCategoryObj1 = new APP_Data.ProductSubCategory();
            SubCategoryObj1.Id = 0;
            SubCategoryObj1.Name = "Select";
            APP_Data.ProductSubCategory SubCategory2 = new APP_Data.ProductSubCategory();
            SubCategory2.Id = 1;
            SubCategory2.Name = "None";
            pSubCatList.Add(SubCategoryObj1);
            pSubCatList.Add(SubCategory2);
            //pSubCatList.AddRange((from c in entity.ProductSubCategories where c.ProductCategoryId == Convert.ToInt32(cboMainCategory.SelectedValue) select c).ToList());
            cboSubCategory.DataSource = pSubCatList;
            cboSubCategory.DisplayMember = "Name";
            cboSubCategory.ValueMember = "Id";
            cboSubCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboSubCategory.AutoCompleteSource = AutoCompleteSource.ListItems;

            List<APP_Data.ProductCategory> pMainCatList = new List<APP_Data.ProductCategory>();
            APP_Data.ProductCategory MainCategoryObj1 = new APP_Data.ProductCategory();
            MainCategoryObj1.Id = 0;
            MainCategoryObj1.Name = "Select";
            APP_Data.ProductCategory MainCategoryObj2 = new APP_Data.ProductCategory();
            MainCategoryObj2.Id = 1;
            MainCategoryObj2.Name = "None";
            pMainCatList.Add(MainCategoryObj1);
            pMainCatList.Add(MainCategoryObj2);
            pMainCatList.AddRange((from MainCategory in entity.ProductCategories select MainCategory).ToList());
            cboMainCategory.DataSource = pMainCatList;
            cboMainCategory.DisplayMember = "Name";
            cboMainCategory.ValueMember = "Id";
            cboMainCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboMainCategory.AutoCompleteSource = AutoCompleteSource.ListItems;

            DataBind();

        }

        private void dgvItemList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            int count = 1;
            foreach (DataGridViewRow row in dgvItemList.Rows)
            {
                ProductPriceChange currentt = (ProductPriceChange)row.DataBoundItem;
                row.Cells[0].Value = currentt.Id;
                row.Cells[1].Value = count.ToString();
                row.Cells[2].Value = currentt.Product.ProductCode;
                row.Cells[3].Value = currentt.Product.Name;
                row.Cells[4].Value = currentt.OldPrice;
                row.Cells[5].Value = currentt.Price;
                row.Cells[6].Value = currentt.User.Name;
                row.Cells[7].Value = currentt.UpdateDate;
                count++;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            IsCategoryId = false;
            CategoryId = 0;
            IsSubCategoryId = false;
            subCategoryId = 0;
            IsBrandId = false;
            BrandId = 0;
            Isname = false;
            name = string.Empty;
            productPriceList.Clear();
            if (cboMainCategory.SelectedIndex > 1)
            {
                IsCategoryId = true;
                CategoryId = Convert.ToInt32(cboMainCategory.SelectedValue);
            }
            if (cboSubCategory.SelectedIndex > 0)
            {
                IsSubCategoryId = true;
                subCategoryId = Convert.ToInt32(cboSubCategory.SelectedValue);
            }
            if (cboBrand.SelectedIndex > 0)
            {
                IsBrandId = true;
                BrandId = Convert.ToInt32(cboBrand.SelectedValue);
            }
            if (txtName.Text.Trim() != string.Empty)
            {
                Isname = true;
                name = txtName.Text;
            }
            // find product code id
            if (IsCategoryId == true && IsSubCategoryId == true && IsBrandId == true && Isname == true)
            {
                if (BrandId == 0)
                {
                    if (subCategoryId == 0)
                    {
                        productPriceList.AddRange((from p in entity.ProductPriceChanges where p.Product.ProductCategoryId == CategoryId && p.Product.ProductSubCategoryId == null && p.Product.BrandId == null && p.Product.Name.Contains(name) select p).ToList());
                    }
                    else
                    {
                        productPriceList.AddRange((from p in entity.ProductPriceChanges where p.Product.ProductCategoryId == CategoryId && p.Product.ProductSubCategoryId == subCategoryId && p.Product.BrandId == null && p.Product.Name.Contains(name) select p).ToList());
                    }
                }
                else
                {
                    if (subCategoryId == 0)
                    {
                        productPriceList.AddRange((from p in entity.ProductPriceChanges where p.Product.ProductCategoryId == CategoryId && p.Product.ProductSubCategoryId == null && p.Product.BrandId == BrandId && p.Product.Name.Contains(name) select p).ToList());
                    }
                    else
                    {
                        productPriceList.AddRange((from p in entity.ProductPriceChanges where p.Product.ProductCategoryId == CategoryId && p.Product.ProductSubCategoryId == subCategoryId && p.Product.BrandId == BrandId && p.Product.Name.Contains(name) select p).ToList());
                    }
                }

                foundDataBind();
            }
            else if (IsCategoryId == true && IsSubCategoryId == true && IsBrandId == true && Isname == false)
            {
                if (BrandId == 0)
                {
                    if (subCategoryId == 0)
                    {
                        productPriceList.AddRange((from p in entity.ProductPriceChanges where p.Product.ProductCategoryId == CategoryId && p.Product.ProductSubCategoryId == null && p.Product.BrandId == null select p).ToList());
                    }
                    else
                    {
                        productPriceList.AddRange((from p in entity.ProductPriceChanges where p.Product.ProductCategoryId == CategoryId && p.Product.ProductSubCategoryId == subCategoryId && p.Product.BrandId == null select p).ToList());
                    }
                }
                else
                {
                    if (subCategoryId == 0)
                    {
                        productPriceList.AddRange((from p in entity.ProductPriceChanges where p.Product.ProductCategoryId == CategoryId && p.Product.ProductSubCategoryId == null && p.Product.BrandId == BrandId select p).ToList());
                    }
                    else
                    {
                        productPriceList.AddRange((from p in entity.ProductPriceChanges where p.Product.ProductCategoryId == CategoryId && p.Product.ProductSubCategoryId == subCategoryId && p.Product.BrandId == BrandId select p).ToList());
                    }
                }

                foundDataBind();
            }
            else if (IsCategoryId == true && IsSubCategoryId == true && IsBrandId == false && Isname == false)
            {
                if (subCategoryId == 0)
                {
                    productPriceList.AddRange((from p in entity.ProductPriceChanges where p.Product.ProductCategoryId == CategoryId && p.Product.ProductSubCategoryId == null select p).ToList());
                }
                else
                {
                    productPriceList.AddRange((from p in entity.ProductPriceChanges where p.Product.ProductCategoryId == CategoryId && p.Product.ProductSubCategoryId == subCategoryId select p).ToList());
                }

                foundDataBind();
            }
            else if (IsCategoryId == true && IsSubCategoryId == true && IsBrandId == false && Isname == true)
            {
                if (subCategoryId == 0)
                {
                    productPriceList.AddRange((from p in entity.ProductPriceChanges where p.Product.ProductCategoryId == CategoryId && p.Product.ProductSubCategoryId == null && p.Product.Name.Contains(name) select p).ToList());
                }
                else
                {
                    productPriceList.AddRange((from p in entity.ProductPriceChanges where p.Product.ProductCategoryId == CategoryId && p.Product.ProductSubCategoryId == subCategoryId && p.Product.Name.Contains(name) select p).ToList());
                }

                foundDataBind();
            }
            else if (IsCategoryId == false && IsSubCategoryId == false && IsBrandId == true && Isname == true)
            {
                if (BrandId == 0)
                {
                    productPriceList.AddRange((from p in entity.ProductPriceChanges where p.Product.BrandId == null && p.Product.Name.Contains(name) select p).ToList());
                }
                else
                {
                    productPriceList.AddRange((from p in entity.ProductPriceChanges where p.Product.BrandId == BrandId && p.Product.Name.Contains(name) select p).ToList());
                }

                foundDataBind();
            }
            else if (IsCategoryId == false && IsSubCategoryId == false && IsBrandId == true && Isname == false)
            {
                if (BrandId == 0)
                {
                    productPriceList.AddRange((from p in entity.ProductPriceChanges where p.Product.BrandId == null select p).ToList());
                }
                else
                {
                    productPriceList.AddRange((from p in entity.ProductPriceChanges where p.Product.BrandId == BrandId select p).ToList());
                }

                foundDataBind();
            }
            else if (IsCategoryId == false && IsSubCategoryId == false && IsBrandId == false && Isname == true)
            {
                productPriceList.AddRange((from p in entity.ProductPriceChanges where p.Product.Name.Contains(name) select p).ToList());
                foundDataBind();
            }
            else if (IsCategoryId == true && IsSubCategoryId == false && IsBrandId == false && Isname == false)
            {
                productPriceList.AddRange((from p in entity.ProductPriceChanges where p.Product.ProductCategoryId == CategoryId select p).ToList());
                foundDataBind();
            }
            else if (IsCategoryId == true && IsSubCategoryId == false && IsBrandId == true && Isname == true)
            {
                if (BrandId == 0)
                {
                    productPriceList.AddRange((from p in entity.ProductPriceChanges where p.Product.ProductCategoryId == CategoryId && p.Product.BrandId == null && p.Product.Name.Contains(name) select p).ToList());
                }
                else
                {
                    productPriceList.AddRange((from p in entity.ProductPriceChanges where p.Product.ProductCategoryId == CategoryId && p.Product.BrandId == BrandId && p.Product.Name.Contains(name) select p).ToList());
                }

                foundDataBind();
            }
            else if (IsCategoryId == true && IsSubCategoryId == false && IsBrandId == true && Isname == false)
            {
                if (BrandId == 0)
                {
                    productPriceList.AddRange((from p in entity.ProductPriceChanges where p.Product.ProductCategoryId == CategoryId && p.Product.BrandId == null select p).ToList());
                }
                else
                {
                    productPriceList.AddRange((from p in entity.ProductPriceChanges where p.Product.ProductCategoryId == CategoryId && p.Product.BrandId == BrandId select p).ToList());
                }

                foundDataBind();
            }
            else if (IsCategoryId == true && IsSubCategoryId == false && IsBrandId == false && Isname == true)
            {
                productPriceList.AddRange((from p in entity.ProductPriceChanges where p.Product.ProductCategoryId == CategoryId && p.Product.Name.Contains(name) select p).ToList());
                foundDataBind();
            }
            else if (IsCategoryId == false && IsSubCategoryId == false && IsBrandId == false && Isname == false)
            {
                productPriceList.AddRange(entity.ProductPriceChanges.ToList());
                foundDataBind();
            }
            //else
            //{
            //    //to show message 
            //    MessageBox.Show("Can't find!", "Cannot find");
            //    dgvItemList.DataSource = "";
            //}
            else
            {
                foundDataBind();
            }
        }

        private void cboMainCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMainCategory.SelectedIndex != 0 && cboMainCategory.SelectedIndex != 1)
            {
                int productCategoryId = Int32.Parse(cboMainCategory.SelectedValue.ToString());
                List<APP_Data.ProductSubCategory> pSubCatList = new List<APP_Data.ProductSubCategory>();
                APP_Data.ProductSubCategory SubCategoryObj1 = new APP_Data.ProductSubCategory();
                SubCategoryObj1.Id = 0;
                SubCategoryObj1.Name = "Select";
                pSubCatList.Add(SubCategoryObj1);
                pSubCatList.AddRange((from c in entity.ProductSubCategories where c.ProductCategoryId == productCategoryId select c).ToList());
                cboSubCategory.DataSource = pSubCatList;
                cboSubCategory.DisplayMember = "Name";
                cboSubCategory.ValueMember = "Id";
                cboSubCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cboSubCategory.AutoCompleteSource = AutoCompleteSource.ListItems;
                cboSubCategory.Enabled = true;
            }
            else
            {
                cboSubCategory.SelectedIndex = 0;
                cboSubCategory.Enabled = false;
            }
        }
        #region Function

        public void DataBind()
        {
            productPriceList.Clear();
            productPriceList.AddRange((from t in entity.ProductPriceChanges orderby t.Id descending select t).Take(100).ToList());
            dgvItemList.DataSource = productPriceList;
        }

        private void foundDataBind()
        {
            dgvItemList.DataSource = "";

            if (productPriceList.Count < 1)
            {
                MessageBox.Show("Item not found!", "Cannot find");
                dgvItemList.DataSource = "";
                return;
            }
            else
            {
                dgvItemList.DataSource = productPriceList;
            }
        }

        #endregion                      

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            dgvItemList.DataSource = entity.ProductPriceChanges.Take(100).ToList();
            cboMainCategory.SelectedIndex = 0;
            cboBrand.SelectedIndex = 0;
            cboSubCategory.SelectedIndex = 0;
            txtName.Text = string.Empty;
         
        }
    }
}
