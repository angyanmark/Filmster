using Filmster.Common.Services;
using Filmster.Helpers;
using Filmster.Services;
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

        public async Task Search(string searchValue)
        {
            var searchItems = await TMDbService.GetMultiSearchAsync(searchValue, await IncludeAdultService.LoadIncludeAdultAsync());
            foreach (var searchItem in searchItems)
            {
                SearchItems.Add(searchItem);
            }
        }
    }
}
