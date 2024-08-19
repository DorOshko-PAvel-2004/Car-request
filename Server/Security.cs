using System.Security.Cryptography;
using System.Text;

namespace MVCE.Server
{
    public class Security
    {
        public static string Encrypt(string data, byte[] key)
        {
            //string data = "GeeksForGeeks Text";
            string answer = "";
            byte[] privateKeyBytes = key;
            byte[] publicKeyBytes = { };
            publicKeyBytes = key;
            byte[] inputByteArray = System.Text.Encoding.UTF8.GetBytes(data);
            using (DESCryptoServiceProvider provider = new DESCryptoServiceProvider())
            {
                var memoryStream = new MemoryStream();
                var cryptoStream = new CryptoStream
                    (memoryStream, provider.CreateEncryptor(publicKeyBytes, privateKeyBytes), CryptoStreamMode.Write);
                cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
                cryptoStream.FlushFinalBlock();
                answer = Convert.ToBase64String(memoryStream.ToArray());
            }
            return answer;
        }
        public static string Decrypt(string data, byte[] key)
        {
            data = data.Trim('\0');
            string answer = "";
            byte[] privateKeyBytes = { };
            privateKeyBytes = key;
            byte[] publicKeyBytes = { };
            publicKeyBytes = key;
            byte[] inputByteArray = new byte[data.Replace(" ", "+").Length];
            inputByteArray = Convert.FromBase64String(data.Replace(" ", "+"));
            using (DESCryptoServiceProvider provider = new DESCryptoServiceProvider())
            {
                var memoryStream = new MemoryStream();
                var cryptoStream = new CryptoStream(memoryStream,
                provider.CreateDecryptor(publicKeyBytes, privateKeyBytes),
                CryptoStreamMode.Write);
                cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
                cryptoStream.FlushFinalBlock();
                answer = Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            return answer;
        }

    }
}
