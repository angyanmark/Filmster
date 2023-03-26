using Filmster.Common.Helpers;
using System;
using TMDbLib.Objects.Discover;
using Windows.UI.Xaml.Data;

namespace Filmster.Converters
{
    public class DiscoverMovieSortByToLocalizedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return $"{nameof(DiscoverMovieSortBy)}_{value}".GetLocalized();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
