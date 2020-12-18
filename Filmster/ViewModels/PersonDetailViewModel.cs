using Filmster.Core.Services;
using Filmster.Helpers;
using System.Threading.Tasks;
using TMDbLib.Objects.People;

namespace Filmster.ViewModels
{
    public class PersonDetailViewModel : Observable
    {
        private Person _person;
        public Person Person
        {
            get { return _person; }
            set { Set(ref _person, value); }
        }

        public PersonDetailViewModel()
        {
        }

        public async Task LoadPerson(int id)
        {
            Person = await TMDbService.GetPersonAsync(id);
        }
    }
}
