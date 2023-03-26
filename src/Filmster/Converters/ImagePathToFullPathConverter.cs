using Filmster.Common.Models.Enums;
using Filmster.Common.Services;
using System;
using System.ComponentModel;
using Windows.UI.Xaml.Data;

namespace Filmster.Converters
{
    public class ImagePathToFullPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
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
                case ImageSizeType.XLargeStill:
                    size = TMDbService.XLargeStillSize;
                    break;
                case ImageSizeType.XXLargeStill:
                    size = TMDbService.XXLargeStillSize;
                    break;
                case ImageSizeType.Original:
                    size = TMDbService.OriginalSize;
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(parameter), (int)imageType, typeof(ImageSizeType));
            }

            var path = value as string;

            return string.IsNullOrEmpty(path)
                ? GetPlaceholder(imageType)
                : $"{TMDbService.SecureBaseUrl}{size}{path}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) =>
            throw new NotImplementedException();

        private string GetPlaceholder(ImageSizeType imageType)
        {
            switch (imageType)
            {
                case ImageSizeType.SmallBackdrop:
                case ImageSizeType.SmallLogo:
                case ImageSizeType.SmallPoster:
                case ImageSizeType.SmallProfile:
                case ImageSizeType.SmallStill:
                    return @"..\Assets\Placeholders\placeholder-poster-small.png";
                case ImageSizeType.MediumBackdrop:
                case ImageSizeType.MediumLogo:
                case ImageSizeType.MediumPoster:
                case ImageSizeType.MediumProfile:
                case ImageSizeType.MediumStill:
                    return @"..\Assets\Placeholders\placeholder-poster-medium.png";
                case ImageSizeType.LargeBackdrop:
                case ImageSizeType.LargeLogo:
                case ImageSizeType.LargePoster:
                case ImageSizeType.LargeProfile:
                case ImageSizeType.LargeStill:
                case ImageSizeType.Original:
                    return @"..\Assets\Placeholders\placeholder-poster-large.png";
                default:
                    return string.Empty;
            }
        }
    }
}
