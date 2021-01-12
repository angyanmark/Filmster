using System;
using TMDbLib.Objects.Discover;

namespace Filmster.Core.Models
{
    public class DiscoverMovieOptions
    {
        public DateTime PrimaryReleaseDateAfter { get; set; }
        public DateTime PrimaryReleaseDateBefore { get; set; }
        public double VoteAverageAtLeast { get; set; }
        public double VoteAverageAtMost { get; set; }
        public int VoteCountAtLeast { get; set; }
        public int GenreId { get; set; }
        public DiscoverMovieSortBy SortBy { get; set; }
    }
}
