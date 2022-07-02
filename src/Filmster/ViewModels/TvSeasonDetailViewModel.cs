using Filmster.Common.Models;
using Filmster.Common.Services;
using Filmster.Extensions;
using Filmster.Helpers;
using Filmster.Services;
using Filmster.ViewModelBases;
using Filmster.Views;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.TvShows;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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

        private TvShowSeasonEpisodeNumbers _tvShowSeasonEpisodeNumbers;
        public TvShowSeasonEpisodeNumbers TvShowSeasonEpisodeNumbers
        {
            get { return _tvShowSeasonEpisodeNumbers; }
            set { Set(ref _tvShowSeasonEpisodeNumbers, value); }
        }

        private Video _video;
        public Video Video
        {
            get { return _video; }
            set { Set(ref _video, value); }
        }

        private double _voteAverage;
        public double VoteAverage
        {
            get { return _voteAverage; }
            set { Set(ref _voteAverage, value); }
        }

        private int _voteCount;
        public int VoteCount
        {
            get { return _voteCount; }
            set { Set(ref _voteCount, value); }
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

        public async Task LoadTvSeasonAsync(TvShowSeasonEpisodeNumbers tvShowSeasonEpisodeNumbers)
        {
            TvSeason = await TMDbService.GetTvSeasonAsync(tvShowSeasonEpisodeNumbers.TvShowId, tvShowSeasonEpisodeNumbers.TvSeasonNumber);
            if (TvSeason == null)
            {
                NavigationService.GoBack();
                return;
            }
            TvShowSeasonEpisodeNumbers = tvShowSeasonEpisodeNumbers;
            SelectedPoster = GetSelectedPoster();
            (VoteAverage, VoteCount) = VoteHelper.GetVoteAverageVoteCount(TvSeason.Episodes.Select(episode => (episode.VoteAverage, episode.VoteCount)));
            Video = TvSeason.Videos.Results.FirstOrDefault();
            Cast.AddRange(TvSeason.Credits.Cast.Take(TMDbService.DefaultCastCrewBackdropCount));
            Crew.AddRange(TvSeason.Credits.Crew.Take(TMDbService.DefaultCastCrewBackdropCount));
        }

        private ImageData GetSelectedPoster()
        {
            return TvSeason.Images.Posters.Find(poster => poster.FilePath == TvSeason.PosterPath) ?? TvSeason.Images.Posters.FirstOrDefault();
        }

        private void CastToggled(bool isChecked)
        {
            if (isChecked)
            {
                Cast.AddRange(TvSeason.Credits.Cast.Skip(TMDbService.DefaultCastCrewBackdropCount));
            }
            else
            {
                Cast.Keep(TMDbService.DefaultCastCrewBackdropCount);
            }
        }

        private void CrewToggled(bool isChecked)
        {
            if (isChecked)
            {
                Crew.AddRange(TvSeason.Credits.Crew.Skip(TMDbService.DefaultCastCrewBackdropCount));
            }
            else
            {
                Crew.Keep(TMDbService.DefaultCastCrewBackdropCount);
            }
        }

        public void PosterClicked(object sender, TappedRoutedEventArgs e)
        {
            if (e.OriginalSource is Image)
            {
                NavigationService.Navigate<ImageGalleryPage>(new ImageGalleryNavigationParameter
                {
                    ImagePaths = TvSeason.Images.Posters.Select(image => image.FilePath),
                    SelectedImagePath = SelectedPoster.FilePath,
                });
            }
        }
    }
}
