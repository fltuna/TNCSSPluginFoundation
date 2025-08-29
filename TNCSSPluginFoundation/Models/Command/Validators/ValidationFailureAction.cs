namespace TNCSSPluginFoundation.Models.Command.Validators;

/// <summary>
/// Action to take when validation fails
/// </summary>
public enum ValidationFailureAction
{
    /// <summary>
    /// Use validator's default behavior (message and abort)
    /// </summary>
    UseDefaultFallback,
        
    /// <summary>
    /// Silently abort command execution without any message
    /// </summary>
    SilentAbort
}