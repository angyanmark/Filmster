using Filmster.Common.Models;
using Filmster.Common.Services;
using Filmster.Extensions;
using Filmster.Helpers;
using Filmster.ViewModelBases;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.System;

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

        public ICommand OpenOriginalClickedCommand;

        public ImageGalleryViewModel()
        {
            OpenOriginalClickedCommand = new RelayCommand<string>(OpenOriginalClickedAsync);
        }

        public void LoadImages(ImageGalleryNavigationParameter parameter)
        {
            ImagePaths.AddRange(parameter.ImagePaths);
            SelectedImagePath = parameter.SelectedImagePath;
        }

        private async void OpenOriginalClickedAsync(string imagePath)
        {
            await Launcher.LaunchUriAsync(new Uri($"{TMDbService.SecureBaseUrl}{TMDbService.OriginalSize}{imagePath}"));
        }
    }
}
