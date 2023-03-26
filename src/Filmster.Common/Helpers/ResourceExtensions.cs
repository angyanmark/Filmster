using System.Linq;
using Windows.ApplicationModel.Resources;

namespace Filmster.Common.Helpers
{
    public static class ResourceExtensions
    {
        private static readonly ResourceLoader _resLoader = new ResourceLoader();

        public static string GetLocalized(this string resourceKey, params object[] args)
        {
            var localized = _resLoader.GetString(resourceKey);
            return args.Any()
                ? string.Format(localized, args)
                : localized;
        }
    }
}
