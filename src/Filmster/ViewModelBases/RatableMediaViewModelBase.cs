using Filmster.Common.Helper.Tile;
using Filmster.Common.Services;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.TvShows;

namespace Filmster.ViewModelBases
{
    public class RatableMediaViewModelBase : MediaViewModelBase
    {
        private bool _isLoggedIn = UserSessionService.IsLoggedIn;
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set => Set(ref _isLoggedIn, value);
        }

        private double _rating;
        public double Rating
        {
            get => _rating;
            set => Set(ref _rating, value);
        }

        private bool _isFavorite;
        public bool IsFavorite
        {
            get => _isFavorite;
            set => Set(ref _isFavorite, value);
        }

        private bool _isNotFavorite;
        public bool IsNotFavorite
        {
            get => _isNotFavorite;
            set => Set(ref _isNotFavorite, value);
        }

        private bool _isWatchlist;
        public bool IsWatchlist
        {
            get => _isWatchlist;
            set => Set(ref _isWatchlist, value);
        }

        private bool _isNotWatchlist;
        public bool IsNotWatchlist
        {
            get => _isNotWatchlist;
            set => Set(ref _isNotWatchlist, value);
        }

        private protected void SetAccountState(AccountState accountState)
        {
            if (accountState != null)
            {
                Rating = accountState.Rating ?? -1;
                IsFavorite = accountState.Favorite;
                IsNotFavorite = !IsFavorite;
                IsWatchlist = accountState.Watchlist;
                IsNotWatchlist = !IsWatchlist;
            }
        }

        private protected void SetAccountState(TvAccountState tvAccountState)
        {
            if (tvAccountState != null)
            {
                Rating = tvAccountState.Rating ?? -1;
            }
        }

        private protected async Task ChangeRatingAsync(MediaType mediaType, double value, int id, int? seasonNumber = null, int? episodeNumber = null)
        {
            if (!IsLoggedIn)
            {
                return;
            }

            bool success = await TMDbService.SetRatingAsync(mediaType, value, id, seasonNumber, episodeNumber);

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
