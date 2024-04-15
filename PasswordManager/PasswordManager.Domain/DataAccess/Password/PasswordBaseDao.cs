using PasswordManager.Domain.Commands;
using PasswordManager.Domain.Models;

namespace PasswordManager.Domain.DataAccess.Password
{
    public abstract class PasswordBaseDao<T> : IPasswordBaseDao<T> where T : BaseModel
    {
        protected readonly IGetAllItemWithSettingCommand<T> getAllItemCommand;
        protected readonly IAddItemWithSettingCommand<T> addItemCommand;
        protected readonly IUpdateItemsWithSettingCommand<T> updateItemsCommand;
        protected readonly IDeleteItemWithSettingCommand<T> deleteItemCommand;
        protected readonly IUpdateItemWithSettingCommand<T> updateItemCommand;
        protected readonly IDeleteFileCommand deleteFileCommand;
        protected readonly IReversePasswordCommand reversePassword;

        public PasswordBaseDao(IGetAllItemWithSettingCommand<T> getALlItemCommand,
            IAddItemWithSettingCommand<T> addItemCommand,
            IUpdateItemsWithSettingCommand<T> updateItemsCommand,
            IDeleteItemWithSettingCommand<T> deleteItemCommand,
            IUpdateItemWithSettingCommand<T> updateItemCommand,
            IDeleteFileCommand deleteFileCommand,
            IReversePasswordCommand reversePassword)
        {
            this.getAllItemCommand = getALlItemCommand;
            this.addItemCommand = addItemCommand;
            this.updateItemsCommand = updateItemsCommand;
            this.deleteItemCommand = deleteItemCommand;
            this.updateItemCommand = updateItemCommand;
            this.deleteFileCommand = deleteFileCommand;
            this.reversePassword = reversePassword;
        }

        public virtual async Task<IEnumerable<T>> GetAllItem(SettingModel setting)
        {
            return await getAllItemCommand.Excute(setting);
        }
        public virtual async Task<SettingModel> AddItem(T item, SettingModel setting)
        {
            return await addItemCommand.Execute(item, setting);
        }

        public virtual async Task<SettingModel> ChangeDataDueToChangedOfSetting(List<T> items, SettingModel newSettingModel, SettingModel oldSettingModel)
        {
            SettingModel updatedSetting =  await updateItemsCommand.Execute(items, newSettingModel, oldSettingModel);

            if (oldSettingModel.SourceSettingDataModel.FileName != "" && newSettingModel.SourceSettingDataModel.FileName != "" &&
                oldSettingModel.SourceSettingDataModel.GetFileNameWithExtention() != newSettingModel.SourceSettingDataModel.GetFileNameWithExtention())
                await deleteFileCommand.Execute(oldSettingModel.SourceSettingDataModel.GetFileNameWithExtention(), newSettingModel.SourceSettingDataModel.FolderPath);
            
            return updatedSetting;
        }

        public virtual async Task UpdatedItem(T item, SettingModel setting)
        {
            await updateItemCommand.Execute(item, setting);
        }

        public virtual async Task DeletedItem(int id, SettingModel setting)
        {
            await deleteItemCommand.Execute(id, setting);
        }

        public virtual async Task<string> GetReversePassword(string password, SettingModel setting)
        {
            return await reversePassword.Execute(password, setting);
        }
    }
}
