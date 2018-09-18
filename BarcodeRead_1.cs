using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Advantis
{
    class BarcodeRead_1
    {
        private static int port = 51235; //server Port
        private static string ip = "192.168.1.2";
        Thread s;
        string barcode = "";
        int x = 0;
        List<string> dbBarcodeValue = new List<string>();
        public void myReader()
        {
            TcpClient client = new TcpClient();
            //     client.ExclusiveAdmyReaderdressUse = true;
            var result = client.BeginConnect(ip, port, null, null);
            var sucess = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;
            con.Open();
           SqlDataAdapter da = new SqlDataAdapter("select SystemBarcode from BarcodeInfo", con);

           DataSet ds = new DataSet();
            da.Fill(ds, "BarcodeInfo");

              dbBarcodeValue = new List<string>();
           foreach (DataRow row in ds.Tables["BarcodeInfo"].Rows)
            {
               dbBarcodeValue.Add(row["SystemBarcode"].ToString());

            }
            while (true)
            {
                if (!sucess)
                {
                    Console.WriteLine("Failed to connect");
                    Thread.Sleep(1000);
                }
                else
                {
                    try
                    {
                        byte[] bytesFrom = new byte[1000];

                        if (client.Connected)
                        {
                            var stream = client.GetStream();
                            stream.Read(bytesFrom, 0, 1000);
                            string readData = Encoding.ASCII.GetString(bytesFrom);



                            if (dbBarcodeValue.Any(str => readData.Contains(str)))
                            {
                                ++x;
                                Console.WriteLine("equal");
                             /*   SqlCommand cmd = new SqlCommand();
                                String updatequery = "update BarcodeInfo set NumberOfCones=1 where SystemBarcode=@barcode";
                                cmd.Connection = con;
                                SqlCommand query = new SqlCommand(updatequery, con);
                                query.Parameters.AddWithValue("@barcode", readData);
                                query.ExecuteNonQuery();
                                */

                            }

                            else
                            {

                                Console.WriteLine("not equal");

                            }

                       
                     //       System.IO.StreamWriter file = new System.IO.StreamWriter(@"D:\\Barcode\BarcodeReader.txt", false);
                       //     Console.WriteLine("barcode" + barcode);

                       //     file.WriteLine(readData);
                      //      file.Close();

                        }

                        else
                        {
                            client = null;
                            client = new TcpClient();
                            client.ExclusiveAddressUse = true;
                            result = client.BeginConnect(ip, port, null, null);
                            sucess = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));

                               Thread.Sleep(1000);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        client = null;
                        client = new TcpClient();
                        client.ExclusiveAddressUse = true;
                        result = client.BeginConnect(ip, port, null, null);
                        sucess = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));

                          Thread.Sleep(1000);
                    }

                }
                sucess = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));
            }
          
        }

    }
}
