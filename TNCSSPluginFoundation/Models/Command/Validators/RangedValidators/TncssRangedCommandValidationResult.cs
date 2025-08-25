namespace TNCSSPluginFoundation.Models.Command.Validators.RangedValidators;

/// <summary>
/// Detailed result set for Ranged Command Validation
/// </summary>
public enum TncssRangedCommandValidationResult
{
    /// <summary>
    /// Range validation succeeded
    /// </summary>
    Success,
    /// <summary>
    /// Value is lower than the minimum expected value
    /// </summary>
    FailedLowerThanExpected,
    /// <summary>
    /// Value is higher than the maximum expected value
    /// </summary>
    FailedHigherThanExpected,
    /// <summary>
    /// Value is out of range or couldn't be parsed
    /// </summary>
    FailedOutOfRange,
}