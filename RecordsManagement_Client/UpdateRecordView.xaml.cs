using RecordsManagement_Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RecordsManagement_Client
{
    /// <summary>
    /// Interaction logic for UpdateRecordView.xaml
    /// </summary>
    public partial class UpdateRecordView : Window
    {
        recordModel updateRecord = null!;
        private readonly Regex _regexPrice = new Regex("[^0-9,.-]+");
        private readonly Regex _regexStock = new Regex("[^0-9-]+");
        private readonly Regex _regexEnglishAlphabet = new Regex("[^A-Za-z0-9_.,-]+$");

        public UpdateRecordView(recordModel recordToUpdate)
        {
            InitializeComponent();
            this.updateRecord = recordToUpdate;
            InitializeFields();
        }

        private void InitializeFields()
        {
            this.tbNewRecordPerformer.Text = updateRecord.Performer;
            this.tbNewRecordTitle.Text = updateRecord.Title;
            this.tbNewRecordPrice.Text = updateRecord.Price.ToString();
            this.tbNewRecordStock.Text = updateRecord.StockCount.ToString();
        }

        private void updateRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ManagementWindow.currentAdmin == null)
            {
                MessageBox.Show("You have to log in to update a record!");
                return;
            }

            UpdateRecordModel recordToUpdate = new UpdateRecordModel()
            {
                UpdateRecordId = this.updateRecord.RecordId
            };

            //New Performer
            if (string.IsNullOrEmpty(tbNewRecordPerformer.Text))
            {
                MessageBox.Show("It seems like there was no Performer typed in.");
                tbNewRecordPerformer.Focus();
                return;
            }
            if (this.updateRecord.Performer != tbNewRecordPerformer.Text)
            {
                if (_regexEnglishAlphabet.IsMatch(tbNewRecordPerformer.Text))
                {
                    MessageBox.Show("Please use only letters from the english alphabet!");
                    tbNewRecordPerformer.Focus();
                    return;
                }

                recordToUpdate.Performer = tbNewRecordPerformer.Text;
            }

            //New Title
            if (string.IsNullOrEmpty(tbNewRecordTitle.Text))
            {
                MessageBox.Show("It seems like there was no Title typed in.");
                tbNewRecordPerformer.Focus();
                return;
            }
            if (this.updateRecord.Title != tbNewRecordTitle.Text)
            {
                if (_regexEnglishAlphabet.IsMatch(tbNewRecordTitle.Text))
                {
                    MessageBox.Show("Please use only letters from the english alphabet!");
                    tbNewRecordTitle.Focus();
                    return;
                }

                recordToUpdate.Title = tbNewRecordTitle.Text;
            }

            //New Price
            if (string.IsNullOrEmpty(tbNewRecordPrice.Text))
            {
                MessageBox.Show("It seems like there was no Price typed in.");
                tbNewRecordPrice.Focus();
                return;
            }
            if (this.updateRecord.Price != double.Parse(tbNewRecordPrice.Text))
            {
                if (_regexPrice.IsMatch(tbNewRecordPrice.Text))
                {
                    MessageBox.Show("It seems like you didn't type in a number.\nPlease use local decimal separators!");
                    tbNewRecordPrice.Text = "";
                    tbNewRecordPrice.Focus();
                    return;
                }
                //because of the thing mentioned in the AddRecords file, I have to pass a wrongly
                //parsed number. But this also will be handled by the API
                recordToUpdate.Price = double.Parse(tbNewRecordPrice.Text);
            }

            //New Stock count
            if (this.updateRecord.StockCount != int.Parse(tbNewRecordStock.Text))
            {
                if (string.IsNullOrEmpty(tbNewRecordStock.Text))
                    recordToUpdate.StockCount = 0;

                if (_regexStock.IsMatch(tbNewRecordStock.Text))
                {
                    MessageBox.Show("It seems like you didn't type in a number.\nPlease use local decimal separators!");
                    tbNewRecordStock.Text = "";
                    tbNewRecordStock.Focus();
                    return;
                }

                recordToUpdate.StockCount = int.Parse(tbNewRecordStock.Text);
            }
            recordToUpdate.AdminName = ManagementWindow.currentAdmin.AdminName;
            recordToUpdate.AdminPass = ManagementWindow.currentAdmin.AdminPass;

            responseModel response = null!;
            try
            {
                response = ManagementWindow.recordsClient.UpdateRecord(recordToUpdate);
                if (response != null)
                {
                    if (response.Error == 0)
                    {
                        MessageBox.Show($"Successfully updated the record!");
                        return;
                    }
                    else
                        MessageBox.Show("There was an error updating!\n" + "Server message: " + response.Message);
                }
                else
                    MessageBox.Show("No response got back from server!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unknown error!\n" + ex.Message);
            }

            this.Close();
        }
    }
}
