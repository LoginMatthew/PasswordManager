
using System.Windows.Input;

namespace PasswordManager.ViewModels.PopUpViewModels
{
    public class ConformationViewModel : ViewModelBase
    {
        private ICommand okCommand;
        private ICommand cancelCommand;
        private string headerContent;
        private string descriptionContent;

        public string DescriptionText => descriptionContent;
        public string HeaderContent => headerContent;
        public ICommand OKCommand => okCommand;

        public ICommand CancelCommand => cancelCommand;

        public ConformationViewModel(string headerContent, string descriptionContent)
        {
            this.headerContent = headerContent;
            this.descriptionContent = descriptionContent;            
        }

        public void SetCommands(ICommand command, ICommand cancelCommand)
        {
            this.okCommand = command;
            this.cancelCommand = cancelCommand;
        }
    }
}
