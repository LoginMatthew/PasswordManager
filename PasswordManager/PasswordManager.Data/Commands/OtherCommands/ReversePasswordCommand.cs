
using PasswordManager.Domain.Commands;
using PasswordManager.Domain.Models;
using PasswordManager.Services.Encryption;

namespace PasswordManager.Data.Commands.OtherCommands
{
    public class ReversePasswordCommand : IReversePasswordCommand
    {
        public async Task<string> Execute(string item, SettingModel setting)
        {
            string dercyptedPassword = "";

            if ((setting.SourceSettingDataModel.SelectedEncryptionType != null || setting.SourceSettingDataModel.SelectedEncryptionType != "") && setting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == "symetric rijndael")
            {
                dercyptedPassword = await EncryptionDecrytionWithRijndaelSymmetricService.DeryptDataFromByteArray<string>(item, setting.SourceSettingDataModel.SymetricKey);
            }
            if ((setting.SourceSettingDataModel.SelectedEncryptionType != null || setting.SourceSettingDataModel.SelectedEncryptionType != "") && setting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == "symetric aes")
            {
                dercyptedPassword =  await EncryptionDecrytionWithAESSymmetricService.DecryptString(item, setting.SourceSettingDataModel.SymetricKey);
            }

            return dercyptedPassword;
        }
    }
}
