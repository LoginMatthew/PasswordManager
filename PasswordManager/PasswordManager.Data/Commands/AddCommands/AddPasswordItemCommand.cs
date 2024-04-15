using PasswordManager.Data.Services;
using PasswordManager.Domain.Commands;
using PasswordManager.Domain.Models;
using PasswordManager.Services.Encryption;
using PasswordManager.Stores;


namespace PasswordManager.Data.Commands.AddCommands
{
    public class AddPasswordItemCommand : IAddItemWithSettingCommand<PasswordModel>
    {
        public async Task<SettingModel> Execute(PasswordModel item, SettingModel setting)
        {
            RandomGenerator rg = new RandomGenerator();
            if (setting.UseEncryption)
            {
                string password = null;
                string key = null;
                if (setting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == "symetric rijndael")
                {
                    
                    if (setting.SourceSettingDataModel.SymetricKey != null && setting.SourceSettingDataModel.SymetricKey.Length > 0)
                        key = setting.SourceSettingDataModel.SymetricKey;

                    else
                        key = rg.GetRandomlyGeneratedStringWithOutNumber(16);

                    password = await EncryptionDecrytionWithRijndaelSymmetricService.EncryptWithKeyToString<string>(item.Password, key);

                    setting.SourceSettingDataModel.SymetricKey = key;
                    item.Password = password;
                }
                else if (setting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == "symetric aes")
                {
                    if (setting.SourceSettingDataModel.SymetricKey != null && setting.SourceSettingDataModel.SymetricKey.Length > 0)
                        key = setting.SourceSettingDataModel.SymetricKey;

                    else
                        key = rg.GetRandomlyGeneratedStringWithOutNumber(16);

                    password = await EncryptionDecrytionWithAESSymmetricService.EncryptString(item.Password, key);

                    setting.SourceSettingDataModel.SymetricKey = key;
                    item.Password = password;
                }
            }

            if (setting.SourceSettingDataModel.SelectedFileType.ToLower() == "json")
            {                
                FileJSONService<PasswordModel> fileJSONHandler = new FileJSONService<PasswordModel>();
                await fileJSONHandler.WriteOutADataRow(setting.SourceSettingDataModel.FolderPath, setting.SourceSettingDataModel.GetFileNameWithExtention(), item);
            }
            if(setting.SourceSettingDataModel.SelectedFileType.ToLower() == "txt")
            {
                
                FileTXTService fileHandler = new FileTXTService();
                await fileHandler.WriteOutADataRow(setting.SourceSettingDataModel.FolderPath, setting.SourceSettingDataModel.GetFileNameWithExtention(), item.ToWriteOutData());
            }

            return setting;
        }
    }
}
