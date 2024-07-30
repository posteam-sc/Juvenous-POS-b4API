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
using System.Data.Objects;
namespace POS
{
    public partial class ExpenseReport : Form
    {
        #region varitable
        POSEntities entity = new POSEntities();
        bool IsStart = false;
        int expcatId = 0;
        int pId = 0;
        List<object> expReportList = new List<object>();
        #endregion
        public ExpenseReport()
        {
            InitializeComponent();
        }

        private void ExpenseReport_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            List<APP_Data.ExpenseCategory> expcategorylist = new List<APP_Data.ExpenseCategory>();

            APP_Data.ExpenseCategory expcat = new APP_Data.ExpenseCategory();
            expcat.Id = 0;
            expcat.Name = "All";

            expcategorylist.Add(expcat);
            expcategorylist.AddRange((from clist in entity.ExpenseCategories select clist).ToList());
            cboexpensecategory.DataSource = expcategorylist;
            cboexpensecategory.DisplayMember = "Name";
            cboexpensecategory.ValueMember = "Id";
            cboexpensecategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboexpensecategory.AutoCompleteSource = AutoCompleteSource.ListItems;
            this.reportViewer1.Refresh();
            Utility.BindShopALL(cboshoplist);
            cboshoplist.Text = SettingController.DefaultShop.ShopName;
            Utility.ShopComBo_EnableOrNot(cboshoplist);
            IsStart = true;
            loadData();
            this.reportViewer1.RefreshReport();
        }

        private void loadData()
        {
            if(IsStart==true)
            {

                expcatId = 0; pId = 0;
                DateTime fromDate = dtFrom.Value.Date;
                DateTime toDate = dtTo.Value.Date;

                     int shopid = Convert.ToInt32(cboshoplist.SelectedValue);
                string currentshortcode = (from p in entity.Shops where p.Id == shopid select p.ShortCode).FirstOrDefault();
               // string currentshopname = (from p in entity.Shops where p.Id == shopid select p.ShopName).FirstOrDefault();
             

                if(currentshortcode == null)
                {
                    currentshortcode = "";
                }

                

                if (cboexpensecategory.SelectedIndex > 0)
                {
                    expcatId = Convert.ToInt32(cboexpensecategory.SelectedValue);
                }
                IQueryable<object> q = from p in entity.Expenses
                                join j in entity.ExpenseDetails on p.Id equals j.ExpenseId
                                join cat in entity.ExpenseCategories on p.ExpenseCategoryId equals cat.Id
                                       where p.IsDeleted == false
                      && (EntityFunctions.TruncateTime((DateTime)p.ExpenseDate) >= fromDate
                      && EntityFunctions.TruncateTime((DateTime)p.ExpenseDate) <= toDate) 
                         && ((currentshortcode == "" && 1 == 1) || (currentshortcode != "" && p.Id.Substring(2, 2) == currentshortcode))
                      && ((expcatId == 0 && 1 == 1) || (expcatId != 0 && p.ExpenseCategoryId == expcatId))
                                select new
                                {
                                    ExpenseId = p.Id,
                                    Description = j.Description,
                                    ExpenseCategory = cat.Name,
                                    ExpenseDate = p.ExpenseDate,
                                    CreatedUser = p.CreatedUser,
                                    Qty = j.Qty,
                                    Price = j.Price,
                                    Amount=j.Qty*j.Price
                                };
               
                expReportList = new List<object>(q);

                string currentshopname = entity.Shops.Where(x => x.ShortCode == currentshortcode).Select(x => x.ShopName).FirstOrDefault();
                #region New
                ReportDataSource rds = new ReportDataSource();
                rds.Name = "DataSet1";
                rds.Value = expReportList;
                #endregion

                string reportPath = Application.StartupPath + "\\Reports\\expensereport.rdlc";

                reportViewer1.LocalReport.ReportPath = reportPath;
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);

                ReportParameter Header = new ReportParameter("Header", "Expense Report report  from " + dtFrom.Value.Date.ToString("dd/MM/yyyy") + " To " + dtTo.Value.Date.ToString("dd/MM/yyyy") + "For " + currentshopname);
                reportViewer1.LocalReport.SetParameters(Header);

                this.reportViewer1.ZoomMode = ZoomMode.Percent;

                reportViewer1.RefreshReport();
            }
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void cboexpensecategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void cboshoplist_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadData();
        }
    }
}
