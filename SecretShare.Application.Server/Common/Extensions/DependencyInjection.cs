using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SecretShare.Domain.Secrets;
using SecretShare.Domain.Secrets.Abstractions;

namespace SecretShare.Application.Server.Common.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        AddFluentValidation(services);
        AddMediatR(services);
        AddServices(services);

        return services;
    }

    private static void AddFluentValidation(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly);
    }

    private static void AddMediatR(IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(AssemblyReference.Assembly);
        });
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddSingleton<ISecretService, SecretService>();
        services.AddSingleton<ISecretExpirationService, SecretExpirationService>();
    }
}