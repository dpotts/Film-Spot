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



namespace Film_Spot
{
    public partial class MainPage : PhoneApplicationPage
    {
        RedditViewModel _viewModel;
        int _offsetKnob = 7;
        MoviesResult selected;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            _viewModel = (RedditViewModel)Resources["viewModel"];
            resultListBox.ItemRealized += resultListBox_ItemRealized;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var progressIndicator = SystemTray.ProgressIndicator;
            if (progressIndicator != null)
            {
                return;
            }

            progressIndicator = new ProgressIndicator();

            SystemTray.SetProgressIndicator(this, progressIndicator);

            Binding binding = new Binding("IsLoading") { Source = _viewModel };
            BindingOperations.SetBinding(
                progressIndicator, ProgressIndicator.IsVisibleProperty, binding);

            binding = new Binding("IsLoading") { Source = _viewModel };
            BindingOperations.SetBinding(
                progressIndicator, ProgressIndicator.IsIndeterminateProperty, binding);

            progressIndicator.Text = "Loading Movies...";

        }

        void resultListBox_ItemRealized(object sender, ItemRealizationEventArgs e)
        {
            if (!_viewModel.IsLoading && resultListBox.ItemsSource != null && resultListBox.ItemsSource.Count >= _offsetKnob)
            {
                if (e.ItemKind == LongListSelectorItemKind.Item)
                {
                    if ((e.Container.Content as MoviesResult).Equals(resultListBox.ItemsSource[resultListBox.ItemsSource.Count - _offsetKnob]))
                    {
                        this.ApplicationBar.IsVisible = false;
                        _viewModel.LoadPage();
                    }
                }
            }
        }
    
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine("get stuff");
            _viewModel.LoadPage();

        }

        private void resultListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (resultListBox.SelectedItem is Model.MoviesResult)
            {
                if (resultListBox.SelectedItem.Equals(selected))
                {
                    this.ApplicationBar.IsVisible = !this.ApplicationBar.IsVisible;
                }
                else
                {
                    this.ApplicationBar.IsVisible = true;
                    selected = (Model.MoviesResult)resultListBox.SelectedItem;
                }
            }
            resultListBox.SelectedItem = null;
        }

        private void play_clicked(object sender, EventArgs e)
        {
            //MessageBox.Show("play " + selected.Title);
            WebBrowserTask webBrowserTask = new WebBrowserTask();

            string cleaned_url = selected.Link.Replace("http://", "");
            cleaned_url = cleaned_url.Replace("https://", "");
            cleaned_url = cleaned_url.Replace("www.", "");
            cleaned_url = cleaned_url.Replace("m.", "");
            var parts = cleaned_url.Split('.');
            string site = parts[0];
            string id;
            if (site == "youtube")
            {
                string[] args = selected.Link.Split('?');
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
            selected = null;
            this.ApplicationBar.IsVisible = false;
        }

        private void info_clicked(object sender, EventArgs e)
        {
            //MessageBox.Show("info " + selected.Title);
            NavigationService.Navigate(new Uri("/info.xaml?id=" + selected.ImdbID, UriKind.Relative));
            this.ApplicationBar.IsVisible = false;
        }

        private void share_clicked(object sender, EventArgs e)
        {
            MessageBox.Show("share " + selected.Title);
            this.ApplicationBar.IsVisible = false;
        }

        private void close_clicked(object sender, EventArgs e)
        {
            this.ApplicationBar.IsVisible = false;
        }
    }
}