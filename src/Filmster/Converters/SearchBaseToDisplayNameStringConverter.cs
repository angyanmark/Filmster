using Filmster.Helpers;
using System;
using TMDbLib.Objects.Search;
using Windows.UI.Xaml.Data;

namespace Filmster.Converters
{
    public class SearchBaseToDisplayNameStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is SearchBase searchBase
                ? DisplayNameHelper.GetSearchBaseDisplayName(searchBase)
                : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
