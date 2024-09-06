namespace SecretShare.Application.Server.Encryption;

public interface IPasswordEncryptor
{
    string Encrypt(string value, string password);
    string Decrypt(string encryptedValue, string password);
    bool Matches(string encryptedValue, string password);
}