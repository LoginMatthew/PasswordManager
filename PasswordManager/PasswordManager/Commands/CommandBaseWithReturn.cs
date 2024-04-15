
namespace PasswordManager.Commands
{
    public abstract class CommandBaseWithReturn : ICommandWithReturn
    {        
        public event EventHandler CanExecuteChanged;
        public virtual bool CanExecute(object parameter) => true;
        public abstract void Execute(object parameter);
        public abstract object ExecuteWithReturnObject(object parameter);

        protected void OnCanExecutedChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
