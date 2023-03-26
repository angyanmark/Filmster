using System.Collections.Generic;
using System.Linq;
using TMDbLib.Objects.General;

namespace Filmster.Services
{
    public static class VideoSelectService
    {
        private const string YouTube = "YouTube";
        private const string Trailer = "Trailer";

        public static Video SelectVideo(List<Video> videos) =>
            videos.LastOrDefault(video => video.Site == YouTube && video.Type == Trailer) ??
            videos.LastOrDefault();
    }
}
