using Filmster.Common.Models;
using Filmster.Common.Services;
using Microsoft.Toolkit.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.Account;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Lists;
using TMDbLib.Objects.Reviews;
using TMDbLib.Objects.Search;

namespace Filmster.Helpers
{
    public class PopularMoviesSource : IIncrementalSource<SearchMovie>
    {
        private SearchContainer<SearchMovie> Movies = new SearchContainer<SearchMovie>();

        public async Task<IEnumerable<SearchMovie>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            pageIndex++;

            if (pageIndex == 1 || pageIndex <= Movies.TotalPages)
            {
                Movies = await TMDbService.GetPopularMoviesAsync(pageIndex);
                return Movies.Results;
            }

            return default;
        }
    }

    public class UpcomingMoviesSource : IIncrementalSource<SearchMovie>
    {
        private SearchContainer<SearchMovie> Movies = new SearchContainer<SearchMovie>();

        public async Task<IEnumerable<SearchMovie>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            pageIndex++;

            if (pageIndex == 1 || pageIndex <= Movies.TotalPages)
            {
                Movies = await TMDbService.GetUpcomingMoviesAsync(pageIndex);
                return Movies.Results;
            }

            return default;
        }
    }

    public class TopRatedMoviesSource : IIncrementalSource<SearchMovie>
    {
        private SearchContainer<SearchMovie> Movies = new SearchContainer<SearchMovie>();

        public async Task<IEnumerable<SearchMovie>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            pageIndex++;

            if (pageIndex == 1 || pageIndex <= Movies.TotalPages)
            {
                Movies = await TMDbService.GetTopRatedMoviesAsync(pageIndex);
                return Movies.Results;
            }

            return default;
        }
    }

    public class PopularTvShowsSource : IIncrementalSource<SearchTv>
    {
        private SearchContainer<SearchTv> TvShows = new SearchContainer<SearchTv>();

        public async Task<IEnumerable<SearchTv>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            pageIndex++;

            if (pageIndex == 1 || pageIndex <= TvShows.TotalPages)
            {
                TvShows = await TMDbService.GetPopularTvShowsAsync(pageIndex);
                return TvShows.Results;
            }

            return default;
        }
    }

    public class TopRatedTvShowsSource : IIncrementalSource<SearchTv>
    {
        private SearchContainer<SearchTv> TvShows = new SearchContainer<SearchTv>();

        public async Task<IEnumerable<SearchTv>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            pageIndex++;

            if (pageIndex == 1 || pageIndex <= TvShows.TotalPages)
            {
                TvShows = await TMDbService.GetTopRatedTvShowsAsync(pageIndex);
                return TvShows.Results;
            }

            return default;
        }
    }

    public class PopularPeopleSource : IIncrementalSource<SearchPerson>
    {
        private SearchContainer<SearchPerson> People = new SearchContainer<SearchPerson>();

        public async Task<IEnumerable<SearchPerson>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            pageIndex++;

            if (pageIndex == 1 || pageIndex <= People.TotalPages)
            {
                People = await TMDbService.GetPopularPeopleAsync(pageIndex);
                return People.Results;
            }

            return default;
        }
    }

    public class DiscoverSource : IIncrementalSource<SearchMovieTvBase>
    {
        public static MediaType MediaType { get; set; }
        public static DiscoverMovieOptions MovieOptions { get; set; }
        public static DiscoverTvShowOptions TvShowOptions { get; set; }
        private readonly SearchContainer<SearchMovieTvBase> Result = new SearchContainer<SearchMovieTvBase>();

        public async Task<IEnumerable<SearchMovieTvBase>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            pageIndex++;

            if (pageIndex == 1 || pageIndex <= Result.TotalPages)
            {
                switch (MediaType)
                {
                    case MediaType.Movie:
                        var movies = await TMDbService.GetDiscoverMoviesAsync(MovieOptions, pageIndex);
                        Result.Page = movies.TotalPages;
                        Result.Results = movies.Results.ConvertAll(r => (SearchMovieTvBase)r);
                        Result.TotalPages = movies.TotalPages;
                        Result.TotalResults = movies.TotalResults;
                        break;
                    case MediaType.Tv:
                        var tvShows = await TMDbService.GetDiscoverTvShowsAsync(TvShowOptions, pageIndex);
                        Result.Page = tvShows.TotalPages;
                        Result.Results = tvShows.Results.ConvertAll(r => (SearchMovieTvBase)r);
                        Result.TotalPages = tvShows.TotalPages;
                        Result.TotalResults = tvShows.TotalResults;
                        break;
                    default:
                        throw new InvalidEnumArgumentException(nameof(MediaType), (int)MediaType, typeof(MediaType));
                }
                
                return Result.Results;
            }

            return default;
        }
    }

    public class ReviewsSource : IIncrementalSource<ReviewBase>
    {
        public static MediaType MediaType { get; set; }
        public static int Id { get; set; }
        private SearchContainerWithId<ReviewBase> Reviews = new SearchContainerWithId<ReviewBase>();

        public async Task<IEnumerable<ReviewBase>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            pageIndex++;

            if (pageIndex == 1 || pageIndex <= Reviews.TotalPages)
            {
                Reviews = await TMDbService.GetReviewsAsync(MediaType, Id, pageIndex);
                return Reviews.Results;
            }

            return default;
        }
    }

    public class MultiSearchSource : IIncrementalSource<SearchBase>
    {
        public static string SearchValue { get; set; }
        private SearchContainer<SearchBase> SearchItems = new SearchContainer<SearchBase>();

        public async Task<IEnumerable<SearchBase>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            pageIndex++;

            if (pageIndex == 1 || pageIndex <= SearchItems.TotalPages)
            {
                SearchItems = await TMDbService.GetMultiSearchAsync(SearchValue, pageIndex);
                return SearchItems.Results;
            }

            return default;
        }
    }

    public class SearchMovieSource : IIncrementalSource<SearchMovie>
    {
        public static string SearchValue { get; set; }
        private SearchContainer<SearchMovie> Movies = new SearchContainer<SearchMovie>();

        public async Task<IEnumerable<SearchMovie>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            pageIndex++;

            if ((pageIndex == 1 || pageIndex <= Movies.TotalPages) && !string.IsNullOrWhiteSpace(SearchValue))
            {
                Movies = await TMDbService.GetSearchMovieAsync(SearchValue, pageIndex);
                return Movies.Results;
            }

            return default;
        }
    }

    public class RatedMoviesSource : IIncrementalSource<SearchMovieWithRating>
    {
        private SearchContainer<SearchMovieWithRating> Movies = new SearchContainer<SearchMovieWithRating>();

        public async Task<IEnumerable<SearchMovieWithRating>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            pageIndex++;

            if (pageIndex == 1 || pageIndex <= Movies.TotalPages)
            {
                Movies = await TMDbService.GetRatedMoviesAsync(pageIndex, AccountSortBy.CreatedAt, SortOrder.Descending);
                return Movies.Results;
            }

            return default;
        }
    }

    public class RatedTvShowsSource : IIncrementalSource<AccountSearchTv>
    {
        private SearchContainer<AccountSearchTv> TvShows = new SearchContainer<AccountSearchTv>();

        public async Task<IEnumerable<AccountSearchTv>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            pageIndex++;

            if (pageIndex == 1 || pageIndex <= TvShows.TotalPages)
            {
                TvShows = await TMDbService.GetRatedTvShowsAsync(pageIndex, AccountSortBy.CreatedAt, SortOrder.Descending);
                return TvShows.Results;
            }

            return default;
        }
    }

    public class RatedTvEpisodesSource : IIncrementalSource<AccountSearchTvEpisode>
    {
        private SearchContainer<AccountSearchTvEpisode> TvEpisodes = new SearchContainer<AccountSearchTvEpisode>();

        public async Task<IEnumerable<AccountSearchTvEpisode>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            pageIndex++;

            if (pageIndex == 1 || pageIndex <= TvEpisodes.TotalPages)
            {
                TvEpisodes = await TMDbService.GetRatedTvEpisodesAsync(pageIndex, AccountSortBy.CreatedAt, SortOrder.Descending);
                return TvEpisodes.Results;
            }

            return default;
        }
    }

    public class FavoriteMoviesSource : IIncrementalSource<SearchMovie>
    {
        private SearchContainer<SearchMovie> Movies = new SearchContainer<SearchMovie>();

        public async Task<IEnumerable<SearchMovie>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            pageIndex++;

            if (pageIndex == 1 || pageIndex <= Movies.TotalPages)
            {
                Movies = await TMDbService.GetFavoriteMoviesAsync(pageIndex, AccountSortBy.CreatedAt, SortOrder.Descending);
                return Movies.Results;
            }

            return default;
        }
    }

    public class FavoriteTvShowsSource : IIncrementalSource<SearchTv>
    {
        private SearchContainer<SearchTv> TvShows = new SearchContainer<SearchTv>();

        public async Task<IEnumerable<SearchTv>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            pageIndex++;

            if (pageIndex == 1 || pageIndex <= TvShows.TotalPages)
            {
                TvShows = await TMDbService.GetFavoriteTvShowsAsync(pageIndex, AccountSortBy.CreatedAt, SortOrder.Descending);
                return TvShows.Results;
            }

            return default;
        }
    }

    public class MovieWatchlistSource : IIncrementalSource<SearchMovie>
    {
        private SearchContainer<SearchMovie> Movies = new SearchContainer<SearchMovie>();

        public async Task<IEnumerable<SearchMovie>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            pageIndex++;

            if (pageIndex == 1 || pageIndex <= Movies.TotalPages)
            {
                Movies = await TMDbService.GetMovieWatchlistAsync(pageIndex, AccountSortBy.CreatedAt, SortOrder.Descending);
                return Movies.Results;
            }

            return default;
        }
    }

    public class TvShowWatchlistSource : IIncrementalSource<SearchTv>
    {
        private SearchContainer<SearchTv> TvShows = new SearchContainer<SearchTv>();

        public async Task<IEnumerable<SearchTv>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            pageIndex++;

            if (pageIndex == 1 || pageIndex <= TvShows.TotalPages)
            {
                TvShows = await TMDbService.GetTvShowWatchlistAsync(pageIndex, AccountSortBy.CreatedAt, SortOrder.Descending);
                return TvShows.Results;
            }

            return default;
        }
    }

    public class ListsSource : IIncrementalSource<AccountList>
    {
        private SearchContainer<AccountList> Lists = new SearchContainer<AccountList>();

        public async Task<IEnumerable<AccountList>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            pageIndex++;

            if (pageIndex == 1 || pageIndex <= Lists.TotalPages)
            {
                Lists = await TMDbService.GetListsAsync(pageIndex);
                return Lists.Results;
            }

            return default;
        }
    }
}
