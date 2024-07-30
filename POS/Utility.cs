using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using POS.APP_Data;
using System.Data;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms.DataVisualization.Charting;
using System.ServiceProcess;
using Amazon.S3.Model;

namespace POS
{
    class Utility
    {
        public static void InsertReceiptLog(ReceiptDataLog receiptData)
        {           
            POSEntities entity = new POSEntities();
            receiptData.InvoiceName=receiptData.InvoiceName.Replace(@"\Reports\", "");
            entity.ReceiptDataLogs.Add(receiptData);
            entity.SaveChanges();
        }

        #region Reprint Receipt
        public static void ReprintReceipt(string ReceiptId)
        {
            if (string.IsNullOrEmpty(ReceiptId))
            {
                return;
            }
            POSEntities entity = new POSEntities();
            ReceiptDataLog receiptData = entity.ReceiptDataLogs.Where(x => x.TransactionId == ReceiptId).FirstOrDefault();

            if (receiptData == null)
            {
                MessageBox.Show("You can't reprint this transaction.");
                return;
            }
            
            ReportViewer rv = new ReportViewer();
            string defaultPrinter= Utility.GetDefaultPrinter();
            string reportPath = Application.StartupPath + "\\Reports\\" + receiptData.InvoiceName;
            rv.Reset();
            // MessageBox.Show(insertedTransaction.Id.ToString()) ;
            rv.LocalReport.ReportPath = reportPath;

            Utility.Slip_Log(rv);
            Utility.Slip_A4_Footer(rv);

            if (receiptData.Note == "")
            {
                ReportParameter Notes = new ReportParameter("Notes", " ");
                rv.LocalReport.SetParameters(Notes);

            }
            else
            {
                ReportParameter Notes = new ReportParameter("Notes", receiptData.Note);
                rv.LocalReport.SetParameters(Notes);
            }

            ReportParameter CustomerName = new ReportParameter("CustomerName", receiptData.PatientName);
            rv.LocalReport.SetParameters(CustomerName);


            
            ReportParameter ShopName = new ReportParameter("ShopName", receiptData.ShopName);
            rv.LocalReport.SetParameters(ShopName);

            ReportParameter BranchName = new ReportParameter("BranchName", receiptData.BranchName);
            rv.LocalReport.SetParameters(BranchName);

            ReportParameter Phone = new ReportParameter("Phone", SettingController.PhoneNo);
            rv.LocalReport.SetParameters(Phone);


            ReportParameter OpeningHours = new ReportParameter("OpeningHours", SettingController.OpeningHours);
            rv.LocalReport.SetParameters(OpeningHours);

            ReportParameter TransactionId = new ReportParameter("TransactionId", receiptData.TransactionId);
            rv.LocalReport.SetParameters(TransactionId);



            ReportParameter CounterName = new ReportParameter("CounterName", receiptData.CounterName);
            rv.LocalReport.SetParameters(CounterName);


            ReportParameter InvoiceDate = new ReportParameter("InvoiceDate", receiptData.InvoiceDate);
            rv.LocalReport.SetParameters(InvoiceDate);


            ReportParameter CasherName = new ReportParameter("CasherName", receiptData.CashierName);
            rv.LocalReport.SetParameters(CasherName);

            ReportParameter TotalAmount = new ReportParameter("TotalAmount", receiptData.Total);
            rv.LocalReport.SetParameters(TotalAmount);
            
            ReportParameter PaidAmount = new ReportParameter("PaidAmount", receiptData.PaidAmount);
            rv.LocalReport.SetParameters(PaidAmount);

            ReportParameter Balance = new ReportParameter("Balance", receiptData.Balance);
            rv.LocalReport.SetParameters(Balance);
            if (!string.IsNullOrEmpty(receiptData.Change))
            {
                ReportParameter Change = new ReportParameter("Change", receiptData.Change);
                rv.LocalReport.SetParameters(Change);
            }
            if (defaultPrinter == "A4 Printer")
            {
                ReportParameter CusAddress = new ReportParameter("CusAddress", receiptData.Address);
                rv.LocalReport.SetParameters(CusAddress);
                //add a parameter for email
                ReportParameter Email = new ReportParameter("Email", SettingController.Email);
                rv.LocalReport.SetParameters(Email);

                //add a parameter for website
                ReportParameter Website = new ReportParameter("Website", SettingController.Website);
                rv.LocalReport.SetParameters(Website);

                ReportParameter PhoneImagePath = new ReportParameter("PhoneImagePath", "file:\\" + Application.StartupPath + "\\Images\\phone.png");
                rv.LocalReport.SetParameters(PhoneImagePath);

                ReportParameter WebsiteImagePath = new ReportParameter("WebsiteImagePath", "file:\\" + Application.StartupPath + "\\Images\\website.png");
                rv.LocalReport.SetParameters(WebsiteImagePath);

                ReportParameter EmailImagePath = new ReportParameter("EmailImagePath", "file:\\" + Application.StartupPath + "\\Images\\mail.png");
                rv.LocalReport.SetParameters(EmailImagePath);

                ReportParameter LocationImagePath = new ReportParameter("LocationImagePath", "file:\\" + Application.StartupPath + "\\Images\\location.png");
                rv.LocalReport.SetParameters(LocationImagePath);

            }
            ReportParameter CusPhoneNumber = new ReportParameter("CusPhoneNumber", receiptData.PatientPhoneNo);
            rv.LocalReport.SetParameters(CusPhoneNumber);


            if (receiptData.InvoiceName.Contains("Credit"))
            {
                dsReportTemp dsReport = new dsReportTemp();
                dsReportTemp.ItemListDataTable dtReport = (dsReportTemp.ItemListDataTable)dsReport.Tables["ItemList"];

                foreach (TransactionDetail transaction in receiptData.Transaction.TransactionDetails)
                {
                    dsReportTemp.ItemListRow newRow = dtReport.NewItemListRow();
                    newRow.Name = transaction.Product.Name;
                    newRow.Qty = transaction.Qty.ToString();
                    newRow.DiscountPercent = transaction.DiscountRate.ToString();
                    newRow.Frequency = Convert.ToString(transaction.Product.PackageQty * transaction.Qty);
                    newRow.TotalAmount = (int)transaction.UnitPrice * (int)transaction.Qty;
                    newRow.PhotoPath = string.IsNullOrEmpty(transaction.Product.PhotoPath) ? "" : Application.StartupPath + transaction.Product.PhotoPath.Remove(0, 1);


                    if (transaction.IsFOC == true)
                    {
                        newRow.IsFOC = "FOC";
                    }

                    switch (defaultPrinter)
                    {
                        case "A4 Printer":
                            newRow.UnitPrice = transaction.UnitPrice.ToString();
                            break;
                        case "Slip Printer":
                            newRow.UnitPrice = "1@" + transaction.UnitPrice.ToString();
                            break;
                    }
                    
                    dtReport.AddItemListRow(newRow);
                }
                ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["ItemList"]);
                rv.LocalReport.DataSources.Add(rds);

                if (receiptData.LoyaltyDiscount!=null && (decimal)receiptData.LoyaltyDiscount >= 0)
                {
                    ReportParameter MCDiscountAmt = new ReportParameter("MCDiscount", "-" + receiptData.LoyaltyDiscount.ToString());
                    rv.LocalReport.SetParameters(MCDiscountAmt);
                }
                else
                {
                    ReportParameter BCDiscountAmt = new ReportParameter("MCDiscount", "0");
                    rv.LocalReport.SetParameters(BCDiscountAmt);
                    receiptData.LoyaltyDiscount = 0;
                }
                
                ReportParameter TAmt = new ReportParameter("TAmt", receiptData.Total);
                rv.LocalReport.SetParameters(TAmt);

                ReportParameter TotalGiftDiscount = new ReportParameter("TotalGiftDiscount", receiptData.TotalDiscount.ToString());
                rv.LocalReport.SetParameters(TotalGiftDiscount);

                ReportParameter TaxAmount = new ReportParameter("TaxAmount", receiptData.Tax.ToString());
                rv.LocalReport.SetParameters(TaxAmount);
                if (receiptData.TotalDiscount!=null && Convert.ToDecimal(receiptData.TotalDiscount) == 0)
                {
                    ReportParameter DiscountAmount = new ReportParameter("DiscountAmount", receiptData.TotalDiscount.ToString());
                    rv.LocalReport.SetParameters(DiscountAmount);

                }
               
                if (receiptData.AdditionalDiscount != null && Convert.ToDecimal(receiptData.AdditionalDiscount) >= 0)
                {
                    ReportParameter AdditionalDiscount = new ReportParameter("AdditionalDiscount", "-" + receiptData.AdditionalDiscount.ToString());
                    rv.LocalReport.SetParameters(AdditionalDiscount);
                    
                }
                ReportParameter PrevOutstanding = new ReportParameter("PrevOutstanding", receiptData.PreviousOutstanding.ToString());
                rv.LocalReport.SetParameters(PrevOutstanding);
                ReportParameter NetPayable = new ReportParameter("NetPayable", receiptData.Netpayable.ToString());
                rv.LocalReport.SetParameters(NetPayable);
            }
            else if (!string.IsNullOrEmpty(receiptData.PaymentType))
            {
                ReportParameter PaymentType = new ReportParameter("PaymentType", receiptData.PaymentType);
                rv.LocalReport.SetParameters(PaymentType);
            }

            Utility.Get_Print(rv);
            rv.Reset();
            MessageBox.Show("Reprinted Successfully.");

        }
        #endregion

        /// <summary>
        /// Decrypting incomming file.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFile"></param>
        public static void DecryptFile(string inputFile, string outputFile)
    {
        try
        {
            string password = @"myKey123";

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] key = UE.GetBytes(password);

            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

            RijndaelManaged RMCrypto = new RijndaelManaged();

            CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateDecryptor(key, key), CryptoStreamMode.Read);

            FileStream fsOut = new FileStream(outputFile, FileMode.Create);

            int data;
            while ((data = cs.ReadByte()) != -1)
                fsOut.WriteByte((byte)data);

