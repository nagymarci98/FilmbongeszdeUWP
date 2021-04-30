using Filmbongeszde.Models;
using System;
using Windows.UI.Xaml.Controls;

namespace Filmbongeszde.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        private void Movies_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var movie = (Movie)e.ClickedItem;
            ViewModel.NavigateToDetails(movie.id);
        }

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
           await ViewModel.SearchMovie(searchBox.Text);
        }

        private async void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(searchBox.Text.Length == 0)
            {
                await ViewModel.GetTopRatedMovies();
                ViewModel.SearchString = "";
                ViewModel.WasSearched = false;
            }
        }
    }
}