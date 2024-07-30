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
    public partial class NewSupplier : Form
    {

        #region Variables

        POSEntities entity = new POSEntities();
        private ToolTip tp = new ToolTip();
        public Boolean isEdit { get; set; }
        public int SupplierId { get; set; }

        #endregion

        #region Event

        public NewSupplier()
        {
            InitializeComponent();
        }

        private void NewSupplier_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            if (isEdit)
            {
                Supplier supplierObj = (from s in entity.Suppliers where s.Id == SupplierId select s).FirstOrDefault();
                txtName.Text = supplierObj.Name;
                txtEmail.Text = supplierObj.Email;
                txtPhNo.Text = supplierObj.PhoneNumber;
                txtAddress.Text = supplierObj.Address;
                txtContactPerson.Text = supplierObj.ContactPerson;
                //requier to change update button
                btnSave.Image = POS.Properties.Resources.update_big;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool hasError = false;
            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            if (txtName.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtName, "Error");
                tp.Show("Please fill up supplier name!", txtName);
                hasError = true;
            }
            else if (txtPhNo.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtPhNo, "Error");
                tp.Show("Please fill up phone number!", txtPhNo);
                hasError = true;
            }
            else if (txtAddress.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtAddress, "Error");
                tp.Show("Please fill up address!", txtAddress);
                hasError = true;
            }
            else if (txtContactPerson.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtContactPerson, "Error");
                tp.Show("Please fill up contact person!", txtContactPerson);
                hasError = true;
            }
            if (!hasError)
            {
                if (isEdit)
                {

                   // Supplier subObj = entity.Suppliers.Where(x => x.Name == txtName.Text.Trim()).FirstOrDefault();
                    //if (subObj == null)
                    //{
                        Supplier updateSupplier = (from s in entity.Suppliers where s.Id == SupplierId select s).FirstOrDefault();
                        updateSupplier.Name = txtName.Text;
                        updateSupplier.PhoneNumber = txtPhNo.Text;
                        updateSupplier.Address = txtAddress.Text;
                        updateSupplier.Email = txtEmail.Text;
                        updateSupplier.ContactPerson = txtContactPerson.Text;
                        entity.Entry(updateSupplier).State = EntityState.Modified;
                        entity.SaveChanges();

                        MessageBox.Show("Successfully Update!", "Update");

                        #region active new PurchaseInput
                        if (System.Windows.Forms.Application.OpenForms["PurchaseInput"] != null)
                        {
                            PurchaseInput newForm = (PurchaseInput)System.Windows.Forms.Application.OpenForms["PurchaseInput"];
                            newForm.Reload();
                            newForm.SetCurrentSupplier(updateSupplier.Id);
                        }
                        #endregion

                        #region active new SupplierList
                        if (System.Windows.Forms.Application.OpenForms["SupplierList"] != null)
                        {
                            SupplierList newForm = (SupplierList)System.Windows.Forms.Application.OpenForms["SupplierList"];
                            newForm.Reload();
                            
                        }
                        #endregion

                        this.Dispose();
                    //}
                    //else
                    //{
                    //    MessageBox.Show("This supplier name is already existed!"); 
                    //}                  
                }
                else
                {

                    Supplier subObj=entity.Suppliers.Where(x=>x.Name==txtName.Text.Trim()).FirstOrDefault();
                    if (subObj == null)
                    {
                        Supplier supplierObj = new Supplier();
                        supplierObj.Name = txtName.Text;
                        supplierObj.PhoneNumber = txtPhNo.Text;
                        supplierObj.Email = txtEmail.Text;
                        supplierObj.Address = txtAddress.Text;
                        supplierObj.ContactPerson = txtContactPerson.Text;
                        entity.Suppliers.Add(supplierObj);
                        entity.SaveChanges();
                        MessageBox.Show("Successfully Saved!", "Save");
                        #region active new PurchaseInput
                        if (System.Windows.Forms.Application.OpenForms["PurchaseInput"] != null)
                        {
                            PurchaseInput newForm = (PurchaseInput)System.Windows.Forms.Application.OpenForms["PurchaseInput"];
                            newForm.Reload();
                            newForm.SetCurrentSupplier(supplierObj.Id);
                        }
                        #endregion
                        this.Dispose();

                        #region active new SupplierList
                        if (System.Windows.Forms.Application.OpenForms["SupplierList"] != null)
                        {
                            SupplierList newForm = (SupplierList)System.Windows.Forms.Application.OpenForms["SupplierList"];
                            newForm.Reload();
                        }
                        #endregion

                    }
                    else
                    {
                        MessageBox.Show("This supplier name is already existed!");
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtName.Text = "";
            txtPhNo.Text = "";
            txtAddress.Text = "";
            txtEmail.Text = "";
            txtContactPerson.Text = "";
            btnSave.Image = POS.Properties.Resources.save_big;
        }
     
        private void NewSupplier_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(txtName);
            tp.Hide(txtPhNo);
            tp.Hide(txtAddress);
            tp.Hide(txtContactPerson);
        }

        #endregion

       

        

        
    }
}
