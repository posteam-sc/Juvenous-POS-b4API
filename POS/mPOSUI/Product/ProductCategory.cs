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
    public partial class ProductCategory : Form
    {
        #region Variable

        POSEntities posEntity = new POSEntities();
        private ToolTip tp = new ToolTip();
        private Boolean isEdit = false;
        private int categoryId = 0;
        string OldName = "";
        string OldPrefix = "";

        #endregion

        #region Event
        public ProductCategory()
        {
            InitializeComponent();
        }
        private void ProductCategory_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            bool notbackoffice = Utility.IsNotBackOffice();
            if (notbackoffice)
            {
                Utility.Gpvisible(groupBox1, isEdit);
            }
            dgvProductCList.AutoGenerateColumns = false;
            dgvProductCList.DataSource = (from pType in posEntity.ProductCategories orderby pType.Id descending select pType).ToList();
            //this.txtName.GotFocus += new System.EventHandler(this.txtName_GotFocus);
            //this.txtName.LostFocus += new System.EventHandler(this.txtName_LostFocus);

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.Category.Add || MemberShip.isAdmin)
            {
                Boolean hasError = false;
                tp.RemoveAll();
                tp.IsBalloon = true;
                tp.ToolTipIcon = ToolTipIcon.Error;
                tp.ToolTipTitle = "Error";

                if (txtName.Text.Trim() =="" || txtName.Text.Trim() == "eg. Baby Corner")
                {
                    tp.SetToolTip(txtName, "Error");
                    tp.Show("Please fill up product category name!", txtName);
                    hasError = true;
                }                        
                if (hasError!=true)
                {
                    APP_Data.ProductCategory pCategory = new APP_Data.ProductCategory();
                    APP_Data.ProductCategory pCObj = (from pC in posEntity.ProductCategories where pC.Name == txtName.Text && pC.Name!=OldName select pC).FirstOrDefault();
                    if (pCObj == null)
                    {
                        //New Product
                            if (!isEdit)
                            {
                                dgvProductCList.DataSource = "";
                                pCategory.Name = txtName.Text;
                                pCategory.IsDelete = false;
                                posEntity.ProductCategories.Add(pCategory);
                                posEntity.SaveChanges();
                                dgvProductCList.DataSource = (from pType in posEntity.ProductCategories orderby pType.Id descending select pType).ToList();
                                MessageBox.Show("Successfully Saved!", "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                #region active new Product
                                if (System.Windows.Forms.Application.OpenForms["NewProduct"] != null)
                                {
                                    NewProduct newForm = (NewProduct)System.Windows.Forms.Application.OpenForms["NewProduct"];
                                    newForm.ReloadCategory();
                                    newForm.SetCurrentCategory(pCategory.Id);
                                }
                                #endregion
                                txtName.Text = "";
                            }
                            else
                            {
                                APP_Data.ProductCategory EditCat = posEntity.ProductCategories.Where(x => x.Id == categoryId).FirstOrDefault();
                                EditCat.Name = txtName.Text.Trim();
                                posEntity.SaveChanges();
                                dgvProductCList.DataSource = (from pType in posEntity.ProductCategories orderby pType.Id descending select pType).ToList();

                                MessageBox.Show("Successfully Update!", "Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                #region active new Product
                                if (System.Windows.Forms.Application.OpenForms["NewProduct"] != null)
                                {
                                    NewProduct newForm = (NewProduct)System.Windows.Forms.Application.OpenForms["NewProduct"];
                                    newForm.ReloadCategory();
                                    newForm.SetCurrentCategory(EditCat.Id);
                                }
                                #endregion
                                bool notbackoffice = Utility.IsNotBackOffice();
                                if (notbackoffice)
                                {
                                    Utility.Gpvisible(groupBox1, false);
                                }
                            }                   
                    }
                    else
                    {
                        tp.SetToolTip(txtName, "Error");
                        tp.Show("This category name is already exist!", txtName);
                    }
                }               
                //txtName.Text = "eg. Baby Corner";
                //txtPrefix.Text = "";
            }
            else
            {
                MessageBox.Show("You are not allowed to add new category", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dgvProductCList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentId;
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 3)
                { //Role Management
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.Category.EditOrDelete || MemberShip.isAdmin)
                    {
                        DataGridViewRow row = dgvProductCList.Rows[e.RowIndex];
                        currentId = Convert.ToInt32(row.Cells[0].Value);
                        if (currentId != 1045)
                        {
                            DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            if (result.Equals(DialogResult.OK))
                            {
                                clear();
                                int subCatCount = posEntity.ProductSubCategories.Where(x => x.ProductCategoryId == currentId).Count();
                                int pcount = posEntity.Products.Where(x => x.ProductCategoryId == currentId).Count();
                                
                                if (subCatCount > 0)
                                {
                                    MessageBox.Show("Please delete this product's sub category first!", "Enable to delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                                if (pcount > 0)
                                {
                                    MessageBox.Show("Please delete  product using this category!", "Enable to delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                int count = (from p in posEntity.Products where p.ProductCategoryId == currentId select p).ToList().Count;
                                if (count < 1)
                                {
                                    dgvProductCList.DataSource = "";
                                    APP_Data.ProductCategory pC = (from pCat in posEntity.ProductCategories where pCat.Id == currentId select pCat).FirstOrDefault();
                                    pC.IsDelete = true;
                                    posEntity.SaveChanges();
                                    dgvProductCList.DataSource = (from pt in posEntity.ProductCategories select pt).ToList();
                                    #region active new Product
                                    if (System.Windows.Forms.Application.OpenForms["NewProduct"] != null)
                                    {
                                        NewProduct newForm = (NewProduct)System.Windows.Forms.Application.OpenForms["NewProduct"];
                                        newForm.ReloadCategory();
                                        newForm.SetCurrentCategory(0);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    //To show message box 
                                    MessageBox.Show("This product category is currently in use!", "Enable to delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }

                        }
                        else
                        {
                            //To show message box 
                            MessageBox.Show("This product category is cannot delete!", "Enable to delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }              
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to delete category", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else if(e.ColumnIndex == 2)
                {
                    bool notbackoffice = Utility.IsNotBackOffice();
                    //Role Management
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.Category.EditOrDelete || MemberShip.isAdmin)
                    {
                        DataGridViewRow row = dgvProductCList.Rows[e.RowIndex];
                        currentId = Convert.ToInt32(row.Cells[0].Value);
                        if (currentId != 1045)
                        {
                            //int pcount = posEntity.Products.Where(x => x.ProductCategoryId == currentId).Count();                           
                            //if (pcount > 0)
                            //{
                            //    MessageBox.Show("This product category is cannot edit!", "Enable to delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //    return;
                            //}
                            APP_Data.ProductCategory pC = (from pCat in posEntity.ProductCategories where pCat.Id == currentId select pCat).FirstOrDefault();
                            txtName.Text = pC.Name;
                            OldName = pC.Name;
                            isEdit = true;
                            categoryId = pC.Id;
                            btnAdd.Image = Properties.Resources.update_small;
                            this.Text = "Edit Product Category";
                            groupBox1.Text = "Edit Product Category";
                            if (notbackoffice)
                            {
                                Utility.Gpvisible(groupBox1, isEdit);
                            }
                        }
                        else
                        {
                            MessageBox.Show("This product category is cannot edit!", "Enable to delete", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                        }
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to edit category", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void ProductCategory_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(txtName);
        }

        private void txtName_Click(object sender, EventArgs e)
        {
            //txtName.Text = "";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            clear();
        }
      
        private void clear()
        {
            isEdit = false;
            this.Text = "Add New Product Category";
            groupBox1.Text = "Add New Product Category";
            categoryId = 0;
            txtName.Text = string.Empty;
            btnAdd.Image = Properties.Resources.add_small;
            bool notbackoffice = Utility.IsNotBackOffice();
            if (notbackoffice)
            {
                Utility.Gpvisible(groupBox1, false);
            }
        }
        #endregion                                      

       
    }
}
