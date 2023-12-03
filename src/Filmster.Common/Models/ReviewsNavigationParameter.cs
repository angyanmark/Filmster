using System;
using TMDbLib.Objects.General;

namespace Filmster.Common.Models
{
    public class ReviewsNavigationParameter
    {
        public string MediaTitle { get; set; }
        public DateTime? MediaReleaseDate { get; set; }
        public MediaType MediaType { get; set; }
        public int Id { get; set; }
    }
}
