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
using System.Windows.Media.Effects;

namespace Advantis
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard 
    {
        public Dashboard()
        {
            //InitializeComponent();
        }

        private void btnLogoutProgressUser_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void GridAllLogoutUserProgress_Click(object sender, RoutedEventArgs e)
        {
            //MainWindow s = new MainWindow();
            //s.Show();
            this.Close();
        }
        private void txtUserIconDisplayText_Click(object sender, RoutedEventArgs e)
        {
            GridAllLogoutUserProgress.Visibility = System.Windows.Visibility.Visible;
        }
       

        private void btnProgressBarUserIcon_Click(object sender, RoutedEventArgs e)
        {
            GridAllLogoutUserProgress.Visibility = System.Windows.Visibility.Visible;
        }
        private void GridMainPrgressSection_Click(object sender, RoutedEventArgs e)
        {
            GridAllLogoutUserProgress.Visibility = System.Windows.Visibility.Hidden;
        }
        

        //SuccessBarcode1
      /*   private void SuccessBarcode1_Click(object sender, RoutedEventArgs e)
         {
             //errorList.IsOpen = true;
             if (SuccessBarcode1.Content == FindResource("DisabledSuccessBarcode1"))
             {
                 SuccessBarcode1.Content = FindResource("SuccessBarcode1");
             }
             else
             {
                 SuccessBarcode1.Content = FindResource("DisabledSuccessBarcode1");
             }
         }

         private void RejectBarcode1_Click(object sender, RoutedEventArgs e)
         {
             if (RejectBarcode1.Content == FindResource("DisabledRejectBarcode1"))
             {
                 RejectBarcode1.Content = FindResource("RejectBarcode1");

             }
             else
             {
                 RejectBarcode1.Content = FindResource("DisabledRejectBarcode1");
             }
         }

         private void SuccessWeight_Click(object sender, RoutedEventArgs e)
         {
             if (SuccessWeight.Content == FindResource("DisabledSuccessWeight"))
             {
                 SuccessWeight.Content = FindResource("SuccessWeight");
             }
             else
             {
                 SuccessWeight.Content = FindResource("DisabledSuccessWeight");
             }
         }

         private void RejectWeight_Click(object sender, RoutedEventArgs e)
         {
             if (RejectWeight.Content == FindResource("DisabledRejectWeight"))
             {
                 RejectWeight.Content = FindResource("RejectWeight");
             }
             else
             {
                 RejectWeight.Content = FindResource("DisabledRejectWeight");
             }
         }

         private void SuccessBarcode2_Click(object sender, RoutedEventArgs e)
         {
             if (SuccessBarcode2.Content == FindResource("DisabledSuccessBarcode2"))
             {
                 SuccessBarcode2.Content = FindResource("SuccessBarcode2");
             }
             else
             {
                 SuccessBarcode2.Content = FindResource("DisabledSuccessBarcode2");
             }
         }

         private void RejectBarcode2_Click(object sender, RoutedEventArgs e)
         {
             if (RejectBarcode2.Content == FindResource("DisabledRejectBarcode2"))
             {
                 RejectBarcode2.Content = FindResource("RejectBarcode2");
             }
             else
             {
                 RejectBarcode2.Content = FindResource("DisabledRejectBarcode2");
             }
         }

         private void AATrack_Click(object sender, RoutedEventArgs e)
         {
             if (AATrack.Content == FindResource("DisabledAATrack"))
             {
                 AATrack.Content = FindResource("AATrack");
             }
             else
             {
                 AATrack.Content = FindResource("DisabledAATrack");
             }
         }

         private void BBTrack_Click(object sender, RoutedEventArgs e)
         {
             if (BBTrack.Content == FindResource("DisabledBBTrack"))
             {
                 BBTrack.Content = FindResource("BBTrack");
             }
             else
             {
                 BBTrack.Content = FindResource("DisabledBBTrack");
             }
         }

         private void CCTrack_Click(object sender, RoutedEventArgs e)
         {
             if (CCTrack.Content == FindResource("DisabledCCTrack"))
             {
                 CCTrack.Content = FindResource("CCTrack");
             }
             else
             {
                 CCTrack.Content = FindResource("DisabledCCTrack");
             }
         }

         private void Stock_Click(object sender, RoutedEventArgs e)
         {
             if (Stock.Content == FindResource("DisabledStock"))
             {
                 Stock.Content = FindResource("Stock");
             }
             else
             {
                 Stock.Content = FindResource("DisabledStock");
             }
         }

         private void PrintSuccess_Click(object sender, RoutedEventArgs e)
         {
             if (PrintSuccess.Content == FindResource("DisabledPrintSuccess"))
             {
                 PrintSuccess.Content = FindResource("PrintSuccess");
             }
             else
             {
                 PrintSuccess.Content = FindResource("DisabledPrintSuccess");
             }
         }

         private void PrintReject_Click(object sender, RoutedEventArgs e)
         {
             if (PrintReject.Content == FindResource("DisabledPrintReject"))
             {
                 PrintReject.Content = FindResource("PrintReject");
             }
             else
             {
                 PrintReject.Content = FindResource("DisabledPrintReject");
             }
         }

         /*private void RePrintBarcode_Click(object sender, RoutedEventArgs e)
         {
             if (RePrintBarcode.Content == FindResource("DisabledRePrintBarcode"))
             {
                 RePrintBarcode.Content = FindResource("RePrintBarcode");
             }
             else
             {
                 RePrintBarcode.Content = FindResource("DisabledRePrintBarcode");
             }
         }*/

    }
}
 