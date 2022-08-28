using Filmster.Helpers;
using Filmster.ViewModelBases;
using Microsoft.Toolkit.Uwp;
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

        public IncrementalLoadingCollection<MultiSearchSource, SearchBase> SearchItems { get; set; } = new IncrementalLoadingCollection<MultiSearchSource, SearchBase>();

        public SearchViewModel()
        {
        }

        public async Task SearchAsync(string searchValue)
        {
            SearchValue = searchValue;
            MultiSearchSource.SearchValue = SearchValue;
            await SearchItems.RefreshAsync();
        }
    }
}
