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
using Microsoft.Reporting.WinForms;

using System.Data.Objects;
namespace POS
{
    public partial class ProfitAndLoss_frm : Form
    {
       

        #region Variable
        List<ProfitAndLoss> plist = new List<ProfitAndLoss>();
        List<ProfitAndLoss> plistspecial = new List<ProfitAndLoss>();
        POSEntities entity = new POSEntities();
        int productId, brandId, counterId;
        Int64 totalSaleQty, totalSaleQty2, totalPurchaseAmount, totalPurchaseAmount2, totalSaleAmount, totalSaleAmount2, totalDiscountAmount, totalDiscountAmount2, totalTaxAmount, totalTaxAmount2, totalProfitAndLossAmount, totalProfitAndLossAmount2, ActualProfit;
        Boolean isstart = false;
        public long OVMemberDiscountAmount = 0;
        public long TransactionDiscountAmt = 0;
        #endregion

        #region  Events
        public ProfitAndLoss_frm()
        {
            InitializeComponent();
            CenterToScreen();
        }

        private void ProfitAndLoss_frm_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            
            reportViewer1.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.PageWidth;
            //bind Brand combo box
            List<APP_Data.Brand> BrandList = new List<APP_Data.Brand>();

            APP_Data.Brand brandObj2 = new APP_Data.Brand();
            brandObj2.Id = 0;
            brandObj2.Name = "All";
            BrandList.Add(brandObj2);
            BrandList.AddRange((from bList in entity.Brands select bList).ToList());
            cboBrand.DataSource = BrandList;
            cboBrand.DisplayMember = "Name";
            cboBrand.ValueMember = "Id";
            cboBrand.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboBrand.AutoCompleteSource = AutoCompleteSource.ListItems;

            //bind product combo box
            List<APP_Data.Product> ProductList = new List<APP_Data.Product>();
            APP_Data.Product pdObj2 = new APP_Data.Product();
            pdObj2.Id = 0;
            pdObj2.Name = "All";
            ProductList.Add(pdObj2);
            ProductList.AddRange((from plist in entity.Products where plist.IsConsignment == false select plist).ToList());
            cboProductName.DataSource = ProductList;
            cboProductName.DisplayMember = "Name";
            cboProductName.ValueMember = "Id";
            cboProductName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboProductName.AutoCompleteSource = AutoCompleteSource.ListItems;

            //bind counter combo box
            List<APP_Data.Counter> counterlist = new List<APP_Data.Counter>();
            APP_Data.Counter cObj = new APP_Data.Counter();
            cObj.Id = 0;
            cObj.Name = "All";
            counterlist.Add(cObj);
            counterlist.AddRange((from c in entity.Counters select c).ToList());
            cboCounter.DataSource = counterlist;
            cboCounter.DisplayMember = "Name";
            cboCounter.ValueMember = "Id";
            cboCounter.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboCounter.AutoCompleteSource = AutoCompleteSource.ListItems;

            rdoAll.Checked = true;
            rdoProduct.Checked = false;
            rdoBrand.Checked = false;

            cboBrand.Enabled = false;
            cboProductName.Enabled = false;

            Utility.BindShop(cboshoplist,true);
            //cboshoplist.Text = SettingController.DefaultShop.ShopName;
            cboshoplist.SelectedIndex = 0;
            cboshoplist.Enabled = false;
            //Utility.ShopComBo_EnableOrNot(cboshoplist);
            isstart = true;
            loadData();
        }       

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void cboProductName_SelectedValueChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void cboBrand_SelectedValueChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void cbo_SubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //bind Brand combo box
            List<APP_Data.Brand> BrandList = new List<APP_Data.Brand>();

            APP_Data.Brand brandObj2 = new APP_Data.Brand();
            brandObj2.Id = 0;
            brandObj2.Name = "All";
            BrandList.Add(brandObj2);
            BrandList.AddRange((from bList in entity.Brands select bList).ToList());
            cboBrand.DataSource = BrandList;
            cboBrand.DisplayMember = "Name";
            cboBrand.ValueMember = "Id";
            cboBrand.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboBrand.AutoCompleteSource = AutoCompleteSource.ListItems;

