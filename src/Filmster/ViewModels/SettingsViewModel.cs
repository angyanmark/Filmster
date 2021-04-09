using System.Threading.Tasks;
using System.Windows.Input;
using Filmster.Helpers;
using Filmster.Services;
using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace Filmster.ViewModels
{
    public class SettingsViewModel : Observable
    {
        private ElementTheme _elementTheme = ThemeSelectorService.Theme;

        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { Set(ref _elementTheme, value); }
        }

        private string _versionDescription;

        public string VersionDescription
        {
            get { return _versionDescription; }

            set { Set(ref _versionDescription, value); }
        }

        private ICommand _switchThemeCommand;

        public ICommand SwitchThemeCommand
        {
            get
            {
                if (_switchThemeCommand == null)
                {
                    _switchThemeCommand = new RelayCommand<ElementTheme>(
                        async (param) =>
                        {
                            ElementTheme = param;
                            await ThemeSelectorService.SetThemeAsync(param);
                        });
                }

                return _switchThemeCommand;
            }
        }

        private bool _includeAdult;

        public bool IncludeAdult
        {
            get { return _includeAdult; }

            set
            {
                Set(ref _includeAdult, value);
                _ = IncludeAdultToggledAsync(IncludeAdult);
            }
        }

        public SettingsViewModel()
        {
        }

        public async Task InitializeAsync()
        {
            IncludeAdult = await IncludeAdultService.LoadIncludeAdultAsync();

            VersionDescription = GetVersionDescription();
        }

        private async Task IncludeAdultToggledAsync(bool isToggled)
        {
            await IncludeAdultService.SaveIncludeAdultAsync(isToggled);
        }

        private string GetVersionDescription()
        {
            var version = Package.Current.Id.Version;
            return $"v{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}
