using Filmster.Core.Services;
using Filmster.Helpers;
using Filmster.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.Trending;

namespace Filmster.ViewModels
{
    public class PeopleViewModel : Observable
    {
        public ObservableCollection<SearchPerson> TrendingPeople { get; set; } = new ObservableCollection<SearchPerson>();

        public ICommand PersonClickedCommand;

        public PeopleViewModel()
        {
            _ = GetPeopleAsync();
            SetCommands();
        }

        private void SetCommands()
        {
            PersonClickedCommand = new RelayCommand<SearchPerson>(PersonClicked);
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

        private void PersonClicked(SearchPerson person)
        {
            NavigationService.NavigateToSearchMediaDetail(person);
        }
    }
}