            //bind product combo box
            List<APP_Data.Product> ProductList = new List<APP_Data.Product>();
            APP_Data.Product pdObj2 = new APP_Data.Product();
            pdObj2.Id = 0;
            pdObj2.Name = "All";
            ProductList.Add(pdObj2);
            ProductList.AddRange((from plist in entity.Products select plist).ToList());
            cboProductName.DataSource = ProductList;
            cboProductName.DisplayMember = "Name";
            cboProductName.ValueMember = "Id";
            cboProductName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboProductName.AutoCompleteSource = AutoCompleteSource.ListItems;

            //bind counter combo box
            List<APP_Data.Counter> counterlist = new List<APP_Data.Counter>();
            APP_Data.Counter cObj = new APP_Data.Counter();
            cObj.Id = 0;
            cObj.Name = "All";
            counterlist.Add(cObj);
            counterlist.AddRange((from c in entity.Counters select c).ToList());
            cboCounter.DataSource = counterlist;
            cboCounter.DisplayMember = "Name";
            cboCounter.ValueMember = "Id";
            cboCounter.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboCounter.AutoCompleteSource = AutoCompleteSource.ListItems;

            rdoAll.Checked = true;
            rdoProduct.Checked = false;
            rdoBrand.Checked = false;

            dtFrom.Value = DateTime.Now;
            dtTo.Value = DateTime.Now;

