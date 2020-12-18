using Filmster.Core.Services;
using Filmster.Helpers;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.Trending;

namespace Filmster.ViewModels
{
    public class PeopleViewModel : Observable
    {
        public ObservableCollection<SearchPerson> PopularPeople { get; set; } = new ObservableCollection<SearchPerson>();

        public PeopleViewModel()
        {
            _ = GetPeopleAsync();
        }

        private async Task GetPeopleAsync()
        {
            await GetPopularPeopleAsync();
        }

        private async Task GetPopularPeopleAsync()
        {
            var people = await TMDbService.GetPopularPeopleAsync(TimeWindow.Week);
            foreach (var person in people)
            {
                PopularPeople.Add(person);
            }
        }
    }
}
