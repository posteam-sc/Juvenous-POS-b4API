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
using System.Text.RegularExpressions;

namespace POS
{
    public partial class mType : Form
    {
        #region Variables

        public Boolean isEdit { get; set; }

        public int CustomerId { get; set; }

        private POSEntities entity = new POSEntities();

        private ToolTip tp = new ToolTip();

        #endregion
        public mType()
        {
            InitializeComponent();
        }

        private void NewMember_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            cboTitle.Items.Add("Mr");
            cboTitle.Items.Add("Mrs");
            cboTitle.Items.Add("Miss");
            cboTitle.Items.Add("Ms");
            cboTitle.Items.Add("U");
            cboTitle.Items.Add("Daw");
            cboTitle.Items.Add("Ko");
            cboTitle.Items.Add("Ma");
            cboTitle.Items.Add("Mg");
            cboTitle.SelectedIndex = 0;

            List<APP_Data.City> cityList = new List<APP_Data.City>();
            APP_Data.City city1 = new APP_Data.City();
            city1.Id = 0;
            city1.CityName = "Select";
            cityList.Add(city1);
            cityList.AddRange(entity.Cities.ToList());
            cboCity.DataSource = cityList;
            cboCity.DisplayMember = "CityName";
            cboCity.ValueMember = "Id";
        }
    }
}
