
namespace PasswordManager.Commands
{
    //Any class which extends this class should implements his own error handling
    public abstract class AsyncCommandBaseWithReturn : CommandBaseWithReturn
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

        public override async Task<object> ExecuteWithReturnObject(object parameter)
        {
            IsExecuting = true;
            object result = null;
            try
            {
                result = await ExecuteWithReturnObjectAsync(parameter);
            }
            catch (Exception)
            {
                throw;
            }

            finally
            {
                IsExecuting = false;
            }

            return result;
        }

        public override bool CanExecute(object parameter)
        {
            return !IsExecuting && base.CanExecute(parameter);
        }

        public abstract Task ExecuteAsync(object paramter);

        public abstract Task<object> ExecuteWithReturnObjectAsync(object paramter);
    }
}
