using PasswordManager.Stores;

namespace PasswordManager.Commands
{
    public class ReversePasswordCommand : AsyncCommandBaseWithReturn
    {
        private readonly PasswordListStore passwordStore;

        public ReversePasswordCommand(string password, PasswordListStore passwordStore)
        {
            this.passwordStore = passwordStore;
        }

        public override async Task<object> ExecuteWithReturnObjectAsync(object parameter)
        {
            if(parameter is string && parameter != null && parameter != "")
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

                return result;
            }

            return  null;
        }

        public override async Task ExecuteAsync(object parameter) => throw new NotImplementedException();
    }
}

