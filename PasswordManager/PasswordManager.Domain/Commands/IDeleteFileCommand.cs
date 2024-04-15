
namespace PasswordManager.Domain.Commands
{
    public interface IDeleteFileCommand 
    {
        Task Execute(string fileNameWithExtention, string folderName);
    }
}
