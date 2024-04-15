using PasswordManager.ViewModels;

namespace PasswordManager.Stores
{
    public class ModalNavigationStore : NavigationStore
    {
        //Properties
        public override ViewModelBase CurrentChildView
        {
            get => base.currentChildView;
            set
            {
                //Dispose from the previous ViewModel
                //On StartUp the app need a '?' since at the first time it is be set as the previous ViewModel is null
                base.currentChildView?.Dispose();
                base.currentChildView = value;
                base.OnCurrentViewModelChanged();
                OnPropertyChanged("CurrentChildView");
            }
        }

        public bool IsModalOpen => CurrentChildView != null;
        public int ActualWindowHeight { get; set; }
        public int ActualWindowWidth { get; set; }

        public ModalNavigationStore()
        {
            ActualWindowHeight = 100;
            ActualWindowWidth = 100;
        }
        public ModalNavigationStore(int width, int height)
        {
            ActualWindowHeight = height;
            ActualWindowWidth = width;
        }

        public void CLose()
        {
            base.CurrentChildView = null;
        }
    }
}