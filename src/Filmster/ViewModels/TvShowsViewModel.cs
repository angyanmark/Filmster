using Filmster.Helpers;
using Filmster.ViewModelBases;
using Microsoft.Toolkit.Uwp;
using TMDbLib.Objects.Search;

namespace Filmster.ViewModels
{
    public class TvShowsViewModel : MediaViewModelBase
    {
        public IncrementalLoadingCollection<PopularTvShowsSource, SearchTv> PopularTvShows { get; set; } = new IncrementalLoadingCollection<PopularTvShowsSource, SearchTv>();
        public IncrementalLoadingCollection<TopRatedTvShowsSource, SearchTv> TopRatedTvShows { get; set; } = new IncrementalLoadingCollection<TopRatedTvShowsSource, SearchTv>();
    }
}
