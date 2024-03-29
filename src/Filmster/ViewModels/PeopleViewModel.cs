﻿using Filmster.Helpers;
using Filmster.ViewModelBases;
using Microsoft.Toolkit.Uwp;
using TMDbLib.Objects.Search;

namespace Filmster.ViewModels
{
    public class PeopleViewModel : MediaViewModelBase
    {
        public IncrementalLoadingCollection<TrendingPeopleSource, SearchPerson> TrendingPeople { get; } = new IncrementalLoadingCollection<TrendingPeopleSource, SearchPerson>();
        public IncrementalLoadingCollection<PopularPeopleSource, SearchPerson> PopularPeople { get; } = new IncrementalLoadingCollection<PopularPeopleSource, SearchPerson>();
    }
}
