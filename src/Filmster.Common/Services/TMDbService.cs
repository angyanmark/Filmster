﻿using Filmster.Common.Models;
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
using TMDbLib.Objects.Reviews;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.Trending;
using TMDbLib.Objects.TvShows;

namespace Filmster.Common.Services
{
    public static class TMDbService
    {
        private static readonly TMDbClient client = new TMDbClient("5e9bcb638329a15acf75c1b2d85ae67e");

        public const string SecureBaseUrl = "https://image.tmdb.org/t/p/";

        public const string SmallBackdropSize = "w300";
        public const string MediumBackdropSize = "w780";
        public const string LargeBackdropSize = "w1280";

        public const string SmallLogoSize = "w92";
        public const string MediumLogoSize = "w185";
        public const string LargeLogoSize = "w300";

        public const string SmallPosterSize = "w154";
        public const string MediumPosterSize = "w342";
        public const string LargePosterSize = "w500";

        public const string SmallProfileSize = "w45";
        public const string MediumProfileSize = "w185";
        public const string LargeProfileSize = "h632";

        public const string SmallStillSize = "w92";
        public const string MediumStillSize = "w185";
        public const string LargeStillSize = "w300";
        public const string XLargeStillSize = "w500";
        public const string XXLargeStillSize = "w1280";

        public const string OriginalSize = "original";

        public const string TMDbBaseUrl = "https://www.themoviedb.org/";
        public const string TMDbMovieBaseUrl = "https://www.themoviedb.org/movie/";
        public const string TMDbTvShowBaseUrl = "https://www.themoviedb.org/tv/";
        public const string TMDbPersonBaseUrl = "https://www.themoviedb.org/person/";
        public const string TMDbCollectionBaseUrl = "https://www.themoviedb.org/collection/";
        public const string TMDbListBaseUrl = "https://www.themoviedb.org/list/";
        public const string IMDbBaseUrl = "https://www.imdb.com/";
        public const string IMDbMovieBaseUrl = "https://www.imdb.com/title/";
        public const string IMDbTvShowBaseUrl = "https://www.imdb.com/title/";
        public const string IMDbTvEpisodeBaseUrl = "https://www.imdb.com/title/";
        public const string IMDbPersonBaseUrl = "https://www.imdb.com/name/";
        public const string YouTubeBaseUrl = "https://www.youtube.com/watch?v=";
        public const string FacebookBaseUrl = "https://www.facebook.com/";
        public const string TwitterBaseUrl = "https://twitter.com/";
        public const string InstagramBaseUrl = "https://www.instagram.com/";

        public const string TMDbTvSeasonUrl = "https://www.themoviedb.org/tv/{0}/season/{1}";
        public const string TMDbTvEpisodeUrl = "https://www.themoviedb.org/tv/{0}/season/{1}/episode/{2}";
        public const string IMDbTvSeasonUrl = "https://www.imdb.com/title/{0}/episodes?season={1}";

        public const string TMDbLogInBaseUrl = "https://www.themoviedb.org/authenticate/";
        public const string GravatarBaseUrl = "https://secure.gravatar.com/avatar/";

        public const int DefaultCastCrewBackdropCount = 15;

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

        public static async Task<SearchContainer<SearchMovie>> GetTrendingMoviesAsync(TimeWindow timeWindow, int page = 0) =>
            await client.GetTrendingMoviesAsync(timeWindow, page, CurrentLanguage);

        public static async Task<SearchContainer<SearchMovie>> GetPopularMoviesAsync(int page = 0) =>
            await client.GetMoviePopularListAsync(CurrentLanguage, page);

        public static async Task<SearchContainerWithDates<SearchMovie>> GetNowPlayingMoviesAsync(int page = 0) =>
            await client.GetMovieNowPlayingListAsync(CurrentLanguage, page);

        public static async Task<SearchContainer<SearchMovie>> GetUpcomingMoviesAsync(int page = 0) =>
            await client.GetMovieUpcomingListAsync(CurrentLanguage, page);

        public static async Task<SearchContainer<SearchMovie>> GetTopRatedMoviesAsync(int page = 0) =>
            await client.GetMovieTopRatedListAsync(CurrentLanguage, page);

