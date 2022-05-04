using Filmster.Common.Services;
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
        public static async Task<List<SearchMovie>> RecommendMoviesAsync()
        {
            var favorites = await TMDbService.GetFavoriteMoviesAsync(accountSortBy: AccountSortBy.CreatedAt, sortOrder: SortOrder.Descending);

            var tasks = new List<Task<SearchContainer<SearchMovie>>>();
            foreach (var favorite in favorites.Results.Shuffle().Take(4))
            {
                tasks.Add(TMDbService.GetMovieRecommendationsAsync(favorite.Id));
            }

            var recommended = new List<SearchMovie>();
            foreach (var task in tasks)
            {
                var recommendations = await task;
                recommended.AddRange(recommendations.Results.Shuffle().Take(4));
            }

            return recommended;
        }
    }
}
