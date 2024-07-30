using POS.APP_Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace POS.mPOSUI.CustomerPurchaseInvoice
{
    public partial class CustomerPurchaseInvoice : Form
    {
        private POSEntities entity = new POSEntities();
        public int editCustomerID { get; set; }
        public CustomerPurchaseInvoice()
        {
            InitializeComponent();
        }
        #region page Load
        private void CustomerPurchaseInvoice_Load(object sender, EventArgs e)
        {
            BindMember();
            BindCustomer();
            dtDate.Value = DateTime.Now;
            dtToDate.Value = DateTime.Now;
            //data binding for package Use grid view
            BindPackageUseGridView(0, 0, null);
            //data binding for package Use History
            BindPackageUsedHistoryGridView(0, 0, null);
            BindPackageUsedHistoryDeleteLogGridView(0, 0, null);
            ChangegvpackageUsedHistoryCellColor();

        }
        #endregion

        #region grid view data bind
        private void BindPackageUsedHistoryGridView(int CustomerId, int MemberId, string PatientID)
        {
            if (CustomerId == 0 && MemberId == 0 && String.IsNullOrWhiteSpace(PatientID))
            {
                var data = (from puh in entity.PackageUsedHistories
                            where puh.IsDelete == false && (puh.UsedDate >= dtDate.Value.Date && puh.UsedDate <= dtToDate.Value.Date
                            || puh.PackagePurchasedInvoice.Customer.Id == editCustomerID)
                            select new
                            {
                                PackageUsedHistoryId = puh.PackageUsedHistoryId,
                                Transaction_ID = puh.PackagePurchasedInvoice.TransactionDetail.TransactionId,
                                Customer_Name = puh.Customer.Name,
                                Transaction_Date = puh.PackagePurchasedInvoice.InvoiceDate,
                                Used_Date = puh.UsedDate,
                                Product_Name = puh.PackagePurchasedInvoice.Product.Name,
                                Offset_Qty = puh.UseQty,
                                Doctor_Name = puh.CustomerIDAsDoctor,
                                Therapist_Name = puh.CustomerIDAsTherapist,
                                Nurse_Aid = puh.CustomerIDAsAssistantNurse,
                                Remark = puh.Remark,
                                Action = "Delete"
                            }).OrderByDescending(y => y.Used_Date).ToList();
                // }).ToList();
                // }).OrderByDescending(y => y.Transaction_Date).ToList();

                if (data.Count > 0)
                {

                    //data.OrderByDescending(y => y.Used_Date);
                    List<PackageUsedHistoryDataBind> packageUsedHistoryDataBindList = new List<PackageUsedHistoryDataBind>();
                    foreach (var item in data)
                    {
                        PackageUsedHistoryDataBind packageUsedHistoryDataBind = new PackageUsedHistoryDataBind();
                        packageUsedHistoryDataBind.Offset_Qty = item.Offset_Qty;
                        packageUsedHistoryDataBind.Transaction_ID = item.Transaction_ID;
                        packageUsedHistoryDataBind.UsedDate = item.Used_Date.ToString("dd-MM-yyyy");
                        packageUsedHistoryDataBind.Customer_Name = item.Customer_Name;
                        packageUsedHistoryDataBind.Doctor_Name = (from c in entity.Customers where c.Id == item.Doctor_Name select c.Name).SingleOrDefault();
                        packageUsedHistoryDataBind.Thrapist_Name = (from c in entity.Customers where c.Id == item.Therapist_Name select c.Name).SingleOrDefault();
                        packageUsedHistoryDataBind.Nurse_Aid = (from c in entity.Customers where c.Id == item.Nurse_Aid select c.Name).SingleOrDefault();
                        packageUsedHistoryDataBind.ProductName = item.Product_Name;
                        packageUsedHistoryDataBind.PackageUsedHistoryId = item.PackageUsedHistoryId;
                        packageUsedHistoryDataBind.TransactionDate = item.Transaction_Date.ToString("dd-MM-yyyy");
                        packageUsedHistoryDataBind.Remark = item.Remark;
                        packageUsedHistoryDataBind.Action = item.Action;
                        packageUsedHistoryDataBindList.Add(packageUsedHistoryDataBind);
                    }
                    ChangegvpackageUsedHistoryCellColor();
                    gvpackageUsedHistory.DataSource = packageUsedHistoryDataBindList;
                    gvpackageUsedHistory.Columns["PackageUsedHistoryId"].Visible = false;
                }
                else
                {
                    gvpackageUsedHistory.DataSource = null;
                }

                ChangegvpackageUsedHistoryCellColor();
            }
            else if (CustomerId > 0 && MemberId == 0 && String.IsNullOrWhiteSpace(PatientID))
            {
                var data = (from puh in entity.PackageUsedHistories
                            where puh.IsDelete == false && (puh.ActualOffsetBy == CustomerId)
                           // || puh.UsedDate >= dtDate.Value.Date && puh.UsedDate <= dtToDate.Value.Date)
                            select new
                            {
                                PackageUsedHistoryId = puh.PackageUsedHistoryId,
                                Transaction_ID = puh.PackagePurchasedInvoice.TransactionDetail.TransactionId,
                                Customer_Name = puh.Customer.Name,
                                Transaction_Date = puh.PackagePurchasedInvoice.InvoiceDate,
                                Used_Date = puh.UsedDate,
                                Product_Name = puh.PackagePurchasedInvoice.Product.Name,
                                Offset_Qty = puh.UseQty,
                                Doctor_Name = puh.CustomerIDAsDoctor,
                                Therapist_Name = puh.CustomerIDAsTherapist,
                                Nurse_Aid = puh.CustomerIDAsAssistantNurse,
                                Remark = puh.Remark,
                                Action = "Delete"
                            }).OrderByDescending(y => y.Used_Date).ToList();
                // }).ToList();
                // }).OrderByDescending(y => y.Transaction_Date).ToList();

                if (data.Count > 0)
                {
                    List<PackageUsedHistoryDataBind> packageUsedHistoryDataBindList = new List<PackageUsedHistoryDataBind>();
                    foreach (var item in data)
                    {
                        PackageUsedHistoryDataBind packageUsedHistoryDataBind = new PackageUsedHistoryDataBind();
                        packageUsedHistoryDataBind.Offset_Qty = item.Offset_Qty;
                        packageUsedHistoryDataBind.Transaction_ID = item.Transaction_ID;
                        packageUsedHistoryDataBind.UsedDate = item.Used_Date.ToString("dd-MM-yyyy");
                        packageUsedHistoryDataBind.Customer_Name = item.Customer_Name;
                        packageUsedHistoryDataBind.Doctor_Name = (from c in entity.Customers where c.Id == item.Doctor_Name select c.Name).SingleOrDefault();
                        packageUsedHistoryDataBind.Thrapist_Name = (from c in entity.Customers where c.Id == item.Therapist_Name select c.Name).SingleOrDefault();
                        packageUsedHistoryDataBind.Nurse_Aid = (from c in entity.Customers where c.Id == item.Nurse_Aid select c.Name).SingleOrDefault();
                        packageUsedHistoryDataBind.ProductName = item.Product_Name;
                        packageUsedHistoryDataBind.PackageUsedHistoryId = item.PackageUsedHistoryId;
                        packageUsedHistoryDataBind.TransactionDate = item.Transaction_Date.ToString("dd-MM-yyyy");
                        packageUsedHistoryDataBind.Remark = item.Remark;
                        packageUsedHistoryDataBind.Action = item.Action;
                        packageUsedHistoryDataBindList.Add(packageUsedHistoryDataBind);

                    }

                    gvpackageUsedHistory.DataSource = packageUsedHistoryDataBindList;
                    gvpackageUsedHistory.Columns["PackageUsedHistoryId"].Visible = false;

                }
                else
                {
                    gvpackageUsedHistory.DataSource = null;
                }


            }
            else if (MemberId > 0 && CustomerId == 0 && String.IsNullOrWhiteSpace(PatientID))
            {
                var data = (from puh in entity.PackageUsedHistories
                            where puh.IsDelete == false && (puh.Customer.MemberTypeID == MemberId)
                            //|| puh.UsedDate >= dtDate.Value.Date && puh.UsedDate <= dtToDate.Value.Date)
                            select new
                            {
                                PackageUsedHistoryId = puh.PackageUsedHistoryId,
                                Transaction_ID = puh.PackagePurchasedInvoice.TransactionDetail.TransactionId,
                                Customer_Name = puh.Customer.Name,
                                Transaction_Date = puh.PackagePurchasedInvoice.InvoiceDate,
                                Used_Date = puh.UsedDate,
                                Product_Name = puh.PackagePurchasedInvoice.Product.Name,
                                Offset_Qty = puh.UseQty,
                                Doctor_Name = puh.CustomerIDAsDoctor,
                                Therapist_Name = puh.CustomerIDAsTherapist,
                                Nurse_Aid = puh.CustomerIDAsAssistantNurse,
                                Remark = puh.Remark,
                                Action = "Delete"
                            }).OrderByDescending(y => y.Used_Date).ToList();

                //}).ToList();
                // }).OrderByDescending(y => y.Transaction_Date).ToList();

                if (data.Count > 0)
                {
                    List<PackageUsedHistoryDataBind> packageUsedHistoryDataBindList = new List<PackageUsedHistoryDataBind>();
                    foreach (var item in data)
                    {
                        PackageUsedHistoryDataBind packageUsedHistoryDataBind = new PackageUsedHistoryDataBind();
                        packageUsedHistoryDataBind.Offset_Qty = item.Offset_Qty;
                        packageUsedHistoryDataBind.Transaction_ID = item.Transaction_ID;
                        packageUsedHistoryDataBind.UsedDate = item.Used_Date.ToString("dd-MM-yyyy");
                        packageUsedHistoryDataBind.Customer_Name = item.Customer_Name;
                        packageUsedHistoryDataBind.Doctor_Name = (from c in entity.Customers where c.Id == item.Doctor_Name select c.Name).SingleOrDefault();
                        packageUsedHistoryDataBind.Thrapist_Name = (from c in entity.Customers where c.Id == item.Therapist_Name select c.Name).SingleOrDefault();
                        packageUsedHistoryDataBind.Nurse_Aid = (from c in entity.Customers where c.Id == item.Nurse_Aid select c.Name).SingleOrDefault();
                        packageUsedHistoryDataBind.ProductName = item.Product_Name;
                        packageUsedHistoryDataBind.PackageUsedHistoryId = item.PackageUsedHistoryId;
                        packageUsedHistoryDataBind.TransactionDate = item.Transaction_Date.ToString("dd-MM-yyyy");
                        packageUsedHistoryDataBind.Remark = item.Remark;
                        packageUsedHistoryDataBind.Action = item.Action;
                        packageUsedHistoryDataBindList.Add(packageUsedHistoryDataBind);
                    }


                    gvpackageUsedHistory.DataSource = packageUsedHistoryDataBindList;
                    gvpackageUsedHistory.Columns["PackageUsedHistoryId"].Visible = false;
                    ChangegvpackageUsedHistoryCellColor();
                }
                else
                {
                    gvpackageUsedHistory.DataSource = null;
                }


            }
            else if (MemberId > 0 && CustomerId > 0 && String.IsNullOrWhiteSpace(PatientID))
            {
                var data = (from puh in entity.PackageUsedHistories
                            where puh.IsDelete == false && (puh.Customer.MemberTypeID == MemberId && puh.ActualOffsetBy == CustomerId)
                           // || puh.UsedDate >= dtDate.Value.Date && puh.UsedDate <= dtToDate.Value.Date)
                            select new
                            {
                                PackageUsedHistoryId = puh.PackageUsedHistoryId,
                                Transaction_ID = puh.PackagePurchasedInvoice.TransactionDetail.TransactionId,
                                Customer_Name = puh.Customer.Name,
                                Transaction_Date = puh.PackagePurchasedInvoice.InvoiceDate,
                                Used_Date = puh.UsedDate,
                                Product_Name = puh.PackagePurchasedInvoice.Product.Name,
                                Offset_Qty = puh.UseQty,
                                Doctor_Name = puh.CustomerIDAsDoctor,
                                Therapist_Name = puh.CustomerIDAsTherapist,
                                Nurse_Aid = puh.CustomerIDAsAssistantNurse,
                                Remark = puh.Remark,
                                Action = "Delete"
                            }).OrderByDescending(y => y.Used_Date).ToList();
                // }).ToList();
                // }).OrderByDescending(y => y.Transaction_Date).ToList();


                if (data.Count > 0)
                {
                    List<PackageUsedHistoryDataBind> packageUsedHistoryDataBindList = new List<PackageUsedHistoryDataBind>();
                    foreach (var item in data)
                    {
                        PackageUsedHistoryDataBind packageUsedHistoryDataBind = new PackageUsedHistoryDataBind();
                        packageUsedHistoryDataBind.Offset_Qty = item.Offset_Qty;
                        packageUsedHistoryDataBind.Transaction_ID = item.Transaction_ID;
                        packageUsedHistoryDataBind.UsedDate = item.Used_Date.ToString("dd-MM-yyyy");
                        packageUsedHistoryDataBind.Customer_Name = item.Customer_Name;
                        packageUsedHistoryDataBind.Doctor_Name = (from c in entity.Customers where c.Id == item.Doctor_Name select c.Name).SingleOrDefault();
                        packageUsedHistoryDataBind.Thrapist_Name = (from c in entity.Customers where c.Id == item.Therapist_Name select c.Name).SingleOrDefault();
                        packageUsedHistoryDataBind.Nurse_Aid = (from c in entity.Customers where c.Id == item.Nurse_Aid select c.Name).SingleOrDefault();
                        packageUsedHistoryDataBind.ProductName = item.Product_Name;
                        packageUsedHistoryDataBind.PackageUsedHistoryId = item.PackageUsedHistoryId;
                        packageUsedHistoryDataBind.TransactionDate = item.Transaction_Date.ToString("dd-MM-yyyy");
                        packageUsedHistoryDataBind.Remark = item.Remark;
                        packageUsedHistoryDataBind.Action = item.Action;
                        packageUsedHistoryDataBindList.Add(packageUsedHistoryDataBind);
                    }
                    gvpackageUsedHistory.DataSource = packageUsedHistoryDataBindList;
                    gvpackageUsedHistory.Columns[0].Visible = false;
                    ChangegvpackageUsedHistoryCellColor();
                }
                else
                {
                    gvpackageUsedHistory.DataSource = null;
                }
                ChangegvpackageUsedHistoryCellColor();
            }
            else if (MemberId == 0 && CustomerId == 0 && !String.IsNullOrWhiteSpace(PatientID))
            {
                var data = (from puh in entity.PackageUsedHistories
                            where puh.IsDelete == false && (puh.Customer.CustomerCode == PatientID
                            )
                            select new
                            {
                                PackageUsedHistoryId = puh.PackageUsedHistoryId,
                                Transaction_ID = puh.PackagePurchasedInvoice.TransactionDetail.TransactionId,
                                Customer_Name = puh.Customer.Name,
                                Transaction_Date = puh.PackagePurchasedInvoice.InvoiceDate,
                                Used_Date = puh.UsedDate,
                                Product_Name = puh.PackagePurchasedInvoice.Product.Name,
                                Offset_Qty = puh.UseQty,
                                Doctor_Name = puh.CustomerIDAsDoctor,
                                Therapist_Name = puh.CustomerIDAsTherapist,
                                Nurse_Aid = puh.CustomerIDAsAssistantNurse,
                                Remark = puh.Remark,
                                Action = "Delete"
                            }).OrderByDescending(y => y.Used_Date).ToList();
                // }).ToList();
                // }).OrderByDescending(y => y.Transaction_Date).ToList();

                if (data.Count > 0)
                {
                    List<PackageUsedHistoryDataBind> packageUsedHistoryDataBindList = new List<PackageUsedHistoryDataBind>();
                    foreach (var item in data)
                    {
                        PackageUsedHistoryDataBind packageUsedHistoryDataBind = new PackageUsedHistoryDataBind();
                        packageUsedHistoryDataBind.Offset_Qty = item.Offset_Qty;
                        packageUsedHistoryDataBind.Transaction_ID = item.Transaction_ID;
                        packageUsedHistoryDataBind.UsedDate = item.Used_Date.ToString("dd-MM-yyyy");
                        packageUsedHistoryDataBind.Customer_Name = item.Customer_Name;
                        packageUsedHistoryDataBind.Doctor_Name = (from c in entity.Customers where c.Id == item.Doctor_Name select c.Name).SingleOrDefault();
                        packageUsedHistoryDataBind.Thrapist_Name = (from c in entity.Customers where c.Id == item.Therapist_Name select c.Name).SingleOrDefault();
                        packageUsedHistoryDataBind.Nurse_Aid = (from c in entity.Customers where c.Id == item.Nurse_Aid select c.Name).SingleOrDefault();
                        packageUsedHistoryDataBind.ProductName = item.Product_Name;
                        packageUsedHistoryDataBind.PackageUsedHistoryId = item.PackageUsedHistoryId;
                        packageUsedHistoryDataBind.TransactionDate = item.Transaction_Date.ToString("dd-MM-yyyy");
                        packageUsedHistoryDataBind.Remark = item.Remark;
                        packageUsedHistoryDataBind.Action = item.Action;
                        packageUsedHistoryDataBindList.Add(packageUsedHistoryDataBind);
                    }
                    gvpackageUsedHistory.DataSource = packageUsedHistoryDataBindList;
                    gvpackageUsedHistory.Columns[0].Visible = false;
                    ChangegvpackageUsedHistoryCellColor();
                }
                else
                {
                    gvpackageUsedHistory.DataSource = null;
                }
                ChangegvpackageUsedHistoryCellColor();
            }
        }
        //DeleteLog for customerPacakgeSale
        private void BindPackageUsedHistoryDeleteLogGridView(int CustomerId, int MemberId, string PatientID)
        {

            if (CustomerId == 0 && MemberId == 0 && String.IsNullOrWhiteSpace(PatientID))
            {
                var data = (from puh in entity.PackageUsedHistories
                            where puh.IsDelete == true && puh.UsedDate >= dtDate.Value.Date && puh.UsedDate <= dtToDate.Value.Date
                            select new
                            {
                                PackageUsedHistoryId = puh.PackageUsedHistoryId,
                                Transaction_ID = puh.PackagePurchasedInvoice.TransactionDetail.TransactionId,
                                Customer_Name = puh.Customer.Name,
                                Transaction_Date = puh.PackagePurchasedInvoice.InvoiceDate,
                                Used_Date = puh.UsedDate,
                                Product_Name = puh.PackagePurchasedInvoice.Product.Name,
                                Offset_Qty = puh.UseQty,
                            }).OrderByDescending(y => y.Transaction_Date).ToList();
                dgvDeleteLogList.DataSource = data;
            }
            else if (CustomerId > 0 && MemberId == 0 && String.IsNullOrWhiteSpace(PatientID))
            {
                var data = (from puh in entity.PackageUsedHistories
                            where puh.IsDelete == true && puh.ActualOffsetBy == CustomerId
                           // && puh.UsedDate >= dtDate.Value.Date && puh.UsedDate <= dtToDate.Value.Date
                            select new
                            {
                                PackageUsedHistoryId = puh.PackageUsedHistoryId,
                                Transaction_ID = puh.PackagePurchasedInvoice.TransactionDetail.TransactionId,
                                Customer_Name = puh.Customer.Name,
                                Transaction_Date = puh.PackagePurchasedInvoice.InvoiceDate,
                                Used_Date = puh.UsedDate,
                                Product_Name = puh.PackagePurchasedInvoice.Product.Name,
                                Offset_Qty = puh.UseQty,
                            }).OrderByDescending(y => y.Transaction_Date).ToList();
                dgvDeleteLogList.DataSource = data;
            }
            else if (MemberId > 0 && CustomerId == 0 && String.IsNullOrWhiteSpace(PatientID))
            {
                var data = (from puh in entity.PackageUsedHistories
                            where puh.IsDelete == true && puh.Customer.MemberTypeID == MemberId
                           // && puh.UsedDate >= dtDate.Value.Date && puh.UsedDate <= dtToDate.Value.Date
                            select new
                            {
                                PackageUsedHistoryId = puh.PackageUsedHistoryId,
                                Transaction_ID = puh.PackagePurchasedInvoice.TransactionDetail.TransactionId,
                                Customer_Name = puh.PackagePurchasedInvoice.Customer.Name,
                                Transaction_Date = puh.PackagePurchasedInvoice.InvoiceDate,
                                Used_Date = puh.UsedDate,
                                Produc_tName = puh.PackagePurchasedInvoice.Product.Name,
                                Offset_Qty = puh.UseQty,
                            }).OrderByDescending(y => y.Transaction_Date).ToList();
                dgvDeleteLogList.DataSource = data;
            }
            else if (MemberId > 0 && CustomerId > 0 && String.IsNullOrWhiteSpace(PatientID))
            {
                var data = (from puh in entity.PackageUsedHistories
                            where puh.IsDelete == true && puh.Customer.MemberTypeID == MemberId && puh.ActualOffsetBy == CustomerId
                           // || puh.UsedDate >= dtDate.Value.Date && puh.UsedDate <= dtToDate.Value.Date
                            select new
                            {
                                PackageUsedHistoryId = puh.PackageUsedHistoryId,
                                Transaction_ID = puh.PackagePurchasedInvoice.TransactionDetail.TransactionId,
                                Customer_Name = puh.PackagePurchasedInvoice.Customer.Name,
                                Transaction_Date = puh.PackagePurchasedInvoice.InvoiceDate,
                                Used_Date = puh.UsedDate,
                                Product_Name = puh.PackagePurchasedInvoice.Product.Name,
                                Offset_Qty = puh.UseQty,
                            }).OrderByDescending(y => y.Transaction_Date).ToList();
                dgvDeleteLogList.DataSource = data;
            }
            else if (MemberId == 0 && CustomerId == 0 && !String.IsNullOrWhiteSpace(PatientID))
            {
                var data = (from puh in entity.PackageUsedHistories
                            where puh.IsDelete == true && puh.Customer.CustomerCode == PatientID

                            select new
                            {
                                PackageUsedHistoryId = puh.PackageUsedHistoryId,
                                Transaction_ID = puh.PackagePurchasedInvoice.TransactionDetail.TransactionId,
                                Customer_Name = puh.PackagePurchasedInvoice.Customer.Name,
                                Transaction_Date = puh.PackagePurchasedInvoice.InvoiceDate,
                                Used_Date = puh.UsedDate,
                                Product_Name = puh.PackagePurchasedInvoice.Product.Name,
                                Offset_Qty = puh.UseQty,
                            }).OrderByDescending(y => y.Transaction_Date).ToList();
                dgvDeleteLogList.DataSource = data;
            }
            dgvDeleteLogList.Columns["PackageUsedHistoryId"].Visible = false;
            dgvDeleteLogList.Columns["Transaction_Date"].DefaultCellStyle.Format = "dd-MM-yyyy";
            dgvDeleteLogList.Columns["Used_Date"].DefaultCellStyle.Format = "dd-MM-yyyy";
        }

        private void ChangegvpackageUsedHistoryCellColor()
        {
            int rowscount = gvpackageUsedHistory.Rows.Count;
            for (int i = 0; i < rowscount; i++)
            {
                // gvpackageUsedHistory.Rows[i].Cells[6].Style.BackColor = Color.Green;
                //gvpackageUsedHistory.Rows[i].Cells[6].Style.ForeColor = Color.Red;
                this.gvpackageUsedHistory.Rows[i].Cells[11].Style.ForeColor = Color.Blue;

                gvpackageUsedHistory.Rows[i].Cells[11].Style.Font = new Font(gvpackageUsedHistory.DefaultCellStyle.Font, FontStyle.Underline);
            }
        }

        private void BindPackageUseGridView(int CustomerId, int MemberId, string PatientID)
        {
            if (CustomerId == 0 && MemberId == 0 && String.IsNullOrWhiteSpace(PatientID))
            {
                var data = (from ppi in entity.PackagePurchasedInvoices
                            where ppi.Product.IsPackage == true
                            && (ppi.InvoiceDate >= dtDate.Value.Date && ppi.InvoiceDate <= dtToDate.Value.Date || ppi.Customer.Id == editCustomerID)
                            && ppi.TransactionDetail.IsDeleted == false && ppi.IsCancelled == false
                            && (ppi.packageFrequency * ppi.TransactionDetail.Qty) - ppi.UseQty != 0
                            select new
                            {
                                packagePurchasedInvoiceId = ppi.PackagePurchasedInvoiceId,
                                TransactionId = ppi.TransactionDetail.Transaction.Id,
                                Transaction_Date = ppi.InvoiceDate,
                                Customer_Name = ppi.Customer.Name,
                                Product_Name = ppi.Product.Name,
                                Procedure_Qty = ppi.packageFrequency * ppi.TransactionDetail.Qty,
                                Offset_Qty = ppi.UseQty,
                                Available_Qty = (ppi.packageFrequency * ppi.TransactionDetail.Qty) - ppi.UseQty,
                                Action = "Offset",
                                Offset_History = "View",
                                TransactionDetailId = ppi.TransactionDetailId,
                                CustomerId = ppi.Customer.Id,
                                AddSharer = "Add Sharer"
                            }).OrderByDescending(y => y.Transaction_Date).ToList();
                CD.DataSource = data;
            }
            else if (CustomerId > 0 && MemberId == 0)
            {
                var data = (from ppi in entity.PackagePurchasedInvoices
                            where ppi.IsDelete == false && ppi.Product.IsPackage == true
                            && (ppi.CustomerId == CustomerId || ppi.PurchasedPackageSharers.Select(x => x.SharedCustomerId).Contains(CustomerId))
                            //&& ppi.InvoiceDate >= dtDate.Value.Date && ppi.InvoiceDate <= dtToDate.Value.Date)
                            && ppi.IsCancelled == false
                            && ppi.TransactionDetail.IsDeleted == false && (ppi.packageFrequency * ppi.TransactionDetail.Qty) - ppi.UseQty != 0
                            select new
                            {
                                packagePurchasedInvoiceId = ppi.PackagePurchasedInvoiceId,
                                TransactionId = ppi.TransactionDetail.Transaction.Id,
                                Transaction_Date = ppi.InvoiceDate,
                                Customer_Name = ppi.Customer.Name,
                                Product_Name = ppi.Product.Name,
                                Procedure_Qty = ppi.packageFrequency * ppi.TransactionDetail.Qty,
                                Offset_Qty = ppi.UseQty,
                                Available_Qty = (ppi.packageFrequency * ppi.TransactionDetail.Qty) - ppi.UseQty,
                                Action = "Offset",
                                Offset_History = "View",
                                TransactionDetailId = ppi.TransactionDetailId,
                                CustomerId = ppi.Customer.Id,
                                AddSharer = "Add Sharer"
                            }).OrderByDescending(y => y.Transaction_Date).ToList();

                //var data = (from ppi in entity.PurchasedPackageSharers
                //            where ppi.IsDeleted == false && ppi.PackagePurchasedInvoice.Product.IsPackage == true
                //            && ((ppi.SharedCustomerId == CustomerId || ppi.PackageOwnerId == CustomerId) && (ppi.PackagePurchasedInvoice.InvoiceDate >= dtDate.Value.Date && ppi.PackagePurchasedInvoice.InvoiceDate <= dtToDate.Value.Date))
                //            && ppi.PackagePurchasedInvoice.IsCancelled == false && ppi.PackagePurchasedInvoice.TransactionDetail.IsDeleted == false && (ppi.PackagePurchasedInvoice.packageFrequency * ppi.PackagePurchasedInvoice.TransactionDetail.Qty) - ppi.PackagePurchasedInvoice.UseQty != 0
                //            select new
                //            {
                //                packagePurchasedInvoiceId = ppi.PackageInvoiceId,
                //                TransactionId = ppi.PackagePurchasedInvoice.TransactionDetail.Transaction.Id,
                //                Transaction_Date = ppi.PackagePurchasedInvoice.InvoiceDate,
                //                Customer_Name = ppi.Customer.Name,
                //                Product_Name = ppi.PackagePurchasedInvoice.Product.Name,
                //                Procedure_Qty = ppi.PackagePurchasedInvoice.packageFrequency * ppi.PackagePurchasedInvoice.TransactionDetail.Qty,
                //                Offset_Qty = ppi.PackagePurchasedInvoice.UseQty,
                //                Available_Qty = (ppi.PackagePurchasedInvoice.packageFrequency * ppi.PackagePurchasedInvoice.TransactionDetail.Qty) - ppi.PackagePurchasedInvoice.UseQty,
                //                Action = "Offset",
                //                Offset_History = "View",
                //                TransactionDetailId = ppi.PackagePurchasedInvoice.TransactionDetailId,
                //                CustomerId = ppi.Customer.Id
                //            }).OrderByDescending(y => y.Transaction_Date).ToList();
                CD.DataSource = data;
            }
            else if (MemberId > 0 && CustomerId == 0 && String.IsNullOrWhiteSpace(PatientID))
            {
                var data = (from ppi in entity.PackagePurchasedInvoices
                            where ppi.IsDelete == false && ppi.Product.IsPackage == true
                           && (ppi.CustomerId == CustomerId || ppi.PurchasedPackageSharers.Select(x => x.Customer1.MemberTypeID).Contains(MemberId))
                           //&& ppi.InvoiceDate >= dtDate.Value.Date && ppi.InvoiceDate <= dtToDate.Value.Date
                           && ppi.IsCancelled == false
                           && ppi.TransactionDetail.IsDeleted == false && (ppi.packageFrequency * ppi.TransactionDetail.Qty) - ppi.UseQty != 0
                            select new
                            {
                                packagePurchasedInvoiceId = ppi.PackagePurchasedInvoiceId,
                                TransactionId = ppi.TransactionDetail.Transaction.Id,
                                Transaction_Date = ppi.InvoiceDate,
                                Customer_Name = ppi.Customer.Name,
                                Product_Name = ppi.Product.Name,
                                Procedure_Qty = ppi.packageFrequency * ppi.TransactionDetail.Qty,
                                Offset_Qty = ppi.UseQty,
                                Available_Qty = (ppi.packageFrequency * ppi.TransactionDetail.Qty) - ppi.UseQty,
                                Action = "Offset",
                                Offset_History = "View",
                                TransactionDetailId = ppi.TransactionDetailId,
                                CustomerId = ppi.Customer.Id,
                                AddSharer = "Add Sharer"

                            }).OrderByDescending(y => y.Transaction_Date).ToList();
                //var data = (from ppi in entity.PurchasedPackageSharers
                //            where ppi.IsDeleted == false && ppi.PackagePurchasedInvoice.Product.IsPackage == true
                //            && ((ppi.Customer.MemberTypeID == MemberId || ppi.Customer1.MemberTypeID == MemberId )&& (ppi.PackagePurchasedInvoice.InvoiceDate >= dtDate.Value.Date && ppi.PackagePurchasedInvoice.InvoiceDate <= dtToDate.Value.Date))
                //            && ppi.PackagePurchasedInvoice.IsCancelled == false && ppi.PackagePurchasedInvoice.TransactionDetail.IsDeleted == false && (ppi.PackagePurchasedInvoice.packageFrequency * ppi.PackagePurchasedInvoice.TransactionDetail.Qty) - ppi.PackagePurchasedInvoice.UseQty != 0
                //            select new
                //            {
                //                packagePurchasedInvoiceId = ppi.PackageInvoiceId,
                //                TransactionId = ppi.PackagePurchasedInvoice.TransactionDetail.Transaction.Id,
                //                Transaction_Date = ppi.PackagePurchasedInvoice.InvoiceDate,
                //                Customer_Name = ppi.Customer.Name,
                //                Product_Name = ppi.PackagePurchasedInvoice.Product.Name,
                //                Procedure_Qty = ppi.PackagePurchasedInvoice.packageFrequency * ppi.PackagePurchasedInvoice.TransactionDetail.Qty,
                //                Offset_Qty = ppi.PackagePurchasedInvoice.UseQty,
                //                Available_Qty = (ppi.PackagePurchasedInvoice.packageFrequency * ppi.PackagePurchasedInvoice.TransactionDetail.Qty) - ppi.PackagePurchasedInvoice.UseQty,
                //                Action = "Offset",
                //                Offset_History = "View",
                //                TransactionDetailId = ppi.PackagePurchasedInvoice.TransactionDetailId,
                //                CustomerId = ppi.Customer.Id
                //            }).OrderByDescending(y => y.Transaction_Date).ToList();
                CD.DataSource = data;
            }
            else if (MemberId > 0 || CustomerId > 0 && String.IsNullOrWhiteSpace(PatientID))
            {
                var data = (from ppi in entity.PackagePurchasedInvoices
                            where ppi.IsDelete == false && ppi.Product.IsPackage == true
                            && ((ppi.CustomerId == CustomerId || ppi.PurchasedPackageSharers.Select(x => x.SharedCustomerId).Contains(CustomerId))
                            || (ppi.CustomerId == CustomerId || ppi.PurchasedPackageSharers.Select(x => x.Customer1.MemberTypeID).Contains(MemberId)))
                            //&& ppi.InvoiceDate >= dtDate.Value.Date && ppi.InvoiceDate <= dtToDate.Value.Date)
                             && ppi.IsCancelled == false
                            && ppi.TransactionDetail.IsDeleted == false && (ppi.packageFrequency * ppi.TransactionDetail.Qty) - ppi.UseQty != 0
                            select new
                            {
                                packagePurchasedInvoiceId = ppi.PackagePurchasedInvoiceId,
                                TransactionId = ppi.TransactionDetail.Transaction.Id,
                                Transaction_Date = ppi.InvoiceDate,
                                Customer_Name = ppi.Customer.Name,
                                Product_Name = ppi.Product.Name,
                                Procedure_Qty = ppi.packageFrequency * ppi.TransactionDetail.Qty,
                                Offset_Qty = ppi.UseQty,
                                Available_Qty = (ppi.packageFrequency * ppi.TransactionDetail.Qty) - ppi.UseQty,
                                Action = "Offset",
                                Offset_History = "View",
                                TransactionDetailId = ppi.TransactionDetailId,
                                CustomerId = ppi.Customer.Id,
                                AddSharer = "Add Sharer",

                            }).OrderByDescending(y => y.Transaction_Date).ToList();
                //var data = (from ppi in entity.PurchasedPackageSharers
                //            where ppi.IsDeleted == false && ppi.PackagePurchasedInvoice.Product.IsPackage == true
                //            && ((ppi.Customer.MemberTypeID == MemberId || ppi.Customer1.MemberTypeID == MemberId || ppi.SharedCustomerId == CustomerId || ppi.PackageOwnerId == CustomerId) && (ppi.PackagePurchasedInvoice.InvoiceDate >= dtDate.Value.Date && ppi.PackagePurchasedInvoice.InvoiceDate <= dtToDate.Value.Date))
                //            && ppi.PackagePurchasedInvoice.IsCancelled == false && ppi.PackagePurchasedInvoice.TransactionDetail.IsDeleted == false && (ppi.PackagePurchasedInvoice.packageFrequency * ppi.PackagePurchasedInvoice.TransactionDetail.Qty) - ppi.PackagePurchasedInvoice.UseQty != 0
                //            select new
                //            {
                //                packagePurchasedInvoiceId = ppi.PackageInvoiceId,
                //                TransactionId = ppi.PackagePurchasedInvoice.TransactionDetail.Transaction.Id,
                //                Transaction_Date = ppi.PackagePurchasedInvoice.InvoiceDate,
                //                Customer_Name = ppi.Customer.Name,
                //                Product_Name = ppi.PackagePurchasedInvoice.Product.Name,
                //                Procedure_Qty = ppi.PackagePurchasedInvoice.packageFrequency * ppi.PackagePurchasedInvoice.TransactionDetail.Qty,
                //                Offset_Qty = ppi.PackagePurchasedInvoice.UseQty,
                //                Available_Qty = (ppi.PackagePurchasedInvoice.packageFrequency * ppi.PackagePurchasedInvoice.TransactionDetail.Qty) - ppi.PackagePurchasedInvoice.UseQty,
                //                Action = "Offset",
                //                Offset_History = "View",
                //                TransactionDetailId = ppi.PackagePurchasedInvoice.TransactionDetailId,
                //                CustomerId = ppi.Customer.Id
                //            }).OrderByDescending(y => y.Transaction_Date).ToList();
                CD.DataSource = data;
            }
            else if (MemberId == 0 && CustomerId == 0 && !String.IsNullOrWhiteSpace(PatientID))
            {
                var data = (from ppi in entity.PackagePurchasedInvoices
                            where ppi.IsDelete == false && ppi.Product.IsPackage == true
                            && (ppi.Customer.CustomerCode == PatientID || ppi.PurchasedPackageSharers.Select(x => x.Customer1.CustomerCode).Contains(PatientID))
                            && ppi.TransactionDetail.IsDeleted == false && (ppi.packageFrequency * ppi.TransactionDetail.Qty) - ppi.UseQty != 0
                             && ppi.IsCancelled == false
                            select new
                            {
                                packagePurchasedInvoiceId = ppi.PackagePurchasedInvoiceId,
                                TransactionId = ppi.TransactionDetail.Transaction.Id,
                                Transaction_Date = ppi.InvoiceDate,
                                Customer_Name = ppi.Customer.Name,
                                Product_Name = ppi.Product.Name,
                                Procedure_Qty = ppi.packageFrequency * ppi.TransactionDetail.Qty,
                                Offset_Qty = ppi.UseQty,
                                Available_Qty = (ppi.packageFrequency * ppi.TransactionDetail.Qty) - ppi.UseQty,
                                Action = "Offset",
                                Offset_History = "View",
                                TransactionDetailId = ppi.TransactionDetailId,
                                CustomerId = ppi.Customer.Id,
                                AddSharer = "Add Sharer"
                            }).OrderByDescending(y => y.Transaction_Date).ToList();
                //var data = (from ppi in entity.PurchasedPackageSharers
                //            where ppi.IsDeleted == false && ppi.PackagePurchasedInvoice.Product.IsPackage == true
                //            && ((ppi.Customer.CustomerCode == PatientID || ppi.Customer1.CustomerCode == PatientID) && (ppi.PackagePurchasedInvoice.InvoiceDate >= dtDate.Value.Date && ppi.PackagePurchasedInvoice.InvoiceDate <= dtToDate.Value.Date))
                //            && ppi.PackagePurchasedInvoice.IsCancelled == false && ppi.PackagePurchasedInvoice.TransactionDetail.IsDeleted == false && (ppi.PackagePurchasedInvoice.packageFrequency * ppi.PackagePurchasedInvoice.TransactionDetail.Qty) - ppi.PackagePurchasedInvoice.UseQty != 0
                //            select new
                //            {
                //                packagePurchasedInvoiceId = ppi.PackageInvoiceId,
                //                TransactionId = ppi.PackagePurchasedInvoice.TransactionDetail.Transaction.Id,
                //                Transaction_Date = ppi.PackagePurchasedInvoice.InvoiceDate,
                //                Customer_Name = ppi.Customer.Name,
                //                Product_Name = ppi.PackagePurchasedInvoice.Product.Name,
                //                Procedure_Qty = ppi.PackagePurchasedInvoice.packageFrequency * ppi.PackagePurchasedInvoice.TransactionDetail.Qty,
                //                Offset_Qty = ppi.PackagePurchasedInvoice.UseQty,
                //                Available_Qty = (ppi.PackagePurchasedInvoice.packageFrequency * ppi.PackagePurchasedInvoice.TransactionDetail.Qty) - ppi.PackagePurchasedInvoice.UseQty,
                //                Action = "Offset",
                //                Offset_History = "View",
                //                TransactionDetailId = ppi.PackagePurchasedInvoice.TransactionDetailId,
                //                CustomerId = ppi.Customer.Id
                //            }).OrderByDescending(y => y.Transaction_Date).ToList();
                CD.DataSource = data;
            }
            CD.Columns["packagePurchasedInvoiceId"].Visible = false;
            CD.Columns["TransactionDetailId"].Visible = false;
            CD.Columns["CustomerId"].Visible = false;
            CD.Columns["Transaction_Date"].DefaultCellStyle.Format = "dd-MM-yyyy";
            CD.Columns["Action"].FillWeight = 70;
            CD.Columns["Offset_History"].FillWeight = 70;
            ChangegvPurchaseInoivceCellColor();
        }

        #endregion

        private void ChangegvPurchaseInoivceCellColor()
        {
            int rowscount = CD.Rows.Count;
            for (int i = 0; i < rowscount; i++)
            {
                //gvPurchaseInoivce.Rows[i].Cells[7].Style.BackColor = Color.Green;
                //gvPurchaseInoivce.Rows[i].Cells[7].Style.ForeColor =Color.Red;
                this.CD.Rows[i].Cells[8].Style.ForeColor = Color.Blue;
                this.CD.Rows[i].Cells[9].Style.ForeColor = Color.Blue;
                CD.Rows[i].Cells[8].Style.Font = new Font(CD.DefaultCellStyle.Font, FontStyle.Underline);
                CD.Rows[i].Cells[9].Style.Font = new Font(CD.DefaultCellStyle.Font, FontStyle.Underline);

                this.CD.Rows[i].Cells[12].Style.ForeColor = Color.Blue;
                CD.Rows[i].Cells[12].Style.Font = new Font(CD.DefaultCellStyle.Font, FontStyle.Underline);
            }
        }

        #region bind customer && Member id
        private void BindCustomer()
        {
            //Add Customer List with default option
            List<APP_Data.Customer> customerList = new List<APP_Data.Customer>();
            APP_Data.Customer customer = new APP_Data.Customer();
            customer.Id = 0;
            customer.Name = "All";
            customerList.Add(customer);
            customerList.AddRange(from c in entity.Customers where c.CustomerTypeId == 1 orderby c.Name select c);
            cboCustomerId.DataSource = customerList;
            cboCustomerId.DisplayMember = "Name";
            cboCustomerId.ValueMember = "Id";
            cboCustomerId.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboCustomerId.AutoCompleteSource = AutoCompleteSource.ListItems;
        }
        private void BindMember()
        {
            List<APP_Data.MemberType> mTypeList = new List<APP_Data.MemberType>();
            APP_Data.MemberType mType = new APP_Data.MemberType();
            mType.Id = 0;
            mType.Name = "All";
            mTypeList.Add(mType);
            mTypeList.AddRange(entity.MemberTypes.ToList());
            cbomemberId.DataSource = mTypeList;
            cbomemberId.DisplayMember = "Name";
            cbomemberId.ValueMember = "Id";
            cbomemberId.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbomemberId.AutoCompleteSource = AutoCompleteSource.ListItems;
        }


        #endregion

        private void gvPurchaseInoivce_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;// get the Row Index
            if (index != -1)
            {
                DataGridViewRow selectedRow = this.CD.Rows[index];
                if (e.ColumnIndex == 8)
                {//click the use package cell 
                    POSEntities entities = new POSEntities();
                    var procedureName = selectedRow.Cells[4].Value.ToString();
                    var procedureList = entities.SetProcedureProducts.Where(x => x.ProcedureProductName.Trim() == procedureName).ToList();
                    if (procedureList.Count > 0)
                    {
                        foreach (var item in procedureList)
                        {
                            var productQty = entities.Products.Where(x => x.Id == item.ProductID).Select(x => x.Qty).FirstOrDefault();
                            if ((productQty - item.ProductQty) < 0)
                            {
                                MessageBox.Show(item.ProductName + "  Item Is Not Enough For Offset", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                    }

                    UsePackageByCustomer UsePackageByCustomer = new UsePackageByCustomer();
                    UsePackageByCustomer.packagePurchasedInvoiceId = selectedRow.Cells[0].Value.ToString();//get the  packagePurchasedInvoiceId
                    UsePackageByCustomer.productname = selectedRow.Cells[4].Value.ToString();
                    UsePackageByCustomer.OwnerId = int.Parse(selectedRow.Cells[11].Value.ToString());
                    UsePackageByCustomer.packageOwner = selectedRow.Cells[3].Value.ToString();
                    if (string.IsNullOrEmpty(selectedRow.Cells[5].Value.ToString())) UsePackageByCustomer.Package_Qty = 0;
                    else UsePackageByCustomer.Package_Qty = Convert.ToInt32(selectedRow.Cells[5].Value.ToString());
                    UsePackageByCustomer.UsedQty = Convert.ToInt32(selectedRow.Cells[6].Value.ToString());
                    UsePackageByCustomer.Show();
                    this.Close();
                }
                if (e.ColumnIndex == 9)
                {
                    List<PackageUsedHistoryDataBind> PackageUsedHistoryList = new List<PackageUsedHistoryDataBind>();
                    long transactiondDetailId = Convert.ToInt64(selectedRow.Cells[10].Value);//get the  transactiondDetailId
                    var PackagePurchaseInvoiceData = (entity.PackagePurchasedInvoices.Where(x => x.TransactionDetailId == transactiondDetailId && x.IsDelete == false)).ToList();
                    foreach (var i in PackagePurchaseInvoiceData)
                    {
                        var data = (from puh in entity.PackageUsedHistories
                                    where puh.IsDelete == false && puh.PackagePurchasedInvoiceId == i.PackagePurchasedInvoiceId
                                    select new
                                    {
                                        PackageUsedHistoryId = puh.PackageUsedHistoryId,
                                        Transaction_ID = puh.PackagePurchasedInvoice.TransactionDetail.TransactionId,
                                        Customer_Name = puh.Customer.Name,
                                        TransactionDate = puh.PackagePurchasedInvoice.InvoiceDate,
                                        UsedDate = puh.UsedDate,
                                        ProductName = puh.PackagePurchasedInvoice.Product.Name,
                                        Offset_Qty = puh.UseQty,
                                        Doctor_Name = puh.CustomerIDAsDoctor,
                                        Therapist_Name = puh.CustomerIDAsTherapist,
                                        Nurse_Name = puh.CustomerIDAsAssistantNurse,
                                        Action = "Delete"
                                    }).OrderByDescending(y => y.TransactionDate)
                                                .ToList();
                        foreach (var d in data)
                        {
                            PackageUsedHistoryDataBind packageUsedHistoryDataBind = new PackageUsedHistoryDataBind();
                            packageUsedHistoryDataBind.PackageUsedHistoryId = d.PackageUsedHistoryId;
                            packageUsedHistoryDataBind.Transaction_ID = d.Transaction_ID;
                            packageUsedHistoryDataBind.Customer_Name = d.Customer_Name;
                            packageUsedHistoryDataBind.ProductName = d.ProductName;
                            packageUsedHistoryDataBind.Offset_Qty = d.Offset_Qty;
                            //packageUsedHistoryDataBind.Offset_Qty = d.Offset_Qty;
                            packageUsedHistoryDataBind.Doctor_Name = (from c in entity.Customers where c.Id == d.Doctor_Name select c.Name).SingleOrDefault();
                            packageUsedHistoryDataBind.Thrapist_Name = (from c in entity.Customers where c.Id == d.Therapist_Name select c.Name).SingleOrDefault();
                            packageUsedHistoryDataBind.Nurse_Aid = (from c in entity.Customers where c.Id == d.Nurse_Name select c.Name).SingleOrDefault();
                            packageUsedHistoryDataBind.UsedDate = d.UsedDate.ToString("dd-MM-yyyy");
                            packageUsedHistoryDataBind.TransactionDate = d.TransactionDate.ToString("dd-MM-yyyy");
                            packageUsedHistoryDataBind.Action = d.Action;
                            PackageUsedHistoryList.Add(packageUsedHistoryDataBind);
                        }//adding the data to the list
                    }//end of package invoice detail.
                    gvpackageUsedHistory.DataSource = PackageUsedHistoryList;
                    gvpackageUsedHistory.Columns["PackageUsedHistoryId"].Visible = false;
                    ChangegvpackageUsedHistoryCellColor();
                    usepackagetab.SelectTab("tabhistory");//go to Use package History Tab automically
                }
                if (e.ColumnIndex == 12)
                {
                    string packageId = selectedRow.Cells[0].Value.ToString();//get the  transactiondDetailId
                    var PackagePurchaseInvoiceData = entity.PackagePurchasedInvoices.Where(x => x.PackagePurchasedInvoiceId == packageId).FirstOrDefault();

                    PurchasedInvoiceSharers frmPurchasedInvoiceSharers = new PurchasedInvoiceSharers();
                    frmPurchasedInvoiceSharers.PurchasedInvoiceId = packageId;
                    frmPurchasedInvoiceSharers.PackageName = PackagePurchaseInvoiceData.TransactionDetail.Product.Name;
                    frmPurchasedInvoiceSharers.OwnerName = PackagePurchaseInvoiceData.Customer.Name;
                    frmPurchasedInvoiceSharers.OwnerId = PackagePurchaseInvoiceData.CustomerId;
                    frmPurchasedInvoiceSharers.ShowDialog();
                }
            }
        }
        class PackageUsedHistoryDataBind
        {
            public string PackageUsedHistoryId { get; set; }
            public string Transaction_ID { get; set; }
            public string Customer_Name { get; set; }
            public string TransactionDate { get; set; }
            public string UsedDate { get; set; }
            public string ProductName { get; set; }
            public int Offset_Qty { get; set; }
            public string Doctor_Name { get; set; }
            public string Thrapist_Name { get; set; }
            public string Nurse_Aid { get; set; }
            public string Remark { get; set; }
            public string Action { get; set; }
        }

        private void gvpackageUsedHistory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 11)
            {//Delete link button
                DialogResult result = MessageBox.Show("Are you sure you to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result.Equals(DialogResult.OK))
                {
                    int index = e.RowIndex;// get the Row Index
                    DataGridViewRow selectedRow = this.gvpackageUsedHistory.Rows[index];
                    string PackageUsedHistoryId = selectedRow.Cells[0].Value.ToString();
                    //updating the package used hisotry 
                    PackageUsedHistory packageUsedHistory = entity.PackageUsedHistories.Where(x => x.PackageUsedHistoryId == PackageUsedHistoryId && x.IsDelete == false).SingleOrDefault();
                    if (packageUsedHistory != null)
                    {
                        packageUsedHistory.IsDelete = true;
                        entity.PackageUsedHistories.AddOrUpdate(packageUsedHistory);
                        //updating package purchase invoice
                        PackagePurchasedInvoice packagePurchasedInvoice = entity.PackagePurchasedInvoices.Where(x => x.PackagePurchasedInvoiceId == packageUsedHistory.PackagePurchasedInvoiceId && x.IsDelete == false).SingleOrDefault();
                        if (packagePurchasedInvoice != null)
                        {
                            packagePurchasedInvoice.UseQty -= packageUsedHistory.UseQty;
                            entity.PackagePurchasedInvoices.AddOrUpdate(packagePurchasedInvoice);
                        }
                        entity.SaveChanges();
                        btnSearch_Click_1(this, new EventArgs());

                    }
                    this.BindPackageUsedHistoryGridView(0, 0, null);
                    this.BindPackageUseGridView(0, 0, null);
                    this.BindPackageUsedHistoryDeleteLogGridView(0, 0, null);
                }//end of DialogResult.OK

            }
        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            int customerId = 0; int memberId = 0;
            string patientID = null;

            if (cboCustomerId.SelectedIndex > 0) customerId = (int)cboCustomerId.SelectedValue;
            if (cbomemberId.SelectedIndex > 0) memberId = (int)cbomemberId.SelectedValue;
            patientID = txtPatientID.Text.Trim();
            BindPackageUseGridView(customerId, memberId, patientID);
            BindPackageUsedHistoryGridView(customerId, memberId, patientID);
            BindPackageUsedHistoryDeleteLogGridView(customerId, memberId, patientID);

        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            cboCustomerId.SelectedIndex = cbomemberId.SelectedIndex = 0;
            dtDate.Value = DateTime.Now;
            dtToDate.Value = DateTime.Now;
            txtPatientID.Clear();
            BindPackageUseGridView(0, 0, null);
            BindPackageUsedHistoryGridView(0, 0, null);
            BindPackageUsedHistoryDeleteLogGridView(0, 0, null);
        }

        private void usepackagetab_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangegvPurchaseInoivceCellColor();
        }

        private void usepackagetab_TabIndexChanged(object sender, EventArgs e)
        {
            ChangegvPurchaseInoivceCellColor();
        }
    }
}
