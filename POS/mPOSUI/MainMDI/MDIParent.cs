using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using POS.APP_Data;
using POS.mPOSUI.CustomerPurchaseInvoice;
using POS.mPOSUI.DoctorSchedule;
using POS.mPOSUI.MedicalPerson;
using POS.mPOSUI.DoctorBooking;
using POS.mPOSUI.Reporting;
using POS.mPOSUI;
using POS;

namespace POS
{
    public partial class MDIParent : Form
    {
        #region Events

        private int childFormNumber = 0;

        public MDIParent()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void MDIParent_Load(object sender, EventArgs e)
        {

            this.TopMost = SettingController.TopMost;
            
            #region SQL ServiceCheck
            if (new Utility.DBService().AllowToStart && !new Utility.DBService().Running)
            {
                new Utility.DBService().Run();
            }
            #endregion
            Localization.Forms_Controls_Insertion();
            Localization.Localize_FormControls(this);
            Create_RequirementsForDB();
            if (!Utility.IsRegister())
            {
                Register regform = new Register();
                regform.WindowState = FormWindowState.Maximized;
                regform.MdiParent = this;
                regform.Show();
                this.menuStrip.Enabled = false;
            }
            else if (!MemberShip.isLogin)
            {
                Login loginForm = new Login();
                loginForm.MdiParent = this;
                loginForm.WindowState = FormWindowState.Maximized;
                loginForm.Show();
                this.menuStrip.Enabled = false;
                toolStripStatusLabel.Text = string.Empty;
                toolStripStatusLabel1.Text = string.Empty;
                tSSBOOrPOS.Text = string.Empty;
                statusToolStripMenuItem.Text = string.Empty;
            }
            else
            {
                Sales form = new Sales();
                form.WindowState = FormWindowState.Maximized;
                form.MdiParent = this;
                form.Show();
            }
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.ShowDialog();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      


        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void productTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductSubCategory newform = new ProductSubCategory();
            newform.ShowDialog();
        }

        private void customerListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.OutstandingCustomer.View || MemberShip.isAdmin)
            {
                OutstandingCustomerList form = new OutstandingCustomerList();
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view outstanding customer", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void productCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductCategory form = new ProductCategory();
            form.ShowDialog();
        }

