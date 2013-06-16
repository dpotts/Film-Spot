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
using System.Windows.Media;
using System.Windows.Navigation;



namespace Film_Spot
{
    public partial class browse : PhoneApplicationPage
    {
        RedditViewModel _viewModel;
        int _offsetKnob = 7;
        MoviesResult selected;
        string search_string = "";


        // Constructor
        public browse()
        {
            InitializeComponent();
            _viewModel = (RedditViewModel)Resources["viewModel"];
            resultListBox.ItemRealized += resultListBox_ItemRealized;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavigationContext.QueryString.TryGetValue("title", out search_string);
            if (string.IsNullOrEmpty(search_string))
            {
                _viewModel.LoadPage();
            }
            else
            {
                view_all.Visibility = Visibility.Visible;
                _viewModel.set_search(search_string);
            }
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
            this.Focus();

            if (!_viewModel.IsLoading && resultListBox.ItemsSource != null && resultListBox.ItemsSource.Count >= _offsetKnob)
            {
                if (e.ItemKind == LongListSelectorItemKind.Item)
                {
                    if ((e.Container.Content as MoviesResult).Equals(resultListBox.ItemsSource[resultListBox.ItemsSource.Count - _offsetKnob]))
                    {
                        _viewModel.LoadPage();
                    }
                }
            }
        }

        private void resultListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (resultListBox.SelectedItem is Model.MoviesResult)
            {
                selected = (Model.MoviesResult)resultListBox.SelectedItem;
                NavigationService.Navigate(new Uri("/info.xaml?id=" + selected.ImdbID + "&url=" + selected.Link + "&share_link=" + selected.Share_URL + "&search=" + _viewModel.get_search() + "&name=" + selected.Title, UriKind.Relative));
            }
            resultListBox.SelectedItem = null;
        }


        private void Search_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            selected = null;
            view_all.Visibility = Visibility.Visible;
            _viewModel.set_search(search.Text);
        }

        private void search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                selected = null;
                view_all.Visibility = Visibility.Visible;
                _viewModel.set_search(search.Text);
                this.Focus();
            }
        }

        private void search_clear(object sender, System.Windows.Input.GestureEventArgs e)
        {
            search.Text = string.Empty;
        }

        private void view_all_Click(object sender, RoutedEventArgs e)
        {
            search_string = "";
            _viewModel.clear_search();
            view_all.Visibility = Visibility.Collapsed;
            selected = null;
            _viewModel.LoadPage();
        }

        private void logo_tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void resultListBox_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Focus();
        }
    }
}