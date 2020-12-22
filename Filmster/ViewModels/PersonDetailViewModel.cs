using Filmster.Core.Services;
using Filmster.Helpers;
using Filmster.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;

namespace Filmster.ViewModels
{
    public class PersonDetailViewModel : LoadingObservable
    {
        private Person _person;
        public Person Person
        {
            get { return _person; }
            set { Set(ref _person, value); }
        }

        private ImageData _selectedPoster;
        public ImageData SelectedPoster
        {
            get { return _selectedPoster; }
            set { Set(ref _selectedPoster, value); }
        }

        public ObservableCollection<TaggedImage> Images { get; set; } = new ObservableCollection<TaggedImage>();

        private bool _isImagesChecked;
        public bool IsImagesChecked
        {
            get { return _isImagesChecked; }
            set
            {
                Set(ref _isImagesChecked, value);
                ImagesToggled(IsImagesChecked);
            }
        }

        public PersonDetailViewModel()
        {
        }

        public async Task LoadPerson(int id)
        {
            Person = await TMDbService.GetPersonAsync(id);
            if (Person == null)
            {
                NavigationService.GoBack();
                return;
            }
            SelectedPoster = GetSelectedPoster();
            ImagesToggled(false);
        }

        private ImageData GetSelectedPoster()
        {
            return Person.Images.Profiles.Find(poster => poster.FilePath == Person.ProfilePath) ?? Person.Images.Profiles.FirstOrDefault();
        }

        private void ImagesToggled(bool isChecked)
        {
            var images = isChecked ? Person.TaggedImages.Results : Person.TaggedImages.Results.Take(TMDbService.DefaultCastCrewBackdropCount);
            Images.Clear();
            foreach (var i in images)
            {
                Images.Add(i);
            }
        }
    }
}