        public static async Task<Movie> GetMovieAsync(int id, bool includeAccountState = false) =>
            await client.GetMovieAsync(id, CurrentLanguage, IncludeImageLanguage, (includeAccountState ? MovieMethods.AccountStates : MovieMethods.Undefined) | MovieMethods.Images | MovieMethods.Videos | MovieMethods.Credits | MovieMethods.Recommendations | MovieMethods.ReleaseDates | MovieMethods.Reviews);

        public static async Task<SearchContainer<SearchMovie>> GetMovieRecommendationsAsync(int id) =>
            await client.GetMovieRecommendationsAsync(id, CurrentLanguage);

        public static async Task<Collection> GetCollectionAsync(int id, CollectionMethods extraMethods = CollectionMethods.Undefined) =>
            await client.GetCollectionAsync(id, CurrentLanguage, IncludeImageLanguage, extraMethods);

        public static async Task<SearchContainerWithId<ReviewBase>> GetReviewsAsync(MediaType mediaType, int id, int page = 0)
        {
            switch (mediaType)
            {
                case MediaType.Movie:
                    return await client.GetMovieReviewsAsync(id, CurrentLanguage, page);
                case MediaType.Tv:
                    return await client.GetTvShowReviewsAsync(id, CurrentLanguage, page);
                default:
                    throw new ArgumentException(string.Format("Cannot get reviews to media type {0}.", mediaType), nameof(mediaType));
            }
        }

        public static async Task<Review> GetReviewAsync(string id) =>
            await client.GetReviewAsync(id, CurrentLanguage);

        public static async Task<SearchContainer<SearchTv>> GetTrendingTvShowsAsync(TimeWindow timeWindow, int page = 0) =>
            await client.GetTrendingTvAsync(timeWindow, page, CurrentLanguage);

        public static async Task<SearchContainer<SearchTv>> GetPopularTvShowsAsync(int page = 0) =>
            await client.GetTvShowPopularAsync(page, CurrentLanguage);

        public static async Task<SearchContainer<SearchTv>> GetTopRatedTvShowsAsync(int page = 0) =>
            await client.GetTvShowTopRatedAsync(page, CurrentLanguage);

        public static async Task<TvShow> GetTvShowAsync(int id, bool includeAccountState = false) =>
            await client.GetTvShowAsync(id, (includeAccountState ? TvShowMethods.AccountStates : TvShowMethods.Undefined) | TvShowMethods.Images | TvShowMethods.Videos | TvShowMethods.Credits | TvShowMethods.Recommendations | TvShowMethods.ContentRatings | TvShowMethods.ExternalIds | TvShowMethods.Reviews, CurrentLanguage, IncludeImageLanguage);

        public static async Task<SearchContainer<SearchTv>> GetTvShowRecommendationsAsync(int id) =>
            await client.GetTvShowRecommendationsAsync(id, CurrentLanguage);

        public static async Task<TvSeason> GetTvSeasonAsync(int tvShowId, int seasonNumber, bool includeAccountState = false) =>
            await client.GetTvSeasonAsync(tvShowId, seasonNumber, (includeAccountState ? TvSeasonMethods.AccountStates : TvSeasonMethods.Undefined) | TvSeasonMethods.Images | TvSeasonMethods.Credits | TvSeasonMethods.Videos, CurrentLanguage, IncludeImageLanguage);

        public static async Task<TvEpisode> GetTvEpisodeAsync(int tvShowId, int seasonNumber, int episodeNumber, bool includeAccountState = false) =>
            await client.GetTvEpisodeAsync(tvShowId, seasonNumber, episodeNumber, (includeAccountState ? TvEpisodeMethods.AccountStates : TvEpisodeMethods.Undefined) | TvEpisodeMethods.Images | TvEpisodeMethods.Credits | TvEpisodeMethods.Videos | TvEpisodeMethods.ExternalIds, CurrentLanguage, IncludeImageLanguage);

        public static async Task<SearchContainer<SearchPerson>> GetTrendingPeopleAsync(TimeWindow timeWindow, int page = 0) =>
            await client.GetTrendingPeopleAsync(timeWindow, page, CurrentLanguage);

