using PasswordManager.Domain.Models;

namespace PasswordManager.Domain.Commands
{
    public interface IAddItemWithSettingCommand<T> where T :BaseModel
    {
        Task<SettingModel> Execute(T item, SettingModel setting);

    }
}
