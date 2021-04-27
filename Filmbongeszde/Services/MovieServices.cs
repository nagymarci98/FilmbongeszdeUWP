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
    class MovieServices
    {
        private readonly Uri serverUrl = new Uri("https://api.themoviedb.org/3/movie/");
        //https://api.themoviedb.org/3/search/movie?api_key=2b94e2a30de7a70832fae1f0d6193472&language=en-US&query=hulk&page=1&include_adult=false
        public async Task<SearchQueryResult> GetTopRatedMoviesAsync()
        {
            return await GetAsync<SearchQueryResult>(new Uri(serverUrl, "top_rated?api_key=2b94e2a30de7a70832fae1f0d6193472"));
        }
        public async Task<SearchQueryResult> GetMoviesBySearch(string keyword)
        {
            return await GetAsync<SearchQueryResult>(new Uri("https://api.themoviedb.org/3/search/movie?api_key=2b94e2a30de7a70832fae1f0d6193472&language=en-US&query=hulk&page=1&include_adult=false"));
        }
        public async Task<Movie> GetMovieTestAsync()
        {
            return await GetAsync<Movie>(new Uri(serverUrl, "550?api_key=2b94e2a30de7a70832fae1f0d6193472"));
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
