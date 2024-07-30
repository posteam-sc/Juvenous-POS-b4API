using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using POS.APP_Data;
namespace POS
{
    public partial class PurchaseOrderItem : Form
    {
        #region Variable

        POSEntities entity = new POSEntities();
        private ToolTip tp = new ToolTip();
        List<Product> productList = new List<Product>();

        #endregion

        #region Event
        public PurchaseOrderItem()
        {
            InitializeComponent();
        }

        private void PurchaseOrderItem_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            List<APP_Data.Brand> BrandList = new List<APP_Data.Brand>();
            APP_Data.Brand brandObj1 = new APP_Data.Brand();
            brandObj1.Id = 0;
            brandObj1.Name = "Select";
            //APP_Data.Brand brandObj2 = new APP_Data.Brand();
            //brandObj2.Id = 1;
            //brandObj2.Name = "None";
            BrandList.Add(brandObj1);
            //BrandList.Add(brandObj2);
            BrandList.AddRange((from bList in entity.Brands select bList).ToList());
            cboBrand.DataSource = BrandList;
            cboBrand.DisplayMember = "Name";
            cboBrand.ValueMember = "Id";
            cboBrand.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboBrand.AutoCompleteSource = AutoCompleteSource.ListItems;

            List<APP_Data.ProductSubCategory> pSubCatList = new List<APP_Data.ProductSubCategory>();
            APP_Data.ProductSubCategory SubCategoryObj1 = new APP_Data.ProductSubCategory();
            SubCategoryObj1.Id = 0;
            SubCategoryObj1.Name = "Select";
            APP_Data.ProductSubCategory SubCategory2 = new APP_Data.ProductSubCategory();
            SubCategory2.Id = 1;
            SubCategory2.Name = "None";
            pSubCatList.Add(SubCategoryObj1);
            pSubCatList.Add(SubCategory2);
            //pSubCatList.AddRange((from c in entity.ProductSubCategories where c.ProductCategoryId == Convert.ToInt32(cboMainCategory.SelectedValue) select c).ToList());
            cboSubCategory.DataSource = pSubCatList;
            cboSubCategory.DisplayMember = "Name";
            cboSubCategory.ValueMember = "Id";
            cboSubCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboSubCategory.AutoCompleteSource = AutoCompleteSource.ListItems;

            List<APP_Data.ProductCategory> pMainCatList = new List<APP_Data.ProductCategory>();
            APP_Data.ProductCategory MainCategoryObj1 = new APP_Data.ProductCategory();
            MainCategoryObj1.Id = 0;
            MainCategoryObj1.Name = "Select";            
            pMainCatList.Add(MainCategoryObj1);            
            pMainCatList.AddRange((from MainCategory in entity.ProductCategories select MainCategory).ToList());
            cboMainCategory.DataSource = pMainCatList;
            cboMainCategory.DisplayMember = "Name";
            cboMainCategory.ValueMember = "Id";
            cboMainCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboMainCategory.AutoCompleteSource = AutoCompleteSource.ListItems;

            this.reportViewer1.RefreshReport();
            LoadData();
        }

