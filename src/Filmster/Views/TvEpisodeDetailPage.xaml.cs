using Filmster.Common.Models;
using Filmster.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Filmster.Views
{
    public sealed partial class TvEpisodeDetailPage : Page
    {
        public TvEpisodeDetailViewModel ViewModel { get; } = new TvEpisodeDetailViewModel();

        public TvEpisodeDetailPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is TvShowSeasonEpisodeNumbers tvShowSeasonEpisodeNumbers)
            {
                await ViewModel.LoadTvEpisodeAsync(tvShowSeasonEpisodeNumbers);
                ViewModel.DataLoaded = true;
            }
        }
    }
}
