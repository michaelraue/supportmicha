namespace SupportMicha.ApiInterface;

/// <summary>
/// An attribute that stores a display name for an enumeration value or other purpose.
/// </summary>
public class DisplayNameAttribute(string displayName) : Attribute
{
    /// <summary>
    /// Gets the display name associated with the attribute.
    /// </summary>
    public string DisplayName { get; } = displayName;
}