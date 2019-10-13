using SQLite.Net;
using StartFinance.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StartFinance.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContactDetail : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");
        public ContactDetail()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            /// Initializing a database
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);

            // Creating table
            Results();
        }
        public void Results()
        {
            // Creating table
            conn.CreateTable<Contact>();
            var query = conn.Table<Contact>();
            TransactionList.ItemsSource = query.ToList();
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Regex reg = new Regex("^[0-9]*$");

            try
            {
                // checks if Shopping details are null
                if (ContactID.Text.ToString() == "" || FirstName.Text.ToString() == "" || LastName.Text.ToString() == ""
                    || CompanyName.Text.ToString() == "" || MobilePhone.Text.ToString() == "" )
                {
                    MessageDialog dialog = new MessageDialog("Empty fields detected", "Please review..!");
                    await dialog.ShowAsync();
                }
                else if (!reg.IsMatch(ContactID.Text.ToString()))
                {
                    MessageDialog dialog = new MessageDialog("Special characters and alphabets are NOT allowed in Contact ID", "Please review..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    // Inserts the data
                    conn.Insert(new Contact()
                    {
                        ContactID = ContactID.Text.ToString(),
                        FirstName = FirstName.Text.ToString(),
                        LastName = LastName.Text.ToString(),
                        CompanyName = CompanyName.Text.ToString(),
                        MobilePhone = MobilePhone.Text.ToString(),
                     });
                    Results();
                }
            }
            catch (Exception ex)
            {
                if (ex is SQLiteException)
                {
                    MessageDialog dialog = new MessageDialog("ContactID already exist, Try Different ID", "Oops..!");
                    await dialog.ShowAsync();
                }
            }
        }

        private async void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog ShowConf = new MessageDialog("Deleting this ContactInfo will delete all info of this Contact", "Important");
            ShowConf.Commands.Add(new UICommand("Yes, Delete")
            {
                Id = 0
            });
            ShowConf.Commands.Add(new UICommand("Cancel")
            {
                Id = 1
            });
            ShowConf.DefaultCommandIndex = 0;
            ShowConf.CancelCommandIndex = 1;

            var result = await ShowConf.ShowAsync();
            if ((int)result.Id == 0)
            {
                // checks if data is null else inserts
                try
                {
                    string ContactLabel = ((Contact)TransactionList.SelectedItem).ContactID;
                    var querydel = conn.Query<Contact>("DELETE FROM Contact WHERE ID ='" + ContactLabel + "'");
                    Results();
                }
                catch (NullReferenceException)
                {
                    MessageDialog ClearDialog = new MessageDialog("Please select the item to Delete", "Oops..!");
                    await ClearDialog.ShowAsync();
                }
            }
            else
            {
                //
            }
        }

        private async void UpdateItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // checks if ShoppingList's details are null
                if (ContactID.Text.ToString() == "" || FirstName.Text.ToString() == "" || LastName.Text.ToString() == ""
                    || CompanyName.Text.ToString() == "" || MobilePhone.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Can't update with empty fields", "Please review..!");
                    await dialog.ShowAsync();
                }
                else
                {

                    conn.Update(new Contact()
                    {
                        ContactID = ContactID.Text.ToString(),
                        FirstName = FirstName.Text.ToString(),
                        LastName = LastName.Text.ToString(),
                        CompanyName = CompanyName.Text.ToString(),
                        MobilePhone = MobilePhone.Text.ToString(),
                        
                    });
                    Results();
                }
            }
            catch (Exception ex)
            {
                if (ex is SQLiteException)
                {
                    MessageDialog dialog = new MessageDialog("ShoppingList ID already exist, Try Different ID", "Oops..!");
                    await dialog.ShowAsync();
                }
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ContactID.Text = "";
            FirstName.Text = "";
            LastName.Text = "";
            CompanyName.Text = "";
            MobilePhone.Text = "";
           
        }

        private void TransactionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Appmnt = TransactionList.SelectedItem as Contact;
            if (Appmnt != null)
            {
                // FirstNametxtBox.Text = ((Appointments)AppointmentsView.SelectedItem).FirstName;
                ContactID.Text = Appmnt.ContactID;
                FirstName.Text = Appmnt.FirstName;
                LastName.Text = Appmnt.LastName;
                CompanyName.Text = Appmnt.CompanyName;
                MobilePhone.Text = Appmnt.MobilePhone;
                
            }
        }
    }
    
    
}
