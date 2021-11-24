using Filmster.ViewModels;
using TMDbLib.Objects.Search;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Filmster.Views
{
    public sealed partial class ListDetailPage : Page
    {
        public ListDetailViewModel ViewModel { get; } = new ListDetailViewModel();

        public ListDetailPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is int id)
            {
                await ViewModel.LoadList(id);
                ViewModel.DataLoaded = true;
            }
        }

        private void DeleteMenuFlyoutItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.ListRemoveClickedCommand.Execute((sender as MenuFlyoutItem).DataContext as SearchMovie);
        }
    }
}
