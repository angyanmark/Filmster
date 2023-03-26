using Filmster.Common.Models;
using Filmster.Helpers;
using Filmster.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Filmster.Views
{
    public sealed partial class ReviewsPage : Page
    {
        public ReviewsViewModel ViewModel { get; } = new ReviewsViewModel();

        public ReviewsPage() => InitializeComponent();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is ReviewsNavigationParameter parameter)
            {
                ReviewsSource.MediaType = parameter.MediaType;
                ReviewsSource.Id = parameter.Id;
            }
        }
    }
}
