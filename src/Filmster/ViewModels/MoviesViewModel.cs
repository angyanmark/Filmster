using Filmster.Helpers;
using Filmster.ViewModelBases;
using Microsoft.Toolkit.Uwp;
using TMDbLib.Objects.Search;

namespace Filmster.ViewModels
{
    public class MoviesViewModel : MediaViewModelBase
    {
        public IncrementalLoadingCollection<PopularMoviesSource, SearchMovie> PopularMovies { get; set; } = new IncrementalLoadingCollection<PopularMoviesSource, SearchMovie>();
        public IncrementalLoadingCollection<UpcomingMoviesSource, SearchMovie> UpcomingMovies { get; set; } = new IncrementalLoadingCollection<UpcomingMoviesSource, SearchMovie>();
        public IncrementalLoadingCollection<TopRatedMoviesSource, SearchMovie> TopRatedMovies { get; set; } = new IncrementalLoadingCollection<TopRatedMoviesSource, SearchMovie>();
    }
}
