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
    public partial class CustomerOffsetfrm : Form
    {
        public int customerID;
        POSEntities entities = new POSEntities();
private void CustomerOffsetfrm_Load(object sender, EventArgs e)
        {
            bindCustomerPackageDetail();
        }

        public CustomerOffsetfrm()
        {
            InitializeComponent();
        }
        private void bindCustomerPackageDetail()
        {
            var data = (from c in entities.PackageUsedHistories                      
                        where c.ActualOffsetBy == customerID && c.IsDelete == false
                        select new
                        {
                            customerID = c.Customer.Id,
                            Customer_Name=c.Customer.Name,
                            Transaction_ID = c.PackagePurchasedInvoice.TransactionDetail.TransactionId,
                            Transation_Date = c.PackagePurchasedInvoice.InvoiceDate,
                            Package_Name = c.PackagePurchasedInvoice.Product.Name,
                            Procedure_Qty = c.PackagePurchasedInvoice.packageFrequency * c.PackagePurchasedInvoice.TransactionDetail.Qty,
                            //Available_Qty = (c.PackagePurchasedInvoice.packageFrequency * c.PackagePurchasedInvoice.TransactionDetail.Qty) - c.PackagePurchasedInvoice.UseQty,
                            Offset_Qty = c.UseQty,
                            Used_Date=c.UsedDate,
                            Doctor_Name = c.CustomerIDAsDoctor,
                            Therapist_Name = c.CustomerIDAsTherapist,
                            Nurse_Aid = c.CustomerIDAsAssistantNurse,
                            Remark = c.Remark
                        }).OrderBy(y => y.Used_Date).ToList();
           
            if (data.Count > 0)
            {
                List<PackageUsedHistoryDataBind> packageUsedHistoryDataBindList = new List<PackageUsedHistoryDataBind>();
                int i = 0;

                foreach (var item in data)
                {
                    
                    i += item.Offset_Qty ;
                    PackageUsedHistoryDataBind packageUsedHistoryDataBind = new PackageUsedHistoryDataBind();
                    packageUsedHistoryDataBind.Offset_Qty = item.Offset_Qty;
                    packageUsedHistoryDataBind.Available_Qty =(int)(item.Procedure_Qty - i) ;                   
                    packageUsedHistoryDataBind.Procedure_Qty =Convert.ToInt32( item.Procedure_Qty);
                    packageUsedHistoryDataBind.Transaction_ID = item.Transaction_ID;
                    packageUsedHistoryDataBind.UsedDate = item.Used_Date;
                    packageUsedHistoryDataBind.Customer_Name = item.Customer_Name;
                    packageUsedHistoryDataBind.Doctor_Name = (from c in entities.Customers where c.Id == item.Doctor_Name select c.Name).SingleOrDefault();
                    packageUsedHistoryDataBind.Thrapist_Name = (from c in entities.Customers where c.Id == item.Therapist_Name select c.Name).SingleOrDefault();
                    packageUsedHistoryDataBind.Nurse_Aid = (from c in entities.Customers where c.Id == item.Nurse_Aid select c.Name).SingleOrDefault();
                    packageUsedHistoryDataBind.ProductName = item.Package_Name;
                    packageUsedHistoryDataBind.customerID = item.customerID;
                    packageUsedHistoryDataBind.TransactionDate = item.Transation_Date;
                    packageUsedHistoryDataBind.Remark = item.Remark;                    
                    packageUsedHistoryDataBindList.Add(packageUsedHistoryDataBind);
                }
                dgvPatientPacakgeDetail.DataSource = packageUsedHistoryDataBindList;
                dgvPatientPacakgeDetail.Columns["customerID"].Visible = false;
            }

            
        }
        class PackageUsedHistoryDataBind
        {
            public int customerID { get; set; }
            public string Transaction_ID { get; set; }
            public string Customer_Name { get; set; }
            public DateTime TransactionDate { get; set; }
            public DateTime UsedDate { get; set; }
            public string ProductName { get; set; }
            public int Offset_Qty { get; set; }
            public int Available_Qty { get; set; }
            public int Procedure_Qty { get; set; }
            public string Doctor_Name { get; set; }
            public string Thrapist_Name { get; set; }
            public string Nurse_Aid { get; set; }
            public string Remark { get; set; }
          
        }
    }
}
