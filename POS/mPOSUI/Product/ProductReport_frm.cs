using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using POS.APP_Data;
using System.IO;

namespace POS
{
    public partial class ProductReport_frm : Form
    {
        public ProductReport_frm()
        {
            InitializeComponent();
            CenterToScreen();
        }


        #region
        POSEntities entity = new POSEntities();
        List<ProductReportController> pdlist = new List<ProductReportController>();
        int totalQty = 0;
        int totalPurPrice = 0;
        int MainCategoryId = 0, SubCategoryId = 0, BrandId = 0;
        string skuCode, BrandName, SegName, SubSegName = " ";
        bool _start = false;
        #endregion

        private void ProductReprotFrm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            if (_start == true)
            {
                List<APP_Data.Product> plist = new List<Product>();
                totalQty = 0; totalPurPrice = 0;
                MainCategoryId = 0; SubCategoryId = 0; BrandId = 0;
                SegName = " "; SubSegName = " "; BrandName = " "; skuCode = " ";
                pdlist.Clear();
                lblCurrentDate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
                string SkuCode = "";

                if (cboMainCategory.SelectedIndex > 0)
                {
                    MainCategoryId = Convert.ToInt32(cboMainCategory.SelectedValue);
                }
                if (cboSubCategory.SelectedIndex > 0)
                {
                    SubCategoryId = Convert.ToInt32(cboSubCategory.SelectedValue);
                }
                if (cboBrand.SelectedIndex > 0)
                {
                    BrandId = Convert.ToInt32(cboBrand.SelectedValue);
                }
                if (txtSKUCode.Text.Trim() != "")
                {
                    SkuCode = txtSKUCode.Text.Trim();
                }
                    System.Data.Objects.ObjectResult<GetProductReport_Result> _allData = null;
              
                    _allData = entity.GetProductReport(MainCategoryId, SubCategoryId, BrandId, SkuCode);
                    Collect_ReportData(_allData);
            }
        }

        private void Collect_ReportData( System.Data.Objects.ObjectResult<GetProductReport_Result> _allData)
        {
            ProductReportController pdCon = new ProductReportController();

            List<ProductReportController> _reportData = new List<ProductReportController>();
            
            if (_allData != null)
            {

                try
                {
                    _reportData.AddRange(_allData.Select(_product =>
                            new ProductReportController
                            {
                                Id = Convert.ToInt32(_product.Id.ToString()),
                                Qty = Convert.ToInt32(_product.Qty),
                                SKUCode = _product.ProductCode.ToString(),
                                ProductName = _product.Name.ToString(),
                                BrandName = _product.Brand_Name.ToString(),
                                Segment = _product.Segment_Name.ToString(),
                                PurchasePrice = Convert.ToInt32(_product.PurchasePrice.ToString()),
                                SubSegment = _product.SubSegment_Name == null ? "" : _product.SubSegment_Name.ToString(),
                                TotalQty = Convert.ToInt32(_product.Qty.ToString()),
                                isConsignment = Convert.ToBoolean(_product.IsConsignment),
                                PhotoPath = _product.PhotoPath == null ? "" : _product.PhotoPath
                            }
                                  )
                          );
                }
                catch
                {
                }

            }

            Get_PurPrice_ReportData(_reportData);
            ShowReportViewer();

        }

