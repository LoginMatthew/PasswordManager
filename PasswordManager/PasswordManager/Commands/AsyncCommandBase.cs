
namespace PasswordManager.Commands
{
    //Any class which extends this class should implements his own error handling
    public abstract class AsyncCommandBase : CommandBase
    {
        private bool _isExectuing;

        private bool IsExecuting
        {
            get => _isExectuing;
            set
            {
                _isExectuing = value;
                OnCanExecutedChanged();
            }
        }

        public override async void Execute(object parameter)
        {
            IsExecuting = true;
            try
            {
                await ExecuteAsync(parameter);
            }
            catch (Exception)
            {
                throw;
            }

            finally
            {
                IsExecuting = false;
            }
        }
        public override bool CanExecute(object parameter)
        {
            return !IsExecuting && base.CanExecute(parameter);
        }

        public abstract Task ExecuteAsync(object paramter);
    }
}
