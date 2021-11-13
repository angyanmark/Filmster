using Filmster.Helpers;
using Filmster.Services;
using Filmster.Views;
using System.Windows.Input;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;

using MovieCast = TMDbLib.Objects.Movies.Cast;
using TvCast = TMDbLib.Objects.TvShows.Cast;

namespace Filmster.ViewModelBases
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
        public ICommand CollectionClickedCommand;

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
            CollectionClickedCommand = new RelayCommand<int>(CollectionClicked);
        }

        private void TvShowClicked(TvShow tvShow)
        {
            NavigationService.Navigate<TvShowDetailPage>(tvShow.Id);
        }

        private void SearchMovieClicked(SearchMovie searchMovie)
        {
            NavigationService.Navigate<MovieDetailPage>(searchMovie.Id);
        }

        private void SearchTvClicked(SearchTv searchTv)
        {
            NavigationService.Navigate<TvShowDetailPage>(searchTv.Id);
        }

        private void SearchPersonClicked(SearchPerson searchPerson)
        {
            NavigationService.Navigate<PersonDetailPage>(searchPerson.Id);
        }

        private void SearchBaseClicked(SearchBase searchBase)
        {
            NavigationService.NavigateSearchBase(searchBase);
        }

        private void MovieCastClicked(MovieCast cast)
        {
            NavigationService.Navigate<PersonDetailPage>(cast.Id);
        }

        private void TvCastClicked(TvCast cast)
        {
            NavigationService.Navigate<PersonDetailPage>(cast.Id);
        }

        private void CrewClicked(Crew crew)
        {
            NavigationService.Navigate<PersonDetailPage>(crew.Id);
        }

        private void MovieRoleClicked(MovieRole movieRole)
        {
            NavigationService.Navigate<MovieDetailPage>(movieRole.Id);
        }

        private void TvRoleClicked(TvRole tvRole)
        {
            NavigationService.Navigate<TvShowDetailPage>(tvRole.Id);
        }

        private void MovieJobClicked(MovieJob movieJob)
        {
            NavigationService.Navigate<MovieDetailPage>(movieJob.Id);
        }

        private void TvJobClicked(TvJob tvJob)
        {
            NavigationService.Navigate<TvShowDetailPage>(tvJob.Id);
        }

        private void CollectionClicked(int id)
        {
            NavigationService.Navigate<CollectionDetailPage>(id);
        }
    }
}
