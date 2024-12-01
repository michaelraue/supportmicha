namespace SupportMicha.BehaviorTests.Drivers;

using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ResultLib;
using SupportMicha.ApiInterface;

public class SupporterDriver(ISupporterService supporterService)
{
    public async Task<Result> SignUp(SupporterDto supporter, CancellationToken cancellationToken = default) =>
        await supporterService.SignUp(supporter, cancellationToken);

    public async Task<IEnumerable<SupporterDto>> GetSupporters(CancellationToken cancellationToken = default)
    {
        var terminateSubject = new Subject<Unit>();
        var supporters = new List<SupporterDto>();
        using var observable = (await supporterService.GetSupporters(cancellationToken))
            .TakeUntil(terminateSubject)
            .Do(x => supporters.Add(x))
            .Subscribe();
        terminateSubject.OnNext(Unit.Default);
        return supporters;
    }
}