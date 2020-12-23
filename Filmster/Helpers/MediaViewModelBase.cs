using Filmster.Services;
using Filmster.Views;
using System.Windows.Input;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;

using MovieCast = TMDbLib.Objects.Movies.Cast;
using TvCast = TMDbLib.Objects.TvShows.Cast;

namespace Filmster.Helpers
{
    public class MediaViewModelBase : LoadingObservable
    {
        public ICommand TvShowClickedCommand;
        public ICommand SearchMovieClickedCommand;
        public ICommand SearchTvClickedCommand;
        public ICommand SearchPersonClickedCommand;
        public ICommand SearchBaseClickedCommand;
        public ICommand MovieCastClickedCommand;
        public ICommand TvCastClickedCommand;
        public ICommand CrewClickedCommand;
        public ICommand MovieRoleClickedCommand;
        public ICommand TvRoleClickedCommand;
        public ICommand MovieJobClickedCommand;
        public ICommand TvJobClickedCommand;

        public MediaViewModelBase()
        {
            SetCommands();
        }

        private void SetCommands()
        {
            TvShowClickedCommand = new RelayCommand<TvShow>(TvShowClicked);
            SearchMovieClickedCommand = new RelayCommand<SearchMovie>(SearchMovieClicked);
            SearchTvClickedCommand = new RelayCommand<SearchTv>(SearchTvClicked);
            SearchPersonClickedCommand = new RelayCommand<SearchPerson>(SearchPersonClicked);
            SearchBaseClickedCommand = new RelayCommand<SearchBase>(SearchBaseClicked);
            MovieCastClickedCommand = new RelayCommand<MovieCast>(MovieCastClicked);
            TvCastClickedCommand = new RelayCommand<TvCast>(TvCastClicked);
            CrewClickedCommand = new RelayCommand<Crew>(CrewClicked);
            MovieRoleClickedCommand = new RelayCommand<MovieRole>(MovieRoleClicked);
            TvRoleClickedCommand = new RelayCommand<TvRole>(TvRoleClicked);
            MovieJobClickedCommand = new RelayCommand<MovieJob>(MovieJobClicked);
            TvJobClickedCommand = new RelayCommand<TvJob>(TvJobClicked);
        }

        private void TvShowClicked(TvShow tvShow)
        {
            NavigationService.Navigate(typeof(TvShowDetailPage), tvShow.Id);
        }

        private void SearchMovieClicked(SearchMovie searchMovie)
        {
            NavigationService.Navigate(typeof(MovieDetailPage), searchMovie.Id);
        }

        private void SearchTvClicked(SearchTv searchTv)
        {
            NavigationService.Navigate(typeof(TvShowDetailPage), searchTv.Id);
        }

        private void SearchPersonClicked(SearchPerson searchPerson)
        {
            NavigationService.Navigate(typeof(PersonDetailPage), searchPerson.Id);
        }

        private void SearchBaseClicked(SearchBase searchBase)
        {
            NavigationService.NavigateSearchBase(searchBase);
        }

        private void MovieCastClicked(MovieCast cast)
        {
            NavigationService.Navigate(typeof(PersonDetailPage), cast.Id);
        }

        private void TvCastClicked(TvCast cast)
        {
            NavigationService.Navigate(typeof(PersonDetailPage), cast.Id);
        }

        private void CrewClicked(Crew crew)
        {
            NavigationService.Navigate(typeof(PersonDetailPage), crew.Id);
        }

        private void MovieRoleClicked(MovieRole movieRole)
        {
            NavigationService.Navigate(typeof(MovieDetailPage), movieRole.Id);
        }

        private void TvRoleClicked(TvRole tvRole)
        {
            NavigationService.Navigate(typeof(TvShowDetailPage), tvRole.Id);
        }

        private void MovieJobClicked(MovieJob movieJob)
        {
            NavigationService.Navigate(typeof(MovieDetailPage), movieJob.Id);
        }

        private void TvJobClicked(TvJob tvJob)
        {
            NavigationService.Navigate(typeof(TvShowDetailPage), tvJob.Id);
        }
    }
}
