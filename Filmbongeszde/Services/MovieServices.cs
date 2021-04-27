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
        private readonly Uri serverUrl = new Uri("https://api.themoviedb.org/3/");
        private readonly string apiKey = "?api_key=2b94e2a30de7a70832fae1f0d6193472";
        public async Task<SearchQueryResult> GetTopRatedMoviesAsync(int pageNumber, string languange="en-US")
        {
            return await GetAsync<SearchQueryResult>(new Uri(serverUrl, "movie/top_rated"+apiKey+"&language="+languange+"&page="+pageNumber));
        }
        public async Task<SearchQueryResult> GetMoviesBySearchAsync(string keyword, string language = "en-US")
        {
            return await GetAsync<SearchQueryResult>(new Uri(serverUrl,"search/movie"+apiKey+"&language="+language+"&query="+keyword+"&page=1&include_adult=false"));
        }
        public async Task<Movie> GetMovieByIdAsync(int Id, string language="en-US")
        {
            return await GetAsync<Movie>(new Uri(serverUrl, "movie/"+Id.ToString()+apiKey));
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
