
using System.Text.Json.Serialization;

namespace PasswordManager.Domain.Models
{
    public class SettingDataModel : BaseModel
    {
        public string FileName { get; set; }
        public string SelectedFileType { get; set; }
        public string FolderPath { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string SymetricKey { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string SelectedEncryptionType { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string EncryptionKey { get; set; }

        public SettingDataModel()
        {

        }

        public SettingDataModel(string fileName, string selectedFileType, string folderPath, string symetricKey, string selectedEnryptionType, string encryptionKey)
        {
            FileName = fileName;
            SelectedFileType = selectedFileType;
            FolderPath = folderPath;
            SymetricKey = symetricKey;
            SelectedEncryptionType = selectedEnryptionType;
            EncryptionKey = encryptionKey;
        }

        public string GetFileNameWithExtention() => this.FileName != null && this.SelectedFileType != null ? this.FileName + "." + this.SelectedFileType.ToLower() : "";
        public string GetFullFileAndPath() => FolderPath+"\\"+GetFileNameWithExtention();

    }
}
