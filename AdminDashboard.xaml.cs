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
    /// Interaction logic for AdminDashboard.xaml
    /// </summary>
    public partial class AdminDashboard 
    {
        public AdminDashboard()
        {
            InitializeComponent();
        }

        //Top Bar Admin Logout
        private void GridLogoutAdminText_Click(object sender, MouseButtonEventArgs e)
        {
           MainWindow s = new MainWindow();
            s.Show();
            this.Close();
        }
        //Logout Top Bar All Dashboard Element
        private void GridAllLogoutAdminText_Click(object sender, MouseButtonEventArgs e)
        {
            MainWindow s = new MainWindow();
            s.Show();
            this.Close();
        }

        //Left Pannel Logout Button
        private void btnLogoutAdmidnIcon_Click(object sender, RoutedEventArgs e)
        {
            //MainWindow s = new MainWindow();
            //s.Show();
            this.Close();
        }
        private void txtLeftLogoutAdmidnIcon_Click(object sender, RoutedEventArgs e)
        {
            MainWindow s = new MainWindow();
            s.Show();
            this.Close();
        }
        private void btnLeftLogoutAdmidnIcon_Click(object sender, RoutedEventArgs e)
        {
            MainWindow s = new MainWindow();
            s.Show();
            this.Close();
        }
       

        //Display Admin Logout Top Bar
        private void btnAdminUserIcon_Click(object sender, RoutedEventArgs e)
        {
            GridLogoutAdminText.Visibility = System.Windows.Visibility.Visible;
            GridAllLogoutAdminText.Visibility = System.Windows.Visibility.Visible;
        }
        private void txtTopLoginAdminDisplayText_Click(object sender, MouseButtonEventArgs e)
        {
            GridLogoutAdminText.Visibility = System.Windows.Visibility.Visible;
            GridAllLogoutAdminText.Visibility = System.Windows.Visibility.Visible;
        }
        // hide logout top button in mouse leave
        private void GridHideLoginSideButton_Click(object sender, MouseButtonEventArgs e)
        {
            GridLogoutAdminText.Visibility = System.Windows.Visibility.Hidden;
            GridAllLogoutAdminText.Visibility = System.Windows.Visibility.Hidden;
            navigationMenu.IsOpen = false;
        }


        //Open Navigation Flyout Window
        private void btnMenuItemAdminDb_Click(object sender, RoutedEventArgs e)
        {
            navigationMenu.IsOpen = true;
        }

        private void btnUserManagement_Click(object sender, RoutedEventArgs e)
        {
            GridDashboardHeaderBackgroundColorHidden.Visibility = System.Windows.Visibility.Hidden;
            //MainDashboardBgVisibility.Visibility = System.Windows.Visibility.Hidden;
            GridAdminDashboardMainMenuSection.Visibility = System.Windows.Visibility.Hidden;
            GridAllHeaderBackgroundColorVisible.Visibility = System.Windows.Visibility.Visible;
            GridUserManagementPanelStart.Visibility = System.Windows.Visibility.Visible;
            if (btnUserManagement.Content == FindResource("UserManagement"))
            {
                btnUserManagement.Content = FindResource("SelectUserManagement");
                btnDashboardIcon.Content = FindResource("UserManagement");
                txtTopMenuTitle.Text = "Manage Users";
            }
        }

        //UserManagement Menu Item Click
        private void btnLeftUserManagement_Click(object sender, RoutedEventArgs e)
        {
            GridDashboardHeaderBackgroundColorHidden.Visibility = System.Windows.Visibility.Hidden;
           // MainDashboardBgVisibility.Visibility = System.Windows.Visibility.Hidden;
            GridAdminDashboardMainMenuSection.Visibility = System.Windows.Visibility.Hidden;
            GridAllHeaderBackgroundColorVisible.Visibility = System.Windows.Visibility.Visible;
            GridUserManagementPanelStart.Visibility = System.Windows.Visibility.Visible;
            navigationMenu.IsOpen = false;
            if (btnLeftUserManagement.Content == FindResource("LeftUserManagement"))
            {
                btnUserManagement.Content = FindResource("SelectUserManagement");
                btnLeftUserManagement.Content = FindResource("UserManagement");
                txtTopMenuTitle.Text = "Manage Users";
                //txtLeftUserManagement.Foreground= Color.FromRgb(15, 34, 28);
            }
        }
        private void txtLeftUserManagement_Clik(object sender, RoutedEventArgs e)
        {
            GridDashboardHeaderBackgroundColorHidden.Visibility = System.Windows.Visibility.Hidden;
            //MainDashboardBgVisibility.Visibility = System.Windows.Visibility.Hidden;
            GridAdminDashboardMainMenuSection.Visibility = System.Windows.Visibility.Hidden;
            GridAllHeaderBackgroundColorVisible.Visibility = System.Windows.Visibility.Visible;
            GridUserManagementPanelStart.Visibility = System.Windows.Visibility.Visible;
            navigationMenu.IsOpen = false;
            if (btnLeftUserManagement.Content == FindResource("LeftUserManagement"))
            {
                btnLeftUserManagement.Content = FindResource("SelectUserManagement");
                btnDashboardIcon.Content = FindResource("UserManagement");
                txtTopMenuTitle.Text = "Manage Users";
            }
        }

        private void btnReportManagement_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
