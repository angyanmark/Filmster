using Filmster.Core.Services;
using Filmster.Helpers;
using Filmster.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.TvShows;

namespace Filmster.ViewModels
{
    public class TvSeasonDetailViewModel : MediaViewModelBase
    {
        private TvSeason _tvSeason;
        public TvSeason TvSeason
        {
            get { return _tvSeason; }
            set { Set(ref _tvSeason, value); }
        }

        private ImageData _selectedPoster;
        public ImageData SelectedPoster
        {
            get { return _selectedPoster; }
            set { Set(ref _selectedPoster, value); }
        }

        public ObservableCollection<Cast> Cast { get; set; } = new ObservableCollection<Cast>();
        public ObservableCollection<Crew> Crew { get; set; } = new ObservableCollection<Crew>();

        private bool _isCastChecked;
        public bool IsCastChecked
        {
            get { return _isCastChecked; }
            set
            {
                Set(ref _isCastChecked, value);
                CastToggled(IsCastChecked);
            }
        }

        private bool _isCrewChecked;
        public bool IsCrewChecked
        {
            get { return _isCrewChecked; }
            set
            {
                Set(ref _isCrewChecked, value);
                CrewToggled(IsCrewChecked);
            }
        }

        public TvSeasonDetailViewModel()
        {
        }

        public async Task LoadTvSeason(int tvShowId, int seasonNumber)
        {
            TvSeason = await TMDbService.GetTvSeasonAsync(tvShowId, seasonNumber);
            if (TvSeason == null)
            {
                NavigationService.GoBack();
                return;
            }
            SelectedPoster = GetSelectedPoster();
            CastToggled(false);
            CrewToggled(false);
        }

        private ImageData GetSelectedPoster()
        {
            return TvSeason.Images.Posters.Find(poster => poster.FilePath == TvSeason.PosterPath) ?? TvSeason.Images.Posters.FirstOrDefault();
        }

        private void CastToggled(bool isChecked)
        {
            var cast = isChecked ? TvSeason.Credits.Cast : TvSeason.Credits.Cast.Take(TMDbService.DefaultCastCrewBackdropCount);
            Cast.Clear();
            foreach (var c in cast)
            {
                Cast.Add(c);
            }
        }

        private void CrewToggled(bool isChecked)
        {
            var crew = isChecked ? TvSeason.Credits.Crew : TvSeason.Credits.Crew.Take(TMDbService.DefaultCastCrewBackdropCount);
            Crew.Clear();
            foreach (var c in crew)
            {
                Crew.Add(c);
            }
        }
    }
}
