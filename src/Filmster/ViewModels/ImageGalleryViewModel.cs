using Filmster.Common.Models;
using Filmster.Helpers;
using System.Collections.ObjectModel;

namespace Filmster.ViewModels
{
    public class ImageGalleryViewModel : Observable
    {
        public ObservableCollection<string> ImagePaths { get; set; } = new ObservableCollection<string>();

        private string _selectedImagePath;
        public string SelectedImagePath
        {
            get { return _selectedImagePath; }
            set { Set(ref _selectedImagePath, value); }
        }

        public ImageGalleryViewModel()
        {
        }

        public void LoadImages(ImageGalleryNavigationParameter parameter)
        {
            foreach (var path in parameter.ImagePaths)
            {
                ImagePaths.Add(path);
            }

            SelectedImagePath = parameter.SelectedImagePath;
        }
    }
}
