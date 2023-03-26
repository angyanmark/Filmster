using System;
using Filmster.Services;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Filmster
{
    public sealed partial class App : Application
    {
        private readonly Lazy<ActivationService> _activationService;

        private ActivationService ActivationService
        {
            get => _activationService.Value;
        }

        public App()
        {
            InitializeComponent();
            UnhandledException += OnAppUnhandledException;

            // Deferred execution until used. Check https://docs.microsoft.com/dotnet/api/system.lazy-1 for further info on Lazy<T> class.
            _activationService = new Lazy<ActivationService>(CreateActivationService);
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (!args.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(args);
            }

            OverrideTitleBar();
        }

        protected override async void OnActivated(IActivatedEventArgs args) =>
            await ActivationService.ActivateAsync(args);

        private void OnAppUnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            // TODO WTS: Please log and handle the exception as appropriate to your scenario
            // For more info see https://docs.microsoft.com/uwp/api/windows.ui.xaml.application.unhandledexception
        }

        private ActivationService CreateActivationService() =>
            new ActivationService(typeof(Views.MoviesPage), new Lazy<UIElement>(CreateShell));

        private UIElement CreateShell() =>
            new Views.ShellPage();

        private void OverrideTitleBar()
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }
    }
}
