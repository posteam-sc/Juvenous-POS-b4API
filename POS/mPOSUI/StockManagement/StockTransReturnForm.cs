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
using System.Data.Objects;
using Microsoft.Reporting.WinForms;

namespace POS
{
    public partial class StockTransReturnForm : Form
    {
        #region variable
        POSEntities entity = new POSEntities();
        List<strockInheader> StockProductList = new List<strockInheader>();
        Boolean isstart = false;
        public long StockTransId;
        private ToolTip tp = new ToolTip();
        List<int> deleteDetailId = new List<int>();
        public Boolean viewdetail = false;
       public Boolean stockTranferStatus = false;
       public Boolean StockInStatus = true;
       public Boolean StockReturnStatus = false;
       public Boolean btnvisible = false;
        #endregion

        #region event
                public StockTransReturnForm()
        {
            InitializeComponent();
        }

        private void cboFromShop_slectedIndexChanged(object sender, EventArgs e)
        {

        }

  

        private void StockTransReturnForm_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            StockProductList.Clear();
            //Utility.BindProductNotConsign(cboproduct);
            List<Product> productList = new List<Product>();
            Product productObj1 = new Product();
            productObj1.Id = 0;
            productObj1.Name = "Select";
            productList.Add(productObj1);

            var _productList = (from p in entity.Products where p.IsConsignment == false select p).ToList();
            productList.AddRange(_productList);
            cboproduct.DataSource = productList;
            cboproduct.DisplayMember = "Name";
            cboproduct.ValueMember = "Id";
            cboproduct.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboproduct.AutoCompleteSource = AutoCompleteSource.ListItems;

            //Utility.BindShop(cboFromShop);

            List<APP_Data.Shop> shoplist = new List<APP_Data.Shop>();

            APP_Data.Shop shopobj = new APP_Data.Shop();
            shopobj.Id = 0;
            shopobj.ShopName = "Select";
            shoplist.Add(shopobj);

            var _shopList = (from p in entity.Shops where p.Id != SettingController.DefaultShop.Id select p).ToList();
            shoplist.AddRange(_shopList);
            cboFromShop.DataSource = shoplist;
            cboFromShop.DisplayMember = "ShopName";
            cboFromShop.ValueMember = "Id";
            cboFromShop.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboFromShop.AutoCompleteSource = AutoCompleteSource.ListItems;

            txtToShop.Text = SettingController.DefaultShop.ShopName;
            btnPrint.Visible = false;
            LoadActiveStatus();
            if (StockTransId != 0)
            {

                cboFromShop.Enabled = false;
                txtStockCodeNo.Enabled = false;
                rdoStockIn.Enabled=false;
                rdoStockReturn.Enabled=false;
                rdostockTrans.Enabled=false;
              if (viewdetail == true)
              {
                  btnSave.Visible = false; btnCancel.Visible = false; btnAdd.Enabled = false;
                  this.Text = "View Stock Detail"; dgvProductList.Columns[5].Visible = false; 
                  txtStockCodeNo.Enabled = false;
                  if (btnvisible == true)
                  {
                      btnPrint.Enabled = true;
                  
                  }
                  else
                  {
                      btnPrint.Enabled = false;
                    
                  }
                  btnPrint.Visible = true;
              }
                else
                {
                    this.Text = "Update Stock Transaction";
                }

                StockInHeader EditStock = (from p in entity.StockInHeaders where p.id == StockTransId select p).FirstOrDefault();
                if (EditStock != null)
                {
                    txtStockCodeNo.Text = EditStock.StockCode.Trim();
                    if (StockInStatus == true)
                    {
                   
                        cboFromShop.SelectedValue = EditStock.FromShopId;
                    }
                    else if(stockTranferStatus==true || StockReturnStatus==true)
                    {
                        cboFromShop.SelectedValue = EditStock.ToShopId;
                    }
               
                    txtToShop.Text = SettingController.DefaultShop.ShopName;
                    dtDate.Text = EditStock.Date.Value.ToString();

                    var stockdetail = (from p in entity.StockInDetails where p.StockInHeaderId == StockTransId select p).ToList();
                    StockProductList.AddRange(stockdetail.Select(_stock =>
                        new strockInheader
                        {
                            StockInHeaderId=_stock.StockInHeaderId,
                            StockInDetailId=_stock.Id,
                            productId=_stock.ProductId,
                            productname=_stock.Product.Name,
                            Qty=_stock.Qty,
                            barcode=_stock.Product.Barcode

                        }));
                  
                    dgvProductList.DataSource = StockProductList;
                    btnSave.Image = POS.Properties.Resources.update_big;
                   
                }
            }

        }

