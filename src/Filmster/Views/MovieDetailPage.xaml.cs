using Filmster.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Filmster.Views
{
    public sealed partial class MovieDetailPage : Page
    {
        public MovieDetailViewModel ViewModel { get; } = new MovieDetailViewModel();

        public MovieDetailPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var id = (int)e.Parameter;
            await ViewModel.LoadMovie(id);
            ViewModel.DataLoaded = true;
        }
    }
}
