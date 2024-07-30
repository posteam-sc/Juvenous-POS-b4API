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
using System.Diagnostics;

namespace POS
{
    public partial class ItemList : Form
    {
        #region Variable
        private string radio_status = "";
        private POSEntities entity = new POSEntities();
        private bool IsCategoryId = false;
        private bool IsSubCategoryId = false;
        private bool IsBrandId = false;
        private bool Isname = false;
        private bool IsBarcode = false;
        private int CategoryId;
        private int subCategoryId;
        private int BrandId;
        private string name;
        private string Barcode;
        private List<Product> productList = new List<Product>();
        List<Stock_Transaction> product_List = new List<Stock_Transaction>();
        int Qty = 0;
        #endregion

        #region Event

        public ItemList()
        {
            InitializeComponent();

        }

        private void ItemList_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            bool notbackoffice = Utility.IsNotBackOffice();
            if (notbackoffice)
            {
                btnAdd.Enabled = false;
            }
            else
            {
                btnAdd.Enabled = true;
            }

            dgvItemList.AutoGenerateColumns = false;
            //List<APP_Data.Brand> BrandList = new List<APP_Data.Brand>();
            //APP_Data.Brand brandObj1 = new APP_Data.Brand();
            //brandObj1.Id = 0;
            //brandObj1.Name = "Select";
       
            //BrandList.Add(brandObj1);
      
            //BrandList.AddRange((from bList in entity.Brands select bList).ToList());
            //cboBrand.DataSource = BrandList;
            //cboBrand.DisplayMember = "Name";
            //cboBrand.ValueMember = "Id";
            //cboBrand.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //cboBrand.AutoCompleteSource = AutoCompleteSource.ListItems;
            Utility.BindBrand(cboBrand);

            List<APP_Data.ProductSubCategory> pSubCatList = new List<APP_Data.ProductSubCategory>();
            APP_Data.ProductSubCategory SubCategoryObj1 = new APP_Data.ProductSubCategory();
            SubCategoryObj1.Id = 0;
            SubCategoryObj1.Name = "Select";
      
            pSubCatList.Add(SubCategoryObj1);
 
            cboSubCategory.DataSource = pSubCatList;
            cboSubCategory.DisplayMember = "Name";
            cboSubCategory.ValueMember = "Id";
            cboSubCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboSubCategory.AutoCompleteSource = AutoCompleteSource.ListItems;

            List<APP_Data.ProductCategory> pMainCatList = new List<APP_Data.ProductCategory>();
            APP_Data.ProductCategory MainCategoryObj1 = new APP_Data.ProductCategory();
            MainCategoryObj1.Id = 0;
            MainCategoryObj1.Name = "Select";
   
            pMainCatList.Add(MainCategoryObj1);
 
            pMainCatList.AddRange((from MainCategory in entity.ProductCategories select MainCategory).ToList());
            cboMainCategory.DataSource = pMainCatList;
            cboMainCategory.DisplayMember = "Name";
            cboMainCategory.ValueMember = "Id";
            cboMainCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboMainCategory.AutoCompleteSource = AutoCompleteSource.ListItems;

            DataBind();


        }

        //private void ItemList_Activated(object sender, EventArgs e)
        //{
        //    foundDataBind();
        //}

        private void btnAdd_Click(object sender, EventArgs e)
        {

            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.Product.Add || MemberShip.isAdmin)
            {
                NewProduct newForm = new NewProduct();
                newForm.isEdit = false;
                newForm.Show();
            }
            else
            {
                MessageBox.Show("You are not allowed to add new product", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dgvItemList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            int count = 1;
            foreach (DataGridViewRow row in dgvItemList.Rows)
            {
                Product productObj = (Product)row.DataBoundItem;

                row.Cells[0].Value = productObj.Id;
                row.Cells[1].Value = count.ToString();
                row.Cells[2].Value = productObj.ProductCode;
                row.Cells[3].Value = productObj.Barcode;
                row.Cells[4].Value = productObj.Name;
                row.Cells[5].Value = productObj.Qty;
                row.Cells[6].Value = productObj.PackageQty;
                row.Cells[7].Value = productObj.Price;
                row.Cells[8].Value = productObj.DiscountRate + "%";
                count++;
            }
        }

        ////private void Radio_Status()
        ////{
        ////    if (rdAll.Checked == true)
        ////    {
        ////        radio_status = "";
        ////    }
        ////    else if (rdConsignment.Checked == true)
        ////    {
        ////        radio_status = " && p.IsConsignment==true ";
        ////    }
        ////    else if (rdNonConsignment.Checked == true)
        ////    {
        ////        radio_status = " && p.IsConsignment==false ";
        ////    }
        ////}

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }


