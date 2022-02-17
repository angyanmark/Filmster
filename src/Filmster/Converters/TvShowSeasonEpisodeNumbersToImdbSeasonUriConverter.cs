using Filmster.Common.Models;
using Filmster.Common.Services;
using System;
using Windows.UI.Xaml.Data;

namespace Filmster.Converters
{
    public class TvShowSeasonEpisodeNumbersToImdbSeasonUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is TvShowSeasonEpisodeNumbers tvShowSeasonEpisodeNumbers && !string.IsNullOrEmpty(tvShowSeasonEpisodeNumbers.TvShowImdbId))
            {
                if (tvShowSeasonEpisodeNumbers.TvSeasonNumber == 0)
                {
                    tvShowSeasonEpisodeNumbers.TvSeasonNumber = -1;
                }
                return new Uri(string.Format(TMDbService.IMDbTvSeasonUrl, tvShowSeasonEpisodeNumbers.TvShowImdbId, tvShowSeasonEpisodeNumbers.TvSeasonNumber));
            }
            else
            {
                return new Uri(TMDbService.IMDbBaseUrl);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
