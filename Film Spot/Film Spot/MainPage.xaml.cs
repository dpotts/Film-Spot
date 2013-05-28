using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Film_Spot.Resources;
using System.IO;
using Newtonsoft.Json;
using System.ComponentModel;



namespace Film_Spot
{
    public partial class MainPage : PhoneApplicationPage
    {
        string json = "";
        RootObject movies = new RootObject();
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            textBlock2.Text = "Retrieving...";
            string url = @"http://www.reddit.com/r/fullmoviesonyoutube+fullmoviesonvimeo/.json";
            HttpWebRequest hWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            hWebRequest.Method = "GET";
            hWebRequest.BeginGetResponse(Response_Completed, hWebRequest);
        }

        public void Response_Completed(IAsyncResult result)
        {
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            {
                json = streamReader.ReadToEnd();
                movies = JsonConvert.DeserializeObject<RootObject>(json);

            }
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                string movie_titles = "";
                foreach(var movie in movies.data.children)
                {
                    movie_titles = movie_titles + "\n\n" + movie.data.title + "\n" + movie.data.url;
                }
                textBlock2.Text = movie_titles;
            });
        }

        public class MediaEmbed
        {
            public string content { get; set; }
            public int width { get; set; }
            public bool scrolling { get; set; }
            public int height { get; set; }
        }

        public class Oembed
        {
            public string provider_url { get; set; }
            public string description { get; set; }
            public string title { get; set; }
            public string url { get; set; }
            public string type { get; set; }
            public string author_name { get; set; }
            public int height { get; set; }
            public int width { get; set; }
            public string html { get; set; }
            public int thumbnail_width { get; set; }
            public string version { get; set; }
            public string provider_name { get; set; }
            public string thumbnail_url { get; set; }
            public int thumbnail_height { get; set; }
            public string author_url { get; set; }
        }

        public class Media
        {
            public string type { get; set; }
            public Oembed oembed { get; set; }
        }

        public class Data2
        {
            public string domain { get; set; }
            public object banned_by { get; set; }
            public MediaEmbed media_embed { get; set; }
            public string subreddit { get; set; }
            public object selftext_html { get; set; }
            public string selftext { get; set; }
            public object likes { get; set; }
            public string link_flair_text { get; set; }
            public string id { get; set; }
            public bool clicked { get; set; }
            public string title { get; set; }
            public Media media { get; set; }
            public int score { get; set; }
            public object approved_by { get; set; }
            public bool over_18 { get; set; }
            public bool hidden { get; set; }
            public string thumbnail { get; set; }
            public string subreddit_id { get; set; }
            public bool edited { get; set; }
            public string link_flair_css_class { get; set; }
            public string author_flair_css_class { get; set; }
            public int downs { get; set; }
            public bool saved { get; set; }
            public bool is_self { get; set; }
            public string permalink { get; set; }
            public string name { get; set; }
            public double created { get; set; }
            public string url { get; set; }
            public string author_flair_text { get; set; }
            public string author { get; set; }
            public double created_utc { get; set; }
            public int ups { get; set; }
            public int num_comments { get; set; }
            public object num_reports { get; set; }
            public object distinguished { get; set; }
        }

        public class Child
        {
            public string kind { get; set; }
            public Data2 data { get; set; }
        }

        public class Data
        {
            public string modhash { get; set; }
            public List<Child> children { get; set; }
            public string after { get; set; }
            public object before { get; set; }
        }

        public class RootObject
        {
            public string kind { get; set; }
            public Data data { get; set; }
        }

       /* protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (YouTube.CancelPlay()) // used to abort current youtube download
                e.Cancel = true;
            else
            {
                // your code here
            }
            base.OnBackKeyPress(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            YouTube.CancelPlay(); // used to reenable page
            SystemTray.ProgressIndicator.IsVisible = false;
            // your code here
            base.OnNavigatedTo(e);
        }
        */
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           // SystemTray.ProgressIndicator.IsVisible = true;
           // YouTube.Play("youtube_id", true, YouTubeQuality.Quality480P, x =>
           // {
           //     if (x != null)
           //         MessageBox.Show(x.Message);
           // });
        }
    }
}