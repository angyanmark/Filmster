using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Filmster.Helpers
{
    public class NullOrEmptyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var v = value?.ToString() ?? string.Empty;
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
