using PasswordManager.Domain.Models;

namespace PasswordManager.Domain.Commands
{
    public interface IGetAllItemWithSettingCommand<T> where T :BaseModel
    {
        Task<IEnumerable<T>> Excute(SettingModel setting);
    }
}
