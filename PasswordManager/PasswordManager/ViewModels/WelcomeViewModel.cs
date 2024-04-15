
using PasswordManager.Commands;
using PasswordManager.Services;
using PasswordManager.Stores;
using System.Windows.Input;
using PasswordManager.Commands.LoadCommands;

namespace PasswordManager.ViewModels
{
    public class WelcomeViewModel : ViewModelBase
    {
        private string userName;

        private bool isViewVisible;
        public NavigationSideViewModel NavigationSideViewModel { get; }

        public SettingDataStore SettingDataStore { get; set; }


        //Properties
        public string UserName
        {
            get => userName;
            set
            {
                userName = value;
                //Name of the property
                //OnPropertyChanged("UserName");
                //Name of the state to get the nme
                OnPropertyChanged(nameof(UserName));
            }
        }
        public bool IsViewVisible
        {
            get => isViewVisible;
            set
            {
                isViewVisible = value;
                //OnPropertyChanged(nameof(_isViewVisible));
                OnPropertyChanged("IsViewVisible");
            }
        }

        //--> Commands
        public ICommand ShowDashBoardViewCommand { get; }

        private ICommand StartLoadSettingCommand { get; }
        //Constructor
        public WelcomeViewModel(SettingDataStore settingDataStore, INavigationService navigationService, NavigationSideViewModel navigationSideViewModel)
        {
            this.IsViewVisible = true;
            this.SettingDataStore = settingDataStore;

            NavigationSideViewModel = navigationSideViewModel;
            ShowDashBoardViewCommand = new StartCommand(navigationService);

            StartLoadSettingCommand = new LoadSettingCommand(settingDataStore);


            StartLoadSettingCommand.Execute(null);


        }


    }
}
