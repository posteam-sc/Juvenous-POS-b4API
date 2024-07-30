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
    public partial class ItemSummary : Form
    {
        #region Variable

        POSEntities entity = new POSEntities();
        List<Product> itemList = new List<Product>();
        System.Data.Objects.ObjectResult<SelectItemListByDateForItemSummary_Result> resultList;
        bool IsStart = false;
        #endregion

        #region Event

        public ItemSummary()
        {
            InitializeComponent();
        }

        private void ItemSummary_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            Utility.BindProduct(cboProductName);
            Utility.BindShop(cboshoplist,true);
            cboshoplist.Text = SettingController.DefaultShop.ShopName;
            cboshoplist.SelectedIndex = 0;
            IsStart = true;
            Utility.ShopComBo_EnableOrNot(cboshoplist);
            LoadData();
            this.reportViewer1.RefreshReport();
        }

        private void rdbSale_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        
        private void rdbRefund_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            #region [ Print ]

            dsReportTemp dsReport = new dsReportTemp();
            dsReportTemp.ItemListDataTable dtItemReport = (dsReportTemp.ItemListDataTable)dsReport.Tables["ItemList"];

            foreach (Product p in itemList)
            {
                dsReportTemp.ItemListRow newRow = dtItemReport.NewItemListRow();
                newRow.ItemId = p.Id.ToString();
                newRow.Name = p.Name;
                newRow.Qty = p.Qty.ToString();
                newRow.TotalAmount = Convert.ToInt32(p.Price);
                dtItemReport.AddItemListRow(newRow);
            }
            string reportPath = "";
            ReportViewer rv = new ReportViewer();
            ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["ItemList"]);
            reportPath = Application.StartupPath + "\\Reports\\ItemReport.rdlc";
            rv.Reset();
            rv.LocalReport.ReportPath = reportPath;
            rv.LocalReport.DataSources.Add(rds);

            ReportParameter ItemReportTitle = new ReportParameter("ItemReportTitle", gbList.Text + " for " + SettingController.ShopName);
            rv.LocalReport.SetParameters(ItemReportTitle);

            ReportParameter Date = new ReportParameter("Date", " from " + dtFrom.Value.Date.ToString("dd/MM/yyyy") + " To " + dtTo.Value.Date.ToString("dd/MM/yyyy"));
            reportViewer1.LocalReport.SetParameters(Date);

            PrintDoc.PrintReport(rv);
            #endregion
        }

        #endregion

        #region Function

        private void LoadData()
        {

            if (IsStart)
            {
            
            DateTime fromDate = dtFrom.Value.Date;
            DateTime toDate = dtTo.Value.Date;
            bool IsSale = rdbSale.Checked;
            bool IsFOC= rdoFOC.Checked;
            itemList.Clear();

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
                    currentshopname = "ALL";
                    currentshortcode = "0";
                }
          

            int _proId = 0;
            if (cboProductName.SelectedIndex != 0)
            {
                _proId = Convert.ToInt32(cboProductName.SelectedValue);
            }


            resultList = entity.SelectItemListByDateForItemSummary(fromDate, toDate, IsSale, _proId, IsFOC, currentshortcode);

            //////foreach (SelectItemListByDateForItemSummary_Result r in resultList)
            //////{
            //////    Product p = new Product();
            //////    p.ProductCode = r.ItemId;
            //////    p.Name = r.ItemName;
            //////    p.Qty = r.ItemQty;
            //////   // p.Price = Convert.ToInt32(r.ItemTotalAmount);
            //////    itemList.Add(p);
            //////}
            if (IsSale)
            {
                gbList.Text = "Item Sale Report";

            }
            else if (IsFOC)
            {
                gbList.Text = "Item FOC Report";
            }
            else
            {
                gbList.Text = "Item Refund Report";

            }
            ShowReportViewer(currentshopname);
            }
        }

        private void ShowReportViewer(string currentshopname)
        {

            //dsReportTemp dsReport = new dsReportTemp();
            //dsReportTemp.ItemListDataTable dtItemReport = (dsReportTemp.ItemListDataTable)dsReport.Tables["ItemList"];

            //foreach (Product p in itemList)
            //{
            //    dsReportTemp.ItemListRow newRow = dtItemReport.NewItemListRow();
            //    newRow.ItemId = p.ProductCode;
            //    newRow.Name = p.Name;
            //    newRow.Qty = p.Qty.ToString();
            //    newRow.TotalAmount = Convert.ToInt32(p.Price);
            //    dtItemReport.AddItemListRow(newRow);
            //}


           // ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["ItemList"]);
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DataSet1";
            rds.Value = resultList;
            string reportPath = Application.StartupPath + "\\Reports\\ItemReport.rdlc";
            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            ReportParameter ItemReportTitle = new ReportParameter("ItemReportTitle", gbList.Text + " for " + currentshopname);
            reportViewer1.LocalReport.SetParameters(ItemReportTitle);

            ReportParameter Date = new ReportParameter("Date", " from " + dtFrom.Value.Date.ToString("dd-MM-yyyy") + " To " + dtTo.Value.Date.ToString("dd-MM-yyyy"));
            reportViewer1.LocalReport.SetParameters(Date);

            reportViewer1.RefreshReport();
        }
        private void cboProductName_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void rdoFOC_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cboshoplist_selectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
        #endregion                



       

    }
}
