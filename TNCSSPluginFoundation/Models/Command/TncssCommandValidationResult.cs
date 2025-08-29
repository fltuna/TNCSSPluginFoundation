namespace TNCSSPluginFoundation.Models.Command;

/// <summary>
/// Result set for TNCSSPluginFoundation CommandBase
/// </summary>
public enum TncssCommandValidationResult
{
    /// <summary>
    /// Executes command normally
    /// </summary>
    Success,
    /// <summary>
    /// Ignores command with base failure message provided by TncssAbstractCommandBase
    /// </summary>
    Failed,
    /// <summary>
    /// Ignores command and do not print base failure message provided by TncssAbstractCommandBase
    /// </summary>
    FailedIgnoreDefault,
}