            fsOut.Close();
            cs.Close();
            fsCrypt.Close();
        }
        catch
        {
            MessageBox.Show("Decryption failed!", "Error");
        }
    }

        /// <summary>
        /// Encrypting incomming file.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFile"></param>
        public static void EncryptFile(string inputFile, string outputFile)
        {
            try
            {
                string password = @"myKey123";
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                string cryptFile = outputFile;

                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateEncryptor(key, key), CryptoStreamMode.Write);

                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);


                fsIn.Close();
                cs.Close();
                fsCrypt.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Encryption failed!", "Error");
            }
        }

        /// <summary>
        /// Decrypt the input string ( Eg: EncryptString("ABC", string.Empty); )  
        /// </summary>
        public static string EncryptString(string Message, string Passphrase)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the encoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToEncrypt = UTF8.GetBytes(Message);

            // Step 5. Attempt to encrypt the string
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the encrypted string as a base64 encoded string
            return Convert.ToBase64String(Results);
        }

        public class DBService
        {
            private ServiceController sqlService;
            public bool Running = true;
            public bool AllowToStart = true;
            /// <summary>
            /// Check and Start the SQL service for sure Running.
            /// </summary>
            public DBService()
            {
                ServiceController[] svc = ServiceController.GetServices();
                AllowToStart = DatabaseControlSetting._ServerName.ToUpper().StartsWith(System.Environment.MachineName.ToUpper());
                for (int i = 0; i < svc.Count(); i++)
                {
                    if (svc[i].ServiceName.Contains("MSSQL$") || svc[i].ServiceName.Contains("MSSQLSERVER"))
                    {
                        svc[i].Refresh();
                        if ((svc[i].ServiceName.Contains("$") && svc[i].ServiceName.Substring(0, svc[i].ServiceName.IndexOf('$')) == "MSSQL") || svc[i].ServiceName == "MSSQLSERVER")
                        {
                            if (svc[i].Status != ServiceControllerStatus.Running)
                            {
                                Running = false;
                                sqlService = svc[i];
                                break;
                            }
                            else
                            {
                                Running = true;
                            }

                        }
                    }
                }
            }

            public void Run()
            {
                if (sqlService.Status == ServiceControllerStatus.Stopped)
                {
                    sqlService.Start();
                }
                else if (sqlService.Status == ServiceControllerStatus.Paused)
                {
                    sqlService.Continue();
                }
                sqlService.WaitForStatus(ServiceControllerStatus.Running);
            }


        }
        /// <summary>
        /// Decrypt the input string ( Eg: DecryptString("LoBCnf0JCg8=", string.Empty); )  
        /// </summary>
        public static string DecryptString(string Message, string Passphrase)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the decoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToDecrypt = Convert.FromBase64String(Message);

            // Step 5. Attempt to decrypt the string
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the decrypted string in UTF8 format
            return UTF8.GetString(Results);
        }

        public static string GetSystemMACID()
        {
            string systemName = System.Windows.Forms.SystemInformation.ComputerName;
            try
            {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                String sMacAddress = string.Empty;
                foreach (NetworkInterface adapter in nics)
                {
                    if (sMacAddress == String.Empty)// only return MAC Address from first card
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                    }
                }
                return Regex.Replace(sMacAddress, ".{2}", "$0-").Substring(0, 17);
            }
            catch (ManagementException e)
            {
            }
            catch (System.UnauthorizedAccessException e)
            {

            }
            return string.Empty;
        }

        public static Boolean IsRegister()
        {
            if (SettingController.IsSourcecode == true)
            {
                return true;
            }
            else
            {
                POSEntities entity = new POSEntities();
                string MacId = Utility.GetSystemMACID();
                foreach (Authorize item in entity.Authorizes.ToList())
                {
                    string currentKey = Utility.DecryptString(item.macAddress, "ABCD");
                    if (currentKey != "" && currentKey == MacId)
                    {
                        return true;
                    }
                }
                return false;
            }

        }

        //Calculate Exchange Rate
        public static decimal CalculateExchangeRate(int Id, decimal totalAmount)
        {
            POSEntities entity = new POSEntities();
            Currency currencyObj = entity.Currencies.FirstOrDefault(x => x.Id == Id);
            decimal value;
            if (currencyObj.CurrencyCode == "MMK")
            {
                value = (decimal)currencyObj.LatestExchangeRate * totalAmount;
                return value;
            }
            else if (currencyObj.CurrencyCode == "USD")
            {
                decimal num = (decimal)(((decimal)totalAmount / (decimal)currencyObj.LatestExchangeRate));
                value = Math.Ceiling(num * (decimal)Math.Pow(10, 2)) / (decimal)Math.Pow(10, 2);
                return value;
                //float remainder = (float)(((float)totalAmount % (float)currencyObj.LatestExchangeRate) );

                //long remainderD = (long)((totalAmount % (long)currencyObj.LatestExchangeRate));

                //float point = 0;
                ////while (remainder != 0)
                ////{
                //    point += (float)((remainder / 100) / 10);
                ////    remainder = (float)((remainder / 100)%10);
                ////}
                //num = (float)(String.Format("{0:0.00}", 123.4567));

                //value = (float)((totalAmount / currencyObj.LatestExchangeRate)) + (float)((((totalAmount % currencyObj.LatestExchangeRate) / 100) / 10));
                //return value;
            }
            return 0;
        }

        //Save TransactionId and current exchange rate
        public static void AddExchangeRateForTransaction(int currrencyId, string transId)
        {
            POSEntities entity = new POSEntities();
            Currency currencyObj = entity.Currencies.FirstOrDefault(x => x.Id == currrencyId);
            ExchangeRateForTransaction ExRTObj = new ExchangeRateForTransaction();
            ExRTObj.CurrencyId = currrencyId;
            ExRTObj.TransactionId = transId;
            ExRTObj.ExchangeRate = Convert.ToInt32(currencyObj.LatestExchangeRate);
            entity.ExchangeRateForTransactions.Add(ExRTObj);
            entity.SaveChanges();
        }

        //public static string FillZero(int id)
        //{
        //    string result = string.Empty;
        //    if (id < 10) result = "00000" + id;
        //    else if (id < 100) result = "0000" + id;
        //    else if (id < 1000) result = "000" + id;
        //    else if (id < 10000) result = "00" + id;
        //    else if (id < 100000) result = "0" + id;
        //    else if (id < 1000000) result = id.ToString();

        //    return result;
        //}

        public static void UpdateProductCode(string s, long p)
        {
            SqlParameter[] paras = new SqlParameter[2];
            paras[0] = new SqlParameter("@ProductCode", s);
            paras[1] = new SqlParameter("@interestAmount", p);
        }


        #region Control Event
        //brand
        public static Boolean Brand_Combo_Control(ComboBox cboBrand)
        {
            bool _condition = false;
            POSEntities entity = new POSEntities();
            string _brandName = cboBrand.Text;

            if (_brandName != "Select" && _brandName != string.Empty)
            {
                var brand = (from c in entity.Brands where c.Name == _brandName select c).ToList();

                if (brand.Count <= 0)
                {
                    MessageBox.Show("Brand Name '" + cboBrand.Text + "' haven't registered yet!", "mPOS");
                    cboBrand.Focus();
                    _condition = true;
                }
            }
            return _condition;
        }

        //main category
        public static Boolean MainCategory_Combo_Control(ComboBox cboCategory)
        {
            bool _condition = false;
            POSEntities entity = new POSEntities();
            string _cagName = cboCategory.Text;

            if (_cagName != "Select" && _cagName != string.Empty)
            {
                var cag = (from c in entity.ProductCategories where c.Name == _cagName select c).ToList();

                if (cag.Count <= 0)
                {
                    MessageBox.Show("Category '" + cboCategory.Text + "' haven't registered yet!", "mPOS");
                    cboCategory.Focus();
                    _condition = true;
                }
            }
            return _condition;
        }


        //sub category
        public static Boolean SubCategory_Combo_Control(ComboBox cboSubCategory)
        {
            bool _condition = false;
            POSEntities entity = new POSEntities();
            string _subCagName = cboSubCategory.Text;

            if (_subCagName != "None" && _subCagName != "Select" && _subCagName != string.Empty)
            {
                var subCag = (from c in entity.ProductSubCategories where c.Name == _subCagName select c).ToList();

                if (subCag.Count <= 0)
                {
                    MessageBox.Show("Sub Category '" + cboSubCategory.Text + "' haven't registered yet!", "mPOS");
                    cboSubCategory.Focus();
                    _condition = true;
                }
            }
            return _condition;
        }

        //////tax
        ////public static Boolean Tax_Combo_Control(ComboBox cboTax)
        ////{
        ////    bool _condition = false;
        ////    POSEntities entity = new POSEntities();
        ////    string _tax = cboTax.Text;

        ////    if (_tax != "Select" && _tax != string.Empty)
        ////    {

        ////        var tax = (from c in entity.Taxes where c.Name == _tax  select c).ToList();

        ////        if (tax.Count <= 0)
        ////        {
        ////            MessageBox.Show("Tax '" + cboTax.Text + "' haven't registered yet!", "mPOS");
        ////            cboTax.Focus();
        ////            _condition = true;
        ////        }
        ////    }
        ////    return _condition;
        //// }

        //unit
        public static Boolean Unit_Combo_Control(ComboBox cboUnit)
        {
            bool _condition = false;
            POSEntities entity = new POSEntities();
            string _unit = cboUnit.Text;

            if (_unit != "Select" && _unit != string.Empty)
            {
                var tax = (from c in entity.Units where c.UnitName == _unit select c).ToList();

                if (tax.Count <= 0)
                {
                    MessageBox.Show("Unit '" + cboUnit.Text + "' haven't registered yet!", "mPOS");
                    cboUnit.Focus();
                    _condition = true;
                }
            }
            return _condition;
        }

        //product
        public static Boolean Product_Combo_Control(ComboBox cboProduct)
        {
            bool _condition = false;
            POSEntities entity = new POSEntities();
            string _productName = cboProduct.Text;

            if (_productName != "Select" && _productName != string.Empty)
            {
                var pro = (from c in entity.Products where c.Name == _productName select c).ToList();

                if (pro.Count <= 0)
                {
                    MessageBox.Show("Product Name '" + cboProduct.Text + "' haven't registered yet!", "mPOS");
                    cboProduct.Focus();
                    _condition = true;
                }
            }
            return _condition;
        }


        //supplier
        public static Boolean Supplier_Combo_Control(ComboBox cboSupplier)
        {
            bool _condition = false;
            POSEntities entity = new POSEntities();
            string _supName = cboSupplier.Text;

            if (_supName != string.Empty && _supName.Trim() != "Select")
            {
                var sup = (from c in entity.Suppliers where c.Name == _supName select c).ToList();

                if (sup.Count <= 0)
                {
                    MessageBox.Show("Supplier Name '" + cboSupplier.Text + "' haven't registered yet!", "mPOS");
                    cboSupplier.Focus();
                    _condition = true;
                }
            }
            return _condition;
        }

        //customer
        public static Boolean Customer_Combo_Control(ComboBox cboCustomer)
        {
            bool _condition = false;
            POSEntities entity = new POSEntities();
            string _cusName = cboCustomer.Text;

            if (_cusName != string.Empty && _cusName.Trim() != "Select" && !SettingController.TicketSale)
            {
                if (_cusName != "None")
                {
                    var cus = (from c in entity.Customers where c.Name == _cusName select c).ToList();

                    if (cus.Count <= 0)
                    {
                        MessageBox.Show("Customer Name '" + cboCustomer.Text + "' haven't registered yet!", "mPOS");
                        cboCustomer.Focus();
                        _condition = true;
                    }

                }
            }
            return _condition;
        }
        #endregion

        #region Combo Bind Event
        //Customer
        public static void BindCustomer(ComboBox cboCustomer)
        {
            POSEntities entity = new POSEntities();
            List<Customer> customerList = new List<Customer>();
            Customer customerObj = new Customer();
            customerObj.Id = 0;
            customerObj.Name = "Select All";
            customerList.Add(customerObj);
            customerList.AddRange(entity.Customers.ToList());
            cboCustomer.DataSource = customerList;
            cboCustomer.DisplayMember = "Name";
            cboCustomer.ValueMember = "Id";
            cboCustomer.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboCustomer.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        //Supplier
        public static void BindSupplier(ComboBox cboSupplier)
        {
            POSEntities entity = new POSEntities();
            List<Supplier> supplierList = new List<Supplier>();
            Supplier supplierObj = new Supplier();
            supplierObj.Id = 0;
            supplierObj.Name = "Select All";
            supplierList.Add(supplierObj);
            supplierList.AddRange(entity.Suppliers.ToList());
            cboSupplier.DataSource = supplierList;
            cboSupplier.DisplayMember = "Name";
            cboSupplier.ValueMember = "Id";
            cboSupplier.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboSupplier.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        //Product
        public static void BindProduct(ComboBox cboProduct)
        {
            POSEntities entity = new POSEntities();
            List<Product> productList = new List<Product>();
            Product productObj = new Product();
            productObj.Id = 0;
            productObj.Name = "Select All";
            productList.Add(productObj);
            productList.AddRange(entity.Products.ToList());
            cboProduct.DataSource = productList;
            cboProduct.DisplayMember = "Name";
            cboProduct.ValueMember = "Id";
            cboProduct.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboProduct.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        //Conversion Product
        public static void BindConversionProduct(ComboBox cboProduct, string unitType)
        {
            POSEntities entity = new POSEntities();
            List<Product> productList = new List<Product>();
            Product productObj = new Product();
            productObj.Id = 0;
            productObj.Name = "ALL";
            productList.Add(productObj);
            productList.AddRange(entity.Products.Where(x => x.UnitType == unitType && (x.IsService == false || x.IsService == null) && x.IsPackage == false).ToList());
            cboProduct.DataSource = productList;
            cboProduct.DisplayMember = "Name";
            cboProduct.ValueMember = "Id";
            cboProduct.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboProduct.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        //Bind Reference Product
        public static void BindReferenceProduct(ComboBox cboProduct)
        {
            POSEntities entity = new POSEntities();
            List<Product> productList = new List<Product>();
            Product productObj = new Product();
            productObj.Id = 0;
            productObj.Name = "Select";
            productList.Add(productObj);
            productList.AddRange(entity.Products.ToList());
            cboProduct.DataSource = productList;
            cboProduct.DisplayMember = "Name";
            cboProduct.ValueMember = "Id";
            cboProduct.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboProduct.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        //Cosignor
        public static void BindConsignor(ComboBox cboConsignor, Boolean IsAll = true)
        {
            POSEntities entity = new POSEntities();
            List<APP_Data.ConsignmentCounter> consignList = new List<APP_Data.ConsignmentCounter>();

            if (IsAll)
            {
                APP_Data.ConsignmentCounter conObj = new APP_Data.ConsignmentCounter();
                conObj.Id = 0;
                conObj.Name = "ALL";
                consignList.Add(conObj);
            }
            consignList.AddRange((from clist in entity.ConsignmentCounters select clist).ToList());
            cboConsignor.DataSource = consignList;
            cboConsignor.DisplayMember = "Name";
            cboConsignor.ValueMember = "Id";
            cboConsignor.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboConsignor.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        //Adjustment Type
        public static void Bind_AdjustmentType(ComboBox cboAdjType)
        {
            //  List<AdjustmentType> adjList = new List<AdjustmentType>();

            POSEntities entity = new POSEntities();
            List<APP_Data.AdjustmentType> adjList = new List<APP_Data.AdjustmentType>();
            //var adjList = (from a in entity.AdjustmentTypes select a).ToList();
            AdjustmentType adjusObj = new AdjustmentType();
            adjusObj.Id = 0;
            adjusObj.Name = "Select";
            adjList.Add(adjusObj);

            adjList.AddRange((from ajList in entity.AdjustmentTypes select ajList).ToList());
            cboAdjType.DataSource = adjList;
            cboAdjType.DisplayMember = "Name";
            cboAdjType.ValueMember = "Id";
            cboAdjType.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboAdjType.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        //Brand
        public static void BindBrand(ComboBox cboBrand)
        {
            POSEntities entity = new POSEntities();
            List<APP_Data.Brand> brandList = new List<APP_Data.Brand>();

            APP_Data.Brand brandObj = new APP_Data.Brand();
            brandObj.Id = 0;
            brandObj.Name = "Select";
            brandList.Add(brandObj);

            brandList.AddRange((from _brand in entity.Brands select _brand).ToList());
            cboBrand.DataSource = brandList;
            cboBrand.DisplayMember = "Name";
            cboBrand.ValueMember = "Id";
            cboBrand.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboBrand.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        //ExpenseCategory
        public static void BindExpenseCategory(ComboBox cboExpense)
        {
            POSEntities entity = new POSEntities();
            List<APP_Data.ExpenseCategory> expenseList = new List<APP_Data.ExpenseCategory>();
            APP_Data.ExpenseCategory expenseObj = new APP_Data.ExpenseCategory();
            expenseObj.Id = 0;
            expenseObj.Name = "All";
            expenseList.Add(expenseObj);
            expenseList.AddRange(entity.ExpenseCategories.ToList());
            cboExpense.DataSource = expenseList;
            cboExpense.DisplayMember = "Name";
            cboExpense.ValueMember = "Id";
            cboExpense.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboExpense.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        public static void BindCity(ComboBox cboCity)
        {
            POSEntities entity = new POSEntities();
            List<APP_Data.City> cityList = entity.Cities.ToList();
            cboCity.DataSource = cityList;
            cboCity.DisplayMember = "CityName";
            cboCity.ValueMember = "Id";

            cboCity.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboCity.AutoCompleteSource = AutoCompleteSource.ListItems;
        }


        public static void BindShop(ComboBox cboShop, bool includeALL = false)
        {
            if (includeALL)
            {
                POSEntities entity = new POSEntities();
                List<APP_Data.Shop> shopList = new List<APP_Data.Shop>();

                APP_Data.Shop shop = new APP_Data.Shop();
                shop.ShopName = "ALL";
                shop.Id = 0;

                var shops = entity.Shops.ToList();
                shopList.Add(shop);
                shopList.AddRange(shops);
                cboShop.DataSource = shopList;
                cboShop.DisplayMember = "ShopName";
                cboShop.ValueMember = "Id";

                cboShop.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cboShop.AutoCompleteSource = AutoCompleteSource.ListItems;
            }
            else
            {
                POSEntities entity = new POSEntities();
                List<APP_Data.Shop> shopList = entity.Shops.ToList();
                cboShop.DataSource = shopList;
                cboShop.DisplayMember = "ShopName";
                cboShop.ValueMember = "Id";

                cboShop.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cboShop.AutoCompleteSource = AutoCompleteSource.ListItems;
            }

        }


        public static void BindShopALL(ComboBox cboShop)
        {
            POSEntities entity = new POSEntities();
            List<APP_Data.Shop> shopList = new List<APP_Data.Shop>();

            APP_Data.Shop shopObj = new APP_Data.Shop();
            shopObj.Id = 0;
            shopObj.ShopName = "ALL";
            shopList.Add(shopObj);
            shopList.AddRange(entity.Shops.ToList());
            cboShop.DataSource = shopList;
            cboShop.DisplayMember = "ShopName";
            cboShop.ValueMember = "Id";

            cboShop.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboShop.AutoCompleteSource = AutoCompleteSource.ListItems;
        }
        #endregion

        #region IsConsignmentPaid
        //For Consignment Product Paid or not Paid
        public static Boolean? IsConsignmentPaid(Product pro)
        {
            if (pro.IsConsignment == true)
            {
                return false;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region StockTransaction


        public static Boolean Stock_Transaction_Process(int _year, int _month, List<long> productIdList)
        {
            Boolean Success = true;

            Purchase_Process(_year, _month, productIdList);

            Refund_Process(_year, _month, productIdList);

            Sale_Process(_year, _month, productIdList);

            //Adjustment_Process(_year, _month, productIdList);
            Adjustment_Process(_year, _month, productIdList);

            Consignment_Process(_year, _month, productIdList);
            return Success;
        }
        /// <summary>
        /// Return List<PurchaseDetail>,StockList retrieved by FeFo or FiFo or LiFo
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public static List<PurchaseDetail> InventoryByControlMethod(long pId, POSEntities entity)
        {
            List<PurchaseDetail> pulist = new List<PurchaseDetail>();
            //for both normal and wrapper items 

            if (SettingController.InventoryControlPattern == "fefo")
            {
                pulist = (from p in entity.PurchaseDetails
                          join m in entity.MainPurchases on p.MainPurchaseId equals m.Id
                          where p.ProductId == pId && p.IsDeleted == false && m.IsCompletedInvoice == true && p.CurrentQy > 0
                          orderby p.expiredDate ascending
                          select p).ToList();
            }
            else
            {
                pulist = SettingController.InventoryControlPattern == "fifo" ?
                       (from p in entity.PurchaseDetails
                        join m in entity.MainPurchases on p.MainPurchaseId equals m.Id
                        where p.ProductId == pId && p.IsDeleted == false && m.IsCompletedInvoice == true && p.CurrentQy > 0
                        orderby p.Date, p.expiredDate ascending
                        select p).ToList() :
                       (from p in entity.PurchaseDetails
                        join m in entity.MainPurchases on p.MainPurchaseId equals m.Id
                        where p.ProductId == pId && p.IsDeleted == false && m.IsCompletedInvoice == true && p.CurrentQy > 0
                        orderby p.expiredDate ascending
                        orderby p.Date descending

                        select p).ToList();
            }
            return pulist;
        }
        //Purchase Process
        public static void Purchase_Process(int _year, int _month, List<long> productIdList)
        {
            POSEntities entity = new POSEntities();
            List<Stock_Transaction> _purchaseResult = Purchase_Balance(_year, _month, productIdList);

            //Purchase Save or Update
            foreach (var p in _purchaseResult)
            {
                string _tranDate = Month_Name(_month, _year);
                var _stockTranID = (from x in entity.StockTransactions
                                    where x.ProductId == p.ProductId
                                        && x.TranDate == _tranDate
                                    select x.StockTranId).FirstOrDefault();
                //  var ok = entity.StockTransactions.Where(x => x.ProductId == p.ProductId && x.TranDate == _tranDate).Select(y => y.StockTranId).FirstOrDefault();
                APP_Data.StockTransaction _stObject = new APP_Data.StockTransaction();
                if (_stockTranID == 0)
                {

                    _stObject.TranDate = _tranDate;
                    _stObject.ProductId = Convert.ToInt32(p.ProductId);
                    _stObject.Opening = Opening_Balance(_year, _month, Convert.ToInt64(p.ProductId));
                    _stObject.Purchase = p.Purchase;
                    _stObject.Refund = 0;
                    _stObject.Sale = 0;
                    _stObject.AdjustmentStockIn = 0;
                    _stObject.AdjustmentStockOut = 0;
                    _stObject.Consignment = 0;
                    _stObject.ConversionStockIn = 0;
                    _stObject.ConversionStockOut = 0;
                    _stObject.Month = _month;
                    _stObject.Year = _year;
                    entity.StockTransactions.Add(_stObject);
                    entity.SaveChanges();
                }
                else
                {

                    APP_Data.StockTransaction updateStockTransaction = (from s in entity.StockTransactions where s.StockTranId == _stockTranID select s).FirstOrDefault();
                    updateStockTransaction.Purchase = p.Purchase;
                    updateStockTransaction.Opening = Opening_Balance(_year, _month, Convert.ToInt64(p.ProductId));
                    entity.Entry(updateStockTransaction).State = EntityState.Modified;
                    entity.SaveChanges();
                }
            }

        }

        //Refund Process
        public static void Refund_Process(int _year, int _month, List<long> productIdList)
        {
            POSEntities entity = new POSEntities();
            List<Stock_Transaction> _refundResult = Refund_Balance(_year, _month, productIdList);

            //Refund Save or Update
            foreach (var r in _refundResult)
            {
                string _tranDate = Month_Name(_month, _year);
                var _stockTranID = (from x in entity.StockTransactions
                                    where x.ProductId == r.ProductId
                                        && x.TranDate == _tranDate
                                    select x.StockTranId).FirstOrDefault();


                if (_stockTranID == 0)
                {
                    APP_Data.StockTransaction _stObject = new APP_Data.StockTransaction();
                    _stObject.TranDate = _tranDate;
                    _stObject.ProductId = Convert.ToInt32(r.ProductId);
                    _stObject.Opening = Opening_Balance(_year, _month, Convert.ToInt64(r.ProductId));
                    _stObject.Purchase = 0;
                    _stObject.Refund = r.Refund;
                    _stObject.Sale = 0;
                    _stObject.AdjustmentStockIn = 0;
                    _stObject.AdjustmentStockOut = 0;
                    _stObject.Consignment = 0;
                    _stObject.ConversionStockIn = 0;
                    _stObject.ConversionStockOut = 0;
                    _stObject.Month = _month;
                    _stObject.Year = _year;
                    entity.StockTransactions.Add(_stObject);
                    entity.SaveChanges();
                }
                else
                {
                    APP_Data.StockTransaction updateStockTransaction = (from s in entity.StockTransactions where s.StockTranId == _stockTranID select s).FirstOrDefault();
                    updateStockTransaction.Refund = r.Refund;
                    updateStockTransaction.Opening = Opening_Balance(_year, _month, Convert.ToInt64(r.ProductId));
                    entity.Entry(updateStockTransaction).State = EntityState.Modified;
                    entity.SaveChanges();
                }
            }

        }

        //Sale Process
        public static void Sale_Process(int _year, int _month, List<long> productIdList)
        {
            POSEntities entity = new POSEntities();
            List<Stock_Transaction> _saleResult = Sale_Balance(_year, _month, productIdList);

            //Sale Save or Update
            foreach (var r in _saleResult)
            {
                string _tranDate = Month_Name(_month, _year);
                var _stockTranID = (from x in entity.StockTransactions
                                    where x.ProductId == r.ProductId
                                        && x.TranDate == _tranDate
                                    select x.StockTranId).FirstOrDefault();


                if (_stockTranID == 0)
                {
                    APP_Data.StockTransaction _stObject = new APP_Data.StockTransaction();
                    _stObject.TranDate = _tranDate;
                    _stObject.ProductId = Convert.ToInt32(r.ProductId);
                    _stObject.Opening = Opening_Balance(_year, _month, Convert.ToInt64(r.ProductId));
                    _stObject.Purchase = 0;
                    _stObject.Refund = 0;
                    _stObject.Sale = r.Sale;
                    _stObject.AdjustmentStockIn = 0;
                    _stObject.AdjustmentStockOut = 0;
                    _stObject.Consignment = 0;
                    _stObject.ConversionStockIn = 0;
                    _stObject.ConversionStockOut = 0;
                    _stObject.Month = _month;
                    _stObject.Year = _year;
                    entity.StockTransactions.Add(_stObject);
                    entity.SaveChanges();
                }
                else
                {
                    APP_Data.StockTransaction updateStockTransaction = (from s in entity.StockTransactions where s.StockTranId == _stockTranID select s).FirstOrDefault();
                    updateStockTransaction.Sale = r.Sale;
                    updateStockTransaction.Opening = Opening_Balance(_year, _month, Convert.ToInt64(r.ProductId));
                    entity.Entry(updateStockTransaction).State = EntityState.Modified;
                    entity.SaveChanges();
                }
            }

        }


        //Adjustment Process
        public static void Adjustment_Process(int _year, int _month, List<long> productIdList)
        {
            POSEntities entity = new POSEntities();
            List<Stock_Transaction> _adjustmentResult = Adjustment_Balance(_year, _month, productIdList);

            //Sale Save or Update
            foreach (var r in _adjustmentResult)
            {
                string _tranDate = Month_Name(_month, _year);
                var _stockTranID = (from x in entity.StockTransactions
                                    where x.ProductId == r.ProductId
                                        && x.TranDate == _tranDate
                                    select x.StockTranId).FirstOrDefault();


                if (_stockTranID == 0)
                {
                    APP_Data.StockTransaction _stObject = new APP_Data.StockTransaction();
                    _stObject.TranDate = _tranDate;
                    _stObject.ProductId = Convert.ToInt32(r.ProductId);
                    _stObject.Opening = Opening_Balance(_year, _month, Convert.ToInt64(r.ProductId));
                    _stObject.Purchase = 0;
                    _stObject.Refund = 0;
                    _stObject.Sale = 0;
                    if (r.AdjustmentQty < 0)
                    {
                        _stObject.AdjustmentStockOut = r.AdjustmentQty * -1;
                        _stObject.AdjustmentStockIn = 0;
                    }
                    else
                    {
                        _stObject.AdjustmentStockIn = r.AdjustmentQty;
                        _stObject.AdjustmentStockOut = 0;
                    }

                    _stObject.Consignment = 0;
                    _stObject.ConversionStockIn = 0;
                    _stObject.ConversionStockOut = 0;
                    _stObject.Month = _month;
                    _stObject.Year = _year;
                    entity.StockTransactions.Add(_stObject);
                    entity.SaveChanges();
                }
                else
                {
                    APP_Data.StockTransaction updateStockTransaction = (from s in entity.StockTransactions where s.StockTranId == _stockTranID select s).FirstOrDefault();
                    if (r.AdjustmentQty < 0)
                    {
                        updateStockTransaction.AdjustmentStockOut = r.AdjustmentQty * -1;
                    }
                    else
                    {
                        updateStockTransaction.AdjustmentStockIn = r.AdjustmentQty;
                    }

                    updateStockTransaction.Opening = Opening_Balance(_year, _month, Convert.ToInt64(r.ProductId));
                    entity.Entry(updateStockTransaction).State = EntityState.Modified;
                    entity.SaveChanges();
                }
            }

        }



        //Consignment Process
        public static void Consignment_Process(int _year, int _month, List<long> productIdList)
        {
            POSEntities entity = new POSEntities();
            List<Stock_Transaction> _consignmentResult = Consignment_Balance(_year, _month, productIdList);

            //Sale Save or Update
            foreach (var r in _consignmentResult)
            {
                string _tranDate = Month_Name(_month, _year);
                var _stockTranID = (from x in entity.StockTransactions
                                    where x.ProductId == r.ProductId
                                        && x.TranDate == _tranDate
                                    select x.StockTranId).FirstOrDefault();


                if (_stockTranID == 0)
                {
                    APP_Data.StockTransaction _stObject = new APP_Data.StockTransaction();
                    _stObject.TranDate = _tranDate;
                    _stObject.ProductId = Convert.ToInt32(r.ProductId);
                    _stObject.Opening = Opening_Balance(_year, _month, Convert.ToInt64(r.ProductId));
                    _stObject.Purchase = 0;
                    _stObject.Refund = 0;
                    _stObject.Sale = 0;
                    _stObject.AdjustmentStockIn = 0;
                    _stObject.AdjustmentStockOut = 0;
                    _stObject.Consignment = Convert.ToInt32(r.Consignment);
                    _stObject.ConversionStockIn = 0;
                    _stObject.ConversionStockOut = 0;
                    _stObject.Month = _month;
                    _stObject.Year = _year;
                    entity.StockTransactions.Add(_stObject);
                    entity.SaveChanges();
                }
                else
                {
                    APP_Data.StockTransaction updateStockTransaction = (from s in entity.StockTransactions where s.StockTranId == _stockTranID select s).FirstOrDefault();
                    updateStockTransaction.Consignment = Convert.ToInt32(r.Consignment);
                    updateStockTransaction.Opening = Opening_Balance(_year, _month, Convert.ToInt64(r.ProductId));
                    entity.Entry(updateStockTransaction).State = EntityState.Modified;
                    entity.SaveChanges();
                }
            }

        }

        //Opening_Balance
        public static int Opening_Balance(int _year, int _month, long productId)
        {
            POSEntities entity = new POSEntities();
            int _openingBalance = 0;
            var _companyStartDate = Convert.ToDateTime(SettingController.Company_StartDate);

            //if (_year >= _companyStartDate.Year && _month > _companyStartDate.Month)
            //{
            int _preMonth = _month - 1;

            string tranDate = Month_Name(_preMonth, _year);


            List<long> productIdList = new List<long> { productId };

            //Purchase Balance
            List<Stock_Transaction> _purResult = Purchase_Balance(_year, _preMonth, productIdList);

            int _purchaseBalance = 0;
            if (_purResult.Count > 0)
            {
                _purchaseBalance = Convert.ToInt32(_purResult.Sum(x => x.Purchase));
            }

            //Refund Balance
            List<Stock_Transaction> _refundResult = Refund_Balance(_year, _preMonth, productIdList);

            int _refundBalance = 0;
            if (_refundResult.Count > 0)
            {
                _refundBalance = Convert.ToInt32(_refundResult.Sum(x => x.Refund));
            }


            //Sale Balance
            List<Stock_Transaction> _saleResult = Sale_Balance(_year, _preMonth, productIdList);

            int _saleBalance = 0;
            if (_saleResult.Count > 0)
            {
                _saleBalance = Convert.ToInt32(_saleResult.Sum(x => x.Sale));
            }


            //Adjustment Balance
            //List<Stock_Transaction> _AdjustmentResult = Adjustment_Balance(_year, _preMonth, productIdList);

            //int _AdjustmentBalance = 0;
            //if (_AdjustmentResult.Count > 0)
            //{
            //    _AdjustmentBalance = Convert.ToInt32(_AdjustmentResult.Sum(x => x.Adjustment));
            //}

            //Adjustment Balance
            List<Stock_Transaction> _adjustmentResult = Adjustment_Balance(_year, _preMonth, productIdList);

            int _AdjustmentBalance = 0;
            if (_adjustmentResult.Count > 0)
            {
                _AdjustmentBalance = Convert.ToInt32(_adjustmentResult.Sum(x => x.AdjustmentQty));
            }


            //Consignment Balance
            List<Stock_Transaction> _consignmentResult = Consignment_Balance(_year, _preMonth, productIdList);

            int _consignBalance = 0;
            if (_consignmentResult.Count > 0)
            {
                _consignBalance = Convert.ToInt32(_consignmentResult.Sum(x => x.Consignment));
            }

            //Opening Balance
            int? _preOpenBalance = 0;
            _preOpenBalance = (from o in entity.StockTransactions where o.TranDate == tranDate && o.ProductId == productId select o.Opening).FirstOrDefault();

            _preOpenBalance = _preOpenBalance == null ? 0 : _preOpenBalance;

            _openingBalance = Convert.ToInt32(_preOpenBalance + (_purchaseBalance + _refundBalance + _consignBalance) - (_saleBalance + _AdjustmentBalance));

            // }

            ////if (_openingBalance == 0)
            ////{
            ////    if (_year > _companyStartDate.Year)
            ////    {
            ////        var stockTranList = (from o in entity.StockTransactions where o.ProductId == productId select o).OrderByDescending(x => x.StockTranId).FirstOrDefault();

            ////        if (stockTranList != null)
            ////        {

            ////            _openingBalance = Convert.ToInt32(stockTranList.Opening + (stockTranList.Purchase + stockTranList.Refund + stockTranList.Consignment + stockTranList.AdjustmentStockIn) - (stockTranList.Sale + stockTranList.AdjustmentStockOut));
            ////        }
            ////    }
            ////    else if (_year == _companyStartDate.Year && _month > _companyStartDate.Month)
            ////    {
            ////       // var stockTranList = (from o in entity.StockTransactions where o.ProductId == productId select o).OrderByDescending(x => x.StockTranId).FirstOrDefault();
            ////        var stockTranList = (from o in entity.StockTransactions where o.ProductId == productId && ((o.Year < _year && o.Year <= _year) || (o.Year == _year) && (o.Year <= _year && o.Month < _month)) select o).OrderBy(x => x.Year).OrderByDescending(x => x.Month).FirstOrDefault();

            ////        if (stockTranList != null)
            ////        {

            ////            _openingBalance = Convert.ToInt32(stockTranList.Opening + (stockTranList.Purchase + stockTranList.Refund + stockTranList.Consignment + stockTranList.AdjustmentStockIn) - (stockTranList.Sale + stockTranList.AdjustmentStockOut));
            ////        }
            ////    }
            ////}


            var stockTranList = (from o in entity.StockTransactions where o.ProductId == productId && ((o.Year < _year && o.Year <= _year) || (o.Year == _year) && (o.Year <= _year && o.Month < _month)) select o).OrderBy(x => x.Year).OrderByDescending(x => x.Month).FirstOrDefault();

            if (stockTranList != null)
            {

                _openingBalance = Convert.ToInt32(stockTranList.Opening + (stockTranList.Purchase + stockTranList.Refund + stockTranList.Consignment + stockTranList.AdjustmentStockIn) - (stockTranList.Sale + stockTranList.AdjustmentStockOut));
            }
            return _openingBalance;
        }

        //check exist or not exist productid in stockTransaction table
        public static List<APP_Data.StockTransaction> Check_StockTransaction(List<long> productIdList, string tranDate)
        {
            POSEntities entity = new POSEntities();

            List<APP_Data.StockTransaction> _dataResult = (from e in entity.StockTransactions
                                                           where e.TranDate == tranDate
                                                           && productIdList.Contains(e.ProductId)

                                                           select e).ToList();
            return _dataResult;

        }

        public static string Month_Name(int _month, int _year)
        {
            string _tranDate = "";
            switch (_month)
            {
                case 1:
                    _tranDate = "Janauary" + "-" + _year.ToString();
                    break;
                case 2:
                    _tranDate = "February" + "-" + _year.ToString();
                    break;
                case 3:
                    _tranDate = "March" + "-" + _year.ToString();
                    break;
                case 4:
                    _tranDate = "April" + "-" + _year.ToString();
                    break;
                case 5:
                    _tranDate = "May" + "-" + _year.ToString();
                    break;
                case 6:
                    _tranDate = "June" + "-" + _year.ToString();
                    break;
                case 7:
                    _tranDate = "July" + "-" + _year.ToString();
                    break;
                case 8:
                    _tranDate = "August" + "-" + _year.ToString();
                    break;
                case 9:
                    _tranDate = "September" + "-" + _year.ToString();
                    break;
                case 10:
                    _tranDate = "October" + "-" + _year.ToString();
                    break;
                case 11:
                    _tranDate = "November" + "-" + _year.ToString();
                    break;
                case 12:
                    _tranDate = "December" + "-" + _year.ToString();
                    break;
            }
            return _tranDate;
        }

        //Purchase (Get ProductId and Purchase Qty List)
        public static List<Stock_Transaction> Purchase_Balance(int _year, int _month, List<long> productIdList)
        {
            POSEntities entity = new POSEntities();
            List<Stock_Transaction> _purchaseResult = new List<Stock_Transaction>();
            var _purchaseList = (from p in entity.PurchaseDetails
                                 join pd in entity.Products on p.ProductId equals pd.Id
                                 join mp in entity.MainPurchases on p.MainPurchaseId equals mp.Id
                                 where p.IsDeleted == false && mp.IsPurchase == true
                                 && productIdList.Contains(pd.Id)
                                    && mp.Date.Value.Month == _month && mp.Date.Value.Year == _year
                                 select new Stock_Transaction
                                 {
                                     ProductId = p.ProductId,
                                     Purchase = p.Qty
                                 }
                                 ).ToList();

            if (_purchaseList.Count > 0)
            {

                _purchaseResult = _purchaseList.GroupBy(x => new { x.ProductId })
                                  .Select(y => new Stock_Transaction()
                                  {


                                      ProductId = y.Key.ProductId,
                                      Purchase = y.Sum(x => x.Purchase)

                                  }).ToList();

            }
            return _purchaseResult;

        }

        //Consignment (Get ProductId and Purchase Consignment Qty List)
        public static List<Stock_Transaction> Consignment_Balance(int _year, int _month, List<long> productIdList)
        {
            POSEntities entity = new POSEntities();
            List<Stock_Transaction> _consignmentResult = new List<Stock_Transaction>();
            var _consignmentList = (from p in entity.ProductQuantityChanges
                                    join pd in entity.Products on p.ProductId equals pd.Id
                                    where
                                      productIdList.Contains(pd.Id)
                                      && p.UpdateDate.Value.Month == _month && p.UpdateDate.Value.Year == _year
                                    select new Stock_Transaction
                                    {
                                        ProductId = p.ProductId,
                                        Consignment = p.StockInQty
                                    }
                                   ).ToList();

            if (_consignmentList.Count > 0)
            {
                _consignmentResult = _consignmentList.GroupBy(x => new { x.ProductId })
                                   .Select(y => new Stock_Transaction()
                                   {


                                       ProductId = y.Key.ProductId,
                                       Consignment = y.Sum(x => x.Consignment)

                                   }).ToList();

            }
            return _consignmentResult;

        }

        //Refund (Get ProductId and Refund Qty List)
        public static List<Stock_Transaction> Refund_Balance(int _year, int _month, List<long> productIdList)
        {
            POSEntities entity = new POSEntities();
            List<string> type = new List<string> { "Refund", "CreditRefund" };
            List<Stock_Transaction> _refundResult = new List<Stock_Transaction>();
            var _refundList = (from td in entity.TransactionDetails
                               join pd in entity.Products on td.ProductId equals pd.Id
                               join t in entity.Transactions on td.TransactionId equals t.Id
                               where t.IsDeleted == false
                               && type.Contains(t.Type)
                               && productIdList.Contains(pd.Id)
                                 && t.DateTime.Value.Month == _month && t.DateTime.Value.Year == _year
                               select new Stock_Transaction
                               {
                                   ProductId = td.ProductId,
                                   Refund = td.Qty
                               }
                                   ).ToList();

            if (_refundList.Count > 0)
            {
                _refundResult = _refundList.GroupBy(x => new { x.ProductId })
                                .Select(y => new Stock_Transaction()
                                {


                                    ProductId = y.Key.ProductId,
                                    Refund = y.Sum(x => x.Refund)

                                }).ToList();


            }
            return _refundResult;
        }

        //Sale (Get ProductId and Sale Qty List)
        public static List<Stock_Transaction> Sale_Balance(int _year, int _month, List<long> productIdList)
        {
            POSEntities entity = new POSEntities();
            List<string> _type = new List<string> { "Sale", "Credit" };
            List<Stock_Transaction> _saleResult = new List<Stock_Transaction>();
            var _saleList = (from tds in entity.TransactionDetails
                             join pd in entity.Products on tds.ProductId equals pd.Id
                             join t in entity.Transactions on tds.TransactionId equals t.Id
                             where tds.IsDeleted == false
                             && t.IsComplete == true
                               && productIdList.Contains(pd.Id)
                             && _type.Contains(t.Type)
                                && t.DateTime.Value.Month == _month && t.DateTime.Value.Year == _year
                                && t.DateTime.Value.Month == _month && t.DateTime.Value.Year == _year
                             select new Stock_Transaction
                             {
                                 ProductId = tds.ProductId,
                                 Sale = tds.Qty
                             }
                                   ).ToList();

            if (_saleList.Count > 0)
            {
                _saleResult = _saleList.GroupBy(x => new { x.ProductId })
                                .Select(y => new Stock_Transaction()
                                {


                                    ProductId = y.Key.ProductId,
                                    Sale = y.Sum(x => x.Sale)

                                }).ToList();

            }
            return _saleResult;
        }


        #region Adjustment (Get ProductId and Adjustment Qty List)
        //public static List<Stock_Transaction> Adjustment_Balance(int _year, int _month, List<long> productIdList)
        //{
        //    POSEntities entity = new POSEntities();
        //    List<string> _type = new List<string> { "Sale", "Credit" };
        //    List<Stock_Transaction> _AdjustmentResult = new List<Stock_Transaction>();
        //    var _AdjustmentList = (from d in entity.Adjustments
        //                       join pd in entity.Products on d.ProductId equals pd.Id
        //                       where d.IsDeleted == false
        //                         && productIdList.Contains(pd.Id)
        //                        && d.AdjustmentDateTime.Value.Month == _month && d.AdjustmentDateTime.Value.Year == _year
        //                       select new Stock_Transaction
        //                       {
        //                           ProductId = d.ProductId,
        //                           Adjustment = d.AdjustmentQty
        //                       }
        //                           ).ToList();

        //    if (_AdjustmentList.Count > 0)
        //    {
        //        _AdjustmentResult = _AdjustmentList.GroupBy(x => new { x.ProductId })
        //                          .Select(y => new Stock_Transaction()
        //                          {


        //                              ProductId = y.Key.ProductId,
        //                              Adjustment = y.Sum(x => x.Adjustment)

        //                          }).ToList();

        //    }
        //    return _AdjustmentResult;
        //}
        #endregion

        #region Adjustment (Get ProductId and Adjustment Qty List)
        public static List<Stock_Transaction> Adjustment_Balance(int _year, int _month, List<long> productIdList)
        {
            POSEntities entity = new POSEntities();
            List<string> _type = new List<string> { "Sale", "Credit" };
            List<Stock_Transaction> _adjustmentResult = new List<Stock_Transaction>();
            var _adjustmentList = (from d in entity.Adjustments
                                   join pd in entity.Products on d.ProductId equals pd.Id
                                   where d.IsDeleted == false
                                     && productIdList.Contains(pd.Id)
                                    && d.AdjustmentDateTime.Value.Month == _month && d.AdjustmentDateTime.Value.Year == _year
                                   select new Stock_Transaction
                                   {
                                       ProductId = d.ProductId,
                                       AdjustmentQty = d.AdjustmentQty
                                   }
                                   ).ToList();

            if (_adjustmentList.Count > 0)
            {
                _adjustmentResult = _adjustmentList.GroupBy(x => new { x.ProductId })
                                  .Select(y => new Stock_Transaction()
                                  {


                                      ProductId = y.Key.ProductId,
                                      AdjustmentQty = y.Sum(x => x.AdjustmentQty)

                                  }).ToList();

            }
            return _adjustmentResult;
        }
        #endregion


        #region Stock Transaction for Backend run process

        //Opening_Balance
        public static int Opening_Run_Balance(int _year, int _month, long productId)
        {
            POSEntities entity = new POSEntities();
            int _openingBalance = 0;

            int _preMonth = _month - 1;

            string tranDate = Month_Name(_preMonth, _year);

            var stockTranList = (from o in entity.StockTransactions where o.ProductId == productId && ((o.Year < _year && o.Year <= _year) || (o.Year == _year) && (o.Year <= _year && o.Month < _month)) select o).OrderBy(x => x.Year).OrderByDescending(x => x.Month).FirstOrDefault();


            if (stockTranList != null)
            {

                _openingBalance = Convert.ToInt32(stockTranList.Opening + (stockTranList.Purchase + stockTranList.Refund + stockTranList.Consignment + stockTranList.AdjustmentStockIn + stockTranList.ConversionStockIn + stockTranList.StockIn) - (stockTranList.Sale + stockTranList.AdjustmentStockOut + stockTranList.ConversionStockOut + stockTranList.StockOut));
            }

            return _openingBalance;
        }

        //update Opening Blance if delete in transaction process
        public static void Update_Opening_BalanceFor_NextMonth(int _month, int _year, int _productId, int _Qty)
        {
            POSEntities entity = new POSEntities();
            if (_month == 12)
            {
                _year = _year + 1;
                _month = 1;
            }
            else
            {
                _month = _month + 1;
            }
            var _stockTranIDList = (from x in entity.StockTransactions
                                    where x.ProductId == _productId
                                        && x.Year >= _year && x.Month >= _month
                                    select x.StockTranId).ToList();
            if (_stockTranIDList.Count > 0)
            {
                foreach (var stocktranId in _stockTranIDList)
                {
                    APP_Data.StockTransaction stockTransaction = (from s in entity.StockTransactions where s.StockTranId == stocktranId select s).FirstOrDefault();
                    stockTransaction.Opening = stockTransaction.Opening + _Qty;
                    entity.Entry(stockTransaction).State = EntityState.Modified;
                    entity.SaveChanges();
                }
            }

        }

        //Purchase Process
        public static void Purchase_Run_Process(int _year, int _month, List<Stock_Transaction> productList)
        {
            POSEntities entity = new POSEntities();

            //Purchase Save or Update
            foreach (var pro in productList)
            {
                string _tranDate = Month_Name(_month, _year);
                var _stockTranID = (from x in entity.StockTransactions
                                    where x.ProductId == pro.ProductId
                                        && x.TranDate == _tranDate
                                    select x.StockTranId).FirstOrDefault();

                APP_Data.StockTransaction _stObject = new APP_Data.StockTransaction();
                if (_stockTranID == 0)
                {

                    _stObject.TranDate = _tranDate;
                    _stObject.ProductId = Convert.ToInt32(pro.ProductId);
                    _stObject.Opening = Opening_Run_Balance(_year, _month, Convert.ToInt64(pro.ProductId));
                    _stObject.Purchase = pro.Purchase;
                    _stObject.Refund = 0;
                    _stObject.Sale = 0;
                    _stObject.AdjustmentStockIn = 0;
                    _stObject.AdjustmentStockOut = 0;
                    _stObject.Consignment = 0;
                    _stObject.ConversionStockIn = 0;
                    _stObject.ConversionStockOut = 0;
                    _stObject.StockIn = 0;
                    _stObject.StockOut = 0;
                    _stObject.Month = _month;
                    _stObject.Year = _year;
                    entity.StockTransactions.Add(_stObject);
                    entity.SaveChanges();



                }
                else
                {

                    APP_Data.StockTransaction updateStockTransaction = (from s in entity.StockTransactions where s.StockTranId == _stockTranID select s).FirstOrDefault();
                    updateStockTransaction.Purchase = updateStockTransaction.Purchase + pro.Purchase;
                    //if (updateStockTransaction.Opening > 0)
                    //{
                    //    updateStockTransaction.Opening = Opening_Run_Balance(_year, _month, Convert.ToInt64(pro.ProductId)) + pro.Purchase;
                    //}

                    entity.Entry(updateStockTransaction).State = EntityState.Modified;
                    entity.SaveChanges();

                }

                Update_Opening_BalanceFor_NextMonth(_month, _year, Convert.ToInt32(pro.ProductId), Convert.ToInt32(pro.Purchase));
            }

        }


        //Consignment Process
        public static void Consignment_Run_Process(int _year, int _month, List<Stock_Transaction> productList)
        {
            POSEntities entity = new POSEntities();

            //Purchase Save or Update
            foreach (var pro in productList)
            {
                string _tranDate = Month_Name(_month, _year);
                var _stockTranID = (from x in entity.StockTransactions
                                    where x.ProductId == pro.ProductId
                                        && x.TranDate == _tranDate
                                    select x.StockTranId).FirstOrDefault();

                APP_Data.StockTransaction _stObject = new APP_Data.StockTransaction();
                if (_stockTranID == 0)
                {

                    _stObject.TranDate = _tranDate;
                    _stObject.ProductId = Convert.ToInt32(pro.ProductId);
                    _stObject.Opening = Opening_Run_Balance(_year, _month, Convert.ToInt64(pro.ProductId));
                    _stObject.Purchase = 0;
                    _stObject.Refund = 0;
                    _stObject.Sale = 0;
                    _stObject.AdjustmentStockIn = 0;
                    _stObject.AdjustmentStockOut = 0;
                    _stObject.Month = _month;
                    _stObject.Year = _year;
                    _stObject.Consignment = Convert.ToInt32(pro.Consignment);
                    _stObject.ConversionStockIn = 0;
                    _stObject.ConversionStockOut = 0;
                    _stObject.StockIn = 0;
                    _stObject.StockOut = 0;
                    entity.StockTransactions.Add(_stObject);
                    entity.SaveChanges();
                }
                else
                {

                    APP_Data.StockTransaction updateStockTransaction = (from s in entity.StockTransactions where s.StockTranId == _stockTranID select s).FirstOrDefault();

                    updateStockTransaction.Consignment = updateStockTransaction.Consignment + Convert.ToInt32(pro.Consignment);
                    //if (updateStockTransaction.Opening > 0)
                    //{
                    //    updateStockTransaction.Opening = Opening_Run_Balance(_year, _month, Convert.ToInt64(pro.ProductId)) + Convert.ToInt32(pro.Consignment);
                    //}
                    entity.Entry(updateStockTransaction).State = EntityState.Modified;
                    entity.SaveChanges();
                }
                Update_Opening_BalanceFor_NextMonth(_month, _year, Convert.ToInt32(pro.ProductId), Convert.ToInt32(pro.Consignment));
            }

        }


        //Refund Process
        public static void Refund_Run_Process(int _year, int _month, List<Stock_Transaction> productList)
        {
            POSEntities entity = new POSEntities();

            //Purchase Save or Update
            foreach (var pro in productList)
            {
                string _tranDate = Month_Name(_month, _year);
                var _stockTranID = (from x in entity.StockTransactions
                                    where x.ProductId == pro.ProductId
                                        && x.TranDate == _tranDate
                                    select x.StockTranId).FirstOrDefault();

                APP_Data.StockTransaction _stObject = new APP_Data.StockTransaction();
                if (_stockTranID == 0)
                {

                    _stObject.TranDate = _tranDate;
                    _stObject.ProductId = Convert.ToInt32(pro.ProductId);
                    _stObject.Opening = Opening_Run_Balance(_year, _month, Convert.ToInt64(pro.ProductId));
                    _stObject.Purchase = 0;
                    _stObject.Refund = pro.Refund;
                    _stObject.Sale = 0;
                    _stObject.AdjustmentStockIn = 0;
                    _stObject.AdjustmentStockOut = 0;
                    _stObject.Consignment = 0;
                    _stObject.ConversionStockIn = 0;
                    _stObject.ConversionStockOut = 0;
                    _stObject.StockIn = 0;
                    _stObject.StockOut = 0;
                    _stObject.Month = _month;
                    _stObject.Year = _year;
                    entity.StockTransactions.Add(_stObject);
                    entity.SaveChanges();
                }
                else
                {

                    APP_Data.StockTransaction updateStockTransaction = (from s in entity.StockTransactions where s.StockTranId == _stockTranID select s).FirstOrDefault();

                    updateStockTransaction.Refund = updateStockTransaction.Refund + pro.Refund;
                    //if (updateStockTransaction.Opening > 0)
                    //{
                    //    updateStockTransaction.Opening = Opening_Run_Balance(_year, _month, Convert.ToInt64(pro.ProductId)) + pro.Refund;
                    //}
                    entity.Entry(updateStockTransaction).State = EntityState.Modified;
                    entity.SaveChanges();
                }
                Update_Opening_BalanceFor_NextMonth(_month, _year, Convert.ToInt32(pro.ProductId), Convert.ToInt32(pro.Refund));
            }

        }

        //Sale Process
        public static void Sale_Run_Process(int _year, int _month, List<Stock_Transaction> productList)
        {
            POSEntities entity = new POSEntities();

            //Purchase Save or Update
            foreach (var pro in productList)
            {
                string _tranDate = Month_Name(_month, _year);
                var _stockTranID = (from x in entity.StockTransactions
                                    where x.ProductId == pro.ProductId
                                        && x.TranDate == _tranDate
                                    select x.StockTranId).FirstOrDefault();

                APP_Data.StockTransaction stockTransaction = new APP_Data.StockTransaction();
                if (_stockTranID == 0)
                {

                    stockTransaction.TranDate = _tranDate;
                    stockTransaction.ProductId = Convert.ToInt32(pro.ProductId);
                    stockTransaction.Opening = Opening_Run_Balance(_year, _month, Convert.ToInt64(pro.ProductId));
                    stockTransaction.Purchase = 0;
                    stockTransaction.Refund = 0;
                    stockTransaction.Sale = pro.Sale;
                    stockTransaction.AdjustmentStockIn = 0;
                    stockTransaction.AdjustmentStockOut = 0;
                    stockTransaction.Consignment = 0;
                    stockTransaction.ConversionStockIn = 0;
                    stockTransaction.ConversionStockOut = 0;
                    stockTransaction.StockIn = 0;
                    stockTransaction.StockOut = 0;
                    stockTransaction.Month = _month;
                    stockTransaction.Year = _year;
                    entity.StockTransactions.Add(stockTransaction);
                    entity.SaveChanges();
                }
                else
                {

                    APP_Data.StockTransaction updateStockTransaction = (from s in entity.StockTransactions where s.StockTranId == _stockTranID select s).FirstOrDefault();
                    updateStockTransaction.Sale = updateStockTransaction.Sale + pro.Sale;
                    entity.Entry(updateStockTransaction).State = EntityState.Modified;
                    entity.SaveChanges();
                }
                Update_Opening_BalanceFor_NextMonth(_month, _year, Convert.ToInt32(pro.ProductId), Convert.ToInt32(pro.Sale * -1));
            }

        }

        #region Adjustment Process
        public static void Adjustment_Run_Process(int _year, int _month, List<Stock_Transaction> productList)
        {
            POSEntities entity = new POSEntities();

            //Purchase Save or Update
            foreach (var pro in productList)
            {
                string _tranDate = Month_Name(_month, _year);
                var _stockTranID = (from x in entity.StockTransactions
                                    where x.ProductId == pro.ProductId
                                        && x.TranDate == _tranDate
                                    select x.StockTranId).FirstOrDefault();

                APP_Data.StockTransaction _stObject = new APP_Data.StockTransaction();
                if (_stockTranID == 0)
                {

                    _stObject.TranDate = _tranDate;
                    _stObject.ProductId = Convert.ToInt32(pro.ProductId);
                    _stObject.Opening = Opening_Run_Balance(_year, _month, Convert.ToInt64(pro.ProductId));
                    _stObject.Purchase = 0;
                    _stObject.Refund = 0;
                    _stObject.Sale = 0;
                    if (pro.AdjustmentQty < 0)
                    {
                        _stObject.AdjustmentStockOut = pro.AdjustmentQty * -1;
                        _stObject.AdjustmentStockIn = 0;
                    }
                    else
                    {
                        _stObject.AdjustmentStockIn = pro.AdjustmentQty;
                        _stObject.AdjustmentStockOut = 0;
                    }

                    _stObject.Consignment = 0;
                    _stObject.ConversionStockIn = 0;
                    _stObject.StockIn = 0;
                    _stObject.StockOut = 0;
                    _stObject.ConversionStockOut = 0;
                    _stObject.Month = _month;
                    _stObject.Year = _year;
                    entity.StockTransactions.Add(_stObject);
                    entity.SaveChanges();
                }
                else
                {

                    APP_Data.StockTransaction updateStockTransaction = (from s in entity.StockTransactions where s.StockTranId == _stockTranID select s).FirstOrDefault();


                    if (pro.AdjustmentQty < 0)
                    {
                        updateStockTransaction.AdjustmentStockOut = updateStockTransaction.AdjustmentStockOut + (pro.AdjustmentQty * -1);
                    }
                    else
                    {
                        updateStockTransaction.AdjustmentStockIn = updateStockTransaction.AdjustmentStockIn + pro.AdjustmentQty;
                    }

                    entity.Entry(updateStockTransaction).State = EntityState.Modified;
                    entity.SaveChanges();
                }
                Update_Opening_BalanceFor_NextMonth(_month, _year, Convert.ToInt32(pro.ProductId), Convert.ToInt32(pro.AdjustmentQty * -1));
            }

        }
        #endregion

        #region StockInOut Process

        public static void Stock_Run_Process(int _year, int _month, List<Stock_Transaction> productList, bool IsUpdate = false)
        {
            POSEntities entity = new POSEntities();

            //Purchase Save or Update
            foreach (var pro in productList)
            {
                string _tranDate = Month_Name(_month, _year);
                var _stockTranID = (from x in entity.StockTransactions
                                    where x.ProductId == pro.ProductId
                                        && x.TranDate == _tranDate
                                    select x.StockTranId).FirstOrDefault();

                APP_Data.StockTransaction _stObject = new APP_Data.StockTransaction();
                if (_stockTranID == 0)
                {

                    _stObject.TranDate = _tranDate;
                    _stObject.ProductId = Convert.ToInt32(pro.ProductId);
                    _stObject.Opening = Opening_Run_Balance(_year, _month, Convert.ToInt64(pro.ProductId));
                    _stObject.Purchase = 0;
                    _stObject.Refund = 0;
                    _stObject.Sale = 0;
                    _stObject.AdjustmentStockIn = 0;
                    _stObject.AdjustmentStockOut = 0;
                    _stObject.Consignment = 0;
                    _stObject.ConversionStockIn = 0;
                    _stObject.ConversionStockOut = 0;
                    _stObject.Month = _month;
                    _stObject.Year = _year;
                    if (pro.StockIn != null)
                    {
                        _stObject.StockIn = pro.StockIn;
                        _stObject.StockOut = 0;
                    }
                    else if (pro.StockOut != null)
                    {
                        _stObject.StockIn = 0;
                        _stObject.StockOut = pro.StockOut;
                    }


                    entity.StockTransactions.Add(_stObject);
                    entity.SaveChanges();
                }
                else
                {

                    APP_Data.StockTransaction updateStockTransaction = (from s in entity.StockTransactions where s.StockTranId == _stockTranID select s).FirstOrDefault();

                    if (pro.StockIn != null)
                    {

                        updateStockTransaction.StockIn += pro.StockIn;

                    }
                    else if (pro.StockOut != null)
                    {


                        updateStockTransaction.StockOut += pro.StockOut;

                    }







                    //if (updateStockTransaction.Opening > 0)
                    //{
                    //    updateStockTransaction.Opening = Opening_Run_Balance(_year, _month, Convert.ToInt64(pro.ProductId)) + pro.Adjustment;
                    //}
                    entity.Entry(updateStockTransaction).State = EntityState.Modified;
                    entity.SaveChanges();
                }
                if (pro.StockIn != null)
                {
                    Update_Opening_BalanceFor_NextMonth(_month, _year, Convert.ToInt32(pro.ProductId), Convert.ToInt32(pro.StockIn));
                }
                else if (pro.StockOut != null)
                {
                    Update_Opening_BalanceFor_NextMonth(_month, _year, Convert.ToInt32(pro.ProductId), Convert.ToInt32(pro.StockOut));
                }


            }

        }

        #endregion


        #region Conversion Process
        public static void Conversion_Run_Process(int _year, int _month, List<Stock_Transaction> productList, bool IsStockIn)
        {
            POSEntities entity = new POSEntities();

            //Purchase Save or Update
            foreach (var pro in productList)
            {
                string _tranDate = Month_Name(_month, _year);
                var _stockTranID = (from x in entity.StockTransactions
                                    where x.ProductId == pro.ProductId
                                        && x.TranDate == _tranDate
                                    select x.StockTranId).FirstOrDefault();

                APP_Data.StockTransaction _stObject = new APP_Data.StockTransaction();
                if (_stockTranID == 0)
                {

                    _stObject.TranDate = _tranDate;
                    _stObject.ProductId = Convert.ToInt32(pro.ProductId);
                    _stObject.Opening = Opening_Run_Balance(_year, _month, Convert.ToInt64(pro.ProductId));
                    _stObject.Purchase = 0;
                    _stObject.Refund = 0;
                    _stObject.Sale = 0;
                    _stObject.AdjustmentStockIn = 0;
                    _stObject.AdjustmentStockOut = 0;
                    _stObject.StockIn = 0;
                    _stObject.StockOut = 0;
                    _stObject.Consignment = 0;
                    _stObject.ConversionStockIn = IsStockIn == true ? pro.ConversionStockIn : 0;
                    _stObject.ConversionStockOut = IsStockIn == true ? 0 : pro.ConversionStockOut;
                    _stObject.Month = _month;
                    _stObject.Year = _year;
                    entity.StockTransactions.Add(_stObject);
                    entity.SaveChanges();
                }
                else
                {

                    APP_Data.StockTransaction updateStockTransaction = (from s in entity.StockTransactions where s.StockTranId == _stockTranID select s).FirstOrDefault();
                    //updateStockTransaction.ConversionStockIn = updateStockTransaction.Sale + pro.Sale;
                    updateStockTransaction.ConversionStockIn = IsStockIn == true ? updateStockTransaction.ConversionStockIn + pro.ConversionStockIn : updateStockTransaction.ConversionStockIn;
                    updateStockTransaction.ConversionStockOut = IsStockIn == true ? updateStockTransaction.ConversionStockOut : updateStockTransaction.ConversionStockOut + pro.ConversionStockOut;
                    //if (updateStockTransaction.Opening > 0)
                    //{
                    //    updateStockTransaction.Opening = Opening_Run_Balance(_year, _month, Convert.ToInt64(pro.ProductId)) + pro.Sale;
                    //}
                    entity.Entry(updateStockTransaction).State = EntityState.Modified;
                    entity.SaveChanges();
                }
                Update_Opening_BalanceFor_NextMonth(_month, _year, Convert.ToInt32(pro.ProductId), IsStockIn == true ? Convert.ToInt32(pro.ConversionStockIn * -1) : Convert.ToInt32(pro.ConversionStockOut * -1));
            }

        }
        #endregion

        #endregion
        #endregion

        #region Convert String To Date Time
        public static DateTime Convert_Date(string date)
        {
            // string date = dgvTransactionList.Rows[e.RowIndex].Cells[2].Value.ToString();
            DateTime _Trandate = DateTime.ParseExact(date, "dd-MM-yyyy", null);
            return _Trandate;
        }

        #endregion

        #region  Remove Comma

        public static string[] Remove_Comma(string _dataList)
        {
            string[] _removeCommaDataList = _dataList.Split(',');
            return _removeCommaDataList;
        }
        #endregion

        #region Convert string[] To Long[]
        public static List<long> Convert_String_To_Long(string[] _dataList)
        {
            List<long> _longDataList = _dataList.Select(long.Parse).ToList();
            return _longDataList;
        }
        #endregion

        #region Get Whole Sale Or Retial Sale
        public static int WholeSalePriceOrSellingPrice(Product pro, bool IsWholeSale, int dynamicPrice = 0)
        {
            int price = 0;
            if (IsWholeSale)
            {
                price = Convert.ToInt32(pro.WholeSalePrice);
            }
            else if (dynamicPrice > 0)
            {
                price = Convert.ToInt32(dynamicPrice);
            }
            else
            {
                price = Convert.ToInt32(pro.Price);
            }
            return price;
        }
        #endregion

        #region Get Report Path
        public static string GetReportPath(string paymentType)
        {
            string reportPath = "";
            string printer = SettingController.SelectDefaultPrinter;
            switch (printer)
            {
                case "A4 Printer":
                    bool _isSourcecode = SettingController.IsSourcecode;
                    switch (paymentType)
                    {
                        case "Cash":

                            if (!_isSourcecode)
                            {
                                if (SettingController.ShowProductImageIn_A4Reports)
                                {
                                    reportPath = "\\Reports\\InvoiceCashA4PImage.rdlc";
                                }
                                else
                                {
                                    reportPath = "\\Reports\\InvoiceCashA4.rdlc";
                                }

                            }
                            else if (_isSourcecode)
                            {
                                reportPath = "\\Reports\\SourcecodeInvoiceCashA4.rdlc";
                            }
                            break;
                        case "Credit":
                            if (!_isSourcecode)
                            {
                                if (SettingController.ShowProductImageIn_A4Reports)
                                {
                                    reportPath = "\\Reports\\InvoiceCreditA4PImage.rdlc";
                                }
                                else
                                {
                                    reportPath = "\\Reports\\InvoiceCreditA4.rdlc";
                                }

                            }
                            else if (_isSourcecode)
                            {
                                reportPath = "\\Reports\\SourcecodeInvoiceCreditA4.rdlc";
                            }
                            break;
                        case "Settlement":
                            if (!_isSourcecode)
                            {
                                reportPath = "\\Reports\\InvoiceSettlementA4.rdlc";
                            }
                            else if (_isSourcecode)
                            {
                                reportPath = "\\Reports\\SourcecodeInvoiceSettlementA4.rdlc";
                            }
                            break;
                        case "MPU":
                            if (SettingController.ShowProductImageIn_A4Reports)
                            {
                                reportPath = "\\Reports\\InvoiceMPUA4PImage.rdlc";
                            }
                            else
                            {
                                reportPath = "\\Reports\\InvoiceMPUA4.rdlc";
                            }

                            break;
                        case "GiftCard":
                            if (SettingController.ShowProductImageIn_A4Reports)
                            {
                                reportPath = "\\Reports\\InvoiceGiftcardA4PImage.rdlc";
                            }
                            else
                            {
                                reportPath = "\\Reports\\InvoiceGiftcardA4.rdlc";
                            }

                            break;
                        case "FOC":
                            if (SettingController.ShowProductImageIn_A4Reports)
                            {
                                reportPath = "\\Reports\\InvoiceFOCA4PImage.rdlc";
                            }
                            else
                            {
                                reportPath = "\\Reports\\InvoiceFOCA4.rdlc";
                            }

                            break;
                        case "Tester":
                            if (SettingController.ShowProductImageIn_A4Reports)
                            {
                                reportPath = "\\Reports\\InvoiceFOCA4PImage.rdlc";
                            }
                            else
                            {
                                reportPath = "\\Reports\\InvoiceFOCA4.rdlc";
                            }
                            break;

                    }
                    break;
                case "Slip Printer":
                    switch (paymentType)
                    {
                        case "Cash":
                            reportPath = "\\Reports\\InvoiceCash.rdlc";
                            break;
                        case "Credit":
                            reportPath = "\\Reports\\InvoiceCredit.rdlc";
                            break;
                        case "MPU":
                            reportPath = "\\Reports\\InvoiceMPU.rdlc";
                            break;
                        case "GiftCard":
                            reportPath = "\\Reports\\InvoiceGiftcard.rdlc";
                            break;
                        case "FOC":
                            reportPath = "\\Reports\\InvoiceFOC.rdlc";
                            break;
                        case "Tester":
                            reportPath = "\\Reports\\InvoiceFOC.rdlc";
                            break;
                        case "Settlement":
                            reportPath = "\\Reports\\InvoiceSettlement.rdlc";
                            break;
                    }
                    break;
            }
            return reportPath;
        }
        #endregion

        
        #region  Get A4 Print or Slip Print
        public static string GetDefaultPrinter()
        {
            string printer = SettingController.SelectDefaultPrinter;
            return printer;
        }
        #endregion

        #region Slip Logo
        public static void Slip_Log(ReportViewer rv)
        {

            ReportParameter ImagePath = new ReportParameter("ImagePath", "file:\\" + Application.StartupPath + "\\img\\logo_frenzo.png");
            if (Get_Image(rv))
            {
                //MessageBox.Show("Please define Logo in Configuration Setting!", "mPOS");
                //Setting form = new Setting();
                //form.ShowDialog();
                Get_Image(rv);
            }
            else
            {
                Image_Parameter(rv, "");
            }

        }

        private static bool Get_Image(ReportViewer rv)
        {
            if (SettingController.Logo != "")
            {
                string imagePath = SettingController.Logo.Substring(7);
                Image_Parameter(rv, imagePath);
                return true;
            }
            else
                Image_Parameter(rv, "");
            return false;
        }

        private static void Image_Parameter(ReportViewer rv, string imagePath)
        {
            if (imagePath == "")
            {
                rv.LocalReport.EnableExternalImages = true;
                ReportParameter ImagePath = new ReportParameter("ImagePath", imagePath);
                rv.LocalReport.SetParameters(ImagePath);
            }
            else
            {
                rv.LocalReport.EnableExternalImages = true;
                ReportParameter ImagePath = new ReportParameter("ImagePath", "file:\\" + Application.StartupPath + "\\logo\\" + imagePath);
                rv.LocalReport.SetParameters(ImagePath);
            }
            
        }
        #endregion

        #region Slip Footer
        public static void Slip_A4_Footer(ReportViewer rv)
        {
            rv.LocalReport.EnableExternalImages = true;

            ReportParameter Footer = new ReportParameter("Footer", SettingController.FooterPage);
            rv.LocalReport.SetParameters(Footer);
        }
        #endregion

        #region Product AutoGenerateNo
        public static string Stock_AutoGenerateNo(int brandId)
        {
            string _registerNo = "020";
            string _autoGenerateNo = Get_BrandAutoNo_IncrementNo(brandId);

            string _stockAutoGenerateNo = _registerNo + _autoGenerateNo;
            return _stockAutoGenerateNo;

        }
        public static string Stock_AutoGenerateNo(int brandId, List<APP_Data.Brand> brand)
        {
            string _registerNo = "020";
            string _autoGenerateNo = Get_BrandAutoNo_IncrementNo(brandId, brand);

            string _stockAutoGenerateNo = _registerNo + _autoGenerateNo;
            return _stockAutoGenerateNo;

        }

        public static string Get_BrandAutoNo_IncrementNo(int brandId)
        {
            string _stockGenerateNo = "";
            POSEntities entity = new POSEntities();

            var _brandData = (from b in entity.Brands where b.Id == brandId select b).FirstOrDefault();
            string _brandId = _brandData.Id.ToString();
            int _incrementNo = 0;
            if ((_brandData.AutoGenerateNo.ToString()).Length >= 5)
            {
                _incrementNo = Convert.ToInt32(_brandData.AutoGenerateNo);
            }
            else
            {
                _incrementNo = Convert.ToInt32(_brandData.AutoGenerateNo) + 1;
            }



            string incrementNo = _incrementNo.ToString();
            string _brandAutoNo = "";
            switch (_brandId.Length)
            {
                case 1:
                    _brandAutoNo = _brandId + "0000";
                    break;
                case 2:
                    _brandAutoNo = _brandId + "000";
                    break;
                case 3:
                    _brandAutoNo = _brandId + "00";
                    break;
                case 4:
                    _brandAutoNo = _brandId + "0";
                    break;
                case 5:
                    _brandAutoNo = _brandId;
                    break;
            }


            string _autoGenerateNo = "";
            switch (incrementNo.Length)
            {
                case 1:
                    _autoGenerateNo = "0000" + incrementNo;
                    break;
                case 2:
                    _autoGenerateNo = "000" + incrementNo;
                    break;
                case 3:
                    _autoGenerateNo = "00" + incrementNo;
                    break;
                case 4:
                    _autoGenerateNo = "0" + incrementNo;
                    break;
                case 5:
                    _autoGenerateNo = incrementNo;
                    break;
            }

            _stockGenerateNo = _brandAutoNo + _autoGenerateNo;

            return _stockGenerateNo;
        }
        public static string Get_BrandAutoNo_IncrementNo(int brandId, List<APP_Data.Brand> brand)
        {
            string _stockGenerateNo = "";
            POSEntities entity = new POSEntities();

            var _brandData = (from b in brand where b.Id == brandId select b).FirstOrDefault();
            string _brandId = _brandData.Id.ToString();
            int _incrementNo = 0;
            if ((_brandData.AutoGenerateNo.ToString()).Length >= 5)
            {
                _incrementNo = Convert.ToInt32(_brandData.AutoGenerateNo);
            }
            else
            {
                _incrementNo = Convert.ToInt32(_brandData.AutoGenerateNo) + 1;
            }



            string incrementNo = _incrementNo.ToString();
            string _brandAutoNo = "";
            switch (_brandId.Length)
            {
                case 1:
                    _brandAutoNo = _brandId + "0000";
                    break;
                case 2:
                    _brandAutoNo = _brandId + "000";
                    break;
                case 3:
                    _brandAutoNo = _brandId + "00";
                    break;
                case 4:
                    _brandAutoNo = _brandId + "0";
                    break;
                case 5:
                    _brandAutoNo = _brandId;
                    break;
            }


            string _autoGenerateNo = "";
            switch (incrementNo.Length)
            {
                case 1:
                    _autoGenerateNo = "0000" + incrementNo;
                    break;
                case 2:
                    _autoGenerateNo = "000" + incrementNo;
                    break;
                case 3:
                    _autoGenerateNo = "00" + incrementNo;
                    break;
                case 4:
                    _autoGenerateNo = "0" + incrementNo;
                    break;
                case 5:
                    _autoGenerateNo = incrementNo;
                    break;
            }

            _stockGenerateNo = _brandAutoNo + _autoGenerateNo;

            return _stockGenerateNo;
        }
        #endregion

        #region Printing
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rv"></param>
        public static void Get_Print(ReportViewer rv)
        {
            int copy = Convert.ToInt32(SettingController.DefaultNoOfCopies);
            for (int i = 0; i < copy; i++)
            {
                PrintDoc.PrintReport(rv, GetDefaultPrinter());
            }
        }
        #endregion

        #region return plus prepaid amount in credit transaction
        public static void Plus_PreaidAmt(Transaction ts)
        {
            POSEntities entity = new POSEntities();
            // update Prepaid Transaction id = false   and delete list in useprepaiddebt table
            var prepaidList = entity.UsePrePaidDebts.Where(x => x.CreditTransactionId == ts.Id && ts.IsActive == true).ToList();

            if (prepaidList.Count > 0)
            {
                List<string> prepaidIdList = prepaidList.Select(x => x.PrePaidDebtTransactionId).ToList();

                (from t in entity.Transactions where prepaidIdList.Contains(t.Id) select t).ToList().ForEach(t => t.IsActive = false);

                //var setToRemove = new HashSet<UsePrePaidDebt>(prepaidList);
                // prepaidList.RemoveAll(x => setToRemove.Contains(x));
                //entity.UsePrePaidDebts.Where(x => x.CreditTransactionId == ts.Id).ToList().RemoveAll(x => setToRemove.Contains(x));
                //entity.SaveChanges();

                foreach (var p in prepaidList)
                {
                    APP_Data.UsePrePaidDebt DeleteObj = (from u in entity.UsePrePaidDebts where u.Id == p.Id select u).FirstOrDefault();
                    entity.UsePrePaidDebts.Remove(DeleteObj);
                    entity.SaveChanges();
                }
            }
        }
        #endregion

        #region Check and Add FOC in Category
        public static void Check_AddFOCInCag()
        {
            POSEntities entity = new POSEntities();
            var val = (from c in entity.ProductCategories where c.Name == "FOC" select c).FirstOrDefault();

            if (val == null)
            {
                APP_Data.ProductCategory cag = new APP_Data.ProductCategory();

                cag.Name = "FOC";
                cag.IsDelete = false;
                entity.ProductCategories.Add(cag);
                entity.SaveChanges();

                APP_Data.ProductSubCategory subCag = new APP_Data.ProductSubCategory();
                subCag.Name = "FOC";
                subCag.ProductCategoryId = cag.Id;
                subCag.IsDelete = false;
                entity.ProductSubCategories.Add(subCag);
                entity.SaveChanges();

            }
        }
        #endregion

        #region Related Shop 
        public static void ShopComBo_EnableOrNot(ComboBox cboShop, Boolean IsSetting = false)
        {
            POSEntities entity = new POSEntities();

            switch (IsSetting)
            {
                case true:
                    Disabled_Shop(cboShop);
                    break;
                case false:
                    var _isBackOffice = entity.Settings.Where(x => x.Key == "IsBackOffice" && x.Value == "1").FirstOrDefault();

                    if (_isBackOffice == null)
                    {
                        Disabled_Shop(cboShop);
                    }
                    break;
            }
        }

        public static bool IsNotBackOffice()
        {
            POSEntities entity = new POSEntities();
            var isNotBackOffice = entity.Settings.Where(x => x.Key == "IsBackOffice" && (x.Value == "0" || x.Value == null)).FirstOrDefault();
            if (isNotBackOffice != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TransactionDelRefHide(int shopid)
        {
            if (SettingController.DefaultShop.Id != shopid)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void Gpvisible(GroupBox g, Boolean edit)
        {
            if (edit == true)
            {
                g.Enabled = true;
            }
            else
            {
                g.Enabled = false;
            }
        }
        private static void Disabled_Shop(ComboBox cboShop)
        {
            POSEntities pOSEntity = new POSEntities();
            int shopId = SettingController.DefaultShop.Id;
            var isExistsRecord = (from t in pOSEntity.Transactions join u in pOSEntity.Users on t.UserId equals u.Id where u.Name == MemberShip.UserName && t.ShopId == shopId select new { u = u.Id }).ToList();
            if (isExistsRecord.Count == 0)
            {
                cboShop.SelectedValue = shopId;
                cboShop.Enabled = true;
            }
            else
            {
                cboShop.Enabled = false;
            }
        }
        #endregion

        #region "Generate Code No"
        public static string Get_CodeNo(string pre, string shortCode)
        {

            string _codeNo = "";
            string month = "";
            if (DateTime.Now.Month < 10)
            {
                month = "0" + DateTime.Now.Month.ToString();
            }
            else
            {
                month = DateTime.Now.Month.ToString();
            }

            _codeNo = pre + shortCode + DateTime.Now.Year + month + DateTime.Now.Day;
            return _codeNo;
        }
        #endregion


        #region "Generate User Code No"
        public static string Get_UserCodeNo(int _shopId)
        {
            POSEntities entity = new POSEntities();
            var _userId = 0;
            _userId = (from con in entity.Users orderby con.Id descending select con.Id).FirstOrDefault();
            var _shortCode = entity.Shops.Where(x => x.Id == _shopId).Select(x => x.ShortCode).FirstOrDefault();
            //string UserCodeNo = "US" + _shortCode + DateTime.Now.Year.ToString() + month + DateTime.Now.Day.ToString() + (_userId + 1).ToString();
            string UserCodeNo = Get_CodeNo("US", _shortCode) + (_userId + 1).ToString();

            return UserCodeNo;
        }

        public static string AESDecryptString(string cipherText, string key)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
        #endregion

    }
    public static class S3Uploader
    {
        public static bool BackupBAK()
        {

            string bucketName = SettingController.S3BucketName;
            List<string> s3EncryptKeys = SettingController.S3EncryptedKeys;

            if (string.IsNullOrEmpty(bucketName) || (s3EncryptKeys.Count != 2))
            {
                return false;
            }

            string activeDir = @"d:\";
            bool directorD = true;
            if (!Directory.Exists(activeDir))
            {
                directorD = false;
                activeDir = @"c:\";
            }
            /*-- Create a new subfolder under the current active folder --*/
            string newPath = System.IO.Path.Combine(activeDir, "DB_Backups");

            if (!System.IO.Directory.Exists(newPath))
            {
                DirectoryInfo di = Directory.CreateDirectory(newPath);
                //di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }

            /*-- Backup DB --*/
            BackupHelper backHelper = new BackupHelper();
            string fileName;
            string bakName = DatabaseControlSetting._DBName + "[" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss-tt") + "].bak";

            fileName = directorD == true ? "D:/DB_Backups/" + bakName : "C:/DB_Backups/" + bakName;

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            bool isBackup = false;

            backHelper.BackupDatabase(DatabaseControlSetting._DBName, DatabaseControlSetting._DBUser, DatabaseControlSetting._DBPassword, DatabaseControlSetting._ServerName, fileName, ref isBackup);

            /*-- Upload to S3 --*/

            if (isBackup)
            {
                S3Uploader.UploadBak(fileName, bakName, bucketName, DateTime.Now.ToString("MMM-yyyy"), s3EncryptKeys);
            }

            return true;
        }
        public static void UploadBak(string SourceFilePath, string bakName, string bucketName, string innerBucketName, List<string> s3EncryptKeys)
        {
            string accessKey = Utility.AESDecryptString(s3EncryptKeys[0], "b14ca5898a4e4133bbce2ea23#4&%@16"), secryptKey = Utility.AESDecryptString(s3EncryptKeys[1], "b14ca5898a4e4133bbce2ea23#4&%@16");
            Amazon.Runtime.BasicAWSCredentials credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secryptKey);

            Amazon.S3.AmazonS3Client s3Client = new Amazon.S3.AmazonS3Client(credentials, Amazon.RegionEndpoint.APSoutheast1);

            #region upload
            try
            {
                //Amazon.S3.Transfer.TransferUtility fileTransferUtility = new Amazon.S3.Transfer.TransferUtility(s3Client);
                //fileTransferUtility.Upload(SourceFilePath, bucketName, bakName);
                var fileStream = new FileStream(SourceFilePath, FileMode.Open);

                var request = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = $"{innerBucketName}/{bakName}", // Specify the full object key including the inner bucket path
                    InputStream = fileStream,
                    ContentType = "application/octet-stream" // Set the content type as needed
                };

                s3Client.PutObject(request);

                fileStream.Close();
                MessageBox.Show("Successfully Exported to S3");
            }
            catch (Amazon.S3.AmazonS3Exception e)
            {
                MessageBox.Show("Error encountered on server. Message:'{0}' while uploading the file", e.Message);
            }
            catch (Exception e)
            {
                MessageBox.Show("Unknown encountered on server. Message:'{0}' while uploading the file", e.Message);
            }
            #endregion

        }
    }

    public class _dynamicPrice
    {
        public string dynamicProductCode { get; set; }

        public int dynamicPrice { get; set; }

    }
    public class GWPTransactionsController
    {
        public string Name;
        public string TransactionNo;
        public string ItemCode;
        public string GiftName;
        public decimal Discount;
        public int Qty;
        public decimal TotalAmount;
    }

    public class GWPSetController
    {
        public int Id;
        public string Name;
        public int Qty;
        public Int64 Amount;
    }
    public class ConvertController
    {
        public int PurchaseDetailId;
        public int ConvertQty;
    }
    public class ProductInOpening
    {
        public int ProductId;
        public string ProductName;
        public string ProductCode;
        public int Purchase;
        public int Refund;
        public int Sale;
        public int AdjustmentStockIn;
        public int AdjustmentStockOut;
        public int Consignment;
        public int ConversionStockIn;
        public int ConversionStockOut;
        public int StockIn;
        public int StockOut;
        public int Opening;
    }
    public class DamageController
    {
        public int Id;
        public string ProductName;
        public string ProductCode;
        public DateTime? AdjustmentDateTime;
        public int? DamageQty;
        public long Price;
        public long? TotalCost;
        public string Reason;
        public string ResponsibleName;

    }

    public class AdjustmentController
    {
        // public int Id;
        public string ProductName;
        public string ProductCode;
        public DateTime? AdjustmentDateTime;
        public int? StockIn;
        public int? StockOut;
        public long Price;
        public long? TotalCost;
        public string Reason;
        public string ResponsibleName;

    }

    public class ExpenseController
    {
        public int ExpenseDetailId;
        public string ExpenseNo;
        public string Description;
        public decimal Qty;
        public decimal Price;
        public decimal Amount;
        public int CategoryId;
        public string CategoryName;
    }
    public class ExpenseRp
    {

        public string ExpenseNo;
        public string Description;
        public int Qty;
        public int Price;
        public int Amount;
        public string CreatedUser;
        public string CategoryName;
        public DateTime ExpenseDate;
    }
    public class strockInheader
    {
        public long? StockInHeaderId;
        public long StockInDetailId;
        public long? productId;
        public string barcode;
        public string productname;
        public int? Qty;

    }
    public class stockheader
    {
        public Int64 StockId;
        public string StockCode;
        public string fromshop;
        public DateTime? date;
        public string status;

    }

    public class Stock_Transaction
    {

        public long? ProductId;
        public string TranDate;
        public int? Opening;
        public int? Purchase;
        public int? Refund;
        public int? Sale;
        // public int? Adjustment;
        public long? Consignment;

        public string ProductName;
        public string ProductCode;
        public int? AdjustmentStockIn;
        public int? AdjustmentStockOut;
        public int? ConversionStockIn;
        public int? ConversionStockOut;

        public int? AdjustmentQty;
        public int? StockIn;
        public int? StockOut;
    }

    public static class MemberShip
    {
        public static string UserName;
        public static string UserRole;
        public static int UserRoleId;
        public static int UserId;
        public static bool isLogin;
        public static bool isAdmin;
        public static int CounterId;
        public static string CounterName;
    }

    public static class TransactionType
    {
        public static string Sale
        {
            get { return "Sale"; }
        }
        public static string Refund
        {
            get { return "Refund"; }
        }
        public static string Settlement
        {
            get { return "Settlement"; }
        }
        public static string Credit
        {
            get { return "Credit"; }
        }
        public static string CreditRefund
        {
            get { return "CreditRefund"; }
        }
        public static string Prepaid
        {
            get { return "Prepaid"; }
        }
    }

    public class TransactionDetailByItemHolder
    {
        public string ItemNo;
        public string Name;
        public string TransactionId;
        public string TransactionType;
        public int Qty;
        public int TotalAmount;
        public DateTime TransactionDate;
        public string Counter_Name;
    }

    public class TopProductHolder
    {
        public string ProductId;
        public string Name;
        public decimal Discount;
        public int Qty;
        public long UnitPrice;
        public long totalAmount;

    }
    public class CustomerInfoHolder
    {
        public int PayableAmount;
        public int Id;       
        public string Name;
        public string PhNo;       
        public string Address;
        public string DOB;
        public string Age;
        public string CustomerCode;
        public long OutstandingAmount;
        public long RefundAmount;
        public long TotalSpentAmount;
        public long VisitCount;       
        public long NoOfRefferedPersons;
        public long TotalSpentAmountByRefferedPersons;
    }


    public class ConsignmentController
    {
        public int ProductId;
        public string Name;
        public int Qty;
        public int Price;
        public long Total;
        public int ConsignmentPrice;
        public long TotalConsignmentPrice;
        public string counter;
        public int Profit;
        public long TotalProfit;
        public int TransactionDetailId;

    }
    public class ReportItemSummary
    {
        public string Id;
        public string Name;
        public int Qty;
        public int UnitPrice;
        public long totalAmount;
        public int PaymentId;
        public string Size;
        public Boolean IsFOC;
        public int SellingPrice;
        public string Remark;
        public int discountrate;
    }
    public class ProductReportController
    {
        public int Id;
        public int Qty;
        public string SKUCode;
        public string ProductName;
        public string BrandName;
        public int TotalQty;
        public string Segment;
        public string SubSegment;
        public string Line;
        public bool IsDiscontinous;
        public bool isConsignment;
        public int UnitPrice;
        public int TotalPrice;
        public int SubTotalPrice;
        public int? PurchasePrice;
        public string PhotoPath;


    }

    public class CustomerSaleController
    {
        public string CustomerName;
        public string ProductName;
        public int Price;
        public int Qty;
        public int TotalAmount;
        public int MemberDiscount;
        public decimal? MCDiscount;
        public int SubTotal;
        public string TransactionId;
        public string MemberType;
        public DateTime SaleDate;
        public string Remark;
    }
    public class SaleBreakDownController
    {
        public int bId;
        public string Name;
        public decimal Sales;
        public decimal BreakDown;
        public int saleQty;
        public decimal Refund;
        public int refundQty;
    }
    public class SpecialPromotionController
    {
        public int bId;
        public string Name;
        public decimal Sales;
        public decimal BreakDown;
        public int saleQty;
        public decimal Refund;
        public int refundQty;
    }

    public class PurchaseReportController
    {
        public DateTime Date;
        public string ProductName;
        public string SupplierName;
        public int UnitPrice;
        public int Qty;
        public Int64 TotalAmount;
        public string VourcherNo;

    }

    public class TotalDailyReport
    {
        public DateTime Date;
        public int TotalTransaction;
        public int TotalQty;
        public Int64 TotalCashAmount;
        public Int64 TotalCreditAmount;
        public Int64 TotalMPUAmount;
        public Int64 TotalGiftCardAmount;
        public Int64 TotalFOCAmount;
        public Int64 TotalTesterAmount;
        public Int64 TotalRefundAmount;
        public Int64 TotalRefundQty;
        public Int64 RepaidAmount;
    }
    public class wproductlist
    {
        public long Id;
        public string ProductCode;
        public string Barcode;
        public string Name;
        public decimal Price;
        public int? ChildQty;
    }
    public class ProfitAndLoss
    {
        public DateTime SaleDate;
        public int TotalSaleQty;
        public Int64 TotalPurchaseAmount;
        public Int64 TotalSaleAmount;
        public Int64 TotalDiscountAmount;
        public Int64 TotalTaxAmount;
        public Int64 ProfitAndLossAmount;
    }

    public class PurchaseProductController
    {

        public int PurchaseDetailId;
        public string Barcode;
        public string ProductName;
        public int Qty;
        public decimal PurchasePrice;
        public decimal Total;
        public Int64 ProductId;
        public DateTime? ExpireDate;
        public int Id;
    }

    public class GiftCardController
    {

        public int GiftCardId;
        public int Amount;
        public string CardNumber;

    }

    public class PurchaseDiscountController
    {
        public DateTime PurchaseDate;
        public string VoucherNo;
        public string SupplierName;
        public Int64 TotalAmount;
        public int DiscountAmount;
    }
    public static class BOOrPOS
    {
        public static bool IsBackOffice;

    }

    public class SaleProductController
    {

        public int PurchaseDetailId;
        public string Barcode;
        public string ProductName;
        public int Qty;
        public Int64 UnitPrice;
        public Int64 ConsignmentPrice;
        public Int64 ProductId;
        public int DiscountPercent;
        public int Tax;
        public bool IsFOC;
    }

    public class TransactionNote
    {
        public string note;
        public string status;
    }
    public class AverageMonthlySaleController
    {
        public string ProductCode;
        public string ProductName;
        public string Unit;
        public int JanQty;
        public int FebQty;
        public int MarQty;
        public int AprQty;
        public int MayQty;
        public int JunQty;
        public int JulQty;
        public int AugQty;
        public int SepQty;
        public int OctQty;
        public int NovQty;
        public int DecQty;
        public int TotalQty;
        public decimal AvgQty;
        public int SellingPrice;
        public long TotalAmountBeforeDiscount;
        public long TotalAmountAfterDiscount;
        public string Remark;
    }




    public class RoleManagementController
    {
        public RoleManagementModel Setting { get; set; }
        public RoleManagementModel CSV { get; set; }
        public RoleManagementModel Consignor { get; set; }
        public RoleManagementModel MeasurementUnit { get; set; }
        public RoleManagementModel CurrencyExchange { get; set; }
        public RoleManagementModel TaxRate { get; set; }
        public RoleManagementModel City { get; set; }
        public RoleManagementModel Product { get; set; }
        public RoleManagementModel Brand { get; set; }
        public RoleManagementModel GiftCard { get; set; }
        public RoleManagementModel Customer { get; set; }
        public RoleManagementModel OutstandingCustomer { get; set; }
        public RoleManagementModel Supplier { get; set; }
        public RoleManagementModel OutstandingSupplier { get; set; }
        public RoleManagementModel ConsigmentSettlement { get; set; }
        public RoleManagementModel Category { get; set; }
        public RoleManagementModel SubCategory { get; set; }
        public RoleManagementModel Counter { get; set; }
        public RoleManagementModel PurchaseRole { get; set; }
        public RoleManagementModel MemberRule { get; set; }
        public RoleManagementModel Adjustment { get; set; }
        public RoleManagementModel Transaction { get; set; }
        public RoleManagementModel CreditTransaction { get; set; }
        public RoleManagementModel TransactionDetail { get; set; }
        public RoleManagementModel Refund { get; set; }
        public RoleManagementModel UnitConversion { get; set; }
        public RoleManagementModel Expense { get; set; }
        public RoleManagementModel ExpenseCategory { get; set; }
        public RoleManagementModel Promotion { get; set; }
        public RoleManagementModel Novelty { get; set; }


        //Reports
        public RoleManagementModel TransactionReport { get; set; }
        public RoleManagementModel ItemSummaryReport { get; set; }
        public RoleManagementModel TaxSummaryReport { get; set; }
        public RoleManagementModel ReorderPointReport { get; set; }
        public RoleManagementModel TransactionDetailReport { get; set; }
        public RoleManagementModel OutstandingCustomerReport { get; set; }
        public RoleManagementModel TopBestSellerReport { get; set; }
        public RoleManagementModel TransactionSummaryReport { get; set; }

        public RoleManagementModel DailySaleSummary { get; set; }
        public RoleManagementModel DailyTotalTransactions { get; set; }
        public RoleManagementModel PurchaseReport { get; set; }
        public RoleManagementModel PurchaseDiscount { get; set; }
        public RoleManagementModel SaleBreakdown { get; set; }
        public RoleManagementModel CustomerSales { get; set; }
        public RoleManagementModel ProductReport { get; set; }
        public RoleManagementModel CustomerInformation { get; set; }
        public RoleManagementModel Consigment { get; set; }
        public RoleManagementModel ProfitAndLoss { get; set; }
        public RoleManagementModel AdjustmentReport { get; set; }
        public RoleManagementModel StockTransactionReport { get; set; }
        public RoleManagementModel AverageMonthlyReport { get; set; }
        public RoleManagementModel NetIncomeReport { get; set; }
        public RoleManagementModel ExpenseReport { get; set; }
        public RoleManagementModel ProductExpireReport { get; set; }
        public RoleManagementModel StockAgingReport { get; set; }
        
        public RoleManagementModel StockManagement { get; set; }
        
        public RoleManagementModel GWPTransactionReport { get; set; }
        public RoleManagementModel PatientAnalysisReport { get; set; }
        public RoleManagementModel TransactionCancelReport { get; set; }


        private int UserRoleId { get; set; }

        public RoleManagementController()
        {
            Setting = new RoleManagementModel();
            Consignor = new RoleManagementModel();
            MeasurementUnit = new RoleManagementModel();
            CurrencyExchange = new RoleManagementModel();
            TaxRate = new RoleManagementModel();
            City = new RoleManagementModel();
            Product = new RoleManagementModel();
            Brand = new RoleManagementModel();
            GiftCard = new RoleManagementModel();
            Customer = new RoleManagementModel();
            OutstandingCustomer = new RoleManagementModel();
            Supplier = new RoleManagementModel();
            OutstandingSupplier = new RoleManagementModel();
            ConsigmentSettlement = new RoleManagementModel();
            Category = new RoleManagementModel();
            SubCategory = new RoleManagementModel();
            Counter = new RoleManagementModel();
            PurchaseRole = new RoleManagementModel();
            MemberRule = new RoleManagementModel();
            Adjustment = new RoleManagementModel();
            Transaction = new RoleManagementModel();
            CreditTransaction = new RoleManagementModel();
            TransactionDetail = new RoleManagementModel();
            Refund = new RoleManagementModel();
            UnitConversion = new RoleManagementModel();
            Expense = new RoleManagementModel();
            ExpenseCategory = new RoleManagementModel();
            CSV = new RoleManagementModel();
            DailySaleSummary = new RoleManagementModel();
            TransactionReport = new RoleManagementModel();
            TransactionSummaryReport = new RoleManagementModel();
            TransactionDetailReport = new RoleManagementModel();
            DailyTotalTransactions = new RoleManagementModel();
            PurchaseReport = new RoleManagementModel();
            PurchaseDiscount = new RoleManagementModel();
            ItemSummaryReport = new RoleManagementModel();
            SaleBreakdown = new RoleManagementModel();
            TaxSummaryReport = new RoleManagementModel();
            TopBestSellerReport = new RoleManagementModel();
            CustomerSales = new RoleManagementModel();
            OutstandingCustomerReport = new RoleManagementModel();
            CustomerInformation = new RoleManagementModel();
            ProductReport = new RoleManagementModel();
            ReorderPointReport = new RoleManagementModel();
            Consigment = new RoleManagementModel();
            ProfitAndLoss = new RoleManagementModel();
            AdjustmentReport = new RoleManagementModel();
            StockTransactionReport = new RoleManagementModel();
            AverageMonthlyReport = new RoleManagementModel();
            NetIncomeReport = new RoleManagementModel();
            ExpenseReport = new RoleManagementModel();
            ProductExpireReport = new RoleManagementModel();
            StockAgingReport = new RoleManagementModel();
            StockManagement = new RoleManagementModel();
            Promotion = new RoleManagementModel();
            Novelty = new RoleManagementModel();
            GWPTransactionReport = new RoleManagementModel();
            PatientAnalysisReport = new RoleManagementModel();
            TransactionCancelReport = new RoleManagementModel();
        }




        public void Load(int roleId)
        {
            UserRoleId = roleId;


            POSEntities entity = new POSEntities();
            //Setting
            Setting.Add = LoadRules(entity, "setting_define");

            //Consignor
            Consignor.EditOrDelete = LoadRules(entity, "consignor_edit");
            Consignor.Add = LoadRules(entity, "consignor_add");

            //Measurement Unit
            MeasurementUnit.EditOrDelete = LoadRules(entity, "measurementunit_edit");
            MeasurementUnit.Add = LoadRules(entity, "measurementunit_add");

            //Currency Exhcange
            CurrencyExchange.Add = LoadRules(entity, "currencyexchange_add");

            //Tax Rate
            TaxRate.EditOrDelete = LoadRules(entity, "taxrate_edit");
            TaxRate.Add = LoadRules(entity, "taxrate_add");

            //City
            City.EditOrDelete = LoadRules(entity, "city_edit");
            City.Add = LoadRules(entity, "city_add");

            //Product
            Product.View = LoadRules(entity, "product_view");
            Product.EditOrDelete = LoadRules(entity, "product_edit");
            Product.Add = LoadRules(entity, "product_add");
            //Brand
            Brand.View = LoadRules(entity, "brand_view");
            Brand.EditOrDelete = LoadRules(entity, "brand_edit");
            Brand.Add = LoadRules(entity, "brand_add");
            //GiftCard
            GiftCard.View = LoadRules(entity, "giftcard_view");
            GiftCard.Add = LoadRules(entity, "giftcard_add");
            GiftCard.EditOrDelete = LoadRules(entity, "giftcard_edit");
            //Customer
            Customer.View = LoadRules(entity, "customer_view");
            Customer.EditOrDelete = LoadRules(entity, "customer_edit");
            Customer.Add = LoadRules(entity, "customer_add");
            Customer.ViewDetail = LoadRules(entity, "customer_viewDetail");
            OutstandingCustomer.View = LoadRules(entity, "customer_viewDetail");
            OutstandingCustomer.View = LoadRules(entity, "outstandingcustomer_view");
            OutstandingCustomer.ViewDetail = LoadRules(entity, "outstandingcustomer_viewDetail");
            Customer.Cancel = LoadRules(entity, "customer_cancel");
            //Supplier
            Supplier.View = LoadRules(entity, "supplier_view");
            Supplier.EditOrDelete = LoadRules(entity, "supplier_edit");
            Supplier.Add = LoadRules(entity, "supplier_add");
            Supplier.ViewDetail = LoadRules(entity, "supplier_viewDetail");
            OutstandingSupplier.View = LoadRules(entity, "outstandingsupplier_view");
            OutstandingSupplier.ViewDetail = LoadRules(entity, "outstandingsupplier_viewDetail");
            //Consignment Settlement
            ConsigmentSettlement.View = LoadRules(entity, "consignmentsettlement_view");
            ConsigmentSettlement.EditOrDelete = LoadRules(entity, "consignmentsettlement_delete");

            //Category
            Category.View = LoadRules(entity, "category_view");
            Category.EditOrDelete = LoadRules(entity, "category_edit");
            Category.Add = LoadRules(entity, "category_add");
            //Sub Category
            SubCategory.View = LoadRules(entity, "subcategory_view");
            SubCategory.EditOrDelete = LoadRules(entity, "subcategory_edit");
            SubCategory.Add = LoadRules(entity, "subcategory_add");
            //Counter
            Counter.EditOrDelete = LoadRules(entity, "counter_edit");
            Counter.Add = LoadRules(entity, "counter_add");
            //Purchase
            PurchaseRole.EditOrDelete = LoadRules(entity, "purchase_delete");
            PurchaseRole.Add = LoadRules(entity, "purchase_add");
            PurchaseRole.ViewDetail = LoadRules(entity, "purchase_viewDetail");
            PurchaseRole.View = LoadRules(entity, "purchase_view");
            PurchaseRole.DeleteLog = LoadRules(entity, "purchase_deletelog");
            PurchaseRole.Approved = LoadRules(entity, "purchase_approved");
            //Member Rule
            MemberRule.Add = LoadRules(entity, "memberrule_add");
            MemberRule.EditOrDelete = LoadRules(entity, "memberrule_delete");
            //Adjustment
            Adjustment.View = LoadRules(entity, "Adjustment_view");
            Adjustment.EditOrDelete = LoadRules(entity, "Adjustment_edit");
            Adjustment.Add = LoadRules(entity, "Adjustment_add");

            //Stock Unit Conversion
            UnitConversion.View = LoadRules(entity, "Stock_Unit_Conversion_view");
            UnitConversion.Add = LoadRules(entity, "Stock_Unit_Conversion_add");

            //Transaction
            Transaction.EditOrDelete = LoadRules(entity, "transaction_delete");
            Transaction.DeleteAndCopy = LoadRules(entity, "transaction_deleteandcopy");
            Transaction.Cancel = LoadRules(entity, "transaction_cancel");
            // Credit Transaction
            CreditTransaction.EditOrDelete = LoadRules(entity, "credit_transaction_delete");
            CreditTransaction.DeleteAndCopy = LoadRules(entity, "credit_transaction_deleteandcopy");
            CreditTransaction.Cancel = LoadRules(entity, "credit_transaction_cancel");

            // Transaction Detail
            TransactionDetail.EditOrDelete = LoadRules(entity, "transactionDetail_delete");

            //Refund
            Refund.EditOrDelete = LoadRules(entity, "refund_delete");

            //Expense Category
            ExpenseCategory.EditOrDelete = LoadRules(entity, "expensecag_edit");
            ExpenseCategory.Add = LoadRules(entity, "expensecag_add");

            //Expense
            Expense.EditOrDelete = LoadRules(entity, "expense_delete");
            Expense.Add = LoadRules(entity, "expense_add");
            Expense.ViewDetail = LoadRules(entity, "expense_viewDetail");
            Expense.View = LoadRules(entity, "expense_view");
            Expense.DeleteLog = LoadRules(entity, "expense_deletelog");
            Expense.Approved = LoadRules(entity, "expense_approved");

            //StockManagement
            StockManagement.Add = LoadRules(entity, "stockmanagement_add");
            StockManagement.EditOrDelete = LoadRules(entity, "stockmanagement_EditDelete");
            StockManagement.Approved = LoadRules(entity, "stockmanagement_approved");
            StockManagement.View = LoadRules(entity, "stockmanagement_view");

            //Promotion
            Promotion.Add = LoadRules(entity, "promotion_add");
            Promotion.EditOrDelete = LoadRules(entity, "promotion_edit");
            Promotion.View = LoadRules(entity, "promotion_view");
            //Novelty
            Novelty.Add = LoadRules(entity, "novelty_add");
            Novelty.EditOrDelete = LoadRules(entity, "novelty_edit");
            Novelty.View = LoadRules(entity, "novelty_view");

            //Reports
            DailySaleSummary.View = LoadRules(entity, "dailySaleSummary_view");
            TransactionReport.View = LoadRules(entity, "transactionReport_view");
            TransactionSummaryReport.View = LoadRules(entity, "transactionSummary_view");
            TransactionDetailReport.View = LoadRules(entity, "transactionDetailReport_view");
            DailyTotalTransactions.View = LoadRules(entity, "dailyTotalTransactions_view");
            PurchaseReport.View = LoadRules(entity, "purchaseReport_view");
            PurchaseDiscount.View = LoadRules(entity, "purchaseDiscount_view");
            ItemSummaryReport.View = LoadRules(entity, "itemSummaryReport_view");
            SaleBreakdown.View = LoadRules(entity, "saleBreakdown_view");
            TaxSummaryReport.View = LoadRules(entity, "taxSummaryReport_view");
            TopBestSellerReport.View = LoadRules(entity, "topBestSellerReport_view");
            CustomerSales.View = LoadRules(entity, "customerSales_view");
            OutstandingCustomerReport.View = LoadRules(entity, "outstandingCustomerReport_view");
            CustomerInformation.View = LoadRules(entity, "customerInformation_view");
            ProductReport.View = LoadRules(entity, "productReport_view");
            ReorderPointReport.View = LoadRules(entity, "reorderPointReport_view");
            Consigment.View = LoadRules(entity, "consigment_view");
            ProfitAndLoss.View = LoadRules(entity, "ProfitAndLoss_view");
            AdjustmentReport.View = LoadRules(entity, "AdjustmentReport_view");
            StockTransactionReport.View = LoadRules(entity, "StockTransactionReport_view");
            AverageMonthlyReport.View = LoadRules(entity, "AverageMonthlyReport_view");
            NetIncomeReport.View = LoadRules(entity, "NetIncomeReport_view");
            ExpenseReport.View = LoadRules(entity, "ExpenseReport_view");
            ProductExpireReport.View = LoadRules(entity, "ProductExpireReport_view");
            StockAgingReport.View = LoadRules(entity, "StockAgingReport_view");
            GWPTransactionReport.View = LoadRules(entity, "GWPReport_view");
            PatientAnalysisReport.View = LoadRules(entity, "PatientAnalysisReport_view");
            TransactionCancelReport.View = LoadRules(entity, "TransactionCancelReport_view");
        }

        public void Save(int roleId)
        {
            UserRoleId = roleId;
            POSEntities entity = new POSEntities();

            //Delete old entry for this userroldId firstly
            List<APP_Data.RoleManagement> RulesListById = entity.RoleManagements.Where(x => x.UserRoleId == UserRoleId).ToList();
            foreach (APP_Data.RoleManagement rule in RulesListById)
            {
                entity.RoleManagements.Remove(rule);
            }

            //Setting
            CreateRules(entity, Setting.Add, "setting_define");

            //Consignor
            CreateRules(entity, Consignor.EditOrDelete, "consignor_edit");
            CreateRules(entity, Consignor.Add, "consignor_add");

            //Measurement Unit
            CreateRules(entity, MeasurementUnit.EditOrDelete, "measurementunit_edit");
            CreateRules(entity, MeasurementUnit.Add, "measurementunit_add");

            //Currency Exhcange
            CreateRules(entity, CurrencyExchange.Add, "currencyexchange_add");

            //Tax Rate
            CreateRules(entity, TaxRate.EditOrDelete, "taxrate_edit");
            CreateRules(entity, TaxRate.Add, "taxrate_add");

            //City
            CreateRules(entity, City.EditOrDelete, "city_edit");
            CreateRules(entity, City.Add, "city_add");

            //Product
            CreateRules(entity, Product.View, "product_view");
            CreateRules(entity, Product.EditOrDelete, "product_edit");
            CreateRules(entity, Product.Add, "product_add");
            //Brand
            CreateRules(entity, Brand.View, "brand_view");
            CreateRules(entity, Brand.EditOrDelete, "brand_edit");
            CreateRules(entity, Brand.Add, "brand_add");
            //GiftCard
            CreateRules(entity, GiftCard.View, "giftcard_view");
            CreateRules(entity, GiftCard.Add, "giftcard_add");
            CreateRules(entity, GiftCard.EditOrDelete, "giftcard_edit");
            //Customer
            CreateRules(entity, Customer.View, "customer_view");
            CreateRules(entity, Customer.EditOrDelete, "customer_edit");
            CreateRules(entity, Customer.Add, "customer_add");
            CreateRules(entity, Customer.ViewDetail, "customer_viewDetail");
            CreateRules(entity, OutstandingCustomer.View, "outstandingcustomer_view");
            CreateRules(entity, OutstandingCustomer.ViewDetail, "outstandingcustomer_viewDetail");
            CreateRules(entity, Customer.Cancel, "customer_cancel");
            //Supplier
            CreateRules(entity, Supplier.View, "supplier_view");
            CreateRules(entity, Supplier.EditOrDelete, "supplier_edit");
            CreateRules(entity, Supplier.Add, "supplier_add");
            CreateRules(entity, Supplier.ViewDetail, "supplier_viewDetail");
            CreateRules(entity, OutstandingSupplier.View, "outstandingsupplier_view");
            CreateRules(entity, OutstandingSupplier.ViewDetail, "outstandingsupplier_viewDetail");
            
            //Consignment Settlement
            CreateRules(entity, ConsigmentSettlement.View, "consignmentsettlement_view");
            CreateRules(entity, ConsigmentSettlement.EditOrDelete, "consignmentsettlement_delete");

            //Category
            CreateRules(entity, Category.View, "category_view");
            CreateRules(entity, Category.EditOrDelete, "category_edit");
            CreateRules(entity, Category.Add, "category_add");
            //Sub Category
            CreateRules(entity, SubCategory.View, "subcategory_view");
            CreateRules(entity, SubCategory.EditOrDelete, "subcategory_edit");
            CreateRules(entity, SubCategory.Add, "subcategory_add");
            //Counter
            CreateRules(entity, Counter.EditOrDelete, "counter_edit");
            CreateRules(entity, Counter.Add, "counter_add");
            //Purchase
            CreateRules(entity, PurchaseRole.EditOrDelete, "purchase_delete");
            CreateRules(entity, PurchaseRole.Add, "purchase_add");
            CreateRules(entity, PurchaseRole.ViewDetail, "purchase_viewDetail");
            CreateRules(entity, PurchaseRole.View, "purchase_view");
            CreateRules(entity, PurchaseRole.DeleteLog, "purchase_deletelog");
            CreateRules(entity, PurchaseRole.Approved, "purchase_approved");
            //Member Rule
            CreateRules(entity, MemberRule.Add, "memberrule_add");
            CreateRules(entity, MemberRule.EditOrDelete, "memberrule_delete");
            //Adjustment
            CreateRules(entity, Adjustment.View, "Adjustment_view");
            CreateRules(entity, Adjustment.EditOrDelete, "Adjustment_edit");
            CreateRules(entity, Adjustment.Add, "Adjustment_add");

            //Stock Unit Conversion
            CreateRules(entity, UnitConversion.View, "Stock_Unit_Conversion_view");
            CreateRules(entity, UnitConversion.Add, "Stock_Unit_Conversion_add");

            // Transaction
            CreateRules(entity, Transaction.EditOrDelete, "transaction_delete");
            CreateRules(entity, Transaction.DeleteAndCopy, "transaction_deleteandcopy");
            CreateRules(entity, Transaction.Cancel, "transaction_cancel");

            // Credit Transaction
            CreateRules(entity, CreditTransaction.EditOrDelete, "credit_transaction_delete");
            CreateRules(entity, CreditTransaction.DeleteAndCopy, "credit_transaction_deleteandcopy");
            CreateRules(entity, CreditTransaction.Cancel, "credit_transaction_cancel");

            // Transaction Detail
            CreateRules(entity, TransactionDetail.EditOrDelete, "transactionDetail_delete");

            //Refund
            CreateRules(entity, Refund.EditOrDelete, "refund_delete");

            //Expense Category
            CreateRules(entity, ExpenseCategory.EditOrDelete, "expensecag_edit");
            CreateRules(entity, ExpenseCategory.Add, "expensecag_add");

            //Expense
            CreateRules(entity, Expense.EditOrDelete, "expense_delete");
            CreateRules(entity, Expense.Add, "expense_add");
            CreateRules(entity, Expense.ViewDetail, "expense_viewDetail");
            CreateRules(entity, Expense.View, "expense_view");
            CreateRules(entity, Expense.DeleteLog, "expense_deletelog");
            CreateRules(entity, Expense.Approved, "expense_approved");

            //Stock Management
            CreateRules(entity, StockManagement.Add, "stockmanagement_add");
            CreateRules(entity, StockManagement.EditOrDelete, "stockmanagement_EditDelete");
            CreateRules(entity, StockManagement.View, "stockmanagement_View");
            CreateRules(entity, StockManagement.Approved, "stockmanagement_approved");

            //Promotion
            CreateRules(entity, Promotion.Add, "promotion_add");
            CreateRules(entity, Promotion.EditOrDelete, "promotion_edit");
            CreateRules(entity, Promotion.View, "promotion_view");
            //Novelty
            CreateRules(entity, Novelty.Add, "novelty_add");
            CreateRules(entity, Novelty.EditOrDelete, "novelty_edit");
            CreateRules(entity, Novelty.View, "novelty_view");
            //Reports
            CreateRules(entity, DailySaleSummary.View, "dailySaleSummary_view");
            CreateRules(entity, TransactionReport.View, "transactionReport_view");
            CreateRules(entity, TransactionSummaryReport.View, "transactionSummary_view");
            CreateRules(entity, TransactionDetailReport.View, "transactionDetailReport_view");
            CreateRules(entity, DailyTotalTransactions.View, "dailyTotalTransactions_view");
            CreateRules(entity, PurchaseReport.View, "purchaseReport_view");
            CreateRules(entity, PurchaseDiscount.View, "purchaseDiscount_view");
            CreateRules(entity, ItemSummaryReport.View, "itemSummaryReport_view");
            CreateRules(entity, SaleBreakdown.View, "saleBreakdown_view");
            CreateRules(entity, TaxSummaryReport.View, "taxSummaryReport_view");
            CreateRules(entity, TopBestSellerReport.View, "topBestSellerReport_view");
            CreateRules(entity, CustomerSales.View, "customerSales_view");
            CreateRules(entity, OutstandingCustomerReport.View, "outstandingCustomerReport_view");
            CreateRules(entity, CustomerInformation.View, "customerInformation_view");
            CreateRules(entity, ProductReport.View, "productReport_view");
            CreateRules(entity, ReorderPointReport.View, "reorderPointReport_view");
            CreateRules(entity, Consigment.View, "consigment_view");
            CreateRules(entity, ProfitAndLoss.View, "ProfitAndLoss_view");
            CreateRules(entity, AdjustmentReport.View, "AdjustmentReport_view");
            CreateRules(entity, StockTransactionReport.View, "StockTransactionReport_view");
            CreateRules(entity, AverageMonthlyReport.View, "AverageMonthlyReport_view");
            CreateRules(entity, NetIncomeReport.View, "NetIncomeReport_view");
            CreateRules(entity, ExpenseReport.View, "ExpenseReport_view");
            CreateRules(entity, ProductExpireReport.View, "ProductExpireReport_view");
            CreateRules(entity, StockAgingReport.View, "StockAgingReport_view");
            CreateRules(entity, GWPTransactionReport.View, "GWPReport_view");
            CreateRules(entity, PatientAnalysisReport.View, "PatientAnalysisReport_view");
            CreateRules(entity, TransactionCancelReport.View, "TransactionCancelReport_view");

        }

        private void CreateRules(POSEntities entity, Boolean IsAllowed, String Rule)
        {
            APP_Data.RoleManagement obj = new APP_Data.RoleManagement();
            obj.UserRoleId = UserRoleId;
            obj.IsAllowed = IsAllowed;
            obj.RuleFeature = Rule;
            entity.RoleManagements.Add(obj);
            entity.SaveChanges();
        }

        private Boolean LoadRules(POSEntities entity, String Rule)
        {
            APP_Data.RoleManagement obj = entity.RoleManagements.Where(x => x.RuleFeature == Rule && x.UserRoleId == UserRoleId).FirstOrDefault();
            Boolean result = false;
            if (obj != null) result = obj.IsAllowed;

            return result;
        }

        public List<bool> IsAllAllowedForOperation(int roleId)
        {
            // First, Make a List of Rule Name which we will show in Role Management Form in Operation Pannel
            #region Rule Name List
            List<string> RuleNameList = new List<string>(new string[]{
            "product_view",
            "product_view",
            "product_edit",

            "brand_view",
            "brand_edit",
            "brand_add",

            "giftcard_view",
            "giftcard_add",
            "giftcard_edit",

            "customer_view",
            "customer_edit",
            "customer_add",
            "customer_viewDetail",
            "customer_cancel",

            "category_view",
            "category_edit",
            "category_add",

            "subcategory_view",
            "subcategory_edit",
            "subcategory_add",

            "counter_edit",
            "counter_add",

            "supplier_view",
            "supplier_edit",
            "supplier_add",
            "supplier_viewDetail",

            "purchase_delete",
            "purchase_add",
            "purchase_viewDetail",
            "purchase_view",

            "memberrule_add",
            "memberrule_delete",

            "Adjustment_view",
            "Adjustment_edit",
            "Adjustment_add",

            "transaction_delete",
            "transaction_deleteandcopy",
            "transaction_cancel",

            "credit_transaction_delete",
            "credit_transaction_deleteandcopy",
            "credit_transaction_cancel",

            "transactionDetail_delete",

            "refund_delete"
            });
            #endregion

            POSEntities entity = new POSEntities();
            var obj = (from rm in entity.RoleManagements.AsEnumerable()
                       join r in RuleNameList on rm.RuleFeature.Trim() equals r
                       where rm.UserRoleId == roleId
                       select rm.IsAllowed).ToList();

            return obj;
        }

        public List<bool> IsAllAllowedForReport(int roleId)
        {
            // First, Make a List of Rule Name which we will show in Role Management Form in Report Pannel
            #region Rule Name List
            List<string> RuleNameList = new List<string>(new string[]{
            "dailySaleSummary_view",
            "transactionReport_view",
            "transactionSummary_view",
            "transactionDetailReport_view",
             "dailyTotalTransactions_view",
             "purchaseReport_view",
           "purchaseDiscount_view",
             "itemSummaryReport_view",
           "saleBreakdown_view",
           "taxSummaryReport_view",
             "topBestSellerReport_view",
             "customerSales_view",
            "outstandingCustomerReport_view",
            "customerInformation_view",
            "productReport_view",
            "reorderPointReport_view",
          "consigment_view",
            "ProfitAndLoss_view",
           "AdjustmentReport_view",
            "StockTransactionReport_view",
            "AverageMonthlyReport_view",
            "NetIncomeReport_view",
            "ExpenseReport_view",
            "ProductExpireReport_view",
            "StockAgingReport_view",
            "PatientAnalysisReport_view",
            "TransactionCancelReport_view"
            });
            #endregion

            POSEntities entity = new POSEntities();
            var obj = (from rm in entity.RoleManagements.AsEnumerable()
                       join r in RuleNameList on rm.RuleFeature.Trim() equals r
                       where rm.UserRoleId == roleId
                       select rm.IsAllowed).ToList();

            return obj;
        }
    }

    public class RoleManagementModel
    {
        public Boolean View { get; set; }
        public Boolean EditOrDelete { get; set; }
        public Boolean Add { get; set; }
        public Boolean ViewDetail { get; set; }
        public Boolean DeleteAndCopy { get; set; }
        public Boolean DeleteLog { get; set; }
        public Boolean Approved { get; set; }
        public Boolean Cancel { get; set; }
    }
    /// <summary>
    /// Perform translation of Controls' Text to desire language.
    /// </summary>
    public class Localization
    {
        static string ZawGyi = "Zawgyi-one";
        static string Myanmar3 = "Myanmar3";
        static string Other1 = "Zawgyi-one";
        static string Other2 = "Zawgyi-one";
        static bool AllowType(Type type)
        {
            if (type == typeof(Label) || type == typeof(GroupBox) || type == typeof(DataGridView) || type == typeof(RadioButton) || type == typeof(CheckBox) || type == typeof(Chart))
            {
                return true;
            }
            return false;
        }
        private static void ControlInsert(Control ct, pjForm dbform)
        {
            POSEntities db = new POSEntities();
            if (ct.GetType() == typeof(MenuStrip))
            {
                MenuStrip menu = (MenuStrip)ct;
                foreach (var tsmi in menu.Items)
                {
                    ToolStripMenuItem tsm = tsmi as ToolStripMenuItem;
                    pjForms_Localization cont = new pjForms_Localization();
                    cont.Id = db.pjForms_Localization.Count() > 0 ? db.pjForms_Localization.Max(a => a.Id) + 1 : 1;
                    cont.FormId = dbform.Id;
                    cont.ControlName = tsm.Name;
                    cont.Type = "MenuItem";
                    cont.Eng = tsm.Text;
                    cont.ZawGyi = "";
                    cont.MM3 = "";
                    cont.Other1 = "";
                    cont.Other2 = "";
                    cont.AllowToLoad = true;
                    db.pjForms_Localization.Add(cont);
                    db.SaveChanges();

                    if (tsm.HasDropDownItems)
                    {
                        if (tsm.Name != "fileMenu")
                        {
                            foreach (var ddi in tsm.DropDownItems)
                            {

                                ToolStripMenuItem ddtsm = ddi as ToolStripMenuItem;
                                if (ddtsm != null)
                                {
                                    pjForms_Localization conte = new pjForms_Localization();
                                    conte.Id = db.pjForms_Localization.Count() > 0 ? db.pjForms_Localization.Max(a => a.Id) + 1 : 1;
                                    conte.FormId = dbform.Id;
                                    conte.ControlName = ddtsm.Name;
                                    conte.Type = "MenuItem";
                                    conte.Eng = ddtsm.Text;
                                    conte.ZawGyi = "";
                                    conte.MM3 = "";
                                    conte.Other1 = "";
                                    conte.Other2 = "";
                                    conte.AllowToLoad = true;
                                    db.pjForms_Localization.Add(conte);
                                    db.SaveChanges();
                                }
                            }
                        }

                    }
                }
            }
            else if (ct.GetType() == typeof(Label))
            {
                Label lab = (Label)ct;
                pjForms_Localization cont = new pjForms_Localization();
                cont.Id = db.pjForms_Localization.Count() > 0 ? db.pjForms_Localization.Max(a => a.Id) + 1 : 1; ;
                cont.FormId = dbform.Id;
                cont.ControlName = lab.Name;
                cont.Type = "Label";
                cont.Eng = lab.Text;
                cont.ZawGyi = "";
                cont.MM3 = "";
                cont.Other1 = "";
                cont.Other2 = "";
                cont.AllowToLoad = lab.Name.Length >= 4 && lab.Name.Substring(0, 4) == "label" || string.IsNullOrEmpty(lab.Text) ? false : true;
                db.pjForms_Localization.Add(cont);
                db.SaveChanges();

            }
            else if (ct.GetType() == typeof(DataGridView))
            {
                DataGridView dgv = (DataGridView)ct;
                pjForms_Localization cont = new pjForms_Localization();
                cont.Id = db.pjForms_Localization.Count() > 0 ? db.pjForms_Localization.Max(a => a.Id) + 1 : 1; ;
                cont.FormId = dbform.Id;
                cont.ControlName = dgv.Name;
                cont.Type = "GridView";
                cont.Eng = dgv.Text;
                cont.ZawGyi = "";
                cont.MM3 = "";
                cont.Other1 = "";
                cont.Other2 = "";
                cont.AllowToLoad = true;
                db.pjForms_Localization.Add(cont);
                db.SaveChanges();

            }
            else if (ct.GetType() == typeof(GroupBox))
            {
                GroupBox dgv = (GroupBox)ct;
                pjForms_Localization cont = new pjForms_Localization();
                cont.Id = db.pjForms_Localization.Count() > 0 ? db.pjForms_Localization.Max(a => a.Id) + 1 : 1; ;
                cont.FormId = dbform.Id;
                cont.ControlName = dgv.Name;
                cont.Type = "GroupBox";
                cont.Eng = dgv.Text;
                cont.ZawGyi = "";
                cont.MM3 = "";
                cont.Other1 = "";
                cont.Other2 = "";
                cont.AllowToLoad = dgv.Name.Length >= 7 && dgv.Name.Substring(0, 7) == "groupbox" || string.IsNullOrEmpty(dgv.Text) ? false : true;
                db.pjForms_Localization.Add(cont);
                db.SaveChanges();
            }
            else if (ct.GetType() == typeof(RadioButton))
            {
                RadioButton rdo = (RadioButton)ct;
                pjForms_Localization cont = new pjForms_Localization();
                cont.Id = db.pjForms_Localization.Count() > 0 ? db.pjForms_Localization.Max(a => a.Id) + 1 : 1; ;
                cont.FormId = dbform.Id;
                cont.ControlName = rdo.Name;
                cont.Type = "Radio";
                cont.Eng = rdo.Text;
                cont.ZawGyi = "";
                cont.MM3 = "";
                cont.Other1 = "";
                cont.Other2 = "";
                cont.AllowToLoad = rdo.Name.Length >= 11 && rdo.Name.Substring(0, 11) == "radiobutton" || string.IsNullOrEmpty(rdo.Text) ? false : true;
                db.pjForms_Localization.Add(cont);
                db.SaveChanges();
            }
            else if (ct.GetType() == typeof(CheckBox))
            {
                CheckBox cbx = (CheckBox)ct;
                pjForms_Localization cont = new pjForms_Localization();
                cont.Id = db.pjForms_Localization.Count() > 0 ? db.pjForms_Localization.Max(a => a.Id) + 1 : 1; ;
                cont.FormId = dbform.Id;
                cont.ControlName = cbx.Name;
                cont.Type = "CheckBox";
                cont.Eng = cbx.Text;
                cont.ZawGyi = "";
                cont.MM3 = "";
                cont.Other1 = "";
                cont.Other2 = "";
                cont.AllowToLoad = cbx.Name.Length >= 8 && cbx.Name.Substring(0, 8) == "checkbox" || string.IsNullOrEmpty(cbx.Text) ? false : true;
                db.pjForms_Localization.Add(cont);
                db.SaveChanges();
            }
            if (ct.GetType() == typeof(System.Windows.Forms.DataVisualization.Charting.Chart))
            {
                System.Windows.Forms.DataVisualization.Charting.Chart chart = (System.Windows.Forms.DataVisualization.Charting.Chart)ct;
                pjForms_Localization cont = new pjForms_Localization();
                cont.Id = db.pjForms_Localization.Count() > 0 ? db.pjForms_Localization.Max(a => a.Id) + 1 : 1; ;
                cont.FormId = dbform.Id;
                cont.ControlName = chart.Titles.Select(a => a.Name).FirstOrDefault().ToString();
                cont.Type = "Chart";
                cont.Eng = chart.Titles.Select(a => a.Text).FirstOrDefault().ToString();
                cont.ZawGyi = "";
                cont.MM3 = "";
                cont.Other1 = "";
                cont.Other2 = "";
                cont.AllowToLoad = true;
                db.pjForms_Localization.Add(cont);
                db.SaveChanges();
            }
        }
        /// <summary>
        /// Insert Foms and Contols of Assembly to database.
        /// </summary>
        public static void Forms_Controls_Insertion()
        {

            #region Variables
            POSEntities db = new POSEntities();
            List<Form> allFormList = new List<Form>();
            Assembly pos = Assembly.Load("POS");
            var existingForms = db.pjForms.Select(p => new { Name = p.Name }).ToList();
            List<pjForms_Localization> existingControls = db.pjForms_Localization.ToList();
            List<pjForms_Localization> newlyAddedControls = new List<pjForms_Localization>();
            bool isFormsFirstTime = existingForms.Count > 0 ? false : true;
            bool isControlsFirstTime = existingControls.Count > 0 ? false : true;
            #endregion
            #region FormInsert
            if (db.pjForms.Count() == 0)
            {
                foreach (var t in pos.GetTypes())
                {
                    if (t.BaseType == typeof(Form))
                    {
                        var emptyCtor = t.GetConstructor(Type.EmptyTypes);
                        if (emptyCtor != null)
                        {
                            var f = (Form)emptyCtor.Invoke(new object[] { });
                            allFormList.Add(f);
                        }
                    }
                }
                if (isFormsFirstTime)
                {
                    List<pjForm> toSaveForms = new List<pjForm>();
                    foreach (Form f in allFormList)
                    {
                        pjForm toSaveForm = new pjForm();
                        toSaveForm.Name = f.Name;
                        toSaveForm.TextEng = f.Text;
                        toSaveForm.AllowToLoad = f.Controls.OfType<Label>().Count() > 0 ||
                                                  f.Controls.OfType<DataGridView>().Count() > 0 ||
                                                  f.Controls.OfType<GroupBox>().Count() > 0 ||
                                                  f.Controls.OfType<MenuStrip>().Count() > 0 ||
                                                  f.Controls.OfType<Panel>().Count() > 0 ||
                                                  f.Controls.OfType<FlowLayoutPanel>().Count() > 0 ||
                                                  f.Controls.OfType<TableLayoutPanel>().Count() > 0 ||
                                                   f.Controls.OfType<TabControl>().Count() > 0 ? true : false;
                        toSaveForm.TextMyanmar = "";
                        db.pjForms.Add(toSaveForm);
                        db.SaveChanges();
                    }
                }
                else
                {
                    var projectsForms = allFormList.Select(f => new { Name = f.Name }).ToList();
                    var newlyAddedForms = projectsForms.Except(existingForms).ToList();
                    if (newlyAddedForms.Count > 0)
                    {
                        foreach (var item in newlyAddedForms)
                        {
                            pjForm newly = new pjForm();
                            Form f = allFormList.Where(a => a.Name == item.Name).FirstOrDefault();
                            newly.Name = f.Name;
                            newly.TextEng = f.Text;
                            newly.TextMyanmar = "";
                            newly.AllowToLoad = f.Controls.OfType<Label>().Count() > 0 ||
                                                  f.Controls.OfType<DataGridView>().Count() > 0 ||
                                                  f.Controls.OfType<GroupBox>().Count() > 0 ||
                                                  f.Controls.OfType<MenuStrip>().Count() > 0 ||
                                                  f.Controls.OfType<Panel>().Count() > 0 ||
                                                  f.Controls.OfType<FlowLayoutPanel>().Count() > 0 ||
                                                  f.Controls.OfType<TableLayoutPanel>().Count() > 0 ||
                                                   f.Controls.OfType<TabControl>().Count() > 0 ? true : false;
                            db.pjForms.Add(newly);
                            db.SaveChanges();
                        }
                    }
                }

                #endregion
                #region ControlInsert
                if (db.pjForms.Count() > 0)
                {
                    foreach (var dbform in db.pjForms.Where(a => a.AllowToLoad == true).ToList())
                    {
                        var formType = Assembly.GetExecutingAssembly().GetTypes()
                                  .Where(a => a.BaseType == typeof(Form) && a.Name == dbform.Name)
                                  .FirstOrDefault();

                        Form form = (Form)Activator.CreateInstance(formType);
                        foreach (var ct in GetAll(form, null).ToList())
                        {

                            if (!isControlsFirstTime && db.pjForms_Localization.Where(a => a.ControlName == ct.Name && a.pjForm.Name == dbform.Name).FirstOrDefault() == null && ct.GetType() != typeof(MenuStrip) && AllowType(ct.GetType()))
                            {
                                ControlInsert(ct, dbform);
                            }
                            else if (isControlsFirstTime)
                            {
                                ControlInsert(ct, dbform);
                            }
                        }
                    }
                }
            }
            #endregion
        }
        private static IEnumerable<Control> GetAll(Control control, Type type = null)
        {
            var controls = control.Controls.Cast<Control>();

            if (type == null)
                return controls.SelectMany(ctrl => GetAll(ctrl, type)).Concat(controls);
            else
                return controls.SelectMany(ctrl => GetAll(ctrl, type)).Concat(controls).Where(c => c.GetType() == type);
        }
        static Font DefaultFont(string defaultFont, string formname = null)
        {
            switch (defaultFont)
            {
                case "ZawGyi":
                    return formname == "chart" || formname == "dashboard" ? new Font(ZawGyi, 10, FontStyle.Bold) : new Font(ZawGyi, 9, FontStyle.Regular);
                case "MM3":
                    return formname == "chart" || formname == "dashboard" ? new Font(Myanmar3, 10, FontStyle.Bold) : new Font(Myanmar3, 9, FontStyle.Regular);
                case "Other1":
                    return formname == "chart" || formname == "dashboard" ? new Font(Other1, 10, FontStyle.Bold) : new Font(Other1, 9, FontStyle.Regular); ;
                case "Other2":
                    return formname == "chart" || formname == "dashboard" ? new Font(Other2, 10, FontStyle.Bold) : new Font(Other2, 9, FontStyle.Regular); ;
                default:
                    return new Font(ZawGyi, 9, FontStyle.Regular);
            }
        }
        /// <summary>
        /// Translate all controls of desired type of given form.
        /// </summary>
        /// <param name="form"></param>
        public static void Localize_FormControls(Form form)
        {
            if (SettingController.TopMost)
            {
                form.TopMost = SettingController.TopMost;
            }
            POSEntities db = new POSEntities();
            pjForm dbForm =
                db.pjForms.AsEnumerable()
                .Where(f => f.Name == form.Name && f.AllowToLoad == true).FirstOrDefault();

            if (dbForm != null)
            {

                if (SettingController.DefaultFont != "English")
                {
                    string FORMNAME = form.Name.ToLower();
                    foreach (var c in GetAll(form, null).ToList())
                    {
                        if (c.GetType() == typeof(MenuStrip))
                        {
                            MenuStrip menu = (MenuStrip)c;
                            foreach (var tsm in menu.Items)
                            {

                                ToolStripMenuItem tsmi = tsm as ToolStripMenuItem;
                                tsmi.Font = DefaultFont(SettingController.DefaultFont, FORMNAME);
                                var dbC = db.pjForms_Localization.Where(p => p.ControlName == tsmi.Name).FirstOrDefault();
                                if (SettingController.DefaultFont == "ZawGyi" && dbC != null && !string.IsNullOrEmpty(dbC.ZawGyi) && !string.IsNullOrWhiteSpace(dbC.ZawGyi))
                                {
                                    tsmi.Text = dbC.ZawGyi;
                                }
                                else if (SettingController.DefaultFont == "Myanmar3" && dbC != null && !string.IsNullOrEmpty(dbC.MM3) && !string.IsNullOrWhiteSpace(dbC.MM3))
                                {
                                    tsmi.Text = dbC.MM3;
                                }
                                else if (SettingController.DefaultFont == "Other1" && dbC != null && !string.IsNullOrEmpty(dbC.Other1) && !string.IsNullOrWhiteSpace(dbC.Other1))
                                {
                                    tsmi.Text = dbC.Other1;
                                }
                                else if (SettingController.DefaultFont == "Other2" && dbC != null && !string.IsNullOrEmpty(dbC.Other2) && !string.IsNullOrWhiteSpace(dbC.Other2))
                                {
                                    tsmi.Text = dbC.Other2;
                                }
                                else if (dbC != null)
                                {
                                    tsmi.Text = dbC.Eng;
                                }
                                if (tsmi.HasDropDownItems)
                                {
                                    if (tsmi.Name != "fileMenu")
                                    {
                                        foreach (var child in tsmi.DropDownItems)
                                        {
                                            ToolStripMenuItem childtsmi = child as ToolStripMenuItem;
                                            childtsmi.Font = DefaultFont(SettingController.DefaultFont, FORMNAME);
                                            var dbChild = db.pjForms_Localization.Where(p => p.ControlName == childtsmi.Name && p.AllowToLoad == true).FirstOrDefault();
                                            if (SettingController.DefaultFont == "ZawGyi" && dbChild != null && !string.IsNullOrWhiteSpace(dbChild.ZawGyi))
                                            {
                                                childtsmi.Text = dbChild.ZawGyi;
                                            }
                                            else if (SettingController.DefaultFont == "Myanmar3" && dbChild != null && !string.IsNullOrWhiteSpace(dbChild.MM3))
                                            {
                                                childtsmi.Text = dbChild.MM3;
                                            }
                                            else if (SettingController.DefaultFont == "Other1" && dbChild != null && !string.IsNullOrWhiteSpace(dbChild.Other1))
                                            {
                                                childtsmi.Text = dbChild.Other1;
                                            }
                                            else if (SettingController.DefaultFont == "Other2" && dbChild != null && !string.IsNullOrWhiteSpace(dbChild.Other2))
                                            {
                                                childtsmi.Text = dbChild.Other2;
                                            }
                                            else if (dbChild != null)
                                            {
                                                childtsmi.Text = dbChild.Eng;
                                            }
                                        }
                                    }

                                }
                            }

                        }
                        else if (c.GetType() == typeof(Label))
                        {
                            Label label = c as Label;
                            label.Font = DefaultFont(SettingController.DefaultFont, FORMNAME);
                            var dbLabel = db.pjForms_Localization.Where(p => p.ControlName == label.Name && p.pjForm.Name == form.Name && p.AllowToLoad == true).FirstOrDefault();
                            if (SettingController.DefaultFont == "ZawGyi" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.ZawGyi))
                            {
                                label.Text = dbLabel.ZawGyi;
                            }
                            else if (SettingController.DefaultFont == "Myanmar3" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.MM3))
                            {
                                label.Text = dbLabel.MM3;
                            }
                            else if (SettingController.DefaultFont == "Other1" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.Other1))
                            {
                                label.Text = dbLabel.Other1;
                            }
                            else if (SettingController.DefaultFont == "Other2" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.Other2))
                            {
                                label.Text = dbLabel.Other2;
                            }
                            else if (dbLabel != null)
                            {
                                label.Text = dbLabel.Eng;
                            }
                        }
                        else if (c.GetType() == typeof(GroupBox))
                        {
                            GroupBox groupbox = c as GroupBox;
                            groupbox.Font = DefaultFont(SettingController.DefaultFont, FORMNAME);
                            var dbLabel = db.pjForms_Localization.Where(p => p.ControlName == groupbox.Name && p.pjForm.Name == form.Name && p.AllowToLoad == true).FirstOrDefault();
                            if (SettingController.DefaultFont == "ZawGyi" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.ZawGyi))
                            {
                                groupbox.Text = dbLabel.ZawGyi;
                            }
                            else if (SettingController.DefaultFont == "Myanmar3" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.MM3))
                            {
                                groupbox.Text = dbLabel.MM3;
                            }
                            else if (SettingController.DefaultFont == "Other1" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.Other1))
                            {
                                groupbox.Text = dbLabel.Other1;
                            }
                            else if (SettingController.DefaultFont == "Other2" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.Other2))
                            {
                                groupbox.Text = dbLabel.Other2;
                            }
                            else if (dbLabel != null)
                            {
                                groupbox.Text = dbLabel.Eng;
                            }
                        }
                        else if (c.GetType() == typeof(RadioButton))
                        {
                            RadioButton groupbox = c as RadioButton;
                            groupbox.Font = DefaultFont(SettingController.DefaultFont, FORMNAME);
                            var dbLabel = db.pjForms_Localization.Where(p => p.ControlName == groupbox.Name && p.pjForm.Name == form.Name && p.AllowToLoad == true).FirstOrDefault();
                            if (SettingController.DefaultFont == "ZawGyi" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.ZawGyi))
                            {
                                groupbox.Text = dbLabel.ZawGyi;
                            }
                            else if (SettingController.DefaultFont == "Myanmar3" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.MM3))
                            {
                                groupbox.Text = dbLabel.MM3;
                            }
                            else if (SettingController.DefaultFont == "Other1" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.Other1))
                            {
                                groupbox.Text = dbLabel.Other1;
                            }
                            else if (SettingController.DefaultFont == "Other2" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.Other2))
                            {
                                groupbox.Text = dbLabel.Other2;
                            }
                            else if (dbLabel != null)
                            {
                                groupbox.Text = dbLabel.Eng;
                            }
                        }
                        else if (c.GetType() == typeof(CheckBox))
                        {
                            CheckBox groupbox = c as CheckBox;
                            groupbox.Font = DefaultFont(SettingController.DefaultFont, FORMNAME);
                            var dbLabel = db.pjForms_Localization.Where(p => p.ControlName == groupbox.Name && p.pjForm.Name == form.Name && p.AllowToLoad == true).FirstOrDefault();
                            if (SettingController.DefaultFont == "ZawGyi" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.ZawGyi))
                            {
                                groupbox.Text = dbLabel.ZawGyi;
                            }
                            else if (SettingController.DefaultFont == "Myanmar3" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.MM3))
                            {
                                groupbox.Text = dbLabel.MM3;
                            }
                            else if (SettingController.DefaultFont == "Other1" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.Other1))
                            {
                                groupbox.Text = dbLabel.Other1;
                            }
                            else if (SettingController.DefaultFont == "Other2" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.Other2))
                            {
                                groupbox.Text = dbLabel.Other2;
                            }
                            else if (dbLabel != null)
                            {
                                groupbox.Text = dbLabel.Eng;
                            }
                        }
                        else if (c.GetType() == typeof(System.Windows.Forms.DataVisualization.Charting.Chart))
                        {
                            System.Windows.Forms.DataVisualization.Charting.Chart chart = (System.Windows.Forms.DataVisualization.Charting.Chart)c;
                            string titleName = ""; string titleText = "";
                            titleName = chart.Titles.Select(a => a.Name).FirstOrDefault().ToString();
                            titleText = chart.Titles.Select(a => a.Text).FirstOrDefault().ToString();
                            chart.Titles.FirstOrDefault().Font = DefaultFont(SettingController.DefaultFont, FORMNAME);
                            //chart.Font = DefaultFont(SettingController.DefaultFont, FORMNAME);

                            var dbLabel = db.pjForms_Localization.Where(p => p.ControlName == titleName && p.pjForm.Name == form.Name && p.AllowToLoad == true).FirstOrDefault();
                            if (SettingController.DefaultFont == "ZawGyi" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.ZawGyi))
                            {
                                chart.Titles.FirstOrDefault().Text = dbLabel.ZawGyi;
                            }
                            else if (SettingController.DefaultFont == "Myanmar3" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.MM3))
                            {
                                chart.Titles.FirstOrDefault().Text = dbLabel.MM3;
                            }
                            else if (SettingController.DefaultFont == "Other1" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.Other1))
                            {
                                chart.Titles.FirstOrDefault().Text = dbLabel.Other1;
                            }
                            else if (SettingController.DefaultFont == "Other2" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.Other2))
                            {
                                chart.Titles.FirstOrDefault().Text = dbLabel.Other2;
                            }
                            else if (dbLabel != null)
                            {
                                chart.Titles.FirstOrDefault().Text = dbLabel.Eng;
                            }
                        }
                        else if (c.GetType() == typeof(ReportViewer))
                        {
                            ReportViewer rvr = c as ReportViewer;
                            rvr.Font = DefaultFont(SettingController.DefaultFont, FORMNAME);
                        }

                    }
                }
            }
        }
    }


    public static class SettingController
    {
        public static Boolean TopMost
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "topmost");
                if (currentSet != null)
                {
                    return Convert.ToBoolean(currentSet.Value);
                }

                return false;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "topmost");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "topmost";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        public static Boolean AllowMinimize
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "allow_minimize");
                if (currentSet != null)
                {
                    return Convert.ToBoolean(currentSet.Value);
                }

                return false;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "allow_minimize");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "allow_minimize";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }

        public static Boolean TicketSale
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "ticketsale");
                if (currentSet != null)
                {
                    return Convert.ToBoolean(currentSet.Value);
                }

                return false;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "ticketsale");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "ticketsale";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        public static Boolean DetectIdle
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "detect_idle");
                if (currentSet != null)
                {
                    return Convert.ToBoolean(currentSet.Value);
                }

                return false;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "detect_idle");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "detect_idle";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        public static int IdleTime
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "idle_Time");
                if (currentSet != null)
                {
                    return Convert.ToInt32(currentSet.Value);
                }

                return 0;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "idle_Time");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "idle_Time";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        public static int ServiceFee
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "service_fee");
                if (currentSet != null)
                {
                    return Convert.ToInt32(currentSet.Value);
                }

                return 0;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "service_fee");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "service_fee";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        public static Boolean UseQueue
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "usequeue");
                if (currentSet != null)
                {
                    return Convert.ToBoolean(currentSet.Value);
                }

                return false;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "usequeue");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "usequeue";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        public static Boolean UseTable
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "usetable");
                if (currentSet != null)
                {
                    return Convert.ToBoolean(currentSet.Value);
                }

                return false;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "usetable");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "usetable";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        public static string InventoryControlPattern
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "retrieve_pattern");
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }


                return string.Empty;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "retrieve_pattern");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "retrieve_pattern";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        public static Boolean AllowDynamicPrice
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "allow_dynamic");
                if (currentSet != null)
                {
                    return Convert.ToBoolean(currentSet.Value);
                }

                return false;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "allow_dynamic");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "allow_dynamic";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        public static Boolean IsBackOffice
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "IsBackOffice");
                if (currentSet != null)
                {
                    return Convert.ToBoolean(Convert.ToInt16(currentSet.Value));
                }

                return false;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "IsBackOffice");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "IsBackOffice";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = Convert.ToInt16(value).ToString();
                }
                entity.SaveChanges();
            }
        }
        public static Boolean ShowProductImageIn_A4Reports
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "a4image");
                if (currentSet != null)
                {
                    return Convert.ToBoolean(currentSet.Value);
                }

                return false;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "a4image");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "a4image";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        public static Boolean UpperCase_ProductName
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "uppercase");
                if (currentSet != null)
                {
                    return Convert.ToBoolean(currentSet.Value);
                }

                return false;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "uppercase");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "uppercase";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        public static Boolean UseCustomSKU
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "customsku");
                if (currentSet != null)
                {
                    return Convert.ToBoolean(currentSet.Value);
                }

                return false;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "customsku");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "customsku";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        public static Boolean UseStockAutoGenerate
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "UseStockAutoGenerate");
                if (currentSet != null)
                {
                    return Convert.ToBoolean(currentSet.Value);
                }

                return false;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "UseStockAutoGenerate");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "UseStockAutoGenerate";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        public static string ApplicationMode
        {
            get
            {
                POSEntities db = new POSEntities();
                var currentMode = db.Settings.Where(s => s.Key == "app_mode").FirstOrDefault();
                if (currentMode != null)
                {
                    return currentMode.Value;
                }
                return string.Empty;
            }
            set
            {
                POSEntities db = new POSEntities();
                var currentMode = db.Settings.Where(a => a.Key == "app_mode").FirstOrDefault();
                if (currentMode == null)
                {
                    currentMode = new APP_Data.Setting();
                    currentMode.Key = "app_mode";
                    currentMode.Value = value.ToString();
                    db.Settings.Add(currentMode);
                }
                else
                {
                    currentMode.Value = value.ToString();
                }
                db.SaveChanges();
            }
        }
        public static string DefaultFont
        {
            get
            {
                POSEntities db = new POSEntities();
                var fontSetting = db.Settings.Where(s => s.Key == "default_font").FirstOrDefault();
                if (fontSetting != null)
                {
                    return fontSetting.Value;
                }
                return string.Empty;
            }
            set
            {
                POSEntities db = new POSEntities();
                var fontSetting = db.Settings.Where(s => s.Key == "default_font").FirstOrDefault();
                if (fontSetting == null)
                {
                    fontSetting = new APP_Data.Setting();
                    fontSetting.Key = "default_font";
                    fontSetting.Value = value.ToString();
                    db.Settings.Add(fontSetting);

                }
                else
                {
                    fontSetting.Value = value.ToString();
                }
                db.SaveChanges();
            }
        }
        public static string FooterPage
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "FooterPage");
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }

                return string.Empty;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "FooterPage");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "FooterPage";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }

        public static string Logo
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "Logo");
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }

                return string.Empty;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "Logo");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "Logo";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }

        public static string ShopName
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "shop_name");
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }

                return string.Empty;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "shop_name");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "shop_name";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }

        public static APP_Data.Shop DefaultShop
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Shop defaultShop = entity.Shops.Where(x => x.IsDefaultShop == true).FirstOrDefault();
                return defaultShop;
            }
        }

        public static string BranchName
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "branch_name");
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }

                return string.Empty;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "branch_name");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "branch_name";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }

        public static string PhoneNo
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "phone_number");
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }

                return string.Empty;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "phone_number");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "phone_number";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }

        public static string OpeningHours
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "opening_hours");
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }

                return string.Empty;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "opening_hours");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "opening_hours";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }

        public static string Email
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "Email");
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }

                return string.Empty;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "Email");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "Email";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }

        public static string Website
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "Website");
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }

                return string.Empty;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "Website");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "Website";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }

        public static string DefaultTaxRate
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "default_tax_rate");
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }

                return string.Empty;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "default_tax_rate");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "default_tax_rate";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }

        public static string SelectDefaultPrinter
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "default_printer");
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }

                return string.Empty;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "default_printer");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "default_printer";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }

        public static int DefaultTopSaleRow
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "default_top_sale_row");
                if (currentSet != null)
                {
                    return Convert.ToInt32(currentSet.Value);
                }

                return 0;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "default_top_sale_row");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "default_top_sale_row";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }

        public static int DefaultCurrency
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "default_currency");
                if (currentSet != null)
                {
                    return Convert.ToInt32(currentSet.Value);
                }
                return 0;
            }

            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentset = entity.Settings.FirstOrDefault(x => x.Key == "default_currency");
                if (currentset == null)
                {
                    currentset = new APP_Data.Setting();
                    currentset.Key = "default_currency";
                    currentset.Value = value.ToString();
                    entity.Settings.Add(currentset);
                }
                else
                {
                    currentset.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }

        public static int DefaultNoOfCopies
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "default_noOfCopies");
                if (currentSet != null)
                {
                    return Convert.ToInt32(currentSet.Value);
                }

                return 1;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "default_noOfCopies");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "default_noOfCopies";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }

        public static string Company_StartDate
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "Company_StartDate");
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }
                return string.Empty;
            }

            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentset = entity.Settings.FirstOrDefault(x => x.Key == "Company_StartDate");
                if (currentset == null)
                {
                    currentset = new APP_Data.Setting();
                    currentset.Key = "Company_StartDate";
                    currentset.Value = value.ToString();
                    entity.Settings.Add(currentset);
                }
                else
                {
                    currentset.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }

        public static int GetExchangeRate(int Id)
        {
            POSEntities entity = new POSEntities();
            Currency currentCurrency = entity.Currencies.FirstOrDefault(x => x.Id == Id);
            if (currentCurrency != null)
            {
                return Convert.ToInt32(currentCurrency.LatestExchangeRate);
            }
            return 0;
        }
        public static void SetExchangeRate(int Id, int value)
        {
            POSEntities entity = new POSEntities();
            Currency currentCurrency = entity.Currencies.FirstOrDefault(x => x.Id == Id);
            currentCurrency.LatestExchangeRate = value;
            entity.SaveChanges();
        }

        public static int DefaultCity
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "default_city_id");
                if (currentSet != null)
                {
                    return Convert.ToInt32(currentSet.Value);
                }
                return 0;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "default_city_id");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "default_city_id";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        public static int POSID
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "pos_id");
                if (currentSet != null)
                {
                    return Convert.ToInt16(currentSet.Value);
                }
                return 0;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "pos_id");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "pos_id";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        public static bool IsSourcecode
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "IsSourcecode");
                if (currentSet != null)
                {
                    return Convert.ToBoolean(currentSet.Value);
                }
                return false;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "IsSourcecode");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "IsSourcecode";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }

        }


        public static string Use
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "default_tax_rate");
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }

                return string.Empty;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "default_tax_rate");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "default_tax_rate";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        public static Boolean IsUsedS3Upload
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "IsUsedS3Upload");
                if (currentSet != null)
                {
                    return Convert.ToBoolean(currentSet.Value);
                }

                return false;
            }
        }
        public static List<string> S3EncryptedKeys
        {
            get
            {
                POSEntities entity = new POSEntities();
                IQueryable<string> currentSet = entity.Settings.Where(x => x.Key == "S3EncryptedKey1" || x.Key == "S3EncryptedKey2").OrderBy(x => x.Key).Select(x => x.Value);
                if (currentSet != null && currentSet.Count() == 2)
                {
                    return currentSet.ToList();
                }

                return null;
            }

        }
        public static string S3BucketName
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "S3BucketName");
                if (currentSet != null)
                {
                    return currentSet.Value;
                }

                return null;
            }
        }

        internal static object CompanyStartDate()
        {
            throw new NotImplementedException();
        }
    }

    public static class DatabaseControlSetting
    {
        public static string _ServerName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["_ServerName"];
            }
        }
        /// <summary>
        /// Get or Set the Database's Name
        /// </summary>
        public static string _DBName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["_DBName"];
            }
        }
        /// <summary>
        /// Get or Set the Database's Login User
        /// </summary>
        public static string _DBUser
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["_DBUser"];
            }
        }
        /// <summary>
        /// Get or Set the Database's Login Password
        /// </summary>
        public static string _DBPassword
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["_DBPassword"];
            }
        }

    }

    public class dailysaleclass
    {
        public string Payment_Type;
        public decimal Total;
    }

    public class RestoreHelper
    {
        public RestoreHelper()
        {

        }

        public void RestoreDatabase(String databaseName, String backUpFile, String serverName, String userName, String password)
        {

            ServerConnection connection = new ServerConnection(serverName, userName, password);
            Server sqlServer = new Server(connection);
            string dbaddr = string.Empty;
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString))
            {
                dbaddr = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            }
            SqlConnection conn = new SqlConnection(dbaddr);
            string s = conn.State.ToString();
            conn.Close();
            SqlConnection.ClearPool(conn);
            sqlServer.KillAllProcesses(databaseName);
            sqlServer.KillDatabase(databaseName);
            Database db = new Database(sqlServer, databaseName);
            db.Create();
            Restore rstDatabase = new Restore();
            rstDatabase.Action = RestoreActionType.Database;
            rstDatabase.Database = databaseName;
            BackupDeviceItem bkpDevice = new BackupDeviceItem(backUpFile, DeviceType.File);
            rstDatabase.Devices.Add(bkpDevice);
            rstDatabase.ReplaceDatabase = true;
            rstDatabase.Complete += new ServerMessageEventHandler(sqlRestore_Complete);
            rstDatabase.PercentCompleteNotification = 10;
            rstDatabase.PercentComplete += new PercentCompleteEventHandler(sqlRestore_PercentComplete);
            rstDatabase.SqlRestore(sqlServer);
            sqlServer.Refresh();
        }

        public event EventHandler<PercentCompleteEventArgs> PercentComplete;

        void sqlRestore_PercentComplete(object sender, PercentCompleteEventArgs e)
        {
            if (PercentComplete != null)
                PercentComplete(sender, e);
        }

        public event EventHandler<ServerMessageEventArgs> Complete;

        void sqlRestore_Complete(object sender, ServerMessageEventArgs e)
        {
            if (Complete != null)
                Complete(sender, e);
        }
    }

    public class BackupHelper
    {
        public BackupHelper()
        {

        }

        public void BackupDatabase(String databaseName, String userName, String password, String serverName, String destinationPath, ref bool isBackUp)
        {
            Backup sqlBackup = new Backup();

            sqlBackup.Action = BackupActionType.Database;
            sqlBackup.BackupSetDescription = "ArchiveDataBase:" + DateTime.Now.ToShortDateString();
            sqlBackup.BackupSetName = "Archive";

            sqlBackup.Database = databaseName;

            BackupDeviceItem deviceItem = new BackupDeviceItem(destinationPath, DeviceType.File);
            ServerConnection connection = new ServerConnection(serverName, userName, password);
            Server sqlServer = new Server(connection);

            Database db = sqlServer.Databases[databaseName];

            sqlBackup.Initialize = true;
            sqlBackup.Checksum = true;
            sqlBackup.ContinueAfterError = true;

            sqlBackup.Devices.Add(deviceItem);
            sqlBackup.Incremental = false;

            sqlBackup.ExpirationDate = DateTime.Now.AddDays(30);
            sqlBackup.LogTruncation = BackupTruncateLogType.Truncate;

            sqlBackup.FormatMedia = false;
            try
            {
                sqlBackup.SqlBackup(sqlServer);
                isBackUp = true;
            }
            catch
            {
                MessageBox.Show("Please check the database if it's properly installed.");
            }
        }
    }

    #region PrintFunctions

    public static class PrintDoc
    {
        private static Boolean isStickerSize = false;
        private static Boolean isSlipSize = false;
        private static Boolean isA4Size = false;
        private static IList<Stream> m_streams;
        private static int m_currentPageIndex;

        #region Printing Functions

        private static void Print()
        {
            try
            {
                if (m_streams == null || m_streams.Count == 0)
                    return;

                PrintDocument printDoc = new PrintDocument();

                if (isStickerSize)
                    printDoc.PrinterSettings.PrinterName = DefaultPrinter.BarcodePrinter;
                else if (isSlipSize)
                    printDoc.PrinterSettings.PrinterName = DefaultPrinter.SlipPrinter;
                else
                    printDoc.PrinterSettings.PrinterName = DefaultPrinter.A4Printer;

                if (!printDoc.PrinterSettings.IsValid)
                {
                    string msg = String.Format("Printing can't be processed!.Can't find printer \"{0}\".", DefaultPrinter.A4Printer);
                    System.Diagnostics.Debug.WriteLine(msg);
                    MessageBox.Show(msg, "Change Printer Setting", MessageBoxButtons.OK);
                    return;
                }
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                printDoc.Print();

                printDoc.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static void PrintReport(ReportViewer rv)
        {
            isStickerSize = false;
            m_currentPageIndex = 0;
            m_streams = null;
            Export(rv.LocalReport);
            Print();
            //  Dispose();
            rv.LocalReport.ReleaseSandboxAppDomain();
        }

        /// <summary>
        /// 
        /// </summary>        
        /// <param name="Type">BarcodeStricker||Slip </param>
        public static void PrintReport(ReportViewer rv, String Type)
        {
            m_currentPageIndex = 0;
            m_streams = null;
            isStickerSize = false; isA4Size = false; isSlipSize = false;
            if (Type == "BarcodeSTicker")
            {
                isStickerSize = true;
            }
            else if (Type == "A4 Printer")
            {
                isA4Size = true;
            }
            else
            {
                isSlipSize = true;
            }

            Export(rv.LocalReport);

            Print();

            //  Dispose();
            rv.LocalReport.ReleaseSandboxAppDomain();
        }

        // Export the given report as an EMF (Enhanced Metafile) file.
        private static void Export(LocalReport report)
        {
            string deviceInfo = string.Empty;
            if (isStickerSize)
            {
                deviceInfo =
                  @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>3in</PageWidth>                
                <MarginTop>0in</MarginTop>
                <MarginLeft>0in</MarginLeft>
                <MarginRight>0in</MarginRight>
                <MarginBottom>0in</MarginBottom>
                </DeviceInfo>";
            }
            else if (isSlipSize)
            {
                deviceInfo =
                  @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>3.1in</PageWidth>                
                <MarginTop>0in</MarginTop>
                <MarginLeft>0in</MarginLeft>
                <MarginRight>0in</MarginRight>
                <MarginBottom>0in</MarginBottom>
            </DeviceInfo>";
            }
            else
            {
                deviceInfo =
                  @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>8in</PageWidth>
                <PageHeight>10.5in</PageHeight>
                <MarginTop>0in</MarginTop>
                <MarginLeft>0in</MarginLeft>
                <MarginRight>0in</MarginRight>
                <MarginBottom>0in</MarginBottom>
            </DeviceInfo>";
            }
            Warning[] warnings;
            m_streams = new List<Stream>();

            report.Refresh();
            var ps = report.GetParameters();

            report.Render("Image", deviceInfo, CreateStream, out warnings);

            foreach (Stream stream in m_streams)
                stream.Position = 0;
        }

        private static void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new Metafile(m_streams[m_currentPageIndex]);
            ev.Graphics.DrawImage(pageImage, ev.PageBounds);
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }

        private static Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }

        #endregion
    }

    public static class ExportReport
    {
        public static void Excel(ReportViewer rv, String FileName)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            byte[] bytes = rv.LocalReport.Render(
               "Excel", null, out mimeType, out encoding,
                out extension,
               out streamids, out warnings);
            try
            {
                FileStream fs = new FileStream(@"D:\Reports\" + FileName + DateTime.Now.ToString("ddMMyyyy") + ".xls", FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
                MessageBox.Show(@"Report file is saved in D\Reports\" + FileName + DateTime.Now.ToString("ddMMyyyy") + ".xls", "Saving Complete");
            }
            catch (DirectoryNotFoundException message)
            {
                MessageBox.Show(@"The file patch (D:\Reports) isn't exist. Please check and create Reports folder in the Drive D", "Error");
            }
        }
    }

    public static class DefaultPrinter
    {
        public static string BarcodePrinter
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "barcode_printer");
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }

                return string.Empty;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "barcode_printer");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "barcode_printer";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }

        public static string A4Printer
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "a4_printer");
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }

                return string.Empty;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "a4_printer");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "a4_printer";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }

        public static string SlipPrinter
        {
            get
            {
                POSEntities entity = new POSEntities();
                string key = "slip_printer_counter" + MemberShip.CounterId.ToString();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == key);
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }

                return string.Empty;
            }
            set
            {
                POSEntities entity = new POSEntities();
                string key = "slip_printer_counter" + MemberShip.CounterId.ToString();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == key);
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = key;
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }


    }

    #endregion
    public class NoveltySaleController
    {
        public string SKUCode;
        public string ProductName;
        public int TotalQty;
        public int TotalAmount;
        public int UnitPrice;
    }

}
