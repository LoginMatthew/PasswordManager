using PasswordManager.Enums;
using PasswordManager.ViewModels;
using PasswordManager.ViewModels.PasswordViewModels;

namespace PasswordManager.Stores
{
    public class NavigationStore : ViewModelBase, INavigationStore
    {
        //Fields
        protected ViewModelBase currentChildView;
        public event Action CurrentViewModelChanged;
        public NavigationEnum NavigaitonSide { get; set; }

        //Properties
        public virtual ViewModelBase CurrentChildView
        {
            get => currentChildView;
            set
            {
                //Dispose from the previous ViewModel
                //On StartUp the app need a '?' since at the first time it is be set as the previous ViewModel is null
                //Avoid memory lake
                currentChildView?.Dispose();
                currentChildView = value;
                OnCurrentViewModelChanged();
                OnPropertyChanged(nameof(CurrentChildView));

                //Simple SideButtonSetter
                var content = currentChildView?.GetType()?.GetProperty("ContentViewModel")?.GetValue(currentChildView);

                if (content != null)
                {
                    if (content.GetType() == typeof(SettingViewModel))
                    {
                        NavigaitonSide = NavigationEnum.Setting;
                    }
                    else if (content.GetType() == typeof(PasswordViewModel))
                    {
                        NavigaitonSide = NavigationEnum.Password;
                    }
                    else if (content.GetType() == typeof(AboutViewModel))
                    {
                        NavigaitonSide = NavigationEnum.About;
                    }
                    else
                    {
                        NavigaitonSide = NavigationEnum.Dashboard;
                    }
                }
            }
        }

        protected void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}