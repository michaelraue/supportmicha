namespace SupportMicha.ApiInterface;

/// <summary>
/// The salutation of a <see cref="SupporterDto"/>.
/// </summary>
public enum SalutationDto
{
    /// <summary>
    /// Represents the "Mr" salutation used for addressing a male supporter.
    /// </summary>
    [DisplayName("Mr.")]
    Mr,

    /// <summary>
    /// Represents the "Mrs" salutation used for addressing a female supporter.
    /// </summary>
    [DisplayName("Mrs.")]
    Mrs,

    /// <summary>
    /// Represents the "Ensign" salutation, commonly used in the United Federation of Planets.
    /// </summary>
    Ensign,

    /// <summary>
    /// Represents the "Lieutenant" salutation, commonly used in the United Federation of Planets.
    /// </summary>
    Lieutenant,

    /// <summary>
    /// Represents the "LieutenantCommander" salutation, commonly used in the United Federation of Planets.
    /// </summary>
    [DisplayName("Lieutenant Commander")]
    LieutenantCommander,

    /// <summary>
    /// Represents the "Commander" salutation, commonly used in the United Federation of Planets.
    /// </summary>
    Commander,

    /// <summary>
    /// Represents the "Captain" salutation, commonly used in the United Federation of Planets.
    /// </summary>
    Captain,

    /// <summary>
    /// Represents the "Admiral" salutation, commonly used in the United Federation of Planets.
    /// </summary>
    Admiral,
}