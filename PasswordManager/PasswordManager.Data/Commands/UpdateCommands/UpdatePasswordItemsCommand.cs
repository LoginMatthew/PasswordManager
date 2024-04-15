using PasswordManager.Data.Services;
using PasswordManager.Domain.Commands;
using PasswordManager.Domain.Models;
using PasswordManager.Services.Encryption;
using PasswordManager.Stores;

namespace PasswordManager.Data.Commands.UpdateCommands
{
    public class UpdatePasswordItemsCommand : IUpdateItemsWithSettingCommand<PasswordModel>
    {
        private readonly RandomGenerator rg;
        private string key;

        public UpdatePasswordItemsCommand()
        {
            rg = new RandomGenerator();
        }

        private async Task<string> PasswordEncryption(string item, SettingModel newSetting, SettingModel oldSetting)
        {
            string hashedPassword = "";

            //Previously NO was set.
            if (!oldSetting.UseEncryption)
            {
                //No salt key is set
                if (newSetting.SourceSettingDataModel.SymetricKey == "" || newSetting.SourceSettingDataModel.SymetricKey == null)
                {
                    if (newSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == "symetric aes")
                    {
                        key = rg.GetRandomlyGeneratedStringWithOutNumber(16);
                        hashedPassword = await EncryptionDecrytionWithAESSymmetricService.EncryptString(item, key);
                    }
                    else if (newSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == "symetric rijndael")
                    {
                        key = rg.GetRandomlyGeneratedStringWithOutNumber(16);
                        hashedPassword = await EncryptionDecrytionWithRijndaelSymmetricService.EncryptWithKeyToString<string>(item, key);
                    }
                }
                //A valid key is set by user
                else if ((newSetting.SourceSettingDataModel.SymetricKey != "" || newSetting.SourceSettingDataModel.SymetricKey != null) && newSetting.SourceSettingDataModel.SymetricKey.Length == 16)
                {
                    if (newSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == "symetric aes")
                    {
                        key = newSetting.SourceSettingDataModel.SymetricKey;
                        hashedPassword = await EncryptionDecrytionWithAESSymmetricService.EncryptString(item, key);
                    }
                    else if(newSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == "symetric rijndael")
                    {
                        key = newSetting.SourceSettingDataModel.SymetricKey;
                        hashedPassword = await EncryptionDecrytionWithRijndaelSymmetricService.EncryptWithKeyToString<string>(item, key);
                    }
                }
            }
            //Previously WAS set.
            else if (oldSetting.UseEncryption)
            {
                //Previously no Key was set and selection is ok and no new key is set
                if ((oldSetting.SourceSettingDataModel.SymetricKey == "" || oldSetting.SourceSettingDataModel.SymetricKey == null) && (newSetting.SourceSettingDataModel.SymetricKey == "" || newSetting.SourceSettingDataModel.SymetricKey == null))
                {
                    if (oldSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == "symetric aes" && newSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == oldSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower())
                    {
                        key = rg.GetRandomlyGeneratedStringWithOutNumber(16);
                        hashedPassword = await EncryptionDecrytionWithAESSymmetricService.EncryptString(item, key);
                    }
                    if (oldSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == "symetric rijndael" && newSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == oldSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower())
                    {
                        key = rg.GetRandomlyGeneratedStringWithOutNumber(16);
                        hashedPassword = await EncryptionDecrytionWithRijndaelSymmetricService.EncryptWithKeyToString<string>(item, key);
                    }
                }

                //Previously Key was set and selection is ok and no new key is set
                else if ((oldSetting.SourceSettingDataModel.SymetricKey != "" || oldSetting.SourceSettingDataModel.SymetricKey != null) && (newSetting.SourceSettingDataModel.SymetricKey == "" || newSetting.SourceSettingDataModel.SymetricKey == null))
                {
                    string deryptedPassword = "";
                    if (oldSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == "symetric aes" && newSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == oldSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower())
                    {
                        key = oldSetting.SourceSettingDataModel.SymetricKey;
                        hashedPassword = await EncryptionDecrytionWithAESSymmetricService.EncryptString(item, key);
                    }
                    else if(oldSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == "symetric rijndael" && newSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == oldSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower())
                    {
                        key = oldSetting.SourceSettingDataModel.SymetricKey;
                        hashedPassword = await EncryptionDecrytionWithRijndaelSymmetricService.EncryptWithKeyToString<string>(item, key);
                    }
                    else if(oldSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == "symetric aes" && newSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == "symetric rijndael")
                    {
                        deryptedPassword = await EncryptionDecrytionWithAESSymmetricService.DecryptString(item, oldSetting.SourceSettingDataModel.SymetricKey);
                        key = oldSetting.SourceSettingDataModel.SymetricKey;
                        hashedPassword = await EncryptionDecrytionWithRijndaelSymmetricService.EncryptWithKeyToString<string>(deryptedPassword, key);
                    }
                    else if (oldSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == "symetric rijndael" && newSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == "symetric aes")
                    {
                        deryptedPassword = await EncryptionDecrytionWithRijndaelSymmetricService.DeryptDataFromByteArray<string>(item, oldSetting.SourceSettingDataModel.SymetricKey);
                        key = oldSetting.SourceSettingDataModel.SymetricKey;
                        hashedPassword = await EncryptionDecrytionWithAESSymmetricService.EncryptString(deryptedPassword, key);
                    }
                }
                //Previously Key was set and selection and (new key is set or same key)
                else if ((oldSetting.SourceSettingDataModel.SymetricKey != "" || oldSetting.SourceSettingDataModel.SymetricKey != null) && (newSetting.SourceSettingDataModel.SymetricKey != "" || newSetting.SourceSettingDataModel.SymetricKey == null) && newSetting.SourceSettingDataModel.SymetricKey.Length == 16)
                {
                    string deryptedPassword = "";
                    if (oldSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == "symetric aes" && newSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == oldSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower())
                    {
                        deryptedPassword = await EncryptionDecrytionWithAESSymmetricService.DecryptString(item, oldSetting.SourceSettingDataModel.SymetricKey);
                        key = newSetting.SourceSettingDataModel.SymetricKey;
                        hashedPassword = await EncryptionDecrytionWithAESSymmetricService.EncryptString(deryptedPassword, key);
                    }
                    else if(oldSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == "symetric rijndael" && newSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == oldSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower())
                    {
                        deryptedPassword = await EncryptionDecrytionWithRijndaelSymmetricService.DeryptDataFromByteArray<string>(item, oldSetting.SourceSettingDataModel.SymetricKey);
                        key = newSetting.SourceSettingDataModel.SymetricKey;
                        hashedPassword = await EncryptionDecrytionWithRijndaelSymmetricService.EncryptWithKeyToString<string>(deryptedPassword, key);
                    }
                    else if (oldSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == "symetric rijndael" && newSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == "symetric aes")
                    {
                        deryptedPassword = await EncryptionDecrytionWithRijndaelSymmetricService.DeryptDataFromByteArray<string>(item, oldSetting.SourceSettingDataModel.SymetricKey);
                        key = newSetting.SourceSettingDataModel.SymetricKey;
                        hashedPassword = await EncryptionDecrytionWithAESSymmetricService.EncryptString(deryptedPassword, key);
                    }
                    else if (oldSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == "symetric aes" && newSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == "symetric rijndael")
                    {
                        deryptedPassword = await EncryptionDecrytionWithAESSymmetricService.DecryptString(item, oldSetting.SourceSettingDataModel.SymetricKey);
                        key = newSetting.SourceSettingDataModel.SymetricKey;
                        hashedPassword = await EncryptionDecrytionWithRijndaelSymmetricService.EncryptWithKeyToString<string>(deryptedPassword, key);
                    }
                }
            }

            return hashedPassword;
        }

