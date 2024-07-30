using POS.APP_Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace POS
{
    public partial class csvExportImport : Form
    {
        POSEntities db = new POSEntities();
        List<_Product> fetched = new List<_Product>();
        List<string> list = new List<string>();
        List<_Product> importedList = new List<_Product>();
        List<Product> toSaveproduct = new List<Product>();
        List<Product> toUpdateproduct = new List<Product>();
        List<APP_Data.Unit> toSaveUnit = new List<APP_Data.Unit>();
        List<APP_Data.Tax> toSaveTax = new List<APP_Data.Tax>();
        List<APP_Data.Brand> toSavebrand = new List<APP_Data.Brand>();
        List<APP_Data.Brand> toupdatebrand = new List<APP_Data.Brand>();
        List<APP_Data.ProductCategory> toSavecategory = new List<APP_Data.ProductCategory>();
        List<APP_Data.ProductSubCategory> toSavesubcategory = new List<APP_Data.ProductSubCategory>();
        List<_Product> existed = null;
        List<_Product> newlist = null;
        public csvExportImport()
        {
            InitializeComponent();
        }
        private void csvExportImport_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
        }
        private class _Product
        {
            public string No { get; set; }
            public string Name { get; set; }
            public string ProductCode { get; set; }
            public string Barcode { get; set; }
            public string Price { get; set; }
            public string WholeSalePrice { get; set; }
            public string Qty { get; set; }
            public string Brand { get; set; }
            public string ProductCategory { get; set; }
            public string ProductSubCategory { get; set; }
            public string IsNotifyMinStock { get; set; }
            public string MinStockQty { get; set; }
            public string Size { get; set; }
            public string Unit { get; set; }
            public decimal? Tax { get; set; }
        }
        private void bntExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = "csv";
            sfd.Filter = "Data Files (*.csv)|*.csv";
            sfd.FileName = "TestExport.csv";
            sfd.RestoreDirectory = true;
            sfd.InitialDirectory = @"D://";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                lblstatus.Text = "Exporting...";
                lblstatus.Refresh();
                StreamWriter sw = new StreamWriter(sfd.OpenFile());
                using (POSEntities db = new POSEntities())
                {
                    List<string> toPrintOut = new List<string>();
                    var pro = db.Products.Where(a => a.IsConsignment == false && a.IsWrapper == false && (a.IsDiscontinue == false || a.IsDiscontinue == null)).ToList().Select
                                (p => new
                                {
                                    p.Id,
                                    p.Name,
                                    p.ProductCode,
                                    p.Barcode,
                                    p.Price,
                                    p.WholeSalePrice,
                                    p.Qty,
                                    Brand = p.Brand.Name,
                                    ProductCategory = p.ProductCategory != null ? p.ProductCategory.Name : "0",
                                    ProductSubCategory = p.ProductSubCategory != null ? p.ProductSubCategory.Name : "0",
                                    IsNotifyMinStock = p.IsNotifyMinStock == true ? "Y" : "N",
                                    p.MinStockQty,
                                    Size = p.Size == null || p.Size == "" ? "0" : p.Size,
                                    PurchasePrice = p.PurchasePrice == null ? "0" : p.PurchasePrice.ToString(),
                                    Unit = p.Unit.UnitName,
                                    Tax=(p.Tax.Name=="None") ? "0": p.Tax.Name
                                }).ToList();

                    if (pro.Count==0)
                    {
                        sw.WriteLine(string.Join(",",
                    new object[] { "Name", "ProductCode", "Barcode", "Price", "WholeSalePrice", "Qty", "Brand", "ProductCategory", "ProductSubCategory", "IsNotifyMinStock", "MinStockQty", "Size", "Unit", "Tax" }));
                        sw.WriteLine(string.Join(",",
                    new object[] { "Yellow Boots", "0or123456", "0or123456", "500000", "300000", "0", "TimberLand", "Boots", "0orTranditional", "YorN", "0or12", "0orSomething", "0orSomething", "0or102030" }));
                    }
                    else
                    {
                        sw.WriteLine(string.Join(",",
                    new object[] { "Name", "ProductCode", "Barcode", "Price", "WholeSalePrice", "Qty", "Brand", "ProductCategory", "ProductSubCategory", "IsNotifyMinStock", "MinStockQty", "Size", "Unit", "Tax" }));
                    }
                    foreach (var p in pro)
                    {
                        sw.WriteLine(string.Join(",", new object[]{
                                   
                                    p.Name,
                                    p.ProductCode,
                                    p.Barcode,
                                    p.Price,
                                    p.WholeSalePrice,
                                    p.Qty,
                                    p.Brand,
                                    p.ProductCategory,
                                    p.ProductSubCategory,
                                    p.IsNotifyMinStock,
                                    p.MinStockQty,
                                    p.Size,
                                    p.Unit,
                                    p.Tax}));
                    }
                    sw.Dispose();
                    sw.Close();
                    lblstatus.Text = "Export Done";
                    lblstatus.Refresh();
                    MessageBox.Show("Successfully saved.");
                }
            }
        }
        private List<_Product> FetchCSV(List<string> importeddata)
        {
            try
            {
                importedList.Clear();
                var data = importeddata.Skip(1).ToList();
                lblstatus.Text = "Reading CSV...";
                lblstatus.Refresh(); lblstatus.ForeColor = Color.Blue;
                for (int i = 0, n = 1; i < data.Count(); i++, n++)
                {
                    //importprogress.Value = (n)*100/data.Count();
                    var sing = data[i].Split(',');
                    if (sing != null || sing[0] != "")
                    {
                        _Product p = new _Product();
                        p.No = n.ToString();
                        p.Name = @sing[0] ?? "";
                        p.ProductCode = sing[1] ?? "";
                        p.Barcode = sing[2] ?? "";
                        p.Price = sing[3] ?? "";
                        p.WholeSalePrice = sing[4] ?? "";
                        p.Qty = sing[5] ?? "";
                        p.Brand = @sing[6] ?? "";
                        p.ProductCategory = @sing[7];
                        p.ProductSubCategory = @sing[8];
                        p.IsNotifyMinStock = sing[9];
                        p.MinStockQty = sing[10];
                        p.Size = sing[11];
                        p.Unit = sing[12];
                        p.Tax = Convert.ToDecimal(sing[13]);
                        importedList.Add(p);
                    }

                }
                return importedList;
            }
            catch (FormatException fx)
            {
                MessageBox.Show(this,"Your Imported file contains prohibited characters."+Environment.NewLine+@" [, / \ '] are not allowed by CSV import.","mPOS-FetchCSV",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                  return new List<_Product>();
            }
           
        }

        private string AutoGenerateProductCode()
        {
            const string prefix = "AG";
            var AGlist = new List<Product>(toSaveproduct);
            AGlist.Where(t => t.ProductCode.Substring(0, 2) == "AG").ToList();
            if ( db.Products.Where(t=>t.ProductCode.Substring(0,2)=="AG").ToList().Count>0 || AGlist.Count > 0)
            {
                var AG = AGlist.Select(t => new { ProductCode = t.ProductCode.Replace("AG", " ") });
                var AGfromDb = db.Products.Select(t=>new{ProductCode=t.ProductCode.Replace("AG"," ")});
                int agNumber = AGfromDb.Count()>0?AGfromDb.Max(t=>Convert.ToInt16(t.ProductCode)+1):AG.Max(t => Convert.ToInt16(t.ProductCode)) + 1;
                string productCode = prefix.ToString() + agNumber.ToString().PadLeft(9, '0');
                return productCode;
            }
            else
            {
                string agNumber = "000000001";
                string ProductCode = prefix + agNumber.ToString();
                return ProductCode;

            }
        }
       
        private void DivideTable()
        {
            lblstatus.Text = "Dividing Tables...";
            lblstatus.ForeColor = Color.Blue;
            lblstatus.Refresh();
            try
            {
                List<Product> totrackproduct = db.Products.ToList();
             
                toSaveproduct = new List<Product>();
                toSavebrand = new List<APP_Data.Brand>();
                toSavecategory = new List<APP_Data.ProductCategory>();
                toSavesubcategory = new List<APP_Data.ProductSubCategory>();
                toSaveUnit = new List<APP_Data.Unit>();
                toSaveTax = new List<APP_Data.Tax>();
                int maincount = toSavebrand.Count() + toSavecategory.Count() + toSavesubcategory.Count() + toSaveTax.Count() + toSaveUnit.Count() + toSaveproduct.Count();
                int counter = 0;
                if (dgvImport.Rows.Count > 0 || importedList.Count > 0)
                {
                    #region Category
                    db = new POSEntities();
                    var existiedCategories = db.ProductCategories.Select(p => p.Name).ToList();
                    var tempcategory = importedList.Select(p => p.ProductCategory).Distinct().ToList().Except(existiedCategories).Select(a => a.ToString()).ToList();
                    if (tempcategory.Count>0)
                    {
                        lblstatus.Text = "Saving Categories...";
                        lblstatus.ForeColor = Color.Blue;
                        lblstatus.Refresh();
                        counter = 0;
                        foreach (var c in tempcategory)
                        {
                            APP_Data.ProductCategory singlecategory = new APP_Data.ProductCategory();
                            singlecategory.Id = db.ProductCategories.Count() > 0 ? db.ProductCategories.Max(a => a.Id) + 1 : 1;
                            singlecategory.Name = c.ToString();
                            singlecategory.IsDelete = false;
                            db.ProductCategories.Add(singlecategory);
                            db.SaveChanges();
                            counter++;
                            importprogress.Value = (counter) * 100 / tempcategory.Count();  
                        }
                        importprogress.Value = 0; importprogress.Refresh();
                    }
                  
                    #endregion
                    #region subCategory
                    db=new POSEntities();
                    var realsubcategory = db.ProductSubCategories.Select(p => new { ProductCategory = p.ProductCategory.Name, ProductSubCategory = p.Name }).ToList();
                    var tempsubCategory = importedList.Select
                        (p => new { ProductCategory = p.ProductCategory, ProductSubCategory = p.ProductSubCategory }).Where(x => x.ProductSubCategory != "0").Distinct()
                        .Except(realsubcategory).Distinct().ToList().OrderBy(a => a.ProductCategory).Select(p => new { p.ProductCategory, p.ProductSubCategory }).ToList();
                    if (tempsubCategory.Count>0)
                    {
                        lblstatus.Text = "Saving SubCategories...";
                        lblstatus.ForeColor = Color.Blue;
                        lblstatus.Refresh();
                        counter = 0;
                        foreach (var sc in tempsubCategory.Where(a => a.ProductSubCategory != "0"))
                        {
                          
                            APP_Data.ProductSubCategory singlesubcategory = new APP_Data.ProductSubCategory();
                            singlesubcategory.Id = db.ProductSubCategories.Count() > 0 ? db.ProductSubCategories.Max(a => a.Id) + 1 : 1;
                            singlesubcategory.Name = sc.ProductSubCategory;
                            singlesubcategory.ProductCategoryId = db.ProductCategories.Where(a => a.Name == sc.ProductCategory).FirstOrDefault().Id;
                            singlesubcategory.Prefix = null;
                            singlesubcategory.IsDelete = false;
                            db.ProductSubCategories.Add(singlesubcategory);
                            db.SaveChanges();
                            counter++;
                            importprogress.Value = (counter) * 100 / tempsubCategory.Count();
                           
                        }
                        importprogress.Value = 0; importprogress.Refresh();
                    }
                    #endregion
                    #region Brand
                    db = new POSEntities();
                    var realbrand = db.Brands.Select(p => p.Name).ToList();
                    var tempbrand = importedList.Select(p => p.Brand).Distinct().ToList().Except(realbrand).Select(a => a.ToString()).ToList();
                    if (tempbrand.Count>0)
                    {
                        lblstatus.Text = "Saving Brands...";
                        lblstatus.ForeColor = Color.Blue;
                        lblstatus.Refresh();
                        counter = 0;
                        foreach (var b in tempbrand)
                        {
                            APP_Data.Brand singleBrand = new APP_Data.Brand();
                            singleBrand.Id = db.Brands.Count() > 0 ? db.Brands.Max(a => a.Id) + 1 : 1;
                            singleBrand.Name = b.ToString();
                            singleBrand.AutoGenerateNo = 0;
                            singleBrand.IsDelete = false;
                            db.Brands.Add(singleBrand);
                            db.SaveChanges();
                            counter++;
                            importprogress.Value = (counter) * 100 / tempbrand.Count();
                        }
                        importprogress.Value=0;
                        importprogress.Refresh();
                    }
                    #endregion
                    #region Unit
                    db=new POSEntities();  
                    var realunit = db.Units.Select(p => p.UnitName).ToList();
                    var tempunit = importedList.Select(p => p.Unit).Distinct().ToList().Except(realunit).Select(u=>u.ToString()).ToList();
                    if (tempunit.Count>0)
	                {
                        lblstatus.Text = "Saving Units...";
                        lblstatus.ForeColor = Color.Blue;
                        lblstatus.Refresh();
                        counter = 0;
                        foreach (var u in tempunit)
                        {
                            Unit singleunit = new Unit();
                            singleunit.Id = db.Units.Count() > 0 ? db.Units.Max(a => a.Id) + 1 : 1;
                            singleunit.UnitName = u.ToString();
                            db.Units.Add(singleunit);
                            db.SaveChanges();
                            counter++;
                            importprogress.Value = (counter) * 100 / tempunit.Count();
                        }
                        importprogress.Value = 0; importprogress.Refresh();
	                }
                    #endregion
                    #region tax
                    db = new POSEntities();
                    var realtax = db.Taxes.Select(p => p.TaxPercent).ToList();
                    var temptax = importedList.Select(p => p.Tax).Distinct().ToList().Except(realtax).Select(u => u.ToString()).ToList();
                    if (temptax.Count>0)
                    {
                        lblstatus.Text = "Saving Taxes...";
                        lblstatus.ForeColor = Color.Blue;
                        lblstatus.Refresh();
                        counter = 0;
                        foreach (var t in temptax)
                        {
                            Tax singletax = new Tax();
                            singletax.Id = db.Taxes.Count() > 0 ? db.Taxes.Max(a => a.Id) + 1 : 1;
                            singletax.Name = t.ToString();
                            singletax.TaxPercent = Convert.ToDecimal(t.ToString());
                            singletax.IsDelete = false;
                            db.Taxes.Add(singletax);
                            db.SaveChanges();
                            counter++;
                            importprogress.Value = (counter) * 100 / temptax.Count();

                        }
                        importprogress.Value = 0; importprogress.Refresh();
                    }
                    #endregion
                    #region Product
                    db = new POSEntities();
                    List<APP_Data.Brand> totrackbrand = db.Brands.ToList();
                    var dbExistedProducts = db.Products.Where(a => a.IsConsignment == false && a.IsWrapper == false && (a.IsDiscontinue == false || a.IsDiscontinue == null)).Select(a => new { Name = a.Name }).OrderBy(a => a.Name).ToList();
                    importedList.ForEach(t =>
                    {
                        foreach (var item in db.Products.Where(a => a.IsConsignment == false && a.IsWrapper == false && (a.IsDiscontinue == false || a.IsDiscontinue == null)).Select(a => new { Name = a.Name, ProductCode = a.ProductCode }).ToList())
                        {
                            if (t.Name == item.Name)
                            {
                                t.ProductCode = item.ProductCode;
                            }
                        }
                    });
                    var incomingProducts = importedList.Select(a => new { Name = a.Name }).OrderBy(a => a.Name).ToList();
                    var NewProductsToCompare = incomingProducts.Except(dbExistedProducts).ToList();

                    var NewProductsToImport = (from i in importedList
                                               from n in NewProductsToCompare
                                               where i.Name == n.Name
                                               select i).ToList();
                    if (NewProductsToImport.Count() > 0)
                    {
                        foreach (_Product i in NewProductsToImport)
                        {
                            APP_Data.Product singleproduct = new APP_Data.Product();
                            //================================incoming data =======================================
                            singleproduct.Id = db.Products.Count() > 0 ? db.Products.Max(a => a.Id) + 1 : 1;
                            singleproduct.Name = i.Name;
                            singleproduct.BrandId = totrackbrand.Where(t => t.Name == i.Brand).FirstOrDefault().Id;
                            singleproduct.ProductCode = i.ProductCode == "0" ? Utility.Stock_AutoGenerateNo((int)singleproduct.BrandId,totrackbrand) : i.ProductCode;
                            singleproduct.Barcode = i.Barcode == "0" ? singleproduct.ProductCode : i.Barcode;
                            singleproduct.ProductCategoryId = db.ProductCategories.Where(t => t.Name == i.ProductCategory).FirstOrDefault().Id;
                            singleproduct.ProductSubCategoryId = db.ProductSubCategories.Where(t => t.Name == i.ProductSubCategory).FirstOrDefault() != null ? db.ProductSubCategories.Where(t => t.Name == i.ProductSubCategory).FirstOrDefault().Id : (int?)null;
                            singleproduct.Qty = i.Qty != "" || i.Qty != "0" || i.Qty != null ? Convert.ToInt32(i.Qty) : 0;
                            singleproduct.IsNotifyMinStock = i.IsNotifyMinStock == "Yes" || i.IsNotifyMinStock == "Y" ? true : false;
                            if (singleproduct.IsNotifyMinStock==true)
                            {
                                singleproduct.MinStockQty = i.MinStockQty != "" ? Convert.ToInt16(i.MinStockQty) : 0;
                            }
                            else
                            {
                                i.MinStockQty = null;
                            }
                            //singleproduct.MinStockQty = singleproduct.IsNotifyMinStock==true?i.MinStockQty != "" ? Convert.ToInt16(i.MinStockQty) : 0;
                            singleproduct.Price = i.Price != "" || i.Price != null ? Convert.ToInt32(i.Price) : 0;
                            singleproduct.PurchasePrice = 0;
                            singleproduct.WholeSalePrice = i.WholeSalePrice != "" || i.WholeSalePrice != null ? Convert.ToInt32(i.WholeSalePrice) : 0;
                            singleproduct.Size = i.Size ?? "";
                            //=============================incoming data ==========================================
                            singleproduct.UpdatedDate = DateTime.Now;
                            singleproduct.CreatedDate = DateTime.Now;
                            singleproduct.DiscountRate = 0;
                            singleproduct.CreatedBy = MemberShip.UserId;
                            singleproduct.UpdatedBy = MemberShip.UserId;
                            singleproduct.UnitId = i.Unit == "0" ? db.Units.FirstOrDefault().Id : db.Units.Where(t => t.UnitName == i.Unit).FirstOrDefault().Id;
                            singleproduct.TaxId = i.Tax == 0 ? db.Taxes.FirstOrDefault().Id : db.Taxes.Where(t => t.TaxPercent == i.Tax).FirstOrDefault().Id;
                            toSaveproduct.Add(singleproduct);

                            #region Update AutoGenerateNo. in brand table
                            var _brandData = (from b in totrackbrand where b.Id == singleproduct.BrandId select b).FirstOrDefault();
                            if (_brandData.AutoGenerateNo == null || _brandData.AutoGenerateNo == 0)
                            {
                                _brandData.AutoGenerateNo = 1;
                            }
                            else
                            {
                                _brandData.AutoGenerateNo += 1;
                            }
                            totrackbrand.Where(t => t.Id == _brandData.Id).FirstOrDefault().AutoGenerateNo = _brandData.AutoGenerateNo;
                            toupdatebrand.Add(_brandData);
                        }
                            #endregion
                    }
                  

                    var eliminateNewproducts = (from i in incomingProducts.Except(NewProductsToCompare).ToList()
                                                from a in importedList
                                                where i.Name == a.Name
                                                select a).ToList();

                    var ExistedProductsToUpdate = (from e in eliminateNewproducts
                                                   from p in db.Products
                                                   where e.Name == p.Name &&
                                                   (e.ProductCode != p.ProductCode || e.Barcode != p.Barcode || Convert.ToInt32(e.Price) != p.Price || e.Brand != p.Brand.Name || e.ProductCategory != p.ProductCategory.Name
                                                   || Convert.ToBoolean(e.IsNotifyMinStock = e.IsNotifyMinStock == "Yes" || e.IsNotifyMinStock == "Y" ? "true" : "false") != p.IsNotifyMinStock)
                                                   select e).Distinct().ToList();

                    existed = new List<_Product>();
                    newlist = new List<_Product>();
                    existed = ExistedProductsToUpdate;
                    NewProductsToImport.ForEach(t =>
                    {
                        foreach (var ts in toSaveproduct)
                        {
                            if (t.Name == ts.Name)
                            {
                                t.ProductCode = ts.ProductCode;
                                t.Barcode = ts.Barcode;
                            }
                        }
                    });
                    newlist = NewProductsToImport;
                    #endregion
                }
            }
            catch (FormatException f)
            {
                MessageBox.Show(this,f.Message,"mPOS-DivideTable",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            catch (ArgumentNullException e)
            {
                MessageBox.Show(e.Message, "mPOS-DivideTable", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                importprogress.Value = 0;
                lblstatus.ForeColor = Color.Blue;
                lblstatus.Text = "Ready";
                this.ControlBox = false;
                dgvImport.DataSource = null;
                dgvImport.Visible = true;
                list = new List<string>();
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.DefaultExt = "csv";
                ofd.Filter = "Data Files (*.csv)|*.csv";
                ofd.RestoreDirectory = true;
                ofd.InitialDirectory = @"D://";

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

                    fetched = FetchCSV(list);
                    if (fetched.Count==0)
                    {
                        lblstatus.Text = "Ready";
                        lblstatus.Refresh();
                        lblstatus.ForeColor = Color.Blue;
                        this.ControlBox = true;
                        importprogress.Value = 0;
                        importprogress.Refresh();
                    }
                    else
                    {

                        importprogress.Value = 0;
                        importprogress.Refresh();
                        if (!DataValidate())
                        {
                            lblstatus.Text = "Ready";
                            lblstatus.Refresh();
                            this.ControlBox = true;
                            return;
                        }
                        int oriName = fetched.Select(a => a.Name).Count();
                        int disName = fetched.Select(a => a.Name).Distinct().ToList().Count();
                        int oriPcode = fetched.Where(a => a.ProductCode != "0").Select(a => a.ProductCode).ToList().Count();
                        int disPcode = fetched.Where(a => a.ProductCode != "0").Select(a => a.ProductCode).Distinct().ToList().Count();
                        int oriBcode = fetched.Where(a => a.Barcode != "0").Select(a => a.Barcode).ToList().Count();
                        int disBcode = fetched.Where(a => a.Barcode != "0").Select(a => a.Barcode).Distinct().ToList().Count();
                        List<string> messages = new List<string>();
                        var originalFromDb = db.Products.Select(a => new { a.Name, a.ProductCode, a.Barcode }).ToList();
                        var originalFromFile = fetched.Select(a => new { a.No, a.Name, a.ProductCode, a.Barcode }).ToList();
                        var eliminateDb = (from odb in originalFromDb
                                           from odf in originalFromFile
                                           where odb.Name == odf.Name || odb.Barcode == odf.Barcode || odb.ProductCode == odf.ProductCode
                                           select odf.No).ToList();
                        if (eliminateDb.Count() > 0)
                        {
                            foreach (var item in eliminateDb)
                            {
                                string message = "Row No : " + item.ToString();
                                messages.Add(message);
                            }
                            string messageContent = string.Join(Environment.NewLine, messages);
                            MessageBox.Show(this, "Imported file includes existed ProductName or ProductCode or BarCode from database!!" + Environment.NewLine + Environment.NewLine +
                                messageContent + Environment.NewLine + "in your file should be removed or modified.", "mPOS-Import", MessageBoxButtons.OK, MessageBoxIcon.Warning
                                );
                            lblstatus.Text = "Ready";
                            lblstatus.ForeColor = Color.Blue;
                            lblstatus.Refresh();
                            this.ControlBox = true;
                            return;
                        }
                        if ((oriName == disName) && (oriPcode == disPcode) && (oriBcode == disBcode))
                        {
                            dgvImport.DataSource = fetched;
                            dgvImport.Refresh();
                            DivideTable();
                            dgvImport.Refresh();
                            ProductSaver();
                            this.ControlBox = true;
                        }
                        else
                        {
                            lblstatus.Text = "Ready";
                            lblstatus.Refresh();
                            lblstatus.ForeColor = Color.Blue;
                            MessageBox.Show("Imported file contains duplicated Product Names or ProductCode or Barcode !!");
                            this.ControlBox = true;
                            importprogress.Value = 0;
                            importprogress.Refresh();
                            return;
                        }
                    }
                   
                }
                else
                {
                    this.ControlBox = true;
                }
            }
            catch (IOException i)
            {
                MessageBox.Show(this, "File in use, Close it first."+Environment.NewLine+i.Message, "mPOS-ImportClick", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                this.ControlBox = true;
            }
           
        }
        
        public bool DataValidate()
        {
            int rowNo = 1;
            bool hasError = false;
            List<string> messages = new List<string>();
            foreach (var item in fetched)
            {
                if (string.IsNullOrEmpty(item.Name))
                {
                    hasError = true;
                    messages.Add("Name column is wrong or blank in row no " + rowNo.ToString());
                }
                else if (string.IsNullOrEmpty(item.ProductCode))
                {
                    hasError = true;
                    messages.Add("Product Code column is wrong in row no " + rowNo.ToString());
                }
                else if (string.IsNullOrEmpty(item.Barcode))
                {
                    hasError = true;
                    messages.Add("Bar Code column is wrong or blank in row no " + rowNo.ToString());
                }
                else if (string.IsNullOrEmpty(item.Brand))
                {
                    hasError = true;
                    messages.Add("Brand column is wrong or blank in row no " + rowNo.ToString());
                }
                else if (string.IsNullOrEmpty(item.Price))
                {
                    hasError = true;
                    messages.Add("Price column is wrong or blank in row no " + rowNo.ToString());
                }
                else if (string.IsNullOrEmpty(item.WholeSalePrice))
                {
                    hasError = true;
                    messages.Add("WholeSalePrice column is wrong or blank in row no " + rowNo.ToString());
                }
                else if (string.IsNullOrEmpty(item.Qty))
                {
                    hasError = true;
                    messages.Add("Quantity column is wrong or blank in row no " + rowNo.ToString());
                }
                else if (string.IsNullOrEmpty(item.ProductCategory))
                {
                    hasError = true;
                    messages.Add("Product Category column is wrong or blank in row no " + rowNo.ToString());
                }
                else if (string.IsNullOrEmpty(item.ProductSubCategory))
                {
                    hasError = true;
                    messages.Add("Product Sub Category column is wrong or blank in row no " + rowNo.ToString());
                }
                else if (string.IsNullOrEmpty(item.IsNotifyMinStock))
                {
                    hasError = true;
                    messages.Add("IsNotifyMinStock column is wrong or blank  in row no " + rowNo.ToString());
                }
                else if (string.IsNullOrEmpty(item.MinStockQty))
                {
                    hasError = true;
                    messages.Add("MinStockQty column is wrong or blank in row no " + rowNo.ToString());
                }
                else if (string.IsNullOrEmpty(item.Size))
                {
                    hasError = true;
                    messages.Add("Size column is wrong or blank in row no " + rowNo.ToString());
                }
                else if (string.IsNullOrEmpty(item.Unit))
                {
                    hasError = true;
                    messages.Add("Unit column is wrong or blank in row no " + rowNo.ToString());
                }
                else if (string.IsNullOrEmpty(item.Tax.ToString()))
                {
                    hasError = true;
                    messages.Add("Tax column is wrong in row no " + rowNo.ToString());
                }
                rowNo++;
                if (hasError)
                {
                    string messageContent = string.Join(Environment.NewLine, messages);
                    MessageBox.Show(this, "Data error or blank field!! " + Environment.NewLine + Environment.NewLine +
                        messageContent + Environment.NewLine, "mPOS-Import", MessageBoxButtons.OK, MessageBoxIcon.Warning
                        );
            
                    return false;
                }
            }
            return true;
        }

        void ProductSaver()
        {
            
            if (toSaveproduct != null)
            {

                lblstatus.Text = "Saving Products...";
                lblstatus.ForeColor = Color.Blue;
                lblstatus.Refresh();
                importprogress.Value = 0; importprogress.Refresh();
                db = new POSEntities();
                int a = 0;
                foreach (var i in toSaveproduct)
                {

                    int ProductCodeCount = 0, BarcodeCount = 0, ProductNameCount = 0;

                    ProductCodeCount = (from p in db.Products where p.ProductCode.Trim() == i.ProductCode.Trim() select p).ToList().Count;
                    BarcodeCount = (from p in db.Products where p.Barcode.Trim() == i.Barcode.Trim() select p).ToList().Count;
                    ProductNameCount = (from p in db.Products where p.Name.Trim() == i.Name.Trim() select p).ToList().Count;

                    if (ProductCodeCount == 0 && BarcodeCount == 0)
                    {
                        APP_Data.Product productObj = new APP_Data.Product();

                        productObj.Id = i.Id;
                        productObj.Barcode = i.Barcode;
                        productObj.ProductCode = i.ProductCode;

                        productObj.Name = i.Name;

                        productObj.BrandId = i.BrandId;

                        productObj.CreatedBy = i.CreatedBy;
                        productObj.CreatedDate = i.CreatedDate;
                        productObj.UpdatedBy = i.UpdatedBy;
                        productObj.UpdatedDate = i.UpdatedDate;
                        productObj.Price = i.Price;
                        if (i.WholeSalePrice.ToString() == string.Empty)
                        {
                            productObj.WholeSalePrice = 0;
                        }
                        else
                        {
                            productObj.WholeSalePrice = i.WholeSalePrice;
                        }
                        productObj.UnitType = "Normal";
                        productObj.UnitId = i.UnitId;
                        productObj.TaxId = i.TaxId;
                        //if discount is null, add default value
                        if (i.DiscountRate.ToString() == string.Empty)
                        {
                            productObj.DiscountRate = 0;
                        }
                        else
                        {
                            productObj.DiscountRate = i.DiscountRate;
                        }
                        productObj.Size = i.Size;
                        if (i.PurchasePrice.ToString() != string.Empty)
                        {
                            productObj.PurchasePrice = i.PurchasePrice;
                        }

                        productObj.IsNotifyMinStock = i.IsNotifyMinStock;
                        productObj.Qty = i.Qty;

                        if (i.IsConsignment == true)
                        {
                            productObj.Qty = i.Qty;
                        }
                        //if minstock qty is null, add default value
                        if (i.MinStockQty.ToString() == string.Empty)
                        {
                            productObj.MinStockQty = 0;
                        }
                        else
                        {
                            productObj.MinStockQty = i.MinStockQty;
                        }

                        if (i.ProductLocation == string.Empty)
                        {
                            productObj.ProductLocation = string.Empty;
                        }
                        else
                        {
                            productObj.ProductLocation = i.ProductLocation;
                        }

                        productObj.ProductCategoryId = i.ProductCategoryId;

                        if (i.ProductSubCategoryId != null || i.ProductSubCategoryId > 0)
                        {
                            productObj.ProductSubCategoryId = i.ProductSubCategoryId;
                        }

                        productObj.IsWrapper = false;

                        productObj.IsConsignment = false;

                        productObj.PhotoPath = string.Empty;

                        db.Products.Add(productObj);
                        a += db.SaveChanges();
                        importprogress.Value = (a) * 100 / toSaveproduct.Count();
                    }
                   
                   
                }
                if (a > 0)
                {
                    if (toupdatebrand.Count > 0)//2
                    {
                        db = new POSEntities();
                        foreach (var item in toupdatebrand)
                        {
                            var brandtoUpdate = db.Brands.Find(item.Id);
                            if (brandtoUpdate != null)
                            {
                                brandtoUpdate.AutoGenerateNo = item.AutoGenerateNo;
                                db.Entry(brandtoUpdate).State = EntityState.Modified;
                            }
                        }
                        db.SaveChanges();
                    }
                    lblstatus.Text = "Importing Done";
                    lblstatus.ForeColor = Color.LimeGreen;
                    lblstatus.Refresh();
                    MessageBox.Show(a.ToString() + " Rows Successully imported.");
                    
                }
                else
                {
                    lblstatus.Text = "Ready";
                    lblstatus.ForeColor = Color.Blue;
                    lblstatus.Refresh();
                    importprogress.Value = 0;
                    importprogress.Refresh();
                }
            }
        }
    }
}