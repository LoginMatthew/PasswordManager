using PasswordManager.Data.Services;
using PasswordManager.Domain.Commands;

namespace PasswordManager.Data.Commands.DeleteCommands
{
    public class DeleteFileCommand : IDeleteFileCommand
    {
        public async Task Execute(string fileNameWithExtention, string folderName)
        {
            try
            {
                HandlerFolderFile.DeleteFile(fileNameWithExtention, folderName);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
