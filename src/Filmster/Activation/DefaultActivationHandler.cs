using System;
using System.Threading.Tasks;
using System.Web;
using Filmster.Services;
using Filmster.Views;
using Windows.ApplicationModel.Activation;

namespace Filmster.Activation
{
    internal class DefaultActivationHandler : ActivationHandler<IActivatedEventArgs>
    {
        private readonly Type _navElement;

        public DefaultActivationHandler(Type navElement)
        {
            _navElement = navElement;
        }

        protected override async Task HandleInternalAsync(IActivatedEventArgs args)
        {
            // When the navigation stack isn't restored, navigate to the first page and configure
            // the new page by passing required information in the navigation parameter
            object arguments = null;
            bool navigateToWatchlist = false;
            if (args is LaunchActivatedEventArgs launchArgs)
            {
                arguments = launchArgs.Arguments;

                var action = HttpUtility.ParseQueryString(launchArgs.Arguments).Get("action");
                if (action == "movie_watchlist")
                {
                    navigateToWatchlist = true;
                }
            }

            if (navigateToWatchlist)
            {
                NavigationService.Navigate<ProfilePage>(2);
            }
            else
            {
                NavigationService.Navigate(_navElement, arguments);
            }

            await Task.CompletedTask;
        }

        protected override bool CanHandleInternal(IActivatedEventArgs args)
        {
            // None of the ActivationHandlers has handled the app activation
            return NavigationService.Frame.Content == null && _navElement != null;
        }
    }
}
