using Filmster.Common.Helper.Tile;
using Filmster.Common.Helpers;
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
using System.Threading.Tasks;
using System.Windows.Input;
using TMDbLib.Objects.Lists;
using TMDbLib.Objects.Search;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace Filmster.ViewModels
{
    public class ProfileViewModel : MediaViewModelBase
    {
        public int AvatarSize { get; } = 144;

        public IncrementalLoadingCollection<RatedMoviesSource, SearchMovieWithRating> RatedMovies { get; set; } = new IncrementalLoadingCollection<RatedMoviesSource, SearchMovieWithRating>();
        public IncrementalLoadingCollection<RatedTvShowsSource, AccountSearchTv> RatedTvShows { get; set; } = new IncrementalLoadingCollection<RatedTvShowsSource, AccountSearchTv>();
        public IncrementalLoadingCollection<MovieWatchlistSource, SearchMovie> MovieWatchlist { get; set; } = new IncrementalLoadingCollection<MovieWatchlistSource, SearchMovie>();
        public IncrementalLoadingCollection<TvShowWatchlistSource, SearchTv> TvShowWatchlist { get; set; } = new IncrementalLoadingCollection<TvShowWatchlistSource, SearchTv>();
        public IncrementalLoadingCollection<FavoriteMoviesSource, SearchMovie> FavoriteMovies { get; set; } = new IncrementalLoadingCollection<FavoriteMoviesSource, SearchMovie>();
        public IncrementalLoadingCollection<FavoriteTvShowsSource, SearchTv> FavoriteTvShows { get; set; } = new IncrementalLoadingCollection<FavoriteTvShowsSource, SearchTv>();
        public IncrementalLoadingCollection<ListsSource, AccountList> Lists { get; set; } = new IncrementalLoadingCollection<ListsSource, AccountList>();
        public ObservableCollection<SearchMovie> Recommendations { get; set; } = new ObservableCollection<SearchMovie>();

        private int _primaryPivotSelectedIndex;
        public int PrimaryPivotSelectedIndex
        {
            get { return _primaryPivotSelectedIndex; }
            set { Set(ref _primaryPivotSelectedIndex, value); }
        }

        private int _watchlistPivotSelectedIndex;
        public int WatchlistPivotSelectedIndex
        {
            get { return _watchlistPivotSelectedIndex; }
            set { Set(ref _watchlistPivotSelectedIndex, value); }
        }

        private string _avatarSource = TMDbService.GravatarBaseUrl;
        public string AvatarSource
        {
            get { return _avatarSource; }
            set { Set(ref _avatarSource, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set { Set(ref _username, value); }
        }

        private bool _hasAvatar;
        public bool HasAvatar
        {
            get { return _hasAvatar; }
            set { Set(ref _hasAvatar, value); }
        }

        private bool _hasNoAvatar;
        public bool HasNoAvatar
        {
            get { return _hasNoAvatar; }
            set { Set(ref _hasNoAvatar, value); }
        }

        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { Set(ref _isLoggedIn, value); }
        }

        private bool _isLoggedOut;
        public bool IsLoggedOut
        {
            get { return _isLoggedOut; }
            set { Set(ref _isLoggedOut, value); }
        }

        private bool _isMovieWatchlistPinned;
        public bool IsMovieWatchlistPinned
        {
            get { return _isMovieWatchlistPinned; }
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
            get { return _isTvShowWatchlistPinned; }
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
        public ICommand CreateListClickedCommand;
        public ICommand AccountListClickedCommand;
        public ICommand RecommendClickedCommand;

        public ProfileViewModel()
        {
            LogInClickedCommand = new RelayCommand(LogInClickedAsync);
            LogOutClickedCommand = new RelayCommand(LogOutClickedAsync);
            CreateListClickedCommand = new RelayCommand(CreateListClickedAsync);
            AccountListClickedCommand = new RelayCommand<AccountList>(AccountListClicked);
            RecommendClickedCommand = new RelayCommand(RecommendClickedAsync);

            UserSessionService.LoggedInEvent += OnLoggedInAsync;
            UserSessionService.LoggedOutEvent += OnLoggedOut;
        }

        public async Task LoadProfile()
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

        private async void OnLoggedInAsync(object sender, EventArgs e)
        {
            await SetLoggedInPropertiesAsync();
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            SetLoggedOutProperties();
        }

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
            if (string.IsNullOrEmpty(accountDetails.Avatar.Gravatar.Hash))
            {
                HasAvatar = false;
                HasNoAvatar = !HasAvatar;
            }
            else
            {
                AvatarSource = $"{TMDbService.GravatarBaseUrl}{accountDetails.Avatar.Gravatar.Hash}.jpg?s={AvatarSize}";
                HasNoAvatar = false;
                HasAvatar = !HasNoAvatar;
            }
            Name = string.IsNullOrEmpty(accountDetails.Name) ? accountDetails.Username : accountDetails.Name;
            Username = accountDetails.Username;
        }

        private void SetLoggedOutProperties()
        {
            DataLoaded = false;
            IsLoggedIn = false;
            AvatarSource = TMDbService.GravatarBaseUrl;
            HasNoAvatar = true;
            HasAvatar = !HasNoAvatar;
            Name = "Profile_GuestName".GetLocalized();
            Username = "Profile_GuestUsername".GetLocalized();
            RatedMovies.Clear();
            RatedTvShows.Clear();
            MovieWatchlist.Clear();
            TvShowWatchlist.Clear();
            FavoriteMovies.Clear();
            FavoriteTvShows.Clear();
            Lists.Clear();
            Recommendations.Clear();
            PrimaryPivotSelectedIndex = default;
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
                var token = await TMDbService.GetAutenticationTokenAsync();
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

        private void AccountListClicked(AccountList accountList)
        {
            NavigationService.Navigate<ListDetailPage>(accountList.Id);
        }

        private async void RecommendClickedAsync()
        {
            var recommendations = await RecommendationService.RecommendMoviesAsync();
            Recommendations.Reset(recommendations);
        }
    }
}
