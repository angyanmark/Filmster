using Filmster.Core.Services;
using Filmster.Helpers;
using Filmster.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TMDbLib.Objects.Search;

namespace Filmster.ViewModels
{
    public class MoviesViewModel : Observable
    {
        public ObservableCollection<SearchMovie> PopularMovies { get; set; } = new ObservableCollection<SearchMovie>();
        public ObservableCollection<SearchMovie> NowPlayingMovies { get; set; } = new ObservableCollection<SearchMovie>();
        public ObservableCollection<SearchMovie> UpcomingMovies { get; set; } = new ObservableCollection<SearchMovie>();
        public ObservableCollection<SearchMovie> TopRatedMovies { get; set; } = new ObservableCollection<SearchMovie>();

        public ICommand MovieClickedCommand;

        public MoviesViewModel()
        {
            _ = GetMoviesAsync();
            SetCommands();
        }

        private void SetCommands()
        {
            MovieClickedCommand = new RelayCommand<SearchMovie>(MovieClicked);
        }

        private async Task GetMoviesAsync()
        {
            await GetPopularMoviesAsync();
            await GetNowPlayingMoviesAsync();
            await GetUpcomingMoviesAsync();
            await GetTopRatedMoviesAsync();
        }

        private async Task GetPopularMoviesAsync()
        {
            var movies = await TMDbService.GetPopularMoviesAsync();
            foreach (var movie in movies)
            {
                PopularMovies.Add(movie);
            }
        }

        private async Task GetNowPlayingMoviesAsync()
        {
            var movies = await TMDbService.GetNowPlayingMoviesAsync();
            foreach (var movie in movies)
            {
                NowPlayingMovies.Add(movie);
            }
        }

        private async Task GetUpcomingMoviesAsync()
        {
            var movies = await TMDbService.GetUpcomingMoviesAsync();
            foreach (var movie in movies)
            {
                UpcomingMovies.Add(movie);
            }
        }

        private async Task GetTopRatedMoviesAsync()
        {
            var movies = await TMDbService.GetTopRatedMoviesAsync();
            foreach (var movie in movies)
            {
                TopRatedMovies.Add(movie);
            }
        }

        private void MovieClicked(SearchMovie movie)
        {
            NavigationService.NavigateToSearchMediaDetail(movie);
        }
    }
}
