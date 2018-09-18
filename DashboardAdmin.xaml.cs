using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
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
using System.Timers;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;

namespace Advantis
{
    /// <summary>
    /// Interaction logic for DashboardAdmin.xaml
    /// </summary>
    /// 

    public partial class DashboardAdmin
    {

        static System.Timers.Timer timer;
        BackgroundWorker worker;
        BackgroundWorker worker2;
        BackgroundWorker loadErrorTable;
        BackgroundWorker loadNewlyPrintedBarcodes;
        BackgroundWorker viewLocation;
        BackgroundWorker breakPartition;
        BackgroundWorker pendingList;
        BackgroundWorker loadUsers;


        List<string> dbBarcodeValue = new List<string>();
        public string listDate { get; set; }
        public DashboardAdmin()
        {
             schedule();
            InitializeComponent();
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


            loadErrorTable = new BackgroundWorker();
            loadErrorTable.WorkerReportsProgress = true;
            loadErrorTable.DoWork += new DoWorkEventHandler(longTaskDowork_loadErrorTable);
            loadErrorTable.ProgressChanged += new ProgressChangedEventHandler(prgressReport_loadErrorTable);
            loadErrorTable.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerComplete_loadErrorTable);
            loadErrorTable.RunWorkerAsync();

            loadNewlyPrintedBarcodes = new BackgroundWorker();
            loadNewlyPrintedBarcodes.WorkerReportsProgress = true;
            loadNewlyPrintedBarcodes.DoWork += new DoWorkEventHandler(longTaskDowork_loadNewlyPrintedBarcodes);
            loadNewlyPrintedBarcodes.ProgressChanged += new ProgressChangedEventHandler(prgressReport_loadNewlyPrintedBarcodes);
            loadNewlyPrintedBarcodes.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerComplete_loadNewlyPrintedBarcodes);
            loadNewlyPrintedBarcodes.RunWorkerAsync();


