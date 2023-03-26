using Filmster.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Filmster.Views
{
    public sealed partial class PeoplePage : Page
    {
        public PeopleViewModel ViewModel { get; } = new PeopleViewModel();

        public PeoplePage() => InitializeComponent();
    }
}
