using Filmster.Core.Models.Enums;
using Filmster.Core.Services;
using System;
using System.ComponentModel;
using Windows.UI.Xaml.Data;

namespace Filmster.Helpers
{
    public class ImagePathToFullPathConverter : IValueConverter
    {
        private readonly string SecureBaseUrl = TMDbService.SecureBaseUrl;
        private readonly string BackdropSize = TMDbService.BackdropSize;
        private readonly string LogoSize = TMDbService.LogoSize;
        private readonly string PosterSize = TMDbService.PosterSize;
        private readonly string ProfileSize = TMDbService.ProfileSize;
        private readonly string StillSize = TMDbService.StillSize;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var path = value as string;
            Enum.TryParse(parameter as string, out ImageType imageType);
            string size;

            switch (imageType)
            {
                case ImageType.Backdrop:
                    size = BackdropSize;
                    break;
                case ImageType.Logo:
                    size = LogoSize;
                    break;
                case ImageType.Poster:
                    size = PosterSize;
                    break;
                case ImageType.Profile:
                    size = ProfileSize;
                    break;
                case ImageType.Still:
                    size = StillSize;
                    break;
                default:
                    throw new InvalidEnumArgumentException("parameter", (int)imageType, typeof(ImageType));
            }

            return $"{SecureBaseUrl}{size}{path}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
