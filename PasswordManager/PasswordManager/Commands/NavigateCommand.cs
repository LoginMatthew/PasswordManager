using PasswordManager.Services;

namespace PasswordManager.Commands
{
    //Generic navigation function
    public class NavigateCommand : CommandBase
    {
        //Fields
        private readonly INavigationService _navigateStervice;

        //Call back need to not to create severeal instance of the given navigation command
        public NavigateCommand(INavigationService navigateStervice)
        {
            _navigateStervice = navigateStervice;
        }

        public override void Execute(object paramter) =>  _navigateStervice.Navigate();        
    }
}
