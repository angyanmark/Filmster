using TMDbLib.Objects.Search;

namespace Filmster.Helpers
{
    public static class DisplayNameHelper
    {
        public static string GetSearchMovieDisplayName(SearchMovie searchMovie)
        {
            if (searchMovie.ReleaseDate.HasValue)
            {
                return $"{searchMovie.Title} ({searchMovie.ReleaseDate.Value.Year})";
            }
            else
            {
                return searchMovie.Title;
            }
        }

        public static string GetSearchTvDisplayName(SearchTv searchTv)
        {
            if (searchTv.FirstAirDate.HasValue)
            {
                return $"{searchTv.Name} ({searchTv.FirstAirDate.Value.Year})";
            }
            else
            {
                return searchTv.Name;
            }
        }
    }
}
