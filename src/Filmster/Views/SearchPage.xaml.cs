using Filmster.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Filmster.Views
{
    public sealed partial class SearchPage : Page
    {
        public SearchViewModel ViewModel { get; } = new SearchViewModel();

        public SearchPage() => InitializeComponent();

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is string searchValue)
            {
                await ViewModel.SearchAsync(searchValue);
                ViewModel.DataLoaded = true;
            }
        }
    }
}
