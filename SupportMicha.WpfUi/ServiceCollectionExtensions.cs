namespace SupportMicha.WpfUi;

using System.Windows;
using System.Windows.Threading;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using SupportMicha.WpfUi.SignUp;
using SupportMicha.WpfUi.SupporterList;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection serviceCollection,
        Application application) =>
        serviceCollection
            .AddSingleton<SignUpControl>()
            .AddSingleton<SignUpViewModel>()
            .AddSingleton<SupporterListControl>()
            .AddSingleton<SupporterListViewModel>()
            .AddSingleton<MainWindow>()
            .AddSingleton(_ => application.Dispatcher)
            .AddTransient<ISnackbarMessageQueue>(
                provider =>
                {
                    var dispatcher = provider.GetRequiredService<Dispatcher>();
                    return new SnackbarMessageQueue(TimeSpan.FromSeconds(3), dispatcher);
                });
}