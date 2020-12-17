using Filmster.Core.Services;
using Filmster.Helpers;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TMDbLib.Objects.Search;

namespace Filmster.ViewModels
{
    public class TvShowsViewModel : Observable
    {
        public ObservableCollection<SearchTv> PopularTvShows { get; set; } = new ObservableCollection<SearchTv>();

        public TvShowsViewModel()
        {
            _ = GetTvShows();
        }

        private async Task GetTvShows()
        {
            await GetPopularTvShowsAsync();
        }

        private async Task GetPopularTvShowsAsync()
        {
            var tvshows = await TMDbService.GetPopularTvShowsAsync();
            foreach (var tvshow in tvshows)
            {
                PopularTvShows.Add(tvshow);
            }
        }
    }
}