        private void LoadData()
        {
            IsCategoryId = false;
            CategoryId = 0;
            IsSubCategoryId = false;
            subCategoryId = 0;
            IsBrandId = false;
            BrandId = 0;
            Isname = false;
            name = string.Empty;
            IsBarcode = false;
            Barcode = string.Empty;
            productList.Clear();
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
            if (txtBarcode.Text.Trim() != string.Empty)
            {
                IsBarcode = true;
                Barcode = txtBarcode.Text.ToString();
            }

            productList = (from p in entity.Products
                           where ((rdAll.Checked == true && 1 == 1) || (rdNonConsignment.Checked == true && p.IsConsignment == false) || (rdConsignment.Checked == true && p.IsConsignment == true)) 
                           && ((BrandId > 0 && p.BrandId == BrandId) || (BrandId == 0 && 1==1)) 
                           && ((CategoryId > 0 && p.ProductCategoryId == CategoryId) || (CategoryId == 0 && 1==1))
                               && ((subCategoryId > 0 && p.ProductSubCategoryId == subCategoryId) || (subCategoryId == 0 && 1==1))
                               && ((Barcode != "" && p.Barcode.Trim().Contains(Barcode)) || (Barcode == "" && 1 == 1))
                               && ((name != "" && p.Name.Trim().Contains(name)) || (name == "" && 1 == 1)) 
                           select p).ToList();

            foundDataBind();
       
        }

        private void dgvItemList_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                //Role Management
                RoleManagementController controller = new RoleManagementController();
                controller.Load(MemberShip.UserRoleId);
                int currentProductId = Convert.ToInt32(dgvItemList.Rows[e.RowIndex].Cells[0].Value);

