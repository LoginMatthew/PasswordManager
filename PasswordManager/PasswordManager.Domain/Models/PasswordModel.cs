
using System.Text.Json.Serialization;

namespace PasswordManager.Domain.Models
{
    public class PasswordModel : BaseModel
    {
        public int Id { get; set; }
        public string Site { get; set; }
        public string Email { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? UserName { get; set; }
        public string Password { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }

        public PasswordModel()
        {

        }

        public PasswordModel(int id, string site, string password, DateTime creationDate, string email, string userName = null, string description = null)
        {
            this.Id = id;
            this.Site = site;
            this.Password = password;
            this.CreationDate = creationDate;
            this.UserName = userName;
            this.Email = email;
            this.Description = description;
        }

        public PasswordModel(string site, string password, DateTime creationDate, string email, string userName = null, string description = null)
        {
            this.Site = site;
            this.Password = password;
            this.CreationDate = creationDate;
            this.UserName = userName;
            this.Email = email;
            this.Description = description;
        }

        public string ToWriteOutData()
        {
            string asd = Id.ToString() + ";" + Site + ";" + Password + ";" + CreationDate.ToString() + ";" + Email + ";" + (UserName == null ? "null" : UserName) + ";" + (Description == null ? "null" : Description);
            return asd;
        }
    }
}
