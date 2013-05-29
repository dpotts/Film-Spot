using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows;
using System.Xml;
using Newtonsoft.Json;
using Film_Spot.Model;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Film_Spot.ViewModels
{
    public class RedditViewModel : INotifyPropertyChanged
    {
        string json = "";
        string last_post_name = "";
        RootObject movies = new RootObject();
        private bool _isLoading = false;

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                NotifyPropertyChanged("IsLoading");

            }
        }

        public RedditViewModel()
        {
            this.MovieCollection = new ObservableCollection<MoviesResult>();
            this.IsLoading = false;
        }

        public ObservableCollection<MoviesResult> MovieCollection
        {
            get;
            private set;
        }

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
            public string imdbID { get; set; }
            public string Type { get; set; }
            public string Response { get; set; }
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

        public void LoadPage()
        {
            Debug.WriteLine("load page");
            IsLoading = true;
            string url = "";
            if (last_post_name == "")
            {
                url = "http://www.reddit.com/r/fullmoviesonyoutube+fullmoviesonvimeo/new.json";
            }
            else
            {
                url = "http://www.reddit.com/r/fullmoviesonyoutube+fullmoviesonvimeo/new.json?after=" + last_post_name;
            }
            HttpWebRequest hWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            hWebRequest.Method = "GET";
            hWebRequest.BeginGetResponse(Response_Completed, hWebRequest);
        }

        public void Response_Completed(IAsyncResult result)
        {
            Debug.WriteLine("got a response");
            try
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
                    foreach (var movie in movies.data.children)
                    {
                        string[] title_year = Regex.Split(movie.data.title, @"\((.*?)\)");
                        if (title_year.Length > 0)
                        {
                            string movie_title = title_year[0].Trim();
                            int movie_year = 0;
                            if(title_year.Length > 1)
                                int.TryParse(title_year[1], out movie_year);

                            Get_Movie_Details(movie_title, movie_year);

                            this.MovieCollection.Add(new MoviesResult()
                            {
                                Title = movie_title,
                                Runtime = "Unknown",
                                Released = "Unknown",
                                Link = movie.data.url
                            });
                        }
                    }  
                    last_post_name = movies.data.after;
                    IsLoading = false;
                });
            }
            catch (Exception e)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("Network error occured " + e.Message);
                });
            }
        }

        public void Get_Movie_Details(string name, int year)
        {
            Debug.WriteLine("getting details");
            IsLoading = true;
            string url = "";
            if (name != "")
            {
                if (year > 0)
                {
                    url = "http://www.omdbapi.com/?t=" + name + "&y=" + year;
                }
                else
                {
                    url = "http://www.omdbapi.com/?t=" + name;
                }
                HttpWebRequest hWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                hWebRequest.Method = "GET";
                hWebRequest.BeginGetResponse(Response_Movie_Details, hWebRequest);
            }
            else
            {
                Debug.WriteLine("no title to search for");
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
                    foreach (MoviesResult SingleMovie in MovieCollection)
                    {
                        if (string.Equals(movie_info.Title, SingleMovie.Title, StringComparison.CurrentCultureIgnoreCase))
                        {
                            SingleMovie.ImageUrl = movie_info.Poster;
                            SingleMovie.Runtime = movie_info.Runtime;
                            SingleMovie.Released = movie_info.Released;

                            double rating;
                            double.TryParse(movie_info.imdbRating, out rating);
                            if (rating > 9.0)
                            {
                                SingleMovie.Star1 = "Assets/full_star.png";
                                SingleMovie.Star2 = "Assets/full_star.png";
                                SingleMovie.Star3 = "Assets/full_star.png";
                                SingleMovie.Star4 = "Assets/full_star.png";
                                SingleMovie.Star5 = "Assets/full_star.png";
                            }
                            else if (rating > 8.0)
                            {
                                SingleMovie.Star1 = "Assets/full_star.png";
                                SingleMovie.Star2 = "Assets/full_star.png";
                                SingleMovie.Star3 = "Assets/full_star.png";
                                SingleMovie.Star4 = "Assets/full_star.png";
                                SingleMovie.Star5 = "Assets/half_star.png";

                            }
                            else if (rating > 7.0)
                            {
                                SingleMovie.Star1 = "Assets/full_star.png";
                                SingleMovie.Star2 = "Assets/full_star.png";
                                SingleMovie.Star3 = "Assets/full_star.png";
                                SingleMovie.Star4 = "Assets/full_star.png";
                                SingleMovie.Star5 = "Assets/no_star.png";
                            }
                            else if (rating > 6.0)
                            {
                                SingleMovie.Star1 = "Assets/full_star.png";
                                SingleMovie.Star2 = "Assets/full_star.png";
                                SingleMovie.Star3 = "Assets/full_star.png";
                                SingleMovie.Star4 = "Assets/half_star.png";
                                SingleMovie.Star5 = "Assets/no_star.png";
                            }
                            else if (rating > 5.0)
                            {
                                SingleMovie.Star1 = "Assets/full_star.png";
                                SingleMovie.Star2 = "Assets/full_star.png";
                                SingleMovie.Star3 = "Assets/full_star.png";
                                SingleMovie.Star4 = "Assets/no_star.png";
                                SingleMovie.Star5 = "Assets/no_star.png";
                            }
                            else if (rating > 4.0)
                            {
                                SingleMovie.Star1 = "Assets/full_star.png";
                                SingleMovie.Star2 = "Assets/full_star.png";
                                SingleMovie.Star3 = "Assets/half_star.png";
                                SingleMovie.Star4 = "Assets/no_star.png";
                                SingleMovie.Star5 = "Assets/no_star.png";
                            }
                            else if (rating > 3.0)
                            {
                                SingleMovie.Star1 = "Assets/full_star.png";
                                SingleMovie.Star2 = "Assets/full_star.png";
                                SingleMovie.Star3 = "Assets/no_star.png";
                                SingleMovie.Star4 = "Assets/no_star.png";
                                SingleMovie.Star5 = "Assets/no_star.png";
                            }
                            else if (rating > 2.0)
                            {
                                SingleMovie.Star1 = "Assets/full_star.png";
                                SingleMovie.Star2 = "Assets/half_star.png";
                                SingleMovie.Star3 = "Assets/no_star.png";
                                SingleMovie.Star4 = "Assets/no_star.png";
                                SingleMovie.Star5 = "Assets/no_star.png";
                            }
                            else if (rating > 1.0)
                            {
                                SingleMovie.Star1 = "Assets/full_star.png";
                                SingleMovie.Star2 = "Assets/no_star.png";
                                SingleMovie.Star3 = "Assets/no_star.png";
                                SingleMovie.Star4 = "Assets/no_star.png";
                                SingleMovie.Star5 = "Assets/no_star.png";
                            }
                            else
                            {
                                SingleMovie.Star1 = "Assets/half_star.png";
                                SingleMovie.Star2 = "Assets/no_star.png";
                                SingleMovie.Star3 = "Assets/no_star.png";
                                SingleMovie.Star4 = "Assets/no_star.png";
                                SingleMovie.Star5 = "Assets/no_star.png";
                            }


                        }
                    }
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

