namespace SupportMicha.Domain;

/// <summary>
/// The salutation of a <see cref="Supporter"/>.
/// </summary>
public record Salutation
{
    /// <summary>
    /// Represents the "Mr" salutation used for addressing a male supporter.
    /// </summary>
    public static readonly Salutation Mr = new(nameof(Mr));

    /// <summary>
    /// Represents the "Mrs" salutation used for addressing a female supporter.
    /// </summary>
    public static readonly Salutation Mrs = new(nameof(Mrs));

    /// <summary>
    /// Represents the "Ensign" salutation, commonly used in the United Federation of Planets.
    /// </summary>
    public static readonly Salutation Ensign = new(nameof(Ensign));

    /// <summary>
    /// Represents the "Lieutenant" salutation, commonly used in the United Federation of Planets.
    /// </summary>
    public static readonly Salutation Lieutenant = new(nameof(Lieutenant));

    /// <summary>
    /// Represents the "LieutenantCommander" salutation, commonly used in the United Federation of Planets.
    /// </summary>
    public static readonly Salutation LieutenantCommander = new(nameof(LieutenantCommander));

    /// <summary>
    /// Represents the "Commander" salutation, commonly used in the United Federation of Planets.
    /// </summary>
    public static readonly Salutation Commander = new(nameof(Commander));

    /// <summary>
    /// Represents the "Captain" salutation, commonly used in the United Federation of Planets.
    /// </summary>
    public static readonly Salutation Captain = new(nameof(Captain));

    /// <summary>
    /// Represents the "Admiral" salutation, commonly used in the United Federation of Planets.
    /// </summary>
    public static readonly Salutation Admiral = new(nameof(Admiral));

    private Salutation(string value) => this.Value = value;

    /// <summary>
    /// Gets the string representation of the salutation.
    /// </summary>
    public string Value { get; }
}