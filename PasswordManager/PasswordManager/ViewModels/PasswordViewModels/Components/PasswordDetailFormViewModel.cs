
using PasswordManager.Commands;
using System.Windows.Input;

namespace PasswordManager.ViewModels.PasswordViewModels.Components
{
    public class PasswordDetailFormViewModel : ViewModelBase
    {
        private string buttonSubmitTxt;
        private string email;
        private string site;
        private string userName;
        private string password;
        private string description;
        private string errorMessage;
        private bool isSubmitting;
        private ICommand submitCommand;
        private ICommand cancelCommand;

        public string ButtonSubmitTxt => buttonSubmitTxt;
        public int Id;
        public string Site
        {
            get => site;
            set
            {
                site = RemoveSelicolumns(RemoveInvalidCharacters(value));
                OnPropertyChanged(nameof(Site));
                OnPropertyChanged(nameof(CanSubmitCommand));
            }
        }

        public string Email
        {
            get => email;
            set
            {
                email = RemoveSelicolumns(RemoveInvalidCharacters(value));
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(CanSubmitCommand));
            }
        }

        public string UserName
        {
            get => userName;
            set
            {
                userName = RemoveSelicolumns(RemoveInvalidCharacters(value));
                OnPropertyChanged(nameof(UserName));
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = RemoveSelicolumns(RemoveInvalidCharacters(value));
                OnPropertyChanged(nameof(Password));
                OnPropertyChanged(nameof(CanSubmitCommand));
            }
        }

        public string Description
        {
            get => description;
            set
            {
                description = RemoveSelicolumns(value);
                OnPropertyChanged(nameof(Description));
            }
        }

        public bool IsSubmitting
        {
            get => isSubmitting;
            set
            {
                isSubmitting = value;
                OnPropertyChanged(nameof(IsSubmitting));
            }
        }

        public int SelectedDefaulJLPTIndex { get; set; }

        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                OnPropertyChanged(nameof(HasErrorMessage));
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        
        public bool CanSubmitCommand => !string.IsNullOrEmpty(Site) && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);
        
        public ICommand SubmitCommand => submitCommand;
        public ICommand CancelCommand => cancelCommand;

        public PasswordDetailFormViewModel()
        {

        }

        public PasswordDetailFormViewModel(string buttontxt)
        {
            buttonSubmitTxt = buttontxt;
        }

        private string RemoveInvalidCharacters(string text) => text != null ? text.Trim().Replace(" ", "") : "";
        private string RemoveSelicolumns(string text) => text != null ? text.Trim().Replace(";", "") : "";

        public void SetCommand(ICommand submitCommand, ICommand cancelCommand, ICommandWithReturn reversePasswordText = null)
        {
            this.submitCommand = submitCommand;
            this.cancelCommand = cancelCommand;

            if(reversePasswordText!= null && password != null && password != "" && reversePasswordText != null)
            {
                var result = reversePasswordText.ExecuteWithReturnObject(password);
                password = result?.GetType()?.GetProperty("Result") != null ? result?.GetType()?.GetProperty("Result")?.GetValue(result).ToString() : password;
            }
        }
    }
}
