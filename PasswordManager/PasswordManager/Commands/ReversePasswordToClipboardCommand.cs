using PasswordManager.Stores;
using System.Windows;

namespace PasswordManager.Commands
{
    public class ReversePasswordToClipboardCommand : AsyncCommandBase
    {
        private readonly PasswordListStore passwordStore;

        public ReversePasswordToClipboardCommand(string password, PasswordListStore passwordStore)
        {
            this.passwordStore = passwordStore;
        }
        public override async Task ExecuteAsync(object parameter)
        {
            string result = "";

            if (passwordStore.SettingDataStore.Setting.UseEncryption)
            {
                result = await passwordStore.GetReversePassword(parameter.ToString());
            }
            else
            {
                result = parameter.ToString();
            }

            Clipboard.SetText(result);
        }
    }
}