        private void cboproduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboproduct.SelectedIndex > 0)
            {
                long productId = Convert.ToInt32(cboproduct.SelectedValue);
                string barcode = (from p in entity.Products where p.Id == productId select p.Barcode).FirstOrDefault();
                txtBarcode.Text = barcode;
            }
        }

        private void txtBarcodeLeave(object sender, EventArgs e)
        {
            string _barcode = txtBarcode.Text;
            long productId = (from p in entity.Products where p.Barcode == _barcode && p.IsConsignment == false select p.Id).FirstOrDefault();
            cboproduct.SelectedValue = productId;
            cboproduct.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Product productobj = new Product();
            Boolean hasError = false;

            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";

            if (cboFromShop.SelectedIndex == 0)
            {
                tp.SetToolTip(cboFromShop, "Error");
                tp.Show("Please fill up shop name!", cboFromShop);
                hasError = true;
            }
            else if (txtBarcode.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtBarcode, "Error");
                tp.Show("Please fill up Barcode!", txtBarcode);
                hasError = true;
            }
            else if (Utility.Product_Combo_Control(cboproduct))
            {
                tp.SetToolTip(cboproduct, "Error");
                tp.Show("Please fill up product!", cboproduct);
                hasError = true;
            }
            else if (txtQty.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtQty, "Error");
                tp.Show("Please fill up product quantity!", txtQty);
                hasError = true;
            }
            else if (StockInStatus==true)
            {
                if (StockTransId == 0)
                {
                    StockInHeader stock = (from p in entity.StockInHeaders where p.StockCode == txtStockCodeNo.Text.Trim() && p.Status == "StockIn" select p).FirstOrDefault();
                    if (stock != null)
                    {
                        tp.SetToolTip(txtStockCodeNo, "Error");
                        tp.Show("StockIn Code number is already exist", txtStockCodeNo);
                        hasError = true;
                    }
                }
            }


            if (!hasError)
            {
                bool isAdd = false;
                dgvProductList.DataSource = "";
                int productId = Convert.ToInt32(cboproduct.SelectedValue);
                productobj = (from p in entity.Products where p.Id == productId select p).FirstOrDefault();
                string tempBarcode = "";
                string tempBarcode1 = "";
                if (productobj != null)
                {
                    strockInheader newpc = new strockInheader();

                    newpc.barcode = productobj.Barcode;
                    newpc.productname = productobj.Name;
                    newpc.Qty = Convert.ToInt32(txtQty.Text.ToString());
                    //    newpc.PurchasePrice = Convert.ToInt32(txtUnitPrice.Text.ToString());
                    //  newpc.Total = Convert.ToInt32(newpc.Qty * newpc.PurchasePrice);
                    newpc.productId = productobj.Id;
                    
                    int count = StockProductList.Count;

                    //if (mainPurchaseId == 0)
                    //{
                    //    count =dgvProductList.RowCount;
                    //}

                    if (count > 0)
                    {
                        for (int i = count - 1; i >= 0; i--)
                        {
                            strockInheader tempProduct1 = StockProductList[i];
                            //    Int32 tempPurchasePrice1 = Convert.ToInt32(tempProduct1.PurchasePrice);
                            tempBarcode1 = Convert.ToString(tempProduct1.barcode);

                            if (tempProduct1.barcode == newpc.barcode && tempProduct1.productId== newpc.productId)
                            {
                                tempProduct1.Qty += newpc.Qty;

                             //   tempProduct1.StockInDetailId = newpc.StockInDetailId;
                                tempProduct1.productId = newpc.productId;
                                tempProduct1.productname = newpc.productname;
                                tempProduct1.barcode = newpc.barcode;
                           
                               StockProductList.RemoveAt(i);//remove second product
                   
                                StockProductList.Insert(i, tempProduct1);

                                isAdd = true;
                     
                            }
                        }


                    }
                    if (isAdd == false)
                    {
                        StockProductList.Add(newpc);
                    }
                }

                dgvProductList.DataSource = StockProductList;
                txtQty.Text = "";
                txtBarcode.Clear();
                cboproduct.SelectedIndex = 0;
                cboproduct.Focus();

            }
        }

        private void dgvProductList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvProductList.Rows)
            {
                //if (mainPurchaseId == 0)
                //{
                strockInheader p = (strockInheader)row.DataBoundItem;
                //    row.Cells[0].Value = Convert.ToInt64(p.PurchaseDetailId);
                row.Cells[0].Value = Convert.ToInt64(p.productId);
                row.Cells[1].Value = p.StockInDetailId;
                row.Cells[2].Value = p.barcode.Trim();
                row.Cells[3].Value = p.productname.Trim();
                row.Cells[4].Value = Convert.ToInt32(p.Qty);
                // row.Cells[5].Value = Convert.ToInt64(p.PurchasePrice);
                // row.Cells[6].Value = Convert.ToInt32(p.Total);

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            APP_Data.StockInHeader StockIn = new APP_Data.StockInHeader();
            // entity = new POSEntities();
            Boolean hasError = false;

            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";

            string codeNo = txtStockCodeNo.ToString();
            int FromShop = Convert.ToInt32(cboFromShop.SelectedValue);
            // APP_Data.StockInHeader stockObj=entity.stockInHeaders.Where(x=>x.id==codeNo && x.FromShopId==FromShop && x.IsDelete==false)


            if (cboFromShop.SelectedIndex == 0)
            {
                tp.SetToolTip(cboFromShop, "Error");
                tp.Show("Please fill up shop name!", cboFromShop);
                hasError = true;
            }

            else if (StockProductList.Count == 0)
            {
                MessageBox.Show("Please fill up product list ", "Error");
                return;
            }
            else if (StockInStatus == true)
            {
                if (StockTransId == 0)
                {
                    StockInHeader stock = (from p in entity.StockInHeaders where p.StockCode == txtStockCodeNo.Text.Trim() && p.Status == "StockIn" select p).FirstOrDefault();
                    if (stock != null)
                    {
                        tp.SetToolTip(txtStockCodeNo, "Error");
                        tp.Show("StockIn Code number is already exist", txtStockCodeNo);
                        hasError = true;
                    }
                }
            }

            if (!hasError)
            {
                if (StockTransId != 0)
                {
                    StockIn = (from c in entity.StockInHeaders where c.id == StockTransId select c).FirstOrDefault();
                    if(stockTranferStatus==true || StockReturnStatus==true)
                    {
                        StockIn.ToShopId = Convert.ToInt32(cboFromShop.SelectedValue);
                    }else if(StockInStatus==true)
                    {
                        StockIn.FromShopId = Convert.ToInt32(cboFromShop.SelectedValue);
                    }
              
                    StockIn.UpdatedDate = DateTime.Now;
                    StockIn.UpdatedUser = MemberShip.UserId;

                    foreach (strockInheader stocklist in StockProductList)
                    {
                        if (stocklist.StockInDetailId == 0)
                        {
                            APP_Data.StockInDetail stockindetail = new APP_Data.StockInDetail();
                            stockindetail.StockInHeaderId = StockTransId;
                            stockindetail.Id = stocklist.StockInDetailId;
                            stockindetail.ProductId = stocklist.productId;
                            stockindetail.Qty = stocklist.Qty;
                            entity.StockInDetails.Add(stockindetail);
                            entity.SaveChanges();
                        }
                        else
                        {
                            APP_Data.StockInDetail stockDetailEdit = entity.StockInDetails.Where(x => x.Id == stocklist.StockInDetailId).FirstOrDefault();
                            stockDetailEdit.Qty = stocklist.Qty;
                            entity.Entry(stockDetailEdit).State = EntityState.Modified;
                            entity.SaveChanges();
                        }
                    }
                    MessageBox.Show("Successfully updated!", "update");
                    this.Close();
                    //     clearDate();

                }
                else
                {
                    string month = "";
                    if (DateTime.Now.Month < 10)
                    {
                        month = "0" + DateTime.Now.Month.ToString();
                    }
                    else
                    {
                        month = DateTime.Now.Month.ToString();

                    }
                    string day = "";
                    if (DateTime.Now.Day < 10)
                    {
                        day = "0" + DateTime.Now.Day.ToString();
                    }
                    else
                    {
                        day = DateTime.Now.Day.ToString();
                    }


                    DateTime date = dtDate.Value.Date;
                    int? _count;
                    _count = (from con in entity.StockInHeaders where (EntityFunctions.TruncateTime(con.CreatedDate) == date) orderby con.id descending where con.Count!=null select con.Count ?? 0).FirstOrDefault();
                    _count += 1;
                    string stockInHeaderId="";
                    if (stockTranferStatus == true)
                    {
                        if (_count < 10)
                        {
                            stockInHeaderId = "ST" + SettingController.DefaultShop.ShortCode + DateTime.Now.Year.ToString() + month + DateTime.Now.Day.ToString() + ("0" + _count).ToString();
                        }
                        else
                        {
                            stockInHeaderId = "ST" + SettingController.DefaultShop.ShortCode + DateTime.Now.Year.ToString() + month + DateTime.Now.Day.ToString() + (_count).ToString();
                        }
                    }
                    else if (StockInStatus == true)
                    {
                        stockInHeaderId = txtStockCodeNo.Text.Trim();
                    }
                    else if(StockReturnStatus==true)
                    {
                        if (_count < 10)
                        {
                            stockInHeaderId = "SR" + SettingController.DefaultShop.ShortCode + DateTime.Now.Year.ToString() + month + DateTime.Now.Day.ToString() + ("0" + _count).ToString();
                        }
                        else
                        {
                            stockInHeaderId = "SR" + SettingController.DefaultShop.ShortCode + DateTime.Now.Year.ToString() + month + DateTime.Now.Day.ToString() + (_count).ToString();
                        }
                    }


                    if (StockProductList.Count > 0)
                    {
                        StockInHeader stockInHeaderObj = new StockInHeader();
                        stockInHeaderObj.StockCode = stockInHeaderId;
                        stockInHeaderObj.Date = dtDate.Value;

                        if (stockTranferStatus == true)
                        {
                            stockInHeaderObj.ToShopId = Convert.ToInt32(cboFromShop.SelectedValue);
                            stockInHeaderObj.FromShopId = SettingController.DefaultShop.Id;
                            stockInHeaderObj.Status = "StockTransfer";
                        }
                        else if(StockInStatus==true)
                        {
                            stockInHeaderObj.FromShopId = Convert.ToInt32(cboFromShop.SelectedValue);
                            stockInHeaderObj.ToShopId = SettingController.DefaultShop.Id;
                            stockInHeaderObj.Status="StockIn";
                        }
                        else if (StockReturnStatus == true)
                        {
                            stockInHeaderObj.ToShopId = Convert.ToInt32(cboFromShop.SelectedValue);
                            stockInHeaderObj.FromShopId = SettingController.DefaultShop.Id;
                           stockInHeaderObj.Status = "StockReturn";
                        }
                     
                        stockInHeaderObj.IsApproved = false;
                        stockInHeaderObj.IsDelete = false;
                        stockInHeaderObj.CreatedDate = dtDate.Value;
                        stockInHeaderObj.CreatedUser = MemberShip.UserId;
                        stockInHeaderObj.Count = _count;
                        entity.StockInHeaders.Add(stockInHeaderObj);
                        entity.SaveChanges();
                    }

                    StockInDetail stockdetail = new StockInDetail();
                    foreach (strockInheader stock in StockProductList)
                    {
                          stockdetail.StockInHeaderId = (from p in entity.StockInHeaders where p.StockCode == stockInHeaderId orderby p.Date descending select p.id).FirstOrDefault();
                        stockdetail.ProductId = stock.productId;
                        stockdetail.Qty = stock.Qty;

                        entity.StockInDetails.Add(stockdetail);
                        entity.SaveChanges();


                    }
                    MessageBox.Show("Successfully saved!", "save");
                    //  clearDate();
                }


            }
            else
            {
                MessageBox.Show("Please,first fill Stock", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    cboExpenseCag.Enabled = true;
            }

            cleardata();

        }
      

        private void dgvProductList_cellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 5)
                {
                    int index = e.RowIndex;
                    int stockdetailId =Convert.ToInt32(dgvProductList[1, e.RowIndex].Value);

                    if (stockdetailId != 0)
                    {
                        StockInDetail deldetail = (from p in entity.StockInDetails where p.Id == stockdetailId select p).FirstOrDefault();
                        if (deldetail != null)
                        {
                            entity.StockInDetails.Remove(deldetail);
                            entity.SaveChanges();
                        }
                    }

               
                    deleteDetailId.Add(stockdetailId);
                    strockInheader stockdetail = StockProductList[index];
                    StockProductList.RemoveAt(index);

                    dgvProductList.DataSource = StockProductList.ToList();

                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cleardata();
        }

        private void rdoStockIn_checkedChanged(object sender, EventArgs e)
        {
            redioChanged();
        }

        private void rdostocktran_checkedChanged(object sender, EventArgs e)
        {
            redioChanged();
        }

        private void rdoStockReturn_checkedChanged(object sender, EventArgs e)
        {
            redioChanged();
        }
