using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
    /// Interaction logic for ManageAccount.xaml
    /// </summary>
    public partial class ManageAccount : UserControl
    {
        public ManageAccount()
        {
            InitializeComponent();
            InitializeFields();
        }
        private void InitializeFields()
        {
            if (ManagementWindow.currentAdmin != null)
            {
                tbAdmin.Text = ManagementWindow.currentAdmin.AdminName;
                pbAdmin.Password = ManagementWindow.currentAdmin.AdminPass;
                this.tbAdmin.IsEnabled = true;
                this.pbAdmin.IsEnabled = true;
                this.saveAcount.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("There is currently no admin logged in!");
                this.tbAdmin.IsEnabled = false;
                this.pbAdmin.IsEnabled = false;
                this.saveAcount.IsEnabled = false;
            }
        }

        private void saveAcount_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbAdmin.Text) || !string.IsNullOrEmpty(pbAdmin.Password))
            {
                UpdateAdminModel adminToUpdate = new UpdateAdminModel();

                //first if all the two is different
                if (tbAdmin.Text != ManagementWindow.currentAdmin.AdminName &&
                    pbAdmin.Password != ManagementWindow.currentAdmin.AdminPass)
                {
                    adminToUpdate.CurrAdminName = ManagementWindow.currentAdmin.AdminName;
                    adminToUpdate.CurrAdminPass = ManagementWindow.currentAdmin.AdminPass;
                    adminToUpdate.NewAdminName = tbAdmin.Text;
                    adminToUpdate.NewAdminPass = pbAdmin.Password;
                }
                //then if one or the other
                else if (tbAdmin.Text != ManagementWindow.currentAdmin.AdminName)
                {
                    adminToUpdate.CurrAdminName = ManagementWindow.currentAdmin.AdminName;
                    adminToUpdate.CurrAdminPass = ManagementWindow.currentAdmin.AdminPass;
                    adminToUpdate.NewAdminName = tbAdmin.Text;
                }
                else if (pbAdmin.Password != ManagementWindow.currentAdmin.AdminPass)
                {
                    adminToUpdate.CurrAdminName = ManagementWindow.currentAdmin.AdminName;
                    adminToUpdate.CurrAdminPass = ManagementWindow.currentAdmin.AdminPass;
                    adminToUpdate.NewAdminPass = pbAdmin.Password;
                }

                ResponseModel response = null!;
                try
                {
                    response = ManagementWindow.authClient.ChangeAccDetails(adminToUpdate);
                    if (response != null)
                    {
                        if (response.Error == 0)
                        {
                            MessageBox.Show($"Successfully updated account details!");
                            ManagementWindow.currentAdmin = new AdminModel()
                            {
                                AdminName = adminToUpdate.NewAdminName,
                                AdminPass = adminToUpdate.NewAdminPass
                            };
                            return;
                        }
                        else
                            MessageBox.Show("There was an error updating account details!\n" + "Server message: " + response.Message);
                    }
                    else
                        MessageBox.Show("No response got back from server!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unknown error!\n" + ex.Message);
                }
            }


            else
                MessageBox.Show("Didn't type in anything to work with!");
        }
    }
}
