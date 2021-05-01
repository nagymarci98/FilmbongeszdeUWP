using Filmbongeszde.Models;
using Filmbongeszde.Services;
using Filmbongeszde.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace Filmbongeszde.ViewModels
{
    class ActorDetailsPageViewModel : ViewModelBase
    {
        #region Props
        private ObservableCollection<CastActorMovies> actorsMovies = new ObservableCollection<CastActorMovies>();
        public ObservableCollection<CastActorMovies> ActorsMovies
        {
            get { return actorsMovies; }
            set { actorsMovies = value; }
        }
        private Person actor;
        public Person Actor
        {
            get { return actor; }
            set { Set(ref actor, value); }
        }
        private string knownAs;
        public string KnownAs
        {
            get { return knownAs; }
            set { Set(ref knownAs, value); }
        }
        private string bio;
        public string Bio
        {
            get { return bio; }
            set { Set(ref bio, value); }
        }

        private string birthDay;
        public string BirthDay
        {
            get { return birthDay; }
            set { Set(ref birthDay, value); }
        }
        private string imdbId;
        public string ImdbId
        {
            get { return imdbId; }
            set { Set(ref imdbId, value); }
        }
        private string name;
        public string Name
        {
            get { return name; }
            set { Set(ref name, value); }
        }
        private string deathDay;
        public string DeathDay
        {
            get { return deathDay; }
            set { Set(ref deathDay, value); }
        }
        private string bithPlace;
        public string BithPlace
        {
            get { return bithPlace; }
            set { Set(ref bithPlace, value); }
        }
        public int ActorId { get; set; }
        private string job;
        public string Job 
        {
            get { return job; }
            set { Set(ref job, value); }
        }
        private PersonService personService;
        #endregion

        public ActorDetailsPageViewModel()
        {
            this.personService = new PersonService();
            this.DeathDay = "";
        }
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            this.ActorId = (int)parameter;
            await getActorDetails(this.ActorId);
            await getActorsMovies(this.ActorId);
            await base.OnNavigatedToAsync(parameter, mode, state);
        }
        private async Task getActorDetails(int actorId)
        {
            var personDetails = await this.personService.GetPersonByIdAsync(actorId);
            makePersonDisplayable(personDetails);
            this.Actor = personDetails;
        }
        private async Task getActorsMovies(int actorId)
        {
            var movies = await this.personService.GetActorMoviesByIdAsync(actorId);
            foreach (var movie in movies.cast)
            {
                movie.backdrop_path = "https://image.tmdb.org/t/p/w500/" + movie.poster_path;
                this.actorsMovies.Add(movie);
            }
        }
        private void makePersonDisplayable(Person personDetails)
        {
            personDetails.profile_path = "https://image.tmdb.org/t/p/w500/" + personDetails.profile_path;
            personDetails.imdb_id = "https://www.imdb.com/name/" + personDetails.imdb_id;
            int counter = 1;
            this.KnownAs = "Ismert még az alábbi neveken:";
            foreach (var name in personDetails.also_known_as)
            {
                if(counter == 1)
                {
                    this.KnownAs = this.KnownAs + " " + name;
                }
                else
                {
                    this.KnownAs = this.KnownAs + " ," + name;
                }
                counter++;
            }
            this.BirthDay = "Születési idő: " + personDetails.birthday;
            this.BithPlace = "Születési hely: " + personDetails.place_of_birth;
            this.DeathDay = personDetails.deathday ?? $"Halálozás: {personDetails.deathday}";
            this.Job = "foglalkozás: " + personDetails.known_for_department;
        }
        internal void NavigateToMovieDetails(int id)
        {
            NavigationService.Navigate(typeof(MovieDetailsPage), id);
        }
    }
}
