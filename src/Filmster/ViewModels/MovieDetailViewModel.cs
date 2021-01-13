using Filmster.Core.Models;
using Filmster.Core.Services;
using Filmster.Helpers;
using Filmster.Services;
using Filmster.Views;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Filmster.ViewModels
{
    public class MovieDetailViewModel : MediaViewModelBase
    {
        private Movie _movie;
        public Movie Movie
        {
            get { return _movie; }
            set { Set(ref _movie, value); }
        }

        private ImageData _selectedPoster;
        public ImageData SelectedPoster
        {
            get { return _selectedPoster; }
            set { Set(ref _selectedPoster, value); }
        }

        private Collection _collection;
        public Collection Collection
        {
            get { return _collection; }
            set { Set(ref _collection, value); }
        }

        private Video _video;
        public Video Video
        {
            get { return _video; }
            set { Set(ref _video, value); }
        }

        private string _certification;
        public string Certification
        {
            get { return _certification; }
            set { Set(ref _certification, value); }
        }

        private string _directors;
        public string Directors
        {
            get { return _directors; }
            set { Set(ref _directors, value); }
        }

        private string _genres;
        public string Genres
        {
            get { return _genres; }
            set { Set(ref _genres, value); }
        }

        public ObservableCollection<Cast> Cast { get; set; } = new ObservableCollection<Cast>();
        public ObservableCollection<Crew> Crew { get; set; } = new ObservableCollection<Crew>();
        public ObservableCollection<ImageData> Images { get; set; } = new ObservableCollection<ImageData>();

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

        public MovieDetailViewModel()
        {
            ImageClickedCommand = new RelayCommand<ImageData>(ImageClicked);
        }

        public async Task LoadMovie(int id)
        {
            Movie = await TMDbService.GetMovieAsync(id);
            if (Movie == null)
            {
                NavigationService.GoBack();
                return;
            }
            SelectedPoster = GetSelectedPoster();
            Directors = GetDirectors();
            Certification = GetCertification();
            Genres = GetGenres();
            Video = Movie.Videos.Results.FirstOrDefault();
            Collection = await GetCollectionAsync();
            CastToggled(false);
            CrewToggled(false);
            ImagesToggled(false);
        }

        private ImageData GetSelectedPoster()
        {
            return Movie.Images.Posters.Find(poster => poster.FilePath == Movie.PosterPath) ?? Movie.Images.Posters.FirstOrDefault();
        }

        private string GetDirectors()
        {
            var directors = Movie.Credits.Crew.FindAll(crew => crew.Job.ToLower() == "director");
            return string.Join(", ", directors.Select(director => director.Name));
        }

        private string GetCertification()
        {
            foreach (var releaseDate in Movie.ReleaseDates.Results)
            {
                if (releaseDate.Iso_3166_1.ToLower() == "us")
                {
                    var cert = releaseDate.ReleaseDates.FirstOrDefault(rd => !string.IsNullOrEmpty(rd.Certification));
                    if (cert != null)
                    {
                        return cert.Certification;
                    }
                }
            }
            return default;
        }

        private string GetGenres()
        {
            return string.Join(", ", Movie.Genres.Select(genre => genre.Name));
        }

        private async Task<Collection> GetCollectionAsync()
        {
            if (Movie.BelongsToCollection != null)
            {
                var collection = await TMDbService.GetCollectionAsync(Movie.BelongsToCollection.Id);
                collection.Parts = collection.Parts
                    .OrderByDescending(part => part.ReleaseDate.HasValue)
                    .ThenBy(p => p.ReleaseDate).ToList();
                return collection;
            }
            return default;
        }

        private void CastToggled(bool isChecked)
        {
            var cast = isChecked ? Movie.Credits.Cast : Movie.Credits.Cast.Take(TMDbService.DefaultCastCrewBackdropCount);
            Cast.Clear();
            foreach (var c in cast)
            {
                Cast.Add(c);
            }
        }

        private void CrewToggled(bool isChecked)
        {
            var crew = isChecked ? Movie.Credits.Crew : Movie.Credits.Crew.Take(TMDbService.DefaultCastCrewBackdropCount);
            Crew.Clear();
            foreach (var c in crew)
            {
                Crew.Add(c);
            }
        }

        private void ImagesToggled(bool isChecked)
        {
            var images = isChecked ? Movie.Images.Backdrops : Movie.Images.Backdrops.Take(TMDbService.DefaultCastCrewBackdropCount);
            Images.Clear();
            foreach (var i in images)
            {
                Images.Add(i);
            }
        }

        private void ImageClicked(ImageData selectedImage)
        {
            var paths = Movie.Images.Backdrops.Select(image => image.FilePath);
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
                var paths = Movie.Images.Posters.Select(image => image.FilePath);
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
