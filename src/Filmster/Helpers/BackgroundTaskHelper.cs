using System;
using Windows.ApplicationModel.Background;

namespace Filmster.Helpers
{
    public static class BackgroundTaskHelper
    {
        private static readonly string TaskName = "TrendingMoviesBackgroundTask";
        private static readonly string TaskEntryPoint = "BackgroundTasks.TrendingMoviesBackgroundTask";

        public static async void RegisterTileBackgroundTaskAsync()
        {
            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();

            if (backgroundAccessStatus == BackgroundAccessStatus.AllowedSubjectToSystemPolicy ||
                backgroundAccessStatus == BackgroundAccessStatus.AlwaysAllowed)
            {
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == TaskName)
                    {
                        task.Value.Unregister(true);
                    }
                }

                BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder
                {
                    Name = TaskName,
                    TaskEntryPoint = TaskEntryPoint,
                };
                taskBuilder.SetTrigger(new TimeTrigger(180, false));
                taskBuilder.Register();
            }
        }
    }
}
