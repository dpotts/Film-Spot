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
    public partial class info : PhoneApplicationPage
    {
        string id = "";
        string link = "";
        string share_link = "";
        string movie_title = "";
        string search_string = "";
        string movie_poster = "";

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

        public info()
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
            NavigationContext.QueryString.TryGetValue("share_link", out share_link);
            NavigationContext.QueryString.TryGetValue("search", out search_string);
            NavigationContext.QueryString.TryGetValue("name", out movie_title);

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
                    if (movie_info.Poster != "N/A" && movie_info.Poster != null)
                    {
                        poster.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(movie_info.Poster, UriKind.Absolute));
                        movie_poster = movie_info.Poster;
                    }

                    if (movie_info.Title != null)
                        movie_title = movie_info.Title;

                    title.Text = movie_title;

                    if (movie_info.Year != null)
                        title.Text += " (" + movie_info.Year + ")";

                    if(movie_info.Rated != null)
                        rating.Text = movie_info.Rated;

                    if (movie_info.Plot != null)
                        plot.Text = movie_info.Plot;

                    if (movie_info.Genre != null)
                        genre.Text = movie_info.Genre;

                    if (movie_info.imdbRating != null)
                        imdb.Text = movie_info.imdbRating;

                    if (movie_info.tomatoRating != null)
                        tomato.Text = movie_info.tomatoRating;

                    if (movie_info.Runtime != null)
                        runtime.Text = movie_info.Runtime;

                    if (movie_info.Writer != null)
                        writers.Text = movie_info.Writer;

                    if (movie_info.Director != null)
                        directors.Text = movie_info.Director;

                    if (movie_info.Actors != null)
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
            //WebBrowserTask webBrowserTask = new WebBrowserTask();
            //webBrowserTask.Uri = new Uri(link, UriKind.Absolute);
            //webBrowserTask.Show();
            NavigationService.Navigate(new Uri("/play.xaml?url=" + link, UriKind.Relative));


        }

        private void share_clicked(object sender, EventArgs e)
        {
            ShareLinkTask slt = new ShareLinkTask();
            slt.Title = movie_title + " on FilmSpot";
            slt.Message = "I found " + movie_title + " for free on FilmSpot. Check it out!";
            slt.LinkUri = new Uri(share_link, UriKind.Absolute);
            slt.Show();
        }

        private void close_clicked(object sender, EventArgs e)
        {
            //NavigationService.GoBack();
            if(string.IsNullOrEmpty(search_string)){
                NavigationService.Navigate(new Uri("/browse.xaml", UriKind.Relative));
            } else {
                NavigationService.Navigate(new Uri("/browse.xaml?title=" + search_string, UriKind.Relative));

            }
        }

        private void pin_clicked(object sender, EventArgs e)
        {
            ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("/play.xaml?url=" + link));

            if (tile == null)
            {
                if (movie_poster == null)
                    movie_poster = "Assets/Tiles/FlipCycleTileSmall.png";

                var secondaryTile = new StandardTileData
                {
                    Title = movie_title,
                    BackBackgroundImage = new Uri("Assets/Tiles/FlipCycleTileSmall.png", UriKind.Relative),
                    BackgroundImage = new Uri(movie_poster, UriKind.RelativeOrAbsolute)
                };
                ShellTile.Create(new Uri("/play.xaml?url=" + link, UriKind.Relative), secondaryTile);
            }
            else
            {
                MessageBox.Show("Already Pinned To Start");
            }

        }
    }
}