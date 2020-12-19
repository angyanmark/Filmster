using Filmster.Core.Models.Enums;
using Filmster.Core.Services;
using System;
using System.ComponentModel;
using Windows.UI.Xaml.Data;

namespace Filmster.Helpers
{
    public class ImagePathToFullPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var path = value as string;
            Enum.TryParse(parameter as string, out ImageType imageType);
            string size;

            switch (imageType)
            {
                case ImageType.Backdrop:
                    size = TMDbService.BackdropSize;
                    break;
                case ImageType.Logo:
                    size = TMDbService.LogoSize;
                    break;
                case ImageType.Poster:
                    size = TMDbService.PosterSize;
                    break;
                case ImageType.Profile:
                    size = TMDbService.ProfileSize;
                    break;
                case ImageType.Still:
                    size = TMDbService.StillSize;
                    break;
                default:
                    throw new InvalidEnumArgumentException("parameter", (int)imageType, typeof(ImageType));
            }

            return $"{TMDbService.SecureBaseUrl}{size}{path}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
