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

namespace RecordsManagement_Client.Components
{
    /// <summary>
    /// Interaction logic for Logout.xaml
    /// </summary>
    public partial class Logout : UserControl
    {
        public Logout()
        {
            InitializeComponent();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to log out?", "Logout",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                AdminModel logoutAttempt = new AdminModel()
                {
                    AdminName = tbAdmin.Text,
                    AdminPass = pbAdmin.Password
                };

                ResponseModel response = null!;
                try
                {
                    response = ManagementWindow.authClient.Logout(logoutAttempt);
                    if (response != null)
                    {
                        if (response.Error == 0)
                        {
                            MessageBox.Show($"Successfully logged out!");
                            ManagementWindow.currentAdmin = null!;
                            return;
                        }
                        else
                            MessageBox.Show("There was an error logging out!\n" + "Server message: " + response.Message);
                    }
                    else
                        MessageBox.Show("No response got back from server!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unknown error!\n" + ex.Message);
                }
                ManagementWindow.currentAdmin = null!;
            }
            else
                return;
        }
    }
}
