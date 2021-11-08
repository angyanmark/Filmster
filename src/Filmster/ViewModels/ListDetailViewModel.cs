using Filmster.Common.Services;
using Filmster.Services;
using Filmster.ViewModelBases;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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

        public ObservableCollection<SearchMovie> Items { get; set; } = new ObservableCollection<SearchMovie>();

        public ListDetailViewModel()
        {
        }

        public async Task LoadList(int id)
        {
            GenericList = await TMDbService.GetListAsync(id);
            if (GenericList == null)
            {
                NavigationService.GoBack();
                return;
            }

            foreach (var item in GenericList.Items)
            {
                Items.Add(item);
            }
        }
    }
}
