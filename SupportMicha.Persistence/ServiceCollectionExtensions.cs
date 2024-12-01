namespace SupportMicha.Persistence;

using Microsoft.Extensions.DependencyInjection;
using SupportMicha.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection serviceCollection) =>
        serviceCollection
            .AddSingleton<ISupporterRepository, SupporterRepository>();
}