using System.Windows;
using PasswordManager.Stores;

namespace PasswordManager.Commands.LoadCommands
{
    public class LoadSettingCommand : AsyncCommandBase
    {
        private readonly SettingDataStore settingDataStore;

        public LoadSettingCommand(SettingDataStore settingDataStore)
        {
            this.settingDataStore = settingDataStore;
        }

        public override async Task ExecuteAsync(object paramter)
        {
            try
            {
                await settingDataStore.Load();
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong during loading setting data!\n" + e, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

