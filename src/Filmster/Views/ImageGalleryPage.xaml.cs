﻿using Filmster.Common.Models;
using Filmster.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Filmster.Views
{
    public sealed partial class ImageGalleryPage : Page
    {
        public ImageGalleryViewModel ViewModel { get; } = new ImageGalleryViewModel();

        public ImageGalleryPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is ImageGalleryNavigationParameter parameter)
            {
                ViewModel.LoadImages(parameter);
            }
        }
    }
}
