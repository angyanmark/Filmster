using Filmster.Core.Services;
using Filmster.Helpers;
using System.Threading.Tasks;
using TMDbLib.Objects.Movies;

namespace Filmster.ViewModels
{
    public class MovieDetailViewModel : Observable
    {
        private Movie _movie;
        public Movie Movie
        {
            get { return _movie; }
            set { Set(ref _movie, value); }
        }

        public MovieDetailViewModel()
        {
        }

        public async Task LoadMovie(int id)
        {
            Movie = await TMDbService.GetMovieAsync(id);
        }
    }
}
