using Filmster.Common.Services;
using Windows.UI.Xaml.Controls;

namespace Filmster.Dialogs
{
    public sealed partial class CreateListContentDialog : ContentDialog
    {
        private string ListName { get; set; }
        private string ListDescription { get; set; }
        public string ListId { get; set; }

        public CreateListContentDialog() => InitializeComponent();

        private async void CreateListButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (!string.IsNullOrWhiteSpace(ListName))
            {
                ListId = await TMDbService.ListCreateAsync(ListName, ListDescription);
            }
        }
    }
}
