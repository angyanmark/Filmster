using System;
using TMDbLib.Objects.Discover;

namespace Filmster.Common.Models
{
    public class DiscoverTvShowOptions
    {
        public DateTime FirstAirDateAfter { get; set; }
        public DateTime FirstAirDateBefore { get; set; }
        public double VoteAverageAtLeast { get; set; }
        public double VoteAverageAtMost { get; set; }
        public int VoteCountAtLeast { get; set; }
        public int GenreId { get; set; }
        public DiscoverTvShowSortBy SortBy { get; set; }
    }
}
