using System.Security.Cryptography;
using SecretShare.Domain.Encryption;
using SecretShare.Infrastructure.Encryption;

namespace SecretShare.Test.Server.Encryption;

public class EncryptionServiceTests
{
    private readonly IEncryptionService _encryptionService = new EncryptionService();

    [Fact]
    public void Encrypt_InvalidPassword_ShouldThrowException()
    {
        var originalValue = "TestValue";
        var password = "InvalidTestPassword";

        Assert.Throws<FormatException>(() => _encryptionService.Encrypt(originalValue, password));
    }

    [Fact]
    public void Decrypt_InvalidEncryptedValue_ShouldThrowException()
    {
        var encryptedValue = "EncryptedTestValue";
        var password = Convert.ToBase64String(Aes.Create().Key);

        Assert.Throws<FormatException>(() => _encryptionService.Decrypt(encryptedValue, password));
    }

    [Fact]
    public void Decrypt_InvalidPassword_ShouldThrowException()
    {
        var originalValue = "TestValue";
        var validPassword = Convert.ToBase64String(Aes.Create().Key);
        var invalidPassword = "InvalidTestPassword";

        var encryptedValue = _encryptionService.Encrypt(originalValue, validPassword);

        Assert.Throws<FormatException>(() => _encryptionService.Decrypt(encryptedValue, invalidPassword));
    }

    [Fact]
    public void Decrypt_WrongPassword_ShouldThrowException()
    {
        var originalValue = "TestValue";
        var correctPassword = Convert.ToBase64String(Aes.Create().Key);
        var wrongPassword = Convert.ToBase64String(Aes.Create().Key);

        var encryptedValue = _encryptionService.Encrypt(originalValue, correctPassword);

        Assert.Throws<CryptographicException>(() => _encryptionService.Decrypt(encryptedValue, wrongPassword));
    }

    [Fact]
    public void Encrypt_Decrypt_ShouldReturnOriginalValue()
    {
        var originalValue = "TestValue";
        var password = Convert.ToBase64String(Aes.Create().Key);

        var encryptedValue = _encryptionService.Encrypt(originalValue, password);
        var decryptedValue = _encryptionService.Decrypt(encryptedValue, password);

        Assert.Equal(originalValue, decryptedValue);
    }

    [Fact]
    public void Matches_WrongPassword_ShouldReturnFalse()
    {
        var originalValue = "TestValue";
        var correctPassword = Convert.ToBase64String(Aes.Create().Key);
        var wrongPassword = Convert.ToBase64String(Aes.Create().Key);

        var encryptedValue = _encryptionService.Encrypt(originalValue, correctPassword);

        Assert.False(_encryptionService.Matches(encryptedValue, wrongPassword));
    }

    [Fact]
    public void Matches_CorrectPassword_ShouldReturnTrue()
    {
        var originalValue = "TestValue";
        var password = Convert.ToBase64String(Aes.Create().Key);

        var encryptedValue = _encryptionService.Encrypt(originalValue, password);

        Assert.True(_encryptionService.Matches(encryptedValue, password));
    }
}