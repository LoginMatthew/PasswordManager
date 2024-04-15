using PasswordManager.ViewModels;
using PasswordManager.Domain.Models;
using System.Collections.ObjectModel;
using PasswordManager.Domain.DataAccess.Password;

namespace PasswordManager.Stores
{
    //Stores is power full not only for lazy inititalization but aslo for reactivity and keeping UI for updated
    //managing applciaiton data in a centrelazied location and support reactivity to avoid still data on the UI
    public class PasswordListStore : ViewModelBase
    {
        //Use lazy not to trigger always data if it is not needed.
        //Lazy does that the initliazion happens only once.
        private Lazy<Task> _initilaizeLazy;
        private IEnumerable<PasswordModel> passwords;
        private PasswordDao passwordDao;
        private SettingDataStore settingDataStore;

        public event Action<PasswordModel> PasswordAdded;
        public event Action<PasswordModel, bool> PasswordUpdatedAction;
        public event Action<int> PasswordDeletedAction;
        public event Action<List<PasswordModel>> PasswordSearch;
        public event Action PasswordsLoaded;
        public event Func<string, object> ChangeEncryptionEvent;

        public SettingDataStore SettingDataStore => settingDataStore;

        public IEnumerable<PasswordModel> Passwords
        {
            get => passwords;
            set
            {
                passwords = value;
                OnPropertyChanged(nameof(Passwords));
            }
        }

        public PasswordListStore(PasswordDao passwordDao, SettingDataStore settingDataStore)
        {
            this.passwordDao = passwordDao;
            Passwords = new ObservableCollection<PasswordModel>();
            this.settingDataStore = settingDataStore;

            this.settingDataStore.ChangeInSettingFile += ModifyDataComponent;
            InitLazy();
        }

        public async Task Initialize()
        {
            Passwords = await passwordDao.GetAllItem(this.settingDataStore.Setting);
            PasswordsLoaded?.Invoke();
        }

        private void InitLazy() => _initilaizeLazy = new Lazy<Task>(Initialize);
        private void OnPasswordAddedMode(PasswordModel item) => PasswordAdded?.Invoke(item);
        private void OnSearchMode(List<PasswordModel> searchItems) => PasswordSearch?.Invoke(searchItems);
        private void OnPasswordUpdatedMode(PasswordModel item, bool isEncryption) => PasswordUpdatedAction?.Invoke(item, isEncryption);
        
        private async Task ModifyDataComponent(SettingModel oldSetting)
        {
            List<PasswordModel> passwordList = new List<PasswordModel>();

            foreach (var item in Passwords)
                passwordList.Add(item);

            settingDataStore.UpdateSetting(await passwordDao.ChangeDataDueToChangedOfSetting(passwordList, settingDataStore.Setting, oldSetting));

            InitLazy();
        }

        public async Task<string> GetReversePassword(string password) => await passwordDao.GetReversePassword(password, this.settingDataStore.Setting);


        public async Task Load()
        {
            //In case of reload the page
            InitLazy();

            //The value is tha task what is the Initialize method
            try
            {
                await _initilaizeLazy.Value;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task AddedPassword(PasswordModel item)
        {
            await settingDataStore.UpdateSetting(await passwordDao.AddItem(item, settingDataStore.Setting));
            await GetAllItem();
            OnPasswordAddedMode(item);
        }

        public async Task SearchPassword(List<PasswordModel> searchItems)
        {
            Passwords = new ObservableCollection<PasswordModel>();
            Passwords = searchItems.OrderBy(x => x.Id);
            OnSearchMode(searchItems);
        }

        public async Task GetAllItem()
        {
            Passwords = new ObservableCollection<PasswordModel>();
            IEnumerable<PasswordModel> passwordList = await passwordDao.GetAllItem(settingDataStore.Setting);
            Passwords = passwordList.OrderBy(x => x.Id);
        }

        public async Task UpdatePassword(PasswordModel item)
        {
                await passwordDao.UpdatedItem(item, settingDataStore.Setting);
                PasswordModel removeItem = (Passwords as List<PasswordModel>)?.SingleOrDefault(x => x.Id == item.Id);
                (Passwords as ObservableCollection<PasswordModel>)?.Remove(removeItem);
                (Passwords as ObservableCollection<PasswordModel>)?.Add(item);

                OnPasswordUpdatedMode(item, settingDataStore.Setting.UseEncryption);            
        }

        public async Task DeletePassword(int id)
        {
            await passwordDao.DeletedItem(id, settingDataStore.Setting);

            var newList = Passwords.Where(x => x.Id != id).ToList();
            Passwords = new ObservableCollection<PasswordModel>();

            foreach (var item in newList)
            {
                (Passwords as ObservableCollection<PasswordModel>)?.Add(item);
            }

            PasswordDeletedAction?.Invoke(id);
        }

        //Unsubsribe from the event
        //Avoid memory lake
        public override void Dispose()
        {
            this.settingDataStore.ChangeInSettingFile -= ModifyDataComponent;

            //Just call the base in any case
            base.Dispose();

        }
    }
}