
using PasswordManager.Data.Constants;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace PasswordManager.Services.Encryption
{
    /// <summary>
    /// This class is able to encrpyt and dercrypt text!
    /// </summary>
    public static class EncryptionDecrytionWithRijndaelSymmetricService
    {
        private static Rijndael GetRijndaelEncryptionAlgorithm(string key)
        {
            Rijndael rijAlg = Rijndael.Create();
            rijAlg.Key = Encoding.UTF8.GetBytes(key);
            rijAlg.IV = Encoding.UTF8.GetBytes(GlobalConstant.EncryptionDecrytionWithRijndaelSymmetricPrivateKey);
            return rijAlg;
        }

        /// <summary>
        /// Encrypt data object
        /// </summary>
        /// <returns>encrypted data object</returns>
        private static async Task<string> EncryptDataToString<T>(T data, Rijndael inputRijAlg)
        {
            byte[] plainText = await ToByteArray<T>(data);
            Rijndael rijAlg = inputRijAlg;

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
            MemoryStream msEncrypt = new MemoryStream();

            try
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    await csEncrypt.WriteAsync(plainText);
                }
            }
            catch (Exception)
            {
                throw;
            }
            

            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        /// <summary>
        /// Dercrypt data object
        /// </summary>
        /// <returns>Decrypted data object</returns>
        /// <exception cref="ArgumentNullException"></exception>
        private static async  Task<T> DeryptDataFromByteArray<T>(string encryptedData, Rijndael rijAlg)
        {
            try
            {
                if (encryptedData == string.Empty)
                    throw new ArgumentNullException("encryptedData is null");

                string fileContents = DecryptStringFromBytes(Convert.FromBase64String(encryptedData), rijAlg);

                return JsonSerializer.Deserialize<T>(fileContents);
            }
            catch (Exception)
            {
                throw;
            }            
        }

        private static async Task<byte[]> ToByteArray<T>(T obj)
        {
            if (obj == null)
                return null;

            try
            {
                return JsonSerializer.SerializeToUtf8Bytes<T>(obj);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string DecryptStringFromBytes(byte[] cipherText, Rijndael inputRijAlg)
        {            
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText is null!");
            
            string plaintext = null;
            Rijndael rijAlg = inputRijAlg;

            // Create a decryptor to perform the stream transformation.
            ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

            try
            {
                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream and then place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            
            return plaintext;
        }

        public static async Task<string> EncryptWithKeyToString<T>(T data, string key)
        {
            return await EncryptDataToString<T>(data, GetRijndaelEncryptionAlgorithm(key));
        }

        public static async Task<T> DeryptDataFromByteArray<T>(string encryptedData, string key)
        {
            return await DeryptDataFromByteArray<T>(encryptedData, GetRijndaelEncryptionAlgorithm(key));
        }
    }
}
