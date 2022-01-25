using Filmster.Common.Helpers;
using Filmster.Common.Services;
using Filmster.Dialogs;
using Filmster.Extensions;
using Filmster.Helpers;
using Filmster.Services;
using Filmster.ViewModelBases;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TMDbLib.Objects.Lists;
using TMDbLib.Objects.Search;
using Windows.UI.Xaml.Controls;

namespace Filmster.ViewModels
{
    public class ListDetailViewModel : MediaViewModelBase
    {
        private GenericList _genericList;
        public GenericList GenericList
        {
            get { return _genericList; }
            set { Set(ref _genericList, value); }
        }

        private double _voteAverage;
        public double VoteAverage
        {
            get { return _voteAverage; }
            set { Set(ref _voteAverage, value); }
        }

        private int _voteCount;
        public int VoteCount
        {
            get { return _voteCount; }
            set { Set(ref _voteCount, value); }
        }

        public ObservableCollection<SearchBase> Items { get; set; } = new ObservableCollection<SearchBase>();

        public ICommand ListAddClickedCommand;
        public ICommand ListClearClickedCommand;
        public ICommand ListDeleteClickedCommand;
        public ICommand ListRemoveClickedCommand;

        public ListDetailViewModel()
        {
            ListAddClickedCommand = new RelayCommand(ListAddClickedAsync);
            ListClearClickedCommand = new RelayCommand(ListClearClickedAsync);
            ListDeleteClickedCommand = new RelayCommand(ListDeleteClickedAsync);
            ListRemoveClickedCommand = new RelayCommand<SearchMovie>(ListRemoveClickedAsync);
        }

        public async Task LoadList(int id)
        {
            GenericList = await TMDbService.GetListAsync(id);
            if (GenericList == null)
            {
                NavigationService.GoBack();
                return;
            }
            Items.AddRange(GenericList.Items);
            SetVoteAverageVoteCount();
        }

        private void SetVoteAverageVoteCount()
        {
            (VoteAverage, VoteCount) = VoteHelper.GetVoteAverageVoteCount(Items.Select(item => ((item as SearchMovieTvBase).VoteAverage, (item as SearchMovieTvBase).VoteCount)));
        }

        private async void ListAddClickedAsync()
        {
            var dialog = new ListAddContentDialog(GenericList.Id);
            await dialog.ShowAsync();

            if (dialog.AddedMovie != null)
            {
                Items.Add(dialog.AddedMovie);
                SetVoteAverageVoteCount();
            }
        }

        private async void ListClearClickedAsync()
        {
            var dialog = new ContentDialog
            {
                Title = "ConfirmDialog_Clear".GetLocalized(),
                Content = "ConfirmDialog_Clear_Content".GetLocalized(),
                CloseButtonText = "ConfirmDialog_Close".GetLocalized(),
                PrimaryButtonText = "ConfirmDialog_Clear".GetLocalized(),
                DefaultButton = ContentDialogButton.Primary,
            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var success = await TMDbService.ListClearAsync(GenericList.Id);
                if (success)
                {
                    Items.Clear();
                    SetVoteAverageVoteCount();
                }
            }
        }

        private async void ListDeleteClickedAsync()
        {
            var dialog = new ContentDialog
            {
                Title = "ConfirmDialog_Delete".GetLocalized(),
                Content = "ConfirmDialog_Delete_Content".GetLocalized(),
                CloseButtonText = "ConfirmDialog_Close".GetLocalized(),
                PrimaryButtonText = "ConfirmDialog_Delete".GetLocalized(),
                DefaultButton = ContentDialogButton.Primary,
            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var success = await TMDbService.ListDeleteAsync(GenericList.Id);
                if (success)
                {
                    NavigationService.GoBack();
                }
            }
        }

        private async void ListRemoveClickedAsync(SearchMovie item)
        {
            var success = await TMDbService.ListRemoveMovieAsync(GenericList.Id, item.Id);
            if (success)
            {
                Items.Remove(item);
                SetVoteAverageVoteCount();
            }
        }
    }
}
