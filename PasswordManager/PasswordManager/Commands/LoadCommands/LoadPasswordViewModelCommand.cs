using System.Windows;
using PasswordManager.Stores;
using PasswordManager.ViewModels.PasswordViewModels.Components;
using PasswordManager.Services;

namespace PasswordManager.Commands
{
    public class LoadPasswordViewModelCommand : AsyncCommandBase
    {
        private readonly PasswordListStore passwordListStore;
        private readonly PasswordListViewModel viewModel;

        public LoadPasswordViewModelCommand(PasswordListStore passwordListStore, PasswordListViewModel viewModel)
        {
            this.passwordListStore = passwordListStore;
            this.viewModel = viewModel;
        }

        public override async Task ExecuteAsync(object paramter)
        {
            viewModel.IsLoading = true;
            viewModel.ErrorMessage = string.Empty;

            try
            {
                await passwordListStore.Load();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error in failed in loading\n" + e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //"finally" part can be skipped
            finally
            {
                viewModel.IsLoading = false;
            }
        }
    }

}

