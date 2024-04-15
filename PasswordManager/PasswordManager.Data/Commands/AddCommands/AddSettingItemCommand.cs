using PasswordManager.Data.Constants;
using PasswordManager.Data.Services;
using PasswordManager.Domain.Commands;
using PasswordManager.Domain.Models;

namespace PasswordManager.Data.Commands.AddCommands
{
    //This will be implemented by the Data layer
    public class AddSettingItemCommand : IAddItemReturnWithItCommand<SettingModel>
    {
        FileJSONService<SettingModel> fileJSONHandler = new FileJSONService<SettingModel>();
        public async Task<SettingModel> Execute(SettingModel item)
        {
            if (item.UseEmbeddedDefaultDataSetting)
                item = EmbededSetting.GetDefaultEmbededData();

            await fileJSONHandler.WriteOutADataRow("Setting", "Setting.json", item, true);

            return item;
        }
    }
}
