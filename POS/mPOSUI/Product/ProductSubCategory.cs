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
using System.Text.RegularExpressions;

namespace POS
{
    public partial class ProductSubCategory : Form
    {
        #region Variable

        private POSEntities posEntity = new POSEntities();
        private ToolTip tp = new ToolTip();
        private Boolean isEdit = false;
        private int SubCategoryId = 0;
        private int CategoryId = 0;

        string oldName = "";

        #endregion

        #region Event

        public ProductSubCategory()
        {
            InitializeComponent();
        }

        private void ProductType_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            bool notbackoffice = Utility.IsNotBackOffice();
            if (notbackoffice)
            {
                Utility.Gpvisible(groupBox1, isEdit);
            }

            List<APP_Data.ProductCategory> pMCategoriesList = new List<APP_Data.ProductCategory>();
            APP_Data.ProductCategory pMObj = new APP_Data.ProductCategory();
            pMObj.Id = 0;
            pMObj.Name = "None";
            pMCategoriesList.Add(pMObj);
            pMCategoriesList.AddRange((from mCat in posEntity.ProductCategories select mCat).ToList());
            cboMCategory.DataSource = pMCategoriesList;
            cboMCategory.DisplayMember = "Name";
            cboMCategory.ValueMember = "Id";
            cboMCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboMCategory.AutoCompleteSource = AutoCompleteSource.ListItems;
            dgvProductList.AutoGenerateColumns = false;
            dgvProductList.DataSource = (from pType in posEntity.ProductSubCategories orderby pType.Id descending select pType).ToList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Boolean hasError = false;
            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            int MainCategoryId = Convert.ToInt32(cboMCategory.SelectedValue);
            if (MainCategoryId > 0)
            {
                if (txtName.Text.Trim() == string.Empty)
                {
                    tp.SetToolTip(txtName, "Error");
                    tp.Show("Please fill up Sub Category Name!", txtName);
                    hasError = true;
                }
                //else if (SettingController.UseStockAutoGenerate == true)
                //{
                //    if (txtPrefix.Text.Trim() == string.Empty)
                //    {
                //        tp.SetToolTip(txtName, "Error");
                //        tp.Show("Please fill up Sub Category Code !", txtPrefix);
                //        hasError = true;
                //    }
                //}

                
                
                if (hasError==false)
                {
                            APP_Data.ProductSubCategory pTypeObj = new APP_Data.ProductSubCategory();         
                            //New
                            if (!isEdit)
                            {
                                //Role Management
                                RoleManagementController controller = new RoleManagementController();
                                controller.Load(MemberShip.UserRoleId);
                                if (controller.SubCategory.Add || MemberShip.isAdmin)
                                {
                                    APP_Data.ProductSubCategory pSubCat = (from psCat in posEntity.ProductSubCategories where psCat.Name == txtName.Text && psCat.ProductCategoryId == MainCategoryId  select psCat).FirstOrDefault();
                                    //APP_Data.ProductSubCategory pSubCat2 = (from p in posEntity.ProductSubCategories where p.Prefix == txtPrefix.Text select p).FirstOrDefault();
                                    if (pSubCat == null)
                                    {
                                        //if (pSubCat2 == null)
                                        //{
                                           


                                        //}
                                        dgvProductList.DataSource = "";
                                        pTypeObj.Name = txtName.Text;
                                        //  pTypeObj.Prefix = txtPrefix.Text;
                                        pTypeObj.ProductCategoryId = Int32.Parse(cboMCategory.SelectedValue.ToString());
                                        pTypeObj.Prefix = txtPrefix.Text.ToString();
                                        pTypeObj.IsDelete = false;
                                        posEntity.ProductSubCategories.Add(pTypeObj);
                                        posEntity.SaveChanges();
                                        dgvProductList.DataSource = (from pType in posEntity.ProductSubCategories orderby pType.Id descending select pType).ToList();
                                        MessageBox.Show("Successfully Saved!", "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        cboMCategory.SelectedIndex = 0;
                                        txtName.Clear();
                                        txtPrefix.Clear();

                                        #region active new Product
                                        if (System.Windows.Forms.Application.OpenForms["NewProduct"] != null)
                                        {
                                            NewProduct newForm = (NewProduct)System.Windows.Forms.Application.OpenForms["NewProduct"];
                                            newForm.ReloadCategory();
                                            newForm.SetCurrentCategory(Convert.ToInt32(pTypeObj.ProductCategoryId));
                                            newForm.SetCurrentSubCategory(pTypeObj.Id);
                                        }
                                        #endregion 
                                        //else
                                        //{
                                        //    tp.SetToolTip(txtName, "Error");
                                        //    tp.Show("This Sub Category Code is already exist!", txtPrefix);
                                        //}
                                    }
                                    else
                                    {
                                        tp.SetToolTip(txtName, "Error");
                                        tp.Show("This sub category name  is already exist!", txtName);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("You are not allowed to add new sub category", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    return;
                                }
                            }
                            //Edit
                            else
                            {
                                //Role Management
                                RoleManagementController controller = new RoleManagementController();

                                string oldprefix = null;
                                controller.Load(MemberShip.UserRoleId);
                                if (controller.SubCategory.Add || MemberShip.isAdmin)
                                {
                                    APP_Data.ProductSubCategory psc = posEntity.ProductSubCategories.Where(x => x.Name == txtName.Text && x.Name!=oldName).FirstOrDefault();
                                    if (psc == null)
                                    {
                                        APP_Data.ProductSubCategory EditCat = posEntity.ProductSubCategories.Where(x => x.Id == SubCategoryId).FirstOrDefault();
                                        //oldprefix = EditCat.Prefix;

                                        EditCat.Name = txtName.Text.Trim();
                                       // EditCat.Prefix = txtPrefix.Text.Trim();
                                        EditCat.ProductCategoryId = Int32.Parse(cboMCategory.SelectedValue.ToString());
                                        EditCat.Prefix = txtPrefix.Text.ToString();
                                        posEntity.SaveChanges();
                                        dgvProductList.DataSource = (from pType in posEntity.ProductSubCategories orderby pType.Id descending select pType).ToList();
                                        MessageBox.Show("Successfully Update !!", "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        cboMCategory.SelectedIndex = 0;

                                        bool notbackoffice = Utility.IsNotBackOffice();
                                        if (notbackoffice)
                                        {
                                            Utility.Gpvisible(groupBox1, false);
                                        }

                                        List<APP_Data.Product> plist = posEntity.Products.Where(x => x.ProductSubCategoryId == SubCategoryId).ToList();
                                        bool isExit = false;
                                        foreach (Product p1 in plist)
                                        {
                                            List<TransactionDetail> tdlist = posEntity.TransactionDetails.Where(x => x.ProductId == p1.Id).ToList();
                                            if (tdlist.Count > 0)
                                            {
                                                isExit = true;
                                            }
                                        }
                                        if (isExit == false)
                                        {
                                            //string newPrefix1 = txtPrefix.Text.ToString();
                                          

                                            List<APP_Data.Product> psublist = posEntity.Products.Where(x => x.ProductSubCategoryId == SubCategoryId).ToList();
                                            APP_Data.Product p4 = posEntity.Products.Where(x => x.ProductSubCategoryId == SubCategoryId).FirstOrDefault();
                                            foreach (APP_Data.Product p3 in psublist)
                                            {
                                                string temp = p3.ProductCode.ToString();
                                                //string s = temp.Replace(oldprefix, newPrefix1);
                                                //p3.ProductCode = s.ToString();
                                                //p3.Barcode = s.ToString();
                                                posEntity.SaveChanges();

                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("This Sub Category is already used in transaction");
                                        }
                                        txtName.Clear();
                                        txtPrefix.Clear();

                                        #region active new Product
                                        if (System.Windows.Forms.Application.OpenForms["NewProduct"] != null)
                                        {
                                            NewProduct newForm = (NewProduct)System.Windows.Forms.Application.OpenForms["NewProduct"];
                                            newForm.ReloadCategory();
                                            newForm.SetCurrentCategory(Convert.ToInt32(EditCat.ProductCategoryId));
                                            newForm.SetCurrentSubCategory(EditCat.Id);
                                        }
                                        #endregion
                                        //APP_Data.ProductSubCategory p = posEntity.ProductSubCategories.Where(x => x.Prefix == txtPrefix.Text && x.Prefix!=oldPrefix2).FirstOrDefault();
                                        //if (p == null)
                                        //{
                                            
                                        //}
                                        //else
                                        //{
                                        //    tp.SetToolTip(txtPrefix, "Error");
                                        //    tp.Show("This Sub Category Code is already exist!", txtPrefix);
                                        //}                                    
                                    }
                                    else
                                    {
                                        tp.SetToolTip(txtName, "Error");
                                        tp.Show("This sub category name  is already exist!", txtName);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("You are not allowed to edit sub category", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    return;      
                                 }
                                         
                    }                  
                }               
            }
            else
            {
                tp.SetToolTip(txtName, "Error");
                tp.Show("Please fill up Main Category!", txtName);
            }           
        }

        private void dgvProductList_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            int currentId;
            if (e.RowIndex >= 0)
            {
                //Delete
                if (e.ColumnIndex == 5)
                {
                    //Role Management
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.SubCategory.EditOrDelete || MemberShip.isAdmin)
                    {
                        DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (result.Equals(DialogResult.OK))
                        {
                            Clear();
                            DataGridViewRow row = dgvProductList.Rows[e.RowIndex];
                            currentId = Convert.ToInt32(row.Cells[0].Value);
                            int count = (from p in posEntity.Products where p.ProductSubCategoryId == currentId select p).ToList().Count;
                            if (count < 1)
                            {
                                dgvProductList.DataSource = "";
                                APP_Data.ProductSubCategory pType = (from pT in posEntity.ProductSubCategories where pT.Id == currentId select pT).FirstOrDefault();
                                int productMainCID = Convert.ToInt32(pType.ProductCategoryId);
                                pType.IsDelete = true;
                                posEntity.ProductSubCategories.Remove(pType);
                                posEntity.SaveChanges();
                                dgvProductList.DataSource = (from pt in posEntity.ProductSubCategories select pt).ToList();

                                #region active new Product
                                if (System.Windows.Forms.Application.OpenForms["NewProduct"] != null)
                                {
                                    NewProduct newForm = (NewProduct)System.Windows.Forms.Application.OpenForms["NewProduct"];
                                    newForm.ReloadCategory();
                                    newForm.SetCurrentCategory(productMainCID);
                                    newForm.SetCurrentSubCategory(0);
                                }
                                #endregion 
                            }
                            else
                            {
                                //To show message box 
                                MessageBox.Show("This product sub category is currently in use!", "Enable to delete", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to delete sub category", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                //Edit
                else if (e.ColumnIndex == 4)
                {
                    bool notbackoffice = Utility.IsNotBackOffice();
                    //Role Management
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.SubCategory.EditOrDelete || MemberShip.isAdmin)
                    {

                        DataGridViewRow row = dgvProductList.Rows[e.RowIndex];
                        currentId = Convert.ToInt32(row.Cells[0].Value);
                        APP_Data.ProductSubCategory pType = (from pT in posEntity.ProductSubCategories where pT.Id == currentId select pT).FirstOrDefault();
                        SubCategoryId = pType.Id;
                        txtName.Text = pType.Name;
                        txtName.Tag = pType.Name;
                        oldName = pType.Name;
                        txtPrefix.Text = pType.Prefix;
                        //txtPrefix.Text = pType.Prefix;
                        //oldPrefix2 = pType.Prefix;
                        cboMCategory.SelectedValue = pType.ProductCategoryId.ToString();
                        cboMCategory.Text = pType.ProductCategory.Name;
                        isEdit = true;
                        btnAdd.Image = Properties.Resources.update_small;
                        this.Text = "Edit SubCategory";
                        groupBox1.Text = "Edit SubCategory";

                        //txtPrefix.Enabled = false;

                        if (notbackoffice)
                        {
                            Utility.Gpvisible(groupBox1, isEdit);
                        }
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to edit sub category", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void ProductType_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(txtName);
            tp.Hide(cboMCategory);
        }

        private void dgvProductList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvProductList.Rows)
            {

                APP_Data.ProductSubCategory currentCategory = (APP_Data.ProductSubCategory)row.DataBoundItem;
                row.Cells[0].Value = (object)currentCategory.Id;
                row.Cells[1].Value = (object)currentCategory.ProductCategory.Name;
                row.Cells[2].Value = (object)currentCategory.Name;

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        #endregion

        #region Function

        private void Clear()
        {
            isEdit = false;
            this.Text = "Add New Product SubCategory";
            groupBox1.Text = "Add New Product SubCategory";
            SubCategoryId = 0;
            CategoryId = 0;
            txtName.Text = string.Empty;
            //txtPrefix.Text = string.Empty;
            cboMCategory.SelectedIndex = 0;
            btnAdd.Image = Properties.Resources.add_small;
            bool notbackoffice = Utility.IsNotBackOffice();
            if (notbackoffice)
            {
                Utility.Gpvisible(groupBox1, false);
            }
        }

        #endregion

        private static bool ContainsNumber(string input)
        {
            return Regex.IsMatch(input, @"\d+");
        }
        string _latestValidText = string.Empty;
        private void txtPrefix_TextChanged(object sender, EventArgs e)
        {
            TextBox target = sender as TextBox;
            if (ContainsNumber(target.Text))
            {
                // display alert and reset text
                target.Text = _latestValidText;
                MessageBox.Show("Please fill up only character");
            }
            else
            {
                _latestValidText = target.Text;
            }
        }

    }
}
