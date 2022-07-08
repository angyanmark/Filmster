using Filmster.Common.Services;
using Filmster.Extensions;
using Filmster.ViewModelBases;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TMDbLib.Objects.Search;

namespace Filmster.ViewModels
{
    public class SearchViewModel : MediaViewModelBase
    {
        private string _searchValue;
        public string SearchValue
        {
            get { return _searchValue; }
            set { Set(ref _searchValue, value); }
        }

        public ObservableCollection<SearchBase> SearchItems { get; set; } = new ObservableCollection<SearchBase>();

        public SearchViewModel()
        {
        }

        public async Task SearchAsync(string searchValue)
        {
            SearchValue = searchValue;
            var searchItems = await TMDbService.GetMultiSearchAsync(searchValue);
            SearchItems.AddRange(searchItems);
        }
    }
}