        private void addNewCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.Customer.Add || MemberShip.isAdmin)
            {
                //mType form = new mType();
                NewCustomer form = new NewCustomer();
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to add customer", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void brandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Brand newForm = new Brand();
            newForm.ShowDialog();
        }

        private void counterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Counter newForm = new Counter();
            newForm.ShowDialog();
        }

        private void userListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserControl form = new UserControl();
            form.ShowDialog();
        }

        private void newUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewUser form = new NewUser();
            form.ShowDialog();
        }
        private void transactionSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.TransactionSummaryReport.View || MemberShip.isAdmin)
            {
                TransactionSummary newform = new TransactionSummary();
                newform.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view transaction summary report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }



        }

        private void newProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewProduct newForm = new NewProduct();
            newForm.ShowDialog();
        }

        private void productListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemList newForm = new ItemList();
            newForm.ShowDialog();
        }

        private void giftCardControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GiftCardControl newForm = new GiftCardControl();
            newForm.ShowDialog();
        }

        private void transactionListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TransactionList newForm = new TransactionList();
            newForm.ShowDialog();
        }

        private void startDayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartDay form = new StartDay();
            form.ShowDialog();
        }

        private void endDayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EndDay form = new EndDay();
            form.ShowDialog();
        }

        private void refundListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefundList newform = new RefundList();
            newform.ShowDialog();
        }

        private void unitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UnitForm newform = new UnitForm();
            newform.ShowDialog();
        }

        private void logInToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Boolean isAlreadyHave = false;

            foreach (Form child in this.MdiChildren)
            {
                if (child.Text == "Login")
                {
                    child.Activate();
                    isAlreadyHave = true;
                }
                else
                {
                    child.Close();
                }
            }
            if (!isAlreadyHave)
            {
                Login f = new Login();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result.Equals(DialogResult.OK))
            {
                Boolean isAlreadyHave = false;

                foreach (Form child in this.MdiChildren)
                {
                    if (child.Text == "LogIn")
                    {
                        child.Activate();
                        isAlreadyHave = true;
                    }
                    else
                    {
                        child.Close();
                    }
                }
                MemberShip.UserId = 0;
                MemberShip.UserName = "";
                MemberShip.UserRole = null;
                if (!isAlreadyHave)
                {
                    Login f = new Login();
                    f.MdiParent = this;
                    f.WindowState = FormWindowState.Maximized;
                    f.Show();
                }
                menuStrip.Enabled = false;
                toolStripStatusLabel.Text = string.Empty;
                toolStripStatusLabel1.Text = string.Empty;
                tSSBOOrPOS.Text = string.Empty;
                statusToolStripMenuItem.Text = string.Empty;
                logOutToolStripMenuItem.Visible = false;
                logInToolStripMenuItem1.Visible = true;
            }
        }

        private void creditTransactionListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreditTransactionList newForm = new CreditTransactionList();
            newForm.ShowDialog();
        }

        private void transactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.TransactionReport.View || MemberShip.isAdmin)
            {
                TransactionReport_FOC_MPU newform = new TransactionReport_FOC_MPU();
                newform.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view transaction report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void itemSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.ItemSummaryReport.View || MemberShip.isAdmin)
            {
                ItemSummary newform = new ItemSummary();
                newform.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view item sale summary report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void roleManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RoleManagement newform = new RoleManagement();
            newform.ShowDialog();
        }

        private void taxesSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.TaxSummaryReport.View || MemberShip.isAdmin)
            {
                TaxesSummary newform = new TaxesSummary();
                newform.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view tax summary report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void addCityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.City.Add || MemberShip.isAdmin)
            {
                City newForm = new City();
                newForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to add city", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void deleteLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteLogForm newform = new DeleteLogForm();
            newform.ShowDialog();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Setting newform = new Setting();
            newform.ShowDialog();
        }

        private void itemPurchaseOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.ReorderPointReport.View || MemberShip.isAdmin)
            {
                PurchaseOrderItem newform = new PurchaseOrderItem();
                newform.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view reorder point report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void transactionDetailByItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.TransactionDetailReport.View || MemberShip.isAdmin)
            {
                TransactionDetailByItem newform = new TransactionDetailByItem();
                newform.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view transaction detail report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }

        private void taxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Taxes newform = new Taxes();
            newform.ShowDialog();
        }

        private void outstandingCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.OutstandingCustomerReport.View || MemberShip.isAdmin)
            {
                OutstandingCustomerReport newform = new OutstandingCustomerReport();
                newform.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view outstanding report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void customerListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.Customer.View || MemberShip.isAdmin)
            {
                CustomerList newForm = new CustomerList();
                newForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view customer list", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void addNewProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.Product.Add || MemberShip.isAdmin)
            {
                NewProduct newForm = new NewProduct();
                newForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to add new product", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }

        private void productCategoryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.Category.View || MemberShip.isAdmin)
            {
                ProductCategory form = new ProductCategory();
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to add product category", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void productSubCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.SubCategory.View || MemberShip.isAdmin)
            {
                ProductSubCategory newform = new ProductSubCategory();
                newform.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view sub product category", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void brandToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.Brand.View || MemberShip.isAdmin)
            {
                Brand newForm = new Brand();
                newForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view brand", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void productListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.Product.View || MemberShip.isAdmin)
            {
                ItemList newForm = new ItemList();
                newForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view product", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void addNewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewUser form = new NewUser();
            form.ShowDialog();
        }

        private void userListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            UserControl form = new UserControl();
            form.ShowDialog();
        }

        private void roleManagementToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RoleManagement newform = new RoleManagement();
            newform.ShowDialog();
        }

        private void configurationSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Setting newform = new Setting();
            newform.ShowDialog();
        }

        private void counterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.Counter.Add || MemberShip.isAdmin)
            {
                Counter newForm = new Counter();
                newForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to add counter", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void giftCardContToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.GiftCard.Add || MemberShip.isAdmin)
            {
                GiftCardControl newForm = new GiftCardControl();
                newForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to add gift card.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void measurementUnitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.MeasurementUnit.Add || MemberShip.isAdmin)
            {
                UnitForm newform = new UnitForm();
                newform.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to add measurement unit.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void taxRatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.TaxRate.Add || MemberShip.isAdmin)
            {
                Taxes newform = new Taxes();
                newform.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to add tax rate.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void transactionListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TransactionList newForm = new TransactionList();
            newForm.ShowDialog();
        }

        private void creditTransactionListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CreditTransactionList newForm = new CreditTransactionList();
            newForm.ShowDialog();
        }

        private void refundListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RefundList newform = new RefundList();
            newform.ShowDialog();
        }


        private void addSupplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewSupplier newForm = new NewSupplier();
            newForm.ShowDialog();
        }

        private void supplierListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SupplierList newForm = new SupplierList();
            newForm.ShowDialog();
        }

        private void productPurchaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PurchaseInput newForm = new PurchaseInput();
            newForm.ShowDialog();
        }

        private void newPurchaseOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.PurchaseRole.Add || MemberShip.isAdmin)
            {
                PurchaseInput newForm = new PurchaseInput();
                newForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to add new purchasing", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void purchaseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.PurchaseRole.View || MemberShip.isAdmin)
            {
                PurchaseListBySupplier newform = new PurchaseListBySupplier();
                newform.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view purchase list", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void databaseExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Backup(true);
        }

        private void databaseImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName = Backup(false);
            Restore(ref fileName);
        }

        private void MDIParent_FormClosing(object sender, FormClosingEventArgs e)
        {
            toolStripStatusLabel.Text = "Saving data.. Please wait";
            //Only main server will make backup
            if (DatabaseControlSetting._ServerName.ToUpper().StartsWith(System.Environment.MachineName.ToUpper()))
            {
                Backup(true);
            }
        }

        private void topToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.TopBestSellerReport.View || MemberShip.isAdmin)
            {
                TopSaleReport newForm = new TopSaleReport();
                newForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view best seller item  report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }
        private void addConsigmentCounterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.Consignor.Add || MemberShip.isAdmin)
            {
                ConsignmentProductCounter newForm = new ConsignmentProductCounter();
                newForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to add counter", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }
        private void consignmentCounterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.Consigment.View || MemberShip.isAdmin)
            {
                ConsignmentProductReport newForm = new ConsignmentProductReport();
                newForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view consigment report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }



        }


        #endregion


        #region Functions


        private void Create_RequirementsForDB()
        {
            POSEntities entity = new POSEntities();
            // First Time Running... and Database is Null / Create a default Admin User
            var _userRoleList = entity.UserRoles.ToList();

            if (_userRoleList.Count == 0)
            {
                #region UserRole
                UserRole _userRole = new UserRole();
                _userRole.RoleName = "Admin";
                entity.UserRoles.Add(_userRole);
                entity.SaveChanges();

                _userRole.RoleName = "Super Cashier";
                entity.UserRoles.Add(_userRole);
                entity.SaveChanges();

                _userRole.RoleName = "Cashier";
                entity.UserRoles.Add(_userRole);
                entity.SaveChanges();
                #endregion

                #region City
                APP_Data.City _city = new APP_Data.City();
                _city.CityName = "Yangon";
                _city.IsDelete = false;
                entity.Cities.Add(_city);
                entity.SaveChanges();
                #endregion


                #region Shop
                APP_Data.Shop _shop = new APP_Data.Shop();
                _shop.ShopName = "MainOffice";
                _shop.CityId = 1;
                _shop.ShortCode = "MO";
                _shop.IsDefaultShop = true;

                entity.Shops.Add(_shop);
                entity.SaveChanges();
                #endregion

                #region Currency
                Currency _currency = new Currency();
                _currency.Country = "Myanmar";
                _currency.Symbol = "Ks";
                _currency.CurrencyCode = "MMK";

                _currency.LatestExchangeRate = 1;
                entity.Currencies.Add(_currency);
                entity.SaveChanges();

                _currency.Country = "United State of America";
                _currency.Symbol = "$";
                _currency.CurrencyCode = "USD";
                _currency.LatestExchangeRate = 1000;
                entity.Currencies.Add(_currency);
                entity.SaveChanges();
                #endregion

                #region Tax
                Tax _tax = new Tax();
                _tax.Name = "None";
                _tax.IsDelete = false;
                _tax.TaxPercent = 0;
                entity.Taxes.Add(_tax);
                entity.SaveChanges();
                #endregion

                #region Setting
                APP_Data.Setting _setting = new APP_Data.Setting();
                _setting.Key = "barcode_printer";
                _setting.Value = "Plz Choose Bar Code Printer";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "a4_printer";
                _setting.Value = "Plz Choose A4 Printer";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "slip_printer_counter1";
                _setting.Value = "Plz Choose Slip Printer";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "shop_name";
                _setting.Value = "Plz Enter Shop Name";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "branch_name";
                _setting.Value = "Plz Enter Branch Name or Address";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "phone_number";
                _setting.Value = "Plz Enter Phone Number";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "opening_hours";
                _setting.Value = "Plz Enter Opening Hours";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "default_tax_rate";
                _setting.Value = "1";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "default_city_id";
                _setting.Value = "1";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "default_top_sale_row";
                _setting.Value = "1";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "default_currency";
                _setting.Value = "1";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "Company_StartDate";
                _setting.Value = DateTime.Now.ToString();
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "default_printer";
                _setting.Value = "Plz choose Default Printer";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "IsBackOffice";
                _setting.Value = "1";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "default_font";
                _setting.Value = "English";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "IsSourcecode";
                _setting.Value = "False";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "pos_id";
                _setting.Value = "0";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "retrieve_pattern";
                _setting.Value = "fefo";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "allow_dynamic";
                _setting.Value = "False";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "app_mode";
                _setting.Value = "Production";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "usetable";
                _setting.Value = "False";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "usequeue";
                _setting.Value = "False";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "service_fee";
                _setting.Value = "0";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "detect_idle";
                _setting.Value = "False";
                entity.Settings.Add(_setting);
                entity.SaveChanges();


                _setting.Key = "idle_Time";
                _setting.Value = "1";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "ticketsale";
                _setting.Value = "False";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "allow_minimize";
                _setting.Value = "True";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "topmost";
                _setting.Value = "False";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "customsku";
                _setting.Value = "False";
                entity.Settings.Add(_setting);
                entity.SaveChanges();


                _setting.Key = "uppercase";
                _setting.Value = "False";
                entity.Settings.Add(_setting);
                entity.SaveChanges();


                _setting.Key = "a4image";
                _setting.Value = "False";
                entity.Settings.Add(_setting);
                entity.SaveChanges();



                #endregion

                #region User
                string UserCodeNo = "";
                User _user = new User();
                _user.ShopId = 1;
                _user.Name = "admin";

                _user.UserRoleId = 1;
                _user.Password = Utility.EncryptString("123456", "SCPos");
                _user.DateTime = System.DateTime.Now;
                _user.MenuPermission = "Both";

                //to get user code No
                UserCodeNo = Utility.Get_UserCodeNo(1);

                _user.UserCodeNo = UserCodeNo;
                entity.Users.Add(_user);
                entity.SaveChanges();


                _user.ShopId = 1;
                _user.Name = "sourcecodeadmin";
                _user.UserRoleId = 1;
                _user.Password = Utility.EncryptString("Sourcec0de", "SCPos");
                _user.DateTime = System.DateTime.Now;
                _user.MenuPermission = "Both";
                //to get user code No
                UserCodeNo = Utility.Get_UserCodeNo(1);

                _user.UserCodeNo = UserCodeNo;

                entity.Users.Add(_user);
                entity.SaveChanges();
                #endregion

                #region Counter
                APP_Data.Counter _counter = new APP_Data.Counter();
                _counter.Name = "One";
                _counter.IsDelete = false;
                entity.Counters.Add(_counter);
                entity.SaveChanges();
                #endregion

                #region Customer Type

                CustomerType customerType = new CustomerType();
                customerType.TypeName = "Patient";
                entity.CustomerTypes.Add(customerType);
                entity.SaveChanges();

                customerType.TypeName = "Doctor";
                entity.CustomerTypes.Add(customerType);
                entity.SaveChanges();

                customerType.TypeName = "Therapist";
                entity.CustomerTypes.Add(customerType);
                entity.SaveChanges();

                customerType.TypeName = "Nurse Aid";
                entity.CustomerTypes.Add(customerType);
                entity.SaveChanges();
                #endregion


                #region Customer
                APP_Data.Customer _customer = new APP_Data.Customer();
                _customer.Name = "Default";
                _customer.CityId = 1;
                _customer.CustomerTypeId = 1;
                _customer.Title = "Mr";
                _customer.PhoneNumber = "";
                _customer.Address = "";
                _customer.NRC = "";
                _customer.Email = "";
                _customer.Birthday = DateTime.Now;
                _customer.Gender = "";
                _customer.VIPMemberId = "";
                _customer.TownShip = "";


                var _customercode = entity.CustomerAutoID(DateTime.Now, SettingController.DefaultShop.ShortCode);
                _customer.CustomerCode = _customercode.FirstOrDefault().ToString();

                entity.Customers.Add(_customer);
                entity.SaveChanges();
                #endregion

                #region PaymentType
                PaymentType CashpaymentType = new PaymentType();

                CashpaymentType.Id =Convert.ToInt32(1);
                CashpaymentType.Name = "Cash";
                entity.PaymentTypes.Add(CashpaymentType);

                PaymentType CreditpaymentType = new PaymentType();
                CreditpaymentType.Id =Convert.ToInt32(2);
                CreditpaymentType.Name = "Credit";
                entity.PaymentTypes.Add(CreditpaymentType);

                PaymentType GpaymentType = new PaymentType();
                GpaymentType.Id =Convert.ToInt32(3);
                GpaymentType.Name = "GiftCard";
                entity.PaymentTypes.Add(GpaymentType);

                PaymentType FpaymentType = new PaymentType();
                FpaymentType.Id = Convert.ToInt32(4);
                FpaymentType.Name = "FOC";
                entity.PaymentTypes.Add(FpaymentType);

                PaymentType BpaymentType = new PaymentType();
                BpaymentType.Id =Convert.ToInt32(5);
                BpaymentType.Name = "Bank";
                entity.PaymentTypes.Add(BpaymentType);

                PaymentType TBpaymentType = new PaymentType();
                TBpaymentType.Id =Convert.ToInt32(6);
                TBpaymentType.Name = "Tester";
                entity.PaymentTypes.Add(TBpaymentType);

                PaymentType BankpaymentType = new PaymentType();
                BankpaymentType.Id =Convert.ToInt32(501);
                BankpaymentType.Name = "Bank Transfer";
                entity.PaymentTypes.Add(BankpaymentType);

                PaymentType MpaymentType = new PaymentType();
                MpaymentType.Id =Convert.ToInt32(502);
                MpaymentType.Name = "MPU";
                entity.PaymentTypes.Add(MpaymentType);

                PaymentType JpaymentType = new PaymentType();
                JpaymentType.Id =Convert.ToInt32(503);
                JpaymentType.Name = "JCB";
                entity.PaymentTypes.Add(JpaymentType);

                PaymentType VpaymentType = new PaymentType();
                VpaymentType.Id =Convert.ToInt32(504);
                VpaymentType.Name = "Visa";
                entity.PaymentTypes.Add(VpaymentType);

                PaymentType MaspaymentType = new PaymentType();
                MaspaymentType.Id =Convert.ToInt32(505);
                MaspaymentType.Name = "Master";
                entity.PaymentTypes.Add(MaspaymentType);

                PaymentType UpaymentType = new PaymentType();
                UpaymentType.Id =Convert.ToInt32(506);
                UpaymentType.Name = "Union Pay";
                entity.PaymentTypes.Add(UpaymentType);

                PaymentType KpaymentType = new PaymentType();
                KpaymentType.Id =Convert.ToInt32(507);
                KpaymentType.Name = "KBZ Pay";
                entity.PaymentTypes.Add(KpaymentType);

                PaymentType CpaymentType = new PaymentType();
                CpaymentType.Id =Convert.ToInt32(508);
                CpaymentType.Name = "CB Pay";
                entity.PaymentTypes.Add(CpaymentType);

                PaymentType ApaymentType = new PaymentType();
                ApaymentType.Id =Convert.ToInt32(509);
                ApaymentType.Name = "AYA Pay";
                entity.PaymentTypes.Add(ApaymentType);

                PaymentType WpaymentType = new PaymentType();
                WpaymentType.Id =Convert.ToInt32(510);
                WpaymentType.Name = "Wave Pay";
                entity.PaymentTypes.Add(WpaymentType);

                PaymentType OpaymentType = new PaymentType();
                OpaymentType.Id =Convert.ToInt32(511);
                OpaymentType.Name = "One Pay";
                entity.PaymentTypes.Add(OpaymentType);

                PaymentType SpaymentType = new PaymentType();
                SpaymentType.Id =Convert.ToInt32(512);
                SpaymentType.Name = "Sai Sai Pay";
                entity.PaymentTypes.Add(SpaymentType);

                entity.SaveChanges();

                #endregion



            }

            //if (entity.TicketButtonAssigns.Count() == 0)
            //{
            //    for (int i = 0; i < 4; i++)
            //    {
            //        TicketButtonAssign tba = new TicketButtonAssign();
            //        tba.ButtonName = "btnAdd" + (i + 1).ToString();
            //        tba.ButtonText = "+";

            //        entity.TicketButtonAssigns.Add(tba);
            //        entity.SaveChanges();
            //        TicketButtonAssign tba1 = new TicketButtonAssign();
            //        tba1.ButtonName = "btnAdd" + (i + 1).ToString() + "Minus";
            //        tba1.ButtonText = "-";

            //        entity.TicketButtonAssigns.Add(tba1);
            //        entity.SaveChanges();
            //        if (tba.ButtonName == "btnAdd1")
            //        {
            //            TicketButtonAssign lblLocalAdult = new TicketButtonAssign();
            //            lblLocalAdult.ButtonName = "lblLocalAdult";
            //            lblLocalAdult.ButtonText = "0";

            //            entity.TicketButtonAssigns.Add(lblLocalAdult);
            //            entity.SaveChanges();
            //        }
            //        if (tba.ButtonName == "btnAdd2")
            //        {
            //            TicketButtonAssign lblLocalChild = new TicketButtonAssign();
            //            lblLocalChild.ButtonName = "lblLocalChild";
            //            lblLocalChild.ButtonText = "0";
            //            entity.TicketButtonAssigns.Add(lblLocalChild);
            //            entity.SaveChanges();
            //        }
            //        if (tba.ButtonName == "btnAdd3")
            //        {
            //            TicketButtonAssign lblForeignAdult = new TicketButtonAssign();
            //            lblForeignAdult.ButtonName = "lblForeignAdult";
            //            lblForeignAdult.ButtonText = "0";
            //            entity.TicketButtonAssigns.Add(lblForeignAdult);
            //            entity.SaveChanges();
            //        }
            //        if (tba.ButtonName == "btnAdd4")
            //        {
            //            TicketButtonAssign lblForeignChild = new TicketButtonAssign();
            //            lblForeignChild.ButtonName = "lblForeignChild";
            //            lblForeignChild.ButtonText = "0";
            //            entity.TicketButtonAssigns.Add(lblForeignChild);
            //            entity.SaveChanges();
            //        }
            //    }
            //}

        }

        private void Restore(ref string fileName)
        {
            if (MessageBox.Show("Are you sure that you want to restore", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                if (ofdDBBackup.ShowDialog(this) == DialogResult.Cancel)
                {
                    return;
                }

                POSEntities entity = new POSEntities();
                entity.ClearDBConnections();

                fileName = ofdDBBackup.FileName;
                string destFileName = string.Empty;
                string[] fnArr1 = fileName.Split('_');
                string[] fnArr2 = fileName.Split('.');
                string[] fnArr3 = fileName.Split('\\');
                string filePath = string.Empty;
                for (int i = 0; i < fnArr3.Length - 1; i++)
                {
                    if (i + 1 != fnArr3.Length - 1)
                    {
                        filePath += fnArr3[i] + "/";
                    }
                    else
                    {
                        filePath += fnArr3[i];
                    }
                }

                /*--Decrypt DB--*/
                for (int i = 0; i < fnArr1.Length - 1; i++)
                {
                    if (i + 1 != fnArr1.Length - 1)
                    {
                        destFileName += fnArr1[i] + "_";
                    }
                    else
                    {
                        destFileName += fnArr1[i];
                    }
                }
                destFileName = destFileName + "." + fnArr2[1];
                if (File.Exists(destFileName)) File.Delete(destFileName);

                Utility.DecryptFile(fileName, destFileName);

                /*--Restore DB--*/
                RestoreHelper restoreHelper = new RestoreHelper();

                string[] tempConString = Properties.Settings.Default.MyConnectionString.Split(';');
                string[] userNameArr = tempConString[tempConString.Length - 2].Split('=');
                string[] passwordArr = tempConString[tempConString.Length - 1].Split('=');

                //restoreHelper.RestoreDatabase(Utility._DBName, destFileName + "." + fnArr2[1], Utility._ServerName, userNameArr[userNameArr.Length - 1], passwordArr[passwordArr.Length - 1]);
                restoreHelper.RestoreDatabase(DatabaseControlSetting._DBName, destFileName, DatabaseControlSetting._ServerName, DatabaseControlSetting._DBUser, DatabaseControlSetting._DBPassword);
                try
                {
                    if (File.Exists(destFileName))
                    {
                        File.Delete(destFileName);
                    }
                    MessageBox.Show("Successfully Restored..");
                }
                catch
                {
                    MessageBox.Show("Can't remove temp files", "Error!!");
                }
            }
        }

        private string Backup(Boolean IsManual)
        {
            if (SettingController.IsUsedS3Upload)
            {
                S3Uploader.BackupBAK();
                return string.Empty;
            }
            string activeDir = @"d:\";
            bool directorD = true;
            if (!Directory.Exists(activeDir))
            {
                directorD = false;
                activeDir = @"c:\";
            }
            /*-- Create a new subfolder under the current active folder --*/
            string newPath;
            if (IsManual)
            {
                newPath = System.IO.Path.Combine(activeDir, "Manual_Backups");
            }
            else
            {
                newPath = System.IO.Path.Combine(activeDir, "DB_Backups");
            }

            if (!System.IO.Directory.Exists(newPath))
            {
                DirectoryInfo di = Directory.CreateDirectory(newPath);
                //di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }

            /*-- Backup DB --*/
            BackupHelper backHelper = new BackupHelper();
            string fileName;
            string bakName = DatabaseControlSetting._DBName + "[" + DateTime.Now.ToString("dd-MM-yyyy hh-mm tt") + "].bak";
            if (IsManual)
                fileName = directorD == true ? "D:/Manual_Backups/" + bakName : "C:/Manual_Backups/" + bakName;
            else
                fileName = directorD == true ? "D:/DB_Backups/" + bakName : "C:/DB_Backups/" + bakName;

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            bool isBackup = false;
            backHelper.BackupDatabase(DatabaseControlSetting._DBName, DatabaseControlSetting._DBUser, DatabaseControlSetting._DBPassword, DatabaseControlSetting._ServerName, fileName, ref isBackup);

            /*-- Encrypt DB --*/
            string[] fileNameArr = fileName.Split('\\');
            string[] encryptFileNameArr = fileName.Split('.');
            string tempFileName = encryptFileNameArr[0] + "_encrypted." + encryptFileNameArr[1];

            if (File.Exists(tempFileName))
            {
                File.Delete(tempFileName);
            }
            if (isBackup)
            {
                Utility.EncryptFile(fileName, tempFileName);
            }

            try
            {
                File.Delete(fileName);
                if (isBackup)
                {
                    MessageBox.Show("Successfully Exported to " + newPath);
                }
            }
            catch
            {
                MessageBox.Show("Can't remove temporary files");
            }
            return fileName;
        }

        #endregion

        private void customerInfomationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.CustomerInformation.View || MemberShip.isAdmin)
            {
                FrmCustomerInfomation newform = new FrmCustomerInfomation();
                newform.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view customer information report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void productReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.ProductReport.View || MemberShip.isAdmin)
            {
                ProductReport_frm newform = new ProductReport_frm();
                newform.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view product report report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }

        private void customersSaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.CustomerSales.View || MemberShip.isAdmin)
            {
                frmCustomerSaleReport newform = new frmCustomerSaleReport();
                newform.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view customer sale report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void saleBreakDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.SaleBreakdown.View || MemberShip.isAdmin)
            {
                SaleBreakDown newform = new SaleBreakDown();
                newform.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view sale breakdown report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }



        }

        private void productPriceChangeHistoryListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PriceChangeHistoryList newform = new PriceChangeHistoryList();
            newform.ShowDialog();
        }

        private void saleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if ((Application.OpenForms["Sales"] as Sales) != null)
            {
                //Form is already open
            }
            else
            {
                Sales form = new Sales();
                form.WindowState = FormWindowState.Maximized;
                form.MdiParent = this;
                form.Show();
            }
        }

        private void dailySummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.DailySaleSummary.View || MemberShip.isAdmin)
            {
                DailySummaryReport newForm = new DailySummaryReport();
                newForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view daily summary report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("MPOS.chm");
        }

        private void purchaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.PurchaseReport.View || MemberShip.isAdmin)
            {
                PurchaseReport newform = new PurchaseReport();
                newform.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view purchase report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }

        private void dailyTotalTransactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.DailyTotalTransactions.View || MemberShip.isAdmin)
            {
                DailyTotalReport newform = new DailyTotalReport();
                newform.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view daily total transaction report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void purchaseDeleteLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.PurchaseRole.View || MemberShip.isAdmin)
            {
                PurchaseDeleteLog_frm newform = new PurchaseDeleteLog_frm();
                newform.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view purchase delete", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }

        private void profitAndLossToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.ProfitAndLoss.View || MemberShip.isAdmin)
            {
                ProfitAndLoss_frm newform = new ProfitAndLoss_frm();
                newform.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view  profit and loss report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }

        private void purchaseDiscountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.PurchaseDiscount.View || MemberShip.isAdmin)
            {
                PurchaseDiscountReport_frm newform = new PurchaseDiscountReport_frm();
                newform.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view purchase discount report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }

        private void addMemeberRuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MemberRule newMember = new MemberRule();
            newMember.ShowDialog();
        }

        private void averageMonthlyReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.AverageMonthlyReport.View || MemberShip.isAdmin)
            {
                AverageMonthlySaleReport_frm avgMonthlyReport = new AverageMonthlySaleReport_frm();
                avgMonthlyReport.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view transaction detail report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void currencyExchangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.CurrencyExchange.Add || MemberShip.isAdmin)
            {
                ExchangeRate newForm = new ExchangeRate();
                newForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to add currency exchange", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void outstandingSupplierListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.OutstandingSupplier.View || MemberShip.isAdmin)
            {
                OutstandingSupplierList form = new OutstandingSupplierList();
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view outstanding supplier list", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void addNewAdjustmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.Adjustment.Add || MemberShip.isAdmin)
            {
                AdjustmentFrm form = new AdjustmentFrm();
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to add damage.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void AdjustmentListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.Adjustment.View || MemberShip.isAdmin)
            {
                AdjustmentListFrm form = new AdjustmentListFrm();
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view damage.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void AdjustmentToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.AdjustmentReport.View || MemberShip.isAdmin)
            {
                AdjustmentRpt form = new AdjustmentRpt();
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view damage report.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void AdjustmentDeleteLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.Adjustment.EditOrDelete || MemberShip.isAdmin)
            {
                AdjustmentDeleteLog form = new AdjustmentDeleteLog();
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view damage delete log.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }



        private void stockTransactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StockTransaction form = new StockTransaction();
            form.ShowDialog();
        }

        private void stockTransactionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.StockTransactionReport.View || MemberShip.isAdmin)
            {
                StockTransactionReport form = new StockTransactionReport();
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view stock transaction report.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void consignmentPaidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.ConsigmentSettlement.EditOrDelete || MemberShip.isAdmin)
            {
                ConsignmentSettlement form = new ConsignmentSettlement();
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view stock transaction report.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void consignmentSettlementListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.ConsigmentSettlement.View || MemberShip.isAdmin)
            {
                ConsignmentSettlementList form = new ConsignmentSettlementList();
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view stock transaction report.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void stockUnitConversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.UnitConversion.Add || MemberShip.isAdmin)
            {
                UnitConversionfrm form = new UnitConversionfrm();
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to add stock unit conversion.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void stockUnitConversionListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.UnitConversion.View || MemberShip.isAdmin)
            {
                UnitConversionListfrm form = new UnitConversionListfrm();
                form.ShowDialog(this);
            }
            else
            {
                MessageBox.Show("You are not allowed to view stock unit conversion.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void createExpenseEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.Expense.Add || MemberShip.isAdmin)
            {
                ExpenseEntry form = new ExpenseEntry();
                form.ShowDialog(this);
            }
            else
            {
                MessageBox.Show("You are not allowed to create expense.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void expenseListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.Expense.View || MemberShip.isAdmin)
            {
                ExpenseList form = new ExpenseList();
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view expense list.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void expenseDeleteLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.Expense.DeleteLog || MemberShip.isAdmin)
            {
                ExpenseDeleteLog form = new ExpenseDeleteLog();
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view expense delete log.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void addExpenseCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.ExpenseCategory.Add || MemberShip.isAdmin)
            {
                ExpenseCategory form = new ExpenseCategory();
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to add expense category.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void netIncomeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.NetIncomeReport.View || MemberShip.isAdmin)
            {
                frmNetIncomeReport form = new frmNetIncomeReport();
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view net income report.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void addShopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Shop form = new Shop();
            form.ShowDialog();
        }

        private void stockInOutReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StockTransReturnForm _form = new StockTransReturnForm();
            _form.ShowDialog();
        }

        private void stockTransactionListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StockReceiveList _form = new StockReceiveList();
            _form.ShowDialog();
        }

        private void centralizedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Backup(false);
            Centralized _frm = new Centralized();
            _frm.ShowDialog();
        }

        private void tSSBOOrPOS_Click(object sender, EventArgs e)
        {
            switch (tSSBOOrPOS.Text)
            {
                case "Continue to Back Office ->":

                    ContinueToBOORPOS("BackOffice");
                    break;
                case "Continue to POS ->":
                    ContinueToBOORPOS("POS");
                    break;
            }
        }

        private void ContinueToBOORPOS(string _menu)
        {
            POSEntities entity = new POSEntities();
            string menuPermission = entity.Users.Where(x => x.Id == MemberShip.UserId).Select(x => x.MenuPermission).FirstOrDefault();
            Login form = new Login();
            form.MdiParent = this;
            form.Permission_BO_OR_POS(_menu, menuPermission);

        }

        private void cSVExportImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MemberShip.isAdmin)
            {
                csvExportImport form = new csvExportImport();
                form.ShowDialog(this);
            }
            else
            {
                MessageBox.Show("You are not allowed to view net income report.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void statusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (statusToolStripMenuItem.Text)
            {
                case "Continue to Back Office":

                    ContinueToBOORPOS("BackOffice");
                    break;
                case "Continue to POS":
                    ContinueToBOORPOS("POS");
                    break;
            }
        }

        private void expenseReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExpenseReport form = new ExpenseReport();
            form.ShowDialog(this);
        }

        private void generateBarcodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Barcode newform = new Barcode();
            newform.Show(this);
        }

        private void localizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmlocalize newform = new frmlocalize();
            newform.Show(this);
        }

        private void versiontoolStripMenuItem_Click(object sender, EventArgs e)
        {
            VersionInfo newfrom = new VersionInfo();
            newfrom.Show(this);
        }

        private void MDIParent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift && e.KeyCode == Keys.D)
            {

                if (MemberShip.isAdmin)
                {
                    if (string.IsNullOrEmpty(SettingController.ApplicationMode))
                    {
                        SettingController.ApplicationMode = "Production";
                    }
                    if (SettingController.ApplicationMode == "Developer")
                    {
                        DialogResult diaResult = MessageBox.Show("Swith to Production Mode", "mPOS", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (diaResult == DialogResult.OK)
                        {
                            SettingController.ApplicationMode = "Production";
                        }
                    }
                    else if (SettingController.ApplicationMode == "Production")
                    {
                        DialogResult diaResult = MessageBox.Show("Swith to Developer Mode", "mPOS", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (diaResult == DialogResult.OK)
                        {
                            SettingController.ApplicationMode = "Developer";
                        }
                    }
                }
            }
            else if (e.Control && e.KeyCode == Keys.R)
            {
                chart newchart = new chart();
                newchart.FormFresh();
            }
        }

        private void productexpiretoolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductExpireReport newform = new ProductExpireReport();
            newform.ShowDialog();
        }

        private void stockagingtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            StockAgingReport newfrom = new StockAgingReport();
            newfrom.ShowDialog();
        }

        private void addTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTable newfrom = new AddTable();
            newfrom.ShowDialog();
        }
        public string minimizePass { get; set; }
        private void MDIParent_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized && !SettingController.AllowMinimize)
            {
                if (minimizePass != "scadm1n")
                {
                    this.WindowState = FormWindowState.Maximized;
                    GeneralPassword newform = new GeneralPassword();
                    newform.Parent = this;
                    DialogResult dir = newform.ShowDialog();
                }
                if (minimizePass == "scadm1n")
                {
                    this.WindowState = FormWindowState.Minimized;
                    minimizePass = "";
                }
                else
                {
                    this.WindowState = FormWindowState.Maximized;
                    minimizePass = "";
                    MessageBox.Show("You can't minimize mPOS");
                }
            }
        }

        private void assignTicketToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AssignTicketButton form = new AssignTicketButton();
            form.Show();
        }

        private void promotionSystemMenuItem_Click(object sender, EventArgs e) {
            PromotionSystem promotionSystemForm = new PromotionSystem();
            promotionSystemForm.Show();
            }

        private void promotionSystemListToolStripMenuItem_Click(object sender, EventArgs e) {
            Promotion_List promotioListForm = new Promotion_List();
            promotioListForm.Show();
            }

        private void noveltySystemToolStripMenuItem_Click(object sender, EventArgs e) {
            NoveltySystem noveltySystemForm = new NoveltySystem();
            noveltySystemForm.Show();
            }

        private void noveltySystemListToolStripMenuItem_Click(object sender, EventArgs e) {
            Novelty_List noveltyListForm = new Novelty_List();
            noveltyListForm.Show();
            }

        private void gWPPWPTransactionToolStripMenuItem_Click(object sender, EventArgs e) {
            GWPTransactionsReport gwptransactionreport = new GWPTransactionsReport();
            gwptransactionreport.Show();
            }

        private void novelToolStripMenuItem_Click(object sender, EventArgs e) {
            NoveltiesSaleReport noveltiesSaleReport = new NoveltiesSaleReport();
            noveltiesSaleReport.Show();
            }

        private void customerSalePackageToolStripMenuItem_Click(object sender, EventArgs e) {
            new CustomerPurchaseInvoice().Show();
            }

        private void doctorScheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoctorSchedulefrm doctorScheduleForm = new DoctorSchedulefrm();
            doctorScheduleForm.Show();
        }

        private void toolStripMenuItemDoctorSchedule_Click(object sender, EventArgs e)
        {
            new DoctorScheduleList().Show();
        }

        private void newDoctorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmMedicalPerson().Show();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            new FrmMedicalPersonList().Show();
        }

        private void doctorBookingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmDoctorBooking().Show();
        }

        private void doctorBookingListToolStripMenuItem_Click(object sender, EventArgs e)
        {
         new FrmBookingList().Show();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DoctorOffsetReport form = new DoctorOffsetReport();
            form.Show();
        }

        private void patientOffsetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PatientOffsetReport patienOffsetForm = new PatientOffsetReport();
            patienOffsetForm.Show();
        }

        private void setProcedureProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSetProcedureProduct formProcedure = new frmSetProcedureProduct();
            formProcedure.Show();

        }

        private void newStockUnitConversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUnitConversion unitConversion = new frmUnitConversion();
            unitConversion.Show();
        }

        private void newStockUnitConversionListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUnitConversionDetailList detail = new frmUnitConversionDetailList();
            detail.Show();
        }

        private void PatientAnalysistoolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPatientAnalysisReport PTest = new frmPatientAnalysisReport();
            PTest.Show();
        }

        private void cancelLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CancelLog cancelLogForm = new CancelLog();
            cancelLogForm.Show();
        }

        private void cancelReporttoolStripMenuItem_Click(object sender, EventArgs e)
        {
            CancelReport cancelReportForm = new CancelReport();
            cancelReportForm.Show();
        }
    }
}

