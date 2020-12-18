using Filmster.Core.Services;
using Filmster.Helpers;
using System.Threading.Tasks;
using TMDbLib.Objects.TvShows;

namespace Filmster.ViewModels
{
    public class TvShowDetailViewModel : Observable
    {
        private TvShow _tvShow;
        public TvShow TvShow
        {
            get { return _tvShow; }
            set { Set(ref _tvShow, value); }
        }

        public TvShowDetailViewModel()
        {
        }

        public async Task LoadTvShow(int id)
        {
            TvShow = await TMDbService.GetTvShowAsync(id);
        }
    }
}
