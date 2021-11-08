using Filmster.Common.Helpers;
using System.Threading.Tasks;
using Windows.Storage;

namespace Filmster.Services
{
    public static class IncludeAdultService
    {
        private const string IncludeAdultKey = "IncludeAdultKey";

        private static readonly bool DefaultIncludeAdult = false;

        public static async Task SaveIncludeAdultAsync(bool includeAdult)
        {
            await ApplicationData.Current.LocalSettings.SaveAsync(IncludeAdultKey, includeAdult);
        }

        public static async Task<bool> LoadIncludeAdultAsync()
        {
            string includeAdult = await ApplicationData.Current.LocalSettings.ReadAsync<string>(IncludeAdultKey);

            return string.IsNullOrEmpty(includeAdult) ? DefaultIncludeAdult : bool.Parse(includeAdult);
        }
    }
}
