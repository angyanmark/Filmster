using Filmster.Core.Services;
using Filmster.Services;
using System.ComponentModel;
using System.Threading.Tasks;
using TileHelperLibrary;
using TMDbLib.Objects.General;

namespace Filmster.Helpers
{
    public class RatableMediaViewModelBase : MediaViewModelBase
    {
        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { Set(ref _isLoggedIn, value); }
        }

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

        public RatableMediaViewModelBase()
        {
            _ = SetLoggedIn();
        }

        private async Task SetLoggedIn()
        {
            IsLoggedIn = await UserSessionService.IsLoggedIn();
        }

        private protected async Task SetAccountStateAsync(MediaType mediaType, int id)
        {
            if (!IsLoggedIn)
            {
                return;
            }

            AccountState accountState;

            switch (mediaType)
            {
                case MediaType.Movie:
                    accountState = await TMDbService.GetMovieAccountStateAsync(id);
                    break;
                case MediaType.Tv:
                    accountState = await TMDbService.GetTvShowAccountStateAsync(id);
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(mediaType), (int)mediaType, typeof(MediaType));
            }

            Rating = accountState.Rating ?? -1;
            IsFavorite = accountState.Favorite;
            IsNotFavorite = !IsFavorite;
            IsWatchlist = accountState.Watchlist;
            IsNotWatchlist = !IsWatchlist;
        }

        private protected async Task ChangeRatingAsync(MediaType mediaType, int id, double value)
        {
            if (!IsLoggedIn)
            {
                return;
            }

            bool success = await TMDbService.SetRatingAsync(mediaType, id, value);

            if (success && value != -1 && IsWatchlist)
            {
                IsWatchlist = !IsWatchlist;
                IsNotWatchlist = !IsWatchlist;

                if (mediaType == MediaType.Movie)
                {
                    await TileUpdateHelper.UpdateMovieWatchlistTileAsync();
                }
                else if (mediaType == MediaType.Tv)
                {
                    await TileUpdateHelper.UpdateTvShowWatchlistTileAsync();
                }
            }
        }

        private protected async Task ChangeFavoriteAsync(MediaType mediaType, int id)
        {
            if (!IsLoggedIn)
            {
                return;
            }

            var success = await TMDbService.ChangeFavoriteStatusAsync(mediaType, id, !IsFavorite);

            if (success)
            {
                IsFavorite = !IsFavorite;
                IsNotFavorite = !IsFavorite;
            }
        }

        private protected async Task ChangeWatchlistAsync(MediaType mediaType, int id)
        {
            if (!IsLoggedIn)
            {
                return;
            }

            var success = await TMDbService.ChangeWatchlistStatusAsync(mediaType, id, !IsWatchlist);

            if (success)
            {
                IsWatchlist = !IsWatchlist;
                IsNotWatchlist = !IsWatchlist;

                if (mediaType == MediaType.Movie)
                {
                    await TileUpdateHelper.UpdateMovieWatchlistTileAsync();
                }
                else if (mediaType == MediaType.Tv)
                {
                    await TileUpdateHelper.UpdateTvShowWatchlistTileAsync();
                }
            }
        }
    }
}
