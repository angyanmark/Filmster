using Filmster.Common.Services;
using Filmster.Services;
using Filmster.ViewModelBases;
using System.Threading.Tasks;
using TMDbLib.Objects.Reviews;

namespace Filmster.ViewModels
{
    public class ReviewDetailViewModel : MediaViewModelBase
    {
        private Review _review;
        public Review Review
        {
            get { return _review; }
            set { Set(ref _review, value); }
        }

        public ReviewDetailViewModel()
        {
        }

        public async Task LoadReviewAsync(string id)
        {
            Review = await TMDbService.GetReviewAsync(id);
            if (Review == null)
            {
                NavigationService.GoBack();
                return;
            }
        }
    }
}
