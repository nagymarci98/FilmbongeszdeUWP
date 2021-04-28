using Filmbongeszde.Models;
using Filmbongeszde.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;
using Template10.Services.NavigationService;
using System.Collections.ObjectModel;
using Filmbongeszde.Views;
namespace Filmbongeszde.ViewModels
{
    public class MovieDetailsPageViewModel : ViewModelBase
    {
        private Movie movie;

        public Movie Movie
        {
            get { return movie; }
            set { movie = value; }
        }
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var movieId = (int)parameter;
            var movieService = new MovieServices();
            this.Movie = await movieService.GetMovieByIdAsync(movieId);
            await base.OnNavigatedToAsync(parameter, mode, state);
        }
    }
}
