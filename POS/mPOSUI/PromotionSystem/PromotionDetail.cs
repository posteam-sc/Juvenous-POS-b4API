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
    public partial class PromotionDetail : Form
    {
        #region Variable

        POSEntities entity = new POSEntities();
        public int currentPromotionId;
        #endregion
        public PromotionDetail()
        {
            InitializeComponent();
        }

        private void PromotionDetail_Load(object sender, EventArgs e)
        {
            GiftSystem currentPromotion = entity.GiftSystems.Where(x => x.Id == currentPromotionId).FirstOrDefault();
            lblName.Text = currentPromotion.Name.ToString();
            lblType.Text = currentPromotion.Type.ToString();
            lblPeriod.Text = "From " + currentPromotion.ValidFrom.Date.ToString("dd-MM-yyyy") + " To " + currentPromotion.ValidTo.Date.ToString("dd-MM-yyyy");
            if (currentPromotion.MustBuyCostTo == 0)
            {
                lblPriceRange.Text = "at least " + currentPromotion.MustBuyCostFrom.ToString() +" price";
            }
            else
            {
                lblPriceRange.Text = "between " + currentPromotion.MustBuyCostFrom.ToString() + " and " + currentPromotion.MustBuyCostTo.ToString() + " price";
            }
            if (currentPromotion.UseBrandFilter == true)
            {
                lblLine.Text = currentPromotion.Brand.Name;
            }
            if (currentPromotion.UseCategoryFilter == true)
            {
                lblSegment.Text = currentPromotion.ProductCategory.Name;
            }
            if (currentPromotion.UseSubCategoryFilter == true)
            {
                lblSubSegment.Text = currentPromotion.ProductSubCategory.Name;
            }
            if (currentPromotion.UseProductFilter == true)
            {
                lblFilterProduct.Text = currentPromotion.Product.Name;
            }
            lblActive.Text = currentPromotion.IsActive.Value.ToString();
            if (currentPromotion.UsePromotionQty == true)
            {
                lblQty.Text = currentPromotion.PromotionQty.ToString();
            }
            if (currentPromotion.Product1 != null)
            {
                lblgiftProduct.Text = currentPromotion.Product1.Name;
                lblSaleTruePrice.Text = currentPromotion.PriceForGiftProduct.ToString();
            }
            lblGiftAmount.Text = currentPromotion.GiftCashAmount.ToString();
            lblGiftDiscount.Text = currentPromotion.DiscountPercentForTransaction.ToString() + "%";
        }
    }
}
