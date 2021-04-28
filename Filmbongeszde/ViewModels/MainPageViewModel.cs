using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using Filmbongeszde.Models;
using System.Collections.ObjectModel;
using Filmbongeszde.Services;
using System.Diagnostics;
using Filmbongeszde.Views;

namespace Filmbongeszde.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private Movie movie;
        private ObservableCollection<Movie> movies = new ObservableCollection<Movie>();
        private int pageNumber;

        public int PageNumber
        {
            get { return pageNumber; }
            set { pageNumber = value; }
        }

        public ObservableCollection<Movie> Movies
        {
            get { return movies; }
            set { movies = value; }
        }
        public Movie Movie
        {
            get { return movie; }
            set { movie = value; }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            this.Movies.Clear();
            var service = new MovieServices();
            var topRatedMovies = await service.GetTopRatedMoviesAsync(1);
            foreach (var item in topRatedMovies.movies)
            {
                item.poster_path = "https://image.tmdb.org/t/p/w500/" + item.poster_path;
                this.Movies.Add(item);
            }
            await base.OnNavigatedToAsync(parameter, mode, state);
        }
        public void NavigateToDetails(int movieId)
        {
            NavigationService.Navigate(typeof(MovieDetailsPage), movieId);
        }
    }
}
