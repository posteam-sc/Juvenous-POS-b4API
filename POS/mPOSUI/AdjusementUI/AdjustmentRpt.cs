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
    public partial class AdjustmentRpt : Form
    {
        
        
        #region variable
        POSEntities entity = new POSEntities();

      // List<object> _adjustmentList = new List<object>();
        List<object> _adjustmentList = new List<object>();
        bool IsStart = false;
        #endregion

        #region Function
      private void Bind_Brand()
      {
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
      }

      private Char Radio_Status()
      {
          Char _St = '\0';
          if (rdoAll.Checked)
          {
              _St = 'A';
          }
          else if (rdConsignment.Checked)
          {
              _St = 'C';
          }
          else
          {
              _St = 'N';
          }
          return _St;
      }

        public void loadData()
        {
            if (IsStart)
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
               
            int _braindId = 0;
            if (cboBrand.SelectedIndex > 0)
            {
                _braindId = Convert.ToInt32(cboBrand.SelectedValue);
            }

            Char _status = Radio_Status();

            DateTime fromDate = dtFrom.Value.Date;
            DateTime toDate = dtTo.Value.Date;
            int typeId = Convert.ToInt32(cboAdjType.SelectedValue);
            entity = new POSEntities();

            IQueryable<object> q = from d in entity.Adjustments
                                   join p in entity.Products on d.ProductId equals p.Id
                                    join adj in entity.AdjustmentTypes on d.AdjustmentTypeId equals adj.Id
                                   where d.IsDeleted == false
                                   && (EntityFunctions.TruncateTime((DateTime)d.AdjustmentDateTime) >= fromDate
                                   && EntityFunctions.TruncateTime((DateTime)d.AdjustmentDateTime) <= toDate)  && ((shopid != 0 && d.AdjustmentNo.Substring(3, 2) == currentshortcode) || (shopid == 0 && 1 == 1))
                                   && ((_status == 'A' && 1 == 1) || (_status == 'C' && p.IsConsignment == true) || (_status == 'N' && p.IsConsignment == false))
                                   && ((_braindId == 0 && 1 == 1) || (_braindId != 0 && p.BrandId == _braindId)) && ((typeId == 0 && 1 == 1) || (typeId != 0 && d.AdjustmentTypeId == typeId))
                                                  select new 
                                   {
                                       //Id = d.Id,
                                       ProductCode = p.ProductCode,
                                       ProductName = p.Name,
                                       Price = p.Price,
                                       StockIn = d.AdjustmentQty > 0 ? d.AdjustmentQty : 0,
                                       StockOut = d.AdjustmentQty < 0 ? d.AdjustmentQty * -1 : 0,
                                       TotalCost = d.AdjustmentQty < 0 ? (d.AdjustmentQty * p.Price * -1) : (d.AdjustmentQty * p.Price),
                                       AdjustmentDateTime = d.AdjustmentDateTime,
                                       ResponsibleName = d.ResponsibleName,
                                       Type=adj.Name,
                                       Reason = d.Reason
                                   };
            _adjustmentList = new List<object>(q);
            
            ShowReportViewer(currentshopname);
            }
        }

        private void ShowReportViewer(string currentshopname)
        {
            string dmgtype = string.Empty;
            if (rdoAll.Checked)
            {
                dmgtype = rdoAll.Text;
            }
            else if (rdNonConsignment.Checked)
            {
                dmgtype = rdNonConsignment.Text;
            }
            else
            {
                dmgtype = rdConsignment.Text;
            }
        
            #region New
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DataSet1";
            rds.Value = _adjustmentList;
            #endregion

            string reportPath = Application.StartupPath + "\\Reports\\Adjustment.rdlc";

            reportViewerAdjustemntReport.LocalReport.ReportPath = reportPath;
            reportViewerAdjustemntReport.LocalReport.DataSources.Clear();
            reportViewerAdjustemntReport.LocalReport.DataSources.Add(rds);

            ReportParameter Header = new ReportParameter("Header", "Adjustment report for " + dmgtype  + " from " + dtFrom.Value.Date.ToString("dd-MM-yyyy") + " To " + dtTo.Value.Date.ToString("dd-MM-yyyy") +"  For "+currentshopname);
            reportViewerAdjustemntReport.LocalReport.SetParameters(Header);
            reportViewerAdjustemntReport.RefreshReport();
        }

        #endregion

        #region method
        public AdjustmentRpt()
        {
            InitializeComponent();
        }
        private void cboBrand_SelectedValueChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void rdConsignment_CheckedChanged(object sender, EventArgs e)
        {
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

        private void cboAdjType_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void DamageRpt_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            Bind_Brand();
            Utility.Bind_AdjustmentType(cboAdjType);
            Utility.BindShop(cboshoplist,true);
            cboshoplist.Text = SettingController.DefaultShop.ShopName;
            cboshoplist.SelectedIndex = 0;
            Utility.ShopComBo_EnableOrNot(cboshoplist);
            IsStart = true;
            loadData();
            this.reportViewerAdjustemntReport.RefreshReport();
        }

        private void cboshoplist_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadData();
        }
        #endregion

     

    }

}
