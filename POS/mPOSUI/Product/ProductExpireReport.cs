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

namespace POS
{
    public partial class ProductExpireReport : Form
    {
        POSEntities entity = new POSEntities();
        public ProductExpireReport()
        {
            InitializeComponent();
        }

        private void ProductExpireReport_Load(object sender, EventArgs e)
        {
            txtCurrentDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            FormRefresh();
            this.rvExpire.RefreshReport();
        }
        struct ExpireIn
        {
            public int Key { get; set; }
            public string Value { get; set; }

            public static List<ExpireIn> ExpireStructureList()
            {
                List<ExpireIn> expireList = new List<ExpireIn>();
                expireList.Add(new ExpireIn { Key = 0, Value = "Already Expire" });
                expireList.Add(new ExpireIn { Key = 1, Value = "1 Month" });
                expireList.Add(new ExpireIn { Key = 2, Value = "2 Months" });
                expireList.Add(new ExpireIn { Key = 3, Value = "3 Months" });
                expireList.Add(new ExpireIn { Key = 6, Value = "6 Months" });
                return expireList;
            }
        }

        void LoadExpireIn()
        {
            cboExpireDay.DisplayMember = "Value";
            cboExpireDay.ValueMember = "Key";
            cboExpireDay.DataSource = ExpireIn.ExpireStructureList();
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


        private void rdoQuantity_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            txtCurrentDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            LoadData();
           
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            FormRefresh();
        }
        void FormRefresh()
        {
            LoadBrand(); LoadCategory(); LoadSubCategory(); LoadExpireIn();
            rvExpire.LocalReport.DataSources.Clear();
            txtCurrentDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            LoadData();
        }
        void LoadData()
        {
            int brand = (int)cboBrand.SelectedValue;
            int category = (int)cboCategory.SelectedValue;
            int subcategory = (int)cboSubCatgory.SelectedValue;
            int expirein = (int)cboExpireDay.SelectedValue;
            DateTime currentDate = DateTime.Now;
            DateTime expireDate = currentDate.AddMonths(expirein);






            var closeExpireList = (from p in entity.Products
                                   from pd in entity.PurchaseDetails
                                   where p.Id == pd.ProductId && (p.IsDiscontinue == false || p.IsDiscontinue == null) && pd.IsDeleted == false && pd.expiredDate != null && pd.CurrentQy > 0
                                   && (((expireDate > currentDate) && (pd.expiredDate.Value >= currentDate && pd.expiredDate <= expireDate)) ||
                                   ((expireDate == currentDate) && (pd.expiredDate.Value < currentDate))) &&
                                   ((brand == 0 && 1 == 1) || (brand > 0 && p.BrandId == brand)) && ((category == 0 && 1 == 1) || (category > 0 && p.ProductCategoryId == category)) &&
                                   ((subcategory == 0 && 1 == 1) || (subcategory > 0 && p.ProductSubCategoryId == subcategory))
                                   select new
                                   {
                                       Barcode = p.Barcode,
                                       ProductCode = p.ProductCode,
                                       ProductName = p.Name,
                                       ExpirationDate = pd.expiredDate,
                                       TotalQty = pd.CurrentQy,
                                       TotalValue = pd.UnitPrice * pd.CurrentQy
                                   }).GroupBy(b => new { b.Barcode, b.ProductCode, b.ProductName, b.ExpirationDate })
                                    .Select
                                    (c => new
                                    {
                                        BarCode = c.Key.Barcode,
                                        ProductCode = c.Key.ProductCode,
                                        ProductName = c.Key.ProductName,
                                        ExpirationDate = c.Min(d => d.ExpirationDate),
                                        TotalQty = c.Sum(d => d.TotalQty),
                                        TotalValue = c.Sum(d => d.TotalValue)
                                    }).OrderBy(a => a.ExpirationDate).ToList();

            //DataSetName StockExpire
            //ProductExpiration.rdlc
            dsReportTemp dsReport = new dsReportTemp();
            dsReportTemp.StockExpireDataTable stockexpiretable = (dsReportTemp.StockExpireDataTable)dsReport.Tables["StockExpire"];

            foreach (var e in closeExpireList)
            {
                dsReportTemp.StockExpireRow row = stockexpiretable.NewStockExpireRow();
                row.Barcode = e.BarCode;
                row.ProductCode = e.ProductCode;
                row.ProductName = e.ProductName;
                row.ExpirationDate = e.ExpirationDate.Value.Date.ToString("dd-MM-yyyy");
                row.TotalQty = e.TotalQty.ToString();
                row.TotalValue = e.TotalValue.ToString();
                stockexpiretable.AddStockExpireRow(row);
            }

            ReportDataSource rds = new ReportDataSource("StockExpire", dsReport.Tables["StockExpire"]);
            string reportPath = Application.StartupPath + "\\Reports\\ProductExpiration.rdlc";

            rvExpire.LocalReport.ReportPath = reportPath;

            rvExpire.LocalReport.DataSources.Clear();
            rvExpire.LocalReport.DataSources.Add(rds);

            ReportParameter alltotal = new ReportParameter("AllTotal", closeExpireList.Sum(a => a.TotalValue).ToString());
            rvExpire.LocalReport.SetParameters(alltotal);

            ReportParameter curdate = new ReportParameter("CurrentDate", currentDate.Date.ToString("dd-MM-yyyy"));
            rvExpire.LocalReport.SetParameters(curdate);

            ReportParameter filtermonth = new ReportParameter("FilterMonth", ExpireIn.ExpireStructureList().Where(a => a.Key == expirein).First().Value);
            rvExpire.LocalReport.SetParameters(filtermonth);

            rvExpire.RefreshReport();
        }
    }
}
