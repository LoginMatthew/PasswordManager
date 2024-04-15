using PasswordManager.Domain.Models;

namespace PasswordManager.Domain.Commands
{
    public interface IGetItemCommand<T> where T :BaseModel
    {
        Task<T> Execute();
    }
}
