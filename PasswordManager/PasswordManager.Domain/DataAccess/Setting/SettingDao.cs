using PasswordManager.Domain.Commands;
using PasswordManager.Domain.Models;

namespace PasswordManager.Domain.DataAccess.Setting
{
    public class SettingDao : SettingBaseDao<SettingModel>
    {
        public SettingDao(IGetItemWithSettingCommand<SettingModel> getItemCommand,
            IAddItemReturnWithItCommand<SettingModel> addItemCommand,
            IUpdateItemReturnWithItCommand<SettingModel> updateItemCommand) : base(getItemCommand, addItemCommand, updateItemCommand)
        {

        }
    }
}
