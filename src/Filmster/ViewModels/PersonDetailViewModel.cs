using Filmster.Common.Models;
using Filmster.Common.Models.Enums;
using Filmster.Common.Services;
using Filmster.Helpers;
using Filmster.Services;
using Filmster.ViewModelBases;
using Filmster.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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

        private PersonCastCrewSortType _selectedCastSortType = PersonCastCrewSortType.ReleaseDate;
        public PersonCastCrewSortType SelectedCastSortType
        {
            get { return _selectedCastSortType; }
            set
            {
                if (value != SelectedCastSortType)
                {
                    Set(ref _selectedCastSortType, value);
                    CastSortTypeChanged(SelectedCastSortType);
                }
            }
        }

        private PersonCastCrewSortType _selectedCrewSortType = PersonCastCrewSortType.ReleaseDate;
        public PersonCastCrewSortType SelectedCrewSortType
        {
            get { return _selectedCrewSortType; }
            set
            {
                if (value != SelectedCrewSortType)
                {
                    Set(ref _selectedCrewSortType, value);
                    CrewSortTypeChanged(SelectedCrewSortType);
                }
            }
        }

        public ObservableCollection<MovieRole> MovieCast { get; set; } = new ObservableCollection<MovieRole>();
        public ObservableCollection<TvRole> TvShowCast { get; set; } = new ObservableCollection<TvRole>();
        public ObservableCollection<MovieJob> MovieCrew { get; set; } = new ObservableCollection<MovieJob>();
        public ObservableCollection<TvJob> TvShowCrew { get; set; } = new ObservableCollection<TvJob>();
        public ObservableCollection<TaggedImage> Images { get; set; } = new ObservableCollection<TaggedImage>();
        public IEnumerable<PersonCastCrewSortType> SortTypes { get; set; } = Enum.GetValues(typeof(PersonCastCrewSortType)).Cast<PersonCastCrewSortType>();

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

        private void CastSortTypeChanged(PersonCastCrewSortType sortType)
        {
            var movieCast = SortHelper.SortMovieCast(MovieCast, sortType);
            var tvShowCast = SortHelper.SortTvShowCast(TvShowCast, sortType);
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

        private void CrewSortTypeChanged(PersonCastCrewSortType sortType)
        {
            var movieCrew = SortHelper.SortMovieCrew(MovieCrew, sortType);
            var tvShowCrew = SortHelper.SortTvShowCrew(TvShowCrew, sortType);
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

        private ImageData GetSelectedPoster()
        {
            return Person.Images.Profiles.Find(poster => poster.FilePath == Person.ProfilePath) ?? Person.Images.Profiles.FirstOrDefault();
        }

        private void CastToggled(bool isChecked)
        {
            var movieCast = SortHelper.SortMovieCast(Person.MovieCredits.Cast, SelectedCastSortType);
            var tvShowCast = SortHelper.SortTvShowCast(Person.TvCredits.Cast, SelectedCastSortType);
            movieCast = isChecked ? movieCast : movieCast.Take(TMDbService.DefaultCastCrewBackdropCount);
            tvShowCast = isChecked ? tvShowCast : tvShowCast.Take(TMDbService.DefaultCastCrewBackdropCount);
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
            var movieCrew = SortHelper.SortMovieCrew(Person.MovieCredits.Crew, SelectedCrewSortType);
            var tvShowCrew = SortHelper.SortTvShowCrew(Person.TvCredits.Crew, SelectedCrewSortType);
            movieCrew = isChecked ? movieCrew : movieCrew.Take(TMDbService.DefaultCastCrewBackdropCount);
            tvShowCrew = isChecked ? tvShowCrew : tvShowCrew.Take(TMDbService.DefaultCastCrewBackdropCount);
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

        public void PosterClicked(object sender, TappedRoutedEventArgs e)
        {
            if (e.OriginalSource is Image)
            {
                var paths = Person.Images.Profiles.Select(image => image.FilePath);
                var selectedPath = SelectedPoster.FilePath;

                NavigationService.Navigate<ImageGalleryPage>(new ImageGalleryNavigationParameter
                {
                    ImagePaths = paths,
                    SelectedImagePath = selectedPath
                });
            }
        }
    }
}
