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
    public partial class UnitConversionfrm : Form
    {
        #region "Initialzie"
        public UnitConversionfrm()
        {
            InitializeComponent();
        }
        #endregion

        #region vairable
        POSEntities entity = new POSEntities();
        private ToolTip tp = new ToolTip();
     
        Boolean IsSave = false;
     
        List<Stock_Transaction> productList = new List<Stock_Transaction>();

        int OriginalMaxCurBalance = 0;
        int OriginalNormalCurBalance = 0;
        List<PurchaseDetail> convertPurList = new List<PurchaseDetail>();
        List<int?> purDetailIdList = new List<int?>();
        int _firstMaxProId = 0;
        int _firstNormalProId = 0;
        #endregion

        #region Method
        private void Enable_TextBox(bool b)
        {
            txtFromQty.Enabled = b;
            txtPiecePerPack .Enabled= b;
       
        }

        private void Collect_PurchaseConverQty(int convertQty, int purDetailId, int currentQty)
        {
            PurchaseDetail pd = new PurchaseDetail();
            pd.Id = purDetailId;
            pd.ConvertQty = convertQty;
            pd.CurrentQy = currentQty;
            convertPurList.Add(pd);
           
        }

        private void Get_PurchasePrice()
        {
            entity = new POSEntities();
            int _balance = 0;
            int maxProductId = Convert.ToInt32(cboFromProduct.SelectedValue);
            List<APP_Data.PurchaseDetail> purlist = (from p in entity.PurchaseDetails
                                                     join m in entity.MainPurchases on p.MainPurchaseId equals m.Id
                                                     where p.ProductId == maxProductId && p.IsDeleted == false && m.IsCompletedInvoice == true && p.CurrentQy > 0
                                                     && (p.ConvertQty  < p.CurrentQy) && (m.IsPurchase == true)
                                                     orderby p.Date ascending
                                                     select p).ToList();

            int FromQty = Convert.ToInt32(txtFromQty.Text);
            int tempFromQty = FromQty;
            int ToQty = Convert.ToInt32(txtToQty.Text);
            double totalMaxPurPrice = 0;
            if (purlist.Count > 0)
            {
                foreach (PurchaseDetail p in purlist)
                {
                    if (p.CurrentQy >= FromQty)
                    {
                     //   int _maxPurPrice = Convert.ToInt32(p.UnitPrice) * Convert.ToInt32(p.Qty);
                        int _maxPurPrice = Convert.ToInt32(p.UnitPrice) * FromQty;
                        totalMaxPurPrice = _maxPurPrice;

                        Collect_PurchaseConverQty(ToQty,p.Id, FromQty);
                        break;
                    }
                    else if (p.CurrentQy < FromQty)
                    {
                        int _maxPurPrice = Convert.ToInt32(p.UnitPrice) * Convert.ToInt32(p.Qty);
                        totalMaxPurPrice += _maxPurPrice;

                        int paraBalance = 0;

                        if (p.Qty < tempFromQty)
                        {
                            paraBalance = Convert.ToInt32(p.Qty);
                            _balance = tempFromQty - Convert.ToInt32(p.Qty);
                            tempFromQty = _balance;
                        }
                        else
                        {
                            paraBalance = tempFromQty;
                        }

                        Collect_PurchaseConverQty(paraBalance, p.Id,_balance);
                    }

                }

            }
            int maxCurBalance = Convert.ToInt32(txtBalanceMax.Text);
            int piecePerPack = Convert.ToInt32(txtPiecePerPack.Text);
          
            double maxUnitPurPrice = totalMaxPurPrice / FromQty;
            var actualMaxUnitPurPrice = Math.Round(maxUnitPurPrice, 0, MidpointRounding.AwayFromZero);
            txtMaxPurPrice.Text = actualMaxUnitPurPrice.ToString();
            lblFromPerPurPrice.Visible = true;

            double normalUnirPurPrice = actualMaxUnitPurPrice / piecePerPack;
            var actualNorUnitPurPrice = Math.Round(normalUnirPurPrice, 0, MidpointRounding.AwayFromZero);
            txtNormalPurPrice.Text = actualNorUnitPurPrice.ToString();
            lblToPerPurPrice.Visible = true;
        }

       private void Save_ConvertQty_ToStockTransaction(List<Stock_Transaction> productList,bool IsStockIn)
        {
            int _year, _month;

            _year = System.DateTime.Now.Year;
            _month = System.DateTime.Now.Month;
            Utility.Conversion_Run_Process(_year, _month, productList, IsStockIn);
        }

       private void Clear_Data()
       {
           cboFromProduct.SelectedIndex = 0;
           txtBalanceMax.Clear();
           cboToProduct.SelectedIndex = 0;
           txtBalanceNormal.Clear();
           txtFromQty.Clear();
           txtPiecePerPack.Clear();
           txtToQty.Clear();

           cboToProduct.Enabled = false;
           lblFromPerPurPrice.Visible = false;
           lblToPerPurPrice.Visible = false;
           txtUnitMax.Text = "";
           txtNormalPurPrice.Text = "";
           txtMaxPurPrice.Text = "";
           Enable_TextBox(false);
       }

       private void Clear_QtyAndPrice()
       {
           txtFromQty.Clear();
           txtMaxPurPrice.Text = "";
           lblFromPerPurPrice.Visible = false;
           txtPiecePerPack.Clear();
           txtToQty.Clear();
           txtNormalPurPrice.Text = "";
           lblToPerPurPrice.Visible = false;
       }

        #endregion

       #region Event
       private void UnitConversionfrm_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            Utility.BindConversionProduct(cboFromProduct,"Maximum");
            Utility.BindConversionProduct(cboToProduct,"Normal");
        }

        private void cboFromProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboFromProduct.SelectedIndex > 0)
            {
                int _productId = Convert.ToInt32(cboFromProduct.SelectedValue);
                var MaxQty = (from p in entity.Products where p.Id == _productId select p.Qty).FirstOrDefault();

                txtBalanceMax.Text = MaxQty.ToString();
                OriginalMaxCurBalance = Convert.ToInt32(MaxQty);

                cboToProduct.Enabled=true;
                if (_productId != _firstMaxProId)
                {
                    Clear_QtyAndPrice();
                }
                _firstMaxProId = _productId;
            }
        }

        private void cboToProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboToProduct.SelectedIndex > 0)
            {
                int _productId = Convert.ToInt32(cboToProduct.SelectedValue);
                var NormalQty = (from p in entity.Products where p.Id == _productId select p.Qty).FirstOrDefault();
                txtBalanceNormal.Text = NormalQty.ToString();
                OriginalNormalCurBalance = Convert.ToInt32(NormalQty);
                Enable_TextBox(true);
                if (_productId != _firstNormalProId)
                {
                    Clear_QtyAndPrice();
                }
                _firstNormalProId = _productId;
            }
        }

        private void txtFromQty_Leave(object sender, EventArgs e)
        {
            if (txtFromQty.Text.Trim() != string.Empty)
            {
                if (OriginalMaxCurBalance < Convert.ToInt32(txtFromQty.Text))
                {
                    tp.SetToolTip(txtFromQty, "Error");
                    tp.Show("Maximum Current Stock Balance is Lower than Convert Qty", txtFromQty);
                    txtFromQty.Focus();
                }
                else
                {
                    tp.SetToolTip(txtFromQty, "Error");
                    tp.Show("", txtFromQty);
                    lblMaxBalance.Text = (OriginalMaxCurBalance - Convert.ToInt32(txtFromQty.Text)).ToString();
                    txtPiecesPerPack_Leave(sender, e);
                }
            }
        }

        private void txtPiecesPerPack_Leave(object sender, EventArgs e)
        {
            if (txtPiecePerPack.Text.Trim() != string.Empty)
            {
            if (OriginalMaxCurBalance >= Convert.ToInt32(txtFromQty.Text))
            {
                int FromQty = Convert.ToInt32(txtFromQty.Text);
                int piece = Convert.ToInt32(txtPiecePerPack.Text);
                if (FromQty > 0 && piece >0)
                {
                    int _totalConvertedQty = FromQty * piece;

                    txtToQty.Text = _totalConvertedQty.ToString();
                    lblNormalBalance.Text = (OriginalNormalCurBalance + _totalConvertedQty).ToString();

                        Get_PurchasePrice();
                    IsSave = true;
                }
                else
                {
                    tp.SetToolTip(txtFromQty, "Error");
                    tp.Show("Please fill up   Convert Qty (Maximum Product)!", txtFromQty);
                    txtFromQty.Focus();
                    Clear_QtyAndPrice();
                }
            }
            else
            {
                tp.SetToolTip(txtBalanceMax, "Error");
                tp.Show("Can't Convert Qty! Maximum Current Stock Balance is Lower than Convert Qty", txtFromQty);
                txtFromQty.Focus();
            }
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (IsSave)
            {
                int normalProductId = Convert.ToInt32(cboToProduct.SelectedValue);

                #region update conver qty in purchase
                foreach (var _data in convertPurList)
                {
                    var _pur = (from pd in entity.PurchaseDetails where pd.Id == _data.Id select pd).FirstOrDefault();
                    purDetailIdList.Add(_data.Id);
                    _pur.ConvertQty = _data.ConvertQty;
                    _pur.CurrentQy -= Convert.ToInt32(_data.CurrentQy);
                    entity.Entry(_pur).State = System.Data.EntityState.Modified;
                }
                entity.SaveChanges();


                #endregion

                #region saveing purchase table
                int totalNormalPurPrice = Convert.ToInt32(txtToQty.Text) * Convert.ToInt32(txtNormalPurPrice.Text);

                MainPurchase mainPurchaseObj = new MainPurchase();
                mainPurchaseObj.Date = DateTime.Now;
                mainPurchaseObj.TotalAmount = totalNormalPurPrice;
                mainPurchaseObj.Cash = totalNormalPurPrice;

                mainPurchaseObj.OldCreditAmount = 0;
                mainPurchaseObj.SettlementAmount = 0;
                mainPurchaseObj.IsActive = true;
                mainPurchaseObj.DiscountAmount = 0;
                mainPurchaseObj.IsDeleted = false;
                mainPurchaseObj.CreatedDate = DateTime.Now;
                mainPurchaseObj.CreatedBy = MemberShip.UserId;
                mainPurchaseObj.IsCompletedInvoice = true;
                mainPurchaseObj.IsCompletedPaid = true;
                mainPurchaseObj.IsPurchase = false;

                entity.MainPurchases.Add(mainPurchaseObj);
                entity.SaveChanges();

                PurchaseDetail purDetailObj = new PurchaseDetail();
                purDetailObj.ProductId = normalProductId;
                purDetailObj.Qty = Convert.ToInt32(txtToQty.Text);
                purDetailObj.CurrentQy = Convert.ToInt32(txtToQty.Text);
                purDetailObj.UnitPrice = Convert.ToInt32(txtNormalPurPrice.Text);
                purDetailObj.IsDeleted = false;
                purDetailObj.Date = DateTime.Now;
                purDetailObj.ConvertQty = 0;
                mainPurchaseObj.PurchaseDetails.Add(purDetailObj);
                entity.SaveChanges();
                #endregion

                #region saving unit conversion table
                UnitConversion _unitConversion = new UnitConversion();
                _unitConversion.FromProductId = Convert.ToInt32(cboFromProduct.SelectedValue);
                _unitConversion.FromQty = Convert.ToInt32(txtFromQty.Text);
                _unitConversion.ToProductId = Convert.ToInt32(cboToProduct.SelectedValue);
                _unitConversion.ToQty = Convert.ToInt32(txtToQty.Text);
                _unitConversion.OnePackQty = Convert.ToInt32(txtPiecePerPack.Text);
                _unitConversion.ConversionDate = DateTime.Now;
                _unitConversion.NormalPurchaseDetailId = mainPurchaseObj.Id;
                _unitConversion.CreatedBy = MemberShip.UserId;
                _unitConversion.CreatedDate = DateTime.Now;
                _unitConversion.NormalUnitPurchasePrice = Convert.ToInt32(txtNormalPurPrice.Text);

                 string _saveMaxPurDetailIdList = string.Join(",", purDetailIdList);
                _unitConversion.MaximumPurchaseDetailId = _saveMaxPurDetailIdList;
                entity.UnitConversions.Add(_unitConversion);
                    entity.SaveChanges();
                #endregion

                #region update product balance
                //update product maximum stock unti balance
                int maxProductId = Convert.ToInt32(cboFromProduct.SelectedValue);
                var maxProduct = (from p in entity.Products where p.Id == maxProductId select p).FirstOrDefault();
                maxProduct.Qty = Convert.ToInt32(lblMaxBalance.Text);
                entity.Entry(maxProduct).State = System.Data.EntityState.Modified;

                //update product normal stock unti balance
               
                var normalProduct = (from p in entity.Products where p.Id == normalProductId select p).FirstOrDefault();
                normalProduct.Qty = Convert.ToInt32(lblNormalBalance.Text);
                entity.Entry(normalProduct).State = System.Data.EntityState.Modified;

                entity.SaveChanges();
                #endregion

                #region Saving Conversion Stock In in Stock Transaction table
                Stock_Transaction st = new Stock_Transaction();
                st.ProductId = normalProductId;
                st.ConversionStockIn = Convert.ToInt32(txtToQty.Text);
                productList.Add(st);

                Save_ConvertQty_ToStockTransaction(productList,true);
                productList.Clear();
                #endregion

                #region Saving Conversion Stock Out in Stock Transaction table
                Stock_Transaction st1 = new Stock_Transaction();
                st1.ProductId = maxProductId;
                st1.ConversionStockOut = Convert.ToInt32(txtFromQty.Text);
                productList.Add(st1);

                Save_ConvertQty_ToStockTransaction(productList,false);
                productList.Clear();
                #endregion

                MessageBox.Show("Successfully Save", "mPOS");
                Clear_Data();
                IsSave = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear_Data();
        }
       #endregion
    }
}
