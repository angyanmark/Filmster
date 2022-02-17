﻿using Filmster.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Filmster.Views
{
    public sealed partial class CollectionDetailPage : Page
    {
        public CollectionDetailViewModel ViewModel { get; } = new CollectionDetailViewModel();

        public CollectionDetailPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is int id)
            {
                await ViewModel.LoadCollection(id);
                ViewModel.DataLoaded = true;
            }
        }
    }
}