
using PasswordManager.Stores;
using System.Windows.Input;

namespace PasswordManager.ViewModels
{
    public class DashBoardViewModel : ViewModelBase
    {
        //Properties
        public NavigationSideViewModel NavigationSideViewModel { get; }
        public ICommand NavigateHomeCommand { get; }

        //Constructor
        public DashBoardViewModel(NavigationSideViewModel navigationSideViewModel)
        {
            NavigationSideViewModel = navigationSideViewModel;
        }

        ~DashBoardViewModel()
        {

        }

        //Unsubsribe from the event
        public override void Dispose()
        {
            //Just call the base in case of every cases
            base.Dispose();
        }
    }
}
