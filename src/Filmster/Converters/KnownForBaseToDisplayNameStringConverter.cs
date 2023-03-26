using Filmster.Helpers;
using System;
using TMDbLib.Objects.Search;
using Windows.UI.Xaml.Data;

namespace Filmster.Converters
{
    public class KnownForBaseToDisplayNameStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) =>
            value is KnownForBase knownForBase
                ? DisplayNameHelper.GetKnownForBaseDisplayName(knownForBase)
                : string.Empty;

        public object ConvertBack(object value, Type targetType, object parameter, string language) =>
            throw new NotImplementedException();
    }
}
