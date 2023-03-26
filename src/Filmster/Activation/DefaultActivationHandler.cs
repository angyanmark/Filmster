using System;
using System.Threading.Tasks;
using System.Web;
using Filmster.Common.Helper.Tile;
using Filmster.Common.Models;
using Filmster.Services;
using Filmster.Views;
using Windows.ApplicationModel.Activation;

namespace Filmster.Activation
{
    internal class DefaultActivationHandler : ActivationHandler<IActivatedEventArgs>
    {
        private readonly Type _navElement;

        public DefaultActivationHandler(Type navElement) =>
            _navElement = navElement;

        protected override async Task HandleInternalAsync(IActivatedEventArgs args)
        {
            // When the navigation stack isn't restored, navigate to the first page and configure
            // the new page by passing required information in the navigation parameter
            object arguments = null;
            WatchlistActivationNavigationParameter watchlistActivationNavigationParameter = null;
            if (args is LaunchActivatedEventArgs launchArgs)
            {
                arguments = launchArgs.Arguments;

                var action = HttpUtility.ParseQueryString(launchArgs.Arguments).Get("action");
                if (action == Constants.MovieWatchlistTileId)
                {
                    watchlistActivationNavigationParameter = new WatchlistActivationNavigationParameter
                    {
                        PrimaryPivotIndex = 2,
                        WatchlistPivotIndex = 0,
                    };
                }
                if (action == Constants.TvShowWatchlistTileId)
                {
                    watchlistActivationNavigationParameter = new WatchlistActivationNavigationParameter
                    {
                        PrimaryPivotIndex = 2,
                        WatchlistPivotIndex = 1,
                    };
                }
            }

            if (watchlistActivationNavigationParameter != null)
            {
                NavigationService.Navigate<ProfilePage>(watchlistActivationNavigationParameter);
            }
            else
            {
                NavigationService.Navigate(_navElement, arguments);
            }

            await Task.CompletedTask;
        }

        // None of the ActivationHandlers has handled the app activation
        protected override bool CanHandleInternal(IActivatedEventArgs args) =>
            NavigationService.Frame.Content == null && _navElement != null;
    }
}
