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

namespace Filmbongeszde.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private Movie movie;
        private ObservableCollection<Movie> testMovies = new ObservableCollection<Movie>();
        public ObservableCollection<Movie> TestMovies
        {
            get { return testMovies; }
            set { testMovies = value; }
        }
        public Movie Movie
        {
            get { return movie; }
            set { movie = value; }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var service = new MovieServices();
            var testMovie = await service.GetMoviesBySearch("hulk");
            var topRatedMovies = await service.GetTopRatedMoviesAsync();
            foreach (var item in topRatedMovies.movies)
            {
                this.TestMovies.Add(item);
            }
            await base.OnNavigatedToAsync(parameter, mode, state);
        }
    }
}
