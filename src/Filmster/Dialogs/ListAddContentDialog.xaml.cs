using Filmster.Common.Helpers;
using Filmster.Common.Services;
using Filmster.Helpers;
using Microsoft.Toolkit.Uwp;
using System;
using System.Windows.Input;
using TMDbLib.Objects.Exceptions;
using TMDbLib.Objects.Search;
using Windows.UI.Xaml.Controls;

namespace Filmster.Dialogs
{
    public sealed partial class ListAddContentDialog : ContentDialog
    {
        private string ListId { get; set; }
        private IncrementalLoadingCollection<SearchMovieSource, SearchMovie> Movies { get; set; } = new IncrementalLoadingCollection<SearchMovieSource, SearchMovie>();
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
                SearchMovieSource.SearchValue = value;
                await Movies.RefreshAsync();
            }
        }
    }
}
