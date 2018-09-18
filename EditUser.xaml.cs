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
    /// Interaction logic for EditUser.xaml
    /// </summary>
    public partial class EditUser 
    {
        public EditUser()
        {
            InitializeComponent();
        }

        private void EdituserSubmit_Click(object sender, RoutedEventArgs e)
        {
            MD5 md5 = MD5.Create();

            //create an array of ascii encoding of password

            byte[] bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(EdituserPassword.Password);

            // create hash of created array element
            byte[] hash = md5.ComputeHash(bytes);
            // when hash is computed then create a string of encrypted password using string builder class and append the values created to make a full string encrypted message

            StringBuilder sb = new StringBuilder();
            for (int b = 0; b < hash.Length; b++)
            {
                // created hex encrypted md5 password in string format
                sb.Append(hash[b].ToString("X2"));

            }
            var password1 = sb.ToString();
            string password = EdituserPassword.Password;
            string designation = EditUserDesignation.Text;
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Password Should not be empty");
            }
            else if (string.IsNullOrEmpty(designation))
            {
                MessageBox.Show("Designation Should not be empty");
            }
            else
            {
                try
                {
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();

                    cmd.CommandText = "update Advantic_Users set empNo=@empNo,empName=@empName,userDesignation=@userDesignation,userOpteam=@userOpteam,password=@password where empNo=@empNo";

                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@empNo", EditUserEmpNo.Text);
                    cmd.Parameters.AddWithValue("@empName", EditUserEmpName.Text);
                    cmd.Parameters.AddWithValue("@userDesignation", EditUserDesignation.Text);
                    cmd.Parameters.AddWithValue("@userOpteam", EditUserOpteam.Text);

                    cmd.Parameters.AddWithValue("@password", password1);

                    cmd.ExecuteNonQuery();


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

            }

        }

        private void userCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

      
    }
}
