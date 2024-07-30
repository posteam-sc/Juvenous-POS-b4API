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

namespace POS
{
    public partial class ConsignmentSettlementList : Form
    {

        #region Initilaize
        public ConsignmentSettlementList()
        {
            InitializeComponent();
        }
        #endregion

        #region variable
        int _Month_Number;
        POSEntities entity = new POSEntities();
        Boolean IsStart = false;
        #endregion

        #region Event
        private void ConsignmentSettlementList_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            #region Setting Hot Kyes For the Controls
            SendKeys.Send("%"); SendKeys.Send("%"); // Clicking "Alt" on page load to show underline of Hot Keys
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(ConsignmentSettlement_KeyDown);
            #endregion

            var startYear = DateTime.Now.Year ;
            cboYear.DataSource = Enumerable.Range(startYear, 100).ToList();
            cboYear.Text = DateTime.Now.ToString("yyyy");
            cboMonth.Text = DateTime.Now.ToString("MMMM");

           // cboMonth.SelectedIndex = 0;
            cboMonth.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboMonth.AutoCompleteSource = AutoCompleteSource.ListItems;
            Month_Number();
            Utility.BindConsignor(cboConsignor);
            Utility.BindShop(cboshoplist);

            cboshoplist.Text = SettingController.DefaultShop.ShopName;
            Utility.ShopComBo_EnableOrNot(cboshoplist);
            IsStart = true;
            Bind_GridData();
        }

