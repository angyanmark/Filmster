using TMDbLib.Objects.Search;

namespace Filmster.Common.Models.Extensions
{
    public class TvSeasonEpisodeWithRating : TvSeasonEpisode
    {
        public double? Rating { get; set; }
    }
}
