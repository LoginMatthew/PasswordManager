using PasswordManager.ViewModels;
using PasswordManager.Stores;

namespace PasswordManager.Services
{
    public class NavigationService<TViewModel> : INavigationService where TViewModel : ViewModelBase
    {
        protected readonly NavigationStore _navigationStore;

        /// <summary>
        /// Call back to create the ViewModel what we want to set as the current ViewModel
        /// </summary>
        protected readonly Func<TViewModel> _createViewModel;

        public NavigationService(NavigationStore navigateStore, Func<TViewModel> createViewModel)
        {
            _navigationStore = navigateStore;
            _createViewModel = createViewModel;
        }

        public virtual void Navigate()
        {
            _navigationStore.CurrentChildView = _createViewModel();
        }
    }
}
