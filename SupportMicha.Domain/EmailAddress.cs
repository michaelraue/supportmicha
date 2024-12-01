namespace SupportMicha.Domain;

using System.Text.RegularExpressions;

/// <summary>
/// The email address of a <see cref="Supporter"/>.
/// </summary>
public record EmailAddress(string Value)
{
    /// <summary>
    /// Validates the email address according to predefined constraints.
    /// </summary>
    /// <returns>A collection of validation error messages, or an empty collection if the validation passes.</returns>
    public IEnumerable<string> Validate()
    {
        if (this.Value.Length > 40)
        {
            yield return "Email address must be at most 40 characters.";
        }

        if (!Regex.IsMatch(this.Value, @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$"))
        {
            yield return "Email address is not valid.";
        }
    }
}