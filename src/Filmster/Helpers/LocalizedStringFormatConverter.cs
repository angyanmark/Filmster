using System;
using Windows.UI.Xaml.Data;

namespace Filmster.Helpers
{
    public class LocalizedStringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || parameter == null)
            {
                return null;
            }

            var str = (parameter as string).GetLocalized();

            return string.Format(str, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
