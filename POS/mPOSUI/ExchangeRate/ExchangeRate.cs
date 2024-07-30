using POS.APP_Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS
{
    public partial class ExchangeRate : Form
    {
        #region Variables

        POSEntities entity = new POSEntities();
        private ToolTip tp = new ToolTip();
        public Boolean IsCloseAtOnce = false;

        #endregion

        public ExchangeRate()
        {
            InitializeComponent();
        }

        private void ExchangeRate_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            List<Currency> currencyList = new List<Currency>();
            Currency currencyObj = new Currency();
            currencyObj.CurrencyCode = "select";
            currencyObj.Id = 0;
            currencyList.Add(currencyObj);
            currencyList.AddRange(entity.Currencies.ToList());
            cboCurrency.DataSource = currencyList;
            cboCurrency.DisplayMember = "CurrencyCode";
            cboCurrency.ValueMember = "Id";
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Boolean hasError = false;

            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";

            if (cboCurrency.SelectedIndex == 0)
            {
                tp.SetToolTip(cboCurrency, "Error");
                tp.Show("Please select currency!", cboCurrency);
                hasError = true;
            }
            if (txtExchangeRate.Text.Trim() != string.Empty)
            {
                if (cboCurrency.SelectedIndex == 0)
                {
                    tp.SetToolTip(cboCurrency, "Error");
                    tp.Show("Please select currency!", cboCurrency);
                    hasError = true;
                }
            }



            if (!hasError)
            {
                int id = Convert.ToInt32(cboCurrency.SelectedValue);
                Currency c = entity.Currencies.FirstOrDefault(x => x.Id == id);
                Int32 ExchangeRate = 0;
                Int32.TryParse(txtExchangeRate.Text, out ExchangeRate);
                c.LatestExchangeRate = ExchangeRate;
                entity.SaveChanges();
                MessageBox.Show("Successfully Save!", "Save");

                if (IsCloseAtOnce) this.Dispose();

                cboCurrency.SelectedIndex = 0;
                txtExchangeRate.Text = "";
            }
        }

        private void cboCurrency_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboCurrency.SelectedIndex > 0)
            {
                int id = Convert.ToInt32(cboCurrency.SelectedValue);
                Currency currentCurrency = entity.Currencies.FirstOrDefault(x => x.Id == id);
                if (currentCurrency != null)
                {
                    txtExchangeRate.Text = currentCurrency.LatestExchangeRate.ToString();
                }
                else
                {
                    txtExchangeRate.Text = "0";
                }
            }
        }

        private void tableLayoutPanel6_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(cboCurrency);
            tp.Hide(txtExchangeRate);
        }
    }
}
