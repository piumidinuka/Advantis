using System;
using System.Windows;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Text;
using System.Security;
using System.Security.Cryptography;

namespace Advantis
{
    /// <summary>
    /// Interaction logic for userManagement.xaml
    /// </summary>
    public partial class userManagement 
    {
        
       

        public userManagement()
        {
                
                       InitializeComponent();
                        binddatagrid();

        }
        //*********************************************************************************************
        private void AdminDashboard_Click(object sender, RoutedEventArgs e)
        {
            if (AdminDashboard.Content == FindResource("DisabledAdminDashboard"))
            {
                AdminDashboard.Content = FindResource("AdminDashboard");
                AdminDashboardFeild.Foreground = System.Windows.Media.Brushes.Gray;
                AddUser.Content = FindResource("DisabledAddUser");
                AddUserFeild.Foreground = System.Windows.Media.Brushes.LightGray;

                // hidden content section
                AdminDashboardGridVisibility.Visibility = System.Windows.Visibility.Visible;
                AddUserGridVisibility.Visibility = System.Windows.Visibility.Hidden;
            }
        }
        //***********************************************************************************************
        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            if (AddUser.Content == FindResource("DisabledAddUser"))
            {
                AddUser.Content = FindResource("AddUser");
                AddUserFeild.Foreground = System.Windows.Media.Brushes.Gray;
                AdminDashboard.Content = FindResource("DisabledAdminDashboard");
                AdminDashboardFeild.Foreground = System.Windows.Media.Brushes.LightGray;

                // hidden content section
                AddUserGridVisibility.Visibility = System.Windows.Visibility.Visible;
                AdminDashboardGridVisibility.Visibility = System.Windows.Visibility.Hidden;
            }
        }
        //***********************************************************************************************

        private void binddatagrid()
        {
            try
            {


                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select empName,empNo,role from Users";
                cmd.Connection = con;

                SqlDataAdapter dr = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Users");
                dr.Fill(dt);
                g1.ItemsSource = dt.DefaultView;
                dr.Update(dt);

                Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaa" + g1.ItemsSource);
                con.Close();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        //***********************************************************************************************

        private void Window_Loaded(object sender,EventArgs e)
        {
          //  this.binddatagrid();
        }

        private void btnViewReports_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnGenerateReport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
                //Add md5 encryption method
            MD5 md5 = MD5.Create();

            //create an array of ascii encoding of password

            byte[] bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(txtPword.Password);

            // create hash of created array element
            byte[] hash = md5.ComputeHash(bytes);
            // when hash is computed then create a string of encrypted password using string builder class and append the values created to make a full string encrypted message

            StringBuilder sb = new StringBuilder();
            for(int b = 0; b < hash.Length; b++)
            {
                // created hex encrypted md5 password in string format
                sb.Append(hash[b].ToString("X2"));

            }
            var password = sb.ToString();
            //insert data to database
              SqlConnection con = new SqlConnection();
              con.ConnectionString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;
           
            SqlCommand cmd = new SqlCommand();
            
            cmd.CommandText = "insert into Advantic_Users(fName,lName,uName,empNo,password,role)values(@fName,@lName,@uName,@empno,@password,@role)";
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("fName", txtFname.Text);
            cmd.Parameters.AddWithValue("lName", txtLname.Text);
            cmd.Parameters.AddWithValue("uName", txtUname.Text);
            cmd.Parameters.AddWithValue("empno", txtEMPNo.Text);
            
            cmd.Parameters.AddWithValue("password", password);
            cmd.Parameters.AddWithValue("role", cmbUserRole.Text);
            SqlDataReader reader;
            try
            {
                con.Open();
                if (this.txtPword.Password== this.txtConfirmPword.Password) {
                    reader = cmd.ExecuteReader();
                    MessageBox.Show("Saved");
                }
                else{
                    MessageBox.Show("Your password should be matched");
                   // MessagePwConfirm.Visibility(Visual);
                }
               
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
           
            
        }

        private void txtFname_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtPword_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void cmbUserRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
