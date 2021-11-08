using Filmster.Common.Helpers;
using Filmster.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.Account;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;

namespace Filmster.Common.Helper.Tile
{
    public static class TileUpdateHelper
    {
        public static async Task UpdatePrimaryTileAsync()
        {
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            var movies = await TMDbService.GetPopularMoviesAsync();
            await UpdateMovieTileAsync(updater, movies);
        }

        public static async Task UpdateSecondaryTilesAsync()
        {
            var tiles = await SecondaryTile.FindAllAsync();

            foreach (var tile in tiles)
            {
                switch (tile.TileId)
                {
                    case Constants.MovieWatchlistTileId:
                        await UpdateMovieWatchlistTileAsync();
                        break;
                    case Constants.TvShowWatchlistTileId:
                        await UpdateTvShowWatchlistTileAsync();
                        break;
                    default:
                        break;
                }
            }
        }

        public static async Task UpdateMovieWatchlistTileAsync()
        {
            var updater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(Constants.MovieWatchlistTileId);
            var movies = await TMDbService.GetMovieWatchlistAsync(AccountSortBy.CreatedAt, SortOrder.Descending);
            await UpdateMovieTileAsync(updater, movies);
        }

        public static async Task UpdateTvShowWatchlistTileAsync()
        {
            var updater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(Constants.TvShowWatchlistTileId);
            var tvShows = await TMDbService.GetTvShowWatchlistAsync(AccountSortBy.CreatedAt, SortOrder.Descending);
            await UpdateTvShowTileAsync(updater, tvShows);
        }

        private static async Task UpdateMovieTileAsync(TileUpdater updater, List<SearchMovie> movies)
        {
            updater.EnableNotificationQueue(true);
            updater.Clear();

            foreach (var movie in movies.Take(5).Reverse())
            {
                var notification = new TileNotification(TileContentHelper.GetContent(movie.Title, movie.ReleaseDate, movie.VoteAverage, movie.VoteCount, movie.PosterPath, movie.BackdropPath).GetXml());
                updater.Update(notification);
            }

            SecondaryTile tile = new SecondaryTile(Constants.MovieWatchlistTileId)
            {
                DisplayName = "Tile_Watchlist".GetLocalized(),
            };
            await tile.UpdateAsync();
        }

        private static async Task UpdateTvShowTileAsync(TileUpdater updater, List<SearchTv> tvShows)
        {
            updater.EnableNotificationQueue(true);
            updater.Clear();

            foreach (var tvShow in tvShows.Take(5).Reverse())
            {
                var notification = new TileNotification(TileContentHelper.GetContent(tvShow.Name, tvShow.FirstAirDate, tvShow.VoteAverage, tvShow.VoteCount, tvShow.PosterPath, tvShow.BackdropPath).GetXml());
                updater.Update(notification);
            }

            SecondaryTile tile = new SecondaryTile(Constants.TvShowWatchlistTileId)
            {
                DisplayName = "Tile_Watchlist".GetLocalized(),
            };
            await tile.UpdateAsync();
        }
    }
}
