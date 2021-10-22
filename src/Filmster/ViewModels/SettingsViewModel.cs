using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Filmster.Core.Models;
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

        private ApplicationLanguage _selectedLanguage;
        public ApplicationLanguage SelectedLanguage
        {
            get { return _selectedLanguage; }
            set { Set(ref _selectedLanguage, value); }
        }

        public ObservableCollection<ApplicationLanguage> Languages { get; private set; } = new ObservableCollection<ApplicationLanguage>();

        public ICommand SaveLanguageCommand { get; set; }

        public SettingsViewModel()
        {
            SaveLanguageCommand = new RelayCommand(async () =>
            {
                await SaveLanguageAsync();
            });
        }

        public async Task InitializeAsync()
        {
            IncludeAdult = await IncludeAdultService.LoadIncludeAdultAsync();
            LoadLanguages();
            VersionDescription = GetVersionDescription();
        }

        private async Task IncludeAdultToggledAsync(bool isToggled)
        {
            await IncludeAdultService.SaveIncludeAdultAsync(isToggled);
        }

        private void LoadLanguages()
        {
            Languages.Clear();
            foreach (var language in LanguageService.AvailableLanguages)
            {
                Languages.Add(new ApplicationLanguage
                {
                    Code = language,
                    DisplayName = (LanguageService.LanguageLocalizationPrefix + language).GetLocalized()
                });
            }

            SelectedLanguage = Languages.FirstOrDefault(language => language.Code == LanguageService.CurrentLanguage);
        }

        private async Task SaveLanguageAsync()
        {
            await LanguageService.SaveLanguageAsync(SelectedLanguage.Code);
        }

        private string GetVersionDescription()
        {
            var version = Package.Current.Id.Version;
            return $"v{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}
