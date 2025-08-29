namespace TNCSSPluginFoundation.Models.Command;

/// <summary>
/// Contains the result of command validation along with validated arguments
/// </summary>
public class TncssCommandValidationContext(
    TncssCommandValidationResult result,
    ValidatedArguments? validatedArguments = null)
{
    /// <summary>
    /// The validation result
    /// </summary>
    public TncssCommandValidationResult Result { get; } = result;

    /// <summary>
    /// The validated arguments (null if validation failed)
    /// </summary>
    public ValidatedArguments? ValidatedArguments { get; } = validatedArguments;

    /// <summary>
    /// Creates a successful validation context with validated arguments
    /// </summary>
    /// <param name="validatedArguments">The validated arguments</param>
    /// <returns>Successful validation context</returns>
    public static TncssCommandValidationContext Success(ValidatedArguments validatedArguments)
    {
        return new TncssCommandValidationContext(TncssCommandValidationResult.Success, validatedArguments);
    }

    /// <summary>
    /// Creates a failed validation context
    /// </summary>
    /// <returns>Failed validation context</returns>
    public static TncssCommandValidationContext Failed()
    {
        return new TncssCommandValidationContext(TncssCommandValidationResult.Failed);
    }

    /// <summary>
    /// Creates a failed validation context that ignores default messages
    /// </summary>
    /// <returns>Failed validation context that ignores defaults</returns>
    public static TncssCommandValidationContext FailedIgnoreDefault()
    {
        return new TncssCommandValidationContext(TncssCommandValidationResult.FailedIgnoreDefault);
    }
}