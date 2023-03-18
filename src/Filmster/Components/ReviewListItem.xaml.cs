using TMDbLib.Objects.Reviews;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Filmster.Components
{
    public sealed partial class ReviewListItem : UserControl
    {
        public ReviewListItem()
        {
            InitializeComponent();
        }

        public ReviewBase Review
        {
            get => (ReviewBase) GetValue(ReviewProperty);
            set => SetValue(ReviewProperty, value);
        }

        public static readonly DependencyProperty ReviewProperty =
            DependencyProperty.Register(nameof(Review), typeof(ReviewBase), typeof(ReviewListItem), default);
    }
}
