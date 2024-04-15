using PasswordManager.Domain.Commands;
using PasswordManager.Domain.Models;

namespace PasswordManager.Domain.DataAccess.Setting
{
    public abstract class SettingBaseDao<T> : ISettingBaseDao<T> where T : BaseModel
    {
        protected readonly IGetItemWithSettingCommand<T> getItemCommand;
        protected readonly IAddItemReturnWithItCommand<T> addItemCommand;
        protected readonly IUpdateItemReturnWithItCommand<T> updateItemCommand;

        public SettingBaseDao(IGetItemWithSettingCommand<T> getItemCommand,
            IAddItemReturnWithItCommand<T> addItemCommand,
            IUpdateItemReturnWithItCommand<T> updateItemCommand)
        {
            this.getItemCommand = getItemCommand;
            this.addItemCommand = addItemCommand;
            this.updateItemCommand = updateItemCommand;
        }

        public virtual async Task<T> GetSetting(SettingModel setting)
        {
            return await getItemCommand.Execute(setting);
        }
        
        public virtual async Task<T> AddSetting(T item)
        {
            return await addItemCommand.Execute(item);
        }

        public virtual async Task<T> UpdatedSetting(T item)
        {
            return await updateItemCommand.Execute(item);
        }

        public virtual async Task<IEnumerable<T>> GetAllSettings()
        {
            return null;
        }

    }
}
