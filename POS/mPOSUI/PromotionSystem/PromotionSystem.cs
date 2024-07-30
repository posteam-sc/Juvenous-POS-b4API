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
    public partial class PromotionSystem : Form
    {
        #region Variables
        POSEntities entity = new POSEntities();
        public int currentPromotionId;
        public bool isEdit = false;
        private ToolTip tp = new ToolTip();
        List<GiftSystemInTransaction> attachList = new List<GiftSystemInTransaction>();
        #endregion
        public PromotionSystem()
        {
            InitializeComponent();
        }

        private void PromotionSystem_Load(object sender, EventArgs e)
        {
            #region Line List
            List<APP_Data.Brand> BrandList = new List<APP_Data.Brand>();
            APP_Data.Brand brand = new APP_Data.Brand();
            brand.Id = 0;
            brand.Name = "Select";
            APP_Data.Brand brandObj2 = new APP_Data.Brand();
            brandObj2.Id = 1;
            brandObj2.Name = "None";
            BrandList.Add(brand);
            BrandList.Add(brandObj2);
            BrandList.AddRange((from bList in entity.Brands select bList).ToList());
            cboLine.DataSource = BrandList;
            cboLine.DisplayMember = "Name";
            cboLine.ValueMember = "Id";
            cboLine.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboLine.AutoCompleteSource = AutoCompleteSource.ListItems;
            #endregion
            #region SubSegment List
            List<APP_Data.ProductSubCategory> pSubCatList = new List<APP_Data.ProductSubCategory>();
            APP_Data.ProductSubCategory SubCategoryObj1 = new APP_Data.ProductSubCategory();
            SubCategoryObj1.Id = 0;
            SubCategoryObj1.Name = "Select";
            pSubCatList.Add(SubCategoryObj1);
            APP_Data.ProductSubCategory SubCategoryObj2 = new APP_Data.ProductSubCategory();
            SubCategoryObj2.Id = 1;
            SubCategoryObj2.Name = "None";
            //pSubCatList.AddRange((from c in entity.ProductSubCategories where c.ProductCategoryId == Convert.ToInt32(cboMainCategory.SelectedValue) select c).ToList());
            cboSubSegment.DataSource = pSubCatList;
            cboSubSegment.DisplayMember = "Name";
            cboSubSegment.ValueMember = "Id";
            cboSubSegment.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboSubSegment.AutoCompleteSource = AutoCompleteSource.ListItems;
            #endregion
            #region Segment List
            List<APP_Data.ProductCategory> pMainCatList = new List<APP_Data.ProductCategory>();
            APP_Data.ProductCategory MainCategoryObj1 = new APP_Data.ProductCategory();
            MainCategoryObj1.Id = 0;
            MainCategoryObj1.Name = "Select";
            pMainCatList.Add(MainCategoryObj1);
            pMainCatList.AddRange((from MainCategory in entity.ProductCategories select MainCategory).ToList());
            cboSegment.DataSource = pMainCatList;
            cboSegment.DisplayMember = "Name";
            cboSegment.ValueMember = "Id";
            cboSegment.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboSegment.AutoCompleteSource = AutoCompleteSource.ListItems;
            #endregion
            #region Product List

            List<Product> productList1 = new List<Product>();
            List<Product> productList2 = new List<Product>();

            Product productObj = new Product();
            productObj.Name = "Select";
            productObj.Id = 0;
            productList1.Add(productObj);
            productList1.AddRange((from pList in entity.Products select pList).ToList());
            cboProduct.DataSource = productList1;
            cboProduct.DisplayMember = "Name";
            cboProduct.ValueMember = "Id";
            cboProduct.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboProduct.AutoCompleteSource = AutoCompleteSource.ListItems;

            productList2.Add(productObj);
            productList2.AddRange((from pList in entity.Products select pList).ToList());
            cboProduct.DataSource = productList1;
            cboProduct.DisplayMember = "Name";
            cboProduct.ValueMember = "Id";
            cboGiftProduct.DataSource = productList2;
            cboGiftProduct.DisplayMember = "Name";
            cboGiftProduct.ValueMember = "Id";
            cboGiftProduct.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboGiftProduct.AutoCompleteSource = AutoCompleteSource.ListItems;

            #endregion
            if (!isEdit)
            {
                chkPriceRanges.Checked = true;
                plPrice.Enabled = true;
                chkIsActive.Checked = true;
                chkQty.Checked = false;
                lblQty.Enabled = false;
                txtQty.Enabled = false;
                rdbGiftProduct.Checked = true;
                tlpGiftProduct.Enabled = true;
                tlpGiftCash.Enabled = false;
                tlpGiftDiscount.Enabled = false;
                rdbOneAmount.Checked = true;
                txtToCost.Enabled = false;
                lblToCost.Enabled = false;
                chkSize.Checked = true;
                chkFilterQty.Checked = true;
            }
            else
            {
                GiftSystem currentPromotion = entity.GiftSystems.Where(x => x.Id == currentPromotionId).FirstOrDefault();
                attachList = entity.GiftSystemInTransactions.Where(x => x.GiftSystemId == currentPromotion.Id).ToList();

                txtName.Text = currentPromotion.Name.ToString();
                if (currentPromotion.Type == "GWP")
                {
                    rdbGWP.Checked = true;
                }
                else if (currentPromotion.Type == "PWP")
                {
                    rdbPWP.Checked = true;
                }
                dtpFrom.Value = currentPromotion.ValidFrom.Date;
                dtpTo.Value = currentPromotion.ValidTo.Date;

                if (currentPromotion.MustBuyCostTo == 0 && currentPromotion.MustBuyCostFrom == 0)
                {
                    chkPriceRanges.Checked = false;
                    plPrice.Enabled = false;
                }
                else if (currentPromotion.MustBuyCostFrom > 0 && currentPromotion.MustBuyCostTo == 0)
                {
                    chkPriceRanges.Checked = true;
                    plPrice.Enabled = true;
                    txtToCost.Enabled = false;
                    lblToCost.Enabled = false;
                    rdbOneAmount.Checked = true;
                    txtFromCost.Text = currentPromotion.MustBuyCostFrom.ToString();
                }
                else
                {
                    chkPriceRanges.Checked = true;
                    plPrice.Enabled = true;
                    rdbBetweenAmount.Checked = true;
                    txtFromCost.Text = currentPromotion.MustBuyCostFrom.ToString();
                    txtToCost.Text = currentPromotion.MustBuyCostTo.ToString();
                }

                if (currentPromotion.UseBrandFilter == true)
                {
                    if (currentPromotion.Brand == null)
                    {
                        cboLine.Text = "None";
                    }
                    else
                    {
                        cboLine.Text = currentPromotion.Brand.Name;
                    }
                }
                if (currentPromotion.UseCategoryFilter == true)
                {
                    cboSegment.Text = currentPromotion.ProductCategory.Name;
                }
                if (currentPromotion.UseSubCategoryFilter == true)
                {
                    if (currentPromotion.ProductSubCategory == null)
                    {
                        cboSubSegment.Text = "None";
                    }
                    else
                    {
                        cboSubSegment.Text = currentPromotion.ProductSubCategory.Name;
                    }
                }
                if (currentPromotion.UseProductFilter == true)
                {
                    cboProduct.Text = currentPromotion.Product.Name;
                }
                if (currentPromotion.UseSizeFilter == true)
                {
                    chkSize.Checked = true;
                    txtSize.Text = currentPromotion.FilterSize.ToString();
                }
                else
                {
                    chkSize.Checked = false;
                    txtSize.Enabled = false;
                }
                if (currentPromotion.UseQtyFilter == true)
                {
                    chkFilterQty.Checked = true;
                    txtFilterQty.Text = currentPromotion.FilterQty.ToString();
                }
                else
                {
                    chkFilterQty.Checked = false;
                    txtFilterQty.Enabled = false;
                }
                if (currentPromotion.IsActive == true)
                {
                    chkIsActive.Checked = true;
                }
                else
                {
                    chkIsActive.Checked = false;
                }
                if (currentPromotion.UsePromotionQty == true)
                {
                    chkQty.Checked = true;
                    lblQty.Enabled = true;
                    txtQty.Enabled = true;
                    txtQty.Text = currentPromotion.PromotionQty.ToString();
                }
                else
                {
                    chkQty.Checked = false;
                    lblQty.Enabled = false;
                    txtQty.Enabled = false;
                }
                if (currentPromotion.Product1 != null)
                {
                    rdbGiftProduct.Checked = true;
                    cboGiftProduct.Text = currentPromotion.Product1.Name;
                    txtSaleTrueValue.Text = currentPromotion.PriceForGiftProduct.ToString();
                    tlpGiftCash.Enabled = false;
                    tlpGiftDiscount.Enabled = false;
                }
                else if (currentPromotion.GiftCashAmount > 0)
                {
                    rdbGiftAmount.Checked = true;
                    txtGiftAmount.Text = currentPromotion.GiftCashAmount.ToString();
                    tlpGiftProduct.Enabled = false;
                    tlpGiftDiscount.Enabled = false;
                }
                else if (currentPromotion.DiscountPercentForTransaction > 0)
                {
                    rdbDiscount.Checked = true;
                    txtGiftDiscount.Text = currentPromotion.DiscountPercentForTransaction.ToString();
                    tlpGiftCash.Enabled = false;
                    tlpGiftProduct.Enabled = false;
                }
                btnSave.Image = POS.Properties.Resources.update_big;
                if (attachList.Count > 0)
                {
                    tlpName.Enabled = false;
                    tlpHasPrice.Enabled = false;
                    lblFrom.Enabled = false;
                    dtpFrom.Enabled = false;
                    tlpValidation.Enabled = false;
                    plType.Enabled = false;
                    plPrice.Enabled = false;
                    plGift.Enabled = false;
                }
            }
        }

        private void cboLine_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboLine.SelectedIndex > 1)
            {

                List<Product> productList1 = new List<Product>();
                int lineId = Convert.ToInt32(cboLine.SelectedValue);
                Product productObj = new Product();
                productObj.Name = "Select";
                productObj.Id = 0;
                productList1.Add(productObj);
                productList1.AddRange(entity.Products.Where(x => x.BrandId == lineId).ToList());
                cboProduct.DataSource = productList1;
                cboProduct.DisplayMember = "Name";
                cboProduct.ValueMember = "Id";
                cboProduct.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cboProduct.AutoCompleteSource = AutoCompleteSource.ListItems;
            }
            else if (cboLine.SelectedIndex == 1)
            {
                List<Product> productList1 = new List<Product>();
                int lineId = Convert.ToInt32(cboLine.SelectedValue);
                Product productObj = new Product();
                productObj.Name = "Select";
                productObj.Id = 0;
                productList1.Add(productObj);
                productList1.AddRange(entity.Products.Where(x => x.BrandId == null).ToList());
                cboProduct.DataSource = productList1;
                cboProduct.DisplayMember = "Name";
                cboProduct.ValueMember = "Id";
                cboProduct.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cboProduct.AutoCompleteSource = AutoCompleteSource.ListItems;
            }
        }

        private void cboSegment_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboSegment.SelectedIndex > 0)
            {
                int productCategoryId = Int32.Parse(cboSegment.SelectedValue.ToString());
                List<APP_Data.ProductSubCategory> pSubCatList = new List<APP_Data.ProductSubCategory>();
                APP_Data.ProductSubCategory SubCategoryObj1 = new APP_Data.ProductSubCategory();
                SubCategoryObj1.Id = 0;
                SubCategoryObj1.Name = "Select";
                APP_Data.ProductSubCategory SubCategoryObj2 = new APP_Data.ProductSubCategory();
                SubCategoryObj2.Id = 1;
                SubCategoryObj2.Name = "None";
                pSubCatList.Add(SubCategoryObj1);
                pSubCatList.Add(SubCategoryObj2);
                pSubCatList.AddRange((from c in entity.ProductSubCategories where c.ProductCategoryId == productCategoryId select c).ToList());
                cboSubSegment.DataSource = pSubCatList;
                cboSubSegment.DisplayMember = "Name";
                cboSubSegment.ValueMember = "Id";
                cboSubSegment.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cboSubSegment.AutoCompleteSource = AutoCompleteSource.ListItems;
                cboSubSegment.Enabled = true;
            }
            else
            {
                cboSubSegment.SelectedIndex = 0;
            }
        }

        private void cboSubSegment_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool hasError = false;

            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            //Validation
            if (txtName.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtName, "Error");
                tp.Show("Please fill up name!", txtName);
                hasError = true;
            }
            if (chkPriceRanges.Checked)
            {
                if (rdbOneAmount.Checked)
                {
                    if (txtFromCost.Text.Trim() == string.Empty)
                    {
                        tp.SetToolTip(txtFromCost, "Error");
                        tp.Show("Please fill up from cost!", txtFromCost);
                        hasError = true;
                    }
                }
                else
                {
                    if (txtFromCost.Text.Trim() == string.Empty)
                    {
                        tp.SetToolTip(txtFromCost, "Error");
                        tp.Show("Please fill up from cost!", txtFromCost);
                        hasError = true;
                    }
                    else if (txtToCost.Text.Trim() == string.Empty)
                    {
                        tp.SetToolTip(txtToCost, "Error");
                        tp.Show("Please fill up to cost!", txtToCost);
                        hasError = true;
                    }
                }
            }
            if (chkSize.Checked)
            {
                if (txtSize.Text.Trim() == string.Empty)
                {
                    tp.SetToolTip(txtSize, "Error");
                    tp.Show("Please fill up size!", txtSize);
                    hasError = true;
                }
            }
            if (chkFilterQty.Checked)
            {
                if (txtFilterQty.Text.Trim() == string.Empty)
                {
                    tp.SetToolTip(txtFilterQty, "Error");
                    tp.Show("Please fill up qty!", txtFilterQty);
                    hasError = true;
                }
            }
            if (chkQty.Checked)
            {
                if (txtQty.Text.Trim() == string.Empty)
                {
                    tp.SetToolTip(txtQty, "Error");
                    tp.Show("Please fill up Qty!", txtQty);
                    hasError = true;
                }
            }
            if (rdbGiftProduct.Checked)
            {
                if (cboGiftProduct.SelectedIndex == 0)
                {
                    tp.SetToolTip(cboGiftProduct, "Error");
                    tp.Show("Please select gift product!", cboGiftProduct);
                    hasError = true;
                }
                else if (txtSaleTrueValue.Text.Trim() == string.Empty)
                {
                    tp.SetToolTip(txtSaleTrueValue, "Error");
                    tp.Show("Please fill up discounted sale true value!", txtSaleTrueValue);
                    hasError = true;
                }
            }
            if (rdbGiftAmount.Checked)
            {
                if (txtGiftAmount.Text.Trim() == string.Empty)
                {
                    tp.SetToolTip(txtSaleTrueValue, "Error");
                    tp.Show("Please fill up gift amount!", txtGiftAmount);
                    hasError = true;
                }
            }
            if (rdbDiscount.Checked)
            {
                if (txtGiftDiscount.Text.Trim() == string.Empty)
                {
                    tp.SetToolTip(txtGiftDiscount, "Error");
                    tp.Show("Please fill up gift discount %!", txtGiftDiscount);
                    hasError = true;
                }
            }



            if (!hasError)
            {
                #region save
                if (!isEdit)
                {
                    GiftSystem giftSystem = new GiftSystem();
                    GiftSystem data_gs = entity.GiftSystems.Where(x => x.Name == txtName.Text).FirstOrDefault();
                    if (data_gs == null)
                    {
                        giftSystem.Name = txtName.Text;
                        if (rdbGWP.Checked)
                        {
                            giftSystem.Type = "GWP";
                        }
                        else
                        {
                            giftSystem.Type = "PWP";
                        }
                        giftSystem.ValidFrom = dtpFrom.Value.Date;
                        giftSystem.ValidTo = dtpTo.Value.Date;
                        if (chkPriceRanges.Checked)
                        {
                            if (rdbOneAmount.Checked)
                            {
                                giftSystem.MustBuyCostFrom = Convert.ToInt64(txtFromCost.Text);
                                giftSystem.MustBuyCostTo = 0;
                            }
                            else
                            {
                                giftSystem.MustBuyCostFrom = Convert.ToInt32(txtFromCost.Text);
                                giftSystem.MustBuyCostTo = Convert.ToInt32(txtToCost.Text);
                            }
                        }
                        else
                        {
                            giftSystem.MustBuyCostFrom = 0;
                            giftSystem.MustBuyCostTo = 0;
                        }
                        //Brand
                        if (cboLine.SelectedIndex > 1)
                        {
                            giftSystem.FilterBrandId = Convert.ToInt32(cboLine.SelectedValue);
                            giftSystem.UseBrandFilter = true;
                        }
                        else if (cboLine.SelectedIndex == 1)
                        {
                            giftSystem.UseBrandFilter = true;
                        }
                        else
                        {
                            giftSystem.UseBrandFilter = false;
                        }
                        //Segment
                        if (cboSegment.SelectedIndex > 0)
                        {
                            giftSystem.UseCategoryFilter = true;
                            giftSystem.FilterCategoryId = Convert.ToInt32(cboSegment.SelectedValue);
                        }
                        else
                        {
                            giftSystem.UseCategoryFilter = false;
                        }
                        //SubSegment
                        if (cboSubSegment.SelectedIndex > 1)
                        {
                            giftSystem.FilterSubCategoryId = Convert.ToInt32(cboSubSegment.SelectedValue);
                            giftSystem.UseSubCategoryFilter = true;
                        }
                        else if (cboSubSegment.SelectedIndex == 1)
                        {
                            giftSystem.UseSubCategoryFilter = true;
                        }
                        else
                        {
                            giftSystem.UseSubCategoryFilter = false;
                        }
                        //Product
                        if (cboProduct.SelectedIndex > 0)
                        {
                            giftSystem.UseProductFilter = true;
                            giftSystem.MustIncludeProductId = Convert.ToInt32(cboProduct.SelectedValue);
                        }
                        else
                        {

                            giftSystem.UseProductFilter = false;
                        }
                        //Size
                        if (chkSize.Checked)
                        {
                            giftSystem.FilterSize = Convert.ToInt32(txtSize.Text);  
                            giftSystem.UseSizeFilter = true;
                        }
                        else
                        {
                            giftSystem.UseSizeFilter = false;
                        }
                        //Qty
                        if (chkFilterQty.Checked)
                        {
                            giftSystem.UseQtyFilter = true;
                            giftSystem.FilterQty = Convert.ToInt32(txtFilterQty.Text);
                        }
                        else
                        {
                            giftSystem.UseQtyFilter = false;
                        }
                        //IsActive
                        giftSystem.IsActive = chkIsActive.Checked;
                        //Limited qty
                        if (chkQty.Checked)
                        {
                            giftSystem.UsePromotionQty = true;
                            giftSystem.PromotionQty = Convert.ToInt32(txtQty.Text);
                        }
                        else
                        {
                            giftSystem.UsePromotionQty = false;
                            giftSystem.PromotionQty = 0;
                        }
                        //Gift Type
                        if (rdbGiftProduct.Checked)
                        {
                            giftSystem.GiftProductId = Convert.ToInt32(cboGiftProduct.SelectedValue);
                            int saleTruePrice;
                            Int32.TryParse(txtSaleTrueValue.Text, out saleTruePrice);
                            giftSystem.PriceForGiftProduct = saleTruePrice;
                            giftSystem.GiftCashAmount = 0;
                            giftSystem.DiscountPercentForTransaction = 0;
                        }
                        else if (rdbGiftAmount.Checked)
                        {
                            giftSystem.GiftProductId = null;
                            giftSystem.PriceForGiftProduct = 0;
                            giftSystem.GiftCashAmount = Convert.ToInt32(txtGiftAmount.Text);
                            giftSystem.DiscountPercentForTransaction = 0;
                        }
                        else if (rdbDiscount.Checked)
                        {
                            giftSystem.GiftProductId = null;
                            giftSystem.PriceForGiftProduct = 0;
                            giftSystem.GiftCashAmount = 0;
                            giftSystem.DiscountPercentForTransaction = Convert.ToInt32(txtGiftDiscount.Text);
                        }
                        entity.GiftSystems.Add(giftSystem);
                        entity.SaveChanges();
                        MessageBox.Show("Successfully Saved!", "Save");
                        this.Dispose();
                        To_PromotionList();

                    }
                    else
                    {
                        MessageBox.Show("Gift System Name should not be same!");
                    }

                }
                #endregion
                #region update
                else
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to update?", "Update", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (result.Equals(DialogResult.OK))
                    {
                        GiftSystem giftSystemObj = entity.GiftSystems.Where(x => x.Id == currentPromotionId).FirstOrDefault();
                        giftSystemObj.Name = txtName.Text;
                        if (rdbGWP.Checked)
                        {
                            giftSystemObj.Type = "GWP";
                        }
                        else
                        {
                            giftSystemObj.Type = "PWP";
                        }
                        giftSystemObj.ValidFrom = dtpFrom.Value.Date;
                        if (attachList.Count > 0)
                        {
                            if (giftSystemObj.ValidTo.Date <= dtpTo.Value.Date)
                            {
                                giftSystemObj.ValidTo = dtpTo.Value.Date;
                            }
                            else
                            {
                                MessageBox.Show("Please select valid to date again!", "Update");
                                return;
                            }
                        }
                        else
                        {
                            giftSystemObj.ValidTo = dtpTo.Value.Date;
                        }
                        if (chkPriceRanges.Checked)
                        {
                            if (rdbOneAmount.Checked)
                            {
                                giftSystemObj.MustBuyCostFrom = Convert.ToInt64(txtFromCost.Text);
                                giftSystemObj.MustBuyCostTo = 0;
                            }
                            else
                            {
                                giftSystemObj.MustBuyCostFrom = Convert.ToInt32(txtFromCost.Text);
                                giftSystemObj.MustBuyCostTo = Convert.ToInt32(txtToCost.Text);
                            }
                        }
                        else
                        {
                            giftSystemObj.MustBuyCostFrom = 0;
                            giftSystemObj.MustBuyCostTo = 0;
                        }
                        //Brand

                        if (cboLine.SelectedIndex > 1)
                        {
                            giftSystemObj.FilterBrandId = Convert.ToInt32(cboLine.SelectedValue);
                            giftSystemObj.UseBrandFilter = true;
                        }
                        else if (cboLine.SelectedIndex == 1)
                        {
                            giftSystemObj.UseBrandFilter = true;
                        }
                        else
                        {
                            giftSystemObj.UseBrandFilter = false;
                        }
                        //Segment

                        if (cboSegment.SelectedIndex > 0)
                        {
                            giftSystemObj.UseCategoryFilter = true;
                            giftSystemObj.FilterCategoryId = Convert.ToInt32(cboSegment.SelectedValue);
                        }
                        else
                        {
                            giftSystemObj.UseCategoryFilter = false;
                            giftSystemObj.FilterCategoryId = null;
                        }
                        //SubSegment

                        if (cboSubSegment.SelectedIndex > 1)
                        {
                            giftSystemObj.FilterSubCategoryId = Convert.ToInt32(cboSubSegment.SelectedValue);
                            giftSystemObj.UseSubCategoryFilter = true;
                        }
                        else if (cboSubSegment.SelectedIndex == 1)
                        {

                            giftSystemObj.UseSubCategoryFilter = true;
                            giftSystemObj.FilterSubCategoryId = Convert.ToInt32(cboSubSegment.SelectedValue);
                        }
                        else
                        {
                            giftSystemObj.UseSubCategoryFilter = false;
                            giftSystemObj.FilterSubCategoryId = null;
                        }
                        //Product

                        if (cboProduct.SelectedIndex > 0)
                        {
                            giftSystemObj.UseProductFilter = true;
                            giftSystemObj.MustIncludeProductId = Convert.ToInt32(cboProduct.SelectedValue);
                        }
                        else
                        {

                            giftSystemObj.UseProductFilter = false;
                            giftSystemObj.MustIncludeProductId = null;
                        }
                        //Size
                        if (chkSize.Checked)
                        {
                            giftSystemObj.FilterSize = Convert.ToInt32(txtSize.Text); //ZMH
                            giftSystemObj.UseSizeFilter = true;
                        }
                        else
                        {
                            giftSystemObj.FilterSize = null;
                            giftSystemObj.UseSizeFilter = false;
                        }
                        //Qty
                        if (chkFilterQty.Checked)
                        {
                            giftSystemObj.UseQtyFilter = true;
                            giftSystemObj.FilterQty = Convert.ToInt32(txtFilterQty.Text);
                        }
                        else
                        {
                            giftSystemObj.FilterQty = null;
                            giftSystemObj.UseQtyFilter = false;
                        }
                        //IsActive
                        giftSystemObj.IsActive = chkIsActive.Checked;
                        //Limited qty
                        if (chkQty.Checked)
                        {
                            giftSystemObj.UsePromotionQty = true;
                            giftSystemObj.PromotionQty = Convert.ToInt32(txtQty.Text);
                        }
                        else
                        {
                            giftSystemObj.UsePromotionQty = false;
                            giftSystemObj.PromotionQty = 0;
                        }
                        //Gift Type
                        if (rdbGiftProduct.Checked)
                        {
                            giftSystemObj.GiftProductId = Convert.ToInt32(cboGiftProduct.SelectedValue);
                            giftSystemObj.PriceForGiftProduct = Convert.ToInt32(txtSaleTrueValue.Text);
                            giftSystemObj.GiftCashAmount = 0;
                            giftSystemObj.DiscountPercentForTransaction = 0;
                        }
                        else if (rdbGiftAmount.Checked)
                        {
                            giftSystemObj.GiftProductId = null;
                            giftSystemObj.PriceForGiftProduct = 0;
                            giftSystemObj.GiftCashAmount = Convert.ToInt32(txtGiftAmount.Text);
                            giftSystemObj.DiscountPercentForTransaction = 0;
                        }
                        else if (rdbDiscount.Checked)
                        {
                            giftSystemObj.GiftProductId = null;
                            giftSystemObj.PriceForGiftProduct = 0;
                            giftSystemObj.GiftCashAmount = 0;
                            giftSystemObj.DiscountPercentForTransaction = Convert.ToInt32(txtGiftDiscount.Text);
                        }
                        entity.Entry(giftSystemObj).State = EntityState.Modified;
                        entity.SaveChanges();
                        MessageBox.Show("Successfully Updated!", "Update");
                        if (System.Windows.Forms.Application.OpenForms["Promotion_List"] != null)
                        {
                            Promotion_List newForm = (Promotion_List)System.Windows.Forms.Application.OpenForms["Promotion_List"];
                            newForm.DataBind();
                        }
                        this.Dispose();
                        To_PromotionList();
                    }
                }
                #endregion

            }

        }
        private void PromotionSystem_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(txtName);
            tp.Hide(txtFromCost);
            tp.Hide(cboGiftProduct);
            tp.Hide(txtGiftDiscount);
            tp.Hide(txtGiftAmount);
            tp.Hide(txtSaleTrueValue);
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            rdbGWP.Checked = false;
            txtName.Text = string.Empty;
            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;
            chkPriceRanges.Checked = false;
            rdbOneAmount.Checked = true;
            txtFromCost.Text = "0";
            txtToCost.Text = "0";
            cboLine.SelectedIndex = 0;
            cboGiftProduct.SelectedIndex = 0;
            cboProduct.SelectedIndex = 0;
            cboSegment.SelectedIndex = 0;
            cboSubSegment.SelectedIndex = 0;
            chkIsActive.Checked = true;
            chkQty.Checked = false;
            txtQty.Text = "0";
            rdbGiftProduct.Checked = true;
            txtSaleTrueValue.Text = "0";
            txtGiftAmount.Text = "0";
            txtGiftDiscount.Text = "0";
        }

        private void DataBindProductList(int LineId, int SegmentId, int SubSegmentId)
        {
            if (LineId == 0)
            {

            }
        }

        private void chkPriceRanges_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPriceRanges.Checked)
            {
                plPrice.Enabled = true;
            }
            else
            {
                plPrice.Enabled = false;
            }
        }
        private void chkSize_CheckedChanged(object sender, EventArgs e)
        {

            if (chkSize.Checked)
            {
                txtSize.Enabled = true;
            }
            else
            {
                txtSize.Clear();
                txtSize.Enabled = false;
            }
        }

        private void chkFilterQty_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFilterQty.Checked)
            {
                txtFilterQty.Enabled = true;
            }
            else
            {
                txtFilterQty.Clear();
                txtFilterQty.Enabled = false;
            }
        }
        private void rdbOneAmount_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbOneAmount.Checked)
            {
                txtToCost.Enabled = false;
                lblToCost.Enabled = false;
            }
            else
            {
                txtToCost.Enabled = true;
                lblToCost.Enabled = true;
            }
        }

        private void rdbGiftProduct_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbGiftProduct.Checked)
            {
                tlpGiftProduct.Enabled = true;
                tlpGiftCash.Enabled = false;
                tlpGiftDiscount.Enabled = false;
            }
        }

        private void rdbGiftAmount_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbGiftAmount.Checked)
            {
                tlpGiftProduct.Enabled = false;
                tlpGiftCash.Enabled = true;
                tlpGiftDiscount.Enabled = false;
            }
        }

        private void rdbDiscount_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbDiscount.Checked)
            {
                tlpGiftProduct.Enabled = false;
                tlpGiftCash.Enabled = false;
                tlpGiftDiscount.Enabled = true;
            }
        }

        private void chkQty_CheckedChanged(object sender, EventArgs e)
        {
            if (chkQty.Checked)
            {
                lblQty.Enabled = true;
                txtQty.Enabled = true;
            }
            else
            {
                lblQty.Enabled = false;
                txtQty.Enabled = false;
                txtQty.Text = "";
            }
        }

        private void rdbPWP_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbPWP.Checked)
            {
                rdbDiscount.Enabled = false;
                rdbGiftAmount.Enabled = false;
            }
            else
            {
                rdbDiscount.Enabled = true;
                rdbGiftAmount.Enabled = true;
            }

        }

        private void rdbGWP_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbGWP.Checked)
            {
                txtSaleTrueValue.Text = "0";
                txtSaleTrueValue.Enabled = false;
            }
            else
            {
                txtSaleTrueValue.Enabled = true;
            }

        }

        #region "Function"
        private void To_PromotionList()
        {
            if (System.Windows.Forms.Application.OpenForms["Promotion_List"] != null)
            {
                Promotion_List newForm = (Promotion_List)System.Windows.Forms.Application.OpenForms["Promotion_List"];
                newForm.Bind_Promotion();
            }
        }
        #endregion
    }
}
