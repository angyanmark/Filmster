using System.Collections.Generic;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.Trending;
using TMDbLib.Objects.TvShows;

namespace Filmster.Core.Services
{
    public static class TMDbService
    {
        private static readonly TMDbClient client = new TMDbClient("5e9bcb638329a15acf75c1b2d85ae67e");

        public static readonly string SecureBaseUrl = "https://image.tmdb.org/t/p/";
        public static readonly string BackdropSize = "w780";
        public static readonly string LogoSize = "w300";
        public static readonly string PosterSize = "w342";
        public static readonly string ProfileSize = "w185";
        public static readonly string StillSize = "w300";

        public static readonly string TMDbMovieBaseUrl = "https://www.themoviedb.org/movie/";
        public static readonly string TMDbTvShowBaseUrl = "https://www.themoviedb.org/tv/";
        public static readonly string TMDbPersonBaseUrl = "https://www.themoviedb.org/person/";
        public static readonly string IMDbMovieBaseUrl = "https://www.imdb.com/title/";
        public static readonly string IMDbTvShowBaseUrl = "https://www.imdb.com/title/";
        public static readonly string IMDbPersonBaseUrl = "https://www.imdb.com/name/";
        public static readonly string YouTubeBaseUrl = "https://www.youtube.com/watch?v=";
        public static readonly string FacebookBaseUrl = "https://www.facebook.com/";
        public static readonly string TwitterBaseUrl = "https://twitter.com/";
        public static readonly string InstagramBaseUrl = "https://www.instagram.com/";

        public static async Task<List<SearchMovie>> GetPopularMoviesAsync()
        {
            return (await client.GetMoviePopularListAsync()).Results;
        }

        public static async Task<List<SearchMovie>> GetNowPlayingMoviesAsync()
        {
            return (await client.GetMovieNowPlayingListAsync()).Results;
        }

        public static async Task<List<SearchMovie>> GetUpcomingMoviesAsync()
        {
            return (await client.GetMovieUpcomingListAsync()).Results;
        }

        public static async Task<List<SearchMovie>> GetTopRatedMoviesAsync()
        {
            return (await client.GetMovieTopRatedListAsync()).Results;
        }

        public static async Task<Movie> GetMovieAsync(int id)
        {
            return await client.GetMovieAsync(id, MovieMethods.Images | MovieMethods.Videos | MovieMethods.ExternalIds);
        }

        public static async Task<List<SearchTv>> GetPopularTvShowsAsync()
        {
            return (await client.GetTvShowPopularAsync()).Results;
        }

        public static async Task<List<SearchTv>> GetTopRatedTvShowsAsync()
        {
            return (await client.GetTvShowTopRatedAsync()).Results;
        }

        public static async Task<TvShow> GetTvShowAsync(int id)
        {
            return await client.GetTvShowAsync(id, TvShowMethods.Images);
        }

        public static async Task<List<SearchPerson>> GetPopularPeopleAsync(TimeWindow timeWindow)
        {
            return (await client.GetTrendingPeopleAsync(timeWindow)).Results;
        }

        public static async Task<Person> GetPersonAsync(int id)
        {
            return await client.GetPersonAsync(id, PersonMethods.Images);
        }

        public static async Task<List<SearchBase>> GetMultiSearchAsync(string value)
        {
            return (await client.SearchMultiAsync(value)).Results;
        }
    }
}
