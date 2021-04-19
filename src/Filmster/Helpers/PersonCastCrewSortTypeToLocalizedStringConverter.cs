using System;
using Windows.UI.Xaml.Data;

namespace Filmster.Helpers
{
    public class PersonCastCrewSortTypeToLocalizedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return $"PersonCastCrewSortType_{value}".GetLocalized();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
