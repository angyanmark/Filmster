using System;
using TMDbLib.Objects.Search;
using Windows.UI.Xaml.Data;

namespace Filmster.Converters
{
    public class SearchMovieTvBaseToTitleReleaseDateStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is SearchMovie movie)
            {
                return movie.ReleaseDate.HasValue
                    ? $"{movie.Title} ({movie.ReleaseDate.Value.Year})"
                    : movie.Title;
            }
            else if (value is SearchTv tv)
            {
                return tv.FirstAirDate.HasValue
                    ? $"{tv.Name} ({tv.FirstAirDate.Value.Year})"
                    : tv.Name;
            }
            else
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
