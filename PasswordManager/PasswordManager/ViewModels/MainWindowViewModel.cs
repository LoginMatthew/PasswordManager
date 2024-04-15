using PasswordManager.Stores;

namespace PasswordManager.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        //Fields
        private NavigationStore _navigationStore;
        private ModalNavigationStore _modalNavigationStore;
        private bool isViewVisible;

        //Properties
        public ViewModelBase CurrentChildViewModel => _navigationStore.CurrentChildView;
        public ViewModelBase CurrentChildModalViewModel => _modalNavigationStore.CurrentChildView;
        public bool IsModalOpen => _modalNavigationStore.IsModalOpen;
        public int ActualWindowHeight => _modalNavigationStore.ActualWindowHeight;
        public int ActualWindowWidth => _modalNavigationStore.ActualWindowWidth;
        public bool IsViewVisible
        {
            get => isViewVisible;
            set
            {
                isViewVisible = value;
                OnPropertyChanged("IsViewVisible");
            }
        }

        //Constructor
        public MainWindowViewModel(NavigationStore navigationStore, ModalNavigationStore modalNavigationStore)
        {
            _navigationStore = navigationStore;
            _modalNavigationStore = modalNavigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _modalNavigationStore.CurrentViewModelChanged += OnCurrentModalViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged("CurrentChildViewModel");
        }

        private void OnCurrentModalViewModelChanged()
        {
            OnPropertyChanged("CurrentChildModalViewModel");
            OnPropertyChanged("IsModalOpen");
        }

        public override void Dispose()
        {
            _navigationStore.CurrentViewModelChanged -= OnCurrentViewModelChanged;
            _modalNavigationStore.CurrentViewModelChanged -= OnCurrentModalViewModelChanged;
            base.Dispose();
        }
    }
}
