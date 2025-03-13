namespace BAL.Interfaces;

public interface IAESService
{
    string Encrypt(string plainText);
    string Decrypt(string encryptedText);

}