                if (e.ColumnIndex == 9)
                {
                    if (controller.Product.EditOrDelete || MemberShip.isAdmin)
                    {
                        NewProduct newform = new NewProduct();
                        newform.isEdit = true;
                        newform.Text = "Edit Product";
                        newform.ProductId = currentProductId;
                        newform.Show();
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to edit products", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                //Print
                else if (e.ColumnIndex == 10)
                {
                    //to print barcode
                    PrintBarcode newform = new PrintBarcode();
                    newform.productId = currentProductId;
                    newform.Show();
                }
                //Delete
                else if (e.ColumnIndex == 11)
                {
                    if (controller.Product.EditOrDelete || MemberShip.isAdmin == true)
                    {

                        DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (result.Equals(DialogResult.OK))
                        {
                            DataGridViewRow row = dgvItemList.Rows[e.RowIndex];
                            Product productObj = (Product)row.DataBoundItem;
                            productObj = (from p in entity.Products where p.Id == productObj.Id select p).FirstOrDefault();
                            List<WrapperItem> wList = entity.WrapperItems.Where(x => x.ChildProductId == productObj.Id).ToList();

                            var _tranDetailCount = (from td in productObj.TransactionDetails where td.IsDeleted == false select td).Count();
                            //if (productObj.TransactionDetails.Count > 0)
                            if (_tranDetailCount > 0)
                            {
                                MessageBox.Show("This product is used in transaction!", "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            if (productObj.PurchaseDetails.Count > 0)
                            {
                                MessageBox.Show("This product is used in purchasing!", "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            else if (wList.Count > 0)
                            {
                                MessageBox.Show("This product is used in a wrapper!", "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else
                            {
                                List<ProductPriceChange> pC = new List<ProductPriceChange>();
                                pC = entity.ProductPriceChanges.Where(x => x.ProductId == productObj.Id).ToList();
                                if (pC.Count > 0)
                                {
                                    foreach (ProductPriceChange p in pC)
                                    {
                                        entity.ProductPriceChanges.Remove(p);
                                    }
                                }

                                //remove product Qty Change  *SD*
                                List<ProductQuantityChange> pQ = new List<ProductQuantityChange>();
                                pQ = entity.ProductQuantityChanges.Where(x => x.ProductId == productObj.Id).ToList();
                                if (pQ.Count > 0)
                                {
                                    foreach (ProductQuantityChange p in pQ)
                                    {
                                        entity.ProductQuantityChanges.Remove(p);
                                    }
                                }

                                if (productObj.IsWrapper == true)
                                {
                                    List<WrapperItem> wL = entity.WrapperItems.Where(x => x.ParentProductId == productObj.Id).ToList();
                                    if (wL.Count > 0)
                                    {
                                        foreach (WrapperItem w in wL)
                                        {
                                            entity.WrapperItems.Remove(w);
                                        }
                                    }
                                }
                                entity.Products.Remove(productObj);

                                List<APP_Data.StockTransaction> st = entity.StockTransactions.Where(x => x.ProductId== productObj.Id).ToList();
                                if (st.Count > 0)
                                {
                                    foreach (APP_Data.StockTransaction s in st)
                                    {
                                        entity.StockTransactions.Remove(s);
                                    }
                                }

                              
                                entity.SaveChanges();
                                //if (productObj.IsConsignment == true)
                                //{
                                //    //save in stock transaction
                                //    Stock_Transaction st = new Stock_Transaction();
                                //    st.ProductId = productObj.Id;
                                //    Qty -= Convert.ToInt32(productObj.Qty);
                                //    st.Purchase = Qty;
                                //    product_List.Add(st);
                                //    Qty = 0;
                                //    Save_ConsingQty_ToStockTransaction(product_List);

                                //}
                                DataBind();
                                MessageBox.Show("Successfully Deleted!", "Delete Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to delete products", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                //Price Detail
                else if (e.ColumnIndex == 12)
                {
                    ProductDetailPrice newForm = new ProductDetailPrice();
                    newForm.ProductId = currentProductId;
                    newForm.ShowDialog();
                }
            }
        }

        private void cboMainCategory_SelectedValueChanged(object sender, EventArgs e)
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

        //private void ItemList_Activated_1(object sender, EventArgs e)
        //{
        //    foundDataBind();
        //}

        #endregion

        #region Function
        #region for saving Cosignment Qty in Stock Transaction table


        private void Save_ConsingQty_ToStockTransaction(List<Stock_Transaction> productList)
        {
            int _year, _month;

            _year = System.DateTime.Now.Year;
            _month = System.DateTime.Now.Month;
            Utility.Consignment_Run_Process(_year, _month, productList);
        }
        #endregion

        public void DataBind()
        {
            entity = new POSEntities();
            dgvItemList.DataSource = entity.Products.ToList();
        }

        private void foundDataBind()
        {
            dgvItemList.DataSource = "";

            if (productList.Count < 1)
            {
                MessageBox.Show("Item not found!", "Cannot find");
                dgvItemList.DataSource = "";
                return;
            }
            else
            {
                dgvItemList.DataSource = productList;
            }
        }

        #endregion

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cboMainCategory.SelectedIndex = 0;
            cboSubCategory.SelectedIndex = 0;
            cboBrand.SelectedIndex = 0;
            txtName.Clear();
            txtBarcode.Clear();
            DataBind();
        }

        private void cboMainCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMainCategory.SelectedIndex != 0)
            {
                int productCategoryId = Int32.Parse(cboMainCategory.SelectedValue.ToString());
                List<APP_Data.ProductSubCategory> pSubCatList = new List<APP_Data.ProductSubCategory>();
                APP_Data.ProductSubCategory SubCategoryObj1 = new APP_Data.ProductSubCategory();
                SubCategoryObj1.Id = -1;
                SubCategoryObj1.Name = "Select";
                //APP_Data.ProductSubCategory SubCategoryObj2 = new APP_Data.ProductSubCategory();
                //SubCategoryObj2.Id = 0;
                //SubCategoryObj2.Name = "None";
                pSubCatList.Add(SubCategoryObj1);
               // pSubCatList.Add(SubCategoryObj2);
                pSubCatList.AddRange((from c in entity.ProductSubCategories where c.ProductCategoryId == productCategoryId select c).ToList());
                cboSubCategory.DataSource = pSubCatList;
                cboSubCategory.DisplayMember = "Name";
                cboSubCategory.ValueMember = "Id";
                cboSubCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cboSubCategory.AutoCompleteSource = AutoCompleteSource.ListItems;
                cboSubCategory.Enabled = true;

            }
            //DataBind();
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            this.AcceptButton = btnSearch;
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            this.AcceptButton = btnSearch;
        }

        private void rdConsignment_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void rdNonConsignment_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void rdAll_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnPBC_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + @"\\POS.btw");
        }
    }
}
