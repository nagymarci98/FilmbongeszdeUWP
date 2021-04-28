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
    }
}