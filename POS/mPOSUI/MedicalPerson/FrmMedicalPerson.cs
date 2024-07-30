using POS.APP_Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS.mPOSUI.MedicalPerson
{
    public partial class FrmMedicalPerson : Form
    {
        private POSEntities entity = new POSEntities();
        private ToolTip tp = new ToolTip();
        public int? CustomerId { get; set; }
         public Boolean isEdit { get; set; }
         public char Type { get; set; }
        public FrmMedicalPerson()
        {
            InitializeComponent();
        }

        private void FrmMedicalPerson_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            cboTitle.Items.Add("Dr.");
            cboTitle.Items.Add("The.");
            cboTitle.Items.Add("Nu.");
            cboTitle.Items.Add("Nr.");
            cboTitle.Items.Add("Nrs.");
            cboTitle.SelectedIndex = 0;

            cboMatialStatus.Items.Add("Single");
            cboMatialStatus.Items.Add("Married");
            cboMatialStatus.Items.Add("Divorce");
            cboMatialStatus.SelectedIndex = 0;

            bindPersonType();
            bindCity();
            if (isEdit)
            {
                //Editing here        
                Customer currentCustomer = (from c in entity.Customers where c.Id == CustomerId select c).FirstOrDefault<Customer>();
    
                txtName.Text = currentCustomer.Name;
                txtPhoneNumber.Text = currentCustomer.PhoneNumber;
                txtNRC.Text = currentCustomer.NRC;
                txtAddress.Text = currentCustomer.Address;
                txtEmail.Text = currentCustomer.Email;
                cboTitle.Text = currentCustomer.Title;
                cboCity.Text = currentCustomer.City.CityName;
                cboMatialStatus.Text = currentCustomer.Maritalstatus;
                txtEmerPhone.Text = currentCustomer.EmergencyContactPhone;
                txtEmergencyContactName.Text = currentCustomer.EmergencyContactName;
                txtRelationship.Text = currentCustomer.Relationship;
                cboDoctorType.SelectedValue = currentCustomer.CustomerTypeId;

                if (currentCustomer.Gender == "Male")
                {
                    rdbMale.Checked = true;
                }
                else
                {
                    rdbFemale.Checked = true;
                }

                if (currentCustomer.Birthday == null)
                {
                    dtpBirthday.Value = DateTime.Now.Date;
                }
                else
                {
                    dtpBirthday.Value = currentCustomer.Birthday.Value.Date;
                }
                btnSubmit.Image = POS.Properties.Resources.update_big;
            }
        }
        public void bindPersonType()
        {
            entity = new POSEntities();
            List<APP_Data.CustomerType> customerTypeList = new List<APP_Data.CustomerType>();
            APP_Data.CustomerType customerType = new APP_Data.CustomerType();
            customerType.Id = 0;
            customerType.TypeName = "Select";
            customerTypeList.Add(customerType);
            customerTypeList.AddRange(entity.CustomerTypes.Where(x => x.TypeName == "doctor" || x.TypeName == "therapist" || x.TypeName == "nurse aid").ToList());           
            cboDoctorType.DataSource = customerTypeList;
            cboDoctorType.DisplayMember = "TypeName";
            cboDoctorType.ValueMember = "Id";
        }
        public void bindCity()
        {
            entity = new POSEntities();
            List<APP_Data.City> cityList = new List<APP_Data.City>();
            cityList.AddRange(entity.Cities.Where(x => x.IsDelete == false).ToList());
            cboCity.DataSource = cityList;
            cboCity.DisplayMember = "CityName";
            cboCity.ValueMember = "Id";
        }
        public void reloadCity(string CityName)
        {
            bindCity();
            cboCity.Text = CityName;
        }
        internal void reloadCity(int p)
        {
            throw new NotImplementedException();
        }

        private void btnAddCity_Click(object sender, EventArgs e)
        {
            City newForm = new City();
            newForm.ShowDialog();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Boolean hasError = false;

            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            //Validation
            if (txtName.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtName, "Error");
                tp.Show("Please fill up Medical Person name!", txtName);
                hasError = true;
            }
            else if (txtEmergencyContactName.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtEmergencyContactName, "Error");
                tp.Show("Please Fill Emergency Contact Name !", txtEmergencyContactName);
                hasError = true;
            }
            else if (txtEmerPhone.Text.Trim()==string.Empty)
            {
                tp.SetToolTip(txtEmerPhone, "Error");
                tp.Show("Please Fill Emergency Contact Phone !", txtEmerPhone);
                hasError = true;
            }
            else if (txtPhoneNumber.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtPhoneNumber, "Error");
                tp.Show("Please Fill Phone Number !", txtPhoneNumber);
                hasError = true;
            }            

            else if (cboCity.SelectedIndex == -1)
            {
                tp.SetToolTip(cboCity, "Error");
                tp.Show("Please fill up city name!", cboCity);
                hasError = true;
            }
            else if (cboDoctorType.SelectedIndex == 0)
            {
                tp.SetToolTip(cboDoctorType, "Error");
                tp.Show("Please fill up Medical Person Type!", cboDoctorType);
                hasError = true;
            }
            Customer currentCustomer = new Customer();
            if (!hasError)
            {
                if (isEdit)
                {
                    int CustomerName = 0;
                    CustomerName = (from p in entity.Customers where p.Name.Trim() == txtName.Text.Trim() && p.Id != CustomerId select p).ToList().Count;
                    if (CustomerName == 0)
                    {
                        currentCustomer = (from c in entity.Customers where c.Id == CustomerId select c).FirstOrDefault<Customer>();
                        currentCustomer.Title = cboTitle.Text;
                        currentCustomer.Name = txtName.Text;
                        currentCustomer.PhoneNumber = txtPhoneNumber.Text;
                        currentCustomer.NRC = txtNRC.Text;
                        currentCustomer.Address = txtAddress.Text;
                        currentCustomer.Email = txtEmail.Text;
                        currentCustomer.Maritalstatus = cboMatialStatus.Text;
                        currentCustomer.EmergencyContactPhone = txtEmerPhone.Text;
                        currentCustomer.EmergencyContactName = txtEmergencyContactName.Text;
                        currentCustomer.Relationship = txtRelationship.Text;
                        currentCustomer.CustomerTypeId =Convert.ToInt32( cboDoctorType.SelectedValue);
                        if (rdbMale.Checked == true)
                        {
                            currentCustomer.Gender = "Male";
                        }
                        else
                        {
                            currentCustomer.Gender = "Female";
                        }
                        if (dtpBirthday.Value.Date == DateTime.Now.Date)
                        {
                            currentCustomer.Birthday = null;
                        }
                        else
                        {
                            currentCustomer.Birthday = dtpBirthday.Value.Date;
                        }
                        currentCustomer.CityId = Convert.ToInt32(cboCity.SelectedValue.ToString());
                        entity.Entry(currentCustomer).State = EntityState.Modified;
                        entity.SaveChanges();

                        MessageBox.Show("Successfully Saved!", "Save");
                        clear();
                            
                        //  this.Dispose();
                        #region active PaidByCreditWithPrePaidDebt
                        if (System.Windows.Forms.Application.OpenForms["PaidByCreditWithPrePaidDebt"] != null)
                        {
                            PaidByCreditWithPrePaidDebt newForm = (PaidByCreditWithPrePaidDebt)System.Windows.Forms.Application.OpenForms["PaidByCreditWithPrePaidDebt"];
                            newForm.LoadForm();
                            this.Dispose();
                        }
                        #endregion
                        #region active PaidByCredit
                        if (System.Windows.Forms.Application.OpenForms["PaidByCreditWithPrePaidDebt"] != null)
                        {
                            PaidByCreditWithPrePaidDebt newForm = (PaidByCreditWithPrePaidDebt)System.Windows.Forms.Application.OpenForms["PaidByCreditWithPrePaidDebt"];
                            newForm.LoadForm();
                            this.Dispose();
                        }
                        #endregion

                        //refresh sales form's medical Person list
                        if (System.Windows.Forms.Application.OpenForms["FrmMedicalPersonList"] != null)
                        {
                            FrmMedicalPersonList newForm = (FrmMedicalPersonList)System.Windows.Forms.Application.OpenForms["FrmMedicalPersonList"];
                            newForm.DataBind();
                        }
                        if (System.Windows.Forms.Application.OpenForms["Sales"] != null)
                        {
                            Sales newForm = (Sales)System.Windows.Forms.Application.OpenForms["Sales"];

                            newForm.ReloadCustomerList();

                            if (Type != 'S')
                            {
                                newForm.SetCurrentCustomer(currentCustomer.Id);
                            }

                            this.Dispose();
                        }
                    }
                    else if (CustomerName > 0)
                    {
                        tp.SetToolTip(txtName, "Error");
                        tp.Show("This Person Name is already exist!", txtName);
                    }

                }
                else
                {
                    int CustomerName = 0;
                    CustomerName = (from p in entity.Customers where p.Name.Trim() == txtName.Text.Trim() select p).ToList().Count;
        
                   	        
		 if (CustomerName == 0)
                    {
                        var CustomerCode = entity.CustomerAutoID(DateTime.Now, SettingController.DefaultShop.ShortCode);
                        currentCustomer.Title = cboTitle.Text;
                        currentCustomer.Name = txtName.Text;
                        currentCustomer.PhoneNumber = txtPhoneNumber.Text;
                        currentCustomer.NRC = txtNRC.Text;
                        currentCustomer.Email = txtEmail.Text;
                        currentCustomer.Address = txtAddress.Text;
                        currentCustomer.CustomerCode = CustomerCode.FirstOrDefault().ToString();
                        currentCustomer.Relationship = txtRelationship.Text;
                        currentCustomer.Maritalstatus = cboMatialStatus.Text; 
                        currentCustomer.EmergencyContactName = txtEmergencyContactName.Text;
                        currentCustomer.EmergencyContactPhone = txtEmerPhone.Text;
                        currentCustomer.CustomerTypeId =Convert.ToInt32( cboDoctorType.SelectedValue);

                        if (rdbMale.Checked == true)
                        {
                            currentCustomer.Gender = "Male";
                        }
                        else
                        {
                            currentCustomer.Gender = "Female";
                        }
                        if (dtpBirthday.Value.Date == DateTime.Now.Date)
                        {
                            currentCustomer.Birthday = null;
                        }
                        else
                        {
                            currentCustomer.Birthday = dtpBirthday.Value.Date;
                        }
                        currentCustomer.CityId = Convert.ToInt32(cboCity.SelectedValue.ToString());
                        entity.Customers.Add(currentCustomer);
                        entity.SaveChanges();

                        //if (TransactionId != null)
                        //{
                        //    var Tran = (from t in entity.Transactions
                        //                where t.Id == TransactionId
                        //                select t).First();
                        //    Tran.CustomerId = currentCustomer.Id;
                        //    entity.SaveChanges();
                        //}

                        MessageBox.Show("Successfully Saved!", "Save");
                        clear();
                        //  this.Dispose();
                        #region active PaidByCreditWithPrePaidDebt
                        if (System.Windows.Forms.Application.OpenForms["PaidByCreditWithPrePaidDebt"] != null)
                        {
                            PaidByCreditWithPrePaidDebt newForm = (PaidByCreditWithPrePaidDebt)System.Windows.Forms.Application.OpenForms["PaidByCreditWithPrePaidDebt"];
                            newForm.LoadForm();
                            this.Dispose();
                        }
                        #endregion
                        #region active PaidByCredit
                        if (System.Windows.Forms.Application.OpenForms["PaidByCreditWithPrePaidDebt"] != null)
                        {
                            PaidByCreditWithPrePaidDebt newForm = (PaidByCreditWithPrePaidDebt)System.Windows.Forms.Application.OpenForms["PaidByCreditWithPrePaidDebt"];
                            newForm.LoadForm();
                            this.Dispose();
                        }
                        #endregion

                        //refresh sales form's customer list
                        if (System.Windows.Forms.Application.OpenForms["FrmMedicalPersonList"] != null)
                        {
                            FrmMedicalPersonList newForm = (FrmMedicalPersonList)System.Windows.Forms.Application.OpenForms["FrmMedicalPersonList"];
                            newForm.DataBind();
                        }

                        if (System.Windows.Forms.Application.OpenForms["Sales"] != null)
                        {
                            Sales newForm = (Sales)System.Windows.Forms.Application.OpenForms["Sales"];
                            if (Type == 'C')
                            {
                                newForm.New = "Yes";
                                newForm.ReloadCustomerList();
                                newForm.SetCurrentCustomer(currentCustomer.Id);
                            }
                            else
                            {
                                newForm.ReloadCustomerList();
                            }


                            this.Dispose();
                        }


                    }
	     
         else if (CustomerName > 0)
                    {
                        tp.SetToolTip(txtName, "Error");
                        tp.Show("This Customer Name is already exist!", txtName);
                    }
                }

            }

            
        }
        public void clear()
        {
            cboDoctorType.SelectedIndex = 0;
            cboTitle.SelectedIndex = 0;
            txtAddress.Text = "";
            txtEmail.Text = "";
            txtName.Text = "";
            txtNRC.Text = "";
            txtPhoneNumber.Text = "";
            txtEmerPhone.Text = "";
            txtEmergencyContactName.Text = "";
            txtRelationship.Text = "";
            cboMatialStatus.SelectedIndex = 0;
            dtpBirthday.Value = DateTime.Now.Date;
            rdbMale.Checked = true;
            int cityId = 0;
            cityId = SettingController.DefaultCity;
            APP_Data.City cus2 = (from c in entity.Cities where c.Id == cityId select c).FirstOrDefault();
            cboCity.Text = cus2.CityName;
            btnSubmit.Image = POS.Properties.Resources.save_big;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //clear();
            this.Close();
        }
        
    }
}
