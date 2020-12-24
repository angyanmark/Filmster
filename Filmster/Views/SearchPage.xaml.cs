using Filmster.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Filmster.Views
{
    public sealed partial class SearchPage : Page
    {
        public SearchViewModel ViewModel { get; } = new SearchViewModel();

        public SearchPage()
        {
            InitializeComponent();
        }
    }
}
