using Filmster.Common.Helper.Tile;
using Filmster.Common.Helpers;
using System;
using System.Threading.Tasks;
using TMDbLib.Objects.Authentication;
using Windows.Storage;

namespace Filmster.Common.Services
{
    public static class UserSessionService
    {
        private const string SessionIdKey = "SessionIdKey";

        public static bool IsLoggedIn { get; private set; }

        public static event EventHandler LoggedInEvent = delegate { };
        public static event EventHandler LoggedOutEvent = delegate { };

        private static async Task SaveSessionIdAsync(string sessionId)
        {
            await ApplicationData.Current.LocalSettings.SaveAsync(SessionIdKey, sessionId);
        }

        private static async Task<string> LoadSessionIdAsync()
        {
            return await ApplicationData.Current.LocalSettings.ReadAsync<string>(SessionIdKey);
        }

        public static async Task StartUserSessionAsync()
        {
            var sessionId = await LoadSessionIdAsync();
            if (!string.IsNullOrEmpty(sessionId))
            {
                IsLoggedIn = true;
                await TMDbService.SetSessionInformationAsync(sessionId, SessionType.UserSession);
            }
        }

        public static async Task LoggedInAsync(string requestToken)
        {
            var userSession = await TMDbService.GetUserSessionAsync(requestToken);
            if (userSession.Success)
            {
                await TMDbService.SetSessionInformationAsync(userSession.SessionId, SessionType.UserSession);
                await SaveSessionIdAsync(userSession.SessionId);
                IsLoggedIn = true;
                LoggedInEvent(null, EventArgs.Empty);
            }
        }

        public static async Task LoggedOutAsync()
        {
            var success = await TMDbService.DeleteUserSessionAsync();
            if (success)
            {
                await SaveSessionIdAsync(string.Empty);
                IsLoggedIn = false;
                LoggedOutEvent(null, EventArgs.Empty);
                await TilePinHelper.UnpinUserTilesAsync();
            }
        }
    }
}
