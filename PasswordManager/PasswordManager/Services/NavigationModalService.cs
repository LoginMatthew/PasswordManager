using PasswordManager.Stores;
using PasswordManager.ViewModels;

namespace PasswordManager.Services
{
    public class NavigationModalService<TViewModel> : INavigationService where TViewModel : ViewModelBase
    {
        protected readonly ModalNavigationStore _navigationModalStore;

        /// <summary>
        /// Call back to create the ViewModel what we want to set as the current ViewModel
        /// </summary>
        protected readonly Func<TViewModel> _createViewModel;

        public NavigationModalService(ModalNavigationStore navigateModalStore, Func<TViewModel> createViewModel)
        {
            _navigationModalStore = navigateModalStore;
            _createViewModel = createViewModel;
        }

        public virtual void Navigate()
        {
            _navigationModalStore.CurrentChildView = _createViewModel();
        }
    }
}
