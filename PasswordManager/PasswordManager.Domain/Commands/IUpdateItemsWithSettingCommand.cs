using PasswordManager.Domain.Models;

namespace PasswordManager.Domain.Commands
{
    public interface IUpdateItemsWithSettingCommand<T> where T :BaseModel
    {
        /// <summary>
        /// Change in password storage mode and update setting hasing cases
        /// </summary>>
        Task<SettingModel> Execute(List<T> items, SettingModel newSetting, SettingModel oldSetting);

    }
}
