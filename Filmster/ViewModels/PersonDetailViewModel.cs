using Filmster.Core.Models;
using Filmster.Core.Services;
using Filmster.Helpers;
using Filmster.Services;
using Filmster.Views;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;

namespace Filmster.ViewModels
{
    public class PersonDetailViewModel : MediaViewModelBase
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

        public ObservableCollection<MovieRole> MovieCast { get; set; } = new ObservableCollection<MovieRole>();
        public ObservableCollection<TvRole> TvShowCast { get; set; } = new ObservableCollection<TvRole>();
        public ObservableCollection<MovieJob> MovieCrew { get; set; } = new ObservableCollection<MovieJob>();
        public ObservableCollection<TvJob> TvShowCrew { get; set; } = new ObservableCollection<TvJob>();
        public ObservableCollection<TaggedImage> Images { get; set; } = new ObservableCollection<TaggedImage>();

        private bool _isCastChecked;
        public bool IsCastChecked
        {
            get { return _isCastChecked; }
            set
            {
                Set(ref _isCastChecked, value);
                CastToggled(IsCastChecked);
            }
        }

        private bool _isCrewChecked;
        public bool IsCrewChecked
        {
            get { return _isCrewChecked; }
            set
            {
                Set(ref _isCrewChecked, value);
                CrewToggled(IsCrewChecked);
            }
        }

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

        public ICommand ImageClickedCommand;

        public PersonDetailViewModel()
        {
            ImageClickedCommand = new RelayCommand<TaggedImage>(ImageClicked);
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
            CastToggled(false);
            CrewToggled(false);
            ImagesToggled(false);
        }

        private ImageData GetSelectedPoster()
        {
            return Person.Images.Profiles.Find(poster => poster.FilePath == Person.ProfilePath) ?? Person.Images.Profiles.FirstOrDefault();
        }

        private void CastToggled(bool isChecked)
        {
            var movieCast = isChecked ? Person.MovieCredits.Cast : Person.MovieCredits.Cast.Take(TMDbService.DefaultCastCrewBackdropCount);
            var tvShowCast = isChecked ? Person.TvCredits.Cast : Person.TvCredits.Cast.Take(TMDbService.DefaultCastCrewBackdropCount);
            MovieCast.Clear();
            TvShowCast.Clear();
            foreach (var c in movieCast)
            {
                MovieCast.Add(c);
            }
            foreach (var c in tvShowCast)
            {
                TvShowCast.Add(c);
            }
        }

        private void CrewToggled(bool isChecked)
        {
            var movieCrew = isChecked ? Person.MovieCredits.Crew : Person.MovieCredits.Crew.Take(TMDbService.DefaultCastCrewBackdropCount);
            var tvShowCrew = isChecked ? Person.TvCredits.Crew : Person.TvCredits.Crew.Take(TMDbService.DefaultCastCrewBackdropCount);
            MovieCrew.Clear();
            TvShowCrew.Clear();
            foreach (var c in movieCrew)
            {
                MovieCrew.Add(c);
            }
            foreach (var c in tvShowCrew)
            {
                TvShowCrew.Add(c);
            }
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

        private void ImageClicked(TaggedImage selectedImage)
        {
            var paths = Person.TaggedImages.Results.Select(image => image.FilePath);
            var selectedPath = selectedImage.FilePath;

            NavigationService.Navigate<ImageGalleryPage>(new ImageGalleryNavigationParameter
            {
                ImagePaths = paths,
                SelectedImagePath = selectedPath
            });
        }
    }
}
