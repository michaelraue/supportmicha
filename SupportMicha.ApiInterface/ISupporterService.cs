namespace SupportMicha.ApiInterface;

using ResultLib;

/// <summary>
/// Service for managing and retrieving supporter information.
/// </summary>
public interface ISupporterService
{
    /// <summary>
    /// Registers a new supporter.
    /// </summary>
    /// <param name="supporter">The data transfer object containing the supporter's information, such as name and email address.</param>
    /// <returns>A <see cref="Result"/> indicating whether the operation was successful or if it encountered validation errors.</returns>
    Task<Result> SignUp(SupporterDto supporter, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves an observable collection of supporters.
    /// </summary>
    Task<IObservable<SupporterDto>> GetSupporters(CancellationToken cancellationToken);
}