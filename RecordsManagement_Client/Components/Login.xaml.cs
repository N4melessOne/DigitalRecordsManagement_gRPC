using RecordsManagement_Client.Model;
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
            /*
            var response = ManagementWindow.Client.Post(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                MessageBox.Show(response.StatusDescription);
            else
            {
                Response responseFromPost = ManegementWindow.Client.Deserialize<Response>(response).Data!;
                if (responseFromPost.Error == 0 && responseFromPost.Message == "Succesfully logged in!")
                {
                    ManegementWindow.currentAdmin = new Admin(tbAdmin.Text, pbAdmin.Password);
                    MessageBox.Show(responseFromPost.Message);
                }
                else
                {
                    MessageBox.Show(responseFromPost.Message);
                }
            }*/
        }
    }
}
