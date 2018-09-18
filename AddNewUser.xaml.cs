using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for AddNewUser.xaml
    /// </summary>
    public partial class AddNewUser 
    {
        public AddNewUser()
        {
            InitializeComponent();
           // AllowsTransparency = true;
        }

        private void userCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void userSubmit_Click(object sender, RoutedEventArgs e)
        {
            MD5 md5 = MD5.Create();

            //create an array of ascii encoding of password

            byte[] bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(userPassword.Password);

            // create hash of created array element
            byte[] hash = md5.ComputeHash(bytes);
            // when hash is computed then create a string of encrypted password using string builder class and append the values created to make a full string encrypted message

            StringBuilder sb = new StringBuilder();
            for (int b = 0; b < hash.Length; b++)
            {
                // created hex encrypted md5 password in string format
                sb.Append(hash[b].ToString("X2"));

            }
            var password = sb.ToString();
            //insert data to database
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;
            con.Open();
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "insert into Advantic_Users(empNo,empName,userDesignation,userOpteam,password)values(@empNo,@empName,@userDesignation,@userOpteam,@password)";
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("empNo", UserEmpNo.Text);
            cmd.Parameters.AddWithValue("empName", UserEmpName.Text);
            cmd.Parameters.AddWithValue("userDesignation", UserDesignation.Text);
            cmd.Parameters.AddWithValue("userOpteam", UserOpteam.Text);

            cmd.Parameters.AddWithValue("password", password);

            cmd.ExecuteNonQuery();
            DashboardAdmin dash = new DashboardAdmin();
            //dash.DataGridUserInfoSection 
            /* try
             {
                 con.Open();
                 if (this.userPassword.Password == this.txtConfirmPword.Password)
                 {
                     reader = cmd.ExecuteReader();
                     MessageBox.Show("Saved");
                 }
                 else
                 {
                     MessageBox.Show("Your password should be matched");
                     // MessagePwConfirm.Visibility(Visual);
                 }

             }
             catch (Exception ex)
             {
                 Console.WriteLine(ex);
             }*/
            this.Close();
        }
    }
}
