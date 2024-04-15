
using System.IO;

namespace PasswordManager.Data.Services
{
    internal static class HandlerFolderFile
    {
        public static void CheckFolderStructuresWithUniqueName(string folderName)
        {
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
        }

        public static bool IsFileExistInGivenFolder(string fileName, string folderName) => File.Exists(folderName + "\\" + fileName);

        public static bool IsFileExistInGivenFolderwithFullPath(string fileWithExtention) => File.Exists(fileWithExtention);

        public static void DeleteFile(string fileName, string folderName)
        {          
            if(File.Exists(folderName + "\\" + fileName))
                File.Delete(folderName + "\\" + fileName);
        }
    }
}
