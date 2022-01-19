using Filmster.Common.Services;
using Filmster.Dialogs;
using Filmster.Extensions;
using Filmster.Helpers;
using Filmster.Services;
using Filmster.ViewModelBases;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TMDbLib.Objects.Lists;
using TMDbLib.Objects.Search;

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
        }

        private async void ListAddClickedAsync()
        {
            var dialog = new ListAddContentDialog(GenericList.Id);
            await dialog.ShowAsync();

            if (dialog.AddedMovie != null)
            {
                Items.Add(dialog.AddedMovie);
            }
        }

        private async void ListClearClickedAsync()
        {
            var success = await TMDbService.ListClearAsync(GenericList.Id);
            if (success)
            {
                Items.Clear();
            }
        }

        private async void ListDeleteClickedAsync()
        {
            var success = await TMDbService.ListDeleteAsync(GenericList.Id);
            if (success)
            {
                NavigationService.GoBack();
            }
        }

        private async void ListRemoveClickedAsync(SearchMovie item)
        {
            var success = await TMDbService.ListRemoveMovieAsync(GenericList.Id, item.Id);
            if (success)
            {
                Items.Remove(item);
            }
        }
    }
}