            viewLocation = new BackgroundWorker();
            viewLocation.WorkerReportsProgress = true;
            viewLocation.DoWork += new DoWorkEventHandler(longTaskDowork_viewLocation);
            viewLocation.ProgressChanged += new ProgressChangedEventHandler(prgressReport_viewLocation);
            viewLocation.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerComplete_viewLocation);
            viewLocation.RunWorkerAsync();

            breakPartition = new BackgroundWorker();
            breakPartition.WorkerReportsProgress = true;
            breakPartition.DoWork += new DoWorkEventHandler(longTaskDowork_breakPartition);
            breakPartition.ProgressChanged += new ProgressChangedEventHandler(prgressReport_breakPartition);
            breakPartition.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerComplete_breakPartition);
            breakPartition.RunWorkerAsync();

            pendingList = new BackgroundWorker();
            pendingList.WorkerReportsProgress = true;
            pendingList.DoWork += new DoWorkEventHandler(longTaskDowork_pendingList);
            pendingList.ProgressChanged += new ProgressChangedEventHandler(prgressReport_pendingList);
            pendingList.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerComplete_pendingList);
            pendingList.RunWorkerAsync();

            loadUsers = new BackgroundWorker();
            loadUsers.WorkerReportsProgress = true;
            loadUsers.DoWork += new DoWorkEventHandler(longTaskDowork_loadUsers);
            loadUsers.ProgressChanged += new ProgressChangedEventHandler(prgressReport_loadUsers);
            loadUsers.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerComplete_loadUsers);
            loadUsers.RunWorkerAsync();

        }
        //*******************************************************************User Login
        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void loginMainWindow_Click(object sender, RoutedEventArgs e)
        {
            if (UserNameLogin.Text == "" || PasswordLogin.Password == "")
            {
                MessageBox.Show("Please provide UserName and Password");
                return;
            }
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;
            con.Open();

            try
            {
                //Create SqlConnection
                SqlCommand cmd_findusername = new SqlCommand();

                cmd_findusername.CommandText = "select empName from Advantic_Users where empNo=@UserNameLogin";
                cmd_findusername.Connection = con;
                cmd_findusername.Parameters.AddWithValue("@UserNameLogin", UserNameLogin.Text);
                cmd_findusername.ExecuteNonQuery();
                string userName = (string)cmd_findusername.ExecuteScalar();
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = "select userOpteam from Advantic_Users where empNo=@username and password=@password";
                cmd.Connection = con;

                cmd.Parameters.AddWithValue("@username", UserNameLogin.Text);
                //cmd.Parameters.AddWithValue("@password", PasswordTextLogin.Password)
                cmd.Parameters.AddWithValue("@password", CommonFunctions.sha256_hash(PasswordLogin.Password));
                Console.WriteLine("Password is" + CommonFunctions.sha256_hash(PasswordLogin.Password));

                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                //  DataSet ds = new DataSet();
                DataTable ds = new DataTable();
                adapt.Fill(ds);


                if (ds.Rows.Count == 1)

                {

                    // Dashboard fm = new Dashboard();
                    //fm.Show();


                    //int count = ds.Tables[0].Rows.Count;

                    if (ds.Rows[0][0].ToString() == "Supervisor")
                    {
                        SupervisorMainWindow.Visibility = System.Windows.Visibility.Visible;
                        GridAdminMainWindow.Visibility = System.Windows.Visibility.Hidden;
                        GridUserLoginMainWindow.Visibility = System.Windows.Visibility.Hidden;

                        LoginUserTextS.Text = "Hi, " + userName;
                   /*     try
                        {
                            System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReader.txt", string.Empty);
                            System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReader2.txt", string.Empty);
                            System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReader3.txt", string.Empty);
                            System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReaderBackup.txt", string.Empty);
                            System.IO.File.WriteAllText(@"D:\\Weight\weight.txt", string.Empty);
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
*/

                    }


                    //UploadCSV uploadCsv = new UploadCSV();
                    //uploadCsv.uploadCsv();



                    else if (ds.Rows[0][0].ToString() == "Administrator")
                    {
                        GridAdminMainWindow.Visibility = System.Windows.Visibility.Visible;
                        SupervisorMainWindow.Visibility = System.Windows.Visibility.Hidden;
                        GridUserLoginMainWindow.Visibility = System.Windows.Visibility.Hidden;
                        LoginUserText.Text = "Hi, " + userName;
                     /*   try
                        {
                            System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReader.txt", string.Empty);
                            System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReader2.txt", string.Empty);
                            System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReader3.txt", string.Empty);
                            System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReaderBackup.txt", string.Empty);
                            System.IO.File.WriteAllText(@"D:\\Weight\weight.txt", string.Empty);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }*/
                    }
                    else
                    {
                        MessageBox.Show("Invalid");
                    }
                }



                else
                {
                    MessageBox.Show("Please check your Username and Password");
                }

            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Console.WriteLine(ex);

            }
            //connection should be closed
            finally
            {
                con.Close();
            }

        }
        //********************************************************End of User Login


            
        //************************BackgroundWorker read values and compare with db--- Barcode reader 1****************************************************
        
        void prgressReport(object sender, ProgressChangedEventArgs e)
        {

            
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;
            con.Open();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("select Material from LX03Automation", con);

                DataSet ds = new DataSet();
                da.Fill(ds, "LX03Automation");

                dbBarcodeValue = new List<string>();
                foreach (DataRow row in ds.Tables["LX03Automation"].Rows)
                {
                    dbBarcodeValue.Add(row["Material"].ToString());

                }
                //get newly printed barcode values into adapter
                List<string> dbBarcodeValue1 = new List<string>();
                try {

                    SqlDataAdapter da1 = new SqlDataAdapter("select Barcode from PartitionBarcodes", con);

                    DataSet ds1 = new DataSet();
                    da1.Fill(ds1, "PartitionBarcodes");


                    foreach (DataRow row in ds1.Tables["PartitionBarcodes"].Rows)
                    {
                        dbBarcodeValue1.Add(row["Barcode"].ToString());

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                string[] lines = System.IO.File.ReadAllLines(@"D:\\Barcode\BarcodeReader.txt");
                string readdata;
                //    Thread.Sleep(1000);

                Console.WriteLine("The first line" + lines[0]);
                readdata = lines[0];

                if (readdata.Length>13)
                {

                  
                    //get barcode value without cone value
                    string barcode = readdata.Substring(1, 13);
                    //string barcode = readdata;
                    Console.WriteLine("barcode" + barcode);
                    //check barcode matched or not with db barcodes
                    if (dbBarcodeValue.Any(str => barcode.Contains(str)) && (barcode.Length == 13))
                    {


                        try
                        {
                            btnBarcodeOneSignal.Content = FindResource("SignalA1");
                            btnBarcodeOneSignalProgress.Content = FindResource("SignalA1New");
                            btnBarcodeOneSignalProgressS.Content = FindResource("SuccessBarcodeOneSignalS");
                            btnBarcodeOneSignalProgressNewS.Content = FindResource("SuccessBarcodeOneSignalNewS");
                            Console.WriteLine("before execute");

                            System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReaderBackup.txt", lines[0]);
                            System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReader.txt", string.Empty);
                            //  AdamControl adamsuccsess = new AdamControl(22);
                         //    adamsuccsess.BulbOn();
                          //    Thread.Sleep(500);
                           //   adamsuccsess.BulbOff();
                          

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception is" + ex);
                        }


                    }
                    //check newly printed barcode values with text file barcode value
                    else if (dbBarcodeValue1.Any(str => barcode.Contains(str)) && (barcode.Length == 13))
                    {
                        try
                        {
                            btnBarcodeOneSignal.Content = FindResource("SignalA1Sec");
                            btnBarcodeOneSignalProgress.Content = FindResource("SignalA1NewBV");
                            btnBarcodeOneSignalProgressS.Content = FindResource("SuccessBarcodeOneSignalSBV");
                            btnBarcodeOneSignalProgressNewS.Content = FindResource("SuccessBarcodeOneSignalNewSBV");
                            System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReaderBackup.txt", lines[0]);
                            System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReader.txt", string.Empty);
                         //    AdamControl adamsuccsess = new AdamControl(22);
                          //   adamsuccsess.BulbOn();
                         //     Thread.Sleep(500);
                         //    adamsuccsess.BulbOff();
                           
                          

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            // System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReaderBackup.txt", lines[0]);
                            //  System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReader.txt", string.Empty);
                        }
                        finally
                        {

                        }
                    }
                    else
                    {
                        try
                        {
                            string dd = DateTime.Now.ToString("yyyy-MM-dd hh:mm tt");

                            //insert error to error table
                            String updatequery = "insert into ErrorDisplay(Device,Barcode,Status,DateTime) values(@device,@barcode,@status,@datetime)";
                            SqlCommand query = new SqlCommand(updatequery, con);

                            query.Connection = con;
                            query.Parameters.AddWithValue("@device", "Barcode Reader 1");

                            query.Parameters.AddWithValue("@barcode", barcode);
                            query.Parameters.AddWithValue("@status", "Barcode doesn't match with system stored barcode");
                            query.Parameters.AddWithValue("@datetime", dd);
                            query.ExecuteNonQuery();
                            btnBarcodeOneSignal.Content = FindResource("SignalA2");
                            btnBarcodeOneSignalProgress.Content = FindResource("SignalA2New");
                            btnBarcodeOneSignalProgressS.Content = FindResource("rejectedSignalBarcodeOneSignalS");
                            btnBarcodeOneSignalProgressNewS.Content = FindResource("rejectedBarcodeOneSignalNewS");
                            Console.WriteLine("passed signal");
                            System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReaderBackup.txt", lines[0]);
                            System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReader.txt", string.Empty);
                              AdamControl ac = new AdamControl(24);
                             ac.BulbOn();
                             Thread.Sleep(500);
                             ac.BulbOff();
                          
                          
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                }
                //barcode can't be read
                else if (readdata.Length < 13)
                {
                    try
                    {
                        string dd = DateTime.Now.ToString("yyyy-MM-dd hh:mm tt");
                        btnBarcodeOneSignal.Content = FindResource("SignalA2Rej");
                        btnBarcodeOneSignalProgress.Content = FindResource("SignalA2NewBV");
                        btnBarcodeOneSignalProgressS.Content = FindResource("rejectedSignalBarcodeOneSignalSBV");
                        btnBarcodeOneSignalProgressNewS.Content = FindResource("rejectedBarcodeOneSignalNewSBV");

                        System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReaderBackup.txt", lines[0]);
                        System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReader.txt", string.Empty);
                        AdamControl adamreject = new AdamControl(24);
                         adamreject.BulbOn();
                         Thread.Sleep(500);
                        adamreject.BulbOff();
                       
                        String updatequery = "insert into ErrorDisplay(Device,Barcode,Status,DateTime) values(@device,@barcode,@status,@datetime)";
                        SqlCommand query = new SqlCommand(updatequery, con);

                        query.Connection = con;
                        query.Parameters.AddWithValue("@device", "Barcode Reader 1");

                        query.Parameters.AddWithValue("@barcode", readdata);
                        query.Parameters.AddWithValue("@status", "Barcode Can't be read");
                        query.Parameters.AddWithValue("@datetime", dd);
                        query.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);

                    }
                }
                //barcode doesn't match with system stored barcode
                                               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

            }            
        }

        void prgressReport2(object sender, ProgressChangedEventArgs e)
        {
            
            try
               {
                   string[] lines = System.IO.File.ReadAllLines(@"D:\\Barcode\BarcodeReaderBackup.txt");
                   string readdata;


                   Console.WriteLine("The first line BarcodeReaderBackup" + lines[0]);
                   readdata = lines[0];
                   
                if (readdata.Length > 13)
                {
                    string barcode = readdata.Substring(1, 13);
                    //string barcode = readdata;
                    Console.WriteLine("BarcodeReaderBackup" + barcode);
                    // get number of cones in a barcode from text file
                    string readCones = readdata.Substring(14, readdata.Length - 14);
                    Console.WriteLine("cones BarcodeReaderBackup" + readCones);
                    //get average weight from relevant material by refering weight details table
                    string first7Digit = readdata.Substring(1, 7);
                    int readNumberOfCones = int.Parse(readCones);

                    //-----------------------------------------------Weight section-------------*************************

                    //   double systemFullBarcodeWeight = readNumberOfCones;
                    //read weight from avery communicator




                    try
                    {
                        string[] readWeight = System.IO.File.ReadAllLines(@"D:\\Weight\weight.txt");
                        string readweightfromavery;

                        //read Box Weight
                        
                            string[] readBoxWeight = System.IO.File.ReadAllLines(@"D:\\Weight\boxWeight.txt");
                            string readboxweightfromadam;
                        

                        if (readWeight != null)
                        {
                            Console.WriteLine("The read line weight" + readWeight[0]);
                            readweightfromavery = readWeight[0];
                            readboxweightfromadam = readBoxWeight[0];
                            //pass weight to double
                            double weightFromAvery = double.Parse(readweightfromavery);
                            double boxWeight = double.Parse(readboxweightfromadam);
                            double actualConeWeight = weightFromAvery - boxWeight;
                            Console.WriteLine("weightFromAvery weight" + weightFromAvery);
                            SqlConnection con = new SqlConnection();
                            con.ConnectionString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;
                            con.Open();
                            string art_tkt = "select AvgWeight from WeightDetails where ART_TKT=@first7Digit";
                            SqlCommand art_tkt_query = new SqlCommand(art_tkt, con);
                            art_tkt_query.Connection = con;

                            art_tkt_query.Parameters.AddWithValue("@first7Digit", first7Digit);
                            art_tkt_query.ExecuteNonQuery();
                            double result = ((double)art_tkt_query.ExecuteScalar());
                            Console.WriteLine("result" + result);
                            //multiply number of cones with average weight
                            double weight = result * readNumberOfCones;
                            Console.WriteLine("weight" + weight);
                            //------------------- condition check
                            if ((weight - 0.1 <= actualConeWeight) && (actualConeWeight <= (weight + 0.1)))
                            {

                                btnWeightSignal.Content = FindResource("sigweigh");
                                btnWeightSignalProgress.Content = FindResource("SignalB1NewA");
                                btnWeightSignalProgressS.Content = FindResource("SuccessWeightSignalSW");
                                btnWeightSignalProgressNewS.Content = FindResource("SuccessWeightSignalNewSW");
                                Console.WriteLine("Weight is------------------" + weight);
                                Console.WriteLine("equal");
                                System.IO.File.WriteAllText(@"D:\\Barcode\\BarcodeReaderBackup.txt", string.Empty);
                                System.IO.File.WriteAllText(@"D:\\Weight\\weight.txt", string.Empty);
                                //  AdamControl ac = new AdamControl(22);
                             //    ac.BulbOn();
                              //  Thread.Sleep(500);
                               //  ac.BulbOff();
                            }
                            else
                            {
                                System.IO.File.WriteAllText(@"D:\\Barcode\\BarcodeReaderBackup.txt", string.Empty);
                                System.IO.File.WriteAllText(@"D:\\Weight\\weight.txt", string.Empty);
                                btnWeightSignal.Content = FindResource("SignalB2");
                                btnWeightSignalProgress.Content = FindResource("SignalB2New");
                                btnWeightSignalProgressS.Content = FindResource("SuccessWeightSignalS");
                                btnWeightSignalProgressNewS.Content = FindResource("SuccessWeightSignalNewS");
                                Console.WriteLine("not equal");

                               
                                AdamControl ac1 = new AdamControl(20);

                                 ac1.BulbOn();
                             Thread.Sleep(500);
                                  ac1.BulbOff();

                          //      AdamControl ac = new AdamControl(23);
                           //     ac.BulbOn();
                           //    Thread.Sleep(500);
                           //      ac.BulbOff();


                                string dd = DateTime.Now.ToString("yyyy-MM-dd hh:mm tt");
                                String updatequery = "insert into ErrorDisplay(Device,Barcode,Status,DateTime) values(@device,@barcode,@status,@datetime)";
                                SqlCommand query = new SqlCommand(updatequery, con);

                                query.Connection = con;
                                query.Parameters.AddWithValue("@device", "Weight Scale");

                                query.Parameters.AddWithValue("@barcode", barcode);
                                query.Parameters.AddWithValue("@status", "Weight is not Equal");
                                query.Parameters.AddWithValue("@datetime", dd);
                                query.ExecuteNonQuery();
                               // MessageBox.Show("Weight is not Equal");

                            }
                            string updatereadweightdate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                            if (barcode.Contains("-"))
                            {
                                string updatereadWeight = "update LX03Automation set ReadWeight=ReadWeight+@IncrementWeight,systemFullBarcodeWeight=systemFullBarcodeWeight+@weight,ReadWeightDate=@readWeightDate where Material=@barcode";

                                SqlCommand updatereadweight_cmd = new SqlCommand(updatereadWeight, con);
                                updatereadweight_cmd.Connection = con;
                                updatereadweight_cmd.Parameters.AddWithValue("@barcode", barcode);
                                updatereadweight_cmd.Parameters.AddWithValue("@IncrementWeight", actualConeWeight);
                                updatereadweight_cmd.Parameters.AddWithValue("@weight", weight);
                                updatereadweight_cmd.Parameters.AddWithValue("@readWeightDate", updatereadweightdate);
                                updatereadweight_cmd.ExecuteNonQuery();
                            }
                            else if (barcode.Contains("."))
                            {
                                string updatereadWeight = "update PartitionBarcodes set ReadWeight=ReadWeight+@IncrementWeight,systemFullBarcodeWeight=systemFullBarcodeWeight+@weight,ReadWeightDate=@readWeightDate where Barcode=@barcode";

                                SqlCommand updatereadweight_cmd = new SqlCommand(updatereadWeight, con);
                                updatereadweight_cmd.Connection = con;
                                updatereadweight_cmd.Parameters.AddWithValue("@barcode", barcode);
                                updatereadweight_cmd.Parameters.AddWithValue("@IncrementWeight", actualConeWeight);
                                updatereadweight_cmd.Parameters.AddWithValue("@weight", weight);
                                updatereadweight_cmd.Parameters.AddWithValue("@readWeightDate", updatereadweightdate);
                                updatereadweight_cmd.ExecuteNonQuery();
                            }

                        }
                        else
                        {
                            System.IO.File.WriteAllText(@"D:\\Barcode\\BarcodeReaderBackup.txt", string.Empty);
                            btnWeightSignal.Content = FindResource("SignalB2A");
                            btnWeightSignalProgress.Content = FindResource("SignalB2NewA");
                            btnWeightSignalProgressS.Content = FindResource("SuccessWeightSignalSA");
                            btnWeightSignalProgressNewS.Content = FindResource("SuccessWeightSignalNewSA");
                         
                           MessageBox.Show("Weight is not corrected");
                             AdamControl ac1 = new AdamControl(20);
                           ac1.BulbOn();
                             Thread.Sleep(500);
                            ac1.BulbOff();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                // weight can't be read, give signals to segression arm and adam signal
                else
                {

                    try
                    {
                        string[] readWeight = System.IO.File.ReadAllLines(@"D:\\Weight\weight.txt");

                        if (readWeight[0] != null)
                        {

                            System.IO.File.WriteAllText(@"D:\\Barcode\\BarcodeReaderBackup.txt", string.Empty);
                            btnWeightSignal.Content = FindResource("SignalB2BA");
                            btnWeightSignalProgress.Content = FindResource("SignalB2NewB");
                            btnWeightSignalProgressS.Content = FindResource("SuccessWeightSignalSB");
                            btnWeightSignalProgressNewS.Content = FindResource("SuccessWeightSignalNewSB");
                           
                      //      MessageBox.Show("Barcode is not corrected");
                             AdamControl ac1 = new AdamControl(20);
                            ac1.BulbOn();
                            Thread.Sleep(500);
                            ac1.BulbOff();

                           // AdamControl ac = new AdamControl(23);
                           //  ac.BulbOn();
                           //  Thread.Sleep(500);
                          //  ac.BulbOff();
                        }
                    }

                    //update error table as a weight issue
                    /*
                    string dd = DateTime.Now.ToString("yyyy-MM-dd hh:mm tt");
                    String updatequery = "insert into ErrorDisplay(Device,Barcode,Status,DateTime) values(@device,@barcode,@status,@datetime)";
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;
                    con.Open();
                    SqlCommand query = new SqlCommand(updatequery, con);

                    query.Connection = con;
                    query.Parameters.AddWithValue("@device", "Weight Scale");

                    query.Parameters.AddWithValue("@barcode", barcode);
                    query.Parameters.AddWithValue("@status", "Weight Can't be Read");
                    query.Parameters.AddWithValue("@status", dd);
                    query.ExecuteNonQuery();*/
                    catch (Exception ex)
                    {

                    }
        }

               }
               catch (Exception ex)
               {
                   Console.WriteLine(ex);
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

        private void btnMenuBar_Click(object sender, RoutedEventArgs e)
        {
            navigationMenu.IsOpen = true;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            GridMainWindowProcessFlow.Visibility = System.Windows.Visibility.Visible;
            GridMainWindowProcessFlowOnly.Visibility = System.Windows.Visibility.Hidden;
            GridBarcodesFlow.Visibility = System.Windows.Visibility.Hidden;
            GridReportsFlow.Visibility = System.Windows.Visibility.Hidden;
            GridUploadDataFlow.Visibility = System.Windows.Visibility.Hidden;
            GridUsersFlow.Visibility = System.Windows.Visibility.Hidden;
            navigationMenu.IsOpen = false;
            TopBarMenuItemText.Text = "HOME";
            btnMenuItemSelectIcon.Content = FindResource("SelectImgHome");

        }



        private void btnProgress_Click(object sender, RoutedEventArgs e)
        {
            GridMainWindowProcessFlowOnly.Visibility = System.Windows.Visibility.Visible;
            GridMainWindowProcessFlow.Visibility = System.Windows.Visibility.Hidden;
            GridBarcodesFlow.Visibility = System.Windows.Visibility.Hidden;
            GridReportsFlow.Visibility = System.Windows.Visibility.Hidden;
            GridUploadDataFlow.Visibility = System.Windows.Visibility.Hidden;
            GridUsersFlow.Visibility = System.Windows.Visibility.Hidden;
            navigationMenu.IsOpen = false;
            TopBarMenuItemText.Text = "PROGRESS";
            btnMenuItemSelectIcon.Content = FindResource("SelectImgProgress");
        }

        private void btnBarcodes_Click(object sender, RoutedEventArgs e)
        {
            GridBarcodesFlow.Visibility = System.Windows.Visibility.Visible;
            GridMainWindowProcessFlowOnly.Visibility = System.Windows.Visibility.Hidden;
            GridMainWindowProcessFlow.Visibility = System.Windows.Visibility.Hidden;
            GridReportsFlow.Visibility = System.Windows.Visibility.Hidden;
            GridUploadDataFlow.Visibility = System.Windows.Visibility.Hidden;
            GridUsersFlow.Visibility = System.Windows.Visibility.Hidden;
            navigationMenu.IsOpen = false;
            TopBarMenuItemText.Text = "BARCODES";
            btnMenuItemSelectIcon.Content = FindResource("SelectImgBarcodes");
        }

        private void btnReports_Click(object sender, RoutedEventArgs e)
        {
            GridReportsFlow.Visibility = System.Windows.Visibility.Visible;
            GridMainWindowProcessFlowOnly.Visibility = System.Windows.Visibility.Hidden;
            GridMainWindowProcessFlow.Visibility = System.Windows.Visibility.Hidden;
            GridBarcodesFlow.Visibility = System.Windows.Visibility.Hidden;
            GridUploadDataFlow.Visibility = System.Windows.Visibility.Hidden;
            GridUsersFlow.Visibility = System.Windows.Visibility.Hidden;
            navigationMenu.IsOpen = false;
            TopBarMenuItemText.Text = "REPORTS";
            btnMenuItemSelectIcon.Content = FindResource("SelectImgReports");
        }

        private void btnUploadData_Click(object sender, RoutedEventArgs e)
        {
            GridUploadDataFlow.Visibility = System.Windows.Visibility.Visible;
            GridMainWindowProcessFlowOnly.Visibility = System.Windows.Visibility.Hidden;
            GridMainWindowProcessFlow.Visibility = System.Windows.Visibility.Hidden;
            GridBarcodesFlow.Visibility = System.Windows.Visibility.Hidden;
            GridReportsFlow.Visibility = System.Windows.Visibility.Hidden;
            GridUsersFlow.Visibility = System.Windows.Visibility.Hidden;
            navigationMenu.IsOpen = false;
            TopBarMenuItemText.Text = "UPLOAD DATA";
            btnMenuItemSelectIcon.Content = FindResource("SelectImgUploadData");
        }

        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            GridUsersFlow.Visibility = System.Windows.Visibility.Visible;
            GridMainWindowProcessFlowOnly.Visibility = System.Windows.Visibility.Hidden;
            GridMainWindowProcessFlow.Visibility = System.Windows.Visibility.Hidden;
            GridBarcodesFlow.Visibility = System.Windows.Visibility.Hidden;
            GridReportsFlow.Visibility = System.Windows.Visibility.Hidden;
            GridUploadDataFlow.Visibility = System.Windows.Visibility.Hidden;
            navigationMenu.IsOpen = false;
            TopBarMenuItemText.Text = "USERS";
            btnMenuItemSelectIcon.Content = FindResource("SelectImgUsers");
        }
        private void AddBarcode_Click(object sender, MouseEventArgs e)
        {
            BarcodeAdd s = new BarcodeAdd();
            s.Show();


        }
        private void UserAdd_Click(object sender, MouseEventArgs e)
        {
            AddNewUser s = new AddNewUser();
            s.Show();
        }
        //admin

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





        private void UsersLoad(object sender, RoutedEventArgs e)
        {
            try
            {
                Console.WriteLine("try call");
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;
                con.Open();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = con;
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "Select empName,userOpteam from Advantic_Users";
                SqlDataAdapter sqlDataAdap = new SqlDataAdapter(sqlCmd);

                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                sqlDataAdap.Fill(dt);


                DataGridUsertlist.ItemsSource = dt.DefaultView;

                sqlDataAdap.Update(dt);
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ex is" + ex);
            }
        }



        void prgressReport_loadErrorTable(object sender, ProgressChangedEventArgs e)
        {
            try
            {

                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;
                con.Open();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = con;
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "Select * from ErrorDisplay order by DateTime DESC";
                SqlDataAdapter sqlDataAdap = new SqlDataAdapter(sqlCmd);

                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                sqlDataAdap.Fill(dt);


                DataGridReportInfo.ItemsSource = dt.DefaultView;
                DataGridErrorListProgress.ItemsSource = dt.DefaultView;
                DataGridReportInfoS.ItemsSource = dt.DefaultView;
                DataGridReportInfoNewS.ItemsSource = dt.DefaultView;
                sqlDataAdap.Update(dt);
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ex is" + ex);
            }
        }
        void workerComplete_loadErrorTable(object sender, RunWorkerCompletedEventArgs e)
        {
            //   System.Windows.MessageBox.Show("upload successfully");

        }
        private void longTaskDowork_loadErrorTable(object sender, DoWorkEventArgs e)
        {
            int i = 0;
            //    for (int i = 0; i < times; i++)
            while (true)

            {
                ++i;
                Thread.Sleep(1000);
                loadErrorTable.ReportProgress(i);
            }
        }



        void prgressReport_pendingList(object sender, ProgressChangedEventArgs e)
        {

            try
            {

                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;
                con.Open();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = con;
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "Select Material from LX03Automation where TotalStock>ReadBarcodeNumberOfCones ";
                SqlDataAdapter sqlDataAdap = new SqlDataAdapter(sqlCmd);

                DataTable dt = new DataTable();

                sqlDataAdap.Fill(dt);


                DataGridBarcodelist.ItemsSource = dt.DefaultView;
                DataGridReportInfoSection.ItemsSource = dt.DefaultView;
                DataGridBarcodelistS.ItemsSource = dt.DefaultView;
                DataGridReportInfoSectionS.ItemsSource = dt.DefaultView;

                sqlDataAdap.Update(dt);
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
             //   System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReaderBackup.txt", string.Empty);
            }

        }

        void workerComplete_pendingList(object sender, RunWorkerCompletedEventArgs e)
        {


        }
        private void longTaskDowork_pendingList(object sender, DoWorkEventArgs e)
        {
            int i = 0;
            //    for (int i = 0; i < times; i++)
            while (true)

            {
                ++i;
                Thread.Sleep(1000);
                pendingList.ReportProgress(i);
            }
        }

        private void DateLoad(object sender, RoutedEventArgs e)
        {
            listDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            ListDate.DataContext = listDate.ToString();
        }

        void prgressReport_loadNewlyPrintedBarcodes(object sender, ProgressChangedEventArgs e)
        {

            try
            {

                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;
                con.Open();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = con;
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "Select empName,DateTime  from PartitionBarcodes order by DateTime DESC";
                SqlDataAdapter sqlDataAdap = new SqlDataAdapter(sqlCmd);

                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                sqlDataAdap.Fill(dt);


                DataGridNewlyPrintedBarcodelist.ItemsSource = dt.DefaultView;
                DataGridNewlyPrintedBarcodelistNew.ItemsSource = dt.DefaultView;
                DataGridNewlyPrintedBarcodelistS.ItemsSource = dt.DefaultView;
              

                sqlDataAdap.Update(dt);
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ex is" + ex);
            }
        }


        void workerComplete_loadNewlyPrintedBarcodes(object sender, RunWorkerCompletedEventArgs e)
        {
            //   System.Windows.MessageBox.Show("upload successfully");

        }
        private void longTaskDowork_loadNewlyPrintedBarcodes(object sender, DoWorkEventArgs e)
        {
            int i = 0;
            //    for (int i = 0; i < times; i++)
            while (true)

            {
                ++i;
                Thread.Sleep(1000);
                loadNewlyPrintedBarcodes.ReportProgress(i);
            }
        }

        //**********************************barcode reader 3 section ************************************************
        void prgressReport_viewLocation(object sender, ProgressChangedEventArgs e)
        {

            try
            {
                //read barocde reader 3 text file
                string[] barcodeForLoaction = System.IO.File.ReadAllLines(@"D:\\Barcode\BarcodeReader3.txt");
                string identifyLocation;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;
                con.Open();

                Console.WriteLine("Barcode Reader 3 :" + barcodeForLoaction[0]);
                identifyLocation = barcodeForLoaction[0];
                if (identifyLocation != null && identifyLocation.Length > 13)
                {
                    // get first 13 digit from barcode
                    string barcodeInfoForLocation = identifyLocation.Substring(1, 13);

                    string barcodeInfo1 = barcodeInfoForLocation.Remove(7, 1);
                    string barcodeInfo = barcodeInfo1.Insert(7, "-");
                    //string barcode = readdata;
                    Console.WriteLine("barcode" + barcodeInfo);

                    //get number of cones from barcode
                    string readCones = identifyLocation.Substring(14, identifyLocation.Length - 14);
                    Console.WriteLine("cones" + readCones);
                    int readNumberOfCones = int.Parse(readCones);
                    string art_tkt = "select ShipToParty from VL06OAutomationForLocation where Material=@identifyLocation AND DeliveryQuantity=@DeliveryQuantity";
                    SqlCommand art_tkt_query = new SqlCommand(art_tkt, con);
                    art_tkt_query.Connection = con;

                    art_tkt_query.Parameters.AddWithValue("@identifyLocation", barcodeInfo);
                    art_tkt_query.Parameters.AddWithValue("@DeliveryQuantity", readNumberOfCones);
                    art_tkt_query.ExecuteNonQuery();

                    //update LX03 Table with Number of Cones

                    string updatequery = "update LX03Automation set ReadBarcodeNumberOfCones=ReadBarcodeNumberOfCones+@IncrementValue where Material=@barcode";
                    SqlCommand query = new SqlCommand(updatequery, con);



                    Console.WriteLine("after execute barcode" + barcodeInfo);
                    query.Parameters.AddWithValue("@barcode", barcodeInfo);

                    query.Parameters.AddWithValue("@IncrementValue", readNumberOfCones);

                    Console.WriteLine("after execute barcode------" + barcodeInfo);
                    query.ExecuteNonQuery();



                    //-----------------------------
                    string shiptoparty = ((string)art_tkt_query.ExecuteScalar());
                    string findlast3digitDelivery = "select Delivery from VL06OAutomationForLocation where Material=@identifyLocation AND DeliveryQuantity=@DeliveryQuantity ";
                    SqlCommand findlast3digit_query = new SqlCommand(findlast3digitDelivery, con);
                    findlast3digit_query.Connection = con;

                    findlast3digit_query.Parameters.AddWithValue("@identifyLocation", barcodeInfo);
                    findlast3digit_query.Parameters.AddWithValue("@DeliveryQuantity", readNumberOfCones);
                    findlast3digit_query.ExecuteNonQuery();
                    string findlast3digitfromDelivery = ((string)findlast3digit_query.ExecuteScalar());
                    string last3digit = findlast3digitfromDelivery.Substring(findlast3digitfromDelivery.Length - 3, 3);
                    // remove row from db that matched with above specified conditions
                    // start trigger to insert another table
                    string deleteLocationQuery = "delete top(1) from VL06OAutomationForLocation where DeliveryQuantity=@DeliveryQuantity AND Material =@material";
                    SqlCommand deleteLocationQuery_cmd = new SqlCommand(deleteLocationQuery, con);
                    deleteLocationQuery_cmd.Connection = con;

                    deleteLocationQuery_cmd.Parameters.AddWithValue("@DeliveryQuantity", readNumberOfCones);
                    Console.WriteLine("readNumberOfCones ----------------------------------" + readNumberOfCones);
                    deleteLocationQuery_cmd.Parameters.AddWithValue("@material", barcodeInfo);
                    deleteLocationQuery_cmd.ExecuteNonQuery();

                    // find Zone in master file
                    string findZone = "select Zone from MasterFile where Customer=@customer";
                    SqlCommand findZone_query = new SqlCommand(findZone, con);
                    findZone_query.Connection = con;

                    findZone_query.Parameters.AddWithValue("@customer", shiptoparty);
                    findZone_query.ExecuteNonQuery();
                    string zone = ((string)findZone_query.ExecuteScalar());
                    string dateTime = DateTime.Now.ToString("yyyy-MM-dd");
                    if (zone != null)
                    {
                        // find path in Location table
                        string findRoute = "select Path from LocationTable where Route=@route";
                        SqlCommand findRoute_query = new SqlCommand(findRoute, con);
                        findRoute_query.Connection = con;

                        findRoute_query.Parameters.AddWithValue("@route", zone);
                        findRoute_query.ExecuteNonQuery();
                        string route = ((string)findRoute_query.ExecuteScalar());



                        DateTime dt = DateTime.Today;
                        string dayOfWeek = dt.DayOfWeek.ToString();
                        string dayOfWeekCharacter = null;
                        if (dayOfWeek == "Monday")
                        {
                            dayOfWeekCharacter = "A";
                        }
                        else if (dayOfWeek == "Tuesday")
                        {
                            dayOfWeekCharacter = "B";
                        }
                        else if (dayOfWeek == "Wednesday")
                        {
                            dayOfWeekCharacter = "C";
                        }
                        else if (dayOfWeek == "Thursday")
                        {
                            dayOfWeekCharacter = "D";

                        }
                        else if (dayOfWeek == "Friday")
                        {
                            dayOfWeekCharacter = "E";
                        }
                        else if (dayOfWeek == "Saturday")
                        {
                            dayOfWeekCharacter = "F";
                        }

                        else if (dayOfWeek == "Sunday")
                        {
                            dayOfWeekCharacter = "G";
                        }
                        string combineRouteDay = route + dayOfWeekCharacter + last3digit;
                        string prefix = "";
                        string suffix = "";
                        string dobiMark = prefix + combineRouteDay + suffix;


                        if (route == "A")
                        {
                            btnBarcodeThreeSignal.Content = FindResource("AAlocationA");
                            btnBarcodeThreeSignalProgress.Content = FindResource("AAlocationAa");
                            btnBarcodeThreeSignalProgressNewS.Content = FindResource("AAlocation");
                            btnPrinterSignalProgressS.Content = FindResource("AAlocationS");
                            Console.WriteLine("Location A");
                            string insertToFinalA = "insert into FinalTable(Material,DeliveryQuantity,Location,DobiMark,DateTime)values(@Material,@DeliveryQuantity,@Location,@DobiMark,@DateTime)";
                            SqlCommand insertToFinal_queryA = new SqlCommand(insertToFinalA, con);
                            insertToFinal_queryA.Connection = con;

                            insertToFinal_queryA.Parameters.AddWithValue("Material", identifyLocation);
                            insertToFinal_queryA.Parameters.AddWithValue("DeliveryQuantity", readCones);
                            insertToFinal_queryA.Parameters.AddWithValue("Location", "A");
                            insertToFinal_queryA.Parameters.AddWithValue("DobiMark", dobiMark);
                            insertToFinal_queryA.Parameters.AddWithValue("DateTime", dateTime);

                            insertToFinal_queryA.ExecuteNonQuery();
                             AdamControl ac = new AdamControl(17);
                          
                              ac.BulbOn();
                              Thread.Sleep(500);
                              ac.BulbOff();

                        }
                        else if (route == "B")
                        {

                            btnBarcodeThreeSignal.Content = FindResource("BBLocationA");
                            btnBarcodeThreeSignalProgress.Content = FindResource("BBLocationAa");
                            btnBarcodeThreeSignalProgressNewS.Content = FindResource("BBLocation");
                            btnPrinterSignalProgressS.Content = FindResource("BBLocationS");
                            Console.WriteLine("Location B");
                            string insertToFinalB = "insert into FinalTable(Material,DeliveryQuantity,Location,DobiMark,DateTime)values(@Material,@DeliveryQuantity,@Location,@DobiMark,@DateTime)";
                            SqlCommand insertToFinal_queryB = new SqlCommand(insertToFinalB, con);
                            insertToFinal_queryB.Connection = con;

                            insertToFinal_queryB.Parameters.AddWithValue("Material", identifyLocation);
                            insertToFinal_queryB.Parameters.AddWithValue("DeliveryQuantity", readCones);
                            insertToFinal_queryB.Parameters.AddWithValue("Location", "B");
                            insertToFinal_queryB.Parameters.AddWithValue("DobiMark", dobiMark);
                            insertToFinal_queryB.Parameters.AddWithValue("DateTime", dateTime);
                            insertToFinal_queryB.ExecuteNonQuery();
                             AdamControl ac = new AdamControl(18);
                            ac.BulbOn();
                            Thread.Sleep(500);
                            ac.BulbOff();
                        }
                        else if (route == "C")
                        {
                            btnBarcodeThreeSignal.Content = FindResource("CCLocationA");
                            btnBarcodeThreeSignalProgress.Content = FindResource("CCLocationAa");
                            btnBarcodeThreeSignalProgressNewS.Content = FindResource("CCLocation");
                            btnPrinterSignalProgressS.Content = FindResource("CCLocationS");
                            Console.WriteLine("Location C");
                            string insertToFinalC = "insert into FinalTable(Material,DeliveryQuantity,Location,DobiMark,DateTime)values(@Material,@DeliveryQuantity,@Location,@DobiMark,@DateTime)";
                            SqlCommand insertToFinal_queryC = new SqlCommand(insertToFinalC, con);
                            insertToFinal_queryC.Connection = con;

                            insertToFinal_queryC.Parameters.AddWithValue("Material", identifyLocation);
                            insertToFinal_queryC.Parameters.AddWithValue("DeliveryQuantity", readCones);
                            insertToFinal_queryC.Parameters.AddWithValue("Location", "C");
                            insertToFinal_queryC.Parameters.AddWithValue("DobiMark", dobiMark);
                            insertToFinal_queryC.Parameters.AddWithValue("DateTime", dateTime);
                            insertToFinal_queryC.ExecuteNonQuery();
                             AdamControl ac = new AdamControl(19);
                            ac.BulbOn();
                            Thread.Sleep(500);
                            ac.BulbOff();
                        }

                        //************************ barcode printer******************************
                        if (route == "A" || route == "B" || route == "C")
                        {
                            System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
                            try
                            {
                                clientSocket.Connect("192.168.1.23", 4500);
                                NetworkStream serverStream = clientSocket.GetStream();
                                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(dobiMark);
                                serverStream.Write(outStream, 0, outStream.Length);

                                byte[] inStream = new byte[10025];
                           
                                serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
                                Console.WriteLine("clientSocket.0ReceiveBufferSize " + clientSocket.ReceiveBufferSize);
                                Console.WriteLine("outStream.Leng0th " + outStream.Length);
                                Console.WriteLine("outStream " + outStream);
                               
                                serverStream.Close();
                                clientSocket.Close();
                                btnPrinterSignal.Content = FindResource("SignalE1");
                                btnPrinterSignalProgress.Content = FindResource("SignalE1New");
                                btnPrinterSignalProgressS.Content = FindResource("SuccessPrinterSignalS");
                                btnPrinterSignalProgressNewS.Content = FindResource("SuccessPrinterSignalNewS");
                                //     clientSocket.Close();
                               
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                               

                                System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReader3.txt", string.Empty);
                                btnPrinterSignal.Content = FindResource("SignalE1A");
                                btnPrinterSignalProgress.Content = FindResource("SignalE1NewA");
                                btnPrinterSignalProgressS.Content = FindResource("SuccessPrinterSignalSA");
                                btnPrinterSignalProgressNewS.Content = FindResource("SuccessPrinterSignalNewSA");
                            }

                            //send invoice number to imaje printer 2

                            System.Net.Sockets.TcpClient clientSocket2 = new System.Net.Sockets.TcpClient();
                            try
                            {
                                string prefix_imaje2 = "";
                                string suffix_imaje2 = "";
                                string invoiceNumber = prefix_imaje2 + findlast3digitfromDelivery + suffix_imaje2;
                                clientSocket2.Connect("192.168.1.24", 4600);
                                NetworkStream serverStream2 = clientSocket2.GetStream();
                                byte[] outStream_findlast3digitDelivery = System.Text.Encoding.ASCII.GetBytes(invoiceNumber);

                                serverStream2.Write(outStream_findlast3digitDelivery, 0, outStream_findlast3digitDelivery.Length);
                                serverStream2.Flush();


                                byte[] inStream1 = new byte[10025];
                                serverStream2.Read(inStream1, 0, (int)clientSocket2.ReceiveBufferSize);
                                clientSocket2.Close();
                                System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReader3.txt", string.Empty);

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);

                            }
                        }
                        
                    }

                    else
                    {
                        btnBarcodeThreeSignal.Content = FindResource("StockLocationA");
                        btnBarcodeThreeSignalProgress.Content = FindResource("StockLocationAa");
                        btnBarcodeThreeSignalProgressNewS.Content = FindResource("StockLocation");
                        btnPrinterSignalProgressS.Content = FindResource("StockLocationS");
                        Console.WriteLine("Location Stock");
                        string insertToFinalStock = "insert into FinalTable(Material,DeliveryQuantity,Location,DobiMark,DateTime)values(@Material,@DeliveryQuantity,@Location,@DobiMark,@DateTime)";
                        SqlCommand insertToFinal_queryStock = new SqlCommand(insertToFinalStock, con);
                        insertToFinal_queryStock.Connection = con;

                        insertToFinal_queryStock.Parameters.AddWithValue("Material", identifyLocation);
                        insertToFinal_queryStock.Parameters.AddWithValue("DeliveryQuantity", readCones);
                        insertToFinal_queryStock.Parameters.AddWithValue("Location", "Stock");
                        insertToFinal_queryStock.Parameters.AddWithValue("DobiMark", null);
                        insertToFinal_queryStock.Parameters.AddWithValue("DateTime", dateTime);
                        insertToFinal_queryStock.ExecuteNonQuery();

                       



                    }
                    //communication with imaje printer for dobi mark print and invoice printer



                   
                }


                else
                {
                    try
                    {
                        string dd = DateTime.Now.ToString("yyyy-MM-dd hh:mm tt");
                        btnBarcodeThreeSignal.Content = FindResource("Signalrejectbarcode3A");
                        btnBarcodeThreeSignalProgress.Content = FindResource("Signalrejectbarcode3B");
                        btnBarcodeThreeSignalProgressNewS.Content = FindResource("Signalrejectbarcode3C");
                        btnPrinterSignalProgressS.Content = FindResource("Signalrejectbarcode3D");
                        AdamControl adamreject = new AdamControl(24);
                        adamreject.BulbOn();
                        Thread.Sleep(500);
                        adamreject.BulbOff();
                        String updatequery = "insert into ErrorDisplay(Device,Barcode,Status,DateTime) values(@device,@barcode,@status,@datetime)";
                        SqlCommand query = new SqlCommand(updatequery, con);

                        query.Connection = con;
                        query.Parameters.AddWithValue("@device", "Barcode Reader 3");

                        query.Parameters.AddWithValue("@barcode", identifyLocation);
                        query.Parameters.AddWithValue("@status", "Barcode Can't be read");
                        query.Parameters.AddWithValue("@datetime", dd);
                        query.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);

                    }
                }
                System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReader3.txt", string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReader3.txt", string.Empty);
            }
            finally
            {

            }
            
        }

        void workerComplete_viewLocation(object sender, RunWorkerCompletedEventArgs e)
        {
            //   System.Windows.MessageBox.Show("upload successfully");

        }
        private void longTaskDowork_viewLocation(object sender, DoWorkEventArgs e)
        {
            int i = 0;
            //    for (int i = 0; i < times; i++)
            while (true)

            {
                ++i;
                Thread.Sleep(1000);
                viewLocation.ReportProgress(i);
            }
        }

        //********************************barcode reader two**********************************
        void prgressReport_breakPartition(object sender, ProgressChangedEventArgs e)
        {
            
            try
            {
                // read barcode value from text file -barcode reader 2
                string[] barcodeForPartition = System.IO.File.ReadAllLines(@"D:\\Barcode\BarcodeReader2.txt");
                string identifyPartition;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;
                con.Open();

                Console.WriteLine("Barcode Reader 2 " + barcodeForPartition[0]);
                identifyPartition = barcodeForPartition[0];

                /// get first 13 digit from barcode reader
                if (identifyPartition.Length > 13)
                {
                    string barcodeInfofirst13digts = identifyPartition.Substring(1, 13);
                    Console.WriteLine("barcodeInfofirst13digts" + barcodeInfofirst13digts);
                    // remove (-/ . ) to compare barcode values in VL06O table
                    string barcodeInfo1 = barcodeInfofirst13digts.Remove(7, 1);
                    Console.WriteLine("barcodeInfo1" + barcodeInfo1);
                    // insert (-) to index 8 beacause VL06O contain only (-) values
                    string barcodeInfo = barcodeInfo1.Insert(7, "-");

                    Console.WriteLine("barcodeInfo" + barcodeInfo);
                    // split cones from barcode
                    string readCones = identifyPartition.Substring(14, identifyPartition.Length - 14);
                    Console.WriteLine("cones" + readCones);
                    // text fle cones  pass to integer 
                    int readNumberOfCones = int.Parse(readCones);
                    List<int> listOfDeliveryQuantity = new List<int>();
                    // get database stored  delivery quantity to list  
                    string art_tkt = "select DeliveryQuantity from VL06OAutomation where Material=@identifyPartition";
                    SqlCommand art_tkt_query = new SqlCommand(art_tkt, con);
                    art_tkt_query.Connection = con;

                    art_tkt_query.Parameters.AddWithValue("@identifyPartition", barcodeInfo);
                    art_tkt_query.ExecuteNonQuery();
                    SqlDataReader dr = art_tkt_query.ExecuteReader();
                    while (dr.Read())
                    {
                        //sort array into decending order
                        listOfDeliveryQuantity.Add(dr.GetInt32(0));
                       

                    }
                    dr.Close();
                    //     int resultNumberOfCones = ((int)art_tkt_query.ExecuteScalar());
                    //     Console.WriteLine("resultNumberOfCones ----------------------------------" + resultNumberOfCones);
                    //check number of cones equal to relevant delivery quantity
                    if (listOfDeliveryQuantity.Contains(readNumberOfCones))
                    {

                        btnBarcodeTwoSignal.Content = FindResource("SignalC1");
                        btnBarcodeTwoSignalProgress.Content = FindResource("SignalC1New");
                        btnBarcodeTwoSignalProgressS.Content = FindResource("SuccessBarcodeTwoSignalS");
                        btnBarcodeTwoSignalProgressNewS.Content = FindResource("SuccessBarcodeTwoSignalNewS");

                        // remove first record that matched with above specified conditions
                        // start trigger to insert that deleted record into another table for find location 
                        string deleteEqualQuery = "delete top(1) from VL06OAutomation where DeliveryQuantity=@DeliveryQuantity AND Material =@material";
                        SqlCommand deleteEqualQuery_cmd = new SqlCommand(deleteEqualQuery, con);
                        deleteEqualQuery_cmd.Connection = con;

                        deleteEqualQuery_cmd.Parameters.AddWithValue("@DeliveryQuantity", readNumberOfCones);
                        Console.WriteLine("readNumberOfCones ----------------------------------" + readNumberOfCones);
                        deleteEqualQuery_cmd.Parameters.AddWithValue("@material", barcodeInfo);
                        deleteEqualQuery_cmd.ExecuteNonQuery();

                        Console.WriteLine("Not Partitioned");
                        System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReader2.txt", string.Empty);
                        System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReader3.txt", barcodeForPartition[0]);
                    }


                    else

                    {
                        try
                        {
                            string empNum = UserNameLogin.Text;
                            btnBarcodeTwoSignal.Content = FindResource("SignalC2");
                            btnBarcodeTwoSignalProgress.Content = FindResource("SignalC2New");
                            btnBarcodeTwoSignalProgressS.Content = FindResource("rejectedSignalBarcodeTwoSignalS");
                            btnBarcodeTwoSignalProgressNewS.Content = FindResource("rejectedBarcodeTwoSignalNewS");
                           
                            AdamControl ac = new AdamControl(21);
                            ac.BulbOn();
                            Thread.Sleep(500);
                            ac.BulbOff();
                            System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReader2.txt", string.Empty);
                          
                            //sort and get  listOfDeliveryQuantity into ddecending order
                            listOfDeliveryQuantity.Sort();
                            listOfDeliveryQuantity.Reverse();
                            //intialize value for remain cones and divided cones
                            List<int> listOfDeliveryQuantity2 = new List<int>();
                            int DividedNumOfCones = 0;
                            int RemainNumOfCones = 0;
                            // find employee name who is printed new barcodes
                            string findempName = "select empName from Advantic_Users where empNo=@empNo ";
                            SqlCommand findempName_cmd = new SqlCommand(findempName, con);

                            findempName_cmd.Parameters.AddWithValue("@empNo", empNum);

                            findempName_cmd.ExecuteNonQuery();

                            string empName = (string)findempName_cmd.ExecuteScalar();
                            //get specified whole delivery quantity details into list 
                            for (int i = 0; i < listOfDeliveryQuantity.Count; i++)
                            {
                                listOfDeliveryQuantity2.Add(listOfDeliveryQuantity[i]);
                            }
                            // foreach (var quantity in listOfDeliveryQuantity)
                            //     {
                            for (int j = 0; j < listOfDeliveryQuantity2.Count; j++)
                            {

                                Console.WriteLine(listOfDeliveryQuantity2[j]);
                                // find delivery quantity list contain less than read number of cones
                                if (listOfDeliveryQuantity2[j] < readNumberOfCones)
                                {
                                    DividedNumOfCones = listOfDeliveryQuantity2[j];
                                    Console.WriteLine("DividedNumOfCones" + DividedNumOfCones);
                                    RemainNumOfCones = readNumberOfCones - DividedNumOfCones;
                                    Console.WriteLine("RemainNumOfCones" + RemainNumOfCones);
                                    string col = identifyPartition.Substring(9, 5);
                                    Console.WriteLine("col" + col);
                                    string art = identifyPartition.Substring(1, 4);
                                    Console.WriteLine("art" + art);
                                    string tkt = identifyPartition.Substring(5, 3);
                                    Console.WriteLine("tkt" + tkt);

                                    string newBarcode = art + tkt + "." + col;
                                    string newBarcode1 = art + tkt + "." + col + DividedNumOfCones.ToString();
                                    string newBarcode2 = art + tkt + "." + col + RemainNumOfCones.ToString();
                                    string dateTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                                    // insert into partition barcode table
                                    string insertDividedPartitionBarcodes = "insert into PartitionBarcodes(Barcode,NumberOfCones,Col,Art,Tkt,ReadWeight,ReadWeightDate,EmpName,DateTime)values(@Barcode,@NumberOfCones,@Col,@Art,@Tkt,@ReadWeight,@ReadWeightDate,@empName,@Datetime)";
                                    SqlCommand insertDividedPartitionBarcodes_query = new SqlCommand(insertDividedPartitionBarcodes, con);
                                    insertDividedPartitionBarcodes_query.Connection = con;

                                    insertDividedPartitionBarcodes_query.Parameters.AddWithValue("Barcode", newBarcode);

                                    insertDividedPartitionBarcodes_query.Parameters.AddWithValue("NumberOfCones", DividedNumOfCones);
                                    insertDividedPartitionBarcodes_query.Parameters.AddWithValue("Col", col);
                                    insertDividedPartitionBarcodes_query.Parameters.AddWithValue("Art", art);
                                    insertDividedPartitionBarcodes_query.Parameters.AddWithValue("Tkt", tkt);
                                    insertDividedPartitionBarcodes_query.Parameters.AddWithValue("ReadWeight", "");
                                    insertDividedPartitionBarcodes_query.Parameters.AddWithValue("ReadWeightDate", "");


                                    insertDividedPartitionBarcodes_query.Parameters.AddWithValue("EmpName", empName);

                                    insertDividedPartitionBarcodes_query.Parameters.AddWithValue("DateTime", dateTime);
                                    insertDividedPartitionBarcodes_query.ExecuteNonQuery();

                                    //remaining barcodes add to db

                                    string insertRemainPartitionBarcodes = "insert into PartitionBarcodes(Barcode,NumberOfCones,Col,Art,Tkt,ReadWeight,ReadWeightDate,EmpName,DateTime)values(@Barcode,@NumberOfCones,@Col,@Art,@Tkt,@ReadWeight,@ReadWeightDate,@empName,@Datetime)";
                                    SqlCommand insertRemainPartitionBarcodes_query = new SqlCommand(insertRemainPartitionBarcodes, con);
                                    insertRemainPartitionBarcodes_query.Connection = con;

                                    insertRemainPartitionBarcodes_query.Parameters.AddWithValue("Barcode", newBarcode);

                                    insertRemainPartitionBarcodes_query.Parameters.AddWithValue("NumberOfCones", RemainNumOfCones);
                                    insertRemainPartitionBarcodes_query.Parameters.AddWithValue("Col", col);
                                    insertRemainPartitionBarcodes_query.Parameters.AddWithValue("Art", art);
                                    insertRemainPartitionBarcodes_query.Parameters.AddWithValue("Tkt", tkt);
                                    insertRemainPartitionBarcodes_query.Parameters.AddWithValue("ReadWeight", "");
                                    insertRemainPartitionBarcodes_query.Parameters.AddWithValue("ReadWeightDate", "");
                                    insertRemainPartitionBarcodes_query.Parameters.AddWithValue("EmpName", empName);
                                    insertRemainPartitionBarcodes_query.Parameters.AddWithValue("DateTime", dateTime);
                                    insertRemainPartitionBarcodes_query.ExecuteNonQuery();
                                    Console.WriteLine("Partitioned");
                                  // print two barcodes for divide number of cones and remain number of cones
                                    System.Windows.Controls.PrintDialog pd = new System.Windows.Controls.PrintDialog();
                                    String commands_correctBarcode = "^XA^CF0,40,40^FO50,100^FDCOL : " + col + "^FS^FO350,100^FDART : " + art + "^FS^FO50,150^FDTKT : " + tkt + "^FS^FO350,150^FD^FS^FO50,200^FD^FS^FO350,150^FDCONE : " + DividedNumOfCones + "^FS^BY2,3^FO30,200^B3N,N,80,N,N^FD" + newBarcode1 + "^FS^XZ";
                                    RawPrinterHelper.SendStringToPrinter("ZDesigner GT800 (ZPL)", commands_correctBarcode);

                                    String commands_remainBarcode = "^XA^CF0,40,40^FO50,100^FDCOL : " + col + "^FS^FO350,100^FDART : " + art + "^FS^FO50,150^FDTKT : " + tkt + "^FS^FO350,150^FD^FS^FO50,200^FD^FS^FO350,150^FDCONE : " + RemainNumOfCones + "^FS^BY2,3^FO30,200^B3N,N,80,N,N^FD" + newBarcode2 + "^FS^XZ";
                                    RawPrinterHelper.SendStringToPrinter("ZDesigner GT800 (ZPL)", commands_remainBarcode);
                                    break;
                                   


                                }

                            }

                            //  }

                            //System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReader2.txt", string.Empty);



                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }

                    }
                }
                else
                {
                    try
                    {
                        string dd = DateTime.Now.ToString("yyyy-MM-dd hh:mm tt");
                        btnBarcodeTwoSignal.Content = FindResource("Signalrejectbarcode2aa");
                        btnBarcodeTwoSignalProgress.Content = FindResource("Signalrejectbarcode2bb");
                        btnBarcodeTwoSignalProgressS.Content = FindResource("Signalrejectbarcode2cc");
                        btnBarcodeTwoSignalProgressNewS.Content = FindResource("Signalrejectbarcode2dd");
                        AdamControl ac = new AdamControl(21);
                        ac.BulbOn();
                        Thread.Sleep(500);
                        ac.BulbOff();

                   //     AdamControl adamreject = new AdamControl(24);
                   //     adamreject.BulbOn();
                    //    Thread.Sleep(500);
                     //   adamreject.BulbOff();

                     
                        String updatequery = "insert into ErrorDisplay(Device,Barcode,Status,DateTime) values(@device,@barcode,@status,@datetime)";
                        SqlCommand query = new SqlCommand(updatequery, con);

                        query.Connection = con;
                        query.Parameters.AddWithValue("@device", "Barcode Reader 2");

                        query.Parameters.AddWithValue("@barcode", identifyPartition);
                        query.Parameters.AddWithValue("@status", "Barcode Can't be read");
                        query.Parameters.AddWithValue("@datetime", dd);
                        query.ExecuteNonQuery();
                        System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReader2.txt", string.Empty);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                System.IO.File.WriteAllText(@"D:\\Barcode\BarcodeReader2.txt", string.Empty);
            }
            
            
                   }

        void workerComplete_breakPartition(object sender, RunWorkerCompletedEventArgs e)
        {
            //   System.Windows.MessageBox.Show("upload successfully");

        }
        private void longTaskDowork_breakPartition(object sender, DoWorkEventArgs e)
        {
            int i = 0;
            //    for (int i = 0; i < times; i++)
            while (true)

            {
                ++i;
                Thread.Sleep(1000);
                breakPartition.ReportProgress(i);
            }
        }








        //-------------- end send signal

        //End of admin backend
        //******************************************************************************************************************
        //root number
        BackgroundWorker workerRN;
        private void UploadFileC_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = "*.csv";
            dlg.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                prgCEx.Visibility = Visibility.Hidden;
                string filename = dlg.FileName.ToString();
                txtFileNameC.Text = filename;
                workerRN = new BackgroundWorker();
                workerRN.WorkerReportsProgress = true;
                workerRN.DoWork += new DoWorkEventHandler(longTaskDoworkerRN);
                workerRN.ProgressChanged += new ProgressChangedEventHandler(prgressReportworkerRN);
                workerRN.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerCompleteworkerRN);
                workerRN.RunWorkerAsync();
                ErrorMsgMsgC.Text = string.Empty;
                ErrorMsgMsgCNew.Text = string.Empty;
                if (txtFileNameC.Text != null)
                {
                    BtnUploadC.IsEnabled = true;
                }
            }
            else
            {
                prgCEx.Visibility = Visibility.Visible;
                txtFileNameC.Text = string.Empty;
                ErrorMsgMsgC.Text = "File upload field";
                SuccessMsgC.Text = string.Empty;
                ErrorMsgMsgCNew.Text = string.Empty;
                if (txtFileNameC.Text != null)
                {
                    BtnUploadC.IsEnabled = false;
                }
            }
        }

        private void workerCompleteworkerRN(object sender, RunWorkerCompletedEventArgs e)
        {
            SuccessMsgC.Text = "File upload successfully";
        }

        private void prgressReportworkerRN(object sender, ProgressChangedEventArgs e)
        {
            prgC.Value = e.ProgressPercentage;
        }

        private void longTaskDoworkerRN(object sender, DoWorkEventArgs e)
        {
            int times = 5;
            for (int i = 0; i < times; i++)
            {
                Thread.Sleep(260);
                workerRN.ReportProgress(i * (260 / times));
            }
        }

        private void BtnCancelC_Click(object sender, RoutedEventArgs e)
        {
            txtFileNameC.Text = string.Empty;
            SuccessMsgC.Text = string.Empty;
            ErrorMsgMsgC.Text = string.Empty;
            ErrorMsgMsgCNew.Text = string.Empty;
            prgCEx.Visibility = Visibility.Visible;
            if (txtFileNameC.Text != null)
            {
                BtnUploadC.IsEnabled = false;
            }
        }

        private void BtnUploadC_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable importDataC = GetDataFromCSVCFile();
                if (importDataC == null) return;
                SaveImportDataCToDatabase(importDataC);
                prgCEx.Visibility = Visibility.Visible;
                txtFileNameC.Text = string.Empty;
                SuccessMsgC.Text = string.Empty;
                ErrorMsgMsgC.Text = string.Empty;
            }
            finally
            {
                if (txtFileNameC.Text != null)
                {
                    BtnUploadC.IsEnabled = false;
                }
            }
        }
        private DataTable GetDataFromCSVCFile()
        {
            DataTable importDataC = new DataTable();
            try
            {
                using (StreamReader srC = new StreamReader(txtFileNameC.Text))
                {
                    string headerC = srC.ReadLine();
                    if (string.IsNullOrEmpty(headerC))
                    {
                        return null;
                    }
                    string[] headerColumnsA = headerC.Split(',');
                    foreach (string headerColumn in headerColumnsA)
                    {
                        importDataC.Columns.Add(headerColumn);
                    }
                    while (!srC.EndOfStream)
                    {
                        string line = srC.ReadLine();
                        if (string.IsNullOrEmpty(line)) continue;
                        string[] fieldsC = line.Split(',');
                        DataRow importtedCRow = importDataC.NewRow();
                        for (int i = 0; i < fieldsC.Count(); i++)
                        {
                            importtedCRow[i] = fieldsC[i];
                        }
                        importDataC.Rows.Add(importtedCRow);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return importDataC;
        }

        private void SaveImportDataCToDatabase(DataTable importDataA)
        {
            string dd = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");
            using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-E6OMA6L;Initial Catalog=advantis;Integrated Security=True;Pooling=False;"))
            {
                con.Open();
                try
                {
                    foreach (DataRow importRow in importDataA.Rows)
                    {
                        SqlCommand cmdRootNumber = new SqlCommand("INSERT INTO PRODDataRootNumber (PRODImportData, StorageType, StorageBin, Material, StockForPutaway, PickQuantity, Batch, StockCategory,Duration, AvailableStock, TotalStock,  EmptyIndicator, StorageBinType    )" +
                                                          "VALUES(@dd,@StorageType, @StorageBin, @Material, @StockForPutaway, @PickQuantity, @Batch, @StockCategory, @Duration , @AvailableStock, @TotalStock,  @EmptyIndicator, @StorageBinType  )", con);
                        cmdRootNumber.Parameters.AddWithValue("@dd", dd);
                        cmdRootNumber.Parameters.AddWithValue("@StorageType", importRow["StorageType"]);
                        cmdRootNumber.Parameters.AddWithValue("@StorageBin", importRow["StorageBin"]);
                        cmdRootNumber.Parameters.AddWithValue("@Material", importRow["Material"]);
                        cmdRootNumber.Parameters.AddWithValue("@StockForPutaway", importRow["StockForPutaway"]);
                        cmdRootNumber.Parameters.AddWithValue("@PickQuantity", importRow["PickQuantity"]);
                        cmdRootNumber.Parameters.AddWithValue("@Batch", importRow["Batch"]);
                        cmdRootNumber.Parameters.AddWithValue("@StockCategory", importRow["StockCategory"]);
                        cmdRootNumber.Parameters.AddWithValue("@Duration", importRow["Duration"]);
                        cmdRootNumber.Parameters.AddWithValue("@AvailableStock", importRow["AvailableStock"]);
                        cmdRootNumber.Parameters.AddWithValue("@TotalStock", importRow["TotalStock"]);
                        cmdRootNumber.Parameters.AddWithValue("@EmptyIndicator", importRow["EmptyIndicator"]);
                        cmdRootNumber.Parameters.AddWithValue("@StorageBinType", importRow["StorageBinType"]);
                        cmdRootNumber.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    ErrorMsgMsgANew.Text = "Upload faild, Please upload correct data file";
                }
                con.Close();
            }
        }





        //****************************************************************************************************************
        //Avg Weight File upload
        BackgroundWorker workerAV;
        private void UploadFileA_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = "*.csv";
            dlg.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                prgAEx.Visibility = Visibility.Hidden;
                string filename = dlg.FileName.ToString();
                txtFileNameA.Text = filename;
                workerAV = new BackgroundWorker();
                workerAV.WorkerReportsProgress = true;
                workerAV.DoWork += new DoWorkEventHandler(longTaskDoworkerAV);
                workerAV.ProgressChanged += new ProgressChangedEventHandler(prgressReportworkerAV);
                workerAV.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerCompleteworkerAV);
                workerAV.RunWorkerAsync();
                ErrorMsgMsgA.Text = string.Empty;
                ErrorMsgMsgANew.Text = string.Empty;
                if (txtFileNameA.Text != null)
                {
                    BtnUploadA.IsEnabled = true;
                }
            }
            else
            {
                prgAEx.Visibility = Visibility.Visible;
                txtFileNameA.Text = string.Empty;
                ErrorMsgMsgA.Text = "File upload faild";
                SuccessMsgA.Text = string.Empty;
                ErrorMsgMsgANew.Text = string.Empty;
                if (txtFileNameA.Text != null)
                {
                    BtnUploadA.IsEnabled = false;
                }
            }
        }

        private void workerCompleteworkerAV(object sender, RunWorkerCompletedEventArgs e)
        {
            SuccessMsgA.Text = "File upload successfully";
        }

        private void prgressReportworkerAV(object sender, ProgressChangedEventArgs e)
        {
            prgA.Value = e.ProgressPercentage;
        }

        private void longTaskDoworkerAV(object sender, DoWorkEventArgs e)
        {
            int times = 5;
            for (int i = 0; i < times; i++)
            {
                Thread.Sleep(260);
                workerAV.ReportProgress(i * (260 / times));
            }
        }

        private void BtnCancelA_Click(object sender, RoutedEventArgs e)
        {
            txtFileNameA.Text = string.Empty;
            SuccessMsgA.Text = string.Empty;
            ErrorMsgMsgA.Text = string.Empty;
            ErrorMsgMsgANew.Text = string.Empty;
            prgAEx.Visibility = Visibility.Visible;
            if (txtFileNameA.Text != null)
            {
                BtnUploadA.IsEnabled = false;
            }
        }
        private void BtnUploadA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable importDataA = GetDataFromCSVAFile();
                if (importDataA == null) return;
                SaveImportDataAToDatabase(importDataA);
                prgAEx.Visibility = Visibility.Visible;
                txtFileNameA.Text = string.Empty;
                SuccessMsgA.Text = string.Empty;
                ErrorMsgMsgA.Text = string.Empty;
            }
            finally
            {
                if (txtFileNameA.Text != null)
                {
                    BtnUploadA.IsEnabled = false;
                }
            }
        }
        private DataTable GetDataFromCSVAFile()
        {
            DataTable importDataA = new DataTable();
            try
            {
                using (StreamReader srA = new StreamReader(txtFileNameA.Text))
                {
                    string headerA = srA.ReadLine();
                    if (string.IsNullOrEmpty(headerA))
                    {
                        return null;
                    }
                    string[] headerColumnsA = headerA.Split(',');
                    foreach (string headerColumn in headerColumnsA)
                    {
                        importDataA.Columns.Add(headerColumn);
                    }
                    while (!srA.EndOfStream)
                    {
                        string line = srA.ReadLine();
                        if (string.IsNullOrEmpty(line)) continue;
                        string[] fieldsA = line.Split(',');
                        DataRow importtedARow = importDataA.NewRow();
                        for (int i = 0; i < fieldsA.Count(); i++)
                        {
                            importtedARow[i] = fieldsA[i];
                        }
                        importDataA.Rows.Add(importtedARow);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return importDataA;
        }

        private void SaveImportDataAToDatabase(DataTable importDataA)
        {
            string dd = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");
            using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-E6OMA6L;Initial Catalog=advantis;Integrated Security=True;Pooling=False;"))
            {
                con.Open();
                try
                {
                    foreach (DataRow importRow in importDataA.Rows)
                    {
                        /* SqlCommand cmdAvgCWgh = new SqlCommand("INSERT INTO WeightDetails(WeightDetailsImportData, ART_TKT, AvgWeight )" +
                                                         "VALUES(@dd, @ART_TKT , @AvgWeight )", con);
                         cmdAvgCWgh.Parameters.AddWithValue("@dd", dd);
                         cmdAvgCWgh.Parameters.AddWithValue("@ART_TKT", importRow["ART_TKT"]);
                         cmdAvgCWgh.Parameters.AddWithValue("@AvgWeight", importRow["AvgWeight"]);
                         cmdAvgCWgh.ExecuteNonQuery();*/

                        SqlCommand cmdAvgCWgh = new SqlCommand("INSERT INTO WeightDetails(WeightDetailsImportData,  ART_TKT, AvgWeight )" +
                                                        "VALUES(@dd,@ART_TKT , @AvgWeight )", con);
                        cmdAvgCWgh.Parameters.AddWithValue("@dd", dd);
                        cmdAvgCWgh.Parameters.AddWithValue("@ART_TKT", importRow["ART_TKT"]);
                        cmdAvgCWgh.Parameters.AddWithValue("@AvgWeight ", importRow["AvgWeight "]);
                        cmdAvgCWgh.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    ErrorMsgMsgANew.Text = "Upload faild, Please upload correct data file";
                }
                con.Close();
            }
        }

        //*****************************************************************end dl worker




        //*************************************************************************************end avgweight
        //*******************************************************************************************************
        //Dilever Location
        BackgroundWorker workerDL;
        private void UploadFileB_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = "*.csv";
            dlg.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                prgBEx.Visibility = Visibility.Hidden;
                string filename = dlg.FileName.ToString();
                txtFileNameB.Text = filename;
                workerDL = new BackgroundWorker();
                workerDL.WorkerReportsProgress = true;
                workerDL.DoWork += new DoWorkEventHandler(longTaskDoworkerDL);
                workerDL.ProgressChanged += new ProgressChangedEventHandler(prgressReportworkerDL);
                workerDL.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerCompleteworkerDL);
                workerDL.RunWorkerAsync();
                ErrorMsgMsgB.Text = string.Empty;
                ErrorMsgMsgBNew.Text = string.Empty;
                if (txtFileNameB.Text != null)
                {
                    BtnUploadB.IsEnabled = true;
                }
            }
            else
            {
                prgBEx.Visibility = Visibility.Visible;
                txtFileNameB.Text = string.Empty;
                ErrorMsgMsgB.Text = "File upload field";
                SuccessMsgB.Text = string.Empty;
                ErrorMsgMsgBNew.Text = string.Empty;
                if (txtFileNameB.Text != null)
                {
                    BtnUploadB.IsEnabled = false;
                }
            }
        }

        private void workerCompleteworkerDL(object sender, RunWorkerCompletedEventArgs e)
        {
            SuccessMsgB.Text = "File upload successfully";
        }

        private void prgressReportworkerDL(object sender, ProgressChangedEventArgs e)
        {
            prgB.Value = e.ProgressPercentage;
        }

        private void longTaskDoworkerDL(object sender, DoWorkEventArgs e)
        {

            int times = 5;
            for (int i = 0; i < times; i++)
            {
                Thread.Sleep(260);
                workerDL.ReportProgress(i * (260 / times));
            }
        }

        private void BtnCancelB_Click(object sender, RoutedEventArgs e)
        {
            txtFileNameB.Text = string.Empty;
            SuccessMsgB.Text = string.Empty;
            ErrorMsgMsgB.Text = string.Empty;
            ErrorMsgMsgBNew.Text = string.Empty;
            prgBEx.Visibility = Visibility.Visible;
            if (txtFileNameB.Text != null)
            {
                BtnUploadB.IsEnabled = false;
            }

        }

        private void BtnUploadB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable importDataB = GetDataFromCSVBFile();
                if (importDataB == null) return;
                SaveImportDataBToDatabase(importDataB);
                prgBEx.Visibility = Visibility.Visible;
                txtFileNameB.Text = string.Empty;
                SuccessMsgB.Text = string.Empty;
                ErrorMsgMsgB.Text = string.Empty;
            }
            finally
            {
                if (txtFileNameB.Text != null)
                {
                    BtnUploadB.IsEnabled = false;
                }
            }
        }
        private DataTable GetDataFromCSVBFile()
        {
            DataTable importDataB = new DataTable();
            try
            {
                using (StreamReader srB = new StreamReader(txtFileNameB.Text))
                {
                    string headerB = srB.ReadLine();
                    if (string.IsNullOrEmpty(headerB))
                    {
                        return null;
                    }
                    string[] headerColumnsB = headerB.Split(',');
                    foreach (string headerColumn in headerColumnsB)
                    {
                        importDataB.Columns.Add(headerColumn);
                    }
                    while (!srB.EndOfStream)
                    {
                        string line = srB.ReadLine();
                        if (string.IsNullOrEmpty(line)) continue;
                        string[] fieldsB = line.Split(',');
                        DataRow importtedBRow = importDataB.NewRow();
                        for (int i = 0; i < fieldsB.Count(); i++)
                        {
                            importtedBRow[i] = fieldsB[i];
                        }
                        importDataB.Rows.Add(importtedBRow);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return importDataB;
        }

        private void SaveImportDataBToDatabase(DataTable importDataB)
        {
            string dd = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");
            using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-E6OMA6L;Initial Catalog=advantis;Integrated Security=True;Pooling=False;"))
            {
                con.Open();
                try
                {
                    foreach (DataRow importRow in importDataB.Rows)
                    {
                        /* SqlCommand cmd = new SqlCommand("INSERT INTO LocationTable(Route,Path)" +
                                                         "VALUES(@Route,@Path )", con);

                         cmd.Parameters.AddWithValue("@Route", importRow["Route"]);
                         cmd.Parameters.AddWithValue("@Path", importRow["Path"]);
                         cmd.ExecuteNonQuery();*/

                        /*
                                                SqlCommand cmd = new SqlCommand("INSERT INTO LocationTable(Route, Path)" +
                                                                               "VALUES(@Route, @Path)", con);

                                                cmd.Parameters.AddWithValue("@Route", importRow["Route"]);
                                                cmd.Parameters.AddWithValue("@Path", importRow["Path"]);
                                                cmd.ExecuteNonQuery();*/


                        SqlCommand cmdAvg = new SqlCommand("INSERT INTO LocationTable(ImportData,  Route, Path )" +
                                                          "VALUES(@dd,@Route , @Path )", con);
                        cmdAvg.Parameters.AddWithValue("@dd", dd);
                        cmdAvg.Parameters.AddWithValue("@Route", importRow["Route"]);
                        cmdAvg.Parameters.AddWithValue("@Path", importRow["Path"]);
                        cmdAvg.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    ErrorMsgMsgBNew.Text = "Upload faild, Please upload correct data file";
                }
                con.Close();
            }
        }

        //*****************************************************************end dl worker



        //******************************************************************************************************
        //LX03-Automation (Receiving) 1
        BackgroundWorker workerLX03;
        private void UploadFile1_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = "*.csv";
            dlg.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                prg1Ex.Visibility = Visibility.Hidden;
                string filename = dlg.FileName.ToString();
                txtFileName1.Text = filename;
                workerLX03 = new BackgroundWorker();
                workerLX03.WorkerReportsProgress = true;
                workerLX03.DoWork += new DoWorkEventHandler(longTaskDoworkLX03);
                workerLX03.ProgressChanged += new ProgressChangedEventHandler(prgressReportLX03);
                workerLX03.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerCompleteLX03);
                workerLX03.RunWorkerAsync();
                ErrorMsgMsg1.Text = string.Empty;
                ErrorMsgMsg1New.Text = string.Empty;
                if (txtFileName1.Text != null)
                {
                    BtnUpload1.IsEnabled = true;
                }
            }
            else
            {
                prg1Ex.Visibility = Visibility.Visible;
                txtFileName1.Text = string.Empty;
                ErrorMsgMsg1.Text = "File upload faild";
                SuccessMsg1.Text = string.Empty;
                ErrorMsgMsg1New.Text = string.Empty;
                if (txtFileName1.Text != null)
                {
                    BtnUpload1.IsEnabled = false;
                }
            }
        }
        private void BtnCancel1_Click(object sender, RoutedEventArgs e)
        {
            txtFileName1.Text = string.Empty;
            SuccessMsg1.Text = string.Empty;
            ErrorMsgMsg1.Text = string.Empty;
            ErrorMsgMsg1New.Text = string.Empty;
            prg1Ex.Visibility = Visibility.Visible;
            if (txtFileName1.Text != null)
            {
                BtnUpload1.IsEnabled = false;
            }
        }

        void prgressReportLX03(object sender, ProgressChangedEventArgs e)
        {
            prg1.Value = e.ProgressPercentage;
        }
        void workerCompleteLX03(object sender, RunWorkerCompletedEventArgs e)
        {
            SuccessMsg1.Text = "File upload successfully";
        }
        private void longTaskDoworkLX03(object sender, DoWorkEventArgs e)
        {
            int times = 5;
            for (int i = 0; i < times; i++)
            {
                Thread.Sleep(260);
                workerLX03.ReportProgress(i * (260 / times));
            }
        }
        private void BtnUpload1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable importData1 = GetDataFromCSV1File();
                if (importData1 == null) return;
                SaveImportData1ToDatabase(importData1);
                prg1Ex.Visibility = Visibility.Visible;
                txtFileName1.Text = string.Empty;
                SuccessMsg1.Text = string.Empty;
                ErrorMsgMsg1.Text = string.Empty;
            }
            finally
            {
                if (txtFileName1.Text != null)
                {
                    BtnUpload1.IsEnabled = false;
                }
            }
        }
        private DataTable GetDataFromCSV1File()
        {
            DataTable importData1 = new DataTable();
            try
            {
                using (StreamReader sr1 = new StreamReader(txtFileName1.Text))
                {
                    string header1 = sr1.ReadLine();
                    if (string.IsNullOrEmpty(header1))
                    {
                        return null;
                    }
                    string[] headerColumns1 = header1.Split(',');
                    foreach (string headerColumn in headerColumns1)
                    {
                        importData1.Columns.Add(headerColumn);
                    }
                    while (!sr1.EndOfStream)
                    {
                        string line = sr1.ReadLine();
                        if (string.IsNullOrEmpty(line)) continue;
                        string[] fields1 = line.Split(',');
                        DataRow importted1Row = importData1.NewRow();
                        for (int i = 0; i < fields1.Count(); i++)
                        {
                            importted1Row[i] = fields1[i];
                        }
                        importData1.Rows.Add(importted1Row);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return importData1;
        }

        private void SaveImportData1ToDatabase(DataTable importData1)
        {
            string dd = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");
            using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-E6OMA6L;Initial Catalog=advantis;Integrated Security=True;Pooling=False;"))
            {
                con.Open();
                try
                {
                    foreach (DataRow importRow in importData1.Rows)
                    {
                        SqlCommand cmd = new SqlCommand("INSERT INTO LX03Automation(LX03ImportData,Material, PickQuantity, AvailableStock, TotalStock, StorageBin, StorageType)" +
                                                        "VALUES(@dd,@Material, @PickQuantity, @AvailableStock, @TotalStock, @StorageBin, @StorageType)", con);
                        cmd.Parameters.AddWithValue("@dd", dd);
                        cmd.Parameters.AddWithValue("@Material", importRow["Material"]);
                        cmd.Parameters.AddWithValue("@PickQuantity", importRow["PickQuantity"]);
                        cmd.Parameters.AddWithValue("@AvailableStock", importRow["AvailableStock"]);
                        cmd.Parameters.AddWithValue("@TotalStock", importRow["TotalStock"]);
                        cmd.Parameters.AddWithValue("@StorageBin", importRow["StorageBin"]);
                        cmd.Parameters.AddWithValue("@StorageType", importRow["StorageType"]);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    ErrorMsgMsg1New.Text = "Upload faild, Please upload correct data file";
                }
                con.Close();
            }
        }
        //******************************************************************************************************
        //******************************************************************************************************
        //VL06O-Automation (using for creating Dobi Mark) 2
        BackgroundWorker workerVL06O;
        private void UploadFile2_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = "*.csv";
            dlg.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                prg2Ex.Visibility = Visibility.Hidden;
                string filename = dlg.FileName.ToString();
                txtFileName2.Text = filename;
                workerVL06O = new BackgroundWorker();
                workerVL06O.WorkerReportsProgress = true;
                workerVL06O.DoWork += new DoWorkEventHandler(longTaskDoworkVL06O);
                workerVL06O.ProgressChanged += new ProgressChangedEventHandler(prgressReportVL06O);
                workerVL06O.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerCompleteVL06O);
                workerVL06O.RunWorkerAsync();
                ErrorMsgMsg2.Text = string.Empty;
                ErrorMsgMsg2New.Text = string.Empty;
                if (txtFileName2.Text != null)
                {
                    BtnUpload2.IsEnabled = true;
                }
            }
            else
            {
                prg2Ex.Visibility = Visibility.Visible;
                txtFileName2.Text = string.Empty;
                ErrorMsgMsg2.Text = "File upload field";
                SuccessMsg2.Text = string.Empty;
                ErrorMsgMsg2New.Text = string.Empty;
                if (txtFileName2.Text != null)
                {
                    BtnUpload2.IsEnabled = false;
                }
            }
        }

        private void BtnCancel2_Click(object sender, RoutedEventArgs e)
        {
            txtFileName1.Text = string.Empty;
            SuccessMsg2.Text = string.Empty;
            ErrorMsgMsg2.Text = string.Empty;
            ErrorMsgMsg2New.Text = string.Empty;
            prg2Ex.Visibility = Visibility.Visible;
            if (txtFileName2.Text != null)
            {
                BtnUpload2.IsEnabled = false;
            }
        }

        private void workerCompleteVL06O(object sender, RunWorkerCompletedEventArgs e)
        {
            SuccessMsg2.Text = "File upload successfully";
        }

        private void prgressReportVL06O(object sender, ProgressChangedEventArgs e)
        {
            prg2.Value = e.ProgressPercentage;
        }

        private void longTaskDoworkVL06O(object sender, DoWorkEventArgs e)
        {
            int times = 5;
            for (int i = 0; i < times; i++)
            {
                Thread.Sleep(260);
                workerVL06O.ReportProgress(i * (260 / times));
            }
        }

        private void BtnUpload2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable importData2 = GetDataFromCSV2File();
                if (importData2 == null) return;
                SaveImportData2ToDatabase(importData2);
                prg2Ex.Visibility = Visibility.Visible;
                txtFileName2.Text = string.Empty;
                SuccessMsg2.Text = string.Empty;
                ErrorMsgMsg2.Text = string.Empty;
            }
            finally
            {
                if (txtFileName2.Text != null)
                {
                    BtnUpload2.IsEnabled = false;
                }
            }
        }

        private DataTable GetDataFromCSV2File()
        {
            DataTable importData2 = new DataTable();
            try
            {
                using (StreamReader sr2 = new StreamReader(txtFileName2.Text))
                {
                    string header2 = sr2.ReadLine();
                    if (string.IsNullOrEmpty(header2))
                    {
                        return null;
                    }
                    string[] headerColumns2 = header2.Split(',');
                    foreach (string headerColumn in headerColumns2)
                    {
                        importData2.Columns.Add(headerColumn);
                    }
                    while (!sr2.EndOfStream)
                    {
                        string line = sr2.ReadLine();
                        if (string.IsNullOrEmpty(line)) continue;
                        string[] fields2 = line.Split(',');
                        DataRow importted2Row = importData2.NewRow();
                        for (int i = 0; i < fields2.Count(); i++)
                        {
                            importted2Row[i] = fields2[i];
                        }
                        importData2.Rows.Add(importted2Row);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return importData2;
        }

        private void SaveImportData2ToDatabase(DataTable importData2)
        {
            string dd = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");
            using (SqlConnection con2 = new SqlConnection(@"Data Source=DESKTOP-E6OMA6L;Initial Catalog=advantis;Integrated Security=True;Pooling=False;"))
            {
                con2.Open();
                try
                {
                    foreach (DataRow importRow in importData2.Rows)
                    {
                        SqlCommand cmd2 = new SqlCommand("INSERT INTO VL06OAutomation(Delivery, ShipToParty, Material, DeliveryQuantity, DateTime)" +
                                                        "VALUES(@Delivery, @ShipToParty, @Material, @DeliveryQuantity, @dd)", con2);
                        
                        cmd2.Parameters.AddWithValue("@Delivery", importRow["Delivery"]);
                        cmd2.Parameters.AddWithValue("@ShipToParty", importRow["ShipToParty"]);
                        cmd2.Parameters.AddWithValue("@Material", importRow["Material"]);
                        cmd2.Parameters.AddWithValue("@DeliveryQuantity", importRow["DeliveryQuantity"]);
                        cmd2.Parameters.AddWithValue("@dd", dd);
                        cmd2.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    ErrorMsgMsg2New.Text = "Upload faild, Please upload correct data file";
                }
                con2.Close();
            }
        }
        //******************************************************************************************************
        //******************************************************************************************************
        //PutawayLT23-Automation 3
        BackgroundWorker workerPutawayLT23;
        private void UploadFile3_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = "*.csv";
            dlg.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                prg3Ex.Visibility = Visibility.Hidden;
                string filename = dlg.FileName.ToString();
                txtFileName3.Text = filename;
                workerPutawayLT23 = new BackgroundWorker();
                workerPutawayLT23.WorkerReportsProgress = true;
                workerPutawayLT23.DoWork += new DoWorkEventHandler(longTaskDoworkPutawayLT23);
                workerPutawayLT23.ProgressChanged += new ProgressChangedEventHandler(prgressReportPutawayLT23);
                workerPutawayLT23.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerCompletePutawayLT23);
                workerPutawayLT23.RunWorkerAsync();
                ErrorMsgMsg3.Text = string.Empty;
                ErrorMsgMsg3New.Text = string.Empty;
                if (txtFileName3.Text != null)
                {
                    BtnUpload3.IsEnabled = true;
                }
            }
            else
            {
                prg3Ex.Visibility = Visibility.Visible;
                txtFileName3.Text = string.Empty;
                ErrorMsgMsg3.Text = "File upload field";
                SuccessMsg3.Text = string.Empty;
                ErrorMsgMsg3New.Text = string.Empty;
                if (txtFileName3.Text != null)
                {
                    BtnUpload3.IsEnabled = false;
                }
            }
        }

        private void BtnCancel3_Click(object sender, RoutedEventArgs e)
        {
            txtFileName3.Text = string.Empty;
            SuccessMsg3.Text = string.Empty;
            ErrorMsgMsg3.Text = string.Empty;
            ErrorMsgMsg3New.Text = string.Empty;
            prg3Ex.Visibility = Visibility.Visible;
            if (txtFileName3.Text != null)
            {
                BtnUpload3.IsEnabled = false;
            }
        }

        private void workerCompletePutawayLT23(object sender, RunWorkerCompletedEventArgs e)
        {
            SuccessMsg3.Text = "File upload successfully";
        }

        private void prgressReportPutawayLT23(object sender, ProgressChangedEventArgs e)
        {
            prg3.Value = e.ProgressPercentage;
        }

        private void longTaskDoworkPutawayLT23(object sender, DoWorkEventArgs e)
        {
            int times = 5;
            for (int i = 0; i < times; i++)
            {
                Thread.Sleep(260);
                workerPutawayLT23.ReportProgress(i * (260 / times));
            }
        }


        private void BtnUpload3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable importData3 = GetDataFromCSV3File();
                if (importData3 == null) return;
                SaveImportData3ToDatabase(importData3);
                prg3Ex.Visibility = Visibility.Visible;
                txtFileName3.Text = string.Empty;
                SuccessMsg3.Text = string.Empty;
                ErrorMsgMsg3.Text = string.Empty;
            }
            finally
            {
                if (txtFileName3.Text != null)
                {
                    BtnUpload3.IsEnabled = false;
                }
            }
        }



        private DataTable GetDataFromCSV3File()
        {
            DataTable importData3 = new DataTable();
            try
            {
                using (StreamReader sr3 = new StreamReader(txtFileName3.Text))
                {
                    string header3 = sr3.ReadLine();
                    if (string.IsNullOrEmpty(header3))
                    {
                        return null;
                    }
                    string[] headerColumns3 = header3.Split(',');
                    foreach (string headerColumn in headerColumns3)
                    {
                        importData3.Columns.Add(headerColumn);
                    }
                    while (!sr3.EndOfStream)
                    {
                        string line = sr3.ReadLine();
                        if (string.IsNullOrEmpty(line)) continue;
                        string[] fields3 = line.Split(',');
                        DataRow importted3Row = importData3.NewRow();
                        for (int i = 0; i < fields3.Count(); i++)
                        {
                            importted3Row[i] = fields3[i];
                        }
                        importData3.Rows.Add(importted3Row);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return importData3;
        }

        private void SaveImportData3ToDatabase(DataTable importData3)
        {
            string dd = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");
            using (SqlConnection con3 = new SqlConnection(@"Data Source=DESKTOP-E6OMA6L;Initial Catalog=advantis;Integrated Security=True;Pooling=False;"))
            {
                con3.Open();
                try
                {
                    foreach (DataRow importRow in importData3.Rows)
                    {
                        SqlCommand cmd3 = new SqlCommand("INSERT INTO PutawayLT23Automation(LT23PutawayImportData, Material, SourceStorageBin, DestStorageType, DestStorageBin, Desttargetquantity )" +
                                                        "VALUES(@dd,@Material, @SourceStorageBin, @DestStorageType, @DestStorageBin, @Desttargetquantity)", con3);
                        cmd3.Parameters.AddWithValue("@dd", dd);
                        cmd3.Parameters.AddWithValue("@Material", importRow["Material"]);
                        cmd3.Parameters.AddWithValue("@SourceStorageBin", importRow["SourceStorageBin"]);
                        cmd3.Parameters.AddWithValue("@DestStorageType", importRow["DestStorageType"]);
                        cmd3.Parameters.AddWithValue("@DestStorageBin", importRow["DestStorageBin"]);
                        cmd3.Parameters.AddWithValue("@Desttargetquantity", importRow["Desttargetquantity"]);
                        cmd3.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    ErrorMsgMsg3New.Text = "Upload faild, Please upload correct data file";
                }
                con3.Close();
            }
        }
        //******************************************************************************************************
        //******************************************************************************************************
        //RackLT23-Automation  4
        BackgroundWorker workerRackLT23;
        private void UploadFile4_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = "*.csv";
            dlg.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                prg4Ex.Visibility = Visibility.Hidden;
                string filename = dlg.FileName.ToString();
                txtFileName4.Text = filename;
                workerRackLT23 = new BackgroundWorker();
                workerRackLT23.WorkerReportsProgress = true;
                workerRackLT23.DoWork += new DoWorkEventHandler(longTaskDoworkRackLT23);
                workerRackLT23.ProgressChanged += new ProgressChangedEventHandler(prgressReportRackLT23);
                workerRackLT23.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerCompleteRackLT23);
                workerRackLT23.RunWorkerAsync();
                ErrorMsgMsg4.Text = string.Empty;
                ErrorMsgMsg4New.Text = string.Empty;
                if (txtFileName4.Text != null)
                {
                    BtnUpload4.IsEnabled = true;
                }
            }
            else
            {
                prg4Ex.Visibility = Visibility.Visible;
                txtFileName4.Text = string.Empty;
                ErrorMsgMsg4.Text = "File upload field";
                SuccessMsg4.Text = string.Empty;
                ErrorMsgMsg4New.Text = string.Empty;
                if (txtFileName4.Text != null)
                {
                    BtnUpload4.IsEnabled = false;
                }
            }
        }

        private void BtnCancel4_Click(object sender, RoutedEventArgs e)
        {
            txtFileName4.Text = string.Empty;
            SuccessMsg4.Text = string.Empty;
            ErrorMsgMsg4.Text = string.Empty;
            ErrorMsgMsg4New.Text = string.Empty;
            prg4Ex.Visibility = Visibility.Visible;
            if (txtFileName4.Text != null)
            {
                BtnUpload4.IsEnabled = false;
            }
        }

        private void workerCompleteRackLT23(object sender, RunWorkerCompletedEventArgs e)
        {
            SuccessMsg4.Text = "File upload successfully";
        }

        private void prgressReportRackLT23(object sender, ProgressChangedEventArgs e)
        {
            prg4.Value = e.ProgressPercentage;
        }

        private void longTaskDoworkRackLT23(object sender, DoWorkEventArgs e)
        {
            int times = 5;
            for (int i = 0; i < times; i++)
            {
                Thread.Sleep(260);
                workerRackLT23.ReportProgress(i * (260 / times));
            }
        }
        private void BtnUpload4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable importData4 = GetDataFromCSV4File();
                if (importData4 == null) return;
                SaveImportData4ToDatabase(importData4);
                prg4Ex.Visibility = Visibility.Visible;
                txtFileName4.Text = string.Empty;
                SuccessMsg4.Text = string.Empty;
                ErrorMsgMsg4.Text = string.Empty;
            }
            finally
            {
                if (txtFileName4.Text != null)
                {
                    BtnUpload4.IsEnabled = false;
                }
            }
        }



        private DataTable GetDataFromCSV4File()
        {
            DataTable importData4 = new DataTable();
            try
            {
                using (StreamReader sr4 = new StreamReader(txtFileName4.Text))
                {
                    string header4 = sr4.ReadLine();
                    if (string.IsNullOrEmpty(header4))
                    {
                        return null;
                    }
                    string[] headerColumns4 = header4.Split(',');
                    foreach (string headerColumn in headerColumns4)
                    {
                        importData4.Columns.Add(headerColumn);
                    }
                    while (!sr4.EndOfStream)
                    {
                        string line = sr4.ReadLine();
                        if (string.IsNullOrEmpty(line)) continue;
                        string[] fields4 = line.Split(',');
                        DataRow importted4Row = importData4.NewRow();
                        for (int i = 0; i < fields4.Count(); i++)
                        {
                            importted4Row[i] = fields4[i];
                        }
                        importData4.Rows.Add(importted4Row);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return importData4;
        }

        private void SaveImportData4ToDatabase(DataTable importData4)
        {
            string dd = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");
            using (SqlConnection con4 = new SqlConnection(@"Data Source=DESKTOP-E6OMA6L;Initial Catalog=advantis;Integrated Security=True;Pooling=False;"))
            {
                con4.Open();
                try
                {
                    foreach (DataRow importRow in importData4.Rows)
                    {
                        SqlCommand cmd4 = new SqlCommand("INSERT INTO RackLT23Automation(LT23RackImportData ,Material, SourceTargetQty1, SourceTargetQty2, DestTargetQuantity, SourceStorageBin, SourceStorageType )" +
                                                        "VALUES(@dd,@Material, @SourceTargetQty1, @SourceTargetQty2, @DestTargetQuantity, @SourceStorageBin , @SourceStorageType )", con4);
                        cmd4.Parameters.AddWithValue("@dd", dd);
                        cmd4.Parameters.AddWithValue("@Material ", importRow["Material"]);
                        cmd4.Parameters.AddWithValue("@SourceTargetQty1", importRow["SourceTargetQty1"]);
                        cmd4.Parameters.AddWithValue("@SourceTargetQty2", importRow["SourceTargetQty2"]);
                        cmd4.Parameters.AddWithValue("@DestTargetQuantity", importRow["DestTargetQuantity"]);
                        cmd4.Parameters.AddWithValue("@SourceStorageBin", importRow["SourceStorageBin"]);
                        cmd4.Parameters.AddWithValue("@SourceStorageType", importRow["SourceStorageType"]);
                        cmd4.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    ErrorMsgMsg4New.Text = "Upload faild, Please upload correct data file";
                }
                con4.Close();
            }
        }
        //******************************************************************************************************
        //******************************************************************************************************
        //Master File - SHIPPING POINTS 5
        BackgroundWorker workerMasterFile;
        private void UploadFile5_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = "*.csv";
            dlg.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                prg5Ex.Visibility = Visibility.Hidden;
                string filename = dlg.FileName.ToString();
                txtFileName5.Text = filename;
                workerMasterFile = new BackgroundWorker();
                workerMasterFile.WorkerReportsProgress = true;
                workerMasterFile.DoWork += new DoWorkEventHandler(longTaskDoworkMasterFile);
                workerMasterFile.ProgressChanged += new ProgressChangedEventHandler(prgressReportMasterFile);
                workerMasterFile.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerCompleteMasterFile);
                workerMasterFile.RunWorkerAsync();
                ErrorMsgMsg5.Text = string.Empty;
                ErrorMsgMsg5New.Text = string.Empty;
                if (txtFileName5.Text != null)
                {
                    BtnUpload5.IsEnabled = true;
                }
            }
            else
            {
                prg5Ex.Visibility = Visibility.Visible;
                txtFileName5.Text = string.Empty;
                ErrorMsgMsg5.Text = "File upload field";
                SuccessMsg5.Text = string.Empty;
                ErrorMsgMsg5New.Text = string.Empty;
                if (txtFileName5.Text != null)
                {
                    BtnUpload5.IsEnabled = false;
                }
            }
        }


        private void BtnCancel5_Click(object sender, RoutedEventArgs e)
        {
            txtFileName5.Text = string.Empty;
            SuccessMsg5.Text = string.Empty;
            ErrorMsgMsg5.Text = string.Empty;
            ErrorMsgMsg5New.Text = string.Empty;
            prg5Ex.Visibility = Visibility.Visible;
            if (txtFileName5.Text != null)
            {
                BtnUpload5.IsEnabled = false;
            }
        }

        private void workerCompleteMasterFile(object sender, RunWorkerCompletedEventArgs e)
        {
            SuccessMsg5.Text = "File upload successfully";
        }

        private void prgressReportMasterFile(object sender, ProgressChangedEventArgs e)
        {
            prg5.Value = e.ProgressPercentage;
        }

        private void longTaskDoworkMasterFile(object sender, DoWorkEventArgs e)
        {
            int times = 5;
            for (int i = 0; i < times; i++)
            {
                Thread.Sleep(260);
                workerMasterFile.ReportProgress(i * (260 / times));
            }
        }

        private void BtnUpload5_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable importData5 = GetDataFromCSV5File();
                if (importData5 == null) return;
                SaveImportData5ToDatabase(importData5);
                prg5Ex.Visibility = Visibility.Visible;
                txtFileName5.Text = string.Empty;
                SuccessMsg5.Text = string.Empty;
                ErrorMsgMsg5.Text = string.Empty;
            }
            finally
            {
                if (txtFileName5.Text != null)
                {
                    BtnUpload5.IsEnabled = false;
                }
            }
        }

        private DataTable GetDataFromCSV5File()
        {
            DataTable importData5 = new DataTable();
            try
            {
                using (StreamReader sr5 = new StreamReader(txtFileName5.Text))
                {
                    string header5 = sr5.ReadLine();
                    if (string.IsNullOrEmpty(header5))
                    {
                        return null;
                    }
                    string[] headerColumns5 = header5.Split(',');
                    foreach (string headerColumn in headerColumns5)
                    {
                        importData5.Columns.Add(headerColumn);
                    }
                    while (!sr5.EndOfStream)
                    {
                        string line = sr5.ReadLine();
                        if (string.IsNullOrEmpty(line)) continue;
                        string[] fields5 = line.Split(',');
                        DataRow importted5Row = importData5.NewRow();
                        for (int i = 0; i < fields5.Count(); i++)
                        {
                            importted5Row[i] = fields5[i];
                        }
                        importData5.Rows.Add(importted5Row);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return importData5;
        }

        private void SaveImportData5ToDatabase(DataTable importData5)
        {
            string dd = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");
            using (SqlConnection con5 = new SqlConnection(@"Data Source=DESKTOP-E6OMA6L;Initial Catalog=advantis;Integrated Security=True;Pooling=False;"))
            {
                con5.Open();
                try
                {
                    foreach (DataRow importRow in importData5.Rows)
                    {
                        SqlCommand cmd5 = new SqlCommand("INSERT INTO MasterFile(MasterFileImportData, Customer, Zone )" +
                                                        "VALUES(@dd, @Customer, @Zone)", con5);
                        cmd5.Parameters.AddWithValue("@dd", dd);
                        cmd5.Parameters.AddWithValue("@Customer", importRow["Customer"]);
                        cmd5.Parameters.AddWithValue("@Zone", importRow["Zone"]);
                        cmd5.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    ErrorMsgMsg5New.Text = "Upload faild, Please upload correct data file";
                }
                con5.Close();
            }
        }
        //******************************************************************************************************
        //******************************************************************************************************
        //Other File - (Root Number)
 


        private void AllUserLoaded(object sender, RoutedEventArgs e)
        {
          
        }
        private void UserEdit_Click(object sender, RoutedEventArgs e)
        {
            AddNewUser a = new AddNewUser();
            a.Show();
        }


        private void UserRemove_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("call");

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;
            con.Open();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = con;
            sqlCmd.CommandType = CommandType.Text;
            //  DataGrid DataGridUserInfoSection = (DataGrid)sender;
            //  DataRowView row_selected = DataGridUserInfoSection.SelectedItem as DataRowView;
            //       dataAdapter.Fill(dataTable);


           // DataGrid DataGridUserInfoSection = (DataGrid)sender;
            DataRowView row_selected = DataGridUserInfoSection.SelectedItem as DataRowView;
           
                 DataRowView drv = (DataRowView)DataGridUserInfoSection.SelectedItem;
                 drv.Row.Delete(); 
                 sqlCmd.CommandText = "Delete from Advantic_Users where empNo=@empNoDelete";
                 sqlCmd.Parameters.AddWithValue("@empNoDelete",row_selected["empNo"].ToString());
                 sqlCmd.ExecuteNonQuery();


             

           
            //******************************************************************************************************

        }

        private void btnReportPVRDown_Click(object sender, RoutedEventArgs e)
        {

            downloadProductVariationReport();
            }
        

        private void btnReportDobiMark_Click(object sender, RoutedEventArgs e)
        {

            downloadNoofPackagesAgainstDobiMark();
            //---------------------------------------------------------------create report


            //---------------------------------------------------------------

        }

        private void btnReportPickingQty_Click(object sender, RoutedEventArgs e)
        {
            downloadProductVariationReportAgainstPickingQty();
        }

        private void btnReportRejectBox_Click(object sender, RoutedEventArgs e)
        {
            downloadRejectBoxesDuetoWeightDifference();
        }

        private void btnReportRejectWrongInput_Click(object sender, RoutedEventArgs e)
        {
            downloadRejectBoxesDuetoWrongInput();
        }

        static void downloadProductVariationReport()
        {

            try
            {

                string dd = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");

                string dd1 = DateTime.Now.ToString("yyyy-MM-dd");
                SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-E6OMA6L;Initial Catalog=advantis;Integrated Security=True;Pooling=False;");
                con.Open();


                string query = "select Material,TotalStock,ReadBarcodeNumberOfCones,LX03ImportData from LX03Automation where TotalStock != ReadBarcodeNumberOfCones";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@dd1", dd1);

                cmd.ExecuteNonQuery();
                SqlDataReader dr;
                dr = cmd.ExecuteReader();
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                System.IO.FileStream fs = new FileStream("D\\ProductVariationReport(System Vs Received Qty)" + "\\" + "Product Variation Report(System Vs Received Qty)_" + dd1 + ".pdf", FileMode.Create);

                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.AddAuthor("Nikini Automation");
                document.AddCreator("Nikini Automation");
                document.AddKeywords("Nikini Automation");
                document.AddSubject("Document subject - Nikini Automation");
                document.AddTitle("The document title - Nikini Automation");
                //  Font red = new Font(12,Font.NORMAL,BaseColor.RED);

                document.Open();
                iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance("D:/Advantis/Advantis/Advantis/Images/ADVANTIS-LOGOS1.PNG");
                pic.Alignment = Element.ALIGN_CENTER;
                pic.ScaleAbsolute(100, 38);
                document.Add(pic);
                iTextSharp.text.Paragraph pspace1 = new iTextSharp.text.Paragraph(" ");
                document.Add(pspace1);

                var fontBold = BaseFont.TIMES_BOLD;
                var FontColour = new BaseColor(35, 39, 48);
                var MyFont = FontFactory.GetFont(fontBold, 14, FontColour);

                var FontColourheaders = new BaseColor(247, 247, 247);
                var MyFontheaders = FontFactory.GetFont(fontBold, 11, FontColourheaders);

                var FontColourCell = new BaseColor(35, 39, 48);
                var fontFamilyCell = BaseFont.TIMES_ROMAN;
                var cellText = FontFactory.GetFont(fontFamilyCell, 10, FontColourCell);
                iTextSharp.text.Font cellTextF = new Font(cellText);

                var FontDate = FontFactory.GetFont(fontFamilyCell, 9, FontColourCell);

                iTextSharp.text.Paragraph prheader = new iTextSharp.text.Paragraph("Product Variation Report(System Vs Received Qty)", MyFont);
                prheader.Alignment = Element.ALIGN_CENTER;
                document.Add(prheader);
                iTextSharp.text.Paragraph pspace = new iTextSharp.text.Paragraph(" ");
                document.Add(pspace);



                PdfPTable table = new PdfPTable(4);
                PdfPCell cell = new PdfPCell();


                iTextSharp.text.Paragraph pr1 = new iTextSharp.text.Paragraph("Material", MyFontheaders);
                cell = new PdfPCell(new Phrase(pr1));
                cell.VerticalAlignment = Element.ALIGN_CENTER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(35, 39, 48);
                cell.PaddingBottom = 6;
                table.AddCell(cell);

                iTextSharp.text.Paragraph pr2 = new iTextSharp.text.Paragraph("TotalStock", MyFontheaders);
                cell = new PdfPCell(new Phrase(pr2));
                cell.VerticalAlignment = Element.ALIGN_CENTER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(35, 39, 48);
                cell.PaddingBottom = 6;
                table.AddCell(cell);

                iTextSharp.text.Paragraph pr4 = new iTextSharp.text.Paragraph("Pick Quantity", MyFontheaders);
                cell = new PdfPCell(new Phrase(pr4));
                cell.VerticalAlignment = Element.ALIGN_CENTER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(35, 39, 48);
                cell.PaddingBottom = 6;
                table.AddCell(cell);

                iTextSharp.text.Paragraph pr3 = new iTextSharp.text.Paragraph("DateTime", MyFontheaders);
                cell = new PdfPCell(new Phrase(pr3));
                cell.VerticalAlignment = Element.ALIGN_CENTER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(35, 39, 48);
                cell.PaddingBottom = 6;
                table.AddCell(cell);




                //  document.Add(pr1);

                while (dr.Read())
                {
                    string a = dr.GetString(dr.GetOrdinal("Material"));
                    int x = dr.GetInt32(dr.GetOrdinal("TotalStock"));
                    int y = dr.GetInt32(dr.GetOrdinal("ReadBarcodeNumberOfCones"));
                    string b = x.ToString();
                    string d = y.ToString();
                    //string b = dr.GetString(dr.GetOrdinal("TotalStock"));
                    //string d = dr.GetString(dr.GetOrdinal("ReadBarcodeNumberOfCones"));
                    // string c = dr.GetString(dr.GetOrdinal("LX03ImportData"));
                    DateTime z = dr.GetDateTime(dr.GetOrdinal("LX03ImportData"));
                    string c = z.ToString();




                    iTextSharp.text.Paragraph pra = new iTextSharp.text.Paragraph(a, cellTextF);
                    pra.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(new Phrase(pra));
                    cell.PaddingBottom = 5;
                    cell.PaddingLeft = 5;
                    cell.UseVariableBorders = true;
                    cell.BorderColorLeft = new BaseColor(103, 103, 103);
                    cell.BorderColorRight = new BaseColor(103, 103, 103);
                    cell.BorderColorTop = new BaseColor(255, 255, 255);
                    cell.BorderColorBottom = new BaseColor(103, 103, 103);
                    table.AddCell(cell);

                    iTextSharp.text.Paragraph prb = new iTextSharp.text.Paragraph(b, cellTextF);
                    prb.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(new Phrase(prb));
                    cell.PaddingBottom = 5;
                    cell.PaddingLeft = 5;
                    cell.UseVariableBorders = true;
                    cell.BorderColorLeft = new BaseColor(255, 255, 255);
                    cell.BorderColorRight = new BaseColor(103, 103, 103);
                    cell.BorderColorTop = new BaseColor(255, 255, 255);
                    cell.BorderColorBottom = new BaseColor(103, 103, 103);
                    table.AddCell(cell);

                    iTextSharp.text.Paragraph prd = new iTextSharp.text.Paragraph(d, cellTextF);
                    prd.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(new Phrase(prd));
                    cell.PaddingBottom = 5;
                    cell.PaddingLeft = 5;
                    cell.UseVariableBorders = true;
                    cell.BorderColorLeft = new BaseColor(255, 255, 255);
                    cell.BorderColorRight = new BaseColor(103, 103, 103);
                    cell.BorderColorTop = new BaseColor(255, 255, 255);
                    cell.BorderColorBottom = new BaseColor(103, 103, 103);
                    table.AddCell(cell);



                    iTextSharp.text.Paragraph prc = new iTextSharp.text.Paragraph(c + "\n", cellTextF);
                    prc.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(new Phrase(prc));
                    cell.PaddingBottom = 5;
                    cell.PaddingLeft = 5;
                    cell.UseVariableBorders = true;
                    cell.BorderColorLeft = new BaseColor(255, 255, 255);
                    cell.BorderColorRight = new BaseColor(103, 103, 103);
                    cell.BorderColorTop = new BaseColor(255, 255, 255);
                    cell.BorderColorBottom = new BaseColor(103, 103, 103);
                    table.AddCell(cell);



                    //   pr.SetLeading(1f,1f);

                    //  document.Add(pr);
                }
                //   document.Add(new iTextSharp.text.Paragraph("Paragraph - Hello World!"));
                //   document.Add(new iTextSharp.text.Paragraph("Paragraph -Piumi Dinuka!"));


                document.Add(table);
                iTextSharp.text.Paragraph prdate = new iTextSharp.text.Paragraph("Date :" + dd, FontDate);
                prdate.Alignment = Element.ALIGN_RIGHT;
                document.Add(prdate);
                var FontColour1 = new BaseColor(255, 0, 0);


                var MyFont1 = FontFactory.GetFont("Arial Black", 8, FontColour1);



                document.Close();
                writer.Close();
                fs.Close();

                // Close the document



                //lblMsg.Text = "Document saved to the pdf folder.";
                MessageBox.Show("Document saved to the pdf folder");
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }

        }
        static void downloadRejectBoxesDuetoWrongInput()
        {
            try
            {
   
                string dd = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");

                string dd1 = DateTime.Now.ToString("yyyy-MM-dd");
                SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-E6OMA6L;Initial Catalog=advantis;Integrated Security=True;Pooling=False;");
                con.Open();


                string query = "select Device,Barcode,Status,DateTime from ErrorDisplay where DateTime>@dd1 and Status=@status1";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@dd1", dd1);

                cmd.Parameters.AddWithValue("@status1", "Barcode Can't be read");


                cmd.ExecuteNonQuery();
                SqlDataReader dr;
                dr = cmd.ExecuteReader();
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                System.IO.FileStream fs = new FileStream("D\\RejectBoxesDuetoWrongInput" + "\\" + "Reject Boxes Due to Wrong Input_" + dd1 + ".pdf", FileMode.Create);

                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.AddAuthor("Nikini Automation");
                document.AddCreator("Nikini Automation");
                document.AddKeywords("Nikini Automation");
                document.AddSubject("Document subject - Nikini Automation");
                document.AddTitle("The document title - Nikini Automation");
                //  Font red = new Font(12,Font.NORMAL,BaseColor.RED);

                document.Open();
                iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance("D:/Advantis/Advantis/Advantis/Images/ADVANTIS-LOGOS1.PNG");
                pic.Alignment = Element.ALIGN_CENTER;
                pic.ScaleAbsolute(100, 38);
                document.Add(pic);
                iTextSharp.text.Paragraph pspace1 = new iTextSharp.text.Paragraph(" ");
                document.Add(pspace1);

                var fontBold = BaseFont.TIMES_BOLD;
                var FontColour = new BaseColor(35, 39, 48);
                var MyFont = FontFactory.GetFont(fontBold, 14, FontColour);

                var FontColourheaders = new BaseColor(247, 247, 247);
                var MyFontheaders = FontFactory.GetFont(fontBold, 11, FontColourheaders);

                var FontColourCell = new BaseColor(35, 39, 48);
                var fontFamilyCell = BaseFont.TIMES_ROMAN;
                var cellText = FontFactory.GetFont(fontFamilyCell, 10, FontColourCell);
                Font cellTextF = new Font(cellText);

                var FontDate = FontFactory.GetFont(fontFamilyCell, 9, FontColourCell);

                iTextSharp.text.Paragraph prheader = new iTextSharp.text.Paragraph("Reject Boxes Due to Wrong Input", MyFont);
                prheader.Alignment = Element.ALIGN_CENTER;
                document.Add(prheader);
                iTextSharp.text.Paragraph pspace = new iTextSharp.text.Paragraph(" ");
                document.Add(pspace);



                PdfPTable table = new PdfPTable(4);
                PdfPCell cell = new PdfPCell();


                iTextSharp.text.Paragraph pr1 = new iTextSharp.text.Paragraph("Device", MyFontheaders);
                cell = new PdfPCell(new Phrase(pr1));
                cell.VerticalAlignment = Element.ALIGN_CENTER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(35, 39, 48);
                cell.PaddingBottom = 6;
                table.AddCell(cell);

                iTextSharp.text.Paragraph pr2 = new iTextSharp.text.Paragraph("Barcode", MyFontheaders);
                cell = new PdfPCell(new Phrase(pr2));
                cell.VerticalAlignment = Element.ALIGN_CENTER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(35, 39, 48);
                cell.PaddingBottom = 6;
                table.AddCell(cell);

                iTextSharp.text.Paragraph pr4 = new iTextSharp.text.Paragraph("Status", MyFontheaders);
                cell = new PdfPCell(new Phrase(pr4));
                cell.VerticalAlignment = Element.ALIGN_CENTER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(35, 39, 48);
                cell.PaddingBottom = 6;
                table.AddCell(cell);

                iTextSharp.text.Paragraph pr3 = new iTextSharp.text.Paragraph("DateTime", MyFontheaders);
                cell = new PdfPCell(new Phrase(pr3));
                cell.VerticalAlignment = Element.ALIGN_CENTER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(35, 39, 48);
                cell.PaddingBottom = 6;
                table.AddCell(cell);




                //  document.Add(pr1);

                while (dr.Read())
                {
                    string a = dr.GetString(dr.GetOrdinal("Device"));
                    string b = dr.GetString(dr.GetOrdinal("Barcode"));
                    string d = dr.GetString(dr.GetOrdinal("Status"));
                    string c = dr.GetString(dr.GetOrdinal("DateTime"));




                    iTextSharp.text.Paragraph pra = new iTextSharp.text.Paragraph(a, cellTextF);
                    pra.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(new Phrase(pra));
                    cell.PaddingBottom = 5;
                    cell.PaddingLeft = 5;
                    cell.UseVariableBorders = true;
                    cell.BorderColorLeft = new BaseColor(103, 103, 103);
                    cell.BorderColorRight = new BaseColor(103, 103, 103);
                    cell.BorderColorTop = new BaseColor(255, 255, 255);
                    cell.BorderColorBottom = new BaseColor(103, 103, 103);
                    table.AddCell(cell);

                    iTextSharp.text.Paragraph prb = new iTextSharp.text.Paragraph(b, cellTextF);
                    prb.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(new Phrase(prb));
                    cell.PaddingBottom = 5;
                    cell.PaddingLeft = 5;
                    cell.UseVariableBorders = true;
                    cell.BorderColorLeft = new BaseColor(255, 255, 255);
                    cell.BorderColorRight = new BaseColor(103, 103, 103);
                    cell.BorderColorTop = new BaseColor(255, 255, 255);
                    cell.BorderColorBottom = new BaseColor(103, 103, 103);
                    table.AddCell(cell);

                    iTextSharp.text.Paragraph prd = new iTextSharp.text.Paragraph(d, cellTextF);
                    prd.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(new Phrase(prd));
                    cell.PaddingBottom = 5;
                    cell.PaddingLeft = 5;
                    cell.UseVariableBorders = true;
                    cell.BorderColorLeft = new BaseColor(255, 255, 255);
                    cell.BorderColorRight = new BaseColor(103, 103, 103);
                    cell.BorderColorTop = new BaseColor(255, 255, 255);
                    cell.BorderColorBottom = new BaseColor(103, 103, 103);
                    table.AddCell(cell);



                    iTextSharp.text.Paragraph prc = new iTextSharp.text.Paragraph(c + "\n", cellTextF);
                    prc.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(new Phrase(prc));
                    cell.PaddingBottom = 5;
                    cell.PaddingLeft = 5;
                    cell.UseVariableBorders = true;
                    cell.BorderColorLeft = new BaseColor(255, 255, 255);
                    cell.BorderColorRight = new BaseColor(103, 103, 103);
                    cell.BorderColorTop = new BaseColor(255, 255, 255);
                    cell.BorderColorBottom = new BaseColor(103, 103, 103);
                    table.AddCell(cell);



                    //   pr.SetLeading(1f,1f);

                    //  document.Add(pr);
                }
                //   document.Add(new iTextSharp.text.Paragraph("Paragraph - Hello World!"));
                //   document.Add(new iTextSharp.text.Paragraph("Paragraph -Piumi Dinuka!"));


                document.Add(table);
                iTextSharp.text.Paragraph prdate = new iTextSharp.text.Paragraph("Date :" + dd, FontDate);
                prdate.Alignment = Element.ALIGN_RIGHT;
                document.Add(prdate);
                var FontColour1 = new BaseColor(255, 0, 0);


                var MyFont1 = FontFactory.GetFont("Arial Black", 8, FontColour1);



                document.Close();
                writer.Close();
                fs.Close();

                // Close the document



                //lblMsg.Text = "Document saved to the pdf folder.";
                MessageBox.Show("Document saved to the pdf folder");
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        static void downloadRejectBoxesDuetoWeightDifference()
        {
            try
            {

                string dd = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");

                string dd1 = DateTime.Now.ToString("yyyy-MM-dd");
                SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-E6OMA6L;Initial Catalog=advantis;Integrated Security=True;Pooling=False;");
                con.Open();


                string query = "select Device,Barcode,Status,DateTime from ErrorDisplay where DateTime>@dd1 and(Status=@status1 or Status=@status2 or Status=@status3)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@dd1", dd1);
                cmd.Parameters.AddWithValue("@status1", "Weight is not Equal");
                cmd.Parameters.AddWithValue("@status2", "Barcode Can't be read");
                cmd.Parameters.AddWithValue("@status3", "Weight Can't be read");
                cmd.ExecuteNonQuery();
                SqlDataReader dr;
                dr = cmd.ExecuteReader();
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                System.IO.FileStream fs = new FileStream("D\\RejectBoxesDuetoWeightDifference" + "\\" + "Reject BoxesDue to Weight Difference_" + dd1 + ".pdf", FileMode.Create);

                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.AddAuthor("Nikini Automation");
                document.AddCreator("Nikini Automation");
                document.AddKeywords("Nikini Automation");
                document.AddSubject("Document subject - Nikini Automation");
                document.AddTitle("The document title - Nikini Automation");
                //  Font red = new Font(12,Font.NORMAL,BaseColor.RED);

                document.Open();
                iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance("D:/Advantis/Advantis/Advantis/Images/ADVANTIS-LOGOS1.PNG");
                pic.Alignment = Element.ALIGN_CENTER;
                pic.ScaleAbsolute(100, 38);
                document.Add(pic);
                iTextSharp.text.Paragraph pspace1 = new iTextSharp.text.Paragraph(" ");
                document.Add(pspace1);

                var fontBold = BaseFont.TIMES_BOLD;
                var FontColour = new BaseColor(35, 39, 48);
                var MyFont = FontFactory.GetFont(fontBold, 14, FontColour);

                var FontColourheaders = new BaseColor(247, 247, 247);
                var MyFontheaders = FontFactory.GetFont(fontBold, 11, FontColourheaders);

                var FontColourCell = new BaseColor(35, 39, 48);
                var fontFamilyCell = BaseFont.TIMES_ROMAN;
                var cellText = FontFactory.GetFont(fontFamilyCell, 10, FontColourCell);
                Font cellTextF = new Font(cellText);

                var FontDate = FontFactory.GetFont(fontFamilyCell, 9, FontColourCell);

                iTextSharp.text.Paragraph prheader = new iTextSharp.text.Paragraph("Reject Boxes Due to Weight Difference", MyFont);
                prheader.Alignment = Element.ALIGN_CENTER;
                document.Add(prheader);
                iTextSharp.text.Paragraph pspace = new iTextSharp.text.Paragraph(" ");
                document.Add(pspace);



                PdfPTable table = new PdfPTable(4);
                PdfPCell cell = new PdfPCell();


                iTextSharp.text.Paragraph pr1 = new iTextSharp.text.Paragraph("Device", MyFontheaders);
                cell = new PdfPCell(new Phrase(pr1));
                cell.VerticalAlignment = Element.ALIGN_CENTER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(35, 39, 48);
                cell.PaddingBottom = 6;
                table.AddCell(cell);

                iTextSharp.text.Paragraph pr2 = new iTextSharp.text.Paragraph("Barcode", MyFontheaders);
                cell = new PdfPCell(new Phrase(pr2));
                cell.VerticalAlignment = Element.ALIGN_CENTER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(35, 39, 48);
                cell.PaddingBottom = 6;
                table.AddCell(cell);

                iTextSharp.text.Paragraph pr4 = new iTextSharp.text.Paragraph("Status", MyFontheaders);
                cell = new PdfPCell(new Phrase(pr4));
                cell.VerticalAlignment = Element.ALIGN_CENTER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(35, 39, 48);
                cell.PaddingBottom = 6;
                table.AddCell(cell);

                iTextSharp.text.Paragraph pr3 = new iTextSharp.text.Paragraph("DateTime", MyFontheaders);
                cell = new PdfPCell(new Phrase(pr3));
                cell.VerticalAlignment = Element.ALIGN_CENTER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(35, 39, 48);
                cell.PaddingBottom = 6;
                table.AddCell(cell);




                //  document.Add(pr1);

                while (dr.Read())
                {
                    string a = dr.GetString(dr.GetOrdinal("Device"));
                    string b = dr.GetString(dr.GetOrdinal("Barcode"));
                    string d = dr.GetString(dr.GetOrdinal("Status"));
                    string c = dr.GetString(dr.GetOrdinal("DateTime"));




                    iTextSharp.text.Paragraph pra = new iTextSharp.text.Paragraph(a, cellTextF);
                    pra.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(new Phrase(pra));
                    cell.PaddingBottom = 5;
                    cell.PaddingLeft = 5;
                    cell.UseVariableBorders = true;
                    cell.BorderColorLeft = new BaseColor(103, 103, 103);
                    cell.BorderColorRight = new BaseColor(103, 103, 103);
                    cell.BorderColorTop = new BaseColor(255, 255, 255);
                    cell.BorderColorBottom = new BaseColor(103, 103, 103);
                    table.AddCell(cell);

                    iTextSharp.text.Paragraph prb = new iTextSharp.text.Paragraph(b, cellTextF);
                    prb.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(new Phrase(prb));
                    cell.PaddingBottom = 5;
                    cell.PaddingLeft = 5;
                    cell.UseVariableBorders = true;
                    cell.BorderColorLeft = new BaseColor(255, 255, 255);
                    cell.BorderColorRight = new BaseColor(103, 103, 103);
                    cell.BorderColorTop = new BaseColor(255, 255, 255);
                    cell.BorderColorBottom = new BaseColor(103, 103, 103);
                    table.AddCell(cell);

                    iTextSharp.text.Paragraph prd = new iTextSharp.text.Paragraph(d, cellTextF);
                    prd.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(new Phrase(prd));
                    cell.PaddingBottom = 5;
                    cell.PaddingLeft = 5;
                    cell.UseVariableBorders = true;
                    cell.BorderColorLeft = new BaseColor(255, 255, 255);
                    cell.BorderColorRight = new BaseColor(103, 103, 103);
                    cell.BorderColorTop = new BaseColor(255, 255, 255);
                    cell.BorderColorBottom = new BaseColor(103, 103, 103);
                    table.AddCell(cell);



                    iTextSharp.text.Paragraph prc = new iTextSharp.text.Paragraph(c + "\n", cellTextF);
                    prc.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(new Phrase(prc));
                    cell.PaddingBottom = 5;
                    cell.PaddingLeft = 5;
                    cell.UseVariableBorders = true;
                    cell.BorderColorLeft = new BaseColor(255, 255, 255);
                    cell.BorderColorRight = new BaseColor(103, 103, 103);
                    cell.BorderColorTop = new BaseColor(255, 255, 255);
                    cell.BorderColorBottom = new BaseColor(103, 103, 103);
                    table.AddCell(cell);



                    //   pr.SetLeading(1f,1f);

                    //  document.Add(pr);
                }
                //   document.Add(new iTextSharp.text.Paragraph("Paragraph - Hello World!"));
                //   document.Add(new iTextSharp.text.Paragraph("Paragraph -Piumi Dinuka!"));


                document.Add(table);
                iTextSharp.text.Paragraph prdate = new iTextSharp.text.Paragraph("Date :" + dd, FontDate);
                prdate.Alignment = Element.ALIGN_RIGHT;
                document.Add(prdate);
                var FontColour1 = new BaseColor(255, 0, 0);


                var MyFont1 = FontFactory.GetFont("Arial Black", 8, FontColour1);



                document.Close();
                writer.Close();
                fs.Close();

                // Close the document



                //lblMsg.Text = "Document saved to the pdf folder.";
                MessageBox.Show("Document saved to the pdf folder");
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }


        static void downloadProductVariationReportAgainstPickingQty()
        {
            try
            {

                string dd = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");

                string dd1 = DateTime.Now.ToString("yyyy-MM-dd");
                SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-E6OMA6L;Initial Catalog=advantis;Integrated Security=True;Pooling=False;");
                con.Open();


                string query = "select Device,Barcode,DateTime from ErrorDisplay where Status=@status  and DateTime>@dd1 ";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@dd1", dd1);
                cmd.Parameters.AddWithValue("@status", "Barcode doesn't match with system stored barcode");
                cmd.ExecuteNonQuery();
                SqlDataReader dr;
                dr = cmd.ExecuteReader();
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                System.IO.FileStream fs = new FileStream("D\\ProductVariationReportAgainstPickQunatity" + "\\" + "Product Variation Report Against Picking Qty_" + dd1 + ".pdf", FileMode.Create);

                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.AddAuthor("Nikini Automation");
                document.AddCreator("Nikini Automation");
                document.AddKeywords("Nikini Automation");
                document.AddSubject("Document subject - Nikini Automation");
                document.AddTitle("The document title - Nikini Automation");
                //  Font red = new Font(12,Font.NORMAL,BaseColor.RED);

                document.Open();
                iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance("D:/Advantis/Advantis/Advantis/Images/ADVANTIS-LOGOS1.PNG");
                pic.Alignment = Element.ALIGN_CENTER;
                pic.ScaleAbsolute(100, 38);
                document.Add(pic);
                iTextSharp.text.Paragraph pspace1 = new iTextSharp.text.Paragraph(" ");
                document.Add(pspace1);

                var fontBold = BaseFont.TIMES_BOLD;
                var FontColour = new BaseColor(35, 39, 48);
                var MyFont = FontFactory.GetFont(fontBold, 14, FontColour);

                var FontColourheaders = new BaseColor(247, 247, 247);
                var MyFontheaders = FontFactory.GetFont(fontBold, 11, FontColourheaders);

                var FontColourCell = new BaseColor(35, 39, 48);
                var fontFamilyCell = BaseFont.TIMES_ROMAN;
                var cellText = FontFactory.GetFont(fontFamilyCell, 10, FontColourCell);
                Font cellTextF = new Font(cellText);

                var FontDate = FontFactory.GetFont(fontFamilyCell, 9, FontColourCell);

                iTextSharp.text.Paragraph prheader = new iTextSharp.text.Paragraph("Product Variation Report Against Picking Qty", MyFont);
                prheader.Alignment = Element.ALIGN_CENTER;
                document.Add(prheader);
                iTextSharp.text.Paragraph pspace = new iTextSharp.text.Paragraph(" ");
                document.Add(pspace);

                // iTextSharp.text.Paragraph pr2 = new iTextSharp.text.Paragraph("Product variation Report");
                // pr2.Alignment = Element.ALIGN_CENTER;
                // pr2.SetLeading(2f, 2f);

                // document.Add(pr2);


                PdfPTable table = new PdfPTable(3);
                PdfPCell cell = new PdfPCell();


                iTextSharp.text.Paragraph pr1 = new iTextSharp.text.Paragraph("Device", MyFontheaders);
                cell = new PdfPCell(new Phrase(pr1));
                cell.VerticalAlignment = Element.ALIGN_CENTER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(35, 39, 48);
                cell.PaddingBottom = 6;
                table.AddCell(cell);

                iTextSharp.text.Paragraph pr2 = new iTextSharp.text.Paragraph("Barcode", MyFontheaders);
                cell = new PdfPCell(new Phrase(pr2));
                cell.VerticalAlignment = Element.ALIGN_CENTER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(35, 39, 48);
                cell.PaddingBottom = 6;
                table.AddCell(cell);

                iTextSharp.text.Paragraph pr3 = new iTextSharp.text.Paragraph("DateTime", MyFontheaders);
                cell = new PdfPCell(new Phrase(pr3));
                cell.VerticalAlignment = Element.ALIGN_CENTER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(35, 39, 48);
                cell.PaddingBottom = 6;
                table.AddCell(cell);




                //  document.Add(pr1);

                while (dr.Read())
                {
                    string a = dr.GetString(dr.GetOrdinal("Device"));
                    string b = dr.GetString(dr.GetOrdinal("Barcode"));
                    string c = dr.GetString(dr.GetOrdinal("DateTime"));




                    iTextSharp.text.Paragraph pra = new iTextSharp.text.Paragraph(a, cellTextF);
                    pra.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(new Phrase(pra));
                    cell.PaddingBottom = 5;
                    cell.PaddingLeft = 5;
                    cell.UseVariableBorders = true;
                    cell.BorderColorLeft = new BaseColor(103, 103, 103);
                    cell.BorderColorRight = new BaseColor(103, 103, 103);
                    cell.BorderColorTop = new BaseColor(255, 255, 255);
                    cell.BorderColorBottom = new BaseColor(103, 103, 103);
                    table.AddCell(cell);

                    iTextSharp.text.Paragraph prb = new iTextSharp.text.Paragraph(b, cellTextF);
                    prb.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(new Phrase(prb));
                    cell.PaddingBottom = 5;
                    cell.PaddingLeft = 5;
                    cell.UseVariableBorders = true;
                    cell.BorderColorLeft = new BaseColor(255, 255, 255);
                    cell.BorderColorRight = new BaseColor(103, 103, 103);
                    cell.BorderColorTop = new BaseColor(255, 255, 255);
                    cell.BorderColorBottom = new BaseColor(103, 103, 103);
                    table.AddCell(cell);

                    iTextSharp.text.Paragraph prc = new iTextSharp.text.Paragraph(c + "\n", cellTextF);
                    prc.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(new Phrase(prc));
                    cell.PaddingBottom = 5;
                    cell.PaddingLeft = 5;
                    cell.UseVariableBorders = true;
                    cell.BorderColorLeft = new BaseColor(255, 255, 255);
                    cell.BorderColorRight = new BaseColor(103, 103, 103);
                    cell.BorderColorTop = new BaseColor(255, 255, 255);
                    cell.BorderColorBottom = new BaseColor(103, 103, 103);
                    table.AddCell(cell);



                    //   pr.SetLeading(1f,1f);

                    //  document.Add(pr);
                }
                //   document.Add(new iTextSharp.text.Paragraph("Paragraph - Hello World!"));
                //   document.Add(new iTextSharp.text.Paragraph("Paragraph -Piumi Dinuka!"));


                document.Add(table);
                iTextSharp.text.Paragraph prdate = new iTextSharp.text.Paragraph("Date :" + dd, FontDate);
                prdate.Alignment = Element.ALIGN_RIGHT;
                document.Add(prdate);
                var FontColour1 = new BaseColor(255, 0, 0);


                var MyFont1 = FontFactory.GetFont("Arial Black", 8, FontColour1);



                document.Close();
                writer.Close();
                fs.Close();

                // Close the document



                //lblMsg.Text = "Document saved to the pdf folder.";
                MessageBox.Show("Document saved to the pdf folder");
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        static void downloadNoofPackagesAgainstDobiMark()
        {
            try
            {

                string dd = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                string dd1 = DateTime.Now.ToString("yyyy-MM-dd");
                SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-E6OMA6L;Initial Catalog=advantis;Integrated Security=True;Pooling=False;");
                con.Open();


                string query = "select Material, DeliveryQuantity, Location, DobiMark from FinalTable where DobiMark IS NOT NULL and DateTime=@dd1";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@dd1", dd1);
                cmd.ExecuteNonQuery();
                SqlDataReader dr;
                dr = cmd.ExecuteReader();
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                System.IO.FileStream fs = new FileStream("D\\NoofPackagesAgainstDobiMark" + "\\" + "No of Packages Against Dobi Mark_" + dd1 + ".pdf", FileMode.Create);

                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.AddAuthor("Nikini Automation");
                document.AddCreator("Nikini Automation");
                document.AddKeywords("Nikini Automation");
                document.AddSubject("Document subject - Nikini Automation");
                document.AddTitle("The document title - Nikini Automation");
                //  Font red = new Font(12,Font.NORMAL,BaseColor.RED);

                document.Open();
                iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance("D:/Advantis/Advantis/Advantis/Images/ADVANTIS-LOGOS1.PNG");
                pic.Alignment = Element.ALIGN_CENTER;
                pic.ScaleAbsolute(100, 38);
                document.Add(pic);
                iTextSharp.text.Paragraph pspace1 = new iTextSharp.text.Paragraph(" ");
                document.Add(pspace1);

                var fontBold = BaseFont.TIMES_BOLD;
                var FontColour = new BaseColor(35, 39, 48);
                var MyFont = FontFactory.GetFont(fontBold, 14, FontColour);

                var FontColourheaders = new BaseColor(247, 247, 247);
                var MyFontheaders = FontFactory.GetFont(fontBold, 11, FontColourheaders);

                var FontColourCell = new BaseColor(35, 39, 48);
                var fontFamilyCell = BaseFont.TIMES_ROMAN;
                var cellText = FontFactory.GetFont(fontFamilyCell, 10, FontColourCell);
                Font cellTextF = new Font(cellText);

                var FontDate = FontFactory.GetFont(fontFamilyCell, 9, FontColourCell);

                iTextSharp.text.Paragraph prheader = new iTextSharp.text.Paragraph("No of Packages Against Dobi Mark", MyFont);
                prheader.Alignment = Element.ALIGN_CENTER;
                document.Add(prheader);
                iTextSharp.text.Paragraph pspace = new iTextSharp.text.Paragraph(" ");
                document.Add(pspace);

                // iTextSharp.text.Paragraph pr2 = new iTextSharp.text.Paragraph("Product variation Report");
                // pr2.Alignment = Element.ALIGN_CENTER;
                // pr2.SetLeading(2f, 2f);

                // document.Add(pr2);


                PdfPTable table = new PdfPTable(4);
                PdfPCell cell = new PdfPCell();


                iTextSharp.text.Paragraph pr1 = new iTextSharp.text.Paragraph("Material", MyFontheaders);
                cell = new PdfPCell(new Phrase(pr1));
                cell.VerticalAlignment = Element.ALIGN_CENTER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(35, 39, 48);
                cell.PaddingBottom = 6;
                table.AddCell(cell);

                iTextSharp.text.Paragraph pr2 = new iTextSharp.text.Paragraph("DeliveryQuantity", MyFontheaders);
                cell = new PdfPCell(new Phrase(pr2));
                cell.VerticalAlignment = Element.ALIGN_CENTER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(35, 39, 48);
                cell.PaddingBottom = 6;
                table.AddCell(cell);

                iTextSharp.text.Paragraph pr3 = new iTextSharp.text.Paragraph("Location", MyFontheaders);
                cell = new PdfPCell(new Phrase(pr3));
                cell.VerticalAlignment = Element.ALIGN_CENTER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(35, 39, 48);
                cell.PaddingBottom = 6;
                table.AddCell(cell);

                iTextSharp.text.Paragraph pr4 = new iTextSharp.text.Paragraph("DobiMark", MyFontheaders);
                cell = new PdfPCell(new Phrase(pr4));
                cell.VerticalAlignment = Element.ALIGN_CENTER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(35, 39, 48);
                cell.PaddingBottom = 6;
                table.AddCell(cell);


                //  document.Add(pr1);

                while (dr.Read())
                {
                    string a = dr.GetString(dr.GetOrdinal("Material"));
                    string b = dr.GetString(dr.GetOrdinal("DeliveryQuantity"));
                    string c = dr.GetString(dr.GetOrdinal("Location"));
                    string d = dr.GetString(dr.GetOrdinal("DobiMark"));



                    iTextSharp.text.Paragraph pra = new iTextSharp.text.Paragraph(a, cellTextF);
                    pra.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(new Phrase(pra));
                    cell.PaddingBottom = 5;
                    cell.PaddingLeft = 5;
                    cell.UseVariableBorders = true;
                    cell.BorderColorLeft = new BaseColor(103, 103, 103);
                    cell.BorderColorRight = new BaseColor(103, 103, 103);
                    cell.BorderColorTop = new BaseColor(255, 255, 255);
                    cell.BorderColorBottom = new BaseColor(103, 103, 103);
                    table.AddCell(cell);

                    iTextSharp.text.Paragraph prb = new iTextSharp.text.Paragraph(b, cellTextF);
                    prb.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(new Phrase(prb));
                    cell.PaddingBottom = 5;
                    cell.PaddingLeft = 5;
                    cell.UseVariableBorders = true;
                    cell.BorderColorLeft = new BaseColor(255, 255, 255);
                    cell.BorderColorRight = new BaseColor(103, 103, 103);
                    cell.BorderColorTop = new BaseColor(255, 255, 255);
                    cell.BorderColorBottom = new BaseColor(103, 103, 103);
                    table.AddCell(cell);

                    iTextSharp.text.Paragraph prc = new iTextSharp.text.Paragraph(c, cellTextF);
                    prc.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(new Phrase(prc));
                    cell.PaddingBottom = 5;
                    cell.PaddingLeft = 5;
                    cell.UseVariableBorders = true;
                    cell.BorderColorLeft = new BaseColor(255, 255, 255);
                    cell.BorderColorRight = new BaseColor(103, 103, 103);
                    cell.BorderColorTop = new BaseColor(255, 255, 255);
                    cell.BorderColorBottom = new BaseColor(103, 103, 103);
                    table.AddCell(cell);


                    iTextSharp.text.Paragraph prd = new iTextSharp.text.Paragraph(d + "\n", cellTextF);
                    prd.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(new Phrase(prd));
                    cell.PaddingBottom = 5;
                    cell.PaddingLeft = 5;
                    cell.UseVariableBorders = true;
                    cell.BorderColorLeft = new BaseColor(255, 255, 255);
                    cell.BorderColorRight = new BaseColor(103, 103, 103);
                    cell.BorderColorTop = new BaseColor(255, 255, 255);
                    cell.BorderColorBottom = new BaseColor(103, 103, 103);
                    table.AddCell(cell);


                    //   pr.SetLeading(1f,1f);

                    //  document.Add(pr);
                }
                //   document.Add(new iTextSharp.text.Paragraph("Paragraph - Hello World!"));
                //   document.Add(new iTextSharp.text.Paragraph("Paragraph -Piumi Dinuka!"));


                document.Add(table);
                iTextSharp.text.Paragraph prdate = new iTextSharp.text.Paragraph("Date :" + dd, FontDate);
                prdate.Alignment = Element.ALIGN_RIGHT;
                document.Add(prdate);
                var FontColour1 = new BaseColor(255, 0, 0);


                var MyFont1 = FontFactory.GetFont("Arial Black", 8, FontColour1);



                document.Close();
                writer.Close();
                fs.Close();

                // Close the document



                //lblMsg.Text = "Document saved to the pdf folder.";
                MessageBox.Show("Document saved to the pdf folder");
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }
        static void schedule()
        {
            DateTime nowTime = DateTime.Now;
            DateTime scheduleTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 16, 51, 0);
            if (nowTime > scheduleTime)
            {
                scheduleTime = scheduleTime.AddDays(1);
            }
            double ticktime = (double)(scheduleTime - DateTime.Now).TotalMilliseconds;
            timer = new System.Timers.Timer(ticktime);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();


        }

        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            downloadNoofPackagesAgainstDobiMark();
            downloadProductVariationReportAgainstPickingQty();
            downloadRejectBoxesDuetoWeightDifference();
            downloadRejectBoxesDuetoWrongInput();
            downloadProductVariationReport();
            schedule();
        }

        
        private void btnReportReworkedQty_Click(object sender, RoutedEventArgs e)
        {
            
        }

        //PDF REPORT
        private void Datagrid_Load(object sender, RoutedEventArgs e)
        {
            /*  String[] files = Directory.GetFiles(@"D:\20_8_Advantis\Advantis\Advantis\Advantis\bin\Debug\D\ProductVariationReport(System Vs Received Qty)");
              DataTable table = new DataTable();
              table.Columns.Add("File Name");

              for (int i = 0; i < files.Length; i++)
              {
                  FileInfo file = new FileInfo(files[i]);
                  table.Rows.Add(file.Name);

              }*/
        }


        //Product Variation Report (System Vs Received Qty) Report
        private void PVReport_Click(object sender, MouseButtonEventArgs e)
        {
            pdfWebViewer.Navigate(new Uri("about:blank"));
            String files = @"D:\Advantis\Advantis\Advantis\bin\Debug\D\ProductVariationReport(System Vs Received Qty)";
            pdfWebViewer.Navigate(files);
            txtReportTitle.Text = "Product Variation Report (System Vs Received Qty)";
        }


        //No of Packages Against Dobi Mark
        private void NOPADMarkR_click(object sender, MouseButtonEventArgs e)
        {
            pdfWebViewer.Navigate(new Uri("about:blank"));
            String files = @"D:\Advantis\Advantis\Advantis\bin\Debug\D\NoofPackagesAgainstDobiMark";
            pdfWebViewer.Navigate(files);
            txtReportTitle.Text = "No of Packages Against Dobi Mark";
        }

        //Product Variation Report Against Picking Qty
        private void PVRAPicQty_click(object sender, MouseButtonEventArgs e)
        {
            pdfWebViewer.Navigate(new Uri("about:blank"));
            String files = @"D:\Advantis\Advantis\Advantis\bin\Debug\D\ProductVariationReportAgainstPickQunatity";
            pdfWebViewer.Navigate(files);
            txtReportTitle.Text = "Product Variation Report Against Picking Qty";
        }

        //Reject BoxesDue to Weight Difference
        private void RBToWD_click(object sender, MouseButtonEventArgs e)
        {
            pdfWebViewer.Navigate(new Uri("about:blank"));
            String files = @"D:\Advantis\Advantis\Advantis\bin\Debug\D\RejectBoxesDuetoWeightDifference";
            pdfWebViewer.Navigate(files);
            txtReportTitle.Text = "Reject BoxesDue to Weight Difference";
        }

        //Reject Boxes Due to Wrong Input
        private void RBDToWrIn_click(object sender, MouseButtonEventArgs e)
        {
            pdfWebViewer.Navigate(new Uri("about:blank"));
            String files = @"D:\Advantis\Advantis\Advantis\bin\Debug\D\RejectBoxesDuetoWrongInput";
            pdfWebViewer.Navigate(files);
            txtReportTitle.Text = "Reject Boxes Due to Wrong Input";
        }




        private void Datagrid_click(object sender, MouseButtonEventArgs e)
        {
            //OpenFileDialog opn = new OpenFileDialog();

        }

        private void click(object sender, MouseButtonEventArgs e)
        {
          //  AddNewUser addNewUser = new AddNewUser();
           // addNewUser.UserEmpNo.Text = this.DataGridUserInfoSection.CurrentItem.ToString();
           // addNewUser.ShowDialog();
            DataGrid DataGridUserInfoSection = (DataGrid)sender;
            DataRowView row_selected = DataGridUserInfoSection.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                EditUser editUser = new EditUser();
                editUser.EditUserEmpNo.Text = row_selected["empNo"].ToString();
                editUser.EditUserEmpName.Text = row_selected["empName"].ToString();
                editUser.EditUserOpteam.Text = row_selected["userOpteam"].ToString();
                editUser.ShowDialog();

            }

        }

        private void DataGridUserInfoSection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          ///  AddNewUser addNewUser = new AddNewUser();
          
        }

        private void DataGridUserInfoSection_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
        void prgressReport_loadUsers(object sender, ProgressChangedEventArgs e)
        {
            

        }
        void workerComplete_loadUsers(object sender, RunWorkerCompletedEventArgs e)
        {
            System.Windows.MessageBox.Show("upload successfully");

        }
        private void longTaskDowork_loadUsers(object sender, DoWorkEventArgs e)
        {
            int i = 0;
            //    for (int i = 0; i < times; i++)
            while (true)

            {
                ++i;
                Thread.Sleep(1000);
              //  loadUsers.ReportProgress(i);
            }
        }

        private void DataGridUserInfoSection_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Console.WriteLine("try call");
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;
                con.Open();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = con;
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "Select empNo,empName,userOpteam from Advantic_Users";
                SqlDataAdapter sqlDataAdap = new SqlDataAdapter(sqlCmd);

                DataTable dt = new DataTable();

                sqlDataAdap.Fill(dt);


                DataGridUserInfoSection.ItemsSource = dt.DefaultView;

                sqlDataAdap.Update(dt);


                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ex is" + ex);
            }
        }
        //********************************************************************************************
       
    }
}
