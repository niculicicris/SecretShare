namespace SecretShare.Domain.Secrets.Abstractions;

public interface ISecretExpirationService
{
    bool IsSecretExpired(Secret secret);
}