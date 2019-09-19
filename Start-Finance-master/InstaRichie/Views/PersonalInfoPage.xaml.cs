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
    public sealed partial class PersonalInfoPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public PersonalInfoPage()
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
            conn.CreateTable<Personal>();
            var query = conn.Table<Personal>();
            PersonList.ItemsSource = query.ToList();
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Regex reg = new Regex("^[0-9]*$");

            try
            {
                // checks if Personal details are null
                if (PersID.Text.ToString() == "" || FirstName.Text.ToString() == "" || LastName.Text.ToString() == ""
                    || DOB.Date.ToString() == "" || Gender.Text.ToString() == "" || Email.Text.ToString() == ""
                    || MobileNum.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Empty fields detected", "Please review..!");
                    await dialog.ShowAsync();
                }
                // Checks if mobile number contains special characters
                else if (!reg.IsMatch(MobileNum.Text.ToString()))
                {
                    MessageDialog dialog = new MessageDialog("Special characters and alphabets are NOT allowed in mobile number", "Please review..!");
                    await dialog.ShowAsync();
                }
                // Checks if personad ID contains special characters
                else if (!reg.IsMatch(PersID.Text.ToString()))
                {
                    MessageDialog dialog = new MessageDialog("Special characters and alphabets are NOT allowed in personal ID", "Please review..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    // Inserts the data
                    conn.Insert(new Personal()
                    {
                        ID = PersID.Text.ToString(),
                        FirstName = FirstName.Text.ToString(),
                        LastName = LastName.Text.ToString(),
                        DOB = DOB.Date.ToString("dd/MM/yyyy"),
                        Gender = Gender.Text.ToString(),
                        Email = Email.Text.ToString(),
                        MobileNum = MobileNum.Text.ToString()
                    });
                    Results();
                }
            }
            catch (Exception ex)
            {   
                if (ex is SQLiteException)
                {
                    MessageDialog dialog = new MessageDialog("Personal ID already exist, Try Different ID", "Oops..!");
                    await dialog.ShowAsync();
                }
            }
        }

        // Clears the fields
        private async void ClearFileds_Click(object sender, RoutedEventArgs e)
        {
            PersID.Text = "";
            FirstName.Text = "";
            LastName.Text = "";
            DOB.Date = DateTime.Parse("01/01/2000");
            Gender.Text = "";
            Email.Text = "";
            MobileNum.Text = "";
            MessageDialog ClearDialog = new MessageDialog("Cleared", "Text fields cleared");
            await ClearDialog.ShowAsync();
        }

        // Displays the data when navigation between pages
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
        }

        private async void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog ShowConf = new MessageDialog("Deleting this Person will delete all info of this person", "Important");
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
                    string PersonLabel = ((Personal)PersonList.SelectedItem).ID;
                    var querydel = conn.Query<Personal>("DELETE FROM Personal WHERE ID ='" + PersonLabel + "'");
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

        private async void updateButton_Click(object sender, RoutedEventArgs e)
        {
            Regex reg = new Regex("^[0-9]*$");

            try
            {
                // checks if Personal details are null
                if (PersID.Text.ToString() == "" || FirstName.Text.ToString() == "" || LastName.Text.ToString() == ""
                    || DOB.Date.ToString() == "" || Gender.Text.ToString() == "" || Email.Text.ToString() == ""
                    || MobileNum.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Empty fields detected", "Please review..!");
                    await dialog.ShowAsync();
                }
                // Checks if mobile number contains special characters
                else if (!reg.IsMatch(MobileNum.Text.ToString()))
                {
                    MessageDialog dialog = new MessageDialog("Special characters and alphabets are NOT allowed in mobile number", "Please review..!");
                    await dialog.ShowAsync();
                }
                // Checks if personad ID contains special characters
                else if (!reg.IsMatch(PersID.Text.ToString()))
                {
                    MessageDialog dialog = new MessageDialog("Special characters and alphabets are NOT allowed in personal ID", "Please review..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    // Updates existing record with new data
                    conn.Update(new Personal()
                    {
                        ID = PersID.Text.ToString(),
                        FirstName = FirstName.Text.ToString(),
                        LastName = LastName.Text.ToString(),
                        DOB = DOB.Date.ToString("dd/MM/yyyy"),
                        Gender = Gender.Text.ToString(),
                        Email = Email.Text.ToString(),
                        MobileNum = MobileNum.Text.ToString()
                    });
                    Results();
                }
            }
            catch (Exception ex)
            {
                if (ex is SQLiteException)
                {
                    MessageDialog dialog = new MessageDialog("Personal ID already exist, Try Different ID", "Oops..!");
                    await dialog.ShowAsync();
                }
            }
        }

        // Update fields with existing record
        private void PersonList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Person = PersonList.SelectedItem as Personal;
            if (Person != null)
            {
                PersID.Text = Person.ID;
                FirstName.Text = Person.FirstName;
                LastName.Text = Person.LastName;
                DOB.Date = DateTime.Parse(Person.DOB);
                Gender.Text = Person.Gender;
                Email.Text = Person.Email;
                MobileNum.Text = Person.MobileNum;
            }

        }
    }
}
