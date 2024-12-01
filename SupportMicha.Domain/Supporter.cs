namespace SupportMicha.Domain;

using ResultLib;

/// <summary>
/// A Supporter is someone who wants Michael to join the team.
/// </summary>
public record Supporter(FirstName FirstName, LastName LastName, EmailAddress EmailAddress, Salutation Salutation)
{
    /// <summary>
    /// Validates the <see cref="Supporter"/> according to the business rules.
    /// </summary>
    /// <returns>
    /// A <see cref="Result"/> indicating success of failure.
    /// </returns>
    public Result Validate()
    {
        var result = Result.Success();
        result = result.Merge(Result.Create(nameof(this.FirstName), this.FirstName.Validate()));
        result = result.Merge(Result.Create(nameof(this.LastName), this.LastName.Validate()));
        result = result.Merge(Result.Create(nameof(this.EmailAddress), this.EmailAddress.Validate()));
        return result;
    }
}