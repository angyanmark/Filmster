using Filmster.Common.Services;
using Filmster.Extensions;
using Filmster.Helpers;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TMDbLib.Objects.Search;
using Windows.UI.Xaml.Controls;

namespace Filmster.Dialogs
{
    public sealed partial class ListAddContentDialog : ContentDialog
    {
        private string ListId { get; set; }
        private ObservableCollection<SearchMovie> Movies { get; set; } = new ObservableCollection<SearchMovie>();
        public SearchMovie AddedMovie { get; set; }

        private readonly ICommand MovieClickedCommand;

        public ListAddContentDialog(string listId)
        {
            InitializeComponent();

            ListId = listId;
            MovieClickedCommand = new RelayCommand<SearchMovie>(MovieClickedAsync);
        }

        private async void MovieClickedAsync(SearchMovie movie)
        {
            var success = await TMDbService.ListAddMovieAsync(ListId, movie.Id);
            if (success)
            {
                AddedMovie = movie;
                Hide();
            }
        }

        private async void SearchBox_TextChangedAsync(object sender, TextChangedEventArgs e)
        {
            var value = (sender as TextBox).Text;
            if (!string.IsNullOrWhiteSpace(value))
            {
                var movies = await TMDbService.GetSearchMovieAsync(value);
                Movies.Reset(movies);
            }
        }
    }
}
