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
using System.Threading;

namespace Advantis
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            AdamControl ac = new AdamControl(17);
            ac.BulbOn();
            Thread.Sleep(2000);
            ac.BulbOff();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            AdamControl ac = new AdamControl(20);
            ac.BulbOff();
        }
    }
}
