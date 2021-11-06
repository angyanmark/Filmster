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
            UpdateMovieTile(updater, movies);
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
            UpdateMovieTile(updater, movies);
        }

        public static async Task UpdateTvShowWatchlistTileAsync()
        {
            var updater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(Constants.TvShowWatchlistTileId);
            var tvShows = await TMDbService.GetTvShowWatchlistAsync(AccountSortBy.CreatedAt, SortOrder.Descending);
            UpdateTvShowTile(updater, tvShows);
        }

        private static void UpdateMovieTile(TileUpdater updater, List<SearchMovie> movies)
        {
            updater.EnableNotificationQueue(true);
            updater.Clear();

            foreach (var movie in movies.Take(5).Reverse())
            {
                var notification = new TileNotification(TileContentHelper.GetContent(movie.Title, movie.ReleaseDate, movie.VoteAverage, movie.VoteCount, movie.PosterPath, movie.BackdropPath).GetXml());
                updater.Update(notification);
            }
        }

        private static void UpdateTvShowTile(TileUpdater updater, List<SearchTv> tvShows)
        {
            updater.EnableNotificationQueue(true);
            updater.Clear();

            foreach (var tvShow in tvShows.Take(5).Reverse())
            {
                var notification = new TileNotification(TileContentHelper.GetContent(tvShow.Name, tvShow.FirstAirDate, tvShow.VoteAverage, tvShow.VoteCount, tvShow.PosterPath, tvShow.BackdropPath).GetXml());
                updater.Update(notification);
            }
        }
    }
}
