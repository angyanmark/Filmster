﻿using Filmster.Common.Models;
using Filmster.Common.Models.Extensions;
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
    public class TvSeasonDetailViewModel : RatableMediaViewModelBase
    {
        private TvSeason _tvSeason;
        public TvSeason TvSeason
        {
            get => _tvSeason;
            set => Set(ref _tvSeason, value);
        }

        private ImageData _selectedPoster;
        public ImageData SelectedPoster
        {
            get => _selectedPoster;
            set => Set(ref _selectedPoster, value);
        }

        private TvShowSeasonEpisodeNumbers _tvShowSeasonEpisodeNumbers;
        public TvShowSeasonEpisodeNumbers TvShowSeasonEpisodeNumbers
        {
            get => _tvShowSeasonEpisodeNumbers;
            set => Set(ref _tvShowSeasonEpisodeNumbers, value);
        }

        private Video _video;
        public Video Video
        {
            get => _video;
            set => Set(ref _video, value);
        }

        private double _voteAverage;
        public double VoteAverage
        {
            get => _voteAverage;
            set => Set(ref _voteAverage, value);
        }

        private int _voteCount;
        public int VoteCount
        {
            get => _voteCount;
            set => Set(ref _voteCount, value);
        }

        public ObservableCollection<TvSeasonEpisodeWithRating> Episodes { get; set; } = new ObservableCollection<TvSeasonEpisodeWithRating>();
        public ObservableCollection<Cast> Cast { get; set; } = new ObservableCollection<Cast>();
        public ObservableCollection<Crew> Crew { get; set; } = new ObservableCollection<Crew>();

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

        public ICommand TvSeasonEpisodeClickedCommand;

        public TvSeasonDetailViewModel() =>
            TvSeasonEpisodeClickedCommand = new RelayCommand<TvSeasonEpisode>(TvSeasonEpisodeClicked);

        public async Task LoadTvSeasonAsync(TvShowSeasonEpisodeNumbers tvShowSeasonEpisodeNumbers)
        {
            TvSeason = await TMDbService.GetTvSeasonAsync(tvShowSeasonEpisodeNumbers.TvShowId, tvShowSeasonEpisodeNumbers.TvSeasonNumber, IsLoggedIn);
            if (TvSeason == null)
            {
                NavigationService.GoBack();
                return;
            }
            TvShowSeasonEpisodeNumbers = tvShowSeasonEpisodeNumbers;
            SetEpisodes();
            SelectedPoster = GetSelectedPoster();
            (VoteAverage, VoteCount) = VoteHelper.GetVoteAverageVoteCount(TvSeason.Episodes.Select(episode => (episode.VoteAverage, episode.VoteCount)));
            Video = VideoSelectService.SelectVideo(TvSeason.Videos.Results);
            Cast.AddRange(TvSeason.Credits.Cast.Take(TMDbService.DefaultCastCrewBackdropCount));
            Crew.AddRange(TvSeason.Credits.Crew.Take(TMDbService.DefaultCastCrewBackdropCount));
        }

        private void SetEpisodes() =>
            Episodes.AddRange(TvSeason.Episodes.Select(episode => new TvSeasonEpisodeWithRating
            {
                Rating = TvSeason.AccountStates.Results.Single(accountState => accountState.EpisodeNumber == episode.EpisodeNumber).Rating,
                AirDate = episode.AirDate,
                Crew = episode.Crew,
                EpisodeNumber = episode.EpisodeNumber,
                GuestStars = episode.GuestStars,
                Id = episode.Id,
                Name = episode.Name,
                Overview = episode.Overview,
                ProductionCode = episode.ProductionCode,
                Runtime = episode.Runtime,
                SeasonNumber = episode.SeasonNumber,
                StillPath = episode.StillPath,
                VoteAverage = episode.VoteAverage,
                VoteCount = episode.VoteCount,
            }));

        private ImageData GetSelectedPoster() =>
            TvSeason.Images.Posters.Find(poster => poster.FilePath == TvSeason.PosterPath) ?? TvSeason.Images.Posters.FirstOrDefault();

        private void TvSeasonEpisodeClicked(TvSeasonEpisode tvSeasonEpisode) =>
            NavigationService.Navigate<TvEpisodeDetailPage>(new TvShowSeasonEpisodeNumbers
            {
                TvShowId = TvShowSeasonEpisodeNumbers.TvShowId,
                TvSeasonNumber = tvSeasonEpisode.SeasonNumber,
                TvEpisodeNumber = tvSeasonEpisode.EpisodeNumber,
                TvShowImdbId = TvShowSeasonEpisodeNumbers.TvShowImdbId,
            });

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
