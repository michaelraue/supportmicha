namespace SupportMicha.BehaviorTests.Hooks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reqnroll;
using Reqnroll.BoDi;
using SupportMicha.ApiInterface;
using SupportMicha.Application;
using SupportMicha.BehaviorTests.Drivers;
using SupportMicha.Persistence;

[Binding]
public class StartApplication
{
    [BeforeScenario]
    public static void Start(IObjectContainer container)
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddApplication();
                services.AddPersistence();
            }).Build();
        container.RegisterInstanceAs(new SupporterDriver(host.Services.GetRequiredService<ISupporterService>()));
    }
}