        private void cboMainCategory_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboMainCategory.SelectedIndex != 0 )
            {
                int productCategoryId = Int32.Parse(cboMainCategory.SelectedValue.ToString());
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
                cboSubCategory.DataSource = pSubCatList;
                cboSubCategory.DisplayMember = "Name";
                cboSubCategory.ValueMember = "Id";
                cboSubCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cboSubCategory.AutoCompleteSource = AutoCompleteSource.ListItems;
                cboSubCategory.Enabled = true;
                
            }
            
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cboBrand.SelectedIndex = 0;
            cboMainCategory.SelectedIndex = 0;
            cboSubCategory.SelectedIndex = 0;
            LoadData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        #endregion

        #region Function

        private void LoadData()
        {
            Boolean hasError = false;

            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            //Validation

            int MainCategoryId = 0, SubCategoryId = 0, BrandId = 0;
            if (cboMainCategory.SelectedIndex > 0)
            {
                if (cboSubCategory.SelectedIndex == 0)
                {
                    tp.SetToolTip(cboSubCategory, "Error");
                    tp.Show("Please select sub category name!", cboSubCategory);
                    hasError = true;
                }
            }
            if (!hasError)
            {
                if (cboMainCategory.SelectedIndex > 0)
                {
                    MainCategoryId = Convert.ToInt32(cboMainCategory.SelectedValue);
                }
                if (cboSubCategory.SelectedIndex > 0)
                {
                    SubCategoryId = Convert.ToInt32(cboSubCategory.SelectedValue);
                }
                if (cboBrand.SelectedIndex > 0)
                {
                    BrandId = Convert.ToInt32(cboBrand.SelectedValue);
                }

                if (MainCategoryId == 0 && SubCategoryId == 0 && BrandId == 0)
                {
                    productList = (from p in entity.Products where p.Qty <= p.MinStockQty && p.IsNotifyMinStock == true orderby p.Id select p).ToList();
                    ShowReportViewer();
                }
                else if (MainCategoryId > 0 && SubCategoryId == 0 && BrandId == 0)
                {
                    productList = (from p in entity.Products where p.ProductCategoryId == MainCategoryId && p.Qty <= p.MinStockQty && p.IsNotifyMinStock == true orderby p.Id select p).ToList();
                    ShowReportViewer();
                }
                else if (MainCategoryId > 0 && SubCategoryId > 0 && BrandId == 0)
                {
                    if (SubCategoryId == 1)
                    {
                        productList = (from p in entity.Products where p.ProductCategoryId == MainCategoryId && p.ProductSubCategoryId == null && p.Qty <= p.MinStockQty && p.IsNotifyMinStock == true orderby p.Id select p).ToList();
                        ShowReportViewer();
                    }
                    else
                    {
                        productList = (from p in entity.Products where p.ProductCategoryId == MainCategoryId && p.ProductSubCategoryId == SubCategoryId && p.Qty <= p.MinStockQty && p.IsNotifyMinStock == true orderby p.Id select p).ToList();
                        ShowReportViewer();
                    }
                }
                else if (MainCategoryId == 0 && SubCategoryId == 0 && BrandId > 0)
                {
                    if (BrandId == 1)
                    {
                        productList = (from p in entity.Products where p.BrandId == null && p.Qty <= p.MinStockQty && p.IsNotifyMinStock == true orderby p.Id select p).ToList();
                        ShowReportViewer();
                    }
                    else
                    {
                        productList = (from p in entity.Products where p.BrandId == BrandId && p.Qty <= p.MinStockQty && p.IsNotifyMinStock == true orderby p.Id select p).ToList();
                        ShowReportViewer();
                    }
                }
                else if (MainCategoryId > 0 && SubCategoryId > 0 && BrandId > 0)
                {
                     if (BrandId == 1)
                    {
                        if (SubCategoryId == 1)
                        {
                            productList = (from p in entity.Products where p.ProductCategoryId == MainCategoryId && p.ProductSubCategoryId == null && p.BrandId == null && p.Qty <= p.MinStockQty && p.IsNotifyMinStock == true orderby p.Id select p).ToList();
                            ShowReportViewer();
                        }
                        else 
                        {
                            productList = (from p in entity.Products where p.ProductCategoryId == MainCategoryId && p.ProductSubCategoryId == SubCategoryId && p.BrandId == null && p.Qty <= p.MinStockQty && p.IsNotifyMinStock == true orderby p.Id select p).ToList();
                            ShowReportViewer();
                        }
                    }
                    else
                    {
                        if (SubCategoryId == 1)
                        {
                            productList = (from p in entity.Products where p.ProductCategoryId == MainCategoryId && p.ProductSubCategoryId == null && p.BrandId == BrandId && p.Qty <= p.MinStockQty && p.IsNotifyMinStock == true orderby p.Id select p).ToList();
                            ShowReportViewer();
                        }
                        else
                        {
                            productList = (from p in entity.Products where p.ProductCategoryId == MainCategoryId && p.ProductSubCategoryId == SubCategoryId && p.BrandId == BrandId && p.Qty <= p.MinStockQty && p.IsNotifyMinStock == true orderby p.Id select p).ToList();
                            ShowReportViewer();
                        }
                    }
                }
                
            }
            
        }

        private void ShowReportViewer()
        {
            dsReportTemp dsReport = new dsReportTemp();
            dsReportTemp.PurchaseOrderListDataTable dtPurchaseOrderReport = (dsReportTemp.PurchaseOrderListDataTable)dsReport.Tables["PurchaseOrderList"];
            
            foreach (Product p in productList)
            {
                dsReportTemp.PurchaseOrderListRow newRow = dtPurchaseOrderReport.NewPurchaseOrderListRow();
                newRow.ItemId = p.ProductCode;
                newRow.Name = p.Name;
                newRow.Qty = p.Qty.ToString();
                newRow.MinQty = p.MinStockQty.ToString();
                newRow.BrandName = (p.Brand == null) ? "-" : p.Brand.Name;
                dtPurchaseOrderReport.AddPurchaseOrderListRow(newRow);
                
            }

            ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["PurchaseOrderList"]);
            string reportPath = Application.StartupPath + "\\Reports\\ReorderPoint.rdlc";
            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            ReportParameter OrderTitle = new ReportParameter("OrderTitle", gbTitle.Text + " for " + SettingController.ShopName);
            reportViewer1.LocalReport.SetParameters(OrderTitle);
            

            reportViewer1.RefreshReport();
        }
        #endregion

        private void btnPrint_Click(object sender, EventArgs e)
        {
            #region Print

            dsReportTemp dsReport = new dsReportTemp();
            dsReportTemp.PurchaseOrderListDataTable dtPurchaseOrderReport = (dsReportTemp.PurchaseOrderListDataTable)dsReport.Tables["PurchaseOrderList"];

            foreach (Product p in productList)
            {
                dsReportTemp.PurchaseOrderListRow newRow = dtPurchaseOrderReport.NewPurchaseOrderListRow();
                newRow.ItemId = p.ProductCode;
                newRow.Name = p.Name;
                newRow.Qty = p.Qty.ToString();
                newRow.MinQty = p.MinStockQty.ToString();
                newRow.BrandName = (p.Brand == null) ? "-" : p.Brand.Name;
                dtPurchaseOrderReport.AddPurchaseOrderListRow(newRow);

            }

            ReportViewer rv = new ReportViewer();
            ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["PurchaseOrderList"]);
            string reportPath = Application.StartupPath + "\\Reports\\ReorderPoint.rdlc";
            rv.Reset();
            rv.LocalReport.ReportPath = reportPath;
            rv.LocalReport.DataSources.Add(rds);

            ReportParameter OrderTitle = new ReportParameter("OrderTitle", gbTitle.Text + " for " + SettingController.ShopName);
            rv.LocalReport.SetParameters(OrderTitle);
            PrintDoc.PrintReport(rv);

             

            #endregion
        }

      

        
    }
}
