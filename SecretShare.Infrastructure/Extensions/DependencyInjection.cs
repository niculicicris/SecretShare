using Microsoft.Extensions.DependencyInjection;
using SecretShare.Application.Server.Encryption;
using SecretShare.Domain.Secrets;
using SecretShare.Infrastructure.Encryption;
using SecretShare.Infrastructure.Repositories;

namespace SecretShare.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        AddRepositories(services);
        AddCommon(services);

        return services;
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddSingleton<ISecretRepository, InMemorySecretRepository>();
    }

    private static void AddCommon(IServiceCollection services)
    {
        services.AddSingleton<IPasswordGenerator, PasswordGenerator>();
        services.AddSingleton<IPasswordEncryptor, PasswordEncryptor>();
    }
}