using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Newtonsoft.Json;

namespace Film_Spot
{
    public partial class play : PhoneApplicationPage
    {
        string id = "";
        string link = "";

        public class RootObject_Details
        {
            public string Title { get; set; }
            public string Year { get; set; }
            public string Rated { get; set; }
            public string Released { get; set; }
            public string Runtime { get; set; }
            public string Genre { get; set; }
            public string Director { get; set; }
            public string Writer { get; set; }
            public string Actors { get; set; }
            public string Plot { get; set; }
            public string Poster { get; set; }
            public string imdbRating { get; set; }
            public string imdbVotes { get; set; }
            public string tomatoRating { get; set; }
            public string imdbID { get; set; }
            public string Type { get; set; }
            public string Response { get; set; }
        }

        public play()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);


            if (NavigationContext.QueryString.TryGetValue("id", out id))
            {
                
                    string url = "http://www.omdbapi.com/?i=" + id + "&tomatoes=true&plot=full";

                    HttpWebRequest hWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                    hWebRequest.Method = "GET";
                    hWebRequest.BeginGetResponse(Response_Movie_Details, hWebRequest);
            }

            NavigationContext.QueryString.TryGetValue("url", out link);
        }

        public void Response_Movie_Details(IAsyncResult result)
        {
            //Debug.WriteLine("got a response for details");
            RootObject_Details movie_info = new RootObject_Details();
            try
            {
                HttpWebRequest request = (HttpWebRequest)result.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string json_details = "";
                    json_details = streamReader.ReadToEnd();
                    movie_info = JsonConvert.DeserializeObject<RootObject_Details>(json_details);

                }
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    if(movie_info.Poster != "N/A")
                        poster.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(movie_info.Poster, UriKind.Absolute));
                    title.Text = movie_info.Title + " (" + movie_info.Year + ")";
                    rating.Text = movie_info.Rated;
                    plot.Text = movie_info.Plot;
                    genre.Text = movie_info.Genre;
                    imdb.Text = movie_info.imdbRating;
                    tomato.Text = movie_info.tomatoRating;
                    runtime.Text = movie_info.Runtime;
                    writers.Text = movie_info.Writer;
                    directors.Text = movie_info.Director;
                    actors.Text = movie_info.Actors;
                });
            }
            catch (Exception e)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("Network error occured getting movie details" + e.Message);
                });
            }
        }

        private void play_clicked(object sender, EventArgs e)
        {
            //MessageBox.Show("play " + selected.Title);
            WebBrowserTask webBrowserTask = new WebBrowserTask();

            string cleaned_url = link.Replace("http://", "");
            cleaned_url = cleaned_url.Replace("https://", "");
            cleaned_url = cleaned_url.Replace("www.", "");
            cleaned_url = cleaned_url.Replace("m.", "");
            var parts = cleaned_url.Split('.');
            string site = parts[0];
            string id;
            if (site == "youtube")
            {
                string[] args = link.Split('?');
                string[] arg = args[args.Length - 1].Split('&');
                string[] vid = arg[arg.Length - 1].Split('=');
                id = vid[vid.Length - 1];
                webBrowserTask.Uri = new Uri("http://www.youtube.com/embed/" + id + "?autoplay=1&modestbranding=1&showinfo=0&showsearch=0&rel=0", UriKind.Absolute);
            }
            else
            {
                string[] args = cleaned_url.Split('/');
                id = args[args.Length - 1];
                if (site == "youtu")
                    webBrowserTask.Uri = new Uri("http://www.youtube.com/embed/" + id + "?autoplay=1&modestbranding=1&showinfo=0&showsearch=0&rel=0", UriKind.Absolute);
                else
                    webBrowserTask.Uri = new Uri("http://vimeo.com/m/" + id, UriKind.Absolute);

            }
            //Debug.WriteLine(id);


            // webBrowserTask.Uri = new Uri(String.Format("vnd.youtube:{0}?vndapp=youtube", youTubeId), UriKind.Absolute);

            webBrowserTask.Show();
        }

        private void share_clicked(object sender, EventArgs e)
        {
            MessageBox.Show("share");
            //this.ApplicationBar.IsVisible = false;
        }

        private void close_clicked(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}