using SecretShare.Domain.Secrets;
using SecretShare.Domain.Secrets.Abstractions;

namespace SecretShare.Infrastructure.Secrets;

public class InMemorySecretRepository : ISecretRepository
{
    private readonly Dictionary<Guid, Secret> _secrets = new();

    public async Task InsertSecretAsync(Secret secret)
    {
        _secrets[secret.Id] = secret;
    }

    public async Task<Secret?> GetSecretAsync(Guid id)
    {
        return _secrets.GetValueOrDefault(id);
    }

    public async Task DeleteSecretAsync(Guid id)
    {
        _secrets.Remove(id);
    }
}