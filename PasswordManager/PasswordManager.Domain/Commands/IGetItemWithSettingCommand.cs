using PasswordManager.Domain.Models;

namespace PasswordManager.Domain.Commands
{
    public interface IGetItemWithSettingCommand<T> where T :BaseModel
    {
        Task<T> Execute(SettingModel setting = null);
    }
}
