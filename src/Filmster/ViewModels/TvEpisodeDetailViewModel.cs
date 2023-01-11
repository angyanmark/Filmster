using Filmster.Common.Models;
using Filmster.Common.Services;
using Filmster.Extensions;
using Filmster.Helpers;
using Filmster.Services;
using Filmster.ViewModelBases;
using Filmster.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TMDbLib.Objects.General;
using TMDbLib.Objects.TvShows;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Filmster.ViewModels
{
    public class TvEpisodeDetailViewModel : RatableMediaViewModelBase
    {
        private TvEpisode _tvEpisode;
        public TvEpisode TvEpisode
        {
            get { return _tvEpisode; }
            set { Set(ref _tvEpisode, value); }
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

        private string _directors;
        public string Directors
        {
            get { return _directors; }
            set { Set(ref _directors, value); }
        }

        public ObservableCollection<Cast> GuestStars { get; set; } = new ObservableCollection<Cast>();
        public ObservableCollection<Cast> Cast { get; set; } = new ObservableCollection<Cast>();
        public ObservableCollection<Crew> Crew { get; set; } = new ObservableCollection<Crew>();
        public ObservableCollection<ImageData> Images { get; set; } = new ObservableCollection<ImageData>();

        private bool _isGuestStarChecked;
        public bool IsGuestStarChecked
        {
            get { return _isGuestStarChecked; }
            set
            {
                Set(ref _isGuestStarChecked, value);
                GuestStarToggled(_isGuestStarChecked);
            }
        }

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

        private bool _isImagesChecked;
        public bool IsImagesChecked
        {
            get { return _isImagesChecked; }
            set
            {
                Set(ref _isImagesChecked, value);
                ImagesToggled(_isImagesChecked);
            }
        }

        public ICommand ImageClickedCommand;

        public TvEpisodeDetailViewModel()
        {
            ImageClickedCommand = new RelayCommand<ImageData>(ImageClicked);
        }

        public async Task LoadTvEpisodeAsync(TvShowSeasonEpisodeNumbers tvShowSeasonEpisodeNumbers)
        {
            TvEpisode = await TMDbService.GetTvEpisodeAsync(tvShowSeasonEpisodeNumbers.TvShowId, tvShowSeasonEpisodeNumbers.TvSeasonNumber, tvShowSeasonEpisodeNumbers.TvEpisodeNumber.Value, IsLoggedIn);
            TvEpisode.ShowId = tvShowSeasonEpisodeNumbers.TvShowId;
            if (TvEpisode == null)
            {
                NavigationService.GoBack();
                return;
            }
            TvShowSeasonEpisodeNumbers = tvShowSeasonEpisodeNumbers;
            SetAccountState(TvEpisode.AccountStates);
            SelectedPoster = GetSelectedPoster();
            Directors = GetDirectors();
            Video = VideoSelectService.SelectVideo(TvEpisode.Videos.Results);
            GuestStars.AddRange(TvEpisode.Credits.GuestStars.Take(TMDbService.DefaultCastCrewBackdropCount));
            Cast.AddRange(TvEpisode.Credits.Cast.Take(TMDbService.DefaultCastCrewBackdropCount));
            Crew.AddRange(TvEpisode.Credits.Crew.Take(TMDbService.DefaultCastCrewBackdropCount));
            Images.AddRange(TvEpisode.Images.Stills.Take(TMDbService.DefaultCastCrewBackdropCount));
        }

        private ImageData GetSelectedPoster()
        {
            return TvEpisode.Images.Stills.Find(image => image.FilePath == TvEpisode.StillPath) ?? TvEpisode.Images.Stills.FirstOrDefault();
        }

        private string GetDirectors()
        {
            var directors = TvEpisode.Credits.Crew.FindAll(crew => crew.Job.ToLower() == "director");
            return string.Join(", ", directors.Select(director => director.Name));
        }

        private void GuestStarToggled(bool isChecked)
        {
            if (isChecked)
            {
                GuestStars.AddRange(TvEpisode.Credits.GuestStars.Skip(TMDbService.DefaultCastCrewBackdropCount));
            }
            else
            {
                GuestStars.Keep(TMDbService.DefaultCastCrewBackdropCount);
            }
        }

        private void CastToggled(bool isChecked)
        {
            if (isChecked)
            {
                Cast.AddRange(TvEpisode.Credits.Cast.Skip(TMDbService.DefaultCastCrewBackdropCount));
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
                Crew.AddRange(TvEpisode.Credits.Crew.Skip(TMDbService.DefaultCastCrewBackdropCount));
            }
            else
            {
                Crew.Keep(TMDbService.DefaultCastCrewBackdropCount);
            }
        }

        private void ImagesToggled(bool isChecked)
        {
            if (isChecked)
            {
                Images.AddRange(TvEpisode.Images.Stills.Skip(TMDbService.DefaultCastCrewBackdropCount));
            }
            else
            {
                Images.Keep(TMDbService.DefaultCastCrewBackdropCount);
            }
        }

        private void ImageClicked(ImageData selectedImage)
        {
            NavigationService.Navigate<ImageGalleryPage>(new ImageGalleryNavigationParameter
            {
                ImagePaths = TvEpisode.Images.Stills.Select(image => image.FilePath),
                SelectedImagePath = selectedImage.FilePath,
            });
        }

        public void PosterClicked(object sender, TappedRoutedEventArgs e)
        {
            if (e.OriginalSource is Image)
            {
                NavigationService.Navigate<ImageGalleryPage>(new ImageGalleryNavigationParameter
                {
                    ImagePaths = TvEpisode.Images.Stills.Select(image => image.FilePath),
                    SelectedImagePath = SelectedPoster.FilePath,
                });
            }
        }

        public async void RatingChangedAsync(RatingControl sender, object args)
        {
            await ChangeRatingAsync(MediaType.Episode, sender.Value, TvEpisode.ShowId, TvEpisode.SeasonNumber, TvEpisode.EpisodeNumber);
        }
    }
}
