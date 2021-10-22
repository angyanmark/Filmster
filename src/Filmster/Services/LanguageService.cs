using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Globalization;

namespace Filmster.Services
{
    public static class LanguageService
    {
        private const string DefaultLanguage = "en-US";

        public const string LanguageLocalizationPrefix = "AppLanguage_";

        public static IReadOnlyList<string> AvailableLanguages => ApplicationLanguages.ManifestLanguages;

        public static string CurrentLanguage
        {
            get
            {
                if (string.IsNullOrEmpty(ApplicationLanguages.PrimaryLanguageOverride))
                {
                    ApplicationLanguages.PrimaryLanguageOverride = DefaultLanguage;
                }
                return ApplicationLanguages.PrimaryLanguageOverride;
            }
        }

        public static async Task SaveLanguageAsync(string languageCode)
        {
            ApplicationLanguages.PrimaryLanguageOverride = languageCode;
            await CoreApplication.RequestRestartAsync(string.Empty);
        }
    }
}
