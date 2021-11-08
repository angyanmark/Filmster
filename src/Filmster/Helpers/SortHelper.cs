using Filmster.Common.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMDbLib.Objects.People;

namespace Filmster.Helpers
{
    public static class SortHelper
    {
        public static IEnumerable<MovieRole> SortMovieCast(IEnumerable<MovieRole> movieCast, PersonCastCrewSortType sortType)
        {
            switch (sortType)
            {
                case PersonCastCrewSortType.ReleaseDate:
                    movieCast = movieCast.OrderByDescending(c => c.ReleaseDate.HasValue).ThenBy(c => c.ReleaseDate);
                    break;
                case PersonCastCrewSortType.Title:
                    movieCast = movieCast.OrderBy(c => c.Title);
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(sortType), (int)sortType, typeof(PersonCastCrewSortType));
            }
            return movieCast.ToList();
        }

        public static IEnumerable<TvRole> SortTvShowCast(IEnumerable<TvRole> tvShowCast, PersonCastCrewSortType sortType)
        {
            switch (sortType)
            {
                case PersonCastCrewSortType.ReleaseDate:
                    tvShowCast = tvShowCast.OrderByDescending(c => c.FirstAirDate.HasValue).ThenBy(c => c.FirstAirDate);
                    break;
                case PersonCastCrewSortType.Title:
                    tvShowCast = tvShowCast.OrderBy(c => c.Name);
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(sortType), (int)sortType, typeof(PersonCastCrewSortType));
            }
            return tvShowCast.ToList();
        }

        public static IEnumerable<MovieJob> SortMovieCrew(IEnumerable<MovieJob> movieCrew, PersonCastCrewSortType sortType)
        {
            switch (sortType)
            {
                case PersonCastCrewSortType.ReleaseDate:
                    movieCrew = movieCrew.OrderByDescending(c => c.ReleaseDate.HasValue).ThenBy(c => c.ReleaseDate);
                    break;
                case PersonCastCrewSortType.Title:
                    movieCrew = movieCrew.OrderBy(c => c.Title);
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(sortType), (int)sortType, typeof(PersonCastCrewSortType));
            }
            return movieCrew.ToList();
        }

        public static IEnumerable<TvJob> SortTvShowCrew(IEnumerable<TvJob> tvShowCrew, PersonCastCrewSortType sortType)
        {
            switch (sortType)
            {
                case PersonCastCrewSortType.ReleaseDate:
                    tvShowCrew = tvShowCrew.OrderByDescending(c => c.FirstAirDate.HasValue).ThenBy(c => c.FirstAirDate);
                    break;
                case PersonCastCrewSortType.Title:
                    tvShowCrew = tvShowCrew.OrderBy(c => c.Name);
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(sortType), (int)sortType, typeof(PersonCastCrewSortType));
            }
            return tvShowCrew.ToList();
        }
    }
}
