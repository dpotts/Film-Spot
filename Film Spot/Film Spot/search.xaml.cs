using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Microsoft.Phone.Controls;


namespace Film_Spot
{
    public partial class search : PhoneApplicationPage
    {

        public search()
        {
            InitializeComponent();
        }

        private void home_click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));

        }

        private void Search_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/browse.xaml?title=" + search_box.Text, UriKind.Relative));
        }

        private void search_box_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                NavigationService.Navigate(new Uri("/browse.xaml?title=" + search_box.Text, UriKind.Relative));
            }
        }

        private void search_clear(object sender, System.Windows.Input.GestureEventArgs e)
        {
            search_box.Text = string.Empty;
        }

        private void logo_tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}