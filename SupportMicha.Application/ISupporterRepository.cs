namespace SupportMicha.Application;

using SupportMicha.Domain;

/// <summary>
/// Represents a repository for managing supporters.
/// </summary>
public interface ISupporterRepository
{
    /// <summary>
    /// Adds a supporter to the repository.
    /// </summary>
    /// <param name="supporter">The supporter to be added.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddSupporter(Supporter supporter, CancellationToken cancellationToken);

    Task<IEnumerable<Supporter>> GetSupporters(CancellationToken cancellationToken);
}