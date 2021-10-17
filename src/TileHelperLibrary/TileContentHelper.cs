using Filmster.Core.Services;
using Microsoft.Toolkit.Uwp.Notifications;
using TMDbLib.Objects.Search;

namespace TileHelperLibrary
{
    public static class TileContentHelper
    {
        public static TileContent GetContent(SearchMovie movie)
        {
            var backgroundImageOverlay = 60;
            var longTitle = $"{movie.Title} ({movie.ReleaseDate.Value.Year})";
            var shortTitle = movie.Title;
            var subtitle = $"{movie.VoteAverage / 2}★ ({movie.VoteCount})";
            var posterPath = $"{TMDbService.SecureBaseUrl}{TMDbService.MediumPosterSize}{movie.PosterPath}";
            var backdropPath = $"{TMDbService.SecureBaseUrl}{TMDbService.MediumPosterSize}{movie.BackdropPath}";

            return new TileContent()
            {
                Visual = new TileVisual()
                {
                    Branding = TileBranding.Name,
                    TileLarge = CreateTile(backgroundImageOverlay, longTitle, subtitle, posterPath, movie.Title, 3),
                    TileMedium = CreateTile(backgroundImageOverlay, shortTitle, subtitle, posterPath, movie.Title, 2),
                    TileWide = CreateTile(backgroundImageOverlay, longTitle, subtitle, backdropPath, movie.Title, 2),
                    TileSmall = CreateTile(null, string.Empty, string.Empty, posterPath, movie.Title, null),
                }
            };
        }

        private static TileBinding CreateTile(int? backgroindImageOverlay, string title, string subtitle, string backgroundPath, string backgroundAlternateText = "", int? titleHintMaxLines = 2)
        {
            return new TileBinding()
            {
                Content = new TileBindingContentAdaptive()
                {
                    TextStacking = TileTextStacking.Center,
                    BackgroundImage = new TileBackgroundImage()
                    {
                        Source = backgroundPath,
                        AlternateText = backgroundAlternateText,
                        HintCrop = TileBackgroundImageCrop.None,
                        HintOverlay = backgroindImageOverlay,
                    },
                    Children =
                    {
                        new AdaptiveText()
                        {
                            Text = title,
                            HintWrap = true,
                            HintMaxLines = titleHintMaxLines,
                            HintStyle = AdaptiveTextStyle.Base,
                            HintAlign = AdaptiveTextAlign.Center,
                        },
                        new AdaptiveText()
                        {
                            Text = subtitle,
                            HintStyle = AdaptiveTextStyle.CaptionSubtle,
                            HintAlign = AdaptiveTextAlign.Center,
                        },
                    }
                }
            };
        }
    }
}
