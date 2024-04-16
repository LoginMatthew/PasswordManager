using PasswordManager.Domain.Models;
using PasswordManager.Services;
using System.Windows;
using PasswordManager.Stores;
using PasswordManager.ViewModels.PasswordViewModels.Components;

namespace PasswordManager.Commands.AddCommands
{
    public class SearchPasswordAsyncCommand : AsyncCommandBase
    {
        private readonly SearchPasswordViewModel searchPasswordViewModel;
        private readonly INavigationService _navigationService;
        private readonly PasswordListStore passwordListStore;

        public SearchPasswordAsyncCommand(SearchPasswordViewModel searchPasswordViewModel, PasswordListStore passwordListStore, INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.passwordListStore = passwordListStore;
            this.searchPasswordViewModel = searchPasswordViewModel;
        }

        public override bool CanExecute(object paramter)
        {
            return base.CanExecute(paramter);

        }

        public override async Task ExecuteAsync(object parameter)
        {
            string searchText = searchPasswordViewModel.SearchText;

            try
            {
                await passwordListStore.GetAllItem();
                List<PasswordModel> foundPasswords = passwordListStore.Passwords.Where(x => x.Site.ToUpper().Contains(searchText.ToUpper())).ToList();

                if (foundPasswords != null)
                {
                    await passwordListStore?.SearchPassword(foundPasswords as List<PasswordModel>);
                    searchPasswordViewModel.SearchText = "";
                    _navigationService.Navigate();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Failled in searching" + e, "Error");
            }
        }
    }
}

