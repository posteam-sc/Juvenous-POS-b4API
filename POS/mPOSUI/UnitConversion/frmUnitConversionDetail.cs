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

namespace POS.mPOSUI
{
    public partial class frmUnitConversionDetail : Form
    {
        public int Headerid { get; set; }
        public string FromProductName { get; set; }
        public int Qty { get; set; }     
        public frmUnitConversionDetail()
        {
            InitializeComponent();
        }

        private void bindData()
        {
            POSEntities entites = new POSEntities();
            List<UnitConvertDetail> unitConvertDetail = (from u in entites.UnitConversionDetails
                                                         join p in entites.Products on u.ToProductId equals p.Id
                                                         select new UnitConvertDetail
                                                         {
                                                            ToProductName=p.Name,
                                                            ToQty =u.ToQty
                                                         }
                                                             ).ToList();
            dgvUnitConversionDetail.DataSource = unitConvertDetail;

        }
        public class UnitConvertDetail
        {
            public string ToProductName { get; set; }
            public int ToQty { get; set; }
        }
        private void frmUnitConversionDetail_Load(object sender, EventArgs e)
        {
            lblFromProductName.Text = FromProductName;
            lblQty.Text = Qty.ToString();
            dgvUnitConversionDetail.AutoGenerateColumns = false;
            bindData();
            
        }

        private void dgvUnitConversionDetail_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvUnitConversionDetail.Rows)
            {
                UnitConvertDetail unitConvertDetail =(UnitConvertDetail)row.DataBoundItem;
                row.Cells[0].Value=  unitConvertDetail.ToProductName;
                row.Cells[1].Value= unitConvertDetail.ToQty;
            }
        }
    }
}
