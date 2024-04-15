
using PasswordManager.Commands;
using PasswordManager.Services;
using PasswordManager.Stores;
using PasswordManager.Domain.Models;
using System.Windows.Input;
using PasswordManager.Commands.DeleteCommands;
using PasswordManager.ViewModels.PopUpViewModels;

namespace PasswordManager.ViewModels.PasswordViewModels.Components
{
    public class PasswordListItemViewModel : PasswordViewModelBase
    {
        private bool isShowPassword;
        private PasswordModel passwordModel;
        private ICommandWithReturn reversePasswordText { get; }

        public bool IsShowPassword
        {
            get => isShowPassword;

            set
            {
                isShowPassword = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        
        public string Email => PasswordModel.Email;
        public string Site => PasswordModel.Site;
        public string UserName => PasswordModel.UserName;
        public string Description => PasswordModel.Description;
        public string ModificationDate => PasswordModel.CreationDate.ToString("d");
        public string FullModificationDate => PasswordModel.CreationDate.ToString("G");

        public PasswordModel PasswordModel
        {
            get => passwordModel;
            set
            {
                passwordModel = value;
                OnPropertyChanged(nameof(PasswordModel));
            }
        }

        public string Password => GetPassword();
        public ICommand PasswordToClipoardCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand ConformationCommand { get; }

        public PasswordListItemViewModel(PasswordModel passwordModel,
            INavigationService closeNavigationService,
            ModalNavigationStore modalNavigationStore,
            PasswordListStore passwordListStore,
            bool isShowPassword = false)
        {
            ErrorMessage = string.Empty;
            PasswordModel = passwordModel;
            this.isShowPassword = isShowPassword;

            var edit = new EditPasswordViewModel(passwordListStore, passwordModel, closeNavigationService);
            NavigationModalService<EditPasswordViewModel> navigationService = new NavigationModalService<EditPasswordViewModel>(modalNavigationStore, () => edit);
            PasswordToClipoardCommand = new ReversePasswordToClipboardCommand(passwordModel.Password, passwordListStore);
            EditCommand = new NavigateCommand(navigationService);

            var conformation = new ConformationViewModel("Delete", "Do you really want to delete this item?");
            conformation.SetCommands(new DeletePasswordAsyncCommand(passwordListStore, this, closeNavigationService), new NavigateCommand(closeNavigationService));
            NavigationModalService<ConformationViewModel> conformationService = new NavigationModalService<ConformationViewModel>(modalNavigationStore, () => conformation);
            
            ConformationCommand = new NavigateCommand(conformationService);
            reversePasswordText = new ReversePasswordCommand(passwordModel.Password, passwordListStore);
        }

        private string GetPassword()
        {
            if (isShowPassword && PasswordModel.Password != null && PasswordModel.Password != "")
            {
                var result = reversePasswordText.ExecuteWithReturnObject(PasswordModel.Password);
                return result?.GetType()?.GetProperty("Result") != null ? result?.GetType()?.GetProperty("Result")?.GetValue(result).ToString() : PasswordModel.Password;
            }
            else if (isShowPassword)
            {
                return "";
            }
            else
            {
                return "●●●●●●●●";
            }
        }

        public void Update(PasswordModel passwordModel)
        {
            PasswordModel = passwordModel;
            OnPropertyChanged(nameof(PasswordModel));
        }
    }
}
