using Filmster.Core.Services;
using Filmster.Helpers;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace Filmster.ViewModels
{
    public class MoviesViewModel : Observable
    {
        public SearchContainer<SearchMovie> PopularMovies { get; set; }

        public MoviesViewModel()
        {
            PopularMovies = TMDbService.GetPopularMovies();
        }
    }
}
