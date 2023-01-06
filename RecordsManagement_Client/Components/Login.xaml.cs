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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            AdminModel loginAttempt = new AdminModel()
            {
                AdminName = tbAdmin.Text,
                AdminPass = pbAdmin.Password
            };

            ResponseModel response = null!;
            try
            {
                response = ManagementWindow.authClient.Login(loginAttempt);
                if (response != null)
                {
                    if (response.Error == 0)
                    {
                        MessageBox.Show($"Successfully logged in!");
                        ManagementWindow.currentAdmin = loginAttempt;
                        return;
                    }
                    else
                        MessageBox.Show("There was an error logging in!\n" + "Server message: " + response.Message);
                }
                else
                    MessageBox.Show("No response got back from server!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unknown error!\n" + ex.Message);
            }
        }
    }
}
