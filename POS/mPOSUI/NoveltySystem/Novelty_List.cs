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
    public partial class Novelty_List : Form
    {
        #region Varible
        POSEntities entity = new POSEntities();
        #endregion
        public Novelty_List()
        {
            InitializeComponent();
        }

        private void Novelty_List_Load(object sender, EventArgs e)
        {
            Bind_Novelty();
        }

        private void dgvNoveltyList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                RoleManagementController controller = new RoleManagementController();
                controller.Load(MemberShip.UserRoleId);
                int currentId = Convert.ToInt32(dgvNoveltyList.Rows[e.RowIndex].Cells[0].Value);
                if (e.ColumnIndex == 4)
                {
                    Novelty_Detail newForm = new Novelty_Detail();
                    newForm.noveltyId = Convert.ToInt32(dgvNoveltyList.Rows[e.RowIndex].Cells[0].Value);
                    newForm.ShowDialog();
                }
                else if (e.ColumnIndex == 5)
                {
                    if (controller.Novelty.EditOrDelete || MemberShip.isAdmin)
                    {
                        //  APP_Data.NoveltySystem editObj = entity.NoveltySystems.FirstOrDefault(x => x.Id == currentId);
                        //List<ProductInNovelty> PList = entity.ProductInNovelties.Where(x => x.NoveltySystemId == currentId).ToList();
                        // bool isEdit = true;
                        // DateTime fromDate = editObj.ValidFrom.Value;
                        // DateTime toDate = editObj.ValidTo.Value;
                        //List<Transaction> tList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                        //foreach (ProductInNovelty p in PList)
                        //{
                        //    foreach (Transaction t in tList)
                        //    {
                        //        if (t.TransactionDetails.Where(x => x.ProductId == p.ProductId).Count() > 0)
                        //        {
                        //            isEdit = false;
                        //        }
                        //    }
                        //}
                        //if (isEdit)
                        //{
                        NoveltySystem newForm = new NoveltySystem();
                        newForm.isEdit = true;
                        newForm.Text = "Edit Novelty System ";
                        newForm.noveltyId = currentId;
                        newForm.ShowDialog();
                        //}
                        //else
                        //{
                        //    //To show message box 
                        //    MessageBox.Show("This novelty system is currently in use!", "Enable to edit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //}

                        //}
                        //else
                        //{
                        //    MessageBox.Show("You are not allowed to edit novelty", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        //}
                    }
                }
                else if (e.ColumnIndex == 6)
                {
                    if (controller.Novelty.EditOrDelete || MemberShip.isAdmin)
                    {
                        DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (result.Equals(DialogResult.OK))
                        {
                            APP_Data.NoveltySystem deleteObj = entity.NoveltySystems.FirstOrDefault(x => x.Id == currentId);
                            List<ProductInNovelty> PList = entity.ProductInNovelties.Where(x => x.NoveltySystemId == currentId).ToList();
                            bool isdelete = true;
                            DateTime fromDate = deleteObj.ValidFrom.Value;
                            DateTime toDate = deleteObj.ValidTo.Value;
                            List<Transaction> tList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                            foreach (ProductInNovelty p in PList)
                            {
                                foreach (Transaction t in tList)
                                {
                                    if(t.TransactionDetails.Where(x => x.ProductId == p.ProductId).Count() > 0)
                                    {
                                        isdelete = false;
                                    }
                                }
                            }
                            if (isdelete)
                            {
                                entity.NoveltySystems.Remove(deleteObj);
                                entity.SaveChanges();

                                dgvNoveltyList.AutoGenerateColumns = false;
                                dgvNoveltyList.DataSource = entity.NoveltySystems.ToList();
                            }
                            else
                            {
                                //To show message box 
                                MessageBox.Show("This novelty system is currently in use!", "Unable to delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to delete novelty", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void dgvNoveltyList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow r in dgvNoveltyList.Rows)
            {
                APP_Data.NoveltySystem noveltySysObj = (APP_Data.NoveltySystem)r.DataBoundItem;
                r.Cells[0].Value = noveltySysObj.Id;
                r.Cells[1].Value = noveltySysObj.Brand.Name;
                r.Cells[2].Value = noveltySysObj.ValidFrom.Value.Date;
                r.Cells[3].Value = noveltySysObj.ValidTo.Value.Date;
            }
        }

        #region Function
        public void Bind_Novelty()
        {
            dgvNoveltyList.AutoGenerateColumns = false;
            dgvNoveltyList.DataSource = entity.NoveltySystems.ToList();
        }
        #endregion
    }
}
