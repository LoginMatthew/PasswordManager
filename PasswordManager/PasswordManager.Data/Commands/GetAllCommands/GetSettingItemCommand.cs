using PasswordManager.Data.Constants;
using PasswordManager.Data.Services;
using PasswordManager.Domain.Commands;
using PasswordManager.Domain.Models;

namespace PasswordManager.Data.Commands.GetAllCommands
{
    //This will be implemented by the Data layer
    public class GetSettingItemCommand : IGetItemWithSettingCommand<SettingModel>
    {
        public async Task<SettingModel> Execute(SettingModel settingModel = null)
        {
            FileJSONService<SettingModel> jSONService = new FileJSONService<SettingModel>();
            string folderName = "Setting";

            HandlerFolderFile.CheckFolderStructuresWithUniqueName(folderName);
            string fileName = "Setting.json";

            //If there is no setting file or use the default then it will be called.
            if (!HandlerFolderFile.IsFileExistInGivenFolder(fileName, folderName) || settingModel.UseEmbeddedDefaultDataSetting)
            {
                settingModel = EmbededSetting.GetDefaultEmbededData();
            }
            else
            {
                settingModel = await jSONService.GetData(folderName, fileName);
            }

            return settingModel;
        }
    }
}
