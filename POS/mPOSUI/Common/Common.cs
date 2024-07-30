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

namespace POS
{
    public partial class Common : GroupBox
    {
        public Common()
        {
            InitializeComponent();
        }

        #region "Variable"
        private POSEntities entity = new POSEntities();
        public int? MemberTypeId = 0;
        public decimal TotalAmt = 0;
        public int? CustomerId = 0;
        public char type='C';
        public string TransactionId;
   
        #endregion


        #region "Method"
        public void Get_MType()
        {

            APP_Data.MemberCardRule data = (from a in entity.MemberCardRules where a.MemberTypeId == MemberTypeId select a).FirstOrDefault();

            if (data != null)
            {
                if (data.RangeTo != "Above")
                {
                    if (TotalAmt > Convert.ToDecimal(data.RangeTo))
                    {
                        MType();
                    }
                }
            }
            else
            {
                List<MemberCardRule> mR = (from p in entity.MemberCardRules select p).ToList();
                if (mR.Count > 0)
                {
                    //var minRange = mR.Min(r => r.RangeFrom);
                    //var maxRange = mR.Max(r => r.RangeFrom);

                 

                    var minRange = mR.Min(r => Convert.ToInt32(r.RangeFrom));
                    var maxRange = mR.Max(r => Convert.ToInt32(r.RangeFrom));

                    if (TotalAmt >= Convert.ToInt32(minRange))
                    {
                        MType();
                    }


                   
                }
            }
        }

        private void MType()
        {
            var list = (from a in entity.MemberCardRules select a).ToList();
            int mTypeId = 0;
            string mTName = "";
            for (int i = 0; i <= list.Count - 1; i++)
            {
                int rgFrom = Convert.ToInt32(list[i].RangeFrom);
                if (list[i].RangeTo == "Above")
                {
                    var above = (from m in entity.MemberCardRules where m.MemberTypeId == MemberTypeId select m.RangeTo).FirstOrDefault();
                    List<MemberCardRule> mR = (from p in entity.MemberCardRules select p).ToList();
                    var maxRange = mR.Max(r => r.RangeFrom);
                    if (above == "Above")
                    {
                        list[i].RangeTo = (TotalAmt + 1).ToString();
                    }
                    else
                    {
                        list[i].RangeTo = maxRange + 1;
                    }
                }
                int rgTo = Convert.ToInt32(list[i].RangeTo);
                int Amt = Convert.ToInt32(TotalAmt);
                if (Amt >= rgFrom && Amt <= rgTo)
                {
                    mTypeId = list[i].MemberTypeId;
                    mTName = (from p in entity.MemberTypes where p.Id == mTypeId select p.Name).FirstOrDefault();
                    break;
                }
            }
            Customer_Display(mTName);
        }

        private void Customer_Display(string mType)
        {
            // MessageBox.Show(" Amount get to " + mType + " Member Card! Please define member card!", "MPOS");

            var cName = (from c in entity.Customers where c.Id == CustomerId select c.Name).FirstOrDefault();
            NewCustomer form = new NewCustomer();
            if (cName == "Default")
            {
                form.isEdit = false;
                form.Text = "Add New Customer";
                form.MemerTypeName = mType;
                //form.MemberTypeID = mTypeId;
                form.TransactionId = TransactionId;
                form._from = "Sales";
                form.isEdit = false;
            }
            else
            {
                form.isEdit = true;
                form.Text = "Edit Customer";
                form.CustomerId = CustomerId;
                form.MemerTypeName = mType;
                form._from = "Sales";
                form.isEdit = true;
            }
            form.Type = type;
            form.ShowDialog();
        }

        #endregion

  
    }
}