        private void Get_PurPrice_ReportData(List<ProductReportController> _reportData)
        {
            foreach (var pr in _reportData)
            {

                List<APP_Data.PurchaseDetail> _compareData = (from p in entity.PurchaseDetails
                                                              join mp in entity.MainPurchases on p.MainPurchaseId equals mp.Id
                                                              where p.ProductId == pr.Id && p.IsDeleted == false && mp.IsCompletedInvoice==true
                                                              orderby p.Date descending select p).ToList();

                if (_compareData.Count > 0)
                {
                    for (int i = 0; i <= _compareData.Count - 1; i++)
                    {
                        if (pr.Qty < 0)
                        {
                            pr.PurchasePrice = 0;
                            Report_Data(pr);
                            break;
                        }
                        if (pr.Qty < _compareData[i].Qty)
                        {
                            pr.PurchasePrice = Convert.ToInt32(_compareData[i].UnitPrice);

                            Report_Data(pr);
                            pr.Qty = 0;
                            break;
                        }
                        else if (pr.Qty == _compareData[i].Qty)
                        {
                            pr.PurchasePrice = Convert.ToInt32(_compareData[i].UnitPrice);
                            Report_Data(pr);
                            pr.Qty = 0;
                            break;

                        }
                        else if (pr.Qty > _compareData[i].Qty)
                        {
                            if (i == _compareData.Count - 1)
                            {
                                pr.PurchasePrice = Convert.ToInt32(_compareData[i].UnitPrice);
                                Report_Data(pr);
                                pr.Qty = 0;
                            }
                            else
                            {
                                var Qty = pr.Qty;
                                pr.PurchasePrice = Convert.ToInt32(_compareData[i].UnitPrice);
                                pr.Qty = Convert.ToInt32(_compareData[i].Qty);
                                Report_Data(pr);
                                pr.Qty = Convert.ToInt32(Qty - _compareData[i].Qty);
                            }
                        }
                    }
                }
                else
                {
                    pr.PurchasePrice = 0;
                    Report_Data(pr);
                  //  break;
                }

            }
        }



        private void Report_Data(ProductReportController pr)
        {
            
            ProductReportController pdCon = new ProductReportController();
            pdCon.SKUCode = pr.SKUCode;
            pdCon.ProductName = pr.ProductName;
            pdCon.BrandName = pr.BrandName;
            pdCon.Segment = pr.Segment;
            pdCon.PurchasePrice = Convert.ToInt32(pr.PurchasePrice);
          
            pdCon.SubSegment = pr.SubSegment;
          
            pdCon.TotalQty = Convert.ToInt32(pr.Qty);
          
            pdCon.isConsignment =Convert.ToBoolean(pr.isConsignment);
            pdCon.PhotoPath = pr.PhotoPath;
      
            pdlist.Add(pdCon);
        }


        private Image bytearraytoimage(byte[] b)
        {
            MemoryStream ms = new MemoryStream(b);
            Image img = Image.FromStream(ms);
            return img;
        }


