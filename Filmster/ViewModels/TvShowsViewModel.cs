using Filmster.Core.Services;
using Filmster.Helpers;
using Filmster.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.Trending;

namespace Filmster.ViewModels
{
    public class TvShowsViewModel : Observable
    {
        public ObservableCollection<SearchTv> TrendingTvShows { get; set; } = new ObservableCollection<SearchTv>();
        public ObservableCollection<SearchTv> PopularTvShows { get; set; } = new ObservableCollection<SearchTv>();
        public ObservableCollection<SearchTv> TopRatedTvShows { get; set; } = new ObservableCollection<SearchTv>();

        public ICommand TvShowClickedCommand;

        public TvShowsViewModel()
        {
            _ = GetTvShowsAsync();
            SetCommands();
        }

        private void SetCommands()
        {
            TvShowClickedCommand = new RelayCommand<SearchTv>(TvShowClicked);
        }

        private async Task GetTvShowsAsync()
        {
            await GetTrendingTvShowsAsync();
            await GetPopularTvShowsAsync();
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

        private async Task GetPopularTvShowsAsync()
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

        private void TvShowClicked(SearchTv tvShow)
        {
            NavigationService.NavigateToSearchMediaDetail(tvShow);
        }
    }
}
