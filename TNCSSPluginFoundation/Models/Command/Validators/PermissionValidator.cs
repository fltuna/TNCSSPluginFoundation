using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace TNCSSPluginFoundation.Models.Command.Validators;


/// <summary>
/// Permission validator for TncssAbstractCommandBase
/// </summary>
/// <param name="requiredPermission">Permission node that required</param>
/// <param name="dontNotifyWhenFailed">When true, it will return TncssCommandValidationResult.FailedIgnoreDefault to avoid print default failure message</param>
public sealed class PermissionValidator(string requiredPermission, bool dontNotifyWhenFailed = false) : CommandValidatorBase
{
    /// <summary>
    /// Name of this validator for identification purposes
    /// </summary>
    public override string ValidatorName => "TncssBuiltinPermissionValidator";

    /// <summary>
    /// Message of validation failure
    /// </summary>
    public override string ValidationFailureMessage => "Common.Validation.Failure.Permission";

    /// <summary>
    /// Validates player permission
    /// </summary>
    /// <param name="player">CCSPlayerController</param>
    /// <param name="commandInfo">CommandInfo</param>
    /// <returns>TncssCommandValidationResult</returns>
    public override TncssCommandValidationResult Validate(CCSPlayerController? player, CommandInfo commandInfo)
    {
        if (AdminManager.PlayerHasPermissions(player, requiredPermission))
            return TncssCommandValidationResult.Success;

        if (dontNotifyWhenFailed)
            return TncssCommandValidationResult.FailedIgnoreDefault;
        
        return TncssCommandValidationResult.Failed;
    }
}