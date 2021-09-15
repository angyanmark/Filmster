using Filmster.Core.Services;
using Filmster.Helpers;
using System;
using System.Threading.Tasks;
using TMDbLib.Objects.Authentication;
using Windows.Storage;

namespace Filmster.Services
{
    public static class UserSessionService
    {
        private const string SessionIdKey = "SessionIdKey";

        public static event EventHandler LoggedInEvent = delegate { };
        public static event EventHandler LoggedOutEvent = delegate { };

        public static async Task<bool> IsLoggedIn()
        {
            return !string.IsNullOrEmpty(await LoadSessionIdAsync());
        }

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

                LoggedInEvent(null, EventArgs.Empty);
            }
        }

        public static async Task LoggedOutAsync()
        {
            await SaveSessionIdAsync(string.Empty);
            LoggedOutEvent(null, EventArgs.Empty);
        }
    }
}
