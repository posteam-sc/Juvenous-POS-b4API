using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Objects;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS.APP_Data;

namespace POS
{
    public partial class DraftList : Form
    {

        #region Variables

        private POSEntities entity = new POSEntities();


        #endregion

        public DraftList()
        {
            InitializeComponent();
        }

        private void DraftList_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            dgvDraftList.Columns["TableNo"].Visible = SettingController.UseTable;
            LoadData();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgvDraftList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvDraftList.Rows)
            {
                Transaction currentt = (Transaction)row.DataBoundItem;
                row.Cells[1].Value = currentt.DateTime.Value.ToString("dd-MM-yyyy");
                row.Cells[2].Value = currentt.DateTime.Value.ToString("hh:mm");
                row.Cells[3].Value = currentt.User.Name;
                row.Cells[4].Value = currentt.Customer==null?"": currentt.Customer.Name;            
                row.Cells[5].Value = currentt.TransactionDetails.Sum(x => x.TotalAmount);
                if (SettingController.UseTable)
                {
                    row.Cells[6].Value = entity.RestaurantTables.Find(currentt.TableIdOrQue).Number;
                }
               
            }
        }

        private void LoadData()
        {
            DateTime fromDate = dtpFrom.Value.Date;
            DateTime toDate = dtpTo.Value.Date;
            List<Transaction> transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == false && t.IsDeleted == false select t).ToList<Transaction>();
            dgvDraftList.AutoGenerateColumns = false;
            dgvDraftList.DataSource = transList;
        }

        private void dgvDraftList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                //Select
                if (e.ColumnIndex == 7)
                {
                    if (System.Windows.Forms.Application.OpenForms["Sales"] != null)
                    {
                        Sales openedForm = (Sales)System.Windows.Forms.Application.OpenForms["Sales"];
                        openedForm.LoadDraft(dgvDraftList.Rows[e.RowIndex].Cells[0].Value.ToString());
                        this.Dispose();
                    }

                }
                //View Detail
                else if (e.ColumnIndex == 8)
                {
                    DraftDetail form = new DraftDetail();
                    form.DraftId = dgvDraftList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    form.Show();

                }
                //Delete
                if (e.ColumnIndex == 9)
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (result.Equals(DialogResult.OK))
                    {
                        //Delete transaction firstly
                        Transaction currentt = (Transaction)dgvDraftList.Rows[e.RowIndex].DataBoundItem;
                        entity.Transactions.Remove(currentt);
                        if (SettingController.UseTable && currentt.TableIdOrQue!=null)
                        {
                            RestaurantTable tableupd = entity.RestaurantTables.Find(currentt.TableIdOrQue);
                            tableupd.Status = true;
                            entity.Entry(tableupd).State = EntityState.Modified;

                        }
                       
                        entity.SaveChanges();
                        LoadData();
                        //if (System.Windows.Forms.Application.OpenForms["Sales"] != null)
                        //{
                        //    Sales openedForm = (Sales)System.Windows.Forms.Application.OpenForms["Sales"];
                        //    openedForm.ForRestaurant();
                            
                        //}
                    }
                }
            }
        }

    }
}
