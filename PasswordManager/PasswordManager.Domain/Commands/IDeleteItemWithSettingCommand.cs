using PasswordManager.Domain.Models;

namespace PasswordManager.Domain.Commands
{
    //This will be implemented by the Data layer
    public interface IDeleteItemWithSettingCommand<T> where T :BaseModel
    {
        Task Execute(int id, SettingModel setting);
    }
}
