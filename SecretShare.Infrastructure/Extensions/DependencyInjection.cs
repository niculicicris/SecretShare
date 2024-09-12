using Microsoft.Extensions.DependencyInjection;
using SecretShare.Application.Server.Encryption;
using SecretShare.Domain.Encryption;
using SecretShare.Domain.Secrets.Abstractions;
using SecretShare.Infrastructure.Encryption;
using SecretShare.Infrastructure.Secrets;

namespace SecretShare.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        AddRepositories(services);
        AddServices(services);

        return services;
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddSingleton<ISecretRepository, InMemorySecretRepository>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddSingleton<IPasswordService, PasswordService>();
        services.AddSingleton<IEncryptionService, EncryptionService>();
    }
}