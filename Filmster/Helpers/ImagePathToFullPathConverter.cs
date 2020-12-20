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
            Enum.TryParse(parameter as string, out ImageSizeType imageType);
            string size;

            switch (imageType)
            {
                case ImageSizeType.SmallBackdrop:
                    size = TMDbService.SmallBackdropSize;
                    break;
                case ImageSizeType.MediumBackdrop:
                    size = TMDbService.MediumBackdropSize;
                    break;
                case ImageSizeType.LargeBackdrop:
                    size = TMDbService.LargeBackdropSize;
                    break;
                case ImageSizeType.SmallLogo:
                    size = TMDbService.SmallLogoSize;
                    break;
                case ImageSizeType.MediumLogo:
                    size = TMDbService.MediumLogoSize;
                    break;
                case ImageSizeType.LargeLogo:
                    size = TMDbService.LargeLogoSize;
                    break;
                case ImageSizeType.SmallPoster:
                    size = TMDbService.SmallPosterSize;
                    break;
                case ImageSizeType.MediumPoster:
                    size = TMDbService.MediumPosterSize;
                    break;
                case ImageSizeType.LargePoster:
                    size = TMDbService.LargePosterSize;
                    break;
                case ImageSizeType.SmallProfile:
                    size = TMDbService.SmallProfileSize;
                    break;
                case ImageSizeType.MediumProfile:
                    size = TMDbService.MediumProfileSize;
                    break;
                case ImageSizeType.LargeProfile:
                    size = TMDbService.LargeProfileSize;
                    break;
                case ImageSizeType.SmallStill:
                    size = TMDbService.SmallStillSize;
                    break;
                case ImageSizeType.MediumStill:
                    size = TMDbService.MediumStillSize;
                    break;
                case ImageSizeType.LargeStill:
                    size = TMDbService.LargeStillSize;
                    break;
                case ImageSizeType.Original:
                    size = TMDbService.OriginalSize;
                    break;
                default:
                    throw new InvalidEnumArgumentException("parameter", (int)imageType, typeof(ImageSizeType));
            }

            return $"{TMDbService.SecureBaseUrl}{size}{path}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
