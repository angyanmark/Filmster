﻿using Filmster.Core.Models;
using Filmster.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Filmster.Views
{
    public sealed partial class TvSeasonDetailPage : Page
    {
        public TvSeasonDetailViewModel ViewModel { get; } = new TvSeasonDetailViewModel();

        public TvSeasonDetailPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var parameter = e.Parameter as TvSeasonNavigationParameter;
            await ViewModel.LoadTvSeason(parameter.TvShowId, parameter.SeasonNumber);
            ViewModel.DataLoaded = true;
        }
    }
}
