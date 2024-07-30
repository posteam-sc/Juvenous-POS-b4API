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
    public partial class Promotion_List : Form
    {
        #region Variable
        POSEntities entity = new POSEntities();
        #endregion

        #region Event
        public Promotion_List()
        {
            InitializeComponent();
        }

        private void Promotion_List_Load(object sender, EventArgs e)
        {
            Bind_Promotion();
        }

        public void Bind_Promotion()
        {
            entity = new POSEntities();
            dgvPromotionList.AutoGenerateColumns = false;
            dgvPromotionList.DataSource = entity.GiftSystems.ToList();
        }

        private void dgvPromotionList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvPromotionList.Rows)
            {
                GiftSystem giftObj = (GiftSystem)row.DataBoundItem;
                row.Cells[0].Value = giftObj.Id;
                row.Cells[1].Value = giftObj.Name;
                row.Cells[2].Value = giftObj.ValidFrom.Date;
                row.Cells[3].Value = giftObj.ValidTo.Date;
                row.Cells[4].Value = giftObj.IsActive;
            }
        }

        private void dgvPromotionList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                RoleManagementController controller = new RoleManagementController();
                controller.Load(MemberShip.UserRoleId);
                int currentId = Convert.ToInt32(dgvPromotionList.Rows[e.RowIndex].Cells[0].Value);
                //view detail
                if (e.ColumnIndex == 5)
                {                    
                    PromotionDetail newForm = new PromotionDetail();
                    newForm.currentPromotionId = currentId;
                    newForm.ShowDialog();
                  
                }
                //edit
                else if (e.ColumnIndex == 6)
                {
                    if (controller.Promotion.EditOrDelete || MemberShip.isAdmin)
                    {
                        PromotionSystem newForm = new PromotionSystem();
                        newForm.currentPromotionId = currentId;
                        newForm.isEdit = true;
                        newForm.Text = "Edit Promotion System";
                        newForm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to edit promotion", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                //delete
                else if (e.ColumnIndex == 7)
                {
                    if (controller.Promotion.EditOrDelete || MemberShip.isAdmin)
                    {
                        DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (result.Equals(DialogResult.OK))
                        {
                            List<GiftSystemInTransaction> deleteList = entity.GiftSystemInTransactions.Where(x => x.GiftSystemId == currentId).ToList();
                            if (deleteList.Count > 0)
                            {
                                MessageBox.Show("This promotion is used in transaction!", "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else
                            {
                                GiftSystem deleteObj = entity.GiftSystems.Where(x => x.Id == currentId).FirstOrDefault();
                                entity.GiftSystems.Remove(deleteObj);
                                entity.SaveChanges();
                                dgvPromotionList.DataSource = entity.GiftSystems.ToList();
                                MessageBox.Show("Successfully Deleted!", "Delete Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to delete promotion", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        #endregion

        #region Function

        public void DataBind()
        {
            
            dgvPromotionList.DataSource = entity.GiftSystems.ToList();
        }

        #endregion
    }
}
