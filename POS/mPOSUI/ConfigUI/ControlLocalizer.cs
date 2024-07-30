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
using System.IO;

namespace POS
{
    public partial class frmlocalize : Form
    {
        POSEntities db = new POSEntities();
        public frmlocalize()
        {
            InitializeComponent();
        }
        class FormControlClass
        {
            public int No { get; set; }
            public int Id { get; set; }
            public int FormId { get; set; }
            public string Name { get; set; }
            public string ControlName { get; set; }
            public string Eng { get; set; }
            public string ZawGyi { get; set; }
            public string MM3 { get; set; }
            public string Other1 { get; set; }
            public string Other2 { get; set; }
            public string Type { get; set; }
            public bool AllowToLoad { get; set; }
        }
        private void ControlLocalizer_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            this.dgvctlLocalize.CellValueChanged -= new DataGridViewCellEventHandler(this.dgvctlLocalize_CellValueChanged);
            List<pjForm> formCboSo = new List<pjForm>();
            pjForm pjStarter = new pjForm();
            pjStarter.Id = 0;
            pjStarter.TextEng = "All";
            List<pjForm> formsouce = db.pjForms.Where(p => p.AllowToLoad == true).ToList();
            formCboSo.Add(pjStarter);
            formCboSo.AddRange(formsouce);

            cboForm.ValueMember = "Id";
            cboForm.DisplayMember = "TextEng";
            this.cboForm.SelectedIndexChanged -= new EventHandler(this.cboForm_SelectedIndexChanged);
            cboForm.DataSource = formCboSo;
            this.cboForm.SelectedIndexChanged += new EventHandler(this.cboForm_SelectedIndexChanged);

            List<string> ctlTypelist = new List<string>();
            string single = "All";

            List<string> ctlsource = db.pjForms_Localization.Select(a => a.Type).Distinct().ToList();
            ctlTypelist.Add(single);
            ctlTypelist.AddRange(ctlsource);
            //cboForm.ValueMember = "Type";
            cboForm.DisplayMember = "Type";

            this.cboControl.SelectedIndexChanged -= new EventHandler(this.cboControl_SelectedIndexChanged);
            this.cboControl.SelectedValueChanged -= new EventHandler(this.cboForm_SelectedValueChanged);
            cboControl.DataSource = ctlTypelist;
            this.cboControl.SelectedIndexChanged += new EventHandler(this.cboControl_SelectedIndexChanged);
            this.cboControl.SelectedValueChanged += new EventHandler(this.cboForm_SelectedValueChanged);
            LoadData();
            this.dgvctlLocalize.CellValueChanged += new DataGridViewCellEventHandler(this.dgvctlLocalize_CellValueChanged);

        }

        private void dgvctlLocalize_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void cboForm_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cboControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        void LoadData()
        {
            int form = Convert.ToInt16(cboForm.SelectedValue);
            string ctltype1 = cboControl.Text;
            string ctlType = cboControl.SelectedItem!= null ? cboControl.SelectedItem.ToString() : "All";
            int no = 1;
            var data = db.pjForms_Localization
                .Where(l => l.AllowToLoad == true && ((form == 0 && 1 == 1) || (form > 0 && l.FormId == form)) &&
                ((ctlType == "All" && 1 == 1) || (ctlType != "All" && l.Type == ctlType)))
                .Select(a => new FormControlClass
                {
                    Id = a.Id,
                    FormId = a.FormId,
                    Name = a.pjForm.Name,
                    ControlName = a.ControlName,
                    Eng = a.Eng,
                    ZawGyi = a.ZawGyi,
                    MM3 = a.MM3,
                    Other1 = a.Other1,
                    Other2 = a.Other2,
                    Type = a.Type,
                    AllowToLoad = a.AllowToLoad
                }).ToList();
            data.ForEach(a => a.No = no++);
           
            dgvctlLocalize.DataSource = data.ToList();
        }

