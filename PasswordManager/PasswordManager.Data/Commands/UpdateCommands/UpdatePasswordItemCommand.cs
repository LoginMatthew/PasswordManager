using PasswordManager.Data.Services;
using PasswordManager.Domain.Commands;
using PasswordManager.Domain.Models;
using PasswordManager.Services.Encryption;

namespace PasswordManager.Data.Commands.UpdateCommands
{
    public class UpdatePasswordItemCommand : IUpdateItemWithSettingCommand<PasswordModel>
    {
        public async Task Execute(PasswordModel item, SettingModel setting)
        {
            string password = item.Password;

            if(setting.UseEncryption && setting.SourceSettingDataModel.SymetricKey != null)
            {
                if (setting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == "symetric rijndael")
                {
                    password = await EncryptionDecrytionWithRijndaelSymmetricService.EncryptWithKeyToString<string>(password, setting.SourceSettingDataModel.SymetricKey);
                }
                else if (setting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == "symetric aes")
                {
                    password = await EncryptionDecrytionWithAESSymmetricService.EncryptString(password, setting.SourceSettingDataModel.SymetricKey);
                }
            }

            item.Password = password;

            if (setting.SourceSettingDataModel.SelectedFileType.ToLower() == "json")
            {
                FileJSONService<PasswordModel> fileJSONHandler = new FileJSONService<PasswordModel>();
                await fileJSONHandler.UpdateADataRow(setting.SourceSettingDataModel.FolderPath, setting.SourceSettingDataModel.GetFileNameWithExtention(), item);
            }
            if (setting.SourceSettingDataModel.SelectedFileType.ToLower() == "txt")
            {
                FileTXTService fileHandler = new FileTXTService();
                await fileHandler.UpdateADataRow(setting.SourceSettingDataModel.FolderPath, setting.SourceSettingDataModel.GetFileNameWithExtention(), item.Id.ToString(), item.ToWriteOutData());
            }
        }
    }
}
