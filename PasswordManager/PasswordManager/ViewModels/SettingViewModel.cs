using PasswordManager.Commands;
using PasswordManager.Domain.Models;
using PasswordManager.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PasswordManager.ViewModels
{
    public class SettingViewModel : ViewModelBase
    { 
        //Fields
        public SettingDataStore SettingDataStore;


        private bool embededDataSetting;
        public bool EmbededDataSetting
        {
            get => embededDataSetting;
            set
            {
                embededDataSetting = value;
                OnPropertyChanged(nameof(EmbededDataSetting));
            }            
        }

        private bool useUniqueSymetricKey;

        public bool UseUniqueSymetricKey
        {
            get => useUniqueSymetricKey;
            set
            {
                useUniqueSymetricKey = value;
                OnPropertyChanged(nameof(UseUniqueSymetricKey));
            }
        }

        private bool useEncryption;

        public bool UseEncryption
        {
            get => useEncryption;
            set
            {
                useEncryption = value;
                OnPropertyChanged(nameof(UseEncryption));
            }
        }

        #region FileType

        private string selectedSourceFileType;
        public string SelectedSourceFileType
        {
            get => selectedSourceFileType;
            set
            {
                selectedSourceFileType = value;
                OnPropertyChanged(nameof(SelectedSourceFileType));
            }
        }


        private string sourceFileName;
        public string SourceFileName
        { 
            get => sourceFileName;
            set
            {
                sourceFileName = value;
                OnPropertyChanged(nameof(SourceFileName));
            }
        }

        public ObservableCollection<string> FileTypeList
        {
            get => SettingDataStore.FileTypeList;
            set => SettingDataStore.FileTypeList = value;
        }

        #endregion

        #region Encryption Type

        private string selectedSourceEncrpytionType;

        public string SelectedSourceEncrpytionType
        {
            get => selectedSourceEncrpytionType;
            set
            {
                selectedSourceEncrpytionType = value;
                OnPropertyChanged(nameof(SelectedSourceEncrpytionType));
            }
        }

        private string symetricKey;

        public string SymetricKey
        {
            get => symetricKey;
            set
            {
                symetricKey = value;
                OnPropertyChanged(nameof(SymetricKey));
            }
        }

        private string encryptionKey;
        public string EncryptionKey
        {
            get => encryptionKey;
            set
            {
                encryptionKey = value;
                OnPropertyChanged(nameof(EncryptionKey));
            }
        }

        public ObservableCollection<string> EncyptionDataList
        {
            get => SettingDataStore.EncyptionDataList;
            set => SettingDataStore.EncyptionDataList = value;
        }
        #endregion

        private string messageText;
        public string MessageText
        {
            get => messageText;
            set
            {
                messageText = value;
                OnPropertyChanged(nameof(MessageText));
                OnPropertyChanged(nameof(SettingDataStore.Setting));
            }
        }

        public ICommand ApplyCommand { get; }

        public SettingViewModel(SettingDataStore settingDataStore, PasswordListStore passwordListStore)
        {
            this.SettingDataStore = settingDataStore;
            this.ApplyCommand = new ApplySettingChangeAsyncCommand(this, settingDataStore, passwordListStore);

            this.SourceFileName = SettingDataStore.Setting.SourceSettingDataModel.FileName;
            this.SelectedSourceFileType = SettingDataStore.Setting.SourceSettingDataModel.SelectedFileType;
            this.EmbededDataSetting = SettingDataStore.Setting.UseEmbeddedDefaultDataSetting;
            this.UseEncryption = SettingDataStore.Setting.UseEncryption;
            this.SelectedSourceEncrpytionType = SettingDataStore.Setting.SourceSettingDataModel.SelectedEncryptionType;
            this.SymetricKey = SettingDataStore.Setting.SourceSettingDataModel.SymetricKey;

            this.SettingDataStore.SettingModelUpdatedAction += UpdateSettingViewDueToModicationInSettingFile;
        }

        private  void UpdateSettingViewDueToModicationInSettingFile(SettingModel setting)
        {
            this.SymetricKey = this.SettingDataStore.Setting.SourceSettingDataModel.SymetricKey;
        }
    }
}