            // cboshoplist.Text = SettingController.DefaultShop.ShopName;
            cboshoplist.SelectedIndex = 0;
          
        }

        private void cbo_Category_SelectedValueChanged(object sender, EventArgs e)
        {
            loadData();

        }

        private void rdoProduct_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoProduct.Checked)
            {
              //  rdoAll.Checked = false;
              //  cboCounter.Enabled = false;
                cboProductName.Enabled = true;
                rdoBrand.Checked = false;
                cboBrand.Enabled = false;
                cboBrand.SelectedIndex = 0;
               // cboCounter.SelectedIndex = 0;
              
            }
        }

        private void rdoBrand_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoBrand.Checked)
            {
                cboBrand.Enabled = true;
            //    rdoAll.Checked = false;
                rdoProduct.Checked = false;
                cboProductName.Enabled = false;
            //    cboCounter.Enabled = false;
              //  rdoCounterName.Checked = false;
                cboProductName.SelectedIndex = 0;
              //  cboCounter.SelectedIndex = 0;
               
            }
        }

        private void rdoAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoAll.Checked)
            {
              //  cboProductName.Enabled = false;
               // cboBrand.Enabled = false;
                rdoCounterName.Checked = false;
                cboCounter.Enabled = false;
               // cboProductName.SelectedIndex = 0;
               // cboBrand.SelectedIndex = 0;
                cboCounter.SelectedIndex = 0;
 
            }
          
        }

        private void rdoCounterName_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoCounterName.Checked)
            {
                rdoAll.Checked = false;
              //  rdoBrand.Checked = false;
               // rdoProduct.Checked = false;
                rdoCounterName.Checked = true;
                cboCounter.Enabled = true;
              //  cboProductName.Enabled = false;
           //     cboBrand.Enabled = false;
           //     cboBrand.SelectedIndex = 0;
                cboCounter.SelectedIndex = 0;
            //    cboProductName.SelectedIndex = 0;

            }

        }

        private void cboCounter_SelectedValueChanged(object sender, EventArgs e)
        {           
            loadData();
        }


        private void cboshoplist_selectedIndexChanged(object sender, EventArgs e)
        {
            loadData();
        }
        #endregion

        #region Functions

        public void loadData()
        {
            if (isstart == true)
            {
                int shopid = Convert.ToInt32(cboshoplist.SelectedValue);
                string currentshortcode = "";
                string currentshopname = "";
                if (shopid!=0)
                {
                     currentshortcode = (from p in entity.Shops where p.Id == shopid select p.ShortCode).FirstOrDefault();
                     currentshopname = (from p in entity.Shops where p.Id == shopid select p.ShopName).FirstOrDefault();
                }
                else
                {
                    currentshortcode = "0";
                    currentshopname = "ALL";
                }
              

                plist.Clear();
                plistspecial.Clear();
                productId = 0; brandId = 0; counterId = 0;
                totalSaleQty = 0; totalPurchaseAmount = 0; totalSaleAmount = 0; totalDiscountAmount = 0; totalTaxAmount = 0; totalProfitAndLossAmount = 0;
                totalSaleQty2 = 0; totalPurchaseAmount2 = 0; totalSaleAmount2 = 0; totalDiscountAmount2 = 0; totalTaxAmount2 = 0; totalProfitAndLossAmount2 = 0;
                DateTime fromDate = dtFrom.Value.Date;
                DateTime toDate = dtTo.Value.Date;


                if (rdoProduct.Checked)
                {
                    if (cboProductName.SelectedIndex > 0)
                    {
                        productId = Convert.ToInt32(cboProductName.SelectedValue);
                    }
                }
                if (rdoBrand.Checked)
                {
                    if (cboBrand.SelectedIndex > 0)
                    {
                        brandId = Convert.ToInt32(cboBrand.SelectedValue);
                    }
                }
                if (rdoCounterName.Checked)
                {
                    if (cboCounter.SelectedIndex > 0)
                    {
                        counterId = Convert.ToInt32(cboCounter.SelectedValue);
                    }

                }

              

                System.Data.Objects.ObjectResult<GetProfitandLoss_Result> prlist = entity.GetProfitandLoss(fromDate, toDate, brandId, productId, counterId,currentshortcode,false);

                System.Data.Objects.ObjectResult<GetProfitandLoss_Result> prlistspecial = entity.GetProfitandLoss(fromDate, toDate, brandId, productId, counterId, currentshortcode,true);
                foreach (GetProfitandLoss_Result pr in prlist)
                {
                    ProfitAndLoss pfl = new ProfitAndLoss();
                    pfl.SaleDate = Convert.ToDateTime(pr.SaleDate);
                    pfl.TotalSaleQty = Convert.ToInt32(pr.TotalSaleQty);
                    pfl.TotalPurchaseAmount = Convert.ToInt64(pr.TotalPurchaseAmount);
                    pfl.TotalSaleAmount = Convert.ToInt64(pr.TotalSaleAmount);
                    pfl.TotalDiscountAmount = Convert.ToInt64(pr.TotalDiscountAmount);
                    pfl.TotalTaxAmount = Convert.ToInt64(pr.TotalTaxAmount);
                    pfl.ProfitAndLossAmount = Convert.ToInt64(pr.Total_ProfitAndLoss);

                    totalSaleQty += pfl.TotalSaleQty;
                    totalPurchaseAmount += pfl.TotalPurchaseAmount;
                    totalSaleAmount += pfl.TotalSaleAmount;
                    totalDiscountAmount += pfl.TotalDiscountAmount;
                    totalTaxAmount += pfl.TotalTaxAmount;
                    totalProfitAndLossAmount += pfl.ProfitAndLossAmount;
                    plist.Add(pfl);
                }

                foreach (GetProfitandLoss_Result pr in prlistspecial)
                {
                    ProfitAndLoss pflSpecial = new ProfitAndLoss();
                    pflSpecial.SaleDate = Convert.ToDateTime(pr.SaleDate);
                    pflSpecial.TotalSaleQty = Convert.ToInt32(pr.TotalSaleQty);
                    pflSpecial.TotalPurchaseAmount = Convert.ToInt64(pr.TotalPurchaseAmount);
                    pflSpecial.TotalSaleAmount = Convert.ToInt64(pr.TotalSaleAmount);
                    pflSpecial.TotalDiscountAmount = Convert.ToInt64(pr.TotalDiscountAmount);
                    pflSpecial.TotalTaxAmount = Convert.ToInt64(pr.TotalTaxAmount);
                    pflSpecial.ProfitAndLossAmount = Convert.ToInt64(pr.Total_ProfitAndLoss);

                    totalSaleQty2 += pflSpecial.TotalSaleQty;
                    totalPurchaseAmount2 += pflSpecial.TotalPurchaseAmount;
                    totalSaleAmount2 += pflSpecial.TotalSaleAmount;
                    totalDiscountAmount2 += pflSpecial.TotalDiscountAmount;
                    totalTaxAmount2 += pflSpecial.TotalTaxAmount;
                    totalProfitAndLossAmount2 += pflSpecial.ProfitAndLossAmount;
                    plistspecial.Add(pflSpecial);
                }
                
                List<Transaction> trans = (from t in entity.Transactions
                                           join td in entity.TransactionDetails on t.Id equals td.TransactionId
                                           join pt in entity.PurchaseDetailInTransactions on td.Id equals pt.TransactionDetailId
                                           join p in entity.Products on td.ProductId equals p.Id
                                           join b in entity.Brands on p.BrandId equals b.Id

                                           where td.IsDeleted == false && t.IsDeleted==false 
                                                 && (EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate
                                                 && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate)
                                                 && ((brandId == 0 && 1 == 1) || (brandId != 0 && b.Id == brandId))
                      && ((productId == 0 && 1 == 1) || (productId != 0 && p.Id == productId))
                      && ((counterId == 0 && 1 == 1) || (counterId != 0 && t.CounterId == counterId))
                      && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1))

                                           select t).Distinct().ToList();

                long overalldis = Convert.ToInt64(trans.Sum(x => x.DiscountAmount));
                TransactionDiscountAmt = overalldis - totalDiscountAmount - totalDiscountAmount2;
                OVMemberDiscountAmount = Convert.ToInt64(trans.Sum(x => x.MCDiscountAmt + x.BDDiscountAmt));
                 ActualProfit = (totalProfitAndLossAmount +totalProfitAndLossAmount2)- TransactionDiscountAmt-OVMemberDiscountAmount;
           
                ShowReportViewer(currentshopname);
            }
        }

        private void ShowReportViewer(string currentshopname)
        {
            string ProductName = ""; string BrandName = ""; string CounterName = "";

            if (rdoAll.Checked)
            {
                ProductName = " All ";
            }
            if (rdoProduct.Checked)
            {
                if (productId == 0)
                {
                    ProductName = "All Product ";
                }
                else
                {
                    APP_Data.Product p = entity.Products.Where(x => x.Id == productId).FirstOrDefault();
                    ProductName = " Product - " + p.Name.ToString();
                } 
            }
            if (rdoBrand.Checked)
            {
                if (brandId == 0)
                {
                    BrandName = " All Brand ";
                }
                else
                {
                    APP_Data.Brand b = entity.Brands.Where(x => x.Id == brandId).FirstOrDefault();
                    BrandName = " Brand - " + b.Name.ToString();
                }                
            }
            if (rdoCounterName.Checked)
            {
                if (counterId == 0)
                {
                    CounterName = " All ";
                }
                else
                {
                    APP_Data.Counter c = entity.Counters.Where(x => x.Id == counterId).FirstOrDefault();
                    CounterName = "Counter - " + c.Name.ToString();   
                }                           
            }

            dsReportTemp dsReport = new dsReportTemp();
            dsReportTemp.ProfitAndLossTableDataTable dtPdReport = (dsReportTemp.ProfitAndLossTableDataTable)dsReport.Tables["ProfitAndLossTable"];
            foreach (ProfitAndLoss pdCon in plist)
            {
                dsReportTemp.ProfitAndLossTableRow newRow = dtPdReport.NewProfitAndLossTableRow();
                newRow.SaleDate = Convert.ToDateTime(pdCon.SaleDate);
                newRow.TotalSaleQty = pdCon.TotalSaleQty.ToString();
                newRow.TotalPurchaseAmount = pdCon.TotalPurchaseAmount.ToString();
                newRow.TotalSaleAmount = pdCon.TotalSaleAmount.ToString();
                newRow.TotalDiscountAmount = pdCon.TotalDiscountAmount.ToString();
                newRow.TotalTaxAmount = pdCon.TotalTaxAmount.ToString();
                newRow.TotalProfitAndLossAmount = pdCon.ProfitAndLossAmount.ToString();
                dtPdReport.AddProfitAndLossTableRow(newRow);
            }
            dsReportTemp.ProfitAndLossTableSpecialDataTable dtPdReportSpecial = (dsReportTemp.ProfitAndLossTableSpecialDataTable)dsReport.Tables["ProfitAndLossTableSpecial"];
            foreach (ProfitAndLoss pdCon in plistspecial)
            {
                dsReportTemp.ProfitAndLossTableSpecialRow newRow = dtPdReportSpecial.NewProfitAndLossTableSpecialRow();
                newRow.SaleDate = Convert.ToDateTime(pdCon.SaleDate);
                newRow.TotalSaleQty = pdCon.TotalSaleQty.ToString();
                newRow.TotalPurchaseAmount = pdCon.TotalPurchaseAmount.ToString();
                newRow.TotalSaleAmount = pdCon.TotalSaleAmount.ToString();
                newRow.TotalDiscountAmount = pdCon.TotalDiscountAmount.ToString();
                newRow.TotalTaxAmount = pdCon.TotalTaxAmount.ToString();
                newRow.TotalProfitAndLossAmount = pdCon.ProfitAndLossAmount.ToString();
                dtPdReportSpecial.AddProfitAndLossTableSpecialRow(newRow);
            }
            ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["ProfitAndLossTable"]);
            ReportDataSource rds2 = new ReportDataSource("DataSet2", dsReport.Tables["ProfitAndLossTableSpecial"]);
            string reportPath = Application.StartupPath + "\\Reports\\ProfitAndLossReport.rdlc";

            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.LocalReport.DataSources.Add(rds2);
            //ReportParameter ShopName = new ReportParameter("ShopName", "Profit and Loss Report for " + currentshopname);
            //reportViewer1.LocalReport.SetParameters(ShopName);

            ReportParameter TotalSaleQty = new ReportParameter("TotalSaleQty", totalSaleQty.ToString());
            reportViewer1.LocalReport.SetParameters(TotalSaleQty);

            ReportParameter TotalSaleQty2 = new ReportParameter("TotalSaleQty2", totalSaleQty2.ToString());
            reportViewer1.LocalReport.SetParameters(TotalSaleQty2);

            ReportParameter TotalPurchaseAmount = new ReportParameter("TotalPurchaseAmount", totalPurchaseAmount.ToString());
            reportViewer1.LocalReport.SetParameters(TotalPurchaseAmount);

            ReportParameter TotalPurchaseAmount2 = new ReportParameter("TotalPurchaseAmount2", totalPurchaseAmount2.ToString());
            reportViewer1.LocalReport.SetParameters(TotalPurchaseAmount2);

            ReportParameter TotalSaleAmount = new ReportParameter("TotalSaleAmount", totalSaleAmount.ToString());
            reportViewer1.LocalReport.SetParameters(TotalSaleAmount);

            ReportParameter TotalSaleAmount2 = new ReportParameter("TotalSaleAmount2", totalSaleAmount2.ToString());
            reportViewer1.LocalReport.SetParameters(TotalSaleAmount2);

            ReportParameter TotalDiscountAmount = new ReportParameter("TotalDiscountAmount", totalDiscountAmount.ToString());
            reportViewer1.LocalReport.SetParameters(TotalDiscountAmount);

            ReportParameter TotalDiscountAmount2 = new ReportParameter("TotalDiscountAmount2", totalDiscountAmount2.ToString());
            reportViewer1.LocalReport.SetParameters(TotalDiscountAmount2);

            ReportParameter TotalTaxAmount = new ReportParameter("TotalTaxAmount", totalTaxAmount.ToString());
            reportViewer1.LocalReport.SetParameters(TotalTaxAmount);

            ReportParameter TotalTaxAmount2 = new ReportParameter("TotalTaxAmount2", totalTaxAmount2.ToString());
            reportViewer1.LocalReport.SetParameters(TotalTaxAmount2);

            ReportParameter TotalProfitAndLossAmount = new ReportParameter("TotalProfitAndLossAmount", totalProfitAndLossAmount.ToString());
            reportViewer1.LocalReport.SetParameters(TotalProfitAndLossAmount);

            ReportParameter TotalProfitAndLossAmount2 = new ReportParameter("TotalProfitAndLossAmount2", totalProfitAndLossAmount2.ToString());
            reportViewer1.LocalReport.SetParameters(TotalProfitAndLossAmount2);

            ReportParameter transdiscount = new ReportParameter("transdiscount", TransactionDiscountAmt.ToString());
            reportViewer1.LocalReport.SetParameters(transdiscount);
            ReportParameter OVMemberDiscount = new ReportParameter("OVMemberDiscount", OVMemberDiscountAmount.ToString());
            reportViewer1.LocalReport.SetParameters(OVMemberDiscount);

            ReportParameter actualprofit = new ReportParameter("actualprofit", ActualProfit.ToString());
            reportViewer1.LocalReport.SetParameters(actualprofit);

            ReportParameter Header = new ReportParameter("Header", "Gross Profit / Loss Report (Normal Product)" + ProductName + BrandName + CounterName + " at " + DateTime.Now.Date.ToString("dd-MM-yyyy"));
            reportViewer1.LocalReport.SetParameters(Header);

            ReportParameter Header2 = new ReportParameter("Header2", "Gross Profit / Loss Report (Special Product)" + ProductName + BrandName + CounterName +  " at " + DateTime.Now.Date.ToString("dd-MM-yyyy"));
            reportViewer1.LocalReport.SetParameters(Header2);
            reportViewer1.RefreshReport();
        }

        #endregion

     

        

       
        
    }
}