        public static async Task<SearchContainer<SearchPerson>> GetPopularPeopleAsync(int page = 0) =>
            await client.GetPersonPopularListAsync(page, CurrentLanguage);

        public static async Task<Person> GetPersonAsync(int id) =>
            await client.GetPersonAsync(id, CurrentLanguage, PersonMethods.Images | PersonMethods.MovieCredits | PersonMethods.TvCredits | PersonMethods.TaggedImages);

        public static async Task<SearchContainer<SearchMovie>> GetSearchMovieAsync(string value, int page = 0) =>
            await client.SearchMovieAsync(value, CurrentLanguage, page);

        public static async Task<SearchContainer<SearchBase>> GetMultiSearchAsync(string value, int page = 0) =>
            await client.SearchMultiAsync(value, CurrentLanguage, page);

        public static async Task<SearchContainer<SearchMovie>> GetDiscoverMoviesAsync(DiscoverMovieOptions options, int page = 0) =>
            await client.DiscoverMoviesAsync()
                .WherePrimaryReleaseDateIsAfter(options.PrimaryReleaseDateAfter)
                .WherePrimaryReleaseDateIsBefore(options.PrimaryReleaseDateBefore)
                .WhereVoteAverageIsAtLeast(options.VoteAverageAtLeast)
                .WhereVoteAverageIsAtMost(options.VoteAverageAtMost)
                .WhereVoteCountIsAtLeast(options.VoteCountAtLeast)
                .IncludeWithAllOfGenre(options.GenreId == 0 ? new List<int>() : new List<int> { options.GenreId })
                .OrderBy(options.SortBy)
                .Query(CurrentLanguage, page);

        public static async Task<SearchContainer<SearchTv>> GetDiscoverTvShowsAsync(DiscoverTvShowOptions options, int page = 0) =>
            await client.DiscoverTvShowsAsync()
                .WhereFirstAirDateIsAfter(options.FirstAirDateAfter)
                .WhereFirstAirDateIsBefore(options.FirstAirDateBefore)
                .WhereVoteAverageIsAtLeast(options.VoteAverageAtLeast)
                .WhereVoteAverageIsAtMost(options.VoteAverageAtMost)
                .WhereVoteCountIsAtLeast(options.VoteCountAtLeast)
                .WhereGenresInclude(options.GenreId == 0 ? new List<int>() : new List<int> { options.GenreId })
                .OrderBy(options.SortBy)
                .Query(CurrentLanguage, page);

        public static async Task<List<Genre>> GetMovieGenresAsync() =>
            await client.GetMovieGenresAsync(CurrentLanguage);

        public static async Task<List<Genre>> GetTvShowGenresAsync() =>
            await client.GetTvGenresAsync(CurrentLanguage);

        public static async Task<Token> GetAuthenticationTokenAsync() =>
            await client.AuthenticationRequestAutenticationTokenAsync();

        public static async Task<UserSession> GetUserSessionAsync(string requestToken) =>
            await client.AuthenticationGetUserSessionAsync(requestToken);

        public static async Task SetSessionInformationAsync(string sessionId, SessionType sessionType) =>
            await client.SetSessionInformationAsync(sessionId, sessionType);

        public static async Task<bool> DeleteUserSessionAsync()
        {
            await client.SetSessionInformationAsync(string.Empty, SessionType.Unassigned);
            return true;
        }

        public static async Task<AccountDetails> GetAccountDetailsAsync() =>
            await client.AccountGetDetailsAsync();

        public static async Task<SearchContainer<SearchMovieWithRating>> GetRatedMoviesAsync(int page = 0, AccountSortBy accountSortBy = AccountSortBy.Undefined, SortOrder sortOrder = SortOrder.Undefined) =>
            await client.AccountGetRatedMoviesAsync(page, accountSortBy, sortOrder, CurrentLanguage);

        public static async Task<SearchContainer<AccountSearchTv>> GetRatedTvShowsAsync(int page = 0, AccountSortBy accountSortBy = AccountSortBy.Undefined, SortOrder sortOrder = SortOrder.Undefined) =>
            await client.AccountGetRatedTvShowsAsync(page, accountSortBy, sortOrder, CurrentLanguage);

