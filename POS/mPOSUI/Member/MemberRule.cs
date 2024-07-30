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
using System.Text.RegularExpressions;

namespace POS
{
    public partial class MemberRule : Form
    {
        #region Variables

        private bool isEdit = false;

        public int ruleID { get; set; }

        private POSEntities entity = new POSEntities();

        private ToolTip tp = new ToolTip();

        MemberCardRule currMember=new MemberCardRule();

        #endregion

        public MemberRule()
        {
            InitializeComponent();
        }

        private void btnAddMember_Click(object sender, EventArgs e)
        {
            newMemberType newType = new newMemberType();
            newType.ShowDialog();
        }

        private void btnAddMember_Click_1(object sender, EventArgs e)
        {
            newMemberType newType = new newMemberType();
            newType.ShowDialog();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            if (cboMemberRule.Text == "Select")
            {
                tp.SetToolTip(cboMemberRule, "Error");
                tp.Show("Please select Member Type!", cboMemberRule);
            }
            else if (txtFrom.Text.Trim() == string.Empty || txtFrom.Text.Trim() == "0")
            {
                tp.SetToolTip(txtFrom, "Error");
                tp.Show("Please fill up Initial Amount!", txtFrom);
            }
            else if (rbtBetween.Checked && txtTo.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtTo, "Error");
                tp.Show("Please fill up Amount!", txtTo);
            }
            else if (txtMemberDis.Text.Trim() == string.Empty || txtMemberDis.Text.Trim() == "0")
            {
                tp.SetToolTip(txtMemberDis, "Error");
                tp.Show("Please fill up Member Card Discount!", txtMemberDis);
            }
            else if (txtBirthdayDis.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtBirthdayDis, "Error");
                tp.Show("Please fill up Birthday Discount!", txtBirthdayDis);
            }
            else if (rbtBetween.Checked && txtTo.Text.Trim() == "Above")
            {
                tp.SetToolTip(txtFrom, "Error");
                tp.Show("Please check your input Amount!", txtFrom);
            }
            else if (rbtBetween.Checked && Convert.ToInt32(txtFrom.Text.Trim()) >= Convert.ToInt32(txtTo.Text.Trim()))
            {
                tp.SetToolTip(txtTo, "Error");
                tp.Show("Please check your input Amount!", txtTo);
            }
            else if (Convert.ToInt32(txtMemberDis.Text.Trim()) <0 || Convert.ToInt32(txtMemberDis.Text.Trim()) > 100)
            {
                tp.SetToolTip(txtMemberDis, "Error");
                tp.Show("Please check your input for Member Discount!", txtMemberDis);
            }
            else if (Convert.ToInt32(txtBirthdayDis.Text.Trim()) < 0 || Convert.ToInt32(txtBirthdayDis.Text.Trim()) > 100)
            {
                tp.SetToolTip(txtBirthdayDis, "Error");
                tp.Show("Please check your input for Birthday Discount!", txtBirthdayDis);
            }
            else
            {
                APP_Data.MemberCardRule mRule = new APP_Data.MemberCardRule();
                //Role Management
                RoleManagementController controller = new RoleManagementController();
                controller.Load(MemberShip.UserRoleId);

                //Add new rule
                if (!isEdit)
                {
                    if (controller.MemberRule.Add || MemberShip.isAdmin)
                    {
                        bool IsSuccessful = false;
                        string mType = cboMemberRule.Text;
                        int mTypeid = (from m in entity.MemberTypes where m.Name == mType select m.Id).SingleOrDefault();
                        string rFrom = txtFrom.Text.Trim();
                        string rTo = null;
                        if (rbtBetween.Checked)
                        {
                            rTo = txtTo.Text.Trim();
                        }
                        else
                        {
                            rTo = "Above";
                        }
                        int mDis = Convert.ToInt32(txtMemberDis.Text.Trim());
                        int bDis = Convert.ToInt32(txtBirthdayDis.Text.Trim());
                        int count = entity.MemberCardRules.Count();
                        if (count == 0)
                        {
                            mRule.MemberTypeId = mTypeid;
                            mRule.RangeFrom = rFrom;
                            mRule.RangeTo = rTo;
                            mRule.MCDiscount = mDis;
                            mRule.BDDiscount = bDis;
                            entity.MemberCardRules.Add(mRule);
                            entity.SaveChanges();
                            reloadMember();
                            reloadMemberList();
                            Clear();
                            IsSuccessful = true;
                        }
                        else
                        {
                            int minRF = (from m in entity.MemberCardRules.AsEnumerable() select Convert.ToInt32(m.RangeFrom)).Min();
                            int minRT = 0;
                            int countRange = (from m in entity.MemberCardRules where m.RangeTo != "Above" select m).Count();
                            if (countRange != 0)
                            {
                                minRT = (from m in entity.MemberCardRules.AsEnumerable() where m.RangeTo != "Above" select Convert.ToInt32(m.RangeTo)).Min();
                            }
                            string miniRT = (from m in entity.MemberCardRules select m.RangeTo).Min();
                            int checkRT;
                            if (miniRT != "Above")
                            {
                                checkRT = minRT;
                            }
                            else
                            {
                                checkRT = minRF;
                            }
                            int maxiRT = 0;
                            if (countRange != 0)
                            {
                                maxiRT = (from m in entity.MemberCardRules.AsEnumerable() where m.RangeTo != "Above" select Convert.ToInt32(m.RangeTo)).Max();
                            }
                            string maxRT = (from m in entity.MemberCardRules select m.RangeTo).Max();
                            if (maxRT != "Above")
                            {
                                maxRT = maxiRT.ToString();
                            }
                            int maxRF = (from m in entity.MemberCardRules.AsEnumerable() select Convert.ToInt32(m.RangeFrom)).Max();
                            if (rbtBetween.Checked)
                            {
                                if (Convert.ToInt32(rTo) < minRF)
                                {
                                    mRule.MemberTypeId = mTypeid;
                                    mRule.RangeFrom = rFrom;
                                    mRule.RangeTo = rTo;
                                    mRule.MCDiscount = mDis;
                                    mRule.BDDiscount = bDis;
                                    entity.MemberCardRules.Add(mRule);
                                    entity.SaveChanges();
                                    reloadMember();
                                    reloadMemberList();
                                    Clear();
                                    IsSuccessful = true;
                                }
                                else if (count == 1 && maxRT != "Above" && Convert.ToInt32(rFrom) > checkRT)
                                {
                                    mRule.MemberTypeId = mTypeid;
                                    mRule.RangeFrom = rFrom;
                                    mRule.RangeTo = rTo;
                                    mRule.MCDiscount = mDis;
                                    mRule.BDDiscount = bDis;
                                    entity.MemberCardRules.Add(mRule);
                                    entity.SaveChanges();
                                    reloadMember();
                                    reloadMemberList();
                                    Clear();
                                    IsSuccessful = true;
                                }
                                else if (Convert.ToInt32(rFrom) >= minRF && maxRF >= Convert.ToInt32(rTo))
                                {
                                    List<APP_Data.MemberCardRule> cardList = new List<APP_Data.MemberCardRule>();
                                    bool isCorrect = false;
                                    cardList = (from m in entity.MemberCardRules orderby m.RangeTo ascending select m).ToList();
                                    foreach (MemberCardRule mcr in cardList)
                                    {
                                        int mcrRF = Convert.ToInt32(mcr.RangeFrom);
                                        int minimumRT = 0;
                                        string minFrom = (from m in entity.MemberCardRules.AsEnumerable() where m.Id != mcr.Id && (Convert.ToInt32(m.RangeFrom)) >= mcrRF select m.RangeFrom).Min();
                                        if (minFrom != null)
                                        {
                                            minimumRT = Convert.ToInt32(minFrom);
                                        }
                                        else
                                        {
                                            minimumRT = 0;
                                        }
                                        if (mcr.RangeTo != "Above")
                                        {
                                            if (Convert.ToInt32(mcr.RangeTo) < Convert.ToInt32(rFrom) && minimumRT > Convert.ToInt32(rTo))
                                            {
                                                mRule.MemberTypeId = mTypeid;
                                                mRule.RangeFrom = rFrom;
                                                mRule.RangeTo = rTo;
                                                mRule.MCDiscount = mDis;
                                                mRule.BDDiscount = bDis;
                                                entity.MemberCardRules.Add(mRule);
                                                entity.SaveChanges();
                                                reloadMember();
                                                reloadMemberList();
                                                Clear();
                                                IsSuccessful = true;
                                                isCorrect = false;
                                                break;
                                            }
                                            else
                                            {
                                                isCorrect = true;
                                            }
                                        }
                                    }
                                    if (isCorrect == true)
                                    {
                                        MessageBox.Show("Please check your Amount!");
                                    }
                                }
                                else
                                {
                                    if (maxRT == "Above" && Convert.ToInt32(rFrom) > maxRF)
                                    {
                                        MessageBox.Show("Please check your Amount.You have already entered amount " + maxRF.ToString() + " - " + maxRT + " in one of your member type!");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Please check your Amount!");
                                    }
                                }
                            }
                            else
                            {
                                if (maxRT == "Above")
                                {
                                    MessageBox.Show("Please check your Amount.You have already entered amount \"" + maxRF.ToString() + " - " + maxRT + "\" in one of your member type!");
                                }
                                else
                                {
                                    if (Convert.ToInt32(maxRT) < Convert.ToInt32(rFrom))
                                    {
                                        mRule.MemberTypeId = mTypeid;
                                        mRule.RangeFrom = rFrom;
                                        mRule.RangeTo = rTo;
                                        mRule.MCDiscount = mDis;
                                        mRule.BDDiscount = bDis;
                                        entity.MemberCardRules.Add(mRule);
                                        entity.SaveChanges();
                                        reloadMember();
                                        reloadMemberList();
                                        Clear();
                                        IsSuccessful = true;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Please check your Amount!");
                                    }
                                }
                            }
                        }
                        if (IsSuccessful == true)
                        {
                            MessageBox.Show("Successfully Added!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to add new Member Card Rule", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                //edit rule
                else
                {
                    if (controller.MemberRule.EditOrDelete || MemberShip.isAdmin)
                    {
                        bool isSuccess = false;
                        APP_Data.MemberCardRule EditMemberRule = entity.MemberCardRules.Where(x => x.Id == ruleID).FirstOrDefault();
                        string mType = cboMemberRule.Text;
                        int mTypeid = (from m in entity.MemberTypes where m.Name == mType select m.Id).SingleOrDefault();
                        string rFrom = txtFrom.Text.Trim();
                        string rTo = null;
                        if (rbtBetween.Checked)
                        {
                            rTo = txtTo.Text.Trim();
                        }
                        else
                        {
                            rTo = "Above";
                        }
                        int mDis = Convert.ToInt32(txtMemberDis.Text.Trim());
                        int bDis = Convert.ToInt32(txtBirthdayDis.Text.Trim());
                        int count = entity.MemberCardRules.Count();
                        if (count == 1)
                        {
                            EditMemberRule.MemberTypeId = mTypeid;
                            EditMemberRule.RangeFrom = rFrom;
                            EditMemberRule.RangeTo = rTo;
                            EditMemberRule.MCDiscount = mDis;
                            EditMemberRule.BDDiscount = bDis;
                            entity.SaveChanges();
                            reloadMember();
                            reloadMemberList();
                            Clear();
                            isSuccess = true;
                        }
                        else
                        {
                            if (EditMemberRule.RangeFrom == rFrom && EditMemberRule.RangeTo == rTo)
                            {
                                EditMemberRule.MemberTypeId = mTypeid;
                                EditMemberRule.RangeFrom = rFrom;
                                EditMemberRule.RangeTo = rTo;
                                EditMemberRule.MCDiscount = mDis;
                                EditMemberRule.BDDiscount = bDis;
                                entity.SaveChanges();
                                reloadMember();
                                reloadMemberList();
                                Clear();
                                isSuccess = true;
                            }
                            else
                            {
                                int minRF = (from m in entity.MemberCardRules.AsEnumerable() select Convert.ToInt32(m.RangeFrom)).Min();
                                string miniRF = minRF.ToString();
                                int minID = (from m in entity.MemberCardRules where m.RangeFrom == miniRF select m.Id).SingleOrDefault();
                                string minRTbyID = (from m in entity.MemberCardRules where m.Id == minID && m.RangeTo != "Above" select m.RangeTo).SingleOrDefault();
                                int maxiRT = (from m in entity.MemberCardRules.AsEnumerable() where m.RangeTo != "Above" select Convert.ToInt32(m.RangeTo)).Max();
                                string maxRT = (from m in entity.MemberCardRules select m.RangeTo).Max();
                                if (maxRT != "Above")
                                {
                                    maxRT = maxiRT.ToString();
                                }
                                int maxRF = (from m in entity.MemberCardRules.AsEnumerable() select Convert.ToInt32(m.RangeFrom)).Max();
                                if (rbtBetween.Checked)
                                {
                                    if (Convert.ToInt32(rTo) < minRF)
                                    {
                                        EditMemberRule.MemberTypeId = mTypeid;
                                        EditMemberRule.RangeFrom = rFrom;
                                        EditMemberRule.RangeTo = rTo;
                                        EditMemberRule.MCDiscount = mDis;
                                        EditMemberRule.BDDiscount = bDis;
                                        entity.SaveChanges();
                                        reloadMember();
                                        reloadMemberList();
                                        Clear();
                                        isSuccess = true;
                                    }
                                    else if (minID == ruleID && Convert.ToInt32(rTo) <= Convert.ToInt32(minRTbyID))
                                    {
                                        EditMemberRule.MemberTypeId = mTypeid;
                                        EditMemberRule.RangeFrom = rFrom;
                                        EditMemberRule.RangeTo = rTo;
                                        EditMemberRule.MCDiscount = mDis;
                                        EditMemberRule.BDDiscount = bDis;
                                        entity.SaveChanges();
                                        reloadMember();
                                        reloadMemberList();
                                        Clear();
                                        isSuccess = true;
                                    }
                                    else if (Convert.ToInt32(rFrom) >= minRF && maxRF >= Convert.ToInt32(rTo))
                                    {
                                        List<APP_Data.MemberCardRule> cardList = new List<APP_Data.MemberCardRule>();
                                        bool isCorrect = false;
                                        cardList = (from m in entity.MemberCardRules where m.Id != ruleID orderby m.RangeTo ascending select m).ToList();
                                        foreach (MemberCardRule mcr in cardList)
                                        {
                                            if (mcr.RangeTo != "Above")
                                            {
                                                string minRange = (from m in entity.MemberCardRules.AsEnumerable() where Convert.ToInt32(m.RangeFrom) > Convert.ToInt32(mcr.RangeTo) select m.RangeFrom).Min();
                                                if (Convert.ToInt32(mcr.RangeTo) < Convert.ToInt32(rFrom) && Convert.ToInt32(minRange) > Convert.ToInt32(rTo))
                                                {
                                                    EditMemberRule.MemberTypeId = mTypeid;
                                                    EditMemberRule.RangeFrom = rFrom;
                                                    EditMemberRule.RangeTo = rTo;
                                                    EditMemberRule.MCDiscount = mDis;
                                                    EditMemberRule.BDDiscount = bDis;
                                                    entity.SaveChanges();
                                                    reloadMember();
                                                    reloadMemberList();
                                                    Clear();
                                                    isSuccess = true;
                                                    isCorrect = false;
                                                    break;
                                                }
                                                else
                                                {
                                                    isCorrect = true;
                                                }
                                            }
                                        }
                                        if (isCorrect == true)
                                        {
                                            MessageBox.Show("Please check your Amount!");
                                        }
                                    }
                                    else
                                    {
                                        if (maxRT == "Above" && Convert.ToInt32(rFrom) > maxRF)
                                        {
                                            MessageBox.Show("Please check your Amount.You have already entered amount \"" + maxRF.ToString() + " - " + maxRT + "\" in one of your member type!");
                                        }
                                        else
                                        {
                                            MessageBox.Show("Please check your Amount!");
                                        }
                                    }
                                }
                                else
                                {
                                    int AboveId = 0;
                                    if (maxRT == "Above")
                                    {
                                        AboveId = (from m in entity.MemberCardRules where m.RangeTo == "Above" select m.Id).SingleOrDefault();
                                    }
                                    if (AboveId != 0)
                                    {
                                        if (maxRT == "Above" && EditMemberRule.Id != AboveId)
                                        {
                                            MessageBox.Show("Please check your Amount.You have already entered amount \"" + maxRF.ToString() + " - " + maxRT + "\" in one of your member type!");
                                        }
                                        else if (maxRT == "Above" && EditMemberRule.Id == AboveId)
                                        {
                                            string maxRTo = (from m in entity.MemberCardRules where m.RangeTo != "Above" select m.RangeTo).Max();
                                            if (Convert.ToInt32(maxRTo) < Convert.ToInt32(rFrom))
                                            {
                                                EditMemberRule.MemberTypeId = mTypeid;
                                                EditMemberRule.RangeFrom = rFrom;
                                                EditMemberRule.RangeTo = rTo;
                                                EditMemberRule.MCDiscount = mDis;
                                                EditMemberRule.BDDiscount = bDis;
                                                entity.SaveChanges();
                                                reloadMember();
                                                reloadMemberList();
                                                Clear();
                                                isSuccess = true;
                                            }
                                            else
                                            {
                                                MessageBox.Show("Please check your Amount!");
                                            }
                                        }
                                        else
                                        {
                                            if (Convert.ToInt32(rFrom) > Convert.ToInt32(maxRT))
                                            {
                                                EditMemberRule.MemberTypeId = mTypeid;
                                                EditMemberRule.RangeFrom = rFrom;
                                                EditMemberRule.RangeTo = rTo;
                                                EditMemberRule.MCDiscount = mDis;
                                                EditMemberRule.BDDiscount = bDis;
                                                entity.SaveChanges();
                                                reloadMember();
                                                reloadMemberList();
                                                Clear();
                                                isSuccess = true;
                                            }
                                            else
                                            {
                                                MessageBox.Show("Please check your Amount!");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (isSuccess == true)
                        {
                            MessageBox.Show("Successfully Updated!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to edit member", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    bool notbackoffice = Utility.IsNotBackOffice();
                    if (notbackoffice)
                    {
                        Utility.Gpvisible(groupBox1, false);
                        btnAddMember.Visible = false;
                    }
                }
            }
        }
        public void reloadMember(string memb=null)
        {
            reloadMemberList();
            List<APP_Data.MemberType> MemList = new List<APP_Data.MemberType>();
            List<APP_Data.MemberType> MemberList = new List<APP_Data.MemberType>();
            int count = 0;
            APP_Data.MemberType memberObj1 = new APP_Data.MemberType();
            memberObj1.Id = 0;
            memberObj1.Name = "Select";
            MemberList.Add(memberObj1);
            MemList=entity.MemberTypes.Where(x=>x.IsDelete==false).ToList();
            foreach (MemberType newMem in MemList)
            {
                count = (from m in entity.MemberCardRules where m.MemberTypeId==newMem.Id select m).Count();
                if (count == 0)
                {
                    APP_Data.MemberType mType = (from m in entity.MemberTypes where m.Id == newMem.Id select m).FirstOrDefault();
                    MemberList.Add(mType);
                }
            }
            cboMemberRule.DataSource = null;
            cboMemberRule.DataSource = MemberList;
            cboMemberRule.DisplayMember = "Name";
            cboMemberRule.ValueMember = "Id";
            cboMemberRule.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboMemberRule.AutoCompleteSource = AutoCompleteSource.ListItems;
            if (memb == null)
            {
                cboMemberRule.Text = "Select";
            }
            else
            {
                cboMemberRule.Text = memb;
            }
        }
        
        private void MemberRule_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            bool notbackoffice = Utility.IsNotBackOffice();
            if (notbackoffice)
            {
                Utility.Gpvisible(groupBox1, isEdit);
                btnAddMember.Visible = false;
            }
            reloadMemberList();
            reloadMember();
            checkRadio();
        }

        private void rbtGreater_CheckedChanged(object sender, EventArgs e)
        {
            checkRadio();
        }

        private void rbtBetween_CheckedChanged(object sender, EventArgs e)
        {
            checkRadio();
        }

        private void checkRadio()
        {
            if (rbtGreater.Checked)
            {
                txtTo.Enabled = false;
                //txtTo.Text = "0";
                lblTo.Enabled = false;
            }
            if (rbtBetween.Checked)
            {
                txtTo.Enabled = true;
                if (txtTo.Text.Trim() == "Above") txtTo.Text = "0";
                lblTo.Enabled = true;
            }
        }

        private void MemberRule_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(cboMemberRule);
            tp.Hide(txtTo);
            tp.Hide(txtFrom);
            tp.Hide(txtMemberDis);
            tp.Hide(txtBirthdayDis);
        }

        public void reloadMemberList()
        {
            entity = new POSEntities();
            dgvMemberList.AutoGenerateColumns = false;
            dgvMemberList.DataSource = (from m in entity.MemberCardRules orderby m.RangeTo ascending select m).ToList();
        }

        private void dgvMemberList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentId;
            if (e.RowIndex >= 0)
            {
                //Delete
                if (e.ColumnIndex ==6)
                {
                    //Role Management
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.MemberRule.EditOrDelete || MemberShip.isAdmin)
                    {
                        DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (result.Equals(DialogResult.OK))
                        {
                            DataGridViewRow row = dgvMemberList.Rows[e.RowIndex];
                            currentId = Convert.ToInt32(row.Cells[0].Value);
                            int getID = 0;
                            getID = (from m in entity.MemberCardRules where m.Id == currentId select m.MemberTypeId).SingleOrDefault();
                            int countID = 0;
                            countID=(from m in entity.Customers where m.MemberTypeID == getID select m).Count();
                            if (countID < 1)
                            {
                                dgvMemberList.DataSource = "";
                                APP_Data.MemberCardRule mType = (from b in entity.MemberCardRules where b.Id == currentId select b).FirstOrDefault();
                                entity.MemberCardRules.Remove(mType);
                                entity.SaveChanges();
                                reloadMemberList();
                                reloadMember();
                                MessageBox.Show("Successfully Deleted!", "Delete Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Cannot Delete Member Type.It has already used in Customer!", "Delete Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to delete member card rule", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                //Edit
                else if (e.ColumnIndex == 5)
                {
                    bool notbackoffice = Utility.IsNotBackOffice();
                
                    //Role Management
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.MemberRule.EditOrDelete || MemberShip.isAdmin)
                    {
                        DataGridViewRow row = dgvMemberList.Rows[e.RowIndex];
                        currentId = Convert.ToInt32(row.Cells[0].Value);

                        APP_Data.MemberCardRule mType = (from b in entity.MemberCardRules where b.Id == currentId select b).FirstOrDefault();
                        APP_Data.MemberType meType = (from m in entity.MemberTypes where m.Id == mType.MemberTypeId select m).FirstOrDefault();
                        cboMemberRule.Text = meType.Name;
                        txtFrom.Text = mType.RangeFrom;
                        txtTo.Text = mType.RangeTo;
                        if (mType.RangeTo.ToString() == "Above")
                        {
                            rbtGreater.Checked = true;
                        }
                        else
                        {
                            rbtBetween.Checked = true;
                        }
                        txtMemberDis.Text = mType.MCDiscount.ToString();
                        txtBirthdayDis.Text = mType.BDDiscount.ToString();
                        this.Text = "Edit Member Card Rule";
                        isEdit = true;


                        if (notbackoffice)
                        {
                            Utility.Gpvisible(groupBox1, isEdit);
                            btnAddMember.Visible = false;
                        }
                        ruleID = mType.Id;
                        btnAdd.Image = Properties.Resources.update_small;
                        reloadMember(meType.Name);

                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to edit member card rule", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void dgvMemberList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvMemberList.Rows)
            {
                APP_Data.MemberCardRule cRule = (APP_Data.MemberCardRule)row.DataBoundItem;
                string amt = cRule.RangeFrom + " - " + cRule.RangeTo;
                row.Cells[0].Value = (object)cRule.Id;
                row.Cells[1].Value = (object)cRule.MemberType.Name;
                row.Cells[2].Value = (object)amt;
            }
        }
        private void Clear()
        {
            isEdit = false;
            ruleID = 0;
            cboMemberRule.Text = "Select";
            txtTo.Text = "0";
            txtFrom.Text = "0";
            txtMemberDis.Text = "0";
            txtBirthdayDis.Text = "0";
            rbtBetween.Checked = true;
            this.Text = "Add New Member Card Rule";
            btnAdd.Image = Properties.Resources.add_small;
            bool notbackoffice = Utility.IsNotBackOffice();
            if (notbackoffice)
            {
                Utility.Gpvisible(groupBox1, false);
            }
        }

        private void txtFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtMemberDis_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtBirthdayDis_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtTo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (rbtBetween.Checked)
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void cboMemberRule_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(cboMemberRule);
        }

        private void txtFrom_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(txtFrom);
        }

        private void txtTo_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(txtTo);
        }

        private void txtMemberDis_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(txtMemberDis);
        }

        private void txtBirthdayDis_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(txtBirthdayDis);
        }
    }
}
