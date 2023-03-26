using Filmster.Common.Models;
using Filmster.Common.Models.Enums;
using Filmster.Common.Services;
using Filmster.Extensions;
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
        public IEnumerable<PersonCastCrewSortType> SortTypes { get; } = Enum.GetValues(typeof(PersonCastCrewSortType)).Cast<PersonCastCrewSortType>();

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

        public async Task LoadPersonAsync(int id)
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
            Images.AddRange(Person.TaggedImages.Results.Take(TMDbService.DefaultCastCrewBackdropCount));
        }

        private ImageData GetSelectedPoster()
        {
            return Person.Images.Profiles.Find(poster => poster.FilePath == Person.ProfilePath) ?? Person.Images.Profiles.FirstOrDefault();
        }

        private void CastSortTypeChanged(PersonCastCrewSortType sortType)
        {
            SetCast(IsCastChecked, sortType);
        }

        private void CrewSortTypeChanged(PersonCastCrewSortType sortType)
        {
            SetCrew(IsCrewChecked, sortType);
        }

        private void CastToggled(bool isChecked)
        {
            SetCast(isChecked, SelectedCastSortType);
        }

        private void CrewToggled(bool isChecked)
        {
            SetCrew(isChecked, SelectedCrewSortType);
        }

        private void SetCast(bool isChecked, PersonCastCrewSortType sortType)
        {
            var movieCast = Person.MovieCredits.Cast.DistinctBy(cast => cast.Id);
            var tvShowCast = Person.TvCredits.Cast.DistinctBy(cast => cast.Id);
            movieCast = SortHelper.SortMovieCast(movieCast, sortType);
            tvShowCast = SortHelper.SortTvShowCast(tvShowCast, sortType);
            movieCast = isChecked ? movieCast : movieCast.Take(TMDbService.DefaultCastCrewBackdropCount);
            tvShowCast = isChecked ? tvShowCast : tvShowCast.Take(TMDbService.DefaultCastCrewBackdropCount);
            MovieCast.Refresh(movieCast);
            TvShowCast.Refresh(tvShowCast);
        }

        private void SetCrew(bool isChecked, PersonCastCrewSortType sortType)
        {
            var movieCrew = Person.MovieCredits.Crew.DistinctBy(crew => crew.Id);
            var tvShowCrew = Person.TvCredits.Crew.DistinctBy(crew => crew.Id);
            movieCrew = SortHelper.SortMovieCrew(movieCrew, sortType);
            tvShowCrew = SortHelper.SortTvShowCrew(tvShowCrew, sortType);
            movieCrew = isChecked ? movieCrew : movieCrew.Take(TMDbService.DefaultCastCrewBackdropCount);
            tvShowCrew = isChecked ? tvShowCrew : tvShowCrew.Take(TMDbService.DefaultCastCrewBackdropCount);
            MovieCrew.Refresh(movieCrew);
            TvShowCrew.Refresh(tvShowCrew);
        }

        private void ImagesToggled(bool isChecked)
        {
            if (isChecked)
            {
                Images.AddRange(Person.TaggedImages.Results.Skip(TMDbService.DefaultCastCrewBackdropCount));
            }
            else
            {
                Images.Keep(TMDbService.DefaultCastCrewBackdropCount);
            }
        }

        private void ImageClicked(TaggedImage selectedImage)
        {
            NavigationService.Navigate<ImageGalleryPage>(new ImageGalleryNavigationParameter
            {
                ImagePaths = Person.TaggedImages.Results.Select(image => image.FilePath),
                SelectedImagePath = selectedImage.FilePath,
            });
        }

        public void PosterClicked(object sender, TappedRoutedEventArgs e)
        {
            if (e.OriginalSource is Image)
            {
                NavigationService.Navigate<ImageGalleryPage>(new ImageGalleryNavigationParameter
                {
                    ImagePaths = Person.Images.Profiles.Select(image => image.FilePath),
                    SelectedImagePath = SelectedPoster.FilePath,
                });
            }
        }
    }
}
