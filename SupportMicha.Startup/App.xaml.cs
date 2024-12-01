namespace SupportMicha.Startup;

using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SupportMicha.Application;
using SupportMicha.Persistence;
using SupportMicha.WpfUi;

public partial class App
{
    [STAThread]
    private static void Main(string[] args)
    {
        MainAsync(args).GetAwaiter().GetResult();
    }

    private static async Task MainAsync(string[] args)
    {
        App app = new();
        app.InitializeComponent();

        using var host = CreateHostBuilder(args, app).Build();
        await host.StartAsync().ConfigureAwait(true);

        app.MainWindow = host.Services.GetRequiredService<MainWindow>();
        app.MainWindow.Visibility = Visibility.Visible;
        app.Run();

        await host.StopAsync().ConfigureAwait(true);
    }

    private static IHostBuilder CreateHostBuilder(string[] args, Application app) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((_, config) => config
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables())
            .UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration))
            .ConfigureServices((hostContext, services) =>
            {
                services.AddPresentation(app);
                services.AddApplication();
                services.AddPersistence();
            });
}
