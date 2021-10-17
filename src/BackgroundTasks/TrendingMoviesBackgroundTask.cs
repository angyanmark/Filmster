using TileHelperLibrary;
using Windows.ApplicationModel.Background;

namespace BackgroundTasks
{
    public sealed class TrendingMoviesBackgroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            await TileUpdateHelper.UpdatePrimaryTileAsync();
            await TileUpdateHelper.UpdateSecondaryTilesAsync();

            deferral.Complete();
        }
    }
}
