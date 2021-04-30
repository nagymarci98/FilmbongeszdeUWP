using Filmbongeszde.Models;
using Filmbongeszde.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Filmbongeszde.Views;
using Template10.Services.NavigationService;

namespace Filmbongeszde.ViewModels
{
    public class MovieDetailsPageViewModel : ViewModelBase
    {
        #region Properties
        private ObservableCollection<Cast> actors = new ObservableCollection<Cast>();
        public ObservableCollection<Cast> Actors
        {
            get { return actors; }
            set { actors = value; }
        }
        private ObservableCollection<Movie> similiarMovies = new ObservableCollection<Movie>();
        public ObservableCollection<Movie> SimiliarMovies
        {
            get { return similiarMovies; }
            set { similiarMovies = value; }
        }
        private Movie movie;
        public Movie Movie
        {
            get { return movie; }
            set {Set(ref movie, value);}
        }
        private string movieDuriation;
        public string MovieDuriation {
            get { return movieDuriation; }
            set { Set(ref movieDuriation, value); } 
        }
        private string revenue;
        public string Revenue
        {
            get { return revenue; }
            set { Set(ref revenue, value); }
        }
        private string budget;
        public string Budget {
            get { return budget; }
            set { Set(ref budget, value);} 
        }
        private string rating;
        public string Rating { 
            get{return rating;}
            set {Set(ref rating, value);}
        }
        private string genres;
        public string Genres { 
            get{ return genres ;}
            set {Set(ref genres, value);}
        }
        private string views;
        public string Views { 
            get{return views;}
            set {Set(ref views, value);}
        }
        private string similiarMoviesText;

        public string SimiliarMoviesText
        {
            get { return similiarMoviesText; }
            set { Set(ref similiarMoviesText ,value); }
        }
        


        #endregion
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            //Get the movie by Id
            var movieId = (int)parameter;
            var movieService = new MovieServices();
            var movieTemp = await movieService.GetMovieByIdAsync(movieId);
            this.setMovieDisplayable(movieTemp);
            this.Movie = movieTemp;
            //Get the credits of the movie
            this.Actors.Clear();
            this.SimiliarMovies.Clear();
            var credits = await movieService.GetMovieCredits(movieId);
            foreach (var item in credits.cast)
            {
                item.profile_path = "https://image.tmdb.org/t/p/w500/" + item.profile_path;
                this.Actors.Add(item);
            }
            var smiliarMoviesTemp = await movieService.GetSimiliarMoviesAsync(movieId);
            foreach (var item in smiliarMoviesTemp.movies)
            {
                item.poster_path = "https://image.tmdb.org/t/p/w500/" + item.poster_path;
                this.SimiliarMovies.Add(item);
            }
            if(smiliarMoviesTemp.movies.Count == 0)
            {
                this.SimiliarMoviesText = "";
            }
            else
            {
                this.SimiliarMoviesText = "Hasonló Filmek";

            }
            await base.OnNavigatedToAsync(parameter, mode, state);
        }

        /// <summary>
        /// Making a movie displayable. Meaning, adding the full path for poster, imdb page, release date, so we dont have to
        /// make string concats in XAML.
        /// </summary>
        /// <param name="movie"></param>
        /// The movie object we're making displayable.
        private void setMovieDisplayable(Movie movie)
        {
            movie.poster_path ="https://image.tmdb.org/t/p/w500/" + movie.poster_path;
            movie.imdb_id = "https://www.imdb.com/title/" + movie.imdb_id;
            movie.release_date = "Megjelenés: " + movie.release_date;
            this.MovieDuriation = "Időtartam: " + movie.runtime.ToString()+" perc";
            this.Revenue = "Teljes bevétel: $" + String.Format("{0:n0}", movie.revenue);
            this.Budget = "Költségvetés: $" + String.Format("{0:n0}", movie.budget);
            this.Rating = "Értékelés: " + movie.vote_average.ToString() + "/10 "+movie.vote_count.ToString()+" értékelésből";
            this.Views = "Nézettség: " + movie.popularity.ToString();
            if (movie.genres.Count > 1)
            {
                this.Genres = "Műfajok: ";
                foreach (var genre in movie.genres)
                {
                    this.Genres = this.Genres+" "+genre.name;
                }
            }
            else{
                this.Genres = "Műfaj: " + movie.genres[0].ToString();
            }
        }

        public void NavigateToActorDetails(int actorId)
        {
            NavigationService.Navigate(typeof(ActorDetailsPage), actorId);
        }
        public void NavigateToMovieDetails(int movieId)
        {
            NavigationService.Navigate(typeof(MovieDetailsPage), movieId);
        }

    }
}
