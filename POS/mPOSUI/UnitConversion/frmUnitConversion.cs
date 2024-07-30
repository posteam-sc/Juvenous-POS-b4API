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

namespace POS
{
    public partial class frmUnitConversion : Form
    {
        #region vairables
        string fromProductCode = null;
        int oldIndex = 0;
        int fpSize = 0;
        int toSize = 0;
        int totalSize = 0;
        int remainSize = 0;
        int removeSize = 0;
        #endregion

        public frmUnitConversion()
        {
            InitializeComponent();
        }

        #region Form Load
        private void frmUnitConversion_Load(object sender, EventArgs e)
        {
            BindFromProduct();
            InitialState();
        }
        #endregion

        #region Methods
        private void InitialState()
        {
            fpSize = 0;
            toSize = 0;
            totalSize = 0;
            remainSize = 0;
            removeSize = 0;
            
            lblFromProductSize.ResetText();
            txtFromProductQty.Clear();
            txtFromProductQty.Enabled = false;
            txtConvertQty.Clear();
            txtConvertQty.Enabled = false;
            txtRemainAmount.Clear();
            txtRemainAmount.Enabled = false;
            cboToProduct.Enabled = false;
            lbltoProductSize.ResetText();
            txtToProductQty.Clear();
            txtToProductQty.Enabled = false;
            txtPiecePerPack.Clear();
            txtPiecePerPack.Enabled = false;
            txtSellingPrice.Clear();
            txtSellingPrice.Enabled = false;
            btnAdd.Enabled = false;
            btnSubmit.Enabled = false;
            dgvUnitConversion.Rows.Clear();
            dgvUnitConversion.DataSource = null;
            
        }

        private void BindFromProduct()
        {
            POSEntities entity = new POSEntities();
            List<Product> products = new List<Product>();
            Product productObj = new Product();
            productObj.Id = 0;
            productObj.Name = "ALL";
            products.Add(productObj);
            products.AddRange(entity.Products.Where(x => (x.IsService == false || x.IsService == null) && x.IsPackage == false).ToList());
            cboFromProduct.DataSource = products;
            cboFromProduct.DisplayMember = "Name";
            cboFromProduct.ValueMember = "Id";
            cboFromProduct.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboFromProduct.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void BindToProduct()
        {
            cboToProduct.DataSource = null;
            POSEntities entity = new POSEntities();
            List<Product> products = new List<Product>();
            Product productObj = new Product();
            productObj.Id = 0;
            productObj.Name = "ALL";
            products.Add(productObj);
            products.AddRange(entity.Products.Where(x => (x.IsService == false || x.IsService == null) && x.IsPackage == false).ToList());
            if (!String.IsNullOrWhiteSpace(fromProductCode))
            {
                products.Remove(entity.Products.Where(x => x.ProductCode.Trim() == fromProductCode.Trim()).FirstOrDefault());
            }
            cboToProduct.DataSource = products;
            cboToProduct.DisplayMember = "Name";
            cboToProduct.ValueMember = "Id";
            cboToProduct.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboToProduct.AutoCompleteSource = AutoCompleteSource.ListItems;
        }
        #endregion

        #region Changed Events
        private void cboFromProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            POSEntities entity = new POSEntities();
            if (cboFromProduct.SelectedIndex != oldIndex)
            {
                InitialState();
            }
            if (cboFromProduct.SelectedIndex > 0)
            {
                int _productId = Convert.ToInt32(cboFromProduct.SelectedValue);
                var fromproduct = entity.Products.Where(p => p.Id == _productId).FirstOrDefault();

                txtFromProductQty.Text = fromproduct.Qty.ToString();
                try
                {
                    fpSize = Convert.ToInt32(fromproduct.Size);
                    lblUnit.Text = fromproduct.Unit.UnitName;
                }
                catch (Exception)
                {
                    MessageBox.Show("Please change From Product Size only Number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                lblFromProductSize.Text = fromproduct.Size + " " + fromproduct.Unit.UnitName;
                if (fromproduct.Qty > 0)
                {
                    txtConvertQty.Enabled = true;
                    cboToProduct.Enabled = true;
                    fromProductCode = fromproduct.ProductCode.Trim();
                    BindToProduct();
                    txtConvertQty.Focus();
                }
            }
            else
                cboToProduct.Enabled = false;
        }

        private void cboToProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            POSEntities entity = new POSEntities();
            if (cboToProduct.SelectedIndex > 0)
            {
                int _productId = Convert.ToInt32(cboToProduct.SelectedValue);
                var toproduct = entity.Products.Where(p => p.Id == _productId).FirstOrDefault();
                if (lblUnit.Text.Trim() == toproduct.Unit.UnitName.Trim())
                {
                    try
                    {
                        toSize = Convert.ToInt32(toproduct.Size);
                        lblUnit.Text = toproduct.Unit.UnitName;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Please change To Product Size only Number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    txtToProductQty.Text = toproduct.Qty.ToString();
                    lbltoProductSize.Text = toproduct.Size.ToString() + " " + toproduct.Unit.UnitName.ToString();
                    txtToProductQty.Text = toproduct.Qty.ToString();
                    txtPiecePerPack.Enabled = true;
                    txtSellingPrice.Enabled = true;
                    txtSellingPrice.Text = toproduct.Price.ToString();
                    txtPiecePerPack.Focus();
                }
                else
                {
                    MessageBox.Show("To Product Unit does not match Form Product Unit", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboToProduct.SelectedIndex = 0;
                    txtToProductQty.Clear();
                    txtPiecePerPack.Clear();
                    txtPiecePerPack.Enabled = false;
                    txtSellingPrice.Clear();
                    txtSellingPrice.Enabled = false;
                    btnAdd.Enabled = false;
                    return;
                }
            }
        }

        private void txtConvertQty_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtConvertQty.Text))
            {
                txtRemainAmount.Clear();
                totalSize = fpSize * Convert.ToInt32(txtConvertQty.Text);
                txtRemainAmount.Text = Convert.ToString(totalSize - remainSize);
                
                btnSubmit.Enabled = remainSize == totalSize ? true : false;
            }
            else
            {
                txtRemainAmount.Clear();
            }
        }

