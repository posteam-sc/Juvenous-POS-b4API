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
    public partial class GiftCardControl : Form
    {
        #region Variables

        POSEntities posEntity = new POSEntities();

        private ToolTip tp = new ToolTip();

        #endregion

        #region Event

        public GiftCardControl()
        {
            InitializeComponent();
        }

        private void GiftCardControl_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            dgvGiftCardList.AutoGenerateColumns = false;
            DataBind();
        }

        private void GiftCardControl_Activated(object sender, EventArgs e)
        {
            DataBind();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
             //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.GiftCard.Add || MemberShip.isAdmin)
            {

                Boolean hasError = false;
                tp.RemoveAll();
                tp.IsBalloon = true;
                tp.ToolTipIcon = ToolTipIcon.Error;
                tp.ToolTipTitle = "Error";
                if (txtCardNumber.Text.Trim() == string.Empty)
                {
                    tp.SetToolTip(txtCardNumber, "Error");
                    tp.Show("Please fill up gift card number!", txtCardNumber);
                    hasError = true;
                }
                else if (txtAmount.Text.Trim() == string.Empty)
                {
                    tp.SetToolTip(txtAmount, "Error");
                    tp.Show("Please fill up amount!", txtAmount);
                    hasError = true;
                }

                if (!hasError)
                {
                    GiftCard giftCardObj1 = new GiftCard();
                    GiftCard giftCardObj2 = (from gC in posEntity.GiftCards where gC.CardNumber == txtCardNumber.Text  && gC.IsDelete==false select gC).FirstOrDefault();

                    if (giftCardObj2 == null)
                    {
                        //dgvBrandList.DataSource = "";
                        giftCardObj1.CardNumber = txtCardNumber.Text;
                        giftCardObj1.Amount = Convert.ToInt32(txtAmount.Text);
                        giftCardObj1.IsDelete = false;
                        posEntity.GiftCards.Add(giftCardObj1);
                        posEntity.SaveChanges();

                        //dgvBrandList.DataSource = (from b in posEntity.Brands orderby b.Id descending select b).ToList();
                        MessageBox.Show("Successfully Saved!", "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        txtCardNumber.Text = "";
                        txtAmount.Text = "";

                        DataBind();
                    }
                    else
                    {
                        tp.SetToolTip(txtCardNumber, "Error");
                        tp.Show("This card number is already exist!", txtCardNumber);
                    }
                }
            }
            else
            {
                MessageBox.Show("You are not allowed to add new giftcard", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);      
            }
        }

        private void dgvGiftCardList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.RowIndex >= 0)
            {
                int currentGiftCardId = Convert.ToInt32(dgvGiftCardList.Rows[e.RowIndex].Cells[0].Value);
                int currentGiftCardAmount = Convert.ToInt32(dgvGiftCardList.Rows[e.RowIndex].Cells[2].Value);

                //Top Up
                if (e.ColumnIndex == 3)
                {
                    //Role Management
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.GiftCard.EditOrDelete || MemberShip.isAdmin)
                    {
                        TopUp newform = new TopUp();
                        newform.GiftCardId = currentGiftCardId;
                        newform.Show();
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to edit giftcards", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);      
                    }
                }
                //Delete
                else if (e.ColumnIndex == 4)
                {
                    //Role Management
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.GiftCard.EditOrDelete || MemberShip.isAdmin)
                    {
                        DialogResult result = MessageBox.Show("Are you sure you want to delete? This card has " + currentGiftCardAmount, "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (result.Equals(DialogResult.OK))
                        {
                            DataGridViewRow row = dgvGiftCardList.Rows[e.RowIndex];
                            GiftCard giftCardObj = (GiftCard)row.DataBoundItem;

                            bool IsAllowDelete = false;
                            var giftCardInTransaction = (from gt in posEntity.Transactions where gt.GiftCardId == giftCardObj.Id select gt).FirstOrDefault();

                            if (giftCardInTransaction != null)
                            {
                                if (giftCardInTransaction.IsDeleted == true)
                                {
                                    IsAllowDelete = true;
                                }
                                else
                                {
                                    IsAllowDelete = false;
                                }

                            }
                            else
                            {
                                IsAllowDelete = true;
                            }

                          //  if (giftCardObj.Transactions.Count == 0)
                              if(IsAllowDelete == true)
                           
                            {

                               // posEntity.GiftCards.Remove(giftCardObj);
                                giftCardObj.IsDelete = true;
                                posEntity.Entry(giftCardObj).State = EntityState.Modified;
                                posEntity.SaveChanges();
                                DataBind();
                                MessageBox.Show("Successfully Deleted!", "Delete Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else 
                            {
                                
                                MessageBox.Show("The Card is already used in transaction", "Unable to Delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to delete giftcards", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);      
                    }
                }
                //View Detail
                else if (e.ColumnIndex == 5)
                {
                    int gridGiftCardId = Convert.ToInt32(dgvGiftCardList.Rows[e.RowIndex].Cells[0].Value.ToString());
                   
                        GiftCardTransactionHistory newForm = new GiftCardTransactionHistory();
                        newForm.GiftCardId = gridGiftCardId;
                        newForm.ShowDialog();
                   
                }
            }
        }

        private void GiftCardControl_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(txtCardNumber);
        }

        #endregion

        #region Function 

        private void DataBind()
        {
            posEntity = new POSEntities();

            string cardNo = txtSearchCardNo.Text;
            dgvGiftCardList.DataSource = "";

            dgvGiftCardList.DataSource = posEntity.GiftCards.Where(x=>x.IsDelete==false && (cardNo=="" && 1==1) || (cardNo != "" && x.CardNumber==cardNo)).ToList();
        }

        #endregion

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataBind();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            
            txtSearchCardNo.Clear();
            DataBind();
        }

        //private void txtCatdNumber_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
        //    {
        //        e.Handled = true;
        //    } 
        //}

    }
}
