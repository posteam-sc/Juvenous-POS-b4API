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
    public partial class SaleBreakDown : Form
    {
     
        #region Variables
        POSEntities entity = new POSEntities();
        List<SaleBreakDownController> ResultList = new List<SaleBreakDownController>();
        List<SaleBreakDownController> FinalResultList = new List<SaleBreakDownController>();
        List<SaleBreakDownController> spresultList = new List<SaleBreakDownController>();
        string name = string.Empty;
        Boolean isstart = false;
        #endregion

        #region event
        public SaleBreakDown()
        {
            InitializeComponent();
        }
        private void SaleBreakDown_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            rdbRange.Checked = true;
            rdbSaleTrueValue.Checked = true;
            Utility.BindShop(cboshoplist,true);
            cboshoplist.Text = SettingController.DefaultShop.ShopName;
            Utility.ShopComBo_EnableOrNot(cboshoplist);
            cboshoplist.SelectedIndex = 0;
            isstart = true;
            LoadData();
        }



        private void rdbUnitPrice_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void gbPeriod_Enter(object sender, EventArgs e)
        {
            LoadData();
        }

        private void rdbSegment_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void rdbSaleTrueValue_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void rdbRange_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void cboshoplist_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        #endregion

        #region method

        private void LoadData()
        {
            if (isstart == true)
            {
                DateTime fromDate = dtFrom.Value.Date;
                DateTime toDate = dtTo.Value.Date;
                bool IsRange = rdbRange.Checked;
                bool IsSaleTrue = rdbSaleTrueValue.Checked;

                decimal total = 0;
                decimal spTotal = 0;
                ResultList.Clear();
                spresultList.Clear();

                int shopid = Convert.ToInt32(cboshoplist.SelectedValue);
                string currentshortcode = "";
                if (shopid!=0)
                {
                    currentshortcode = (from p in entity.Shops where p.Id == shopid select p.ShortCode).FirstOrDefault();
                }
                else
                {
                    currentshortcode = "0";
                }
                
                #region Brand

                if (IsRange)
                {
                    name = "Brand";
                    if (IsSaleTrue)
                    {
                        System.Data.Objects.ObjectResult<SaleBreakDownByRangeWithSaleTrueValue_Result> rlist = entity.SaleBreakDownByRangeWithSaleTrueValue(fromDate, toDate, false, currentshortcode);
                        List<SaleBreakDownController> ResultList2 = new List<SaleBreakDownController>();
                        foreach (SaleBreakDownByRangeWithSaleTrueValue_Result r in rlist)
                        {
                            SaleBreakDownController saleObj = new SaleBreakDownController();
                            saleObj.bId = Convert.ToInt32(r.Id);
                            saleObj.Name = r.Name.ToString();
                            saleObj.Sales = (r.TotalSale == null) ? 0 : Convert.ToDecimal(r.TotalSale);
                            saleObj.saleQty = (r.SaleQty == null) ? 0 : Convert.ToInt32(r.SaleQty);
                            saleObj.Refund = (r.TotalRefund == null) ? 0 : Convert.ToDecimal(r.TotalRefund);
                            saleObj.refundQty = (r.RefundQty == null) ? 0 : Convert.ToInt32(r.RefundQty);
                            total += Convert.ToInt32(r.TotalSale);
                            ResultList.Add(saleObj);
                            ResultList2.Add(saleObj);
                        }

                        System.Data.Objects.ObjectResult<SaleBreakDownByRangeWithSaleTrueValue_Result> specialPromotionList = entity.SaleBreakDownByRangeWithSaleTrueValue(fromDate, toDate, true, currentshortcode);

                        foreach (SaleBreakDownByRangeWithSaleTrueValue_Result sp in specialPromotionList)
                        {
                            SaleBreakDownController saleObj = new SaleBreakDownController();
                            saleObj.bId = Convert.ToInt32(sp.Id);
                            saleObj.Name = sp.Name;
                            saleObj.Sales = (sp.TotalSale == null) ? 0 : Convert.ToDecimal(sp.TotalSale);
                            saleObj.saleQty = (sp.SaleQty == null) ? 0 : Convert.ToInt32(sp.SaleQty);
                            saleObj.Refund = (sp.TotalRefund == null) ? 0 : Convert.ToDecimal(sp.TotalRefund);
                            saleObj.refundQty = (sp.RefundQty == null) ? 0 : Convert.ToInt32(sp.RefundQty);
                            spTotal += Convert.ToInt32(sp.TotalSale);
                            spresultList.Add(saleObj);
                        }
                        int i = 0;
                        foreach (SaleBreakDownController sb in ResultList2)
                        {

                            System.Data.Objects.ObjectResult<GetSaleSpecialPromotionByCustomerId_Result> splist = entity.GetSaleSpecialPromotionByCustomerId(fromDate, toDate, sb.bId, true, currentshortcode);
                            List<SpecialPromotionController> SPCList = new List<SpecialPromotionController>();

                            foreach (GetSaleSpecialPromotionByCustomerId_Result sp in splist)
                            {
                                SpecialPromotionController sObj = new SpecialPromotionController();
                                sObj.Name = sp.Name;
                                sObj.saleQty = Convert.ToInt32(sp.SaleQty);
                                sObj.Sales = Convert.ToInt32(sp.TotalSale);
                                sObj.refundQty = Convert.ToInt32(sp.RefundQty);
                                sObj.Refund = Convert.ToInt32(sp.TotalRefund);
                                SPCList.Add(sObj);
                            }
                            if (SPCList.Count > 0)
                            {
                                ResultList[i].saleQty += SPCList[0].saleQty;
                                ResultList[i].Sales += Convert.ToDecimal(SPCList[0].Sales);
                                ResultList[i].refundQty += SPCList[0].refundQty;
                                ResultList[i].Refund += Convert.ToDecimal(SPCList[0].Refund);
                                total += Convert.ToInt32(SPCList[0].Sales);
                            }

                            i++;
                        }
                    }
                    else
                    {
                        System.Data.Objects.ObjectResult<SaleBreakDownByRangeWithUnitValue_Result> rList = entity.SaleBreakDownByRangeWithUnitValue(fromDate, toDate, false, currentshortcode);
                        List<SaleBreakDownController> ResultList2 = new List<SaleBreakDownController>();
                        foreach (SaleBreakDownByRangeWithUnitValue_Result r in rList)
                        {
                            SaleBreakDownController saleObj = new SaleBreakDownController();
                            saleObj.bId = Convert.ToInt32(r.Id);
                            saleObj.Name = r.Name;
                            saleObj.Sales = (r.TotalSale == null) ? 0 : Convert.ToDecimal(r.TotalSale);
                            saleObj.saleQty = (r.SaleQty == null) ? 0 : Convert.ToInt32(r.SaleQty);
                            saleObj.Refund = (r.TotalRefund == null) ? 0 : Convert.ToDecimal(r.TotalRefund);
                            saleObj.refundQty = (r.RefundQty == null) ? 0 : Convert.ToInt32(r.RefundQty);
                            total += Convert.ToInt32(r.TotalSale);
                            ResultList.Add(saleObj);
                            ResultList2.Add(saleObj);
                        }

                        System.Data.Objects.ObjectResult<SaleBreakDownByRangeWithUnitValue_Result> specialPromotionList = entity.SaleBreakDownByRangeWithUnitValue(fromDate, toDate, true, currentshortcode);

                        foreach (SaleBreakDownByRangeWithUnitValue_Result sp in specialPromotionList)
                        {
                            SaleBreakDownController saleObj = new SaleBreakDownController();
                            saleObj.bId = Convert.ToInt32(sp.Id);
                            saleObj.Name = sp.Name;
                            saleObj.Sales = (sp.TotalSale == null) ? 0 : Convert.ToDecimal(sp.TotalSale);
                            saleObj.saleQty = (sp.SaleQty == null) ? 0 : Convert.ToInt32(sp.SaleQty);
                            saleObj.Refund = (sp.TotalRefund == null) ? 0 : Convert.ToDecimal(sp.TotalRefund);
                            saleObj.refundQty = (sp.RefundQty == null) ? 0 : Convert.ToInt32(sp.RefundQty);
                            spTotal += Convert.ToInt32(sp.TotalSale);
                            spresultList.Add(saleObj);

                        }
                        int i = 0;
                        foreach (SaleBreakDownController sb in ResultList2)
                        {

                            System.Data.Objects.ObjectResult<GetSaleSpecialPromotionByCustomerId_Result> splist = entity.GetSaleSpecialPromotionByCustomerId(fromDate, toDate, sb.bId, false, currentshortcode);
                            List<SpecialPromotionController> SPCList = new List<SpecialPromotionController>();

                            foreach (GetSaleSpecialPromotionByCustomerId_Result sp in splist)
                            {
                                SpecialPromotionController sObj = new SpecialPromotionController();
                                sObj.Name = sp.Name;
                                sObj.saleQty = Convert.ToInt32(sp.SaleQty);
                                sObj.Sales = Convert.ToInt32(sp.TotalSale);
                                sObj.refundQty = Convert.ToInt32(sp.RefundQty);
                                sObj.Refund = Convert.ToInt32(sp.TotalRefund);
                                SPCList.Add(sObj);
                            }
                            if (SPCList.Count > 0)
                            {
                                ResultList[i].saleQty += SPCList[0].saleQty;
                                ResultList[i].Sales += Convert.ToDecimal(SPCList[0].Sales);
                                ResultList[i].refundQty += SPCList[0].refundQty;
                                ResultList[i].Refund += Convert.ToDecimal(SPCList[0].Refund);
                                total += Convert.ToInt32(SPCList[0].Sales);
                            }

                            i++;

                        }
                    }
                }
                #endregion


                #region Segment
                else
                {
                    name = "Category";
                    if (IsSaleTrue)
                    {
                        System.Data.Objects.ObjectResult<SaleBreakDownBySegmentWithSaleTrueValue_Result> rList = entity.SaleBreakDownBySegmentWithSaleTrueValue(fromDate, toDate, false, currentshortcode);
                        List<SaleBreakDownController> ResultList2 = new List<SaleBreakDownController>();
                        foreach (SaleBreakDownBySegmentWithSaleTrueValue_Result r in rList)
                        {
                            SaleBreakDownController saleObj = new SaleBreakDownController();
                            saleObj.bId = Convert.ToInt32(r.Id);
                            saleObj.Name = r.Name;
                            saleObj.Sales = (r.TotalSale == null) ? 0 : Convert.ToDecimal(r.TotalSale);
                            saleObj.saleQty = (r.SaleQty == null) ? 0 : Convert.ToInt32(r.SaleQty);
                            saleObj.Refund = (r.TotalRefund == null) ? 0 : Convert.ToDecimal(r.TotalRefund);
                            saleObj.refundQty = (r.RefundQty == null) ? 0 : Convert.ToInt32(r.RefundQty);
                            total += Convert.ToInt32(r.TotalSale);
                            ResultList.Add(saleObj);
                            ResultList2.Add(saleObj);
                        }

                        System.Data.Objects.ObjectResult<SaleBreakDownBySegmentWithSaleTrueValue_Result> specialPromotionList = entity.SaleBreakDownBySegmentWithSaleTrueValue(fromDate, toDate, true, currentshortcode);

                        foreach (SaleBreakDownBySegmentWithSaleTrueValue_Result sp in specialPromotionList)
                        {
                            SaleBreakDownController saleObj = new SaleBreakDownController();
                            saleObj.bId = Convert.ToInt32(sp.Id);
                            saleObj.Name = sp.Name;
                            saleObj.Sales = (sp.TotalSale == null) ? 0 : Convert.ToDecimal(sp.TotalSale);
                            saleObj.saleQty = (sp.SaleQty == null) ? 0 : Convert.ToInt32(sp.SaleQty);
                            saleObj.Refund = (sp.TotalRefund == null) ? 0 : Convert.ToDecimal(sp.TotalRefund);
                            saleObj.refundQty = (sp.RefundQty == null) ? 0 : Convert.ToInt32(sp.RefundQty);
                            spTotal += Convert.ToInt32(sp.TotalSale);
                            spresultList.Add(saleObj);

                        }

                        int i = 0;
                        foreach (SaleBreakDownController sb in ResultList2)
                        {

                            System.Data.Objects.ObjectResult<GetSaleSpecialPromotionSegmentByCustomerId_Result> splist = entity.GetSaleSpecialPromotionSegmentByCustomerId(fromDate, toDate, sb.bId, true, currentshortcode);
                            List<SpecialPromotionController> SPCList = new List<SpecialPromotionController>();

                            foreach (GetSaleSpecialPromotionSegmentByCustomerId_Result sp in splist)
                            {
                                SpecialPromotionController sObj = new SpecialPromotionController();

                                sObj.Name = sp.Name;
                                sObj.saleQty = Convert.ToInt32(sp.SaleQty);
                                sObj.Sales = Convert.ToInt32(sp.TotalSale);
                                sObj.refundQty = Convert.ToInt32(sp.RefundQty);
                                sObj.Refund = Convert.ToInt32(sp.TotalRefund);
                                SPCList.Add(sObj);

                            }
                            if (SPCList.Count > 0)
                            {
                                ResultList[i].saleQty += SPCList[0].saleQty;
                                ResultList[i].Sales += Convert.ToDecimal(SPCList[0].Sales);
                                ResultList[i].refundQty += SPCList[0].refundQty;
                                ResultList[i].Refund += Convert.ToDecimal(SPCList[0].Refund);
                                total += Convert.ToInt32(SPCList[0].Sales);
                            }

                            i++;

                        }

                    }
                    else
                    {
                        System.Data.Objects.ObjectResult<SaleBreakDownBySegmentWithUnitValue_Result> rList = entity.SaleBreakDownBySegmentWithUnitValue(fromDate, toDate, false, currentshortcode);
                        List<SaleBreakDownController> ResultList2 = new List<SaleBreakDownController>();
                        foreach (SaleBreakDownBySegmentWithUnitValue_Result r in rList)
                        {
                            SaleBreakDownController saleObj = new SaleBreakDownController();
                            saleObj.bId = Convert.ToInt32(r.Id);
                            saleObj.Name = r.Name;
                            saleObj.Sales = (r.TotalSale == null) ? 0 : Convert.ToDecimal(r.TotalSale);
                            saleObj.saleQty = (r.SaleQty == null) ? 0 : Convert.ToInt32(r.SaleQty);
                            saleObj.Refund = (r.TotalRefund == null) ? 0 : Convert.ToDecimal(r.TotalRefund);
                            saleObj.refundQty = (r.RefundQty == null) ? 0 : Convert.ToInt32(r.RefundQty);
                            total += Convert.ToInt32(r.TotalSale);
                            ResultList.Add(saleObj);
                            ResultList2.Add(saleObj);
                        }

                        System.Data.Objects.ObjectResult<SaleBreakDownBySegmentWithUnitValue_Result> specialPromotionList = entity.SaleBreakDownBySegmentWithUnitValue(fromDate, toDate, true, currentshortcode);

                        foreach (SaleBreakDownBySegmentWithUnitValue_Result sp in specialPromotionList)
                        {
                            SaleBreakDownController saleObj = new SaleBreakDownController();
                            saleObj.bId = Convert.ToInt32(sp.Id);
                            saleObj.Name = sp.Name;
                            saleObj.Sales = (sp.TotalSale == null) ? 0 : Convert.ToDecimal(sp.TotalSale);
                            saleObj.saleQty = (sp.SaleQty == null) ? 0 : Convert.ToInt32(sp.SaleQty);
                            saleObj.Refund = (sp.TotalRefund == null) ? 0 : Convert.ToDecimal(sp.TotalRefund);
                            saleObj.refundQty = (sp.RefundQty == null) ? 0 : Convert.ToInt32(sp.RefundQty);
                            spTotal += Convert.ToInt32(sp.TotalSale);
                            spresultList.Add(saleObj);

                        }

                        int i = 0;
                        foreach (SaleBreakDownController sb in ResultList2)
                        {

                            System.Data.Objects.ObjectResult<GetSaleSpecialPromotionSegmentByCustomerId_Result> splist = entity.GetSaleSpecialPromotionSegmentByCustomerId(fromDate, toDate, sb.bId, false, currentshortcode);
                            List<SpecialPromotionController> SPCList = new List<SpecialPromotionController>();

                            foreach (GetSaleSpecialPromotionSegmentByCustomerId_Result sp in splist)
                            {
                                SpecialPromotionController sObj = new SpecialPromotionController();

                                sObj.Name = sp.Name;
                                sObj.saleQty = Convert.ToInt32(sp.SaleQty);
                                sObj.Sales = Convert.ToInt32(sp.TotalSale);
                                sObj.refundQty = Convert.ToInt32(sp.RefundQty);
                                sObj.Refund = Convert.ToInt32(sp.TotalRefund);
                                SPCList.Add(sObj);

                            }
                            if (SPCList.Count > 0)
                            {
                                ResultList[i].saleQty += SPCList[0].saleQty;
                                ResultList[i].Sales += Convert.ToDecimal(SPCList[0].Sales);
                                ResultList[i].refundQty += SPCList[0].refundQty;
                                ResultList[i].Refund += Convert.ToDecimal(SPCList[0].Refund);
                                total += Convert.ToInt32(SPCList[0].Sales);
                            }

                            i++;

                        }
                    }
                }
                #endregion


                FinalResultList.Clear();
                foreach (SaleBreakDownController s in ResultList)
                {
                    SaleBreakDownController sObj = new SaleBreakDownController();
                    s.BreakDown = (s.Sales == 0) ? 0 : Math.Round((s.Sales / total) * 100, 2);
                    FinalResultList.Add(s);
                }
                ShowReportViewer();

            }
        }

        private void ShowReportViewer()
        {
            dsReportTemp dsReport = new dsReportTemp();
            dsReportTemp.SaleBreakDownDataTable dtSaleBreakReport = (dsReportTemp.SaleBreakDownDataTable)dsReport.Tables["SaleBreakDown"];
            foreach (SaleBreakDownController r in FinalResultList)
            {
                dsReportTemp.SaleBreakDownRow newRow = dtSaleBreakReport.NewSaleBreakDownRow();
                newRow.Name = r.Name;
                newRow.ToalSales = r.Sales;
                newRow.SaleQty = r.saleQty;
                newRow.BreakDown = r.BreakDown;
                newRow.TotalRefund = r.Refund;
                newRow.RefundQty = r.refundQty; dtSaleBreakReport.AddSaleBreakDownRow(newRow);
            }
            ReportDataSource rds1 = new ReportDataSource("DataSet1", dsReport.Tables["SaleBreakDown"]);
            dsReportTemp SPdsReport = new dsReportTemp();
            dsReportTemp.SaleBreakDownDataTable dtSPSaleBreakReport = (dsReportTemp.SaleBreakDownDataTable)SPdsReport.Tables["SaleBreakDown"];
            foreach (SaleBreakDownController r in spresultList)
            {
                dsReportTemp.SaleBreakDownRow newRow = dtSPSaleBreakReport.NewSaleBreakDownRow();
                newRow.Name = r.Name;
                newRow.ToalSales = r.Sales;
                newRow.SaleQty = r.saleQty;
                newRow.BreakDown = 100;
                newRow.TotalRefund = r.Refund;
                newRow.RefundQty = r.refundQty; dtSPSaleBreakReport.AddSaleBreakDownRow(newRow);
            }
            ReportDataSource rds2 = new ReportDataSource("DataSet2", SPdsReport.Tables["SaleBreakDown"]);


            string reportPath = Application.StartupPath + "\\Reports\\SaleBreakDown.rdlc";
            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds1);
            reportViewer1.LocalReport.DataSources.Add(rds2);

            string stDate = dtFrom.Value.Date.ToString("dd-MM-yyyy");
            string endDate = dtTo.Value.Date.ToString("dd-MM-yyyy");

            ReportParameter Header = new ReportParameter("Header", stDate + " To " + endDate + " Sale Break Down");
            reportViewer1.LocalReport.SetParameters(Header);

            ReportParameter Title = new ReportParameter("Title", name);
            reportViewer1.LocalReport.SetParameters(Title);

            reportViewer1.RefreshReport();

        }
        #endregion

       
    }
}
