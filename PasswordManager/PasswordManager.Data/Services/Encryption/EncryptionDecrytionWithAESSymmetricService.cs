
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.Services.Encryption
{
    /// <summary>
    /// This class is able to encrpyt and dercrypt data object!
    /// </summary>
    public static class EncryptionDecrytionWithAESSymmetricService
    {
        public static async Task<string> EncryptString(string plainText, string key)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                try
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                            {
                                await streamWriter.WriteAsync(plainText);
                            }

                            array = memoryStream.ToArray();
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return Convert.ToBase64String(array);
        }

        public static async Task<string> DecryptString(string encryptedData, string key)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(encryptedData);

            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = iv;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            {
                                return await streamReader.ReadToEndAsync();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
