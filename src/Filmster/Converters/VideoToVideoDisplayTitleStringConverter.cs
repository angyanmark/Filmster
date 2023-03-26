using Filmster.Common.Helpers;
using System;
using TMDbLib.Objects.General;
using Windows.UI.Xaml.Data;

namespace Filmster.Converters
{
    public class VideoToVideoDisplayTitleStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) =>
            value is Video video
                ? $"{video.Name} ({video.Site}) [{video.Size}{"Video_PixelSize".GetLocalized()}]"
                : string.Empty;

        public object ConvertBack(object value, Type targetType, object parameter, string language) =>
            throw new NotImplementedException();
    }
}
