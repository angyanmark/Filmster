using Filmster.Common.Services;
using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Filmster.Converters
{
    public class ReviewAvatarPathToFullPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var path = value as string;
            var parsed = int.TryParse(parameter.ToString(), out var avatarSize);

            if (string.IsNullOrEmpty(path) || !parsed)
            {
                return default;
            }

            var uriString = path.Contains("www.gravatar.com")
                ? $"{(path[0].Equals('/') ? path.Substring(1) : path)}?s={avatarSize}"
                : $"{TMDbService.SecureBaseUrl}w{avatarSize}{path}";

            return new BitmapImage(new Uri(uriString, UriKind.Absolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
