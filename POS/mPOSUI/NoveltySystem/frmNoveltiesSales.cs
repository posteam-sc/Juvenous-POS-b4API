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
    public partial class NoveltiesSaleReport : Form
    {
        public NoveltiesSaleReport()
        {
            InitializeComponent();
        }

        public decimal totalQty;
        public decimal totalAmt;
        POSEntities entity = new POSEntities();
       Nullable<int> brandId  = 0;
       Nullable<System.DateTime> validFrom;
        Nullable<System.DateTime> validTo;
        List<NoveltySaleController> nvControllerlist = new List<NoveltySaleController>();
        List<String> dateCollection=new List<String>() ;

        DateTime fdate = new DateTime();
        DateTime tdate = new DateTime();

        bool isAll = false;
        bool isVIP = false;
        bool isNonVIP = false;
        bool isNoveltyList = false;
        bool isDate = false;


        private void frmNoveltiesSales_Load(object sender, EventArgs e)
        {
           

            List<APP_Data.Brand> nvlist = new List<APP_Data.Brand>();
            APP_Data.Brand b = new APP_Data.Brand();
            b.Id = 0;
            b.Name = "Select";
            nvlist.Add(b);


            System.Data.Objects.ObjectResult<GetNoveltiesSale_Result> pin = entity.GetNoveltiesSale();
            foreach (GetNoveltiesSale_Result pr in pin)
            {
                APP_Data.Brand nv = new APP_Data.Brand();
                nv.Id =Convert.ToInt32(pr.BrandId);
                nv.Name    = pr.Name;

                nvlist.Add(nv);
 
            }
            cboNvlist.DataSource = nvlist;
            cboNvlist.DisplayMember = "Name";
            cboNvlist.ValueMember = "Id";
            cboNvlist.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboNvlist.AutoCompleteSource = AutoCompleteSource.ListItems;

            City_Bind();
            Counter_Bind();

            loadData(Convert.ToInt32(cboNvlist.SelectedValue),null,null);           
        }

        private void cboCity_SelectedValueChanged(object sender, EventArgs e)
        {
            loadData(brandId, validFrom, validTo);
        }

        private void cboCounter_SelectedValueChanged(object sender, EventArgs e)
        {
            loadData(brandId, validFrom, validTo);
        }

        private void cboNvlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (cboNvlist.SelectedIndex > 0)
            {
                dateCollection.Clear();
                cboDate.DataSource = null;
                cboDate.Items.Add("Select");
                dateCollection.Add("Select");
                brandId = Convert.ToInt32(cboNvlist.SelectedValue);
                List<string> dlist = new List<string>();
                System.Data.Objects.ObjectResult<GetNoveltySaleDate_Result> pdResult = entity.GetNoveltySaleDate(brandId);
                foreach (GetNoveltySaleDate_Result pr in pdResult)
                {
                    validFrom=pr.ValidFrom;
                    validTo=pr.ValidTo;
                    String s = pr.ValidFrom.ToString() + "-" + pr.ValidTo.ToString();
                    cboDate.Items.Add(s);
                    dateCollection.Add(s);
                }
                isNoveltyList = true;
                loadData(brandId, validFrom,validTo);
            }
           
        }

        private void cboDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDate.SelectedIndex>0)
            {
                brandId = Convert.ToInt32(cboNvlist.SelectedValue);
                string ss=cboDate.SelectedItem.ToString();
                string[] sss = ss.Split('-');
              
                 validFrom= Convert.ToDateTime(sss[0]);
                 validTo = Convert.ToDateTime(sss[1]);

                isDate = true;
                loadData(brandId, validFrom, validTo);
            }
            
        }

        private void City_Bind()
        {
            List<APP_Data.City> cityList = new List<APP_Data.City>();
            APP_Data.City city1 = new APP_Data.City();
            city1.Id = 0;
            city1.CityName = "Select";
            cityList.Add(city1);
            cityList.AddRange(entity.Cities.ToList());
            cboCity.DataSource = cityList;
            cboCity.DisplayMember = "CityName";
            cboCity.ValueMember = "Id";
        }

        private void Counter_Bind()
        {
            List<APP_Data.Counter> counterList = new List<APP_Data.Counter>();
            APP_Data.Counter counterObj = new APP_Data.Counter();
            counterObj.Id = 0;
            counterObj.Name = "Select";
            counterList.Add(counterObj);
            counterList.AddRange((from c in entity.Counters orderby c.Id select c).ToList());
            cboCounter.DataSource = counterList;
            cboCounter.DisplayMember = "Name";
            cboCounter.ValueMember = "Id";
        }

        private void loadData(Nullable<int> brandId, Nullable<System.DateTime> validFrom, Nullable<System.DateTime> validTo)
        {
            int CityId = 0, CountryId = 0;

            if (cboCity.SelectedIndex != 0)
            {
                CityId = Convert.ToInt32(cboCity.SelectedValue);
            }

            if (cboCounter.SelectedIndex != 0)
            {
                CountryId = Convert.ToInt32(cboCounter.SelectedValue);
            }

            if (rdoAll.Checked == true)
            {
                totalQty = 0;
                totalAmt = 0;
                System.Data.Objects.ObjectResult<GetNoveliesSaleByCTypte_Result> r = entity.GetNoveliesSaleByCTypte("ALL",brandId,validFrom,validTo,CityId,CountryId);
                foreach (GetNoveliesSaleByCTypte_Result gr in r)
                {
                    NoveltySaleController nvObj = new NoveltySaleController();
                    nvObj.SKUCode = gr.ProductCode;
                    nvObj.ProductName = gr.Name;
                    nvObj.UnitPrice =Convert.ToInt32( gr.UnitPrice);
                    nvObj.TotalQty =Convert.ToInt32(gr.TotalQty);
                    nvObj.TotalAmount =Convert.ToInt32( gr.TotalAmount);

                    nvControllerlist.Add(nvObj);

                    totalQty += nvObj.TotalQty;
                    totalAmt += nvObj.TotalAmount;
                }


            }
            else if (rdbVIP.Checked == true)
            {
                totalQty = 0;
                totalAmt = 0;
                System.Data.Objects.ObjectResult<GetNoveliesSaleByCTypte_Result> r = entity.GetNoveliesSaleByCTypte("VIP", brandId, validFrom, validTo, CityId, CountryId);
                foreach (GetNoveliesSaleByCTypte_Result gr in r)
                {
                    NoveltySaleController nvObj = new NoveltySaleController();
                    nvObj.SKUCode = gr.ProductCode;
                    nvObj.ProductName = gr.Name;
                    nvObj.UnitPrice = Convert.ToInt32(gr.UnitPrice);
                    nvObj.TotalQty = Convert.ToInt32(gr.TotalQty);
                    nvObj.TotalAmount = Convert.ToInt32(gr.TotalAmount);
                    nvControllerlist.Add(nvObj);


                    totalQty += nvObj.TotalQty;
                    totalAmt += nvObj.TotalAmount;
                }

            }
            else if (rdbNon_VIP.Checked == true)
            {
                totalQty = 0;
                totalAmt = 0;
                System.Data.Objects.ObjectResult<GetNoveliesSaleByCTypte_Result> r = entity.GetNoveliesSaleByCTypte("NonVIP", brandId, validFrom, validTo, CityId, CountryId);
                foreach (GetNoveliesSaleByCTypte_Result gr in r)
                {
                    NoveltySaleController nvObj = new NoveltySaleController();
                    nvObj.SKUCode = gr.ProductCode;
                    nvObj.ProductName = gr.Name;
                    nvObj.UnitPrice = Convert.ToInt32(gr.UnitPrice);
                    nvObj.TotalQty = Convert.ToInt32(gr.TotalQty);
                    nvObj.TotalAmount = Convert.ToInt32(gr.TotalAmount);
                    nvControllerlist.Add(nvObj);

                    totalQty += nvObj.TotalQty;
                    totalAmt += nvObj.TotalAmount;
                }

            }
            else if (isNoveltyList == true)
            {
                totalQty = 0;
                totalAmt = 0;
                System.Data.Objects.ObjectResult<GetNoveltySaleByBrandId_Result> r = entity.GetNoveltySaleByBrandId(brandId, CityId, CountryId);
                foreach (GetNoveltySaleByBrandId_Result gr in r)
                {
                    NoveltySaleController nvObj = new NoveltySaleController();
                    nvObj.SKUCode = gr.ProductCode;
                    nvObj.ProductName = gr.Name;
                    nvObj.UnitPrice = Convert.ToInt32(gr.UnitPrice);
                    nvObj.TotalQty = Convert.ToInt32(gr.TotalQty);
                    nvObj.TotalAmount = Convert.ToInt32(gr.TotalAmount);
                    nvControllerlist.Add(nvObj);

                    totalQty += nvObj.TotalQty;
                    totalAmt += nvObj.TotalAmount;
                }


            }
            else if (isDate == true)
            {
                totalQty = 0;
                totalAmt = 0;
                System.Data.Objects.ObjectResult<GetNoveltySaleByDate_Result> r = entity.GetNoveltySaleByDate(brandId, fdate, tdate, CityId, CountryId);
                foreach (GetNoveltySaleByDate_Result gr in r)
                {
                    NoveltySaleController nvObj = new NoveltySaleController();
                    nvObj.SKUCode = gr.ProductCode;
                    nvObj.ProductName = gr.Name;
                    nvObj.UnitPrice = Convert.ToInt32(gr.UnitPrice);
                    nvObj.TotalQty = Convert.ToInt32(gr.TotalQty);
                    nvObj.TotalAmount = Convert.ToInt32(gr.TotalAmount);
                    nvControllerlist.Add(nvObj);

                    totalQty += nvObj.TotalQty;
                    totalAmt += nvObj.TotalAmount;
                }
            }
            ShowReportViewer();                     
        }

        private void ShowReportViewer()
        {
            dsReportTemp dsReport = new dsReportTemp();
            dsReportTemp.NoveltySaleReportDataTable dtPdReport = (dsReportTemp.NoveltySaleReportDataTable)dsReport.Tables["NoveltySaleReport"];
            foreach (NoveltySaleController pdCon in nvControllerlist)
            {
                dsReportTemp.NoveltySaleReportRow newRow = dtPdReport.NewNoveltySaleReportRow();
                newRow.Name = pdCon.ProductName;
                newRow.ProductCode = pdCon.SKUCode;
                newRow.Unit = pdCon.UnitPrice.ToString();
                newRow.Qty = pdCon.TotalQty.ToString();
                newRow.TotalAmount = pdCon.TotalAmount.ToString();                
                dtPdReport.AddNoveltySaleReportRow(newRow);
            }

        

            ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["NoveltySaleReport"]);
            string reportPath = Application.StartupPath + "\\Reports\\NoveltySaleReport.rdlc";

            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

         

            ReportParameter Header = new ReportParameter("Header", "Novelty Sale Report ");
            reportViewer1.LocalReport.SetParameters(Header);

            ReportParameter TotalQty = new ReportParameter("TotalQty", totalQty.ToString());
            reportViewer1.LocalReport.SetParameters(TotalQty);

            ReportParameter TotalAmt = new ReportParameter("TotalAmt", totalAmt.ToString());
            reportViewer1.LocalReport.SetParameters(TotalAmt);


            reportViewer1.RefreshReport();
            nvControllerlist.Clear();
            //cboDate.DataSource = null;
            cboDate.DataSource = dateCollection;
        }

        private void rdbVIP_CheckedChanged(object sender, EventArgs e)
        {
            loadData(brandId, validFrom, validTo);
        }

        private void rdbNon_VIP_CheckedChanged(object sender, EventArgs e)
        {
            loadData(brandId, validFrom, validTo);
        }

        private void rdoAll_CheckedChanged(object sender, EventArgs e)
        {
            loadData(brandId, validFrom, validTo);
        }

        
    }
}
