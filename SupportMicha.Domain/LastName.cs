namespace SupportMicha.Domain;

/// <summary>
/// The last name of a <see cref="Supporter"/>.
/// </summary>
public record LastName(string Value)
{
    /// <summary>
    /// Validates the last name according to predefined constraints.
    /// </summary>
    /// <returns>A collection of validation error messages, or an empty collection if the validation passes.</returns>
    public IEnumerable<string> Validate()
    {
        if (this.Value.Length < 2)
        {
            yield return "Last name must be at least 2 characters.";
        }

        if (this.Value.Length > 20)
        {
            yield return "Last name must be at most 20 characters.";
        }
    }
}