using Filmster.Core.Services;
using Filmster.Helpers;
using Filmster.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TMDbLib.Objects.Search;

namespace Filmster.ViewModels
{
    public class ProfileViewModel : MediaViewModelBase
    {
        public int AvatarSize { get; } = 144;

        public ObservableCollection<SearchMovie> MovieWatchlist { get; set; } = new ObservableCollection<SearchMovie>();
        public ObservableCollection<SearchTv> TvShowWatchlist { get; set; } = new ObservableCollection<SearchTv>();
        public ObservableCollection<SearchMovie> FavoriteMovies { get; set; } = new ObservableCollection<SearchMovie>();
        public ObservableCollection<SearchTv> FavoriteTvShows { get; set; } = new ObservableCollection<SearchTv>();
        public ObservableCollection<SearchMovieWithRating> RatedMovies { get; set; } = new ObservableCollection<SearchMovieWithRating>();
        public ObservableCollection<AccountSearchTv> RatedTvShows { get; set; } = new ObservableCollection<AccountSearchTv>();

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

        public ICommand LogInClickedCommand;

        public ICommand LogOutClickedCommand;

        public ProfileViewModel()
        {
            LogInClickedCommand = new RelayCommand(LogInClickedAsync);
            LogOutClickedCommand = new RelayCommand(LogOutClickedAsync);

            UserSessionService.LoggedInEvent += OnLoggedInAsync;
            UserSessionService.LoggedOutEvent += OnLoggedOut;
        }

        public async Task LoadProfile()
        {
            var isLoggenIn = await UserSessionService.IsLoggedIn();

            if (isLoggenIn)
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
            await GetDetailsAsync();
            IsLoggedIn = !IsLoggedOut;
            DataLoaded = true;
        }

        private async Task GetDetailsAsync()
        {
            await GetMovieWatchlistAsync();
            await GetTvShowWatchlistAsync();
            await GetFavoriteMoviesAsync();
            await GetFavoriteTvShowsAsync();
            await GetRatedMoviesAsync();
            await GetRatedTvShowsAsync();
        }

        private async Task GetMovieWatchlistAsync()
        {
            var movies = await TMDbService.GetMovieWatchlistAsync();
            foreach (var movie in movies)
            {
                MovieWatchlist.Add(movie);
            }
        }

        private async Task GetTvShowWatchlistAsync()
        {
            var tvShows = await TMDbService.GetTvShowWatchlistAsync();
            foreach (var tvShow in tvShows)
            {
                TvShowWatchlist.Add(tvShow);
            }
        }

        private async Task GetFavoriteMoviesAsync()
        {
            var movies = await TMDbService.GetFavoriteMoviesAsync();
            foreach (var movie in movies)
            {
                FavoriteMovies.Add(movie);
            }
        }

        private async Task GetFavoriteTvShowsAsync()
        {
            var tvShows = await TMDbService.GetFavoriteTvShowsAsync();
            foreach (var tvShow in tvShows)
            {
                FavoriteTvShows.Add(tvShow);
            }
        }

        private async Task GetRatedMoviesAsync()
        {
            var movies = await TMDbService.GetRatedMoviesAsync();
            foreach (var movie in movies)
            {
                RatedMovies.Add(movie);
            }
        }

        private async Task GetRatedTvShowsAsync()
        {
            var tvShows = await TMDbService.GetRatedTvShowsAsync();
            foreach (var tvShow in tvShows)
            {
                RatedTvShows.Add(tvShow);
            }
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
            MovieWatchlist.Clear();
            TvShowWatchlist.Clear();
            FavoriteMovies.Clear();
            FavoriteTvShows.Clear();
            RatedMovies.Clear();
            RatedTvShows.Clear();
            IsLoggedOut = !IsLoggedIn;
            DataLoaded = true;
        }

        private async void LogInClickedAsync()
        {
            var isLoggenIn = await UserSessionService.IsLoggedIn();

            if (!isLoggenIn)
            {
                var token = await TMDbService.GetAutenticationTokenAsync();
                var logInUri = new Uri(TMDbService.TMDbLogInBaseUrl + token.RequestToken + "?redirect_to=filmster.login:");
                await Windows.System.Launcher.LaunchUriAsync(logInUri);
            }
        }

        private async void LogOutClickedAsync()
        {
            var isLoggenIn = await UserSessionService.IsLoggedIn();

            if (isLoggenIn)
            {
                var success = await TMDbService.DeleteUserSessionAsync();

                if (success)
                {
                    await UserSessionService.LoggedOutAsync();
                }
            }
        }
    }
}
