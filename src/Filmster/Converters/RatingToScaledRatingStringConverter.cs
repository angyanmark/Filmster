using System;
using Windows.UI.Xaml.Data;

namespace Filmster.Converters
{
    public class RatingToScaledRatingStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ((double)new RatingToScaledRatingConverter().Convert(value, targetType, parameter, language)).ToString("0.##");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return new RatingToScaledRatingConverter().ConvertBack(value, targetType, parameter, language);
        }
    }
}
