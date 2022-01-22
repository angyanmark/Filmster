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

            var recommendations = new List<SearchMovie>();

            foreach (var favorite in favorites.Results.Shuffle().Take(4))
            {
                var movie = await TMDbService.GetMovieAsync(favorite.Id);
                recommendations.AddRange(movie.Recommendations.Results.Shuffle().Take(4));
            }

            return recommendations;
        }
    }
}
