using POS.APP_Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace POS.mPOSUI.CustomerPurchaseInvoice
{
    public partial class UsePackageByCustomer : Form
    {
        public string productname { get; set; }
        public string packageOwner { get; set; }
        public int Package_Qty { get; set; }
        public int UsedQty { get; set; }
        public int OwnerId { get; set; }
        List<Stock_Transaction> stockproductList = new List<Stock_Transaction>();

        public string packagePurchasedInvoiceId { get; set; }

        public UsePackageByCustomer()
        {
            InitializeComponent();
        }

        private void UsePackageByCustomer_Load(object sender, EventArgs e)
        {
            textDatabind();
            bindDoctor();
            bindAssistantNurse();
            bindTherapist();
            bindPackageSharedUser();
        }
        public void bindPackageSharedUser()
        {
            POSEntities entity = new POSEntities();
            entity = new POSEntities();
            List<APP_Data.Customer> customerList = new List<APP_Data.Customer>();
            APP_Data.Customer customerL = new APP_Data.Customer();
            customerL.Id = OwnerId;
            customerL.Name = packageOwner==null?"By Package Owner":packageOwner;
            customerList.Add(customerL);
            customerList.AddRange(entity.PurchasedPackageSharers.Where(x => x.PackageInvoiceId == packagePurchasedInvoiceId).Select(x=>x.Customer1).ToList());
            cboCurrentOffsetUser.DataSource = customerList;
            cboCurrentOffsetUser.DisplayMember = "Name";
            cboCurrentOffsetUser.ValueMember = "Id";
        }
        public void SetCurrentUser(int selectedValue)
        {
            cboCurrentOffsetUser.SelectedValue = selectedValue;
        }
        public void bindDoctor()
        {
            POSEntities entity = new POSEntities();
            entity = new POSEntities();
            List<APP_Data.Customer> doctorList = new List<APP_Data.Customer>();
            APP_Data.Customer doctor = new APP_Data.Customer();
            doctor.Id = 0;
            doctor.Name = "Select";
            doctorList.Add(doctor);
            doctorList.AddRange(entity.Customers.Where(x => x.CustomerTypeId == 2).ToList());
            cboDoctorName.DataSource = doctorList;
            cboDoctorName.DisplayMember = "Name";
            cboDoctorName.ValueMember = "Id";
        }

        public void bindTherapist()
        {
            POSEntities entity = new POSEntities();
            entity = new POSEntities();
            List<APP_Data.Customer> thereapistList = new List<APP_Data.Customer>();
            APP_Data.Customer therapsit = new APP_Data.Customer();
            therapsit.Id = 0;
            therapsit.Name = "Select";
            thereapistList.Add(therapsit);
            thereapistList.AddRange(entity.Customers.Where(x => x.CustomerTypeId == 3).ToList());
            cboTherepist.DataSource = thereapistList;
            cboTherepist.DisplayMember = "Name";
            cboTherepist.ValueMember = "Id";
        }

        public void bindAssistantNurse()
        {
            POSEntities entity = new POSEntities();
            entity = new POSEntities();
            List<APP_Data.Customer> nurseList = new List<APP_Data.Customer>();
            APP_Data.Customer nurse = new Customer();
            nurse.Id = 0;
            nurse.Name = "Select";
            nurseList.Add(nurse);
            nurseList.AddRange(entity.Customers.Where(x => x.CustomerTypeId == 4).ToList());
            cboNurse.DataSource = nurseList;
            cboNurse.DisplayMember = "Name";
            cboNurse.ValueMember = "Id";
        }

        private void Save_SaleQty_ToStockTransaction(List<Stock_Transaction> productList)
        {
            int _year, _month;

            _year = System.DateTime.Now.Year;
            _month = System.DateTime.Now.Month;
            Utility.Sale_Run_Process(_year, _month, productList);
        }

        private void textDatabind()
        {
            if (UsedQty == 0)
                txtAvailableQty.Text = Package_Qty.ToString();
            else txtAvailableQty.Text = (Package_Qty - UsedQty).ToString();

            txtproductname.Text = productname;
        }

        private void btnSubmit_Click_1(object sender, EventArgs e)
        {
            if (cboDoctorName.SelectedIndex < 0 || cboNurse.SelectedIndex < 0 || cboTherepist.SelectedIndex < 0)
            {
                MessageBox.Show("Select  Doctor or Nurse ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (cboCurrentOffsetUser.SelectedIndex < 0)
            {
                MessageBox.Show("Select  current offset customer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtQty.Text) || Convert.ToInt32(txtQty.Text) <= 0)
            {
                MessageBox.Show("Invalid data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Convert.ToInt32(txtQty.Text) > Convert.ToInt32(txtAvailableQty.Text))
            {
                MessageBox.Show("Not allow for input Quantity is greater than Abailable Qty", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you to use it?", "Use Package", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result.Equals(DialogResult.OK))
            {
                POSEntities entity = new POSEntities();
                var procedureList = entity.SetProcedureProducts.Where(x => x.ProcedureProductName.Trim() == txtproductname.Text).ToList();
                if (procedureList.Count > 0)
                {
                    foreach (var item in procedureList)
                    {

                        Product product = new Product();
                        product = entity.Products.Where(x => x.Name == item.ProductName).FirstOrDefault();
                        var productQty = entity.Products.Where(x => x.Name.Trim() == item.ProductName).Select(x => x.Qty).FirstOrDefault();
                        product.Qty = productQty - item.ProductQty;

                        //Save in Stock Transaction
                        Stock_Transaction st = new Stock_Transaction();
                        st.ProductId = item.ProductID;
                        st.Sale = item.ProductQty;
                        stockproductList.Add(st);
                        Save_SaleQty_ToStockTransaction(stockproductList);
                        stockproductList.Clear();

                        //Save In Purchase Detail
                        List<PurchaseDetail> pulist = Utility.InventoryByControlMethod(item.ProductID, entity);
                        if (pulist.Count > 0)
                        {
                            int TotalQty = Convert.ToInt32(pulist.Sum(x => x.CurrentQy));
                            int Qty = 0;
                            if (TotalQty >= item.ProductQty)
                            {
                                foreach (PurchaseDetail p in pulist)
                                {
                                    if (p.CurrentQy >= item.ProductQty)
                                    {
                                        p.CurrentQy = ((p.CurrentQy - item.ProductQty) - Qty);
                                        Qty = 0;
                                    }
                                    else
                                    {
                                        Qty =(int) (item.ProductQty - p.CurrentQy);
                                        p.CurrentQy = 0;
                                    }
                                }

                            }
                        }

                    }
                    entity.SaveChanges();
                }
                PackageUsedHistory packageUsedHistory = new PackageUsedHistory();
                packageUsedHistory.PackageUsedHistoryId = System.Guid.NewGuid().ToString();
                packageUsedHistory.PackagePurchasedInvoiceId = packagePurchasedInvoiceId;
                packageUsedHistory.UsedDate = DateTime.Now;
                packageUsedHistory.UseQty = Convert.ToInt32(txtQty.Text);
                packageUsedHistory.IsDelete = false;
                packageUsedHistory.UserId = MemberShip.UserId;
                packageUsedHistory.CustomerIDAsDoctor = Convert.ToInt32(cboDoctorName.SelectedValue);
                packageUsedHistory.CustomerIDAsAssistantNurse = Convert.ToInt32(cboNurse.SelectedValue);
                packageUsedHistory.CustomerIDAsTherapist = Convert.ToInt32(cboTherepist.SelectedValue);
                packageUsedHistory.Remark = txtRemark.Text;
                packageUsedHistory.ActualOffsetBy= Convert.ToInt32(cboCurrentOffsetUser.SelectedValue);
                entity.PackageUsedHistories.Add(packageUsedHistory);

                PackagePurchasedInvoice packagePurchaseinvoice = new PackagePurchasedInvoice();
                packagePurchaseinvoice = entity.PackagePurchasedInvoices.Where(x => x.PackagePurchasedInvoiceId == packagePurchasedInvoiceId).SingleOrDefault();
                if (packagePurchaseinvoice != null)
                {
                    packagePurchaseinvoice.UseQty += (Convert.ToInt32(txtQty.Text));
                    int i = entity.SaveChanges();
                    if (i > 0) MessageBox.Show("Success !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    CustomerPurchaseInvoice CustomerPurchaseInvoiceForm = new CustomerPurchaseInvoice();
                    CustomerPurchaseInvoiceForm.Show();
                }
            }
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.Close();
            CustomerPurchaseInvoice CustomerPurchaseInvoiceForm = new CustomerPurchaseInvoice();
            CustomerPurchaseInvoiceForm.Show();
        }

        private void btnCurrentOffSetUserAdd_Click(object sender, EventArgs e)
        {
            PurchasedInvoiceSharers frmPurchasedInvoiceSharers = new PurchasedInvoiceSharers();
            frmPurchasedInvoiceSharers.PurchasedInvoiceId = packagePurchasedInvoiceId;
            frmPurchasedInvoiceSharers.PackageName = productname;
            frmPurchasedInvoiceSharers.OwnerName = packageOwner;
            frmPurchasedInvoiceSharers.OwnerId = OwnerId;
            frmPurchasedInvoiceSharers.ShowDialog();
            
        }
    }
}
