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
    public partial class UserControl : Form
    {

        #region Variable
        private int _shopId = 0;
        private POSEntities entity = new POSEntities();
        #endregion

        

        public UserControl()
        {
            InitializeComponent();
            dgvSalesPersonList.AutoGenerateColumns = false;
        }

        private void btnAddSalesPerson_Click(object sender, EventArgs e)
        {
            NewUser form = new NewUser();
            form.isEdit = false;
            form.ShowDialog();
        }

        public void SetLoad()
        {
            txtShop.Text = SettingController.DefaultShop.ShopName;
            _shopId = SettingController.DefaultShop.Id;
            var userlist = (from p in entity.Users select p.CreatedBy).ToList();
            List<int> user = (from c in entity.Users where userlist.Contains(c.Id) && c.ShopId == SettingController.DefaultShop.Id select c.Id).ToList();
            dgvSalesPersonList.DataSource = (from u in entity.Users where u.Name != "sourcecodeadmin" && (user.Contains(u.CreatedBy ?? 0) || u.ShopId == _shopId) select u).ToList();
            if (Utility.IsNotBackOffice())
            {
                dgvSalesPersonList.Columns[4].Visible = false;
            }
            else
            {
                dgvSalesPersonList.Columns[4].Visible = true;
            }
        }

        private void UserControl_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            //dgvSalesPersonList.DataSource = entity.Users.ToList();
          //  _shop = Utility.Get_DefaultShop();
            SetLoad();
        }

        private void dgvSalesPersonList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvSalesPersonList.Rows)
            {                
                User user = (User)row.DataBoundItem;             
                row.Cells[2].Value = user.UserRole.RoleName;
                row.Cells[4].Value=(from p in entity.Shops where p.Id==user.ShopId select p.ShopName).FirstOrDefault();
            }
        }

        private void UserControl_Activated(object sender, EventArgs e)
        {

          //  dgvSalesPersonList.DataSource = entity.Users.ToList();
            dgvSalesPersonList.DataSource = (from u in entity.Users where u.Name != "sourcecodeadmin" && u.ShopId == _shopId select u).ToList();
        }

        private void dgvSalesPersonList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                //Delete
                if (e.ColumnIndex == 6)
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (result.Equals(DialogResult.OK))
                    {
                        DataGridViewRow row = dgvSalesPersonList.Rows[e.RowIndex];
                        User user = (User)row.DataBoundItem;
                        user = (from c in entity.Users where c.Id == user.Id select c).FirstOrDefault<User>();

                        //Need to recheck
                        if (user.Transactions.Count > 0)
                        {
                            MessageBox.Show("This user is already in use.", "Cannot Delete");
                            return;
                        }
                        else
                        {
                            entity.Users.Remove(user);
                            entity.SaveChanges();
                            dgvSalesPersonList.DataSource = (from u in entity.Users where u.Name != "sourcecodeadmin" && u.ShopId == _shopId select u).ToList();
                        }
                    }
                }
                //Edit
                else if (e.ColumnIndex == 5)
                {
                    NewUser form = new NewUser();
                    form.isEdit = true;
                    form.Text = "Edit User";
                    form.UserId = Convert.ToInt32(dgvSalesPersonList.Rows[e.RowIndex].Cells[0].Value);
                    form.Show();
                }
            }
        }

        #region Method
 
        #endregion
    }
}
