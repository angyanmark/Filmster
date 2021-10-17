using Filmster.Core.Services;
using System.Threading.Tasks;
using TileHelperLibrary;
using TMDbLib.Objects.General;

namespace Filmster.Helpers
{
    public class RatableMediaViewModelBase : MediaViewModelBase
    {
        private double _rating;
        public double Rating
        {
            get { return _rating; }
            set { Set(ref _rating, value); }
        }

        private bool _isFavorite;
        public bool IsFavorite
        {
            get { return _isFavorite; }
            set { Set(ref _isFavorite, value); }
        }

        private bool _isNotFavorite;
        public bool IsNotFavorite
        {
            get { return _isNotFavorite; }
            set { Set(ref _isNotFavorite, value); }
        }

        private bool _isWatchlist;
        public bool IsWatchlist
        {
            get { return _isWatchlist; }
            set { Set(ref _isWatchlist, value); }
        }

        private bool _isNotWatchlist;
        public bool IsNotWatchlist
        {
            get { return _isNotWatchlist; }
            set { Set(ref _isNotWatchlist, value); }
        }

        private protected void SetAccountState(AccountState accountState)
        {
            Rating = accountState.Rating ?? -1;
            IsFavorite = accountState.Favorite;
            IsNotFavorite = !IsFavorite;
            IsWatchlist = accountState.Watchlist;
            IsNotWatchlist = !IsWatchlist;
        }

        private protected async Task ChangeRatingAsync(MediaType mediaType, int id, double value)
        {
            bool success = await TMDbService.SetRatingAsync(mediaType, id, value);

            if (success && value != -1 && IsWatchlist)
            {
                IsWatchlist = !IsWatchlist;
                IsNotWatchlist = !IsWatchlist;
            }
        }

        private protected async Task ChangeFavoriteAsync(MediaType mediaType, int id)
        {
            var success = await TMDbService.ChangeFavoriteStatusAsync(mediaType, id, !IsFavorite);

            if (success)
            {
                IsFavorite = !IsFavorite;
                IsNotFavorite = !IsFavorite;
            }
        }

        private protected async Task ChangeWatchlistAsync(MediaType mediaType, int id)
        {
            var success = await TMDbService.ChangeWatchlistStatusAsync(mediaType, id, !IsWatchlist);

            if (success)
            {
                IsWatchlist = !IsWatchlist;
                IsNotWatchlist = !IsWatchlist;

                await TileUpdateHelper.UpdateWatchlistTileAsync();
            }
        }
    }
}
