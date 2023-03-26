using Filmster.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Filmster.Views
{
    public sealed partial class TvShowDetailPage : Page
    {
        public TvShowDetailViewModel ViewModel { get; } = new TvShowDetailViewModel();

        public TvShowDetailPage() => InitializeComponent();

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is int id)
            {
                await ViewModel.LoadTvShowAsync(id);
                ViewModel.DataLoaded = true;
            }
        }
    }
}
