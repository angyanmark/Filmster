using Filmster.Core.Services;
using Filmster.Helpers;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.Trending;

namespace Filmster.ViewModels
{
    public class TvShowsViewModel : MediaViewModelBase
    {
        public ObservableCollection<SearchTv> TrendingTvShows { get; set; } = new ObservableCollection<SearchTv>();
        public ObservableCollection<SearchTv> TopRatedTvShows { get; set; } = new ObservableCollection<SearchTv>();

        public TvShowsViewModel()
        {
            _ = GetTvShowsAsync();
        }

        private async Task GetTvShowsAsync()
        {
            await GetTrendingTvShowsAsync();
            await GetTopRatedTvShowsAsync();
        }

        private async Task GetTrendingTvShowsAsync()
        {
            var tvshows = await TMDbService.GetTrendingTvShowsAsync(TimeWindow.Week);
            foreach (var tvshow in tvshows)
            {
                TrendingTvShows.Add(tvshow);
            }
        }

        private async Task GetTopRatedTvShowsAsync()
        {
            var tvshows = await TMDbService.GetTopRatedTvShowsAsync();
            foreach (var tvshow in tvshows)
            {
                TopRatedTvShows.Add(tvshow);
            }
        }
    }
}
