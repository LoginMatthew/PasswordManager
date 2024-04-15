
namespace PasswordManager.Data.Services
{
    public interface IFileServiceBase
    {
        Task WriteOutADataRow(string folderName, string fileNameWithExtension, string datastream);
        Task<IEnumerable<string>> GetDataRows(string folderName, string fileNameWithExtension);
        Task DeleteADataRow(string folderName, string fileNameWithExtension, string firstColumn);
        Task UpdateADataRow(string folderName, string fileNameWithExtension, string firstColumn, string replaceRowData);
    }
}
