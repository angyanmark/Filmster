using Filmster.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.Account;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Lists;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.Trending;
using TMDbLib.Objects.TvShows;

namespace Filmster.Common.Services
{
    public static class TMDbService
    {
        private static readonly TMDbClient client = new TMDbClient("5e9bcb638329a15acf75c1b2d85ae67e");

        public static readonly string SecureBaseUrl = "https://image.tmdb.org/t/p/";

        public static readonly string SmallBackdropSize = "w300";
        public static readonly string MediumBackdropSize = "w780";
        public static readonly string LargeBackdropSize = "w1280";

        public static readonly string SmallLogoSize = "w92";
        public static readonly string MediumLogoSize = "w185";
        public static readonly string LargeLogoSize = "w300";

        public static readonly string SmallPosterSize = "w154";
        public static readonly string MediumPosterSize = "w342";
        public static readonly string LargePosterSize = "w500";

        public static readonly string SmallProfileSize = "w45";
        public static readonly string MediumProfileSize = "w185";
        public static readonly string LargeProfileSize = "h632";

        public static readonly string SmallStillSize = "w92";
        public static readonly string MediumStillSize = "w185";
        public static readonly string LargeStillSize = "w300";

        public static readonly string OriginalSize = "original";

        public static readonly string TMDbBaseUrl = "https://www.themoviedb.org/";
        public static readonly string TMDbMovieBaseUrl = "https://www.themoviedb.org/movie/";
        public static readonly string TMDbTvShowBaseUrl = "https://www.themoviedb.org/tv/";
        public static readonly string TMDbPersonBaseUrl = "https://www.themoviedb.org/person/";
        public static readonly string TMDbCollectionBaseUrl = "https://www.themoviedb.org/collection/";
        public static readonly string TMDbListBaseUrl = "https://www.themoviedb.org/list/";
        public static readonly string IMDbBaseUrl = "https://www.imdb.com/";
        public static readonly string IMDbMovieBaseUrl = "https://www.imdb.com/title/";
        public static readonly string IMDbTvShowBaseUrl = "https://www.imdb.com/title/";
        public static readonly string IMDbPersonBaseUrl = "https://www.imdb.com/name/";
        public static readonly string YouTubeBaseUrl = "https://www.youtube.com/watch?v=";
        public static readonly string FacebookBaseUrl = "https://www.facebook.com/";
        public static readonly string TwitterBaseUrl = "https://twitter.com/";
        public static readonly string InstagramBaseUrl = "https://www.instagram.com/";

        public static readonly string TMDbTvSeasonUrl = "https://www.themoviedb.org/tv/{0}/season/{1}";
        public static readonly string TMDbTvEpisodeUrl = "https://www.themoviedb.org/tv/{0}/season/{1}/episode/{2}";
        public static readonly string IMDbTvSeasonUrl = "https://www.imdb.com/title/{0}/episodes?season={1}";

        public static readonly string TMDbLogInBaseUrl = "https://www.themoviedb.org/authenticate/";
        public static readonly string GravatarBaseUrl = "https://secure.gravatar.com/avatar/";

        public static readonly int DefaultCastCrewBackdropCount = 15;

        private static string CurrentLanguage { get; } = LanguageService.CurrentLanguage;

        private static string IncludeImageLanguage
        {
            get
            {
                var includeLanguage = "null,en";
                var code = CurrentLanguage.Substring(0, 2);
                return code == "en"
                    ? includeLanguage
                    : $"{code},{includeLanguage}";
            }
        }

        public static async Task<SearchContainer<SearchMovie>> GetPopularMoviesAsync(int page = 0)
        {
            return await client.GetMoviePopularListAsync(CurrentLanguage, page);
        }

        public static async Task<SearchContainer<SearchMovie>> GetUpcomingMoviesAsync(int page = 0)
        {
            return await client.GetMovieUpcomingListAsync(CurrentLanguage, page);
        }

        public static async Task<SearchContainer<SearchMovie>> GetTopRatedMoviesAsync(int page = 0)
        {
            return await client.GetMovieTopRatedListAsync(CurrentLanguage, page);
        }

        public static async Task<Movie> GetMovieAsync(int id, bool includeAccountState = false)
        {
            return await client.GetMovieAsync(id, CurrentLanguage, IncludeImageLanguage, (includeAccountState ? MovieMethods.AccountStates : MovieMethods.Undefined) | MovieMethods.Images | MovieMethods.Videos | MovieMethods.Credits | MovieMethods.Recommendations | MovieMethods.ReleaseDates);
        }