        void ConsignmentSettlement_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)     // Ctrl + P => Click Paid
            {
                cboConsignor.Focus();
            }
            else if (e.Control && e.KeyCode == Keys.M)     // Ctrl + M => Click Paid
            {
                cboMonth.Focus();
            }
        }

        private void cboConsignor_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_GridData();
        }
        private void cboshoplist_selectedIndexChanged(object sender, EventArgs e)
        {
            Bind_GridData();
        }


        private void dgvConSettlementList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                //edit
                if (e.ColumnIndex == 10)
                {
                    ConsignmentSettlement_DetailList form = new ConsignmentSettlement_DetailList();
                    form.ConsignmentId = Convert.ToInt32(dgvConSettlementList.Rows[e.RowIndex].Cells[0].Value);
                    //form.TransactionDetailId = dgvConSettlementList.Rows[e.RowIndex].Cells[1].Value.ToString();
                    form.ConsignmentNo = dgvConSettlementList.Rows[e.RowIndex].Cells[2].Value.ToString();
                    form.Consignor = dgvConSettlementList.Rows[e.RowIndex].Cells[3].Value.ToString();
                    form.SettlementDate = Convert.ToDateTime(dgvConSettlementList.Rows[e.RowIndex].Cells[4].Value).ToString("dd-MM-yyyy");
                    form.MonthName = Convert.ToDateTime(dgvConSettlementList.Rows[e.RowIndex].Cells[4].Value).ToString("MMMM");
                    form.Comment = dgvConSettlementList.Rows[e.RowIndex].Cells[9].Value.ToString();
                    form.ShowDialog();
                }
                    //delete
                else if (e.ColumnIndex == 11)
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (result.Equals(DialogResult.OK))
                    {
                        int _conSignId = Convert.ToInt32(dgvConSettlementList.Rows[e.RowIndex].Cells[0].Value);
                       
                        string consingmentNo=dgvConSettlementList.Rows[e.RowIndex].Cells[2].Value.ToString();
                        var _editTranDetailIdList = entity.ConsignmentSettlementDetails.Where(x => x.ConsignmentNo == consingmentNo).Select(x => x.TransactionDetailId).ToList();

                        #region update Transaction Detail list table And Consignment Settlement table
                        // update Transaction Delist table (IsConsignmentPaid = false)
                        (from t in entity.TransactionDetails where _editTranDetailIdList.Contains(t.Id) select t).ToList().ForEach(t => t.IsConsignmentPaid = false);

                        // update ConsignmentSettlement table (IsDelete = true)
                        (from t in entity.ConsignmentSettlements where t.Id == _conSignId select t).ToList().ForEach(t => t.IsDelete = true);

                       
                        entity.SaveChanges();
                        Bind_GridData();
                        #endregion
                    }
                }
            }
        }

        private void cboMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            Month_Number();
            Bind_GridData();
        }
        #endregion

        #region Function
        private void Month_Number()
        {
            switch (cboMonth.Text)
            {
                case "Janauary":
                    _Month_Number = 1;
                    break;
                case "February":
                    _Month_Number = 2;
                    break;
                case "March":
                    _Month_Number = 3;
                    break;
                case "April":
                    _Month_Number = 4;
                    break;
                case "May":
                    _Month_Number = 5;
                    break;
                case "June":
                    _Month_Number = 6;
                    break;
                case "July":
                    _Month_Number = 7;
                    break;
                case "August":
                    _Month_Number = 8;
                    break;
                case "September":
                    _Month_Number = 9;
                    break;
                case "October":
                    _Month_Number = 10;
                    break;
                case "November":
                    _Month_Number = 11;
                    break;
                case "December":
                    _Month_Number = 12;
                    break;
            }
        }

        private void Bind_GridData()
        {
            if (IsStart)
            {
                int consignId = 0;
                int _year = Convert.ToInt32( cboYear.Text);
                if (cboConsignor.SelectedIndex > 0)
                {
                    consignId = Convert.ToInt32(cboConsignor.SelectedValue);
                }
                int shopid = Convert.ToInt32(cboshoplist.SelectedValue);
                string currentshortcode = (from p in entity.Shops where p.Id == shopid select p.ShortCode).FirstOrDefault();
                entity = new POSEntities();
                dgvConSettlementList_CustomCellFormatting();
                IQueryable<object> _conSettlementData = (from c in entity.ConsignmentSettlements
                                                         join con in entity.ConsignmentCounters on c.ConsignorId equals con.Id
                                                         join u in entity.Users on c.CreatedBy equals u.Id
                                                         where ((consignId == 0 && 1 == 1 || consignId != 0 && c.ConsignorId == consignId)) && (c.IsDelete == false)
                                                         && (c.SettlementDate.Month == _Month_Number) 
                                                         &&(c.SettlementDate.Year == _year)
                                                         && c.ConsignmentNo.Substring(4,2)==currentshortcode
                                                         orderby c.SettlementDate
                                                         select new
                                                         {
                                                             Id = c.Id,
                                                             ConsignmentNo=c.ConsignmentNo,
                                                             //TranDetailID = c.TransactionDetailId,
                                                             Consignor = con.Name,
                                                             SettlementDate = c.SettlementDate,
                                                             SettlementAmount = c.TotalSettlementPrice,
                                                             CreatedUser=u.Name,
                                                             FromSaleDate=c.FromTransactionDate,
                                                             ToSaleDate=c.ToTransactionDate,
                                                             Comment=c.Comment
                                                             
                                                         });
                List<object> _gridDatas = new List<object>(_conSettlementData);

                dgvConSettlementList.AutoGenerateColumns = false;
                dgvConSettlementList.DataSource = _gridDatas.ToList();

                txtTotalSettlementAmount.Text = (dgvConSettlementList.Rows.Cast<DataGridViewRow>()
                                                                       .Sum(t => Convert.ToInt32(t.Cells[5].Value))).ToString();
            }
        }

        private void dgvConSettlementList_CustomCellFormatting()
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            // Consignment Settlement List Delete
            if (!MemberShip.isAdmin && !controller.ConsigmentSettlement.EditOrDelete)
            {
                dgvConSettlementList.Columns["colDelete"].Visible = false;
            }
        
        }

        private void cboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_GridData();
        }
        #endregion

     
    }
}
