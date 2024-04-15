using PasswordManager.Data.Services;
using PasswordManager.Domain.Commands;
using PasswordManager.Domain.Models;

namespace PasswordManager.Data.Commands.GetAllCommands
{
    public class GetAllPasswordItemCommand : IGetAllItemWithSettingCommand<PasswordModel>
    {
        public async Task<IEnumerable<PasswordModel>> Excute(SettingModel setting)
        {
            List<PasswordModel> passwordModels = new List<PasswordModel>();

            try
            {
                if (setting.SourceSettingDataModel.SelectedFileType != null)
                {
                    if (setting.SourceSettingDataModel.SelectedFileType.ToLower() == "json")
                    {
                        FileJSONService<PasswordModel> fileJSONHandler = new FileJSONService<PasswordModel>();
                        passwordModels = (await fileJSONHandler.GetDataRows(setting.SourceSettingDataModel.FolderPath, setting.SourceSettingDataModel.GetFileNameWithExtention()) as List<PasswordModel>);
                    }
                    else if (setting.SourceSettingDataModel.SelectedFileType.ToLower() == "txt")
                    {
                        FileTXTService fileHandler = new FileTXTService();

                        IEnumerable<string> dataRows = new List<string>();

                        dataRows = await fileHandler.GetDataRows(setting.SourceSettingDataModel.FolderPath, setting.SourceSettingDataModel.GetFileNameWithExtention());

                        foreach (var row in dataRows)
                        {
                            string[] lineColumns = row.Split(";");
                            passwordModels.Add(new PasswordModel(int.Parse(lineColumns[0]), lineColumns[1], lineColumns[2], DateTime.Parse(lineColumns[3]), lineColumns[4], lineColumns[5] == "null" ? null : lineColumns[5], lineColumns[6] == "null" ? null : lineColumns[6]));
                        }
                    }
                }
                else
                {
                    throw new Exception("No selected file type");
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return passwordModels;
        }
    }
}
