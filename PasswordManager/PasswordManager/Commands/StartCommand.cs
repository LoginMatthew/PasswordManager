using PasswordManager.Services;

namespace PasswordManager.Commands
{
    public class StartCommand : CommandBase
    {
        private readonly INavigationService _navigationService;

        //to have paramater
        public StartCommand(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object paramter) =>  _navigationService.Navigate();        
    }
}

