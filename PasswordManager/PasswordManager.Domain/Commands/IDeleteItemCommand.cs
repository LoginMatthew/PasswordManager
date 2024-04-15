using PasswordManager.Domain.Models;

namespace PasswordManager.Domain.Commands
{
    public interface IDeleteItemCommand<T> where T :BaseModel
    {
        Task Execute(int id);
    }
}
