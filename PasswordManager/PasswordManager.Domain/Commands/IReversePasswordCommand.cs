using PasswordManager.Domain.Models;

namespace PasswordManager.Domain.Commands
{
    public interface IReversePasswordCommand
    {
        Task<string> Execute(string item, SettingModel setting);
    }
}
