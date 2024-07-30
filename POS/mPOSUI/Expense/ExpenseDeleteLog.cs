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


namespace POS
{
    public partial class ExpenseDeleteLog : Form
    {
        #region Variables

        private POSEntities entity = new POSEntities();
        bool _startProcess = false;
        DateTime _fromDate = new DateTime();
        DateTime _toDate = new DateTime();
        #endregion

        #region Events

        public ExpenseDeleteLog()
        {
            InitializeComponent();
        }

        private void ExpenseDeleteLog_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            dtFrom.Format = DateTimePickerFormat.Custom;
            dtFrom.CustomFormat = "dd-MM-yyyy";

            dtTo.Format = DateTimePickerFormat.Custom;
            dtTo.CustomFormat = "dd-MM-yyyy";

            Default_Date();
            Utility.BindShop(cboshoplist);
            cboshoplist.Text = SettingController.DefaultShop.ShopName;

            Utility.BindExpenseCategory(cboExpenseCag);
            Utility.ShopComBo_EnableOrNot(cboshoplist);
            _startProcess = true;
            LoadData();
        }

        private void cboAdjType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            Date_Assign();
            LoadData();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            Date_Assign();
            LoadData();
        }

        private void cboExpenseCag_SelectedIndexChanged(object sender, EventArgs e)
        {
            Date_Assign();
            LoadData();
        }

        private void cboshoplist_selectedindexchanged(object sender, EventArgs e)
        {
            LoadData();
        }
        #endregion

        #region Function
        private void Default_Date()
        {
            dtTo.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            dtFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            Date_Assign();
        }

        private void Date_Assign()
        {
            _fromDate = dtFrom.Value;
            _toDate = dtTo.Value;
        }

        public void LoadData()
        {
            if (_startProcess == true)
            {
                dgvExpenseList.Columns[1].DefaultCellStyle.Format = "dd-MM-yyyy";

                int expenseCagId = Convert.ToInt32(cboExpenseCag.SelectedValue);
                dgvExpenseList.AutoGenerateColumns = false;
                int shopid = Convert.ToInt32(cboshoplist.SelectedValue);
                string currentshortcode = (from p in entity.Shops where p.Id == shopid select p.ShortCode).FirstOrDefault();

                var girdList = (from mp in entity.Expenses
                                where mp.IsDeleted == true
                                && mp.IsApproved == false
                               && (EntityFunctions.TruncateTime(mp.ExpenseDate.Value) >= _fromDate)
                             && (EntityFunctions.TruncateTime(mp.ExpenseDate.Value) <= _toDate)
                             && ((expenseCagId == 0 && 1 == 1) || (expenseCagId != 0 && mp.ExpenseCategoryId == expenseCagId))
                             && mp.Id.Substring(2,2)==currentshortcode
                                orderby mp.ExpenseDate descending
                                select new
                                {
                                    Id = mp.Id,
                                    ExpenseDate = mp.ExpenseDate,
                                    ExpenseNo = mp.Id,
                                    TotalExpenseAmount = mp.TotalExpenseAmount,
                                    CreatedUser = mp.User.Name,
                                    ExpenseCag = mp.ExpenseCategory.Name,
                                    Comment = mp.Comment

                                }).ToList();

                dgvExpenseList.DataSource = girdList;
                Calculating_TotalExpenseAmount();

            }
        }

        private void Calculating_TotalExpenseAmount()
        {
            Int64 TotalExpenseAmt = dgvExpenseList.Rows.Cast<DataGridViewRow>()
             .Sum(t => Convert.ToInt32(t.Cells["colTotalExpenseAmount"].Value));

            txtTotalExpenseAmt.Text = TotalExpenseAmt.ToString();
        }
        #endregion







    }
}
