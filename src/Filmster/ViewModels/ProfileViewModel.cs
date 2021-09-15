using Filmster.Core.Services;
using Filmster.Helpers;
using Filmster.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Filmster.ViewModels
{
    public class ProfileViewModel : MediaViewModelBase
    {
        public int AvatarSize { get; } = 144;

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
            IsLoggedIn = !IsLoggedOut;
            DataLoaded = true;
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
