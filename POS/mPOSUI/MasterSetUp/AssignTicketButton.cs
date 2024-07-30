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
    public partial class AssignTicketButton : Form
    {
        POSEntities db = new POSEntities();
        public AssignTicketButton()
        {
            InitializeComponent();
        }

        private void AssignTicketButton_Load(object sender, EventArgs e)
        {
            this.TopMost = SettingController.TopMost;
            if (db.TicketButtonAssigns.Count() == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    TicketButtonAssign tba = new TicketButtonAssign();
                    tba.ButtonName = "btnAdd" + (i + 1).ToString();
                    tba.ButtonText = "+";
                    
                    db.TicketButtonAssigns.Add(tba);
                    db.SaveChanges();
                    TicketButtonAssign tba1 = new TicketButtonAssign();
                    tba1.ButtonName = "btnAdd" + (i + 1).ToString() + "Minus";
                    tba1.ButtonText = "-";
                   
                    db.TicketButtonAssigns.Add(tba1);
                    db.SaveChanges();
                    if (tba.ButtonName=="btnAdd1")
                    {
                        TicketButtonAssign lblLocalAdult = new TicketButtonAssign();
                        lblLocalAdult.ButtonName = "lblLocalAdult";
                        lblLocalAdult.ButtonText = "0";

                        db.TicketButtonAssigns.Add(lblLocalAdult);
                        db.SaveChanges();
                    }
                    if (tba.ButtonName=="btnAdd2")
                    {
                        TicketButtonAssign lblLocalChild = new TicketButtonAssign();
                        lblLocalChild.ButtonName = "lblLocalChild";
                        lblLocalChild.ButtonText = "0";
                        db.TicketButtonAssigns.Add(lblLocalChild);
                        db.SaveChanges();
                    }
                    if (tba.ButtonName == "btnAdd3")
                    {
                        TicketButtonAssign lblForeignAdult = new TicketButtonAssign();
                        lblForeignAdult.ButtonName = "lblForeignAdult";
                        lblForeignAdult.ButtonText = "0";
                        db.TicketButtonAssigns.Add(lblForeignAdult);
                        db.SaveChanges();
                    }
                    if (tba.ButtonName == "btnAdd4")
                    {
                        TicketButtonAssign lblForeignChild = new TicketButtonAssign();
                        lblForeignChild.ButtonName = "lblForeignChild";
                        lblForeignChild.ButtonText = "0";
                        db.TicketButtonAssigns.Add(lblForeignChild);
                        db.SaveChanges();
                    }
                }
            }
            comboList();
            GridLoad();
        }

        private void comboList()
        {
            List<Product> plist = new List<Product>();
            Product p = new Product();
            p.Id = 0;
            p.Name = "Select";
            plist.Add(p);

            List<Product> prolist = db.Products.Where(a=>a.ProductCategory.Name=="Ticket").ToList();
            plist.AddRange(prolist);

            cboProduct.ValueMember = "Id";
            cboProduct.DisplayMember = "Name";
            cboProduct.DataSource = plist;
        }
        void GridLoad()
        {

            db = new POSEntities();
            var data = db.TicketButtonAssigns.AsEnumerable().Select
                (a=> new {
                    Id=a.Id,
                    ButtonName=a.ButtonName,
                    ButtonText=a.ButtonText,
                    ProductName = a.Product == null ? "" : a.Product.Name,
                    Defined=a.Defined,
                    Defined1=a.Defined1}).ToList();
            dgvButtons.DataSource = data.ToList();

        }

        private void dgvButtons_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
           // dgvButtons.CommitEdit(new DataGridViewDataErrorContexts());
            //foreach (DataGridViewRow row in dgvButtons.Rows)
            //{
               
            //}
        }

        private void dgvButtons_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>-1)
            {
                var a = dgvButtons.Columns["Edit"].Index;
                if (e.ColumnIndex==1)
                {
                    DataGridViewRow dr = dgvButtons.Rows[e.RowIndex];

                    txtbutton.Tag = (int)dr.Cells["Id"].Value;
                    txtbutton.Text = dr.Cells["ButtonName"].Value.ToString();
                    var id =db.TicketButtonAssigns.Find((int)dr.Cells["Id"].Value).Assignproductid;
                    cboProduct.SelectedValue = id == null ? 0 : id;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtbutton.Text=="")
            {
                MessageBox.Show(this, "Please select a button to update.", "mpos");
                return;
            }
            if (Convert.ToInt16(cboProduct.SelectedValue)!=0)
            {
                int id = (int)txtbutton.Tag;
                var ptoe = db.TicketButtonAssigns.Find(id);
                ptoe.Assignproductid = Convert.ToInt16(cboProduct.SelectedValue);
                db.Entry(ptoe);
                int row =db.SaveChanges();
                if (row>0)
                {
                    Clear();
                }
                
            }
            else
            {

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            txtbutton.Text = "";
            txtbutton.Tag = "";
            cboProduct.SelectedValue = 0;
            GridLoad();
        }

        private void AssignTicketButton_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((Application.OpenForms["Sales"] as Sales) != null)
            {
                Sales form = (Application.OpenForms["Sales"] as Sales);
                form.Close();

                Sales form2 = new Sales();

                form2.WindowState = FormWindowState.Maximized;
                MDIParent parent = (Application.OpenForms["MDIParent"] as MDIParent);
                form2.MdiParent = parent;
                form2.Show();
            }
            else
            {
               
            }
        }
    }

}
