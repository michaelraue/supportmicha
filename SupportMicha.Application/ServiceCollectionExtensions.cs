namespace SupportMicha.Application;

using Microsoft.Extensions.DependencyInjection;
using SupportMicha.ApiInterface;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection) =>
        serviceCollection
            .AddSingleton<ISupporterService, SupporterService>();
}