using Filmster.Common.Services;
using Microsoft.Toolkit.Uwp.Notifications;
using System;

namespace Filmster.Common.Helper.Tile
{
    public static class TileContentHelper
    {
        public static TileContent GetContent(string title, DateTime? releaseDate, double voteAverage, int voteCount, string posterPathKey, string backdropPathKey)
        {
            var backgroundImageOverlay = 60;
            var longTitle = $"{title} ({releaseDate.Value.Year})";
            var shortTitle = title;
            var subtitle = $"{voteAverage / 2}★ ({voteCount})";
            var posterPath = $"{TMDbService.SecureBaseUrl}{TMDbService.MediumPosterSize}{posterPathKey}";
            var backdropPath = $"{TMDbService.SecureBaseUrl}{TMDbService.MediumPosterSize}{backdropPathKey}";

            return new TileContent()
            {
                Visual = new TileVisual()
                {
                    Branding = TileBranding.Name,
                    TileLarge = CreateTile(backgroundImageOverlay, longTitle, subtitle, posterPath, title, 3),
                    TileMedium = CreateTile(backgroundImageOverlay, shortTitle, subtitle, posterPath, title, 2),
                    TileWide = CreateTile(backgroundImageOverlay, longTitle, subtitle, backdropPath, title, 2),
                    TileSmall = CreateTile(null, string.Empty, string.Empty, posterPath, title, null),
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
