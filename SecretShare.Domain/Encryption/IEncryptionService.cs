namespace SecretShare.Domain.Encryption;

public interface IEncryptionService
{
    string Encrypt(string value, string password);
    string Decrypt(string encryptedValue, string password);
    bool Matches(string encryptedValue, string password);
}