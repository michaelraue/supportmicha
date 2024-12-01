namespace SupportMicha.Persistence;

using System.Collections.Concurrent;
using SupportMicha.Application;
using SupportMicha.Domain;

public class SupporterRepository : ISupporterRepository
{
    private readonly ConcurrentBag<Supporter> supporters = new();

    public Task AddSupporter(Supporter supporter, CancellationToken cancellationToken)
    {
        this.supporters.Add(supporter);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Supporter>> GetSupporters(CancellationToken cancellationToken) =>
        Task.FromResult<IEnumerable<Supporter>>(this.supporters);
}