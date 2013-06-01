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

            string id = "";

            if (NavigationContext.QueryString.TryGetValue("id", out id))
            {
                
                    string url = "http://www.omdbapi.com/?i=" + id + "&tomatoes=true&plot=full";

                    HttpWebRequest hWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                    hWebRequest.Method = "GET";
                    hWebRequest.BeginGetResponse(Response_Movie_Details, hWebRequest);
            }
        }

        public void Response_Movie_Details(IAsyncResult result)
        {
            Debug.WriteLine("got a response for details");
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
    }
}