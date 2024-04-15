using PasswordManager.Domain.Commands;
using PasswordManager.Domain.Models;

namespace PasswordManager.Domain.DataAccess.Password
{
    public class PasswordDao : PasswordBaseDao<PasswordModel>
    {
        public PasswordDao(IGetAllItemWithSettingCommand<PasswordModel> getALlItemCommand,
            IAddItemWithSettingCommand<PasswordModel> addItemCommand,
            IUpdateItemsWithSettingCommand<PasswordModel> updateItemsCommand,
            IDeleteItemWithSettingCommand<PasswordModel> deleteItemCommand,
            IUpdateItemWithSettingCommand<PasswordModel> updateItemCommand,
            IDeleteFileCommand deleteFileCommand,
            IReversePasswordCommand reversePassword) : base(getALlItemCommand, addItemCommand, updateItemsCommand, deleteItemCommand, updateItemCommand, deleteFileCommand, reversePassword)
        {

        }
    }
}
