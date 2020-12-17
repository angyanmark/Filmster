using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace Filmster.Core.Services
{
    public static class TMDbService
    {
        private static readonly TMDbClient client = new TMDbClient("5e9bcb638329a15acf75c1b2d85ae67e");

        public static SearchContainer<SearchMovie> GetPopularMovies()
        {
            return client.GetMoviePopularListAsync().Result;
        }
    }
}
