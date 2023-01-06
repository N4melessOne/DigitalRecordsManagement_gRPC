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
            List<recordModel> allRecords = new List<recordModel>();
            using (var call = ManagementWindow.recordsClient.AllRecords(new allRecordsRequest()))
            {
                while (await call.ResponseStream.MoveNext(new CancellationToken()))
                {
                    allRecords.Add(call.ResponseStream.Current);
                }
            }

            dataGrid.ItemsSource = allRecords;
        }

        private void btnUpdateRecord_Click(object sender, RoutedEventArgs e)
        {
            if (ManagementWindow.currentAdmin == null)
            {
                MessageBox.Show("There is no admin currently logged in!");
                return;
            }

            if ((dataGrid.SelectedItem as recordModel) != null)
            {
                //gRPC
                try
                {
                    int updateRecordId = (dataGrid.SelectedItem as recordModel)!.RecordId;
                    recordModel updateRecord = ManagementWindow.recordsClient.GetRecordById(new IdOfRecord
                    {
                        RecordId = updateRecordId
                    });
                    if (updateRecord != null)
                    {
                        UpdateRecordView updateRecordView = new UpdateRecordView(updateRecord);
                        updateRecordView.ShowDialog();

                        RefreshRecordsGrid();
                    }
                    else
                        MessageBox.Show("The selected record is currently not available!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Other error!\n" + ex.Message);
                }
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

            if ((dataGrid.SelectedItem as recordModel) != null)
            {
                var result = MessageBox.Show($"Are you sure to delete {(dataGrid.SelectedItem as recordModel)!.Performer} - {(dataGrid.SelectedItem as recordModel)!.Title}?",
                                    "DELETE", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    int deleteRecordId = (dataGrid.SelectedItem as recordModel)!.RecordId;
                    DeleteRecordModel recordToDelete = new DeleteRecordModel();
                    recordToDelete.AdminName = ManagementWindow.currentAdmin.AdminName;
                    recordToDelete.AdminPass = ManagementWindow.currentAdmin.AdminPass;
                    recordToDelete.DeleteRecordId = deleteRecordId;

                    //gRPC
                    responseModel response = null!;
                    try
                    {
                        response = ManagementWindow.recordsClient.DeleteRecord(recordToDelete);
                        if (response != null)
                        {
                            if (response.Error == 0)
                            {
                                MessageBox.Show($"Successfully deleted a record!");
                            }
                            else
                                MessageBox.Show("There was an error deleting!\n" + "Server message: " + response.Message);
                        }
                        else
                            MessageBox.Show("No response got back from server!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Unknown error!\n" + ex.Message);
                    }

                    RefreshRecordsGrid();
                }
                else
                    return;
            }
            else
                MessageBox.Show("There is no record selected!");
        }
    }
}
