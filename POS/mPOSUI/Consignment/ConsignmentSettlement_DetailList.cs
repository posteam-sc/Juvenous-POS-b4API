using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using POS.APP_Data;

namespace POS
{
    public partial class ConsignmentSettlement_DetailList : Form
    {
        #region Initialize
        public ConsignmentSettlement_DetailList()
        {
            InitializeComponent();
        }
        #endregion

        #region Variable
        public string Consignor = "";
        public string ConsignmentNo = "";
        public string SettlementDate = "";
        public int ConsignmentId = 0;
       // public string TransactionDetailId = "";
        public string MonthName = "";
        POSEntities entity = new POSEntities();
        public string Comment = "";
        List<object> _gridData = new List<object>();
        #endregion

        #region Function
        private void Bind_GridData()
        {
            entity=new POSEntities();
            //remove Comma
           // string[] _removeCommaList=Utility.Remove_Comma(TransactionDetailId);

            //convert string[] to Long[]
           // List<long> _longTranDetailidList = Utility.Convert_String_To_Long(_removeCommaList);

            var _longTranDetailidList = entity.ConsignmentSettlementDetails.Where(x => x.ConsignmentNo == ConsignmentNo).Select(x => x.TransactionDetailId).ToList();
            IQueryable<object> _getData = (from _detail in entity.TransactionDetails
                                    join pro in entity.Products on _detail.ProductId equals pro.Id
                                    where _longTranDetailidList.Contains(_detail.Id)
                                    select new
                                    {
                                        TranDetailId= _detail.Id,
                                        Barcode=pro.Barcode,
                                        Name = pro.Name,
                                        ConsignQty=_detail.Qty,
                                        SellingPrice = (_detail.UnitPrice) - (_detail.UnitPrice * _detail.DiscountRate / 100),
                                        ProfitPrice = ((_detail.UnitPrice) - (_detail.UnitPrice * _detail.DiscountRate / 100) - _detail.ConsignmentPrice),
                                        ConsignmentPrice=_detail.ConsignmentPrice
                                    });

             _gridData = new List<object>(_getData);
            dgvConsignmentDetail.AutoGenerateColumns = false;
            dgvConsignmentDetail.DataSource = _gridData.ToList();

            txtTotalQuantity.Text= (dgvConsignmentDetail.Rows.Cast<DataGridViewRow>()
                                                                .Sum(t => Convert.ToInt32(t.Cells[3].Value))).ToString();

            txtTotalSellingAmt.Text = (dgvConsignmentDetail.Rows.Cast<DataGridViewRow>()
                                                                .Sum(t => Convert.ToInt32(t.Cells[4].Value) * Convert.ToInt32(t.Cells[3].Value))).ToString();

            txtTotalConsignmentAmt.Text = (dgvConsignmentDetail.Rows.Cast<DataGridViewRow>()
                                                           .Sum(t => Convert.ToInt32(t.Cells[5].Value) * Convert.ToInt32(t.Cells[3].Value) )).ToString();

            txtTotalProfitAmt.Text = (dgvConsignmentDetail.Rows.Cast<DataGridViewRow>()
                                                           .Sum(t => Convert.ToInt32(t.Cells[6].Value) * Convert.ToInt32(t.Cells[3].Value))).ToString();
        }
        #endregion

        #region Event
        private void ConsignmentSettlement_DetailList_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            txtConsignor.Text = Consignor;
            txtConsignmentNo.Text = ConsignmentNo;
            txtSettlementDate.Text = SettlementDate;

            Bind_GridData();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            #region Print
            ConsignmentSettlementReport form = new ConsignmentSettlementReport();
            form._Data = _gridData;
            form.Consignor = txtConsignor.Text;
            form.ConsignmentNo = txtConsignmentNo.Text;

            form.SettlementDate = txtSettlementDate.Text;
            form.Month = DateTime.Now.ToString("MMMM");
            form.ShowDialog();

            #endregion
        }
        #endregion

    
    }
}
