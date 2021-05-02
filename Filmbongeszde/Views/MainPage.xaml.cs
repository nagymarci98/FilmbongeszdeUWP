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
           // ViewModel.GenreCB = GenreCB;
        }

        private void Movies_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var item = (TvSeriesOrMovieDisplaySmall)e.ClickedItem;
            if (ViewModel.IsMovie)
            {
                ViewModel.NavigateToMovieDetails(item.id);
            }
            else
            {
                ViewModel.NavigateToTvSeriesDetails(item.id);
            }
        }

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if(searchBox.Text.Length > 0)
            {
                ViewModel.PageNumber = 1;
                if (ViewModel.IsMovie)
                {
                    await ViewModel.SearchMovie(searchBox.Text, ViewModel.PageNumber);
                }
                else
                {
                    await ViewModel.SearchTvSeries(searchBox.Text, ViewModel.PageNumber);

                }
            }
        }

        private async void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(searchBox.Text.Length == 0)
            {
                ViewModel.SearchString = "";
                ViewModel.WasSearched = false;
                ViewModel.PageNumber = 1;

                if (ViewModel.IsMovie)
                {
                    ViewModel.HeaderString = "Legjobb minősítésű filmek";
                    await ViewModel.GetTopRatedMovies(ViewModel.PageNumber);
                }
                else
                {
                    ViewModel.HeaderString = "Legjobb minősítésű Sorozatok";
                    await ViewModel.GetTopRatedTvSeries(ViewModel.PageNumber);
                }
            }
        }

        private async void NextPageButtonClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await ViewModel.NextPage();
        }

        private async void PrevPageButtonClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await ViewModel.PrevPage();
        }

        private async void TypeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            searchBox.Text = "";
            if(Type.SelectedIndex == 0)
            {
                ViewModel.PageNumber = 1;
                ViewModel.IsMovie = true;
                await ViewModel.GetTopRatedMovies(ViewModel.PageNumber);
            }
            else
            {
                ViewModel.PageNumber = 1;
                ViewModel.IsMovie = false;
                await ViewModel.GetTopRatedTvSeries(ViewModel.PageNumber);
            }
        }

        private async void GenreCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var temp = (Genre)GenreCB.SelectedValue;
           // ViewModel.SelectedGenre = temp.name;
            //ViewModel.WasGenreSelected = true;
            //await ViewModel.SearchMovieByGenre();
        }
    }
}