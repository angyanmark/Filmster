using Filmster.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Filmster.Views
{
    public sealed partial class MovieDetailPage : Page
    {
        public MovieDetailViewModel ViewModel { get; } = new MovieDetailViewModel();

        public MovieDetailPage()
        {
            InitializeComponent();
        }
    }
}
