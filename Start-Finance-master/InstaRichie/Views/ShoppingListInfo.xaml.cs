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
    public sealed partial class ShoppingListInfo : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");
        public ShoppingListInfo()
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
            conn.CreateTable<ShoppingList>();
            var query = conn.Table<ShoppingList>();
            Shopping_List.ItemsSource = query.ToList();
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Regex reg = new Regex("^[0-9]*$");

            try
            {
                // checks if Shopping details are null
                if (ShopItemID.Text.ToString() == "" || ShopName.Text.ToString() == "" || NameOfItem.Text.ToString() == ""
                    || ShoppingDate.Date.ToString() == "" || PriceQuoted.Text.ToString() == "" )
                {
                    MessageDialog dialog = new MessageDialog("Empty fields detected", "Please review..!");
                    await dialog.ShowAsync();
                }
                else if (!reg.IsMatch(ShopItemID.Text.ToString()))
                {
                    MessageDialog dialog = new MessageDialog("Special characters and alphabets are NOT allowed in Shopping ID", "Please review..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    // Inserts the data
                    conn.Insert(new ShoppingList()
                    {
                        ItemID = ShopItemID.Text.ToString(),
                        ShopName = ShopName.Text.ToString(),
                        NameOfItem = NameOfItem.Text.ToString(),
                        ShoppingDate = ShoppingDate.Date.ToString("dd/MM/yyyy"),
                        PriceQuoted = PriceQuoted.Text.ToString(),
                     });
                    Results();
                }
            }
            catch (Exception ex)
            {
                if (ex is SQLiteException)
                {
                    MessageDialog dialog = new MessageDialog("ShoppingID already exist, Try Different ID", "Oops..!");
                    await dialog.ShowAsync();
                }
            }
        }

        private async void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog ShowConf = new MessageDialog("Deleting this ShoppingInfo will delete all info of this ShoppingList", "Important");
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
                    string ShoppingLabel = ((ShoppingList)Shopping_List.SelectedItem).ItemID;
                    var querydel = conn.Query<ShoppingList>("DELETE FROM ShoppingList WHERE ID ='" + ShoppingLabel + "'");
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
            Regex reg = new Regex("^[0-9]*$");
            try
            {
                // checks if Shopping's details are null
                if (ShopItemID.Text.ToString() == "" || ShopName.Text.ToString() == "" || NameOfItem.Text.ToString() == ""
                    || ShoppingDate.Date.ToString() == "" || PriceQuoted.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Can't update with empty fields", "Please review..!");
                    await dialog.ShowAsync();
                }
                else
                {

                    conn.Update(new ShoppingList()
                    {
                        ItemID = ShopItemID.Text.ToString(),
                        ShopName = ShopName.Text.ToString(),
                        NameOfItem = NameOfItem.Text.ToString(),
                        ShoppingDate = ShoppingDate.Date.ToString("dd/MM/yyyy"),
                        PriceQuoted = PriceQuoted.Text.ToString(),
                        
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
            ShopItemID.Text = "";
            ShopName.Text = "";
            NameOfItem.Text = "";
            ShoppingDate.Date = DateTime.Parse("01/01/2019");
            PriceQuoted.Text = "";
           
        }

        private void ShoppingList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Shopping = Shopping_List.SelectedItem as ShoppingList;
            if (Shopping != null)
            {
                ShopItemID.Text = Shopping.ItemID;
                ShopName.Text = Shopping.ShopName;
                NameOfItem.Text = Shopping.NameOfItem;
                ShoppingDate.Date = DateTime.Parse(Shopping.ShoppingDate);
                PriceQuoted.Text = Shopping.PriceQuoted;
             }
        }
    }
    
    
}
