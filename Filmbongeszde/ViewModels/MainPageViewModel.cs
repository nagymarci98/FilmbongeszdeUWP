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
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;

namespace Filmbongeszde.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MainPageViewModel()
        {
            this.HeaderString = "Legjobb Mínősítésű Filmek";
            this.SearchString = "";
            this.PageNumber = 1;
            this.IsMovie = true;
            // this.WasGenreSelected = false;
        }
        /// <summary>
        /// List for the genres
        /// </summary>
        private ObservableCollection<Genre> genres = new ObservableCollection<Genre>();
        public ObservableCollection<Genre> Genres
        {
            get { return genres; }
            set { genres = value; }
        }
        /// <summary>
        /// List for the displayable movies and tv series
        /// </summary>
        private ObservableCollection<TvSeriesOrMovieDisplaySmall> displayables = new ObservableCollection<TvSeriesOrMovieDisplaySmall>();
        public ObservableCollection<TvSeriesOrMovieDisplaySmall> Displayables
        {
            get { return displayables; }
            set { displayables = value; }
        }
        /// <summary>
        /// The amount of the results
        /// </summary>
        public int SeachPagesCount { get; set; }
        public int TopRatedPagesCount { get; set; }
        /// <summary>
        /// Bool for determine if its tv series or movie
        /// </summary>
        public bool IsMovie { get; set; }
        /// <summary>
        /// Actual page number
        /// </summary>
        private int pageNumber;

        public int PageNumber
        {
            get { return pageNumber; }
            set { Set(ref pageNumber, value); }
        }
        /// <summary>
        /// Header string to show in the XAML
        /// </summary>
        private string headerString;

        public string HeaderString
        {
            get { return headerString; }
            set { Set(ref headerString, value); }
        }
        /// <summary>
        /// Boolean for save if was searched or not
        /// </summary>
        public bool WasSearched { get; set; }
        public string SearchString { get; set; }
        public string SelectedGenre { get; internal set; }
        //public bool WasGenreSelected { get; set; }
        // public bool WasSortingSelected { get; set; }
        //public ComboBox GenreCB { get; set; }

        /// <summary>
        /// Runs when navigated to the view.
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="mode"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (IsMovie)
            {
                if (WasSearched && this.SearchString.Length != 0)
                {
                    await SearchMovie(this.SearchString, this.PageNumber);
                }
                else
                {
                    await GetTopRatedMovies(this.PageNumber);
                }
            }
            else
            {
                if (WasSearched && this.SearchString.Length != 0)
                {
                    await SearchTvSeries(this.SearchString, this.PageNumber);
                }
                else
                {
                    await GetTopRatedTvSeries(this.PageNumber);
                }
            }
            //await GetAllGenres();
            /*if (WasGenreSelected)
            {
                setGenreSelected();
            }*/
            await base.OnNavigatedToAsync(parameter, mode, state);
        }

        

        /*
private void setGenreSelected()
{
   var slectedGenreFromList = Genres.Where(x => x.name == this.SelectedGenre).FirstOrDefault();
   if(slectedGenreFromList != null)
   {
       GenreCB.SelectedIndex = this.Genres.IndexOf((Genre)slectedGenreFromList);
   }
   else
   {
       GenreCB.SelectedIndex = this.Genres.IndexOf(Genres.ElementAt(0));
   }
}
*/
        /// <summary>
        /// Moves to the next page of resutls if theres any
        /// </summary>
        /// <returns></returns>
        internal async Task NextPage()
        {
            if (WasSearched)
            {

                if (this.PageNumber + 1 < this.SeachPagesCount)
                    if (IsMovie)
                    {
                        await SearchMovie(SearchString, ++this.PageNumber);
                    }
                    else
                    {
                        await SearchTvSeries(SearchString, ++this.PageNumber);
                    }
            }
            else
            {
                if (this.PageNumber + 1 < this.TopRatedPagesCount)
                {
                    if (IsMovie)
                    {
                        await GetTopRatedMovies(++this.PageNumber);
                    }
                    else
                    {
                        await GetTopRatedTvSeries(++this.PageNumber);
                    }
                }
            }
        }
        /// <summary>
        /// Same as next page but it goes back
        /// </summary>
        /// <returns></returns>
        internal async Task PrevPage()
        {
            if (WasSearched)
            {
                if (this.PageNumber != 1)
                {
                    if (IsMovie)
                    {
                        await SearchMovie(SearchString, --this.PageNumber);
                    }
                    else
                    {
                        await SearchTvSeries(SearchString, --this.PageNumber);
                    }
                }
            }
            else
            {
                if (this.PageNumber != 1)
                {
                    if (IsMovie)
                    {
                        await GetTopRatedMovies(--this.PageNumber);

                    }
                    else
                    {
                        await GetTopRatedTvSeries(--this.PageNumber);
                    }
                }
            }
        }


        /*private async Task GetAllGenres()
        {
            var ms = new MovieServices();
            var genresTemp =await ms.GetAllGenreAsync();
            var allGenre = new Genre();
            allGenre.id = -1;
            allGenre.name = "Összes";
            this.Genres.Add(allGenre);
            foreach (var genre in genresTemp.genres)
            {
                this.Genres.Add(genre);
            }
        }*/
        /// <summary>
        /// Navigation functions, to movieDetails, tv series details wtc.
        /// </summary>
        /// <param name="movieId"></param>
        public void NavigateToTvSeriesDetails(int movieId)
        {
            NavigationService.Navigate(typeof(TvSeriesDetailsPage), movieId);
        }
        public void NavigateToMovieDetails(int movieId)
        {
            NavigationService.Navigate(typeof(MovieDetailsPage), movieId);
        }
        /// <summary>
        /// Function for searching movies
        /// </summary>
        /// <param name="keyword">The keywords to search for</param>
        /// <param name="page">The number of page requested</param>
        /// <returns></returns>
        public async Task SearchMovie(string keyword, int page)
        {
            this.Displayables.Clear();
            this.SearchString = keyword;
            this.WasSearched = true;
            this.HeaderString = "Keresés...";
            var movieService = new MovieServices();
            var searchedMovies = await movieService.GetMoviesBySearchAsync(keyword, page);
            if (searchedMovies.movies.Count == 0)
            {
                this.HeaderString = "Nincs találat a keresésre";
            }
            else
            {
                foreach (var movie in searchedMovies.movies)
                {
                    movie.poster_path = "https://image.tmdb.org/t/p/w500/" + movie.poster_path;
                    this.Displayables.Add(new TvSeriesOrMovieDisplaySmall(true, movie.poster_path, movie.Title, movie.id));
                }
                this.HeaderString = $"{searchedMovies.movies.Count} találat van";
            }
            this.SeachPagesCount = searchedMovies.total_pages;
        }
        /// <summary>
        /// Function to get the top rated movies
        /// </summary>
        /// <param name="page">The number of page that was requested</param>
        /// <returns></returns>
        public async Task GetTopRatedMovies(int page)
        {
            this.Displayables.Clear();
            var service = new MovieServices();
            var topRatedMovies = await service.GetTopRatedMoviesAsync(page);
            foreach (var item in topRatedMovies.movies)
            {
                item.poster_path = "https://image.tmdb.org/t/p/w500" + item.poster_path;
                this.Displayables.Add(new TvSeriesOrMovieDisplaySmall(true, item.poster_path, item.Title, item.id));
            }
            this.TopRatedPagesCount = topRatedMovies.total_pages;
        }
        /// <summary>
        /// Function for searching tv series
        /// </summary>
        /// <param name="searchString">The name of the tv series to search for</param>
        /// <param name="pageNumber">The number of page the was requested</param>
        /// <returns></returns>
        internal async Task SearchTvSeries(string searchString, int pageNumber)
        {
            this.Displayables.Clear();
            this.SearchString = searchString;
            this.WasSearched = true;
            this.HeaderString = "Keresés...";
            var tvs = new TvSeriesService();
            var topRatedTemp = await tvs.GetTvSeriesBySearchAsync(this.SearchString, PageNumber);
            if (topRatedTemp.tvSeries.Count == 0)
            {
                this.HeaderString = "Nincs találat a keresésre";
            }
            else
            {
                foreach (var tvSeries in topRatedTemp.tvSeries)
                {
                    tvSeries.poster_path = "https://image.tmdb.org/t/p/w500" + tvSeries.poster_path;
                    this.Displayables.Add(new TvSeriesOrMovieDisplaySmall(false, tvSeries.poster_path, tvSeries.name, tvSeries.id));
                }
            }
            this.SeachPagesCount = topRatedTemp.total_pages;
        }
        /// <summary>
        /// Function to get the top rated tv series
        /// </summary>
        /// <param name="pageNumber">The pagenumber that was requested</param>
        /// <returns></returns>
        public async Task GetTopRatedTvSeries(int pageNumber)
        {
            var tvs = new TvSeriesService();
            var topRatedTemp = await tvs.GetTopRatedTvSeriesAsync(PageNumber);
            this.Displayables.Clear();
            foreach (var tvSeries in topRatedTemp.tvSeries)
            {
                tvSeries.poster_path = "https://image.tmdb.org/t/p/w500" + tvSeries.poster_path;
                this.Displayables.Add(new TvSeriesOrMovieDisplaySmall(false, tvSeries.poster_path, tvSeries.name, tvSeries.id));
            }
            this.SeachPagesCount = topRatedTemp.total_pages;
        }

    }
}
