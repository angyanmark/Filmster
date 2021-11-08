using Filmster.Common.Services;
using Filmster.ViewModelBases;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TMDbLib.Objects.Search;

namespace Filmster.ViewModels
{
    public class TvShowsViewModel : MediaViewModelBase
    {
        public ObservableCollection<SearchTv> PopularTvShows { get; set; } = new ObservableCollection<SearchTv>();
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
            var tvshows = await TMDbService.GetPopularTvShowsAsync();
            foreach (var tvshow in tvshows)
            {
                PopularTvShows.Add(tvshow);
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
