using Filmster.Helpers;
using Filmster.ViewModelBases;
using Microsoft.Toolkit.Uwp;
using TMDbLib.Objects.Search;

namespace Filmster.ViewModels
{
    public class TvShowsViewModel : MediaViewModelBase
    {
        public IncrementalLoadingCollection<TrendingTvShowsSource, SearchTv> TrendingTvShows { get; } = new IncrementalLoadingCollection<TrendingTvShowsSource, SearchTv>();
        public IncrementalLoadingCollection<PopularTvShowsSource, SearchTv> PopularTvShows { get; } = new IncrementalLoadingCollection<PopularTvShowsSource, SearchTv>();
        public IncrementalLoadingCollection<TopRatedTvShowsSource, SearchTv> TopRatedTvShows { get; } = new IncrementalLoadingCollection<TopRatedTvShowsSource, SearchTv>();
    }
}
