using System.Collections.Generic;

namespace Filmster.Core.Models
{
    public class ImageGalleryNavigationParameter
    {
        public IEnumerable<string> ImagePaths { get; set; }
        public string SelectedImagePath { get; set; }
    }
}
