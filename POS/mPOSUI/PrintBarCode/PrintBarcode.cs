using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using POS.APP_Data;

namespace POS
{
    public partial class PrintBarcode : Form
    {
        #region Variable

        public int productId;
        POSEntities entity = new POSEntities();
        Product productObj = new Product();
        #endregion

        #region Event

        public PrintBarcode()
        {
            InitializeComponent();
        }

        private void PrintBarcode_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            productObj = (from p in entity.Products where p.Id == productId select p).FirstOrDefault();
            lblBarCode.Text = productObj.Barcode.ToString();        
            lblItemName.Text = productObj.Name;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            #region [ Print ]

            int icount = Convert.ToInt32(txtQty.Text);

            for (int i = 1; i <= icount; i++)
            {
                string price = productObj.Price + "Ks";
                print(productObj.Barcode.ToString(), productObj.Name.ToString(), price); 
            }         
           
            #endregion
        }

        private void print(string barcode, string productname, string bid)
        {
            //string barcodePrinter = (from b in entity.Settings where b.Key == "barcode_printer" select b.Value).SingleOrDefault();
            //PrintLab.OpenPort(barcodePrinter);
            //PrintLab.OpenPort("POSTEK C168/200s (Copy 1)");
            PrintLab.OpenPort("POSTEK C168/200s");
            PrintLab.PTK_ClearBuffer();
            PrintLab.PTK_SetPrintSpeed(4);
            PrintLab.PTK_SetDarkness(10);
            PrintLab.PTK_SetLabelHeight(1, 1, 0, false);
            PrintLab.PTK_SetLabelWidth(800);


                    PrintLab.PTK_DrawTextTrueTypeW(100, 10, 30, 0, "Zawgyi-One", 1, 100, false, false, false, "A1", bid); //price       
                    PrintLab.PTK_DrawBarcode(10, 40, 0, "1", 2, 2, 50, 'B', barcode); //barcode 

                    PrintLab.PTK_DrawTextTrueTypeW(390, 10, 30, 0, "Zawgyi-One", 1, 100, false, false, false, "A1", bid); //price       
                    PrintLab.PTK_DrawBarcode(280, 40, 0, "1", 2, 2, 50, 'B', barcode); //barcode 

                    PrintLab.PTK_DrawTextTrueTypeW(660, 10, 30, 0, "Zawgyi-One", 1, 100, false, false, false, "A1", bid); //price      
                    PrintLab.PTK_DrawBarcode(550, 40, 0, "1", 2, 2, 50, 'B', barcode); //barcode 

                    PrintLab.PTK_PrintLabel(1, 1);
                    PrintLab.ClosePort();
        }

        #endregion
    }
   
}
public class PrintLab
{
    [DllImport("WINPSK.dll")]
    public static extern int OpenPort(string printname);
    [DllImport("WINPSK.dll")]
    public static extern int PTK_SetPrintSpeed(uint px);
    [DllImport("WINPSK.dll")]
    public static extern int PTK_SetDarkness(uint id);
    [DllImport("WINPSK.dll")]
    public static extern int ClosePort();
    [DllImport("WINPSK.dll")]
    public static extern int PTK_PrintLabel(uint number, uint cpnumber);
    [DllImport("WINPSK.dll")]
    public static extern int PTK_DrawTextTrueTypeW
                                        (int x, int y, int FHeight,
                                        int FWidth, string FType,
                                        int Fspin, int FWeight,
                                        bool FItalic, bool FUnline,
                                        bool FStrikeOut,
                                        string id_name,
                                        string data);
    [DllImport("WINPSK.dll")]
    public static extern int PTK_DrawBarcode(uint px,
                                    uint py,
                                    uint pdirec,
                                    string pCode,
                                    uint pHorizontal,
                                    uint pVertical,
                                    uint pbright,
                                    char ptext,
                                    string pstr);
    [DllImport("WINPSK.dll")]
    public static extern int PTK_SetLabelHeight(uint lheight, uint gapH, int gapOffset, bool bFlag);
    [DllImport("WINPSK.dll")]
    public static extern int PTK_SetLabelWidth(uint lwidth);
    [DllImport("WINPSK.dll")]
    public static extern int PTK_ClearBuffer();
    [DllImport("WINPSK.dll")]
    public static extern int PTK_DrawRectangle(uint px, uint py, uint thickness, uint pEx, uint pEy);
    [DllImport("WINPSK.dll")]
    public static extern int PTK_DrawLineOr(uint px, uint py, uint pLength, uint pH);
    [DllImport("WINPSK.dll")]
    public static extern int PTK_DrawBar2D_QR(uint x, uint y, uint w, uint v, uint o, uint r, uint m, uint g, uint s, string pstr);
    [DllImport("WINPSK.dll")]
    public static extern int PTK_DrawBar2D_Pdf417(uint x, uint y, uint w, uint v, uint s, uint c, uint px, uint py, uint r, uint l, uint t, uint o, string pstr);
    [DllImport("WINPSK.dll")]
    public static extern int PTK_PcxGraphicsDel(string pid);
    [DllImport("WINPSK.dll")]
    public static extern int PTK_PcxGraphicsDownload(string pcxname, string pcxpath);
    [DllImport("WINPSK.dll")]
    public static extern int PTK_DrawPcxGraphics(uint px, uint py, string gname);
    [DllImport("WINPSK.dll")]
    public static extern int PTK_DrawText(uint px, uint py, uint pdirec, uint pFont, uint pHorizontal, uint pVertical, char ptext, string pstr);

}