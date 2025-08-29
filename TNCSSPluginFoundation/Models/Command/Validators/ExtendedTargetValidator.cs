using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using TNCSSPluginFoundation.Extensions.Targeting;

namespace TNCSSPluginFoundation.Models.Command.Validators;


/// <summary>
/// Extended targeting validator for TncssAbstractCommandBase <br/>
/// Find players by using ExtendedTargeting. This validator fails when if no players found
/// </summary>
/// <param name="targetString">Target string</param>
/// <param name="dontNotifyWhenFailed">When true, it will return TncssCommandValidationResult.FailedIgnoreDefault to avoid print default failure message</param>
public class ExtendedTargetValidator(string targetString, bool dontNotifyWhenFailed = false): ICommandValidator
{
    /// <summary>
    /// Name of this validator for identification purposes
    /// </summary>
    public string ValidatorName => "TncssBuiltinExtendedTargetValidator";

    /// <summary>
    /// Message of validation failure
    /// </summary>
    public string ValidationFailureMessage => "Common.Validation.Failure.ExtendedTarget";
    
    /// <summary>
    /// Find players by using ExtendedTargeting. This validator fails when if no players found
    /// </summary>
    /// <param name="player">CCSPlayerController</param>
    /// <param name="commandInfo">CommandInfo</param>
    /// <returns>TncssCommandValidationResult</returns>
    public TncssCommandValidationResult Validate(CCSPlayerController? player, CommandInfo commandInfo)
    {
        if (!ExtendedTargeting.ResolveExtendedTarget(targetString, player, out var foundTargets))
        {
            if (dontNotifyWhenFailed)
                return TncssCommandValidationResult.FailedIgnoreDefault;
            
            return TncssCommandValidationResult.Failed;
        }
        
        return TncssCommandValidationResult.Success;
    }
}