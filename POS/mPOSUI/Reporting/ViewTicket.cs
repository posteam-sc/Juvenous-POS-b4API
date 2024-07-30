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
    public partial class ViewTicket : Form
    {
        #region Variable
        private POSEntities entity = new POSEntities();
        public int transactionDetailId = 0;
        public string transactionId;
        public List<TransactionDetail> DetailList = new List<TransactionDetail>();
        string shopname = string.Empty;
        string tranno = string.Empty;
        #endregion
        public ViewTicket()
        {
            InitializeComponent();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvViewTicket.Rows)
             {
                if (Convert.ToBoolean(row.Cells[4].Value))
                {
                  
                    string tId = "";
                    tId = row.Cells[5].Value.ToString();  // chk in dgv                   
                    var curTicket = entity.Tickets.Where(t => t.Id == tId).FirstOrDefault();
                   

                    if (curTicket.Status == false && DateTime.Now.Date == curTicket.CreatedDate.Date) 
                    {
                       
                        shopname = SettingController.DefaultShop.ShopName;
                       
                        dsReportTemp dsReport = new dsReportTemp();
                        dsReportTemp.TicketReportDataTable dtReport = (dsReportTemp.TicketReportDataTable)dsReport.Tables["TicketReport"];
                        dsReportTemp.TicketReportRow newRow = dtReport.NewTicketReportRow();
                        newRow.DateTime = curTicket.CreatedDate.Date.ToString("dd-MM-yyyy");
                        newRow.EventName = shopname;

                        QRCoder.QRCodeGenerator qrgenerator = new QRCoder.QRCodeGenerator();
                        QRCoder.QRCodeData qrdata = qrgenerator.CreateQrCode(curTicket.TicketNo, QRCoder.QRCodeGenerator.ECCLevel.Q);
                        QRCoder.QRCode qrcode = new QRCoder.QRCode(qrdata);
                        Bitmap qrImage = qrcode.GetGraphic(20);

                        ImageConverter converter = new ImageConverter();

                        newRow.QRCode = (byte[])converter.ConvertTo(qrImage, typeof(byte[]));

                        dtReport.AddTicketReportRow(newRow);

                        string reportPath = "";
                        ReportViewer rv = new ReportViewer();
                        ReportDataSource rds = new ReportDataSource("Ticket", dsReport.Tables["TicketReport"]);

                        reportPath = Application.StartupPath + "\\Reports\\Ticket.rdlc";

                        rv.Reset();
                        rv.LocalReport.ReportPath = reportPath;
                        rv.LocalReport.DataSources.Add(rds);

                        ReportParameter TranNo = new ReportParameter("TranNo", curTicket.TransactionDetail.Transaction.Id);
                        rv.LocalReport.SetParameters(TranNo);

                        ReportParameter ShopName = new ReportParameter("ShopName", shopname);
                        rv.LocalReport.SetParameters(ShopName);

                        Utility.Get_Print(rv);

                        APP_Data.Ticket Edt = entity.Tickets.Where(x => x.Id == tId).FirstOrDefault();
                        Edt.RePrint = Edt.RePrint == null ? 1 : ++Edt.RePrint;

                        entity.Entry(Edt).State = EntityState.Modified;
                        entity.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to reprint because it is expired!", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        
                    }
                
                    
                }
             }
        }


     
        private void ViewTicket_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
           LoadData();
        }

        private void LoadData()
        {
            dgvViewTicket.AutoGenerateColumns = false;
            var currentTransaction = entity.Transactions.Find(transactionId);
            var list = currentTransaction.TransactionDetails.SelectMany(a => a.Tickets).Where(b => b.isDelete == false || b.isDelete==null).ToList();
            dgvViewTicket.DataSource = list;

        }

        private void dgvViewTicket_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvViewTicket.Rows)
            {
                Ticket curti = (Ticket)row.DataBoundItem;
                row.Cells[0].Value = curti.TicketNo;
                row.Cells[1].Value = curti.Status;
                row.Cells[2].Value = curti.CreatedDate;
                row.Cells[3].Value = curti.Category;
                row.Cells[5].Value = curti.Id;
            }
        }
    }
}
