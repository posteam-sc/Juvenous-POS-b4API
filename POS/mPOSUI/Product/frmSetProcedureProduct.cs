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

namespace POS.mPOSUI
{
    public partial class frmSetProcedureProduct : Form
    {

        #region Variables
        private ToolTip tp = new ToolTip();
        private bool isEdit=false;
        #endregion

        public frmSetProcedureProduct()
        {
            InitializeComponent();
        }

        #region Form Load
        private void frmSetProcedureProduct_Load(object sender, EventArgs e)
        {
            BindProcedureProduct();
            BindProduct();
            dgvSetProduct.AutoGenerateColumns = false;
        }
        #endregion

        #region Method

        #region Bind Combo
        private void BindProduct()
        {
            POSEntities entity = new POSEntities();
            List<Product> productList = new List<Product>();
            Product product = new Product();
            product.Id = 0;
            product.Name = "Select";
            productList.Add(product);
            var _product = entity.Products.Where(x => x.IsConsignment == false && (x.IsService == false || x.IsService == null) && x.IsPackage == false).ToList();
            productList.AddRange(_product);
            cboProductName.DataSource = productList;
            cboProductName.DisplayMember = "Name";
            cboProductName.ValueMember = "Id";
            cboProductName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboProductName.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void BindProcedureProduct()
        {
            POSEntities entity = new POSEntities();
            List<Product> productList = new List<Product>();
            Product product = new Product();
            product.Id = 0;
            product.Name = "Select";
            productList.Add(product);
            var _product = entity.Products.Where(x => x.IsPackage == true).ToList();
            productList.AddRange(_product);
            cboProcedureProductName.DataSource = productList;
            cboProcedureProductName.DisplayMember = "Name";
            cboProcedureProductName.ValueMember = "Id";
            cboProcedureProductName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboProcedureProductName.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        #endregion

        #region Validation
        private bool checkAddGrid()
        {
            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            if (cboProductName.SelectedIndex == 0)
            {
                tp.SetToolTip(cboProductName, "Error");
                tp.Show("Please Choose Product Name!", cboProductName);
                cboProductName.Focus();
                return false;
            }
            else if (String.IsNullOrWhiteSpace(txtQty.Text) || txtQty.Text == "0")
            {
                tp.SetToolTip(txtQty, "Error");
                tp.Show("Please fill up Qty!", txtQty);
                txtQty.Focus();
                return false;
            }
            foreach (DataGridViewRow row in dgvSetProduct.Rows)
            {
                var name = Convert.ToString(row.Cells[1].Value);
                if(cboProductName.Text == name)
                {
                    MessageBox.Show("This Product Is Already Defined", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        private bool checkValidation()
        {
            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            if (cboProcedureProductName.SelectedIndex == 0)
            {
                tp.SetToolTip(cboProcedureProductName, "Error");
                tp.Show("Please Choose Procedure Product Name!", cboProcedureProductName);
                cboProcedureProductName.Focus();
                return false;
            }

            else if (dgvSetProduct.Rows.Count == 0)
            {
                MessageBox.Show("Product Must be One Row", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        #endregion

        private bool Save()
        {
            try
            {
                POSEntities entities = new POSEntities();
                foreach (DataGridViewRow row in dgvSetProduct.Rows)
                {
                    SetProcedureProduct product = new SetProcedureProduct
                    {
                        ProcedureProductID = Convert.ToInt64(cboProcedureProductName.SelectedValue),
                        ProcedureProductName = cboProcedureProductName.Text,
                        ProductID = Convert.ToInt64(row.Cells[0].Value),
                        ProductName = Convert.ToString(row.Cells[1].Value),
                        ProductQty = Convert.ToInt32(row.Cells[2].Value)
                    };
                    entities.SetProcedureProducts.Add(product);
                    //entities.Entry(product).State = EntityState.Added;
                    entities.SaveChanges();

                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void BindData()
        {
            POSEntities entities = new POSEntities();
            var procedureList = entities.SetProcedureProducts.Where(x => x.ProcedureProductName.Trim() == cboProcedureProductName.Text).ToList();
            if (procedureList.Count > 0)
            {
                dgvSetProduct.Rows.Clear();
                foreach (var item in procedureList)
                {                    
                    string[] row = new string[] { item.ProductID.ToString(), item.ProductName.ToString(), item.ProductQty.ToString() };
                    dgvSetProduct.Rows.Add(row);
                }
                isEdit = true;
                btnSave.Text = "Update";
            }
            else
            {
                
                isEdit = false;
                txtQty.Clear();
                btnSave.Text = "Save";
                dgvSetProduct.Rows.Clear();
            }

        }

        private void InitialState()
        {
            cboProcedureProductName.SelectedIndex = 0;
            cboProductName.SelectedIndex = 0;
            txtQty.Clear();
            dgvSetProduct.Rows.Clear();
            isEdit = false;
            btnSave.Text = "Save";
        }

        private bool Delete()
        {
            try
            {
                POSEntities entities = new POSEntities();
                var procedureList = entities.SetProcedureProducts.Where(x => x.ProcedureProductName.Trim() == cboProcedureProductName.Text).ToList();
                if (procedureList.Count > 0)
                {
                    foreach (var item in procedureList)
                    {
                        entities.SetProcedureProducts.Remove(item);

                    }
                    entities.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;

            }
        }
        #endregion

        #region Button Click Event
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string[] row = new string[] { cboProductName.SelectedValue.ToString(), cboProductName.Text.ToString(), txtQty.Text };
            if (checkAddGrid())
            {
                dgvSetProduct.Rows.Add(row);
                cboProductName.SelectedIndex = 0;
                txtQty.Clear();
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (checkValidation())
            {
                if (isEdit == true)
                {
                    if (Delete())
                    {
                        if (Save())
                        {
                            MessageBox.Show("Update Successful", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            InitialState();
                        }
                    }
                }else
                {
                    if (Save())
                    {
                        MessageBox.Show("Save Successful", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        InitialState();
                    }
                }
                
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            InitialState();
        }

        #endregion

        #region Key Press

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        #endregion

        #region Selected Index Change
        private void cboProcedureProductName_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }
        #endregion

        #region Datagrid Cell Click

        private void dgvSetProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 3)
                {
                    DataGridViewRow row = dgvSetProduct.Rows[e.RowIndex];
                    DialogResult comfirmYes = MessageBox.Show("Are you sure to delete?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DialogResult.Yes == comfirmYes)
                    {
                        dgvSetProduct.Rows.RemoveAt(row.Index);
                    }
                }
            }
        }

        #endregion

        #region Mouse Move

        private void frmSetProcedureProduct_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(cboProcedureProductName);
            tp.Hide(cboProductName);
            tp.Hide(txtQty);
        }
        #endregion

       
    }
}
