using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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

namespace Advantis
{
    /// <summary>
    /// Interaction logic for AllUserLogin.xaml
    /// </summary>
    public partial class AllUserLogin 
    {
        public AllUserLogin()
        {
            InitializeComponent();
           
        }

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
                        DashboardAdmin supervisorModule = new DashboardAdmin();
                        supervisorModule.Show();
                       
                    }


                    //UploadCSV uploadCsv = new UploadCSV();
                    //uploadCsv.uploadCsv();



                    else if (ds.Rows[0][0].ToString() == "Administrator")
                    {
                        DashboardAdmin dashboardAdmin = new DashboardAdmin();
                        dashboardAdmin.Show();
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
    }
}
