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
using System.Data.Objects;


namespace POS
{
    public partial class ExpenseEntry : Form
    {
        #region variable
        POSEntities entity = new POSEntities();
        private ToolTip tp = new ToolTip();
        private decimal totalAmount;
        List<ExpenseController> expenseList = new List<ExpenseController>();
        public string ExpenseNo = "";
        public string expenseId = "";
        public bool IsAdd = false;
        List<int> expDetailIdList = new List<int>();

        #endregion

        #region event

        public ExpenseEntry()
        {
            InitializeComponent();
            CenterToScreen();
        }

     

        private void ExpenseEntry_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            Bind_ExpCag();
            if (expenseId == string.Empty)
            {
                dtDate.Enabled = true;
                totalAmount = 0;
            }
            else
            {
                dtDate.Enabled = false;
                btnCancel.Enabled = false;
                Expense editExp = (from exp in entity.Expenses where exp.Id == expenseId select exp).FirstOrDefault();
           
                if (editExp != null)
                {
                    dtDate.Value = editExp.ExpenseDate.Value;
                    txtExpNo.Text = editExp.Id.Trim();
                    cboExpenseCag.Text = (editExp.ExpenseCategory == null) ? "-" : editExp.ExpenseCategory.Name;
                    cboExpenseCag.Enabled = false;
                    cboExpenseCag.SelectedValue = editExp.ExpenseCategoryId;

                    txtTotalExpense.Text = Convert.ToInt32(editExp.TotalExpenseAmount).ToString();
                    totalAmount = Convert.ToDecimal(editExp.TotalExpenseAmount);
                    txtComment.Text = editExp.Comment.ToString();

                    var editDetailExp = (from p in entity.ExpenseDetails where p.ExpenseId == expenseId select p).ToList();

                    expenseList.AddRange(editDetailExp.Select(_exp =>
                        new ExpenseController
                        {
                            ExpenseDetailId = Convert.ToInt32(_exp.Id.ToString()),
                            ExpenseNo = _exp.ExpenseId.ToString(),
                            Description = _exp.Description.ToString(),
                            Qty = Convert.ToDecimal(_exp.Qty.ToString()),
                            Price = Convert.ToDecimal(_exp.Price.ToString()),
                            Amount = Convert.ToDecimal(_exp.Qty.ToString()) * Convert.ToDecimal(_exp.Price.ToString())

                        }
                        )
                        );
                    dgvExpenseList.DataSource = expenseList;
                    btnSave.Image = POS.Properties.Resources.update_big;
                    this.Text = "Update Expense";
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            bool hasError = false;
            dgvExpenseList.AutoGenerateColumns = false;
            Expense newexp = new Expense();
            if (cboExpenseCag.SelectedIndex == 0)
            {
                tp.SetToolTip(cboExpenseCag, "Error");
                tp.Show("Please fill up Expense category name!", cboExpenseCag);
                hasError = true;
            }
            else if (txtDescription.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtDescription, "Error");
                tp.Show("Please fill up Description for expense", txtDescription);
                hasError = true;
            }
            else if (txtQty.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtQty, "Error");
                tp.Show("Please fill up Qty", txtQty);
                hasError = true;
            }
            else if (txtPrice.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtPrice, "Error");
                tp.Show("Please fill up Price of expense", txtPrice);
                hasError = true;
            }
       
