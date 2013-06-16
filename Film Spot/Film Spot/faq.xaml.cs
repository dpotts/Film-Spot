using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Film_Spot
{
    public partial class faq : PhoneApplicationPage
    {
        public faq()
        {
            InitializeComponent();

            playback.Text = "Q: When I try and play the video it says 'The uploader has not made this video available in your country.'\n";
            playback.Text += "A: Some of the videos have been set by the uploader to only work in sme countries.  There is nothing we can do about it.";

            playback.Text += "\n\nQ: When I try and play the video it says it is restricted from playback on certain sites.\n";
            playback.Text += "A: To get it to play, click 'Watch on YouTube'";

            playback.Text += "\n\nQ: I clicked 'Watch on YouTube' and now it says it is not available on mobile and that I have to watch it on PC.\n";
            playback.Text += "A: Change YouTube to Desktop mode by clicking the 3 horizontal lines at top and click 'Desktop'.";

            playback.Text += "\n\nQ: When I try and play the video it says 'The video is currently unavailable'.\n";
            playback.Text += "A: The uploader has deleted the film.  There is nothing we can do about it.";

        }

        private void home_click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void logo_tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}