        private void dgvctlLocalize_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex > -1)
            {
                DataGridViewRow row = dgvctlLocalize.Rows[e.RowIndex];
                int id = (int)row.Cells["Id"].Value;
                pjForms_Localization rowtoEdit = db.pjForms_Localization.Find(id);
                rowtoEdit.ZawGyi = row.Cells["ZawGyi"].Value==null?"":row.Cells["ZawGyi"].Value.ToString();
                rowtoEdit.MM3 = row.Cells["MM3"].Value == null ? "" : row.Cells["MM3"].Value.ToString(); ;
                rowtoEdit.Other1 = row.Cells["Other1"].Value == null ? "" : row.Cells["Other1"].Value.ToString(); ;
                rowtoEdit.Other2 = row.Cells["Other2"].Value == null ? "" : row.Cells["Other2"].Value.ToString(); ; 
                db.Entry(rowtoEdit).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        private void dgvctlLocalize_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
           
        }

        private void cboForm_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = "csv";
            sfd.Filter = "Data Files (*.csv)|*.csv";
            sfd.FileName = "mPOSLanguage.csv";
            sfd.RestoreDirectory = true;
            sfd.InitialDirectory = @"D://";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
               
                StreamWriter sw = new StreamWriter(sfd.OpenFile());
                using (POSEntities db = new POSEntities())
                {
                    string[] removeChar = new string[] { "0", "", "-" };
                    List<string> toPrintOut = new List<string>();
                    var pro = db.pjForms_Localization.Where(a =>!removeChar.Contains(a.Eng) && a.AllowToLoad==true && a.pjForm.AllowToLoad==true).ToList().Select
                                (p => new
                                {
                                    p.pjForm.Name,
                                    p.ControlName,
                                    p.Eng,
                                    p.ZawGyi,
                                    p.MM3,
                                    p.Other1,
                                    p.Other2
                                    
                                }).ToList();

                    sw.WriteLine(string.Join(",",
                    new object[] { "FormName", "ControlName", "Eng", "ZawGyi", "MM3", "Other1", "Other2" }));
                    foreach (var p in pro)
                    {
                        sw.WriteLine(string.Join(",", new object[]{

                                    p.Name,
                                    p.ControlName,
                                    p.Eng,
                                    p.ZawGyi,
                                    p.MM3,
                                    p.Other1,
                                    p.Other2}));
                    }
                    sw.Dispose();
                    sw.Close();
                    MessageBox.Show("Successfully saved.");
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {   
            int cou=0;
            List<string> list = new List<string>();
            List<pjForms_Localization> controllist = new List<pjForms_Localization>();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = "csv";
            ofd.Filter = "Data Files (*.csv)|*.csv";
            ofd.RestoreDirectory = true;
            ofd.InitialDirectory = @"D://";
            try
            {
                
               
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                   
                    StreamReader sr = new StreamReader(ofd.OpenFile());
                    while (!sr.EndOfStream)
                    {
                        var singleLine = sr.ReadLine();
                        if (singleLine != string.Empty)
                        {
                            list.Add(singleLine);
                        }
                    }
                    sr.Close();
                    sr.Dispose();

                    if (!loading.IsBusy)
                    {
                        loading.RunWorkerAsync(new Object[]{list,controllist});
                        pcLoading.Visible = true;
                        lbllanguage.Visible = true;
                        ControlState(false);
                    }
                }
            }
            catch (IOException x)
            {
                MessageBox.Show(x.Message, "mPOS");
            }
            catch (DBConcurrencyException dbe)
            {
                MessageBox.Show(dbe.Message, "mPOS");
            }
        }

        private void loading_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] arrObj = e.Argument as object[];
            if (arrObj!=null)
            {
                List<string> list = new List<string>();
                List<pjForms_Localization> controllist = new List<pjForms_Localization>();
                for (int i = 0; i < arrObj.Count(); i++)
                {
                    if (i==0)
                    {
                        list = arrObj[0] as List<string>;
                    }
                    else if (i==1)
                    {
                        controllist=arrObj[1] as List<pjForms_Localization>;
                    }
                }
                if (list.Count > 0)
                {

                    //pictureBox1.Visible = true;
                    ////pictureBox1.Refresh();
                    //lblImporting.Visible = true;
                    //lblImporting.Refresh();
                    //pcLoading.Refresh();
                    var data = list.Skip(1).ToList();
                    for (int i = 0, n = 1; i < data.Count(); i++, n++)
                    {
                        var sing = data[i].Split(',');
                        if (sing != null || sing[0] != "")
                        {
                            pjForms_Localization p = new pjForms_Localization();
                            string fromname = sing[0].ToString();
                            p.FormId = db.pjForms.Where(f => f.Name == fromname).FirstOrDefault().Id;
                            p.ControlName = sing[1] ?? "";
                            p.Eng = sing[2] ?? "";
                            p.ZawGyi = sing[3] ?? "";
                            p.MM3 = sing[4] ?? "";
                            p.Other1 = sing[5] ?? "";
                            p.Other2 = sing[6] ?? "";
                            controllist.Add(p);
                        }
                    }
                }
                var incomingList = controllist.Select(a => new { FormId = a.FormId, ControlName = a.ControlName, Eng = a.Eng, ZawGyi = a.ZawGyi, MM3 = a.MM3, Other1 = a.Other1, Other2 = a.Other2 }).ToList();
                var existingList = db.pjForms_Localization.Select(a => new { FormId = a.FormId, ControlName = a.ControlName, Eng = a.Eng, ZawGyi = a.ZawGyi, MM3 = a.MM3, Other1 = a.Other1, Other2 = a.Other2 }).ToList();
                db = new POSEntities();
                int cou = 0;
                foreach (var ct in incomingList)
                {
                    pjForms_Localization ctrlTo = db.pjForms_Localization.Where(a => a.FormId == ct.FormId && a.ControlName == ct.ControlName).FirstOrDefault();

                    if (ctrlTo != null)
                    {
                        ctrlTo.Eng = ct.Eng;
                        ctrlTo.ZawGyi = ct.ZawGyi;
                        ctrlTo.MM3 = ct.MM3;
                        ctrlTo.Other1 = ct.Other1;
                        ctrlTo.Other2 = ct.Other2;
                        db.Entry(ctrlTo).State = EntityState.Modified;
                        cou += db.SaveChanges();
                    }
                }
                if (cou>0)
                {
                    e.Result = true;
                }
            }
          
        }

        private void loading_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((bool)e.Result==true)
            {
                LoadData();
                pcLoading.Visible = false;
                lbllanguage.Visible = false;
                ControlState(true);
            }
        }

        void ControlState(bool state)
        {
            cboControl.Enabled = cboForm.Enabled = gbLanguage.Enabled = dgvctlLocalize.Enabled = state;
        }
    }
}
