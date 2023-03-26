using Filmster.Common.Helpers;
using Filmster.Common.Models.Enums;
using System;
using Windows.UI.Xaml.Data;

namespace Filmster.Converters
{
    public class PersonCastCrewSortTypeToLocalizedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return $"{nameof(PersonCastCrewSortType)}_{value}".GetLocalized();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
