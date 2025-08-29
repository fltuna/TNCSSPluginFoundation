using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;

namespace TNCSSPluginFoundation.Models.Command.Validators;

/// <summary>
/// Validates that the command has the minimum required number of arguments
/// </summary>
public class ArgumentCountValidator : CommandValidatorBase
{
    private readonly int _minArguments;
    private readonly int _userSpecifiedMinArguments;
    private readonly bool _dontNotifyWhenFailed;

    /// <summary>
    /// Initializes a new instance of ArgumentCountValidator
    /// </summary>
    /// <param name="minArguments">Minimum number of arguments required (excluding command name)</param>
    /// <param name="dontNotifyWhenFailed">Whether to suppress failure notifications</param>
    public ArgumentCountValidator(int minArguments, bool dontNotifyWhenFailed = false)
    {
        _userSpecifiedMinArguments = Math.Max(0, minArguments);
        _minArguments = Math.Max(1, minArguments + 1); // +1 for command name
        _dontNotifyWhenFailed = dontNotifyWhenFailed;
    }

    /// <summary>
    /// Name of this validator for identification purposes
    /// </summary>
    public override string ValidatorName => "TncssBuiltinArgumentCountValidator";

    /// <summary>
    /// Message of validation failure
    /// </summary>
    public override string ValidationFailureMessage => "Common.Validation.Failure.ArgumentCount";

    /// <summary>
    /// Validates argument count
    /// </summary>
    /// <param name="player">CCSPlayerController</param>
    /// <param name="commandInfo">CommandInfo</param>
    /// <returns>TncssCommandValidationResult</returns>
    public override TncssCommandValidationResult Validate(CCSPlayerController? player, CommandInfo commandInfo)
    {
        var argCount = commandInfo.ArgCount;

        if (argCount < _minArguments)
        {
            return _dontNotifyWhenFailed ? TncssCommandValidationResult.FailedIgnoreDefault : TncssCommandValidationResult.Failed;
        }

        return TncssCommandValidationResult.Success;
    }

    /// <summary>
    /// Gets the minimum required arguments (excluding command name)
    /// </summary>
    /// <returns>Minimum required argument count as specified by user</returns>
    public int GetMinimumArguments() => _userSpecifiedMinArguments;

    /// <summary>
    /// Gets the argument count requirements as a string
    /// </summary>
    /// <returns>String representation of the argument count requirements</returns>
    public string GetArgumentCountDescription()
    {
        return _userSpecifiedMinArguments == 0 
            ? "no arguments required" 
            : $"at least {_userSpecifiedMinArguments} argument(s) required";
    }
}