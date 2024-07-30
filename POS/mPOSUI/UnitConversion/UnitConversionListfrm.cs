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
    public partial class UnitConversionListfrm : Form
    {
        #region "Initialize"
        public UnitConversionListfrm()
        {
            InitializeComponent();
        }
        #endregion

        #region Vairable
        POSEntities entity = new POSEntities();
        bool IsStart = false;
        #endregion

        #region Method
        private void Bind_GridData()
        {
            if (IsStart)
            {
                entity = new POSEntities();
                int maxProId = Convert.ToInt32(cboMaxProduct.SelectedValue);
                //DateTime _convertDate = dtpFrom.Value.Date;

                DateTime fromDate = dtpFrom.Value.Date;
                DateTime toDate = dtpTo.Value.Date;
                IQueryable<object> _conversionData = (from uc in entity.UnitConversions
                                                      join pro in entity.Products on uc.FromProductId equals pro.Id
                                                      join pro1 in entity.Products on uc.ToProductId equals pro1.Id
                                                      where (EntityFunctions.TruncateTime((DateTime)uc.ConversionDate) >= fromDate && EntityFunctions.TruncateTime((DateTime)uc.ConversionDate) <= toDate)
                                                      && ((maxProId == 0 && 1 == 1) || (maxProId != 0 && uc.FromProductId == maxProId))
                                                      orderby uc.ConversionDate
                                                      select new
                                                      {
                                                          Id = uc.Id,
                                                          ConversionDate = uc.ConversionDate,
                                                          MaximumProduct = pro.Name,
                                                          NormalProduct = pro1.Name,
                                                          MaximumConvertedQty = uc.FromQty,
                                                          PiecesPerPack = uc.OnePackQty,
                                                          TotalConvertedNormalQty = uc.ToQty,
                                                          NormalUnitPurPrice = uc.NormalUnitPurchasePrice
                                                      });
                List<object> _gridDatas = new List<object>(_conversionData);

                dgvConversionList.AutoGenerateColumns = false;
                dgvConversionList.DataSource = _gridDatas.ToList();
            }
      
        }
        #endregion

        #region Event
        private void UnitConversionListfrm_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            Utility.BindConversionProduct(cboMaxProduct, "Maximum");
            IsStart = true;
            Bind_GridData();
        }

        private void dtpConversionDate_ValueChanged(object sender, EventArgs e)
        {
            Bind_GridData();
        }

        private void cboMaxProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_GridData();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            Bind_GridData();
        }
        #endregion

      

    }
}