        public static async Task<SearchContainer<AccountSearchTvEpisode>> GetRatedTvEpisodesAsync(int page = 0, AccountSortBy accountSortBy = AccountSortBy.Undefined, SortOrder sortOrder = SortOrder.Undefined) =>
            await client.AccountGetRatedTvShowEpisodesAsync(page, accountSortBy, sortOrder, CurrentLanguage);

        public static async Task<SearchContainer<SearchMovie>> GetFavoriteMoviesAsync(int page = 0, AccountSortBy accountSortBy = AccountSortBy.Undefined, SortOrder sortOrder = SortOrder.Undefined) =>
            await client.AccountGetFavoriteMoviesAsync(page, accountSortBy, sortOrder, CurrentLanguage);

        public static async Task<SearchContainer<SearchTv>> GetFavoriteTvShowsAsync(int page = 0, AccountSortBy accountSortBy = AccountSortBy.Undefined, SortOrder sortOrder = SortOrder.Undefined) =>
            await client.AccountGetFavoriteTvAsync(page, accountSortBy, sortOrder, CurrentLanguage);

        public static async Task<SearchContainer<SearchMovie>> GetMovieWatchlistAsync(int page = 0, AccountSortBy sortBy = AccountSortBy.Undefined, SortOrder sortOrder = SortOrder.Undefined) =>
            await client.AccountGetMovieWatchlistAsync(page, sortBy, sortOrder, language: CurrentLanguage);

        public static async Task<SearchContainer<SearchTv>> GetTvShowWatchlistAsync(int page = 0, AccountSortBy sortBy = AccountSortBy.Undefined, SortOrder sortOrder = SortOrder.Undefined) =>
            await client.AccountGetTvWatchlistAsync(page, sortBy, sortOrder, language: CurrentLanguage);

        public static async Task<bool> SetRatingAsync(MediaType mediaType, double value, int id, int? seasonNumber = null, int? episodeNumber = null)
        {
            switch (mediaType)
            {
                case MediaType.Movie:
                    return value == -1
                        ? await client.MovieRemoveRatingAsync(id)
                        : await client.MovieSetRatingAsync(id, value * 2);
                case MediaType.Tv:
                    return value == -1
                        ? await client.TvShowRemoveRatingAsync(id)
                        : await client.TvShowSetRatingAsync(id, value * 2);
                case MediaType.Episode:
                    return value == -1
                        ? await client.TvEpisodeRemoveRatingAsync(id, seasonNumber.Value, episodeNumber.Value)
                        : await client.TvEpisodeSetRatingAsync(id, seasonNumber.Value, episodeNumber.Value, value * 2);
                default:
                    throw new ArgumentException(string.Format("Cannot set rating to media type {0}.", mediaType), nameof(mediaType));
            }
        }

        public static async Task<bool> ChangeFavoriteStatusAsync(MediaType mediaType, int id, bool isFavorite) =>
            await client.AccountChangeFavoriteStatusAsync(mediaType, id, isFavorite);

        public static async Task<bool> ChangeWatchlistStatusAsync(MediaType mediaType, int id, bool isWatchlist) =>
            await client.AccountChangeWatchlistStatusAsync(mediaType, id, isWatchlist);

        public static async Task<SearchContainer<AccountList>> GetListsAsync(int page = 0) =>
            await client.AccountGetListsAsync(page, CurrentLanguage);

        public static async Task<GenericList> GetListAsync(int id) =>
            await client.GetListAsync(id.ToString(), CurrentLanguage);

        public static async Task<string> ListCreateAsync(string name, string description = "") =>
            await client.ListCreateAsync(name, description);

        public static async Task<bool> ListAddMovieAsync(string listId, int id) =>
            await client.ListAddMovieAsync(listId, id);

        public static async Task<bool> ListRemoveMovieAsync(string listId, int id) =>
            await client.ListRemoveMovieAsync(listId, id);

        public static async Task<bool> ListClearAsync(string listId) =>
            await client.ListClearAsync(listId);

        public static async Task<bool> ListDeleteAsync(string listId) =>
            await client.ListDeleteAsync(listId);
    }
}
