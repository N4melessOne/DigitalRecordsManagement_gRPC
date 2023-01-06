using RecordsManagement_Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
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
    /// Interaction logic for AllRecords.xaml
    /// </summary>
    public partial class AllRecords : UserControl
    {
        public AllRecords()
        {
            InitializeComponent();
            RefreshRecordsGrid();
        }

        private async void RefreshRecordsGrid()
        {
            List<Record> allRecords = new List<Record>();
            /*using (var call = ManagementWindow.recordsClient.AllRecords(new allRecordsRequest()))
            {
                while (await call.ResponseStream.MoveNext(new CancellationToken()))
                {
                    allRecords.Add(call.ResponseStream.Current);
                }
            }*/

            dataGrid.ItemsSource = allRecords;
        }

        private void btnUpdateRecord_Click(object sender, RoutedEventArgs e)
        {
            if (ManagementWindow.currentAdmin == null)
            {
                MessageBox.Show("There is no admin currently logged in!");
                return;
            }
            if ((dataGrid.SelectedItem as Record) != null)
            {
                //gRPC
                /*if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    MessageBox.Show(response.StatusDescription);
                else
                {
                    Record recordFromGet = JsonSerializer.Deserialize<Record>(response.Content!)!;
                    UpdateRecordView updateRecordView = new UpdateRecordView(recordFromGet);
                    updateRecordView.ShowDialog();

                    RefreshRecordsGrid();
                }*/
            }
            else
                MessageBox.Show("No records selected!");
        }

        private void btnDeleteRecord_Click(object sender, RoutedEventArgs e)
        {
            if (ManagementWindow.currentAdmin == null)
            {
                MessageBox.Show("There is no admin currently logged in!");
                return;
            }

            if ((dataGrid.SelectedItem as Record) != null)
            {
                var result = MessageBox.Show($"Are you sure to delete {(dataGrid.SelectedItem as Record)!.Performer} - {(dataGrid.SelectedItem as Record)!.Title}?",
                                    "DELETE", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    /*
                    Dictionary<string, object> jsonObject = new Dictionary<string, object>();
                    jsonObject.Add("current_admin_name", ManegementWindow.currentAdmin!.AdminName);
                    jsonObject.Add("current_admin_password", ManegementWindow.currentAdmin!.AdminPass);
                    jsonObject.Add("recordid_to_delete", (dataGrid.SelectedItem as Record)!.Id);

                    var json = JsonSerializer.Serialize(jsonObject, typeof(Dictionary<string, object>));
                    request.AddBody(json);

                    var response = ManegementWindow.Client.Delete(request);

                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        MessageBox.Show(response.StatusDescription);
                    else
                    {
                        Response responseFromDelete = ManegementWindow.Client.Deserialize<Response>(response).Data!;
                        if (responseFromDelete.Error == 0 && responseFromDelete.Message == "Deleted successfully!")
                        {
                            MessageBox.Show(responseFromDelete.Message);
                            RefreshRecordsGrid();
                        }
                        else
                        {
                            MessageBox.Show(responseFromDelete.Message);
                        }
                    }*/
                }
                else
                    return;
            }
            else
                MessageBox.Show("There is no record selected!");
        }
    }
}
