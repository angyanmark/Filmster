using Filmster.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Filmster.Views
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsViewModel ViewModel { get; } = new SettingsViewModel();

        public SettingsPage() => InitializeComponent();
    }
}
