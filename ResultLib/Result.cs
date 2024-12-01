namespace ResultLib;

/// <summary>
/// A result indicates success of failure.
/// </summary>
public record Result
{
    private Result() => this.ValidationMessages = new Dictionary<string, IEnumerable<string>>();

    private Result(IDictionary<string, IEnumerable<string>> validationMessages) => this.ValidationMessages = validationMessages;

    /// <summary>
    /// Gets validation messages, not empty only if this Result indicates failure.
    /// </summary>
    public IDictionary<string, IEnumerable<string>> ValidationMessages { get; }

    /// <summary>
    /// Gets a value indicating whether the validation was successful.
    /// </summary>
    public bool IsSuccessful => !this.ValidationMessages.Any();

    /// <summary>
    /// Gets a value indicating whether the validation was failed.
    /// </summary>
    public bool IsFailure => !this.IsSuccessful;

    /// <summary>
    /// Creates a Result which indicates success.
    /// </summary>
    /// <returns>The new Result.</returns>
    public static Result Success() =>
        new();

    /// <summary>
    /// Creates a Result which will be successful or failed depending on the given validationMessages.
    /// </summary>
    /// <param name="propertyName">The name of the validated property.</param>
    /// <param name="validationMessages">The potential generated validation messages.</param>
    /// <returns>The new Result.</returns>
    public static Result Create(string propertyName, IEnumerable<string> validationMessages) =>
        validationMessages.Any() ? new(new Dictionary<string, IEnumerable<string>> { { propertyName, validationMessages } }) : Success();

    /// <summary>
    /// Merges this Results with other Results.
    /// </summary>
    /// <remarks>If at least one Result indicates failure, the merged Result will also be failed.</remarks>
    /// <param name="results">The other Results to merge.</param>
    /// <returns>The new Result with all validation messages of all Results.</returns>
    public Result Merge(params Result[] results) =>
        new(this.MergeValidationMessages(results));

    private Dictionary<string, IEnumerable<string>> MergeValidationMessages(IEnumerable<Result> results)
    {
        var validationMessages = this.ValidationMessages.ToDictionary();
        foreach (var result in results)
        {
            foreach (var (property, messages) in result.ValidationMessages)
            {
                if (!validationMessages.ContainsKey(property))
                {
                    validationMessages[property] = [];
                }

                validationMessages[property] = validationMessages[property].Concat(messages);
            }
        }

        return validationMessages;
    }
}
