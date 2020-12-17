using Filmster.Core.Services;
using Filmster.Helpers;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TMDbLib.Objects.Search;

namespace Filmster.ViewModels
{
    public class MoviesViewModel : Observable
    {
        public ObservableCollection<SearchMovie> PopularMovies { get; set; } = new ObservableCollection<SearchMovie>();
        public ObservableCollection<SearchMovie> NowPlayingMovies { get; set; } = new ObservableCollection<SearchMovie>();
        public ObservableCollection<SearchMovie> UpcomingMovies { get; set; } = new ObservableCollection<SearchMovie>();

        public MoviesViewModel()
        {
            _ = GetMovies();
        }

        private async Task GetMovies()
        {
            await GetPopularMoviesAsync();
            await GetNowPlayingMoviesAsync();
            await GetUpcomingMoviesAsync();
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
    }
}
