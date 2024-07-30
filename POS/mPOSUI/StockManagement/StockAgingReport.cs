using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using POS.APP_Data;
using Microsoft.Reporting.WinForms;

namespace POS
{
    public partial class StockAgingReport : Form
    {
        POSEntities entity = new POSEntities();
        List<AgingReport> agelist = new List<AgingReport>();
        public StockAgingReport()
        {
            InitializeComponent();
        }

        private void StockAgingReport_Load(object sender, EventArgs e)
        {
            txtCurrentDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            FormRefresh();
            rvExpire.LocalReport.Refresh();
           
        }
        class AgingReport
        {
            public string Barcode {get; set;}
            public string Productcode {get; set;}
            public string ProductName {get; set;}
            public int? TotalQty {get; set;}
            public decimal? TotalValue {get; set;}
            public int? ZOneFilterQty = 0;
            public decimal? ZOneFilterValue = 0;
            public int? OTwoFilterQty = 0;
            public decimal? OTwoFilterValue = 0;
            public int? TThreeFilterQty = 0;
            public decimal? TThreeFilterValue = 0;
            public int? TSixFilterQty = 0;
            public decimal? TSixFilterValue = 0;
            public int? UnknownQty = 0;
            public decimal? UnknownValue = 0;
        }
        void LoadBrand()
        {
            List<APP_Data.Brand> BrandList = new List<APP_Data.Brand>();
            APP_Data.Brand brandObj1 = new APP_Data.Brand();
            brandObj1.Id = 0;
            brandObj1.Name = "Select";

            BrandList.Add(brandObj1);
            BrandList.AddRange((from bList in entity.Brands where bList.IsDelete == false select bList).ToList());
            cboBrand.DataSource = BrandList;
            cboBrand.DisplayMember = "Name";
            cboBrand.ValueMember = "Id";
            cboBrand.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboBrand.AutoCompleteSource = AutoCompleteSource.ListItems;
        }
        void LoadCategory()
        {
            List<APP_Data.ProductCategory> pMainCatList = new List<APP_Data.ProductCategory>();
            APP_Data.ProductCategory MainCategoryObj1 = new APP_Data.ProductCategory();
            MainCategoryObj1.Id = 0;
            MainCategoryObj1.Name = "Select";
            pMainCatList.Add(MainCategoryObj1);
            pMainCatList.AddRange((from MainCategory in entity.ProductCategories where MainCategory.IsDelete == false select MainCategory).ToList());
            cboCategory.DataSource = pMainCatList;
            cboCategory.DisplayMember = "Name";
            cboCategory.ValueMember = "Id";
            cboCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboCategory.AutoCompleteSource = AutoCompleteSource.ListItems;
        }
        void FormRefresh()
        {
            LoadBrand(); LoadCategory(); LoadSubCategory();
            rvExpire.LocalReport.DataSources.Clear();
            txtCurrentDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            if (!Loader.IsBusy)
            {
                int brand = (int)cboBrand.SelectedValue;
                int category = (int)cboCategory.SelectedValue;
                int subcategory = (int)cboSubCatgory.SelectedValue;
                Loader.RunWorkerAsync(new object[] {brand,category,subcategory});
            }
           
        }
        void LoadSubCategory()
        {
            List<APP_Data.ProductSubCategory> pSubCatList = new List<APP_Data.ProductSubCategory>();
            APP_Data.ProductSubCategory SubCategoryObj1 = new APP_Data.ProductSubCategory();
            SubCategoryObj1.Id = 0;
            SubCategoryObj1.Name = "Select";
            pSubCatList.Add(SubCategoryObj1);
            cboSubCatgory.DataSource = pSubCatList;
            cboSubCatgory.DisplayMember = "Name";
            cboSubCatgory.ValueMember = "Id";
        }

        void LoadData()
        {
            
            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            txtCurrentDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            agelist = new List<AgingReport>();
                if (!Loader.IsBusy)
                {
                    int brand = (int)cboBrand.SelectedValue;
                    int category = (int)cboCategory.SelectedValue;
                    int subcategory = (int)cboSubCatgory.SelectedValue;
                    Loader.RunWorkerAsync(new object[] { brand, category, subcategory });
                }
           
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            FormRefresh();
        }

