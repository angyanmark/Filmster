using Filmster.Common.Helpers;
using Filmster.Common.Models;
using Windows.ApplicationModel.DataTransfer;

namespace Filmster.Services
{
    public static class ShareService
    {
        private static readonly DataTransferManager dataTransferManager;

        private static ShareParameter ShareParameter { get; set; }

        static ShareService()
        {
            dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
        }

        private static void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var appDisplayName = "AppDisplayName".GetLocalized();

            DataRequest request = args.Request;
            request.Data.Properties.Title = appDisplayName;
            request.Data.Properties.Description = ShareParameter.Text;
            request.Data.SetText($"{ShareParameter.Text}\n#{appDisplayName}");
            request.Data.SetWebLink(ShareParameter.Uri);
        }

        public static void Share(ShareParameter shareParameter)
        {
            ShareParameter = shareParameter;
            DataTransferManager.ShowShareUI();
        }
    }
}
