﻿using Filmster.Core.Services;
using Filmster.Helpers;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.Trending;

namespace Filmster.ViewModels
{
    public class MoviesViewModel : MediaViewModelBase
    {
        public ObservableCollection<SearchMovie> TrendingMovies { get; set; } = new ObservableCollection<SearchMovie>();
        public ObservableCollection<SearchMovie> UpcomingMovies { get; set; } = new ObservableCollection<SearchMovie>();
        public ObservableCollection<SearchMovie> TopRatedMovies { get; set; } = new ObservableCollection<SearchMovie>();

        public MoviesViewModel()
        {
            _ = GetMoviesAsync();
        }

        private async Task GetMoviesAsync()
        {
            await GetTrendingMoviesAsync();
            await GetUpcomingMoviesAsync();
            await GetTopRatedMoviesAsync();
        }

        private async Task GetTrendingMoviesAsync()
        {
            var movies = await TMDbService.GetTrendingMoviesAsync(TimeWindow.Week);
            foreach (var movie in movies)
            {
                TrendingMovies.Add(movie);
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
