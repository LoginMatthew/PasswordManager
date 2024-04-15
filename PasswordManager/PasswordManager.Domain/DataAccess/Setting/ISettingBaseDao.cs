using PasswordManager.Domain.Models;

namespace PasswordManager.Domain.DataAccess.Setting
{
    public interface ISettingBaseDao<T> where T : BaseModel
    {
        /// <summary>
        /// In case of the program is used by others
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllSettings();
        Task<T> GetSetting(SettingModel setting = null);
        Task<T> AddSetting(T item);
        Task<T> UpdatedSetting(T item);
    }
}
