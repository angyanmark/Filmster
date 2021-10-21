using Filmster.ViewModels;
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
            var id = (int)e.Parameter;
            await ViewModel.LoadList(id);
            ViewModel.DataLoaded = true;
        }
    }
}
