using Filmster.Helpers;
using Filmster.ViewModelBases;
using Microsoft.Toolkit.Uwp;
using TMDbLib.Objects.Reviews;

namespace Filmster.ViewModels
{
    public class ReviewsViewModel : MediaViewModelBase
    {
        public IncrementalLoadingCollection<ReviewsSource, ReviewBase> Reviews { get; set; } = new IncrementalLoadingCollection<ReviewsSource, ReviewBase>();
    }
}
