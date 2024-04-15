using PasswordManager.Commands;
using PasswordManager.Commands.AddCommands;
using PasswordManager.Services;
using PasswordManager.Stores;

namespace PasswordManager.ViewModels.PasswordViewModels.Components
{
    public class AddPasswordViewModel : ViewModelBase
    {
        public string PageHeader => "Add new Site";
        public PasswordDetailFormViewModel ContentPasswordDetailFormViewModel { get; set; }

        public AddPasswordViewModel(PasswordDetailFormViewModel passwordDetailFormViewModel, PasswordListStore passwordListStore, INavigationService closeNavigationService)
        {
            passwordDetailFormViewModel.SetCommand(new AddPasswordAsyncCommand(this, passwordListStore, closeNavigationService), new NavigateCommand(closeNavigationService));
            ContentPasswordDetailFormViewModel = passwordDetailFormViewModel;
        }
    }
}
