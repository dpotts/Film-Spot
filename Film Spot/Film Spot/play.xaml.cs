using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Film_Spot
{
    public partial class play : PhoneApplicationPage
    {
        public play()
        {
            InitializeComponent();
        }

         protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string url;

            if (NavigationContext.QueryString.TryGetValue("url", out url))
            {

                browser.NavigateToString("<!doctype html>" +
                                            "<html><head><title></title></head><body style=\"margin:0px;padding:0px;overflow:hidden;min-height: 100%\">" +
                                            "<iframe src=\"" + url + "\" frameborder=\"0\" style=\"overflow:hidden;overflow-x:hidden;overflow-y:hidden;height:100%;width:100%;position:absolute;top:0px;left:0px;right:0px;bottom:0px\" width=\"100%\" height=\"100%\" allowfullscreen></iframe>" +
                                            "</body></html>");
            }
        }

         private void logo_tap(object sender, System.Windows.Input.GestureEventArgs e)
         {
             NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
         }
    }
}