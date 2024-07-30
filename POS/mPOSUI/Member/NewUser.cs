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
    public partial class NewUser : Form
    {
        #region Variables

        public Boolean isEdit { get; set; }

        public int UserId { get; set; }

        private POSEntities entity = new POSEntities();

        private ToolTip tp = new ToolTip();
        private string _menuPermission = "";
        private string month = "";
        private Boolean _isStart = false;
        #endregion

        #region Events

        public NewUser()
        {
            InitializeComponent();
        }

        private void NewUser_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            cboUserRole.DataSource = entity.UserRoles.ToList();
            cboUserRole.DisplayMember = "RoleName";
            cboUserRole.ValueMember = "Id";

            Utility.BindShop(cboShop);
            Utility.ShopComBo_EnableOrNot(cboShop);
            _isStart = true;
            UserRole_EnableOrNot();
            if (isEdit)
            {
                //Editing here
                User currentUser = (from c in entity.Users where c.Id == UserId select c).FirstOrDefault<User>();
                txtName.Text = currentUser.Name;
                cboUserRole.SelectedValue = currentUser.UserRoleId;
                cboShop.SelectedValue = currentUser.ShopId;
                cboShop.Enabled = false;
                Check_Radio(currentUser.MenuPermission);
                txtPassword.Text = Utility.DecryptString(currentUser.Password, "SCPos");
                txtConfirmPassword.Text = Utility.DecryptString(currentUser.Password, "SCPos");
                btnSubmit.Image = POS.Properties.Resources.update_big;
            }
          //  this.ActiveControl = txtName;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Boolean hasError = false;
            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            //Validation
            if (txtName.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtName, "Error");
                tp.Show("User name cannot be empty!", txtName);
                hasError = true;
            }
            else if (txtPassword.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtPassword, "Error");
                tp.Show("Password cannot be empty!", txtPassword);
                hasError = true;
            }
            else if (txtConfirmPassword.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtConfirmPassword, "Error");
                tp.Show("Confirm Passoword cannot be empty!", txtConfirmPassword);
                hasError = true;
            }
            else if (txtConfirmPassword.Text.Trim() != txtPassword.Text.Trim())
            {
                tp.SetToolTip(txtConfirmPassword, "Error");
                tp.Show("Password and confirm password do not match!", txtConfirmPassword);
                hasError = true;
            }

            if (!hasError)
            {
                //Edit
                int _shopId = Convert.ToInt32(cboShop.SelectedValue);
                var _userData = (from u in entity.Users where u.Name == txtName.Text && u.ShopId == _shopId && u.Id != UserId select u).FirstOrDefault();

                if (isEdit)
                {

                    if (_userData == null)
                    {
                        User currentUser = (from c in entity.Users where c.Id == UserId select c).FirstOrDefault<User>();
                        currentUser.Name = txtName.Text;
                        currentUser.Password = Utility.EncryptString(txtPassword.Text, "SCPos");
                        currentUser.UpdatedBy = MemberShip.UserId;
                        currentUser.UpdatedDate = DateTime.Now;
                        Radio_Name();
                        currentUser.MenuPermission = _menuPermission;
                        if (cboUserRole.SelectedValue != null) currentUser.UserRoleId = Convert.ToInt32(cboUserRole.SelectedValue.ToString());

                        entity.Entry(currentUser).State = EntityState.Modified;
                        entity.SaveChanges();
                        MessageBox.Show("Successfully Update!", "Update");
                        this.Dispose();

                    }
                    else
                    {
                        tp.SetToolTip(txtName, "Error");
                        tp.Show("This user name is already exist!", txtName);
                    }
                }
                //create new user
                else
                {

                    if (_userData == null)
                    {
                        User newUser = new User();
                        newUser.Name = txtName.Text;
                        if (cboUserRole.SelectedValue != null) newUser.UserRoleId = Convert.ToInt32(cboUserRole.SelectedValue.ToString());
                        newUser.Password = Utility.EncryptString(txtPassword.Text, "SCPos");
                        newUser.CreatedBy = MemberShip.UserId;
                        newUser.CreatedDate = DateTime.Now;
                        newUser.DateTime = DateTime.Now;
                        newUser.ShopId = _shopId;
                        Radio_Name();
                        newUser.MenuPermission = _menuPermission;
                        
                        //to get user code No
                        string UserCodeNo = Utility.Get_UserCodeNo(_shopId);

                        newUser.UserCodeNo = UserCodeNo;
                        entity.Users.Add(newUser);
                        entity.SaveChanges();
                        MessageBox.Show("Successfully Saved!", "Save");
                        this.Dispose();

                    }
                    else
                    {
                        tp.SetToolTip(txtName, "Error");
                        tp.Show("This user name is already exist!", txtName);
                    }
                }
                Back_CityData_ToCallForm();
            }
        }

        private void Back_CityData_ToCallForm()
        {
            #region active setting and customer

            if (System.Windows.Forms.Application.OpenForms["UserControl"] != null)
            {
                UserControl newForm = (UserControl)System.Windows.Forms.Application.OpenForms["UserControl"];
                newForm.SetLoad();
                this.Close();
            }
           
            #endregion
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to cancel?", "Cancel", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result.Equals(DialogResult.OK))
            {
                this.Dispose();
            }
        }

        private void NewUser_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(txtName);
            tp.Hide(txtPassword);
            tp.Hide(txtConfirmPassword);
        }

        private void cboShop_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserRole_EnableOrNot();
        }
        #endregion

        #region Function

        ////private void Get_MonthNo()
        ////{

        ////    if (DateTime.Now.Month < 10)
        ////    {
        ////        month = "0" + DateTime.Now.Month.ToString();
        ////    }
        ////    else
        ////    {
        ////        month = DateTime.Now.Month.ToString();
        ////    }
        ////}
        private void Radio_Name()
        {
            if (rdoBackOffice.Checked)
            {
                _menuPermission = "BackOffice";
            }
            else if (rdoPOS.Checked)
            {
                _menuPermission = "POS";
            }
            else
            {
                _menuPermission = "Both";
            }
        }

        private void Check_Radio(string menuName)
        {
            switch (menuName)
            {
                case "BackOffice":
                    rdoBackOffice.Checked = true;
                    break;
                case "POS":
                    rdoPOS.Checked = true;
                    break;
                case "Both":
                    rdoBoth.Checked = true;
                    break;

            }
        }

        private void UserRole_EnableOrNot()
        {
            if (_isStart == true)
            {
                if (Convert.ToInt32(cboShop.SelectedValue) != SettingController.DefaultShop.Id)
                {
                    var _userRoleId = entity.UserRoles.Where(x => x.RoleName == "Admin").Select(x => x.Id).FirstOrDefault();
                    cboUserRole.SelectedValue = _userRoleId;
                    cboUserRole.Enabled = false;
                }
                else
                {
                    cboUserRole.Enabled = true;
                }
            }
        }
        #endregion

  
    }
}
