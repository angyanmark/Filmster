using Filmster.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Filmster.Views
{
    public sealed partial class ReviewDetailPage : Page
    {
        public ReviewDetailViewModel ViewModel { get; } = new ReviewDetailViewModel();

        public ReviewDetailPage() => InitializeComponent();

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is string id)
            {
                await ViewModel.LoadReviewAsync(id);
                ViewModel.DataLoaded = true;
            }
        }
    }
}
