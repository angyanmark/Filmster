using Filmster.Common.Models;
using Filmster.Common.Services;
using Filmster.Helpers;
using Filmster.Services;
using Filmster.ViewModelBases;
using Filmster.Views;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.General;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Filmster.ViewModels
{
    public class CollectionDetailViewModel : MediaViewModelBase
    {
        private Collection _collection;
        public Collection Collection
        {
            get { return _collection; }
            set { Set(ref _collection, value); }
        }

        private ImageData _selectedPoster;
        public ImageData SelectedPoster
        {
            get { return _selectedPoster; }
            set { Set(ref _selectedPoster, value); }
        }

        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get { return _startDate; }
            set { Set(ref _startDate, value); }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get { return _endDate; }
            set { Set(ref _endDate, value); }
        }

        private double _voteAverage;
        public double VoteAverage
        {
            get { return _voteAverage; }
            set { Set(ref _voteAverage, value); }
        }

        private int _voteCount;
        public int VoteCount
        {
            get { return _voteCount; }
            set { Set(ref _voteCount, value); }
        }

        public ICommand ImageClickedCommand;

        public CollectionDetailViewModel()
        {
            ImageClickedCommand = new RelayCommand<ImageData>(ImageClicked);
        }

        public async Task LoadCollectionAsync(int id)
        {
            Collection = await GetCollectionAsync(id);
            if (Collection == null)
            {
                NavigationService.GoBack();
                return;
            }
            SelectedPoster = GetSelectedPoster();
            SetStartEndDate();
            (VoteAverage, VoteCount) = VoteHelper.GetVoteAverageVoteCount(Collection.Parts.Select(part => (part.VoteAverage, part.VoteCount)));
        }

        private async Task<Collection> GetCollectionAsync(int id)
        {
            var collection = await TMDbService.GetCollectionAsync(id, CollectionMethods.Images);
            collection.Parts = collection.Parts
                .OrderByDescending(part => part.ReleaseDate.HasValue)
                .ThenBy(p => p.ReleaseDate)
                .ToList();
            return collection;
        }

        private ImageData GetSelectedPoster()
        {
            return Collection.Images.Posters.Find(poster => poster.FilePath == Collection.PosterPath) ?? Collection.Images.Posters.FirstOrDefault();
        }

        private void SetStartEndDate()
        {
            var dates = Collection.Parts.Select(part => part.ReleaseDate);
            StartDate = dates.Min();
            EndDate = dates.Any(date => !date.HasValue) ? null : dates.Max();
        }

        private void ImageClicked(ImageData selectedImage)
        {
            NavigationService.Navigate<ImageGalleryPage>(new ImageGalleryNavigationParameter
            {
                ImagePaths = Collection.Images.Backdrops.Select(image => image.FilePath),
                SelectedImagePath = selectedImage.FilePath,
            });
        }

        public void PosterClicked(object sender, TappedRoutedEventArgs e)
        {
            if (e.OriginalSource is Image)
            {
                NavigationService.Navigate<ImageGalleryPage>(new ImageGalleryNavigationParameter
                {
                    ImagePaths = Collection.Images.Posters.Select(image => image.FilePath),
                    SelectedImagePath = SelectedPoster.FilePath,
                });
            }
        }
    }
}
