using PasswordManager.Stores;
using System.Windows;
using PasswordManager.ViewModels.PasswordViewModels.Components;
using PasswordManager.Services;

namespace PasswordManager.Commands.DeleteCommands
{
    public class DeletePasswordAsyncCommand : AsyncCommandBase
    {
        private readonly PasswordListStore passwordListStore;
        private readonly PasswordListItemViewModel passwordListItemViewModel;
        private readonly INavigationService _navigationService;

        public DeletePasswordAsyncCommand(PasswordListStore passwordListStore, PasswordListItemViewModel passwordListItemViewModel, INavigationService navigationService)
        {
            this.passwordListItemViewModel = passwordListItemViewModel;
            this.passwordListStore = passwordListStore;
            this._navigationService = navigationService;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            passwordListItemViewModel.IsDeleting = true;
            passwordListItemViewModel.ErrorMessage = string.Empty;
            try
            {
                var passwordItem = passwordListStore.Passwords.Where(x => x.Id == passwordListItemViewModel.PasswordModel.Id).FirstOrDefault();                
                await passwordListStore.DeletePassword(passwordItem.Id);
                _navigationService.Navigate();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error in deletion:\n " + e, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                passwordListItemViewModel.ErrorMessage = "Failed to delete item. Please try again later.";
            }
            finally
            {
                passwordListItemViewModel.IsDeleting = false;
            }
        }
    }
}