        private void txtPiecePerPack_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtPiecePerPack.Text))
            {
                removeSize = toSize * Convert.ToInt32(txtPiecePerPack.Text);
                btnAdd.Enabled = true;
            }
        }
        #endregion

        #region Click Events
        private void btnAdd_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvUnitConversion.Rows)
            {
                var productName = Convert.ToString(row.Cells[1].Value);
                if (cboToProduct.Text == productName)
                {
                    MessageBox.Show("This Product Is Already Defined", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (totalSize - (remainSize + removeSize) >= 0)
            {
                
                string[] row = new string[] { cboToProduct.SelectedValue.ToString(), cboToProduct.Text.ToString(), toSize.ToString(), lbltoProductSize.Text, txtPiecePerPack.Text.ToString(), txtSellingPrice.Text };
                dgvUnitConversion.Rows.Add(row);

                txtRemainAmount.Text = Convert.ToString(totalSize - (remainSize + removeSize));
                remainSize += removeSize;

                cboToProduct.SelectedIndex = 0;
                lbltoProductSize.ResetText();
                txtToProductQty.Clear();
                txtPiecePerPack.Clear();
                txtPiecePerPack.Enabled = false;
                txtSellingPrice.Clear();
                txtSellingPrice.Enabled = false;
                toSize = 0;
                btnAdd.Enabled = false;
                btnSubmit.Enabled = remainSize == totalSize ? true : false;
                cboToProduct.Focus();
            }
            else
            {
                MessageBox.Show("From Product Amount does not enough. Please add Convert Qty.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            POSEntities entity = new POSEntities();
            List<Stock_Transaction> productList = new List<Stock_Transaction>();
            Decimal totalPurchasePrice = 0;
            
            try
            {
                int fromProductId = Convert.ToInt32(cboFromProduct.SelectedValue);

                #region PurchaseDetail Qty Update
                List<PurchaseDetail> purchaseDetailList = (from pd in entity.PurchaseDetails
                                                           join mp in entity.MainPurchases on pd.MainPurchaseId equals mp.Id
                                                           where pd.ProductId == fromProductId
                                                           && pd.IsDeleted == false
                                                           && pd.CurrentQy != 0
                                                           && mp.IsCompletedInvoice == true
                                                           && mp.IsPurchase == true
                                                           orderby mp.UpdatedDate ascending
                                                           select pd).ToList();

                int convertQty = Convert.ToInt32(txtConvertQty.Text);
                
                foreach (PurchaseDetail pDetail in purchaseDetailList)
                {
                    //If convert Qty complete, Exist;
                    if (convertQty < 0)
                    {
                        break;
                    }

                    //For Purchase Detail Update
                    PurchaseDetail pDetailUpdate = entity.PurchaseDetails.Where(p => p.Id == pDetail.Id).FirstOrDefault();
                    if (pDetail.CurrentQy > convertQty)
                    {
                        totalPurchasePrice += (Decimal)pDetail.UnitPrice * Convert.ToDecimal(convertQty);
                        pDetailUpdate.CurrentQy = pDetail.CurrentQy - convertQty;
                        pDetailUpdate.ConvertQty = convertQty;
                        convertQty = 0;
                    }
                    else
                    {
                        totalPurchasePrice += (Decimal)pDetail.UnitPrice * Convert.ToDecimal(pDetail.CurrentQy);
                        convertQty -= (int)pDetail.CurrentQy;
                        pDetailUpdate.CurrentQy = 0;
                        pDetailUpdate.ConvertQty = pDetail.CurrentQy;
                    }
                    entity.Entry(pDetailUpdate).State = EntityState.Modified;
                }
                entity.SaveChanges();
                #endregion
                
                #region Insert UnitConversionHeader
                UnitConversionHeader header = new UnitConversionHeader();
                header.FromProductId = fromProductId;
                header.FromQty = Convert.ToInt32(txtConvertQty.Text);
                header.CreatedBy = MemberShip.UserId;
                header.CreatedDate = DateTime.Now;
                entity.UnitConversionHeaders.Add(header);
                entity.SaveChanges();

                int headerId = header.Id;
                #endregion

                #region Update From Product Balance
                var fromProduct = (from p in entity.Products where p.Id == fromProductId select p).FirstOrDefault();
                fromProduct.Qty -= Convert.ToInt32(txtConvertQty.Text);
                entity.Entry(fromProduct).State = EntityState.Modified;
                entity.SaveChanges();
                #endregion

                #region Saving Conversion Stock Out in Stock Transaction table
                Stock_Transaction stFromProduct = new Stock_Transaction();
                stFromProduct.ProductId = fromProductId;
                stFromProduct.ConversionStockOut = Convert.ToInt32(txtConvertQty.Text);
                productList.Add(stFromProduct);

                Utility.Conversion_Run_Process(DateTime.Now.Year, DateTime.Now.Month, productList, false);
                productList.Clear();
                #endregion

                foreach (DataGridViewRow row in dgvUnitConversion.Rows)
                {
                    int toproductId = Convert.ToInt32(row.Cells[0].Value);

                    #region Insert To Product Purchase
                    Decimal totalToProductPurchase = 0;
                    Decimal toProductUnitPrice = 0;

                    /***** //MKK
                     Total To Product Purchase Price = (Total From Product Purchase Price * Total To Product Size) / Total From Product Convert Size
                     -------------------------------------------------------------------------------------------------------------------------------

                     // Total From Product Purchase Price ==> totalPurchasePrice
                     // Total To Product Size             ==> To Product Qty * To Product Size
                     // Total From Product Convert Size   ==> From Product Size * Convert Qty
                     ****/

                    /*                    Total From Product Purchase Price |     To Product Qty        |             To Product Size           |From Product Size |         Convert Qty       */
                    totalToProductPurchase = (totalPurchasePrice * (Convert.ToInt32(row.Cells[4].Value) * Convert.ToInt32(row.Cells[2].Value))) / (fpSize * Convert.ToInt32(txtConvertQty.Text));
                    toProductUnitPrice = totalToProductPurchase / Convert.ToInt32(row.Cells[4].Value);

                    //MainPurchase
                    MainPurchase mainPurchase = new MainPurchase();
                    mainPurchase.Date = DateTime.Now;
                    mainPurchase.TotalAmount = totalToProductPurchase;
                    mainPurchase.Cash = totalToProductPurchase;
                    mainPurchase.OldCreditAmount = 0;
                    mainPurchase.SettlementAmount = 0;
                    mainPurchase.IsActive = true;
                    mainPurchase.DiscountAmount = 0;
                    mainPurchase.IsDeleted = false;
                    mainPurchase.CreatedDate = DateTime.Now;
                    mainPurchase.CreatedBy = MemberShip.UserId;
                    mainPurchase.IsCompletedInvoice = true;
                    mainPurchase.IsCompletedPaid = true;
                    mainPurchase.IsPurchase = false;
                    entity.MainPurchases.Add(mainPurchase);
                    entity.SaveChanges();

                    //Purchase Details
                    PurchaseDetail purchaseDetail = new PurchaseDetail();
                    purchaseDetail.ProductId = toproductId;
                    purchaseDetail.Qty = Convert.ToInt32(row.Cells[4].Value);
                    purchaseDetail.CurrentQy = Convert.ToInt32(row.Cells[4].Value);
                    purchaseDetail.UnitPrice = toProductUnitPrice;
                    purchaseDetail.IsDeleted = false;
                    purchaseDetail.Date = DateTime.Now;
                    purchaseDetail.ConvertQty = 0;
                    mainPurchase.PurchaseDetails.Add(purchaseDetail);
                    entity.SaveChanges();
                    #endregion

                    #region Insert UnitConversionDetails
                    UnitConversionDetail details = new UnitConversionDetail();
                    details.UnitConversionHeaderId = headerId;
                    details.FromProductId = fromProductId;
                    details.ToProductId = toproductId;
                    details.ToQty = Convert.ToInt32(row.Cells[4].Value);
                    entity.UnitConversionDetails.Add(details);
                    entity.SaveChanges();
                    #endregion

                    #region Update To Product Balance
                    var toProduct = (from p in entity.Products where p.Id == toproductId select p).FirstOrDefault();
                    toProduct.Qty += Convert.ToInt32(row.Cells[4].Value);
                    entity.Entry(toProduct).State = EntityState.Modified;
                    entity.SaveChanges();
                    #endregion

                    #region Saving Conversion Stock In in Stock Transaction table
                    Stock_Transaction stToProduct = new Stock_Transaction();
                    stToProduct.ProductId = toproductId;
                    stToProduct.ConversionStockIn = Convert.ToInt32(row.Cells[4].Value);
                    productList.Add(stToProduct);

                    Utility.Conversion_Run_Process(DateTime.Now.Year, DateTime.Now.Month, productList, true);
                    productList.Clear();
                    #endregion

                    #region ToProduct Price
                    if (toProduct.Price != Convert.ToDecimal(row.Cells[5].Value))
                    {
                        //Product Price Change
                        ProductPriceChange pc = new ProductPriceChange();
                        pc.ProductId = toproductId;
                        pc.OldPrice = toProduct.Price;
                        pc.Price = Convert.ToDecimal(row.Cells[5].Value);
                        pc.UserID = MemberShip.UserId;
                        pc.UpdateDate = DateTime.Now;
                        entity.ProductPriceChanges.Add(pc);

                        //Product Price Update
                        toProduct.Price = Convert.ToDecimal(row.Cells[5].Value);

                        entity.SaveChanges();
                    }
                    #endregion
                }
                MessageBox.Show("Successfully Save", "mPOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                InitialState();
                cboFromProduct.SelectedIndex = 0;
                oldIndex = 0;
                cboFromProduct.Focus();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            InitialState();
            cboFromProduct.SelectedIndex = 0;
            oldIndex = 0;
            cboFromProduct.Focus();
        }

        private void dgvUnitConversion_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 6)
                {
                    DataGridViewRow row = dgvUnitConversion.Rows[e.RowIndex];
                    DialogResult comfirm = MessageBox.Show("Are you sure to delete?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DialogResult.Yes == comfirm)
                    {
                        int dgvremove = Convert.ToInt32(dgvUnitConversion.Rows[e.RowIndex].Cells[2].Value) * Convert.ToInt32(dgvUnitConversion.Rows[e.RowIndex].Cells[4].Value);
                        remainSize -= dgvremove;
                        txtRemainAmount.Text = Convert.ToString(totalSize - remainSize);

                        dgvUnitConversion.Rows.RemoveAt(row.Index);
                        btnSubmit.Enabled = remainSize == totalSize ? true : false;
                        cboToProduct.Focus();
                    }
                }
            }
        }
        #endregion

        #region KeyPress
        private void txtConvertQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
            // checks to make sure only 1 decimal is allowed
            if (e.KeyChar == 46)
            {
                if ((sender as TextBox).Text.IndexOf(e.KeyChar) != -1)
                    e.Handled = true;
            }
        }

        private void txtPiecePerPack_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
            // checks to make sure only 1 decimal is allowed
            if (e.KeyChar == 46)
            {
                if ((sender as TextBox).Text.IndexOf(e.KeyChar) != -1)
                    e.Handled = true;
            }
        }

        private void txtSellingPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
            // checks to make sure only 1 decimal is allowed
            if (e.KeyChar == 46)
            {
                if ((sender as TextBox).Text.IndexOf(e.KeyChar) != -1)
                    e.Handled = true;
            }
        }
        #endregion

        #region KeyDown
        private void txtConvertQty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }

        private void txtPiecePerPack_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }

        private void txtSellingPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }
        #endregion
    }
}
