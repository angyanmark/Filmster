using Filmster.Core.Services;
using Filmster.Helpers;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TMDbLib.Objects.Search;

namespace Filmster.ViewModels
{
    public class MoviesViewModel : MediaViewModelBase
    {
        public ObservableCollection<SearchMovie> PopularMovies { get; set; } = new ObservableCollection<SearchMovie>();
        public ObservableCollection<SearchMovie> UpcomingMovies { get; set; } = new ObservableCollection<SearchMovie>();
        public ObservableCollection<SearchMovie> TopRatedMovies { get; set; } = new ObservableCollection<SearchMovie>();

        public MoviesViewModel()
        {
            _ = GetMoviesAsync();
        }

        private async Task GetMoviesAsync()
        {
            await GetPopularMoviesAsync();
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
    }
}
