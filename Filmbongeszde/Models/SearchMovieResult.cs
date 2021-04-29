using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmbongeszde.Models
{

    public class SearchMovieResult
    {
        public int page { get; set; }

        [JsonProperty(PropertyName = "results")]
        public List<Movie> movies { get; set; }
        public int total_pages { get; set; }
        public int total_results { get; set; }
    }



}
