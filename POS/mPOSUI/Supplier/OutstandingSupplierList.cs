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
    public partial class OutstandingSupplierList : Form
    {
        private POSEntities entity = new POSEntities();
      
        public OutstandingSupplierList()
        {
            InitializeComponent();
            dgvSupplierList.AutoGenerateColumns = false;
        }

        private void OutstandingSuppliererList_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            Utility.BindSupplier(cboSupplierName);
            DataBind();
        }       


        private void dgvSupplierList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                //View Detail
                 if (e.ColumnIndex == 4)
                {
                      //Role Management
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.OutstandingSupplier.ViewDetail|| MemberShip.isAdmin)
                    {
                    //Show Outstanding Supplier Detail Form
                    OutstandingSupplierDetail form = new OutstandingSupplierDetail();
                    form.supplierId = Convert.ToInt32(dgvSupplierList.Rows[e.RowIndex].Cells[0].Value);
                    //form.TotalOutstanding = Convert.ToInt64(dgvSupplierList.Rows[e.RowIndex].Cells[3].Value);
                    form.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to view outstanding supplier detail", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void DataBind()
        {
            entity = new POSEntities();

            int _supId = 0;
            if (cboSupplierName.SelectedIndex != 0)
            {
                _supId = Convert.ToInt32(cboSupplierName.SelectedValue);
            }
           
            IQueryable<object> q = from s in entity.Suppliers
                           join p in entity.MainPurchases on s.Id equals p.SupplierId
                            where p.OldCreditAmount != 0 && p.IsCompletedInvoice==true && (_supId==0 && 1==1 || _supId !=0 && p.SupplierId==_supId)
                             group p by new { s.Id, s.Name,s.PhoneNumber} into outstandingtotal
                           select new { Id = outstandingtotal.Key.Id, Name = outstandingtotal.Key.Name, PhoneNumber = outstandingtotal.Key.PhoneNumber, OutstandingCreditAmount = outstandingtotal.Sum(o=> o.OldCreditAmount) };
            List<object> sup = new List<object>(q);
       
            dgvSupplierList.AutoGenerateColumns = false;
            dgvSupplierList.DataSource = sup;
        }

        private void OutstandingCustomerList_Activated(object sender, EventArgs e)
        {
            DataBind();
        }

        private void cboSupplierName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataBind();
        }
    }
}
