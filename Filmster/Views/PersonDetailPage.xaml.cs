using Filmster.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Filmster.Views
{
    public sealed partial class PersonDetailPage : Page
    {
        public PersonDetailViewModel ViewModel { get; } = new PersonDetailViewModel();

        public PersonDetailPage()
        {
            InitializeComponent();
        }
    }
}
