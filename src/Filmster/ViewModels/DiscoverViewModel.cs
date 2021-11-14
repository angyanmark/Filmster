using Filmster.Common.Helpers;
using Filmster.Common.Models;
using Filmster.Common.Services;
using Filmster.Extensions;
using Filmster.Helpers;
using Filmster.ViewModelBases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public readonly int VoteCountMax = 20000;

        private readonly IEnumerable<DiscoverMovieSortBy> ExcludedSortByItems = new List<DiscoverMovieSortBy>
        {
            DiscoverMovieSortBy.Undefined,
            DiscoverMovieSortBy.OriginalTitle,
            DiscoverMovieSortBy.OriginalTitleDesc,
            DiscoverMovieSortBy.ReleaseDate,
            DiscoverMovieSortBy.ReleaseDateDesc
        };

        public ObservableCollection<SearchMovie> Movies { get; set; } = new ObservableCollection<SearchMovie>();

        public List<Genre> Genres { get; set; } = new List<Genre>();
        private Genre EmptyGenre { get; set; } = new Genre { Id = 0, Name = string.Empty };

        public ObservableCollection<DiscoverMovieSortByItem> SortByItems { get; set; } = new ObservableCollection<DiscoverMovieSortByItem>();

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

        private DiscoverMovieSortByItem _selectedSortByItem;
        public DiscoverMovieSortByItem SelectedSortByItem
        {
            get { return _selectedSortByItem; }
            set { Set(ref _selectedSortByItem, value); }
        }

        public ICommand ClearCommand;
        public ICommand DiscoverCommand;

        public DiscoverViewModel()
        {
            _ = LoadDataAsync();
            SetCommands();
        }

        public void SetCommands()
        {
            ClearCommand = new RelayCommand(Clear);
            DiscoverCommand = new RelayCommand(async () => await DiscoverAsync());
        }

        private async Task LoadDataAsync()
        {
            await GetGenresAsync();
            GetSortBy();
            Clear();
            await DiscoverAsync();
        }

        private async Task GetGenresAsync()
        {
            Genres.Add(EmptyGenre);
            var genres = await TMDbService.GetMovieGenresAsync();
            Genres.AddRange(genres);
        }

        private void GetSortBy()
        {
            var enums = Enum.GetValues(typeof(DiscoverMovieSortBy)).Cast<DiscoverMovieSortBy>();
            foreach (var e in enums)
            {
                if (!ExcludedSortByItems.Contains(e))
                {
                    SortByItems.Add(new DiscoverMovieSortByItem
                    {
                        SortBy = e,
                        DisplayName = $"DiscoverMovieSortBy_{e}".GetLocalized()
                    });
                }
            }
        }

        private void Clear()
        {
            ReleaseDateFrom = ReleaseDateMin;
            ReleaseDateTo = ReleaseDateMax;
            VoteAverageAtLeast = VoteAverageMin;
            VoteAverageAtMost = VoteAverageMax;
            VoteCountAtLeast = VoteCountMin;
            GenreId = EmptyGenre.Id;
            SelectedSortByItem = SortByItems.FirstOrDefault(sortByItem => sortByItem.SortBy == DiscoverMovieSortBy.PopularityDesc);
        }

        private async Task DiscoverAsync()
        {
            var options = new DiscoverMovieOptions
            {
                PrimaryReleaseDateAfter = new DateTime(ReleaseDateFrom, 1, 1),
                PrimaryReleaseDateBefore = new DateTime(ReleaseDateTo, 1, 1),
                VoteAverageAtLeast = VoteAverageAtLeast * 2,
                VoteAverageAtMost = VoteAverageAtMost * 2,
                VoteCountAtLeast = VoteCountAtLeast,
                GenreId = GenreId,
                SortBy = SelectedSortByItem.SortBy
            };

            var movies = await TMDbService.GetDiscoverMoviesAsync(options);
            Movies.Refresh(movies);
        }
    }
}
