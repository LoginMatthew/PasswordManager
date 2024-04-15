
using System.Windows.Input;

namespace PasswordManager.ViewModels.PasswordViewModels.Components
{
    public class SearchPasswordViewModel : ViewModelBase
    {
        private ICommand searchCommand;
        private ICommand cancelCommand;
        private string search;

        public string SearchText
        {
            get => search;
            set
            {
                search = value.Trim().Replace(";", "");
                OnPropertyChanged(nameof(SearchText));
            }
        }

        public string HeaderContent => "Search";
        public ICommand SearchCommand => searchCommand;
        public ICommand CancelCommand => cancelCommand;

        public SearchPasswordViewModel()
        {

        }

        public void SetCommands(ICommand command, ICommand cancelCommand)
        {
            this.searchCommand = command;
            this.cancelCommand = cancelCommand;
        }
    }
}
