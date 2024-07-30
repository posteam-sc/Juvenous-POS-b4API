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
    public partial class SupplierList : Form
    {
        #region Variable

        POSEntities entity = new POSEntities();

        #endregion

        #region Event

        public SupplierList()
        {
            InitializeComponent();
            dgvSupplierList.AutoGenerateColumns = false;
        }

        private void SupplierList_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            Bind_Supplier();
            DataBind();
        }

        private void Bind_Supplier()
        {
            List<Supplier> supplierList = new List<Supplier>();
            Supplier supplierObj = new Supplier();
            supplierObj.Id = 0;
            supplierObj.Name = "All";
            supplierList.Add(supplierObj);
            supplierList.AddRange(entity.Suppliers.ToList());
            cboSupplierName.DataSource = supplierList;
            cboSupplierName.DisplayMember = "Name";
            cboSupplierName.ValueMember = "Id";
            cboSupplierName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboSupplierName.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void dgvSupplierList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.RowIndex >= 0)
            {
                int currentSupplierId = Convert.ToInt32(dgvSupplierList.Rows[e.RowIndex].Cells[0].Value);
                string currentSupplierName = dgvSupplierList.Rows[e.RowIndex].Cells[1].Value.ToString();
                int previousBalance = Convert.ToInt32(dgvSupplierList.Rows[e.RowIndex].Cells[6].Value);

                if (e.ColumnIndex == 8)
                {
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.Supplier.ViewDetail || MemberShip.isAdmin)
                    {
                        PurchaseListBySupplier newForm = new PurchaseListBySupplier();
                        newForm.supplierId = currentSupplierId;
                        newForm.supplierName = currentSupplierName;
                        newForm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to view detail  voucher", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                   
                }               
                else if (e.ColumnIndex == 9)
                {
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.Supplier.EditOrDelete || MemberShip.isAdmin)
                    {
                        NewSupplier newForm = new NewSupplier();
                        newForm.isEdit = true;
                        newForm.Text = "Edit Supplier";
                        newForm.SupplierId = currentSupplierId;
                        newForm.ShowDialog();
                        Bind_Supplier();
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to edit supplier", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                   
                }
                else if (e.ColumnIndex == 10)
                {
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.Supplier.EditOrDelete || MemberShip.isAdmin)
                    {
                        DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (result.Equals(DialogResult.OK))
                        {
                            DataGridViewRow row = dgvSupplierList.Rows[e.RowIndex];
                            Supplier supplier = (Supplier)row.DataBoundItem;
                            supplier = (from s in entity.Suppliers where s.Id == supplier.Id select s).FirstOrDefault();
                            if (supplier.MainPurchases.Count > 0)
                            {
                                MessageBox.Show("This supplier is used in transaction!", "Cannot Delete");
                                return;
                            }
                            else
                            {
                                entity.Suppliers.Remove(supplier);
                                entity.SaveChanges();
                                Bind_Supplier();
                                DataBind();
                                MessageBox.Show("Successfully Deleted!", "Delete Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to delete supplier", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    
                }
            }
        }

        #endregion

        

        #region Function

        private void DataBind()
        {
            entity = new APP_Data.POSEntities();
            if (cboSupplierName.SelectedIndex == 0)
            {
               // dgvSupplierList.DataSource = (from s in entity.Suppliers where s.Id != 1 select s).ToList();
                dgvSupplierList.DataSource = (from s in entity.Suppliers select s).ToList();
            }
            else
            {
                int supplierId = Convert.ToInt32(cboSupplierName.SelectedValue);
                dgvSupplierList.DataSource = (from s in entity.Suppliers where s.Id == supplierId select s).ToList();
            }
        }

        #endregion

        private void dgvSupplierList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvSupplierList.Rows)
            {
                Supplier sp = (Supplier)row.DataBoundItem;
                //int OldCreditAmount = 0;
                //int SettlementAmount = 0;
                row.Cells[0].Value = sp.Id;
                row.Cells[1].Value = sp.Name;
                row.Cells[2].Value = sp.PhoneNumber;
                row.Cells[3].Value = sp.Email;
                row.Cells[4].Value = sp.Address;
                row.Cells[5].Value = sp.ContactPerson;

                List<MainPurchase> mplist = (from m in entity.MainPurchases where m.SupplierId == sp.Id  select m).ToList();
            
                decimal? _totalOldCreditAmt = entity.MainPurchases.Where(x => x.SupplierId == sp.Id && x.IsActive == true && x.IsCompletedInvoice==true).Sum(x => x.OldCreditAmount);
                row.Cells[6].Value = (_totalOldCreditAmt == null) ? 0 : _totalOldCreditAmt;

            }
        }

        public void Reload()
        {
          
            DataBind();
        }

        private void cboSupplierName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataBind();
        }

        private void cboSupplierName_SelectedValueChanged(object sender, EventArgs e)
        {
            DataBind();
        }
    }
}
