using Filmster.Core.Services;
using Filmster.Helpers;
using Filmster.Services;
using Filmster.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.Trending;

namespace Filmster.ViewModels
{
    public class PeopleViewModel : Observable
    {
        public ObservableCollection<SearchPerson> PopularPeople { get; set; } = new ObservableCollection<SearchPerson>();

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

        private void PersonClicked(SearchPerson person)
        {
            NavigationService.Navigate(typeof(PersonDetailPage), person.Id);
        }
    }
}
