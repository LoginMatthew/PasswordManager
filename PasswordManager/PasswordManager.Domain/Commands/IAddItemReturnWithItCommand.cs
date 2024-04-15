using PasswordManager.Domain.Models;

namespace PasswordManager.Domain.Commands
{
    public interface IAddItemReturnWithItCommand<T> where T :BaseModel
    {
        Task<T> Execute(T item);
    }
}