            else
            {
                dgvExpenseList.DataSource = "";
               
                if (!hasError)
                {
                    if (IsAdd == false)
                    {


                        int expCategoryID = Convert.ToInt32(cboExpenseCag.SelectedIndex);

                        ExpenseController exp = new ExpenseController();
                        exp.Description = txtDescription.Text.Trim();
                        exp.Qty = Convert.ToDecimal(txtQty.Text); ;
                        exp.Price = Convert.ToDecimal(txtPrice.Text);
                        exp.Amount = exp.Qty * exp.Price;
                        exp.CategoryId = (int)cboExpenseCag.SelectedValue;
                        exp.CategoryName = entity.ExpenseCategories.Where(x => x.Id == exp.CategoryId).FirstOrDefault().Name;


                        expenseList.Add(exp);

                        IsAdd = true;
                        if (IsAdd)
                        {
                            cboExpenseCag.Enabled = false;
                        }
                    }
                    else
                    {
                        cboExpenseCag.Enabled = false;

                        int expCategoryID = Convert.ToInt32(cboExpenseCag.SelectedIndex);

                        ExpenseController exp = new ExpenseController();
                        exp.Description = txtDescription.Text.Trim();
                        exp.Qty = Convert.ToDecimal(txtQty.Text); ;
                        exp.Price = Convert.ToDecimal(txtPrice.Text);
                        exp.Amount = exp.Qty * exp.Price;
                        exp.CategoryId = (int)cboExpenseCag.SelectedValue;
                        exp.CategoryName = entity.ExpenseCategories.Where(x => x.Id == exp.CategoryId).FirstOrDefault().Name;

                        expenseList.Add(exp);




                    }
                }
                dgvExpenseList.DataSource = expenseList;
                if (expenseId != string.Empty)
                {
                    totalAmount = Convert.ToDecimal(txtTotalExpense.Text);
                }
                totalAmount += Convert.ToInt32(txtQty.Text.ToString()) * Convert.ToInt32(txtPrice.Text.ToString());
                txtTotalExpense.Text = Convert.ToInt32(totalAmount).ToString();

                txtDescription.Text = "";
                txtQty.Text = "";
                txtPrice.Text = "";

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Expense MainExp = new Expense();
            entity = new POSEntities();
            if (expenseId != string.Empty)
            {
                Expense exp = entity.Expenses.Where(x => x.Id == expenseId).FirstOrDefault();
                exp.Id = expenseId;
                exp.ExpenseDate = dtDate.Value.Date;
                exp.IsApproved = false;
                exp.IsDeleted = false;
                exp.UpdatedDate = DateTime.Now;
                exp.UpdatedUser = MemberShip.UserId;
                exp.TotalExpenseAmount = totalAmount;
                exp.Comment = txtComment.Text;

                entity.SaveChanges();
                foreach (int id in expDetailIdList)
                {
                        ExpenseDetail deldetail = (from p in entity.ExpenseDetails where p.Id == id select p).FirstOrDefault();
                        if (deldetail != null)
                        {
                            entity.ExpenseDetails.Remove(deldetail);
                            entity.SaveChanges();
                        }
                    
                }
                foreach (ExpenseController explist in expenseList)
                {
                    if (explist.ExpenseDetailId == 0)
                    {
                        APP_Data.ExpenseDetail expnewadd = new APP_Data.ExpenseDetail();
                        expnewadd.ExpenseId = expenseId;
                        expnewadd.Description = explist.Description;
                        expnewadd.Qty = explist.Qty;
                        expnewadd.Price = explist.Price;
                        entity.ExpenseDetails.Add(expnewadd);
                        entity.SaveChanges();
                    }
                    else
                    {   
                            
                                      APP_Data.ExpenseDetail expeditdetail = entity.ExpenseDetails.Where(x => x.Id == explist.ExpenseDetailId).FirstOrDefault();
                                      entity.Entry(expeditdetail).State = EntityState.Modified;
                                      entity.SaveChanges();
                                
                    
                    }
                }

                if (System.Windows.Forms.Application.OpenForms["ExpenseList"] != null)
                {
                    ExpenseList newForm = (ExpenseList)System.Windows.Forms.Application.OpenForms["ExpenseList"];
                    newForm.DataBind();
                }
                MessageBox.Show("Successfully updated!", "update");
                this.Close();
                clearDate();
            }
            else
            {
                //auto generate expense number
                string month = "";
                if (dtDate.Value.Date.Month < 10)
                {
                    month = "0" + dtDate.Value.Date.Month.ToString();
                }
                else
                {
                    month = dtDate.Value.Date.Month.ToString();

                }
                string day = "";
                if (dtDate.Value.Date.Day < 10)
                {
                    day = "0" + dtDate.Value.Date.Day.ToString();
                }
                else
                {
                    day = dtDate.Value.Date.Day.ToString();
                }


                DateTime date=dtDate.Value.Date;
                int?  _count;
                _count = (from con in entity.Expenses where (EntityFunctions.TruncateTime(con.ExpenseDate) == date) orderby con.Id descending select con.Count ?? 0).FirstOrDefault();
                _count += 1;
                string ExpenseNumber;
                if (_count < 10)
                {
                    ExpenseNumber = "EP" + SettingController.DefaultShop.ShortCode + dtDate.Value.Date.Year.ToString() + month + day + ("0" + _count).ToString();
                }
                else
                {
                    ExpenseNumber = "EP" + SettingController.DefaultShop.ShortCode + dtDate.Value.Date.Year.ToString() + month + day + (_count).ToString();
                }
                if (expenseList.Count > 0)
                {
                    Expense expenseObj = new Expense();
                    expenseObj.Id = ExpenseNumber;
                    expenseObj.ExpenseDate = dtDate.Value.Date;
                    expenseObj.ExpenseCategoryId = Convert.ToInt32(cboExpenseCag.SelectedValue); ;
                    expenseObj.CreatedDate = DateTime.Now;
                    expenseObj.CreatedUser = MemberShip.UserId;
                    expenseObj.IsApproved = false;
                    expenseObj.IsDeleted = false;
                    expenseObj.TotalExpenseAmount = totalAmount;
                    expenseObj.Comment = txtComment.Text;
                    expenseObj.Count = _count;

                    entity.Expenses.Add(expenseObj);
                    entity.SaveChanges();


                    ExpenseDetail expDetailObj = new ExpenseDetail();
                    foreach (ExpenseController exp in expenseList)
                    {
                        expDetailObj.ExpenseId = (from p in entity.Expenses where p.Id == ExpenseNumber select p.Id).FirstOrDefault();
                        expDetailObj.Description = exp.Description;
                        expDetailObj.Qty = exp.Qty;
                        expDetailObj.Price = exp.Price;

                        entity.ExpenseDetails.Add(expDetailObj);
                        entity.SaveChanges();
                    }
                    MessageBox.Show("Successfully saved!", "save");
                    clearDate();
                }
                else
                {
                    MessageBox.Show("Please add at least one Expense.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cboExpenseCag.Enabled = true;
                }
            }

            if (System.Windows.Forms.Application.OpenForms["ExpenseList"] != null)
            {
                ExpenseList newForm = (ExpenseList)System.Windows.Forms.Application.OpenForms["ExpenseList"];
                newForm.DataBind();
            }
        }

        private void txtNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void Expense_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(txtDescription);
            tp.Hide(txtQty);
            tp.Hide(txtPrice);
            tp.Hide(cboExpenseCag);

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            clearDate();
           
        }


