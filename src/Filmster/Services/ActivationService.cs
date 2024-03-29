﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Filmster.Activation;
using Filmster.Common.Helpers;
using Filmster.Common.Services;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Filmster.Services
{
    // For more information on understanding and extending activation flow see
    // https://github.com/Microsoft/WindowsTemplateStudio/blob/release/docs/UWP/activation.md
    internal class ActivationService
    {
        private readonly Type _defaultNavItem;
        private readonly Lazy<UIElement> _shell;

        public ActivationService(Type defaultNavItem, Lazy<UIElement> shell = null)
        {
            _shell = shell;
            _defaultNavItem = defaultNavItem;
        }

        public async Task ActivateAsync(object activationArgs)
        {
            if (IsInteractive(activationArgs))
            {
                // Initialize services that you need before app activation
                // take into account that the splash screen is shown while this code runs.
                await InitializeAsync();

                // Do not repeat app initialization when the Window already has content,
                // just ensure that the window is active
                if (Window.Current.Content == null)
                {
                    // Create a Shell or Frame to act as the navigation context
                    Window.Current.Content = _shell?.Value ?? new Frame();
                }
            }

            // Depending on activationArgs one of ActivationHandlers or DefaultActivationHandler
            // will navigate to the first page
            await HandleActivationAsync(activationArgs);

            if (IsInteractive(activationArgs))
            {
                // Ensure the current window is active
                Window.Current.Activate();

                // Tasks after activation
                await StartupAsync(activationArgs as IActivatedEventArgs);
            }
        }

        private async Task InitializeAsync()
        {
            await ThemeSelectorService.InitializeAsync().ConfigureAwait(false);
            await UserSessionService.StartUserSessionAsync();
        }

        private async Task HandleActivationAsync(object activationArgs)
        {
            var activationHandler = GetActivationHandlers()
                .FirstOrDefault(h => h.CanHandle(activationArgs));

            if (activationHandler != null)
            {
                await activationHandler.HandleAsync(activationArgs);
            }

            if (IsInteractive(activationArgs))
            {
                var defaultHandler = new DefaultActivationHandler(_defaultNavItem);
                if (defaultHandler.CanHandle(activationArgs))
                {
                    await defaultHandler.HandleAsync(activationArgs);
                }
            }
        }

        private async Task StartupAsync(IActivatedEventArgs activationArgs)
        {
            await ThemeSelectorService.SetRequestedThemeAsync();

            if (activationArgs.Kind == ActivationKind.Protocol)
            {
                var protocolArgs = activationArgs as ProtocolActivatedEventArgs;
                var query = protocolArgs.Uri.Query;

                var requestToken = HttpUtility.ParseQueryString(query).Get("request_token");
                bool.TryParse(HttpUtility.ParseQueryString(query).Get("approved"), out bool approved);

                if (approved)
                {
                    await UserSessionService.LoggedInAsync(requestToken);
                }
                else
                {
                    var dialog = new ContentDialog
                    {
                        Title = "ContentDialog_Login_Failed".GetLocalized(),
                        Content = "ContentDialog_Login_Failed_Content".GetLocalized(),
                        CloseButtonText = "ContentDialog_Ok".GetLocalized(),
                    };

                    await dialog.ShowAsync();
                }
            }
        }

        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
            yield break;
        }

        private bool IsInteractive(object args) =>
            args is IActivatedEventArgs;
    }
}
