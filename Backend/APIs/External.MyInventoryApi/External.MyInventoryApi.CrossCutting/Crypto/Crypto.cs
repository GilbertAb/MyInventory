using External.MyInventoryApi.CrossCutting.Contracts;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace External.MyInventoryApi.CrossCutting.Crypto
{
    public class Crypto : ICrypto
    {
        //***************************** Aes Algotithm ***********************************************
        private readonly IConfiguration _configuration;
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public Crypto(IConfiguration configuration)
        {
            _configuration = configuration;

            string keyCrypt = _configuration.GetSection("Crypto:KEY").Value
                ?? throw new ArgumentNullException("KEY", "Couldn't find encryption key");
            string ivCrypt = _configuration.GetSection("Crypto:IV").Value
                ?? throw new ArgumentNullException("IV", "Couldn´t find initialization vector");

            _key = Encoding.UTF8.GetBytes(keyCrypt);
            _iv = Encoding.UTF8.GetBytes(ivCrypt);
        }

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            using Aes aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var memoryStream = new MemoryStream();
            using (var criptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            using (var streamWriter = new StreamWriter(criptoStream))
            {
                streamWriter.Write(plainText);
            }

            return Convert.ToBase64String(memoryStream.ToArray());
        }
        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;

            using Aes aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var memoryStream = new MemoryStream(Convert.FromBase64String(cipherText));
            using var criptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(criptoStream);

            return streamReader.ReadToEnd();
        }
    }
}
