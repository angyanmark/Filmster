using Filmster.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Filmster.Views
{
    public sealed partial class TvShowDetailPage : Page
    {
        public TvShowDetailViewModel ViewModel { get; } = new TvShowDetailViewModel();

        public TvShowDetailPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var id = (int)e.Parameter;
            await ViewModel.LoadTvShow(id);
            base.OnNavigatedTo(e);
        }
    }
}
