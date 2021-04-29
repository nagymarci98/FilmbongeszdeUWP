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
    public class PersonService
    {
        private readonly Uri serverUrl = new Uri("https://api.themoviedb.org/3/");
        private readonly string apiKey = "?api_key=2b94e2a30de7a70832fae1f0d6193472";
        public async Task<Person> GetPersonByIdAsync(int Id, string language = "en-US")
        {
            return await GetAsync<Person>(new Uri(serverUrl, "person/" + Id.ToString() + apiKey+"&"+language));
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
