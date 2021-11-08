using Filmster.Common.Helper.Tile;
using Filmster.Common.Services;
using Windows.ApplicationModel.Background;

namespace BackgroundTasks
{
    public sealed class TileUpdateBackgroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            await UserSessionService.StartUserSessionAsync();
            await TileUpdateHelper.UpdatePrimaryTileAsync();
            await TileUpdateHelper.UpdateSecondaryTilesAsync();

            deferral.Complete();
        }
    }
}