        private async Task<string> RijndaelDecryptPassword(string item, SettingModel newSetting, SettingModel oldSetting)
        {
            string hashedPassword = "";

            //Previously key WAS set.
            if (oldSetting.UseEncryption)
            {
                //Use old key and No new key
                if (oldSetting.SourceSettingDataModel.SymetricKey != "" || oldSetting.SourceSettingDataModel.SymetricKey != null)
                {
                    hashedPassword = await EncryptionDecrytionWithRijndaelSymmetricService.DeryptDataFromByteArray<string>(item, oldSetting.SourceSettingDataModel.SymetricKey);
                }
            }

            return hashedPassword;
        }

        private async Task<string> AESDecryptPassword(string item, SettingModel newSetting, SettingModel oldSetting)
        {
            string hashedPassword = "";

            //Previously key WAS set.
            if (oldSetting.UseEncryption)
            {
                //Use old key and No new key
                if (oldSetting.SourceSettingDataModel.SymetricKey != "" || oldSetting.SourceSettingDataModel.SymetricKey != null)
                {
                    hashedPassword = await EncryptionDecrytionWithAESSymmetricService.DecryptString(item, oldSetting.SourceSettingDataModel.SymetricKey);
                }
            }

            return hashedPassword;
        }

        public async Task<SettingModel> Execute(List<PasswordModel> items, SettingModel newSetting, SettingModel oldSetting)
        {

            if ((newSetting.SourceSettingDataModel.SelectedFileType != null && newSetting.SourceSettingDataModel.SelectedEncryptionType != null) || newSetting.UseEmbeddedDefaultDataSetting)
            {
                foreach (var item in items)
                {
                    if (newSetting.UseEncryption)
                    {
                        item.Password = await PasswordEncryption(item.Password, newSetting, oldSetting);
                        newSetting.SourceSettingDataModel.SymetricKey = key;
                    }
                    //Remove Encryption
                    else if (oldSetting.UseEncryption || newSetting.UseEmbeddedDefaultDataSetting)
                    {
                        if (oldSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == "symetric rijndael")
                        {
                            item.Password = await RijndaelDecryptPassword(item.Password, newSetting, oldSetting);
                            newSetting.SourceSettingDataModel.SymetricKey = "";
                        }
                        else if (oldSetting.SourceSettingDataModel.SelectedEncryptionType.ToLower() == "symetric aes")
                        {
                            item.Password = await AESDecryptPassword(item.Password, newSetting, oldSetting);
                            newSetting.SourceSettingDataModel.SymetricKey = "";
                        }
                    }
                }

                if (newSetting.SourceSettingDataModel.SelectedFileType.ToLower() == "json")
                {
                    FileJSONService<PasswordModel> fileJSONHandler = new FileJSONService<PasswordModel>();
                    await fileJSONHandler.WriteOutADataListRows(newSetting.SourceSettingDataModel.FolderPath, newSetting.SourceSettingDataModel.GetFileNameWithExtention(), items);
                }

                else if (newSetting.SourceSettingDataModel.SelectedFileType.ToLower() == "txt")
                {
                    FileTXTService fileHandler = new FileTXTService();
                    List<string> dataRows = new List<string>();

                    foreach (var item in items)
                        dataRows.Add(item.ToWriteOutData());

                    await fileHandler.WriteOutADataRows(newSetting.SourceSettingDataModel.FolderPath, newSetting.SourceSettingDataModel.GetFileNameWithExtention(), dataRows);
                }
            }

            return newSetting;
        }
    }
}
