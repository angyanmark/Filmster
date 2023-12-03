using Filmster.Helpers;
using Filmster.ViewModelBases;
using Microsoft.Toolkit.Uwp;
using TMDbLib.Objects.Search;

namespace Filmster.ViewModels
{
    public class MoviesViewModel : MediaViewModelBase
    {
        public IncrementalLoadingCollection<TrendingMoviesSource, SearchMovie> TrendingMovies { get; } = new IncrementalLoadingCollection<TrendingMoviesSource, SearchMovie>();
        public IncrementalLoadingCollection<PopularMoviesSource, SearchMovie> PopularMovies { get; } = new IncrementalLoadingCollection<PopularMoviesSource, SearchMovie>();
        public IncrementalLoadingCollection<NowPlayingMoviesSource, SearchMovie> NowPlayingMovies { get; } = new IncrementalLoadingCollection<NowPlayingMoviesSource, SearchMovie>();
        public IncrementalLoadingCollection<UpcomingMoviesSource, SearchMovie> UpcomingMovies { get; } = new IncrementalLoadingCollection<UpcomingMoviesSource, SearchMovie>();
        public IncrementalLoadingCollection<TopRatedMoviesSource, SearchMovie> TopRatedMovies { get; } = new IncrementalLoadingCollection<TopRatedMoviesSource, SearchMovie>();
    }
}
