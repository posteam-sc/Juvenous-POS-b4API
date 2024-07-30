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
    public partial class AddTable : Form
    {
        POSEntities db = new POSEntities();
        bool isEdit = false;
        public AddTable()
        {
            InitializeComponent();
        }

        private void AddTable_Load(object sender, EventArgs e)
        {
            Clean();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtTable.Text!=null)
            {
                if (isEdit)
                {
                    int Id = (int)txtTable.Tag;
                    var tblObj = db.RestaurantTables.Find(Id);
                    tblObj.Number = txtTable.Text;
                    db.Entry(tblObj).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    RestaurantTable tblObj = new RestaurantTable();
                    tblObj.Id = db.RestaurantTables.ToList().Count() > 0 ? db.RestaurantTables.Max(a => a.Id) + 1 : 1;
                    tblObj.Number = txtTable.Text;
                    tblObj.Status = true;
                    db.RestaurantTables.Add(tblObj);
                    db.SaveChanges();
                }
               
            }

            Clean();
        }

        private void dgvTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>-1)
            {
                if (e.ColumnIndex==2)
                {
                    int Id = (int)dgvTable["Id", e.RowIndex].Value;
                    string celVal = dgvTable.Rows[e.RowIndex].Cells["Status"].Value.ToString();
                    bool status = celVal == "Free" ? false : true;
                    var editRow = db.RestaurantTables.Find(Id);
                    editRow.Status = status;
                    db.Entry(editRow).State = EntityState.Modified;
                    db.SaveChanges();
                    Clean();
                }
                else if (e.ColumnIndex==3)
                {
                    isEdit = true;
                    int Id = (int)dgvTable["Id", e.RowIndex].Value;
                    var editRow = db.RestaurantTables.Find(Id);
                    txtTable.Tag = editRow.Id;
                    txtTable.Text = editRow.Number;
                }
                else if (e.ColumnIndex==4)
                {
                    if (MessageBox.Show("Are you sure want to delete?","mPOS",MessageBoxButtons.OKCancel,MessageBoxIcon.Question)==DialogResult.OK)
                    {
                        int Id = (int)dgvTable["Id", e.RowIndex].Value;
                        var deleteRow = db.RestaurantTables.Find(Id);
                        db.RestaurantTables.Remove(deleteRow);
                        db.SaveChanges();
                        Clean();
                    }
                }
            }
        }

        void BindCombo()
        {
            List<RestaurantTable> tablelist = new List<RestaurantTable>();
            RestaurantTable productObj = new RestaurantTable();
            productObj.Id = 0;
            productObj.Number = "Select";
            tablelist.Add(productObj);
            tablelist.AddRange(db.RestaurantTables.Where(a=>a.Status==true).ToList());

            cboTable.DisplayMember = "Number";
            cboTable.ValueMember = "Id";
            cboTable.DataSource = tablelist;
            cboTable.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboTable.AutoCompleteSource = AutoCompleteSource.ListItems;
        }
        void Clean()
        {
            txtTable.Text = "";
            txtTable.Tag = "";
            isEdit = false;
            LoadData();
            BindCombo();
        }
        void LoadData(int Id=0)
        {
            dgvTable.AutoGenerateColumns = false;
            dgvTable.DataSource = db.RestaurantTables.AsEnumerable().Where(a=> (Id==0 && 1==1) || (Id>0 && a.Id==Id))
                .Select(a=>new { a.Id,a.Number,Status=a.Status?"Free":"Occupied"}).ToList();
        }

        private void dgvTable_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clean();
        }

        private void cboTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTable.SelectedIndex>-1)
            {
                LoadData((int)cboTable.SelectedValue);
            }
        }
    }
}
