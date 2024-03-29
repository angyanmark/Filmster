﻿using Filmster.Common.Helper.Tile;
using Filmster.Common.Helpers;
using Filmster.Common.Models;
using Filmster.Common.Services;
using Filmster.Dialogs;
using Filmster.Extensions;
using Filmster.Helpers;
using Filmster.Services;
using Filmster.ViewModelBases;
using Filmster.Views;
using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TMDbLib.Objects.Lists;
using TMDbLib.Objects.Search;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Filmster.ViewModels
{
    public class ProfileViewModel : MediaViewModelBase
    {
        public readonly int AvatarSize = 144;

        public IncrementalLoadingCollection<RatedMoviesSource, SearchMovieWithRating> RatedMovies { get; set; } = new IncrementalLoadingCollection<RatedMoviesSource, SearchMovieWithRating>();
        public IncrementalLoadingCollection<RatedTvShowsSource, AccountSearchTv> RatedTvShows { get; set; } = new IncrementalLoadingCollection<RatedTvShowsSource, AccountSearchTv>();
        public IncrementalLoadingCollection<RatedTvEpisodesSource, AccountSearchTvEpisode> RatedTvEpisodes { get; set; } = new IncrementalLoadingCollection<RatedTvEpisodesSource, AccountSearchTvEpisode>();
        public IncrementalLoadingCollection<MovieWatchlistSource, SearchMovie> MovieWatchlist { get; set; } = new IncrementalLoadingCollection<MovieWatchlistSource, SearchMovie>();
        public IncrementalLoadingCollection<TvShowWatchlistSource, SearchTv> TvShowWatchlist { get; set; } = new IncrementalLoadingCollection<TvShowWatchlistSource, SearchTv>();
        public IncrementalLoadingCollection<FavoriteMoviesSource, SearchMovie> FavoriteMovies { get; set; } = new IncrementalLoadingCollection<FavoriteMoviesSource, SearchMovie>();
        public IncrementalLoadingCollection<FavoriteTvShowsSource, SearchTv> FavoriteTvShows { get; set; } = new IncrementalLoadingCollection<FavoriteTvShowsSource, SearchTv>();
        public IncrementalLoadingCollection<ListsSource, AccountList> Lists { get; set; } = new IncrementalLoadingCollection<ListsSource, AccountList>();
        public ObservableCollection<SearchMovieTvBase> Recommendations { get; set; } = new ObservableCollection<SearchMovieTvBase>();

        private bool _recommendationsLoaded;
        public bool RecommendationsLoaded
        {
            get => _recommendationsLoaded;
            set => Set(ref _recommendationsLoaded, value);
        }

        private int _primaryPivotSelectedIndex;
        public int PrimaryPivotSelectedIndex
        {
            get => _primaryPivotSelectedIndex;
            set
            {
                Set(ref _primaryPivotSelectedIndex, value);
                PrimaryPivotSelectedIndexChanged();
            }
        }

        private int _ratedPivotSelectedIndex;
        public int RatedPivotSelectedIndex
        {
            get => _ratedPivotSelectedIndex;
            set => Set(ref _ratedPivotSelectedIndex, value);
        }

        private int _favoritesPivotSelectedIndex;
        public int FavoritesPivotSelectedIndex
        {
            get => _favoritesPivotSelectedIndex;
            set => Set(ref _favoritesPivotSelectedIndex, value);
        }

        private int _watchlistPivotSelectedIndex;
        public int WatchlistPivotSelectedIndex
        {
            get => _watchlistPivotSelectedIndex;
            set => Set(ref _watchlistPivotSelectedIndex, value);
        }

        private BitmapImage _avatarSource;
        public BitmapImage AvatarSource
        {
            get => _avatarSource;
            set => Set(ref _avatarSource, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        private string _username;
        public string Username
        {
            get => _username;
            set => Set(ref _username, value);
        }

        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set => Set(ref _isLoggedIn, value);
        }

        private bool _isLoggedOut;
        public bool IsLoggedOut
        {
            get => _isLoggedOut;
            set => Set(ref _isLoggedOut, value);
        }

        private bool _isMovieWatchlistPinned;
        public bool IsMovieWatchlistPinned
        {
            get => _isMovieWatchlistPinned;
            set
            {
                if (_isMovieWatchlistPinned != value)
                {
                    Set(ref _isMovieWatchlistPinned, value);
                    MovieWatchlistPinnedChangedAsync();
                }
            }
        }

        private bool _isTvShowWatchlistPinned;
        public bool IsTvShowWatchlistPinned
        {
            get => _isTvShowWatchlistPinned;
            set
            {
                if (_isTvShowWatchlistPinned != value)
                {
                    Set(ref _isTvShowWatchlistPinned, value);
                    TvShowWatchlistPinnedChangedAsync();
                }
            }
        }

        public ICommand LogInClickedCommand;
        public ICommand LogOutClickedCommand;
        public ICommand AccountSearchTvEpisodeClickedCommand;
        public ICommand CreateListClickedCommand;
        public ICommand AccountListClickedCommand;
        public ICommand RecommendMoviesClickedCommand;
        public ICommand RecommendTvShowsClickedCommand;

        public ProfileViewModel()
        {
            LogInClickedCommand = new RelayCommand(LogInClickedAsync);
            LogOutClickedCommand = new RelayCommand(LogOutClickedAsync);
            AccountSearchTvEpisodeClickedCommand = new RelayCommand<AccountSearchTvEpisode>(AccountSearchTvEpisodeClicked);
            CreateListClickedCommand = new RelayCommand(CreateListClickedAsync);
            AccountListClickedCommand = new RelayCommand<AccountList>(AccountListClicked);
            RecommendMoviesClickedCommand = new RelayCommand(RecommendMoviesClickedAsync);
            RecommendTvShowsClickedCommand = new RelayCommand(RecommendTvShowsClickedAsync);

            UserSessionService.LoggedInEvent += OnLoggedIn;
            UserSessionService.LoggedOutEvent += OnLoggedOut;
        }

        public async Task LoadProfileAsync()
        {
            if (UserSessionService.IsLoggedIn)
            {
                await SetLoggedInPropertiesAsync();
            }
            else
            {
                SetLoggedOutProperties();
            }
        }

        private void OnLoggedIn(object sender, EventArgs e) =>
            NavigationService.Reload();

        private void OnLoggedOut(object sender, EventArgs e) =>
            SetLoggedOutProperties();

        private async Task SetLoggedInPropertiesAsync()
        {
            DataLoaded = false;
            IsLoggedOut = false;
            await GetAccountDetailsAsync();
            IsMovieWatchlistPinned = TilePinHelper.IsTilePinned(Constants.MovieWatchlistTileId);
            IsTvShowWatchlistPinned = TilePinHelper.IsTilePinned(Constants.TvShowWatchlistTileId);
            IsLoggedIn = !IsLoggedOut;
            DataLoaded = true;
        }

        private async Task GetAccountDetailsAsync()
        {
            var accountDetails = await TMDbService.GetAccountDetailsAsync();
            if (!string.IsNullOrEmpty(accountDetails.Avatar.Gravatar.Hash))
            {
                AvatarSource = new BitmapImage(new Uri($"{TMDbService.GravatarBaseUrl}{accountDetails.Avatar.Gravatar.Hash}.jpg?s={AvatarSize}", UriKind.Absolute));
            }
            Name = string.IsNullOrEmpty(accountDetails.Name) ? accountDetails.Username : accountDetails.Name;
            Username = accountDetails.Username;
        }

        private void SetLoggedOutProperties()
        {
            DataLoaded = false;
            IsLoggedIn = false;
            AvatarSource = default;
            Name = "Profile_GuestName".GetLocalized();
            Username = "Profile_GuestUsername".GetLocalized();
            RatedMovies.Clear();
            RatedTvShows.Clear();
            RatedTvEpisodes.Clear();
            MovieWatchlist.Clear();
            TvShowWatchlist.Clear();
            FavoriteMovies.Clear();
            FavoriteTvShows.Clear();
            Lists.Clear();
            Recommendations.Clear();
            PrimaryPivotSelectedIndex = default;
            RatedPivotSelectedIndex = default;
            FavoritesPivotSelectedIndex = default;
            WatchlistPivotSelectedIndex = default;
            IsMovieWatchlistPinned = default;
            IsTvShowWatchlistPinned = default;
            IsLoggedOut = !IsLoggedIn;
            DataLoaded = true;
        }

        private async void LogInClickedAsync()
        {
            if (!UserSessionService.IsLoggedIn)
            {
                var token = await TMDbService.GetAuthenticationTokenAsync();
                var logInUri = new Uri($"{TMDbService.TMDbLogInBaseUrl}{token.RequestToken}?redirect_to=filmster.login:");
                await Launcher.LaunchUriAsync(logInUri);
            }
        }

        private async void LogOutClickedAsync()
        {
            var dialog = new ContentDialog
            {
                Title = "ConfirmDialog_LogOut".GetLocalized(),
                Content = "ConfirmDialog_LogOut_Content".GetLocalized(),
                CloseButtonText = "ConfirmDialog_Close".GetLocalized(),
                PrimaryButtonText = "ConfirmDialog_LogOut".GetLocalized(),
                DefaultButton = ContentDialogButton.Primary,
            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                if (UserSessionService.IsLoggedIn)
                {
                    await UserSessionService.LoggedOutAsync();
                }
            }
        }

        private void AccountSearchTvEpisodeClicked(AccountSearchTvEpisode accountSearchTvEpisode) =>
            NavigationService.Navigate<TvEpisodeDetailPage>(new TvShowSeasonEpisodeNumbers
            {
                TvShowId = accountSearchTvEpisode.ShowId,
                TvSeasonNumber = accountSearchTvEpisode.SeasonNumber,
                TvEpisodeNumber = accountSearchTvEpisode.EpisodeNumber,
                TvShowImdbId = string.Empty,
            });

        private async void MovieWatchlistPinnedChangedAsync()
        {
            if (IsMovieWatchlistPinned)
            {
                bool isPinned = await TilePinHelper.PinWatchlistAsync(Constants.MovieWatchlistTileId, "Tile_Watchlist".GetLocalized());
                if (!isPinned)
                {
                    IsMovieWatchlistPinned = isPinned;
                }
            }
            else
            {
                await TilePinHelper.UnpinTileAsync(Constants.MovieWatchlistTileId);
            }
        }

        private async void TvShowWatchlistPinnedChangedAsync()
        {
            if (IsTvShowWatchlistPinned)
            {
                bool isPinned = await TilePinHelper.PinWatchlistAsync(Constants.TvShowWatchlistTileId, "Tile_Watchlist".GetLocalized());
                if (!isPinned)
                {
                    IsTvShowWatchlistPinned = isPinned;
                }
            }
            else
            {
                await TilePinHelper.UnpinTileAsync(Constants.TvShowWatchlistTileId);
            }
        }

        private async void CreateListClickedAsync()
        {
            var dialog = new CreateListContentDialog();
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary && !string.IsNullOrEmpty(dialog.ListId))
            {
                NavigationService.Navigate<ListDetailPage>(int.Parse(dialog.ListId));
            }
        }

        private void AccountListClicked(AccountList accountList) =>
            NavigationService.Navigate<ListDetailPage>(accountList.Id);

        private async void RecommendMoviesClickedAsync()
        {
            RecommendationsLoaded = false;
            var recommendations = await RecommendationService.RecommendMoviesAsync();
            Recommendations.Reset(recommendations);
            RecommendationsLoaded = true;
        }

        private async void RecommendTvShowsClickedAsync()
        {
            RecommendationsLoaded = false;
            var recommendations = await RecommendationService.RecommendTvShowsAsync();
            Recommendations.Reset(recommendations);
            RecommendationsLoaded = true;
        }

        private void PrimaryPivotSelectedIndexChanged()
        {
            if (PrimaryPivotSelectedIndex == 4 &&
                !Recommendations.Any() &&
                RecommendMoviesClickedCommand.CanExecute(default))
            {
                RecommendMoviesClickedCommand.Execute(default);
            }
        }
    }
}
