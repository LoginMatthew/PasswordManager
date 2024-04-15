
using PasswordManager.Commands;
using PasswordManager.Services;
using PasswordManager.Domain.Models;
using PasswordManager.Stores;
using PasswordManager.Commands.UpdateCommands;

namespace PasswordManager.ViewModels.PasswordViewModels.Components
{
    public class EditPasswordViewModel : ViewModelBase
    {        
        public string PageHeader => "Edit Data";
        public PasswordDetailFormViewModel ContentPasswordDetailFormViewModel { get; set; }
        
        public EditPasswordViewModel(PasswordListStore passwordListStore, PasswordModel passwordModel,
            INavigationService closeNavigationService)
        {
            PasswordDetailFormViewModel passwordDetailFormViewModel = new PasswordDetailFormViewModel("Modify");
            passwordDetailFormViewModel.Id = passwordModel.Id;
            passwordDetailFormViewModel.Site = passwordModel.Site;
            passwordDetailFormViewModel.Email = passwordModel.Email;
            passwordDetailFormViewModel.Description = passwordModel.Description;
            passwordDetailFormViewModel.Password = passwordModel.Password;
            passwordDetailFormViewModel.UserName = passwordModel.UserName;
            EditPasswordViewModel thisView = this;
            passwordDetailFormViewModel.SetCommand(new UpdatePasswordAsyncCommand(thisView, closeNavigationService, passwordListStore), new NavigateCommand(closeNavigationService), new ReversePasswordCommand(passwordModel.Password, passwordListStore));
            ContentPasswordDetailFormViewModel = passwordDetailFormViewModel;
        }
    }
}
