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
    public sealed partial class AppointmentsPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public AppointmentsPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            /// Initializing a database
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);

            // Creating table
            Results();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
        }

        public void Results()
        {
            // Creating table
            conn.CreateTable<Appointments>();
            var query = conn.Table<Appointments>();
            TransactionList.ItemsSource = query.ToList();
        }


        private async void AddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // checks if Appointment's details are null
                if (AppID.Text.ToString() == "" || AppName.Text.ToString() == "" || AppLoc.Text.ToString() == ""
                    || AppDate.Date.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Empty fields detected", "Please review..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.Insert(new Appointments()
                    {
                        ID = AppID.Text.ToString(),
                        EventName = AppName.Text.ToString(),
                        Location = AppLoc.Text.ToString(),
                        EventDate = AppDate.Date.ToString("dd/MM/yyyy"),
                        StartTime = StartTime.Time.ToString(),
                        EndTime = EndTime.Time.ToString()
                    });
                    Results();
                }
            }
            catch (Exception ex)
            {
                if (ex is SQLiteException)
                {
                    MessageDialog dialog = new MessageDialog("Appointment ID already exist, Try Different ID", "Oops..!");
                    await dialog.ShowAsync();
                }
            }
        }

        private async void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog ShowConf = new MessageDialog("Deleting this Appointment will delete all its information", "Important");
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
                    string Appmnt = ((Appointments)TransactionList.SelectedItem).ID;
                    var querydel = conn.Query<Appointments>("DELETE FROM Appointments WHERE ID ='" + Appmnt + "'");
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
                // checks if Appointment's details are null
                if (AppID.Text.ToString() == "" || AppName.Text.ToString() == "" || AppLoc.Text.ToString() == ""
                    || AppDate.Date.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Can't update with empty fields", "Please review..!");
                    await dialog.ShowAsync();
                }
                else
                {
                  
                    conn.Update(new Appointments()
                    {
                        ID = AppID.Text.ToString(),
                        EventName = AppName.Text.ToString(),
                        Location = AppLoc.Text.ToString(),
                        EventDate = AppDate.Date.ToString("dd/MM/yyyy"),
                        StartTime = StartTime.Time.ToString(),
                        EndTime = EndTime.Time.ToString()
                    });
                    Results();
                }
            }
            catch (Exception ex)
            {
                if (ex is SQLiteException)
                {
                    MessageDialog dialog = new MessageDialog("Appointment ID already exist, Try Different ID", "Oops..!");
                    await dialog.ShowAsync();
                }
            }
        }

        private void TransactionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Appmnt = TransactionList.SelectedItem as Appointments;
            if (Appmnt != null)
            {
                // FirstNametxtBox.Text = ((Appointments)AppointmentsView.SelectedItem).FirstName;
                AppID.Text = Appmnt.ID;
                AppName.Text = Appmnt.EventName;
                AppLoc.Text = Appmnt.Location;
                AppDate.Date = DateTime.Parse(Appmnt.EventDate);
                StartTime.Time = TimeSpan.Parse(Appmnt.StartTime);
                EndTime.Time = TimeSpan.Parse(Appmnt.StartTime);
            }
        }

        // Clears the fields
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            AppID.Text = "";
            AppName.Text = "";
            AppLoc.Text = "";            
            AppDate.Date = DateTime.Parse("01/01/2100");
            StartTime.Time = TimeSpan.Parse("12:00:00");
            EndTime.Time = TimeSpan.Parse("12:00:00");              
        }

        // Displays the data when navigation between pages
    }
}
