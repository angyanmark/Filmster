using System;
using System.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Filmster.Converters
{
    public class NullOrEmptyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var v = value is ICollection collection
                ? collection.Count > 0
                    ? collection.ToString()
                    : default
                : value?.ToString();

            bool.TryParse(parameter as string, out bool negate);

            if (string.IsNullOrEmpty(v))
            {
                return negate ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                return negate ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
