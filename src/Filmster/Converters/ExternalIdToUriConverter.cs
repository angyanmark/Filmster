using Filmster.Common.Models.Enums;
using Filmster.Common.Services;
using System;
using System.ComponentModel;
using Windows.UI.Xaml.Data;

namespace Filmster.Converters
{
    public class ExternalIdToUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var id = value;
            Enum.TryParse(parameter as string, out ExternalIdType externalIdType);
            string baseUrl;

            switch (externalIdType)
            {
                case ExternalIdType.Homepage:
                    var homepage = id as string;
                    if (string.IsNullOrEmpty(homepage))
                    {
                        return new Uri(TMDbService.TMDbBaseUrl);
                    }
                    else
                    {
                        return new Uri(homepage);
                    }
                case ExternalIdType.TMDbMovie:
                    baseUrl = TMDbService.TMDbMovieBaseUrl;
                    break;
                case ExternalIdType.TMDbTvShow:
                    baseUrl = TMDbService.TMDbTvShowBaseUrl;
                    break;
                case ExternalIdType.TMDbPerson:
                    baseUrl = TMDbService.TMDbPersonBaseUrl;
                    break;
                case ExternalIdType.IMDbMovie:
                    baseUrl = TMDbService.IMDbMovieBaseUrl;
                    break;
                case ExternalIdType.IMDbTvShow:
                    baseUrl = TMDbService.IMDbTvShowBaseUrl;
                    break;
                case ExternalIdType.IMDbPerson:
                    baseUrl = TMDbService.IMDbPersonBaseUrl;
                    break;
                case ExternalIdType.YouTube:
                    baseUrl = TMDbService.YouTubeBaseUrl;
                    break;
                case ExternalIdType.Facebook:
                    baseUrl = TMDbService.FacebookBaseUrl;
                    break;
                case ExternalIdType.Twitter:
                    baseUrl = TMDbService.TwitterBaseUrl;
                    break;
                case ExternalIdType.Instagram:
                    baseUrl = TMDbService.InstagramBaseUrl;
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(parameter), (int)externalIdType, typeof(ExternalIdType));
            }

            return new Uri($"{baseUrl}{id}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
