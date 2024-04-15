using PasswordManager.Domain.Models;
using PasswordManager.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PasswordManager.Stores;
using System.Collections.Specialized;
using PasswordManager.Commands;
using Microsoft.Windows.Themes;

namespace PasswordManager.ViewModels.PasswordViewModels.Components
{
    public class PasswordListViewModel : PasswordViewModelBase
    {
        private ObservableCollection<PasswordListItemViewModel> passwordListItems;
        private readonly PasswordListStore passwordListStore;
        private PasswordListItemViewModel selectedPasswordListItemViewModel;
        private readonly ModalNavigationStore modalNavigationStore;
        private readonly INavigationService closeNavigationService;
        private bool isShowPassword;
        public void ChangePassword(bool isShowPassword)
        {
            this.isShowPassword = isShowPassword;
            foreach (var item in passwordListItems)
            {
                item.IsShowPassword = isShowPassword;
            }
        }

        public ObservableCollection<string> ErrorMessageList { get; set; }
        public bool HasErrorMessage { get; }
        public string ErrorMessage { get; set; }

        public bool HasSelectedPassword => SelectedPasswordListItemViewModel != null;
        public ICommand LoadPasswordCommand { get; }

        public PasswordListItemViewModel SelectedPasswordListItemViewModel
        {
            get => selectedPasswordListItemViewModel;
            set
            {
                selectedPasswordListItemViewModel = value;
                OnPropertyChanged(nameof(SelectedPasswordListItemViewModel));
            }
        }

        public IEnumerable<PasswordListItemViewModel> PasswordListItems
        {
            get => passwordListItems;
            set
            {
                passwordListItems = (ObservableCollection<PasswordListItemViewModel>)value;
                OnPropertyChanged(nameof(PasswordListItems));
            }
        }

        public PasswordListViewModel(PasswordListStore passwordListStore,
            ModalNavigationStore modalNavigationStore,
            INavigationService closeNavigationService)
        {
            this.passwordListStore = passwordListStore;
            this.modalNavigationStore = modalNavigationStore;
            this.closeNavigationService = closeNavigationService;
            this.passwordListItems = new ObservableCollection<PasswordListItemViewModel>();

            this.LoadPasswordCommand = new LoadPasswordViewModelCommand(passwordListStore, this);

            passwordListStore.PasswordAdded += AddedItemList;
            passwordListStore.PasswordsLoaded += LoadedPasswordListItemViewModelList;
            passwordListStore.PasswordUpdatedAction += UpdatedItemList;
            passwordListStore.PasswordDeletedAction += DeletedItem;
            passwordListStore.PasswordSearch += SearchItemList;
            passwordListItems.CollectionChanged += PasswordCollectionChanged;
        }

        //This is a  static Factory Method and it is called to load this class's contents
        public static PasswordListViewModel LoadPasswordListViewModel(PasswordListStore passwordListStore, ModalNavigationStore modalNavigationStore,
            INavigationService closeNavigationService)
        {
            PasswordListViewModel viewModel = new PasswordListViewModel(passwordListStore, modalNavigationStore, closeNavigationService);
            viewModel.LoadPasswordCommand.Execute(null);
            return viewModel;
        }

        private void AddedItemList(PasswordModel passwordModel) => LoadedPasswordListItemViewModelList();
        private void SearchItemList(List<PasswordModel> searchItems) => LoadedPasswordListItemViewModelList();
        private void PasswordCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => OnPropertyChanged(nameof(SelectedPasswordListItemViewModel));

        private void DeletedItem(int id)
        {
            PasswordListItemViewModel passwordListItemViewModel = passwordListItems.FirstOrDefault(x => x.PasswordModel.Id == id);

            if (passwordListItemViewModel != null)
            {
                passwordListItems.Remove(passwordListItemViewModel);
                SelectedPasswordListItemViewModel = null;
            }
        }

        private PasswordListItemViewModel CreatePasswordListItemViewModel(PasswordModel passwordModel) =>
         new PasswordListItemViewModel(passwordModel, closeNavigationService, modalNavigationStore, passwordListStore, isShowPassword);

        private void UpdatedItemList(PasswordModel item, bool isEncryption)
        {
            PasswordListItemViewModel passwordListItemViewModel = passwordListItems.FirstOrDefault(x => x.PasswordModel.Id == item.Id);

            if (passwordListItemViewModel != null)
            {
                passwordListItems.Remove(passwordListItemViewModel);

                if (isEncryption)
                    passwordListItemViewModel.PasswordModel.Password = passwordListStore.Passwords.FirstOrDefault(x => x.Id == passwordListItemViewModel.PasswordModel.Id).Password;

                passwordListItemViewModel.Update(item);
                passwordListItemViewModel.IsShowPassword = isShowPassword;
                passwordListItems.Add(passwordListItemViewModel);

                SelectedPasswordListItemViewModel = passwordListItemViewModel;
                UpdateOrderOfTheList();
            }
        }

        private void UpdateOrderOfTheList()
        {
            var newList = passwordListItems.OrderBy(x => x.PasswordModel.Site).ToList();
            passwordListItems.Clear();
            foreach (PasswordListItemViewModel item in newList)
            {
                item.IsShowPassword = isShowPassword;
                passwordListItems.Add(item);
            }
        }

        /// <summary>
        /// Order List by site name
        /// </summary>
        public void LoadedPasswordListItemViewModelList()
        {
            passwordListItems.Clear();
            foreach (PasswordModel item in passwordListStore.Passwords.OrderBy(x => x.Site).ToList())
                passwordListItems.Add(CreatePasswordListItemViewModel(item));
        }

        //Unsubsribe from the event
        //Avoid memory lake
        public override void Dispose()
        {
            passwordListStore.PasswordAdded -= AddedItemList;
            passwordListStore.PasswordsLoaded -= LoadedPasswordListItemViewModelList;
            passwordListStore.PasswordUpdatedAction -= UpdatedItemList;
            passwordListStore.PasswordDeletedAction -= DeletedItem;
            passwordListStore.PasswordSearch -= SearchItemList;
            passwordListItems.CollectionChanged -= PasswordCollectionChanged;
            //Just call the base in any case
            base.Dispose();
        }
    }
}
