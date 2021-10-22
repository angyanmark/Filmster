using Filmster.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.Account;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;

namespace TileHelperLibrary
{
    public static class TileUpdateHelper
    {
        public static async Task UpdatePrimaryTileAsync()
        {
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            var movies = await TMDbService.GetPopularMoviesAsync();
            UpdateTile(updater, movies);
        }

        public static async Task UpdateSecondaryTilesAsync()
        {
            var tiles = await SecondaryTile.FindAllAsync();

            foreach (var tile in tiles)
            {
                switch (tile.TileId)
                {
                    case Constants.MovieWatchlistTileId:
                        await UpdateWatchlistTileAsync();
                        break;
                    default:
                        break;
                }
            }
        }

        public static async Task UpdateWatchlistTileAsync()
        {
            var updater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(Constants.MovieWatchlistTileId);
            var movies = await TMDbService.GetMovieWatchlistAsync(AccountSortBy.CreatedAt, SortOrder.Descending);
            UpdateTile(updater, movies);
        }

        private static void UpdateTile(TileUpdater updater, List<SearchMovie> movies)
        {
            updater.EnableNotificationQueue(true);
            updater.Clear();

            foreach (var movie in movies.Take(5).Reverse())
            {
                var notification = new TileNotification(TileContentHelper.GetContent(movie).GetXml());
                updater.Update(notification);
            }
        }
    }
}
