using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmbongeszde.Models
{
    public class TvSeriesOrMovieDisplaySmall
    {
        public TvSeriesOrMovieDisplaySmall(bool isMovie, string picPath, string title, int id)
        {
            IsMovie = isMovie;
            PicPath = picPath;
            Title = title;
            this.id = id;
        }

        public bool IsMovie { get; set; }
        public string PicPath { get; set; }
        public string Title { get; set; }
        public int id { get; set; }
    }
}