        public static async Task<Collection> GetCollectionAsync(int id, CollectionMethods extraMethods = CollectionMethods.Undefined)
        {
            return await client.GetCollectionAsync(id, CurrentLanguage, IncludeImageLanguage, extraMethods);
        }

        public static async Task<SearchContainer<SearchTv>> GetPopularTvShowsAsync(int page = 0)
        {
            return await client.GetTvShowPopularAsync(page, CurrentLanguage);
        }

        public static async Task<SearchContainer<SearchTv>> GetTopRatedTvShowsAsync(int page = 0)
        {
            return await client.GetTvShowTopRatedAsync(page, CurrentLanguage);
        }

        public static async Task<TvShow> GetTvShowAsync(int id, bool includeAccountState = false)
        {
            return await client.GetTvShowAsync(id, (includeAccountState ? TvShowMethods.AccountStates : TvShowMethods.Undefined) | TvShowMethods.Images | TvShowMethods.Videos | TvShowMethods.Credits | TvShowMethods.Recommendations | TvShowMethods.ContentRatings | TvShowMethods.ExternalIds, CurrentLanguage, IncludeImageLanguage);
        }

        public static async Task<TvSeason> GetTvSeasonAsync(int tvShowId, int seasonNumber)
        {
            return await client.GetTvSeasonAsync(tvShowId, seasonNumber, TvSeasonMethods.Images | TvSeasonMethods.Credits | TvSeasonMethods.Videos, CurrentLanguage, IncludeImageLanguage);
        }

        public static async Task<SearchContainer<SearchPerson>> GetTrendingPeopleAsync(TimeWindow timeWindow, int page = 0)
        {
            return await client.GetTrendingPeopleAsync(timeWindow, page);  // NOTE: no language filter
        }

        public static async Task<Person> GetPersonAsync(int id)
        {
            return await client.GetPersonAsync(id, CurrentLanguage, PersonMethods.Images | PersonMethods.MovieCredits | PersonMethods.TvCredits | PersonMethods.TaggedImages);
        }

        public static async Task<List<SearchMovie>> GetSearchMovieAsync(string value, bool includeAdult = false)
        {
            return (await client.SearchMovieAsync(value, CurrentLanguage, includeAdult: includeAdult)).Results;
        }

        public static async Task<List<SearchBase>> GetMultiSearchAsync(string value, bool includeAdult = false)
        {
            return (await client.SearchMultiAsync(value, CurrentLanguage, includeAdult: includeAdult)).Results;
        }

        public static async Task<List<SearchMovie>> GetDiscoverMoviesAsync(DiscoverMovieOptions options)
        {
            var query = await client.DiscoverMoviesAsync()
                .WherePrimaryReleaseDateIsAfter(options.PrimaryReleaseDateAfter)
                .WherePrimaryReleaseDateIsBefore(options.PrimaryReleaseDateBefore)
                .WhereVoteAverageIsAtLeast(options.VoteAverageAtLeast)
                .WhereVoteAverageIsAtMost(options.VoteAverageAtMost)
                .WhereVoteCountIsAtLeast(options.VoteCountAtLeast)
                .IncludeWithAllOfGenre(options.GenreId == 0 ? new List<int>() : new List<int> { options.GenreId })
                .OrderBy(options.SortBy)
                .Query(CurrentLanguage);

            return query.Results;
        }

        public static async Task<List<Genre>> GetMovieGenresAsync()
        {
            return await client.GetMovieGenresAsync(CurrentLanguage);
        }

        public static async Task<Token> GetAutenticationTokenAsync()
        {
            return await client.AuthenticationRequestAutenticationTokenAsync();
        }

        public static async Task<UserSession> GetUserSessionAsync(string requestToken)
        {
            return await client.AuthenticationGetUserSessionAsync(requestToken);
        }

        public static async Task SetSessionInformationAsync(string sessionId, SessionType sessionType)
        {
            await client.SetSessionInformationAsync(sessionId, sessionType);
        }

        public static async Task<bool> DeleteUserSessionAsync()
        {
            // TODO: delete session
            await client.SetSessionInformationAsync(string.Empty, SessionType.Unassigned);
            return true;
        }

        public static async Task<AccountDetails> GetAccountDetailsAsync()
        {
            return await client.AccountGetDetailsAsync();
        }

