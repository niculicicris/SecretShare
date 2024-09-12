using SecretShare.Domain.Encryption;
using SecretShare.Domain.Secrets.Abstractions;

namespace SecretShare.Domain.Secrets;

public class SecretService(
    IEncryptionService encryptionService,
    ISecretExpirationService expirationService,
    ISecretRepository secretRepository) : ISecretService
{
    public Secret CreateSecret(string content, string password)
    {
        var encryptedContent = encryptionService.Encrypt(content, password);
        return new Secret(Guid.NewGuid(), encryptedContent);
    }

    public async Task<string?> RetrieveSecret(Guid id, string password)
    {
        var secret = await secretRepository.GetSecretAsync(id);

        if (secret is null)
            return null;

        if (expirationService.IsSecretExpired(secret))
        {
            await secretRepository.DeleteSecretAsync(id);
            return null;
        }

        if (!encryptionService.Matches(secret.EncryptedContent, password))
            return null;

        var content = encryptionService.Decrypt(secret.EncryptedContent, password);
        await secretRepository.DeleteSecretAsync(id);

        return content;
    }
}