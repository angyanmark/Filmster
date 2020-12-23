using Filmster.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Filmster.Views
{
    public sealed partial class TvSeasonDetailPage : Page
    {
        public TvSeasonDetailViewModel ViewModel { get; } = new TvSeasonDetailViewModel();

        public TvSeasonDetailPage()
        {
            InitializeComponent();
        }
    }
}
