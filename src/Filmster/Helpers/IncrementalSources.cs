using Filmster.Common.Services;
using Microsoft.Toolkit.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Lists;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.Trending;

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

    public class TrendingPeopleSource : IIncrementalSource<SearchPerson>
    {
        private SearchContainer<SearchPerson> People = new SearchContainer<SearchPerson>();

        public async Task<IEnumerable<SearchPerson>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            pageIndex++;

            if (pageIndex == 1 || pageIndex <= People.TotalPages)
            {
                People = await TMDbService.GetTrendingPeopleAsync(TimeWindow.Week, pageIndex);
                return People.Results;
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
                Movies = await TMDbService.GetRatedMoviesAsync(pageIndex);
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
                TvShows = await TMDbService.GetRatedTvShowsAsync(pageIndex);
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
                Movies = await TMDbService.GetMovieWatchlistAsync(pageIndex);
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
                TvShows = await TMDbService.GetTvShowWatchlistAsync(pageIndex);
                return TvShows.Results;
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
                Movies = await TMDbService.GetFavoriteMoviesAsync(pageIndex);
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
                TvShows = await TMDbService.GetFavoriteTvShowsAsync(pageIndex);
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
