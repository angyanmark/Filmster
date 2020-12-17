using System;

using Filmster.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Filmster.Views
{
    public sealed partial class TvShowsPage : Page
    {
        public TvShowsViewModel ViewModel { get; } = new TvShowsViewModel();

        public TvShowsPage()
        {
            InitializeComponent();
        }
    }
}