        private void Loader_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] obj = e.Argument as Object[];
            int brand = (int)obj[0]; /*(int)cboBrand.SelectedValue;*/
            int category = (int)obj[1];// (int)cboCategory.SelectedValue;
            int subcategory = (int)obj[2];// (int)cboSubCatgory.SelectedValue;
            DateTime currentDate = DateTime.Now; //DateTime.Parse(txtCurrentDate.Text);
            DateTime OneDate = currentDate.AddMonths(-1);
            DateTime TwoDate = currentDate.AddMonths(-2);
            DateTime ThreeDate = currentDate.AddMonths(-3);
            DateTime SixDate = currentDate.AddMonths(-6);


            var GeneralProductList = (from p in entity.Products
                                      join pd in entity.PurchaseDetails on p.Id equals pd.ProductId 
                                      join mp in entity.MainPurchases on pd.MainPurchaseId equals mp.Id
                                      where p.Id == pd.ProductId && (p.IsDiscontinue == false || p.IsDiscontinue == null) && pd.IsDeleted == false && pd.CurrentQy > 0 &&
                                      ((brand == 0 && 1 == 1) || (brand > 0 && p.BrandId == brand)) && ((category == 0 && 1 == 1) || (category > 0 && p.ProductCategoryId == category)) &&
                                      ((subcategory == 0 && 1 == 1) || (subcategory > 0 && p.ProductSubCategoryId == subcategory)) && mp.IsCompletedInvoice==true
                                      select new
                                      {
                                          BarCode = p.Barcode,
                                          ProductCode = p.ProductCode,
                                          ProductName = p.Name,
                                          Unit = p.Unit.UnitName,
                                          Location = p.ProductLocation,
                                          StockDate = pd.Date,
                                          Qty = pd.CurrentQy,
                                          Value = pd.UnitPrice * pd.CurrentQy
                                      });

            var grandtotallistFromgeneral = GeneralProductList.GroupBy(x => new { x.BarCode, x.ProductCode, x.ProductName })
                .Select(c => new AgingReport
                {
                    Barcode = c.Key.BarCode,
                    Productcode = c.Key.ProductCode,
                    ProductName = c.Key.ProductName,
                    TotalQty = c.Sum(d => d.Qty),
                    TotalValue = c.Sum(d => d.Value),
                }).ToList();
            agelist.AddRange(grandtotallistFromgeneral);
            var ZOList = GeneralProductList.AsEnumerable().Where(pd => (pd.StockDate.Value.Date >= OneDate.Date && pd.StockDate.Value.Date <= currentDate.Date))
                                    .GroupBy(b => new { b.BarCode, b.ProductCode, b.ProductName })
                                   .Select
                                   (c => new AgingReport
                                   {
                                       Barcode = c.Key.BarCode,
                                       Productcode = c.Key.ProductCode,
                                       ProductName = c.Key.ProductName,
                                       ZOneFilterQty = c.Sum(d => d.Qty),
                                       ZOneFilterValue = c.Sum(d => d.Value),
                                   }).ToList();
            agelist.AddRange(ZOList);
            var OTList = GeneralProductList.AsEnumerable().Where(pd => (pd.StockDate.Value.Date >= TwoDate.Date && pd.StockDate.Value.Date < OneDate.Date))
                .GroupBy(b => new { b.BarCode, b.ProductCode, b.ProductName })
                                .Select
                                (c => new AgingReport
                                {
                                    Barcode = c.Key.BarCode,
                                    Productcode = c.Key.ProductCode,
                                    ProductName = c.Key.ProductName,
                                    OTwoFilterQty = c.Sum(d => d.Qty),
                                    OTwoFilterValue = c.Sum(d => d.Value),
                                }).ToList();
            agelist.AddRange(OTList);
            var TTList = GeneralProductList.AsEnumerable().Where(pd => (pd.StockDate.Value.Date >= ThreeDate.Date && pd.StockDate.Value.Date < TwoDate.Date)
                            ).GroupBy(b => new { b.BarCode, b.ProductCode, b.ProductName })
                                .Select
                                (c => new AgingReport
                                {
                                    Barcode = c.Key.BarCode,
                                    Productcode = c.Key.ProductCode,
                                    ProductName = c.Key.ProductName,
                                    TThreeFilterQty = c.Sum(d => d.Qty),
                                    TThreeFilterValue = c.Sum(d => d.Value),
                                }).ToList();
            agelist.AddRange(TTList);
            var TSList = GeneralProductList.AsEnumerable().Where(pd => (pd.StockDate.Value.Date >= SixDate.Date && pd.StockDate.Value.Date < ThreeDate.Date))
            .GroupBy(b => new { b.BarCode, b.ProductCode, b.ProductName })
                                .Select
                                (c => new AgingReport
                                {
                                    Barcode = c.Key.BarCode,
                                    Productcode = c.Key.ProductCode,
                                    ProductName = c.Key.ProductName,
                                    TSixFilterQty = c.Sum(d => d.Qty),
                                    TSixFilterValue = c.Sum(d => d.Value),
                                }).ToList();
            agelist.AddRange(TSList);
            var SUList = GeneralProductList.Where(pd => (pd.StockDate.Value < SixDate))
            .GroupBy(b => new { b.BarCode, b.ProductCode, b.ProductName })
                                .Select
                                (c => new AgingReport
                                {
                                    Barcode = c.Key.BarCode,
                                    Productcode = c.Key.ProductCode,
                                    ProductName = c.Key.ProductName,
                                    UnknownQty = c.Sum(d => d.Qty),
                                    UnknownValue = c.Sum(d => d.Value),
                                }).ToList();

