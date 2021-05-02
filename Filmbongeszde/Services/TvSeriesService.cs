using Filmbongeszde.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Filmbongeszde.Services
{
    class TvSeriesService
    {
        private readonly Uri serverUrl = new Uri("https://api.themoviedb.org/3/");
        private readonly string apiKey = "?api_key=2b94e2a30de7a70832fae1f0d6193472";
        public async Task<TvSeries> GetTvSeriesByIdAsync(int Id, string language = "hu-HU")
        {
            return await GetAsync<TvSeries>(new Uri(serverUrl, $"tv/{Id}{apiKey}&language={language}"));
        }
        public async Task<TvSeriesSearch> GetTvSeriesBySearchAsync(string keyword, int pageNumner, string language = "hu-HU")
        {
            return await GetAsync<TvSeriesSearch>(new Uri(serverUrl, $"search/tv{apiKey}&language={language}&page={pageNumner}&query={keyword}&include_adult=false"));
        }
        public async Task<TvSeriesSearch> GetTopRatedTvSeriesAsync(int pageNumber, string languange = "hu-HU")
        {
            return await GetAsync<TvSeriesSearch>(new Uri(serverUrl, "tv/top_rated" + apiKey + "&language=" + languange + "&page=" + pageNumber));
        }
        public async Task<Season> GetSeasonOfTvSeriesAsync(int seasonNumber, int tvSeriesId, string languange = "hu-HU")
        {
            return await GetAsync<Season>(new Uri(serverUrl, $"tv/{tvSeriesId}/season/{seasonNumber}{apiKey}&language={languange}"));
        }
        //https://api.themoviedb.org/3/tv/1100/season/1?api_key=2b94e2a30de7a70832fae1f0d6193472&language=hu-HU
        public async Task<TvSeriesSearch> GetSimiliarTvSeriesByIdAsync(int tvSeriesId, string language = "hu-HU")
        {
            return await GetAsync<TvSeriesSearch>(new Uri(serverUrl, $"tv/{tvSeriesId}/similar{apiKey}&language={language}&page=1"));
        }
        //https://api.themoviedb.org/3/tv/1100/similar?api_key=2b94e2a30de7a70832fae1f0d6193472&language=hu-HU&page=1
        private async Task<T> GetAsync<T>(Uri uri)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                var json = await response.Content.ReadAsStringAsync();
                T result = JsonConvert.DeserializeObject<T>(json);
                return result;
            }
        }
    }
}
