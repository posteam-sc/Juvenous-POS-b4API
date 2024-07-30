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
    public partial class TransactionDetailByItem : Form
    {

        #region Variable

        POSEntities entity = new POSEntities();
        private ToolTip tp = new ToolTip();
        List<Product> productList = new List<Product>();
        //System.Data.Objects.ObjectResult<TransactionDetailByItem_Result> resultList;
        List<TransactionDetailByItemHolder> DetailLists = new List<TransactionDetailByItemHolder>();
        System.Data.Objects.ObjectResult<TransactionDetailReport_Result> resultList;
        Boolean isstart = false;
        #endregion

        #region Event
        public TransactionDetailByItem()
        {
            InitializeComponent();

        }

        private void TransactionDetailByItem_Load(object sender, EventArgs e)
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
            //APP_Data.ProductSubCategory SubCategory2 = new APP_Data.ProductSubCategory();
            //SubCategory2.Id = 1;
            //SubCategory2.Name = "None";
            pSubCatList.Add(SubCategoryObj1);
            //pSubCatList.Add(SubCategory2);
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

            List<APP_Data.Counter> counterList = new List<APP_Data.Counter>();
            APP_Data.Counter ctObj = new APP_Data.Counter();
            ctObj.Id = 0;
            ctObj.Name = "Select";
            counterList.Add(ctObj);
            counterList.AddRange((from ct in entity.Counters select ct).ToList());
            cboCounter.DataSource = counterList;
            cboCounter.DisplayMember = "Name";
            cboCounter.ValueMember = "Id";
            cboCounter.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboCounter.AutoCompleteSource = AutoCompleteSource.ListItems;


            Utility.BindShop(cboshoplist, true);
            cboshoplist.Text = SettingController.DefaultShop.ShopName;
            Utility.ShopComBo_EnableOrNot(cboshoplist);
            isstart = true;
            cboshoplist.SelectedIndex = 0;
            this.reportViewer1.RefreshReport();
            LoadData();
        }

        private void cboMainCategory_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboMainCategory.SelectedIndex != 0)
            {
                int productCategoryId = Int32.Parse(cboMainCategory.SelectedValue.ToString());
                List<APP_Data.ProductSubCategory> pSubCatList = new List<APP_Data.ProductSubCategory>();
                APP_Data.ProductSubCategory SubCategoryObj1 = new APP_Data.ProductSubCategory();
                SubCategoryObj1.Id = 0;
                SubCategoryObj1.Name = "Select";
                APP_Data.ProductSubCategory SubCategoryObj2 = new APP_Data.ProductSubCategory();
                //SubCategoryObj2.Id = 1;
                //SubCategoryObj2.Name = "None";
                pSubCatList.Add(SubCategoryObj1);
                //pSubCatList.Add(SubCategoryObj2);
                pSubCatList.AddRange((from c in entity.ProductSubCategories where c.ProductCategoryId == productCategoryId select c).ToList());
                cboSubCategory.DataSource = pSubCatList;
                cboSubCategory.DisplayMember = "Name";
                cboSubCategory.ValueMember = "Id";
                cboSubCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cboSubCategory.AutoCompleteSource = AutoCompleteSource.ListItems;
                cboSubCategory.Enabled = true;

            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            rdbSale.Checked = true;
            cboBrand.SelectedIndex = 0;
            List<APP_Data.ProductSubCategory> pSubCatList = new List<APP_Data.ProductSubCategory>();
            APP_Data.ProductSubCategory SubCategoryObj1 = new APP_Data.ProductSubCategory();
            SubCategoryObj1.Id = 0;
            SubCategoryObj1.Name = "Select";
            //APP_Data.ProductSubCategory SubCategory2 = new APP_Data.ProductSubCategory();
            //SubCategory2.Id = 1;
            //SubCategory2.Name = "None";
            pSubCatList.Add(SubCategoryObj1);
            //   pSubCatList.Add(SubCategory2);
            //pSubCatList.AddRange((from c in entity.ProductSubCategories where c.ProductCategoryId == Convert.ToInt32(cboMainCategory.SelectedValue) select c).ToList());
            cboSubCategory.DataSource = pSubCatList;
            cboSubCategory.DisplayMember = "Name";
            cboSubCategory.ValueMember = "Id";
            cboSubCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboSubCategory.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboSubCategory.SelectedIndex = 0;
            cboMainCategory.SelectedIndex = 0;
            cboCounter.SelectedIndex = 0;

            LoadData();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            dsReportTemp dsReport = new dsReportTemp();
            dsReportTemp.TransactionDetailByItemDataTable dtTransactionReport = (dsReportTemp.TransactionDetailByItemDataTable)dsReport.Tables["TransactionDetailByItem"];
            foreach (TransactionDetailByItemHolder r in DetailLists)
            {
                dsReportTemp.TransactionDetailByItemRow newRow = dtTransactionReport.NewTransactionDetailByItemRow();
                newRow.ItemNo = r.ItemNo.ToString();
                //newRow.Name = r.Name;
                newRow.TransactionId = r.TransactionId.ToString();
                newRow.TransactionType = r.TransactionType;
                newRow.Qty = Convert.ToInt32(r.Qty.ToString());
                newRow.TotalAmount = Convert.ToInt32(r.TotalAmount);
                //newRow.DateTime = Convert.ToDateTime(r.TransactionDate);
                newRow.CounterName = r.Counter_Name;
                dtTransactionReport.AddTransactionDetailByItemRow(newRow);
            }






            ReportViewer rv = new ReportViewer();
            ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["TransactionDetailByItem"]);    //20190108
                                                                                                                    //ReportDataSource rds = new ReportDataSource();
                                                                                                                    //rds.Name = "DataSet1";
                                                                                                                    //rds.Value = resultList;

            string reportPath = Application.StartupPath + "\\Reports\\TransactionDetailByItem.rdlc";
            rv.Reset();
            rv.LocalReport.ReportPath = reportPath;
            rv.LocalReport.DataSources.Clear();
            rv.LocalReport.DataSources.Add(rds);

            ReportParameter TitlePara = new ReportParameter("TitlePara", gbList.Text + " for " + SettingController.ShopName);
            rv.LocalReport.SetParameters(TitlePara);

            ReportParameter Date = new ReportParameter("Date", " from " + dtFrom.Value.Date.ToString("dd/MM/yyyy") + " To " + dtTo.Value.Date.ToString("dd/MM/yyyy"));
            rv.LocalReport.SetParameters(Date);
            //var ps = rv.LocalReport.GetParameters();
            PrintDoc.PrintReport(rv);
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cboBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cboSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void rdbRefund_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void rdbSale_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        #endregion

        #region Function

        private void LoadData()
        {
            if (isstart == true)
            {
                int shopid = Convert.ToInt32(cboshoplist.SelectedValue);
                string currentshortcode = "";
                string currentshopname = "";

                if (shopid != 0)
                {
                    currentshortcode = (from p in entity.Shops where p.Id == shopid select p.ShortCode).FirstOrDefault();
                    currentshopname = (from p in entity.Shops where p.Id == shopid select p.ShopName).FirstOrDefault(); ;
                }
                else
                {
                    currentshopname = "ALL";
                    currentshortcode = "0";
                }
                //Boolean hasError = false;
                int MainCategoryId = 0, SubCategoryId = 0, BrandId = 0, counterId = 0;
                DateTime fromDate = dtFrom.Value.Date;
                DateTime toDate = dtTo.Value.Date;
                bool IsSale = rdbSale.Checked;
                bool IsFOC = rdoFOC.Checked;
                tp.RemoveAll();
                tp.IsBalloon = true;
                tp.ToolTipIcon = ToolTipIcon.Error;
                tp.ToolTipTitle = "Error";

                DetailLists.Clear();

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

                if (cboCounter.SelectedIndex > 0)
                {
                    counterId = Convert.ToInt32(cboCounter.SelectedValue);
                }

                resultList = entity.TransactionDetailReport(fromDate, toDate, IsSale, BrandId, MainCategoryId, SubCategoryId, counterId, IsFOC, currentshortcode);


                if (IsSale)
                {
                    gbList.Text = "Sale Transaction Detail Report";
                }
                else if (IsFOC)
                {
                    gbList.Text = "FOC Transaction Detail Report";
                }
                else
                {
                    gbList.Text = "Refund Transaction Detail Report";
                }
                ShowReportViewer(currentshopname);

                // }
            }
        }
        private void ShowReportViewer(string shopname)
        {

            ////dsReportTemp dsReport = new dsReportTemp();
            ////dsReportTemp.TransactionDetailByItemDataTable dtTransactionReport = (dsReportTemp.TransactionDetailByItemDataTable)dsReport.Tables["TransactionDetailByItem"];

            ////foreach (TransactionDetailByItemHolder r in DetailLists)
            ////{
            ////    dsReportTemp.TransactionDetailByItemRow newRow = dtTransactionReport.NewTransactionDetailByItemRow();
            ////    //newRow.ItemId = r.ItemId.ToString();
            ////    newRow.ItemNo = r.ItemNo.ToString();
            ////    newRow.Name = r.Name;
            ////    newRow.TransactionId = r.TransactionId.ToString();
            ////    newRow.TransactionType = r.TransactionType;
            ////    newRow.Qty = Convert.ToInt32(r.Qty.ToString());
            ////    newRow.TotalAmount =Convert.ToInt32(r.TotalAmount);
            ////    newRow.DateTime =Convert.ToDateTime(r.TransactinDateTime);
            ////    newRow.CounterName = r.CounterName;
            ////    dtTransactionReport.AddTransactionDetailByItemRow(newRow);
            ////}

            //   ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["TransactionDetailByItem"]);
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DataSet1";
            rds.Value = resultList;
            string reportPath = Application.StartupPath + "\\Reports\\TransactionDetailByItem.rdlc";
            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            ReportParameter TitlePara = new ReportParameter("TitlePara", gbList.Text + " for " + shopname);
            reportViewer1.LocalReport.SetParameters(TitlePara);

            ReportParameter Date = new ReportParameter("Date", " from " + dtFrom.Value.Date.ToString("dd-MM-yyyy") + " To " + dtTo.Value.Date.ToString("dd-MM-yyyy"));
            reportViewer1.LocalReport.SetParameters(Date);

            reportViewer1.RefreshReport();
        }
        #endregion               

        private void cboCounter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void rdoFOC_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cboshoplist_selectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }


    }
}
