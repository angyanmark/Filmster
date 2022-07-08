using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace Filmster.Helpers
{
    public static class DisplayNameHelper
    {
        public static string GetSearchBaseDisplayName(SearchBase searchBase)
        {
            switch (searchBase.MediaType)
            {
                case MediaType.Movie:
                    return GetSearchMovieDisplayName(searchBase as SearchMovie);
                case MediaType.Tv:
                    return GetSearchTvDisplayName(searchBase as SearchTv);
                case MediaType.Person:
                    return (searchBase as SearchPerson).Name;
                default:
                    return string.Empty;
            }
        }

        public static string GetKnownForBaseDisplayName(KnownForBase knownForBase)
        {
            switch (knownForBase.MediaType)
            {
                case MediaType.Movie:
                    return GetKnownForMovieDisplayName(knownForBase as KnownForMovie);
                case MediaType.Tv:
                    return GetKnownForTvDisplayName(knownForBase as KnownForTv);
                default:
                    return string.Empty;
            }
        }

        private static string GetSearchMovieDisplayName(SearchMovie movie)
        {
            return movie.ReleaseDate.HasValue
                ? $"{movie.Title} ({movie.ReleaseDate.Value.Year})"
                : movie.Title;
        }

        private static string GetSearchTvDisplayName(SearchTv tv)
        {
            return tv.FirstAirDate.HasValue
                ? $"{tv.Name} ({tv.FirstAirDate.Value.Year})"
                : tv.Name;
        }

        private static string GetKnownForMovieDisplayName(KnownForMovie movie)
        {
            return movie.ReleaseDate.HasValue
                ? $"{movie.Title} ({movie.ReleaseDate.Value.Year})"
                : movie.Title;
        }

        private static string GetKnownForTvDisplayName(KnownForTv tv)
        {
            return tv.FirstAirDate.HasValue
                ? $"{tv.Name} ({tv.FirstAirDate.Value.Year})"
                : tv.Name;
        }
    }
}
