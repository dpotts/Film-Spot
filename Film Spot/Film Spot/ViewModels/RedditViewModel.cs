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
        string search = "";
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
            public List<Search> Search { get; set; }
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

        public class Search
        {
            public string Title { get; set; }
            public string Year { get; set; }
            public string imdbID { get; set; }
            public string Type { get; set; }
        }

        public void set_search(string search_input)
        {
            search = search_input;
            this.MovieCollection.Clear();
            last_post_name = "";
            LoadPage();
        }

        public string get_search()
        {
            return search;
        }

        public void clear_search()
        {
            search = "";
            this.MovieCollection.Clear();
            last_post_name = "";
            LoadPage();
        }

        public void LoadPage()
        {
            IsLoading = true;
            string url = "";
            if (string.IsNullOrEmpty(last_post_name))
            {
                if (MovieCollection.Count == 0)
                {
                    if (!string.IsNullOrEmpty(search))
                    {
                        url = "http://www.reddit.com/r/fullmoviesonyoutube+fullmoviesonvimeo/search.json?q=" + search + "&restrict_sr=on";
                    }
                    else
                    {

                        url = "http://www.reddit.com/r/fullmoviesonyoutube+fullmoviesonvimeo/new.json";
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(search))
                {
                    url = "http://www.reddit.com/r/fullmoviesonyoutube+fullmoviesonvimeo/search.json?q=" + search + "&restrict_sr=on&after=" + last_post_name;
                }
                else
                {
                    url = "http://www.reddit.com/r/fullmoviesonyoutube+fullmoviesonvimeo/new.json?after=" + last_post_name;
                }
            }
            if (!string.IsNullOrEmpty(url))
            {
                HttpWebRequest hWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                hWebRequest.Method = "GET";
                hWebRequest.BeginGetResponse(Response_Completed, hWebRequest);
            }
            else
            {
                    IsLoading = false;
            }
        }

        public void Response_Completed(IAsyncResult result)
        {
            //Debug.WriteLine("got a response");
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
                    if (movies.data.children.Count == 0)
                    {
                        if (string.IsNullOrEmpty(search))
                        {
                            MessageBox.Show("No Movies Available To Watch.  Please Come Back Later.");
                        }
                        else
                        {
                            MessageBox.Show("No Movie Found Called " + search);
                            search = "";
                            last_post_name = "";
                            LoadPage();
                        }
                    }
                    else
                    {
                        foreach (var movie in movies.data.children)
                        {
                            string[] title_year = Regex.Split(movie.data.title, @"\((.*?)\)");
                            string cleaned_url = movie.data.url.Replace("http://", "");
                            cleaned_url = cleaned_url.Replace("https://", "");
                            cleaned_url = cleaned_url.Replace("www.", "");
                            cleaned_url = cleaned_url.Replace("m.", "");
                            var parts = cleaned_url.Split('.');
                            string site = parts[0];

                            if (title_year.Length > 0 && (site == "youtube" || site == "youtu" || site == "vimeo"))
                            {
                                int movie_year = 0;
                                if (title_year.Length > 1)
                                {

                                    int.TryParse(title_year[1], out movie_year);
                                }
                                else
                                {
                                    string[] title_year_try = Regex.Split(title_year[0], @"\[(.*?)\]");
                                    if (title_year_try.Length > 1)
                                    {
                                        int.TryParse(title_year_try[1], out movie_year);
                                        title_year[0] = title_year_try[0];
                                    }
                                }

                                string movie_title = title_year[0];

                                movie_title = movie_title.Replace("&amp;", " ");
                                movie_title = movie_title.Replace(" - ", " ");
                                movie_title = movie_title.Replace(" HD", "");
                                movie_title = movie_title.Replace(" SD", "");
                                movie_title = movie_title.Replace(" 3D", "");
                                movie_title = movie_title.Replace(" 720p", "");
                                movie_title = movie_title.Replace(" 1080p", "");
                                movie_title = movie_title.Replace(" 360p", "");
                                movie_title = movie_title.Replace(" 240p", "");
                                movie_title = movie_title.Replace(" 3D", "");
                                movie_title = movie_title.Replace(":", " ");
                                movie_title = movie_title.Replace("\"", "");

                                movie_title = movie_title.Replace("  ", " ");
                                movie_title = movie_title.Trim();


                                Get_Movie_Details(movie_title, movie_year);
                                string url = "";
                                string share_url = "";
                                string id;
                                if (site == "youtube")
                                {
                                    string[] args = movie.data.url.Split('?');
                                    string[] arg = args[args.Length - 1].Split('&');
                                    string[] vid = arg[arg.Length - 1].Split('=');
                                    id = vid[vid.Length - 1];
                                    url = "http://www.youtube.com/embed/" + id + "?autoplay=1&modestbranding=1&showinfo=0&showsearch=0&rel=0";
                                    share_url = "http://www.youtube.com/v/" + id;
                                }
                                else
                                {
                                    string[] args = cleaned_url.Split('/');
                                    id = args[args.Length - 1];
                                    if (site == "youtu")
                                    {
                                        share_url = "http://www.youtube.com/v/" + id;
                                        url = "http://www.youtube.com/embed/" + id + "?autoplay=1&modestbranding=1&showinfo=0&showsearch=0&rel=0";
                                    }
                                    else
                                    {
                                        share_url = "http://www.vimeo.com/" + id;
                                        url = "http://vimeo.com/m/" + id;
                                    }

                                }

                                this.MovieCollection.Add(new MoviesResult()
                                {
                                    Title = movie_title,
                                    Runtime = "Unknown",
                                    Released = "Unknown",
                                    Share_URL = share_url,
                                    Link = url,

                                });
                            }
                        }
                        last_post_name = movies.data.after;
                    }
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

        public void Get_Movie_Details(string name, int year, int search = 0)
        {
            //Debug.WriteLine("getting details");
            //IsLoading = true;
            string url = "";
            if (name != "")
            {
                if (year > 0)
                {
                    url = "http://www.omdbapi.com/?t=" + name + "&y=" + year;
                }
                else
                {
                    if (search == 0)
                        url = "http://www.omdbapi.com/?t=" + name;
                    else
                    {
                        url = "http://www.omdbapi.com/?s=" + name;
                    }
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
                    string arguments = response.ResponseUri.ToString().Replace("http://www.omdbapi.com/?", "");
                    string[] query_type = arguments.Split('&');
                    query_type = query_type[0].Split('=');
                    string search_type = "";
                    string search_title = "";
                    if (query_type.Length > 0)
                    {
                        search_type = query_type[0];
                        search_title = query_type[1];
                    }
                    else
                    {
                        Debug.WriteLine("Search movie details error");
                    }
                    if ((movie_info.Response == "True" && movie_info.Type == "movie") || (search_type == "s" && movie_info.Response != "False" && movie_info.Search[0].Type == "movie"))
                    {

                        if(search_type == "t")
                        {
                            foreach (MoviesResult SingleMovie in MovieCollection)
                            {
                                if (string.Equals(search_title, SingleMovie.Title, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    SingleMovie.Title = movie_info.Title;
                                    SingleMovie.ImageUrl = movie_info.Poster;
                                    SingleMovie.Runtime = movie_info.Runtime;
                                    SingleMovie.Released = movie_info.Released;
                                    SingleMovie.ImdbID = movie_info.imdbID;

                                    double rating;
                                    double.TryParse(movie_info.imdbRating, out rating);
                                    if (rating > 0)
                                        SingleMovie.Rating = rating.ToString();
                                    else
                                        SingleMovie.Rating = "";
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
                                    else if (rating <= 1.0 & rating > 0.0)
                                    {
                                        SingleMovie.Star1 = "Assets/half_star.png";
                                        SingleMovie.Star2 = "Assets/no_star.png";
                                        SingleMovie.Star3 = "Assets/no_star.png";
                                        SingleMovie.Star4 = "Assets/no_star.png";
                                        SingleMovie.Star5 = "Assets/no_star.png";
                                    }
                                    else
                                    {
                                        SingleMovie.Star1 = "Assets/no_star.png";
                                        SingleMovie.Star2 = "Assets/no_star.png";
                                        SingleMovie.Star3 = "Assets/no_star.png";
                                        SingleMovie.Star4 = "Assets/no_star.png";
                                        SingleMovie.Star5 = "Assets/no_star.png";
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Debug.WriteLine("Found movie details for {0} ({1})", movie_info.Search[0].Title, search_title);
                            int new_year = 0;
                            int.TryParse(movie_info.Search[0].Year, out new_year);
                            foreach (MoviesResult SingleMovie in MovieCollection)
                            {
                                if (string.Equals(search_title, SingleMovie.Title, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    SingleMovie.Title = movie_info.Search[0].Title;
                                }
                            }
                            Get_Movie_Details(movie_info.Search[0].Title, new_year);
                        }
                    }
                    else if (search_type == "t")
                    {
                        //Debug.WriteLine("Search for {0}",search_title);
                        string[] more_cleaning_title = search_title.Split('[');
                        more_cleaning_title = more_cleaning_title[0].Split('(');
                        string more_cleaned_title = more_cleaning_title[0].Replace("  ", " ");

                        more_cleaned_title = more_cleaned_title.Trim();

                        foreach (MoviesResult SingleMovie in MovieCollection)
                        {
                            if (string.Equals(search_title, SingleMovie.Title, StringComparison.CurrentCultureIgnoreCase))
                            {
                                SingleMovie.Title = more_cleaned_title;
                            }
                        }
                        Get_Movie_Details(more_cleaned_title, 0, 1);
                    }
                    else
                    {
                        Debug.WriteLine("No {0} details available for {1}", search_type, search_title);
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

