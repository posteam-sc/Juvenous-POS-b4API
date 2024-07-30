using POS.APP_Data;
using POS.mPOSUI.CustomerPurchaseInvoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace POS
{
    public partial class PurchasedInvoiceSharers : Form
    {
        #region Variables

        POSEntities posEntity = new POSEntities();
        private ToolTip tp = new ToolTip();
        public string PurchasedInvoiceId { get; set; }
        public string PackageName { get; set; }
        public string OwnerName { get; set; }
        public int OwnerId { get; set; }
        private int CurrentCustomerId = 0;
        #endregion

        #region Event
        public PurchasedInvoiceSharers()
        {
            InitializeComponent();
        }

        private void PurchasedInvoiceShares_Load(object sender, EventArgs e)
        {
            CurrentCustomerId = OwnerId;
            Localization.Localize_FormControls(this);
            dgvInvoiceSharers.AutoGenerateColumns = false;
            lblPackageName.Text = PackageName;
            lblPackageOwner.Text = OwnerName;
            BindCustomer();
            BindDataGridView();
         }

        private void BindDataGridView()
        {

            dgvInvoiceSharers.DataSource = posEntity.PurchasedPackageSharers.Where(b=>b.PackageInvoiceId == PurchasedInvoiceId && b.IsDeleted == false).OrderByDescending(x=>x.Id).Select(x=> new SharedInvoiceComboSet(){ CustomerName=x.Customer1.Name, SharedDateTime=x.SharedDateTime }).ToList();

        }
        
        private void BindCustomer()
        {
            //Add Customer List with default option
            List<APP_Data.Customer> customerList = (from c in posEntity.Customers orderby c.Name select c).ToList();
            cboCustmer.DataSource = customerList;
            cboCustmer.DisplayMember = "Name";
            cboCustmer.ValueMember = "Id";
            cboCustmer.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboCustmer.AutoCompleteSource = AutoCompleteSource.ListItems;
        }
        private void Save()
        {
            tp.RemoveAll();
            tp = new ToolTip();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            if (!string.IsNullOrEmpty(cboCustmer.Text))
            {
                int customerId = 0;
                int.TryParse(cboCustmer.SelectedValue.ToString(), out customerId);
                APP_Data.PurchasedPackageSharer bObj = (from b in posEntity.PurchasedPackageSharers where b.PackageInvoiceId == PurchasedInvoiceId && (b.PackageOwnerId==customerId || b.SharedCustomerId==customerId) select b).FirstOrDefault();
                if (bObj == null)
                {
                    PurchasedPackageSharer newsharer = new PurchasedPackageSharer()
                    {
                        PackageInvoiceId = PurchasedInvoiceId,
                        PackageOwnerId = OwnerId,
                        SharedCustomerId = customerId,
                        SharedDateTime=DateTime.Now,
                        IsDeleted = false
                    };
                    CurrentCustomerId = customerId;
                    posEntity.PurchasedPackageSharers.Add(newsharer);
                    posEntity.SaveChanges();
                    BindDataGridView();
                }
                else
                {
                    tp.SetToolTip(cboCustmer, "Error");
                    tp.Show("This customer is current package owner or sharer!", cboCustmer);
                }
            }
            else
            {
                tp.SetToolTip(cboCustmer, "Error");
                tp.Show("Please choose a customer!", cboCustmer);
            }
            cboCustmer.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Save();
            
        }

        private void AdjustmentType_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(cboCustmer);
        }







        #endregion

        private void PurchasedInvoiceSharers_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (System.Windows.Forms.Application.OpenForms["UsePackageByCustomer"] != null)
            {
                UsePackageByCustomer newForm = (UsePackageByCustomer)System.Windows.Forms.Application.OpenForms["UsePackageByCustomer"];
                newForm.bindPackageSharedUser();
                newForm.SetCurrentUser(CurrentCustomerId);
            }
        }
    }
    class SharedInvoiceComboSet
    {
        public string CustomerName { get; set; }
        public DateTime SharedDateTime { get; set; }

    }
}
