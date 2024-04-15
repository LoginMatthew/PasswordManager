using PasswordManager.Commands;
using System.Windows.Input;
using PasswordManager.Services;
using PasswordManager.Stores;
using PasswordManager.Enums;

namespace PasswordManager.ViewModels
{
    public class NavigationSideViewModel : ViewModelBase
    {
        public NavigationStore _navigationStore;

        //Properties
        public ICommand NavigateToDashoardCommand { get; }
        public ICommand NavigateSettingCommand { get; }
        public ICommand NavigateAboutCommand { get; }
        public ICommand NavigatePasswordCommand { get; }

        public NavigationEnum NavigationSide 
        {
            get
            {
                return _navigationStore.NavigaitonSide;
            }

            set
            {
                _navigationStore.NavigaitonSide = value;
                OnPropertyChanged(nameof(NavigationSide));
            }
        }

        public NavigationSideViewModel(
            INavigationService settingsNavigationService,
            INavigationService dashboardNavigationService,
            INavigationService aboutNavigationService,
            INavigationService passwordNavigationService,
            NavigationStore navigationStore)
        {
            NavigateSettingCommand = new NavigateCommand(settingsNavigationService);
            NavigateToDashoardCommand = new NavigateCommand(dashboardNavigationService);
            NavigateAboutCommand = new NavigateCommand(aboutNavigationService);
            NavigatePasswordCommand = new NavigateCommand(passwordNavigationService);
            _navigationStore = navigationStore;
        }

        //This is called by the LayoutViewModel
        public override void Dispose()
        {
            base.Dispose();
        }

        //Transient, not a singelton, its a multiple instances created/destroyed throughout application lifetime
    }
}
