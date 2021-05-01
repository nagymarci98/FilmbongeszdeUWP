using Filmbongeszde.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            return await GetAsync<TvSeries>(new Uri(serverUrl, ""));
        }
        public async Task<TvSeriesSearch> GetTvSeriesBySearchAsync(string keyword, int pageNumner, string language = "hu-HU")
        {
            return await GetAsync<TvSeriesSearch>(new Uri(serverUrl, ""));
        }
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
