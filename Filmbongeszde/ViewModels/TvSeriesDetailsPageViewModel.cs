using Filmbongeszde.Models;
using Filmbongeszde.Services;
using Filmbongeszde.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace Filmbongeszde.ViewModels
{
    public class TvSeriesDetailsPageViewModel : ViewModelBase
    {
        private TvSeries tvSeries;
        public TvSeries TvSeries
        {
            get { return tvSeries; }
            set { Set(ref tvSeries, value); }
        }

        private ObservableCollection<Episode> episodes = new ObservableCollection<Episode>();
        public ObservableCollection<Episode> Episodes
        {
            get { return episodes; }
            set { episodes = value; }
        }
        private ObservableCollection<TvSeries> similiarTvSeries = new ObservableCollection<TvSeries>();
        public ObservableCollection<TvSeries> SimiliarTvSeries
        {
            get { return similiarTvSeries; }
            set { similiarTvSeries = value; }
        }
        private string firstEpisodeDate;

        public string FirstEpisodeDate
        {
            get { return firstEpisodeDate; }
            set { Set(ref firstEpisodeDate, value); }
        }

        private string genres;

        public string Genres
        {
            get { return genres; }
            set {Set(ref genres, value); }
        }
        private string lastEpisodeDate;

        public string LastEpisodeDate
        {
            get { return lastEpisodeDate; }
            set { Set(ref lastEpisodeDate, value); }
        }
        private int episodesCount;

        public int EpisodesCount
        {
            get { return episodesCount; }
            set { Set(ref episodesCount, value); }
        }
        private string status;

        public string Status
        {
            get { return status; }
            set { Set(ref status, value); }
        }
        private string rating;

        public string Rating
        {
            get { return rating; }
            set { Set( ref rating, value); }
        }
        private string similiarTvSeriesText;

        public string SimiliarTvSeriesText
        {
            get { return similiarTvSeriesText; }
            set { Set( ref similiarTvSeriesText, value); }
        }
        internal void NavigateToTvSerailsDetails(int id)
        {
            NavigationService.Navigate(typeof(TvSeriesDetailsPage), id);
        }
        public async override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var id = (int)parameter;
            this.TvSeries = await GetTvSeries(id);
            //this.Episodes = await GetTvSeriesEpisodes(TvSeries);
            //this.SimiliarTvSeries = await GetSimiliars(TvSeries);
            var service = new TvSeriesService();
            var similiarSeriesTemp = await service.GetSimiliarTvSeriesByIdAsync(id);
            foreach (var item in similiarSeriesTemp.tvSeries)
            {
                item.poster_path = "https://image.tmdb.org/t/p/w500/" + item.poster_path;
                this.SimiliarTvSeries.Add(item);
            }
            if (similiarSeriesTemp.tvSeries.Count == 0)
            {
                this.SimiliarTvSeriesText = "";
            }
            else
            {
                this.SimiliarTvSeriesText = "Hasonló Sorozatok";

            }
            foreach (var season in TvSeries.seasons)
            {
                var seasonTemp = await service.GetSeasonOfTvSeriesAsync(season.season_number, tvSeries.id);
                foreach (var episode in seasonTemp.episodes)
                {
                    episode.still_path = "https://image.tmdb.org/t/p/w500" + season.poster_path;
                    episode.EpisodeNumberText = episode.episode_number.ToString() + ".rész";
                    episode.SeasonNumberText = episode.season_number.ToString() + ".évad";
                    this.Episodes.Add(episode);
                }
            }
            await base.OnNavigatedToAsync(parameter, mode, state);
        }
        private async Task<TvSeries> GetTvSeries(int id)
        {
            var tvService = new TvSeriesService();
            var tv = await tvService.GetTvSeriesByIdAsync(id);
            makeTvSeriesDisplayable(tv);
            return tv;
        }

        private void makeTvSeriesDisplayable(TvSeries tv)
        {
            tv.backdrop_path = "https://image.tmdb.org/t/p/w500" + tv.backdrop_path;
            this.FirstEpisodeDate = "Első rész megjelenés: "+tv.first_air_date;
            this.Genres = "Műfaj:";
            int counter = 0;
            foreach (var genre in tv.genres)
            {
                if(counter == 0)
                {
                    this.Genres = this.Genres + " " + genre;
                }
                else
                {
                    this.Genres = this.Genres + " ," + genre;
                }
            }
            this.LastEpisodeDate = tv.last_air_date ?? "Utolsó epizód megjelenés: "+tv.last_air_date;
            this.Rating = $"{tv.vote_average}/10, {tv.vote_count} értékelésből";
            this.Status = "Jelenleg: "+tv.status;
        }
    }
}
