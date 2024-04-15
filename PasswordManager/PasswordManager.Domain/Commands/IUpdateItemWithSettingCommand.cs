using PasswordManager.Domain.Models;

namespace PasswordManager.Domain.Commands
{
    public interface IUpdateItemWithSettingCommand<T> where T :BaseModel
    {
        Task Execute(T item, SettingModel setting);
    }
}
