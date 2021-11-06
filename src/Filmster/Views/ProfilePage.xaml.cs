using Filmster.Core.Models;
using Filmster.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Filmster.Views
{
    public sealed partial class ProfilePage : Page
    {
        public ProfileViewModel ViewModel { get; } = new ProfileViewModel();

        public ProfilePage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var parameter = e.Parameter as WatchlistActivationNavigationParameter;
            ViewModel.PrimaryPivotSelectedIndex = parameter?.PrimaryPivotIndex ?? 0;
            ViewModel.WatchlistPivotSelectedIndex = parameter?.WatchlistPivotIndex ?? 0;
            await ViewModel.LoadProfile();
            ViewModel.DataLoaded = true;
        }
    }
}
