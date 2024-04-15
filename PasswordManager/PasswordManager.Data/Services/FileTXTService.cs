
using System.IO;

namespace PasswordManager.Data.Services
{
    /// <summary>
    /// Use to handle setting and password files
    /// </summary>
    internal class FileTXTService : IFileServiceBase
    {
        public FileTXTService()
        {

        }

        private void ArgumentCheck(string folderName, string fileNameWithExtension, string datastream)
        {
            if (datastream == null || folderName == null || fileNameWithExtension == null)
            {
                throw new ArgumentNullException();
            }
        }

        private void ArgumentCheck(string folderName, string fileNameWithExtension, List<string> datastreams)
        {
            if (datastreams == null || folderName == null || fileNameWithExtension == null)
            {
                throw new ArgumentNullException();
            }
        }

        private async Task RemoveTemporaryFile(string folderName, string fileNameWithExtension)
        {
            //Delete old file
            File.Delete(folderName + "\\" + fileNameWithExtension);
            File.Move(folderName + "\\Temp_" + fileNameWithExtension, folderName + "\\Temp_" + fileNameWithExtension);

            try
            {
                using (FileStream sourceStream = File.Open(folderName + "\\Temp_" + fileNameWithExtension, FileMode.Open))
                {
                    using (FileStream destinationStream = File.Create(folderName + "\\" + fileNameWithExtension))
                    {
                        await sourceStream.CopyToAsync(destinationStream);
                        sourceStream.Close();

                        //Delete temporary file
                        File.Delete(folderName + "\\Temp_" + fileNameWithExtension);
                    }
                }
            }
            catch (IOException ioex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task WriteOutADataRow(string folderName, string fileNameWithExtension, string datastream)
        {
            ArgumentCheck(folderName, fileNameWithExtension, datastream);
            HandlerFolderFile.CheckFolderStructuresWithUniqueName(folderName);
            string fileWithFolder = folderName + "\\" + fileNameWithExtension;

            //if file not exist create it.
            if (!HandlerFolderFile.IsFileExistInGivenFolderwithFullPath(fileWithFolder))
            {
                FileStream fs = new FileStream(fileWithFolder, FileMode.OpenOrCreate, FileAccess.Write);
                fs.Dispose();
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(fileWithFolder, true))
                {
                    await writer.WriteLineAsync(datastream);
                }
            }
            catch (IOException ioex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }            
        }

        public async Task WriteOutADataRows(string folderName, string fileNameWithExtension, List<string> datastreams)
        {
            ArgumentCheck(folderName, fileNameWithExtension, datastreams);
            HandlerFolderFile.CheckFolderStructuresWithUniqueName(folderName);
            string fileWithFolder = folderName + "\\" + fileNameWithExtension;

            try
            {
                //Delete old file
                File.Delete(fileWithFolder);
                FileStream fileStream = new FileStream(fileWithFolder, FileMode.CreateNew);
                fileStream.Close();

                using (StreamWriter writer = new StreamWriter(fileWithFolder, true))
                {
                    foreach (var item in datastreams)
                        await writer.WriteLineAsync(item);                    
                }
            }
            catch (IOException ioex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<string>> GetDataRows(string folderName, string fileNameWithExtension)
        {
            List<string> dataRows = new List<string>();
            HandlerFolderFile.CheckFolderStructuresWithUniqueName(folderName);
            string fileWithFolder = folderName + "\\" + fileNameWithExtension;

            //if file not exist create it.
            if (!HandlerFolderFile.IsFileExistInGivenFolderwithFullPath(fileWithFolder))
            {
                FileStream fs = new FileStream(fileWithFolder, FileMode.OpenOrCreate, FileAccess.Write);
                fs.Dispose();
            }

            else
            {
                try
                {
                    using (StreamReader reader = new StreamReader(fileWithFolder))
                    {
                        string rowData;
                        while ((rowData = await reader.ReadLineAsync()) != null)
                        {
                            dataRows.Add(rowData);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
            }

            return dataRows;
        }

        public async Task DeleteADataRow(string folderName, string fileNameWithExtension, string firstColumn)
        {
            if (firstColumn == null)
            {
                throw new ArgumentNullException();
            }

            string rowData;

            try
            {
                using (StreamReader reader = new StreamReader(folderName + "\\" + fileNameWithExtension))
                {
                    using (StreamWriter writer = new StreamWriter(folderName + "\\Temp_" + fileNameWithExtension))
                    {
                        while ((rowData = await reader.ReadLineAsync()) != null)
                        {
                            if (rowData.Split(";")[0] == firstColumn)
                                continue;

                            await writer.WriteLineAsync(rowData);
                        }
                    }
                }
            }
            catch (IOException ioex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }            

            await RemoveTemporaryFile(folderName, fileNameWithExtension);
        }

        public async Task UpdateADataRow(string folderName, string fileNameWithExtension, string firstColumn, string replaceRowData)
        {
            if (firstColumn == null || replaceRowData ==  null)
            {
                throw new ArgumentNullException();
            }

            string rowData;

            try
            {
                using (StreamReader reader = new StreamReader(folderName + "\\" + fileNameWithExtension))
                {
                    using (StreamWriter writer = new StreamWriter(folderName + "\\Temp_" + fileNameWithExtension))
                    {
                        while ((rowData = await reader.ReadLineAsync()) != null)
                        {
                            if (rowData.Split(";")[0] == firstColumn)
                                await writer.WriteLineAsync(replaceRowData);
                            else
                                await writer.WriteLineAsync(rowData);
                        }
                    }
                }
            }
            catch (IOException ioex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }

            await RemoveTemporaryFile(folderName, fileNameWithExtension);
        }
    }
}
