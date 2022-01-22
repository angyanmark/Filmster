using Filmster.Common.Helpers;
using System;
using TMDbLib.Objects.General;
using Windows.UI.Xaml.Data;

namespace Filmster.Converters
{
    public class MediaTypeToLocalizedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return $"{default(MediaType).GetType().Name}_{value}".GetLocalized();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
