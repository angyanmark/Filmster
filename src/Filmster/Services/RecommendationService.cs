﻿using Filmster.Common.Services;
using Filmster.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.Account;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace Filmster.Services
{
    public static class RecommendationService
    {
        private static readonly int TakeCount = 4;

        public static async Task<List<SearchMovie>> RecommendMoviesAsync()
        {
            var favorites = await TMDbService.GetFavoriteMoviesAsync(accountSortBy: AccountSortBy.CreatedAt, sortOrder: SortOrder.Descending);

            var tasks = new List<Task<SearchContainer<SearchMovie>>>();
            foreach (var favorite in favorites.Results.Shuffle().Take(TakeCount))
            {
                tasks.Add(TMDbService.GetMovieRecommendationsAsync(favorite.Id));
            }

            var recommended = new List<SearchMovie>();
            foreach (var task in tasks)
            {
                var recommendations = await task;
                recommended.AddRange(recommendations.Results.Shuffle().Take(TakeCount));
            }

            return recommended;
        }

        public static async Task<List<SearchTv>> RecommendTvShowsAsync()
        {
            var favorites = await TMDbService.GetFavoriteTvShowsAsync(accountSortBy: AccountSortBy.CreatedAt, sortOrder: SortOrder.Descending);

            var tasks = new List<Task<SearchContainer<SearchTv>>>();
            foreach (var favorite in favorites.Results.Shuffle().Take(TakeCount))
            {
                tasks.Add(TMDbService.GetTvShowRecommendationsAsync(favorite.Id));
            }

            var recommended = new List<SearchTv>();
            foreach (var task in tasks)
            {
                var recommendations = await task;
                recommended.AddRange(recommendations.Results.Shuffle().Take(TakeCount));
            }

            return recommended;
        }
    }
}
