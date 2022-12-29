using RecordsManagement_Client.Model;
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
                /*
                Dictionary<string, string> jsonObject = new Dictionary<string, string>();
                var request = new RestRequest(ManegementWindow.RestURL + "/login.php", Method.Put);

                //first if all the two is different
                if (tbAdmin.Text != ManegementWindow.currentAdmin.AdminName &&
                    pbAdmin.Password != ManegementWindow.currentAdmin.AdminPass)
                {
                    jsonObject.Add("current_admin_name", ManegementWindow.currentAdmin.AdminName);
                    jsonObject.Add("current_admin_password", ManegementWindow.currentAdmin.AdminPass);
                    jsonObject.Add("new_admin_name", tbAdmin.Text);
                    jsonObject.Add("new_admin_password", pbAdmin.Password);
                    var json = JsonSerializer.Serialize(jsonObject, typeof(Dictionary<string, string>));

                    request.AddBody(json);
                }
                //then if one or the other
                else if (tbAdmin.Text != ManegementWindow.currentAdmin.AdminName)
                {
                    jsonObject.Add("current_admin_name", ManegementWindow.currentAdmin.AdminName);
                    jsonObject.Add("current_admin_password", ManegementWindow.currentAdmin.AdminPass);
                    jsonObject.Add("new_admin_name", tbAdmin.Text);
                    var json = JsonSerializer.Serialize(jsonObject, typeof(Dictionary<string, string>));

                    request.AddBody(json);
                }
                else if (pbAdmin.Password != ManegementWindow.currentAdmin.AdminPass)
                {
                    jsonObject.Add("current_admin_name", ManegementWindow.currentAdmin.AdminName);
                    jsonObject.Add("current_admin_password", ManegementWindow.currentAdmin.AdminPass);
                    jsonObject.Add("new_admin_password", pbAdmin.Password);
                    var json = JsonSerializer.Serialize(jsonObject, typeof(Dictionary<string, string>));

                    request.AddBody(json);
                }

                var response = ManegementWindow.Client.Put(request);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    MessageBox.Show(response.StatusDescription);
                else
                {
                    Response responseFromPut = ManegementWindow.Client.Deserialize<Response>(response).Data!;
                    if (responseFromPut.Error == 0 && responseFromPut.Message == "Updated succsessfully!")
                    {
                        ManegementWindow.currentAdmin = new Admin(tbAdmin.Text, pbAdmin.Password);
                        InitializeFields();
                        MessageBox.Show(responseFromPut.Message);
                    }
                    else
                    {
                        MessageBox.Show(responseFromPut.Message);
                    }
                }
            }


            else
                MessageBox.Show("Didn't type in anything to work with!");
                */
            }
        }
    }
}
