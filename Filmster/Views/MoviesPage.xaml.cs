using System;

using Filmster.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Filmster.Views
{
    public sealed partial class MoviesPage : Page
    {
        public MoviesViewModel ViewModel { get; } = new MoviesViewModel();

        public MoviesPage()
        {
            InitializeComponent();
        }
    }
}
