using Filmster.Helpers;
using Filmster.ViewModelBases;
using Microsoft.Toolkit.Uwp;
using System;
using TMDbLib.Objects.Reviews;

namespace Filmster.ViewModels
{
    public class ReviewsViewModel : MediaViewModelBase
    {
        public string MediaTitle { get; set; }
        public DateTime? MediaReleaseDate { get; set; }

        public IncrementalLoadingCollection<ReviewsSource, ReviewBase> Reviews { get; set; } = new IncrementalLoadingCollection<ReviewsSource, ReviewBase>();
    }
}
