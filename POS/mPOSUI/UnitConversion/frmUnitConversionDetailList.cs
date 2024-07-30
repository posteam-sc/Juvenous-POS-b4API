using POS.APP_Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Objects;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS.mPOSUI
{
    public partial class frmUnitConversionDetailList : Form
    {
        public frmUnitConversionDetailList()
        {
            InitializeComponent();
        }

        private void frmUnitConversionDetailList_Load(object sender, EventArgs e)
        {
            Search();

        }
        public class UnitConversion
        {
            public int Id { get; set; }
            public string  ProductName { get; set; }
            public int  Qty { get; set; }
            public DateTime ConvertDate { get; set; }
        }

        private void InitialState()
        {
            dtpFromDate.Value = DateTime.Now;
            dtpToDate.Value = DateTime.Now;
            Search();
        }
        private void Search()
        {
            POSEntities entity = new POSEntities();
            dgvUnitConversionList.AutoGenerateColumns = false;            
            List<UnitConversion> unitConversionSearchList = (from u in entity.UnitConversionHeaders
                                                             join p in entity.Products on u.FromProductId equals p.Id
                                                             where EntityFunctions.TruncateTime(u.CreatedDate) >= dtpFromDate.Value.Date && EntityFunctions.TruncateTime(u.CreatedDate) <= dtpToDate.Value.Date
                                                             select new UnitConversion
                                                             {
                                                                 Id = u.Id,
                                                                 ProductName = p.Name,
                                                                 Qty = u.FromQty,
                                                                 ConvertDate = u.CreatedDate
                                                             }).ToList();

            dgvUnitConversionList.DataSource = unitConversionSearchList;
        }

        private void dgvUnitConversionList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow rows in dgvUnitConversionList.Rows)
            {
                UnitConversion unitConversion =(UnitConversion) rows.DataBoundItem;
                rows.Cells[0].Value = unitConversion.Id;
                rows.Cells[1].Value= unitConversion.ProductName;
                rows.Cells[2].Value=unitConversion.Qty;
                rows.Cells[3].Value= unitConversion.ConvertDate;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            InitialState();
        }

        private void dgvUnitConversionList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            POSEntities entities = new POSEntities();
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 4)
                {
                    
                    frmUnitConversionDetail detail = new frmUnitConversionDetail();
                    detail.Headerid = Convert.ToInt32(dgvUnitConversionList.CurrentRow.Cells[0].Value);
                    detail.FromProductName =Convert.ToString(dgvUnitConversionList.CurrentRow.Cells[1].Value);
                    detail.Qty = Convert.ToInt32(dgvUnitConversionList.CurrentRow.Cells[2].Value);
                    detail.ShowDialog();

                }
            }
        }
    }
}
