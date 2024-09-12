using SecretShare.Domain.Secrets.Abstractions;

namespace SecretShare.Domain.Secrets;

public class SecretExpirationService : ISecretExpirationService
{
    public bool IsSecretExpired(Secret secret)
    {
        return secret.CreationDate.AddDays(1) < DateTime.UtcNow;
    }
}