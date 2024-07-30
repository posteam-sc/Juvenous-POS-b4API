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
    public partial class Shop : Form
    {
        #region Variables

        public Boolean isEdit { get; set; }

        public int ShopId { get; set; }

        private POSEntities entity = new POSEntities();

        private ToolTip tp = new ToolTip();

        #endregion

        #region Events

        public Shop()
        {
            InitializeComponent();
        }

        private void Shop_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            bool notbackoffice = Utility.IsNotBackOffice();
            if (notbackoffice)
            {
                Utility.Gpvisible(groupBox2, isEdit);
            }

            BindCity();
            bind_grid();
            txtShortCode.Enabled = true;
            //if (isEdit)
            //{
            //    APP_Data.Shop updateShop = (from s in entity.Shops where s.Id == ShopId select s).FirstOrDefault();
            //    txtShopName.Text = updateShop.ShopName;
            //    txtAddress.Text = updateShop.Address;
            //    txtPhone.Text = updateShop.PhoneNumber;
            //    txtOpeningHours.Text = updateShop.OpeningHours;
            //    txtShortCode.Text = updateShop.ShortCode;
            //    cboCity.Text = updateShop.City.CityName;
            //    entity.Entry(updateShop).State = EntityState.Modified;
            //    entity.SaveChanges();
            //}

        }


        private void btnSave_Click_1(object sender, EventArgs e)
        {
            bool hasError = false;
            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            if (txtShopName.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtShopName, "Error");
                tp.Show("Please fill up shop name!", txtShopName);
                hasError = true;
            }
            else if (txtShortCode.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtShortCode, "Error");
                tp.Show("Please fill up short code!", txtShortCode);
                hasError = true;
            }

            else
            {

                //Check if short code already exist
         
                //if (!isEdit)
                //{
                    APP_Data.Shop duplicateShopname = (from s in entity.Shops where s.ShopName == txtShopName.Text.Trim()  &&  ((ShopId ==0 && 1==1) || (ShopId > 0 && s.Id != ShopId))select s).FirstOrDefault();
                    string duplicateShortCode = (from s in entity.Shops where s.ShortCode == txtShortCode.Text.Trim()  && s.Id != ShopId select s.ShortCode).FirstOrDefault();
                    if (duplicateShopname != null)
                    {
                        hasError = true;
                        tp.SetToolTip(txtShopName, "Error");
                        tp.Show("shop name already exist!", txtShopName);
                    }
                    else if (duplicateShortCode != null)
                    {
                        hasError = true;
                        tp.SetToolTip(txtShortCode, "Error");
                        tp.Show("Short code already exist!", txtShortCode);
                    }
            

            }



            if (!hasError)
            {
                if (isEdit)
                {
                    APP_Data.Shop updateShop = (from s in entity.Shops where s.Id == ShopId select s).FirstOrDefault();
                    updateShop.ShopName = txtShopName.Text;
                    updateShop.Address = txtAddress.Text;
                    updateShop.PhoneNumber = txtPhone.Text;
                    updateShop.OpeningHours = txtOpeningHours.Text;
                    updateShop.ShortCode = txtShortCode.Text;
                    updateShop.CityId = Convert.ToInt32(cboCity.SelectedValue);
                    entity.Entry(updateShop).State = EntityState.Modified;
                    entity.SaveChanges();

                    MessageBox.Show("Successfully Update!", "Update");
                  
                    //    this.Dispose();
                }
                else
                {
                    APP_Data.Shop shopObj = new APP_Data.Shop();
                    shopObj.ShopName = txtShopName.Text;
                    shopObj.Address = txtAddress.Text;
                    shopObj.PhoneNumber = txtPhone.Text;
                    shopObj.OpeningHours = txtOpeningHours.Text;
                    shopObj.ShortCode = txtShortCode.Text;
                    shopObj.IsDefaultShop = false;
                    shopObj.CityId = Convert.ToInt32(cboCity.SelectedValue);
                    entity.Shops.Add(shopObj);
                    entity.SaveChanges();

                    MessageBox.Show("Successfully Saved!", "Save");
                
                    // this.Dispose();
                }

                if (Application.OpenForms["Setting"] != null)
                {
                    //Setting newForm = (Setting)Application.OpenForms["Setting"];
                    //newForm.UpdateShopList(txtShopName.Text);
                }
                clean();
                bind_grid();
            }
       

        }

        private void  btnCity_Click_1(object sender, EventArgs e)
        {
            City form = new City();
            form.ShowDialog();
        }


        private void ShortCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void Phone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dgvshopview_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvshopview.Rows)
            {
                APP_Data.Shop shopview = (APP_Data.Shop)row.DataBoundItem;
                APP_Data.City city = (from c in entity.Cities where c.Id == shopview.CityId select c).FirstOrDefault();
                row.Cells[0].Value = shopview.Id;
                row.Cells[1].Value = shopview.ShortCode;
                row.Cells[2].Value = shopview.ShopName;
                row.Cells[3].Value = shopview.PhoneNumber;
                row.Cells[4].Value = shopview.OpeningHours;
                row.Cells[5].Value = shopview.Address;
                row.Cells[6].Value = city.CityName;

            }
        }

        private void dgvshopview_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentId;
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (e.RowIndex >= 0)
            {
                //Edit
                if (e.ColumnIndex == 7)
                {
                    bool notbackoffice = Utility.IsNotBackOffice();
                    if (controller.Brand.EditOrDelete || MemberShip.isAdmin)
                    {
                        DataGridViewRow currentrow = dgvshopview.Rows[e.RowIndex];
                        currentId = Convert.ToInt32(currentrow.Cells[0].Value);

                        APP_Data.Shop editshop = (from b in entity.Shops where b.Id == currentId select b).FirstOrDefault();

                            txtShopName.Text = editshop.ShopName;
                            txtPhone.Text = editshop.PhoneNumber;
                            txtOpeningHours.Text = editshop.OpeningHours;
                            txtShortCode.Text = editshop.ShortCode;
                            txtAddress.Text = editshop.Address;
                            cboCity.Text = editshop.City.CityName;
                           
                            if (editshop.Transactions.Count > 0)
                            {
                                txtShortCode.Enabled = false;
                            }

                            this.Text = "Edit Shop";
                            groupBox2.Text = "Edit Shop";
                            ShopId = editshop.Id;
                            isEdit = true;
                            this.btnSave.Image = POS.Properties.Resources.update_small;
                      
                            if (notbackoffice)
                            {
                                Utility.Gpvisible(groupBox2, isEdit);
                            }
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to edit brand", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else if (e.ColumnIndex == 8)
                {
                    //Delete
                    if (controller.Brand.EditOrDelete || MemberShip.isAdmin)
                    {
                        DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if(result.Equals(DialogResult.OK)){
                            clean();
                            DataGridViewRow row=dgvshopview.Rows[e.RowIndex];
                            currentId=Convert.ToInt32(row.Cells[0].Value);
                            APP_Data.Shop transShop=(from t in entity.Shops where t.Id==currentId select t).FirstOrDefault();
                            List<APP_Data.User> _userList=(from u in entity.Users where u.ShopId == currentId select u).ToList();
                            if (transShop.Transactions.Count > 0 || _userList.Count > 0)
                            {
                                 MessageBox.Show("This shop already made transactions! or This shop already used in User!", "Unable to Delete");
                                return;
                            }else{
                                entity.Shops.Remove(transShop);
                                entity.SaveChanges();
                                bind_grid();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to delete brand", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            clean();
        }
        #endregion

        #region Method
        public void BindCity()
        {
            Utility.BindCity(cboCity);
        }

        public void SetCurrentCity(Int32 CityID)
        {
            APP_Data.City _city = entity.Cities.Where(x => x.Id == CityID).FirstOrDefault();
            if (_city != null)
            {
                cboCity.Text = _city.CityName;
            }
        }

        private void bind_grid()
        {
            bool notbackoffice = Utility.IsNotBackOffice();
            if (notbackoffice)
            {
                Utility.Gpvisible(groupBox2,false);
                dgvshopview.AutoGenerateColumns = false;
                dgvshopview.DataSource = (from b in entity.Shops where b.Id==SettingController.DefaultShop.Id orderby b.Id descending select b).ToList();
            }
            else
            {
                dgvshopview.AutoGenerateColumns = false;
                dgvshopview.DataSource = (from b in entity.Shops orderby b.Id descending select b).ToList();
            }
           
        }

        private void clean()
        {
            txtShopName.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtOpeningHours.Text = string.Empty;
            txtShortCode.Text = string.Empty;
            BindCity();
            btnSave.Image = Properties.Resources.add_small;
            isEdit = false;
            this.Text = "Add New Shop";
            groupBox2.Text = "Add New Shop";
            ShopId = 0;
            txtShortCode.Enabled = true;
            bool notbackoffice = Utility.IsNotBackOffice();
            if (notbackoffice)
            {
                Utility.Gpvisible(groupBox2, false);
            }
        }
        #endregion

        private void Shop_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(txtShopName);
            tp.Hide(txtShortCode);
        }
    }
}
