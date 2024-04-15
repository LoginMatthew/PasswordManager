using PasswordManager.ViewModels;
using PasswordManager.Stores;

namespace PasswordManager.Services
{
    //Note: To support multiple parameters, create and object to hold all the parameters and use that object as the TParameter tpye
    public class ParameterNavigationService<TParameter, TViewModel> where TViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<TParameter, TViewModel> _createViewModel;

        public ParameterNavigationService(NavigationStore navigateStore, Func<TParameter, TViewModel> createViewModel)
        {
            _navigationStore = navigateStore;
            _createViewModel = createViewModel;
        }

        public void Navigate(TParameter parameter)
        {
            _navigationStore.CurrentChildView = _createViewModel(parameter);
        }
    }
}
