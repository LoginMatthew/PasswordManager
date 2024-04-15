using PasswordManager.Domain.Models;
using PasswordManager.Services;
using System.Windows;
using PasswordManager.Stores;
using PasswordManager.ViewModels.PasswordViewModels.Components;

namespace PasswordManager.Commands.AddCommands
{
    public class AddPasswordAsyncCommand : AsyncCommandBase
    {
        private readonly AddPasswordViewModel addPasswordViewModel;
        private readonly INavigationService _navigationService;
        private readonly PasswordListStore passwordListStore;

        public AddPasswordAsyncCommand(AddPasswordViewModel addPasswordViewModel, PasswordListStore passwordListStore, INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.passwordListStore = passwordListStore;
            this.addPasswordViewModel = addPasswordViewModel;
        }

        public override bool CanExecute(object paramter)
        {
            return base.CanExecute(paramter);

        }

        public override async Task ExecuteAsync(object parameter)
        {
            addPasswordViewModel.ContentPasswordDetailFormViewModel.IsSubmitting = true;
            addPasswordViewModel.ContentPasswordDetailFormViewModel.ErrorMessage = string.Empty;
            var detailForm = addPasswordViewModel.ContentPasswordDetailFormViewModel;

            try
            {
                var foundPassword = passwordListStore.Passwords.FirstOrDefault(x => x.Site == detailForm.Site && x.Email == detailForm.Email);

                if (foundPassword == null)
                {
                    PasswordModel newSite = new PasswordModel(passwordListStore.Passwords.Count() > 0 ? passwordListStore.Passwords.OrderBy(x => x.Id).LastOrDefault().Id + 1 : 1, detailForm.Site, detailForm.Password, DateTime.Now, detailForm.Email, detailForm.UserName == null ? "" : detailForm.UserName, detailForm.Description == null ? "" : detailForm.Description);

                    await passwordListStore?.AddedPassword(newSite);
                    _navigationService.Navigate();
                }
                else
                {
                    addPasswordViewModel.ContentPasswordDetailFormViewModel.ErrorMessage = "This email address is already assigned to this site!";
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Failled to added" + e, "Error");
                addPasswordViewModel.ContentPasswordDetailFormViewModel.ErrorMessage = "Failed to add item. Please try again later.";
            }
            finally
            {
                addPasswordViewModel.ContentPasswordDetailFormViewModel.IsSubmitting = false;
            }
        }
    }
}

