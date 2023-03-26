using Filmster.Common.Models;
using Filmster.Common.Services;
using Filmster.Extensions;
using Filmster.Helpers;
using Filmster.ViewModelBases;
using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace Filmster.ViewModels
{
    public class DiscoverViewModel : MediaViewModelBase
    {
        public readonly int ReleaseDateMin = 1870;
        public readonly int ReleaseDateMax = DateTime.Now.Year + 10;
        public readonly double VoteAverageMin = 0;
        public readonly double VoteAverageMax = 5;
        public readonly int VoteCountMin = 0;
        public readonly int VoteCountMax = 50000;

        private readonly Genre EmptyGenre = new Genre { Id = 0, Name = string.Empty };
        private List<Genre> MovieGenres { get; } = new List<Genre>();
        private List<Genre> TvShowGenres { get; } = new List<Genre>();
        public ObservableCollection<Genre> Genres { get; set; } = new ObservableCollection<Genre>();

        public IncrementalLoadingCollection<DiscoverSource, SearchMovieTvBase> Results { get; set; } = new IncrementalLoadingCollection<DiscoverSource, SearchMovieTvBase>();

        public List<MediaType> MediaTypes { get; } = new List<MediaType>
        {
            MediaType.Movie,
            MediaType.Tv,
        };

        public List<DiscoverMovieSortBy> MovieSortByItems { get; } = new List<DiscoverMovieSortBy>
        {
            DiscoverMovieSortBy.Popularity,
            DiscoverMovieSortBy.PopularityDesc,
            DiscoverMovieSortBy.Revenue,
            DiscoverMovieSortBy.RevenueDesc,
            DiscoverMovieSortBy.PrimaryReleaseDate,
            DiscoverMovieSortBy.PrimaryReleaseDateDesc,
            DiscoverMovieSortBy.VoteAverage,
            DiscoverMovieSortBy.VoteAverageDesc,
            DiscoverMovieSortBy.VoteCount,
            DiscoverMovieSortBy.VoteCountDesc,
        };

        public List<DiscoverTvShowSortBy> TvShowSortByItems { get; } = new List<DiscoverTvShowSortBy>
        {
            DiscoverTvShowSortBy.VoteAverage,
            DiscoverTvShowSortBy.VoteAverageDesc,
            DiscoverTvShowSortBy.FirstAirDate,
            DiscoverTvShowSortBy.FirstAirDateDesc,
            DiscoverTvShowSortBy.Popularity,
            DiscoverTvShowSortBy.PopularityDesc,
        };

        private MediaType _selectedMediaType = MediaType.Movie;
        public MediaType SelectedMediaType
        {
            get { return _selectedMediaType; }
            set
            {
                if (value != SelectedMediaType)
                {
                    Set(ref _selectedMediaType, value);
                    MediaTypeChanged();
                }
            }
        }

        private bool _isMovieMediaType = true;
        public bool IsMovieMediaType
        {
            get { return _isMovieMediaType; }
            set { Set(ref _isMovieMediaType, value); }
        }

        private int _releaseDateFrom;
        public int ReleaseDateFrom
        {
            get { return _releaseDateFrom; }
            set { Set(ref _releaseDateFrom, value); }
        }

        private int _releaseDateTo;
        public int ReleaseDateTo
        {
            get { return _releaseDateTo; }
            set { Set(ref _releaseDateTo, value); }
        }

        private double _voteAverageAtLeast;
        public double VoteAverageAtLeast
        {
            get { return _voteAverageAtLeast; }
            set { Set(ref _voteAverageAtLeast, value); }
        }

        private double _voteAverageAtMost;
        public double VoteAverageAtMost
        {
            get { return _voteAverageAtMost; }
            set { Set(ref _voteAverageAtMost, value); }
        }

        private int _voteCountAtLeast;
        public int VoteCountAtLeast
        {
            get { return _voteCountAtLeast; }
            set { Set(ref _voteCountAtLeast, value); }
        }

        private int _genreId;
        public int GenreId
        {
            get { return _genreId; }
            set { Set(ref _genreId, value); }
        }

        private DiscoverMovieSortBy _selectedMovieSortByItem;
        public DiscoverMovieSortBy SelectedMovieSortByItem
        {
            get { return _selectedMovieSortByItem; }
            set { Set(ref _selectedMovieSortByItem, value); }
        }

        private DiscoverTvShowSortBy _selectedTvShowSortByItem;
        public DiscoverTvShowSortBy SelectedTvShowSortByItem
        {
            get { return _selectedTvShowSortByItem; }
            set { Set(ref _selectedTvShowSortByItem, value); }
        }

        public ICommand ClearCommand;
        public ICommand DiscoverCommand;

        public DiscoverViewModel()
        {
            SetCommands();
            Clear();
            SetDiscoverSource();
        }

        public void SetCommands()
        {
            ClearCommand = new RelayCommand(Clear);
            DiscoverCommand = new RelayCommand(async () => await DiscoverAsync());
        }

        private void Clear()
        {
            ReleaseDateFrom = ReleaseDateMin;
            ReleaseDateTo = ReleaseDateMax;
            VoteAverageAtLeast = VoteAverageMin;
            VoteAverageAtMost = VoteAverageMax;
            VoteCountAtLeast = VoteCountMin;
            GenreId = EmptyGenre.Id;
            SelectedMovieSortByItem = DiscoverMovieSortBy.PopularityDesc;
            SelectedTvShowSortByItem = DiscoverTvShowSortBy.PopularityDesc;
        }

        private void MediaTypeChanged()
        {
            GenreId = EmptyGenre.Id;
            SetGenres();
            IsMovieMediaType = SelectedMediaType == MediaType.Movie;
        }

        public async Task LoadDataAsync()
        {
            await Task.WhenAll(new List<Task>
            {
                GetMovieGenresAsync(),
                GetTvShowGenresAsync(),
                DiscoverAsync(),
            });
            Genres.Add(EmptyGenre);
            SetGenres();
        }

        private async Task GetMovieGenresAsync()
        {
            var movieGenres = await TMDbService.GetMovieGenresAsync();
            MovieGenres.AddRange(movieGenres);
        }

        private async Task GetTvShowGenresAsync()
        {
            var tvShowGenres = await TMDbService.GetTvShowGenresAsync();
            TvShowGenres.AddRange(tvShowGenres);
        }

        private void SetGenres()
        {
            Genres.Keep(1);
            switch (SelectedMediaType)
            {
                case MediaType.Movie:
                    Genres.AddRange(MovieGenres);
                    break;
                case MediaType.Tv:
                    Genres.AddRange(TvShowGenres);
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(SelectedMediaType), (int)SelectedMediaType, typeof(MediaType));
            }
        }

        private async Task DiscoverAsync()
        {
            SetDiscoverSource();
            await Results.RefreshAsync();
        }

        private void SetDiscoverSource()
        {
            DiscoverSource.MediaType = SelectedMediaType;
            DiscoverSource.MovieOptions = GetMovieOptions();
            DiscoverSource.TvShowOptions = GetTvShowOptions();
        }

        private DiscoverMovieOptions GetMovieOptions() =>
            new DiscoverMovieOptions
            {
                PrimaryReleaseDateAfter = new DateTime(ReleaseDateFrom, 1, 1),
                PrimaryReleaseDateBefore = new DateTime(ReleaseDateTo, 12, 31),
                VoteAverageAtLeast = Math.Round(VoteAverageAtLeast * 2, 1),
                VoteAverageAtMost = Math.Round(VoteAverageAtMost * 2, 1),
                VoteCountAtLeast = VoteCountAtLeast,
                GenreId = GenreId,
                SortBy = SelectedMovieSortByItem,
            };

        private DiscoverTvShowOptions GetTvShowOptions() =>
            new DiscoverTvShowOptions
            {
                FirstAirDateAfter = new DateTime(ReleaseDateFrom, 1, 1),
                FirstAirDateBefore = new DateTime(ReleaseDateTo, 12, 31),
                VoteAverageAtLeast = Math.Round(VoteAverageAtLeast * 2, 1),
                VoteAverageAtMost = Math.Round(VoteAverageAtMost * 2, 1),
                VoteCountAtLeast = VoteCountAtLeast,
                GenreId = GenreId,
                SortBy = SelectedTvShowSortByItem,
            };
    }
}
