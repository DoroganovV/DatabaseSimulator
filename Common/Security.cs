using System;
using System.IO;
using System.Security.Cryptography;

namespace Common
{
    public class Security
    {
        public static string CreateSha512Hash(string Phrase)
        {
            SHA512Managed HashTool = new SHA512Managed();
            byte[] PhraseAsByte = System.Text.Encoding.UTF8.GetBytes(string.Concat(Phrase));
            byte[] EncryptedBytes = HashTool.ComputeHash(PhraseAsByte);
            HashTool.Clear();
            var str = "";
            foreach (var el in EncryptedBytes)
                str += string.Format("{0:x2}", el);
            return str.ToUpper();
        }
        public static string EncryptStringAES(string plainText, string sharedSecret)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            string outStr;                       // Encrypted string to return
            RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data.

            // generate the key from the shared secret and the salt
            var key = new Rfc2898DeriveBytes(sharedSecret, new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });

            // Create a RijndaelManaged object
            aesAlg = new RijndaelManaged();
            aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

            // Create a decryptor to perform the stream transform.
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for encryption.
            using (var msEncrypt = new MemoryStream())
            {
                // prepend the IV
                msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                }
                outStr = Convert.ToBase64String(msEncrypt.ToArray());
            }
            // Clear the RijndaelManaged object.
            if (aesAlg != null)
                aesAlg.Clear();

            // Return the encrypted bytes from the memory stream.
            return outStr;
        }
    }
}
