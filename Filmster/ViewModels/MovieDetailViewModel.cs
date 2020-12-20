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
using TMDbLib.Objects.Search;

namespace Filmster.ViewModels
{
    public class MovieDetailViewModel : Observable
    {
        private Movie _movie;
        public Movie Movie
        {
            get { return _movie; }
            set { Set(ref _movie, value); }
        }

        private Collection _collection;
        public Collection Collection
        {
            get { return _collection; }
            set { Set(ref _collection, value); }
        }

        public ObservableCollection<Cast> Cast { get; set; } = new ObservableCollection<Cast>();
        public ObservableCollection<Crew> Crew { get; set; } = new ObservableCollection<Crew>();
        public ObservableCollection<ImageData> Backdrops { get; set; } = new ObservableCollection<ImageData>();

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

        private bool _isBackdropsChecked;
        public bool IsBackdropsChecked
        {
            get { return _isBackdropsChecked; }
            set
            {
                Set(ref _isBackdropsChecked, value);
                BackdropsToggled(IsBackdropsChecked);
            }
        }

        public ICommand CastClickedCommand;
        public ICommand CrewClickedCommand;
        public ICommand MediaClickedCommand;

        public MovieDetailViewModel()
        {
            SetCommands();
        }

        private void SetCommands()
        {
            CastClickedCommand = new RelayCommand<Cast>(CastClicked);
            CrewClickedCommand = new RelayCommand<Crew>(CrewClicked);
            MediaClickedCommand = new RelayCommand<SearchBase>(MediaClicked);
        }

        public async Task LoadMovie(int id)
        {
            Movie = await TMDbService.GetMovieAsync(id);
            CastToggled(false);
            CrewToggled(false);
            BackdropsToggled(false);
            await GetCollectionAsync();
        }

        private void CastClicked(Cast cast)
        {
            NavigationService.Navigate(typeof(PersonDetailPage), cast.Id);
        }

        private void CrewClicked(Crew crew)
        {
            NavigationService.Navigate(typeof(PersonDetailPage), crew.Id);
        }

        private void MediaClicked(SearchBase media)
        {
            NavigationService.NavigateToSearchMediaDetail(media);
        }

        private void CastToggled(bool isChecked)
        {
            var cast = isChecked ? Movie.Credits.Cast : Movie.Credits.Cast.Take(15);
            Cast.Clear();
            foreach (var c in cast)
            {
                Cast.Add(c);
            }
        }

        private void CrewToggled(bool isChecked)
        {
            var crew = isChecked ? Movie.Credits.Crew : Movie.Credits.Crew.Take(15);
            Crew.Clear();
            foreach (var c in crew)
            {
                Crew.Add(c);
            }
        }

        private void BackdropsToggled(bool isChecked)
        {
            var backdrops = isChecked ? Movie.Images.Backdrops : Movie.Images.Backdrops.Take(15);
            Backdrops.Clear();
            foreach (var b in backdrops)
            {
                Backdrops.Add(b);
            }
        }

        private async Task GetCollectionAsync()
        {
            if (Movie.BelongsToCollection != null)
            {
                Collection = await TMDbService.GetCollectionAsync(Movie.BelongsToCollection.Id);
            }
        }
    }
}
