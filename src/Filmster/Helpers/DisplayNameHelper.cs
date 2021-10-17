using TMDbLib.Objects.Search;

namespace Filmster.Helpers
{
    public static class DisplayNameHelper
    {
        public static string GetSearchMovieDisplayName(SearchMovie searchMovie)
        {
            return searchMovie.ReleaseDate.HasValue
                ? $"{searchMovie.Title} ({searchMovie.ReleaseDate.Value.Year})"
                : searchMovie.Title;
        }

        public static string GetSearchTvDisplayName(SearchTv searchTv)
        {
            return searchTv.FirstAirDate.HasValue
                ? $"{searchTv.Name} ({searchTv.FirstAirDate.Value.Year})"
                : searchTv.Name;
        }
    }
}
