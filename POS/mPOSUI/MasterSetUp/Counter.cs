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
    public partial class Counter : Form
    {
        #region Variables

        POSEntities posEntity = new POSEntities();
        private ToolTip tp = new ToolTip();

        private bool isEdit = false;
        private int CounterId = 0;
        private bool EditClick = false;
        #endregion

        #region Method
        private void Clear()
        {
            isEdit = false;
            this.Text = "Add New Counter";
            groupBox1.Text = "Add New Counter";
            txtName.Text = string.Empty;
            CounterId = 0;
            btnAdd.Image = Properties.Resources.add_small;
        }

        private void Back_Data()
        {
            if (System.Windows.Forms.Application.OpenForms["MDIParent"] != null)
            {
                //this.ParentForm.Enabled = true;
                ////MDIParent newForm = (MDIParent)System.Windows.Forms.Application.OpenForms["MDIParent"];
                //var _counterName = (from c in posEntity.Counters where c.Id == MemberShip.CounterId select c.Name).FirstOrDefault();
                //((MDIParent)this.ParentForm).toolStripStatusLabel.Text = "Sales Person : " + MemberShip.UserName + " | Counter : " + _counterName + "";
                //this.Dispose();
            }

          
        }

        #endregion

        #region Event
        public Counter()
        {
            InitializeComponent();
        }
        private void Counter_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            bool notbackoffice=Utility.IsNotBackOffice();
            if (notbackoffice)
            {
                Utility.Gpvisible(groupBox1, isEdit);
            }

            dgvCounterList.AutoGenerateColumns = false;
            dgvCounterList.DataSource = (from c in posEntity.Counters where c.IsDelete==false select c).ToList();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
         
                tp.RemoveAll();
                tp.IsBalloon = true;
                tp.ToolTipIcon = ToolTipIcon.Error;
                tp.ToolTipTitle = "Error";
                if (txtName.Text.Trim() != string.Empty)
                {
                    APP_Data.Counter cObj = new APP_Data.Counter();
                  
                     APP_Data.Counter counter  = (from counterobj in posEntity.Counters where counterobj.Name == txtName.Text && ((CounterId == 0 && 1==1) || (CounterId >0 && counterobj.Id !=CounterId)) select counterobj).FirstOrDefault();
                    if (counter == null)
                    {
                        dgvCounterList.DataSource = "";

                        //Role Management
                        RoleManagementController controller = new RoleManagementController();
                        controller.Load(MemberShip.UserRoleId);
                        int counterId = 0;
                   
                          //New Counter
                        if (!isEdit)
                        {
                            if (controller.Counter.Add || MemberShip.isAdmin)
                            {
                                cObj.Name = txtName.Text;
                                cObj.IsDelete = false;
                                posEntity.Counters.Add(cObj);
                                posEntity.SaveChanges();
                                dgvCounterList.DataSource = (from c in posEntity.Counters select c).ToList();
                                MessageBox.Show("Successfully Saved!", "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                dgvCounterList.DataSource = (from b in posEntity.Counters orderby b.Id descending select b).ToList();
                                MessageBox.Show("You are not allowed to add new counter", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                        }

                          //Edit Current Brand
                        else
                        {
                            if (controller.Brand.EditOrDelete || MemberShip.isAdmin)
                            {

                                APP_Data.Counter EditCounter = posEntity.Counters.Where(x => x.Id == CounterId).FirstOrDefault();
                                EditCounter.Name = txtName.Text.Trim();
                                posEntity.SaveChanges();

                                dgvCounterList.DataSource = (from b in posEntity.Counters orderby b.Id descending select b).ToList();
                                Clear();
                                counterId = EditCounter.Id;
                            }
                            else
                            {
                                dgvCounterList.DataSource = (from b in posEntity.Counters orderby b.Id descending select b).ToList();
                                MessageBox.Show("You are not allowed to edit counter", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                            bool notbackoffice = Utility.IsNotBackOffice();
                            if (notbackoffice)
                            {
                                Utility.Gpvisible(groupBox1, false);
                            }

                        }
                       
                    }
                    else
                    {
                        tp.SetToolTip(txtName, "Error");
                        tp.Show("This counter name is already exist!", txtName);
                    }
                }
                else
                {
                    tp.SetToolTip(txtName, "Error");
                    tp.Show("Please fill up counter name!", txtName);
                }
                txtName.Text = "";
          
        }

        private void dgvCounterList_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            int currentId;
            List<APP_Data.Counter> cList = (from c in posEntity.Counters select c).ToList();

            if (e.RowIndex >= 0)
            {
                //Edit
                if (e.ColumnIndex ==2)
                {
                    bool notbackoffice = Utility.IsNotBackOffice();
                    //Role Management
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.Counter.EditOrDelete || MemberShip.isAdmin)
                    {
                        DataGridViewRow row = dgvCounterList.Rows[e.RowIndex];
                        currentId = Convert.ToInt32(row.Cells[0].Value);

                        APP_Data.Counter Counter = (from b in posEntity.Counters where b.Id == currentId select b).FirstOrDefault();
                        txtName.Text = Counter.Name;
                        isEdit = true;
                        this.Text = "Edit Counter";
                        groupBox1.Text = "Edit Counter";
                        this.btnAdd.Image = POS.Properties.Resources.update_small;
                        CounterId = Counter.Id;
                        //btnAdd.Image = Properties.Resources.save_small;
                      
                        if (notbackoffice)
                        {
                            Utility.Gpvisible(groupBox1, isEdit);
                        }
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to edit counters", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }

                //Delete
                if (e.ColumnIndex == 3)
                {

                    //Role Management
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.Counter.EditOrDelete || MemberShip.isAdmin)
                    {

                        if (cList.Count == 1)
                        {
                            MessageBox.Show("Counter table should have at least one counter!", "Enable to delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            if (result.Equals(DialogResult.OK))
                            {
                                Clear();
                                DataGridViewRow row = dgvCounterList.Rows[e.RowIndex];
                                currentId = Convert.ToInt32(row.Cells[0].Value);
                                int count = (from t in posEntity.Transactions where t.CounterId == currentId select t).ToList().Count;

                                if (MemberShip.CounterId == currentId)
                                {
                                    count += 1;
                                }
                                if (count < 1)
                                {
                                    dgvCounterList.DataSource = "";
                                    APP_Data.Counter Brand = (from c in posEntity.Counters where c.Id == currentId select c).FirstOrDefault();
                                    Brand.IsDelete = true;
                                   
                                    posEntity.SaveChanges();
                                    dgvCounterList.DataSource = (from c in posEntity.Counters select c).ToList();
                                    MessageBox.Show("Successfully Deleted!", "Delete Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }

                                else
                                {
                                    //To show message box 
                                    MessageBox.Show("This counter name is currently in use!", "Enable to delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to delete counters", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);     
                    }
                }

            }
        }

        private void Counter_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(txtName);
        }

        #endregion

        private void Counter_FormClosing(object sender, FormClosingEventArgs e)
        {
            Back_Data();
        }


    }
}
