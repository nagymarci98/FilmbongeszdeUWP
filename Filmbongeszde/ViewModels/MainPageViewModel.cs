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
        public MainPageViewModel()
        {
            this.HeaderString = "Legjobb Mínősítésű Filmek";
            this.SearchString = "";
        }
        private Movie movie;
        public Movie Movie
        {
            get { return movie; }
            set { movie = value; }
        }
        private ObservableCollection<Movie> movies = new ObservableCollection<Movie>();
        public ObservableCollection<Movie> Movies
        {
            get { return movies; }
            set { movies = value; }
        }
        private int pageNumber;

        public int PageNumber
        {
            get { return pageNumber; }
            set { pageNumber = value; }
        }
        private string headerString;

        public string HeaderString
        {
            get { return headerString; }
            set { Set(ref headerString, value); }
        }
        public bool WasSearched { get; set; }
        public string SearchString { get; set; }
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (WasSearched && this.SearchString.Length != 0)
            {
                await SearchMovie(this.SearchString);
            }
            else
            {
                await GetTopRatedMovies();
            }
            await base.OnNavigatedToAsync(parameter, mode, state);
        }
        public async Task GetTopRatedMovies()
        {
            this.Movies.Clear();
            var service = new MovieServices();
            var topRatedMovies = await service.GetTopRatedMoviesAsync(1);
            foreach (var item in topRatedMovies.movies)
            {
                item.poster_path = "https://image.tmdb.org/t/p/w500/" + item.poster_path;
                this.Movies.Add(item);
            }
        }
        public void NavigateToDetails(int movieId)
        {
            NavigationService.Navigate(typeof(MovieDetailsPage), movieId);
        }
        public async Task SearchMovie(string keyword)
        {
            this.SearchString = keyword;
            this.WasSearched = true;
            this.HeaderString = "Keresés...";
            var movieService = new MovieServices();
            var searchedMovies = await movieService.GetMoviesBySearchAsync(keyword);
            if (searchedMovies.movies.Count == 0)
            {
                this.HeaderString = "Nincs találat a keresésre";
            }
            else
            {
                this.Movies.Clear();
                foreach (var movie in searchedMovies.movies)
                {
                    movie.poster_path = "https://image.tmdb.org/t/p/w500/" + movie.poster_path;
                    this.Movies.Add(movie);
                }
                this.HeaderString = $"{searchedMovies.movies.Count} találat van";
            }
        }
    }
}
