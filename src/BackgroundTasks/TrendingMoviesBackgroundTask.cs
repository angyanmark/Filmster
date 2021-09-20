using Filmster.Core.Services;
using Microsoft.Toolkit.Uwp.Notifications;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.Trending;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace BackgroundTasks
{
    public sealed class TrendingMoviesBackgroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // Get a deferral, to prevent the task from closing prematurely
            // while asynchronous code is still running.
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            await UpdateTileAsync();

            // Inform the system that the task is finished.
            deferral.Complete();
        }

        private async Task UpdateTileAsync()
        {
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Clear();

            var movies = await TMDbService.GetTrendingMoviesAsync(TimeWindow.Week);

            foreach (var movie in movies.Take(5))
            {
                var notification = new TileNotification(GetContent(movie).GetXml());
                updater.Update(notification);
            }
        }

        private TileContent GetContent(SearchMovie movie)
        {
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
                    TileLarge = CreateTile(longTitle, subtitle, posterPath, movie.Title, 3),
                    TileMedium = CreateTile(shortTitle, subtitle, posterPath, movie.Title, 2),
                    TileWide = CreateTile(longTitle, subtitle, backdropPath, movie.Title, 2),
                }
            };
        }

        private TileBinding CreateTile(string title, string subtitle, string backgroundPath, string backgroundAlternateText = "", int? titleHintMaxLines = 2)
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
                        HintOverlay = 60,
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
