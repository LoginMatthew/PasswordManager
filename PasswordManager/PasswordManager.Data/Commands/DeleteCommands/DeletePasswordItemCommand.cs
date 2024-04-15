using PasswordManager.Data.Services;
using PasswordManager.Domain.Commands;
using PasswordManager.Domain.Models;


namespace PasswordManager.Data.Commands.DeleteCommands
{
    public class DeletePasswordItemCommand : IDeleteItemWithSettingCommand<PasswordModel>
    {
        public async Task Execute(int id, SettingModel setting)
        {
            if (setting.SourceSettingDataModel.SelectedFileType.ToLower() == "json")
            {
                FileJSONService<PasswordModel> fileJSONHandler = new FileJSONService<PasswordModel>();
                await fileJSONHandler.DeleteADataRow(setting.SourceSettingDataModel.FolderPath, setting.SourceSettingDataModel.GetFileNameWithExtention(), id.ToString());
            }
            if (setting.SourceSettingDataModel.SelectedFileType.ToLower() == "txt")
            {
                FileTXTService fileHandler = new FileTXTService();
                await fileHandler.DeleteADataRow(setting.SourceSettingDataModel.FolderPath, setting.SourceSettingDataModel.GetFileNameWithExtention(), id.ToString());
            }
        }
    }
}
