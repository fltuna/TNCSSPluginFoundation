using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Commands.Targeting;
using TNCSSPluginFoundation.Extensions.Targeting;

namespace TNCSSPluginFoundation.Models.Command.Validators;


/// <summary>
/// Extended targeting validator for TncssAbstractCommandBase <br/>
/// Find players by using ExtendedTargeting. This validator fails when if no players found
/// </summary>
/// <param name="argumentIndex">Index of the argument containing the target string (1-based)</param>
/// <param name="dontNotifyWhenFailed">When true, it will return TncssCommandValidationResult.FailedIgnoreDefault to avoid print default failure message</param>
public class ExtendedTargetValidator(int argumentIndex, bool dontNotifyWhenFailed = false): CommandValidatorBase
{
    private TargetResult? _lastFoundTargets;
    private string? _lastTargetString;
    /// <summary>
    /// Name of this validator for identification purposes
    /// </summary>
    public override string ValidatorName => "TncssBuiltinExtendedTargetValidator";

    /// <summary>
    /// Message of validation failure
    /// </summary>
    public override string ValidationFailureMessage => "Common.Validation.Failure.ExtendedTarget";
    
    /// <summary>
    /// Find players by using ExtendedTargeting. This validator fails when if no players found
    /// </summary>
    /// <param name="player">CCSPlayerController</param>
    /// <param name="commandInfo">CommandInfo</param>
    /// <returns>TncssCommandValidationResult</returns>
    public override TncssCommandValidationResult Validate(CCSPlayerController? player, CommandInfo commandInfo)
    {
        _lastFoundTargets = null;
        _lastTargetString = null;

        // Check if the argument exists
        if (commandInfo.ArgCount <= argumentIndex)
        {
            if (dontNotifyWhenFailed)
                return TncssCommandValidationResult.FailedIgnoreDefault;
            
            return TncssCommandValidationResult.Failed;
        }

        var targetString = commandInfo.GetArg(argumentIndex);
        _lastTargetString = targetString;

        if (!ExtendedTargeting.ResolveExtendedTarget(targetString, player, out var foundTargets))
        {
            if (dontNotifyWhenFailed)
                return TncssCommandValidationResult.FailedIgnoreDefault;
            
            return TncssCommandValidationResult.Failed;
        }
        
        _lastFoundTargets = foundTargets;
        return TncssCommandValidationResult.Success;
    }

    /// <summary>
    /// Gets the last found targets from validation
    /// </summary>
    /// <returns>Array of found players or null if no validation was performed or failed</returns>
    public TargetResult? GetFoundTargets() => _lastFoundTargets;

    /// <summary>
    /// Gets the last target string that was validated
    /// </summary>
    /// <returns>Target string or null if no validation was performed</returns>
    public string? GetLastTargetString() => _lastTargetString;

    /// <summary>
    /// Gets the argument index this validator is configured to check
    /// </summary>
    /// <returns>Argument index (1-based)</returns>
    public int GetArgumentIndex() => argumentIndex;

    /// <summary>
    /// Extracts validated arguments after successful validation
    /// </summary>
    /// <param name="player">CCSPlayerController</param>
    /// <param name="commandInfo">CommandInfo</param>
    /// <returns>ValidatedArguments with found targets</returns>
    protected override ValidatedArguments ExtractArguments(CCSPlayerController? player, CommandInfo commandInfo)
    {
        var validatedArguments = base.ExtractArguments(player, commandInfo);
        
        if (_lastFoundTargets != null)
        {
            validatedArguments.SetArgument(argumentIndex, _lastFoundTargets);
        }
        
        return validatedArguments;
    }
}