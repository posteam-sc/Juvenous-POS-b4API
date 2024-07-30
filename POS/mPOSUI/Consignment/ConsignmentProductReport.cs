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
    public partial class ConsignmentProductReport : Form
   {
        public ConsignmentProductReport()
        {
            InitializeComponent();
            CenterToScreen();
        }

        #region variable

        POSEntities entity = new POSEntities();
        List<ConsignmentController> conlist = new List<ConsignmentController>();
        string counterName;
        bool start = false;
        List<object> _Data = new List<object>();
        #endregion    

        #region Function

        private void ShowReportViwer(string currentshopname)
        {
            if (cboCounter.SelectedIndex > 0)
            {
                counterName = cboCounter.Text.ToString();
            }
            else
            {
                counterName = "All Counter";
            }

           #region New
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DataSet1";
            rds.Value = _Data;
           #endregion

            string reportPath = Application.StartupPath + "\\Reports\\ConsignmentProduct.rdlc";

            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            ReportParameter Header = new ReportParameter("Header", currentshopname  + "  Consignment report for " + counterName + " from " + dtpFrom.Value.Date.ToString("dd-MM-yyyy") + " To " + dtpTo.Value.Date.ToString("dd-MM-yyyy"));
            reportViewer1.LocalReport.SetParameters(Header);
            reportViewer1.RefreshReport();
        }

        private void loadData()
        {
       
            if (start)
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
                    currentshopname = "ALL";
                    currentshortcode = "0";
                }
              
                if (cboCounter.SelectedIndex > -1)
                {
                    entity = new POSEntities();
                    int conId = 0;
                    conId = Convert.ToInt32(cboCounter.SelectedValue);

                    DateTime fromdate = dtpFrom.Value.Date;
                    DateTime todate = dtpTo.Value.Date;

                    List<string> type = new List<string> { "Refund", "CreditRefund" };

                    //select consingment all records 
                    var ConsignList = (from td in entity.TransactionDetails
                                       join t in entity.Transactions on td.TransactionId equals t.Id
                                       join p in entity.Products on td.ProductId equals p.Id
                                       join c in entity.ConsignmentCounters on p.ConsignmentCounterId equals c.Id
                                       where p.IsConsignment == true && ((conId == 0 && 1 == 1) || (conId != 0 && p.ConsignmentCounterId == conId))
                                        && (EntityFunctions.TruncateTime(t.DateTime.Value) >= fromdate)
                                               && (EntityFunctions.TruncateTime(t.DateTime.Value) <= todate)
                                                && (td.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1))

                                       group td by new { td.UnitPrice, td.ProductId, p.Name, td.ConsignmentPrice, t.Type, td.DiscountRate,  Consignor= c.Name  } into totalConsignQty
                                       select new
                                       {
                                           ProductId = totalConsignQty.Key.ProductId,
                                           Name = totalConsignQty.Key.Name,
                                           Counter=totalConsignQty.Key.Consignor,
                                           SellingPrice = (totalConsignQty.Key.UnitPrice) - (totalConsignQty.Key.UnitPrice * totalConsignQty.Key.DiscountRate / 100),
                                           ConsignmentPrice = totalConsignQty.Key.ConsignmentPrice,
                                           RefundQty = type.Contains(totalConsignQty.Key.Type) ? totalConsignQty.Sum(o => o.Qty) : 0,
                                           ConsginQty = !type.Contains(totalConsignQty.Key.Type) ? totalConsignQty.Sum(o => o.Qty) : 0,
                                           Type = totalConsignQty.Key.Type
                                       }).Distinct();

                    var CosignQtyList = ConsignList.Where(x => x.ConsginQty > 0).Select(x => x.ConsginQty).Sum();

                    if (CosignQtyList > 0)
                    {
                        //filter consignment list already minus refund
                        IQueryable<object> q = (from g in ConsignList
                                                group g by new { g.ProductId, g.Name, g.SellingPrice, g.ConsignmentPrice, g.Counter } into _gridData
                                                select new
                                                {
                                                    ProductId = _gridData.Key.ProductId,
                                                    Name = _gridData.Key.Name,
                                                    Counter = _gridData.Key.Counter,
                                                    Price = _gridData.Key.SellingPrice,
                                                    ConsignmentPrice = _gridData.Key.ConsignmentPrice,
                                                    
                                                    Qty =   _gridData.Sum(s => s.ConsginQty) - _gridData.Sum(s => s.RefundQty),
                                                    ProfitPrice = (_gridData.Key.SellingPrice - _gridData.Key.ConsignmentPrice),
                                                    TotalConsignmentPrice = (_gridData.Sum(s => s.ConsginQty) - _gridData.Sum(s => s.RefundQty)) * _gridData.Key.ConsignmentPrice,
                                                    TotalProfitPrice = (_gridData.Key.SellingPrice - _gridData.Key.ConsignmentPrice) * _gridData.Sum(s => s.ConsginQty) - _gridData.Sum(s => s.RefundQty),
                                                });

                        _Data = new List<object>(q);

                    
                    }
                    else
                    {
                        _Data = new List<object>();
                    }
                    ShowReportViwer(currentshopname);
                }
            }
        }
            
            
        
        #endregion

        #region Event

        private void ConsignmentProductReport_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            Utility.BindConsignor(cboCounter);
            Utility.BindShop(cboshoplist,true);
            cboshoplist.Text = SettingController.DefaultShop.ShopName;
            Utility.ShopComBo_EnableOrNot(cboshoplist);
            cboshoplist.SelectedIndex = 0;
            start = true;
            loadData();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void cboCounter_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void rdoUnitPrice_CheckedChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void cboshoplist_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadData();
        }
        #endregion

   
    }
}

 