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
using BarcodeLib.Barcode.RDLCReports;
using BarcodeLib.Barcode;
using System.Drawing.Imaging;
using Microsoft.Reporting.WinForms;

namespace POS
{
    public partial class Barcode : Form
    {
        POSEntities db = new POSEntities();
        public Barcode()
        {
            InitializeComponent();
        }

        private void Barcode_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            List<Product> prolist = new List<Product>();
            var pList = db.Products.ToList();

            Product p = new Product();
            p.Id = 0;
            p.Name = "None";
            prolist.Add(p);
            prolist.AddRange(pList);
            cboproduct.ValueMember = "Id";
            cboproduct.DisplayMember = "Name";
            cboproduct.DataSource = pList;
            cboproduct.Refresh();
            txtrow.Text = "1";
        }

        void LoadData()
        {
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            Product produ = db.Products.Find(Convert.ToInt16(cboproduct.SelectedValue));
            if (produ == null)
            {
                return;
            }
            dsReportTemp dsReport = new dsReportTemp();
            //dsReportTemp.ItemListDataTable dtItemReport = (dsReportTemp.ItemListDataTable)dsReport.Tables["LO'c_ItemSummary"];
            dsReportTemp.BarcodeDataTable dtItemReport = (dsReportTemp.BarcodeDataTable)dsReport.Tables["Barcode"];



            LinearRDLC barcode = new LinearRDLC();


            // set barcode type to Code 128
            barcode.Type = BarcodeType.CODE128;

            // draw barcodes for each data row

            // set barcode encoding data value
            barcode.Data = produ.Barcode.ToString();

            // set drawing barcode image format
            barcode.ImageFormat = System.Drawing.Imaging.ImageFormat.Png;
            barcode.drawBarcode("D://abc.png");
            int counter = Convert.ToInt16(txtrow.Text);
            if (counter > 1)
            {
                for (int i = 0; i < counter; i++)
                {
                    dsReportTemp.BarcodeRow reportrow = dtItemReport.NewBarcodeRow();
                    reportrow.id = (short)i;
                    reportrow.Barcode = barcode.drawBarcodeAsBytes();
                    reportrow.Name = produ.Name;
                    dtItemReport.AddBarcodeRow(reportrow);

                }
            }
            else
            {
                dsReportTemp.BarcodeRow reportrow = dtItemReport.NewBarcodeRow();
                reportrow.id = 1;
                reportrow.Barcode = barcode.drawBarcodeAsBytes();
                reportrow.Name = produ.Name;
                dtItemReport.AddBarcodeRow(reportrow);
            }




            ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["Barcode"]);
            string reportPath = Application.StartupPath + "\\Reports\\Barcode.rdlc";

            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.LocalReport.EnableExternalImages = true;
            ReportParameter img = new ReportParameter("price", produ.Price.ToString());
            reportViewer1.LocalReport.SetParameters(img);

            this.reportViewer1.RefreshReport();
        }

        private void btnrprint_Click(object sender, EventArgs e)
        {

        }
    }
}
