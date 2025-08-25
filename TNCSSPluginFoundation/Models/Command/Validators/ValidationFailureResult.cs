namespace TNCSSPluginFoundation.Models.Command.Validators;

/// <summary>
/// Result of handling a validation failure
/// </summary>
public class ValidationFailureResult
{
    /// <summary>
    /// Action to take
    /// </summary>
    public ValidationFailureAction Action { get; set; }

    /// <summary>
    /// Creates a result to use validator's default fallback behavior
    /// </summary>
    /// <returns>ValidationFailureResult</returns>
    public static ValidationFailureResult UseDefaultFallback() => new() { Action = ValidationFailureAction.UseDefaultFallback };
        
    /// <summary>
    /// Creates a result for silent abort
    /// </summary>
    /// <returns>ValidationFailureResult</returns>
    public static ValidationFailureResult SilentAbort() => new() { Action = ValidationFailureAction.SilentAbort };
}