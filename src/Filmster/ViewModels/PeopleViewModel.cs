using Filmster.Common.Services;
using Filmster.Helpers;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.Trending;

namespace Filmster.ViewModels
{
    public class PeopleViewModel : MediaViewModelBase
    {
        public ObservableCollection<SearchPerson> TrendingPeople { get; set; } = new ObservableCollection<SearchPerson>();

        public PeopleViewModel()
        {
            _ = GetPeopleAsync();
        }

        private async Task GetPeopleAsync()
        {
            await GetTrendingPeopleAsync();
        }

        private async Task GetTrendingPeopleAsync()
        {
            var people = await TMDbService.GetTrendingPeopleAsync(TimeWindow.Week);
            foreach (var person in people)
            {
                TrendingPeople.Add(person);
            }
        }
    }
}
