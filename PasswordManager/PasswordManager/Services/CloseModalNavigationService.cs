using PasswordManager.Stores;

namespace PasswordManager.Services
{
    //Navigate to pop up a window
    public class CloseModalNavigationService : INavigationService 
    {
        private readonly ModalNavigationStore _modalNavigationStore;

        public CloseModalNavigationService(ModalNavigationStore modalnavigateStore)
        {
            _modalNavigationStore = modalnavigateStore;
        }

        public void Navigate()
        {
            _modalNavigationStore.CLose();
        }
    }
}