        private void ShowReportViewer()
        {

            if (cboMainCategory.SelectedIndex > 0)
            {
                APP_Data.ProductCategory pdC = (from p in entity.ProductCategories where p.Id == MainCategoryId select p).FirstOrDefault();

                if (cboBrand.SelectedIndex > 1)
                {
                    SegName = "," + pdC.Name.ToString();
                }
                else
                {
                    SegName = pdC.Name.ToString();

                }
            }

            if (cboSubCategory.SelectedIndex > 0)
            {
                if (SubCategoryId == 0)
                {
                    SubSegName = "";
                }
                else
                {
                    if (cboMainCategory.SelectedIndex > 0)
                    {
                        APP_Data.ProductSubCategory pdCsub = (from p in entity.ProductSubCategories where p.Id == SubCategoryId select p).FirstOrDefault();
                        SubSegName = "," + pdCsub.Name;
                    }
                    else
                    {
                        APP_Data.ProductSubCategory pdCsub = (from p in entity.ProductSubCategories where p.Id == SubCategoryId select p).FirstOrDefault();
                        SubSegName = pdCsub.Name;
                    }
                }
            }

            if (cboBrand.SelectedIndex > 1)
            {
                APP_Data.Brand bd = (from b in entity.Brands where b.Id == BrandId select b).FirstOrDefault();
                BrandName = bd.Name;
            }
            if (txtSKUCode.Text.Trim() != "")
            {
                skuCode = txtSKUCode.Text;
            }

            dsReportTemp dsReport = new dsReportTemp();
            dsReportTemp.ProductReportDataTable dtPdReport = (dsReportTemp.ProductReportDataTable)dsReport.Tables["ProductReport"];
            foreach (ProductReportController pdCon in pdlist)
            {
                if (rbtConsignment.Checked)
                {
                    if (pdCon.isConsignment)
                    {
                        dsReportTemp.ProductReportRow newRow = dtPdReport.NewProductReportRow();
                        newRow.Name = pdCon.ProductName;
                        newRow.ProductCode = pdCon.SKUCode;
                        newRow.BrandName = pdCon.BrandName;
                        newRow.SegmentName = pdCon.Segment;
                        newRow.SubSegmentName = pdCon.SubSegment;
                        newRow.Qty = Convert.ToInt32(pdCon.TotalQty);
                        if (pdCon.PhotoPath != null && pdCon.PhotoPath != "")
                        {
                            string FileNmae = pdCon.PhotoPath.Substring(9);
                            string imagepath = Application.StartupPath + "\\Images\\" + FileNmae;
                            newRow.PhotoPath = imagepath;
                        }
                        else
                        {
                            string FileNmae = "default_product.png";
                            string imagepath = Application.StartupPath + "\\Images\\" + FileNmae;
                            newRow.PhotoPath = imagepath;
                        }

                        totalQty += Convert.ToInt32(pdCon.Qty);
                        //totalPurPrice += Convert.ToInt32(pdCon.PurchasePrice);
                        //newRow.PurchasePrice = pdCon.PurchasePrice.ToString();
                        dtPdReport.AddProductReportRow(newRow);
                    }
                }
                else
                {
                    if (!pdCon.isConsignment)
                    {
                        dsReportTemp.ProductReportRow newRow = dtPdReport.NewProductReportRow();
                        newRow.Name = pdCon.ProductName;
                        newRow.ProductCode = pdCon.SKUCode;
                        newRow.BrandName = pdCon.BrandName;
                        newRow.SegmentName = pdCon.Segment;
                        newRow.SubSegmentName = pdCon.SubSegment;
                        newRow.Qty = Convert.ToInt32(pdCon.TotalQty);
                        newRow.PurchasePrice = Convert.ToDouble(pdCon.PurchasePrice);
                        if (pdCon.PhotoPath != null && pdCon.PhotoPath != "")
                        {
                            string FileNmae = pdCon.PhotoPath.Substring(9);

                            string imagepath = Application.StartupPath + "\\Images\\" + FileNmae;
                            newRow.PhotoPath = imagepath;
                        }
                        else
                        {
                            string FileNmae = "default_product.png";
                            string imagepath = Application.StartupPath + "\\Images\\" + FileNmae;
                            newRow.PhotoPath = imagepath;
                        }
                     
                        totalQty += Convert.ToInt32(pdCon.Qty);
                        totalPurPrice += Convert.ToInt32(pdCon.PurchasePrice);
                        dtPdReport.AddProductReportRow(newRow);
                    }
                }
            }

            if (rbtConsignment.Checked)
            {
                
                reportViewer1.LocalReport.EnableExternalImages = true;
                ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["ProductReport"]);
                string reportPath = Application.StartupPath + "\\Reports\\ProductReport1.rdlc";

                reportViewer1.LocalReport.ReportPath = reportPath;
                
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);

