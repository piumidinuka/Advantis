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

namespace Advantis
{
    /// <summary>
    /// Interaction logic for BarcodeList.xaml
    /// </summary>
    public partial class BarcodeList 
    {
        public BarcodeList()
        {
            InitializeComponent();
        }

        private void PrintNewBarcode_Click(object sender, RoutedEventArgs e)
        {
            SecureLogin secureLogin = new SecureLogin();
            secureLogin.Show();
        }
    }
}
