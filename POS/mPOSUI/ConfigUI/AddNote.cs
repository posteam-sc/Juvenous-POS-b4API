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
    public partial class AddNote : Form
    {
        private POSEntities entity = new POSEntities();
        public string InstructionNoteForProduct { get; set; }
        public string status;
        public string editnote;
        public string tranid;

        public AddNote()
        {
            InitializeComponent();
        }

        private void AddNote_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            if (status == "ADD")
            {
                button3.Visible = false;
                txtnote.Text = InstructionNoteForProduct;
                button1.Visible = true;
                button2.Visible = false;
            }
            else if (status == "EDIT")
            {
                this.Text = "Edit Note  For This Transaction";
                button3.Visible = true;
                txtnote.Text = editnote;
                button1.Visible = false;
                button2.Visible = true;
            }
            else if (status == "Display")
            {
                this.Text = "Note For This Transaction";
                txtnote.Text = editnote;
                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (tranid != null)
            {
                Transaction t = entity.Transactions.Where(x => x.Id == tranid).FirstOrDefault();
                t.Note = txtnote.Text.ToString();
                entity.Entry(t).State = EntityState.Modified;
                entity.SaveChanges();
                this.Dispose();
            }
            else
            {
                if (System.Windows.Forms.Application.OpenForms["CustomerDetail"] != null)
                {
                    CustomerDetail newForm = (CustomerDetail)System.Windows.Forms.Application.OpenForms["CustomerDetail"];

                    newForm.note = txtnote.Text;

                    this.Dispose();
                }

                if (System.Windows.Forms.Application.OpenForms["Sales"] != null)
                {
                    Sales newForm = (Sales)System.Windows.Forms.Application.OpenForms["Sales"];

                    newForm.note = txtnote.Text;

                    this.Dispose();
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (tranid != null)
            {
                Transaction t = entity.Transactions.Where(x => x.Id == tranid).FirstOrDefault();
                t.Note = txtnote.Text.ToString();
                entity.Entry(t).State = EntityState.Modified;
                entity.SaveChanges();
                this.Dispose();
            }
            else
            {
                if (System.Windows.Forms.Application.OpenForms["CustomerDetail"] != null)
                {
                    CustomerDetail newForm = (CustomerDetail)System.Windows.Forms.Application.OpenForms["CustomerDetail"];

                    newForm.note = txtnote.Text;

                    this.Dispose();
                }

                if (System.Windows.Forms.Application.OpenForms["Sales"] != null)
                {
                    Sales newForm = (Sales)System.Windows.Forms.Application.OpenForms["Sales"];

                    newForm.note = txtnote.Text;

                    this.Dispose();
                }

            }
        }
    }
}
