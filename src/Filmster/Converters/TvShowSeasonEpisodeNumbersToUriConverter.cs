using Filmster.Common.Models;
using Filmster.Common.Services;
using System;
using Windows.UI.Xaml.Data;

namespace Filmster.Converters
{
    public class TvShowSeasonEpisodeNumbersToUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is TvShowSeasonEpisodeNumbers tvShowSeasonEpisodeNumbers
                ? tvShowSeasonEpisodeNumbers.TvEpisodeNumber.HasValue
                    ? new Uri(string.Format(TMDbService.TMDbTvEpisodeUrl, tvShowSeasonEpisodeNumbers.TvShowId, tvShowSeasonEpisodeNumbers.TvSeasonNumber, tvShowSeasonEpisodeNumbers.TvEpisodeNumber))
                    : new Uri(string.Format(TMDbService.TMDbTvSeasonUrl, tvShowSeasonEpisodeNumbers.TvShowId, tvShowSeasonEpisodeNumbers.TvSeasonNumber))
                : new Uri(TMDbService.TMDbBaseUrl);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
