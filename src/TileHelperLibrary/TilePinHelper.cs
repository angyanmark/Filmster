using System;
using System.Threading.Tasks;
using Windows.UI.StartScreen;

namespace TileHelperLibrary
{
    public static class TilePinHelper
    {
        public static bool IsTilePinned(string tileId)
        {
            return SecondaryTile.Exists(tileId);
        }

        public static async Task<bool> PinWatchlistAsync(string displayName)
        {
            bool isAlreadyPinned = SecondaryTile.Exists(Constants.MovieWatchlistTileId);
            if (isAlreadyPinned)
            {
                return true;
            }

            SecondaryTile tile = new SecondaryTile(
                Constants.MovieWatchlistTileId,
                displayName,
                "action=movie_watchlist",
                new Uri("ms-appx:///Assets/Square150x150Logo.png"),
                TileSize.Default);

            bool isPinned = await tile.RequestCreateAsync();
            if (isPinned)
            {
                await TileUpdateHelper.UpdateWatchlistTileAsync();
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
        }
    }
}
