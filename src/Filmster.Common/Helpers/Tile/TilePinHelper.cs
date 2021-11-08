using System;
using System.Threading.Tasks;
using Windows.UI.StartScreen;

namespace Filmster.Common.Helper.Tile
{
    public static class TilePinHelper
    {
        public static bool IsTilePinned(string tileId)
        {
            return SecondaryTile.Exists(tileId);
        }

        public static async Task<bool> PinWatchlistAsync(string tileId, string displayName)
        {
            bool isAlreadyPinned = SecondaryTile.Exists(tileId);
            if (isAlreadyPinned)
            {
                return true;
            }

            SecondaryTile tile = new SecondaryTile(
                tileId,
                displayName,
                $"action={tileId}",
                new Uri("ms-appx:///Assets/Square150x150Logo.png"),
                TileSize.Default);

            bool isPinned = await tile.RequestCreateAsync();
            if (isPinned)
            {
                switch (tileId)
                {
                    case Constants.MovieWatchlistTileId:
                        await TileUpdateHelper.UpdateMovieWatchlistTileAsync();
                        break;
                    case Constants.TvShowWatchlistTileId:
                        await TileUpdateHelper.UpdateTvShowWatchlistTileAsync();
                        break;
                    default:
                        throw new ArgumentException(string.Format("Invalid tile ID: {0}.", tileId), nameof(tileId));
                }
            }

            return isPinned;
        }

        public static async Task UnpinTileAsync(string tileId)
        {
            bool isPinned = SecondaryTile.Exists(tileId);
            if (isPinned)
            {
                SecondaryTile tile = new SecondaryTile(tileId);
                await tile.RequestDeleteAsync();
            }
        }

        public static async Task UnpinUserTilesAsync()
        {
            await UnpinTileAsync(Constants.MovieWatchlistTileId);
            await UnpinTileAsync(Constants.TvShowWatchlistTileId);
        }
    }
}