            agelist.AddRange(SUList);

            List<AgingReport> finallist = agelist.GroupBy(a => new { a.Barcode, a.Productcode, a.ProductName })
                .Select(b => new AgingReport
                {
                    Barcode = b.Key.Barcode,
                    Productcode = b.Key.Productcode,
                    ProductName = b.Key.ProductName,
                    TotalQty = b.Sum(c => c.TotalQty),
                    TotalValue = b.Sum(c => c.TotalValue),
                    ZOneFilterQty = b.Sum(c => c.ZOneFilterQty),
                    ZOneFilterValue = b.Sum(c => c.ZOneFilterValue),
                    OTwoFilterQty = b.Sum(c => c.OTwoFilterQty),
                    OTwoFilterValue = b.Sum(c => c.OTwoFilterValue),
                    TThreeFilterQty = b.Sum(c => c.TThreeFilterQty),
                    TThreeFilterValue = b.Sum(c => c.TThreeFilterValue),
                    TSixFilterQty = b.Sum(c => c.TSixFilterQty),
                    TSixFilterValue = b.Sum(c => c.TSixFilterValue),
                    UnknownQty = b.Sum(c => c.UnknownQty),
                    UnknownValue = b.Sum(c => c.UnknownValue)
                }).ToList();
            e.Result = finallist;
        }

        private void Loader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            List<AgingReport> finallist = e.Result as List<AgingReport>;
            if (finallist.Count() > 0)
            {
                dsReportTemp ds = new dsReportTemp();
                dsReportTemp.StockAgingDataTable stockagetable = (dsReportTemp.StockAgingDataTable)ds.Tables["StockAging"];
                foreach (var f in finallist)
                {
                    dsReportTemp.StockAgingRow row = stockagetable.NewStockAgingRow();
                    row.Barcode = f.Barcode;
                    row.ProductCode = f.Productcode;
                    row.ProductName = f.ProductName;
                    row.TotalQty = f.TotalQty.ToString();
                    row.TotalValue = f.TotalValue.ToString();
                    row.ZOneFilterQty = f.ZOneFilterQty.ToString();
                    row.ZOneFilterValue = f.ZOneFilterValue.ToString();
                    row.OTwoFilterQty = f.OTwoFilterQty.ToString();
                    row.OTwoFilterValue = f.OTwoFilterValue.ToString();
                    row.TThreeFilterQty = f.TThreeFilterQty.ToString();
                    row.TThreeFilterValue = f.TThreeFilterValue.ToString();
                    row.TSixFilterQty = f.TSixFilterQty.ToString();
                    row.TSixFilterValue = f.TSixFilterValue.ToString();
                    row.UnknownQty = f.UnknownQty.ToString();
                    row.UnknownValue = f.UnknownValue.ToString();
                    stockagetable.AddStockAgingRow(row);

                }

                ReportDataSource rds = new ReportDataSource("StockAging", ds.Tables["StockAging"]);
                string reportpath = Application.StartupPath + "\\Reports\\StockAging.rdlc";
                rvExpire.LocalReport.ReportPath = reportpath;
                rvExpire.LocalReport.DataSources.Clear();
                rvExpire.LocalReport.DataSources.Add(rds);

                ReportParameter currentDay = new ReportParameter("CurrentDate", DateTime.Now.ToString("dd-MM-yyyy"));
                rvExpire.LocalReport.SetParameters(currentDay);

                rvExpire.RefreshReport();
            }
        }
    }
}
