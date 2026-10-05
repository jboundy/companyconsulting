using System.Security.Cryptography;
using System.Text;

namespace Consulting.Encryption
{
    public static class EncryptionWrapper
    {
        // TODO: make this key more secret instead of in the dll
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("Your32ByteSecretKeyForAES256!!!!");
        public static string Decrypt(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            var fullCipher = Convert.FromBase64String(input);

            using var aes = Aes.Create();
            aes.Key = Key;

            // Extract the 16-byte IV from the start of the payload
            var iv = new byte[16];
            var cipherTextBytes = new byte[fullCipher.Length - iv.Length];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipherTextBytes, 0, cipherTextBytes.Length);

            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(cipherTextBytes);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }

        public static string Encrypt(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            using var aes = Aes.Create();
            aes.Key = Key;
            aes.GenerateIV();
            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            ms.Write(aes.IV, 0, aes.IV.Length);

            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(input);
            }
            return Convert.ToBase64String(ms.ToArray());
        }
    }
}