namespace SecretShare.Domain.Secrets.Abstractions;

public interface ISecretService
{
    Secret CreateSecret(string content, string password);
    Task<string?> RetrieveSecret(Guid id, string password);
}