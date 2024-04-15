using PasswordManager.Domain.Models;

namespace PasswordManager.Domain.DataAccess.Password
{
    public interface IPasswordBaseDao<T> where T : BaseModel
    {
        Task<IEnumerable<T>> GetAllItem(SettingModel setting);
        Task<SettingModel> AddItem(T item, SettingModel setting);
        Task UpdatedItem(T item, SettingModel setting);
        Task DeletedItem(int id, SettingModel setting);
        Task<string> GetReversePassword(string password, SettingModel setting);
    }
}
