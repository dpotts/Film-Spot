using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Diagnostics;
using System.Windows;
using System.Linq;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Tasks;

namespace Film_Spot
{
    public partial class MainPage : PhoneApplicationPage
    {

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("MainPage.xaml"));
            if (tile != null)
            {
                ApplicationBarIconButton btn = (ApplicationBarIconButton)ApplicationBar.Buttons[0];

                btn.IsEnabled = false;
            }

            string directoryName = "FilmSpot";
            string fileName = "data.txt";
            string visit_count = "1";

            try
            {
                IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
                if (!string.IsNullOrEmpty(directoryName) && !myIsolatedStorage.DirectoryExists(directoryName))
                {
                    myIsolatedStorage.CreateDirectory(directoryName);
                    using (StreamWriter writeFile = new StreamWriter(new IsolatedStorageFileStream(directoryName + "\\" + fileName, FileMode.Create, FileAccess.Write, myIsolatedStorage)))
                    {
                        writeFile.WriteLine(visit_count);
                        writeFile.Close();
                    }
                }
                else
                {
                    IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(directoryName + "\\" + fileName, FileMode.Open, FileAccess.Read);
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        visit_count = reader.ReadLine();
                    }

                    int visit_counter = Convert.ToInt32(visit_count);

                    if (visit_counter <= 5)
                    {

                        if (visit_counter == 5)
                        {
                            Popup popup = new Popup();
                            popup.Height = 1280;
                            popup.Width = 768;
                            popup.VerticalOffset = 20;
                            PopupUserControl control = new PopupUserControl();
                            popup.Child = control;
                            popup.IsOpen = true;
                            this.ApplicationBar.IsVisible = false;


                            control.btnOK.Tap += (s, args) =>
                            {
                                popup.IsOpen = false;
                                this.ApplicationBar.IsVisible = true;

                                MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
                                marketplaceReviewTask.Show();
                            };

                            control.btnCancel.Tap += (s, args) =>
                            {
                                popup.IsOpen = false;
                                this.ApplicationBar.IsVisible = true;
                                MessageBox.Show("If you wish to rate FilmSpot in the future, simply click on the rate button in the application bar.");
                            };
                        }

                        visit_counter++;
                        using (IsolatedStorageFileStream file = myIsolatedStorage.OpenFile(directoryName + "\\" + fileName, FileMode.Open, FileAccess.Write))
                        {
                            using (StreamWriter writer = new StreamWriter(file))
                            {
                                writer.Write(visit_counter.ToString());
                                writer.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle the exception
            }



        }

        private void browse_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/browse.xaml", UriKind.Relative));
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/search.xaml", UriKind.Relative));
        }

        private void pin_Click(object sender, EventArgs e)
        {
            ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("MainPage.xaml"));

            if (tile == null)
            {
                IconicTileData icontile = new IconicTileData();
                icontile.Title = "FilmSpot";
                //icontile.Count = ;

                icontile.IconImage = new Uri("Assets/Tiles/IconicTileMediumLarge.png", UriKind.Relative);
                icontile.SmallIconImage = new Uri("Assets/Tiles/IconicTileSmall.png", UriKind.Relative);
                //icontile.WideContent1 = "";
                //icontile.WideContent2 = "";
                // icontile.WideContent3 = "";

                ShellTile.Create(new Uri("/MainPage.xaml", UriKind.Relative), icontile, true);
            }
            else
            {
                MessageBox.Show("Already Pinned To Start");
            }
        }

        private void rate_Click(object sender, EventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();

            marketplaceReviewTask.Show();
        }

        private void faq_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/faq.xaml", UriKind.Relative));
        }

        private void search_tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/search.xaml", UriKind.Relative));
        }


        private void browse_tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/browse.xaml", UriKind.Relative));
        }
    }
}