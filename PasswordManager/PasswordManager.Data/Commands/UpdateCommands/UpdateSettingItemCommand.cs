using PasswordManager.Data.Constants;
using PasswordManager.Data.Services;
using PasswordManager.Domain.Commands;
using PasswordManager.Domain.Models;


namespace PasswordManager.Data.Commands.UpdateCommands
{
    /// <summary>
    /// Reutrn with default data
    /// </summary>
    public class UpdateSettingItemCommand : IUpdateItemReturnWithItCommand<SettingModel>
    {
        public async Task<SettingModel> Execute(SettingModel item)
        {
            FileJSONService<SettingModel> fileJSONHandler = new FileJSONService<SettingModel>();

            if (item.UseEmbeddedDefaultDataSetting)
                item = EmbededSetting.GetDefaultEmbededData();

            await fileJSONHandler.WriteOutADataRow("Setting", "Setting.json", item, true);

            return item;
        }
    }
}
