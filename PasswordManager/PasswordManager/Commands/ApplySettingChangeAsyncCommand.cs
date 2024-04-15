using PasswordManager.Domain.Models;
using PasswordManager.ViewModels;
using System.Windows;
using PasswordManager.Stores;
using System.Text.RegularExpressions;

namespace PasswordManager.Commands
{
    public class ApplySettingChangeAsyncCommand : AsyncCommandBase
    {
        private readonly SettingViewModel settingViewModel;
        private readonly SettingDataStore settingDataStore;
        private readonly PasswordListStore passwordListStore;
        private SettingModel newSettingModel;

        public ApplySettingChangeAsyncCommand(SettingViewModel settingViewModel, SettingDataStore settingDataStore, PasswordListStore passwordListStore)
        {
            this.settingDataStore = settingDataStore;
            this.settingViewModel = settingViewModel;
            this.passwordListStore = passwordListStore;
        }

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object parameter)
        {
            newSettingModel = new SettingModel();

            newSettingModel.UseEmbeddedDefaultDataSetting = settingViewModel.EmbededDataSetting;
            newSettingModel.UseEncryption = settingViewModel.UseEncryption;
            newSettingModel.SourceSettingDataModel.SymetricKey = settingViewModel.SymetricKey != null && settingViewModel.SymetricKey !="" ? settingViewModel.SymetricKey.ToUpper() : settingViewModel.SymetricKey;
            newSettingModel.SourceSettingDataModel.FileName = settingViewModel.SourceFileName;
            newSettingModel.SourceSettingDataModel.FolderPath = settingDataStore.Setting.SourceSettingDataModel.FolderPath;
            newSettingModel.SourceSettingDataModel.SelectedFileType = settingViewModel.SelectedSourceFileType;
            newSettingModel.SourceSettingDataModel.SelectedEncryptionType = settingViewModel.SelectedSourceEncrpytionType;

            try
            {
                if (!newSettingModel.UseEmbeddedDefaultDataSetting)
                {
                    if (settingDataStore.Setting.SourceSettingDataModel.FileName != settingViewModel.SourceFileName ||
                        settingDataStore.Setting.SourceSettingDataModel.SelectedFileType != settingViewModel.SelectedSourceFileType)
                    {
                        await PaswordLoad();
                    }

                    if (newSettingModel.UseEncryption && (newSettingModel.SourceSettingDataModel.SelectedEncryptionType != "" || newSettingModel.SourceSettingDataModel.SelectedEncryptionType != null ||
                        settingDataStore.Setting.SourceSettingDataModel.SelectedEncryptionType != null || settingDataStore.Setting.SourceSettingDataModel.SelectedEncryptionType != ""))
                    {
                        if (settingViewModel.SelectedSourceEncrpytionType == null)
                        {
                            settingViewModel.MessageText = "No Encryption type was selected!";
                        }
                        else if (settingViewModel.UseUniqueSymetricKey && (settingViewModel.SymetricKey == null || settingViewModel.SymetricKey == ""))
                        {
                            settingViewModel.MessageText = "Symetric key was not set!";
                        }
                        else if (settingViewModel.UseUniqueSymetricKey && (settingViewModel.SymetricKey != null || settingViewModel.SymetricKey != "") && settingDataStore.Setting.SourceSettingDataModel.SymetricKey == settingViewModel.SymetricKey)
                        {
                            settingViewModel.MessageText = "Symetric key was not modified!";
                        }
                        else if (settingViewModel.UseUniqueSymetricKey && (settingViewModel.SymetricKey != null && settingViewModel.SymetricKey != "" && !Regex.IsMatch(settingViewModel.SymetricKey, @"^[a-zA-Z]+$")) )
                        {
                            settingViewModel.MessageText = "Symetric key contains invalid character(s)!";
                        }
                        else if (settingViewModel.UseUniqueSymetricKey && (settingViewModel.SymetricKey != null && settingViewModel.SymetricKey != "" && Regex.IsMatch(settingViewModel.SymetricKey, @"^[a-zA-Z]+$") && settingViewModel.SymetricKey.Length != 16))
                        {
                            settingViewModel.MessageText = "Symetric key length is invalid!";
                        }
                        else
                        {
                            await UpdateSetting("Update Done.");
                        }
                        if (settingViewModel.UseUniqueSymetricKey)
                            settingViewModel.UseUniqueSymetricKey = false;
                    }
                    else
                    {
                        settingViewModel.MessageText = "Encryption type was not selected!";
                    }

                    if(!newSettingModel.UseEncryption)
                    {
                        settingViewModel.UseUniqueSymetricKey = false;
                        newSettingModel.SourceSettingDataModel.SymetricKey = "";
                        newSettingModel.SourceSettingDataModel.SelectedEncryptionType = "";
                        settingViewModel.SelectedSourceEncrpytionType = null;
                        await UpdateSetting("Update Done.");
                    }
                }
                else
                {
                    await UpdateSetting("Embeded data setting is used!");

                    settingViewModel.UseUniqueSymetricKey = false;
                    settingViewModel.UseEncryption = settingDataStore.Setting.UseEncryption;
                    settingViewModel.SymetricKey = settingDataStore.Setting.SourceSettingDataModel.SymetricKey;
                    settingViewModel.SourceFileName = settingDataStore.Setting.SourceSettingDataModel.FileName;
                    settingViewModel.SelectedSourceFileType = settingDataStore.Setting.SourceSettingDataModel.SelectedFileType;
                    settingViewModel.SelectedSourceEncrpytionType = null;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Failled to added" + e, "Error");
                settingViewModel.MessageText = "Failed to upadte item.";
            }
            finally
            {
                settingDataStore.Setting = settingDataStore.Setting;
            }
        }

        /// <summary>
        /// When PasswordModelView has never been opened before
        /// </summary>
        private async Task PaswordLoad()
        {            
            if (passwordListStore.Passwords.Count() <= 0)
                await passwordListStore.Load();
        }

        private async Task UpdateSetting(string text)
        {
            await PaswordLoad();
            await settingDataStore.UpdateSetting(newSettingModel, settingDataStore.Setting);
            settingViewModel.MessageText = text;
        }
    }
}

