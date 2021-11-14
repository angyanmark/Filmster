using Filmster.Common.Models;
using Filmster.Extensions;
using Filmster.ViewModelBases;
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
            ImagePaths.AddRange(parameter.ImagePaths);
            SelectedImagePath = parameter.SelectedImagePath;
        }
    }
}
