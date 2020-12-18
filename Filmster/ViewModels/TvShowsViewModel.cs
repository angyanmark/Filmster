using Filmster.Core.Services;
using Filmster.Helpers;
using Filmster.Services;
using Filmster.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TMDbLib.Objects.Search;

namespace Filmster.ViewModels
{
    public class TvShowsViewModel : Observable
    {
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
            await GetPopularTvShowsAsync();
            await GetTopRatedTvShowsAsync();
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
            NavigationService.Navigate(typeof(TvShowDetailPage), tvShow.Id);
        }
    }
}
