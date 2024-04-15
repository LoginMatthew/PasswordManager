using PasswordManager.Stores;
using PasswordManager.ViewModels;

namespace PasswordManager.Services
{
    public class NavigationLayoutService<TViewModel> : NavigationService<TViewModel> where TViewModel : ViewModelBase
    {
        //When this class is instantiated a new layout ViewModel, a navigationSideViewModel will be created with call back function
        //Other meaning: Only one single instance was created here but it is not good since Dispose is used and onyl 1 times was a subscribe is done in the Constructor.
        private readonly Func<NavigationSideViewModel> _createNavigateSideViewModel;
        private readonly string _caption;
        
        public NavigationLayoutService(NavigationStore navigateStore, Func<TViewModel> createViewModel, Func<NavigationSideViewModel> createNavigateSideViewModel, string caption) :base(navigateStore, createViewModel)
        {
            _createNavigateSideViewModel = createNavigateSideViewModel;
            _caption = caption;
        }

        public override void Navigate()
        {
            base._navigationStore.CurrentChildView = new LayoutViewModel(_createNavigateSideViewModel(), base._createViewModel(), _caption);
        }
    }
}