#endregion


        #region method
        private void cleardata()
        {
            cboFromShop.SelectedValue = 0;
            txtStockCodeNo.Text = "";
            txtBarcode.Text = "";
            cboproduct.SelectedValue = 0;
            txtQty.Text = "";
            StockProductList.Clear();
            dgvProductList.DataSource = "";
        }

        private void ForTranferInterface()
        {
           
            lblFromshop.Text = "*To Shop";
            lblToshop.Text = "* From Shop";
        }

        private void ForStockInInterface()
        {
            lblFromshop.Text = "* From Shop";
            lblToshop.Text = "* To Shop";
        }
        private void redioChanged()
        {
            if (rdoStockIn.Checked == true)
            {
                ForStockInInterface();
                txtStockCodeNo.Enabled = true;
                stockTranferStatus = false;
                StockReturnStatus = false;
                StockInStatus = true;
            }
            else if (rdostockTrans.Checked == true)
            {
                ForTranferInterface();
                txtStockCodeNo.Enabled = false;
                txtStockCodeNo.Text = "";
                stockTranferStatus = true;
                StockReturnStatus = false;
                StockInStatus = false;
            }
            else if(rdoStockReturn.Checked==true)
            {
                ForTranferInterface();
                txtStockCodeNo.Enabled = false;
                txtStockCodeNo.Text = "";
                stockTranferStatus = false;
                StockReturnStatus = true;
                StockInStatus = false;
            }
        }

        private void LoadActiveStatus()
        {
            if (stockTranferStatus == true)
            {
                ForTranferInterface();
                rdostockTrans.Checked = true;
                rdoStockIn.Checked = false;
                rdoStockReturn.Checked = false;
                groupBox1.Text = "Stock Transfer";
            }
            else if (StockInStatus == true)
            {
                ForStockInInterface();
                rdostockTrans.Checked = false;
                rdoStockIn.Checked = true;
                rdoStockReturn.Checked = false;
                groupBox1.Text = "Stock In";
            }
            else
            {
                ForTranferInterface();
                rdostockTrans.Checked = false;
                rdoStockIn.Checked = false;
                rdoStockReturn.Checked = true;
                groupBox1.Text = "Stock Return";
            }
        }
        #endregion

        private void btnPrintClick(object sender, EventArgs e)
        {
            #region [print]
            StockInHeader stock = (from p in entity.StockInHeaders where p.id == StockTransId select p).FirstOrDefault();
            string toshop = (from p in entity.Shops where p.Id == stock.ToShopId select p.ShopName).FirstOrDefault();
            string fromshop = (from p in entity.Shops where p.Id == stock.FromShopId select p.ShopName).FirstOrDefault();
            dsReportTemp dsReport = new dsReportTemp();
            dsReportTemp.StockTransferReturnDataTable dtStockTransferReport = (dsReportTemp.StockTransferReturnDataTable)dsReport.Tables["StockTransferReturn"];
            foreach (strockInheader stproduct in StockProductList)
            {
                dsReportTemp.StockTransferReturnRow newRow = dtStockTransferReport.NewStockTransferReturnRow();
                newRow.PId = stproduct.productId.ToString();
                newRow.ProductName = stproduct.productname;
                newRow.Qty = Convert.ToInt32(stproduct.Qty);
                newRow.Barcode = stproduct.barcode.ToString();
                dtStockTransferReport.AddStockTransferReturnRow(newRow);
            }
            ReportViewer rv = new ReportViewer();
            ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["StockTransferReturn"]);
            string reportPath = Application.StartupPath + "\\Reports\\StockTransferReturnPrint.rdlc";

            rv.LocalReport.ReportPath = reportPath;
            rv.LocalReport.DataSources.Clear();
            rv.LocalReport.DataSources.Add(rds);


            ReportParameter StockCode = new ReportParameter("StockCode", stock.StockCode);
            rv.LocalReport.SetParameters(StockCode);

            ReportParameter TransferTo = new ReportParameter("TransferTo", toshop);
            rv.LocalReport.SetParameters(TransferTo);

            ReportParameter TransferFrom = new ReportParameter("TransferFrom", fromshop);
            rv.LocalReport.SetParameters(TransferFrom);

            ReportParameter TransferDate = new ReportParameter("TransferDate", stock.Date.ToString());
            rv.LocalReport.SetParameters(TransferDate);






            Utility.Get_Print(rv);
         //   PrintDoc.PrintReport(rv);
            #endregion
        }

      
    }





   
}
