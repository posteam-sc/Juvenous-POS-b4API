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
    public partial class City : Form
    {
        #region Variable

        POSEntities entity = new POSEntities();
        private ToolTip tp = new ToolTip();
        int cityId = 0;
        private bool isEdit = false;
        private int CityId = 0;
        int currentId;
        #endregion

        public City()
        {
            InitializeComponent();
        }

        private void City_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            bool notbackoffice = Utility.IsNotBackOffice();
            if (notbackoffice)
            {
                Utility.Gpvisible(groupBox1, isEdit);
            }
            dgvCityList.AutoGenerateColumns = false;
            dgvCityList.DataSource = entity.Cities.Where(x=>x.IsDelete==false).ToList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            bool HaveError = false;
            if (txtName.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtName, "Error");
                tp.Show("Please fill up brand name!", txtName);
                HaveError = true;
            }
            if (!HaveError)
            {
                string CityName = txtName.Text.Trim();
                APP_Data.City CityObj = new APP_Data.City();
                APP_Data.City alredyCityObj = new APP_Data.City();
                if (currentId != 0)
                {
                    alredyCityObj = entity.Cities.Where(x => x.CityName.Trim() == CityName && x.Id != currentId && x.IsDelete == false).FirstOrDefault();
                }
                else
                {
                    alredyCityObj = entity.Cities.Where(x => x.CityName.Trim() == CityName && x.IsDelete == false).FirstOrDefault();
                }
                if (alredyCityObj == null)
                {

                    if (!isEdit)
                    {
                        dgvCityList.DataSource = "";
                        CityObj.CityName = txtName.Text;
                        CityObj.IsDelete = false;
                        entity.Cities.Add(CityObj);
                        entity.SaveChanges();
                        dgvCityList.DataSource = entity.Cities.Where(x=>x.IsDelete==false).ToList();
                        cityId = CityObj.Id;
                        MessageBox.Show("Successfully Saved!", "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Back_CityData_ToCallForm();
                    }
                    else
                    {
                        APP_Data.City EditCity = entity.Cities.Where(x => x.Id == CityId).FirstOrDefault();
                        EditCity.CityName = txtName.Text.Trim();
                        EditCity.IsDelete = false;
                        entity.SaveChanges();

                        dgvCityList.DataSource = (from b in entity.Cities where b.IsDelete==false orderby b.Id descending select b).ToList();
                        bool notbackoffice = Utility.IsNotBackOffice();
                        if (notbackoffice)
                        {
                            Utility.Gpvisible(groupBox1, false);
                        }
                        cityId = EditCity.Id;
                        Back_CityData_ToCallForm();
                    }
                    Clear();
                }
                else
                {
                    tp.SetToolTip(txtName, "Error");
                    tp.Show("This city name is already exist!", txtName);
                }
            }
        }

        private void dgvCityList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
         
            if (e.RowIndex >= 0)
            {
                //Role Management
                RoleManagementController controller = new RoleManagementController();
                controller.Load(MemberShip.UserRoleId);
                if (controller.City.EditOrDelete || MemberShip.isAdmin)
                {
                    //Edit
                    if (e.ColumnIndex == 2)
                    {

                        //Role Management
                        bool notbackoffice = Utility.IsNotBackOffice();
                        DataGridViewRow row = dgvCityList.Rows[e.RowIndex];
                        currentId = Convert.ToInt32(row.Cells[0].Value);

                        APP_Data.City City = (from b in entity.Cities where b.IsDelete==false where b.Id == currentId select b).FirstOrDefault();
                        txtName.Text = City.CityName;
                        isEdit = true;
                        this.Text = "Edit City";
                        groupBox1.Text = "Edit City";
                        CityId = City.Id;
                        btnAdd.Image = Properties.Resources.update_small;
                        
                        if (notbackoffice)
                        {
                            Utility.Gpvisible(groupBox1, isEdit);
                        }

                    }
                    //Delete
                    if (e.ColumnIndex == 3)
                    {
                        DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (result.Equals(DialogResult.OK))
                        {
                            Clear();
                            DataGridViewRow row = dgvCityList.Rows[e.RowIndex];
                            currentId = Convert.ToInt32(row.Cells[0].Value);
                            int count = (from Cus in entity.Customers where Cus.CityId == currentId select Cus).ToList().Count;
                            if (count < 1)
                            {
                                dgvCityList.DataSource = "";
                                APP_Data.City city = (from c in entity.Cities where c.Id == currentId select c).FirstOrDefault();
                                city.IsDelete = true;
                               
                                entity.SaveChanges();
                                dgvCityList.DataSource = entity.Cities.Where(x=>x.IsDelete==true).ToList();

                                cityId = 0;
                                MessageBox.Show("Successfully Deleted!", "Delete Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                txtName.Text = "Select";

                                Back_CityData_ToCallForm();
                            }
                            else
                            {
                                //To show message box 
                                MessageBox.Show("This city name is currently in use!", "Enable to delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("You are not allowed to edit/delete city.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        #region Method
        private void Back_CityData_ToCallForm()
        {
            #region active setting and customer
            if (System.Windows.Forms.Application.OpenForms["Shop"] != null)
            {
                Shop newForm = (Shop)System.Windows.Forms.Application.OpenForms["Shop"];
                //  newForm.ReLoadCity();
                newForm.SetCurrentCity(cityId);
                this.Close();
            }
            cityId = 0;

            if (System.Windows.Forms.Application.OpenForms["NewCustomer"] != null)
            {
                NewCustomer newForm = (NewCustomer)System.Windows.Forms.Application.OpenForms["NewCustomer"];
                newForm.reloadCity(txtName.Text);
                this.Close();
            }
            txtName.Text = "";
           
            #endregion
        }

        private void Clear()
        {
            isEdit = false;
            this.Text = "Add New City";
            groupBox1.Text = "Add New City";
            txtName.Text = string.Empty;
            CityId = 0;
            btnAdd.Image = Properties.Resources.add_small;
            bool notbackoffice = Utility.IsNotBackOffice();
            if (notbackoffice)
            {
                Utility.Gpvisible(groupBox1, false);
            }
        }
        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

    }
}
