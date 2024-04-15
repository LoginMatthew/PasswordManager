using PasswordManager.Domain.Models;

namespace PasswordManager.Data.Constants
{
    /// <summary>
    /// If there is no "settings" file or use default settings, then use these data.
    /// </summary>
    internal static class EmbededSetting
    {
        public static SettingModel GetDefaultEmbededData()
        {
            SettingModel settingModel = new SettingModel();
            settingModel.SourceSettingDataModel = new SettingDataModel();
            settingModel.UseEmbeddedDefaultDataSetting = true;
            settingModel.UseEncryption = false;

            settingModel.SourceSettingDataModel.FolderPath = "Pwd Files";
            settingModel.SourceSettingDataModel.FileName = "plainPasswords";
            settingModel.SourceSettingDataModel.SelectedFileType = "JSON";

            return settingModel;
        }
    }
}
