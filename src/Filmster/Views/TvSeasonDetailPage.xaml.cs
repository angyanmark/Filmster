using Filmster.Common.Models;
using Filmster.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Filmster.Views
{
    public sealed partial class TvSeasonDetailPage : Page
    {
        public TvSeasonDetailViewModel ViewModel { get; } = new TvSeasonDetailViewModel();

        public TvSeasonDetailPage() => InitializeComponent();

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is TvShowSeasonEpisodeNumbers tvShowSeasonEpisodeNumbers)
            {
                await ViewModel.LoadTvSeasonAsync(tvShowSeasonEpisodeNumbers);
                ViewModel.DataLoaded = true;
            }
        }
    }
}
