using System.Security.Cryptography;
using System.Text;
using BAL.Interfaces;

public class AESImple : IAESService
{


    public string Encrypt(string plainText)
    {
        string secretKey = "$ASPcAwSNIgcPPEoTSa0ODw#";

        byte[] secretBytes = Encoding.UTF8.GetBytes(secretKey);

        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = secretBytes;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;

            byte[] encryptedBytes = null;
            using (ICryptoTransform encryptor = aes.CreateEncryptor())
            {
                encryptedBytes = encryptor.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);
            }

            return Convert.ToBase64String(encryptedBytes);
        }
    }

    // hello
    public string Decrypt(string encryptedText)
    {
        string secretKey = "$ASPcAwSNIgcPPEoTSa0ODw#";


        byte[] secretBytes = Encoding.UTF8.GetBytes(secretKey);

        byte[] encryptedBytes = Convert.FromBase64String(encryptedText);


        using (Aes aes = Aes.Create())
        {
            aes.Key = secretBytes;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;

            byte[] decryptedBytes = null;
            using (ICryptoTransform decryptor = aes.CreateDecryptor())
            {
                decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            }

            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}