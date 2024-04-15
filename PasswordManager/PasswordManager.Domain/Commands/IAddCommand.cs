using PasswordManager.Domain.Models;

namespace PasswordManager.Domain.Commands
{
    public interface IAddCommand<T> where T :BaseModel
    {
        Task Execute(T item);
    }
}