                ReportParameter TotalQty = new ReportParameter("TotalQty", totalQty.ToString());
                reportViewer1.LocalReport.SetParameters(TotalQty);

            
                ReportParameter Header = new ReportParameter("Header", "Product report for " + BrandName + SegName + SubSegName + skuCode + " at " + DateTime.Now.Date.ToString("dd/MM/yyyy"));
                reportViewer1.LocalReport.SetParameters(Header);
                reportViewer1.RefreshReport();
            }
            else
            {
                reportViewer1.LocalReport.EnableExternalImages = true;
                ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["ProductReport"]);
                string reportPath = Application.StartupPath + "\\Reports\\ProductReport.rdlc";

                reportViewer1.LocalReport.ReportPath = reportPath;
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);

                ReportParameter TotalQty = new ReportParameter("TotalQty", totalQty.ToString());
                reportViewer1.LocalReport.SetParameters(TotalQty);

                ReportParameter TotalPurPrice = new ReportParameter("TotalPurPrice", totalPurPrice.ToString());
                reportViewer1.LocalReport.SetParameters(TotalPurPrice);



                ReportParameter Header = new ReportParameter("Header", "Product report for " + BrandName + SegName + SubSegName + skuCode + " at " + DateTime.Now.Date.ToString("dd/MM/yyyy"));
                reportViewer1.LocalReport.SetParameters(Header);
                reportViewer1.RefreshReport();
            }
        }

        private void cboMainCategory_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboMainCategory.SelectedIndex != 0)
            {
                int productCategoryId = Int32.Parse(cboMainCategory.SelectedValue.ToString());
                List<APP_Data.ProductSubCategory> pSubCatList = new List<APP_Data.ProductSubCategory>();
                APP_Data.ProductSubCategory SubCategoryObj1 = new APP_Data.ProductSubCategory();
                SubCategoryObj1.Id = -1;
                SubCategoryObj1.Name = "Select";
                //APP_Data.ProductSubCategory SubCategoryObj2 = new APP_Data.ProductSubCategory();
                //SubCategoryObj2.Id = 0;
                //SubCategoryObj2.Name = "None";
                pSubCatList.Add(SubCategoryObj1);
               // pSubCatList.Add(SubCategoryObj2);
                pSubCatList.AddRange((from c in entity.ProductSubCategories where c.ProductCategoryId == productCategoryId select c).ToList());
                cboSubCategory.DataSource = pSubCatList;
                cboSubCategory.DisplayMember = "Name";
                cboSubCategory.ValueMember = "Id";
                cboSubCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cboSubCategory.AutoCompleteSource = AutoCompleteSource.ListItems;
                cboSubCategory.Enabled = true;

            }
            //maythu
            LoadData();
        }

        private void cboSubCategory_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void cboSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cboBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
            txtSKUCode.Text = "";
        }

        private void cboMainCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            List<APP_Data.Brand> BrandList = new List<APP_Data.Brand>();
            APP_Data.Brand brandObj1 = new APP_Data.Brand();
            brandObj1.Id = 0;
            brandObj1.Name = "Select";
            //APP_Data.Brand brandObj2 = new APP_Data.Brand();
            //brandObj2.Id = 1;
            //brandObj2.Name = "None";
            BrandList.Add(brandObj1);
            //BrandList.Add(brandObj2);
            BrandList.AddRange((from bList in entity.Brands select bList).ToList());
            cboBrand.DataSource = BrandList;
            cboBrand.DisplayMember = "Name";
            cboBrand.ValueMember = "Id";
            cboBrand.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboBrand.AutoCompleteSource = AutoCompleteSource.ListItems;

            List<APP_Data.ProductSubCategory> pSubCatList = new List<APP_Data.ProductSubCategory>();
            APP_Data.ProductSubCategory SubCategoryObj1 = new APP_Data.ProductSubCategory();
            SubCategoryObj1.Id = -1;
            SubCategoryObj1.Name = "Select";
            //APP_Data.ProductSubCategory SubCategory2 = new APP_Data.ProductSubCategory();
            //SubCategory2.Id = 0;
            //SubCategory2.Name = "None";
            pSubCatList.Add(SubCategoryObj1);
          //  pSubCatList.Add(SubCategory2);
            //pSubCatList.AddRange((from c in entity.ProductSubCategories where c.ProductCategoryId == Convert.ToInt32(cboMainCategory.SelectedValue) select c).ToList());
            cboSubCategory.DataSource = pSubCatList;
            cboSubCategory.DisplayMember = "Name";
            cboSubCategory.ValueMember = "Id";
            cboSubCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboSubCategory.AutoCompleteSource = AutoCompleteSource.ListItems;

            List<APP_Data.ProductCategory> pMainCatList = new List<APP_Data.ProductCategory>();
            APP_Data.ProductCategory MainCategoryObj1 = new APP_Data.ProductCategory();
            MainCategoryObj1.Id = 0;
            MainCategoryObj1.Name = "Select";
            pMainCatList.Add(MainCategoryObj1);
            pMainCatList.AddRange((from MainCategory in entity.ProductCategories select MainCategory).ToList());
            cboMainCategory.DataSource = pMainCatList;
            cboMainCategory.DisplayMember = "Name";
            cboMainCategory.ValueMember = "Id";
            cboMainCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboMainCategory.AutoCompleteSource = AutoCompleteSource.ListItems;
            LoadData();
            txtSKUCode.Clear();
            //chkDiscontinous.Checked = false;



        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void txtSKUCode_Enter(object sender, EventArgs e)
        {
            LoadData();
        }

        private void txtSKUCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (txtSKUCode.Text != string.Empty)
                {
                    LoadData();
                }
            }
        }

        private void chkDiscontinous_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void ProductReport_frm_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            List<APP_Data.Brand> BrandList = new List<APP_Data.Brand>();
            APP_Data.Brand brandObj1 = new APP_Data.Brand();
            brandObj1.Id = 0;
            brandObj1.Name = "Select";
            //APP_Data.Brand brandObj2 = new APP_Data.Brand();
            //brandObj2.Id = 1;
            //brandObj2.Name = "None";
            BrandList.Add(brandObj1);
            //BrandList.Add(brandObj2);
            BrandList.AddRange((from bList in entity.Brands select bList).ToList());
            cboBrand.DataSource = BrandList;
            cboBrand.DisplayMember = "Name";
            cboBrand.ValueMember = "Id";
            cboBrand.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboBrand.AutoCompleteSource = AutoCompleteSource.ListItems;

            List<APP_Data.ProductSubCategory> pSubCatList = new List<APP_Data.ProductSubCategory>();
            APP_Data.ProductSubCategory SubCategoryObj1 = new APP_Data.ProductSubCategory();
            SubCategoryObj1.Id = -1;
            SubCategoryObj1.Name = "Select";
            //APP_Data.ProductSubCategory SubCategory2 = new APP_Data.ProductSubCategory();
            //SubCategory2.Id = 0;
            //SubCategory2.Name = "None";
            pSubCatList.Add(SubCategoryObj1);
          //  pSubCatList.Add(SubCategory2);
            //pSubCatList.AddRange((from c in entity.ProductSubCategories where c.ProductCategoryId == Convert.ToInt32(cboMainCategory.SelectedValue) select c).ToList());
            cboSubCategory.DataSource = pSubCatList;
            cboSubCategory.DisplayMember = "Name";
            cboSubCategory.ValueMember = "Id";
            cboSubCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboSubCategory.AutoCompleteSource = AutoCompleteSource.ListItems;

            List<APP_Data.ProductCategory> pMainCatList = new List<APP_Data.ProductCategory>();
            APP_Data.ProductCategory MainCategoryObj1 = new APP_Data.ProductCategory();
            MainCategoryObj1.Id = 0;
            MainCategoryObj1.Name = "Select";
            pMainCatList.Add(MainCategoryObj1);
            pMainCatList.AddRange((from MainCategory in entity.ProductCategories select MainCategory).ToList());
            cboMainCategory.DataSource = pMainCatList;
            cboMainCategory.DisplayMember = "Name";
            cboMainCategory.ValueMember = "Id";
            cboMainCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboMainCategory.AutoCompleteSource = AutoCompleteSource.ListItems;

            _start = true;
            LoadData();
        }

        private void rbtConsignment_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void rbtNonConsignment_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
