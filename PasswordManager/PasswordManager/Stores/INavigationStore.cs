using PasswordManager.ViewModels;

namespace PasswordManager.Stores
{
    public interface INavigationStore
    {
        public ViewModelBase CurrentChildView { get; set; }
    }
}