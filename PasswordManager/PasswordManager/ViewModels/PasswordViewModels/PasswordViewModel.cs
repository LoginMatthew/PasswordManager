using PasswordManager.Commands;
using PasswordManager.Commands.AddCommands;
using PasswordManager.Services;
using PasswordManager.Stores;
using PasswordManager.ViewModels.PasswordViewModels.Components;
using System.Windows.Input;

namespace PasswordManager.ViewModels.PasswordViewModels
{
    public class PasswordViewModel : ViewModelBase
    {
        //Fields
        private readonly PasswordListStore passwordListStore;
        private ViewModelBase passwordContent;
        private bool isShowPassword;
        public event Action ActionContentChanged;

        public bool IsShowPassword
        {
            get => isShowPassword;
            set
            {
                isShowPassword = value;
                var method = PasswordContent.GetType().GetMethod("ChangePassword");
                method.Invoke(PasswordContent, new object[] { isShowPassword });
                OnPropertyChanged(nameof(IsShowPassword));
            }
        }

        public ViewModelBase PasswordContent
        {
            get => passwordContent;
            set
            {
                passwordContent = value;
                OnPropertyChanged(nameof(PasswordContent));
                ActionContentChanged?.Invoke();
            }
        }

        public ICommand AddSiteCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand RealoadCommand { get; }

        public PasswordViewModel(PasswordListStore passwordListStore, PasswordListViewModel passwordListViewModel, ViewModelBase passwordContentViewModel, INavigationService addSiteViewModel, ModalNavigationStore modalNavigationStore, INavigationService closeNavigationService)
        {
            this.passwordListStore = passwordListStore;
            this.PasswordContent = passwordContentViewModel;

            var search = new SearchPasswordViewModel();
            search.SetCommands(new SearchPasswordAsyncCommand(search, passwordListStore, closeNavigationService), new NavigateCommand(closeNavigationService));
            NavigationModalService<SearchPasswordViewModel> searchNavigationService = new NavigationModalService<SearchPasswordViewModel>(modalNavigationStore, () => search);

            RealoadCommand = new LoadPasswordViewModelCommand(passwordListStore, passwordListViewModel);
            SearchCommand = new NavigateCommand(searchNavigationService);
            AddSiteCommand = new NavigateCommand(addSiteViewModel);     
        }
    }
}
