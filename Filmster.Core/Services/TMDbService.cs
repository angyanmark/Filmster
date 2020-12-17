using System.Collections.Generic;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.Search;

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

        public static async Task<List<SearchTv>> GetPopularTvShowsAsync()
        {
            return (await client.GetTvShowPopularAsync()).Results;
        }
    }
}