        public static async Task<SearchContainer<SearchMovieWithRating>> GetRatedMoviesAsync(int page = 0, AccountSortBy accountSortBy = AccountSortBy.Undefined, SortOrder sortOrder = SortOrder.Undefined)
        {
            return await client.AccountGetRatedMoviesAsync(page, accountSortBy, sortOrder, CurrentLanguage);
        }

        public static async Task<SearchContainer<AccountSearchTv>> GetRatedTvShowsAsync(int page = 0, AccountSortBy accountSortBy = AccountSortBy.Undefined, SortOrder sortOrder = SortOrder.Undefined)
        {
            return await client.AccountGetRatedTvShowsAsync(page, accountSortBy, sortOrder, CurrentLanguage);
        }

        public static async Task<SearchContainer<SearchMovie>> GetFavoriteMoviesAsync(int page = 0, AccountSortBy accountSortBy = AccountSortBy.Undefined, SortOrder sortOrder = SortOrder.Undefined)
        {
            return await client.AccountGetFavoriteMoviesAsync(page, accountSortBy, sortOrder, CurrentLanguage);
        }

        public static async Task<SearchContainer<SearchTv>> GetFavoriteTvShowsAsync(int page = 0, AccountSortBy accountSortBy = AccountSortBy.Undefined, SortOrder sortOrder = SortOrder.Undefined)
        {
            return await client.AccountGetFavoriteTvAsync(page, accountSortBy, sortOrder, CurrentLanguage);
        }

        public static async Task<SearchContainer<SearchMovie>> GetMovieWatchlistAsync(int page = 0, AccountSortBy sortBy = AccountSortBy.Undefined, SortOrder sortOrder = SortOrder.Undefined)
        {
            return await client.AccountGetMovieWatchlistAsync(page, sortBy, sortOrder, language: CurrentLanguage);
        }

        public static async Task<SearchContainer<SearchTv>> GetTvShowWatchlistAsync(int page = 0, AccountSortBy sortBy = AccountSortBy.Undefined, SortOrder sortOrder = SortOrder.Undefined)
        {
            return await client.AccountGetTvWatchlistAsync(page, sortBy, sortOrder, language: CurrentLanguage);
        }

        public static async Task<bool> SetRatingAsync(MediaType mediaType, int id, double value)
        {
            bool success;

            switch (mediaType)
            {
                case MediaType.Movie:
                    success = value == -1
                        ? await client.MovieRemoveRatingAsync(id)
                        : await client.MovieSetRatingAsync(id, value * 2);
                    break;
                case MediaType.Tv:
                    success = value == -1
                        ? await client.TvShowRemoveRatingAsync(id)
                        : await client.TvShowSetRatingAsync(id, value * 2);
                    break;
                default:
                    throw new ArgumentException(string.Format("Cannot set rating to media type {0}.", mediaType), nameof(mediaType));
            }

            return success;
        }

        public static async Task<bool> ChangeFavoriteStatusAsync(MediaType mediaType, int id, bool isFavorite)
        {
            return await client.AccountChangeFavoriteStatusAsync(mediaType, id, isFavorite);
        }

        public static async Task<bool> ChangeWatchlistStatusAsync(MediaType mediaType, int id, bool isWatchlist)
        {
            return await client.AccountChangeWatchlistStatusAsync(mediaType, id, isWatchlist);
        }

        public static async Task<SearchContainer<AccountList>> GetListsAsync(int page = 0)
        {
            return await client.AccountGetListsAsync(page, CurrentLanguage);
        }

        public static async Task<GenericList> GetListAsync(int id)
        {
            return await client.GetListAsync(id.ToString(), CurrentLanguage);
        }

        public static async Task<string> ListCreateAsync(string name, string description = "")
        {
            return await client.ListCreateAsync(name, description);
        }

        public static async Task<bool> ListAddMovieAsync(string listId, int id)
        {
            return await client.ListAddMovieAsync(listId, id);
        }

        public static async Task<bool> ListRemoveMovieAsync(string listId, int id)
        {
            return await client.ListRemoveMovieAsync(listId, id);
        }

        public static async Task<bool> ListClearAsync(string listId)
        {
            return await client.ListClearAsync(listId);
        }

        public static async Task<bool> ListDeleteAsync(string listId)
        {
            return await client.ListDeleteAsync(listId);
        }
    }
}
