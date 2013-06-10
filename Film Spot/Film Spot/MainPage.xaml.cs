using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Film_Spot.Model;
using Film_Spot.ViewModels;
using Microsoft.Phone.Tasks;
using System.Windows.Media;

using System.Linq;


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
    }
}