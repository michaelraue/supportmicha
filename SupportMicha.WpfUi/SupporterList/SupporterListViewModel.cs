namespace SupportMicha.WpfUi.SupporterList;

using System.Collections.ObjectModel;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using SupportMicha.ApiInterface;

public class SupporterListViewModel : ObservableObject
{
    private readonly ISupporterService supporterService;

    private IDisposable? observableUnsubscribe;

    public SupporterListViewModel(ISupporterService supporterService)
    {
        this.supporterService = supporterService;
        this.Supporters = [];
    }

    public ObservableCollection<SupporterDto> Supporters { get; }

    public async Task Initialize(CancellationToken cancellationToken = default)
    {
        await this.SeedRandomSupporters(cancellationToken);
        this.observableUnsubscribe = (await this.supporterService.GetSupporters(cancellationToken))
            .ObserveOn(SynchronizationContext.Current!)
            .Subscribe(supporter => this.Supporters.Add(supporter));
    }

    public void UnsubscribeFromDomainSubscriber()
    {
        this.observableUnsubscribe?.Dispose();
    }

    private async Task SeedRandomSupporters(CancellationToken cancellationToken)
    {
        var faker = new Bogus.Faker<SupporterDto>()
            .RuleFor(s => s.FirstName, f => f.Name.FirstName())
            .RuleFor(s => s.LastName, f => f.Name.LastName())
            .RuleFor(s => s.EmailAddress, (f, s) => f.Internet.Email(s.FirstName, s.LastName))
            .RuleFor(s => s.Salutation, f => f.PickRandom<SalutationDto>());

        var numberOfSupporters = new Random().Next(2, 4);
        for (var i = 0; i < numberOfSupporters; i++)
        {
            await this.supporterService.SignUp(faker.Generate(), cancellationToken);
        }
    }
}