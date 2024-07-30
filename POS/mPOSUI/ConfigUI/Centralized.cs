using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using System.IO.Compression;
using System.Drawing;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.IO;


using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Threading; // Required for this example
using POS.APP_Data;
using System.Text.RegularExpressions;

namespace POS
{
    public partial class Centralized : Form
    {
        public Centralized()
        {
            InitializeComponent();
        }

        private void Centralized_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            label1.Refresh();
        }

        #region EncryptDecrypt

        /// <summary>
        /// Encrypting incomming file.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFile"></param>
        public static void EncryptFile(string inputFile, string outputFile)
        {
            try
            {
                string password = @"mykey123";
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

                File.Delete(inputFile);
            }
            catch
            {
                MessageBox.Show("Encryption failed!", "Error");
            }
        }

        /// <summary>
        /// Decrypting incomming file.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFile"></param>
        public static void DecryptFile(string inputFile, string outputFile)
        {
            try
            {
                string password = @"mykey123";

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

        #endregion

        #region Variable
        POSEntities entity = new POSEntities();
        #endregion

        #region Export
        private void btnExport_Click(object sender, EventArgs e)
        {
            int month = 2;
            if (!string.IsNullOrEmpty(txtmonth.Text.Trim()) && txtmonth.Text.Trim().Length<=2)
            {
                try
                {
                    month = Convert.ToInt16(txtmonth.Text.Trim());
                }
                catch (FormatException fe)
                {
                    MessageBox.Show(this, "Month textbox only allow 1 or 2 digit between 0-9.");
                    return;
                }
            }else
            {
                MessageBox.Show(this, "Month textbox only allow 1 or 2 digit between 0-9.");
                return;
            }
            string connetionString = null;
            SqlConnection connection;
            SqlDataAdapter adapter;
            DataSet dsUserRole= new DataSet("UserRole");
            DataSet dsUser = new DataSet("User");
            DataSet dsBrand = new DataSet("Brand");
            DataSet dsCity = new DataSet("City");
            DataSet dsConsignmentCounter = new DataSet("ConsignmentCounter");
            DataSet dsLCounter = new DataSet("Counter");
            DataSet dsCurrency = new DataSet("Currency");
            DataSet dsMemberType = new DataSet("MemberType");
            DataSet dsPaymentType = new DataSet("PaymentType");
            DataSet dsUnit = new DataSet("Unit");
            DataSet dsTax = new DataSet("Tax");
            DataSet dsProductCategory = new DataSet("ProductCategory");
            DataSet dsProductSubCategory = new DataSet("ProductSubCategory");
            DataSet dsExpenseCategory = new DataSet("ExpenseCategory");
        
            DataSet dsRoleManagement = new DataSet("RoleManagement");
           // DataSet dsAdjustment = new DataSet("Adjustment");
            DataSet dsExpense = new DataSet("Expense");
            DataSet dsExpenseDetail = new DataSet("ExpenseDetail");
            DataSet dsMemberCardRuler = new DataSet("MemberCardRule");
            DataSet dsCustomer = new DataSet("Customer");
            DataSet dsProduct = new DataSet("Product");
            DataSet dsProductPriceChange = new DataSet("ProductPriceChange");
            DataSet dsProductQuantityChange = new DataSet("ProductQuantityChange");
            DataSet dsWrapperItem = new DataSet("WrapperItem");
            DataSet dsUnitConversion = new DataSet("UnitConversion");
           
            DataSet dsPurchaseDetail = new DataSet("PurchaseDetail");
            DataSet dsPurchaseDetailInTransaction = new DataSet("PurchaseDetailInTransaction");
            DataSet dsPurchaseDeleteLog = new DataSet("PurchaseDeleteLog");
            DataSet dsTransaction = new DataSet("Transaction");
            DataSet dsTransactionDetail = new DataSet("TransactionDetail");
            DataSet dsConsignmentSettlement = new DataSet("ConsignmentSettlement");
            DataSet dsConsignmentSettlementDetail = new DataSet("ConsignmentSettlementDetail");
            DataSet dsExchangeRateForTransaction = new DataSet("ExchangeRateForTransaction");
            DataSet dsSPDetail = new DataSet("SPDetail");
            DataSet dsUsePrePaidDebt = new DataSet("UsePrePaidDebt");

            DateTime today = DateTime.Today;
            DateTime TranDate = DateTime.Today.AddMonths(-month);


            string dtToday = today.ToString("yyyy-MM-dd");
            string dtTranDate = TranDate.ToString("yyyy-MM-dd");

            connetionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            connection = new SqlConnection(connetionString);

            string SqlUserRole = "Select * from UserRole";
            string SqlUser = "Select * from [User]";
            string sqlBrand = "select * from Brand";
            string sqlCity = "Select * from City";
            string sqlConsignmentCounter = "Select c.Id,C.Name,C.CounterLocation,c.PhoneNo,c.Email,c.Address,isnull(c.IsDelete,0) as IsDelete from ConsignmentCounter as C";
            string SqlCounter = "Select * from Counter";
            string SqlCurrency = "Select * from Currency";
            string SqlMemberType = "Select * from MemberType";
            string SqlPaymentType = "Select * from PaymentType";
            string SqlGiftCard = "Select * from GiftCard ";
            string SqlUnit = "Select * from Unit";
            string SqlTax = "Select * from Tax";
            string SqlProductCategory = "Select * from ProductCategory";
            string SqlProductSubCategory = "Select PS.*, PC.Name as CName from ProductSubCategory as PS inner join ProductCategory as PC on PS.ProductCategoryId = PC.Id";
            string SqlExpenseCategory = "Select * from ExpenseCategory";
            string SqlExpense = "Select * from Expense where IsApproved=1 and cast(ExpenseDate as date) >= Cast('" + dtTranDate + "' as date) and cast(ExpenseDate as date) <= Cast('" + dtToday + "' as date)";
            string SqlExpenseDetail = "Select * from ExpenseDetail ED Inner join Expense AS E on E.Id =ED.ExpenseId where E.IsApproved=1 and cast(ExpenseDate as date) >= Cast('" + dtTranDate + "' as date) and cast(ExpenseDate as date) <= Cast('" + dtToday + "' as date)";
            string SqlMemberCardRule = "Select * from MemberCardRule";
            string sqlCustomer = "select Cus.*,isnull(Cus.Birthday,' ') as Birthday, C.CityName from Customer as Cus left join City as C on Cus.CityId = C.Id";
            string sqlProduct = "select P.ID,P.CreatedBy as CreatedBy1,P.UpdatedBy as UpdatedBy1,P.UpdatedDate,ISNULL(P.Name,'') AS Name,ProductCode,BarCode,Price,ISNULL(Qty,0) AS Qty,ISNULL(BrandId,0) As BrandId, ISNULL(ProductLocation,'') As ProductLocation,ISNULL(P.ProductCategoryId,0) As ProductCategoryId,ISNULL(ProductSubCategoryId,0) As ProductSubCategoryId ,ISNULL(UnitId,0) As UnitId,ISNULL(TaxId,0) As TaxId,ISNULL(MinStockQty,0) As MinStockQty,ISNULL(DiscountRate,0.00) As DiscountRate, ISNULL(IsWrapper,'false') As IsWrapper,ISNULL(IsConsignment,'false') As IsConsignment,ISNULL(IsDiscontinue,'false') As IsDiscontinue ,ISNULL(ConsignmentPrice,0) As ConsignmentPrice, ISNULL(ConsignmentCounterId,0) As ConsignmentCounterId,ISNULL(Size,'') As Size,ISNULL(PurchasePrice,0) As PurchasePrice,ISNULL(IsNotifyMinStock,'false')  As IsNotifyMinStock, B.Name as BName, PC.Name as CName, ISNULL(PS.Name,'') as SCName, ISNuLL(T.Name, '') as TaxName   from Product as P left join Brand as B on P.BrandId = B.Id left join ProductCategory as PC on P.ProductCategoryId = PC.Id left join ProductSubCategory   as PS on P.ProductSubCategoryId = PS.Id left join Tax as T on T.Id = P.TaxId";
            string SqlProductPriceChange = "Select PC.*, U.Name as UName, P.ProductCode from ProductPriceChange as PC inner join Product as P on PC.ProductId = P.Id inner join [User] as U on U.Id = PC.UserID";          
            string SqlWrapperItem = "Select W.*, P1.ProductCode as ParentProductCode, p2.ProductCode as ChildProductCode from WrapperItem as W inner join Product as P1 on p1.Id = W.ParentProductId inner join Product as P2 on p2.Id = w.ChildProductId";
            string sqlTransaction = "select T.*,ISNULL(T.Note,'') As Note,ISNULL(T.ParentId,'') As ParentID1,ISNULL(T.TranVouNos,'') As TranVouNos1,T.GiftCardId,T.CustomerId,T.ReceivedCurrencyId,T.ShopId, C.Address, C.Email, C.PhoneNumber, C.CustomerCode, Co.Name as CounterName, U.UserCodeNo as UserCodeNo, S.ShopName as ShopName, S.Address as SAddress,  G.CardNumber from [Transaction] as T left join Customer as C on T.CustomerId = C.Id inner join Counter Co on Co.Id = T.CounterId   inner join [User] as U on T.UserId = U.Id left join Shop as S on S.Id = T.ShopId  left join GiftCard G on G.Id = T.GiftCardId   WHERE T.IsComplete=1  and cast(T.UpdatedDate as date) >= Cast('" + dtTranDate + "' as date) and cast(T.UpdatedDate as date) <= Cast('" + dtToday + "' as date)";
            string sqlTransactionDetail = "select TD.*,ISNULL(Td.IsConsignmentPaid,'') As IsConsignmentPaid1, P.ProductCode , IsNull(ConsignmentNo,'') As ConsignmentNo from TransactionDetail as TD inner join Product as P on TD.ProductId= P.Id inner join [Transaction] as t on t.Id=TD.TransactionId left join ConsignmentSettlementDetail csd on csd.TransactionDetailId=td.Id where t.IsComplete=1 and cast(t.UpdatedDate as date) >= Cast( '" + dtTranDate + "' as date) and cast(t.UpdatedDate as date) <= Cast('" + dtToday + "' as date)";
            string SqlExchangeRateForTransaction = "Select * from ExchangeRateForTransaction E inner join [Transaction] as T on T.Id=E.TransactionId where Cast(T.UpdatedDate as date)  >= Cast('" + dtTranDate + "' as date) and cast(T.UpdatedDate as date) <= Cast('" + dtToday + "' as date) ";
            string SqlSPDetail = "Select SP.*, P1.ProductCode as ParentProductCode, P2.ProductCode as ChildProductCode, T.Id as TID from SPDetail as SP inner join Product as P1 on p1.Id = SP.ParentProductId inner join Product as P2 on p2.Id = SP.ChildProductId inner join TransactionDetail as Td on Td.Id = SP.TransactionDetailID inner join [Transaction] as T on T.Id = Td.TransactionId where Cast(T.UpdatedDate as date)  >= Cast('" + dtTranDate + "' as date) and cast(T.UpdatedDate as date) <= Cast('" + dtToday + "' as date) ";
            string SqlUsePrePaidDebt = "Select U.*, C.Name as CounterName, Us.Name as UName from UsePrePaidDebt as U inner join Counter as C on U.CounterId = C.Id inner join [User] as Us on U.CashierId = Us.Id";
            string SqlDeletelog = "Select DL.*, C.Name as CounterName, U.Name as UName from Deletelog as DL inner join Counter as C on DL.CounterId = C.Id inner join [User] as U on DL.UserId = U.Id inner join [Transaction] as T on T.Id=DL.TransactionId where Cast(T.UpdatedDate as date)  >= Cast('" + dtTranDate + "' as date) and cast(T.UpdatedDate as date) <= Cast('" + dtToday + "' as date) ";
            string SqlConsignmentSettlement = "Select CS.*,U.UserCodeNo as UserCodeNo from ConsignmentSettlement as CS Inner join [User] as U on U.Id=CS.CreatedBy Where  cast(SettlementDate as date) >= Cast('" + dtTranDate + "' as date) and cast(SettlementDate as date) <= Cast('" + dtToday + "' as date) ";
            string SqlConsignmentSettlementDetail = "select csd.* from ConsignmentSettlementDetail csd inner join ConsignmentSettlement cs on cs.ConsignmentNo=csd.ConsignmentNo Where cast(SettlementDate as date) >= Cast('" + dtTranDate + "' as date) and cast(SettlementDate as date) <= Cast('" + dtToday + "' as date) ";
            string SqlShop = "Select S.*,ISNULL(S.Address,'') as Address,ISNULL(S.PhoneNumber,'') as PhoneNumber,ISNULL(S.OpeningHours,'') as OpeningHours, C.CityName from Shop as S left join City as C on S.CityId = C.Id";
            //stock in,out
            string SqlStockInHeader = "Select * from StockInHeader where IsDelete=0 and IsApproved = 1 and  Status <> 'StockIn' and Cast(Date as date)  >= Cast('" + dtTranDate + "' as date) and cast(Date as date) <= Cast('" + dtToday + "' as date)  ";
            string SqlStockInDetail = "select SD.*, P.ProductCode, SH.StockCode ,SH.ToShopId  from StockInDetail as SD inner join Product as P on SD.ProductId= P.Id inner join StockInHeader as SH on SH.Id=SD.StockInHeaderId where IsDelete=0 and IsApproved = 1 and Status <> 'StockIn' and Cast(Date as date)  >= Cast('" + dtTranDate + "' as date) and cast(Date as date) <= Cast('" + dtToday + "' as date)  ";

            //promotion system 
            string SqlGiftSystem = "SELECT * FROM GiftSystem where  Cast(ValidFrom as date)  >= Cast('" + dtTranDate + "' as date) and cast(ValidTo as date) <= Cast('" + dtToday + "' as date) ";
            string SqlGiftSystemInTransaction = "select * from GiftSystemInTransaction";
            string sqlGiftCardInTransaction = "select * from GiftCardInTransaction";  
            //NoveltySystem
            string SqlNoveltySystem = "select * from NoveltySystem";
            string sqlProductInNovelty = "select * from ProductInNovelty where IsDeleted=0";  
            try {
                connection.Open();
                //Create combination data main dataset.
                DataSet dsCombineData = new DataSet("ExportData");

                DataTable UserRole = new DataTable("UserRole");
                DataTable User = new DataTable("User");
                DataTable Brand = new DataTable("Brand");
                DataTable City = new DataTable("City");
                DataTable ConsignmentCounter = new DataTable("ConsignmentCounter");
                DataTable Counter = new DataTable("Counter");
                DataTable Currency = new DataTable("Currency");
                DataTable MemberType = new DataTable("MemberType");
                DataTable PaymentType = new DataTable("PaymentType");
                DataTable GiftCard = new DataTable("GiftCard");
                DataTable Unit = new DataTable("Unit");
                DataTable Tax = new DataTable("Tax");
                DataTable ProductCategory = new DataTable("ProductCategory");
                DataTable ProductSubCategory = new DataTable("ProductSubCategory");
                DataTable ExpenseCategory = new DataTable("ExpenseCategory");
           
                DataTable RoleManagement = new DataTable("RoleManagement");
           
                DataTable Expense = new DataTable("Expense");
                DataTable ExpenseDetail = new DataTable("ExpenseDetail");
                DataTable MemberCardRule = new DataTable("MemberCardRule");
                DataTable Customer = new DataTable("Customer");
                DataTable Product = new DataTable("Product");
                DataTable ProductPriceChange = new DataTable("ProductPriceChange");
                DataTable ProductQuantityChange = new DataTable("ProductQuantityChange");
                DataTable WrapperItem = new DataTable("WrapperItem");
          
                DataTable Transaction = new DataTable("Transaction");
                DataTable TransactionDetail = new DataTable("TransactionDetail");
                DataTable ExchangeRateForTransaction = new DataTable("ExchangeRateForTransaction");
                DataTable SPDetail = new DataTable("SPDetail");
                DataTable UsePrePaidDebt = new DataTable("UsePrePaidDebt");
                DataTable DeleteLog = new DataTable("DeleteLog");
                DataTable ConsignmentSettlement = new DataTable("ConsignmentSettlement");

                DataTable ConsignmentSettlementDetail = new DataTable("ConsignmentSettlementDetail");
         
                DataTable Shop = new DataTable("Shop");

                DataTable StockInHeader = new DataTable("StockInHeader");
                DataTable StockInDetail = new DataTable("StockInDetail");
                //promotion system db
                DataTable GiftSystem = new DataTable("GiftSystem");
                DataTable GiftSystemInTransaction = new DataTable("GiftSystemInTransaction");
                DataTable GiftCardInTransaction = new DataTable("GiftCardInTransaction");
                //Novelty system
                DataTable NoveltySystem = new DataTable("NoveltySystem");
                DataTable ProductInNovelty = new DataTable("ProductInNovelty");

                //filling the records to each datatables.
                adapter = new SqlDataAdapter(SqlUserRole, connection);
                adapter.Fill(UserRole);
                adapter = new SqlDataAdapter(SqlUser, connection);
                adapter.Fill(User);
                adapter = new SqlDataAdapter(sqlBrand, connection);
                adapter.Fill(Brand);
                adapter = new SqlDataAdapter(sqlCity, connection);
                adapter.Fill(City);
                adapter = new SqlDataAdapter(sqlConsignmentCounter, connection);
                adapter.Fill(ConsignmentCounter);
                adapter = new SqlDataAdapter(SqlCounter, connection);
                adapter.Fill(Counter);
                adapter = new SqlDataAdapter(SqlCurrency, connection);
                adapter.Fill(Currency);
                adapter = new SqlDataAdapter(SqlMemberType, connection);
                adapter.Fill(MemberType);
                adapter = new SqlDataAdapter(SqlPaymentType, connection);
                adapter.Fill(PaymentType);
                adapter = new SqlDataAdapter(SqlGiftCard, connection);
                adapter.Fill(GiftCard);
                adapter = new SqlDataAdapter(SqlUnit, connection);
                adapter.Fill(Unit);
                adapter = new SqlDataAdapter(SqlTax, connection);
                adapter.Fill(Tax);
                adapter = new SqlDataAdapter(SqlProductCategory, connection);
                adapter.Fill(ProductCategory);
                adapter = new SqlDataAdapter(SqlProductSubCategory, connection);
                adapter.Fill(ProductSubCategory);
                adapter = new SqlDataAdapter(SqlExpenseCategory, connection);
                adapter.Fill(ExpenseCategory);
           
              
                adapter = new SqlDataAdapter(SqlExpense, connection);
                adapter.Fill(Expense);
                adapter = new SqlDataAdapter(SqlExpenseDetail, connection);
                adapter.Fill(ExpenseDetail);
                adapter = new SqlDataAdapter(SqlMemberCardRule, connection);
                adapter.Fill(MemberCardRule);
                adapter = new SqlDataAdapter(sqlCustomer, connection);
                adapter.Fill(Customer);
                adapter = new SqlDataAdapter(sqlProduct, connection);
                adapter.Fill(Product);
                adapter = new SqlDataAdapter(SqlProductPriceChange, connection);
                adapter.Fill(ProductPriceChange);
             
                adapter = new SqlDataAdapter(SqlWrapperItem, connection);
                adapter.Fill(WrapperItem);
            
                adapter = new SqlDataAdapter(sqlTransaction, connection);
                adapter.Fill(Transaction);
                adapter = new SqlDataAdapter(sqlTransactionDetail, connection);
                adapter.Fill(TransactionDetail);
                adapter = new SqlDataAdapter(SqlExchangeRateForTransaction, connection);
                adapter.Fill(ExchangeRateForTransaction);
                adapter = new SqlDataAdapter(SqlSPDetail, connection);
                adapter.Fill(SPDetail);
                adapter = new SqlDataAdapter(SqlUsePrePaidDebt, connection);
                adapter.Fill(UsePrePaidDebt);
                adapter = new SqlDataAdapter(SqlDeletelog, connection);
                adapter.Fill(DeleteLog);
                adapter = new SqlDataAdapter(SqlConsignmentSettlement, connection);
                adapter.Fill(ConsignmentSettlement);

                adapter = new SqlDataAdapter(SqlConsignmentSettlementDetail, connection);
                adapter.Fill(ConsignmentSettlementDetail);

                adapter = new SqlDataAdapter(SqlShop, connection);
                adapter.Fill(Shop);

                adapter = new SqlDataAdapter(SqlStockInHeader, connection);
                adapter.Fill(StockInHeader);

                adapter = new SqlDataAdapter(SqlStockInDetail, connection);
                adapter.Fill(StockInDetail);

                //promotion system
                adapter = new SqlDataAdapter(SqlGiftSystem, connection);
                adapter.Fill(GiftSystem);
                adapter = new SqlDataAdapter(SqlGiftSystemInTransaction, connection);
                adapter.Fill(GiftSystemInTransaction);
                adapter = new SqlDataAdapter(sqlGiftCardInTransaction, connection);
                adapter.Fill(GiftCardInTransaction); 

                //Novelty system
                adapter = new SqlDataAdapter(SqlNoveltySystem, connection);
                adapter.Fill(NoveltySystem);
                adapter = new SqlDataAdapter(sqlProductInNovelty, connection);
                adapter.Fill(ProductInNovelty);


                //data combine the main datables.
                dsCombineData.Tables.Add(UserRole);
                dsCombineData.Tables.Add(User);
                dsCombineData.Tables.Add(Brand);
                dsCombineData.Tables.Add(City);
                dsCombineData.Tables.Add(ConsignmentCounter);
                dsCombineData.Tables.Add(Counter);
                dsCombineData.Tables.Add(Currency);
                dsCombineData.Tables.Add(MemberType);
                dsCombineData.Tables.Add(PaymentType);
                dsCombineData.Tables.Add(GiftCard);
                dsCombineData.Tables.Add(Unit);
                dsCombineData.Tables.Add(Tax);
                dsCombineData.Tables.Add(ProductCategory);
                dsCombineData.Tables.Add(ProductSubCategory);
                dsCombineData.Tables.Add(ExpenseCategory);
              
                dsCombineData.Tables.Add(RoleManagement);
              
                dsCombineData.Tables.Add(Expense);
                dsCombineData.Tables.Add(ExpenseDetail);
                dsCombineData.Tables.Add(MemberCardRule);
                dsCombineData.Tables.Add(Customer);
                dsCombineData.Tables.Add(Product);
                dsCombineData.Tables.Add(ProductPriceChange);
                dsCombineData.Tables.Add(ProductQuantityChange);
                dsCombineData.Tables.Add(WrapperItem);
                dsCombineData.Tables.Add(Transaction);
                dsCombineData.Tables.Add(TransactionDetail);
                dsCombineData.Tables.Add(ExchangeRateForTransaction);
                dsCombineData.Tables.Add(SPDetail);
                dsCombineData.Tables.Add(UsePrePaidDebt);
                dsCombineData.Tables.Add(DeleteLog);
                dsCombineData.Tables.Add(ConsignmentSettlement);
                dsCombineData.Tables.Add(ConsignmentSettlementDetail);
                dsCombineData.Tables.Add(Shop);

                dsCombineData.Tables.Add(StockInHeader);
                dsCombineData.Tables.Add(StockInDetail);
                //promotion system data export.
                dsCombineData.Tables.Add(GiftSystem);
                dsCombineData.Tables.Add(GiftSystemInTransaction);
                dsCombineData.Tables.Add(GiftCardInTransaction);

                //Novelty system data export.
                dsCombineData.Tables.Add(NoveltySystem);
                dsCombineData.Tables.Add(ProductInNovelty);



                //Create path to save
                string activeDir = @"d:\";
                if(!System.IO.Directory.Exists(activeDir))
                {
                     activeDir = @"e:\";
                }
                string newPath = System.IO.Path.Combine(activeDir, "POS_Export");

                if (!System.IO.Directory.Exists(newPath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(newPath);
                }

                //Create File name
                string fileName = newPath+"/" + Environment.MachineName + "_POS_ExportFile[" + DateTime.Now.ToString("dd-MM-yyyy hh-mm tt") + "].xml";
                dsCombineData.WriteXml(fileName);
                string[] encryptFileNameArr = fileName.Split('.');
                string tempFileName = encryptFileNameArr[0] + ".sc";

                //Encrypt File and delete original file
                EncryptFile(fileName, tempFileName);

                connection.Close();

                MessageBox.Show("Done, file saved to " + tempFileName,"infromation :) ",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region Import
        private void btnImport_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure that you want to import the data?", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                ofdImportFile.Filter = "Data Files (*.sc)|*.sc";
                if (ofdImportFile.ShowDialog(this) == DialogResult.Cancel)
                {
                    return;
                }
                #region Get xml filename and decrypt

                string fileName = ofdImportFile.FileName;
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
                destFileName = destFileName + ".xml";
                if (File.Exists(destFileName)) File.Delete(destFileName);

                DecryptFile(fileName, destFileName);

                #endregion

                APP_Data.POSEntities entity = new APP_Data.POSEntities();

                //reading XML file and storing it's data to dataset.
                DataSet dsxml = new DataSet();
                dsxml.ReadXml(destFileName);
                DataTable dtxmlUserRole = dsxml.Tables["UserRole"];
                DataTable dtxmlUser = dsxml.Tables["User"];
                DataTable dtxmlBrand = dsxml.Tables["Brand"];
                DataTable dtxmlCity = dsxml.Tables["City"];
                DataTable dtxmlConsignmentCounter = dsxml.Tables["ConsignmentCounter"];
                DataTable dtxmlCounter = dsxml.Tables["Counter"];
                DataTable dtxmlCurrency = dsxml.Tables["Currency"];
                DataTable dtxmlMemberType = dsxml.Tables["MemberType"];
                DataTable dtxmlPaymentType = dsxml.Tables["PaymentType"];
                DataTable dtxmlGiftCard = dsxml.Tables["GiftCard"];
                DataTable dtxmlUnit = dsxml.Tables["Unit"];
                DataTable dtxmlTax = dsxml.Tables["Tax"];
                DataTable dtxmlProductCategory = dsxml.Tables["ProductCategory"];
                DataTable dtxmlProductSubCategory = dsxml.Tables["ProductSubCategory"];
                DataTable dtxmlExpenseCategory = dsxml.Tables["ExpenseCategory"];
                DataTable dtxmlAdjustmentType = dsxml.Tables["AdjustmentType"];
                DataTable dtxmlSupplier = dsxml.Tables["Supplier"];
                DataTable dtxmlRoleManagement = dsxml.Tables["RoleManagement"];
                //  DataTable dtxmlSetting = dsxml.Tables["Setting"];
                DataTable dtxmlAdjustment = dsxml.Tables["Adjustment"];
                DataTable dtxmlExpense = dsxml.Tables["Expense"];
                DataTable dtxmlExpenseDetail = dsxml.Tables["ExpenseDetail"];
                DataTable dtxmlMemberCardRule = dsxml.Tables["MemberCardRule"];
                DataTable dtxmlCustomer = dsxml.Tables["Customer"];
                DataTable dtxmlProduct = dsxml.Tables["Product"];
                DataTable dtxmlProductPriceChange = dsxml.Tables["ProductPriceChange"];
                DataTable dtxmlProductQuantityChange = dsxml.Tables["ProductQuantityChange"];
                DataTable dtxmlWrapperItem = dsxml.Tables["WrapperItem"];
                DataTable dtxmlUnitConversion = dsxml.Tables["UnitConversion"];
                DataTable dtxmlTransaction = dsxml.Tables["Transaction"];
                DataTable dtxmlTransactionDetail = dsxml.Tables["TransactionDetail"];
                DataTable dtxmlExchangeRateForTransaction = dsxml.Tables["ExchangeRateForTransaction"];
                DataTable dtxmlSPDetail = dsxml.Tables["SPDetail"];
                DataTable dtxmlUserPrePaidDebt = dsxml.Tables["UsePrePaidDebt"];
                DataTable dtxmlDeleteLog = dsxml.Tables["DeleteLog"];
                DataTable dtxmlConsignmentSettlement = dsxml.Tables["ConsignmentSettlement"];
                DataTable dtxmlConsignmentSettlementDetail = dsxml.Tables["ConsignmentSettlementDetail"];
                DataTable dtxmlShop = dsxml.Tables["Shop"];
                DataTable dtxmlStockInHeader = dsxml.Tables["StockInHeader"];
                DataTable dtxmlStockInDetail = dsxml.Tables["StockInDetail"];

                DataTable dtxmlGiftSystem = dsxml.Tables["GiftSystem"];
                DataTable dtxmlGiftSystemInTransaction = dsxml.Tables["GiftSystemInTransaction"];
                DataTable dtxmlGiftCardInTransaction = dsxml.Tables["GiftCardInTransaction"];

                DataTable dtxmlNoveltySystem = dsxml.Tables["NoveltySystem"];
                DataTable dtxmlProductInNovelty = dsxml.Tables["ProductInNovelty"];

                #region UserRole

                entity = new APP_Data.POSEntities();

                if (dtxmlUserRole != null)
                {
                    //loop through dataRow from xml and check if the UserRole is already exist or newone.
                    label1.Text = "Step 1 of 40 : Processing User Role table please wait!";
                    label1.Refresh();
                    Progressbar1.Minimum = 1;
                    Progressbar1.Maximum = dtxmlUserRole.Rows.Count;
                    Progressbar1.Value = 1;
                    Progressbar1.Step = 1;
                    foreach (DataRow dataFromXML in dtxmlUserRole.Rows)
                    {
                        Progressbar1.PerformStep();

                        bool SameRowExit = false; 
                        string RoleName = dataFromXML["RoleName"].ToString();
                        int Id  =Convert.ToInt32( dataFromXML["Id"]);
                        APP_Data.UserRole FoundUserRole = entity.UserRoles.Where(x => x.Id ==Id ).FirstOrDefault();
                        //same User Role found
                        if (FoundUserRole != null)
                        {
                            SameRowExit = true;
                        }
                        //Found same row,Update the Current One
                        if (SameRowExit)
                        {
                            GetUserRoleFromXML(dataFromXML, FoundUserRole);
                            entity.SaveChanges();

                            UserRole_ForeignKeyTBL(dataFromXML,dtxmlUser,dtxmlRoleManagement,FoundUserRole);
                        }
                        //FoundUserRole is not exist in the current database
                        //add new row
                        else
                        {
                            APP_Data.UserRole userRole = new APP_Data.UserRole();
                            GetUserRoleFromXML(dataFromXML, userRole); ;
                            entity.UserRoles.Add(userRole);
                            entity.SaveChanges();

                            UserRole_ForeignKeyTBL(dataFromXML, dtxmlUser, dtxmlRoleManagement, userRole);
                        }
                    }
                }


                #endregion

                #region User

                entity = new APP_Data.POSEntities();
                if (dtxmlUser != null)
                {
                    label1.Text = "Step 2 of 40 : Processing User table please wait!";
                    label1.Refresh();
                    Progressbar1.Minimum = 1;
                    Progressbar1.Maximum = dtxmlUser.Rows.Count;
                    Progressbar1.Value = 1;
                    Progressbar1.Step = 1;
                    //loop through dataRow from xml and check if the User is already exist or newone.
                    foreach (DataRow dataFromXML in dtxmlUser.Rows)
                    {
                        Progressbar1.PerformStep();
                        bool SameRowExit = false;
                       // string Name = dataFromXML["Name"].ToString();
                        //int UserRoleId = Convert.ToInt32(dataFromXML["UserRoleId"].ToString());
                        string UserCodeNo = dataFromXML["UserCodeNo"].ToString();
                        APP_Data.User FoundUser = entity.Users.Where(x => x.UserCodeNo == UserCodeNo ).FirstOrDefault();//&& x.ShopId==SettingController.DefaultShop.Id
                        //same User Role found
                        if (FoundUser != null)
                        {
                            SameRowExit = true;
                        }
                        //Found same row,Update the Current One
                        if (SameRowExit)
                        {
                            GetUserFromXML(dataFromXML, FoundUser);
                            entity.SaveChanges();

                         
                            User_ForeignKeyTBL(dataFromXML, dtxmlConsignmentSettlement, dtxmlDeleteLog, dtxmlExpense, dtxmlProduct, dtxmlProductPriceChange, dtxmlProductQuantityChange, dtxmlStockInHeader,
                                dtxmlTransaction, dtxmlUnitConversion, dtxmlUserPrePaidDebt, FoundUser);
                          
                        }
                        //FoundUserRole is not exist in the current database
                        //add new row
                        else
                        {
                            APP_Data.User user = new APP_Data.User();
                            GetUserFromXML(dataFromXML, user); ;
                            entity.Users.Add(user);
                            entity.SaveChanges();

                            User_ForeignKeyTBL(dataFromXML, dtxmlConsignmentSettlement, dtxmlDeleteLog, dtxmlExpense, dtxmlProduct, dtxmlProductPriceChange, dtxmlProductQuantityChange, dtxmlStockInHeader,
                              dtxmlTransaction, dtxmlUnitConversion, dtxmlUserPrePaidDebt, user);
                        }
                    }
                }

                #endregion

                #region Brand

                entity = new APP_Data.POSEntities();
                //Loop through dataRow come from xml and check if the brnad is already exist or brand is new one
                if (dtxmlBrand != null)
                {
                    label1.Text = "Step 3 of 40 : Processing Line table please wait!";
                    label1.Refresh();
                    Progressbar1.Minimum = 1;
                    Progressbar1.Maximum = dtxmlBrand.Rows.Count;
                    Progressbar1.Value = 1;
                    Progressbar1.Step = 1;

                    foreach (DataRow dataRowFromXML in dtxmlBrand.Rows)
                    {
                        Progressbar1.PerformStep();
                        Boolean sameRowExist = false;
                    
                        int _brandId=Convert.ToInt32( dataRowFromXML["Id"].ToString());
                        APP_Data.Brand FoundBrand = entity.Brands.Where(x => x.Id == _brandId).FirstOrDefault();
                        //same brand name found
                        if (FoundBrand != null)
                        {
                            sameRowExist = true;
                        }
                        //Found same row,Update the Current One
                        if (sameRowExist)
                        {
                            GetBrandFromXML(dataRowFromXML, FoundBrand);
                            entity.SaveChanges();

                            Brand_ForeignKeyTBL(dataRowFromXML, dtxmlProduct, FoundBrand);
                           
                        }
                        //brand name is not exist in the current database
                        //add new row
                        else
                        {
                            APP_Data.Brand brand = new APP_Data.Brand();
                            GetBrandFromXML(dataRowFromXML, brand);
                            entity.Brands.Add(brand);
                            entity.SaveChanges();

                            Brand_ForeignKeyTBL(dataRowFromXML, dtxmlProduct, brand);
                     
                        
                        }
                    }
                }
                #endregion

                #region City
                entity = new APP_Data.POSEntities();
                //loop through dataRow come from xml and check if the city is already exist or brand is new one
                if (dtxmlCity != null)
                {
                    label1.Text = "Step 4 of 40 : Processing City table please wait!";
                    label1.Refresh();
                    Progressbar1.Minimum = 1;
                    Progressbar1.Maximum = dtxmlCity.Rows.Count;
                    Progressbar1.Value = 1;
                    Progressbar1.Step = 1;

                    foreach (DataRow dataRowFromXML in dtxmlCity.Rows)
                    {

                        Progressbar1.PerformStep();
                        Boolean sameRowExist = false;
                        int _cityId = Convert.ToInt32(dataRowFromXML["Id"].ToString());
                        string cityname=dataRowFromXML["CityName"].ToString();
                        APP_Data.City FoundCity = entity.Cities.Where(x => x.Id == _cityId).FirstOrDefault();
                        //found same ctiy name
                        if (FoundCity != null)
                        {
                            sameRowExist = true;
                        }
                        //Found same row ,Update the Current One
                        if (sameRowExist)
                        {
                            GetCityFromXML(dataRowFromXML, FoundCity);
                            entity.SaveChanges();

                            City_ForeignKeyTBL(dataRowFromXML, dtxmlCustomer, dtxmlShop, FoundCity);
                        }
                        //City name is not exist in current database
                        //add new row
                        else
                        {
                            APP_Data.City city = new APP_Data.City();
                            GetCityFromXML(dataRowFromXML, city);
                            entity.Cities.Add(city);
                            entity.SaveChanges();

                            City_ForeignKeyTBL(dataRowFromXML, dtxmlCustomer, dtxmlShop, FoundCity);
                        }
                    }
                }
                #endregion

                #region Consigment Counter

                entity = new APP_Data.POSEntities();
                if (dtxmlConsignmentCounter != null)
                {
                    label1.Text = "Step 5 of 40 : Processing Consignment Counter table please wait!";
                    label1.Refresh();
                    Progressbar1.Minimum = 1;
                    Progressbar1.Maximum = dtxmlConsignmentCounter.Rows.Count;
                    Progressbar1.Value = 1;
                    Progressbar1.Step = 1;
                    //loop through dataRow come form xml and check if the Consigment Counter is already exist or Consigment Counter is new one
                    foreach (DataRow dataRowFromXML in dtxmlConsignmentCounter.Rows)
                    {

                        Progressbar1.PerformStep();
                        Boolean sameRowExist = false;
                        int id =Convert.ToInt32(dataRowFromXML["Id"].ToString());
                       
                        APP_Data.ConsignmentCounter FoundConsignmentCounter = entity.ConsignmentCounters.Where(x =>  x.Id == id).FirstOrDefault();
                        //fount same consignment counter
                        if (FoundConsignmentCounter != null)
                        {
                            sameRowExist = true;
                        }
                        //Found same row ,Update the Current One
                        if (sameRowExist)
                        {
                            GetConsignmentCounterFromXML(dataRowFromXML, FoundConsignmentCounter);
                            entity.SaveChanges();

                            ConsignmentCounter_ForeignKeyTBL(dataRowFromXML, dtxmlProduct,dtxmlConsignmentSettlement ,FoundConsignmentCounter);

                        }
                        //Consigment Counter is not exist in current database
                        //add new row
                        else
                        {
                            APP_Data.ConsignmentCounter consignmentCounter = new APP_Data.ConsignmentCounter();
                            GetConsignmentCounterFromXML(dataRowFromXML, consignmentCounter);
                            entity.ConsignmentCounters.Add(consignmentCounter);
                            entity.SaveChanges();

                            ConsignmentCounter_ForeignKeyTBL(dataRowFromXML, dtxmlProduct, dtxmlConsignmentSettlement, consignmentCounter);
                        }
                    }
                }

                #endregion

                #region Counter

                entity = new APP_Data.POSEntities();
                //Loop through dataRow come from xml and check if the Counter is already exist or Counter is new one
                if (dtxmlCounter != null)
                {
                    label1.Text = "Step 6 of 40 : Processing Counter table please wait!";
                    label1.Refresh();
                    Progressbar1.Minimum = 1;
                    Progressbar1.Maximum = dtxmlCounter.Rows.Count;
                    Progressbar1.Value = 1;
                    Progressbar1.Step = 1;

                    foreach (DataRow dataRowFromXML in dtxmlCounter.Rows)
                    {

                        Progressbar1.PerformStep();
                        bool SameRowExit = false;
                        int _counterId = Convert.ToInt32(dataRowFromXML["Id"].ToString());
                        string CounterName = dataRowFromXML["Name"].ToString();
                        APP_Data.Counter Foundcounter = entity.Counters.Where(x => x.Name == CounterName).FirstOrDefault();
                        //Same Counter Code Found
                        if (Foundcounter != null)
                        {
                            SameRowExit = true;
                        }
                        //Found same row,Update the Current One
                        if (SameRowExit)
                        {
                            GetCounterFromXML(dataRowFromXML, Foundcounter);
                            entity.SaveChanges();

                            Counter_ForeignKeyTBL(dataRowFromXML, dtxmlTransaction, dtxmlUserPrePaidDebt, Foundcounter);

                        }
                        //Counter is not exist in the current database
                        //add new row
                        else
                        {
                            APP_Data.Counter counter = new APP_Data.Counter();
                            GetCounterFromXML(dataRowFromXML, counter);
                            entity.Counters.Add(counter);
                            entity.SaveChanges();

                            Counter_ForeignKeyTBL(dataRowFromXML, dtxmlTransaction, dtxmlUserPrePaidDebt, counter);

                        }
                    }
                }
                #endregion

                #region Currency

                entity = new APP_Data.POSEntities();
                if (dtxmlCurrency != null)
                {
                    label1.Text = "Step 7 of 40 : Processing Currency table please wait!";
                    label1.Refresh();
                    Progressbar1.Minimum = 1;
                    Progressbar1.Maximum = dtxmlCurrency.Rows.Count;
                    Progressbar1.Value = 1;
                    Progressbar1.Step = 1;
                    //through rowdata come from xml and check if the currency is already exists or new one;
                    foreach (DataRow dataFromXML in dtxmlCurrency.Rows)
                    {
                        Progressbar1.PerformStep();
                        bool SameRowExit = false;
                        string country = dataFromXML["Country"].ToString();
                        int latestExchangeRate = Convert.ToInt32(dataFromXML["LatestExchangeRate"].ToString());
                        APP_Data.Currency FoundCurrency = entity.Currencies.Where(x => x.Country == country).FirstOrDefault();
                        if (FoundCurrency != null)
                        {
                            SameRowExit = true;
                        }
                        if (SameRowExit)
                        {
                            if (latestExchangeRate < FoundCurrency.LatestExchangeRate)
                            {
                                GetCurrencyFromXML(dataFromXML, FoundCurrency);
                                entity.SaveChanges();
                            }

                            Currency_ForeignKeyTBL(dataFromXML, dtxmlTransaction, dtxmlExchangeRateForTransaction, FoundCurrency);
                        }
                        else
                        {
                            APP_Data.Currency currency = new APP_Data.Currency();
                            GetCurrencyFromXML(dataFromXML, currency);
                            entity.Currencies.Add(currency);
                            entity.SaveChanges();

                            Currency_ForeignKeyTBL(dataFromXML, dtxmlTransaction, dtxmlExchangeRateForTransaction, currency);
                        }
                    }
                }
                #endregion

                #region Member Type

                entity = new APP_Data.POSEntities();
                if (dtxmlMemberType != null)
                {
                    label1.Text = "Step 8 of 40 : Processing Member Type table please wait!";
                    label1.Refresh();
                    Progressbar1.Minimum = 1;
                    Progressbar1.Maximum = dtxmlMemberType.Rows.Count;
                    Progressbar1.Value = 1;
                    Progressbar1.Step = 1;
                    //through rowdata come from xml and check if the currency is already exists or new one;
                    foreach (DataRow dataFromXML in dtxmlMemberType.Rows)
                    {
                        Progressbar1.PerformStep();
                        bool SameRowExit = false;
                       
                        int id=Convert.ToInt32(dataFromXML["Id"].ToString());
                        APP_Data.MemberType FoundMemberType = entity.MemberTypes.Where(x => x.Id == id  ).FirstOrDefault();
                        if (FoundMemberType != null)
                        {
                            SameRowExit = true;
                        }
                        if (SameRowExit)
                        {
                            
                                GetMemberTypeFromXML(dataFromXML, FoundMemberType);
                                entity.SaveChanges();


                                MemberType_ForeignKeyTBL(dataFromXML, dtxmlMemberCardRule, dtxmlTransaction, FoundMemberType);
                        }
                        else
                        {
                            APP_Data.MemberType memberType = new APP_Data.MemberType();
                            GetMemberTypeFromXML(dataFromXML, memberType);
                            entity.MemberTypes.Add(memberType);
                            entity.SaveChanges();

                            MemberType_ForeignKeyTBL(dataFromXML, dtxmlMemberCardRule, dtxmlTransaction, memberType);
                        }
                    }
                }
                #endregion

                #region PaymentType

                entity = new APP_Data.POSEntities();
                if (dtxmlPaymentType != null)
                {
                    label1.Text = "Step 9 of 40 : Processing Payment Type table please wait!";
                    label1.Refresh();
                    Progressbar1.Minimum = 1;
                    Progressbar1.Maximum = dtxmlPaymentType.Rows.Count;
                    Progressbar1.Value = 1;
                    Progressbar1.Step = 1;
                    //through rowdata come from xml and check if the currency is already exists or new one;
                    foreach (DataRow dataFromXML in dtxmlPaymentType.Rows)
                    {
                        Progressbar1.PerformStep();
                        bool SameRowExit = false;
                     
                        int Id = Convert.ToInt32(dataFromXML["Id"].ToString());
                        APP_Data.PaymentType FoundPaymentType = entity.PaymentTypes.Where(x =>x.Id == Id ).FirstOrDefault();
                        if (FoundPaymentType != null)
                        {
                            SameRowExit = true;
                        }
                        if (SameRowExit)
                        {
                            GetPaymentTypeFromXML(dataFromXML, FoundPaymentType);
                            entity.SaveChanges();

                            PaymentType_ForeignKeyTBL(dataFromXML, dtxmlTransaction, FoundPaymentType);
                        }
                        else
                        {
                            APP_Data.PaymentType paymentType = new APP_Data.PaymentType();
                            GetPaymentTypeFromXML(dataFromXML, paymentType);
                            entity.PaymentTypes.Add(paymentType);
                            entity.SaveChanges();

                            PaymentType_ForeignKeyTBL(dataFromXML, dtxmlTransaction, paymentType);
                        }
                    }
                #endregion

                 #region GiftCard

                    entity = new APP_Data.POSEntities();
                    if (dtxmlGiftCard != null)
                    {
                        label1.Text = "Step 20 of 31 : Processing Gift Card table please wait!";
                        label1.Refresh();
                        Progressbar1.Minimum = 1;
                        Progressbar1.Maximum = dtxmlGiftCard.Rows.Count;
                        Progressbar1.Value = 1;
                        Progressbar1.Step = 1;
                        //through rowdata come from xml and check if the GiftCard is already existed or new one;
                        foreach (DataRow dataFromXML in dtxmlGiftCard.Rows)
                        {

                            Progressbar1.PerformStep();
                            bool SameRowExit = false;
                            string CardNumber = dataFromXML["CardNumber"].ToString();

                            APP_Data.GiftCard FoundGiftCard = entity.GiftCards.Where(x => x.CardNumber == CardNumber).FirstOrDefault();
                            if (FoundGiftCard != null)
                            {
                                SameRowExit = true;
                            }
                            if (SameRowExit)
                            {

                                GetGiftCardFromXML(dataFromXML, FoundGiftCard);
                                entity.SaveChanges();

                                GiftCard_ForeignKeyTBL(dataFromXML, dtxmlTransaction, FoundGiftCard);
                            }
                            else
                            {
                                APP_Data.GiftCard giftcard = new APP_Data.GiftCard();
                                GetGiftCardFromXML(dataFromXML, giftcard);
                                entity.GiftCards.Add(giftcard);
                                entity.SaveChanges();

                                GiftCard_ForeignKeyTBL(dataFromXML, dtxmlTransaction, giftcard);
                            }
                        }
                    }

                    #endregion

                    #region Unit

                    entity = new APP_Data.POSEntities();
                    if (dtxmlUnit != null)
                    {
                        label1.Text = "Step 10 of 40 : Processing Unit table please wait!";
                        label1.Refresh();
                        Progressbar1.Minimum = 1;
                        Progressbar1.Maximum = dtxmlUnit.Rows.Count;
                        Progressbar1.Value = 1;
                        Progressbar1.Step = 1;
                        //loop through dataRow come from xml and check if the Unit is already exist or Unit is new one
                        foreach (DataRow dataRowFromXMl in dtxmlUnit.Rows)
                        {

                            Progressbar1.PerformStep();
                            bool SameRowExit = false;
                        
                            int Id = Convert.ToInt32(dataRowFromXMl["Id"].ToString());
                            APP_Data.Unit FoundUnit = entity.Units.Where(x =>x.Id==Id ).FirstOrDefault();
                            if (FoundUnit != null)
                            {
                                SameRowExit = true;
                            }
                            if (SameRowExit)
                            {
                                GetUnitFromXML(dataRowFromXMl, FoundUnit);
                                entity.SaveChanges();

                                Unit_ForeignKeyTBL(dataRowFromXMl, dtxmlProduct, FoundUnit);
                            }
                            else
                            {
                                APP_Data.Unit unit = new APP_Data.Unit();
                                GetUnitFromXML(dataRowFromXMl, unit);
                                entity.Units.Add(unit);
                                entity.SaveChanges();

                                Unit_ForeignKeyTBL(dataRowFromXMl, dtxmlProduct, unit);
                            }
                        }
                    }

                    #endregion

                    #region Tax
                    entity = new APP_Data.POSEntities();

                    if (dtxmlTax != null)
                    {
                        label1.Text = "Step 11 of 40 : Processing Tax table please wait!";
                        label1.Refresh();
                        Progressbar1.Minimum = 1;
                        Progressbar1.Maximum = dtxmlTax.Rows.Count;
                        Progressbar1.Value = 1;
                        Progressbar1.Step = 1;
                        foreach (DataRow dataFromXML in dtxmlTax.Rows)
                        {

                            Progressbar1.PerformStep();
                            bool SameRowExist = false;
                         
                            int Id = Convert.ToInt32(dataFromXML["Id"].ToString());
                            APP_Data.Tax FoundTax = entity.Taxes.Where(x => x.Id == Id ).FirstOrDefault();
                            if (FoundTax != null)
                            {
                                SameRowExist = true;
                            }
                            if (SameRowExist)
                            {
                                GetTaxFromXML(dataFromXML, FoundTax);
                                entity.SaveChanges();

                                Tax_ForeignKeyTBL(dataFromXML, dtxmlProduct, FoundTax);
                            }
                            else
                            {
                                APP_Data.Tax tax = new APP_Data.Tax();
                                GetTaxFromXML(dataFromXML, tax);
                                entity.Taxes.Add(tax);
                                entity.SaveChanges();

                                Tax_ForeignKeyTBL(dataFromXML, dtxmlProduct, tax);
                            }
                        }
                    }

                    #endregion

                    #region Product Category

                    entity = new APP_Data.POSEntities();
                    if (dtxmlProductCategory != null)
                    {
                        label1.Text = "Step 12 of 40 : Processing Product Category table please wait!";
                        label1.Refresh();
                        Progressbar1.Minimum = 1;
                        Progressbar1.Maximum = dtxmlProductCategory.Rows.Count;
                        Progressbar1.Value = 1;
                        Progressbar1.Step = 1;
                        //loop through dataRow come from xml and check if the product category is already exist or product category is new one
                        foreach (DataRow dataRowFromXML in dtxmlProductCategory.Rows)
                        {

                            Progressbar1.PerformStep();
                            bool SameRowExit = false;
                           
                            int Id = Convert.ToInt32(dataRowFromXML["Id"].ToString());

                            APP_Data.ProductCategory FoundProductCategory = entity.ProductCategories.Where(x => x.Id == Id ).FirstOrDefault();
                            if (FoundProductCategory != null)
                            {
                                SameRowExit = true;
                            }
                            if (SameRowExit)
                            {
                                GetProductCategoryFromXML(dataRowFromXML, FoundProductCategory);
                                entity.SaveChanges();

                                ProductCategory_ForeignKeyTBL(dataRowFromXML, dtxmlProduct, dtxmlProductSubCategory, FoundProductCategory);
                            }
                            else
                            {
                                APP_Data.ProductCategory productCategory = new APP_Data.ProductCategory();
                                GetProductCategoryFromXML(dataRowFromXML, productCategory);
                                entity.ProductCategories.Add(productCategory);
                                entity.SaveChanges();

                                ProductCategory_ForeignKeyTBL(dataRowFromXML, dtxmlProduct, dtxmlProductSubCategory, productCategory);

                            }
                        }
                    }
                    #endregion

                    #region ProductSubCategory

                    entity = new APP_Data.POSEntities();
                    //loop though dataRow come from xml and check if the counter is already exist or counter is new on
                    if (dtxmlProductSubCategory != null)
                    {
                        label1.Text = "Step 13 of 40 : Processing Product Sub Category table please wait!";
                        label1.Refresh();
                        Progressbar1.Minimum = 1;
                        Progressbar1.Maximum = dtxmlProductSubCategory.Rows.Count;
                        Progressbar1.Value = 1;
                        Progressbar1.Step = 1;
                        foreach (DataRow dataRowFromXML in dtxmlProductSubCategory.Rows)
                        {

                            Progressbar1.PerformStep();
                            bool SameRowExit = false;
                        
                            int ProductCategoryId = Convert.ToInt32(dataRowFromXML["ProductCategoryId"].ToString());
                            int id= Convert.ToInt32(dataRowFromXML["Id"].ToString());
                            APP_Data.ProductSubCategory FoundProductSubCategory = entity.ProductSubCategories.Where(  x => x.Id== id ).FirstOrDefault();
                            if (FoundProductSubCategory != null)
                            {
                                SameRowExit = true;
                            }
                            if (SameRowExit)
                            {
                                GetProductSubCategoryFromXML(dataRowFromXML, FoundProductSubCategory);
                                entity.SaveChanges();

                                ProductSubCag_ForeignKeyTBL(dataRowFromXML, dtxmlProduct, FoundProductSubCategory);

                            }
                            else
                            {
                                APP_Data.ProductSubCategory productSubCategory = new APP_Data.ProductSubCategory();
                                GetProductSubCategoryFromXML(dataRowFromXML, productSubCategory);
                                entity.ProductSubCategories.Add(productSubCategory);
                                entity.SaveChanges();

                                ProductSubCag_ForeignKeyTBL(dataRowFromXML, dtxmlProduct, productSubCategory);
                            }
                        }
                    }

                    #endregion

                    #region Expense Category

                    entity = new APP_Data.POSEntities();
                    if (dtxmlExpenseCategory != null)
                    {
                        label1.Text = "Step 17 of 40 : Processing Expense Category table please wait!";
                        label1.Refresh();
                        Progressbar1.Minimum = 1;
                        Progressbar1.Maximum = dtxmlExpenseCategory.Rows.Count;
                        Progressbar1.Value = 1;
                        Progressbar1.Step = 1;
                        //loop through dataRow come from xml and check if the Unit is already exist or Unit is new one
                        foreach (DataRow dataRowFromXMl in dtxmlExpenseCategory.Rows)
                        {

                            Progressbar1.PerformStep();
                            bool SameRowExit = false;
                         
                            int id = Convert.ToInt32(dataRowFromXMl["Id"].ToString());
                            APP_Data.ExpenseCategory FoundExpenseCategory = entity.ExpenseCategories.Where(x => x.Id == id ).FirstOrDefault();
                            if (FoundExpenseCategory != null)
                            {
                                SameRowExit = true;
                            }
                            if (SameRowExit)
                            {
                                GetExpenseCategoryFromXML(dataRowFromXMl, FoundExpenseCategory);
                                entity.SaveChanges();

                                ExpenseCag_ForeignKeyTBL(dataRowFromXMl, dtxmlExpense, FoundExpenseCategory);
                            }
                            else
                            {
                                APP_Data.ExpenseCategory expenseCag = new APP_Data.ExpenseCategory();
                                GetExpenseCategoryFromXML(dataRowFromXMl, expenseCag);
                                entity.ExpenseCategories.Add(expenseCag);
                                entity.SaveChanges();

                                ExpenseCag_ForeignKeyTBL(dataRowFromXMl, dtxmlExpense, expenseCag);
                            }
                        }
                    }

                    #endregion

                    #region Expense 

                    entity = new APP_Data.POSEntities();
                    if (dtxmlExpense != null)
                    {
                        label1.Text = "Step 18 of 40 : Processing Expense  table please wait!";
                        label1.Refresh();
                        Progressbar1.Minimum = 1;
                        Progressbar1.Maximum = dtxmlExpense.Rows.Count;
                        Progressbar1.Value = 1;
                        Progressbar1.Step = 1;
                        //loop through dataRow come from xml and check if the Unit is already exist or Unit is new one
                        foreach (DataRow dataRowFromXMl in dtxmlExpense.Rows)
                        {

                            Progressbar1.PerformStep();
                            bool SameRowExit = false;

                            string id = dataRowFromXMl["Id"].ToString();
                            APP_Data.Expense FoundExpense = entity.Expenses.Where(x => x.Id == id ).FirstOrDefault();
                            if (FoundExpense != null)
                            {
                                SameRowExit = true;
                            }
                            if (!SameRowExit)
                            {
                                ////GetExpenseFromXML(dataRowFromXMl, FoundExpense);
                                ////entity.SaveChanges();

                                ////Expense_ForeignKeyTBL(dataRowFromXMl, dtxmlExpenseDetail, FoundExpense);

                                APP_Data.Expense expense = new APP_Data.Expense();
                                GetExpenseFromXML(dataRowFromXMl, expense);
                                entity.Expenses.Add(expense);
                                entity.SaveChanges();

                                Expense_ForeignKeyTBL(dataRowFromXMl, dtxmlExpenseDetail, expense);
                            }
                          
                        }
                    }

                    #endregion

                    #region Expense Detail

                    entity = new APP_Data.POSEntities();
                    if (dtxmlExpenseDetail != null)
                    {
                        label1.Text = "Step 19 of 40 : Processing Expense Detail  table please wait!";
                        label1.Refresh();
                        Progressbar1.Minimum = 1;
                        Progressbar1.Maximum = dtxmlExpenseDetail.Rows.Count;
                        Progressbar1.Value = 1;
                        Progressbar1.Step = 1;
                        //loop through dataRow come from xml and check if the Unit is already exist or Unit is new one
                        foreach (DataRow dataRowFromXMl in dtxmlExpenseDetail.Rows)
                        {

                            Progressbar1.PerformStep();
                            bool SameRowExit = false;

                            string _expenseId = dataRowFromXMl["ExpenseId"].ToString();
                            string _description = dataRowFromXMl["Description"].ToString();
                            APP_Data.ExpenseDetail FoundExpenseDetail = entity.ExpenseDetails.Where(x => x.ExpenseId == _expenseId && x.Description == _description ).FirstOrDefault();
                            if (FoundExpenseDetail != null)
                            {
                                SameRowExit = true;
                            }
                            if (!SameRowExit)
                            {
                                APP_Data.ExpenseDetail expenseDetail = new APP_Data.ExpenseDetail();
                                GetExpenseDetailFromXML(dataRowFromXMl, expenseDetail);
                                entity.ExpenseDetails.Add(expenseDetail);
                                entity.SaveChanges();
                            }
                         
                            


                            
                        }
                    }




                    #endregion

                    #region Member Card Rule

                    entity = new APP_Data.POSEntities();
                    if (dtxmlMemberCardRule != null)
                    {
                        label1.Text = "Step 20 of 40 : Processing Member Card Rule table please wait!";
                        label1.Refresh();
                        Progressbar1.Minimum = 1;
                        Progressbar1.Maximum = dtxmlMemberCardRule.Rows.Count;
                        Progressbar1.Value = 1;
                        Progressbar1.Step = 1;
                        //loop through dataRow come from xml and check if the Unit is already exist or Unit is new one
                        foreach (DataRow dataRowFromXMl in dtxmlMemberCardRule.Rows)
                        {

                            Progressbar1.PerformStep();
                            bool SameRowExit = false;

                            int _id = Convert.ToInt32(dataRowFromXMl["Id"].ToString());
                            int _memberTypeId = Convert.ToInt32(dataRowFromXMl["MemberTypeId"].ToString());
                            APP_Data.MemberCardRule FoundMemberCardRule = entity.MemberCardRules.Where(x => x.Id == _id).FirstOrDefault();
                            if (FoundMemberCardRule != null)
                            {
                                SameRowExit = true;
                            }
                            if (SameRowExit)
                            {
                                GetMemberCardRuleFromXML(dataRowFromXMl, FoundMemberCardRule);
                                entity.SaveChanges();

                            }
                            else
                            {
                                APP_Data.MemberCardRule memberCardRule = new APP_Data.MemberCardRule();
                                GetMemberCardRuleFromXML(dataRowFromXMl, memberCardRule);
                                entity.MemberCardRules.Add(memberCardRule);
                                entity.SaveChanges();


                            }
                        }
                    }

                    #endregion

                #region Customer
                entity = new APP_Data.POSEntities();
                //Loop through dataRow come from xml and check if the customer is already exist or a new customer
                if (dtxmlCustomer != null)
                {
                    Progressbar1.Minimum = 1;
                    Progressbar1.Maximum = dtxmlCustomer.Rows.Count;
                    Progressbar1.Value = 1;
                    Progressbar1.Step = 1;
                    label1.Text = "Step 21 of 40 : Processing Customer table please wait!";
                    label1.Refresh();

                    foreach (DataRow dataRowFromXML in dtxmlCustomer.Rows)
                    {
                        Progressbar1.PerformStep();
                        Boolean SameRowExist = false;

                       

                        String CustomerCode = dataRowFromXML["CustomerCode"].ToString();
                       
                        APP_Data.Customer FoundCustomer = entity.Customers.Where(x => x.CustomerCode == CustomerCode).FirstOrDefault();

                        if (FoundCustomer != null)
                        {
                            SameRowExist = true;
                        }
                   
                        //Found same row,Update the Current One
                        if (SameRowExist)
                        {
                            //APP_Data.Customer FoundCustomer = entity.Customers.Where(x => x.Id == SameCustomerId).FirstOrDefault();
                            GetCustomerFromXML(dataRowFromXML, FoundCustomer);
                            entity.SaveChanges();

                            Customer_ForeignKeyTBL(dataRowFromXML, dtxmlTransaction, FoundCustomer);
                        }
                        //Customer Data is not exist in the database
                        //add new row
                        else
                        {
                            APP_Data.Customer cs = new APP_Data.Customer();
                            GetCustomerFromXML(dataRowFromXML, cs);
                            entity.Customers.Add(cs);
                            entity.SaveChanges();

                            Customer_ForeignKeyTBL(dataRowFromXML, dtxmlTransaction, cs);
                        
                        }
                    }
                }
                #endregion

                 #region Product
                entity = new APP_Data.POSEntities();

                if (dtxmlProduct != null)
                {
                    label1.Text = "Step 24 of 40 : Processing Product table please wait!";
                    label1.Refresh();
                    Progressbar1.Minimum = 1;
                    Progressbar1.Maximum = dtxmlProduct.Rows.Count;
                    Progressbar1.Value = 1;
                    Progressbar1.Step = 1;
                    //Loop through dataRow come from xml and check if the product is already exist or product is new one
                    foreach (DataRow dataRowFromXML in dtxmlProduct.Rows)
                    {
                        Progressbar1.PerformStep();
                        Boolean SameRowExist = false;
                        String ProductCode = dataRowFromXML["ProductCode"].ToString();
                        String Barcode = dataRowFromXML["BarCode"].ToString();
                       // DateTime UpdateDate = Convert.ToDateTime(dataRowFromXML["UpdatedDate"].ToString());
                        bool IsWrapperItem = Convert.ToBoolean(dataRowFromXML["IsWrapper"].ToString());
                        string Price = dataRowFromXML["Price"].ToString();

                        APP_Data.Product FoundProduct = entity.Products.Where(x => x.ProductCode == ProductCode).FirstOrDefault();
                       // APP_Data.Product FoundProductWithBarcode = entity.Products.Where(x => x.Barcode == Barcode).FirstOrDefault();

                    
                 
                        //   Same Product Code Found
                        if (FoundProduct != null)
                        {
                            SameRowExist = true;
                        }
                        //   Same Barcode Found
                        //if (FoundProductWithBarcode != null)
                        //{
                        //    SameRowExist = true;
                        //    FoundProduct = FoundProductWithBarcode;
                        //}
                        // Found same row,Update the Current One
                        if (SameRowExist)
                        {
                            GetProductFromXML(dataRowFromXML, FoundProduct);
                            entity.SaveChanges();

                            Product_ForeignKeyTBL(dataRowFromXML, dtxmlProductQuantityChange, dtxmlProductPriceChange, dtxmlSPDetail, dtxmlStockInDetail, dtxmlTransactionDetail, dtxmlUnitConversion, dtxmlWrapperItem, FoundProduct);
                        }
                        //  product code & barcode is not exist in the current database
                        //  add new row
                        else
                        {
                            APP_Data.Product product = new APP_Data.Product();
                            GetProductFromXML(dataRowFromXML, product);
                            product.Qty = 0;
                            entity.Products.Add(product);
                            entity.SaveChanges();
                            Product_ForeignKeyTBL(dataRowFromXML, dtxmlProductQuantityChange, dtxmlProductPriceChange, dtxmlSPDetail, dtxmlStockInDetail, dtxmlTransactionDetail, dtxmlUnitConversion, dtxmlWrapperItem, product);
                        }
                    }
                }
             
                #endregion

                #region ProductPriceChange

                entity = new APP_Data.POSEntities();

                if (dtxmlProductPriceChange != null)
                {
                    label1.Text = "Step 25 of 40 : Processing Product Price Change table please wait!";
                    label1.Refresh();
                    Progressbar1.Minimum = 1;
                    Progressbar1.Maximum = dtxmlProductPriceChange.Rows.Count;
                    Progressbar1.Value = 1;
                    Progressbar1.Step = 1;
                    foreach (DataRow dataFromXML in dtxmlProductPriceChange.Rows)
                    {
                        Progressbar1.PerformStep();
                        bool SameRowExit = false;
                        int SameFieldCount = 0;
                        long pdcId = 0;

                        long ProductId = Convert.ToInt64(dataFromXML["ProductId"].ToString());
                        DateTime UpdateDate = Convert.ToDateTime(dataFromXML["UpdateDate"].ToString());
                        long newPrice = Convert.ToInt64(dataFromXML["Price"].ToString());
                        long oldPrice = Convert.ToInt64(dataFromXML["OldPrice"].ToString());

                        foreach (APP_Data.ProductPriceChange pdc in entity.ProductPriceChanges)
                        {
                            SameFieldCount = 0;
                            if (pdc.ProductId == ProductId) SameFieldCount++;
                            if (pdc.UpdateDate == UpdateDate) SameFieldCount++;
                            if (pdc.Price == newPrice) SameFieldCount++;
                            if (pdc.OldPrice == oldPrice) SameFieldCount++;

                            if (SameFieldCount >= 4)
                            {
                                SameRowExit = true;
                                pdcId = pdc.Id;
                                break;
                            }
                        }
                        if (SameRowExit)
                        {
                            APP_Data.ProductPriceChange Foundpdc = entity.ProductPriceChanges.Where(x => x.Id == pdcId).FirstOrDefault();
                            GetProductPriceChangeFromXML(dataFromXML, Foundpdc);
                            entity.SaveChanges();
                        }
                        else
                        {
                            APP_Data.ProductPriceChange pdcObj = new APP_Data.ProductPriceChange();
                            GetProductPriceChangeFromXML(dataFromXML, pdcObj);
                            entity.ProductPriceChanges.Add(pdcObj);
                            entity.SaveChanges();
                        }
                    }
                }


                #endregion


                 #region WrapperItem

                entity = new APP_Data.POSEntities();
                if (dtxmlWrapperItem != null)
                {
                    label1.Text = "Step 26 of 40 : Processing Wrapper Item table please wait!";
                    label1.Refresh();
                    Progressbar1.Minimum = 1;
                    Progressbar1.Maximum = dtxmlWrapperItem.Rows.Count;
                    Progressbar1.Value = 1;
                    Progressbar1.Step = 1;
                    foreach (DataRow dataFromXML in dtxmlWrapperItem.Rows)
                    {
                        label1.Text = "Records Wrapper Item Processing ";
                        Progressbar1.PerformStep();
                        bool SameRowExit = false;
                        long ParentProductId = Convert.ToInt64(dataFromXML["ParentProductId"].ToString());
                        long ChildProductId = Convert.ToInt64(dataFromXML["ChildProductId"].ToString());

                        APP_Data.WrapperItem FoundWrapper = entity.WrapperItems.Where(x => x.ParentProductId == ParentProductId && x.ChildProductId == ChildProductId).FirstOrDefault();
                        if (FoundWrapper != null)
                        {
                            SameRowExit = true;
                        }
                        if (SameRowExit)
                        {
                            GetWrapperItemFromXML(dataFromXML, FoundWrapper);
                            entity.SaveChanges();
                        }
                        else
                        {
                            APP_Data.WrapperItem wpObj = new APP_Data.WrapperItem();
                            GetWrapperItemFromXML(dataFromXML, wpObj);
                            entity.WrapperItems.Add(wpObj);
                            entity.SaveChanges();
                        }
                    }
                }

                #endregion

                   #region Shop
                entity = new APP_Data.POSEntities();
                if (dtxmlShop != null)
                {
                    label1.Text = "Step 27 of 40 : Processing Shop table please wait!";
                    label1.Refresh();
                    Progressbar1.Minimum = 1;
                    Progressbar1.Maximum = dtxmlShop.Rows.Count;
                    Progressbar1.Value = 1;
                    Progressbar1.Step = 1;
                    //loop through dataRow from xml and check if the shop is already exist or newone.
                    foreach (DataRow dataFromXML in dtxmlShop.Rows)
                    {
                        Progressbar1.PerformStep();
                        bool SameRowExit = false;
                        //string ShopName = dataFromXML["ShopName"].ToString();
                        //string Address = dataFromXML["Address"].ToString();

                        int _shopId = Convert.ToInt32(dataFromXML["Id"].ToString());
                        APP_Data.Shop FoundShop = entity.Shops.Where(x => x.Id == _shopId).FirstOrDefault();
                        //same shop name found
                        if (FoundShop != null)
                        {
                            SameRowExit = true;
                        }
                        //Found same row,Update the Current One
                        if (SameRowExit)
                        {
                            GetShopFromXML(dataFromXML, FoundShop);
                            entity.SaveChanges();

                            Shop_ForeignKeyTBL(dataFromXML, dtxmlTransaction, dtxmlUser, FoundShop);
                        }
                        //shop name is not exist in the current database
                        //add new row
                        else
                        {
                            APP_Data.Shop shop = new APP_Data.Shop();
                            GetShopFromXML(dataFromXML, shop);
                            entity.Shops.Add(shop);
                            entity.SaveChanges();
                            Shop_ForeignKeyTBL(dataFromXML, dtxmlTransaction, dtxmlUser, shop);
                        }
                    }
                }
                #endregion

                    #region Transaction

                    entity = new APP_Data.POSEntities();
                    List<int> TranIdList = new List<int>();
                    if (dtxmlTransaction != null)
                    {
                        label1.Text = "Step 28 of 40 : Processing Transaction table please wait!";
                        label1.Refresh();
                        Progressbar1.Minimum = 1;
                        Progressbar1.Maximum = dtxmlTransaction.Rows.Count;
                        Progressbar1.Value = 1;
                        Progressbar1.Step = 1;
                        //Loop through dataRow come from xml and check if the transaction is already exist or a new one
                        DateTime today = DateTime.Today;
                        DateTime TranDate = DateTime.Today.AddMonths(-5);
                        DataTable _TranProcessing = new DataTable();

                        foreach (DataRow dataRowFromXML in dtxmlTransaction.Rows)
                        {

                            Progressbar1.PerformStep();
                            Boolean SameRowExist = false;
                            String Id = dataRowFromXML["Id"].ToString();
                            int shopIdfromxml=Convert.ToInt32(dataRowFromXML["ShopId"].ToString());

                            APP_Data.Transaction FoundTransaction = entity.Transactions.Where(x => x.Id == Id && x.ShopId==shopIdfromxml).FirstOrDefault();
                            //Same TransactionId Found
                            if (FoundTransaction != null)
                            {
                                SameRowExist = true;
                            }
                            //Found same row,Update the Current One if xml transactions have more recent updatedDate
                            if (SameRowExist)
                            {
                                DateTime currentUpdateDateFromXML = Convert.ToDateTime(dataRowFromXML["UpdatedDate"].ToString());
                                if (FoundTransaction.UpdatedDate <= currentUpdateDateFromXML)
                                {
                                    GetTransactionFromXML(dataRowFromXML, FoundTransaction);
                                    entity.Entry(FoundTransaction).State = EntityState.Modified;
                                    entity.SaveChanges();
                                    if (dataRowFromXML["Type"].ToString()=="Refund" || dataRowFromXML["Type"].ToString() == "CreditRefund")
                                    {

                                        //APP_Data.Transaction mainRow = entity.Transactions.Find(dtxmlTransaction.Select("Id ='" + OldParentId + "'"));
                                        APP_Data.Transaction mainRow = entity.Transactions.Find(dataRowFromXML["ParentId1"].ToString());
                                        if (entity.Transactions.Find(mainRow.ParentId) != null)
                                        {
                                            APP_Data.Transaction refundRow_ToUpdate = entity.Transactions.Find(mainRow.ParentId);
                                            refundRow_ToUpdate.ParentId = mainRow.Id;
                                            entity.Entry(refundRow_ToUpdate).State = EntityState.Modified;
                                            entity.SaveChanges();
                                        }
                                    }
                                   

                                    Transaction_ForeignKeyTBL(dataRowFromXML, dtxmlTransactionDetail, dtxmlUserPrePaidDebt, dtxmlExchangeRateForTransaction, FoundTransaction);
                                }
                            }
                            //Transaction Id is not exist in current database
                            //add new row
                            else
                            {
                                APP_Data.Transaction ts = new APP_Data.Transaction();
                                APP_Data.Transaction tsRefund = new APP_Data.Transaction();
                                //Check  for transaction type whether sale or credit or sale or prepaid or fefund or credit refund 

                                if (dataRowFromXML["Type"].ToString() == "Refund" || dataRowFromXML["Type"].ToString() == "CreditRefund")
                                {
                                    //Get  sale or credit transaction Id of  current refund or credit refund transaction.
                                    string OldParentId = dataRowFromXML["ParentId1"].ToString();
                                    string refundid = dataRowFromXML["Id"].ToString();
                                    //chcek at main Main Database whether having Transaction about parentId of current refund  or credit refund or debt transaction.
                                    APP_Data.Transaction tsParentId = entity.Transactions.Where(x => x.Id == OldParentId).FirstOrDefault();

                                    if (tsParentId != null)
                                    {
                                        GetTransactionFromXML(dataRowFromXML, ts);
                                        entity.Transactions.Add(ts);
                                        entity.SaveChanges();
                                    }
                                    else
                                    {
                                        GetTransactionFromXML(dataRowFromXML, tsRefund);
                                        entity.Transactions.Add(tsRefund);
                                        entity.SaveChanges();
                                        Tran_ForeignKeyTBL(dataRowFromXML, dtxmlTransactionDetail, dtxmlUserPrePaidDebt, dtxmlExchangeRateForTransaction, tsRefund);
                                        foreach (DataRow dr in dtxmlTransaction.Select("Id ='" + OldParentId + "'"))
                                        {
                                            if (dr != null && dr["Type"].ToString() != "Refund" && dr["Type"].ToString() != "CreditRefund")
                                            {
                                                GetTransactionFromXML(dr, ts);
                                                entity.Transactions.Add(ts);
                                                int row=entity.SaveChanges();

                                                APP_Data.Transaction mainRow = entity.Transactions.Find(OldParentId);
                                                if (entity.Transactions.Find(mainRow.ParentId)!=null)
                                                {
                                                    APP_Data.Transaction refundRow_ToUpdate = entity.Transactions.Find(mainRow.ParentId);
                                                    refundRow_ToUpdate.ParentId = mainRow.Id;
                                                    entity.Entry(refundRow_ToUpdate).State = EntityState.Modified;
                                                    entity.SaveChanges();
                                                }

                                                Tran_ForeignKeyTBL(dr, dtxmlTransactionDetail, dtxmlUserPrePaidDebt, dtxmlExchangeRateForTransaction,ts);
                                            }
                                        }
                                       
                                    }
                                }
                                else
                                {
                                    GetTransactionFromXML(dataRowFromXML, ts);
                                    entity.Transactions.Add(ts);
                                    entity.SaveChanges();
                                }

                                Tran_ForeignKeyTBL(dataRowFromXML, dtxmlTransactionDetail, dtxmlUserPrePaidDebt, dtxmlExchangeRateForTransaction, ts);
                            }
                        }
                    }
                    #endregion

                    #region TransactionDetail

                    entity = new APP_Data.POSEntities();

                    if (dtxmlTransactionDetail != null)
                    {
                        //Loop through dataRow come from xml and check if the transaction is already exist or a new one
                        label1.Text = "Step 29 of 40 : Processing Transaction Detail table please wait!";
                        label1.Refresh();
                        Progressbar1.Minimum = 1;
                        //Progressbar1.Maximum = dtxmlTransactionDetail.Rows.Count;
                        Progressbar1.Maximum = dtxmlTransactionDetail.Rows.Count;
                        Progressbar1.Value = 1;
                        Progressbar1.Step = 1;

                        foreach (DataRow dataRowFromXML in dtxmlTransactionDetail.Rows)
                        {
                            Progressbar1.PerformStep();
                            Boolean SameRowExist = false;
                            String TransactionId = dataRowFromXML["TransactionId"].ToString();
                            long ProductId = Convert.ToInt64(dataRowFromXML["ProductId"].ToString());
                            Boolean IsDelete = Convert.ToBoolean(dataRowFromXML["IsDeleted"].ToString());
                            string ProductCode = dataRowFromXML["ProductCode"].ToString();
                            APP_Data.TransactionDetail FoundTransactionDetail = entity.TransactionDetails.Where(x => x.TransactionId == TransactionId && x.ProductId == ProductId).FirstOrDefault();
                            //Same TransactionId Found
                            if (FoundTransactionDetail != null)
                            {
                                SameRowExist = true;
                            }
                            //Found same row,Update the Current One if xml transactions have more recent updatedDate
                            if (SameRowExist)
                            {
                                GetTransactionDetailFromXML(dataRowFromXML, FoundTransactionDetail);
                                entity.SaveChanges();

                                TransactionDetail_ForeignKeyTBL(dataRowFromXML, dtxmlUserPrePaidDebt, dtxmlSPDetail,dtxmlConsignmentSettlementDetail ,FoundTransactionDetail);

                            }
                            //Transaction Id is not exist in current database
                            //add new row
                            else
                            {
                                APP_Data.TransactionDetail ts = new APP_Data.TransactionDetail();
                                GetTransactionDetailFromXML(dataRowFromXML, ts);
                                entity.TransactionDetails.Add(ts);
                                entity.SaveChanges();

                                TransactionDetail_ForeignKeyTBL(dataRowFromXML, dtxmlUserPrePaidDebt, dtxmlSPDetail, dtxmlConsignmentSettlementDetail, ts);
                            }
                        }
                    }
                    #endregion

                    #region ExchangeRateForTransaction

                    entity = new APP_Data.POSEntities();

                    if (dtxmlExchangeRateForTransaction != null)
                    {
                        label1.Text = "Step 31 of 40 : Processing Exchange Rate For Transaction table please wait!";
                        label1.Refresh();
                        Progressbar1.Minimum = 1;
                        Progressbar1.Maximum = dtxmlExchangeRateForTransaction.Rows.Count;
                        Progressbar1.Value = 1;
                        Progressbar1.Step = 1;
                        //loop through the dataRow come from xml and check if ExchangeRateForTransaction is already existed or new one
                        foreach (DataRow dataFromXML in dtxmlExchangeRateForTransaction.Rows)
                        {

                            Progressbar1.PerformStep();
                            bool SameRowExist = false;
                            String TransactionId = dataFromXML["TransactionId"].ToString();
                            APP_Data.ExchangeRateForTransaction FoundExchangeRate = entity.ExchangeRateForTransactions.Where(x => x.TransactionId == TransactionId).FirstOrDefault();
                            if (FoundExchangeRate != null)
                            {
                                SameRowExist = true;
                            }
                            if (SameRowExist)
                            {
                                GetExchangeReateForTansction(dataFromXML, FoundExchangeRate);
                                entity.SaveChanges();
                            }
                            else
                            {
                                APP_Data.ExchangeRateForTransaction exchageRate = new APP_Data.ExchangeRateForTransaction();
                                GetExchangeReateForTansction(dataFromXML, exchageRate);
                                entity.ExchangeRateForTransactions.Add(exchageRate);
                                entity.SaveChanges();
                            }
                        }
                    }

                    #endregion

                    #region SPDetail
                    entity = new APP_Data.POSEntities();
                    if (dtxmlSPDetail != null)
                    {
                        label1.Text = "Step 32 of 40 : Processing SP Detail table please wait!";
                        label1.Refresh();
                        Progressbar1.Minimum = 1;
                        Progressbar1.Maximum = dtxmlSPDetail.Rows.Count;
                        Progressbar1.Value = 1;
                        Progressbar1.Step = 1;
                        foreach (DataRow dataFromXML in dtxmlSPDetail.Rows)
                        {
                            Progressbar1.PerformStep();
                            bool SameRowExit = false;
                            int SameFieldCount = 0;
                            string spId = "";

                            int TransactionDetailID = Convert.ToInt32(dataFromXML["TransactionDetailID"].ToString());
                            long ParentProductID = Convert.ToInt64(dataFromXML["ParentProductID"].ToString());
                            long ChildProductID = Convert.ToInt64(dataFromXML["ChildProductID"].ToString());
                            string SPDetailID = dataFromXML["SPDetailID"].ToString();

                            foreach (APP_Data.SPDetail sp in entity.SPDetails)
                            {
                                SameFieldCount = 0;
                                if (sp.TransactionDetailID == TransactionDetailID) SameFieldCount++;
                                if (sp.ParentProductID == ParentProductID) SameFieldCount++;
                                if (sp.ChildProductID == ChildProductID) SameFieldCount++;
                                if (sp.SPDetailID == SPDetailID) SameFieldCount++;

                                if (SameFieldCount >= 4)
                                {
                                    SameRowExit = true;
                                    spId = sp.SPDetailID;
                                    break;
                                }
                            }
                            if (SameRowExit)
                            {
                                APP_Data.SPDetail foundSpDetail = entity.SPDetails.Where(x => x.SPDetailID == spId).FirstOrDefault();
                                GetSPDetailFromXML(dataFromXML, foundSpDetail);
                                entity.SaveChanges();
                            }
                            else
                            {
                                APP_Data.SPDetail spDObj = new APP_Data.SPDetail();
                                GetSPDetailFromXML(dataFromXML, spDObj);
                                entity.SPDetails.Add(spDObj);
                                entity.SaveChanges();
                            }
                        }
                    }
                    #endregion

                    #region UsePrePaidDebt

                    entity = new APP_Data.POSEntities();

                    if (dtxmlUserPrePaidDebt != null)
                    {
                        label1.Text = "Step 33 of 40 : Processing Use Pre Paid Debt table please wait!";
                        label1.Refresh();
                        Progressbar1.Minimum = 1;
                        Progressbar1.Maximum = dtxmlUserPrePaidDebt.Rows.Count;
                        Progressbar1.Value = 1;
                        Progressbar1.Step = 1;
                        //through rowdata come from xml and check if the GiftCard is already existed or new one;
                        foreach (DataRow dataFromXML in dtxmlUserPrePaidDebt.Rows)
                        {
                            Progressbar1.PerformStep();
                            bool SameRowExit = false;
                            string CreditTransactionId = dataFromXML["CreditTransactionId"].ToString();
                            string PrePaidDebtTransactionId = dataFromXML["PrePaidDebtTransactionId"].ToString();
                            int UseAmount = Convert.ToInt32(dataFromXML["UseAmount"].ToString());

                            //APP_Data.UsePrePaidDebt FoundUserPrePaidDebt = entity.UsePrePaidDebts.Where(x => x.CreditTransactionId == CreditTransactionId && x.PrePaidDebtTransactionId == PrePaidDebtTransactionId && x.UseAmount == UseAmount).FirstOrDefault();
                            APP_Data.UsePrePaidDebt FoundUserPrePaidDebt = entity.UsePrePaidDebts.Where(x => x.CreditTransactionId == CreditTransactionId).FirstOrDefault();
                            if (FoundUserPrePaidDebt != null)
                            {
                                SameRowExit = true;
                            }
                            if (SameRowExit)
                            {
                                UserPrePaidDebtFromXML(dataFromXML, FoundUserPrePaidDebt);
                                entity.SaveChanges();
                            }
                            else
                            {
                                APP_Data.UsePrePaidDebt usePrePaidDebt = new APP_Data.UsePrePaidDebt();
                                UserPrePaidDebtFromXML(dataFromXML, usePrePaidDebt);
                                entity.UsePrePaidDebts.Add(usePrePaidDebt);
                                entity.SaveChanges();
                            }
                        }
                    }

                    #endregion

                    #region Deletelog

                    entity = new APP_Data.POSEntities();

                    if (dtxmlDeleteLog != null)
                    {
                        label1.Text = "Step 34 of 40 : Processing Delete Log table please wait!";
                        label1.Refresh();
                        Progressbar1.Minimum = 1;
                        Progressbar1.Maximum = dtxmlDeleteLog.Rows.Count;
                        Progressbar1.Value = 1;
                        Progressbar1.Step = 1;
                        //loop through dataRow from xml and check if the deletelog is already exist  or a new one
                        foreach (DataRow dataFromXML in dtxmlDeleteLog.Rows)
                        {

                            Progressbar1.PerformStep();
                            bool SameRowExit = false;
                            string transactionId = dataFromXML["TransactionId"].ToString();
                            APP_Data.DeleteLog FoundDeleteLog = entity.DeleteLogs.Where(x => x.TransactionId == transactionId).FirstOrDefault();

                            if (FoundDeleteLog != null)
                            {
                                SameRowExit = true;
                            }
                            if (SameRowExit)
                            {
                                GetDeleteLogFromXML(dataFromXML, FoundDeleteLog);
                                entity.SaveChanges();
                            }
                            else
                            {
                                APP_Data.DeleteLog deleteLog = new APP_Data.DeleteLog();
                                GetDeleteLogFromXML(dataFromXML, deleteLog);
                                entity.DeleteLogs.Add(deleteLog);
                                entity.SaveChanges();
                            }
                        }
                    }

                    #endregion


                    #region Consingment Settlement

                    entity = new APP_Data.POSEntities();
                    if (dtxmlConsignmentSettlement != null)
                    {
                        label1.Text = "Step 35 of 40 : Processing Consingment Settlement  table please wait!";
                        label1.Refresh();
                        Progressbar1.Minimum = 1;
                        Progressbar1.Maximum = dtxmlConsignmentSettlement.Rows.Count;
                        Progressbar1.Value = 1;
                        Progressbar1.Step = 1;
                        //loop through dataRow come from xml and check if the Unit is already exist or Unit is new one
                        foreach (DataRow dataRowFromXMl in dtxmlConsignmentSettlement.Rows)
                        {

                            Progressbar1.PerformStep();
                            bool SameRowExit = false;

                            string _consignmentNo = dataRowFromXMl["ConsignmentNo"].ToString();
                            APP_Data.ConsignmentSettlement FoundConsignmentSettlement = entity.ConsignmentSettlements.Where(x => x.ConsignmentNo == _consignmentNo).FirstOrDefault();
                            if (FoundConsignmentSettlement != null)
                            {
                                SameRowExit = true;
                            }
                            if (SameRowExit)
                            {
                                GetConsignmentSettlementFromXML(dataRowFromXMl, FoundConsignmentSettlement);
                                entity.SaveChanges();

                            }
                            else
                            {
                                APP_Data.ConsignmentSettlement consignmentSettlement = new APP_Data.ConsignmentSettlement();
                                GetConsignmentSettlementFromXML(dataRowFromXMl, consignmentSettlement);
                                entity.ConsignmentSettlements.Add(consignmentSettlement);
                                entity.SaveChanges();


                            }
                        }
                    }

                    #endregion


                    #region Consingment Settlement Detail

                    entity = new APP_Data.POSEntities();
                    if (dtxmlConsignmentSettlementDetail != null)
                    {
                        label1.Text = "Step 36 of 40 : Processing Consingment Settlement Detail  table please wait!";
                        label1.Refresh();
                        Progressbar1.Minimum = 1;
                        Progressbar1.Maximum = dtxmlConsignmentSettlementDetail.Rows.Count;
                        Progressbar1.Value = 1;
                        Progressbar1.Step = 1;
                        //loop through dataRow come from xml and check if the Unit is already exist or Unit is new one
                        foreach (DataRow dataRowFromXMl in dtxmlConsignmentSettlementDetail.Rows)
                        {

                            Progressbar1.PerformStep();
                            bool SameRowExit = false;

                            string _consignmentNo = dataRowFromXMl["ConsignmentNo"].ToString();
                            int _tranDetailId = Convert.ToInt32(dataRowFromXMl["TransactionDetailId"].ToString());
                            APP_Data.ConsignmentSettlementDetail FoundConsignmentSettlementDetail = entity.ConsignmentSettlementDetails.Where(x => x.ConsignmentNo == _consignmentNo && x.TransactionDetailId == _tranDetailId).FirstOrDefault();
                            if (FoundConsignmentSettlementDetail != null)
                            {
                                SameRowExit = true;
                            }
                            if (SameRowExit)
                            {
                                GetConsignmentSettlementDetailFromXML(dataRowFromXMl, FoundConsignmentSettlementDetail);
                                entity.SaveChanges();

                            }
                            else
                            {
                                APP_Data.ConsignmentSettlementDetail consignmentSettlementDetail = new APP_Data.ConsignmentSettlementDetail();
                                GetConsignmentSettlementDetailFromXML(dataRowFromXMl, consignmentSettlementDetail);
                                entity.ConsignmentSettlementDetails.Add(consignmentSettlementDetail);
                                entity.SaveChanges();


                            }
                        }
                    }

                    #endregion

                    #region Stock In Header

                    entity = new APP_Data.POSEntities();
                    if (dtxmlStockInHeader != null)
                    {
                        label1.Text = "Step 37 of 40 : Processing Stock In Header  table please wait!";
                        label1.Refresh();
                        Progressbar1.Minimum = 1;
                        Progressbar1.Maximum = dtxmlStockInHeader.Rows.Count;
                        Progressbar1.Value = 1;
                        Progressbar1.Step = 1;
                        //loop through dataRow come from xml and check if the Unit is already exist or Unit is new one
                        foreach (DataRow dataRowFromXMl in dtxmlStockInHeader.Rows)
                        {

                            Progressbar1.PerformStep();
                            bool SameRowExit = false;

                            string _stockCode = dataRowFromXMl["StockCode"].ToString();
                            int _toShopId = Convert.ToInt32(dataRowFromXMl["ToShopId"].ToString());

                            //select stock transfer list by stock code and realtive shop
                            APP_Data.StockInHeader FoundStockInHeader = entity.StockInHeaders.Where(x => x.StockCode == _stockCode  && x.ToShopId==_toShopId).FirstOrDefault();
                            if (FoundStockInHeader != null)
                            {
                                SameRowExit = true;
                            }

                           
                            if (SameRowExit)
                            {
                                APP_Data.StockInHeader NewStockInHeader = GetStockInHeaderFromXML(dataRowFromXMl, FoundStockInHeader, dtxmlStockInDetail, SameRowExit);
                                
                                if(NewStockInHeader != null)
                                { 
                                StockInHeader_ForeignKeyTBL(dataRowFromXMl, dtxmlStockInDetail, NewStockInHeader);
                                }
                            }
                            else
                            {
                                var _ownShopId = SettingController.DefaultShop.Id;
                                if(_ownShopId == _toShopId)
                                { 
                                
                                 APP_Data.StockInHeader NewStockInHeader=GetStockInHeaderFromXML(dataRowFromXMl,FoundStockInHeader, dtxmlStockInDetail, SameRowExit);


                                 StockInHeader_ForeignKeyTBL(dataRowFromXMl, dtxmlStockInDetail, NewStockInHeader);
                                }
                            }
                        }
                    }

                    #endregion

                    #region Stock In Detail

                    entity = new APP_Data.POSEntities();
                    if (dtxmlStockInDetail != null)
                    {
                        label1.Text = "Step 38 of 40 : Processing Stock In Detail  table please wait!";
                        label1.Refresh();
                        Progressbar1.Minimum = 1;
                        Progressbar1.Maximum = dtxmlStockInDetail.Rows.Count;
                        Progressbar1.Value = 1;
                        Progressbar1.Step = 1;
                        //loop through dataRow come from xml and check if the Unit is already exist or Unit is new one
                        foreach (DataRow dataRowFromXMl in dtxmlStockInDetail.Rows)
                        {

                            Progressbar1.PerformStep();
                            bool SameRowExit = false;

                            int _stockInHeaderId = Convert.ToInt32(dataRowFromXMl["StockInHeaderId"].ToString());
                            int _productId = Convert.ToInt32(dataRowFromXMl["ProductId"].ToString());
                            string _stockCode = dataRowFromXMl["StockCode"].ToString();
                            int _defaultShopId = SettingController.DefaultShop.Id;
                            int _toShopId = Convert.ToInt32(dataRowFromXMl["ToShopId"].ToString());
                          
                            APP_Data.StockInDetail FoundStockInDetail = (from sd in entity.StockInDetails
                                                                         join sh in entity.StockInHeaders on sd.StockInHeaderId equals sh.id
                                                                         where sd.StockInHeaderId == _stockInHeaderId && sd.ProductId == _productId
                                                                         && sh.StockCode == _stockCode
                                                                         select sd).FirstOrDefault();
                            if (FoundStockInDetail != null)
                            {
                                SameRowExit = true;
                            }
                            if (!SameRowExit)
                            {
                                //checking  own shop equal export shop
                                if (_defaultShopId == _toShopId)
                                {
                                    long? _alreadyStockInHeaderId = 0;

                                    //select stock in header  list by stockinheaderid(export StockInHeaderId)
                                     _alreadyStockInHeaderId=entity.StockInHeaders.Where(x => x.StockCode == _stockCode && x.Status != "StockIn").Select(x=>x.id).FirstOrDefault();

                                    List<StockInDetail> _existStockInDetail = new List<StockInDetail>();

                                    if (_alreadyStockInHeaderId != 0)
                                        ///select stock in detail list by stockinheaderid and productid
                                        _existStockInDetail = entity.StockInDetails.Where(x => x.StockInHeaderId == _alreadyStockInHeaderId && x.ProductId == _productId).ToList();


                                    if (_existStockInDetail.Count == 0)
                                    { 
                                    APP_Data.StockInDetail stockInDetail = new APP_Data.StockInDetail();
                                    GetStockInDetailFromXML(dataRowFromXMl, stockInDetail);
                                    entity.StockInDetails.Add(stockInDetail);
                                    entity.SaveChanges();
                                    }
                                }

                          
                            }
                        }
                    }
                    #endregion


                    #region GiftSystem

                    entity = new APP_Data.POSEntities();

                    if (dtxmlGiftSystem != null) {
                        //Loop through dataRow come from xml and check if the transaction is already exist or a new one
                        label1.Text = "Step 39 of 40 : Processing GiftSystem table please wait!";
                        label1.Refresh();
                        Progressbar1.Minimum = 1;
                        Progressbar1.Maximum = dtxmlGiftSystem.Rows.Count;
                        Progressbar1.Value = 1;
                        Progressbar1.Step = 1;

                        foreach (DataRow dataRowFromXML in dtxmlGiftSystem.Rows) {
                            Progressbar1.PerformStep();
                            bool IsSameRecord = false;
                            int primaryKeyId =Convert.ToInt32( dataRowFromXML["Id"].ToString());
                            //long ProductId = Convert.ToInt64(dataRowFromXML["ProductId"].ToString());
                            //Boolean IsDelete = Convert.ToBoolean(dataRowFromXML["IsDeleted"].ToString());
                            APP_Data.GiftSystem dataGiftSystem = entity.GiftSystems.Where(x => x.Id == primaryKeyId).FirstOrDefault();
                            //Same giftsystemid Found
                            if (dataGiftSystem != null) {
                                IsSameRecord = true;
                                }
                            //Found same row,Update the Current One if xml transactions have more recent updatedDate
                            if (IsSameRecord) {
                                GetGiftSystemFromXML(dataRowFromXML, dataGiftSystem);
                                entity.SaveChanges();
                                GiftSystem_ForeignKeyTBL(dataRowFromXML, dtxmlTransaction, dtxmlUser, dataGiftSystem);
                                }
                            //GiftSystem Id is not exist in current database
                            //add new row and saving to the database.
                            else {
                                APP_Data.GiftSystem giftSystem = new APP_Data.GiftSystem();
                                GetGiftSystemFromXML(dataRowFromXML, giftSystem);
                                entity.GiftSystems.Add(giftSystem);
                                entity.SaveChanges();
                                GiftSystem_ForeignKeyTBL(dataRowFromXML, dtxmlTransaction, dtxmlUser, dataGiftSystem);
                                }
                            }
                        }
                    #endregion

                    #region GiftSystemInTransaction
                    entity = new POSEntities();
                    //loop through dataRow come from xml and check if the city is already exist or brand is new one
                    if (dtxmlGiftSystemInTransaction != null) {
                        label1.Text = "Step 40 of 41 : Processing City table please wait!";
                        label1.Refresh();
                        Progressbar1.Minimum = 1;
                        Progressbar1.Maximum =dtxmlGiftSystemInTransaction.Rows.Count;
                        Progressbar1.Value = 1;
                        Progressbar1.Step = 1;

                        foreach (DataRow dataRowFromXML in dtxmlGiftSystemInTransaction.Rows) {

                            Progressbar1.PerformStep();
                            bool HasRecord = false;
                            //int Id = Convert.ToInt32(dataRowFromXML["Id"].ToString());
                            int GiftSystemId = Convert.ToInt32(dataRowFromXML["GiftSystemId"].ToString());
                            string TransactionId = (dataRowFromXML["TransactionId"].ToString());// x.Id == Id&&
                            GiftSystemInTransaction giftSystemInTransaction= entity.GiftSystemInTransactions.Where(x =>x.GiftSystemId==GiftSystemId&&x.TransactionId==TransactionId).FirstOrDefault();
                            //found same GiftSystemInTransactiondata 
                            if (giftSystemInTransaction != null) {
                                HasRecord = true;
                                }
                            //Found same row ,Update the Current One
                            if (HasRecord) {
                                GetGiftSystemInTransactionFromXML(dataRowFromXML, giftSystemInTransaction);
                                entity.Entry(giftSystemInTransaction).State = EntityState.Modified;
                                entity.SaveChanges();
                                }
                            //GiftSystemInTransactiondata  is not exist in current database
                            //add new row and Inserting the record to the database.
                            else {
                                GetGiftSystemInTransactionFromXML(dataRowFromXML, giftSystemInTransaction);
                                entity.GiftSystemInTransactions.Add(giftSystemInTransaction);
                                entity.SaveChanges();
                                }
                            }
                        }//end of for each.
                    #endregion



                    #region NoveltySystem

                    entity = new APP_Data.POSEntities();

                    if (dtxmlNoveltySystem != null) {
                        //loop through dataRow from xml and check if the UserRole is already exist or newone.
                        label1.Text = "Step 41 of 41 : Processing User Role table please wait!";
                        label1.Refresh();
                        Progressbar1.Minimum = 1;
                        Progressbar1.Maximum = dtxmlNoveltySystem.Rows.Count;
                        Progressbar1.Value = 1;
                        Progressbar1.Step = 1;
                        foreach (DataRow dataFromXML in dtxmlNoveltySystem.Rows) {
                            Progressbar1.PerformStep();
                            bool HaveSameRecord = false;
                            int Id = Convert.ToInt32(dataFromXML["Id"]);
                            APP_Data.NoveltySystem noveltySystem = entity.NoveltySystems.Where(x => x.Id == Id).SingleOrDefault();
                            //same Record found in database.
                            if (noveltySystem != null) {
                                HaveSameRecord = true;
                                }
                            //Found same row,Update the Current One
                            if (HaveSameRecord) {
                                GetNoveltySystemFromXML(dataFromXML, noveltySystem);
                                entity.Entry(noveltySystem).State = EntityState.Modified;
                                entity.SaveChanges();
                                }
                            //add new row and saving to the database.
                            else {
                                GetNoveltySystemFromXML(dataFromXML, noveltySystem);
                                entity.NoveltySystems.Add(noveltySystem);
                                entity.SaveChanges();
                                }
                            }
                        }


                    #endregion


                    #region ProductInNovelty

                    entity = new APP_Data.POSEntities();

                    if (dtxmlProductInNovelty != null) {
                        //loop through dataRow from xml and check if the UserRole is already exist or newone.
                        label1.Text = "Step 41 of 41 : Processing ProductInNovelty table please wait!";
                        label1.Refresh();
                        Progressbar1.Minimum = 1;
                        Progressbar1.Maximum = dtxmlProductInNovelty.Rows.Count;
                        Progressbar1.Value = 1;
                        Progressbar1.Step = 1;
                        foreach (DataRow dataFromXML in dtxmlProductInNovelty.Rows) {
                            Progressbar1.PerformStep();
                            bool HaveSameRecord = false;
                            int Id = Convert.ToInt32(dataFromXML["Id"]);
                            APP_Data.ProductInNovelty productInNovelty = entity.ProductInNovelties.Where(x => x.Id == Id).SingleOrDefault();
                            //same Record found in database.
                            if (productInNovelty != null) {
                                HaveSameRecord = true;
                                }
                            //Found same row,Update the Current One
                            if (HaveSameRecord) {
                                GetProductInNoveltyXML(dataFromXML, productInNovelty);
                                entity.Entry(productInNovelty).State = EntityState.Modified;
                                entity.SaveChanges();
                                }
                            //add new row and saving to the database.
                            else {
                                GetProductInNoveltyXML(dataFromXML, productInNovelty);
                                entity.ProductInNovelties.Add(productInNovelty);
                                entity.SaveChanges();
                                }
                            }
                        }


                    #endregion

                    label1.Visible = false;
                    Progressbar1.Visible = false;
                    File.Delete(destFileName);
                    MessageBox.Show("Data updating completed!","mpos",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }
        }

        private void GetSPDetailFromXML(DataRow dataFromXML, APP_Data.SPDetail foundSpDetail)
        {
            if (dataFromXML["ParentProductID"].ToString().Trim() != string.Empty && dataFromXML["ParentProductID"].ToString() != "")
            {
                foundSpDetail.ParentProductID = Convert.ToInt64(dataFromXML["ParentProductID"].ToString());
            }
            if (dataFromXML["ChildProductID"].ToString().Trim() != string.Empty && dataFromXML["ChildProductID"].ToString() != "")
            {
                foundSpDetail.ChildProductID = Convert.ToInt64(dataFromXML["ChildProductID"].ToString());
            }
            if (dataFromXML.Table.Columns.Contains("TransactionDetailID"))
            {
                if (dataFromXML["TransactionDetailID"].ToString().Trim() != string.Empty && dataFromXML["TransactionDetailID"].ToString() != "")
                {
                    foundSpDetail.TransactionDetailID = Convert.ToInt64(dataFromXML["TransactionDetailID"].ToString());
                }
            }
            foundSpDetail.SPDetailID = Convert.ToString(dataFromXML["SPDetailID"].ToString());

            if (dataFromXML["DiscountRate"].ToString().Trim() != string.Empty && dataFromXML["DiscountRate"].ToString() != "")
            {
                foundSpDetail.DiscountRate = Convert.ToDecimal(dataFromXML["DiscountRate"].ToString()); //Repair for error
            }
            if (dataFromXML["Price"].ToString().Trim() != string.Empty && dataFromXML["Price"].ToString() != "")
            {
                foundSpDetail.Price = Convert.ToInt64(dataFromXML["Price"].ToString());
            }
            if (dataFromXML["Price"].ToString().Trim() != string.Empty && dataFromXML["Price"].ToString() != "")
            {
                foundSpDetail.Price = Convert.ToInt64(dataFromXML["Price"].ToString());
            }
            if (dataFromXML["ChildQty"].ToString().Trim() != string.Empty && dataFromXML["ChildQty"].ToString() != "")
            {
                foundSpDetail.ChildQty = Convert.ToInt32(dataFromXML["ChildQty"].ToString());
            }
        }

        private void GetProductPriceChangeFromXML(DataRow dataFromXML, APP_Data.ProductPriceChange Foundpdc)
        {
            if (dataFromXML["ProductId"].ToString().Trim() != string.Empty && dataFromXML["ProductId"].ToString() != "")
            {
                Foundpdc.ProductId = Convert.ToInt64(dataFromXML["ProductId"].ToString());
            }
            if (dataFromXML["UpdateDate"].ToString().Trim() != string.Empty && dataFromXML["UpdateDate"].ToString() != "")
            {
                Foundpdc.UpdateDate = Convert.ToDateTime(dataFromXML["UpdateDate"].ToString());
            }
            if (dataFromXML["UserID"].ToString().Trim() != string.Empty && dataFromXML["UserID"].ToString() != "")
            {
                Foundpdc.UserID = Convert.ToInt32(dataFromXML["UserID"].ToString());
            }
            if (dataFromXML["Price"].ToString().Trim() != string.Empty && dataFromXML["Price"].ToString() != "")
            {
                Foundpdc.Price = Convert.ToInt64(dataFromXML["Price"].ToString());
            }
            if (dataFromXML["OldPrice"].ToString().Trim() != string.Empty && dataFromXML["OldPrice"].ToString() != "")
            {
                Foundpdc.OldPrice = Convert.ToInt64(dataFromXML["OldPrice"].ToString());
            }
        }

        private void GetWrapperItemFromXML(DataRow dataFromXML, APP_Data.WrapperItem foundWrapperItem)
        {
            if (dataFromXML["ChildProductId"].ToString().Trim() != String.Empty && dataFromXML["ChildProductId"].ToString() != "")
            {
                foundWrapperItem.ChildProductId = Convert.ToInt32(dataFromXML["ChildProductId"].ToString());
            }
            if (dataFromXML["ParentProductId"].ToString().Trim() != String.Empty && dataFromXML["ParentProductId"].ToString() != "")
            {
                foundWrapperItem.ParentProductId = Convert.ToInt32(dataFromXML["ParentProductId"].ToString());
            }
            if (dataFromXML["ChildQty"].ToString().Trim() != String.Empty && dataFromXML["ChildQty"].ToString() != "")
            {
                foundWrapperItem.ChildQty = Convert.ToInt32(dataFromXML["ChildQty"].ToString());
            }
            if (dataFromXML["IsDelete"].ToString().Trim() != String.Empty && dataFromXML["IsDelete"].ToString() != "")
            {
                foundWrapperItem.IsDelete = Convert.ToBoolean(dataFromXML["IsDelete"].ToString());
            }
        }
        
        private void UserPrePaidDebtFromXML(DataRow dataFromXML, APP_Data.UsePrePaidDebt FoundUserPrePaidDebt)
        {
            FoundUserPrePaidDebt.CreditTransactionId = dataFromXML["CreditTransactionId"].ToString();
            FoundUserPrePaidDebt.PrePaidDebtTransactionId = dataFromXML["PrePaidDebtTransactionId"].ToString();

            if (dataFromXML["UseAmount"].ToString() != string.Empty && dataFromXML["UseAmount"].ToString() != "")
            {
                FoundUserPrePaidDebt.UseAmount = Convert.ToInt32(dataFromXML["UseAmount"].ToString());
            }
            if (dataFromXML["CashierId"].ToString() != string.Empty && dataFromXML["CashierId"].ToString() != "")
            {
                FoundUserPrePaidDebt.CashierId = Convert.ToInt32(dataFromXML["CashierId"].ToString());
            }
            if (dataFromXML["CounterId"].ToString() != string.Empty && dataFromXML["CounterId"].ToString() != "")
            {
                FoundUserPrePaidDebt.CounterId = Convert.ToInt32(dataFromXML["CounterId"].ToString());
            }
        }

 
        private void GetRoleManagementFromXML(DataRow dataFromXML, APP_Data.RoleManagement FoundRoleManagement)
        {
            FoundRoleManagement.RuleFeature = dataFromXML["RuleFeature"].ToString();
            FoundRoleManagement.UserRoleId = Convert.ToInt32(dataFromXML["UserRoleId"].ToString());
            FoundRoleManagement.IsAllowed = Convert.ToBoolean(dataFromXML["IsAllowed"].ToString());
        }

        private void GetGiftCardFromXML(DataRow dataFromXML, APP_Data.GiftCard FoundGiftCard)
        {
            FoundGiftCard.CardNumber = dataFromXML["CardNumber"].ToString();
            if (dataFromXML["Amount"].ToString() != string.Empty && dataFromXML["Amount"].ToString() != "")
            {
                FoundGiftCard.Amount = Convert.ToInt64(dataFromXML["Amount"].ToString());
            }
                if (dataFromXML["IsDelete"].ToString() != string.Empty && dataFromXML["IsDelete"].ToString() != "")
            {
                FoundGiftCard.IsDelete = Convert.ToBoolean(dataFromXML["IsDelete"].ToString());
            }
			        
					
 }

        private void GetUserFromXML(DataRow dataFromXML, APP_Data.User FoundUser)
        {
            FoundUser.Name = dataFromXML["Name"].ToString();
            if (dataFromXML["UserRoleId"].ToString().Trim() != String.Empty || dataFromXML["UserRoleId"].ToString() == "")
            {
                FoundUser.UserRoleId = Convert.ToInt32(dataFromXML["UserRoleId"].ToString());
            }
            FoundUser.Password = dataFromXML["Password"].ToString();
            if (dataFromXML["DateTime"].ToString().Trim() != String.Empty || dataFromXML["DateTime"].ToString() == "")
            {
                FoundUser.DateTime = Convert.ToDateTime(dataFromXML["DateTime"].ToString());
            }
            FoundUser.UserCodeNo = dataFromXML["UserCodeNo"].ToString();
            FoundUser.ShopId = Convert.ToInt32(dataFromXML["ShopId"].ToString());
            FoundUser.MenuPermission = dataFromXML["MenuPermission"].ToString();
        }

        private void GetUserRoleFromXML(DataRow dataFromXML, APP_Data.UserRole FoundUserRole)
        {
            FoundUserRole.RoleName = dataFromXML["RoleName"].ToString();
        }

        private void GetSettingTypeFromXML(DataRow dataFromXML, APP_Data.Setting FoundSetting)
        {
            FoundSetting.Key = dataFromXML["Key"].ToString();
            FoundSetting.Value = dataFromXML["Value"].ToString();
        }

        private void GetPaymentTypeFromXML(DataRow dataFromXML, APP_Data.PaymentType FoundPaymentType)
        {
            FoundPaymentType.Name = dataFromXML["Name"].ToString();
        }

        private void GetExchangeReateForTansction(DataRow dataFromXML, APP_Data.ExchangeRateForTransaction FoundExchangeRate)
        {
            if (dataFromXML["CurrencyId"].ToString().Trim() != string.Empty && dataFromXML["CurrencyId"].ToString() != "")
            {
                FoundExchangeRate.CurrencyId = Convert.ToInt32(dataFromXML["CurrencyId"].ToString());
            }
            if (dataFromXML["ExchangeRate"].ToString().Trim() != string.Empty && dataFromXML["ExchangeRate"].ToString() != "")
            {
                FoundExchangeRate.ExchangeRate = Convert.ToInt32(dataFromXML["ExchangeRate"].ToString());
            }
            FoundExchangeRate.TransactionId = dataFromXML["TransactionId"].ToString();
        }

        private void GetCurrencyFromXML(DataRow dataFromXML, APP_Data.Currency FoundCurrency)
        {
            FoundCurrency.Country = dataFromXML["Country"].ToString();
            FoundCurrency.Symbol = dataFromXML["Symbol"].ToString();
            FoundCurrency.CurrencyCode = dataFromXML["CurrencyCode"].ToString();
            if (dataFromXML["LatestExchangeRate"].ToString().Trim() != string.Empty && dataFromXML["LatestExchangeRate"].ToString() != "")
            {
                FoundCurrency.LatestExchangeRate = Convert.ToInt32(dataFromXML["LatestExchangeRate"].ToString());
            }
        }

        private void GetMemberTypeFromXML(DataRow dataFromXML, APP_Data.MemberType FoundMemberType)
        {
            FoundMemberType.Name = dataFromXML["Name"].ToString();
          

            Boolean isdelete = Convert.ToBoolean(dataFromXML["IsDelete"].ToString());
            int membertypeid = Convert.ToInt32(dataFromXML["Id"].ToString());
          
            if (isdelete == true)
            {
                if ((from p in entity.Customers where p.MemberTypeID == membertypeid select p).FirstOrDefault() == null
                    && (from p in entity.MemberCardRules where p.MemberTypeId == membertypeid select p).FirstOrDefault() == null
                    )
                {
                    FoundMemberType.IsDelete = isdelete;
                }

            }
            else
            {
                FoundMemberType.IsDelete = isdelete;
            }
        }

        private void GetDeleteLogFromXML(DataRow dataFromXML, APP_Data.DeleteLog FoundDeleteLog)
        {
            if (dataFromXML["UserId"].ToString().Trim() != string.Empty && dataFromXML["UserId"].ToString() != "")
            {
                FoundDeleteLog.UserId = Convert.ToInt32(dataFromXML["UserId"]);
            }
            if (dataFromXML["CounterId"].ToString().Trim() != string.Empty && dataFromXML["CounterId"].ToString() != "")
            {
                FoundDeleteLog.CounterId = Convert.ToInt32(dataFromXML["CounterId"]);
            }
            if (dataFromXML.Table.Columns.Contains("TransactionId"))
            {
                FoundDeleteLog.TransactionId = dataFromXML["TransactionId"].ToString();
            }
            if (dataFromXML.Table.Columns.Contains("TransactionDetailId"))
            {
                if (dataFromXML["TransactionDetailId"].ToString().Trim() != string.Empty && dataFromXML["TransactionDetailId"].ToString() != "")
                {
                    FoundDeleteLog.TransactionDetailId = Convert.ToInt64(dataFromXML["TransactionDetailId"].ToString());
                }
            }
            if (dataFromXML.Table.Columns.Contains("IsParent"))
            {
                if (dataFromXML["IsParent"].ToString().Trim() != string.Empty && dataFromXML["IsParent"].ToString() != "")
                {
                    FoundDeleteLog.IsParent = Convert.ToBoolean(dataFromXML["IsParent"].ToString());
                }
            }
            if (dataFromXML["DeletedDate"].ToString().Trim() != string.Empty && dataFromXML["DeletedDate"].ToString() != "")
            {
                FoundDeleteLog.DeletedDate = Convert.ToDateTime(dataFromXML["DeletedDate"].ToString());
            }
        }

        private void GetTaxFromXML(DataRow dataFromXML, APP_Data.Tax FoundTax)
        {
            FoundTax.Name = dataFromXML["Name"].ToString();
            FoundTax.TaxPercent = Convert.ToDecimal(dataFromXML["TaxPercent"].ToString());
          

            Boolean isdelete = Convert.ToBoolean(dataFromXML["IsDelete"].ToString());
            int TaxId = Convert.ToInt32(dataFromXML["Id"].ToString());
         
            if (isdelete == true)
            {
                if ((from p in entity.Products where p.TaxId == TaxId select p).FirstOrDefault() == null)
                {
                    FoundTax.IsDelete = isdelete;
                }

            }
            else
            {
                FoundTax.IsDelete = isdelete;
            }
        }

        private void GetShopFromXML(DataRow dataFromXML, APP_Data.Shop FoundShop)
        {
            
            FoundShop.ShopName = dataFromXML["ShopName"].ToString();
            if (dataFromXML["Address1"].ToString().Trim() != string.Empty && dataFromXML["Address1"].ToString() != "")
            {
                FoundShop.Address = dataFromXML["Address"].ToString();
            }
            if (dataFromXML["PhoneNumber1"].ToString().Trim() != string.Empty && dataFromXML["PhoneNumber1"].ToString() != "")
            {
                FoundShop.PhoneNumber = dataFromXML["PhoneNumber"].ToString();
            }
            if (dataFromXML["OpeningHours1"].ToString().Trim() != string.Empty && dataFromXML["OpeningHours1"].ToString() != "")
            {
                FoundShop.OpeningHours = dataFromXML["OpeningHours"].ToString();
            }
            APP_Data.Shop shop = entity.Shops.Where(x => x.IsDefaultShop == true).FirstOrDefault();
            if (dataFromXML["CityId"].ToString().Trim() != string.Empty && dataFromXML["CityId"].ToString() != "")
            {
                FoundShop.CityId = Convert.ToInt16(dataFromXML["CityId"].ToString());
            }
            FoundShop.ShortCode = dataFromXML["ShortCode"].ToString();
            if (dataFromXML["IsDefaultShop"].ToString().Trim() != string.Empty && dataFromXML["IsDefaultShop"].ToString() != "")
            {

                //FoundShop.IsDefaultShop = Convert.ToBoolean(dataFromXML["IsDefaultShop"].ToString());
                if (FoundShop.ShopName == shop.ShopName)
                {
                    FoundShop.IsDefaultShop = true;
                }
                else
                {
                    FoundShop.IsDefaultShop = false;
                }
            }
        }

        private void GetUnitFromXML(DataRow dataRowFromXMl, APP_Data.Unit FoundUnit)
        {
            FoundUnit.UnitName = dataRowFromXMl["UnitName"].ToString();
        }

        private void GetExpenseCategoryFromXML(DataRow dataRowFromXMl, APP_Data.ExpenseCategory FoundExpenseCategory)
        {
            FoundExpenseCategory.Name = dataRowFromXMl["Name"].ToString();
          
            Boolean isdelete = Convert.ToBoolean(dataRowFromXMl["IsDelete"].ToString());
            int expensecatId = Convert.ToInt32(dataRowFromXMl["Id"].ToString());
        
            if (isdelete == true)
            {
                if ((from p in entity.Expenses where p.ExpenseCategoryId == expensecatId select p).FirstOrDefault() == null)
                {
                    FoundExpenseCategory.IsDelete = isdelete;
                }

            }
            else
            {
                FoundExpenseCategory.IsDelete = isdelete;
            }
        }

        private void GetExpenseFromXML(DataRow dataRowFromXMl, APP_Data.Expense FoundExpense)
        {
            FoundExpense.Id = dataRowFromXMl["Id"].ToString();
            FoundExpense.ExpenseDate = Convert.ToDateTime(dataRowFromXMl["ExpenseDate"].ToString());
            FoundExpense.IsApproved = Convert.ToBoolean(dataRowFromXMl["IsApproved"].ToString());
            FoundExpense.IsDeleted = Convert.ToBoolean(dataRowFromXMl["IsDeleted"].ToString());
            FoundExpense.CreatedDate = Convert.ToDateTime(dataRowFromXMl["CreatedDate"].ToString());
            FoundExpense.CreatedUser = Convert.ToInt32(dataRowFromXMl["CreatedUser"].ToString());
            if (dataRowFromXMl.Table.Columns.Contains("UpdatedDate"))
            {
                if (dataRowFromXMl["UpdatedDate"].ToString().Trim() != String.Empty && dataRowFromXMl["UpdatedDate"].ToString() != "")
                {
                    FoundExpense.UpdatedDate = Convert.ToDateTime(dataRowFromXMl["UpdatedDate"].ToString());
                }
            }
            if (dataRowFromXMl.Table.Columns.Contains("UpdatedUser"))
            {
                if (dataRowFromXMl["UpdatedUser"].ToString().Trim() != String.Empty && dataRowFromXMl["UpdatedUser"].ToString() != "")
                {
                    FoundExpense.UpdatedUser = Convert.ToInt32(dataRowFromXMl["UpdatedUser"].ToString());
                }
            }
            FoundExpense.TotalExpenseAmount = Convert.ToDecimal(dataRowFromXMl["TotalExpenseAmount"].ToString());
            FoundExpense.ExpenseCategoryId = Convert.ToInt32(dataRowFromXMl["ExpenseCategoryId"].ToString());
         //   FoundExpense.Count = Convert.ToInt32(dataRowFromXMl["Count"].ToString());
        }

        private void GetExpenseDetailFromXML(DataRow dataRowFromXMl, APP_Data.ExpenseDetail FoundExpenseDetail)
        {
            
            FoundExpenseDetail.ExpenseId = dataRowFromXMl["ExpenseId"].ToString();
            FoundExpenseDetail.Description = dataRowFromXMl["Description"].ToString();
            FoundExpenseDetail.Qty = Convert.ToDecimal(dataRowFromXMl["Qty"].ToString());
            FoundExpenseDetail.Price = Convert.ToDecimal(dataRowFromXMl["Price"].ToString());
          
        }


        private APP_Data.StockInHeader GetStockInHeaderFromXML(DataRow dataRowFromXMl, APP_Data.StockInHeader FoundStockInHeader, DataTable dtxmlStockInDetail,Boolean SameRowExit)
        {

            APP_Data.StockInHeader NewStockInHeader = new APP_Data.StockInHeader();

            //samerowexit means already had stock code in own database
            if (SameRowExit)
            {
                // Status is StockTransfer or StockReturn
                if(FoundStockInHeader.Status != "StockIn")
                {
                    return null;
                }
                    //already entry stock code by theirselves and haven't approved yet 
                else if (FoundStockInHeader.Status == "StockIn" && FoundStockInHeader.IsApproved== false)
                {
                    //if not exist stock code with status "StockTransfer" or "StockReturn" in own database
                    if (Check_ExistTransferOrReturn(FoundStockInHeader))
                    { 
                        //save in stockinheader table
                      NewStockInHeader=Save_NewStockInHeader(dataRowFromXMl);
                    }
                    else
                    {
                        NewStockInHeader = null;
                    }
                }
                //already entry stock code by theirselves and already approved 
                else if (FoundStockInHeader.Status == "StockIn" && FoundStockInHeader.IsApproved == true)
                {
                    //if not exist stock code with status "StockTransfer" or "StockReturn" in own database
                    if (Check_ExistTransferOrReturn(FoundStockInHeader))
                    { 
                        //Isdelete=true by stockcode with status "StockTransfer" or "StockReturn"
                         NewStockInHeader= Save_NewStockInHeader( dataRowFromXMl, true);
                    }
                    else
                    {
                        NewStockInHeader = null;
                    }
                }

             
            }
            else
            {
                NewStockInHeader=Save_NewStockInHeader(dataRowFromXMl);
            }
            return NewStockInHeader;
           
           
        }

        private bool Check_ExistTransferOrReturn(APP_Data.StockInHeader _foundStockInHeader)
        {
            
            string _stockCode = _foundStockInHeader.StockCode;

            var _filterList = entity.StockInHeaders.Where(x => x.StockCode == _stockCode && x.Status != "StockIn").ToList();
            if(_filterList.Count > 0)
            {
                return false ;
            }
            return true;
        }


        private APP_Data.StockInHeader Save_NewStockInHeader( DataRow dataRowFromXMl, bool isDelete = false)
        {
            
            APP_Data.StockInHeader NewStockInHeader = new APP_Data.StockInHeader();
            ///save new stock code
            NewStockInHeader.IsApproved = false;
            NewStockInHeader.IsDelete = isDelete;


            NewStockInHeader.StockCode = dataRowFromXMl["StockCode"].ToString();
            NewStockInHeader.Date = Convert.ToDateTime(dataRowFromXMl["Date"].ToString());
            NewStockInHeader.FromShopId = Convert.ToInt32(dataRowFromXMl["FromShopId"].ToString());
            NewStockInHeader.ToShopId = Convert.ToInt32(dataRowFromXMl["ToShopId"].ToString());

            NewStockInHeader.CreatedUser = Convert.ToInt32(dataRowFromXMl["CreatedUser"].ToString());
            NewStockInHeader.CreatedDate = Convert.ToDateTime(dataRowFromXMl["CreatedDate"].ToString());

            if (dataRowFromXMl.Table.Columns.Contains("UpdatedDate"))
            {
                if (dataRowFromXMl["UpdatedDate"].ToString().Trim() != String.Empty && dataRowFromXMl["UpdatedDate"].ToString() != "")
                {
                    NewStockInHeader.UpdatedDate = Convert.ToDateTime(dataRowFromXMl["UpdatedDate"].ToString());
                }
            }
            if (dataRowFromXMl.Table.Columns.Contains("UpdatedUser"))
            {
                if (dataRowFromXMl["UpdatedUser"].ToString().Trim() != String.Empty && dataRowFromXMl["UpdatedUser"].ToString() != "")
                {
                    NewStockInHeader.UpdatedUser = Convert.ToInt32(dataRowFromXMl["UpdatedUser"].ToString());
                }
            }
            NewStockInHeader.Status = dataRowFromXMl["Status"].ToString();


            entity.StockInHeaders.Add(NewStockInHeader);
            entity.SaveChanges();
            return NewStockInHeader;
        }

        private void GetStockInDetailFromXML(DataRow dataRowFromXMl, APP_Data.StockInDetail FoundStockInDetail)
        {

            FoundStockInDetail.StockInHeaderId = Convert.ToInt32(dataRowFromXMl["StockInHeaderId"].ToString());
            FoundStockInDetail.ProductId = Convert.ToInt32(dataRowFromXMl["ProductId"].ToString());
            FoundStockInDetail.Qty = Convert.ToInt32(dataRowFromXMl["Qty"].ToString());
            

        }

        private void GetMemberCardRuleFromXML(DataRow dataRowFromXMl, APP_Data.MemberCardRule FoundMemberCardRule)
        {
            
            FoundMemberCardRule.MemberTypeId = Convert.ToInt32(dataRowFromXMl["MemberTypeId"].ToString());
            FoundMemberCardRule.RangeFrom = dataRowFromXMl["RangeFrom"].ToString();
            FoundMemberCardRule.RangeTo = dataRowFromXMl["RangeTo"].ToString();
            FoundMemberCardRule.MCDiscount = Convert.ToInt32(dataRowFromXMl["MCDiscount"].ToString());
            FoundMemberCardRule.BDDiscount = Convert.ToInt32(dataRowFromXMl["BDDiscount"].ToString());

        }

        private void GetConsignmentSettlementFromXML(DataRow dataRowFromXMl, APP_Data.ConsignmentSettlement FoundConsignmentSettlement)
        {

            FoundConsignmentSettlement.SettlementDate = Convert.ToDateTime(dataRowFromXMl["SettlementDate"].ToString());
            FoundConsignmentSettlement.ConsignorId = Convert.ToInt32(dataRowFromXMl["ConsignorId"].ToString());
           // FoundConsignmentSettlement.TransactionDetailId = dataRowFromXMl["TransactionDetailId"].ToString();
            FoundConsignmentSettlement.TotalSettlementPrice = Convert.ToInt32(dataRowFromXMl["TotalSettlementPrice"].ToString());
            FoundConsignmentSettlement.CreatedDate = Convert.ToDateTime(dataRowFromXMl["CreatedDate"].ToString());
            FoundConsignmentSettlement.CreatedBy = Convert.ToInt32(dataRowFromXMl["CreatedBy"].ToString());
            FoundConsignmentSettlement.IsDelete = Convert.ToBoolean(dataRowFromXMl["IsDelete"].ToString());
            FoundConsignmentSettlement.ConsignmentNo = dataRowFromXMl["ConsignmentNo"].ToString();
            FoundConsignmentSettlement.FromTransactionDate = Convert.ToDateTime(dataRowFromXMl["FromTransactionDate"].ToString());
            FoundConsignmentSettlement.ToTransactionDate = Convert.ToDateTime(dataRowFromXMl["ToTransactionDate"].ToString());
            FoundConsignmentSettlement.Comment = dataRowFromXMl["Comment"].ToString();
        }

        private void GetConsignmentSettlementDetailFromXML(DataRow dataRowFromXMl, APP_Data.ConsignmentSettlementDetail FoundConsignmentSettlementDetail)
        {

            FoundConsignmentSettlementDetail.ConsignmentNo = dataRowFromXMl["ConsignmentNo"].ToString();
            FoundConsignmentSettlementDetail.TransactionDetailId = Convert.ToInt32(dataRowFromXMl["TransactionDetailId"].ToString());
          
        }

        private void GetProductSubCategoryFromXML(DataRow dataRowFromXML, APP_Data.ProductSubCategory FoundProductSubCategory)
        {
            FoundProductSubCategory.Name = dataRowFromXML["Name"].ToString();

            if (dataRowFromXML["ProductCategoryId"].ToString().Trim() != String.Empty && dataRowFromXML["ProductCategoryId"].ToString() != "")
            {
                FoundProductSubCategory.ProductCategoryId = Convert.ToInt32(dataRowFromXML["ProductCategoryId"].ToString());
            }

            Boolean isdelete = Convert.ToBoolean(dataRowFromXML["IsDelete"].ToString());
            int SubId = Convert.ToInt32(dataRowFromXML["Id"].ToString());
        
            if (isdelete == true)
            {
                if ((from p in entity.Products where p.ProductSubCategoryId == SubId select p).FirstOrDefault() == null)
                {
                    FoundProductSubCategory.IsDelete = isdelete;
                }

            }
            else
            {
                FoundProductSubCategory.IsDelete = isdelete;
            }
         
        }

        private void GetProductCategoryFromXML(DataRow dataRowFromXML, APP_Data.ProductCategory FoundProductCategory)
        {
            Boolean isdelete = Convert.ToBoolean(dataRowFromXML["IsDelete"].ToString());
            int pcId = Convert.ToInt32(dataRowFromXML["Id"].ToString());
   
            if (isdelete == true)
            {
                if ((from p in entity.Products where p.ProductCategoryId == pcId select p).FirstOrDefault() == null && (from p in entity.ProductSubCategories where p.ProductCategoryId == pcId select p).FirstOrDefault() == null)
                {
                    FoundProductCategory.IsDelete = isdelete;
                }

            }
            else
            {
                FoundProductCategory.IsDelete = isdelete;
            }
            FoundProductCategory.Name = dataRowFromXML["Name"].ToString();
        }

        private void GetCounterFromXML(DataRow dataRowFromXML, APP_Data.Counter Foundcounter)
        {
            Foundcounter.Name = dataRowFromXML["Name"].ToString();
            Foundcounter.IsDelete = Convert.ToBoolean(dataRowFromXML["IsDelete"].ToString());

            Boolean isdelete = Convert.ToBoolean(dataRowFromXML["IsDelete"].ToString());
            int counterid = Convert.ToInt32(dataRowFromXML["Id"].ToString());
    
            if (isdelete == true)
            {
                if ((from p in entity.Transactions where p.CounterId == counterid select p).FirstOrDefault() == null 
                    && (from p in entity.DeleteLogs where p.CounterId == counterid select p).FirstOrDefault() == null
                    && (from p in entity.PurchaseDeleteLogs where p.CounterId == counterid select p).FirstOrDefault() == null 
                   && ( from p in entity.UsePrePaidDebts where p.CounterId == counterid select p).FirstOrDefault() == null)
                {
                    Foundcounter.IsDelete = isdelete;
                }
            }
            else
            {
                Foundcounter.IsDelete = isdelete;
            }
        }

        private void GetConsignmentCounterFromXML(DataRow dataRowFromXML, APP_Data.ConsignmentCounter FoundConsignmentCounter)
        {
            Boolean isdelete = Convert.ToBoolean(dataRowFromXML["IsDelete"].ToString());
            int id = Convert.ToInt32(dataRowFromXML["Id"].ToString());
            FoundConsignmentCounter.Name = dataRowFromXML["Name"].ToString();
            FoundConsignmentCounter.IsDelete = Convert.ToBoolean(dataRowFromXML["IsDelete"].ToString());
            if (isdelete == true)
            {
                if ((from p in entity.Products where p.ConsignmentCounterId == id select p).FirstOrDefault() == null && (from p in entity.ConsignmentSettlements where p.ConsignorId == id select p).FirstOrDefault() == null)
                {
                    FoundConsignmentCounter.IsDelete = isdelete;
                }

            }
            else
            {
                FoundConsignmentCounter.IsDelete = isdelete;
            }
            // FoundConsignmentCounter.CounterLocation = dataRowFromXML["CounterLocation"].ToString();
            if (dataRowFromXML.Table.Columns.Contains("PhoneNo"))
            {
                FoundConsignmentCounter.PhoneNo = dataRowFromXML["PhoneNo"].ToString();
            }
            if (dataRowFromXML.Table.Columns.Contains("Email"))
            {
                FoundConsignmentCounter.Email = dataRowFromXML["Email"].ToString();
            }
            if (dataRowFromXML.Table.Columns.Contains("Address"))
            {
                FoundConsignmentCounter.Address = dataRowFromXML["Address"].ToString();
            }
        }

        private void GetCityFromXML(DataRow dataRowFromXML, APP_Data.City City)
        {
            Boolean isdelete = Convert.ToBoolean(dataRowFromXML["IsDelete"].ToString());
            int city = Convert.ToInt32(dataRowFromXML["Id"].ToString());
            City.CityName = dataRowFromXML["CityName"].ToString();
            if (isdelete == true)
            {
                if ((from p in entity.Customers where p.CityId == city select p).FirstOrDefault() == null && (from p in entity.Shops where p.CityId == city select p).FirstOrDefault() == null)
                  {
                      City.IsDelete = isdelete;
                  }
            }
            else
            {
                City.IsDelete = isdelete;
            }
        
        }

        private void GetGiftSystemInTransactionFromXML(DataRow dataRowFromXML,GiftSystemInTransaction giftSystemInTransaction) {
                        giftSystemInTransaction.GiftSystemId = Convert.ToInt32(dataRowFromXML["GiftSystemId"].ToString());
                        giftSystemInTransaction.TransactionId = dataRowFromXML["TransactionId"].ToString();            
            }
        private void GetNoveltySystemFromXML(DataRow dataRowFromXML, APP_Data.NoveltySystem noveltySystem) {
            noveltySystem.BrandId = Convert.ToInt32(dataRowFromXML["BrandId"].ToString());
            noveltySystem.ValidFrom =Convert.ToDateTime( dataRowFromXML["ValidFrom"].ToString());
            noveltySystem.ValidTo = Convert.ToDateTime(dataRowFromXML["ValidTo"].ToString());
            noveltySystem.UpdateDate = Convert.ToDateTime(dataRowFromXML["UpdateDate"].ToString());
            }
        private void GetProductInNoveltyXML(DataRow dataRowFromXML, APP_Data.ProductInNovelty productInNovelty) {
            productInNovelty.NoveltySystemId = Convert.ToInt32(dataRowFromXML["NoveltySystemId"].ToString());
            productInNovelty.ProductId = Convert.ToInt32(dataRowFromXML["ProductId"].ToString());
            productInNovelty.IsDeleted = Convert.ToBoolean(dataRowFromXML["IsDeleted"].ToString());
            }
        private void GetBrandFromXML(DataRow dataRowFromXML, APP_Data.Brand brand)
        {
            Boolean isdelete=Convert.ToBoolean(dataRowFromXML["IsDelete"].ToString());
            int brandId=Convert.ToInt32(dataRowFromXML["Id"].ToString());
            brand.Name = dataRowFromXML["Name"].ToString();
            if(isdelete==true)
            {
                if((from p in entity.Products where p.BrandId==brandId select p).FirstOrDefault()==null)
                {
                    brand.IsDelete = isdelete;
                }
            
            }
            else
            {
                brand.IsDelete = isdelete;
            }
           
        }

        private void GetProductFromXML(DataRow dataRowFromXML, APP_Data.Product product)
        {
            product.Name = dataRowFromXML["Name"].ToString();
            product.ProductCode = dataRowFromXML["ProductCode"].ToString();
            product.Barcode = dataRowFromXML["BarCode"].ToString();

            if (dataRowFromXML["Price"].ToString().Trim() != String.Empty && dataRowFromXML["Price"].ToString() != "")
            {
                product.Price = Convert.ToInt64(dataRowFromXML["Price"].ToString());
            }
            //   product.Qty = Convert.ToInt32(dataRowFromXML["Qty"].ToString());
            if (dataRowFromXML["BrandId"].ToString() != "0" && dataRowFromXML["BrandId"].ToString() != "")
            {
                product.BrandId = Convert.ToInt32(dataRowFromXML["BrandId"].ToString());
            }
            if (dataRowFromXML["ProductLocation"].ToString() != string.Empty && dataRowFromXML["ProductLocation"].ToString() != "")
            {
                product.ProductLocation = dataRowFromXML["ProductLocation"].ToString();
            }

            if (dataRowFromXML["ProductCategoryId"].ToString().Trim() != "0" && dataRowFromXML["ProductCategoryId"].ToString() != "")
            {
                product.ProductCategoryId = Convert.ToInt32(dataRowFromXML["ProductCategoryId"].ToString());
            }
            if (dataRowFromXML["ProductSubCategoryId"].ToString().Trim() != "0" && dataRowFromXML["ProductSubCategoryId"].ToString() != "")
            {
                product.ProductSubCategoryId = Convert.ToInt32(dataRowFromXML["ProductSubCategoryId"].ToString());
            }
            if (dataRowFromXML["UnitID"].ToString().Trim() != "0" && dataRowFromXML["UnitID"].ToString() != "")
            {
                product.UnitId = Convert.ToInt32(dataRowFromXML["UnitID"].ToString());
            }
            if (dataRowFromXML["TaxId"].ToString().Trim() != String.Empty && dataRowFromXML["TaxId"].ToString() != "")
            {
                product.TaxId = Convert.ToInt32(dataRowFromXML["TaxId"].ToString());
            }
            if (dataRowFromXML["MinStockQty"].ToString().Trim() != String.Empty && dataRowFromXML["MinStockQty"].ToString() != "")
            {
                product.MinStockQty = Convert.ToInt32(dataRowFromXML["MinStockQty"].ToString());
            }
            if (dataRowFromXML["DiscountRate"].ToString().Trim() != String.Empty && dataRowFromXML["DiscountRate"].ToString() != "")
            {
                product.DiscountRate = Convert.ToDecimal(dataRowFromXML["DiscountRate"].ToString());
            }
            if (dataRowFromXML["IsWrapper"].ToString().Trim() != String.Empty && dataRowFromXML["IsWrapper"].ToString() != "")
            {
                product.IsWrapper = Convert.ToBoolean(dataRowFromXML["IsWrapper"].ToString());
            }
            if (dataRowFromXML["IsConsignment"].ToString().Trim() != String.Empty && dataRowFromXML["IsConsignment"].ToString() != "")
            {
                product.IsConsignment = Convert.ToBoolean(dataRowFromXML["IsConsignment"].ToString());
            }
            if (dataRowFromXML["IsDiscontinue"].ToString().Trim() != String.Empty && dataRowFromXML["IsDiscontinue"].ToString() != "")
            {
                product.IsDiscontinue = Convert.ToBoolean(dataRowFromXML["IsDiscontinue"].ToString());
            }
          
            if (dataRowFromXML["ConsignmentPrice"].ToString().Trim() != String.Empty && dataRowFromXML["ConsignmentPrice"].ToString() != "")
            {
                product.ConsignmentPrice = Convert.ToInt64(dataRowFromXML["ConsignmentPrice"].ToString());
            }
            if (dataRowFromXML.Table.Columns.Contains("ConsignmentCounterId"))
            {
                if (dataRowFromXML["ConsignmentCounterId"].ToString().Trim() != "0" && dataRowFromXML["ConsignmentCounterId"].ToString() != "")
                {
                    product.ConsignmentCounterId = Convert.ToInt32(dataRowFromXML["ConsignmentCounterId"].ToString());
                }
            }
            if (dataRowFromXML["Size"].ToString().Trim() != String.Empty && dataRowFromXML["Size"].ToString() != "")
            {
                product.Size = dataRowFromXML["Size"].ToString();
            }
            if (dataRowFromXML["PurchasePrice"].ToString().Trim() != String.Empty && dataRowFromXML["PurchasePrice"].ToString() != "")
            {
                product.PurchasePrice = Convert.ToInt64(dataRowFromXML["PurchasePrice"].ToString());
            }
            if (dataRowFromXML["IsNotifyMinStock"].ToString().Trim() != String.Empty && dataRowFromXML["IsNotifyMinStock"].ToString() != "")
            {
                product.IsNotifyMinStock = Convert.ToBoolean(dataRowFromXML["IsNotifyMinStock"].ToString());
            }
            if (dataRowFromXML.Table.Columns.Contains("CreatedDate"))
            {
                if (dataRowFromXML["CreatedDate"].ToString().Trim() != String.Empty && dataRowFromXML["CreatedDate"].ToString() != "")
                {
                    product.CreatedDate = Convert.ToDateTime(dataRowFromXML["CreatedDate"].ToString());
                }
            }
            if (dataRowFromXML.Table.Columns.Contains("UpdatedBy"))
            {
                if (dataRowFromXML["UpdatedBy"].ToString().Trim() != String.Empty && dataRowFromXML["UpdatedBy"].ToString() != "")
                {
                    product.UpdatedBy = Convert.ToInt32(dataRowFromXML["UpdatedBy"].ToString());
                }
            }
            if (dataRowFromXML.Table.Columns.Contains("CreatedBy"))
            {
                if (dataRowFromXML["CreatedBy"].ToString().Trim() != String.Empty && dataRowFromXML["CreatedBy"].ToString() != "")
                {
                    product.CreatedBy = Convert.ToInt32(dataRowFromXML["CreatedBy"].ToString());
                }
            }
            if (dataRowFromXML.Table.Columns.Contains("UpdatedDate"))
            {
                if (dataRowFromXML["UpdatedDate"].ToString().Trim() != String.Empty && dataRowFromXML["UpdatedDate"].ToString() != "")
                {
                    product.UpdatedDate = Convert.ToDateTime(dataRowFromXML["UpdatedDate"].ToString());
                }
            }
         
        
           
        
        }

        private void GetTransactionFromXML(DataRow dataRowFromXML, APP_Data.Transaction transaction)
        {
            transaction.Id = Convert.ToString(dataRowFromXML["Id"].ToString());

            if (dataRowFromXML["DateTime"].ToString().Trim() != string.Empty && dataRowFromXML["DateTime"].ToString() != "")
            {
                transaction.DateTime = Convert.ToDateTime(dataRowFromXML["DateTime"].ToString());
            }
            if(dataRowFromXML.Table.Columns.Contains("UpdatedDate"))
            { 
            if (dataRowFromXML["UpdatedDate"].ToString().Trim() != string.Empty && dataRowFromXML["UpdatedDate"].ToString() != "")
            {
                transaction.UpdatedDate = Convert.ToDateTime(dataRowFromXML["UpdatedDate"].ToString());
            }
             }
        
            if (dataRowFromXML["UserId"].ToString().Trim() != string.Empty && dataRowFromXML["UserId"].ToString() != "")
            {
                transaction.UserId = Convert.ToInt32(dataRowFromXML["UserId"].ToString());
            }
            if (dataRowFromXML["CounterId"].ToString().Trim() != string.Empty && dataRowFromXML["CounterId"].ToString() != "")
            {
                transaction.CounterId = Convert.ToInt32(dataRowFromXML["CounterId"].ToString());
            }
            transaction.Type = Convert.ToString(dataRowFromXML["Type"].ToString());

            if (dataRowFromXML["IsPaid"].ToString().Trim() != string.Empty && dataRowFromXML["IsPaid"].ToString() != "")
            {
                transaction.IsPaid = Convert.ToBoolean(dataRowFromXML["IsPaid"].ToString());
            }
            if (dataRowFromXML["IsComplete"].ToString().Trim() != string.Empty && dataRowFromXML["IsComplete"].ToString() != "")
            {
                transaction.IsComplete = Convert.ToBoolean(dataRowFromXML["IsComplete"].ToString());
            }
            if (dataRowFromXML["IsActive"].ToString().Trim() != string.Empty && dataRowFromXML["IsActive"].ToString() != "")
            {
                transaction.IsActive = Convert.ToBoolean(dataRowFromXML["IsActive"].ToString());
            }
            if (dataRowFromXML["IsDeleted"].ToString().Trim() != string.Empty && dataRowFromXML["IsDeleted"].ToString() != "")
            {
                transaction.IsDeleted = Convert.ToBoolean(dataRowFromXML["IsDeleted"].ToString());
            }
           
            if (dataRowFromXML["PaymentTypeId"].ToString().Trim() != string.Empty && dataRowFromXML["PaymentTypeId"].ToString() != "")
            {
                transaction.PaymentTypeId = Convert.ToInt32(dataRowFromXML["PaymentTypeId"].ToString());
            }
            if (dataRowFromXML["TaxAmount"].ToString().Trim() != string.Empty && dataRowFromXML["TaxAmount"].ToString() != "")
            {
                transaction.TaxAmount = Convert.ToInt32(dataRowFromXML["TaxAmount"].ToString());
            }
           
         
            if (dataRowFromXML["DiscountAmount"].ToString().Trim() != string.Empty && dataRowFromXML["DiscountAmount"].ToString() != "")
            {
                transaction.DiscountAmount = Convert.ToInt32(dataRowFromXML["DiscountAmount"].ToString());
            }
            if (dataRowFromXML.Table.Columns.Contains("ParentId1"))
            {
                if (dataRowFromXML["ParentId1"].ToString().Trim() != string.Empty && dataRowFromXML["ParentId1"].ToString() != "")
                {
                    string type = dataRowFromXML["Type"].ToString();
                    if (type=="Refund" || type=="CreditRefund")
                    {
                        transaction.ParentId = null;
                    }
                    else
                    {
                        transaction.ParentId = dataRowFromXML["ParentId1"].ToString();
                    }
                    
                }
            }
            if (dataRowFromXML["CustomerId"].ToString().Trim() != string.Empty && dataRowFromXML["CustomerId"].ToString() != "")
            {
                transaction.CustomerId = Convert.ToInt32(dataRowFromXML["CustomerId"].ToString());
            }
            if (dataRowFromXML.Table.Columns.Contains("ReceivedCurrencyId"))
            {
                if (dataRowFromXML["ReceivedCurrencyId"].ToString().Trim() != string.Empty && dataRowFromXML["ReceivedCurrencyId"].ToString() != "")
                {
                    transaction.ReceivedCurrencyId = Convert.ToInt32(dataRowFromXML["ReceivedCurrencyId"].ToString());
                }
            }
            if (dataRowFromXML["ShopId"].ToString().Trim() != string.Empty && dataRowFromXML["ShopId"].ToString() != "")
            {
                transaction.ShopId = Convert.ToInt32(dataRowFromXML["ShopId"].ToString());
            }

            if (dataRowFromXML["MCDiscountAmt"].ToString().Trim() != string.Empty && dataRowFromXML["MCDiscountAmt"].ToString() != "")
            {
                transaction.MCDiscountAmt = Convert.ToDecimal(dataRowFromXML["MCDiscountAmt"].ToString());
            }

            if (dataRowFromXML["BDDiscountAmt"].ToString().Trim() != string.Empty && dataRowFromXML["BDDiscountAmt"].ToString() != "")
            {
                transaction.BDDiscountAmt = Convert.ToDecimal(dataRowFromXML["BDDiscountAmt"].ToString());
            }

            if (dataRowFromXML["MemberTypeId"].ToString().Trim() != string.Empty && dataRowFromXML["MemberTypeId"].ToString() != "")
            {
                transaction.MemberTypeId = Convert.ToInt32(dataRowFromXML["MemberTypeId"].ToString());
            }

            if (dataRowFromXML["MCDiscountPercent"].ToString().Trim() != string.Empty && dataRowFromXML["MCDiscountPercent"].ToString() != "")
            {
                transaction.MCDiscountPercent = Convert.ToDecimal(dataRowFromXML["MCDiscountPercent"].ToString());
            }

            if (dataRowFromXML["IsSettlement"].ToString().Trim() != string.Empty && dataRowFromXML["IsSettlement"].ToString() != "")
            {
                transaction.IsSettlement = Convert.ToBoolean(dataRowFromXML["IsSettlement"].ToString());
            }

                transaction.TranVouNos = dataRowFromXML["TranVouNos1"].ToString();
         
                transaction.IsWholeSale = Convert.ToBoolean(dataRowFromXML["IsWholeSale"].ToString());
         
            if (dataRowFromXML["GiftCardAmount"].ToString().Trim() != string.Empty && dataRowFromXML["GiftCardAmount"].ToString() != "")
            {
                transaction.GiftCardAmount = Convert.ToDecimal(dataRowFromXML["GiftCardAmount"].ToString());
            }

          

            if (dataRowFromXML["TotalAmount"].ToString().Trim() != string.Empty && dataRowFromXML["TotalAmount"].ToString() != "")
            {
                transaction.TotalAmount = Convert.ToInt64(dataRowFromXML["TotalAmount"].ToString());
            }

            if (dataRowFromXML["RecieveAmount"].ToString().Trim() != string.Empty && dataRowFromXML["RecieveAmount"].ToString() != "")
            {
                transaction.RecieveAmount = Convert.ToInt64(dataRowFromXML["RecieveAmount"].ToString());
            }

            if (dataRowFromXML["ReceivedCurrencyId"].ToString().Trim() != string.Empty && dataRowFromXML["ReceivedCurrencyId"].ToString() != "")
            {
                transaction.ReceivedCurrencyId = Convert.ToInt32(dataRowFromXML["ReceivedCurrencyId"].ToString());
            }
            if (dataRowFromXML["Note"].ToString().Trim() != string.Empty && dataRowFromXML["Note"].ToString() != "")
            {
                transaction.Note = (dataRowFromXML["Note"].ToString());
            }

        }

        private void GetTransactionDetailFromXML(DataRow dataRowFromXML, APP_Data.TransactionDetail transactionDetail)
        {
            transactionDetail.TransactionId = Convert.ToString(dataRowFromXML["TransactionId"].ToString());

            if (dataRowFromXML["ProductId"].ToString().Trim() != string.Empty && dataRowFromXML["ProductId"].ToString() != "")
            {
                transactionDetail.ProductId = Convert.ToInt64(dataRowFromXML["ProductId"].ToString());
            }
            if (dataRowFromXML["Qty"].ToString().Trim() != string.Empty && dataRowFromXML["Qty"].ToString() != "")
            {
                transactionDetail.Qty = Convert.ToInt32(dataRowFromXML["Qty"].ToString());
            }
            if (dataRowFromXML["UnitPrice"].ToString().Trim() != string.Empty && dataRowFromXML["UnitPrice"].ToString() != "")
            {
                transactionDetail.UnitPrice = Convert.ToInt64(dataRowFromXML["UnitPrice"].ToString());
            }
            if (dataRowFromXML["DiscountRate"].ToString().Trim() != string.Empty && dataRowFromXML["DiscountRate"].ToString() != "")
            {
                transactionDetail.DiscountRate = Convert.ToDecimal(dataRowFromXML["DiscountRate"].ToString());
            }
            if (dataRowFromXML["TaxRate"].ToString().Trim() != string.Empty && dataRowFromXML["TaxRate"].ToString() != "")
            {
                transactionDetail.TaxRate = Convert.ToDecimal(dataRowFromXML["TaxRate"].ToString());
            }
            if (dataRowFromXML["TotalAmount"].ToString().Trim() != string.Empty && dataRowFromXML["TotalAmount"].ToString() != "")
            {
                transactionDetail.TotalAmount = Convert.ToInt64(dataRowFromXML["TotalAmount"].ToString());
            }
            if (dataRowFromXML.Table.Columns.Contains("IsDeleted"))
            {
                if (dataRowFromXML["IsDeleted"].ToString().Trim() != string.Empty && dataRowFromXML["IsDeleted"].ToString() != "")
                {
                    transactionDetail.IsDeleted = Convert.ToBoolean(dataRowFromXML["IsDeleted"].ToString());
                }
            }
            if (dataRowFromXML["ConsignmentPrice"].ToString().Trim() != string.Empty && dataRowFromXML["ConsignmentPrice"].ToString() != "")
            {
                transactionDetail.ConsignmentPrice = Convert.ToInt64(dataRowFromXML["ConsignmentPrice"].ToString());
            }
            if (dataRowFromXML["IsConsignmentPaid1"].ToString().Trim() != string.Empty && dataRowFromXML["IsConsignmentPaid1"].ToString() != "")
            {
                transactionDetail.IsConsignmentPaid = Convert.ToBoolean(dataRowFromXML["IsConsignmentPaid1"].ToString());
            }
            if (dataRowFromXML["IsFOC"].ToString().Trim() != string.Empty && dataRowFromXML["IsFOC"].ToString() != "")
            {
                transactionDetail.IsFOC = Convert.ToBoolean(dataRowFromXML["IsFOC"].ToString());
            }
            if (dataRowFromXML["SellingPrice"].ToString().Trim() != string.Empty && dataRowFromXML["SellingPrice"].ToString() != "")
            {
                transactionDetail.SellingPrice = Convert.ToInt64(dataRowFromXML["SellingPrice"].ToString());
            }
        }

        private void GetCustomerFromXML(DataRow dataRowFromXML, APP_Data.Customer customer)
        {

            if (dataRowFromXML["Title"].ToString().Trim() != String.Empty && dataRowFromXML["Title"].ToString() != "")
            {
                customer.Title = Convert.ToString(dataRowFromXML["Title"].ToString());
            }
            customer.Name = Convert.ToString(dataRowFromXML["Name"].ToString());
            customer.PhoneNumber = Convert.ToString(dataRowFromXML["PhoneNumber"].ToString());
            customer.Address = Convert.ToString(dataRowFromXML["Address"].ToString());
            customer.NRC = Convert.ToString(dataRowFromXML["NRC"].ToString());
            customer.Email = Convert.ToString(dataRowFromXML["Email"].ToString());

            if (dataRowFromXML["CityId"].ToString().Trim() != String.Empty && dataRowFromXML["CityId"].ToString() != "")
            {
                customer.CityId = Convert.ToInt32(dataRowFromXML["CityId"].ToString());
            }
            if (dataRowFromXML.Table.Columns.Contains("Gender"))
            {
                if (dataRowFromXML["Gender"].ToString() != "")
                {
                    customer.Gender = Convert.ToString(dataRowFromXML["Gender"].ToString());
                }
            }
            if (dataRowFromXML.Table.Columns.Contains("Birthday"))
            {
                if (dataRowFromXML["Birthday"].ToString() != "")
                {
                    customer.Birthday = Convert.ToDateTime(dataRowFromXML["Birthday"].ToString());
                }
            }
        
           
            if (dataRowFromXML.Table.Columns.Contains("StartDate"))
            {
                if (dataRowFromXML["StartDate"].ToString() != "")
                {
                    customer.StartDate = Convert.ToDateTime(dataRowFromXML["StartDate"].ToString());
                }
            }
            if (dataRowFromXML.Table.Columns.Contains("VIPMemberId"))
            {
                if (dataRowFromXML["VIPMemberId"].ToString() != "")
                {
                    customer.VIPMemberId = Convert.ToString(dataRowFromXML["VIPMemberId"].ToString());
                }
            }
    

            if (dataRowFromXML.Table.Columns.Contains("CustomerCode"))
            {
                if (dataRowFromXML["CustomerCode"].ToString() != "")
                {
                    customer.CustomerCode = dataRowFromXML["CustomerCode"].ToString();
                }
            }
            if (dataRowFromXML.Table.Columns.Contains("MemberTypeId"))
            {
                if (dataRowFromXML["MemberTypeId"].ToString() != "")
                {
                    customer.MemberTypeID = Convert.ToInt32(dataRowFromXML["MemberTypeId"].ToString());
                }
            }

        }

        private void GetGiftSystemFromXML(DataRow dataRowFromXML, APP_Data.GiftSystem giftSystem) {

            if (dataRowFromXML["Type"].ToString().Trim() != String.Empty && dataRowFromXML["Type"].ToString() != "") {
                giftSystem.Type = Convert.ToString(dataRowFromXML["Type"].ToString());
                }
            giftSystem.Name = Convert.ToString(dataRowFromXML["Name"].ToString());
            giftSystem.MustBuyCostFrom = Convert.ToInt64(dataRowFromXML["MustBuyCostFrom"].ToString());
            giftSystem.MustBuyCostTo = Convert.ToInt64(dataRowFromXML["MustBuyCostTo"].ToString());
          

            //if (dataRowFromXML["MustIncludeProductId"].ToString() != "") { 
            //    giftSystem.MustIncludeProductId = Convert.ToInt64(dataRowFromXML["MustIncludeProductId"].ToString());
            //}

            if (dataRowFromXML.Table.Columns.Contains("FilterBrandId")) { 
            if (dataRowFromXML["FilterBrandId"].ToString() != "")
                giftSystem.FilterBrandId = Convert.ToInt32(dataRowFromXML["FilterBrandId"].ToString());
            }
            if (dataRowFromXML.Table.Columns.Contains("FilterCategoryId")) {
                if (dataRowFromXML["FilterBrandId"].ToString() != "")
                    giftSystem.FilterCategoryId = Convert.ToInt32(dataRowFromXML["FilterCategoryId"].ToString());
                }
            if (dataRowFromXML.Table.Columns.Contains("FilterSubCategoryId")) {
                if (dataRowFromXML["FilterBrandId"].ToString() != "")
                    giftSystem.FilterSubCategoryId = Convert.ToInt32(dataRowFromXML["FilterSubCategoryId"].ToString());
                }

            if (dataRowFromXML.Table.Columns.Contains("ValidFrom")) {
                if (dataRowFromXML["ValidFrom"].ToString() != "") {
                    giftSystem.ValidFrom = Convert.ToDateTime(dataRowFromXML["ValidFrom"].ToString());
                    }
                }


            if (dataRowFromXML.Table.Columns.Contains("ValidTo")) {
                if (dataRowFromXML["ValidTo"].ToString() != "") {
                    giftSystem.ValidTo = Convert.ToDateTime(dataRowFromXML["ValidTo"].ToString());
                    }
                }
            if (dataRowFromXML.Table.Columns.Contains("UsePromotionQty")) {
                if (dataRowFromXML["UsePromotionQty"].ToString() != "") {
                    giftSystem.UsePromotionQty = Convert.ToBoolean(dataRowFromXML["UsePromotionQty"].ToString());
                    }
                }


            if (dataRowFromXML.Table.Columns.Contains("PromotionQty")) {
                if (dataRowFromXML["PromotionQty"].ToString() != "") {
                    giftSystem.PromotionQty = Convert.ToInt32(dataRowFromXML["PromotionQty"].ToString());
                    }
                }
            if (dataRowFromXML.Table.Columns.Contains("GiftProductId")) {
                if (dataRowFromXML["GiftProductId"].ToString() != "") {
                    giftSystem.GiftProductId = Convert.ToInt32(dataRowFromXML["GiftProductId"].ToString());
                    }
                }
            if (dataRowFromXML.Table.Columns.Contains("PriceForGiftProduct")) {
                if (dataRowFromXML["PriceForGiftProduct"].ToString() != "") {
                    giftSystem.PriceForGiftProduct = Convert.ToInt32(dataRowFromXML["PriceForGiftProduct"].ToString());
                    }
                }
            if (dataRowFromXML.Table.Columns.Contains("GiftCashAmount")) {
                if (dataRowFromXML["GiftCashAmount"].ToString() != "") {
                    giftSystem.GiftCashAmount = Convert.ToInt32(dataRowFromXML["GiftCashAmount"].ToString());
                    }
                }
            if (dataRowFromXML.Table.Columns.Contains("DiscountPercentForTransaction")) {
                if (dataRowFromXML["DiscountPercentForTransaction"].ToString() != "") {
                    giftSystem.DiscountPercentForTransaction = Convert.ToInt32(dataRowFromXML["DiscountPercentForTransaction"].ToString());
                    }
                }

            if (dataRowFromXML.Table.Columns.Contains("UseBrandFilter")) {
                if (dataRowFromXML["UseBrandFilter"].ToString() != "") {
                    giftSystem.UseBrandFilter = Convert.ToBoolean(dataRowFromXML["UseBrandFilter"].ToString());
                    }
                }
            //
            if (dataRowFromXML.Table.Columns.Contains("UseCategoryFilter")) {
                if (dataRowFromXML["UseCategoryFilter"].ToString() != "") {
                    giftSystem.UseCategoryFilter = Convert.ToBoolean(dataRowFromXML["UseCategoryFilter"].ToString());
                    }
                }
                if (dataRowFromXML.Table.Columns.Contains("UseSubCategoryFilter")) {
                    if (dataRowFromXML["UseSubCategoryFilter"].ToString() != "") {
                        giftSystem.UseSubCategoryFilter = Convert.ToBoolean(dataRowFromXML["UseSubCategoryFilter"].ToString());
                        }
                    }
                    if (dataRowFromXML.Table.Columns.Contains("UseProductFilter")) {
                        if (dataRowFromXML["UseProductFilter"].ToString() != "") {
                            giftSystem.UseProductFilter = Convert.ToBoolean(dataRowFromXML["UseProductFilter"].ToString());
                            }
                        }
                        if (dataRowFromXML.Table.Columns.Contains("IsActive")) {
                            if (dataRowFromXML["IsActive"].ToString() != "") {
                                giftSystem.IsActive = Convert.ToBoolean(dataRowFromXML["IsActive"].ToString());
                                }
                            }
                            if (dataRowFromXML.Table.Columns.Contains("UseSizeFilter")) {
                                if (dataRowFromXML["UseSizeFilter"].ToString() != "") {
                                    giftSystem.UseSizeFilter = Convert.ToBoolean(dataRowFromXML["UseSizeFilter"].ToString());
                                    }
                                }
                                if (dataRowFromXML.Table.Columns.Contains("UseQtyFilter")) {
                                    if (dataRowFromXML["UseQtyFilter"].ToString() != "") {
                                        giftSystem.UseQtyFilter = Convert.ToBoolean(dataRowFromXML["UseQtyFilter"].ToString());
                                        }
                                    }
                                        if (dataRowFromXML.Table.Columns.Contains("FilterSize")) {
                                            if (dataRowFromXML["FilterSize"].ToString() != "") {
                                                giftSystem.FilterSize = Convert.ToInt32(dataRowFromXML["FilterSize"].ToString());
                                                }
                                            }

            }

        #region Relation Foreign key table
        //User Role
        private void UserRole_ForeignKeyTBL(DataRow dataFromXML, DataTable dtxmlUser, DataTable dtxmlRoleManagement, APP_Data.UserRole FoundUserRole)
        {
            string OldUserRoleId = dataFromXML["Id"].ToString();
            if (dtxmlUser != null)
            {
                foreach (DataRow o in dtxmlUser.Select("UserRoleId='" + OldUserRoleId + "'"))
                {
                    o["UserRoleId"] = FoundUserRole.Id.ToString();
                }
            }
            if (dtxmlRoleManagement != null)
            {
                foreach (DataRow o in dtxmlRoleManagement.Select("UserRoleId='" + OldUserRoleId + "'"))
                {
                    o["UserRoleId"] = FoundUserRole.Id.ToString();
                }
            }

        }

        //User
        private void User_ForeignKeyTBL(DataRow dataFromXML, DataTable dtxmlConsignmentSettlement, DataTable dtxmlDeleteLog, DataTable dtxmlExpense, DataTable dtxmlProduct ,
            DataTable dtxmlProductPriceChange, DataTable dtxmlProductQuantityChange, DataTable dtxmlStockInHeader, DataTable dtxmlTransaction, DataTable dtxmlUnitConversion,
            DataTable dtxmlUserPrePaidDebt, APP_Data.User FoundUser)

        {
            string OldUserId = dataFromXML["Id"].ToString();
            if (dtxmlTransaction != null)
            {
                foreach (DataRow o in dtxmlTransaction.Select("UserId='" + OldUserId + "'" ))
                {
                    o["UserId"] = FoundUser.Id.ToString();
                }
            }
            if (dtxmlUserPrePaidDebt != null)
            {
                foreach (DataRow o in dtxmlUserPrePaidDebt.Select("CashierId='" + OldUserId + "'"))
                {
                    o["CashierId"] = FoundUser.Id.ToString();
                }
            }
            if (dtxmlDeleteLog != null)
            {
                if (dtxmlDeleteLog != null)
                {
                    foreach (DataRow o in dtxmlDeleteLog.Select("UserId='" + OldUserId + "'"))
                    {
                        o["UserId"] = FoundUser.Id.ToString();
                    }
                }
            }
            if (dtxmlConsignmentSettlement != null)
            {
                foreach (DataRow o in dtxmlConsignmentSettlement.Select("CreatedBy='" + OldUserId + "'"))
                {
                    o["CreatedBy"] = FoundUser.Id.ToString();
                }
            }
            if (dtxmlProductPriceChange != null)
            {
                foreach (DataRow o in dtxmlProductPriceChange.Select("UserID ='" + OldUserId + "'"))
                {
                    o["UserID"] = FoundUser.Id.ToString();
                }
            }
            if (dtxmlExpense != null)
            {
                foreach (DataRow o in dtxmlExpense.Select("CreatedUser ='" + OldUserId + "'"))
                {
                    o["CreatedUser"] = FoundUser.Id.ToString();
                }
            }
            if (dtxmlProduct != null && dtxmlProduct.Columns.Contains("CreatedBy1") && dtxmlProduct.Columns["CreatedBy1"].ToString() != string.Empty)
            {
                foreach (DataRow o in dtxmlProduct.Select("CreatedBy1 ='" + OldUserId + "'"))
                {
                    o["CreatedBy1"] = FoundUser.Id.ToString();

                }
            }
            if (dtxmlProduct != null && dtxmlProduct.Columns.Contains("UpdatedBy1") && dtxmlProduct.Columns["UpdatedBy1"].ToString() != string.Empty)
            {
                foreach (DataRow o in dtxmlProduct.Select("UpdatedBy1 ='" + OldUserId + "'"))
                {
                    o["UpdatedBy1"] = FoundUser.Id.ToString();
                }

            }
            if (dtxmlProductQuantityChange != null)
            {
                foreach (DataRow o in dtxmlProductQuantityChange.Select("UserId ='" + OldUserId + "'"))
                {
                    o["UserId"] = FoundUser.Id.ToString();
                }
            }

            if (dtxmlStockInHeader != null)
            {
                foreach (DataRow o in dtxmlStockInHeader.Select("CreatedUser ='" + OldUserId + "'"))
                {
                    o["CreatedUser"] = FoundUser.Id.ToString();
                }

                foreach (DataRow o in dtxmlStockInHeader.Select("UpdatedUser ='" + OldUserId + "'"))
                {
                    o["UpdatedUser"] = FoundUser.Id.ToString();
                }
            }

            if (dtxmlTransaction != null)
            {
                foreach (DataRow o in dtxmlTransaction.Select("UserId ='" + OldUserId + "'"))
                {
                    o["UserId"] = FoundUser.Id.ToString();
                }
            }

            if (dtxmlUnitConversion != null)
            {
                foreach (DataRow o in dtxmlUnitConversion.Select("CreatedBy ='" + OldUserId + "'"))
                {
                    o["CreatedBy"] = FoundUser.Id.ToString();
                }

                foreach (DataRow o in dtxmlUnitConversion.Select("UpdatedBy ='" + OldUserId + "'"))
                {
                    o["UpdatedBy"] = FoundUser.Id.ToString();
                }
            }
        }

        //Brand
        private void Brand_ForeignKeyTBL(DataRow dataFromXML, DataTable dtxmlProduct, APP_Data.Brand FoundBrand)
        {
            string oldBrandId = dataFromXML["Id"].ToString();
            string BName = dataFromXML["Name"].ToString();
            if (dtxmlProduct != null)
            {
                foreach (DataRow o in dtxmlProduct.Select("BrandId = '" + oldBrandId + "' and BName ='" + BName.Replace("'", "''") + "'"))
                {
                    o["BrandId"] = FoundBrand.Id.ToString();
                }
            }
        }

        //City
        private void City_ForeignKeyTBL(DataRow dataRowFromXML, DataTable dtxmlCustomer, DataTable dtxmlShop, APP_Data.City FoundCity )
        {
            String oldCityId = dataRowFromXML["Id"].ToString();
            String CName = dataRowFromXML["CityName"].ToString();
            //update city in City Table
            if (dtxmlCustomer != null)
            {
                foreach (DataRow dr in dtxmlCustomer.Select("CityId='" + oldCityId + "' and CityName ='" + CName.Replace("'", "''") + "'"))
                {
                    dr["CityId"] = FoundCity.Id.ToString();
                }
            }
            if (dtxmlShop != null)
            {
                foreach (DataRow dr in dtxmlShop.Select("CityId='" + oldCityId + "' and CityName ='" + CName.Replace("'", "''") + "'"))
                {
                    dr["CityId"] = FoundCity.Id.ToString();
                }
            }
        }

        //Customer
        private void Customer_ForeignKeyTBL(DataRow dataRowFromXML, DataTable dtxmlTransaction, APP_Data.Customer FoundCustomer)
        {
            string oldCustomerId = dataRowFromXML["Id"].ToString();
            string CustomerCode = dataRowFromXML["CustomerCode"].ToString();

              if (dtxmlTransaction != null)
            {
                foreach (DataRow o in dtxmlTransaction.Select("CustomerCode ='" + CustomerCode + "'"))
                {
                    o["CustomerId"] = FoundCustomer.Id.ToString();
                    var c = FoundCustomer.Name;
                }
            }
        }

        //Counter
        private void Counter_ForeignKeyTBL(DataRow dataRowFromXML, DataTable dtxmlTransaction, DataTable dtxmlUserPrePaidDebt, APP_Data.Counter Foundcounter)
        {
            String OldCounterId = dataRowFromXML["Id"].ToString();
            String counterName = dataRowFromXML["Name"].ToString();
            if (dtxmlTransaction != null)
            {
                foreach (DataRow o in dtxmlTransaction.Select("CounterId='" + OldCounterId + "'and CounterName ='" + counterName.Replace("'", "''") + "'"))
                {
                    o["CounterId"] = Foundcounter.Id.ToString();
                }
            }
            if (dtxmlUserPrePaidDebt != null)
            {
                foreach (DataRow o in dtxmlUserPrePaidDebt.Select("CounterId='" + OldCounterId + "'and CounterName ='" + counterName.Replace("'", "''") + "'"))
                {
                    o["CounterId"] = Foundcounter.Id.ToString();
                }
            }
          
        }

        //Consignment Counter
        private void ConsignmentCounter_ForeignKeyTBL(DataRow dataRowFromXML, DataTable dtxmlProduct, DataTable dtxmlConsignmentSettlement,APP_Data.ConsignmentCounter FoundConsignmentCounter)
        {
             String OldConsignmentCounterId = dataRowFromXML["Id"].ToString();
                            //update Consigment Counter in City Table
             if (dtxmlProduct != null)
             {
                 foreach (DataRow o in dtxmlProduct.Select("ConsignmentCounterId='" + OldConsignmentCounterId + "'"))
                 {
                     o["ConsignmentCounterId"] = FoundConsignmentCounter.Id.ToString();
                 }
             }


             if (dtxmlConsignmentSettlement != null)
             {
                 foreach (DataRow o in dtxmlConsignmentSettlement.Select("ConsignorId='" + OldConsignmentCounterId + "'"))
                 {
                     o["ConsignorId"] = FoundConsignmentCounter.Id.ToString();
                 }
             }
    }

        //Unit
        private void Unit_ForeignKeyTBL(DataRow dataRowFromXMl, DataTable dtxmlProduct, APP_Data.Unit FoundUnit)
        {

            string OldUnitId = dataRowFromXMl["Id"].ToString();
            if (dtxmlProduct != null)
            {
                foreach (DataRow o in dtxmlProduct.Select("UnitId='" + OldUnitId + "'"))
                {
                    o["UnitId"] = FoundUnit.Id.ToString();
                }
            }
        }

        //Expense Category
        private void ExpenseCag_ForeignKeyTBL(DataRow dataRowFromXMl, DataTable dtxmlExpense, APP_Data.ExpenseCategory FoundExpenseCategory)
        {

            string OldExpCagId = dataRowFromXMl["Id"].ToString();
            if (dtxmlExpense != null)
            {
                foreach (DataRow o in dtxmlExpense.Select("ExpenseCategoryId='" + OldExpCagId + "'"))
                {
                    o["ExpenseCategoryId"] = FoundExpenseCategory.Id.ToString();
                }
            }
        }

        //Expense 
        private void Expense_ForeignKeyTBL(DataRow dataRowFromXMl, DataTable dtxmlExpenseDetail, APP_Data.Expense FoundExpense)
        {

            string OldExpId = dataRowFromXMl["Id"].ToString();
            if (dtxmlExpenseDetail != null)
            {
                foreach (DataRow o in dtxmlExpenseDetail.Select("ExpenseId='" + OldExpId + "'"))
                {
                    o["ExpenseId"] = FoundExpense.Id.ToString();
                }
            }
        }

        //Stock In Header 
        private void StockInHeader_ForeignKeyTBL(DataRow dataRowFromXMl, DataTable dtxmlStockInDetail, APP_Data.StockInHeader FoundStockInHeader)
        {

            string OldStockInHeaderId = dataRowFromXMl["Id"].ToString();
            if (dtxmlStockInDetail != null)
            {
                foreach (DataRow o in dtxmlStockInDetail.Select("StockInHeaderId='" + OldStockInHeaderId + "'"))
                {
                    o["StockInHeaderId"] = FoundStockInHeader.id.ToString();
                }
            }
        }

        //Product Category
        private void ProductCategory_ForeignKeyTBL(DataRow dataRowFromXML, DataTable dtxmlProduct, DataTable dtxmlProductSubCategory, APP_Data.ProductCategory FoundProductCategory)
        {
            String OldCategoryId = dataRowFromXML["Id"].ToString();

            if (dtxmlProduct != null)
            {
                foreach (DataRow o in dtxmlProduct.Select("ProductCategoryId='" + OldCategoryId + "'and CName ='" + Name.Replace("'", "''") + "'"))
                {
                    o["ProductCategoryId"] = FoundProductCategory.Id.ToString();
                }
            }
            if (dtxmlProductSubCategory != null)
            {
                foreach (DataRow o in dtxmlProductSubCategory.Select("ProductCategoryId='" + OldCategoryId + "'and CName ='" + Name.Replace("'", "''") + "'"))
                {
                    o["ProductCategoryId"] = FoundProductCategory.Id.ToString();
                }
            }
        }

        //Product Sub Category
        private void ProductSubCag_ForeignKeyTBL(DataRow dataRowFromXML, DataTable dtxmlProduct, APP_Data.ProductSubCategory FoundProductSubCategory)
        {
            int OldProductSubCategoryId = Convert.ToInt32(dataRowFromXML["Id"].ToString());
            if (dtxmlProduct != null)
            {
                foreach (DataRow o in dtxmlProduct.Select("ProductSubCategoryId='" + OldProductSubCategoryId + "'and SCName ='" + Name.Replace("'", "''") + "'"))
                {
                    o["ProductSubCategoryId"] = FoundProductSubCategory.Id.ToString();
                }
            }
        }

        //Tax
        private void Tax_ForeignKeyTBL(DataRow dataFromXML, DataTable dtxmlProduct, APP_Data.Tax FoundTax)
        {
            string OldTaxId = dataFromXML["Id"].ToString();
            if (dtxmlProduct != null)
            {
                foreach (DataRow o in dtxmlProduct.Select("TaxId='" + OldTaxId + "'and TaxName ='" + Name.Replace("'", "''") + "'"))
                {
                    o["TaxId"] = FoundTax.Id.ToString();
                }
            }
        }

        //product
        private void Product_ForeignKeyTBL(DataRow dataRowFromXML, DataTable dtxmlProductQuantityChange, DataTable dtxmlProductPriceChange,
             DataTable dtxmlSPDetail, DataTable dtxmlStockInDetail,
            DataTable dtxmlTransactionDetail, DataTable dtxmlUnitConversion, DataTable dtxmlWrapperItem, APP_Data.Product FoundProduct)
        {
            string oldProductId = dataRowFromXML["Id"].ToString();
            string ProductCode = dataRowFromXML["ProductCode"].ToString() ;
            ProductCode=ProductCode.Replace("'","\"");
           
            if (dtxmlTransactionDetail != null)
            {
                foreach (DataRow o in dtxmlTransactionDetail.Select("ProductCode ='" + ProductCode + "'"))
                {
                    o["ProductId"] = FoundProduct.Id.ToString();
                }
            }

            if (dtxmlWrapperItem != null && dtxmlWrapperItem.Columns.Contains("ParentProductId"))
            {
                foreach (DataRow o in dtxmlWrapperItem.Select("ParentProductCode ='" + ProductCode + "'"))
                {
                    o["ParentProductId"] = FoundProduct.Id.ToString();
                }
            }
            if (dtxmlWrapperItem != null && dtxmlWrapperItem.Columns.Contains("ChildProductId"))
            {
                foreach (DataRow o in dtxmlWrapperItem.Select("ChildProductCode ='" + ProductCode + "'"))
                {
                    o["ChildProductId"] = FoundProduct.Id.ToString();
                }
            }
            if (dtxmlProductPriceChange != null && dtxmlProductPriceChange.Columns.Contains("ProductId"))
            {
                foreach (DataRow o in dtxmlProductPriceChange.Select("ProductCode ='" + ProductCode + "'"))
                {
                    o["ProductId"] = FoundProduct.Id.ToString();
                }
            }

            if (dtxmlSPDetail != null && dtxmlSPDetail.Columns.Contains("ParentProductID"))
            {
                foreach (DataRow o in dtxmlSPDetail.Select("ParentProductCode ='" + ProductCode + "'"))
                {
                    o["ParentProductID"] = FoundProduct.Id.ToString();
                }
            }
            if (dtxmlSPDetail != null && dtxmlSPDetail.Columns.Contains("ChildProductID"))
            {
                foreach (DataRow o in dtxmlSPDetail.Select("ChildProductCode ='" + ProductCode + "'"))
                {
                    o["ChildProductID"] = FoundProduct.Id.ToString();
                }
            }

            if (dtxmlProductQuantityChange != null && dtxmlProductQuantityChange.Columns.Contains("ProductId"))
            {
                foreach (DataRow o in dtxmlStockInDetail.Select("ProductCode ='" + ProductCode + "'"))
                {
                    o["ProductId"] = FoundProduct.Id.ToString();
                }
            }

            if (dtxmlStockInDetail != null && dtxmlStockInDetail.Columns.Contains("ProductId"))
            {
                foreach (DataRow o in dtxmlStockInDetail.Select("ProductCode ='" + ProductCode + "'"))
                {
                    o["ProductId"] = FoundProduct.Id.ToString();
                }
            }

            if (dtxmlUnitConversion != null && dtxmlUnitConversion.Columns.Contains("ProductId"))
            {
                foreach (DataRow o in dtxmlUnitConversion.Select("ProductCode ='" + ProductCode + "'"))
                {
                    o["FromProductId"] = FoundProduct.Id.ToString();
                }
            }

            if (dtxmlUnitConversion != null && dtxmlUnitConversion.Columns.Contains("ProductId"))
            {
                foreach (DataRow o in dtxmlUnitConversion.Select("ProductCode ='" + ProductCode + "'"))
                {
                    o["ToProductId"] = FoundProduct.Id.ToString();
                }
            }
        }

        //User
        private void Shop_ForeignKeyTBL(DataRow dataRowFromXML, DataTable dtxmlTransaction, DataTable dtxmlUser, APP_Data.Shop FoundShop)
        {
            string OldShopId = dataRowFromXML["Id"].ToString();
            string ShopName = dataRowFromXML["ShopName"].ToString();
            //update in transaction DataTables
            if (dtxmlTransaction != null)
            {
                foreach (DataRow o in dtxmlTransaction.Select("ShopId='" + OldShopId + "' and ShopName ='" + ShopName.Replace("'", "''") + "'"))
                {
                    o["ShopId"] = FoundShop.Id.ToString();
                }
            }

            if (dtxmlUser != null)
            {
                foreach (DataRow o in dtxmlUser.Select("ShopId='" + OldShopId + "'"))
                {
                    o["ShopId"] = FoundShop.Id.ToString();
                }
            }
        }

        //Currency 
        private void Currency_ForeignKeyTBL(DataRow dataRowFromXML, DataTable dtxmlTransaction, DataTable dtxmlExchangeRateForTransaction,APP_Data.Currency FoundCurrency)
        {
            int oldCurrencyId = Convert.ToInt32(dataRowFromXML["Id"].ToString());
            if (dtxmlExchangeRateForTransaction != null)
            {
                foreach (DataRow o in dtxmlExchangeRateForTransaction.Select("CurrencyId='" + oldCurrencyId + "'"))
                {
                    o["CurrencyId"] = FoundCurrency.Id.ToString();
                }
            }
            if (dtxmlTransaction != null)
            {
                foreach (DataRow o in dtxmlTransaction.Select("ReceivedCurrencyId='" + oldCurrencyId + "'"))
                {
                    o["ReceivedCurrencyId"] = FoundCurrency.Id.ToString();
                }
            }
        }

        //MemberType 
        private void MemberType_ForeignKeyTBL(DataRow dataRowFromXML, DataTable dtxmlMemberCardRule, DataTable dtxmlTransaction, APP_Data.MemberType FoundMemberType)
        {
            int oldMemberTypeId = Convert.ToInt32(dataRowFromXML["Id"].ToString());
            if (dtxmlMemberCardRule != null)
            {
                foreach (DataRow o in dtxmlMemberCardRule.Select("Id='" + oldMemberTypeId + "'"))
                {
                    o["Id"] = FoundMemberType.Id.ToString();
                }
            }
            if (dtxmlTransaction != null)
            {
                foreach (DataRow o in dtxmlTransaction.Select("MemberTypeId='" + oldMemberTypeId + "'"))
                {
                    o["MemberTypeId"] = FoundMemberType.Id.ToString();
                }
            }
        }

        //PaymentType
        private void PaymentType_ForeignKeyTBL(DataRow dataRowFromXML, DataTable dtxmlTransaction, APP_Data.PaymentType FoundPaymentType)
        {
            int OldPaymentTypeId = Convert.ToInt32(dataRowFromXML["Id"].ToString());
            if (dtxmlTransaction != null)
            {
                foreach (DataRow o in dtxmlTransaction.Select("PaymentTypeId='" + OldPaymentTypeId + "'"))
                {
                    o["PaymentTypeId"] = FoundPaymentType.Id.ToString();
                }
            }
        }

        //GiftCard
        private void GiftCard_ForeignKeyTBL(DataRow dataRowFromXML, DataTable dtxmlTransaction, APP_Data.GiftCard FoundGiftCard)
        {
            int OldGiftCardId = Convert.ToInt32(dataRowFromXML["Id"].ToString());
            string GCardNumber = Convert.ToString(dataRowFromXML["CardNumber"].ToString());

                if (dtxmlTransaction != null && dtxmlTransaction.Columns.Contains("GiftCardId"))
            {
                foreach (DataRow o in dtxmlTransaction.Select("CardNumber ='" + GCardNumber.Replace("'", "''") + "'"))
                {
                    o["GiftCardId"] = FoundGiftCard.Id.ToString();
                }
            }

        }

        //Transaction
        private void Transaction_ForeignKeyTBL(DataRow dataRowFromXML, DataTable dtxmlTransactionDetail, DataTable dtxmlUserPrePaidDebt, DataTable dtxmlExchangeRateForTransaction, APP_Data.Transaction FoundTransaction)
        {
            string OldTransactionId = dataRowFromXML["Id"].ToString();
            if (dtxmlTransactionDetail != null && dtxmlTransactionDetail.Columns.Contains("TransactionId"))
            {
                foreach (DataRow o in dtxmlTransactionDetail.Select("TransactionId ='" + OldTransactionId + "'"))
                {
                    o["TransactionId"] = FoundTransaction.Id.ToString();
                }
            }

            if (dtxmlUserPrePaidDebt != null && dtxmlUserPrePaidDebt.Columns.Contains("TransactionId"))
            {
                foreach (DataRow o in dtxmlUserPrePaidDebt.Select("TransactionId ='" + OldTransactionId + "'"))
                {
                    o["TransactionId"] = FoundTransaction.Id.ToString();
                }
            }

            if (dtxmlUserPrePaidDebt != null && dtxmlUserPrePaidDebt.Columns.Contains("PrePaidDebtTransactionId"))
            {
                foreach (DataRow o in dtxmlUserPrePaidDebt.Select("PrePaidDebtTransactionId ='" + OldTransactionId + "'"))
                {
                    o["PrePaidDebtTransactionId"] = FoundTransaction.Id.ToString();
                }
            }


            if (dtxmlExchangeRateForTransaction != null && dtxmlExchangeRateForTransaction.Columns.Contains("TransactionId"))
            {
                foreach (DataRow o in dtxmlExchangeRateForTransaction.Select("TransactionId ='" + OldTransactionId + "'"))
                {
                    o["TransactionId"] = FoundTransaction.Id.ToString();
                }
            }

            if (dtxmlUserPrePaidDebt != null && dtxmlUserPrePaidDebt.Columns.Contains("TransactionId"))
            {
                foreach (DataRow o in dtxmlUserPrePaidDebt.Select("CreditTransactionId ='" + OldTransactionId + "'"))
                {
                    o["CreditTransactionId"] = FoundTransaction.Id.ToString();
                }
            }
          
        }

        //Transaction
        private void Tran_ForeignKeyTBL(DataRow dr,DataTable dtxmlTransactionDetail, DataTable dtxmlUserPrePaidDebt, DataTable dtxmlExchangeRateForTransaction, 
            Transaction transaction )
        {
            string OldTransactionIdInner = dr["Id"].ToString();
            if (dtxmlTransactionDetail != null && dtxmlTransactionDetail.Columns.Contains("TransactionId"))
            {
                foreach (DataRow o in dtxmlTransactionDetail.Select("TransactionId ='" + OldTransactionIdInner + "'"))
                {
                    o["TransactionId"] = transaction.Id.ToString();
                }
            }

            if (dtxmlUserPrePaidDebt != null && dtxmlUserPrePaidDebt.Columns.Contains("TransactionId"))
            {
                foreach (DataRow o in dtxmlUserPrePaidDebt.Select("TransactionId ='" + OldTransactionIdInner + "'"))
                {
                    o["TransactionId"] = transaction.Id.ToString();
                }
            }
            if (dtxmlExchangeRateForTransaction != null && dtxmlExchangeRateForTransaction.Columns.Contains("TransactionId"))
            {
                foreach (DataRow o in dtxmlExchangeRateForTransaction.Select("TransactionId ='" + OldTransactionIdInner + "'"))
                {
                    o["TransactionId"] = transaction.Id.ToString();
                }
            }

            if (dtxmlUserPrePaidDebt != null && dtxmlUserPrePaidDebt.Columns.Contains("TransactionId"))
            {
                foreach (DataRow o in dtxmlUserPrePaidDebt.Select("CreditTransactionId ='" + OldTransactionIdInner + "'"))
                {
                    o["CreditTransactionId"] = transaction.Id.ToString();
                }
            }
            if (dtxmlUserPrePaidDebt != null && dtxmlUserPrePaidDebt.Columns.Contains("PrePaidDebtTransactionId"))
            {
                foreach (DataRow o in dtxmlUserPrePaidDebt.Select("PrePaidDebtTransactionId ='" + OldTransactionIdInner + "'"))
                {
                    o["PrePaidDebtTransactionId"] = transaction.Id.ToString();
                }
            }
        }

        //TransactionDetail
        private void TransactionDetail_ForeignKeyTBL(DataRow dataRowFromXML, DataTable dtxmlUserPrePaidDebt, DataTable dtxmlSPDetail,DataTable dtxmlConsignmentSettlementDetail ,
            APP_Data.TransactionDetail FoundTransactionDetail)
        {
            string OldTransactionDetailId = dataRowFromXML["Id"].ToString();
            String TransactionId = dataRowFromXML["TransactionId"].ToString();
            string ConsignmentNo = dataRowFromXML["ConsignmentNo"].ToString();
       
            string ProductCode = dataRowFromXML["ProductCode"].ToString();

            if (dtxmlUserPrePaidDebt != null && dtxmlUserPrePaidDebt.Columns.Contains("TransactionDetailId"))
            {
                foreach (DataRow dr in dtxmlUserPrePaidDebt.Select("TransactionDetailId ='" + OldTransactionDetailId + "'"))
                {
                    dr["TransactionDetailId"] = FoundTransactionDetail.Id.ToString();
                }
            }
            if (dtxmlSPDetail != null && dtxmlSPDetail.Columns.Contains("TransactionDetailID"))
            {
                foreach (DataRow o in dtxmlSPDetail.Select("TransactionDetailID ='" + OldTransactionDetailId + "' and TID ='" + TransactionId + "' and ParentProductCode ='" + ProductCode + "'"))
                {
                    o["TransactionDetailID"] = FoundTransactionDetail.Id.ToString();
                }
            }

            if (dtxmlConsignmentSettlementDetail != null && dtxmlConsignmentSettlementDetail.Columns.Contains("ConsignmentNo"))
            {
               
                foreach (DataRow o in dtxmlConsignmentSettlementDetail.Select("TransactionDetailID ='" + OldTransactionDetailId + "' and ConsignmentNo ='" + ConsignmentNo +"'"))
                {
                    o["TransactionDetailID"] = FoundTransactionDetail.Id.ToString();
                }


             
            }
        }

        //GiftSystem
        private void GiftSystem_ForeignKeyTBL(DataRow dataRowFromXML, DataTable dtxmlGiftSystem, DataTable dtxmlUser, APP_Data.GiftSystem giftSystem) {
            string oldgifysystemid = dataRowFromXML["Id"].ToString();
            //update in GiftSystem DataTables
            if (dtxmlGiftSystem != null) {
                foreach (DataRow dr in dtxmlGiftSystem.Select("Id='" + oldgifysystemid + "'")) {
                    dr["Id"] = giftSystem.Id.ToString();
                    }
                }
            }

        #endregion


        #endregion
        }
}
