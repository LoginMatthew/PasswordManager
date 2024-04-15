using System.Windows.Input;

namespace PasswordManager.Commands
{
    public interface ICommandWithReturn : ICommand
    {
        /// <param name="parameter">It is used for future development e.g. login</param>
        /// <returns></returns>
        object ExecuteWithReturnObject(object parameter);
    }
}
