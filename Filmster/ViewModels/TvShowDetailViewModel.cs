using Filmster.Core.Services;
using Filmster.Helpers;
using Filmster.Services;
using Filmster.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;

namespace Filmster.ViewModels
{
    public class TvShowDetailViewModel : LoadingObservable
    {
        private TvShow _tvShow;
        public TvShow TvShow
        {
            get { return _tvShow; }
            set { Set(ref _tvShow, value); }
        }

        private ImageData _selectedPoster;
        public ImageData SelectedPoster
        {
            get { return _selectedPoster; }
            set { Set(ref _selectedPoster, value); }
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

        private string _creators;
        public string Creators
        {
            get { return _creators; }
            set { Set(ref _creators, value); }
        }

        private int _episodeRuntime;
        public int EpisodeRuntime
        {
            get { return _episodeRuntime; }
            set { Set(ref _episodeRuntime, value); }
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

        public ICommand TvShowClickedCommand;
        public ICommand CastClickedCommand;
        public ICommand CrewClickedCommand;
        public ICommand MediaClickedCommand;

        public TvShowDetailViewModel()
        {
            SetCommands();
        }

        private void SetCommands()
        {
            TvShowClickedCommand = new RelayCommand<TvShow>(TvShowClicked);
            CastClickedCommand = new RelayCommand<Cast>(CastClicked);
            CrewClickedCommand = new RelayCommand<Crew>(CrewClicked);
            MediaClickedCommand = new RelayCommand<SearchBase>(MediaClicked);
        }

        public async Task LoadTvShow(int id)
        {
            TvShow = await TMDbService.GetTvShowAsync(id);
            if (TvShow == null)
            {
                NavigationService.GoBack();
                return;
            }
            SelectedPoster = GetSelectedPoster();
            Creators = GetCreators();
            EpisodeRuntime = Convert.ToInt32(TvShow.EpisodeRunTime.DefaultIfEmpty(0).Average());
            Certification = GetCertification();
            Genres = GetGenres();
            Video = TvShow.Videos.Results.FirstOrDefault();
            CastToggled(false);
            CrewToggled(false);
            ImagesToggled(false);
        }

        private ImageData GetSelectedPoster()
        {
            return TvShow.Images.Posters.Find(poster => poster.FilePath == TvShow.PosterPath) ?? TvShow.Images.Posters.FirstOrDefault();
        }

        private string GetCreators()
        {
            var creators = TvShow.CreatedBy.Select(creator => creator.Name);
            return string.Join(", ", creators);
        }

        private string GetCertification()
        {
            foreach (var rating in TvShow.ContentRatings.Results)
            {
                if (rating.Iso_3166_1.ToLower() == "us")
                {
                    if (!string.IsNullOrEmpty(rating.Rating))
                    {
                        return rating.Rating;
                    }
                }
            }
            return default;
        }

        private string GetGenres()
        {
            return string.Join(", ", TvShow.Genres.Select(genre => genre.Name));
        }

        private void TvShowClicked(TvShow tvShow)
        {
            NavigationService.Navigate(typeof(TvShowDetailPage), tvShow.Id);
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
            var cast = isChecked ? TvShow.Credits.Cast : TvShow.Credits.Cast.Take(TMDbService.DefaultCastCrewBackdropCount);
            Cast.Clear();
            foreach (var c in cast)
            {
                Cast.Add(c);
            }
        }

        private void CrewToggled(bool isChecked)
        {
            var crew = isChecked ? TvShow.Credits.Crew : TvShow.Credits.Crew.Take(TMDbService.DefaultCastCrewBackdropCount);
            Crew.Clear();
            foreach (var c in crew)
            {
                Crew.Add(c);
            }
        }

        private void ImagesToggled(bool isChecked)
        {
            var images = isChecked ? TvShow.Images.Backdrops : TvShow.Images.Backdrops.Take(TMDbService.DefaultCastCrewBackdropCount);
            Images.Clear();
            foreach (var i in images)
            {
                Images.Add(i);
            }
        }
    }
}
