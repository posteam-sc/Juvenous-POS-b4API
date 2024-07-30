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
    public partial class PurchaseInput : Form
    {
        #region Variable

        POSEntities entity = new POSEntities();
        List<PurchaseProductController> PurchaseProductList = new List<PurchaseProductController>();
        //List<Product> PurchaseProductList = new List<Product>();

        private ToolTip tp = new ToolTip();
        private decimal totalmount = 0;
        private decimal paidamount = 0;
        private int DiscountAmount = 0;
        // Int64 actualAmount = 0;
        //  bool Isfirst = false;
        Int64 CreditAmount = 0;
        public int mainPurchaseId;
        public int CrAmt = 0;

        List<int> purDetailIdList = new List<int>();
        #endregion

        #region Event

        private void txtCashAmount_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtCashAmount.Text))
            {
                decimal cashAmount = Convert.ToDecimal(txtCashAmount.Text.Trim());
                txtCashAmount.Text = cashAmount.ToString("#,0.00");
            }
            Calcuate_CreditAmount();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((txtDiscount.Focused) && (keyData == (Keys.Enter) || keyData == (Keys.Tab)))
            {
                AssignZeroInEmptyValue();
                if (Convert.ToDecimal(txtTotalAmount.Text) > 0)
                {
                    if (Convert.ToInt32(txtDiscount.Text) >= Convert.ToInt32(txtTotalAmount.Text))
                    {
                        if (dgvProductList.Rows.Count > 0)
                        {
                            MessageBox.Show("Discount Amount should not be greather than or equal Total Amount!", "Information");
                            txtDiscount.Focus();
                        }
                        else
                        {
                            if (Convert.ToInt32(txtDiscount.Text) == 0)
                            {
                                Calcuate_PayableAmount();
                                Calcuate_CreditAmount();
                                txtCashAmount.Focus();
                            }
                        }
                    }
                    else
                    {
                        Calcuate_PayableAmount();
                        Calcuate_CreditAmount();
                        txtCashAmount.Focus();
                    }
                }
                return true;
            }

            else if ((txtCashAmount.Focused) && (keyData == (Keys.Enter) || keyData == (Keys.Tab)))
            {
                Calcuate_CreditAmount();
                btnSave.Focus();
                return true;
            }

            else if (cboProductName.Focused && keyData == Keys.Tab)
            {
                if (Utility.Product_Combo_Control(cboProductName))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

            else if ((txtBarcode.Focused) && (keyData == (Keys.Enter) || keyData == (Keys.Tab)))
            {
                if (txtBarcode.Text.Trim() != string.Empty)
                {
                    Barcode_Input();
                    return true;
                }

            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public PurchaseInput()
        {
            InitializeComponent();
            CenterToScreen();
        }

        private void PurchaseDetail_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);

            dgvProductList.AutoGenerateColumns = false;
            PurchaseProductList.Clear();
            List<Product> productList = new List<Product>();
            Product productObj1 = new Product();
            productObj1.Id = 0;
            productObj1.Name = "Select";
            productList.Add(productObj1);

            var _productList = (from p in entity.Products where p.IsConsignment == false && (p.IsService == false || p.IsService == null) && p.IsPackage == false select p).ToList();
            productList.AddRange(_productList);
            cboProductName.DataSource = productList;
            cboProductName.DisplayMember = "Name";
            cboProductName.ValueMember = "Id";
            cboProductName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboProductName.AutoCompleteSource = AutoCompleteSource.ListItems;

            List<Supplier> supplierList = new List<Supplier>();
            Supplier supplierObj = new Supplier();
            supplierObj.Id = 0;
            supplierObj.Name = "Select";
            supplierList.Add(supplierObj);
            supplierList.AddRange(entity.Suppliers.ToList());
            cboSupplierName.DataSource = supplierList;
            cboSupplierName.DisplayMember = "Name";
            cboSupplierName.ValueMember = "Id";
            cboSupplierName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboSupplierName.AutoCompleteSource = AutoCompleteSource.ListItems;

            //for edit case   *SD*
            if (mainPurchaseId != 0)
            {
                txtVoucherNo.Enabled = false;
                txtCashAmount.Clear();
                dgvProductList.AutoGenerateColumns = false;
                MainPurchase currentMP = (from mp in entity.MainPurchases where mp.Id == mainPurchaseId select mp).FirstOrDefault();
                if (currentMP != null)
                {
                    dtDate.Text = currentMP.Date.ToString();
                    txtVoucherNo.Text = (currentMP.VoucherNo == null) ? "-" : currentMP.VoucherNo;
                    cboSupplierName.Text = (currentMP.Supplier == null) ? "-" : currentMP.Supplier.Name;
                    cboSupplierName.Enabled = false;
                    //  txtOldCredit.Text = currentMP.OldCreditAmount.ToString();
                    txtDiscount.Text = currentMP.DiscountAmount.ToString();
                    txtTotalAmount.Text = currentMP.TotalAmount.ToString();
                    txtCashAmount.Text = currentMP.Cash.ToString();
                    var editItemData = (from pd in entity.PurchaseDetails where pd.MainPurchaseId == mainPurchaseId && pd.IsDeleted == false select pd).ToList();

                    PurchaseProductList.AddRange(editItemData.Select(_product =>
                      new PurchaseProductController
                      {
                          PurchaseDetailId = Convert.ToInt32(_product.Id.ToString()),
                          ProductId = Convert.ToInt32(_product.Product.Id.ToString()),
                          Barcode = _product.Product.Barcode.ToString(),
                          ProductName = _product.Product.Name.ToString(),
                          Qty = Convert.ToInt32(_product.Qty),
                          ExpireDate = _product.expiredDate == null ? (DateTime?)null : _product.expiredDate.Value.Date,
                          PurchasePrice = Convert.ToDecimal(_product.UnitPrice.ToString()),
                          Total = (decimal)_product.Qty * (decimal)_product.UnitPrice

                      }
                            )
                    );
                }
                dgvProductList.DataSource = PurchaseProductList;
                Calcuate_PayableAmount();
                Calcuate_CreditAmount();
                btnCancel.Enabled = false;
                btnSave.Image = POS.Properties.Resources.update_big;
            }
        }

        private void txtNO_KeyPress(object sender, KeyPressEventArgs e)
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //mainPurchaseId = 0;
            Product productObj = new Product();
            Boolean hasError = false;

            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            //Validation
            if (cboProductName.SelectedIndex == 0)
            {
                tp.SetToolTip(cboProductName, "Error");
                tp.Show("Please fill up product name!", cboProductName);
                hasError = true;
            }
            else if (Utility.Product_Combo_Control(cboProductName))
            {
                return;
            }
            else if (txtQty.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtQty, "Error");
                tp.Show("Please fill up product quantity!", txtQty);
                hasError = true;
            }
            else if (txtUnitPrice.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtUnitPrice, "Error");
                tp.Show("Please fill up product unit price!", txtUnitPrice);
                hasError = true;
            }


            if (!hasError)
            {
                bool isAdd = false;
                dgvProductList.DataSource = "";
                int productId = Convert.ToInt32(cboProductName.SelectedValue);
                productObj = (from p in entity.Products where p.Id == productId select p).FirstOrDefault();
                string tempBarcode = "";
                string tempBarcode1 = "";
                if (productObj != null)
                {
                    PurchaseProductController newpc = new PurchaseProductController();

                    newpc.Barcode = productObj.Barcode;
                    newpc.ProductName = productObj.Name;
                    newpc.Qty = Convert.ToInt32(txtQty.Text.ToString());
                    newpc.PurchasePrice = Convert.ToDecimal(txtUnitPrice.Text);
                    newpc.Total = Convert.ToDecimal(newpc.Qty * newpc.PurchasePrice);
                    newpc.ProductId = productObj.Id;
                    newpc.ExpireDate = dpkExpire.Checked ? dpkExpire.Value.Date : (DateTime?)null;


                    int count = PurchaseProductList.Count;

                    //if (mainPurchaseId == 0)
                    //{
                    //    count =dgvProductList.RowCount;
                    //}

                    if (count > 0)
                    {
                        for (int i = count - 1; i >= 0; i--)
                        {
                            PurchaseProductController tempProduct1 = PurchaseProductList[i];
                            decimal tempPurchasePrice1 = tempProduct1.PurchasePrice;
                            tempBarcode1 = Convert.ToString(tempProduct1.Barcode);
                            DateTime? tempExpireDate = tempProduct1.ExpireDate ?? tempProduct1.ExpireDate;

                            if (tempProduct1.Barcode == newpc.Barcode && tempProduct1.PurchasePrice == newpc.PurchasePrice && tempExpireDate.Equals(newpc.ExpireDate))
                            {
                                tempProduct1.Qty += newpc.Qty;

                                tempProduct1.PurchaseDetailId = 0;
                                tempProduct1.ProductId = newpc.ProductId;
                                tempProduct1.ProductName = newpc.ProductName;
                                tempProduct1.Barcode = newpc.Barcode;
                                tempProduct1.PurchasePrice = newpc.PurchasePrice;
                                tempProduct1.Total += newpc.Total;
                                tempProduct1.ExpireDate = newpc.ExpireDate;
                                PurchaseProductList.RemoveAt(i);//remove second product

                                PurchaseProductList.Insert(i, tempProduct1);

                                isAdd = true;
                            }
                        }


                    }
                    if (isAdd == false)
                    {
                        PurchaseProductList.Add(newpc);
                    }
                }

                dgvProductList.DataSource = PurchaseProductList;
                if (mainPurchaseId != 0)
                {
                    totalmount = Convert.ToDecimal(txtTotalAmount.Text);
                }
                totalmount += Convert.ToInt32(txtQty.Text.ToString()) * Convert.ToDecimal(txtUnitPrice.Text.ToString());
                txtTotalAmount.Text = totalmount.ToString("#,0.00");

                Calcuate_PayableAmount();
                Calcuate_CreditAmount();
                txtQty.Text = "";
                txtUnitPrice.Text = "";
                cboProductName.SelectedIndex = 0;
                dpkExpire.Checked = false;
                txtBarcode.Clear();
                cboProductName.Focus();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear_Data();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            MainPurchase mainPurchaseObj = new MainPurchase();
            entity = new POSEntities();
            Boolean hasError = false;

            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";

            string vNo = txtVoucherNo.Text.ToString();
            int spId = Convert.ToInt32(cboSupplierName.SelectedValue);
            APP_Data.MainPurchase mainp = entity.MainPurchases.Where(x => x.VoucherNo == vNo && x.SupplierId == spId && x.IsDeleted == false).FirstOrDefault();

            if (txtVoucherNo.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtVoucherNo, "Error");
                tp.Show("Please fill up Voucher No. !", txtVoucherNo);
                hasError = true;
            }
            else if (Utility.Supplier_Combo_Control(cboSupplierName))
            {
                return;
            }
            else if (cboSupplierName.SelectedIndex == 0 || cboSupplierName.SelectedIndex == -1)
            {
                tp.SetToolTip(cboSupplierName, "Error");
                tp.Show("Please fill up supplier name!", cboSupplierName);
                hasError = true;
            }

            else if (PurchaseProductList.Count == 0)
            {
                MessageBox.Show("Please fill up product list for purchase voucher by clicking Add Button!", "Information");
                return;
            }

            else if (txtTotalAmount.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtTotalAmount, "Error");
                tp.Show("Please fill up total amount!", txtTotalAmount);
                hasError = true;
            }
            else if (mainp != null)
            {
                if (mainPurchaseId == 0)
                {
                    tp.SetToolTip(txtVoucherNo, "Error");
                    tp.Show("Duplicate Voucher No. !", txtVoucherNo);
                    hasError = true;
                }
            }


            AssignZeroInEmptyValue();
            if (Convert.ToDecimal(txtTotalAmount.Text) > 0)
            {

                if (txtDiscount.Text.Trim() != string.Empty)
                {

                    if (Convert.ToDecimal(txtDiscount.Text) >= Convert.ToDecimal(txtTotalAmount.Text))
                    {
                        if (dgvProductList.Rows.Count > 0)
                        {
                            MessageBox.Show("Discount Amount should not be greather than or equal Total Amount!", "Information");
                            txtDiscount.Focus();
                            return;
                        }
                        else
                        {
                            if (Convert.ToDecimal(txtDiscount.Text) == 0)
                            {
                                Calcuate_PayableAmount();
                                Calcuate_CreditAmount();
                                txtCashAmount.Focus();
                            }
                        }
                    }
                }
            }

            if (txtCashAmount.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtCashAmount, "Error");
                tp.Show("Please fill up cash amount!", txtCashAmount);
                hasError = true;
            }

            else if (txtCashAmount.Text.Trim() != string.Empty)
            {

                if (Convert.ToDecimal(txtCashAmount.Text) > Convert.ToDecimal(txtTotalPayableAmount.Text))
                {
                    MessageBox.Show("Your Paid Amount is more than Total Payable Amount!", "Error");
                    return;
                }
            }



            if (!hasError)
            {
                //Other purchase voucher's isActive change false for supplier
                if (mainPurchaseId != 0)
                {
                    mainPurchaseObj = (from c in entity.MainPurchases where c.Id == mainPurchaseId select c).FirstOrDefault<MainPurchase>();
                }
                int supplierId = Convert.ToInt32(cboSupplierName.SelectedValue);

                mainPurchaseObj.Date = dtDate.Value;
                mainPurchaseObj.VoucherNo = txtVoucherNo.Text;
                mainPurchaseObj.SupplierId = Convert.ToInt32(cboSupplierName.SelectedValue);
                mainPurchaseObj.TotalAmount = Convert.ToDecimal(txtTotalAmount.Text);
                mainPurchaseObj.Cash = Convert.ToDecimal(txtCashAmount.Text);

                mainPurchaseObj.SettlementAmount = 0;
                mainPurchaseObj.IsPurchase = true;


                mainPurchaseObj.OldCreditAmount = Convert.ToDecimal(txtCreditAmt.Text);
                mainPurchaseObj.DiscountAmount = (txtDiscount.Text == "") ? 0 : Convert.ToInt32(txtDiscount.Text);
                DiscountAmount = (txtDiscount.Text == "") ? 0 : Convert.ToInt32(txtDiscount.Text);
                mainPurchaseObj.IsActive = true;

                mainPurchaseObj.CreatedDate = DateTime.Now;
                mainPurchaseObj.CreatedBy = MemberShip.UserId;

                mainPurchaseObj.IsDeleted = false;

                if (Convert.ToDecimal(txtTotalPayableAmount.Text) == Convert.ToDecimal(txtCashAmount.Text))
                {
                    mainPurchaseObj.IsCompletedPaid = true;
                }
                else
                {
                    mainPurchaseObj.IsCompletedPaid = false;
                }
                mainPurchaseObj.IsCompletedInvoice = false;

                //for edit
                if (mainPurchaseId != 0)
                {
                    int _supplierId = Convert.ToInt32(cboSupplierName.SelectedValue);

                    MainPurchase _mPar = entity.MainPurchases.Where(x => x.Id == mainPurchaseId).FirstOrDefault();


                    _mPar.OldCreditAmount = mainPurchaseObj.OldCreditAmount;

                    mainPurchaseObj.UpdatedBy = MemberShip.UserId;
                    mainPurchaseObj.UpdatedDate = DateTime.Now;
                    entity.Entry(_mPar).State = EntityState.Modified;
                    entity.SaveChanges();

                    List<PurchaseDetail> proObj = entity.PurchaseDetails.Where(x => x.MainPurchaseId == mainPurchaseId).ToList();

                    foreach (var pro in proObj)
                    {
                        APP_Data.PurchaseDetail purDetail = entity.PurchaseDetails.Where(x => x.Id == pro.Id).FirstOrDefault();


                        purDetail.IsDeleted = true;
                        purDetail.DeletedDate = DateTime.Now;
                        purDetail.DeletedUser = MemberShip.UserId;
                        purDetail.MainPurchaseId = mainPurchaseId;
                        purDetail.Id = pro.Id;
                        entity.Entry(purDetail).State = EntityState.Modified;
                        entity.SaveChanges();

                    }

                    for (int i = 0; i < purDetailIdList.Count; i++)
                    {
                        //Delete Log for partial  Delete log to main Deletelog 
                        var realpd = entity.PurchaseDetails.Find(purDetailIdList[i]);
                        if (realpd != null)
                        {
                            APP_Data.PurchaseDeleteLog delObj = new APP_Data.PurchaseDeleteLog();
                            delObj.CounterId = MemberShip.CounterId;
                            delObj.UserId = MemberShip.UserId;
                            delObj.MainPurchaseId = mainPurchaseId;
                            delObj.PurchaseDetailId = purDetailIdList[i];
                            delObj.DeletedDate = DateTime.Now;
                            delObj.IsParent = false;
                            delObj.VoucherNo = txtVoucherNo.Text;
                            entity.PurchaseDeleteLogs.Add(delObj);

                            entity.SaveChanges();
                        }

                    }


                    foreach (PurchaseProductController p in PurchaseProductList)
                    {
                        if (p.PurchaseDetailId == 0)
                        {
                            APP_Data.PurchaseDetail purchaseDetailObj = new APP_Data.PurchaseDetail();

                            purchaseDetailObj.ProductId = p.ProductId;
                            purchaseDetailObj.Qty = p.Qty;
                            purchaseDetailObj.CurrentQy = p.Qty;
                            purchaseDetailObj.UnitPrice = Convert.ToDecimal(p.PurchasePrice);
                            purchaseDetailObj.IsDeleted = false;
                            purchaseDetailObj.Date = DateTime.Now;
                            purchaseDetailObj.MainPurchaseId = mainPurchaseId;
                            purchaseDetailObj.expiredDate = p.ExpireDate;
                            purchaseDetailObj.ConvertQty = 0;
                            entity.PurchaseDetails.Add(purchaseDetailObj);
                            entity.SaveChanges();
                        }
                        else
                        {
                            APP_Data.PurchaseDetail purDetail = entity.PurchaseDetails.Where(x => x.Id == p.PurchaseDetailId).FirstOrDefault();
                            purDetail.IsDeleted = false;
                            purDetail.DeletedDate = null;
                            purDetail.DeletedUser = null;

                            entity.Entry(purDetail).State = EntityState.Modified;
                            entity.SaveChanges();
                        }
                    }

                    entity.Entry(mainPurchaseObj).State = EntityState.Modified;

                    entity.SaveChanges();

                    if (System.Windows.Forms.Application.OpenForms["PurchaseListBySupplier"] != null)
                    {
                        PurchaseListBySupplier newForm = (PurchaseListBySupplier)System.Windows.Forms.Application.OpenForms["PurchaseListBySupplier"];
                        newForm.DataBind();
                    }
                }
                //for save
                else
                {
                    //add purchase detail
                    foreach (PurchaseProductController p in PurchaseProductList)
                    {
                        APP_Data.PurchaseDetail purchaseDetailObj = new APP_Data.PurchaseDetail();

                        purchaseDetailObj.ProductId = p.ProductId;
                        purchaseDetailObj.Qty = p.Qty;
                        purchaseDetailObj.CurrentQy = p.Qty;
                        purchaseDetailObj.UnitPrice = Convert.ToDecimal(p.PurchasePrice);
                        purchaseDetailObj.IsDeleted = false;
                        purchaseDetailObj.Date = DateTime.Now;
                        purchaseDetailObj.ConvertQty = 0;
                        purchaseDetailObj.expiredDate = p.ExpireDate;
                        mainPurchaseObj.PurchaseDetails.Add(purchaseDetailObj);

                    }

                    entity.MainPurchases.Add(mainPurchaseObj);

                    entity.SaveChanges();

                }

                MessageBox.Show("Successfully Saved!", "Save");
                CreditAmount = 0;
                //this.Dispose();
                Clear_Data();
            }
            if (System.Windows.Forms.Application.OpenForms["chart"] != null)
            {
                chart newForm = (chart)System.Windows.Forms.Application.OpenForms["chart"];
                newForm.FormFresh();
            }
        }

        private void cboSupplierName_SelectedValueChanged(object sender, EventArgs e)
        {
            #region oldcode
            //if (cboSupplierName.SelectedIndex > 0)
            //{
            //    int supplierId = (int)cboSupplierName.SelectedValue;


            //    if (supplierId != 1)
            //    {
            //        MainPurchase mainPurchaseObj = (from mp in entity.MainPurchases where mp.IsActive == true && mp.SupplierId == supplierId && mp.IsDeleted==false select mp).FirstOrDefault();
            //        if (mainPurchaseObj != null)
            //        {
            //           // txtOldCredit.Text = (((mainPurchaseObj.TotalAmount - mainPurchaseObj.DiscountAmount) + mainPurchaseObj.OldCreditAmount) - mainPurchaseObj.Cash).ToString();
            //            txtOldCredit.Text = mainPurchaseObj.OldCreditAmount.ToString();
            //        }
            //        else
            //        {
            //            txtOldCredit.Text = "0";
            //            Isfirst = true;

            //        }
            //    }
            //    else
            //    {
            //        txtOldCredit.Text = "0";
            //    }

            //}

            #endregion

            if (cboSupplierName.SelectedIndex > 0)
            {
                int supplierId = Convert.ToInt32(cboSupplierName.SelectedValue);

                CreditAmount = 0;
                List<MainPurchase> mainPurchaseObj = new List<MainPurchase>();

                mainPurchaseObj = (from mp in entity.MainPurchases where mp.IsActive == true && mp.SupplierId == supplierId && mp.IsDeleted == false && ((mainPurchaseId != 0 && mp.Id != mainPurchaseId) || (mainPurchaseId == 0 && 1 == 1)) select mp).ToList();
                //MainPurchase mainPurchaseObj = (from mp in entity.MainPurchases where mp.IsActive == true && mp.SupplierId == supplierId && mp.IsDeleted == false orderby mp.Id descending select mp).FirstOrDefault();

                if (mainPurchaseObj != null)
                {
                    CreditAmount = Convert.ToInt64(mainPurchaseObj.Sum(x => x.OldCreditAmount));
                    // CreditAmount = Convert.ToInt64(mainPurchaseObj.OldCreditAmount);

                }
                txtOldCredit.Text = CreditAmount.ToString();
                // Calcuate_PayableAmount();
            }
        }

        //add new one by ZMH
        private void btnAddSupplier_Click(object sender, EventArgs e)
        {
            NewSupplier newform = new NewSupplier();
            newform.ShowDialog();
        }

        //add new one by ZMH

        private void btnAddNewProduct_Click(object sender, EventArgs e)
        {
            NewProduct newform = new NewProduct();
            newform.ShowDialog();
        }
        //add new one by ZMH


        private void dgvProductList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvProductList.Rows)
            {
                //if (mainPurchaseId == 0)
                //{
                PurchaseProductController p = (PurchaseProductController)row.DataBoundItem;
                row.Cells[0].Value = Convert.ToInt64(p.PurchaseDetailId);
                row.Cells[1].Value = Convert.ToInt64(p.ProductId);
                row.Cells[2].Value = p.Barcode.ToString();
                row.Cells[3].Value = p.ProductName.ToString();
                row.Cells[4].Value = Convert.ToInt32(p.Qty);
                row.Cells[5].Value = Convert.ToDecimal(p.PurchasePrice);
                row.Cells[6].Value = p.ExpireDate == null ? "" : p.ExpireDate.Value.Date.ToString("dd-MMM-yyyy");
                row.Cells[7].Value = Convert.ToDecimal(p.Total);

            }
        }

        private void dgvProductList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 8)
                {
                    int index = e.RowIndex;

                    //temp Delete purDetailId in edit mode   *SD*
                    int purDetailId = Convert.ToInt32(dgvProductList[0, e.RowIndex].Value);
                    purDetailIdList.Add(purDetailId);
                    //**//
                    PurchaseProductController pdo = PurchaseProductList[index];
                    PurchaseProductList.RemoveAt(index);
                    dgvProductList.DataSource = PurchaseProductList.ToList();
                    txtTotalAmount.Text = "";
                    totalmount = 0;
                    paidamount = 0;

                    totalmount = Convert.ToInt32(PurchaseProductList.Sum(x => x.Qty * x.PurchasePrice));
                    txtTotalAmount.Text = totalmount.ToString();
                    Calcuate_PayableAmount();
                    Calcuate_CreditAmount();


                }
            }
        }

        private void PurchaseInput_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(txtVoucherNo);
            tp.Hide(cboSupplierName);
            tp.Hide(txtCashAmount);
            tp.Hide(txtTotalAmount);
            tp.Hide(cboProductName);


        }

        private void txtDiscount_Leave(object sender, EventArgs e)
        {
            AssignZeroInEmptyValue();
            if (Convert.ToDecimal(txtTotalAmount.Text) > 0)
            {
                if (Convert.ToDecimal(txtDiscount.Text) >= Convert.ToDecimal(txtTotalAmount.Text))
                {
                    if (dgvProductList.Rows.Count > 0)
                    {
                        MessageBox.Show("Discount Amount should not be greather than or equal Total Amount!", "Information");
                        txtDiscount.Focus();
                    }
                    else
                    {
                        if (Convert.ToInt32(txtDiscount.Text) == 0)
                        {
                            Calcuate_PayableAmount();
                            Calcuate_CreditAmount();
                            txtCashAmount.Focus();
                        }
                    }
                }

                else
                {
                    Calcuate_PayableAmount();
                    Calcuate_CreditAmount();
                    txtCashAmount.Focus();
                }
            }

        }

        private void cboProductName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProductName.SelectedIndex > 0)
            {
                long productId = Convert.ToInt32(cboProductName.SelectedValue);
                string barcode = (from p in entity.Products where p.Id == productId select p.Barcode).FirstOrDefault();
                txtBarcode.Text = barcode;
            }
        }

        private void txtBarcode_Leave(object sender, EventArgs e)
        {
            if (txtBarcode.Text.Trim() != string.Empty)
            {
                Barcode_Input();
            }
        }


        #endregion

        #region Method
        private void Clear_Data()
        {
            totalmount = 0;
            txtCashAmount.Text = "";
            txtOldCredit.Text = "";
            txtQty.Text = "";
            txtTotalAmount.Text = "";
            txtUnitPrice.Text = "";
            txtVoucherNo.Text = "";
            txtDiscount.Text = "0";
            cboSupplierName.SelectedIndex = 0;
            cboProductName.SelectedIndex = 0;
            PurchaseProductList.Clear();
            dgvProductList.DataSource = "";
            txtTotalPayableAmount.Clear();
            txtCreditAmt.Clear();
            dpkExpire.Checked = false;
            dpkExpire.Value = DateTime.Now;
        }

        private void Barcode_Input()
        {
            string _barcode = txtBarcode.Text;
            long productId = (from p in entity.Products where p.Barcode == _barcode && p.IsConsignment == false select p.Id).FirstOrDefault();
            cboProductName.SelectedValue = productId;
            cboProductName.Focus();
        }


        private void AssignZeroInEmptyValue()
        {
            if (txtTotalAmount.Text == String.Empty)
            {
                txtTotalAmount.Text = "0";
            }

            if (txtDiscount.Text == string.Empty)
            {
                txtDiscount.Text = "0";
            }
        }


        public void SetCurrentProduct(long productId)
        {
            APP_Data.Product productList = (from e in entity.Products where e.Id == productId select e).FirstOrDefault();
            if (productList != null)
            {
                cboProductName.SelectedValue = productList.Id;
            }
        }

        public void SetCurrentSupplier(long supplierId)
        {
            APP_Data.Supplier supplierList = (from s in entity.Suppliers where s.Id == supplierId select s).FirstOrDefault();
            if (supplierList != null)
            {
                cboSupplierName.SelectedValue = supplierList.Id;
            }
        }

        public void Reload()
        {
            List<Supplier> supplierList = new List<Supplier>();
            Supplier supplierObj = new Supplier();
            supplierObj.Id = 0;
            supplierObj.Name = "Select";
            supplierList.Add(supplierObj);
            supplierList.AddRange(entity.Suppliers.ToList());
            cboSupplierName.DataSource = supplierList;
            cboSupplierName.DisplayMember = "Name";
            cboSupplierName.ValueMember = "Id";
            cboSupplierName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboSupplierName.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        public void Clear()
        {
            dgvProductList.AutoGenerateColumns = false;
            PurchaseProductList.Clear();
            ReloadProduct();
        }

        public void ReloadProduct()
        {
            List<Product> productList = new List<Product>();
            Product productObj1 = new Product();
            productObj1.Id = 0;
            productObj1.Name = "Select";
            productList.Add(productObj1);
            //productList.AddRange(entity.Products.ToList());
            var _product = (from a in entity.Products where a.IsConsignment == false && (a.IsService == false || a.IsService == null) && a.IsPackage == false select a).ToList(); ;
            productList.AddRange(_product);
            cboProductName.DataSource = productList;
            cboProductName.DisplayMember = "Name";
            cboProductName.ValueMember = "Id";
            cboProductName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboProductName.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void Calcuate_CreditAmount()
        {

            if (txtCashAmount.Text != string.Empty)
            {
                txtCreditAmt.Text = (Convert.ToDecimal(txtTotalPayableAmount.Text) - Convert.ToDecimal(txtCashAmount.Text)).ToString("#,0.00");
            }
        }

        private void Calcuate_PayableAmount()
        {
            AssignZeroInEmptyValue();

            if (txtTotalAmount.Text != string.Empty && txtDiscount.Text != string.Empty)
            {
                txtTotalPayableAmount.Text = (Convert.ToDecimal(txtTotalAmount.Text) - Convert.ToDecimal(txtDiscount.Text)).ToString("#,0.00");
            }
        }


        #endregion

        private void txtUnitPrice_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtUnitPrice.Text))
            {
                decimal unitPrice = Convert.ToDecimal(txtUnitPrice.Text.Trim());
                txtUnitPrice.Text = unitPrice.ToString("#,0.00");
            }

        }

        private void txtUnitPrice_KeyPress(object sender, KeyPressEventArgs e)
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
    }
}
