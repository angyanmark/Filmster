using Filmster.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Filmster.Views
{
    public sealed partial class TvShowDetailPage : Page
    {
        public TvShowDetailViewModel ViewModel { get; } = new TvShowDetailViewModel();

        public TvShowDetailPage()
        {
            InitializeComponent();
        }
    }
}
