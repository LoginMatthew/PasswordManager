
using System.Collections.ObjectModel;

namespace PasswordManager.ViewModels.PasswordViewModels.Components
{
    public class PasswordViewModelBase : ViewModelBase
    {
        public virtual bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage) || (ErrorMessageList != null&& ErrorMessageList.Count >0);

        protected string _errorMessage;
        protected ObservableCollection<string> _errorMessageList;
        public virtual string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(HasErrorMessage));
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public virtual ObservableCollection<string> ErrorMessageList
        {
            get => _errorMessageList;
            set
            {
                _errorMessageList = value;
                OnPropertyChanged(nameof(ErrorMessageList));
                OnPropertyChanged(nameof(HasErrorMessage));
            }
        }


        protected bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        protected bool _isDeleting;
        public bool IsDeleting
        {
            get => _isDeleting;

            set
            {
                _isDeleting = value;
                OnPropertyChanged(nameof(IsDeleting));
            }

        }
    }
}
