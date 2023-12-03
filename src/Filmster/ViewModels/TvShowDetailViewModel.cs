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
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Filmster.ViewModels
{
    public class TvShowDetailViewModel : RatableMediaViewModelBase
    {
        private TvShow _tvShow;
        public TvShow TvShow
        {
            get => _tvShow;
            set => Set(ref _tvShow, value);
        }

        private ImageData _selectedPoster;
        public ImageData SelectedPoster
        {
            get => _selectedPoster;
            set => Set(ref _selectedPoster, value);
        }

        private Video _video;
        public Video Video
        {
            get => _video;
            set => Set(ref _video, value);
        }

        private string _certification;
        public string Certification
        {
            get => _certification;
            set => Set(ref _certification, value);
        }

        private string _creators;
        public string Creators
        {
            get => _creators;
            set => Set(ref _creators, value);
        }

        private int _episodeRuntime;
        public int EpisodeRuntime
        {
            get => _episodeRuntime;
            set => Set(ref _episodeRuntime, value);
        }

        private string _genres;
        public string Genres
        {
            get => _genres;
            set => Set(ref _genres, value);
        }

        private string _networks;
        public string Networks
        {
            get => _networks;
            set => Set(ref _networks, value);
        }

        private string _productionCompanies;
        public string ProductionCompanies
        {
            get => _productionCompanies;
            set => Set(ref _productionCompanies, value);
        }

        public ObservableCollection<Cast> Cast { get; set; } = new ObservableCollection<Cast>();
        public ObservableCollection<Crew> Crew { get; set; } = new ObservableCollection<Crew>();
        public ObservableCollection<ImageData> Images { get; set; } = new ObservableCollection<ImageData>();

        private bool _isCastChecked;
        public bool IsCastChecked
        {
            get => _isCastChecked;
            set
            {
                Set(ref _isCastChecked, value);
                CastToggled(IsCastChecked);
            }
        }

        private bool _isCrewChecked;
        public bool IsCrewChecked
        {
            get => _isCrewChecked;
            set
            {
                Set(ref _isCrewChecked, value);
                CrewToggled(IsCrewChecked);
            }
        }

        private bool _isImagesChecked;
        public bool IsImagesChecked
        {
            get => _isImagesChecked;
            set
            {
                Set(ref _isImagesChecked, value);
                ImagesToggled(IsImagesChecked);
            }
        }

        public ICommand SearchTvSeasonClickedCommand;
        public ICommand ImageClickedCommand;
        public ICommand ShareClickedCommand;
        public ICommand ReviewsClickedCommand;

        public TvShowDetailViewModel()
        {
            SearchTvSeasonClickedCommand = new RelayCommand<SearchTvSeason>(SearchTvSeasonClicked);
            ImageClickedCommand = new RelayCommand<ImageData>(ImageClicked);
            ShareClickedCommand = new RelayCommand(ShareClicked);
            ReviewsClickedCommand = new RelayCommand(ReviewsClicked);
        }

        public async Task LoadTvShowAsync(int id)
        {
            TvShow = await TMDbService.GetTvShowAsync(id, IsLoggedIn);
            if (TvShow == null)
            {
                NavigationService.GoBack();
                return;
            }
            SetAccountState(TvShow.AccountStates);
            SelectedPoster = GetSelectedPoster();
            Creators = GetCreators();
            EpisodeRuntime = Convert.ToInt32(TvShow.EpisodeRunTime.DefaultIfEmpty().Average());
            Certification = GetCertification();
            Genres = GetGenres();
            Networks = GetNetworks();
            ProductionCompanies = GetProductionCompanies();
            Video = VideoSelectService.SelectVideo(TvShow.Videos.Results);
            Cast.AddRange(TvShow.Credits.Cast.Take(TMDbService.DefaultCastCrewBackdropCount));
            Crew.AddRange(TvShow.Credits.Crew.Take(TMDbService.DefaultCastCrewBackdropCount));
            Images.AddRange(TvShow.Images.Backdrops.Take(TMDbService.DefaultCastCrewBackdropCount));
        }

        private ImageData GetSelectedPoster() =>
            TvShow.Images.Posters.Find(poster => poster.FilePath == TvShow.PosterPath) ?? TvShow.Images.Posters.FirstOrDefault();

        private string GetCreators() =>
            string.Join(", ", TvShow.CreatedBy.Select(creator => creator.Name));

        private string GetCertification()
        {
            foreach (var rating in TvShow.ContentRatings.Results)
            {
                if (rating.Iso_3166_1.ToLower() == "us")
                {
                    if (!string.IsNullOrEmpty(rating.Rating))
                    {
                        return rating.Rating;
                    }
                }
            }
            return default;
        }

        private string GetGenres() =>
            string.Join(", ", TvShow.Genres.Select(genre => genre.Name));

        private string GetNetworks() =>
            string.Join(", ", TvShow.Networks.Select(network => network.Name));

        private string GetProductionCompanies() =>
            string.Join(", ", TvShow.ProductionCompanies.Select(productionCompany => productionCompany.Name));

        private void SearchTvSeasonClicked(SearchTvSeason searchTvSeason) =>
            NavigationService.Navigate<TvSeasonDetailPage>(new TvShowSeasonEpisodeNumbers
            {
                TvShowId = TvShow.Id,
                TvSeasonNumber = searchTvSeason.SeasonNumber,
                TvShowImdbId = TvShow.ExternalIds.ImdbId,
            });

        private void CastToggled(bool isChecked)
        {
            if (isChecked)
            {
                Cast.AddRange(TvShow.Credits.Cast.Skip(TMDbService.DefaultCastCrewBackdropCount));
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
                Crew.AddRange(TvShow.Credits.Crew.Skip(TMDbService.DefaultCastCrewBackdropCount));
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
                Images.AddRange(TvShow.Images.Backdrops.Skip(TMDbService.DefaultCastCrewBackdropCount));
            }
            else
            {
                Images.Keep(TMDbService.DefaultCastCrewBackdropCount);
            }
        }

        private void ImageClicked(ImageData selectedImage) =>
            NavigationService.Navigate<ImageGalleryPage>(new ImageGalleryNavigationParameter
            {
                ImagePaths = TvShow.Images.Backdrops.Select(image => image.FilePath),
                SelectedImagePath = selectedImage.FilePath,
            });

        private void ShareClicked() =>
            ShareService.Share(new ShareParameter
            {
                Text = TvShow.Name,
                Uri = new Uri($"{TMDbService.TMDbTvShowBaseUrl}{TvShow.Id}"),
            });

        private void ReviewsClicked() =>
            NavigationService.Navigate<ReviewsPage>(new ReviewsNavigationParameter
            {
                MediaTitle = TvShow.Name,
                MediaReleaseDate = TvShow.FirstAirDate,
                MediaType = MediaType.Tv,
                Id = TvShow.Id,
            });

        public void PosterClicked(object sender, TappedRoutedEventArgs e)
        {
            if (e.OriginalSource is Image)
            {
                NavigationService.Navigate<ImageGalleryPage>(new ImageGalleryNavigationParameter
                {
                    ImagePaths = TvShow.Images.Posters.Select(image => image.FilePath),
                    SelectedImagePath = SelectedPoster.FilePath,
                });
            }
        }

        public async void RatingChangedAsync(RatingControl sender, object args) =>
            await ChangeRatingAsync(MediaType.Tv, sender.Value, TvShow.Id);

        public async void FavoriteClickedAsync(object sender, TappedRoutedEventArgs e)
        {
            if (e.OriginalSource is TextBlock)
            {
                await ChangeFavoriteAsync(MediaType.Tv, TvShow.Id);
            }
        }

        public async void WatchlistClickedAsync(object sender, TappedRoutedEventArgs e)
        {
            if (e.OriginalSource is TextBlock)
            {
                await ChangeWatchlistAsync(MediaType.Tv, TvShow.Id);
            }
        }
    }
}
