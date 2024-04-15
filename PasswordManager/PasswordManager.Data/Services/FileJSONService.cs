
using PasswordManager.Domain.Models;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace PasswordManager.Data.Services
{
    internal class FileJSONService<T> where T : BaseModel
    {
        public FileJSONService()
        {

        }

        private void ArgumentCheck(string folderName, string fileNameWithExtension, T data)
        {
            if (folderName == null || fileNameWithExtension == null || data == null)
            {
                throw new ArgumentNullException();
            }
        }

        private void ArgumentCheck(string folderName, string fileNameWithExtension, List<T> data)
        {
            if (folderName == null || fileNameWithExtension == null || data == null)
            {
                throw new ArgumentNullException();
            }
        }

        public async Task WriteOutADataRow(string folderName, string fileNameWithExtension, T data, bool cerateNewFile = false)
        {
            ArgumentCheck(folderName, fileNameWithExtension, data);

            string fileWithFolder = folderName + "\\" + fileNameWithExtension;
            List<T> dataList = new List<T>();
            

            HandlerFolderFile.CheckFolderStructuresWithUniqueName(folderName);

            FileStream fileStream;

            if (!File.Exists(fileWithFolder) || cerateNewFile)
            {
                fileStream = File.Create(fileWithFolder);
            }
            else
            {                
                dataList = await GetDataRows(folderName, fileNameWithExtension) as List<T>;
                fileStream = File.OpenWrite(fileWithFolder);
            }

            dataList.Add(data);

            try
            {
                await JsonSerializer.SerializeAsync(fileStream, dataList, new JsonSerializerOptions()
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                });
            }
            catch (Exception e) 
            {
                throw;
            }
            
            fileStream.Close();
        }

        public async Task WriteOutADataListRows(string folderName, string fileNameWithExtension, List<T> dataList)
        {
            ArgumentCheck(folderName, fileNameWithExtension, dataList);
            string fileWithFolder = folderName + "\\" + fileNameWithExtension;
            HandlerFolderFile.CheckFolderStructuresWithUniqueName(folderName);

            FileStream fileStream;
            try
            {
                //Delete old file
                File.Delete(fileWithFolder);

                fileStream = new FileStream(fileWithFolder, FileMode.CreateNew, FileAccess.Write);

                await JsonSerializer.SerializeAsync(fileStream, dataList, new JsonSerializerOptions()
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                });
            }
            catch (Exception e)
            {
                throw;
            }

            fileStream.Close();
        }

        public async Task<IEnumerable<T>> GetDataRows(string folderName, string fileNameWithExtension)
        {
            List<T> dataRows = new List<T>();

            HandlerFolderFile.CheckFolderStructuresWithUniqueName(folderName);

            string fileWithFolder = folderName + "\\" + fileNameWithExtension;

            JsonSerializerOptions _optionReader = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            if (HandlerFolderFile.IsFileExistInGivenFolderwithFullPath(fileWithFolder))
            {
                try
                {
                    FileStream fileStream = File.OpenRead(fileWithFolder);
                    dataRows = await JsonSerializer.DeserializeAsync<List<T>>(fileStream, _optionReader);
                    fileStream.Close();
                }
                catch (Exception e)
                {
                    throw;
                }                
            }

            return dataRows;
        }

        public async Task<T> GetData(string folderName, string fileNameWithExtension)
        {
            T data = null;

            HandlerFolderFile.CheckFolderStructuresWithUniqueName(folderName);
            string fileWithFolder = folderName + "\\" + fileNameWithExtension;

            JsonSerializerOptions _optionReader = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            if (HandlerFolderFile.IsFileExistInGivenFolderwithFullPath(fileWithFolder))
            {
                try
                {
                    FileStream fileStream = File.OpenRead(fileWithFolder);
                    List<T> resultList = await JsonSerializer.DeserializeAsync<List<T>>(fileStream, _optionReader);
                    data = resultList.FirstOrDefault();
                    fileStream.Close();
                }
                catch (Exception e)
                {
                    throw;
                }
            }

            return data;
        }

        public async Task UpdateADataRow(string folderName, string fileNameWithExtension, T data)
        {
            try
            {
                var DataType = data.GetType();
                PropertyInfo prop = DataType?.GetProperty("Id");
                var DataValue = prop?.GetValue(data);

                List<T> dataRows = await GetDataRows(folderName, fileNameWithExtension) as List<T>;
                T foundItem = null;
                foreach (var row in dataRows)
                {
                    if (DataValue.ToString() == row.GetType().GetProperty("Id").GetValue(row).ToString())
                    {
                        foundItem = row;
                        break;
                    }
                }
                
                dataRows.Remove(foundItem);
                dataRows.Add(data);
                dataRows.OrderBy(x => x.GetType().GetProperty("Id"));

                await WriteOutADataListRows(folderName, fileNameWithExtension, dataRows);
            }
            catch (Exception)
            {
                throw;
            }            
        }

        public async Task DeleteADataRow(string folderName, string fileNameWithExtension, string id)
        {
            try
            {
                List<T> dataRows = await GetDataRows(folderName, fileNameWithExtension) as List<T>;

                dataRows?.RemoveAll(x => x.GetType().GetProperty("Id").GetValue(x).ToString() == id);
                dataRows.OrderBy(x => x.GetType().GetProperty("Id"));

                await WriteOutADataListRows(folderName, fileNameWithExtension, dataRows);
            }
            catch (Exception)
            {
                throw;
            }            
        }
    }
}
