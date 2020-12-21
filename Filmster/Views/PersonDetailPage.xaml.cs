using Filmster.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Filmster.Views
{
    public sealed partial class PersonDetailPage : Page
    {
        public PersonDetailViewModel ViewModel { get; } = new PersonDetailViewModel();

        public PersonDetailPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var id = (int)e.Parameter;
            await ViewModel.LoadPerson(id);
        }
    }
}
