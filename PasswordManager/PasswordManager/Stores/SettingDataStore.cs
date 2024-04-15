using PasswordManager.ViewModels;
using PasswordManager.Domain.Models;
using System.Collections.ObjectModel;
using PasswordManager.Domain.DataAccess.Setting;

namespace PasswordManager.Stores
{
    //Stores is power full not only for lazy inititalization but aslo for reactivity and keeping UI for updated
    //managing applciaiton data in a centrelazied location and support reactivity to avoid still data on the UI
    public class SettingDataStore : ViewModelBase
    {
        //Use lazy not to trigger always data if it is not needed.
        //Lazy does that the initliazion happens only once.
        private Lazy<Task> _initilaizeLazy;
        private ObservableCollection<string> fileTypeList;
        private ObservableCollection<string> encyptionDataList;
        private SettingDao settingDao;

        public event Action SettingsLoaded;
        public event Action<SettingModel> SettingModelUpdatedAction;

        /// <summary>
        /// If the data file is changed then save the data into the new file
        /// </summary>
        public event Func<SettingModel, object> ChangeInSettingFile;
        public SettingModel Setting { get; set; }

        public ObservableCollection<string> FileTypeList
        {
            get => fileTypeList;
            set => fileTypeList = value;
        }

        
        public ObservableCollection<string> EncyptionDataList
        {
            get => encyptionDataList;
            set => encyptionDataList = value;
        }

        public SettingDataStore(ObservableCollection<string> encyptionDataList, ObservableCollection<string> fileTypeList, SettingModel setting , SettingDao settingDao)
        {
            this.Setting = setting;
            this.EncyptionDataList = encyptionDataList;
            this.FileTypeList = fileTypeList;
            this.settingDao = settingDao;

            _initilaizeLazy = new Lazy<Task>(Initialize);
        }

        private void OnSettingUpdatedMode(SettingModel item) => SettingModelUpdatedAction?.Invoke(item);

        public async Task Initialize()
        {
            this.Setting = await settingDao.GetSetting(this.Setting);

            if (this.Setting.UseEmbeddedDefaultDataSetting)
                this.Setting = await settingDao.AddSetting(this.Setting);                
            
            SettingsLoaded?.Invoke();
        }

        public async Task UpdateSetting(SettingModel item)
        {
            this.Setting = await settingDao.AddSetting(item);
            OnSettingUpdatedMode(item);
        }

        public async Task UpdateSetting(SettingModel newSetting, SettingModel oldSetting)
        {
            this.Setting = await settingDao.AddSetting(newSetting);
            ChangeInSettingFile?.Invoke(oldSetting);
        }

        public async Task Load()
        {
            //The value is tha task what is the Initialize method
            try
            {
                await _initilaizeLazy.Value;
            }
            catch (Exception)
            {
                //need it to reload the data page again
                _initilaizeLazy = new Lazy<Task>(Initialize);
                throw;
            }
        }
    }
}