using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Data.Entity;
using System.Security;
using System.Security.Cryptography;
namespace Advantis
{
    /// <summary>
    /// Interaction logic for SecureLogin.xaml
    /// </summary>
    public partial class SecureLogin
    {
        public SecureLogin()
        {
            InitializeComponent();
            AllowsTransparency = true;
        }

        private void loginSecure_Click(object sender, RoutedEventArgs e)
        {
            if (UserNameSecureLogin.Text == "" || PasswordSecureLogin.Password == "")
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

                cmd.CommandText = "select role from Advantic_Users where uName=@username and password=@password";
                cmd.Connection = con;

                cmd.Parameters.AddWithValue("@username", UserNameSecureLogin.Text);
               
                cmd.Parameters.AddWithValue("@password", CommonFunctions.sha256_hash(PasswordSecureLogin.Password));
                Console.WriteLine("Password is" + CommonFunctions.sha256_hash(PasswordSecureLogin.Password));

                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
              
                DataTable ds = new DataTable();
                adapt.Fill(ds);

                if (ds.Rows.Count == 1)

                { 

                    if (ds.Rows[0][0].ToString() == "Secure User")
                    {

                        //BarcodeList fm = new BarcodeList();
                        //fm.Show();

                     

                    }

                    else
                    {
                        MessageBox.Show("You have not authorize to access this page");
                    }

                }


                else
                {
                    MessageBox.Show("Please check your Username and Password");
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine("Exception" + ex);
            }
        }
    }
}