using RecordsManagement_Client.Components;
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

namespace RecordsManagement_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ManagementWindow : Window
    {

        static internal Admin currentAdmin = null!;

        public ManagementWindow()
        {
            InitializeComponent();
        }

        private void tbtnReadAll_Checked(object sender, RoutedEventArgs e)
        {
            if (tbtnReadAll.IsChecked == true)
            {
                tbtnAccount.IsChecked = false;
                tbtnLogin.IsChecked = false;
                tbtnAdd.IsChecked = false;
            }
            mainGrid.Children.Clear();

            AllRecords newAllRecords = new AllRecords();
            mainGrid.Children.Add(newAllRecords);
        }

        //tbtnAdd_Checked event -> AddRecord User Control render!
        private void tbtnAdd_Checked(object sender, RoutedEventArgs e)
        {
            if (tbtnAdd.IsChecked == true)
            {
                tbtnAccount.IsChecked = false;
                tbtnLogin.IsChecked = false;
                tbtnReadAll.IsChecked = false;
            }
            mainGrid.Children.Clear();

            AddRecord addRecordGrid = new AddRecord();
            mainGrid.Children.Add(addRecordGrid);
        }

        //tbtnAccount_Checked -> ManageAdminAccount render, id passzolása!
        private void tbtnAccount_Checked(object sender, RoutedEventArgs e)
        {
            if (tbtnAccount.IsChecked == true)
            {
                tbtnAdd.IsChecked = false;
                tbtnLogin.IsChecked = false;
                tbtnReadAll.IsChecked = false;
            }

            mainGrid.Children.Clear();
            ManageAccount adminAccount = new ManageAccount();
            mainGrid.Children.Add(adminAccount);
        }

        private void tbtnLogin_Checked(object sender, RoutedEventArgs e)
        {

            if (tbtnLogin.IsChecked == true)
            {
                tbtnAdd.IsChecked = false;
                tbtnAccount.IsChecked = false;
                tbtnReadAll.IsChecked = false;
            }

            mainGrid.Children.Clear();

            if (currentAdmin != null)
            {
                Logout logoutGrid = new Logout();
                mainGrid.Children.Add(logoutGrid);
            }
            else
            {
                Login loginGrid = new Login();
                mainGrid.Children.Add(loginGrid);
            }
        }
    }
}
