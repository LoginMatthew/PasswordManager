
namespace PasswordManager.ViewModels
{
    public class LayoutViewModel : ViewModelBase
    {
        public NavigationSideViewModel NavigationSideViewModel { get; }
        public ViewModelBase ContentViewModel { get; }
        public string Caption { get; }

        public LayoutViewModel(NavigationSideViewModel navigationSideBarViewModel, ViewModelBase contentViewModel, string caption)
        {
            NavigationSideViewModel = navigationSideBarViewModel;
            ContentViewModel = contentViewModel;

            Caption = caption;
        }

        //Need to dispose of all the child ViedModel, It is propagated down to any child ViewModel that's on ViewModel
        public override void Dispose()
        {
            ContentViewModel.Dispose();
            NavigationSideViewModel.Dispose();
            base.Dispose();
        }
    }
}