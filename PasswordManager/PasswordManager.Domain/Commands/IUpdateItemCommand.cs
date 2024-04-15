using PasswordManager.Domain.Models;

namespace PasswordManager.Domain.Commands
{
    public interface IUpdateItemCommand<T> where T :BaseModel
    {
        Task Execute(T item);
    }
}
