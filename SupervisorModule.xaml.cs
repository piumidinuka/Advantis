using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Advantis
{
    /// <summary>
    /// Interaction logic for SupervisorModule.xaml
    /// </summary>
    public partial class SupervisorModule 
    {

     /*   
        BackgroundWorker worker;
        BackgroundWorker worker2;
        BackgroundWorker loadErrorTable;
        BackgroundWorker loadNewlyPrintedBarcodes;
        BackgroundWorker viewLocation;
        BackgroundWorker breakPartition;
        */
        List<string> dbBarcodeValue = new List<string>();
        public string listDate { get; set; }
        public SupervisorModule()
        {
            InitializeComponent();
      /*
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += new DoWorkEventHandler(longTaskDowork);
            worker.ProgressChanged += new ProgressChangedEventHandler(prgressReport);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerComplete);
            worker.RunWorkerAsync();

            worker2 = new BackgroundWorker();
            worker2.WorkerReportsProgress = true;
            worker2.DoWork += new DoWorkEventHandler(longTaskDowork2);
            worker2.ProgressChanged += new ProgressChangedEventHandler(prgressReport2);
            worker2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerComplete2);
            worker2.RunWorkerAsync();
            */

        }

        //Start Supervisor Back end 

        private void btnMenuBarS_Click(object sender, RoutedEventArgs e)
        {
            navigationMenuSupervisor.IsOpen = true;
        }

        private void btnHomeS_Click(object sender, RoutedEventArgs e)
        {
            GridMainWindowProcessFlowOnlyS.Visibility = System.Windows.Visibility.Visible;
            GridNewProccessS.Visibility = System.Windows.Visibility.Hidden;
            GridNewlyPrintedBarcodeS.Visibility = System.Windows.Visibility.Hidden;
            GridReportsFlowS.Visibility = System.Windows.Visibility.Hidden;
            navigationMenuSupervisor.IsOpen = false;
            TopBarMenuItemTextS.Text = "HOME";
            btnMenuItemSelectIconS.Content = FindResource("SelectImgHomeS");
        }

        private void btnProgressS_Click(object sender, RoutedEventArgs e)
        {
            GridNewProccessS.Visibility = System.Windows.Visibility.Visible;
            GridMainWindowProcessFlowOnlyS.Visibility = System.Windows.Visibility.Hidden;
            GridNewlyPrintedBarcodeS.Visibility = System.Windows.Visibility.Hidden;
            GridReportsFlowS.Visibility = System.Windows.Visibility.Hidden;
            navigationMenuSupervisor.IsOpen = false;
            TopBarMenuItemTextS.Text = "PROGRESS";
            btnMenuItemSelectIconS.Content = FindResource("SelectImgProgressS");
        }

        private void btnBarcodesS_Click(object sender, RoutedEventArgs e)
        {
            GridNewlyPrintedBarcodeS.Visibility = System.Windows.Visibility.Visible;
            GridMainWindowProcessFlowOnlyS.Visibility = System.Windows.Visibility.Hidden;
            GridNewProccessS.Visibility = System.Windows.Visibility.Hidden;
            GridReportsFlowS.Visibility = System.Windows.Visibility.Hidden;
            navigationMenuSupervisor.IsOpen = false;
            TopBarMenuItemTextS.Text = "BARCODES";
            btnMenuItemSelectIconS.Content = FindResource("SelectImgBarcodesS");
        }

        private void btnReportsS_Click(object sender, RoutedEventArgs e)
        {
            GridReportsFlowS.Visibility = System.Windows.Visibility.Visible;
            GridMainWindowProcessFlowOnlyS.Visibility = System.Windows.Visibility.Hidden;
            GridNewProccessS.Visibility = System.Windows.Visibility.Hidden;
            GridNewlyPrintedBarcodeS.Visibility = System.Windows.Visibility.Hidden;
            navigationMenuSupervisor.IsOpen = false;
            TopBarMenuItemTextS.Text = "REPORTS";
            btnMenuItemSelectIconS.Content = FindResource("SelectImgReportsS");
        }

        private void btnLogoutS_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddNewBarcodeS_Click(object sender, RoutedEventArgs e)
        {
            BarcodeAdd s = new BarcodeAdd();
            s.Show();
        }
        /*
        void prgressReport(object sender, ProgressChangedEventArgs e)
        {


            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;
            con.Open();
            try
            {

                string[] lines = System.IO.File.ReadAllLines(@"D:\\Barcode\BarcodeReader.txt");
                string readdata;


                Console.WriteLine("The first line" + lines[0]);
                readdata = lines[0];
                string barcode = readdata.Substring(1, 13);
                //string barcode = readdata;
                Console.WriteLine("barcode" + barcode);

                string readCones = readdata.Substring(14, readdata.Length - 14);
                Console.WriteLine("cones" + readCones);
                string first7Digit = readdata.Substring(1, 7);
                int readNumberOfCones = int.Parse(readCones);
                Console.WriteLine("readNumberOfCones" + readNumberOfCones);
                Console.WriteLine("first7Digit" + first7Digit);

                double systemFullBarcodeWeight = readNumberOfCones;




                // Console.WriteLine("weight---------" + weight);
                SqlDataAdapter da = new SqlDataAdapter("select SystemBarcode from BarcodeInfo", con);

                DataSet ds = new DataSet();
                da.Fill(ds, "BarcodeInfo");

                dbBarcodeValue = new List<string>();
                foreach (DataRow row in ds.Tables["BarcodeInfo"].Rows)
                {
                    dbBarcodeValue.Add(row["SystemBarcode"].ToString());

                }
                if (dbBarcodeValue.Any(str => barcode.Contains(str)) && (barcode.Length == 13))
                {


                    try
                    {
                        btnBarcodeOneSignalProgress.Content = FindResource("SignalA1");
                   
                        Console.WriteLine("before execute");
                        string art_tkt = "select AVGWeight from WeightDetails_ATS_wise where ART_TKT=@first7Digit";
                        SqlCommand art_tkt_query = new SqlCommand(art_tkt, con);
                        art_tkt_query.Connection = con;

                        art_tkt_query.Parameters.AddWithValue("@first7Digit", first7Digit);
                        art_tkt_query.ExecuteNonQuery();
                        double result = ((double)art_tkt_query.ExecuteScalar());
                        double weight = result * readNumberOfCones;


                        SqlCommand cmd = new SqlCommand();

                        string updatequery = "update BarcodeInfo set ReadBarcodeNumberOfCones=ReadBarcodeNumberOfCones+@IncrementValue,SystemFullBarcodeWeight=SystemFullBarcodeWeight+@weight where SystemBarcode=@barcode";
                        cmd.Connection = con;
                        SqlCommand query = new SqlCommand(updatequery, con);
                        Console.WriteLine("after execute");
                        Console.WriteLine("after execute barcode" + barcode);
                        query.Parameters.AddWithValue("@barcode", barcode);
                        query.Parameters.AddWithValue("@weight", weight);
                        query.Parameters.AddWithValue("@IncrementValue", readNumberOfCones);



                        Console.WriteLine("after execute barcode------" + barcode);
                        query.ExecuteNonQuery();

                        Console.WriteLine(" query.ExecuteNonQuery() barcode------" + updatequery);





                        //   Console.WriteLine("tollerance"+ tollerance);
                        //  if ((weightFromAvery - weight == tollerance) || (weightFromAvery - weight == -tollerance))
                        // {
                        System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReaderBackup.txt", lines[0]);
                        System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReader.txt", string.Empty);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception is" + ex);
                    }


                }
                else
                {

                    string dd = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                    btnBarcodeOneSignalProgress.Content = FindResource("SignalA2");
                    btnBarcodeOneSignalProgress.Content = FindResource("SignalA2");
                    String updatequery = "insert into ErrorDisplay(Device,Barcode,Status,DateTime) values(@device,@barcode,@status,@datetime)";
                    SqlCommand query = new SqlCommand(updatequery, con);

                    query.Connection = con;
                    query.Parameters.AddWithValue("@device", "Barcode Reader 1");

                    query.Parameters.AddWithValue("@barcode", barcode);
                    query.Parameters.AddWithValue("@status", "Barcode doesn't match with system stored barcode");
                    query.Parameters.AddWithValue("@status", dd);
                    query.ExecuteNonQuery();
                    //   AdamControl ac = new AdamControl(17);
                    //   ac.BulbOn();
                    //   Thread.Sleep(10);
                    //     ac.BulbOff();

                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

            }
            finally
            {
                Console.WriteLine("finally");


                // System.IO.File.WriteAllText(@"D:\\Weight\weight.txt", string.Empty);
                System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReader.txt", string.Empty);
            }



        }

        void prgressReport2(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(@"D:\\Barcode\BarcodeReaderBackup.txt");
                string readdata;


                Console.WriteLine("The first line" + lines[0]);
                readdata = lines[0];
                string barcode = readdata.Substring(1, 13);
                //string barcode = readdata;
                Console.WriteLine("barcode" + barcode);

                string readCones = readdata.Substring(14, readdata.Length - 14);
                Console.WriteLine("cones" + readCones);
                string first7Digit = readdata.Substring(1, 7);
                int readNumberOfCones = int.Parse(readCones);
                Console.WriteLine("readNumberOfCones" + readNumberOfCones);
                Console.WriteLine("first7Digit" + first7Digit);

                double systemFullBarcodeWeight = readNumberOfCones;

                string[] readWeight = System.IO.File.ReadAllLines(@"D:\\Weight\weight.txt");
                string readweightfromavery;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;
                con.Open();
                if (readWeight != null)
                {
                    Console.WriteLine("The read line" + readWeight[0]);
                    readweightfromavery = readWeight[0];

                    double weightFromAvery = double.Parse(readweightfromavery);
                    Console.WriteLine("weightFromAvery" + weightFromAvery);

                    string art_tkt = "select AVGWeight from WeightDetails_ATS_wise where ART_TKT=@first7Digit";
                    SqlCommand art_tkt_query = new SqlCommand(art_tkt, con);
                    art_tkt_query.Connection = con;

                    art_tkt_query.Parameters.AddWithValue("@first7Digit", first7Digit);
                    art_tkt_query.ExecuteNonQuery();
                    double result = ((double)art_tkt_query.ExecuteScalar());
                    double weight = result * readNumberOfCones;

                    //------------------- condition check
                    if ((weight - 0.1 <= weightFromAvery) && (weightFromAvery <= (weight + 0.1)))
                    {
                        Console.WriteLine("Weight is------------------" + weight);
                        Console.WriteLine("equal");
                        
                        btnWeightSignalProgress.Content = FindResource("SignalB1");
                        AdamControl ac = new AdamControl(19);
                        ac.BulbOff();
                        Thread.Sleep(10);


                    }
                    else
                    {
                        Console.WriteLine("not equal");
                       
                        btnWeightSignalProgress.Content = FindResource("SignalB2");
                        AdamControl ac = new AdamControl(17);
                        //  ac.BulbOn();
                        Thread.Sleep(10);
                        ac.BulbOff();

                    }
                    string updatereadweightdate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                    string updatereadWeight = "update BarcodeInfo set ReadWeight=ReadWeight+@IncrementWeight,ReadWeightDate=@readWeightDate where SystemBarcode=@barcode";

                    SqlCommand updatereadweight_cmd = new SqlCommand(updatereadWeight, con);
                    updatereadweight_cmd.Connection = con;
                    updatereadweight_cmd.Parameters.AddWithValue("@barcode", barcode);
                    updatereadweight_cmd.Parameters.AddWithValue("@IncrementWeight", weightFromAvery);
                    updatereadweight_cmd.Parameters.AddWithValue("@readWeightDate", updatereadweightdate);
                    updatereadweight_cmd.ExecuteNonQuery();
                }
                else
                {
                    AdamControl ac = new AdamControl(17);
                    ac.BulbOn();
                    AdamControl ad = new AdamControl(18);
                    ad.BulbOn();
                    string dd = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                    String updatequery = "insert into ErrorDisplay(Device,Barcode,Status,DateTime) values(@device,@barcode,@status,@datetime)";
                    SqlCommand query = new SqlCommand(updatequery, con);

                    query.Connection = con;
                    query.Parameters.AddWithValue("@device", "Weight Scale");

                    query.Parameters.AddWithValue("@barcode", barcode);
                    query.Parameters.AddWithValue("@status", "Weight is not Equal");
                    query.Parameters.AddWithValue("@status", dd);
                    query.ExecuteNonQuery();

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReaderBackup.txt", string.Empty);
            }

        }

        void workerComplete2(object sender, RunWorkerCompletedEventArgs e)
        {
            System.Windows.MessageBox.Show("upload successfully");

        }
        private void longTaskDowork2(object sender, DoWorkEventArgs e)
        {
            int i = 0;
            //    for (int i = 0; i < times; i++)
            while (true)

            {
                ++i;
                Thread.Sleep(1000);
                worker2.ReportProgress(i);
            }
        }

        void workerComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            System.Windows.MessageBox.Show("upload successfully");

        }
        private void longTaskDowork(object sender, DoWorkEventArgs e)
        {
            int i = 0;
            //    for (int i = 0; i < times; i++)
            while (true)

            {
                ++i;
                Thread.Sleep(1000);
                worker.ReportProgress(i);
            }
        }


    */
    }
}
