using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Printing;
using System.Drawing.Printing;
using System.Globalization;

using System.Windows.Xps;



using System.Drawing.Imaging;
using Limilabs.Barcode;
using System.Drawing;
using Microsoft.Win32.SafeHandles;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Advantis
{
    /// <summary>
    /// Interaction logic for BarcodeAdd.xaml
    /// </summary>
    public partial class BarcodeAdd
    {
        public string col { get; set; }
        public string tkt { get; set; }
        public string tex { get; set; }
        public string art { get; set; }
        public string numberOfCone { get; set; }
        public string empNo { get; set; }
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess,
uint dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition,
uint dwFlagsAndAttributes, IntPtr hTemplateFile);
        public BarcodeAdd()
        {

            InitializeComponent();
           // AllowsTransparency = true;
            DataContext = this;


        }

        private void barcodeCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void barcodePrint_Click(object sender, RoutedEventArgs e)
        {

            string colvalue = col.ToString();
            string artvalue = art.ToString();
            string tktvalue = tkt.ToString();
         //   string texvalue = tex.ToString();
            string numberOfConevalue = numberOfCone.ToString();
            string empNoValue = empNo.ToString();
            string finalGeneratedBarcode = artvalue + tktvalue + "." + colvalue + numberOfCone;



            string NewlyPrintedDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;
            con.Open();
            string findempName = "select empName from Advantic_Users where empNo=@empNo ";
            SqlCommand findempName_cmd = new SqlCommand(findempName, con);

            findempName_cmd.Parameters.AddWithValue("@empNo", empNoValue);

            findempName_cmd.ExecuteNonQuery();

            string empName = (string)findempName_cmd.ExecuteScalar();
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "insert into PartitionBarcodes(Barcode,NumberOfCones,Col,Art,Tkt,ReadWeight,ReadWeightDate,EmpName,DateTime)values(@Barcode,@NumberOfCones,@Col,@Art,@Tkt,'','',@empName,@Datetime)";
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@Barcode", finalGeneratedBarcode);
            cmd.Parameters.AddWithValue("@NumberOfCones", numberOfConevalue);
            cmd.Parameters.AddWithValue("@Col", colvalue);
            cmd.Parameters.AddWithValue("@Art", artvalue);
            cmd.Parameters.AddWithValue("@Tkt", tktvalue);
            cmd.Parameters.AddWithValue("@ReadWeight", "");
            cmd.Parameters.AddWithValue("@ReadWeightDate", "");
            cmd.Parameters.AddWithValue("@empName", empName);
            
            cmd.Parameters.AddWithValue("@DateTime", NewlyPrintedDate);


            cmd.ExecuteNonQuery();





            SqlCommand newlyprinted_cmd = new SqlCommand();

            newlyprinted_cmd.CommandText = "insert into NewlyPrintedBarcodeInfo(SystemBarcode,EmployeeName,NewlyPrintedDate)values(@SystemBarcode,@EmployeeName,@NewlyPrintedDate)";
            newlyprinted_cmd.Connection = con;
            newlyprinted_cmd.Parameters.AddWithValue("@SystemBarcode", finalGeneratedBarcode);
            newlyprinted_cmd.Parameters.AddWithValue("@EmployeeName", empName);
            newlyprinted_cmd.Parameters.AddWithValue("@NewlyPrintedDate", NewlyPrintedDate);


            newlyprinted_cmd.ExecuteNonQuery();
        
          
          
        string s = finalGeneratedBarcode;
           
             
            System.Windows.Controls.PrintDialog pd = new System.Windows.Controls.PrintDialog();
            String commands = "^XA^CF0,30,30^FO50,100^FDCOL : " + col + "^FS^FO350,100^FDART : " + art + "^FS^FO50,150^FDTKT : " + tkt + "^FS^FO350,150^FD^FS^FO50,200^FD^FS^FO350,200^FD"+ numberOfConevalue + "^FS^BY2,3^FO50,250^B3N,N,100,N,N^FD" + finalGeneratedBarcode + "^FS^XZ";
          
          
            RawPrinterHelper.SendStringToPrinter("ZDesigner GT800 (ZPL)", commands);


            // RawPrinterHelper.SendStringToPrinter("ZDesigner GT800 (ZPL)", commands);
            //

            this.Close();
    
        }

       
    }
}
