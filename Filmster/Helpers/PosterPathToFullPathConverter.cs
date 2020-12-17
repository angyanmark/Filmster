using Filmster.Core.Services;
using System;
using Windows.UI.Xaml.Data;

namespace Filmster.Helpers
{
    public class PosterPathToFullPathConverter : IValueConverter
    {
        private readonly string SecureBaseUrl = TMDbService.SecureBaseUrl;
        private readonly string PosterSize = TMDbService.PosterSize;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var path = value as string;
            return $"{SecureBaseUrl}{PosterSize}{path}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
