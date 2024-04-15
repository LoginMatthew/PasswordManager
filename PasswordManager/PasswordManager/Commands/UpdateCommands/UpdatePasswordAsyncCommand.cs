using PasswordManager.Domain.Models;
using PasswordManager.Services;
using System.Windows;
using PasswordManager.Stores;
using PasswordManager.ViewModels.PasswordViewModels.Components;

namespace PasswordManager.Commands.UpdateCommands
{
    public class UpdatePasswordAsyncCommand : AsyncCommandBase
    {
        private readonly INavigationService navigationService;
        private readonly PasswordListStore passwordListStore;
        private readonly EditPasswordViewModel editPasswordViewModel;

        public UpdatePasswordAsyncCommand(EditPasswordViewModel editPasswordViewModel, INavigationService navigationService, PasswordListStore passwordListStore)
        {
            this.navigationService = navigationService;
            this.editPasswordViewModel = editPasswordViewModel;
            this.passwordListStore = passwordListStore;
        }

        public override bool CanExecute(object paramter)
        {
            return base.CanExecute(paramter);
        }

        public override async Task ExecuteAsync(object parameter)
        {
            editPasswordViewModel.ContentPasswordDetailFormViewModel.IsSubmitting = true;
            editPasswordViewModel.ContentPasswordDetailFormViewModel.ErrorMessage = string.Empty;
            var detailForm = editPasswordViewModel.ContentPasswordDetailFormViewModel;
            var item = new PasswordModel(detailForm.Id != null ? detailForm.Id : -1, detailForm.Site, detailForm.Password, DateTime.Now, detailForm.Email, detailForm.UserName == null ? "" : detailForm.UserName, detailForm.Description == null ? "" : detailForm.Description);
            
            try
            {
                if (passwordListStore.Passwords.FirstOrDefault(x => x.Site == item.Site && x.Email == item.Email && x.Id != item.Id) != null)
                {
                    editPasswordViewModel.ContentPasswordDetailFormViewModel.ErrorMessage = "This email address is already assigned to this site!";
                }
                else
                {
                    await passwordListStore.UpdatePassword(item);
                    navigationService.Navigate();
                }
            }
            catch (Exception e)
            {
                editPasswordViewModel.ContentPasswordDetailFormViewModel.ErrorMessage = "Failed to edit item. Please try again later.";
                MessageBox.Show("Failed in upadation!\n" + e, "Error");
            }
            finally
            {
                editPasswordViewModel.ContentPasswordDetailFormViewModel.IsSubmitting = false;
            }
        }
    }
}

