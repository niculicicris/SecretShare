namespace SecretShare.Domain.Secrets;

public interface ISecretRepository
{
    Task InsertSecretAsync(Secret secret);
    Task<Secret?> GetSecretAsync(Guid id);
    Task DeleteSecretAsync(Guid id);
}