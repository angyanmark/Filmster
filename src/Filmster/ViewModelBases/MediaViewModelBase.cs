﻿using Filmster.Helpers;
using Filmster.Services;
using Filmster.Views;
using System.Windows.Input;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Reviews;
using TMDbLib.Objects.Search;

using MovieCast = TMDbLib.Objects.Movies.Cast;
using TvCast = TMDbLib.Objects.TvShows.Cast;

namespace Filmster.ViewModelBases
{
    public class MediaViewModelBase : LoadingObservable
    {
        public ICommand SearchBaseClickedCommand;
        public ICommand MovieCastClickedCommand;
        public ICommand TvCastClickedCommand;
        public ICommand CrewClickedCommand;
        public ICommand ReviewBaseClickedCommand;
        public ICommand MovieRoleClickedCommand;
        public ICommand TvRoleClickedCommand;
        public ICommand MovieJobClickedCommand;
        public ICommand TvJobClickedCommand;
        public ICommand CollectionClickedCommand;

        public MediaViewModelBase() =>
            SetCommands();

        private void SetCommands()
        {
            SearchBaseClickedCommand = new RelayCommand<SearchBase>(SearchBaseClicked);
            MovieCastClickedCommand = new RelayCommand<MovieCast>(MovieCastClicked);
            TvCastClickedCommand = new RelayCommand<TvCast>(TvCastClicked);
            CrewClickedCommand = new RelayCommand<Crew>(CrewClicked);
            ReviewBaseClickedCommand = new RelayCommand<ReviewBase>(ReviewBaseClicked);
            MovieRoleClickedCommand = new RelayCommand<MovieRole>(MovieRoleClicked);
            TvRoleClickedCommand = new RelayCommand<TvRole>(TvRoleClicked);
            MovieJobClickedCommand = new RelayCommand<MovieJob>(MovieJobClicked);
            TvJobClickedCommand = new RelayCommand<TvJob>(TvJobClicked);
            CollectionClickedCommand = new RelayCommand<int>(CollectionClicked);
        }

        private void SearchBaseClicked(SearchBase searchBase) => NavigationService.NavigateSearchBase(searchBase);
        private void MovieCastClicked(MovieCast cast) => NavigationService.Navigate<PersonDetailPage>(cast.Id);
        private void TvCastClicked(TvCast cast) => NavigationService.Navigate<PersonDetailPage>(cast.Id);
        private void CrewClicked(Crew crew) => NavigationService.Navigate<PersonDetailPage>(crew.Id);
        private void ReviewBaseClicked(ReviewBase reviewBase) => NavigationService.Navigate<ReviewDetailPage>(reviewBase.Id);
        private void MovieRoleClicked(MovieRole movieRole) => NavigationService.Navigate<MovieDetailPage>(movieRole.Id);
        private void TvRoleClicked(TvRole tvRole) => NavigationService.Navigate<TvShowDetailPage>(tvRole.Id);
        private void MovieJobClicked(MovieJob movieJob) => NavigationService.Navigate<MovieDetailPage>(movieJob.Id);
        private void TvJobClicked(TvJob tvJob) => NavigationService.Navigate<TvShowDetailPage>(tvJob.Id);
        private void CollectionClicked(int id) => NavigationService.Navigate<CollectionDetailPage>(id);
    }
}
