using System.Collections.Generic;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.Search;

namespace Filmster.Core.Services
{
    public static class TMDbService
    {
        private static readonly TMDbClient client = new TMDbClient("5e9bcb638329a15acf75c1b2d85ae67e");

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
    }
}
