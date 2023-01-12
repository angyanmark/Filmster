﻿using Filmster.Common.Helpers;
using Filmster.Common.Models;
using Filmster.Common.Services;
using Filmster.Helpers;
using Filmster.ViewModelBases;
using Microsoft.Toolkit.Uwp;
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
        public readonly int VoteCountMax = 30000;

        private readonly IEnumerable<DiscoverMovieSortBy> ExcludedSortByEnums = new List<DiscoverMovieSortBy>
        {
            DiscoverMovieSortBy.Undefined,
            DiscoverMovieSortBy.OriginalTitle,
            DiscoverMovieSortBy.OriginalTitleDesc,
            DiscoverMovieSortBy.ReleaseDate,
            DiscoverMovieSortBy.ReleaseDateDesc,
        };

        public IncrementalLoadingCollection<DiscoverMoviesSource, SearchMovie> Movies { get; set; } = new IncrementalLoadingCollection<DiscoverMoviesSource, SearchMovie>();

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
            SetCommands();
            SetSortBy();
            Clear();
            DiscoverMoviesSource.Options = GetOptions();
        }

        public void SetCommands()
        {
            ClearCommand = new RelayCommand(Clear);
            DiscoverCommand = new RelayCommand(async () => await DiscoverAsync());
        }

        private void SetSortBy()
        {
            var sortByEnums = Enum.GetValues(typeof(DiscoverMovieSortBy)).Cast<DiscoverMovieSortBy>();
            foreach (var sortByEnum in sortByEnums)
            {
                if (!ExcludedSortByEnums.Contains(sortByEnum))
                {
                    SortByItems.Add(new DiscoverMovieSortByItem
                    {
                        SortBy = sortByEnum,
                        DisplayName = $"DiscoverMovieSortBy_{sortByEnum}".GetLocalized(),
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
            SelectedSortByItem = SortByItems.SingleOrDefault(sortByItem => sortByItem.SortBy == DiscoverMovieSortBy.PopularityDesc);
        }

        public async Task LoadDataAsync()
        {
            await Task.WhenAll(new List<Task>
            {
                GetGenresAsync(),
                DiscoverAsync(),
            });
        }

        private async Task GetGenresAsync()
        {
            Genres.Add(EmptyGenre);
            var genres = await TMDbService.GetMovieGenresAsync();
            Genres.AddRange(genres);
        }

        private async Task DiscoverAsync()
        {
            DiscoverMoviesSource.Options = GetOptions();
            await Movies.RefreshAsync();
        }

        private DiscoverMovieOptions GetOptions()
        {
            return new DiscoverMovieOptions
            {
                PrimaryReleaseDateAfter = new DateTime(ReleaseDateFrom, 1, 1),
                PrimaryReleaseDateBefore = new DateTime(ReleaseDateTo, 12, 31),
                VoteAverageAtLeast = Math.Round(VoteAverageAtLeast * 2, 1),
                VoteAverageAtMost = Math.Round(VoteAverageAtMost * 2, 1),
                VoteCountAtLeast = VoteCountAtLeast,
                GenreId = GenreId,
                SortBy = SelectedSortByItem.SortBy,
            };
        }
    }
}
