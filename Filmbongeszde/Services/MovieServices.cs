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
        public async Task<SearchMovieResult> GetTopRatedMoviesAsync(int pageNumber, string languange="hu-HU")
        {
            return await GetAsync<SearchMovieResult>(new Uri(serverUrl, "movie/top_rated"+apiKey+"&language="+languange+"&page="+pageNumber));
        }
        public async Task<SearchMovieResult> GetMoviesBySearchAsync(string keyword,int pageNumner, string language = "hu-HU")
        {
            return await GetAsync<SearchMovieResult>(new Uri(serverUrl,"search/movie"+apiKey+"&language="+language+"&query="+keyword+"&page="+pageNumner+"&include_adult=false"));
        }
        public async Task<SearchMovieResult> GetSimiliarMoviesAsync(int movieId, string language = "hu-HU")
        {
           // return await GetAsync<SearchMovieResult>(new Uri(serverUrl, "movie/" + movieId+""+apiKey + "&language=" +language+"&page=1"));
            return await GetAsync<SearchMovieResult>(new Uri(serverUrl,$"movie/{movieId}/similar{apiKey}&language={language}&page=1"));
        }
        public async Task<MovieCredits> GetMovieCredits(int movieId, string language = "hu-HU")
        {
            return await GetAsync<MovieCredits>(new Uri(serverUrl, "movie/" + movieId.ToString() +"/credits"+apiKey+"&language="+language));
        }

        public async Task<Movie> GetMovieByIdAsync(int Id, string language= "hu-HU")
        {
            return await GetAsync<Movie>(new Uri(serverUrl, "movie/"+Id.ToString()+apiKey+"&language="+language));
        }
        public async Task<ListOfGenres> GetAllGenreAsync(string language = "hu-HU")
        {
            return await GetAsync<ListOfGenres>(new Uri("https://api.themoviedb.org/3/genre/movie/list?api_key=2b94e2a30de7a70832fae1f0d6193472&language=hu-HU"));
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
