using Filmster.Common.Helpers;
using Filmster.Common.Services;
using Filmster.Extensions;
using Filmster.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TMDbLib.Objects.Exceptions;
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
            try
            {
                var success = await TMDbService.ListAddMovieAsync(ListId, movie.Id);
                if (success)
                {
                    AddedMovie = movie;
                    Hide();
                }
            }
            catch (GeneralHttpException exception)
            {
                Hide();

                var dialog = new ContentDialog
                {
                    Title = "ContentDialog_AddToList_Failed".GetLocalized(),
                    Content = exception.Message,
                    CloseButtonText = "ContentDialog_Ok".GetLocalized(),
                };

                await dialog.ShowAsync();
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
