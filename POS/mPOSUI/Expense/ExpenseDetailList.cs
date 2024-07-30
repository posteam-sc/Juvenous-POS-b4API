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
    public partial class ExpenseDetailList : Form
    {
        #region Variables

        POSEntities entity = new POSEntities();
        public string expenseId="";
        public string ExpenseCag = "";
        public string ExpenseNo = "";
        public string ExpenseDate = "";

        #endregion

        #region Event

        public ExpenseDetailList()
        {
            InitializeComponent();
            CenterToScreen();
        }

        private void PurchaseDetailList_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            loadData();
        }

        public void loadData()
        {

            lblExpenseCag.Text = ExpenseCag;
            lblExpenseNo.Text = ExpenseNo;
            lblExpenseDate.Text = ExpenseDate;
            dgvExpenseList.AutoGenerateColumns = false;

            var girdList = (from mp in entity.ExpenseDetails
                            where mp.ExpenseId == expenseId

                            orderby mp.Description descending
                            select new
                            {
                                Id = mp.Id,
                                Description = mp.Description,
                                Qty = mp.Qty,

                                UnitPrice = mp.Price,
                                Amount = (mp.Qty * mp.Price)

                            }).ToList();

            dgvExpenseList.DataSource = girdList;
            Calculating_TotalExpenseAmount();
        }
        #endregion

        #region Function
        private void Calculating_TotalExpenseAmount()
        {
            Int64 TotalExpenseAmt = dgvExpenseList.Rows.Cast<DataGridViewRow>()
             .Sum(t => Convert.ToInt32(t.Cells["colAmount"].Value));

            txtTotalExpenseAmt.Text = TotalExpenseAmt.ToString();
        }
        #endregion
    }
}
