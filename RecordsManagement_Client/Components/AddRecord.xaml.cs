using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AddRecord.xaml
    /// </summary>
    public partial class AddRecord : UserControl
    {
        private readonly Regex _regexPrice = new Regex("[^0-9,.-]+");
        private readonly Regex _regexStock = new Regex("[^0-9-]+");
        private readonly Regex _regexEnglishAlphabet = new Regex("[^A-Za-z0-9_.,-]+$");

        public AddRecord()
        {
            InitializeComponent();
        }

        private void addRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ManagementWindow.currentAdmin == null)
            {
                MessageBox.Show("You have to log in to add a new record!");
                return;
            }

            //this dictionary should be replaced!
            //Dictionary<string, object> jsonObject = new Dictionary<string, object>();
            //jsonObject.Add("current_admin_name", ManagementWindow.currentAdmin.AdminName);
            //jsonObject.Add("current_admin_password", ManagementWindow.currentAdmin.AdminPass);

            //New Performer
            if (string.IsNullOrEmpty(tbNewRecordPerformer.Text))
            {
                MessageBox.Show("It seems like there was no Performer typed in.");
                tbNewRecordPerformer.Focus();
                return;
            }
            if (_regexEnglishAlphabet.IsMatch(tbNewRecordPerformer.Text))
            {
                MessageBox.Show("Please use only letters from the english alphabet!");
                tbNewRecordPerformer.Focus();
                return;
            }
            //jsonObject.Add("new_record_performer", tbNewRecordPerformer.Text);

            //New Title
            if (string.IsNullOrEmpty(tbNewRecordTitle.Text))
            {
                MessageBox.Show("It seems like there was no Title typed in.");
                tbNewRecordTitle.Focus();
                return;
            }
            if (_regexEnglishAlphabet.IsMatch(tbNewRecordTitle.Text))
            {
                MessageBox.Show("Please use only letters from the english alphabet!");
                tbNewRecordPerformer.Focus();
                return;
            }
            //jsonObject.Add("new_record_title", tbNewRecordTitle.Text);

            //New Price
            if (string.IsNullOrEmpty(tbNewRecordPrice.Text))
            {
                MessageBox.Show("It seems like there was no Price typed in.");
                tbNewRecordPrice.Focus();
                return;
            }
            if (_regexPrice.IsMatch(tbNewRecordPrice.Text))
            {
                MessageBox.Show("It seems like you didn't type in a number.\nPlease use local decimal separators!");
                tbNewRecordPrice.Text = "";
                tbNewRecordPrice.Focus();
                return;
            }

            //jsonObject.Add("new_record_price", double.Parse(tbNewRecordPrice.Text));

            //New Stock(optional)
            if (!string.IsNullOrEmpty(tbNewRecordStock.Text))
            {
                if (_regexStock.IsMatch(tbNewRecordStock.Text))
                {
                    MessageBox.Show("It seems like you didn't type in a number.");
                    tbNewRecordStock.Text = "";
                    tbNewRecordStock.Focus();
                    return;
                }
                //jsonObject.Add("new_record_stock", int.Parse(tbNewRecordStock.Text));
            }
            else
                //jsonObject.Add("new_record_stock", 0);

            //check if all parameters are fulfilled
            /*if (jsonObject.Count != 6) //6 rows should be in the Dictionary!
            {
                MessageBox.Show("Missing Parameters!");
                return;
            }*/

            //gRPC call

            tbNewRecordPerformer.Text = "";
            tbNewRecordTitle.Text = "";
            tbNewRecordPrice.Text = "";
            tbNewRecordStock.Text = "";
        }
    }
}
