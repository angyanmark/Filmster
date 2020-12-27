using Filmster.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Filmster.Views
{
    public sealed partial class DiscoverPage : Page
    {
        public DiscoverViewModel ViewModel { get; } = new DiscoverViewModel();

        public DiscoverPage()
        {
            InitializeComponent();
        }
    }
}
