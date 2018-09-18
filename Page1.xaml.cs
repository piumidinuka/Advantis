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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Data.Entity;
namespace Advantis
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 { 
        public Page1()
        {
            InitializeComponent();
            binddatagrid();
        }

        private void g1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          //binddatagrid();
        }

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
                g1.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = dt });
                g1.ItemsSource = dt.DefaultView;
               // g1.DataContext = dt.DefaultView;
                dr.Update(dt);

                Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaa" + g1.ItemsSource);
              con.Close();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private void Window_Loaded(object sender, EventArgs e)
        {
           // this.binddatagrid();
        }
    }
}
