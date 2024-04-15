
namespace PasswordManager.Domain.Models
{
    public class SettingModel : BaseModel
    {
        /// <summary>
        /// For future development, to store User settings by id
        /// </summary>
        public int Id { get; set; }
        public bool UseEmbeddedDefaultDataSetting { get; set; }
        public bool UseEncryption { get; set; }
        public SettingDataModel SourceSettingDataModel { get => sourceSettingDataModel; set => sourceSettingDataModel = value; }
        private SettingDataModel sourceSettingDataModel;
                
        public SettingModel()
        {
            SourceSettingDataModel = new SettingDataModel();
        }

        public SettingModel(SettingDataModel source)
        {
            this.SourceSettingDataModel = source;
        }
    }
}
