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
using System.Data.Objects;
using Microsoft.Reporting.WinForms;

namespace POS
{
    public partial class ConsignmentSettlement : Form
    {        

        #region variable

        POSEntities entity = new POSEntities();
        Boolean IsStart = false;
        List<object> _Data = new List<object>();
        #endregion

        #region Event
        public ConsignmentSettlement()
        {
            InitializeComponent();
        }

        private void ConsignmentSettlement_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            #region Setting Hot Kyes For the Controls
            SendKeys.Send("%"); SendKeys.Send("%"); // Clicking "Alt" on page load to show underline of Hot Keys
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(ConsignmentSettlement_KeyDown);
            #endregion

            Utility.BindConsignor(cboConsignor, false);
            txtdefaultshopname.Text = SettingController.DefaultShop.ShopName;
            IsStart = true;
            Bind_GridData();
        }

        private void cboCounter_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_GridData();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            Bind_GridData();
        }

        private void btnPaidCash_Click(object sender, EventArgs e)
        {
            if (dgvConsingmentPaid.Rows.Count > 0)
            {
                int conId = 0;
                conId = Convert.ToInt32(cboConsignor.SelectedValue);

                string defaultshortcode = SettingController.DefaultShop.ShortCode;

                DateTime fromdate = dtpFrom.Value.Date;
                DateTime todate = dtpTo.Value.Date;

                List<string> type = new List<string> { "Refund", "CreditRefund" };

                //select consingment all records 
                var _tranDetailIdList = (from td in entity.TransactionDetails
                                         join t in entity.Transactions on td.TransactionId equals t.Id
                                         join p in entity.Products on td.ProductId equals p.Id
                                         join c in entity.ConsignmentCounters on p.ConsignmentCounterId equals c.Id
                                         where p.IsConsignment == true && p.ConsignmentCounterId == conId
                                          && (EntityFunctions.TruncateTime(t.DateTime.Value) >= fromdate)
                                                 && (EntityFunctions.TruncateTime(t.DateTime.Value) <= todate)
                                                 && (td.IsConsignmentPaid == false) && (td.IsDeleted == false)
                                                 && (!type.Contains(t.Type)) && t.Id.Substring(2,2)==defaultshortcode
                                         select (td.Id)).ToList();

                //collect trandetailIdList with comma 
               // string _saveTranDetailIdList = string.Join(",", _tranDetailIdList);

                #region saving Consignment Settlement
                APP_Data.ConsignmentSettlement _consignmentSettlement = new APP_Data.ConsignmentSettlement();

     

                string month = "";
                if (DateTime.Now.Month < 10)
                {
                    month = "0" + DateTime.Now.Month.ToString();
                }
                else
                {
                    month = DateTime.Now.Month.ToString();
                }
              
                int? count;
                //_count = (from con in entity.Expenses where (EntityFunctions.TruncateTime(con.CreatedDate) == date) orderby con.Id descending select con.Count ?? 0).FirstOrDefault();
                //_count += 1;
                count = (from c in entity.ConsignmentSettlements where (EntityFunctions.TruncateTime(c.CreatedDate) == todate) orderby c.Id descending select c.count ?? 0).FirstOrDefault();
                count += 1;
                string ConsignmentNo = "CNMT" + SettingController.DefaultShop.ShortCode + DateTime.Now.Year.ToString() + month + DateTime.Now.Day.ToString() + count.ToString();

                _consignmentSettlement.ConsignmentNo = ConsignmentNo;
                _consignmentSettlement.SettlementDate = DateTime.Now;
                _consignmentSettlement.ConsignorId = conId;
                //_consignmentSettlement.TransactionDetailId = _saveTranDetailIdList;
                _consignmentSettlement.TotalSettlementPrice = Convert.ToDecimal(txtTotalConsignPaidAmt.Text);
                _consignmentSettlement.FromTransactionDate = dtpFrom.Value;
                _consignmentSettlement.ToTransactionDate = dtpTo.Value;
                _consignmentSettlement.Comment = txtComment.Text;
                _consignmentSettlement.CreatedDate = DateTime.Now;
                _consignmentSettlement.CreatedBy = MemberShip.UserId;
                _consignmentSettlement.IsDelete = false;

                entity.ConsignmentSettlements.Add(_consignmentSettlement);
                entity.SaveChanges();


                //saving Consignment Settlement Detail
                 APP_Data.ConsignmentSettlementDetail _conSetDetail = new APP_Data.ConsignmentSettlementDetail();
                foreach(var _conDetail in _tranDetailIdList)
                {
                   
                    _conSetDetail.ConsignmentNo = ConsignmentNo;
                    _conSetDetail.TransactionDetailId = _conDetail;
                    entity.ConsignmentSettlementDetails.Add(_conSetDetail);
                    entity.SaveChanges();
                }
                #endregion

                //remove comma in trandetailIdList
                //string[] _TranDetailIdList = Utility.Remove_Comma(_saveTranDetailIdList);

                //convert string[] to List<long>
                //List<long> _editTranDetailIdList = Utility.Convert_String_To_Long(_TranDetailIdList);

                #region update Transaction Delist List (IsConsignmentPaid = true)
                (from t in entity.TransactionDetails where _tranDetailIdList.Contains(t.Id) select t).ToList().ForEach(t => t.IsConsignmentPaid = true);

                entity.SaveChanges();
                #endregion

                MessageBox.Show("Successfully made Consignment Settlement!", "mPOS");
                //Bind_GridData();


                #region Print
                ConsignmentSettlementReport form = new ConsignmentSettlementReport();
                form._Data = _Data;
                form.Consignor = cboConsignor.Text;
                form.ConsignmentNo = ConsignmentNo;
                form.SettlementDate = DateTime.Now.ToString("dd - MMM - yyyy");
                form.Month = DateTime.Now.ToString("MMMM");
                form.ShowDialog();

                #endregion
                Clear_Data();
            }
            if (System.Windows.Forms.Application.OpenForms["chart"] != null)
            {
                chart newForm = (chart)System.Windows.Forms.Application.OpenForms["chart"];
                newForm.FormFresh();
            }
        }

        void ConsignmentSettlement_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)     // Ctrl + P => Click Paid
            {
                cboConsignor.Focus();
            }
            else if (e.Control && e.KeyCode == Keys.F)     // Ctrl + P => Click Paid
            {
                dtpFrom.Focus();
            }
            else if (e.Control && e.KeyCode == Keys.T)     // Ctrl + P => Click Paid
            {
                dtpTo.Focus();
            }
            else if (e.Control && e.KeyCode == Keys.M)     // Ctrl + P => Click Paid
            {
                txtComment.Focus();
            }
            else if (e.Control && e.KeyCode == Keys.G)     // Ctrl + P => Click Paid
            {
                dgvConsingmentPaid.Focus();
            }
            else if (e.Control && e.KeyCode == Keys.P)     // Ctrl + P => Click Paid
            {
                btnPaidCash.PerformClick();
            }

        }
        #endregion

        #region Function
        private void Clear_Data()
        {
            dgvConsingmentPaid.DataSource = "";
            txtTotalConsignPaidAmt.Text = "0";
            txtTotalProfitAmt.Text = "0";
            txtComment.Clear();
        }

        private void Bind_GridData()
        {
            if (IsStart)
            {
                if (cboConsignor.SelectedIndex >= 0)
                {
                    entity = new POSEntities();
                    int conId = 0;
                    conId = Convert.ToInt32(cboConsignor.SelectedValue);

                    int shopId = 0;
                    shopId = SettingController.DefaultShop.Id;
                    string defaultshortcode = SettingController.DefaultShop.ShortCode;

                    DateTime fromdate = dtpFrom.Value.Date;
                    DateTime todate = dtpTo.Value.Date;

                    List<string> type = new List<string> { "Refund", "CreditRefund" };

                    //select consingment all records 
                    var ConsignList = (from td in entity.TransactionDetails
                                       join t in entity.Transactions on td.TransactionId equals t.Id
                                       join p in entity.Products on td.ProductId equals p.Id
                                       join c in entity.ConsignmentCounters on p.ConsignmentCounterId equals c.Id
                                       where p.IsConsignment == true && p.ConsignmentCounterId == conId
                                        && (EntityFunctions.TruncateTime(t.DateTime.Value) >= fromdate)
                                               && (EntityFunctions.TruncateTime(t.DateTime.Value) <= todate)
                                               && (td.IsConsignmentPaid == false) && (td.IsDeleted == false)
                                               && (t.Id.Substring(2,2) == defaultshortcode)
                                       group td by new { td.UnitPrice, td.ProductId, p.Name, td.ConsignmentPrice, t.Type, td.DiscountRate, p.Barcode } into totalConsignQty
                                       select new
                                       {
                                           ProductId = totalConsignQty.Key.ProductId,
                                           Barcode = totalConsignQty.Key.Barcode,
                                           Name = totalConsignQty.Key.Name,
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
                                                group g by new { g.ProductId, g.Name, g.SellingPrice, g.ConsignmentPrice, g.Barcode } into _gridData
                                                select new
                                                {
                                                    ProductId = _gridData.Key.ProductId,
                                                    Barcode = _gridData.Key.Barcode,
                                                    Name = _gridData.Key.Name,
                                                    SellingPrice = _gridData.Key.SellingPrice,
                                                    ConsignmentPrice = _gridData.Key.ConsignmentPrice,
                                                    ConsignQty = _gridData.Sum(s => s.ConsginQty) - _gridData.Sum(s => s.RefundQty),
                                                    TotalProfit = (_gridData.Key.SellingPrice - _gridData.Key.ConsignmentPrice) * (_gridData.Sum(s => s.ConsginQty) - _gridData.Sum(s => s.RefundQty)),
                                                    TotalConsignmentPrice = (_gridData.Sum(s => s.ConsginQty) - _gridData.Sum(s => s.RefundQty)) * _gridData.Key.ConsignmentPrice,
                                                });


                        _Data = new List<object>(q);

                        dgvConsingmentPaid.AutoGenerateColumns = false;
                        dgvConsingmentPaid.DataSource = _Data.ToList();

                        txtTotalConsignPaidAmt.Text = (dgvConsingmentPaid.Rows.Cast<DataGridViewRow>()
                                                                .Sum(t => Convert.ToInt32(t.Cells[5].Value))).ToString();

                        txtTotalProfitAmt.Text = (dgvConsingmentPaid.Rows.Cast<DataGridViewRow>()
                                                                .Sum(t => Convert.ToInt32(t.Cells[6].Value))).ToString();
                    }
                    else
                    {
                        Clear_Data();
                    }

                }
            }
        }
        #endregion

    }
}
