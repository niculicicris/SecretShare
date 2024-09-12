using System.Security.Cryptography;
using SecretShare.Domain.Encryption;

namespace SecretShare.Infrastructure.Encryption;

public class EncryptionService : IEncryptionService
{
    public string Encrypt(string value, string password)
    {
        using var aes = Aes.Create();
        aes.Key = Convert.FromBase64String(password);
        aes.IV = new byte[16];

        var encryptor = aes.CreateEncryptor();

        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        using (var streamWriter = new StreamWriter(cryptoStream))
        {
            streamWriter.Write(value);
        }

        return Convert.ToBase64String(memoryStream.ToArray());
    }

    public string Decrypt(string encryptedValue, string password)
    {
        var encryptedArray = Convert.FromBase64String(encryptedValue);

        using var aes = Aes.Create();
        aes.Key = Convert.FromBase64String(password);
        aes.IV = new byte[16];

        var decryptor = aes.CreateDecryptor();

        using var memoryStream = new MemoryStream(encryptedArray);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);

        return streamReader.ReadToEnd();
    }

    public bool Matches(string encryptedValue, string password)
    {
        try
        {
            var encryptedArray = Convert.FromBase64String(encryptedValue);

            using var aes = Aes.Create();
            aes.Key = Convert.FromBase64String(password);
            aes.IV = new byte[16];

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var memoryStream = new MemoryStream(encryptedArray);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);

            streamReader.ReadToEnd();
        }
        catch (CryptographicException)
        {
            return false;
        }

        return true;
    }
}