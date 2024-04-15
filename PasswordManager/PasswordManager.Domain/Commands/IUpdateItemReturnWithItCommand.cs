using PasswordManager.Domain.Models;

namespace PasswordManager.Domain.Commands
{
    public interface IUpdateItemReturnWithItCommand<T> where T :BaseModel
    {
        Task<T> Execute(T item);
    }
}