        private void dgvExpenseList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvExpenseList.Rows)
            {
                ExpenseController exp = (ExpenseController)row.DataBoundItem;
                row.Cells[0].Value = Convert.ToInt32(exp.ExpenseDetailId);
                row.Cells[2].Value = exp.Description.ToString();
                row.Cells[3].Value = Convert.ToInt32(exp.Qty);
                row.Cells[4].Value = Convert.ToInt32(exp.Price);
                row.Cells[5].Value = Convert.ToInt32(exp.Amount);
                row.Cells[6].Value = Convert.ToString(exp.CategoryName);
                row.Cells[8].Value = Convert.ToInt32(exp.CategoryId);
            }
        }

        private void dgvExpenseList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DialogResult re = MessageBox.Show("Do you want permanently delete this expense detail?", "Warning",MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (re.ToString() == DialogResult.OK.ToString())
            {
                if (e.RowIndex >= 0)
                {
                    if (e.ColumnIndex == 7)
                    {
                        int index = e.RowIndex;
                        int expDetailId = Convert.ToInt32(dgvExpenseList[0, e.RowIndex].Value);

                        //if (expenseId != string.Empty)
                        //{
                        //    ExpenseDetail deldetail = (from p in entity.ExpenseDetails where p.Id == expDetailId select p).FirstOrDefault();
                        //    if (deldetail != null)
                        //    {
                        //        entity.ExpenseDetails.Remove(deldetail);
                        //        entity.SaveChanges();
                        //    }
                        //}
                        if (expenseId != string.Empty) {
                            ExpenseDetail deldetail = (from p in entity.ExpenseDetails where p.Id == expDetailId select p).FirstOrDefault();
                            if (deldetail != null)
                            {
                                expDetailIdList.Add(expDetailId); 
                            }
                            
                            }
                      

                        ExpenseController expobj = expenseList[index];
                        expenseList.RemoveAt(index);

                        dgvExpenseList.DataSource = expenseList.ToList();

                        txtTotalExpense.Text = "";
                        totalAmount = 0;
                        totalAmount = Convert.ToDecimal(expenseList.Sum(x => x.Qty * x.Price));

                        txtTotalExpense.Text = totalAmount.ToString();



                    }
                }
            }
        }
        #endregion

        #region method
        public void Bind_ExpCag()
        {
            Utility.BindExpenseCategory(cboExpenseCag);
        }

        public void SetCurrentExpCag(long expCagId)
        {
            APP_Data.ExpenseCategory expCagList = (from e in entity.ExpenseCategories where e.Id == expCagId select e).FirstOrDefault();
            if (expCagList != null)
            {
                cboExpenseCag.SelectedValue = expCagList.Id;
            }
        }

        private void clearDate()
        {
            cboExpenseCag.Enabled = true;
            dgvExpenseList.DataSource = "";
            txtTotalExpense.Text = "";
            txtComment.Text = "";
            expenseList.Clear();
            totalAmount = 0;
            txtExpNo.Text = "";
            cboExpenseCag.SelectedIndex = 0;
            btnCancel.Enabled = true;
            expenseId = "";
        }
        #endregion

        private void btnExpCag_Click(object sender, EventArgs e)
        {
            ExpenseCategory form = new ExpenseCategory();
            form.ShowDialog();
        }

        private void btnexplist_Click(object sender, EventArgs e)
        {
          
        }

        private void btnexplist_Click_1(object sender, EventArgs e)
        {
            ExpenseList exp = new ExpenseList();
            exp.Show(this);
        }
    }
}
