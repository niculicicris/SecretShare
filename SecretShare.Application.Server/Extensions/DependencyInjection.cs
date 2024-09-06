using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace SecretShare.Application.Server.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        AddFluentValidation(services);
        AddMediatR(services);

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
}